using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class Service
    {
        public Company Company { get; set; }
        [Key]
        [Column(Order = 1)]
        public int CompanyID { get; set; }
        [Key]
        [Column(Order = 2)]
        [Display(Name = "Serial", ResourceType = typeof(Resources.Resource))]
        public int ServiceID { get; set; }
        public ServiceGroup ServiceGroup { get; set; }
        [Key]
        [Column(Order = 3)]
        [Display(Name = "ServiceGroup", ResourceType = typeof(Resources.Resource))]
        public int ServiceGroupID { get; set; }
        [Required]
        [Display(Name = "ArabicName", ResourceType = typeof(Resources.Resource))]
        public string ArabicName { get; set; }
        [Display(Name = "EnglishName", ResourceType = typeof(Resources.Resource))]
        public string EnglishName { get; set; }
        [Display(Name = "CostPrice", ResourceType = typeof(Resources.Resource))]
        public double CostPrice { get; set; }
        [Display(Name = "SalePrice", ResourceType = typeof(Resources.Resource))]
        public double SalePrice { get; set; }
        [Display(Name = "TaxPercentage", ResourceType = typeof(Resources.Resource))]
        public double TaxPercentage { get; set; }
        [Display(Name = "Note", ResourceType = typeof(Resources.Resource))]
        public string Note { get; set; }
        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]
        public string InsUserID { get; set; }
        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime InsDateTime { get; set; }
    }
}