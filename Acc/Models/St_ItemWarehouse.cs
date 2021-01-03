using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace Acc.Models
{
    public class St_ItemWarehouse
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
        [Key]
        [Column(Order = 4)]
        [Display(Name = "StockCode", ResourceType = typeof(Resources.Resource))]
        public string StockCode { get; set; }
        [Display(Name = "MinimumItem", ResourceType = typeof(Resources.Resource))]
        public double StockMinimumItemNo { get; set; }
        [Display(Name = "MaximumItem", ResourceType = typeof(Resources.Resource))]
        public double StockMaximumItemNo { get; set; }
    }
}