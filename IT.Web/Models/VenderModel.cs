using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IT.Web.Models
{
    public class VenderModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string CityName { get; set; }
        public string Country { get; set; }
        public string LandLine { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Remarks { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public string UserName { get; set; }
        public string TRN { get; set; }
        public string Representative { get; set; }
        public string Title { get; set; }
    }
}