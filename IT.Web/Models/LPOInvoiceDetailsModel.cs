using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IT.Web.Models
{
    public class LPOInvoiceDetailsModel
    {
        public int Id { get; set; }
        public int LPOId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string UnitName { get; set; }
        public int UnitId { get; set; }

        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public int Qunatity { get; set; }
        public decimal Total { get; set; }

        public decimal VAT { get; set; }
        public decimal SubTotal { get; set; }
    }
}