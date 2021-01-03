using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class OtherAccount
    {
        public int Id { get; set; }
        public int CompanyID { get; set; }
        [Display(Name = "ChequeFunds", ResourceType = typeof(Resources.Resource))]
        public string ChequeFundAccountNumber { get; set; }
        [Display(Name = "Expenses", ResourceType = typeof(Resources.Resource))]
        public string ExpensesAccountNumber { get; set; }
        [Display(Name = "PaidExpenses", ResourceType = typeof(Resources.Resource))]
        public string PaidExpensesAccountNumber { get; set; }
        [Display(Name = "CashFunds", ResourceType = typeof(Resources.Resource))]
        public string CashFundsAccountNumber { get; set; }
        [Display(Name = "Revenues", ResourceType = typeof(Resources.Resource))]
        public string RevenuesAccountNumber { get; set; }
        [Display(Name = "RevenuesReceived", ResourceType = typeof(Resources.Resource))]
        public string RevenuesReceivedAccountNumber { get; set; }
        [Display(Name = "SalesTax", ResourceType = typeof(Resources.Resource))]
        public string SalesTaxAccountNumber { get; set; }
        [Display(Name = "PurchasesTax", ResourceType = typeof(Resources.Resource))]
        public string PurchasesTaxAccountNumber { get; set; }
        [Display(Name = "ReturnSalesTax", ResourceType = typeof(Resources.Resource))]
        public string ReturnSalesTaxAccountNumber { get; set; }
        [Display(Name = "ReturnPurchasesTax", ResourceType = typeof(Resources.Resource))]
        public string ReturnPurchasesTaxAccountNumber { get; set; }
        [Display(Name = "Customers", ResourceType = typeof(Resources.Resource))]
        public string CustomerAccountNumber { get; set; }
        [Display(Name = "Suppliers", ResourceType = typeof(Resources.Resource))]
        public string SupplierAccountNumber { get; set; }

        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]
        public string InsUserID { get; set; }
        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime InsDateTime { get; set; }

      

    }
}