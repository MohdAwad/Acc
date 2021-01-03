using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class CustomerInformation
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
        public int CityID { get; set; }

        [Display(Name = "AreaName", ResourceType = typeof(Resources.Resource))]
        public int AreaID { get; set; }

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

        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]
        public string InsUserID { get; set; }
        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime InsDateTime { get; set; }
        [Display(Name = "TaxCase", ResourceType = typeof(Resources.Resource))]
        public int TaxCaseID { get; set; }
        [Display(Name = "UpperLimitForUncollectedCheques", ResourceType = typeof(Resources.Resource))]
        public double UpperLimitForUncollectedCheques { get; set; }
    }
}