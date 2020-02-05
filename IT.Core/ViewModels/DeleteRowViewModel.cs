using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class DeleteRowViewModel
    {
        public int Id { get; set; }
        public int detailId { get; set; }
        public int VAT { get; set; }
        public decimal RowTotal { get; set; }
    }
}
