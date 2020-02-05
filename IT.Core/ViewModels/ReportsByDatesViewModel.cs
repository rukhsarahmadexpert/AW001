using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class ReportsByDatesViewModel
    {
        public int Id { get; set; }
        public string OrderDates { get; set; }
        public int VehicleId { get; set; }
        public string TraficPlateNumber { get; set; }

        public List<ReportsByDatesVehicleDetails> reportsByDatesVehicleDetails { get; set; }
    }
}
