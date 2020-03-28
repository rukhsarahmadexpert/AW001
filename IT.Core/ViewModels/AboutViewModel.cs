using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class AboutViewModel
    {
        public int Id { get; set; }
        public string Heading1 { get; set; }
        public string BackGroundImageUrl { get; set; }
        public string SmallImage { get; set; }
        public string Heading2 { get; set; }
        public string Details1 { get; set; }
        public string Heading3 { get; set; }
        public string Details2 { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public string VisionHeading { get; set; }
        public string VisionDetails { get; set; }
    }
}
