using DataAccessLayer.Etities;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace presentationLayer.ViewModels
{
    public class CourseModel
    {
        public int Id { get; set; }
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Name is Required !!")]
        [MaxLength(50, ErrorMessage = "the max length is 50")]
        public string Name { get; set; }

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Code is Required !!")]
        [Unique()]

        public string Code { get; set; }

        public Instructor Instructor;

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Instructor is Required !!")]
        public int InstructorId { get; set; }

        public GradeEnum Grade { get; set; }


        public ICollection<Student> Students { get; set; }

        public ICollection<Session> Sessions { get; set; }
    }
}
