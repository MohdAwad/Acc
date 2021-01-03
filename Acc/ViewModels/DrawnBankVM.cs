using Acc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace Acc.ViewModels
{
    public class DrawnBankVM
    {
        public int CompanyID { get; set; }

        [Display(Name = "Serial", ResourceType = typeof(Resources.Resource))]
        public int BankID { get; set; }
        [Display(Name = "ArabicName", ResourceType = typeof(Resources.Resource))]
        public string ArabicName { get; set; }
        [Display(Name = "EnglishName", ResourceType = typeof(Resources.Resource))]
        public string EnglishName { get; set; }
        [Display(Name = "Active", ResourceType = typeof(Resources.Resource))]
        public string Active { get; set; }
    }
}