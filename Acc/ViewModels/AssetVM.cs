using Acc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class AssetVM
    {
        public int IntAssID { get; set; }
     
        public int CompanyID { get; set; }

        public IEnumerable<AssetType> AssetType { get; set; }

        [Display(Name = "AssetType", ResourceType = typeof(Resources.Resource))]
        public int FAssetTypeID { get; set; }

        [Display(Name = "AssetID", ResourceType = typeof(Resources.Resource))]
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
        public string DebitAccountNo { get; set; }//مصروف الاستهلاك


        [Display(Name = "ConsumptionComplexNo", ResourceType = typeof(Resources.Resource))]
        public string CreditAccountNo { get; set; }//مجمع الاستهلاك دائن

        [Display(Name = "DepreciationExpenseName", ResourceType = typeof(Resources.Resource))]
        public string DebitAccountName { get; set; }

        [Display(Name = "ConsumptionComplexName", ResourceType = typeof(Resources.Resource))]
        public string CreditAccountName { get; set; }


        [Display(Name = "CostCenter", ResourceType = typeof(Resources.Resource))]
        public string DebitCostNo { get; set; }//مصروف الاستهلاك


        [Display(Name = "CostCenter", ResourceType = typeof(Resources.Resource))]
        public string CreditCostNo { get; set; }//مجمع الاستهلاك دائن

        [Display(Name = "Name", ResourceType = typeof(Resources.Resource))]
        public string DebitCostName { get; set; }

        [Display(Name = "Name", ResourceType = typeof(Resources.Resource))]
        public string CreditCostName { get; set; }



        [Display(Name = "SupplierAccountNo", ResourceType = typeof(Resources.Resource))]
        public string SupplierAccountNo { get; set; }

        [Display(Name = "SupplierAccountName", ResourceType = typeof(Resources.Resource))]
        public string SupplierAccountName { get; set; }

        [Display(Name = "PurchaseOrderNo", ResourceType = typeof(Resources.Resource))]
        public string PurchaseOrderNo { get; set; }

        [Display(Name = "PurchaseinvoiceNo", ResourceType = typeof(Resources.Resource))]
        public string PurchaseinvoiceNo { get; set; }


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

        public string AssetTypeName { get; set; }


        [Display(Name = "UpdateFinancialInfo", ResourceType = typeof(Resources.Resource))]
        public bool UpdateFinancialInfo { get; set; }

        public int CompanyYear { get; set; }


        [Display(Name = "LastConsumptionDate", ResourceType = typeof(Resources.Resource))]
        public DateTime LastConsumptionDate { get; set; }

        [Display(Name = "ComplexConsumption", ResourceType = typeof(Resources.Resource))]
        public double ComplexConsumption { get; set; }

        [Display(Name = "AnnualConsumption", ResourceType = typeof(Resources.Resource))]
        public double AnnualConsumption { get; set; }

        [Display(Name = "PreviousConsumption", ResourceType = typeof(Resources.Resource))]
        public double PreviousConsumption { get; set; }


        [Display(Name = "ValueofConsumption", ResourceType = typeof(Resources.Resource))]
        public double ValueofConsumption { get; set; }

        [Display(Name = "ToDate", ResourceType = typeof(Resources.Resource))]
        public DateTime ToDate { get; set; }


        [Display(Name = "ConsumptionPerDay", ResourceType = typeof(Resources.Resource))]
        public double ConsumptionPerDay { get; set; }



        [Display(Name = "ConsumptionEndPeriod", ResourceType = typeof(Resources.Resource))]
        public double ConsumptionEndPeriod { get; set; }


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
        public Boolean WorkWithCostCenter { get; set; }

        public int TheDecimalPointForTheLocalCurrency { get; set; }
        public int TheDecimalPointForTheForeignCurrency { get; set; }

        public int CurrentYear { get; set; }

    }

}