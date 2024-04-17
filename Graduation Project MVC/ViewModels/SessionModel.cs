using DataAccessLayer.Etities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace presentationLayer.ViewModels
{
    public class SessionModel
    {
        public int Id { get; set; }

        public SessionTypeEnum SessionType { get; set; }

        public LocationEnum Location { get; set; }

        public GroupEnum Group { get; set; }

        public DateTime SessionTime { get; set; } = DateTime.Now;

        public Course Course { get; set; }

        //[ForeignKey("Course")]
        public int CourseId { get; set; }
        public Instructor Instructor { get; set; }
        public int InstructorId { get; set; }

        //[ForeignKey("Instructor")]
        //public int InstructorId { get; set; }

        public IEnumerable<Student> Students { get; set; }

        [Required(ErrorMessage = "Atendance Duration is Reuired")]
        [Range(2, 120, ErrorMessage = "Atendance Duration must be between 2 and 120 minutes")]
        public int? AtendanceDuration { get; set; }

    }
}
