using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consultations.Data;
using Consultations.Models;
using Consultations.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Consultations.Controllers
{
    public class TeacherController : Controller
    {
        private static ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public TeacherController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var list = _context.AppUsers.Select(n => new DisplayUserViewModel
            {
                FirstName = n.FirstName,
                LastName = n.LastName,
                Email = n.Email,
                Pesel = n.Pesel,
                PhoneNumber = n.PhoneNumber,
                Roles = _context.UserRoles.Join(_context.Roles, e => e.RoleId, r => r.Id, (e, r) => new {
                    RName = r.Name,
                    UId = e.UserId
                })
                    .Where(q => q.UId == n.Id).Select(p => p.RName)
            });

            var listTeacher = list.Where(q => q.Roles.Contains("Teacher"));
            return View(listTeacher);
        }

        //[Authorize(Roles="Teacher")]
        //public IActionResult Panel()
        //{
        //    return View();
        //}

    }
}