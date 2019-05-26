using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consultations.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Pesel { get; set; }

        public ICollection<UserConsultation> Consultations { get; set; }

        public AppUser()
        {
            Consultations = new HashSet<UserConsultation>();
        }



    }
}
