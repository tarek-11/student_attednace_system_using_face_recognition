using System.ComponentModel.DataAnnotations;
using ServiceStack.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;
using ForeignKeyAttribute = System.ComponentModel.DataAnnotations.Schema.ForeignKeyAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Etities
{
    public class Course
    {
        public int Id { get; set; }
        [Required()]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required()]
        [Unique()]
        public string Code { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.Now;
        /// <summary>
        /// ////////////
        /// </summary>

        public GradeEnum Grade { get; set; }

        public ICollection<CourseStudent> CourseStudents { get; set; }

        public ICollection<Session> Sessions { get; set; }
        /// <summary>
        /// ///////
        /// </summary>
        public bool IsDeleted { get; set; }

        public Instructor Instructor;

        //[ForeignKey("Department")]
        public int InstructorId { get; set; }

    }
    public enum GradeEnum
    {
        frist, second, third, fourth
    }
}
