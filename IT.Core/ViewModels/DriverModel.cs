using System;
using System.Collections.Generic;

namespace IT.Core.ViewModels
{
    public class DriverModel
    {
        public int DriverId { get; set; }
        public int Id { get; set; }
        public int createdBy { get; set; }
        public string Name { get; set; }
        public string DriverName { get; set; }
        public string ContactNumber { get; set; }
        public string TraficPlateNumber { get; set; }       
        public int VehicleId { get; set; }

        public static implicit operator DriverModel(List<DriverModel> v)
        {
            throw new NotImplementedException();
        }
    }
}

