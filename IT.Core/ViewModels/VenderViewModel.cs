using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class VenderViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter vender name")]
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string CityName { get; set; }
        public string Country { get; set; }
        public string LandLine { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Remarks { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please enter vender TRN Number")]
        public string TRN { get; set; }
        public string Representative { get; set; }
        public string Title { get; set; }
        public int TotalRows { get; set; }
    }
}
