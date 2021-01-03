using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace Acc.Models
{
    public class St_ItemCard
    {
        public Company Company { get; set; }
        [Key]
        [Column(Order = 1)]
        public int CompanyID { get; set; }
        [Key]
        [Column(Order = 2)]
        [Display(Name = "ItemCode", ResourceType = typeof(Resources.Resource))]
        public string ItemCode { get; set; }
        public int ItemSerial { get; set; }
        [Display(Name = "ArabicName", ResourceType = typeof(Resources.Resource))]
        public string ArabicName { get; set; }
        [Display(Name = "EnglishName", ResourceType = typeof(Resources.Resource))]
        public string EnglishName { get; set; }
        [Display(Name = "TaxRate", ResourceType = typeof(Resources.Resource))]
        public double TaxRate { get; set; }
        public int TaxRateNo { get; set; }
        [Display(Name = "TaxType", ResourceType = typeof(Resources.Resource))]
        public int TaxTypeNo { get; set; }
        [Display(Name = "MinimumItem", ResourceType = typeof(Resources.Resource))]
        public double MinimumItemNo { get; set; }
        [Display(Name = "MaximumItem", ResourceType = typeof(Resources.Resource))]
        public double MaximumItemNo { get; set; }
        [Display(Name = "SupplierAccountNo", ResourceType = typeof(Resources.Resource))]
        public string SupplierAccountNumber { get; set; }
        [Display(Name = "ManufacturerCompany", ResourceType = typeof(Resources.Resource))]
        public int ManufacturerCompanyNo { get; set; }
        [Display(Name = "CountryOfOrigin", ResourceType = typeof(Resources.Resource))]
        public int CountryOfOriginNo { get; set; }
        [Display(Name = "LocalCost", ResourceType = typeof(Resources.Resource))]
        public double LocalCost { get; set; }
        [Display(Name = "ForeignCost", ResourceType = typeof(Resources.Resource))]
        public double ForeignCost { get; set; }
        [Display(Name = "SalePrice", ResourceType = typeof(Resources.Resource))]
        public double SalePrice { get; set; }
        [Display(Name = "PointOfSalePrice", ResourceType = typeof(Resources.Resource))]
        public double PointOfSalePrice { get; set; }
        [Display(Name = "CostRate", ResourceType = typeof(Resources.Resource))]
        public double CostRate { get; set; }
        [Display(Name = "MinimumSaleAmount", ResourceType = typeof(Resources.Resource))]
        public double MinimumSaleAmount { get; set; }

        [Display(Name = "TrackAnExpirationDate", ResourceType = typeof(Resources.Resource))]
        public bool TrackAnExpirationDate { get; set; }
        [Display(Name = "TrackSequence", ResourceType = typeof(Resources.Resource))]
        public bool TrackSequence { get; set; }
        [Display(Name = "TrackSequenceUponInput", ResourceType = typeof(Resources.Resource))]
        public bool TrackSequenceUponInput { get; set; }
        [Display(Name = "TrackSequenceUponOutput", ResourceType = typeof(Resources.Resource))]
        public bool TrackSequenceUponOutput { get; set; }
        [Display(Name = "TrackCustoms", ResourceType = typeof(Resources.Resource))]
        public bool TrackCustoms { get; set; }
        [Display(Name = "Smoke", ResourceType = typeof(Resources.Resource))]
        public bool Smoke { get; set; }
        [Display(Name = "ScaleItem", ResourceType = typeof(Resources.Resource))]
        public int ScaleItem { get; set; }
        [Display(Name = "ItemSalesAndPurchases", ResourceType = typeof(Resources.Resource))]
        public int ItemSalesAndPurchases { get; set; }
        [Display(Name = "StopItem", ResourceType = typeof(Resources.Resource))]
        public bool StopItem { get; set; }
        [Display(Name = "StoppingItemFromSelling", ResourceType = typeof(Resources.Resource))]
        public bool StoppingItemFromSelling { get; set; }
        [Display(Name = "StoppingItemFromBuying", ResourceType = typeof(Resources.Resource))]
        public bool StoppingItemFromBuying { get; set; }
        [Display(Name = "StoppingItemFromPointOfSale", ResourceType = typeof(Resources.Resource))]
        public bool StoppingItemFromPointOfSale { get; set; }
        [Display(Name = "ItemServicesWithoutAnInventory", ResourceType = typeof(Resources.Resource))]
        public bool ItemServicesWithoutAnInventory { get; set; }
        [Display(Name = "NotRelatedToTheUnitAbove", ResourceType = typeof(Resources.Resource))]
        public bool NotRelatedToTheUnitAbove { get; set; }
        [Display(Name = "OfferType", ResourceType = typeof(Resources.Resource))]
        public int OfferType { get; set; }
        [Display(Name = "Unit", ResourceType = typeof(Resources.Resource))]
        public int ItemUnitNo { get; set; }
        [Display(Name = "ItemNature", ResourceType = typeof(Resources.Resource))]
        public int ItemNatureNo { get; set; }
        public int Categorie_1 { get; set; }
        public int Categorie_2 { get; set; }
        public int Categorie_3 { get; set; }
        public int Categorie_4 { get; set; }
        public int Categorie_5 { get; set; }
        public int Categorie_6 { get; set; }
        public int Categorie_7 { get; set; }
        public int Categorie_8 { get; set; }
        public int Categorie_9 { get; set; }
        public int Categorie_10 { get; set; }
        public int Categorie_11 { get; set; }
        public int Categorie_12 { get; set; }
        public int Categorie_13 { get; set; }
        public int Categorie_14 { get; set; }
        public int Categorie_15 { get; set; }

        [Display(Name = "CBM", ResourceType = typeof(Resources.Resource))]
        public double CBM { get; set; }
        [Display(Name = "Length", ResourceType = typeof(Resources.Resource))]
        public int LengthNo { get; set; }
        [Display(Name = "Width", ResourceType = typeof(Resources.Resource))]
        public int WidthNo { get; set; }
        [Display(Name = "Height", ResourceType = typeof(Resources.Resource))]
        public int HeightNo { get; set; }
        [Display(Name = "Size", ResourceType = typeof(Resources.Resource))]
        public int SizeNo { get; set; }
        [Display(Name = "Weight", ResourceType = typeof(Resources.Resource))]
        public int WeightNo { get; set; }
        [Display(Name = "Unit", ResourceType = typeof(Resources.Resource))]
        public int UnitNo { get; set; }
        public double CategoriePrice_1 { get; set; }
        public double CategoriePrice_2 { get; set; }
        public double CategoriePrice_3 { get; set; }
        public double CategoriePrice_4 { get; set; }
        public double CategoriePrice_5 { get; set; }
        public double CategoriePrice_6 { get; set; }
        public double CategoriePrice_7 { get; set; }
        public double CategoriePrice_8 { get; set; }
        public double CategoriePrice_9 { get; set; }
        public double CategoriePrice_10 { get; set; }
        public string ItemLogo { get; set; }
        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]
        public string InsUserID { get; set; }
        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime InsDateTime { get; set; }
        public DateTime OpeningDate { get; set; }
        [Display(Name = "TheTargetMonthlyAmount", ResourceType = typeof(Resources.Resource))]
        public int TheTargetMonthlyAmount { get; set; }
    }
}