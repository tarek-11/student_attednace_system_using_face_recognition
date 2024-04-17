using DataAccessLayer.Etities;
using Microsoft.AspNetCore.Http;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static DataAccessLayer.Etities.Instructor;

namespace presentationLayer.ViewModels
{
    public class InstructorModel
    {
        public int Id { get; set; }
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Name is Required !!")]
        [MaxLength(50, ErrorMessage = "the max length is 50")]

        public string Name { get; set; }

        [System.ComponentModel.DataAnnotations.Range(23, 90, ErrorMessage = "the age must be between 23 and 90")]
        public int Age { get; set; }

        public string ImageName { get; set; }
        public string OldImageName { get; set; }

        [EmailAddress(ErrorMessage ="This Email is not valid")]
        [Unique]
        public string Email { get; set; }
        [RegularExpression(@"^[0-9]{1,3}-[a-zA-Z]{3,15}-[a-zA-Z]{3,15}")] 
        public string Address { get; set; }
        public InstructorGradEnum InstructorGrad { get; set; }

        public ICollection<Course> Courses { get; set; }
        public ICollection<Session> Sesions { get; set; }

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Image Is Required")]
        public IFormFile Image { get; set; }


    }
}
