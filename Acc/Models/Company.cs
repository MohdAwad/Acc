using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class Company 
    {
        [Key]
        [Column(Order = 1)]
        public int CompanyID { get; set; }
        [Display(Name = "ArabicName", ResourceType = typeof(Resources.Resource))]
        public string ArabicName { get; set; }
        [Display(Name = "EnglishName", ResourceType = typeof(Resources.Resource))]
        public string EnglishName { get; set; }
        [Display(Name = "Website", ResourceType = typeof(Resources.Resource))]
        public string Website { get; set; }
        [Display(Name = "Email", ResourceType = typeof(Resources.Resource))]
        public string Email { get; set; }
        [Display(Name = "Phone1", ResourceType = typeof(Resources.Resource))]
        public string Telephone { get; set; }
        [Display(Name = "Phone2", ResourceType = typeof(Resources.Resource))]
        public string Mobile { get; set; }
         [Display(Name = "TeleFax", ResourceType = typeof(Resources.Resource))]
        public string TeleFax { get; set; }
        [Display(Name = "ArbAddress", ResourceType = typeof(Resources.Resource))]
        public string ArabicAddress { get; set; }
         [Display(Name = "EngAddress", ResourceType = typeof(Resources.Resource))]
        public string EnglishAddress { get; set; }
        [Display(Name = "LogoPath", ResourceType = typeof(Resources.Resource))]
        public string LogoPath { get; set; }
        [Display(Name = "AccountChart", ResourceType = typeof(Resources.Resource))]
        public string AccountChart { get; set; }
        public string AccountChartZero { get; set; }
        [Display(Name = "CostChart", ResourceType = typeof(Resources.Resource))]
        public string CostChart { get; set; }
        public string CostChartZero { get; set; }

        [Display(Name = "WorkWithCostCenter", ResourceType = typeof(Resources.Resource))]
        public bool WorkWithCostCenter { get; set; }
        [Display(Name = "DirectExportTheTransactions", ResourceType = typeof(Resources.Resource))]
        public bool DirectExportTheTransactions { get; set; }
        public string PDFEmail { get; set; }
        [Display(Name = "TheDecimalPointForTheLocalCurrency", ResourceType = typeof(Resources.Resource))]
        public int TheDecimalPointForTheLocalCurrency { get; set; }
        [Display(Name = "TheDecimalPointForTheForeignCurrency", ResourceType = typeof(Resources.Resource))]
        public int TheDecimalPointForTheForeignCurrency { get; set; }
        [Display(Name = "CompanyLogo", ResourceType = typeof(Resources.Resource))]
        public string CompanyLogo { get; set; }

        public int CurrencyRef { get; set; }



        public int WorkWithStock { get; set; }
        
        public int Hiajneh { get; set; }

        public static implicit operator int(Company v)
        {
            throw new NotImplementedException();
        }
    }
}