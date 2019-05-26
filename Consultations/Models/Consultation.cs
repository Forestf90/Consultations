using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consultations.Models
{
    public class Consultation
    {
        public string Id { get; set; }
        public int Room { get; set; }
        public DateTime Date { get; set; }
        public ICollection<UserConsultation> AppUsers { get; set; }

        public Consultation()
        {
            AppUsers = new HashSet<UserConsultation>();
        }
    }
}
