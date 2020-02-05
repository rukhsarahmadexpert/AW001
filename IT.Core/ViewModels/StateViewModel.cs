using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class StateViewModel
    {
        public int Id { get; set; }
        public string States { get; set; }
        public int CountryId { get; set; }
        public bool IsActive { get; set; }
        public List<CityViewModel> cityViewModels { get; set; }
    }
}
