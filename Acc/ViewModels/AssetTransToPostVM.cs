using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class AssetTransToPostVM
    {
        public double Debit { get; set; }
        public double Credit { get; set; }

        public string AccountNo { get; set; }

        public string AccountName { get; set; }

        public string Note { get; set; }

        public string CostCenterNo{ get; set; }
        public string CostCenterName { get; set; }

        public double Value { get; set; }

    }
}