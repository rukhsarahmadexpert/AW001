using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Web.Models
{
    public class BookingConfirmationViewModel
    {
        public int Id { get; set; }
        public DateTime DueDate { get; set; }
        public string CompanyName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
