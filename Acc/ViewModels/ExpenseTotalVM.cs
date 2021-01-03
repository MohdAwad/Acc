using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class ExpenseTotalVM
    {
        public int MonthNo { get; set; }
        public double Debit  { get; set; }
        public double Credit { get; set; }
        public double NET { get; set; }
        public int AccountTypeID { get; set;  }
    }
}