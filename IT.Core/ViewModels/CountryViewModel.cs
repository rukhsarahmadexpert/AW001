using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class CountryViewModel
    {
        public int Id { get; set; }
        public string CountryName { get; set; }
        public List<StateViewModel> stateViewModels { get; set; }
    }
}
