using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class DashBoardVM
    {
        public int ActiveYear { get; set; }

        public string CompanyName { get; set; }
        public bool MyProperty { get; set; }
        public bool WorkWithCostCenter { get; set; }
        public int TheDecimalPointForTheLocalCurrency { get; set; }
        public int TheDecimalPointForTheForeignCurrency { get; set; }
    }
}