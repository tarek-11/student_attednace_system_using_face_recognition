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
    public class InstructorRepository : GenericRepository<Instructor> , IInstructorRepository
    {
        private readonly FacDbContext _dbContext;

        public InstructorRepository(FacDbContext dbContext):base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Instructor GetInstructorByEmail(string email)
        {
            return  _dbContext.Set<Instructor>().Where(I => I.Email == email).FirstOrDefault();
        }
    }
}
