using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class UserViewModel
    {
        public int UserId { get; set; }        
        public string FullName { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [CompareAttribute("Password", ErrorMessage = "Password and ConfirmPasswrod doesn't match.")]
        public string ConfirmPassword { get; set; }
        public string ImageUrl { get; set; }
        public string Gender { get; set; }
        public DateTime? DOB { get; set; }
        public int? CreatedBy { get; set; }
        public int? CompanyId { get; set; }
        public bool? IsActive { get; set; }
        public int AuthorityId { get; set; }
    }
}
