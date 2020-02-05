using System;
namespace IT.Core.ViewModels
{
    public class CustomerNotificationViewModel
    {
        public int  Id { get; set; }
        public string ImageUrl { get; set; }
        public string MessageTitle { get; set; }
        public string MessageDescription { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public string ClientDocs { get; set; }
    }
}
