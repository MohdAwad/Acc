using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class Asset
    {
        public int Id { get; set; }

        public int IntAssID { get; set; }
        public int FAssetTypeID { get; set; }

        [Index("IX_AssetFirstAndSecond", 1, IsUnique = true)]
        public int CompanyID { get; set; }

        [Display(Name = "AssetID", ResourceType = typeof(Resources.Resource))]
        [Index("IX_AssetFirstAndSecond", 2, IsUnique = true)]
        public int AssetID { get; set; }


        [Display(Name = "AssetName", ResourceType = typeof(Resources.Resource))]
        public string AssetName { get; set; }


        [Display(Name = "AssetBarCode", ResourceType = typeof(Resources.Resource))]
        public string AssetBarCode { get; set; }


        [Display(Name = "AssetCost", ResourceType = typeof(Resources.Resource))]
        public double AssetCost { get; set; }

        [Display(Name = "AssetConsumRatio", ResourceType = typeof(Resources.Resource))]
        public double AssetConsumRatio { get; set; }


        [Display(Name = "ConsStartDate", ResourceType = typeof(Resources.Resource))]
        public DateTime ConsStartDate { get; set; }


        [Display(Name = "CombinedConsum", ResourceType = typeof(Resources.Resource))]
        public double CombinedConsum { get; set; }


        [Display(Name = "CombinedtDate", ResourceType = typeof(Resources.Resource))]
        public DateTime CombinedtDate { get; set; }


        [Display(Name = "BookValue", ResourceType = typeof(Resources.Resource))]
        public double BookValue { get; set; }

        [Display(Name = "Note", ResourceType = typeof(Resources.Resource))]
        public string Note { get; set; }

        [Display(Name = "DepreciationExpenseNo", ResourceType = typeof(Resources.Resource))]
        public string DebitAccountNo { get; set; }

        [Display(Name = "ConsumptionComplexNo", ResourceType = typeof(Resources.Resource))]
        public string CreditAccountNo { get; set; }
        [Display(Name = "CostCenter", ResourceType = typeof(Resources.Resource))]
        public string DebitCostNo { get; set; }

        [Display(Name = "CostCenter", ResourceType = typeof(Resources.Resource))]
        public string CreditCostNo { get; set; }

        [Display(Name = "SupplierAccountNo", ResourceType = typeof(Resources.Resource))]
        public string SupplierAccountNo { get; set; }
        [Display(Name = "PurchaseOrderNo", ResourceType = typeof(Resources.Resource))]
        public string PurchaseOrderNo { get; set; }
        [Display(Name = "PurchaseinvoiceNo", ResourceType = typeof(Resources.Resource))]
        public string PurchaseinvoiceNo { get; set; }
        public string PurchaseinvoiceNo2 { get; set; }
        [Display(Name = "PurchaseInvDate", ResourceType = typeof(Resources.Resource))]
        public DateTime PurchaseInvDate { get; set; }
        [Display(Name = "GuaranteedDate", ResourceType = typeof(Resources.Resource))]
        public DateTime GuaranteedDate { get; set; }
        [Display(Name = "LastMaintenanceDate", ResourceType = typeof(Resources.Resource))]
        public DateTime LastMaintenanceDate { get; set; }
        [Display(Name = "AssetSerialNo", ResourceType = typeof(Resources.Resource))]
        public string AssetSerialNo { get; set; }

        [Display(Name = "SupplierNote", ResourceType = typeof(Resources.Resource))]
        public string SupplierNote { get; set; }
        [Display(Name = "UpdateFinancialInfo", ResourceType = typeof(Resources.Resource))]
        public bool UpdateFinancialInfo { get; set; }

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
        [Display(Name = "ValueofConsumption", ResourceType = typeof(Resources.Resource))]
        public double ValueofConsumption { get; set; }
        public int AssetStatus { get; set; }
        [Display(Name = "SaleDate", ResourceType = typeof(Resources.Resource))]
        public DateTime SaleDate { get; set; }
        [Display(Name = "SaleValue", ResourceType = typeof(Resources.Resource))]
        public double SaleValue { get; set; }
        [Display(Name = "SaleNote", ResourceType = typeof(Resources.Resource))]
        public string SaleNote { get; set; }
        [Display(Name = "ConsumingNote", ResourceType = typeof(Resources.Resource))]
        public string ConsumingNote { get; set; }

        [Display(Name = "ConsumingDate", ResourceType = typeof(Resources.Resource))]
        public DateTime ConsumingDate { get; set; }
        public int SaleConsumID { get; set; }
        public double SoldConsNet { get; set; }
        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]
        public string InsUserID { get; set; }
        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime InsDateTime { get; set; }

    }
}