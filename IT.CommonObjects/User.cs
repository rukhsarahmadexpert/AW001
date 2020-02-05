namespace IT.CommonObjects
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string Fname { get; set; }

        [StringLength(50)]
        public string Insertion { get; set; }

        [StringLength(50)]
        public string Lname { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        [StringLength(200)]
        public string ImageUrl { get; set; }

        [StringLength(10)]
        public string Gender { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DOB { get; set; }

        public int? CreatedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CreatdDate { get; set; }

        public int? UpdatedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? UpdatedDate { get; set; }

        public int? RoleId { get; set; }

        public int? CompanyId { get; set; }

        public bool? IsActive { get; set; }
    }
}
