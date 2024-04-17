using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Etities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using pusinessLogicLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace presentationLayer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public UserController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var users = _userManager.Users;
            //_userManager.GetRolesAsync(someuser);
            //await _userManager.AddToRoleAsync(user, "Admin");
            return View(users);
        }
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> Create(ApplicationUser studentModel)
        //{
        //    if (ModelState.IsValid)
        //    {

        //        studentModel.ImageName =
        //            FileSettings.UploadFile(studentModel.Image, "images");
        //        var mappedStudent =
        //            _mapper.Map<StudentModel, Student>(studentModel);
        //        var StudentCourses = _courseRepository.GetCoursesByGrade(mappedStudent.Grade);
        //        mappedStudent.Courses = await StudentCourses;
        //        await _userManager.Add(mappedStudent);
        //        return RedirectToAction(nameof(Index));
        //    }

        //    return View(studentModel);
        //}

        public async Task<IActionResult> Details(string id, string viewName = "Details")
        {
            if (id == null)
                return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            return View(viewName, user);
        }

        public async Task<IActionResult> Edit(string id)
        {

            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, ApplicationUser updatedApplicationUser)
        {
            if (id != updatedApplicationUser.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(id);
                    user.UserName = updatedApplicationUser.UserName;
                    user.PhoneNumber = updatedApplicationUser.PhoneNumber;
                    if(user.UserRoleName != "Admin")
                    {
                        await _userManager.RemoveFromRoleAsync(user, user.UserRoleName);
                        await _userManager.AddToRoleAsync(user, updatedApplicationUser.UserRoleName);
                        user.UserRoleName = updatedApplicationUser.UserRoleName;
                    }

                    await _userManager.UpdateAsync(user);
                    //_roleManager.

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    return View(updatedApplicationUser);
                }
            }
            return View(updatedApplicationUser);
        }

        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string id, ApplicationUser applicationUser)
        {
            if (id != applicationUser.Id)
            {
                return BadRequest(); 
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(id);
                    if (user.UserRoleName == "Admin")
                        return View(applicationUser);
                    var userInstructor = _unitOfWork._instructorRepository.GetInstructorByEmail(user.Email);
                    userInstructor.ApplicationUserId = null;
                    await _userManager.DeleteAsync(user);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    return View(applicationUser);
                }
            }
            return View(applicationUser);
        }
    }
}
