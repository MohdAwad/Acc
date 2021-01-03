using Acc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class St_BankHVM
    {
        public Company Company { get; set; }
        public int CompanyID { get; set; }
        [Display(Name = "BankID", ResourceType = typeof(Resources.Resource))]
        public int BankID { get; set; }
        [Display(Name = "ArabicName", ResourceType = typeof(Resources.Resource))]
        public string ArabicName { get; set; }
        [Display(Name = "EnglishName", ResourceType = typeof(Resources.Resource))]
        public string EnglishName { get; set; }
        [Display(Name = "FundingAgency", ResourceType = typeof(Resources.Resource))]
        public int FundingAgencyID { get; set; }
        [Display(Name = "BankAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string BankAccountNumber { get; set; }
        public string BankAccountName { get; set; }
        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]
        public string InsUserID { get; set; }
        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime InsDateTime { get; set; }
        [Display(Name = "FundingAgency", ResourceType = typeof(Resources.Resource))]
        public IEnumerable<St_FundingAgencyH> St_FundingAgencyH { get; set; }

        public string BankName { get; set; }

        public string FundingAgencyName { get; set; }
    }
}