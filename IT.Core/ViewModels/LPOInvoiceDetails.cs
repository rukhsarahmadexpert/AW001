using System;
namespace IT.Core.ViewModels
{
    public class LPOInvoiceDetails
    {
        public int Id { get; set; }

        public int LPOId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string UnitName { get; set; }
        public int UnitId { get; set; }
        public string Description { get; set; }
        public Decimal UnitPrice { get; set; }
        public int Qunatity { get; set; }
        public Decimal Total { get; set; }
        public Decimal VAT { get; set; }
        public Decimal SubTotal { get; set; }

    }
}
