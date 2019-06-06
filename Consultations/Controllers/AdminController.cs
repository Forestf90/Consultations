using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consultations.Data;
using Consultations.Models;
using Consultations.PagedLists;
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

        public IActionResult Index(int page = 1, int itemsOnPage = 5)
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

            var pagedList = new UserPagedList();
            pagedList.CurrentPage = page;
            pagedList.TotalPages = list.Count() / itemsOnPage;
            if (list.Count() % itemsOnPage > 0)
                pagedList.TotalPages++;
            pagedList.ItemsOnPage = itemsOnPage;

            pagedList.GetUsers = new List<DisplayUserViewModel>();
            pagedList.GetUsers = list.Skip((page - 1) * itemsOnPage)
                .Take(itemsOnPage).ToList();

            return View(pagedList);
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


        [HttpPost]
        public IActionResult AdminRole(string email)
        {
            var user = _context.AppUsers.Where(q => q.Email == email).FirstOrDefault();

            //_userManager.AddToRole(user, "Student");
            var roles = _userManager.GetRolesAsync(user).Result;

            if (roles.Contains("Admin"))
            {
                _userManager.RemoveFromRoleAsync(user, "Admin");
                
            }
            else
            {
                _userManager.AddToRoleAsync(user, "Admin");
                
            }

            return RedirectToAction(nameof(Index));
        }

    }
}