using BusinessLogicLayer.BllHelpers;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Context;
using DataAccessLayer.Etities;
using Microsoft.EntityFrameworkCore;
using pusinessLogicLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Repositories
{
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        private readonly FacDbContext _facDbContext;
        private readonly IServiceProvider _serviceProvider;

        public StudentRepository(FacDbContext facDbContext, IServiceProvider serviceProvider) :base(facDbContext)
        {
            _facDbContext = facDbContext;
            _serviceProvider = serviceProvider;
        }

        public async Task<IEnumerable<Student>> GetStudentsByCoursesAsync(IEnumerable<Course> courses)
        {
            var courseIds = courses.Select(c => c.Id).ToList();
            var students = Enumerable.Empty<Student>().ToList();
            try
            {
            students = await _facDbContext.Students
                .Where(s => s.CourseStudents.Any(cs => courseIds.Contains(cs.CourseId)))
                .ToListAsync();
                ///return _context.Students
                //.Where(s => s.CourseStudents.Any(cs => cs.CourseId == courseId))
                //.ToListAsync();
            }
            catch (Exception ex)
            {

            }

            return students;
        }
        public async Task AddCoursesToStudentAsync(Student student, ICollection<Course> courses)
        {
            if(courses != null)
                foreach (var course in courses)
                {
                    var studentCourse = new CourseStudent
                    {
                        StudentId = student.Id,
                        CourseId = course.Id
                    };

                    await _facDbContext.CourseStudent.AddAsync(studentCourse);
                }

            await _facDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Student>> GetStudentsBySessionAsync(Session session)
        {
            return await _facDbContext.Students
                .Include(s => s.SessionStudents)
                .Where(s => s.SessionStudents.Any(sc => sc.SessionId == session.Id))
                .ToListAsync();
        }

        //public void AddStudentsToSessionAsync(IEnumerable<Student> students, Session session)
        //{
        //    if(students != null)
        //        foreach (var s in students)
        //        {
        //            var sessionStudent = new SessionStudent
        //            {
        //                StudentId = s.Id,
        //                SessionId = session.Id
        //            };
        //            _facDbContext.SessionStudent.AddAsync(sessionStudent);
        //        }

        //    _facDbContext.SaveChangesAsync();
        //}
        public async Task AddStudentsToSession(string sessionObject)
        {
            var sessionData = JsonSerializer.Deserialize<SessionJson>(sessionObject);

            var sessionId = sessionData.SessionId;
            var studentIds = sessionData.Students;
            var sessionCourse = Enumerable.Empty<Course>().ToList();


            var session = _facDbContext.Sessions.FirstOrDefault(s => s.Id == sessionId);
            sessionCourse.Add(_facDbContext.Courses.Where(c => c.Id == session.CourseId).FirstOrDefault());
            var allStudents = await GetStudentsByCoursesAsync(sessionCourse);

            foreach (var studentId in studentIds)
            {
                Student validStudent = allStudents.FirstOrDefault(s => s.Id == studentId);
                if (validStudent != null)
                {
                    var sessionStudent = new SessionStudent
                    {
                        //SessionId = session.Id,
                        Session = session,
                        //StudentId = validStudent.Id,
                        Student = validStudent
                    };
                    try
                    {
                        await _facDbContext.SessionStudent.AddAsync(sessionStudent);
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }

                }

            }

            await _facDbContext.SaveChangesAsync();
        }
        
    }
}
