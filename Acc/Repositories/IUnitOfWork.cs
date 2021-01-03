namespace Acc.Repositories
{
    public interface IUnitOfWork
    {
        //--------------- Account ---------------------------
        ICompanyRepo Company { get; }
        IUserRepo User { get; }
        IAccountTypeRepo AccountType { get; }
        IChartOfAccountRepo ChartOfAccount { get; }
        INativeSqlRepo NativeSql { get; }
        ICurrencyRepo Currency { get; }
        ICostCenterRepo CostCenter { get; }
        ICustomerInformationRepo CustomerInformation { get; }
        ISupplierInformationRepo SupplierInformation { get; }
        IAccreditationInformationRepo AccreditationInformation { get; }
        ISaleRepo Sale { get; }
        ITransactionKindRepo TransactionKind { get; }
        ICompanyTransactionKindRepo CompanyTransactionKind { get; }
        IServiceGroupRepo ServiceGroup { get; }
        IServiceRepo Service { get; }
        ICurrencyValueRepo CurrencyValue { get; }
        IHeaderRepo  Header { get; }
        ITempPrepaidAndRevenueReceivedRepo TempPrepaidAndRevenueReceived { get; }
        ITempPrepaidAndRevenueReceivedDetailsRepo TempPrepaidAndRevenueReceivedDetail { get; }
        ITransactionRepo  Transaction { get; }
        IDefinitionBankRepo DefinitionBank { get; }
        IBankGuaranteeRepo BankGuarantee { get; }
        IPaperRepo Paper { get; }
        IDefinitionOtherAccountRepo DefinitionOtherAccount { get; }
        IServiceBillHeaderRepo  ServiceBillHeader { get; }
        ITransActionServiceBillRepo TransActionServiceBill { get; }
        IEstimatedBudgetRepo EstimatedBudget { get; }
        IAssetTypeRepo AssetType { get; }
        IAssetRepo Asset { get; }
        IAssetHeaderRepo AssetHeader { get; }
        IAssetTransActionRepo  AssetTransAction  { get; }
        IDrawnBankRepo DrawnBank { get; }
        IAssetSaleConsumRepo AssetSaleConsum { get; }
        IAssetMaintenanceRepo AssetMaintenance { get; }
        IAssetTrusteeRepo AssetTrustee { get; }
        IAssetSiteRepo AssetSite { get; }
        IAssetSectionRepo AssetSection { get; }
        IAssetCircleRepo AssetCircle { get; }
        IAssetAdministrationRepo AssetAdministration { get; }
        IDefinitionAssetSiteRepo DefinitionAssetSite { get; }
        ICustomerCityRepo CustomerCity { get; }
        ICustomerAreaRepo CustomerArea { get; }
        ISupplierBankRepo SupplierBank { get; }
        ISupplierCountryRepo SupplierCountry { get; }
        ISupplierCountryBankRepo SupplierCountryBank { get; }
        ISupplierCityRepo SupplierCity { get; }
        ISupplierCityBankRepo SupplierCityBank { get; }
        ISupplierBranchBankRepo SupplierBranchBank { get; }
        IFreezeTransactionRepo FreezeTransaction { get; }
        ICompanyTransactionKindMonthlySerialRepo CompanyTransactionKindMonthlySerial { get;}
        ITransActionFileRepo TransActionFile { get; set; }
        IFavScreenRepo FavScreen { get; }

        //--------------- Warehouse ---------------------------
        ISt_WarehouseRepo St_Warehouse { get; }
        ISt_WarehouseHRepo St_WarehouseH { get; }
        ISt_FactoryHRepo St_FactoryH { get; }
        ISt_SectionsOfFactoryHRepo St_SectionsOfFactoryH { get; }
        ISt_ItemGroupHRepo St_ItemGroupH { get; }
        ISt_ItemUnitHRepo St_ItemUnitH { get; }
        ISt_SubItemColorHRepo St_SubItemColorH { get; }
        ISt_BranchHRepo St_BranchH { get; }
        ISt_CompanyTransactionKindRepo St_CompanyTransactionKind { get; }
        ISt_CompanyTransactionKindSymbolSerialRepo St_CompanyTransactionKindSymbolSerial { get; }
        ISt_CompanyTransactionKindHRepo St_CompanyTransactionKindH { get; }
        ISt_CompanyTransactionKindSymbolSerialHRepo St_CompanyTransactionKindSymbolSerialH { get; }
        ISt_ItemCardHRepo St_ItemCardH { get; }
        ISt_ItemGallaryRepo  St_ItemGallary  { get; }
        ISt_DescriptionRepo  St_Description  { get; }
        ISt_DescriptionDetailRepo St_DescriptionDetail { get; }
        ISt_CategoryPriceRepo St_CategoryPrice { get; }
        ISt_MeasurementDetailRepo St_MeasurementDetail { get; }
        ISt_TransactionKindRepo St_TransactionKind { get; }
        ISt_FundingAgencyHRepo St_FundingAgencyH { get; }
        ISt_WarehouseAccountRepo St_WarehouseAccount { get; }
        ISt_AdvertisingRepresentativeRepo St_AdvertisingRepresentative { get; }
        ISt_ManufacturerCompanyRepo St_ManufacturerCompany { get; }
        ISt_CountryOfOriginRepo St_CountryOfOrigin { get; }
        ISt_BankHRepo St_BankH { get; }
        ISt_OtherAccountRepo St_OtherAccount { get; }
        ISt_OtherAccountHRepo St_OtherAccountH { get; }
        ISt_CarpenterHRepo St_CarpenterH { get; }
        ISt_SalesManHRepo St_SalesManH { get; }
        ISt_ItemUnitRepo St_ItemUnit { get; }
        ISt_BranchAccountHRepo St_BranchAccountH { get; }
        ISt_ItemCardRepo St_ItemCard { get; }
        ISt_DriverRepo St_Driver { get; }
        ISt_DelegateReceivingHRepo St_DelegateReceivingH { get; }
        ISt_ExtraExpenseRepo St_ExtraExpense { get; }
        ISt_HeaderRepo St_Header { get; }
        ISt_PurchaseOrderRepo St_PurchaseOrder { get; }
        ISt_TransactionKindHRepo St_TransactionKindH { get; }
        ISt_OfferPriceRepo St_OfferPrice { get; }
        ISt_SalesOrderRepo St_SalesOrder { get; }

        ISt_TransActionFileRepo St_TransActionFile { get; set; }

        void Complete();
    }
}