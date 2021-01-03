using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class St_SimilarItemH
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
        [Display(Name = "ItemCode", ResourceType = typeof(Resources.Resource))]
        public string SimilarItemCode { get; set; }
    }
}