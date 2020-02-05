using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class CustomerOrderStatistics
    {
        public int TotolRequestedQuantity { get; set; }
        public int TotalDrivers { get; set; }
        public int TotalVehicles { get; set; }
        public int TotalDeliverdQuantity { get; set; }

        public List<CustomerOrderDateViewModel> RequestedBySevenDayed { get; set; }
        public List<CustomerOrderDateViewModel> DeliverdBySevenDayed { get; set; }
        public List<CustomerNotification> customerNotification { get; set; }
    }
}
