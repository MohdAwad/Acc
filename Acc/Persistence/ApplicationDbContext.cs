using Acc.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Acc.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        //--------------- Account ---------------------------
        public DbSet<Company> Companys { get; set; }
        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<ChartOfAccount> ChartOfAccounts { get; set; }
        public DbSet<Currency> Currencys { get; set; }
        public DbSet<CurrencyValue> CurrencyValues { get; set; }
        public DbSet<CustomerArea> CustomerAreas { get; set; }
        public DbSet<CustomerCity> CustomerCitys { get; set; }
        public DbSet<CustomerInformation> CustomerInformations { get; set; }
        public DbSet<SupplierBank> SupplierBanks { get; set; }
        public DbSet<SupplierBranchBank> SupplierBranchBanks { get; set; }
        public DbSet<SupplierCity> SupplierCitys { get; set; }
        public DbSet<SupplierCityBank> SupplierCityBanks { get; set; }
        public DbSet<SupplierCountry> SupplierCountrys { get; set; }
        public DbSet<SupplierCountryBank> SupplierCountryBanks { get; set; }
        public DbSet<SupplierInformation> SupplierInformation { get; set; }
        public DbSet<CostCenter> CostCenters { get; set; }
        public DbSet<AccreditationInformation> AccreditationInformations { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<TransactionKind> TransactionKinds { get; set; }
        public DbSet<CompanyTransactionKind> CompanyTransactionKinds { get; set; }
        public DbSet<ServiceGroup> ServiceGroups { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Header> Headers { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<DefinitionBank> DefinitionBanks { get; set; }
        public DbSet<PapersCase> PapersCases { get; set; }
        public DbSet<DrawnBank> DrawnBanks { get; set; }
        public DbSet<TempPrepaidAndRevenueReceived> TempPrepaidAndRevenueReceiveds { get; set; }
        public DbSet<TempPrepaidAndRevenueReceivedDetail> TempPrepaidAndRevenueReceivedDetails { get; set; }
        public DbSet<Paper> Papers { get; set; }
        public DbSet<TracingPaper> TracingPapers { get; set; }
        public DbSet<BankGuarantee> BankGuarantees { get; set; }
        public DbSet<OtherAccount> OtherAccounts { get; set; }
        public DbSet<UserActiveYear> UserActiveYears { get; set; }
        public DbSet<SupplierType> SupplierTypes { get; set; }
        public DbSet<HeaderServiceBill> HeaderServiceBills { get; set; }
        public DbSet<TransActionServiceBill> TransActionServiceBills { get; set; }
        public DbSet<EstimatedBudget> EstimatedBudgets { get; set; }
        public DbSet<AssetType> AssetTypes { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<AssetTransAction> AssetTransActions { get; set; }
        public DbSet<AssetHeader> AssetHeaders { get; set; }
        public DbSet<AssetSaleConsum> AssetSaleConsums { get; set; }
        public DbSet<AssetMaintenance> AssetMaintenances { get; set; }
        public DbSet<AssetTrustee> AssetTrustees { get; set; }
        public DbSet<AssetSite> AssetSites { get; set; }
        public DbSet<AssetSection> AssetSections { get; set; }
        public DbSet<AssetCircle> AssetCircles { get; set; }
        public DbSet<AssetAdministration> AssetAdministrations { get; set; }
        public DbSet<DefinitionAssetSite> DefinitionAssetSites { get; set; }
        public DbSet<FreezeTransaction> FreezeTransactions { get; set; }
        public DbSet<MonthName> MonthNames { get; set; }  
        public DbSet<CompanyTransactionKindMonthlySerial> CompanyTransactionKindMonthlySerials { get; set; }
        public DbSet<TransActionFile> TransActionFiles { get; set; }
        public DbSet<FavScreen> FavScreens { get; set; }
        public DbSet<BillCase> BillCases { get; set; }


        public DbSet<AccYear> AccYears { get; set; }
        

        //--------------- Warehouse ---------------------------
        public DbSet<St_Warehouse> St_Warehouses { get; set; }
        public DbSet<St_WarehouseH> St_WarehouseHs { get; set; }
        public DbSet<St_FactoryH> St_FactoryHs { get; set; }
        public DbSet<St_SectionsOfFactoryH> St_SectionsOfFactoryHs { get; set; }
        public DbSet<St_BranchH> St_BrancheHs { get; set; }
        public DbSet<St_ItemGroupH> St_ItemGroupHs { get; set; }
        public DbSet<St_ItemUnitH> St_ItemUnitHs { get; set; }
        public DbSet<St_ItemUnit> St_ItemUnits { get; set; }
        public DbSet<St_Register> St_Registers { get; set; }
        public DbSet<St_SubItemColorH> St_SubItemColorHs { get; set; }
        public DbSet<St_TransactionKind> St_TransactionKinds { get; set; }
        public DbSet<St_CompanyTransactionKind> St_CompanyTransactionKinds { get; set; }
        public DbSet<St_CompanyTransactionKindSymbolSerial> St_CompanyTransactionKindSymbolSerials { get; set; }
        public DbSet<St_TransactionKindH> St_TransactionKindHs { get; set; }
        public DbSet<St_CompanyTransactionKindH> St_CompanyTransactionKindHs { get; set; }
        public DbSet<St_CompanyTransactionKindSymbolSerialH> St_CompanyTransactionKindSymbolSerialHs { get; set; }
        public DbSet<St_ItemCardH> St_ItemCradHs { get; set; }
        public DbSet<St_RelatedItemH> St_RelatedItemHs { get; set; }
        public DbSet<St_SimilarItemH> St_SimilarItemHs { get; set; }
        public DbSet<St_SubColorsItemH> St_SubColorsItemHs { get; set; }
        public DbSet<St_ManufacturingStageH> St_ManufacturingStageHs { get; set; }
        public DbSet<St_RawMaterialH> St_RawMaterialHs { get; set; }
        public DbSet<St_ItemWarehouseH> St_ItemWarehouseHs { get; set; }
        public DbSet<St_ItemGallary> St_ItemGallarys { get; set; }
        public DbSet<St_HeaderH> St_HeaderHs { get; set; }
        public DbSet<St_TransactionH> St_TransactionHs { get; set; }
        public DbSet<St_Description> St_Descriptions { get; set; }
        public DbSet<St_DescriptionDetail> St_DescriptionDetails { get; set; }
        public DbSet<St_Measurement> St_Measurements { get; set; }
        public DbSet<St_CategoryPrice> St_CategoryPrices { get; set; }
        public DbSet<St_MeasurementDetail> St_MeasurementDetails { get; set; }
        public DbSet<St_FundingAgencyH> St_FundingAgencyHs { get; set; }
        public DbSet<St_WarehouseAccount> St_WarehouseAccounts { get; set; }
        public DbSet<St_AdvertisingRepresentative> St_AdvertisingRepresentatives { get; set; }
        public DbSet<St_ManufacturerCompany> St_ManufacturerCompanies { get; set; }
        public DbSet<St_CountryOfOrigin> St_CountryOfOrigins { get; set; }
        public DbSet<St_OtherAccount> St_OtherAccounts { get; set; }
        public DbSet<St_OtherAccountH> St_OtherAccountHs { get; set; }
        public DbSet<St_BankH> St_BankHs { get; set; }
        public DbSet<St_CarpenterH> St_Carpenters { get; set; }
        public DbSet<St_SalesManH> St_SalesManHs { get; set; }
        public DbSet<St_BranchAccountH> St_BranchAccountHs { get; set; }
        public DbSet<St_BranchExpenseAccountH> St_BranchExpenseAccountHs { get; set; }
        public DbSet<St_BranchOtherExpenseAccountH> St_BranchOtherExpenseAccountHs { get; set; }
        public DbSet<St_ItemCard> St_ItemCards { get; set; }
        public DbSet<St_AlternativeItem> St_AlternativeItems { get; set; }
        public DbSet<St_SimilarItem> St_SimilarItems { get; set; }
        public DbSet<St_ItemOtherUnit> St_ItemOtherUnits { get; set; }
        public DbSet<St_ItemOffer> St_ItemOffers { get; set; }
        public DbSet<St_ItemWarehouse> St_ItemWarehouses { get; set; }
        public DbSet<St_Driver> St_Drivers { get; set; }
        public DbSet<St_DelegateReceivingH> St_DelegateReceivingHs { get; set; }
        public DbSet<St_Header> St_Headers { get; set; }
        public DbSet<St_Transaction> St_Transactions { get; set; }
        public DbSet<St_ExtraExpense> St_ExtraExpenses { get; set; }
        public DbSet<St_ItemAdvertisingRepresentative> St_ItemAdvertisingRepresentatives { get; set; }
        public DbSet<St_PurchaseOrderHeader> St_PurchaseOrderHeaders { get; set; }
        public DbSet<St_PurchaseOrderTransaction> St_PurchaseOrderTransactions { get; set; }
        public DbSet<St_OfferPriceHeader> St_OfferPriceHeaders { get; set; }
        public DbSet<St_OfferPriceTransaction> St_OfferPriceTransactions { get; set; }
        public DbSet<St_SalesOrderHeader> St_SalesOrderHeaders { get; set; }
        public DbSet<St_SalesOrderTransaction> St_SalesOrderTransactions { get; set; }
        public DbSet<St_TransActionFile> St_TransActionFiles { get; set; }
        public DbSet<St_TempCalculateAverageCostH> St_TempCalculateAverageCostHs { get; set; }
        public DbSet<St_TempCalculateAverageCost> St_TempCalculateAverageCosts { get; set; }

        //--------------- Orders ---------------------------
        public DbSet<O_HeaderH> O_HeaderHs { get; set; }
        public DbSet<O_TransactionH> O_TransactionHs { get; set; }
        public DbSet<O_DailyCloseH> O_DailyCloseHs { get; set; }
        public DbSet<O_HeaderHistoryH> O_HeaderHistoryHs { get; set; }
        public DbSet<O_TransactionHistoryH> O_TransactionHistoryHs { get; set; }
        public DbSet<O_PaperH> O_PaperHs { get; set; }
        public DbSet<O_PaperHistoryH> O_PaperHistoryHs { get; set; }
        public DbSet<O_ReceiptBillHeaderH> O_ReceiptBillHeaderHs { get; set; }
        public DbSet<O_ReceiptBillTransactionH> O_ReceiptBillTransactionHs { get; set; }
        public DbSet<O_ReceiptBillHeaderHistoryH> O_ReceiptBillHeaderHistoryHs { get; set; }
        public DbSet<O_ReceiptBillTransactionHistoryH> O_ReceiptBillTransactionHistoryHs { get; set; }
        public DbSet<O_PurchaseOrderHeaderH> O_PurchaseOrderHeaderHs { get; set; }
        public DbSet<O_PurchaseOrderTransactionH> O_PurchaseOrderTransactionHs { get; set; }
        public DbSet<O_PurchaseOrderHeaderHistoryH> O_PurchaseOrderHeaderHistoryHs { get; set; }
        public DbSet<O_PurchaseOrderTransactionHistoryH> O_PurchaseOrderTransactionHistoryHs { get; set; }
       
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
