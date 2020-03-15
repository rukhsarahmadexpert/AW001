using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IT.Core.ViewModels
{
    public class UploadDocumentsViewModel
    {
        public int Id { get; set; }
        public string FileUrl { get; set; }
        public int InvoiceId { get; set; }
        public int QuotationId { get; set; }
        public int BillId { get; set; }
        public int DriverId { get; set; }
        public int LPOId { get; set; }
        public int EmployeeId { get; set; }
        public int CompanyId { get; set; }
        public int BookingId { get; set; }
        public string FilesName { get; set; }
        public int OrderId { get; set; }
        public int VehicleId { get; set; }
        public int StorageId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsActive { get; set; }

        public HttpPostedFileBase FileUrlFile { get; set; }
    }
}
