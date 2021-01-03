using Acc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class SupplierCityBankVM
    {        
        public int CompanyID { get; set; }

        [Display(Name = "CountryBankName", ResourceType = typeof(Resources.Resource))]
        public int SupplierCountryBankID { get; set; }
        [Display(Name = "Serial", ResourceType = typeof(Resources.Resource))]
        public int SupplierCityBankID { get; set; }
        [Display(Name = "ArabicName", ResourceType = typeof(Resources.Resource))]
        public string ArabicName { get; set; }
        [Display(Name = "EnglishName", ResourceType = typeof(Resources.Resource))]
        public string EnglishName { get; set; }
        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]
        public string InsUserID { get; set; }
        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime InsDateTime { get; set; }
        [Display(Name = "Countries", ResourceType = typeof(Resources.Resource))]
        public IEnumerable<SupplierCountryBank> SupplierCountryBank { get; set; }
        [Display(Name = "CountryName", ResourceType = typeof(Resources.Resource))]
        public string SupplierCountryBankName { get; set; }
        [Display(Name = "CityName", ResourceType = typeof(Resources.Resource))]
        public string SupplierCityBankName { get; set; }
    }
}