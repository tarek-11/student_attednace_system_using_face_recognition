using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Etities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using presentationLayer.ViewModels;
using pusinessLogicLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace presentationLayer.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public CourseController(IMapper mapper,
            UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var courses = Enumerable.Empty<Course>();
            if (user.UserRoleName != "Admin")
            {
                var instructor = _unitOfWork._instructorRepository.GetInstructorByEmail(user.Email);
                courses = await _unitOfWork._courseRepository.GetCoursesByInstructor(instructor.Id);
            }
            else
                courses = await _unitOfWork._courseRepository.GetAll();

            var mappedCourses =
                _mapper.Map<IEnumerable<Course>, IEnumerable<CourseModel>>(courses);
            return View(mappedCourses);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CourseModel courseModel)
        {
            if (ModelState.IsValid)
            {
                var mappedCourse = _mapper.Map<CourseModel, Course>(courseModel);
                await _unitOfWork._courseRepository.Add(mappedCourse);
                return RedirectToAction(nameof(Index));
            }
            return View(courseModel);
        }

        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if(id == null)
            {
                return NotFound();
            }
            var myCourse =await _unitOfWork._courseRepository.Get(id.Value);
            if(myCourse == null)
            {
                return NotFound();
            }
            var mappedCourse = _mapper.Map<Course, CourseModel>(myCourse);
            return View(viewName, mappedCourse);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id, "Edit");
            //if (id == null)
            //{
            //    return NotFound();
            //}
            //var c = _CourseRepository.Get(id.Value);
            //if (c == null)
            //{
            //    return NotFound();
            //}
            //return View(c);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit([FromRoute]int id, CourseModel c)
        {
            if(id != c.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var mappedCourse = _mapper.Map<CourseModel, Course>(c);
                    await _unitOfWork._courseRepository.Update(mappedCourse);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    return View(c);
                }
            }
            return View(c);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromRoute] int id, CourseModel c)
        {
            if (id != c.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var mappedCourse = _mapper.Map<CourseModel, Course>(c);
                    await _unitOfWork._courseRepository.Delete(mappedCourse);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    return View(c);
                }
            }
            return View(c);
        }

    }
}
