using AutoMapper;
using BusinessLogicLayer.BllHelpers;
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
using System.Text.Json;
using System.Threading.Tasks;

namespace presentationLayer.Controllers
{
    [Authorize]
    public class SessionController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public SessionController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var sessions = await _unitOfWork._sessionRepository.GetAll();
            var mappedSessions =
                _mapper.Map<IEnumerable<Session>, IEnumerable<SessionModel>>(sessions);
            return View(mappedSessions);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(SessionModel sessionModel)
        {
            var sessionCourse = await _unitOfWork._courseRepository.Get(sessionModel.CourseId);
            sessionModel.InstructorId = sessionCourse.InstructorId;
            if (ModelState.IsValid)
            {
                var mappedSession = _mapper.Map<SessionModel, Session>(sessionModel);
                await _unitOfWork._sessionRepository.Add(mappedSession);
                // just for Ex.
                var sessionStudents = await _unitOfWork._studentRepository.GetAll();
                var studentIds = sessionStudents.Select(ss => ss.Id);
                var sessionId = mappedSession.Id;
                var sessionJson = new SessionJson
                {
                    SessionId = sessionId,
                    Students = new List<int>()
                };

                foreach (var studentId in studentIds)
                {
                    var studentJson = studentId;

                    sessionJson.Students.Add(studentJson);
                }

                var sessionJsonString = JsonSerializer.Serialize(sessionJson);
                await _unitOfWork._studentRepository.AddStudentsToSession(sessionJsonString);
                //int sessionId, int[] studentIds
                //string json = @"{ ""SessionId"": 1, ""StudentIds"": [1,2,3] }";
                //await studentRepository.AddSessionToStudentsAsync(json);
                //var sessionJson = JsonSerializer.Serialize(new
                //{
                //    sessionId = mappedSession.Id,
                //    students = sessionStudents
                //});
                //////////////
                return RedirectToAction(nameof(Index));
            }
            return View(sessionModel);
        }

        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id == null)
            {
                return NotFound();
            }
            var mySession = await _unitOfWork._sessionRepository.Get(id.Value);
            if (mySession == null)
            {
                return NotFound();
            }
            var mappedSession = _mapper.Map<Session, SessionModel>(mySession);
            mappedSession.Course = await _unitOfWork._courseRepository.Get(mappedSession.CourseId);
            mappedSession.Students = await _unitOfWork._studentRepository.GetStudentsBySessionAsync(mySession);

            return View(viewName, mappedSession);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id, "Edit");
            //if (id == null)
            //{
            //    return NotFound();
            //}
            //var c = _SessionRepository.Get(id.Value);
            //if (c == null)
            //{
            //    return NotFound();
            //}
            //return View(c);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit([FromRoute] int id, SessionModel c)
        {
            if (id != c.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var session = await _unitOfWork._sessionRepository.Get(id);
                    c.Instructor = session.Instructor; 
                    c.InstructorId = session.InstructorId; 
                    var mappedSession = _mapper.Map<SessionModel, Session>(c);
                    await _unitOfWork._sessionRepository.Update(mappedSession);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
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
        public async Task<IActionResult> Delete([FromRoute] int id, SessionModel sessionModel)
        {
            ModelState.Clear();
            var c = await _unitOfWork._courseRepository.Get(sessionModel.CourseId);
            sessionModel.Course = c;
            sessionModel.Course.Code = c.Code;
            sessionModel.InstructorId = c.InstructorId; 
            sessionModel.Instructor = c.Instructor; 
            if (id != sessionModel.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var mappedSession = _mapper.Map<SessionModel, Session>(sessionModel);
                    await _unitOfWork._sessionRepository.Delete(mappedSession);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    return View(sessionModel);
                }
            }
            else
            {
            //    // Create a list to hold the errors
            //    var errors = new List<string>();

            //    // Iterate over the ModelState.Values collection
            //    foreach (var state in ModelState.Values)
            //    {
            //        if (state.Errors != null && state.Errors.Count > 0)
            //        {
            //            // Iterate over the Errors collection of the ModelStateEntry object
            //            foreach (var error in state.Errors)
            //            {
            //                // Add the error message to the list
            //                errors.Add(error.ErrorMessage);
            //            }
            //        }
            //    }
            }
            return View(sessionModel);
        }

    }
}

