using DataAccessLayer.Etities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pusinessLogicLayer.Interfaces
{
    public interface ICourseRepository : IGenericRepository<Course>
    {
        Task<ICollection<Course>> GetCoursesByGrade(GradeEnum grade);
        Task<IQueryable<Course>> GetCoursesByInstructor(int id);

    }
}
