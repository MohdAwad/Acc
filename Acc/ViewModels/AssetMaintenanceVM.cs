using Acc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class AssetMaintenanceVM
    {
        public string AssetTypeName { get; set; }
        public int Id { get; set; }
        [Display(Name = "Serial", ResourceType = typeof(Resources.Resource))]
        public int Serial { get; set; }

        [Display(Name = "Date", ResourceType = typeof(Resources.Resource))]
        public DateTime TrDate { get; set; }

        [Display(Name = "MaintenancKind", ResourceType = typeof(Resources.Resource))]
        public int TrKind { get; set; }

        [Display(Name = "PayValue", ResourceType = typeof(Resources.Resource))]
        public double PayValue { get; set; }

        [Display(Name = "Warrantyuntildate", ResourceType = typeof(Resources.Resource))]
        public DateTime GrundToDate { get; set; }
        [Display(Name = "MaintenanceNo", ResourceType = typeof(Resources.Resource))]
        public string MaintenanceNo { get; set; }

        [Display(Name = "MaintenanceVoucherNo", ResourceType = typeof(Resources.Resource))]
        public string MaintenanceVoucherNo { get; set; }

        [Display(Name = "MaintenanceVoucherDate", ResourceType = typeof(Resources.Resource))]
        public DateTime VoucherDate { get; set; }

        [Display(Name = "Note", ResourceType = typeof(Resources.Resource))]
        public string MaintenanceNote { get; set; }

        [Display(Name = "AssetType", ResourceType = typeof(Resources.Resource))]
       public  IEnumerable<AssetType> AssetType { get; set; }
        [Display(Name = "AssetType", ResourceType = typeof(Resources.Resource))]
        public int FAssetTypeID { get; set; }


        public int CompanyID { get; set; }

        [Display(Name = "AssetID", ResourceType = typeof(Resources.Resource))]

        public int AssetID { get; set; }


        [Display(Name = "AssetName", ResourceType = typeof(Resources.Resource))]
        public string AssetName { get; set; }

        [Display(Name = "ConsumptionComplexNo", ResourceType = typeof(Resources.Resource))]
        public string CreditAccountNo { get; set; }//مجمع الاستهلاك دائن

        [Display(Name = "ConsumptionComplexName", ResourceType = typeof(Resources.Resource))]
        public string CreditAccountName { get; set; }

        public string AddBy { get; set; }

        [Display(Name = "Note", ResourceType = typeof(Resources.Resource))]
        public string Note { get; set; }
        [Display(Name = "CostCenter", ResourceType = typeof(Resources.Resource))]
        public string CreditCostNo { get; set; }//مجمع الاستهلاك دائن

        [Display(Name = "Name", ResourceType = typeof(Resources.Resource))]
        public string CreditCostName { get; set; }
        public Boolean WorkWithCostCenter { get; set; }
        public int TheDecimalPointForTheLocalCurrency { get; set; }
        public int TheDecimalPointForTheForeignCurrency { get; set; }

        public int CurrentYear { get; set; }

        public int CompanyYear { get; set; }
    }
}