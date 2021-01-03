using Acc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Acc.ViewModels
{
    public class ChartOfAccountVM
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

        public int AccountStatusID { get; set; }

        [Required]
        [Display(Name = "AccountKind", ResourceType = typeof(Resources.Resource))]
        public int AccountKind { get; set; }

        [Display(Name = "OpeningBalanceDebit", ResourceType = typeof(Resources.Resource))]
        public double OpeningBalanceDebit { get; set; }
        [Display(Name = "OpeningBalanceCredit", ResourceType = typeof(Resources.Resource))]
        public double OpeningBalanceCredit { get; set; }


        [Display(Name = "ChequeBalanceDebit", ResourceType = typeof(Resources.Resource))]
        public double ChequeBalanceDebit { get; set; }

        [Display(Name = "ChequeBalanceCredit", ResourceType = typeof(Resources.Resource))]
        public double ChequeBalanceCredit { get; set; }

        [Display(Name = "BillBalanceDebit", ResourceType = typeof(Resources.Resource))]
        public double BillBalanceDebit { get; set; }

        [Display(Name = "BillBalanceCredit", ResourceType = typeof(Resources.Resource))]
        public double BillBalanceCredit { get; set; }

        [Display(Name = "Currency", ResourceType = typeof(Resources.Resource))]
        public IEnumerable<Currency> Currency { get; set; }
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

        [Display(Name = "CityName", ResourceType = typeof(Resources.Resource))]
        public string CityName { get; set; }

        [Display(Name = "AreaName", ResourceType = typeof(Resources.Resource))]
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
        public IEnumerable<CustomerArea> CustomerArea { get; set; }

        [Display(Name = "CityName", ResourceType = typeof(Resources.Resource))]
        public int CityID { get; set; }

        [Display(Name = "AreaName", ResourceType = typeof(Resources.Resource))]
        public int AreaID { get; set; }

        //------Supplier Info--------//

        
        [Display(Name = "CityName", ResourceType = typeof(Resources.Resource))]
        public int SupplierCityID { get; set; }
        public string SupplierCityName { get; set; }

        [Display(Name = "CountryName", ResourceType = typeof(Resources.Resource))]
        public int SupplierCountryID { get; set; }
        public string SupplierCountryName { get; set; }

        [Display(Name = "Address", ResourceType = typeof(Resources.Resource))]
        public string Address { get; set; }



        [Display(Name = "NameOfPersonInCharge", ResourceType = typeof(Resources.Resource))]
        public string NameOfPersonInCharge { get; set; }


        [Display(Name = "PhoneOfPersonInCharge", ResourceType = typeof(Resources.Resource))]
        public string PhoneOfPersonInCharge { get; set; }


        [Display(Name = "BankAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string BankAccountNumber { get; set; }
        [Display(Name = "BeneficiaryName", ResourceType = typeof(Resources.Resource))]
        public string BeneficiaryName { get; set; }
        [Display(Name = "IBAN", ResourceType = typeof(Resources.Resource))]
        public string IBAN { get; set; }
        [Display(Name = "SwiftCode", ResourceType = typeof(Resources.Resource))]
        public string SwiftCode { get; set; }
        [Display(Name = "BankName", ResourceType = typeof(Resources.Resource))]
        public int SupplierBankID { get; set; }
        public string SupplierBankName { get; set; }
        [Display(Name = "BranchBankName", ResourceType = typeof(Resources.Resource))]
        public int SupplierBranchBankID { get; set; }
        public string SupplierBranchBankName { get; set; }
        [Display(Name = "BankAdderss", ResourceType = typeof(Resources.Resource))]
        public string BankAdderss { get; set; }
        [Display(Name = "CountryBankName", ResourceType = typeof(Resources.Resource))]
        public int SupplierCountryBankID { get; set; }
        public string SupplierCountryBankName { get; set; }
        [Display(Name = "CityBankName", ResourceType = typeof(Resources.Resource))]
        public int SupplierCityBankID { get; set; }
        public string SupplierCityBankName { get; set; }
        [Display(Name = "SupplierType", ResourceType = typeof(Resources.Resource))]
        public int SupplierTypeID { get; set; }
        public string SupplierTypeName { get; set; }

        public IEnumerable<SupplierBranchBank> SupplierBranchBank { get; set; }
        public IEnumerable<SupplierCity> SupplierCity { get; set; }
        public IEnumerable<SupplierCityBank> SupplierCityBank { get; set; }
        public IEnumerable<SupplierType> SupplierType { get; set; }













    }
}