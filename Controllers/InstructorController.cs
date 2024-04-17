using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Etities;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles ="Admin")]
    public class InstructorController : Controller
    {
        private readonly IUnitOfWork _uninOfWork;
        private readonly IMapper _mapper;
        public InstructorController(IUnitOfWork uninOfWork, IMapper mapper)
        {
            _uninOfWork = uninOfWork;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var instructors = await _uninOfWork._instructorRepository.GetAll();
            var mappedInstructors = _mapper.Map
                <IEnumerable<Instructor>, IEnumerable<InstructorModel>>(instructors);

            return View(mappedInstructors);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(InstructorModel instructorModel)
        {
            if (ModelState.IsValid)
            {
                instructorModel.ImageName =
                    FileSettings.UploadFile(instructorModel.Image, "images");
                var mappedInstructor =
                    _mapper.Map<InstructorModel, Instructor>(instructorModel);
                await _uninOfWork._instructorRepository.Add(mappedInstructor);
                return RedirectToAction(nameof(Index));
            }

            return View(instructorModel);
        }

        public async Task<IActionResult> Details(int? id,string viewName = "Details")
        {
            if (id == null)
                return BadRequest();

            var instructor = await _uninOfWork._instructorRepository.Get(id.Value);
            if (instructor == null)
                return NotFound();
            
            var mappedInstructor = _mapper.Map<Instructor, InstructorModel>(instructor);

            mappedInstructor.OldImageName = mappedInstructor.ImageName;
            return View(viewName, mappedInstructor);
        }

        public async Task<IActionResult> Edit(int? id)
        {

            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, InstructorModel InstructorModel)
        {
            if (id != InstructorModel.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if(InstructorModel.Image != null)
                        InstructorModel.ImageName = FileSettings.UploadFile(InstructorModel.Image, "images");
                    var mappedInstructor = _mapper.Map<InstructorModel, Instructor>(InstructorModel);
                    var rowsAffected = await _uninOfWork._instructorRepository.Update(mappedInstructor);
                    if(rowsAffected > 0)
                    {
                        FileSettings.DeleteFile(InstructorModel.OldImageName, "images");
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    return View(InstructorModel);
                }
            }
            return View(InstructorModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, InstructorModel instructorModel)
        {
            if (id != instructorModel.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var mappedInstructor = _mapper.Map<InstructorModel, Instructor>(instructorModel);
                    var affectedRows = await _uninOfWork._instructorRepository.Delete(mappedInstructor);
                    if (affectedRows > 0)
                        FileSettings.DeleteFile(instructorModel.ImageName, "images");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    return View(instructorModel);
                }
            }
            return View(instructorModel);
        }
    }
}
