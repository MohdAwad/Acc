using Acc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class St_WarehouseAccountVM
    {
        public int CompanyID { get; set; }
        [Display(Name = "StockCode", ResourceType = typeof(Resources.Resource))]
        public string StockCode { get; set; }
        [Display(Name = "Warehouses", ResourceType = typeof(Resources.Resource))]
        public IEnumerable<St_Warehouse> St_Warehouse { get; set; }
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
        public string SalesFundAccountName { get; set; }
        public string SalesAccountName { get; set; }
        public string CostOfGoodsAccountName { get; set; }
        public string SalesRefundFundAccountName { get; set; }
        public string SalesReturnAccountName { get; set; }
        public string ReturnCostOfGoodsAccountName { get; set; }
        public string PurchaseFundAccountName{ get; set; }
        public string PurchaseAccountName { get; set; }
        public string PurchaseFundReturnAccountName { get; set; }
        public string PurchaseReturnAccountName { get; set; }
        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]
        public string InsUserID { get; set; }
        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime InsDateTime { get; set; }
        public int InventoryType { get; set; }
    }
}