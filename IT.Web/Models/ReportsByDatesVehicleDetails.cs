using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IT.Web.Models
{
    public class ReportsByDatesVehicleDetails
    {
        public int Id { get; set; }
        public string CustomerOrderNumber { get; set; }
        public string DeliveryNoteNumber { get; set; }
        public int RequestedQuantity { get; set; }
        public int DeliverdQuantity { get; set; }
        public string CurrentStatus { get; set; }
        public string OrderDates { get; set; }
        public int VehicleId { get; set; }
        public string TraficPlateNumber { get; set; }
        public int CustomerId { get; set; }
        public string CompanyName { get; set; }
        public string ProductName { get; set; }
    }
}