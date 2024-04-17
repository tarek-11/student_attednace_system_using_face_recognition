using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Etities
{
    public class Student
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is Required !!")]
        [MaxLength(50, ErrorMessage = "the max length is 50")]
        public string Name { get; set; }

        public GradeEnum Grade { get; set; }

        [Range(18, 90, ErrorMessage = "the age must be between 18 and 90")]
        public int Age { get; set; }
        
        [EmailAddress()]
        public string Email { get; set; }
        
        [RegularExpression(@"^[0-9]{1,3}-[a-zA-Z]{3,15}-[a-zA-Z]{3,15}")] //not here
        public string Address { get; set; }

        public string ImageName { get; set; }

        public ICollection<CourseStudent> CourseStudents { get; set; }

        public ICollection<SessionStudent> SessionStudents { get; set; }

    }
}
