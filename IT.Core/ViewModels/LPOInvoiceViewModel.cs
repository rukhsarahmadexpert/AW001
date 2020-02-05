using System;
using System.Collections.Generic;

namespace IT.Core.ViewModels
{
    public class LPOInvoiceViewModel
    {
        public int Id { get; set; }
        public string VenderId { get; set; }
        public string Name { get; set; }
        public Decimal Total { get; set; }
        public Decimal VAT { get; set; }
        public int LPOId { get; set; }
        public Decimal GrandTotal { get; set; }
        public string TermCondition { get; set; }
        public string CustomerNote { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime DueDate { get; set; }
        public Decimal Balance { get; set; }
        public string PONumber { get; set; }
        public string RefrenceNumber { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
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
        public string IsDownload { get; set; }
        public string SendByEmail { get; set; }
        public string ReasonUpdated { get; set; }
        public Boolean IsUpdated { get; set; }
        public string Heading { get; set; }
        public int CustomerId { get; set; }
        public Decimal ReceivedAmount { get; set; }
        public string BillNumber { get; set; }
        public int UpdatedBy { get; set; }
        public List<LPOInvoiceDetails> lPOInvoiceDetailsList { get; set; }
    }
}
