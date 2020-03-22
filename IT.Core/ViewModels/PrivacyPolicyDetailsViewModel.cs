using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class PrivacyPolicyDetailsViewModel
    {
        public int Id { get; set; }
        public int PrivacypolicyId { get; set; }
        public string Heading { get; set; }
        public string Details { get; set; }
    }
}
