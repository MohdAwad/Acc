using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class St_ItemOtherUnitVM
    {
        public IEnumerable<St_ItemOtherUnitVM> St_ItemUnit { get; set; }
        public int OtherItemUnitNumber { get; set; }
        public string OtherItemUnitName { get; set; }
        public string OtherUnitName { get; set; }
        public string OtherUnitBarcode { get; set; }
        public int OtherUnitNumber { get; set; }
        public double OtherUnitQuantity { get; set; }
        public double OtherUnitSalePrice { get; set; }
        public double OtherUnitPurchasePrice { get; set; }
        public double OtherUnitQuantityTotal { get; set; }
        public double OtherUnitChooseQuantity { get; set; }
        public double OtherUnitPrice { get; set; }
        public string ItemCode { get; set; }
        public int TheDecimalPointForTheLocalCurrency { get; set; }
    }
}