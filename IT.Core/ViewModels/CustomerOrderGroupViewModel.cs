using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class CustomerOrderGroupViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CustomerOrderId { get; set; }
        public int RequestedQuantity { get; set; }
        public int DeliverdQuantity { get; set; }
        public string OrderProgress { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsRead { get; set; }
        public bool IsActive { get; set; }
        public string CustomerNote { get; set; }
        public bool IsCustomerViewNotification { get; set; }
        public bool IsTrue { get; set; }
        public int CreatedBy { get; set; }
        public bool IsDeliverd { get; set; }
        public string CreatedDates { get; set; }
        public bool IsSend { get; set; }
        public int OrderId { get; set; }
        public string Contact { get; set; }
        public string TraficPlateNumber { get; set; }
        public string VehicleNumber { get; set; }
        public int CompanyId { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string longitude { get; set; }
        public string latitude { get; set; }
        public string locationFullUrl { get; set; }
        public string pickingPoint { get; set; }
        public string Description { get; set; }

        public List<CustomerGroupOrderDetailsViewModel> customerGroupOrderDetailsViewModels { get; set; }
    }
}
