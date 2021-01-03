using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class St_ItemTransactionReportWithTotalVM
    {
        public double OpeningBalance { get; set; }
        public double StartOfPeriodBalance { get; set; }
        public double EndOfPeriodBalance { get; set; }
        public double LastAverageCost { get; set; }
        public double LastSumTotalCostLocal { get; set; }
        public double PreviousSumTotalCostLocal { get; set; }
        public double PreviousAverageCost { get; set; }
        public double PreviousSumQuantityInputOutput { get; set; }

        public IEnumerable<St_ItemTransactionReportVM> St_ItemTransactionReportVM { get; set; }
    }
}