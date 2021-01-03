using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Acc.ViewModels
{
    public class FreezeTransactionVM
    {
        public string MonthName { get; set; }
        public int MonthID { get; set; }
        public string MonthCase { get; set; }
        public int MonthCaseID { get; set; }
        public int CompanyYear { get; set; }
        public DateTime MinmumDate { get; set; }
        public DateTime MaximumDate { get; set; }

    }
}