using Acc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class ServiceVM
    {
        public int CompanyID { get; set; }
        [Display(Name = "Serial", ResourceType = typeof(Resources.Resource))]
        public int ServiceID { get; set; }
        [Display(Name = "ServiceGroup", ResourceType = typeof(Resources.Resource))]
        public int ServiceGroupID { get; set; }
        [Display(Name = "Group", ResourceType = typeof(Resources.Resource))]
        public IEnumerable<ServiceGroup> ServiceGroup { get; set; }
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
        public string ServiceName { get; set; }
        public string ServiceGroupName { get; set; }
        public string SSalePrice { get; set; }
        public string SCostPrice { get; set; }
        public string STaxPercentage { get; set; }
        public int TheDecimalPointForTheLocalCurrency { get; set; }
        public int TheDecimalPointForTheForeignCurrency { get; set; }
    }
}