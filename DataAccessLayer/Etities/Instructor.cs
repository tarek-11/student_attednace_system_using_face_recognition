using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DataAccessLayer.Etities
{
    public class Instructor
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is Required !!")]
        [MaxLength(50, ErrorMessage ="the max length is 50")]

        public string Name { get; set; }

        [Range(23, 90, ErrorMessage ="the age must be between 23 and 90")]
        public int Age { get; set; }
        /// <summary>
        /// ///////////////////
        /// </summary>
        public string Email { get; set; }
        public string Address { get; set; }
        public InstructorGradEnum InstructorGrad { get; set; }

        public ICollection<Course> Courses { get; set; }
        public ICollection<Session> Sesions { get; set; }
        /// <summary>
        /// /////////////////
        /// </summary>
        public DateTime DateOfCreation { get; set; } = DateTime.Now;

        public bool IsDeleted { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public string ApplicationUserId { get; set; }

        public string ImageName { get; set; }

    }
    public enum InstructorGradEnum
    {
        HelperTeacher, Teacher, Doctor, Professor
    }
}
