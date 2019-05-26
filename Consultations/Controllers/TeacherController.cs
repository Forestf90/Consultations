using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consultations.Data;
using Consultations.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Consultations.Controllers
{
    public class TeacherController : Controller
    {
        private static ApplicationDbContext _context;

        public TeacherController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {

            return View(_context.Teachers.ToList());
        }

        [Authorize(Roles="Teacher")]
        public IActionResult Panel()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Teacher teacher)
        {
            try {
                _context.AddTeacher(teacher);
            }
            catch
            {

            }
            return View();
        }
    }
}