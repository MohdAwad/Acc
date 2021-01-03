using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class CustSuppRepVM
    {

        [Display(Name = "AccountNumber", ResourceType = typeof(Resources.Resource))]
        public string AccountNumber { get; set; }


        [Display(Name = "AccountName", ResourceType = typeof(Resources.Resource))]
        public string AccountName { get; set; }
        public int AccountTypeID { get; set; }

        [Display(Name = "AccountKind", ResourceType = typeof(Resources.Resource))]
        public int AccountKind { get; set; }

        [Display(Name = "Sales", ResourceType = typeof(Resources.Resource))]
        public int SalesID { get; set; }

        [Display(Name = "Sales", ResourceType = typeof(Resources.Resource))]
        public string SaleName { get; set; }

        [Display(Name = "Email", ResourceType = typeof(Resources.Resource))]
        public string Email { get; set; }
        [Display(Name = "Phone1", ResourceType = typeof(Resources.Resource))]
        public string Telephone { get; set; }
        [Display(Name = "Phone2", ResourceType = typeof(Resources.Resource))]
        public string Mobile { get; set; }
        [Display(Name = "TeleFax", ResourceType = typeof(Resources.Resource))]
        public string TeleFax { get; set; }

        [Display(Name = "CityName", ResourceType = typeof(Resources.Resource))]
        public string CustomerCity { get; set; }

        [Display(Name = "AreaName", ResourceType = typeof(Resources.Resource))]
        public string CustomerArea { get; set; }

        [Display(Name = "StreetName", ResourceType = typeof(Resources.Resource))]
        public string StreetName { get; set; }

        [Display(Name = "Balance", ResourceType = typeof(Resources.Resource))]
        public double Balance { get; set; }
        [Display(Name = "NameOfPersonInCharge", ResourceType = typeof(Resources.Resource))]
        public string NameOfPersonInCharge { get; set; }
        [Display(Name = "CountryName", ResourceType = typeof(Resources.Resource))]
        public string SupplierCountry { get; set; }
        [Display(Name = "CityName", ResourceType = typeof(Resources.Resource))]
        public string SupplierCity { get; set; }
        [Display(Name = "OpeningBalance", ResourceType = typeof(Resources.Resource))]
        public double OpeningBalanceDebit { get; set; }

        [Display(Name = "OpeningBalance", ResourceType = typeof(Resources.Resource))]
        public double OpeningBalanceCredit { get; set; }


    }
}