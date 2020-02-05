using System;
namespace IT.Core.ViewModels
{
    public class OrderTransferRequestsViewModel
    {
        public int Id { get; set; }
        public int DriverId { get; set; }
        public string DriverName { get; set; }
        public int RequestType { get; set; }
        public string RequestTypeName { get; set; }
        public string Description { get; set; }
        public Boolean IsAccepted { get; set; }
        public int AcceptBy { get; set; }
        public Boolean IsOpen { get; set; }
        public string CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Boolean IsActive { get; set; }
        public int OrderId { get; set; }
        public int CompanyId { get; set; }
        public int PartialQuantity { get; set; }
        public string IsFullOrPartial { get; set; }
        public string Email { get; set; }
        public int TransferdQuantity { get; set; }
        public int OrderTransferRequestId { get; set; }
       
    }
}
