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

        public int _pageSize { get; set; } = 10;

        public int CompanyId { get; set; }
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

        public static implicit operator PagingParameterModel(List<PagingParameterModel> v)
        {
            throw new NotImplementedException();
        }

        public void SetPageSize(int v)
        {
            throw new NotImplementedException();
        }
    }
}
