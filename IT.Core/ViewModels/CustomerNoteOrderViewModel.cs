using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class CustomerNoteOrderViewModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int DriverId { get; set; }
        public int VehicleId { get; set; }
        public string Company { get; set; }
        public int OrderQuantity { get; set; }
        public int DeliverdQuantity { get; set; }
        public string DriverName { get; set; }
        public string TraficPlateNumber { get; set; }
        public string Cell { get; set; }
        public string Contact { get; set; }
        public string LogoURL { get; set; }
        public string statment { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public bool IsRead { get; set; }
        public string Note { get; set; }
        public string LocationLink { get; set; }
        public int CreatedBy { get; set; }
        public string UserName { get; set; }
        public int CompanyId { get; set; }
        public string CustomerOrderId { get; set; }
        public int RequestedQuantity { get; set; }
        public string Name { get; set; }
        public string TRN { get; set; }
        public string OrderProgress { get; set; }
        public string CreateDates { get; set; }
        public bool IsSend { get; set; }
        public string CustomerOrderNumber { get; set; }
        public string DeliveryNoteNumber { get; set; }
    }
}
