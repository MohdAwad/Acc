using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class St_BranchH
    {
        public Company Company { get; set; }
        [Key]
        [Column(Order = 1)]
        public int CompanyID { get; set; }
        [Key]
        [Column(Order = 2)]
        [Display(Name = "BranchCode", ResourceType = typeof(Resources.Resource))]
        public string BranchCode { get; set; }
        [Display(Name = "ArabicName", ResourceType = typeof(Resources.Resource))]
        public string ArabicName { get; set; }
        [Display(Name = "EnglishName", ResourceType = typeof(Resources.Resource))]
        public string EnglishName { get; set; }
        [Display(Name = "Address", ResourceType = typeof(Resources.Resource))]
        public string Address { get; set; }
        [Display(Name = "Telephone", ResourceType = typeof(Resources.Resource))]
        public string Telephone { get; set; }
        [Display(Name = "BranchStockCode", ResourceType = typeof(Resources.Resource))]
        public string BranchStockCode { get; set; }
        [Display(Name = "MainStockCode", ResourceType = typeof(Resources.Resource))]
        public string MainStockCode { get; set; }

        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]
        public string InsUserID { get; set; }
        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime InsDateTime { get; set; }
    }
}