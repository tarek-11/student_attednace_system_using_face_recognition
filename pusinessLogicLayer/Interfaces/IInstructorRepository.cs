using DataAccessLayer.Etities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pusinessLogicLayer.Interfaces
{
    public interface IInstructorRepository : IGenericRepository<Instructor>
    {
         Instructor GetInstructorByEmail(string email);
    }
}
