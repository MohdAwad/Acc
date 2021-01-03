using Acc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Acc.ViewModels
{
    public class ChartOfAccountClientVM
    {

        public int CompanyID { get; set; }



        public int CompanyYear { get; set; }

        [Required]
        [Display(Name = "AccountNumber", ResourceType = typeof(Resources.Resource))]
        public string AccountNumber { get; set; }
        [Required]
        [Display(Name = "ArabicName", ResourceType = typeof(Resources.Resource))]
        public string ArabicName { get; set; }
        [Display(Name = "EnglishName", ResourceType = typeof(Resources.Resource))]
        public string EnglishName { get; set; }

        [Display(Name = "AccountLevel", ResourceType = typeof(Resources.Resource))]
        public int AccountLevel { get; set; }

        [Display(Name = "AccountFather", ResourceType = typeof(Resources.Resource))]
        public string AccountFather { get; set; }

        [Display(Name = "AccountFatherName", ResourceType = typeof(Resources.Resource))]
        public string AccountFatherName { get; set; }
       
        [Required]
        [Display(Name = "AccountType", ResourceType = typeof(Resources.Resource))]
        public int AccountTypeID { get; set; }
        public IEnumerable<AccountType> AccountType { get; set; }
        public IEnumerable<MutilUseSearchVM> CustomerFatherAccount { get; set; }
       
        public int AccountStatusID { get; set; }
        //[Display(Name = "OpeningBalanceDebit", ResourceType = typeof(Resources.Resource))]

        [Required]
        [Display(Name = "AccountKind", ResourceType = typeof(Resources.Resource))]
        public int AccountKind { get; set; }
        //[Display(Name = "AccountStatus", ResourceType = typeof(Resources.Resource))]







        [Display(Name = "OpeningBalanceDebit", ResourceType = typeof(Resources.Resource))]
        public double OpeningBalanceDebit { get; set; }
        [Display(Name = "OpeningBalanceCredit", ResourceType = typeof(Resources.Resource))]
        public double OpeningBalanceCredit { get; set; }
        //[Display(Name = "Note", ResourceType = typeof(Resources.Resource))]

        public double ChequeBalanceDebit { get; set; }

        [Display(Name = "ChequeBalanceCredit", ResourceType = typeof(Resources.Resource))]
        public double ChequeBalanceCredit { get; set; }

        [Display(Name = "BillBalanceDebit", ResourceType = typeof(Resources.Resource))]
        public double BillBalanceDebit { get; set; }

        [Display(Name = "BillBalanceCredit", ResourceType = typeof(Resources.Resource))]
        public double BillBalanceCredit { get; set; }

        [Display(Name = "Currency", ResourceType = typeof(Resources.Resource))]
        public IEnumerable<Currency> Currency { get; set; }
        //[Display(Name = "AccountCurrency", ResourceType = typeof(Resources.Resource))]

        [Display(Name = "CurrencyID", ResourceType = typeof(Resources.Resource))]
        [Required]
        public int CurrencyID { get; set; }
       
        [Display(Name = "StoppedAccount", ResourceType = typeof(Resources.Resource))]
        public bool StoppedAccount { get; set; }

        [Display(Name = "Note", ResourceType = typeof(Resources.Resource))]
        public string Note { get; set; }
       
        [Display(Name = "InsDate", ResourceType = typeof(Resources.Resource))]
        public string InsUserNumber { get; set; }
       
        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime InsDateTime { get; set; }


        [Display(Name = "IsFirstLevel", ResourceType = typeof(Resources.Resource))]
        public bool IsFirstLevel { get; set; }

        [Display(Name = "AccountChart", ResourceType = typeof(Resources.Resource))]
        public string AccountChart { get; set; }

        [Display(Name = "AccountChart", ResourceType = typeof(Resources.Resource))]
        public string AccountChartZero { get; set; }

        public string LevelZero { get; set; }

        public string OrignalAccount { get; set; }
        [Display(Name = "Sales", ResourceType = typeof(Resources.Resource))]
        public int FSalesID { get; set; }
        public IEnumerable<Sale> Sale { get; set; }

        //-----Client Info-----//

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
        public IEnumerable<CustomerArea> CustomerArea { get; set; }
        public IEnumerable<CustomerCity> CustomerCity { get; set; }

        [Display(Name = "CityName", ResourceType = typeof(Resources.Resource))]
        public int CityID { get; set; }
        public string CityName { get; set; }

        [Display(Name = "AreaName", ResourceType = typeof(Resources.Resource))]
        public int AreaID { get; set; }
        public string AreaName { get; set; }

        [Display(Name = "NextTo", ResourceType = typeof(Resources.Resource))]
        public string NextTo { get; set; }
        [Display(Name = "StreetName", ResourceType = typeof(Resources.Resource))]
        public string StreetName { get; set; }
        [Display(Name = "BuildingNo", ResourceType = typeof(Resources.Resource))]
        public string BuildingNo { get; set; }
        [Display(Name = "FloorNo", ResourceType = typeof(Resources.Resource))]
        public string FloorNo { get; set; }
        [Display(Name = "KnownTo", ResourceType = typeof(Resources.Resource))]
        public string KnownTo { get; set; }
        [Display(Name = "TradeName", ResourceType = typeof(Resources.Resource))]
        public string TradeName { get; set; }
        [Display(Name = "CommercialRecord", ResourceType = typeof(Resources.Resource))]
        public string CommercialRecord { get; set; }
        [Display(Name = "ProfessionLicence", ResourceType = typeof(Resources.Resource))]
        public string ProfessionLicence { get; set; }
        [Display(Name = "NationalNumberOfTheFacility", ResourceType = typeof(Resources.Resource))]
        public string NationalNumberOfTheFacility { get; set; }
        [Display(Name = "AuthorizedSignatory", ResourceType = typeof(Resources.Resource))]
        public string AuthorizedSignatory { get; set; }
        [Display(Name = "PaymnetMethodTypeName", ResourceType = typeof(Resources.Resource))]
        public int PaymnetMethodTypeID { get; set; }

        [Display(Name = "DebitLimit", ResourceType = typeof(Resources.Resource))]
        public double DebitLimit { get; set; }
        [Display(Name = "DebitPeriod", ResourceType = typeof(Resources.Resource))]
        public int DebitPeriod { get; set; }
        [Display(Name = "DiscountPercentage", ResourceType = typeof(Resources.Resource))]
        public double DiscountPercentage { get; set; }
        [Display(Name = "TaxCase", ResourceType = typeof(Resources.Resource))]
        public int TaxCaseID { get; set; }
        [Display(Name = "UpperLimitForUncollectedCheques", ResourceType = typeof(Resources.Resource))]
        public double UpperLimitForUncollectedCheques { get; set; }

    }
}