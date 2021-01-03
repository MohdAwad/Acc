using Acc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class DefinitionAssetSiteVM
    {
        public int CompanyID { get; set; }
        [Display(Name = "AssetID", ResourceType = typeof(Resources.Resource))]
        public int AssetID { get; set; }
        public string AssetName { get; set; }
        [Display(Name = "AssetTypeID", ResourceType = typeof(Resources.Resource))]
        public int AssetTypeID { get; set; }
        public string AssetTypeName { get; set; }
        [Display(Name = "AssetAdministration", ResourceType = typeof(Resources.Resource))]
        public int AdministrationID { get; set; }
        public IEnumerable<AssetAdministration> AssetAdministration { get; set; }
        [Display(Name = "AssetCircle", ResourceType = typeof(Resources.Resource))]
        public int CircleID { get; set; }
        public IEnumerable<AssetCircle> AssetCircle { get; set; }
        [Display(Name = "AssetSection", ResourceType = typeof(Resources.Resource))]
        public int SectionID { get; set; }
        public IEnumerable<AssetSection> AssetSection { get; set; }
        [Display(Name = "AssetSite", ResourceType = typeof(Resources.Resource))]
        public int SiteID { get; set; }
        public IEnumerable<AssetSite> AssetSite { get; set; }
        [Display(Name = "TransferDate", ResourceType = typeof(Resources.Resource))]
        public DateTime TransferDate { get; set; }
        [Display(Name = "Note", ResourceType = typeof(Resources.Resource))]
        public string Note { get; set; }
        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]
        public string InsUserID { get; set; }
        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime InsDateTime { get; set; }
        [Display(Name = "AssetTrustee", ResourceType = typeof(Resources.Resource))]
        public int TrusteeID { get; set; }
        public IEnumerable<AssetTrustee> AssetTrustee { get; set; }
        public string AdministrationName { get; set; }
        public string CircleName { get; set; }
        public string SectionName { get; set; }
        public string SiteName { get; set; }
        public string TrusteeName { get; set; }
        [Display(Name = "DeliveryDate", ResourceType = typeof(Resources.Resource))]
        public DateTime DeliveryDate { get; set; }
        [Display(Name = "Note", ResourceType = typeof(Resources.Resource))]
        public string DeliveryNote { get; set; }
        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]
        public string DeliveryInsUserID { get; set; }
        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime DeliveryInsDateTime { get; set; }
        public int SerialID { get; set; }
        public string DefinitionUserName { get; set; }
        public string DeliveryUserName { get; set; }
        public double AssetCost { get; set; }
        public double CombinedConsum { get; set; }
        public double BookValue { get; set; }
        public double AnnualConsumption { get; set; }
        public IEnumerable<AssetType> AssetType { get; set; }
        public string SumAssetCost { get; set; }
        public string SumCombinedConsum { get; set; }
        public string SumBookValue { get; set; }
        public string SumAnnualConsumption { get; set; }
        public string SumValueofConsumption { get; set; }
        public string SumPreviousConsumption { get; set; }
        public int Count { get; set; }
        public Boolean ApproveTransferDate { get; set; }
        public Boolean ApproveDeliveryDate { get; set; }
        [Display(Name = "FromDate", ResourceType = typeof(Resources.Resource))]
        public DateTime FromTransferDate { get; set; }

        [Display(Name = "ToDate", ResourceType = typeof(Resources.Resource))]
        public DateTime ToTransferDate { get; set; }
        [Display(Name = "FromDate", ResourceType = typeof(Resources.Resource))]
        public DateTime FromDeliveryDate { get; set; }

        [Display(Name = "ToDate", ResourceType = typeof(Resources.Resource))]
        public DateTime ToDeliveryDate { get; set; }
        public DateTime ConsStartDate { get; set; }
        public DateTime CombinedtDate { get; set; }
        public DateTime GuaranteedDate { get; set; }
        public double AssetConsumRatio { get; set; }
        public double YearCombined { get; set; }
        public string DebitAccountNo { get; set; }
        public string CreditAccountNo { get; set; }
        public string DebitCostNo { get; set; }
        public string CreditCostNo { get; set; }
        public string DebitAccountName { get; set; }
        public string CreditAccountName { get; set; }
        public string DebitCostName { get; set; }
        public string CreditCostName { get; set; }
        public string AssetBarCode { get; set; }
        public string AssetSerialNo { get; set; }
        public Boolean ApproveConsStartDate { get; set; }
        public Boolean ApproveCombinedtDate { get; set; }
        [Display(Name = "FromDate", ResourceType = typeof(Resources.Resource))]
        public DateTime FromConsStartDate { get; set; }

        [Display(Name = "ToDate", ResourceType = typeof(Resources.Resource))]
        public DateTime ToConsStartDate { get; set; }
        [Display(Name = "FromDate", ResourceType = typeof(Resources.Resource))]
        public DateTime FromCombinedtDate { get; set; }

        [Display(Name = "ToDate", ResourceType = typeof(Resources.Resource))]
        public DateTime ToCombinedtDate { get; set; }
        public Boolean WorkWithCostCenter { get; set; }
        public int VouchrNo { get; set; }
        public DateTime VouchrDate { get; set; }
        public double Total { get; set; }
        [Display(Name = "FromTheDateOfConsumptionWork", ResourceType = typeof(Resources.Resource))]
        public DateTime FromVouchrDate { get; set; }

        [Display(Name = "ToDate", ResourceType = typeof(Resources.Resource))]
        public DateTime ToVouchrDate { get; set; }
        public double ValueofConsumption { get; set; }
        public double PreviousConsumption { get; set; }
        public DateTime LastConsumptionDate { get; set; }
        public DateTime ConsumptionDate { get; set; }
        public bool ApproveLastConsumptionDate { get; set; }
        public bool ApproveConsumptionDate { get; set; }
        [Display(Name = "FromDate", ResourceType = typeof(Resources.Resource))]
        public DateTime FromLastConsumptionDate { get; set; }

        [Display(Name = "ToDate", ResourceType = typeof(Resources.Resource))]
        public DateTime ToLastConsumptionDate { get; set; }
        [Display(Name = "FromDate", ResourceType = typeof(Resources.Resource))]
        public DateTime FromConsumptionDate { get; set; }

        [Display(Name = "ToDate", ResourceType = typeof(Resources.Resource))]
        public DateTime ToConsumptionDate { get; set; }

        public int TheDecimalPointForTheLocalCurrency { get; set; }
        public int TheDecimalPointForTheForeignCurrency { get; set; }

        public int CompanyYear { get; set; }

        public int CurrentYear { get; set; }

    }
}