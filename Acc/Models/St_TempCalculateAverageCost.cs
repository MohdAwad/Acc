using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class St_TempCalculateAverageCost
    {
        public int Id { get; set; }
        public int TransactionKindNo { get; set; }
        public string ItemCode { get; set; }
        public string VoucherNumber { get; set; }
        public DateTime VoucherDate { get; set; }
        public string StockCode { get; set; }
        public double TotalCostLocal { get; set; }
        public double QuantityInputOutput { get; set; }
        public double SumTotalCostLocal { get; set; }
        public double SumQuantityInputOutput { get; set; }
        public double CostPieceLocal { get; set; }
        public double AverageCost { get; set; }
        public string InsUserID { get; set; }
        public DateTime InsDateTime { get; set; }
        public double PricePieceLocalAfterDiscount { get; set; }
        public double TotalLocalAfterDiscount { get; set; }
        public string TransactionKindName { get; set; }
        public string StockName { get; set; }
        public string Remark { get; set; }
        public string Hint { get; set; }
        public double QuantityIn { get; set; }
        public double QuantityOut { get; set; }
        public string AccountName { get; set; }
        public string FromStockName { get; set; }
        public string ToStockName { get; set; }
    }
}