using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class StorageViewModel
    {
        public int Id { get; set; }
        public double StockIn { get; set; }
        public double StockOut { get; set; }
        public int VehicleId { get; set; }
        public int SiteId { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int ClientVehicleId { get; set; }
        public int LPOId { get; set; }
        public string Source { get; set; }
        public string UserName { get; set; }
        public string SiteName { get; set; }
        public string Decription { get; set; }
        public string TrafficPlateNumber { get; set; }
        public string PONumber { get; set; }
        public bool Action { get; set; }
        public int ProductId { get; set; }
        public string uniques { get; set; }
        public int UpdateBy { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string ToSource { get; set; }
    }
}

