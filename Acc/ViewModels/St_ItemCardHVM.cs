using Acc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class St_ItemCardHVM
    {
        public IEnumerable<St_ItemWarehouseH> St_ItemWarehouseH { get; set; }
        public IEnumerable<St_SubColorsItemH> St_SubColorsItemH { get; set; }
        public IEnumerable<St_RawMaterialH> St_RawMaterialH { get; set; }
        public IEnumerable<St_ManufacturingStageH> St_ManufacturingStageH { get; set; }
        public IEnumerable<St_RelatedItemH> St_RelatedItemH { get; set; }
        public IEnumerable<St_SimilarItemH> St_SimilarItemH { get; set; }
        public string UserName { get; set; }
        public int TheDecimalPointForTheLocalCurrency { get; set; }
        //------------------------------------------------------------------------
        public int CompanyID { get; set; }
        [Display(Name = "GroupCode", ResourceType = typeof(Resources.Resource))]
        public string GroupCode { get; set; }
        public string GroupName { get; set; }
        public IEnumerable<St_ItemGroupH> St_ItemGroupH { get; set; }

        [Display(Name = "ItemCode", ResourceType = typeof(Resources.Resource))]
        public string ItemCode { get; set; }
        public int ItemSerial { get; set; }
        [Display(Name = "ArabicName", ResourceType = typeof(Resources.Resource))]
        public string ArabicName { get; set; }
        [Display(Name = "EnglishName", ResourceType = typeof(Resources.Resource))]
        public string EnglishName { get; set; }
        [Display(Name = "ItemName", ResourceType = typeof(Resources.Resource))]
        public string ItemName { get; set; }
        [Display(Name = "NumberOfPieces", ResourceType = typeof(Resources.Resource))]
        public int NumberOfPieces { get; set; }
        [Display(Name = "CBM", ResourceType = typeof(Resources.Resource))]
        public double CBM { get; set; }
        [Display(Name = "Unit", ResourceType = typeof(Resources.Resource))]
        public int ItemUnitNo { get; set; }
        public string ItemUnitName { get; set; }
        public IEnumerable<St_ItemUnitH> St_ItemUnitH { get; set; }
        [Display(Name = "SupplierAccountNo", ResourceType = typeof(Resources.Resource))]
        public string SupplierAccountNumber { get; set; }
        public string SupplierAccountName { get; set; }
        [Display(Name = "Style", ResourceType = typeof(Resources.Resource))]
        public int StyleNo { get; set; }
        [Display(Name = "ItemLevel", ResourceType = typeof(Resources.Resource))]
        public int ItemLevelNo { get; set; }
        [Display(Name = "StopItem", ResourceType = typeof(Resources.Resource))]
        public bool StopItem { get; set; }
        [Display(Name = "ShowOnline", ResourceType = typeof(Resources.Resource))]
        public int ShowOnline { get; set; }
        public double QuantityOnline { get; set; }
        [Display(Name = "SaleOfOfferedArticleIsPermitted", ResourceType = typeof(Resources.Resource))]
        public bool SaleOfOfferedArticleIsPermitted { get; set; }
        [Display(Name = "ItemType", ResourceType = typeof(Resources.Resource))]
        public int ItemTypeNo { get; set; }
        public string ItemTypeName { get; set; }
        [Display(Name = "FactoryName", ResourceType = typeof(Resources.Resource))]
        public int FactoryNo { get; set; }
        public string FactoryName { get; set; }
        public IEnumerable<St_FactoryH> St_FactoryH { get; set; }
        [Display(Name = "NumberOfWorkingDays", ResourceType = typeof(Resources.Resource))]
        public int NumberOfWorkingDays { get; set; }
        [Display(Name = "NumberOfStages", ResourceType = typeof(Resources.Resource))]
        public int NumberOfStages { get; set; }
        [Display(Name = "WageRate", ResourceType = typeof(Resources.Resource))]
        public double WageRate { get; set; }
        [Display(Name = "ApprovingTheWarehouseQuantityLessThan", ResourceType = typeof(Resources.Resource))]
        public bool ApprovingTheWarehouseQuantityLessThan { get; set; }
        public double QuantityManufacturing { get; set; }
        [Display(Name = "FabricChangeIsAllowed", ResourceType = typeof(Resources.Resource))]
        public bool FabricChangeIsAllowed { get; set; }
        [Display(Name = "AllowWoodToChangeColor", ResourceType = typeof(Resources.Resource))]
        public bool AllowWoodToChangeColor { get; set; }
        [Display(Name = "FactoryNotes", ResourceType = typeof(Resources.Resource))]
        public string FactoryNotes { get; set; }
        [Display(Name = "SalePrice", ResourceType = typeof(Resources.Resource))]
        public double SalePrice { get; set; }
        [Display(Name = "TaxRate", ResourceType = typeof(Resources.Resource))]
        public double TaxRate { get; set; }
        [Display(Name = "TaxType", ResourceType = typeof(Resources.Resource))]
        public int TaxTypeNo { get; set; }
        [Display(Name = "LocalCost", ResourceType = typeof(Resources.Resource))]
        public double LocalCost { get; set; }
        [Display(Name = "ForeignCost", ResourceType = typeof(Resources.Resource))]
        public double ForeignCost { get; set; }
        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]
        public string InsUserID { get; set; }
        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime InsDateTime { get; set; }
        public DateTime OpeningDate { get; set; }
        [Display(Name = "LastLocalPurchasePrice", ResourceType = typeof(Resources.Resource))]
        public double LastLocalPurchasePrice { get; set; }
        [Display(Name = "LastForeignPurchasePrice", ResourceType = typeof(Resources.Resource))]
        public double LastForeignPurchasePrice { get; set; }
        [Display(Name = "CostRate", ResourceType = typeof(Resources.Resource))]
        public double CostRate { get; set; }
        [Display(Name = "TotalQuantitySold", ResourceType = typeof(Resources.Resource))]
        public double TotalQuantitySold { get; set; }
        [Display(Name = "TotalValueSold", ResourceType = typeof(Resources.Resource))]
        public double TotalValueSold { get; set; }
        [Display(Name = "TheNumberOfDaysTheCardIsOpened", ResourceType = typeof(Resources.Resource))]
        public int TheNumberOfDaysTheCardIsOpened { get; set; }
        [Display(Name = "TheTargetMonthlyAmount", ResourceType = typeof(Resources.Resource))]
        public int TheTargetMonthlyAmount { get; set; }
        [Display(Name = "ItemCase", ResourceType = typeof(Resources.Resource))]
        public string ItemCase { get; set; }
        [Display(Name = "ItemCase", ResourceType = typeof(Resources.Resource))]
        public int ItemCaseInt { get; set; }
        [Display(Name = "ReOrderLimit", ResourceType = typeof(Resources.Resource))]
        public int ReOrderLimit { get; set; }
        [Display(Name = "OrderQuantity", ResourceType = typeof(Resources.Resource))]
        public int OrderQuantity { get; set; }
        //------------------------------------------------------------------------
        //------------------------------------------------------------------------
        [Display(Name = "StockCode", ResourceType = typeof(Resources.Resource))]
        public string StockCode { get; set; }
        [Display(Name = "StockName", ResourceType = typeof(Resources.Resource))]
        public string StockName { get; set; }
        public int ItemWarehouseRowNumber { get; set; }
        //------------------------------------------------------------------------

        //------------------------------------------------------------------------
        [Display(Name = "FactoryName", ResourceType = typeof(Resources.Resource))]
        public int ManufacturingStageFactoryNo { get; set; }
        [Display(Name = "NumberOfDays", ResourceType = typeof(Resources.Resource))]
        public int NumberOfDays { get; set; }
        [Display(Name = "FactoryName", ResourceType = typeof(Resources.Resource))]
        public string ManufacturingStageFactoryName { get; set; }
        public int ManufacturingStageRowNumber { get; set; }
        //------------------------------------------------------------------------

        //------------------------------------------------------------------------
        [Display(Name = "ItemCode", ResourceType = typeof(Resources.Resource))]
        public string RawMaterialCode { get; set; }
        [Display(Name = "ItemName", ResourceType = typeof(Resources.Resource))]
        public string RawMaterialName { get; set; }
        [Display(Name = "Quantity", ResourceType = typeof(Resources.Resource))]
        public double Quantity { get; set; }
        [Display(Name = "Cost", ResourceType = typeof(Resources.Resource))]
        public double Cost { get; set; }
        [Display(Name = "Total", ResourceType = typeof(Resources.Resource))]
        public double Total { get; set; }
        public int RawMaterialRowNumber { get; set; }
        [Display(Name = "TotalQuantity", ResourceType = typeof(Resources.Resource))]
        public double TotalQuantity { get; set; }
        public string sTotalQuantity { get; set; }
        [Display(Name = "TotalCost", ResourceType = typeof(Resources.Resource))]
        public double TotalCost { get; set; }
        public string sTotalCost { get; set; }
        [Display(Name = "NetTotal", ResourceType = typeof(Resources.Resource))]
        public double NetTotal { get; set; }
        public string sNetTotal { get; set; }
        //------------------------------------------------------------------------

        //------------------------------------------------------------------------
        public int RelatedItemRowNumber { get; set; }
        [Display(Name = "ItemCode", ResourceType = typeof(Resources.Resource))]
        public string RelatedItemCode { get; set; }
        [Display(Name = "ItemName", ResourceType = typeof(Resources.Resource))]
        public string RelatedItemName { get; set; }
        //------------------------------------------------------------------------

        //------------------------------------------------------------------------
        public int SimilarItemRowNumber { get; set; }
        [Display(Name = "ItemCode", ResourceType = typeof(Resources.Resource))]
        public string SimilarItemCode { get; set; }
        [Display(Name = "ItemName", ResourceType = typeof(Resources.Resource))]
        public string SimilarItemName { get; set; }
        //------------------------------------------------------------------------

        //------------------------------------------------------------------------
        public int SubItemColorRowNumber { get; set; }
        [Display(Name = "ColorNumber", ResourceType = typeof(Resources.Resource))]
        public int SubItemColorCode { get; set; }
        [Display(Name = "Color", ResourceType = typeof(Resources.Resource))]
        public string SubItemColorName { get; set; }
        //------------------------------------------------------------------------
        public string ItemLogo { get; set; }
    }
}