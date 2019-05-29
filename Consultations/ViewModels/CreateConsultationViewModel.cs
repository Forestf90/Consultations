using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Consultations.ViewModels
{
    public class CreateConsultationViewModel
    {
        public string Id { get; set; }
        [Required]
        public List<string> Students { get; set; }
        [Required]
        [Range(1, 500)]
        public int Room { get; set; }
        [Required]
        public DateTime Date { get; set; }

    }
}
