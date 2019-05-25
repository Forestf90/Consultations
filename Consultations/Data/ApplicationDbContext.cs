using System;
using System.Collections.Generic;
using System.Text;
using Consultations.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Consultations.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Consultation> Consultations { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Teacher>();
            //modelBuilder.Entity<Student>();

            modelBuilder.Entity<StudentConsultation>()
            .HasKey(pt => new { pt.Id });

            modelBuilder.Entity<StudentConsultation>()
                        .HasOne(pt => pt.Student)
                        .WithMany(t => t.Consultations)
                        .HasForeignKey(pt => pt.StudentId);

            modelBuilder.Entity<StudentConsultation>()
                        .HasOne(pt => pt.Consultation)
                        .WithMany(p => p.Students)
                        .HasForeignKey(pt => pt.ConsultationId);
        }

        public void AddTeacher(Teacher t)
        {
            Teachers.Add(t);
            SaveChanges();
        }
    }
}
