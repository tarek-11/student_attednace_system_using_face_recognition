using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Etities
{
    public class Session
    {
        public int Id { get; set; }

        public SessionTypeEnum SessionType { get; set; }

        public LocationEnum Location { get; set; }

        public GroupEnum Group { get; set; }

        public DateTime SessionTime { get; set; } = DateTime.Now;

        public Course Course { get; set; }

        [ForeignKey("CourseId")]
        public int CourseId { get; set; }
        
        public Instructor Instructor { get; set; }

        [ForeignKey("Instructor")]
        public int InstructorId { get; set; }

        public ICollection<SessionStudent> SessionStudents { get; set; }

        public int? Atendanceduration { get; set; }


    }

    public enum LocationEnum
    {
        Hall_1, Hall_2, Hall_3, Hall_4, Lab_1_IOT, Lab_2_IOT, Lab_1_network, Lab_2_network,
        Lab_1_embedded, Lab_2_embedded
    }
    public enum SessionTypeEnum
    {
        Lecture, Section, Lab
    }
    public enum GroupEnum
    {
        AllStudents, Group_A, Group_B, Section_1, Section_2, Section_3, Section_4
    }
}
