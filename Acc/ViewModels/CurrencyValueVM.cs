using Acc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class CurrencyValueVM
    {
        public int CompanyID { get; set; }
        [Display(Name = "CurrencyID", ResourceType = typeof(Resources.Resource))]
        public int CurrencyID { get; set; }
        [Display(Name = "Currency", ResourceType = typeof(Resources.Resource))]
        public IEnumerable<Currency> Currency { get; set; }

        [Display(Name = "ConversionFactor", ResourceType = typeof(Resources.Resource))]
        public double ConversionFactor { get; set; }
        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]
        public string InsUserID { get; set; }
        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime InsDateTime { get; set; }
        //--------------- Currency Tabel 
        [Display(Name = "ArabicName", ResourceType = typeof(Resources.Resource))]
        public string ArabicName { get; set; }
        [Display(Name = "EnglishName", ResourceType = typeof(Resources.Resource))]
        public string EnglishName { get; set; }

        public string SConversionFactor { get; set; }

        public string sInsDateTime { get; set; }

    }
}