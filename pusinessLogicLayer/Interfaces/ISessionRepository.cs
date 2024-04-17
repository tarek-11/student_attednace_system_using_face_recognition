using DataAccessLayer.Etities;
using pusinessLogicLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface ISessionRepository : IGenericRepository<Session>
    {
        IQueryable<Session> GetSessionsByCourses(IQueryable<Course> courses);
        Task<IEnumerable<Session>> GetSessionsByStudentAsync(int studentId);
    }
}
