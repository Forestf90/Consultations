using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consultations.ViewModels
{
    public class DisplayConsultationViewModel
    {
        public string Id { get; set; }
        public string TeacherId { get; set; }
        public string Teacher { get; set; }
        public int Students { get; set; }
        public int Room { get; set; }
        public DateTime Date { get; set; }

    }
}
