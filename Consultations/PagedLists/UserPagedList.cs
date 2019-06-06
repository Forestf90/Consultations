using Consultations.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consultations.PagedLists
{
    public class UserPagedList
    {

        public List<DisplayUserViewModel> GetUsers { get; set; }
        public DisplayUserViewModel User { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int ItemsOnPage { get; set; }

    }
}
