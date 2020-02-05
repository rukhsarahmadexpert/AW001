using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class CustomerOrderDeliverVewModel
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int RowQuantity { get; set; }
        public int OrderAsignId { get; set; }
        public string KiloMeter { get; set; }
        public string Note { get; set; }
    }
}
