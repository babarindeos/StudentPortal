using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Web.Data;
using StudentPortal.Web.Models;
using StudentPortal.Web.Models.Entities;
using System.Runtime.InteropServices;

namespace StudentPortal.Web.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDBContext _DbContext;
        public StudentsController(ApplicationDBContext DbContext)
        {
            _DbContext = DbContext;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddStudentViewModel viewModel)
        {
            Student student = new Student
            {
                Name = viewModel.Name,
                Email = viewModel.Email,
                Phone = viewModel.Phone,
                Subscribed = viewModel.Subscribed
            };

            await _DbContext.Students.AddAsync(student);
            await _DbContext.SaveChangesAsync();

            //return View();
            return RedirectToAction("List", "Students");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var students = await _DbContext.Students.ToListAsync();
            return View(students);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var student = await _DbContext.Students.FindAsync(id);

            return View(student);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(Student viewModel)
        {
            var student = await _DbContext.Students.FindAsync(viewModel.Id);
            
            if (student is not null)
            {
                student.Name = viewModel.Name;
                student.Email = viewModel.Email;
                student.Phone = viewModel.Phone;
                student.Subscribed = viewModel.Subscribed;

                await _DbContext.SaveChangesAsync();

            }

            return RedirectToAction("List", "Students");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var student = await _DbContext.Students.FindAsync(Id);

            if (student is not null)
            {
                _DbContext.Students.Remove(student);
               await  _DbContext.SaveChangesAsync();
            }

            return RedirectToAction("List", "Students");

        }

        [HttpPost]
        public async Task<IActionResult> Destroy(Student viewModel)
        {
            var student = await _DbContext.Students.FirstOrDefaultAsync(x => x.Id == viewModel.Id);

            if (student is not null)
            {
                _DbContext.Students.Remove(student);
                await _DbContext.SaveChangesAsync();
            }

            return RedirectToAction("List", "Students");
        }
    }
}
