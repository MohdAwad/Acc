using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class St_WarehouseSearchHVM
    {
        public string GroupCode { get; set; }
        public string GroupName { get; set; }
        public string SearchGroupCode { get; set; }
        public string SearchGroupName { get; set; }
        public string SearchItemCode { get; set; }
        public string SearchItemName { get; set; }
        public double SearchSalePrice { get; set; }
        public double SearchLocalCost { get; set; }
        public Boolean SearchStopItem { get; set; }
        public int FactoryID { get; set; }
        public string FactoryName { get; set; }
        public double SearchCostRate { get; set; }
        public IEnumerable<St_ItemGroupH> St_ItemGroupH { get; set; }
        public int TheDecimalPointForTheLocalCurrency { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public double SalePrice { get; set; }
        public Boolean StopItem { get; set; }
        public double CostRate { get; set; }
        public double TaxRate { get; set; }
    }
}