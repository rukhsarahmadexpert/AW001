using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
  public class FuelPricesViewModel
    {
        public int Id { get; set; }
        public decimal Price  { get; set; }
        public int Unit { get; set; }
        public string UnitName { get; set; }
        public string Month  { get; set; }
        public int CreatedBy  { get; set; }
        public string CreatedDate  { get; set; }
        public int UpdatedBy  { get; set; }
        public string UserName  { get; set; }
        public bool IsActive  { get; set; }
    }
}
