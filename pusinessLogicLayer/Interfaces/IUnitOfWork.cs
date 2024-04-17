using pusinessLogicLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IUnitOfWork
    {
        public ISessionRepository _sessionRepository { get; set; }
        public ICourseRepository _courseRepository { get; set; }
        public IInstructorRepository _instructorRepository { get; set; }
        public IStudentRepository _studentRepository { get; set; }
    }
}
