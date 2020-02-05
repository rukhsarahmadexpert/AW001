using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class CompanyViewModel
    {
        public int Id { get; set; }
        [Required()]
        public string Name { get; set; }
        public string Street { get; set; }
        public string Postcode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Cell { get; set; }
        public string Email { get; set; }
        public string Web { get; set; }
        public string Commentes { get; set; }
        public string FindSource { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string LogoUrl { get; set; }
        public bool IsActive { get; set; }
        public string TradeLicense { get; set; }
        public string VATCertificate { get; set; }
        public string PassportFirstPage { get; set; }
        public string PassportLastPage { get; set; }
        public string IDCardUAE { get; set; }
        public string UID { get; set; }
        [MaxLength(25), MinLength(25), Required]
        public string TRN { get; set; }
        public string OwnerRepresentaive { get; set; }
        public string Flag { get; set; }
        public string CurrentStatus { get; set; }
        public string Token { get; set; }
        public string DeviceId { get; set; }
        public string Authority { get; set; }
        public string Device { get; set; }
        public string UserName { get; set; }
        public bool IsCashCompany { get; set; }
    }
}
