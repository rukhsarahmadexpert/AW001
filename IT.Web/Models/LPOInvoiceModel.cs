using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IT.Web.Models
{
    public class LPOInvoiceModel
    {
        public int Id { get; set; }
        public int VenderId { get; set; }
        public decimal Total { get; set; }
        public decimal VAT { get; set; }

        public decimal GrandTotal { get; set; }
        public string TermCondition { get; set; }
        public string CustomerNote { get; set; }

        public string PONumber { get; set; }
        public string RefrenceNumber { get; set; }
        public int CreatedBy { get; set; }
        public string Name { get; set; }
        public string LandLine { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string TRN { get; set; }
        public string FDate { get; set; }
        public string DDate { get; set; }
        public string CreatedDates { get; set; }
        public string UserName { get; set; }
        public string Representative { get; set; }
        public int detailId { get; set; }

        public List<IT.Web.Models.LPOInvoiceDetailsModel> lPOInvoiceDetailsList { get; set; } 
        public List<IT.Web.Models.CompnayModel> compnays { get; set; }
        public List<IT.Web.Models.VenderModel> venders { get; set; }
    }
}