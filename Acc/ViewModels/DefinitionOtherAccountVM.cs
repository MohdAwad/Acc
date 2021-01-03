using Acc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class DefinitionOtherAccountVM
    {
        public OtherAccount DefinitionOtherAccount { get; set; }
        public string ChequeFundAccountName { get; set; }
        public string CashFundsAccountName { get; set; }
        public string ExpensesAccountName { get; set; }
        public string PaidExpensesAccountName { get; set; }
        public string RevenuesAccountName { get; set; }
        public string RevenuesReceivedAccountName { get; set; }
        public string SalesTaxAccountName { get; set; }
        public string PurchasesTaxAccountName { get; set; }
        public string ReturnSalesTaxAccountName { get; set; }
        public string ReturnPurchasesTaxAccountName { get; set; }
        public string CustomerAccountName { get; set; }
        public string SupplierAccountName { get; set; }
    }
}