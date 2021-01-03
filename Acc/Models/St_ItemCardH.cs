using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class St_ItemCardH
    {
        public Company Company { get; set; }
        [Key]
        [Column(Order = 1)]
        public int CompanyID { get; set; }
        [Key]
        [Column(Order = 2)]
        [Display(Name = "GroupCode", ResourceType = typeof(Resources.Resource))]
        public string GroupCode { get; set; }
        [Key]
        [Column(Order = 3)]
        [Display(Name = "ItemCode", ResourceType = typeof(Resources.Resource))]
        public string ItemCode { get; set; }
        public int ItemSerial { get; set; }
        [Display(Name = "ArabicName", ResourceType = typeof(Resources.Resource))]
        public string ArabicName { get; set; }
        [Display(Name = "EnglishName", ResourceType = typeof(Resources.Resource))]
        public string EnglishName { get; set; }
        [Display(Name = "NumberOfPieces", ResourceType = typeof(Resources.Resource))]
        public int NumberOfPieces { get; set; }
        [Display(Name = "CBM", ResourceType = typeof(Resources.Resource))]
        public double CBM { get; set; }
        [Display(Name = "Unit", ResourceType = typeof(Resources.Resource))]
        public int ItemUnitNo { get; set; }
        [Display(Name = "SupplierAccountNo", ResourceType = typeof(Resources.Resource))]
        public string SupplierAccountNumber { get; set; }
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
        [Display(Name = "FactoryName", ResourceType = typeof(Resources.Resource))]
        public int FactoryNo { get; set; }
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
        [Display(Name = "TotalQuantity", ResourceType = typeof(Resources.Resource))]
        public double TotalQuantity { get; set; }
        [Display(Name = "TotalCost", ResourceType = typeof(Resources.Resource))]
        public double TotalCost { get; set; }
        [Display(Name = "NetTotal", ResourceType = typeof(Resources.Resource))]
        public double NetTotal { get; set; }
        
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
        [Display(Name = "TheTargetMonthlyAmount", ResourceType = typeof(Resources.Resource))]
        public int TheTargetMonthlyAmount { get; set; }

        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]
        public string InsUserID { get; set; }
        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime InsDateTime { get; set; }
        public DateTime OpeningDate { get; set; }
        [Display(Name = "CostRate", ResourceType = typeof(Resources.Resource))]
        public double CostRate { get; set; }
        [Display(Name = "ReOrderLimit", ResourceType = typeof(Resources.Resource))]
        public int ReOrderLimit { get; set; }
        [Display(Name = "OrderQuantity", ResourceType = typeof(Resources.Resource))]
        public int OrderQuantity { get; set; }
        
        public string ItemLogo { get; set; }
    }
}