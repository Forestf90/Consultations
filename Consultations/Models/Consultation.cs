using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consultations.Models
{
    public class Consultation
    {
        public int Id { get; set; }
        public int Room { get; set; }
        public DateTime Date { get; set; }
        public Teacher Teacher { get; set; }
        public ICollection<StudentConsultation> Students { get; set; }

        public Consultation()
        {
            Students = new HashSet<StudentConsultation>();
        }
    }
}
