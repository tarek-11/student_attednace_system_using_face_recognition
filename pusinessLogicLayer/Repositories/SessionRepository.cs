using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Context;
using DataAccessLayer.Etities;
using Microsoft.EntityFrameworkCore;
using pusinessLogicLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Repositories
{
    public class SessionRepository : GenericRepository<Session> , ISessionRepository
    {
        private readonly FacDbContext _facDbContext;

        public SessionRepository(FacDbContext facDbContext):base(facDbContext)
        {
            _facDbContext = facDbContext;
        }

        public IQueryable<Session> GetSessionsByCourses(IQueryable<Course> courses)
        {
            IQueryable<Session> allSessions = Queryable.DefaultIfEmpty<Session>(null);
            foreach (var c in courses)
            {
                var sessions = _facDbContext.Set<Session>().Where(s => s.CourseId == c.Id);
                foreach (var s in sessions)
                {
                allSessions.Append(s);
                }
            }
            return allSessions;
        }
        public async Task<IEnumerable<Session>> GetSessionsByStudentAsync(int studentId)
        {
            var sessions = await _facDbContext.Sessions
            .Include(s => s.SessionStudents)
                .ThenInclude(ss => ss.Student)
            .Where(s => s.SessionStudents.Any(ss => ss.StudentId == studentId))
            .OrderBy(s => s.CourseId) // Sort by course property
            .ToListAsync();
            return sessions;

            //.Sessions
            //.Include(s => s.SessionStudents)
            //.Where(s => s.SessionStudents.Any(sc => sc.StudentId == studentId))
            //.ToListAsync();
        }
    }
}

