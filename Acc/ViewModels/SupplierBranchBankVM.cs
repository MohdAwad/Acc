using Acc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class SupplierBranchBankVM
    {
        public int CompanyID { get; set; }
        [Display(Name = "BankName", ResourceType = typeof(Resources.Resource))]
        public int SupplierBankID { get; set; }
        [Display(Name = "Serial", ResourceType = typeof(Resources.Resource))]
        public int SupplierBranchBankID { get; set; }
        [Display(Name = "ArabicName", ResourceType = typeof(Resources.Resource))]
        public string ArabicName { get; set; }
        [Display(Name = "EnglishName", ResourceType = typeof(Resources.Resource))]
        public string EnglishName { get; set; }
        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]
        public string InsUserID { get; set; }
        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime InsDateTime { get; set; }
        [Display(Name = "Banks", ResourceType = typeof(Resources.Resource))]
        public IEnumerable<SupplierBank> SupplierBank { get; set; }
        [Display(Name = "BankName", ResourceType = typeof(Resources.Resource))]
        public string SupplierBankName { get; set; }
        [Display(Name = "BranchName", ResourceType = typeof(Resources.Resource))]
        public string SupplierBranchBankName { get; set; }
    }
}