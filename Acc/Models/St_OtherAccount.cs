using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class St_OtherAccount
    {
        public int Id { get; set; }
        public int CompanyID { get; set; }
        [Display(Name = "SalesTaxAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string SalesTaxAccountNumber { get; set; }
        [Display(Name = "SalesTaxReturnAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string SalesTaxReturnAccountNumber { get; set; }
        [Display(Name = "PurchaseTaxAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string PurchaseTaxAccountNumber { get; set; }
        [Display(Name = "PurchaseTaxReturnAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string PurchaseTaxReturnAccountNumber { get; set; }
        [Display(Name = "ForeignPurchaseAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string ForeignPurchaseAccountNumber { get; set; }
        [Display(Name = "DirectAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string DirectAccountNumber { get; set; }
        [Display(Name = "DifferenceAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string DifferenceAccountNumber { get; set; }
        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]
        public string InsUserID { get; set; }
        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime InsDateTime { get; set; }
    }
}