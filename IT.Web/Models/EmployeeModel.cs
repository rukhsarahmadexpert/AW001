using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IT.Web.Models
{
    public class EmployeeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Nationality { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public string Nation { get; set; }
        public string DesignationName { get; set; }
        public string Facebook { get; set; }
        public string Comments { get; set; }
        public string UserName { get; set; }
        public decimal BasicSalary { get; set; }
    }
}