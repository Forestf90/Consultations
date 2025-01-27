﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Consultations.Data;
using Consultations.Models;
using Consultations.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Consultations.Controllers
{
    public class ConsultationController : Controller
    {
        private static ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public ConsultationController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public void FindStudents()
        {
            var students = _context.UserRoles.Join(_context.Roles, e => e.RoleId, r => r.Id, (e, r) => new {
                RName = r.Name,
                UId = e.UserId
            })
        .Where(q => q.RName == "Student").Select(o => o.UId);

            var pesels = new List<string>();
            foreach (var stu in students)
            {
                var temp = _context.AppUsers.Where(q => q.Id == stu).Select(o => o.Pesel).FirstOrDefault();
                pesels.Add(temp);
            }
            ViewBag.Students = pesels;
        }
        // GET: Consultation
        public ActionResult Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var teacher = _context.UserRoles.Join(_context.Roles, e => e.RoleId, r => r.Id, (e, r) => new {
                RName = r.Name,
                UId = e.UserId
            })
                    .Where(q => q.RName == "Teacher").Select(o => o.UId);

          

            var list = _context.Consultations
                .Where(p => p.AppUsers.Select(z => z.UserId).Contains(userId))
                .Select(x => new DisplayConsultationViewModel
                {
                    Id =x.Id,
                    Teacher = x.AppUsers.Where(p => teacher.Contains(p.UserId)).First().User.FirstName+" "+
                                x.AppUsers.Where(p => teacher.Contains(p.UserId)).First().User.LastName,
                    TeacherId = x.AppUsers.Where(p => teacher.Contains(p.UserId)).First().User.Email,
                    Students = x.AppUsers.Count()-1,
                    Room = x.Room,
                    Date = x.Date
                });
            return View(list);
        }

        [Authorize(Roles ="Teacher")]
        public ActionResult Edit(string id, string teacher, string error=null)
        {

            ViewData["Consult"] = error;
            var consultation = _context.Consultations
                .GroupJoin(_context.UserConsultation, e => e.Id, r => r.ConsultationId, (e, r) =>
                 new { Date = e.Date, Room = e.Room, AppStudents = r.Select(t => t.User.Pesel),
                     Emails = r.Select(o => o.User.Email), Id = e.Id })
                .Where(q => q.Id == id).FirstOrDefault();

            if (!consultation.Emails.Contains(teacher))
            {
                return RedirectToAction(nameof(Edit));
            }

            FindStudents();

            var createCon = new CreateConsultationViewModel
            {
                Id =consultation.Id,
                Date=consultation.Date,
                Room =consultation.Room,
                Students =consultation.AppStudents.ToList()
            };

            return View(createCon);
        }
        [Authorize(Roles = "Teacher")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit(CreateConsultationViewModel createConsultationViewModel)
        {
            FindStudents();

            try
            {

                if (ModelState.IsValid)
                {
                    var consultation = _context.Consultations.Where(x => x.Id == createConsultationViewModel.Id).FirstOrDefault();
                    var usercons = _context.UserConsultation.Where(z => z.ConsultationId == createConsultationViewModel.Id);
                    _context.UserConsultation.RemoveRange(usercons);

                    var students = new List<UserConsultation>();
                    var teacherId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                    createConsultationViewModel.Students
                        .Add(_context.AppUsers.Where(x => x.Id == teacherId).Select(o => o.Pesel).FirstOrDefault());

                    var when = createConsultationViewModel.Date;
                    if (when < DateTime.Now.AddHours(-24))
                    {
                        return RedirectToAction(nameof(Edit), new { id= consultation.Id,
                            teacher = _context.AppUsers.Where(x => x.Id == teacherId).Select(o => o.Email).FirstOrDefault()
                            , error = "Cannot edit consultaton 24h before start" });
                    }


                    foreach (var stu in createConsultationViewModel.Students)
                    {
                        var temp = _context.AppUsers.Where(q => q.Id == stu).FirstOrDefault();
                        students.Add(new UserConsultation { User = _context.AppUsers.Where(o => o.Pesel == stu).FirstOrDefault() });
                    }

                   // _context.UserConsultation.RemoveRange(students);
                    //var consultation = _context.Consultations.Where(x => x.Id == createConsultationViewModel.Id).FirstOrDefault();

                    consultation.AppUsers = students;
                    consultation.Room = createConsultationViewModel.Room;
                    consultation.Date = createConsultationViewModel.Date;
                    _context.SaveChanges();

                }

                return RedirectToAction(nameof(Edit), new { error="ok"});
            }
            catch
            {
                return View();
            }
        
        }

        // GET: Consultation/Create
        [Authorize(Roles = "Teacher")]
        public ActionResult Create(string error = null)
        {
            ViewData["Consult"] = error;
            FindStudents();
            return View();
        }

        // POST: Consultation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public ActionResult Create(CreateConsultationViewModel createConsultationViewModel)
        {
            try
            {
                var when = createConsultationViewModel.Date;
                if (when < DateTime.Now.AddHours(-24))
                {
                    return RedirectToAction(nameof(Create), new { error = "Cannot create consultaton 24h before start" });
                }
                if (ModelState.IsValid)
                {
                    var students = new List<UserConsultation>();
                    var teacherId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                    createConsultationViewModel.Students
                        .Add(_context.AppUsers.Where(x => x.Id == teacherId).Select(o => o.Pesel).FirstOrDefault());

                    foreach (var stu in createConsultationViewModel.Students)
                    {
                        var temp = _context.AppUsers.Where(q => q.Id == stu).FirstOrDefault();
                        students.Add(new UserConsultation {User=_context.AppUsers.Where(o => o.Pesel==stu).FirstOrDefault()});
                    }
                    var consult = new Consultation
                    {
                        AppUsers = students,
                        Room = createConsultationViewModel.Room,
                        Date = createConsultationViewModel.Date
                    };
                    _context.Consultations.Add(consult);
                    _context.SaveChanges();
                }

                return RedirectToAction(nameof(Create), new { error = "ok" });
            }
            catch
            {
                return View();
            }
        }

       
    }
}