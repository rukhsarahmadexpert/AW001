using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class CustomerRemainingBookingViewModel
    {
        public int BookingId { get; set; }
        public int BookQuantity { get; set; }
        public int DeliverdQuantity { get; set; }
        public string CompanyName { get; set; }
    }
}
