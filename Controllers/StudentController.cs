using AutoMapper;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Repositories;
using DataAccessLayer.Etities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using presentationLayer.Helpers;
using presentationLayer.ViewModels;
using pusinessLogicLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace presentationLayer.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public StudentController(IMapper mapper,
            UserManager<ApplicationUser> userManager,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var students = Enumerable.Empty<Student>();
            if (user.UserRoleName != "Admin")
            {
                var instructor = _unitOfWork._instructorRepository.GetInstructorByEmail(user.Email);
                var courses = Enumerable.Empty<Course>();
                courses = await _unitOfWork._courseRepository.GetCoursesByInstructor(instructor.Id);
                students = await _unitOfWork._studentRepository.GetStudentsByCoursesAsync(courses);
            }
            else
                students = await _unitOfWork._studentRepository.GetAll();
            var mappedStudents = _mapper.Map
                <IEnumerable<Student>, IEnumerable<StudentModel>>(students);

            return View(mappedStudents);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(StudentModel studentModel)
        {
            if (ModelState.IsValid)
            {

                studentModel.ImageName =
                    FileSettings.UploadFile(studentModel.Image, "images");
                var mappedStudent =
                    _mapper.Map<StudentModel, Student>(studentModel);
                var StudentCourses = await _unitOfWork._courseRepository.GetCoursesByGrade(mappedStudent.Grade);
                await _unitOfWork._studentRepository.Add(mappedStudent);
                await _unitOfWork._studentRepository.AddCoursesToStudentAsync(mappedStudent, StudentCourses);
                return RedirectToAction(nameof(Index));
            }

            return View(studentModel);
        }

        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id == null)
                return BadRequest();

            var student = await _unitOfWork._studentRepository.Get(id.Value);
            if (student == null)
                return NotFound();

            var mappedStudent = _mapper.Map<Student, StudentModel>(student);
            mappedStudent.Sessions = await _unitOfWork._sessionRepository.GetSessionsByStudentAsync(student.Id);
            mappedStudent.OldImageName = mappedStudent.ImageName;
            return View(viewName, mappedStudent);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {

            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit([FromRoute] int id, StudentModel StudentModel)
        {
            if (id != StudentModel.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (StudentModel.Image != null)
                        StudentModel.ImageName = FileSettings.UploadFile(StudentModel.Image, "images");
                    var mappedStudent = _mapper.Map<StudentModel, Student>(StudentModel);
                    var rowsAffected = await _unitOfWork._studentRepository.Update(mappedStudent);
                    if (rowsAffected > 0)
                    {
                        FileSettings.DeleteFile(StudentModel.OldImageName, "images");
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    return View(StudentModel);
                }
            }
            return View(StudentModel);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromRoute] int id, StudentModel studentModel)
        {
            if (id != studentModel.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var mappedStudent = _mapper.Map<StudentModel, Student>(studentModel);
                    var affectedRows = await _unitOfWork._studentRepository.Delete(mappedStudent);
                    if (affectedRows > 0)
                        FileSettings.DeleteFile(studentModel.ImageName, "images");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    return View(studentModel);
                }
            }
            return View(studentModel);
        }
    }
}

