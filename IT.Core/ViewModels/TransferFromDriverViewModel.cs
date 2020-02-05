using System;
namespace IT.Core.ViewModels
{
    public class TransferFromDriverViewModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int Qunantity { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public int CreatedBy { get; set; }
        public int OrderTransferRequestId { get; set; }
        public string CreatedDate { get; set; }
        public int CompanyId { get; set; }
        public bool IsTransferd { get; set; }
        public bool IsFullOrder { get; set; }
        public string Descriptions { get; set; }
        public string EmailFrom { get; set; }
        public string EmailTo { get; set; }
        public string FromDriver { get; set; }
        public string ToDriver { get; set; }
        public string TransferdBy { get; set; }

    }
}
