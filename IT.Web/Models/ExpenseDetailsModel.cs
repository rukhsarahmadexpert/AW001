using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IT.Web.Models
{
    public class ExpenseDetailsModel
    {
        public int Id { get; set; }
        public int ExpenseId { get; set; }
        public string Description { get; set; }
        public int ExpenseType { get; set; }
        public decimal SubTotal { get; set; }
        public decimal VAT { get; set; }
        public decimal NetTotal { get; set; }
        public string ExpDates { get; set; }
        public string Category { get; set; }
        public int ExpenseRefrenceId { get; set; }
        public string TraficPlateNumber { get; set; }
        public string ExpenseName { get; set; }
        public string Name { get; set; }
    }
}