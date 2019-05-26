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
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private static ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<AppUser> userManager)
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
                Roles =_context.UserRoles.Join(_context.Roles, e => e.RoleId, r => r.Id, (e, r) => new {
                    RName = r.Name,
                    UId = e.UserId
                })
                .Where(q => q.UId == n.Id).Select(p => p.RName)
                //.Roles.Where(q => q.).Select(g => g.Name)
            });
            return View(list);
        }


        [HttpPost]
        public  IActionResult TeacherRole(string email)
        {
            var user = _context.AppUsers.Where(q => q.Email == email).FirstOrDefault();

            //_userManager.AddToRole(user, "Student");
            var roles = _userManager.GetRolesAsync(user).Result;

            if (roles.Contains("Teacher"))
            {
                _userManager.RemoveFromRoleAsync(user, "Teacher");
                _userManager.AddToRoleAsync(user, "Student");
            }
            else
            {
                _userManager.AddToRoleAsync(user, "Teacher");
                _userManager.RemoveFromRoleAsync(user, "Student");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}