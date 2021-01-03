using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class SupplierInformation
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
        public int SupplierCityID { get; set; }

        [Display(Name = "CountryName", ResourceType = typeof(Resources.Resource))]
        public int SupplierCountryID { get; set; }

        [Display(Name = "Address", ResourceType = typeof(Resources.Resource))]
        public string Address { get; set; }

        [Display(Name = "PaymnetMethodTypeName", ResourceType = typeof(Resources.Resource))]
        public int PaymnetMethodTypeID { get; set; }

        [Display(Name = "NameOfPersonInCharge", ResourceType = typeof(Resources.Resource))]
        public string NameOfPersonInCharge { get; set; }

        [Display(Name = "PhoneOfPersonInCharge", ResourceType = typeof(Resources.Resource))]
        public string PhoneOfPersonInCharge { get; set; }

        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]
        public string InsUserID { get; set; }


        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime InsDateTime { get; set; }


        [Display(Name = "DebitPeriod", ResourceType = typeof(Resources.Resource))]
        public int DebitPeriod { get; set; }

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
        [Display(Name = "BranchBankName", ResourceType = typeof(Resources.Resource))]
        public int SupplierBranchBankID { get; set; }
        [Display(Name = "BankAdderss", ResourceType = typeof(Resources.Resource))]
        public string BankAdderss { get; set; }
        [Display(Name = "CountryBankName", ResourceType = typeof(Resources.Resource))]
        public int SupplierCountryBankID { get; set; }
        [Display(Name = "CityBankName", ResourceType = typeof(Resources.Resource))]
        public int SupplierCityBankID { get; set; }
        [Display(Name = "SupplierType", ResourceType = typeof(Resources.Resource))]
        public int SupplierTypeID { get; set; }
    }
}