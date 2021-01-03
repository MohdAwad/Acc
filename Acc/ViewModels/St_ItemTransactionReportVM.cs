using Acc.Models;
using DevExpress.XtraWaitForm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class St_ItemTransactionReportVM
    {
        public IEnumerable<St_Warehouse> St_Warehouse { get; set; }
        public string StockCode { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public Boolean ApprovingPreviousBalance { get; set; }
        public Boolean ShowItemCost { get; set; }
        public int TheDecimalPointForTheLocalCurrency { get; set; }
        public double OpeningBalance { get; set; }
        public double StartOfPeriodBalance { get; set; }
        public double EndOfPeriodBalance { get; set; }
        public double AverageCost { get; set; }
        public string VoucherNumber { get; set; }
        public DateTime VoucherDate { get; set; }
        public string TransactionKindName { get; set; }
        public string StockName { get; set; }
        public double PricePieceLocalAfterDiscount { get; set; }
        public double QuantityIn { get; set; }
        public double QuantityOut { get; set; }
        public double QuantityBalance { get; set; }
        public double TotalLocalAfterDiscount { get; set; }
        public double RemainingCostBalance { get; set; }
        public string AccountName { get; set; }
        public string FromStockName { get; set; }
        public string ToStockName { get; set; }
        public string FromStockCode { get; set; }
        public string ToStockCode { get; set; }
        public string Remark { get; set; }
        public int TransactionKindNo { get; set; }
        public double TotalCostLocal { get; set; }
        public double QuantityInputOutput { get; set; }
        public double SumTotalCostLocal { get; set; }
        public double SumQuantityInputOutput { get; set; }
        public double CostPieceLocal { get; set; }
        public string InsUserID { get; set; }
        public DateTime InsDateTime { get; set; }
        public double LastAverageCost { get; set; }
        public double LastSumTotalCostLocal { get; set; }
    }
}