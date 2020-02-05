using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public int Unit { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UnitName { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public string UserName { get; set; }
        public decimal UPrice { get; set; }
    }
}
