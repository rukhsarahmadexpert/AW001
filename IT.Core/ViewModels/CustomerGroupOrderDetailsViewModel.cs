using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class CustomerGroupOrderDetailsViewModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int VehicleId { get; set; }
        public string TraficPlateNumber { get; set; }
        public int DriverId { get; set; }
        public string Name { get; set; }
        public int RequestedQuantity { get; set; }
        public int DeliverdQuantity { get; set; }
        public string Comments { get; set; }
        public string Color { get; set; }
        public int OdrderDetailsId { get; set; }
        public bool IsAsigned { get; set; }
        public string CurrentStatus { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
