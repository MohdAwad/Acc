using Acc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace Acc.ViewModels
{
    public class TempPrepaidAndRevenueReceivedFilterVM
    {
        [Display(Name = "VoucherNumber", ResourceType = typeof(Resources.Resource))]
        public string VoucherNumber { get; set; }
        [Display(Name = "FromDate", ResourceType = typeof(Resources.Resource))]
        public DateTime FromDate { get; set; }

        [Display(Name = "ToDate", ResourceType = typeof(Resources.Resource))]
        public DateTime ToDate { get; set; }
        [Display(Name = "FromDateFirstPayment", ResourceType = typeof(Resources.Resource))]
        public DateTime FromDateFirstPayment { get; set; }
        [Display(Name = "ToDate", ResourceType = typeof(Resources.Resource))]
        public DateTime ToDateFirstPayment { get; set; }
        public string UserName { get; set; }
        public double Total { get; set; }
        public double RemainingAmount { get; set; }
        public int NumberOfPayments { get; set; }
        public int RemainingPayments { get; set; }

        [Display(Name = "VoucherDate", ResourceType = typeof(Resources.Resource))]
        public DateTime VoucherDate { get; set; }
        public DateTime DateFirstPayment { get; set; }
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
        public int iRowTable { get; set; }
        public bool WorkWithCostCenter { get; set; }
        public double Amount { get; set; }
        public DateTime CollectionDate { get; set; }
        public Header Header { get; set; }
        public IEnumerable<TempTransactionVm> Transaction { get; set; }
        public IEnumerable<TempPrepaidAndRevenueReceivedDetail> TempPrepaidAndRevenueReceivedDetail { get; set; }
        public int TransactionKindNo { get; set; }
        public string Note { get; set; }
        public int RowNumber { get; set; }
        public int CompanyYear { get; set; }
        public int CompanyTransactionKindNo { get; set; }
        public int Differences { get; set; }
        [Display(Name = "FromDate", ResourceType = typeof(Resources.Resource))]
        public DateTime FromCollectionDate { get; set; }

        [Display(Name = "ToDate", ResourceType = typeof(Resources.Resource))]
        public DateTime ToCollectionDate { get; set; }
        [Display(Name = "TransactionKind", ResourceType = typeof(Resources.Resource))]
        public int CompanyTransactionKindID { get; set; }
        public IEnumerable<CompanyTransactionKind> CompanyTransactionKind { get; set; }
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
        public int TheDecimalPointForTheLocalCurrency { get; set; }
        public int TheDecimalPointForTheForeignCurrency { get; set; }
        public int CheckCase { get; set; }

    }
}