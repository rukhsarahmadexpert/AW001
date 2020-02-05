using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IT.Web.Models
{
    public class CustomerOrder
    {
        public int OrderId { get; set; }
        public int OrderQuantity { get; set; }
        public int DriverId { get; set; }
        public int VehicleId { get; set; }
        public int CompanyId { get; set; }
        public string RequestThrough { get; set; }
        public DateTime CreatedDate { get; set; }
        public TimeSpan CreatedTime { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsRead { get; set; }
        public string OrderProgress { get; set; }
    }
}