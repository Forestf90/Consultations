using System;
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
                    
                    Teacher = x.AppUsers.Where(p => teacher.Contains(p.UserId)).First().User.FirstName+" "+
                                x.AppUsers.Where(p => teacher.Contains(p.UserId)).First().User.LastName,
                    Students = x.AppUsers.Count(),
                    Room = x.Room,
                    Date = x.Date
                });
            return View(list);
        }

        // GET: Consultation/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Consultation/Create
        [Authorize(Roles = "Teacher")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Consultation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

       
    }
}