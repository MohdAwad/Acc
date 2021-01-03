using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class St_AvaregCostHVM
    {
        public string ItemCode { get; set; }
        public string VoucherNumber { get; set; }
        public DateTime VoucherDate { get; set; }
        public int TransactionKindNo { get; set; }
        public string StockCode { get; set; }
        public string BranchCode { get; set; }
        public double TotalCostLocal { get; set; }
        public double QuantityInputOutput { get; set; }
        public double SumTotalCostLocal { get; set; }
        public double SumQuantityInputOutput { get; set; }
        public double CostPieceLocal { get; set; }
        public double AverageCost { get; set; }
        public string InsUserID { get; set; }
        public DateTime InsDateTime { get; set; }
    }
}