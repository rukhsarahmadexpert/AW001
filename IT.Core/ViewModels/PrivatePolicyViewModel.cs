using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class PrivatePolicyViewModel
    {
        public int Id { get; set; }
        public string EffectiveDates { get; set; }
        public List<PrivacyPolicyDetailsViewModel> privacyPolicyDetailsViewModels { get; set; }
    }
}
