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
        public DbSet<AppUser> AppUsers { get; set; }
       // public DbSet<Student> Students { get; set; }
        public DbSet<Consultation> Consultations { get; set; }
        public DbSet<UserConsultation> UserConsultation { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Teacher>();
            //modelBuilder.Entity<Student>();

            modelBuilder.Entity<UserConsultation>()
            .HasKey(pt => new { pt.Id });

            modelBuilder.Entity<UserConsultation>()
                        .HasOne(pt => pt.User)
                        .WithMany(t => t.Consultations)
                        .HasForeignKey(pt => pt.UserId);

            modelBuilder.Entity<UserConsultation>()
                        .HasOne(pt => pt.Consultation)
                        .WithMany(p => p.AppUsers)
                        .HasForeignKey(pt => pt.ConsultationId);
        }

    }
}
