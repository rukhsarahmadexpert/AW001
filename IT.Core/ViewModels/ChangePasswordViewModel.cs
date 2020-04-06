using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class ChangePasswordViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please provide your emails")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please provide your current Password")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please provide your New Password")]
        public string NewPassword { get; set; }
    }
}
