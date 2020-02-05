using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class CompnayModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string Postcode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Cell { get; set; }
        public string Email { get; set; }
        public string Web { get; set; }
        public string LogoUrl { get; set; }
        public bool IsActive { get; set; }
        public string TRN { get; set; }
        public string Remarks { get; set; }
        public string OwnerRepresentaive { get; set; }
        public bool IsCashCompany { get; set; }
        public string CurrentStatus { get; set; }
        public int UserId { get; set; }
    }
}

