using Acc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class ChartOfAccountAccreditationVM
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
        // public AccountType AccountType { get; set; }
        //[Display(Name = "AccountType", ResourceType = typeof(Resources.Resource))]

        [Required]
        [Display(Name = "AccountType", ResourceType = typeof(Resources.Resource))]
        public int AccountTypeID { get; set; }
        public IEnumerable<AccountType> AccountType { get; set; }

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
        //[Display(Name = "AccountCurrency", ResourceType = typeof(Resources.Resource))]

        [Display(Name = "CurrencyID", ResourceType = typeof(Resources.Resource))]
        [Required]
        public int CurrencyID { get; set; }
        //[Display(Name = "StoppedAccount", ResourceType = typeof(Resources.Resource))]

        [Display(Name = "StoppedAccount", ResourceType = typeof(Resources.Resource))]
        public bool StoppedAccount { get; set; }

        [Display(Name = "Note", ResourceType = typeof(Resources.Resource))]
        public string Note { get; set; }
        // [Display(Name = "InsUserNumber", ResourceType = typeof(Resources.Resource))]

        [Display(Name = "InsDate", ResourceType = typeof(Resources.Resource))]
        public string InsUserNumber { get; set; }
        // [Display(Name = "InsDate", ResourceType = typeof(Resources.Resource))]

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

        //--



        [Display(Name = "Bank", ResourceType = typeof(Resources.Resource))]
        public string Bank { get; set; }

        [Display(Name = "CreditInlocalCurrency", ResourceType = typeof(Resources.Resource))]
        public double CreditInlocalCurrency { get; set; }


        [Display(Name = "CreditForeignCurrency", ResourceType = typeof(Resources.Resource))]
        public double CreditForeignCurrency { get; set; }


        [Display(Name = "CurrencyType", ResourceType = typeof(Resources.Resource))]
        public string CurrencyType { get; set; }


        [Display(Name = "ExchangeRate", ResourceType = typeof(Resources.Resource))]
        public double ExchangeRate { get; set; }


        [Display(Name = "TypeOfGoods", ResourceType = typeof(Resources.Resource))]
        public string TypeOfGoods { get; set; }


        [Display(Name = "ValidityOfAccreditation", ResourceType = typeof(Resources.Resource))]
        public DateTime ValidityOfAccreditation { get; set; }


        [Display(Name = "PaymentTerms", ResourceType = typeof(Resources.Resource))]
        public string PaymentTerms { get; set; }


        [Display(Name = "ShippingPort", ResourceType = typeof(Resources.Resource))]
        public string ShippingPort { get; set; }


        [Display(Name = "ArrivePort", ResourceType = typeof(Resources.Resource))]
        public string ArrivePort { get; set; }


        [Display(Name = "BankInsurance", ResourceType = typeof(Resources.Resource))]
        public double BankInsurance { get; set; }


        [Display(Name = "CommissionsPaid", ResourceType = typeof(Resources.Resource))]
        public double CommissionsPaid { get; set; }


        [Display(Name = "DateOfIssuanceOfCredit", ResourceType = typeof(Resources.Resource))]
        public DateTime DateOfIssuanceOfCredit { get; set; }


        [Display(Name = "LastShipmentDate", ResourceType = typeof(Resources.Resource))]
        public DateTime LastShipmentDate { get; set; }


        [Display(Name = "AccreditationExpiresPlace", ResourceType = typeof(Resources.Resource))]
        public string AccreditationExpiresPlace { get; set; }


        [Display(Name = "BeneficiaryOfAccreditation", ResourceType = typeof(Resources.Resource))]
        public string BeneficiaryOfAccreditation { get; set; }


        [Display(Name = "PartialCharging", ResourceType = typeof(Resources.Resource))]
        public string PartialCharging { get; set; }


        [Display(Name = "MultipleShippingMethods", ResourceType = typeof(Resources.Resource))]
        public string MultipleShippingMethods { get; set; }
    }
}