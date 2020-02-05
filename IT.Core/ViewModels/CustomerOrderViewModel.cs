using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class CustomerOrderViewModel
    {
        public int Id { get; set; }
        public int OrderQuantity { get; set; }
        public int VehicleId { get; set; }
        public int DriverId { get; set; }
        public int CompanyId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public string RequestThrough { get; set; }
        public int MyProperty { get; set; }
        public bool IsRead { get; set; }
        public bool IsActive { get; set; }
        public bool Status { get; set; }
        public string DriverName { get; set; }
        public string VehicleNumber { get; set; }
        public string UserName { get; set; }
        public string CreateDates { get; set; }
        public string Comments { get; set; }
        public string CreatedTime { get; set; }
        public string OrderProgress { get; set; }
        public string CompanyName { get; set; }
        public string AcceptDate { get; set; }
        public string OrderDate { get; set; }
        public string Name { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UPrice { get; set; }

    }
}
