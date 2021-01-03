using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class TempTransactionVm
    {
        public double Amount { get; set; }
        public string ExpenseAccountNumber { get; set; }
        public string ExpenseCostNumber { get; set; }
        public string PrepaidExpenseAccountNumber { get; set; }
        public string PrepaidExpenseCostNumber { get; set; }
        public string Note { get; set; }
        public string RevenueAccountNumber { get; set; }
        public string RevenueReceivedAccountNumber { get; set; }
        public string RevenueCostNumber { get; set; }
        public string RevenueReceivedCostNumber { get; set; }
    }
}