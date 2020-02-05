using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT.Core.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Nationality { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public string Nation { get; set; }
        public int Designation { get; set; }
        public string DesignationName { get; set; }
        public string Facebook { get; set; }
        public string Comments { get; set; }
        public string PassportFront { get; set; }
        public string PassportBack { get; set; }
        public string UID { get; set; }
        public string UserName { get; set; }
        public decimal BasicSalary { get; set; }
        public int CompanyId { get; set; }
        public int UpdatedBy { get; set; }
        public int ProjectId { get; set; }
    }
}
