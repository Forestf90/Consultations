using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consultations.Models
{
    public class StudentConsultation
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public int ConsultationId { get; set; }
        public Consultation Consultation { get; set; }

    }
}
