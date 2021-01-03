using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class St_WarehouseAccount
    {
        public Company Company { get; set; }
        [Key]
        [Column(Order = 1)]
        public int CompanyID { get; set; }
        [Key]
        [Column(Order = 2)]
        [Display(Name = "StockCode", ResourceType = typeof(Resources.Resource))]
        public string StockCode { get; set; }
        [Display(Name = "SalesFundAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string SalesFundAccountNumber { get; set; }
        
        [Display(Name = "SalesAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string SalesAccountNumber { get; set; }
        [Display(Name = "CostOfGoodsAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string CostOfGoodsAccountNumber { get; set; }
        [Display(Name = "SalesRefundFundAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string SalesRefundFundAccountNumber { get; set; }
        [Display(Name = "SalesReturnAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string SalesReturnAccountNumber { get; set; }
        [Display(Name = "ReturnCostOfGoodsAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string ReturnCostOfGoodsAccountNumber { get; set; }
        [Display(Name = "PurchaseFundAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string PurchaseFundAccountNumber { get; set; }
        [Display(Name = "PurchaseAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string PurchaseAccountNumber { get; set; }
        [Display(Name = "PurchaseFundReturnAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string PurchaseFundReturnAccountNumber { get; set; }
        [Display(Name = "PurchaseReturnAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string PurchaseReturnAccountNumber { get; set; }
        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]
        public string InsUserID { get; set; }
        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime InsDateTime { get; set; }
    }
}