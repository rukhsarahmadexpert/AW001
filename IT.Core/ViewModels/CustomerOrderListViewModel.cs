using System;
using System.Collections.Generic;

namespace IT.Core.ViewModels
{
    public class CustomerOrderListViewModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int RequestedQuantity { get; set; }
        public int DeliverdQuantity { get; set; }
        public string OrderProgress { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CustomerNote { get; set; }
        public int CustomerOrderId { get; set; }
        public int DriverId { get; set; }
        public int VehicleId { get; set; }
        public string DeliveryNoteNumber { get; set; }
        public string CustomerOrderNumber { get; set; }
        public string Note { get; set; }
        public string RequestThrough { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public string NotificationCode { get; set; }
        public string email { get; set; }
        public string longitude { get; set; }
        public string latitude { get; set; }
        public string LocationFullUrl { get; set; }
        public string PickingPoint { get; set; }
        public int SiteId { get; set; }
        public string DriverName { get; set; }
        public string ContactNumber { get; set; }
        public string UserName { get; set; }
        public string CompanyName { get; set; }
        public List<CustomerOrderViewModel> customerOrderViewModels { get; set; }
    

    }
}
