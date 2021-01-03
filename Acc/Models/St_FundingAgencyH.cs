using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace Acc.Models
{
    public class St_FundingAgencyH
    {
        public Company Company { get; set; }
        [Key]
        [Column(Order = 1)]
        public int CompanyID { get; set; }
        [Key]
        [Column(Order = 2)]
        [Display(Name = "FundingAgencyID", ResourceType = typeof(Resources.Resource))]
        public int FundingAgencyID { get; set; }
        [Display(Name = "ArabicName", ResourceType = typeof(Resources.Resource))]
        public string ArabicName { get; set; }
        [Display(Name = "EnglishName", ResourceType = typeof(Resources.Resource))]
        public string EnglishName { get; set; }
        [Display(Name = "FlatCommission_12", ResourceType = typeof(Resources.Resource))]
        public double FlatCommission12 { get; set; }
        [Display(Name = "FlatCommission_24", ResourceType = typeof(Resources.Resource))]
        public double FlatCommission24 { get; set; }
        [Display(Name = "FlatCommission_36", ResourceType = typeof(Resources.Resource))]
        public double FlatCommission36 { get; set; }
        [Display(Name = "FlatCommission_48", ResourceType = typeof(Resources.Resource))]
        public double FlatCommission48 { get; set; }
        [Display(Name = "FlatCommission_60", ResourceType = typeof(Resources.Resource))]
        public double FlatCommission60 { get; set; }
        [Display(Name = "CommissionAccount", ResourceType = typeof(Resources.Resource))]
        public string CommissionAccount { get; set; }
        [Display(Name = "NameOfPersonInCharge", ResourceType = typeof(Resources.Resource))]
        public string NameOfPersonInCharge { get; set; }
        [Display(Name = "PhoneOfPersonInCharge", ResourceType = typeof(Resources.Resource))]
        public string PhoneOfPersonInCharge { get; set; }
        [Display(Name = "EmailOfPersonInCharge", ResourceType = typeof(Resources.Resource))]
        public string EmailOfPersonInCharge { get; set; }

        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]
        public string InsUserID { get; set; }
        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime InsDateTime { get; set; }


    }
}