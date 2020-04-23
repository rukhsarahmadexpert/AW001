using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class PagingParameterModel
    {
        const int maxPageSize = 50;
        public int requestedQuantity;
        public int Id { get; set; }
        public int pageNumber { get; set; } = 1;

        public int _pageSize { get; set; } = 50;

        public int CompanyId { get; set; }
        public string SerachKey { get; set; }
        public string OrderProgress { get; set; }
        public bool IsSend { get; set; }
        public int DriverId { get; set; }

        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }

    }
}
