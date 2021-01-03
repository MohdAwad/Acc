using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class St_WarehouseSearchVM
    {
        public int ItemUnitCode { get; set; }
        public string ItemUnitName { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public double SalePrice { get; set; }
        public double PurchasePrice { get; set; }
        public bool StopItem { get; set; }
        public int ItemUnitNo { get; set; }
        public int ItemNatureNo { get; set; }
        public int ItemCaseInt { get; set; }
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
        public double CostRate { get; set; }
        public string SupplierAccountNumber { get; set; }
        public string SearchSupplierAccountNumber { get; set; }
        public string ItemNatureName { get; set; }
        public string ItemCase { get; set; }
        public string SupplierAccountName { get; set; }
        public string SearchSupplierAccountName { get; set; }
        public int TheDecimalPointForTheLocalCurrency { get; set; }
        public int CountryOfOriginNo { get; set; }
        public int ManufacturerCompanyNo { get; set; }
        public double TaxRate { get; set; }
        public string CountryOfOriginName { get; set; }
        public string ManufacturerCompanyName { get; set; }
        public IEnumerable<St_ItemUnit> St_ItemUnit { get; set; }
        public IEnumerable<St_CountryOfOrigin> St_CountryOfOrigin { get; set; }
        public IEnumerable<St_ManufacturerCompany> St_ManufacturerCompany { get; set; }
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

    }
}