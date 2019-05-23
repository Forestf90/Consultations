using Consultations.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consultations.Models
{
    public class Teacher : User
    {
        public Title Title { get; set; }
        public ICollection<StudentConsultation> Consultations { get; set; }

        public Teacher()
        {
            Consultations = new HashSet<StudentConsultation>();
        }
    }
}
