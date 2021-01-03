using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class St_ItemOffer
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
        [Display(Name = "QuantitySold", ResourceType = typeof(Resources.Resource))]
        public double ItemOfferQuantitySold { get; set; }
        [Display(Name = "Bonus", ResourceType = typeof(Resources.Resource))]
        public double ItemOfferBonus { get; set; }
        [Display(Name = "UnitPrice", ResourceType = typeof(Resources.Resource))]
        public double ItemOfferUnitPrice { get; set; }
        [Display(Name = "TotalPrice", ResourceType = typeof(Resources.Resource))]
        public double ItemOfferTotalPrice { get; set; }
    }
}