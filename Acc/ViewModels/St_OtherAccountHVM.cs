using Acc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class St_OtherAccountHVM
    {
        public int Id { get; set; }
        public int CompanyID { get; set; }
        [Display(Name = "SalesTaxAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string SalesTaxAccountNumber { get; set; }
        [Display(Name = "PurchaseTaxAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string PurchaseTaxAccountNumber { get; set; }
        [Display(Name = "PurchaseAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string PurchaseAccountNumber { get; set; }
        [Display(Name = "ForeignPurchaseAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string ForeignPurchaseAccountNumber { get; set; }
        [Display(Name = "DirectAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string DirectAccountNumber { get; set; }
        [Display(Name = "DifferenceAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string DifferenceAccountNumber { get; set; }
        [Display(Name = "BillsOfExchangeAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string BillsOfExchangeAccountNumber { get; set; }
        public string SalesTaxAccountName { get; set; }
        public string PurchaseTaxAccountName { get; set; }
        public string PurchaseAccountName { get; set; }
        public string ForeignPurchaseAccountName { get; set; }
        public string DirectAccountName { get; set; }
        public string DifferenceAccountName { get; set; }
        public string BillsOfExchangeAccountName { get; set; }
        
        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]
        public string InsUserID { get; set; }
        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime InsDateTime { get; set; }
    }
}