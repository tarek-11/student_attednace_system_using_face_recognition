using DataAccessLayer.Context;
using DataAccessLayer.Etities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using presentationLayer.ViewModels;
using pusinessLogicLayer.Interfaces;
using pusinessLogicLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace presentationLayer.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IInstructorRepository _instructorRepository;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IInstructorRepository instructorRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _instructorRepository = instructorRepository;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var appUser = new ApplicationUser()
                {
                    UserName = model.Email.Split('@')[0],
                    Email = model.Email,
                    Isagree = model.IsAgree
                };

                
                var instructor = _instructorRepository.GetInstructorByEmail(model.Email);
                if(instructor != null && instructor.ApplicationUserId == null)
                {
                    var result = await _userManager.CreateAsync(appUser, model.PassWord);
                    if (result.Succeeded)
                    {
                        var user = await _userManager.FindByEmailAsync(appUser.Email);
                        var roleState = await _userManager.AddToRoleAsync(user, "User");
                        user.UserRoleName = "User";
                        await _userManager.UpdateAsync(user);
                        instructor.ApplicationUserId = user.Id;
                        var rows = _instructorRepository.Update(instructor);
                        return RedirectToAction(nameof(LogIn));
                    }
                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                }

            }
            return View(model);
        }


        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(LogInModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if(user != null)
                {
                    bool f = await _userManager.CheckPasswordAsync(user, model.PassWord);
                    if (f)
                    {
                        var result = await _signInManager.PasswordSignInAsync
                            (user, model.PassWord, model.RememberMe, false);
                        if (result.Succeeded)
                            return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError(string.Empty, "Password isn't correct");
                }
                ModelState.AddModelError(string.Empty, "Email isn't correct");
            }
            return View(model);
        }

        public new async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(LogIn));
        }
    }
}
