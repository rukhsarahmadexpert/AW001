using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class CustomerOrderSiteAssignedViewModel
    {
        public int OrderId { get; set; }
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerOrderId { get; set; }
        public int RequestedQuantity { get; set; }
        public int DeliverdQuantity { get; set; }
        public string OrderProgress { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedDates { get; set; }
        public string CustomerNote { get; set; }
        public bool IsDeliverd { get; set; }
        public int CompanyId { get; set; }
        public string LogoURL { get; set; }
        public string serachKey { get; set; }
        public string SearchFlage { get; set; }
        public string longitude { get; set; }
        public string latitude { get; set; }
        public string locationFullUrl { get; set; }
        public string pickingPoint { get; set; }
        public string Description { get; set; }
        public string SIteName { get; set; }
    }
}
