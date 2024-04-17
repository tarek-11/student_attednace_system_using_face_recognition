using BusinessLogicLayer.BllHelpers;
using DataAccessLayer.Etities;
using pusinessLogicLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IStudentRepository : IGenericRepository<Student>
    {
        Task<IEnumerable<Student>> GetStudentsByCoursesAsync(IEnumerable<Course> courses);
        Task AddCoursesToStudentAsync(Student student, ICollection<Course> courses);

        Task<IEnumerable<Student>> GetStudentsBySessionAsync(Session session);
        //void AddStudentsToSessionAsync(IEnumerable<Student> students, Session session);
        public Task AddStudentsToSession(string sessionObject);
    }
}
