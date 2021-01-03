using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class ResttingAccountVM
    {
        public string AccountNumber { get; set; }
        public string ArabicName { get; set; }

        public double Total { get; set; }

        public double    Debit { get; set; }
        public double Credit { get; set; }

    }
}