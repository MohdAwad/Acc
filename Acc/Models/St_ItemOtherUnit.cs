using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class St_ItemOtherUnit
    {
        public Company Company { get; set; }
        [Key]
        [Column(Order = 1)]
        public int CompanyID { get; set; }
        [Key]
        [Column(Order = 2)]
        [Display(Name = "ItemCode", ResourceType = typeof(Resources.Resource))]
        public string ItemCode { get; set; }
        [Key]
        [Column(Order = 3)]
        public int RowNumber { get; set; }
        [Display(Name = "Unit", ResourceType = typeof(Resources.Resource))]
        public int OtherUnitNumber { get; set; }
        [Display(Name = "Quantity", ResourceType = typeof(Resources.Resource))]
        public double OtherUnitQuantity { get; set; }
        [Display(Name = "SalePrice", ResourceType = typeof(Resources.Resource))]
        public double OtherUnitSalePrice { get; set; }
        [Display(Name = "PurchasePrice", ResourceType = typeof(Resources.Resource))]
        public double OtherUnitPurchasePrice { get; set; }
        [Display(Name = "Barcode", ResourceType = typeof(Resources.Resource))]
        public string OtherUnitBarcode { get; set; }
    }
}