using System;
namespace IT.Core.ViewModels
{
    public class SiteViewModel
    {
        public int Id { get; set; }
        public string SiteName { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPhone { get; set; }
        public string SiteCell { get; set; }
        public string FaceBook { get; set; }
        public string SiteEmail { get; set; }
        public string Address { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdateBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Boolean IsActive { get; set; }
        public string UserName { get; set; }
        public string CreatedDates { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string PickingPoint { get; set; }
        public string locationFullUrl { get; set; }
        public int CompanyId { get; set; }
    }
}
