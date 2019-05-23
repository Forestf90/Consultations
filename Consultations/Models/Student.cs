using Consultations.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consultations.Models
{
    public class Student : IdentityUser
    {
        //public int Id { get; set; }
        public short Semester { get; set; }
        public Field Field { get; set; }
        public ICollection<StudentConsultation> Consultations { get; set; }

        public Student()
        {
            Consultations = new HashSet<StudentConsultation>();
        }

    }
}
