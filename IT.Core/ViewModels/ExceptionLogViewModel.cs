using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class ExceptionLogViewModel
    {
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string ExceptionType { get; set; }
        public string ExceptionDescription { get; set; }
        public int CompanyId { get; set; }
        public int UserId { get; set; }
        public DateTime ExceptionDatetime { get; set; }
    }
}
