using AttendanceManagementSystem.Models;
using AttendanceManagementSystem.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManagementSystem.Controllers;

public class StudentController : Controller
{
    private          StudentRepository repository;

    public StudentController()
    {
        repository = new StudentRepository();
    }

    // GET: /Student
    public async Task<IActionResult> Index()
    {
        List<Student> students =
            await repository.GetAllStudentsAsync();

        return View(students);
    }


    // GET: /Student/Create
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }


    // POST: /Student/Create
    [HttpPost]
    public async Task<IActionResult> Create(Student student)
    {
        if (!ModelState.IsValid)
        {
            return View(student);
        }

        await repository.AddStudent(student);

        return RedirectToAction("Index");
    }


    // GET: /Student/Edit/5
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        Student? student =
            await repository.GetStudentBYID(id);

        if (student == null)
        {
            return NotFound();
        }

        return View(student);
    }


    // POST: /Student/Edit
    [HttpPost]
    public async Task<IActionResult> Edit(Student student)
    {
        if (!ModelState.IsValid)
        {
            return View(student);
        }

        await repository.UpdateStudent(student);

        return RedirectToAction("Index");
    }


    // POST: /Student/Delete/5
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await repository.DeleteStudent(id);

        return RedirectToAction("Index");
    }
}