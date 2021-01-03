using Acc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class TempPrepaidAndRevenueReceivedVM
    {
        public TempPrepaidAndRevenueReceived TempPrepaidAndRevenueReceived { get; set; }
        [Display(Name = "TransactionKind", ResourceType = typeof(Resources.Resource))]
        public int CompanyTransactionKindID { get; set; }
        public IEnumerable<CompanyTransactionKind> CompanyTransactionKind { get; set; }
        public IEnumerable<TempPrepaidAndRevenueReceivedDetail> TempPrepaidAndRevenueReceivedDetail { get; set; }
        [Display(Name = "ExpenseAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string ExpenseAccountNumber { get; set; }
        public string ExpenseAccountName { get; set; }
        [Display(Name = "PrepaidExpenseAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string PrepaidExpenseAccountNumber { get; set; }
        public string PrepaidExpenseAccountName { get; set; }
        [Display(Name = "ExpenseCostNumber", ResourceType = typeof(Resources.Resource))]
        public string ExpenseCostNumber { get; set; }
        public string ExpenseCostName { get; set; }
        [Display(Name = "PrepaidExpenseCostNumber", ResourceType = typeof(Resources.Resource))]
        public string PrepaidExpenseCostNumber { get; set; }
        public string PrepaidExpenseCostName { get; set; }
        public int CompanyYear { get; set; }
        public int iRowTable { get; set; }
        public bool WorkWithCostCenter { get; set; }
        public double Amount { get; set; }
        public DateTime CollectionDate { get; set; }
        public string Note { get; set; }
        public string Exported { get; set; }
        public int CompanyID { get; set; }
        public int RowNumber { get; set; }
        public int CompanyTransactionKindNo { get; set; }

        public int TransactionKindNo { get; set; }

        public string VoucherNumber { get; set; }
        [Display(Name = "RevenueAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string RevenueAccountNumber { get; set; }
        public string RevenueAccountName { get; set; }
        [Display(Name = "RevenueReceivedAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string RevenueReceivedAccountNumber { get; set; }
        public string RevenueReceivedAccountName { get; set; }
        [Display(Name = "RevenueCostNumber", ResourceType = typeof(Resources.Resource))]
        public string RevenueCostNumber { get; set; }
        public string RevenueCostName { get; set; }
        [Display(Name = "RevenueReceivedCostNumber", ResourceType = typeof(Resources.Resource))]
        public string RevenueReceivedCostNumber { get; set; }
        public string RevenueReceivedCostName { get; set; }

        [Display(Name = "FromCollectionDate", ResourceType = typeof(Resources.Resource))]
        public DateTime FromCollectionDate { get; set; }
        [Display(Name = "ToDate", ResourceType = typeof(Resources.Resource))]
        public DateTime ToCollectionDate { get; set; }
        public Boolean Collector { get; set; }
        public Boolean NotCollected { get; set; }
        public Boolean All { get; set; }
        public int Count { get; set; }
        public string SumAmount { get; set; }
        public DateTime VoucherDate { get; set; }
        public string UserName { get; set; }
        public int TheDecimalPointForTheLocalCurrency { get; set; }
        public int TheDecimalPointForTheForeignCurrency { get; set; }


    }
}