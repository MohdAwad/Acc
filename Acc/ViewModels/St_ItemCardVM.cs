using Acc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace Acc.ViewModels
{
    public class St_ItemCardVM
    {
        public IEnumerable<St_ItemWarehouse> St_ItemWarehouse { get; set; }
        public IEnumerable<St_Warehouse> St_Warehouse { get; set; }
        public IEnumerable<St_SimilarItem> St_SimilarItem { get; set; }
        public IEnumerable<St_AlternativeItem> St_AlternativeItem { get; set; }
        public IEnumerable<St_ItemOtherUnit> St_ItemOtherUnit { get; set; }
        public IEnumerable<St_ItemOffer> St_ItemOffer { get; set; }
        public IEnumerable<St_AdvertisingRepresentative> St_AdvertisingRepresentative { get; set; }
        public IEnumerable<St_ItemAdvertisingRepresentative> St_ItemAdvertisingRepresentative { get; set; }
        public string UserName { get; set; }
        public int TheDecimalPointForTheLocalCurrency { get; set; }
        public int iRowTable { get; set; }
        public string SearchStockCode { get; set; }
        public int SearchAdvertisingRepresentativeID { get; set; }
        public string UpdateItemCode { get; set; }
        public double Quantity { get; set; }
        public double TotalLocal { get; set; }
        public double PricePieceTotalLocal { get; set; }
        //------------------------------------------------------------------------
        public int CompanyID { get; set; }
        [Display(Name = "ItemCode", ResourceType = typeof(Resources.Resource))]
        public string ItemCode { get; set; }
        public int ItemSerial { get; set; }
        [Display(Name = "ArabicName", ResourceType = typeof(Resources.Resource))]
        public string ArabicName { get; set; }
        [Display(Name = "EnglishName", ResourceType = typeof(Resources.Resource))]
        public string EnglishName { get; set; }
        [Display(Name = "ItemName", ResourceType = typeof(Resources.Resource))]
        public string ItemName { get; set; }
        [Display(Name = "TaxRate", ResourceType = typeof(Resources.Resource))]
        public double TaxRate { get; set; }
        public int TaxRateNo { get; set; }
        [Display(Name = "TaxType", ResourceType = typeof(Resources.Resource))]
        public int TaxTypeNo { get; set; }
        [Display(Name = "MinimumItem", ResourceType = typeof(Resources.Resource))]
        public double MinimumItemNo { get; set; }
        [Display(Name = "MaximumItem", ResourceType = typeof(Resources.Resource))]
        public double MaximumItemNo { get; set; }
        [Display(Name = "QuantityAvailable", ResourceType = typeof(Resources.Resource))]
        public double QuantityAvailable { get; set; }
        [Display(Name = "SupplierAccountNo", ResourceType = typeof(Resources.Resource))]
        public string SupplierAccountNumber { get; set; }
        public string SupplierAccountName { get; set; }
        [Display(Name = "ManufacturerCompany", ResourceType = typeof(Resources.Resource))]
        public IEnumerable<St_ManufacturerCompany> St_ManufacturerCompany { get; set; }
        public string ManufacturerCompanyName { get; set; }
        public int ManufacturerCompanyNo { get; set; }
        [Display(Name = "CountryOfOrigin", ResourceType = typeof(Resources.Resource))]
        public int CountryOfOriginNo { get; set; }
        public string CountryOfOriginName { get; set; }
        public IEnumerable<St_CountryOfOrigin> St_CountryOfOrigin { get; set; }
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
        [Display(Name = "LastLocalPurchasePrice", ResourceType = typeof(Resources.Resource))]
        public double LastLocalPurchasePrice { get; set; }
        [Display(Name = "LastForeignPurchasePrice", ResourceType = typeof(Resources.Resource))]
        public double LastForeignPurchasePrice { get; set; }
        [Display(Name = "MinimumSaleAmount", ResourceType = typeof(Resources.Resource))]
        public double MinimumSaleAmount { get; set; }
        [Display(Name = "TotalQuantitySold", ResourceType = typeof(Resources.Resource))]
        public double TotalQuantitySold { get; set; }
        [Display(Name = "TotalValueSold", ResourceType = typeof(Resources.Resource))]
        public double TotalValueSold { get; set; }
        [Display(Name = "TheNumberOfDaysTheCardIsOpened", ResourceType = typeof(Resources.Resource))]
        public int TheNumberOfDaysTheCardIsOpened { get; set; }

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
        [Display(Name = "ItemCase", ResourceType = typeof(Resources.Resource))]
        public int ItemCaseInt { get; set; }
        public string ItemCase { get; set; }
        [Display(Name = "ItemNature", ResourceType = typeof(Resources.Resource))]
        public int ItemNatureNo { get; set; }
        public string ItemNatureName { get; set; }
        [Display(Name = "Unit", ResourceType = typeof(Resources.Resource))]
        public IEnumerable<St_ItemUnit> St_ItemUnit { get; set; }
        public int ItemUnitNo { get; set; }
        public string ItemUnitName { get; set; }
        public IEnumerable<St_DescriptionDetail> St_DescriptionDetail1 { get; set; }
        public IEnumerable<St_DescriptionDetail> St_DescriptionDetail2 { get; set; }
        public IEnumerable<St_DescriptionDetail> St_DescriptionDetail3 { get; set; }
        public IEnumerable<St_DescriptionDetail> St_DescriptionDetail4 { get; set; }
        public IEnumerable<St_DescriptionDetail> St_DescriptionDetail5 { get; set; }
        public IEnumerable<St_DescriptionDetail> St_DescriptionDetail6 { get; set; }
        public IEnumerable<St_DescriptionDetail> St_DescriptionDetail7 { get; set; }
        public IEnumerable<St_DescriptionDetail> St_DescriptionDetail8 { get; set; }
        public IEnumerable<St_DescriptionDetail> St_DescriptionDetail9 { get; set; }
        public IEnumerable<St_DescriptionDetail> St_DescriptionDetail10 { get; set; }
        public IEnumerable<St_DescriptionDetail> St_DescriptionDetail11 { get; set; }
        public IEnumerable<St_DescriptionDetail> St_DescriptionDetail12 { get; set; }
        public IEnumerable<St_DescriptionDetail> St_DescriptionDetail13 { get; set; }
        public IEnumerable<St_DescriptionDetail> St_DescriptionDetail14 { get; set; }
        public IEnumerable<St_DescriptionDetail> St_DescriptionDetail15 { get; set; }
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
        public string Categorie_1Name { get; set; }
        public string Categorie_2Name { get; set; }
        public string Categorie_3Name { get; set; }
        public string Categorie_4Name { get; set; }
        public string Categorie_5Name { get; set; }
        public string Categorie_6Name { get; set; }
        public string Categorie_7Name { get; set; }
        public string Categorie_8Name { get; set; }
        public string Categorie_9Name { get; set; }
        public string Categorie_10Name { get; set; }
        public string Categorie_11Name { get; set; }
        public string Categorie_12Name { get; set; }
        public string Categorie_13Name { get; set; }
        public string Categorie_14Name { get; set; }
        public string Categorie_15Name { get; set; }

        [Display(Name = "CBM", ResourceType = typeof(Resources.Resource))]
        public double CBM { get; set; }
        [Display(Name = "Length", ResourceType = typeof(Resources.Resource))]
        public IEnumerable<St_MeasurementDetail> St_MeasurementDetailLength { get; set; }
        public int LengthNo { get; set; }
        [Display(Name = "Width", ResourceType = typeof(Resources.Resource))]
        public IEnumerable<St_MeasurementDetail> St_MeasurementDetailWidth { get; set; }
        public int WidthNo { get; set; }
        [Display(Name = "Height", ResourceType = typeof(Resources.Resource))]
        public IEnumerable<St_MeasurementDetail> St_MeasurementDetailHeight { get; set; }
        public int HeightNo { get; set; }
        [Display(Name = "Size", ResourceType = typeof(Resources.Resource))]
        public IEnumerable<St_MeasurementDetail> St_MeasurementDetailSize { get; set; }
        public int SizeNo { get; set; }
        [Display(Name = "Weight", ResourceType = typeof(Resources.Resource))]
        public IEnumerable<St_MeasurementDetail> St_MeasurementDetailWeight { get; set; }
        public int WeightNo { get; set; }
        [Display(Name = "Unit", ResourceType = typeof(Resources.Resource))]
        public IEnumerable<St_MeasurementDetail> St_MeasurementDetailUnit { get; set; }
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
        public string CategoriePrice_1Name { get; set; }
        public string CategoriePrice_2Name { get; set; }
        public string CategoriePrice_3Name { get; set; }
        public string CategoriePrice_4Name { get; set; }
        public string CategoriePrice_5Name { get; set; }
        public string CategoriePrice_6Name { get; set; }
        public string CategoriePrice_7Name { get; set; }
        public string CategoriePrice_8Name { get; set; }
        public string CategoriePrice_9Name { get; set; }
        public string CategoriePrice_10Name { get; set; }
        public string ItemLogo { get; set; }
        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]
        public string InsUserID { get; set; }
        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime InsDateTime { get; set; }
        public DateTime OpeningDate { get; set; }
        [Display(Name = "TheTargetMonthlyAmount", ResourceType = typeof(Resources.Resource))]
        public int TheTargetMonthlyAmount { get; set; }
        //------------------------------------------------------------------------
        //------------------------------------------------------------------------
        [Display(Name = "StockCode", ResourceType = typeof(Resources.Resource))]
        public string StockCode { get; set; }
        [Display(Name = "StockName", ResourceType = typeof(Resources.Resource))]
        public string StockName { get; set; }
        public int ItemWarehouseRowNumber { get; set; }
        [Display(Name = "MinimumItem", ResourceType = typeof(Resources.Resource))]
        public double StockMinimumItemNo { get; set; }
        [Display(Name = "MaximumItem", ResourceType = typeof(Resources.Resource))]
        public double StockMaximumItemNo { get; set; }
        //------------------------------------------------------------------------
        //------------------------------------------------------------------------
        public int AlternativeItemRowNumber { get; set; }
        [Display(Name = "ItemCode", ResourceType = typeof(Resources.Resource))]
        public string AlternativeItemCode { get; set; }
        [Display(Name = "ItemName", ResourceType = typeof(Resources.Resource))]
        public string AlternativeItemName { get; set; }
        [Display(Name = "SalePrice", ResourceType = typeof(Resources.Resource))]
        public double AlternativeSalePrice { get; set; }
        [Display(Name = "Quantity", ResourceType = typeof(Resources.Resource))]
        public double AlternativeQuantity { get; set; }
        [Display(Name = "CountryOfOrigin", ResourceType = typeof(Resources.Resource))]
        public string AlternativeCountryOfOrigin { get; set; }
        [Display(Name = "ManufacturerCompany", ResourceType = typeof(Resources.Resource))]
        public string AlternativeManufacturerCompany { get; set; }
        [Display(Name = "Unit", ResourceType = typeof(Resources.Resource))]
        public string AlternativeUnit { get; set; }
        //------------------------------------------------------------------------
        //------------------------------------------------------------------------
        public int SimilarItemRowNumber { get; set; }
        [Display(Name = "ItemCode", ResourceType = typeof(Resources.Resource))]
        public string SimilarItemCode { get; set; }
        [Display(Name = "UnitName", ResourceType = typeof(Resources.Resource))]
        public string SimilarUnitName { get; set; }
        public int SimilarItemType { get; set; }
        public int OldSimilarItemRowNumber { get; set; }
        //------------------------------------------------------------------------
        //------------------------------------------------------------------------
        public int OtherUnitRowNumber { get; set; }
        [Display(Name = "Unit", ResourceType = typeof(Resources.Resource))]
        public int OtherUnitNumber { get; set; }
        [Display(Name = "UnitName", ResourceType = typeof(Resources.Resource))]
        public string OtherUnitName { get; set; }
        [Display(Name = "Quantity", ResourceType = typeof(Resources.Resource))]
        public double OtherUnitQuantity { get; set; }
        [Display(Name = "SalePrice", ResourceType = typeof(Resources.Resource))]
        public double OtherUnitSalePrice { get; set; }
        [Display(Name = "PurchasePrice", ResourceType = typeof(Resources.Resource))]
        public double OtherUnitPurchasePrice { get; set; }
        [Display(Name = "Barcode", ResourceType = typeof(Resources.Resource))]
        public string OtherUnitBarcode { get; set; }
        public int OldOtherUnitRowNumber { get; set; }
        //------------------------------------------------------------------------
        //------------------------------------------------------------------------
        public int ItemOfferRowNumber { get; set; }
        [Display(Name = "QuantitySold", ResourceType = typeof(Resources.Resource))]
        public double ItemOfferQuantitySold { get; set; }
        [Display(Name = "Bonus", ResourceType = typeof(Resources.Resource))]
        public double ItemOfferBonus { get; set; }
        [Display(Name = "UnitPrice", ResourceType = typeof(Resources.Resource))]
        public double ItemOfferUnitPrice { get; set; }
        [Display(Name = "TotalPrice", ResourceType = typeof(Resources.Resource))]
        public double ItemOfferTotalPrice { get; set; }
        //------------------------------------------------------------------------
        //------------------------------------------------------------------------
        [Display(Name = "AdvertisingRepresentative", ResourceType = typeof(Resources.Resource))]
        public int AdvertisingRepresentativeID { get; set; }
        [Display(Name = "AdvertisingRepresentativeName", ResourceType = typeof(Resources.Resource))]
        public string AdvertisingRepresentativeName { get; set; }
        public int ItemAdvertisingRepresentativeRowNumber { get; set; }
        //------------------------------------------------------------------------
    }
}