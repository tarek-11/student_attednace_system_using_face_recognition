using DataAccessLayer.Etities;
using Microsoft.AspNetCore.Http;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace presentationLayer.ViewModels
{
    public class StudentModel
    {
        public int Id { get; set; }

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Name is Required !!")]
        [MaxLength(50, ErrorMessage = "the max length is 50")]
        public string Name { get; set; }

        public GradeEnum Grade { get; set; }

        [System.ComponentModel.DataAnnotations.Range(18, 90, ErrorMessage = "the age must be between 18 and 90")]
        public int Age { get; set; }

        [EmailAddress()]
        [Unique]
        public string Email { get; set; }

        [RegularExpression(@"^[0-9]{1,3}-[a-zA-Z]{3,15}-[a-zA-Z]{3,15}")] //not here
        public string Address { get; set; }

        public string ImageName { get; set; }

        public string OldImageName { get; set; }


        public ICollection<Course> Courses { get; set; }

        public IEnumerable<Session> Sessions { get; set; }

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Image Is Required")]
        public IFormFile Image { get; set; }
    }
}
