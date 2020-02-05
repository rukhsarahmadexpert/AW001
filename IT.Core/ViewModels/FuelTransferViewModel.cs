using System;
namespace IT.Core.ViewModels
{
    public class FuelTransferViewModel
    {
        public int Id { get; set; }
        public int SiteId { get; set; }
        public int VehicleId { get; set; }
        public int Quantity { get; set; }
        public string FuelTransferDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string Reason { get; set; }
        public string UserName { get; set; }
        public Boolean IsActive { get; set; }
        public string SiteName { get; set; }
        public string TraficPlateNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
