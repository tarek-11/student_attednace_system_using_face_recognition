using DataAccessLayer.Etities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Context
{
    public class FacDbContext : IdentityDbContext<ApplicationUser>
    {
        public FacDbContext(DbContextOptions<FacDbContext> options):base(options)
        {
              
        }


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //   => optionsBuilder.UseSqlServer("server = .; database = FacDb; trustedConnection = true");



        public DbSet<Course> Courses { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<CourseStudent> CourseStudent { get; set; }
        public DbSet<SessionStudent> SessionStudent { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CourseStudent>()
                .HasKey(cs => new { cs.CourseId, cs.StudentId });
            modelBuilder.Entity<CourseStudent>()
                .HasOne(cs => cs.Student)
                .WithMany(s => s.CourseStudents)
                .HasForeignKey(cs => cs.StudentId);
            modelBuilder.Entity<CourseStudent>()
                .HasOne(cs => cs.Course)
                .WithMany(c => c.CourseStudents)
                .HasForeignKey(cs => cs.CourseId);
            ////////////////////
            modelBuilder.Entity<SessionStudent>()
                .HasKey(cs => new { cs.SessionId, cs.StudentId });
            modelBuilder.Entity<SessionStudent>()
                .HasOne(cs => cs.Student)
                .WithMany(s => s.SessionStudents)
                .HasForeignKey(cs => cs.StudentId);
            modelBuilder.Entity<SessionStudent>()
                .HasOne(cs => cs.Session)
                .WithMany(c => c.SessionStudents)
                .HasForeignKey(cs => cs.SessionId);
            modelBuilder.Entity<Session>()
                .HasOne(s => s.Instructor)
                .WithMany(i => i.Sesions)
                .HasForeignKey(s => s.InstructorId)
                .OnDelete(DeleteBehavior.Restrict);

        }


    }
}
