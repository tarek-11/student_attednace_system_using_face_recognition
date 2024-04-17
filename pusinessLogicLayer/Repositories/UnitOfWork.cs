using BusinessLogicLayer.Interfaces;
using pusinessLogicLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public ISessionRepository _sessionRepository { get; set; }
        public ICourseRepository _courseRepository { get; set; }
        public IInstructorRepository _instructorRepository { get; set; }
        public IStudentRepository _studentRepository { get; set; }
        public UnitOfWork(IStudentRepository StudentRepository,ICourseRepository courseRepository,
            IInstructorRepository instructorRepository, ISessionRepository sessionRepository)
        {
            _studentRepository = StudentRepository;
            _courseRepository = courseRepository;
            _instructorRepository = instructorRepository;
            _sessionRepository = sessionRepository;
        }
    }
}
