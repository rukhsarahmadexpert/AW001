using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class UpdateReasonDescriptionViewModel
    {
        public int Id { get; set; }
        public string ReasonDescription { get; set; }
        public string Flag { get; set; }
        public int CreatedBy { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
