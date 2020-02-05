using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class UserViewModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string Insertion { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string ImageUrl { get; set; }
        public string Gender { get; set; }
        public DateTime? DOB { get; set; }
        public int? CreatedBy { get; set; }
        public int? CompanyId { get; set; }
        public bool? IsActive { get; set; }
    }
}
