using DataAccessLayer.Context;
using DataAccessLayer.Etities;
using Microsoft.EntityFrameworkCore;
using pusinessLogicLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pusinessLogicLayer.Repositories
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        private readonly FacDbContext _dbcon;

        public CourseRepository(FacDbContext Dbcon):base(Dbcon)
        {
            _dbcon = Dbcon;
        }

        public async Task<ICollection<Course>> GetCoursesByGrade(GradeEnum grade)
        {
            return await _dbcon.Set<Course>().Where(c => c.Grade == grade)?.ToListAsync();
        }

        public async Task<IQueryable<Course>> GetCoursesByInstructor(int id)
        {
            return _dbcon.Set<Course>().Where(c => c.InstructorId == id);
        }
    }

}
