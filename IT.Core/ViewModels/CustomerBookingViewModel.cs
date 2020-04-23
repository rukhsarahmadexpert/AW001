using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class CustomerBookingViewModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        [Required(ErrorMessage = "Please add Quantity")]
        [Range(1, int.MaxValue, ErrorMessage = "Please add Quantity")]
        public int BookQuantity { get; set; }
        [Required(ErrorMessage = "Please Add Price")]
        [Range(1, int.MaxValue, ErrorMessage = "Please add Price")]
        public decimal UnitPrice { get; set; }
        public bool IsAccepted { get; set; }
        public decimal VAT { get; set; }
        public decimal TotalAmount { get; set; }
        public string Description { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedDates { get; set; }
        public DateTime DueDate { get; set; }
        public int UpdateBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsOpen { get; set; }
        [Required(ErrorMessage = "Please select Product")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select Product")]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Please Product Unit")]
        [Range(1, int.MaxValue, ErrorMessage = "Please Product Unit")]
        public int UnitId { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }
        public string UserName { get; set; }
        public string CompanyName { get; set; }
        public string bookingUpdateReason { get; set; }
        public string UpdateReason { get; set; }
        public string RejectedReason { get; set; }
        public string IsAwfuelAdmin { get; set; }
        public string ReasonDescription { get; set; }
        public List<UploadDocumentsViewModel> uploadDocumentsViewModels { get; set; }
        public UpdateReasonDescriptionViewModel UpdateReasonDescriptionViewModel { get; set; }
        public List<UpdateReasonDescriptionViewModel> updateReasonDescriptionViewModels { get; set; }

    }
}
