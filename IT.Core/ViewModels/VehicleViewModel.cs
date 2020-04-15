using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace IT.Core.ViewModels
{
    public class VehicleViewModel
    {
        public int Id { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please select Vehicle Type")]
        public int VehicleType { get; set; }
        public string VehicleTypeName { get; set; }
        [Required(ErrorMessage = "Please enter plate number")]
        public string TraficPlateNumber { get; set; }
        public string PlaceofIssue { get; set; }
        public string TCNumber { get; set; }
        public string Model { get; set; }
        [Required(ErrorMessage = "Please select color")]
        public string Color { get; set; }
        public string MulkiaExpiry { get; set; }
        public string InsuranceExpiry { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy MMM dd}", ApplyFormatInEditMode = true)]
        public DateTime MulkiaExpiryDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy MMM dd}", ApplyFormatInEditMode = true)]
        public DateTime InsuranceExpiryDate { get; set; }
        public string RegisteredRegion { get; set; }
        public string Brand { get; set; }
        public string MulkiaFront1 { get; set; }
        public string MulkiaBack1 { get; set; }
        public string MulkiaFront2 { get; set; }
        public string MulkiaBack2 { get; set; }
        public string Comments { get; set; }
        public int CompanyId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdateBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UID { get; set; }
        public string Oprater { get; set; }
        public bool IsActive { get; set; }
        public string OrderProgress { get; set; }

        public HttpPostedFileBase MulkiaFront1File { get; set; }
        public HttpPostedFileBase MulkiaBack1File { get; set; }
        public HttpPostedFileBase MulkiaFront2File { get; set; }
        public HttpPostedFileBase MulkiaBack2File { get; set; }

        public List<UploadDocumentsViewModel> uploadDocumentsViewModels { get; set; }
    }
}
