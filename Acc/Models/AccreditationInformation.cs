using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class AccreditationInformation
    {

        public Company Company { get; set; }
        [Key]
        [Column(Order = 1)]
        public int CompanyID { get; set; }
        public ChartOfAccount ChartOfAccount { get; set; }
        [Key]
        [Column(Order = 2)]
        [Display(Name = "AccountNumber", ResourceType = typeof(Resources.Resource))]
        public string AccountNumber { get; set; }



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