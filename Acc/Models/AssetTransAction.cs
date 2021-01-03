using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class AssetTransAction
    {
        public int Id { get; set; }

        public int VouchrNo { get; set; }
 

        public int CompanyID { get; set; }
        [Display(Name = "AssetID", ResourceType = typeof(Resources.Resource))]
        public int AssetID { get; set; }
        [Display(Name = "AssetBarCode", ResourceType = typeof(Resources.Resource))]
        public string AssetBarCode { get; set; }


        [Display(Name = "AssetCost", ResourceType = typeof(Resources.Resource))]
        public double AssetCost { get; set; }

        [Display(Name = "AssetConsumRatio", ResourceType = typeof(Resources.Resource))]
        public double AssetConsumRatio { get; set; }

        [Display(Name = "CombinedConsum", ResourceType = typeof(Resources.Resource))]
        public double CombinedConsum { get; set; }

        [Display(Name = "BookValue", ResourceType = typeof(Resources.Resource))]
        public double BookValue { get; set; }

        [Display(Name = "Note", ResourceType = typeof(Resources.Resource))]
        public string Note { get; set; }

        [Display(Name = "ValueofConsumption", ResourceType = typeof(Resources.Resource))]
        public double ValueofConsumption { get; set; }

        [Display(Name = "LastConsumptionDate", ResourceType = typeof(Resources.Resource))]
        public DateTime LastConsumptionDate { get; set; }

        [Display(Name = "ComplexConsumption", ResourceType = typeof(Resources.Resource))]
        public double ComplexConsumption { get; set; }

        [Display(Name = "AnnualConsumption", ResourceType = typeof(Resources.Resource))]
        public double AnnualConsumption { get; set; }

        [Display(Name = "PreviousConsumption", ResourceType = typeof(Resources.Resource))]
        public double PreviousConsumption { get; set; }

        [Display(Name = "ConsumptionPerDay", ResourceType = typeof(Resources.Resource))]
        public double ConsumptionPerDay { get; set; }

        public bool Exported { get; set; }
    }
}