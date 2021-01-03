using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class AssetMaintenance
    {
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
        public int FAssetTypeID { get; set; }
        public int CompanyID { get; set; }
        [Display(Name = "AssetID", ResourceType = typeof(Resources.Resource))]
        public int AssetID { get; set; }

        [Display(Name = "ConsumptionComplexNo", ResourceType = typeof(Resources.Resource))]
        public string CreditAccountNo { get; set; }//مجمع الاستهلاك دائن
        public string AddBy { get; set; }
        [Display(Name = "Note", ResourceType = typeof(Resources.Resource))]
        public string Note { get; set; }

        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]
        public string InsUserID { get; set; }
        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime InsDateTime { get; set; }
    }
}