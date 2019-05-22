using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consultations.Models
{
    public class Consultation
    {
        public int Room { get; set; }
        public DateTime Date { get; set; }
        public Teacher Teacher { get; set; }
        public ICollection<Student> Students { get; set; }

        public Consultation()
        {
            Students = new HashSet<Student>();
        }
    }
}
