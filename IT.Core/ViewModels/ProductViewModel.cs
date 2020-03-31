using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please select unit")]
        public int Unit { get; set; }
        [Required(ErrorMessage = "Please enter product Name")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string UnitName { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please enter unit price")]
        public decimal UPrice { get; set; }
    }
}
