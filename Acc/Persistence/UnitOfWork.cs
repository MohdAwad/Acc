using Acc.Models;
using Acc.Repositories;

namespace Acc.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        //--------------- Account ---------------------------
        private readonly ApplicationDbContext _context;
        public ICompanyRepo Company { get; set; }
        public IUserRepo User { get; set; }
        public IAccountTypeRepo AccountType { get; set; }
        public IChartOfAccountRepo ChartOfAccount { get; set; }
        public INativeSqlRepo NativeSql { get; set; }
        public ICurrencyRepo Currency { get; set; }
        public ICostCenterRepo CostCenter { get; set; }
        public ICustomerInformationRepo CustomerInformation { get; set; }
        public ISupplierInformationRepo SupplierInformation { get; set; }
        public IAccreditationInformationRepo AccreditationInformation { get; set; }
        public ISaleRepo Sale { get; set; }
        public ITransactionKindRepo TransactionKind { get; set; }
        public ICompanyTransactionKindRepo CompanyTransactionKind { get; set; }
        public IServiceGroupRepo ServiceGroup { get; set; }
        public IServiceRepo Service { get; set; }
        public ITransactionRepo Transaction { get; set; }
        public IHeaderRepo Header { get; set; }
        public ITempPrepaidAndRevenueReceivedRepo TempPrepaidAndRevenueReceived { get; set; }
        public ITempPrepaidAndRevenueReceivedDetailsRepo TempPrepaidAndRevenueReceivedDetail { get; set; }
        public ICurrencyValueRepo CurrencyValue { get; set; }
        public IDefinitionBankRepo DefinitionBank { get; set; }
        public IBankGuaranteeRepo BankGuarantee { get; set; }
        public IPaperRepo Paper { get; set; }
        public IDefinitionOtherAccountRepo DefinitionOtherAccount { get; set; }
        public IServiceBillHeaderRepo ServiceBillHeader { get; set; }
        public ITransActionServiceBillRepo  TransActionServiceBill  { get; set; }
        public IEstimatedBudgetRepo  EstimatedBudget { get; set; }
        public IAssetTypeRepo AssetType  { get; set; }
        public IAssetRepo Asset { get; set; }
        public IAssetHeaderRepo AssetHeader  { get; set; }
        public IDrawnBankRepo DrawnBank { get; set; }
        public IAssetTransActionRepo AssetTransAction { get; set; }
        public IAssetSaleConsumRepo AssetSaleConsum { get; set; }
        public IAssetMaintenanceRepo AssetMaintenance { get; set; }
        public IAssetTrusteeRepo AssetTrustee { get; set; }
        public IAssetSiteRepo AssetSite { get; set; }
        public IAssetCircleRepo AssetCircle { get; set; }
        public IAssetSectionRepo AssetSection { get; set; }
        public IAssetAdministrationRepo AssetAdministration { get; set; }
        public IDefinitionAssetSiteRepo DefinitionAssetSite { get; set; }
        public ICustomerCityRepo CustomerCity { get; set; }
        public ICustomerAreaRepo CustomerArea { get; set; }
        public ISupplierBankRepo SupplierBank { get; set; }
        public ISupplierCountryRepo SupplierCountry { get; set; }
        public ISupplierCountryBankRepo SupplierCountryBank { get; set; }
        public ISupplierCityRepo SupplierCity { get; set; }
        public ISupplierCityBankRepo SupplierCityBank { get; set; }
        public ISupplierBranchBankRepo SupplierBranchBank { get; set; }
        public IFreezeTransactionRepo FreezeTransaction { get; set; }
        public ICompanyTransactionKindMonthlySerialRepo CompanyTransactionKindMonthlySerial { get; set; }
        public IFavScreenRepo FavScreen { get; set; }
        public ITransActionFileRepo TransActionFile { get; set; }

        //--------------- Warehouse ---------------------------
        public ISt_WarehouseRepo St_Warehouse { get; set; }
        public ISt_WarehouseHRepo St_WarehouseH { get; set; }
        public ISt_FactoryHRepo St_FactoryH { get; set; }
        public ISt_SectionsOfFactoryHRepo St_SectionsOfFactoryH { get; set; }
        public ISt_ItemGroupHRepo St_ItemGroupH { get; set; }
        public ISt_SubItemColorHRepo St_SubItemColorH { get; set; }
        public ISt_ItemUnitHRepo St_ItemUnitH { get; set; }
        public ISt_BranchHRepo St_BranchH { get; set; }
        public ISt_CompanyTransactionKindSymbolSerialRepo St_CompanyTransactionKindSymbolSerial { get; set; }
        public ISt_CompanyTransactionKindRepo St_CompanyTransactionKind { get; set; }
        public ISt_CompanyTransactionKindSymbolSerialHRepo St_CompanyTransactionKindSymbolSerialH { get; set; }
        public ISt_CompanyTransactionKindHRepo St_CompanyTransactionKindH { get; set; }
        public ISt_ItemCardHRepo St_ItemCardH { get; set; }
        public ISt_ItemGallaryRepo St_ItemGallary { get; set; }
        public ISt_DescriptionRepo St_Description { get; set; }
        public ISt_DescriptionDetailRepo St_DescriptionDetail { get; set; }
        public ISt_CategoryPriceRepo St_CategoryPrice { get; set; }
        public ISt_MeasurementDetailRepo St_MeasurementDetail { get; set; }
        public ISt_TransactionKindRepo St_TransactionKind { get; set; }
        public ISt_FundingAgencyHRepo St_FundingAgencyH { get; set; }
        public ISt_WarehouseAccountRepo St_WarehouseAccount { get; set; }
        public ISt_AdvertisingRepresentativeRepo St_AdvertisingRepresentative { get; set; }
        public ISt_BankHRepo St_BankH { get; set; }
        public ISt_ManufacturerCompanyRepo St_ManufacturerCompany { get; set; }
        public ISt_CountryOfOriginRepo St_CountryOfOrigin { get; set; }
        public ISt_OtherAccountRepo St_OtherAccount { get; set; }
        public  ISt_OtherAccountHRepo St_OtherAccountH { get; set; }
        public  ISt_CarpenterHRepo St_CarpenterH { get; set; }
        public  ISt_SalesManHRepo St_SalesManH { get; set; }
        public   ISt_ItemUnitRepo St_ItemUnit { get; set; }
        public ISt_BranchAccountHRepo St_BranchAccountH { get; set; }
        public ISt_ItemCardRepo St_ItemCard { get; set; }
        public ISt_DriverRepo St_Driver { get; set; }
        public ISt_DelegateReceivingHRepo St_DelegateReceivingH { get; set; }
        public ISt_ExtraExpenseRepo St_ExtraExpense { get; set; }
        public ISt_HeaderRepo St_Header { get; set; }
        public ISt_PurchaseOrderRepo St_PurchaseOrder { get; set; }
        public ISt_OfferPriceRepo St_OfferPrice { get; set; }
        public ISt_SalesOrderRepo St_SalesOrder { get; set; }
        public ISt_TransactionKindHRepo St_TransactionKindH { get; set; }

        public ISt_TransActionFileRepo St_TransActionFile { get; set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            //--------------- Account ---------------------------
            TransActionFile = new TransActionFileRepo(_context);
            FavScreen = new FavScreenRepo(_context);
            FreezeTransaction = new FreezeTransactionRepo(_context);
            CompanyTransactionKindMonthlySerial = new CompanyTransactionKindMonthlySerialRepo(_context);
            CustomerCity = new CustomerCityRepo(_context);
            CustomerArea = new CustomerAreaRepo(_context);
            SupplierBank = new SupplierBankRepo(_context);
            SupplierCountry = new SupplierCountryRepo(_context);
            SupplierCountryBank = new SupplierCountryBankRepo(_context);
            SupplierCity = new SupplierCityRepo(_context);
            SupplierCityBank = new SupplierCityBankRepo(_context);
            SupplierBranchBank = new SupplierBranchBankRepo(_context);
            DefinitionAssetSite = new DefinitionAssetSiteRepo(_context);
            AssetAdministration = new AssetAdministrationRepo(_context);
            AssetSection = new AssetSectionRepo(_context);
            AssetCircle = new AssetCircleRepo(_context);
            AssetSite = new AssetSiteRepo(_context);
            AssetTrustee = new AssetTrusteeRepo(_context);
            AssetMaintenance = new  AssetMaintenanceRepo(_context);
            AssetSaleConsum = new AssetSaleConsumRepo(_context);
            AssetTransAction = new AssetTransActionRepo(_context);
            AssetHeader = new AssetHeaderRepo(_context);
            AssetType = new  AssetTypeRepo(_context);
            Asset = new AssetRepo(_context);
            EstimatedBudget = new EstimatedBudgetRepo(_context);
            TransActionServiceBill = new  TransActionServiceBillRepo(_context);
            ServiceBillHeader = new ServiceBillHeaderRepo(_context);
            BankGuarantee = new BankGuaranteeRepo(_context);
            Header = new HeaderRepo(_context);
            TempPrepaidAndRevenueReceived = new TempPrepaidAndRevenueReceivedRepo(_context);
            TempPrepaidAndRevenueReceivedDetail = new TempPrepaidAndRevenueReceivedDetailsRepo(_context);
            Transaction = new TransactionRepo(_context);
            Sale = new SaleRepo(_context);
            AccreditationInformation = new AccreditationInformationRepo(_context);
            SupplierInformation = new SupplierInformationRepo(_context);
            CustomerInformation = new CustomerInformationRepo(_context);
            Company = new CompanyRepo(_context);
            User = new UserRepo(_context);
            NativeSql = new NativeSqlRepo(_context);
            AccountType = new AccountTypeRepo(_context);
            ChartOfAccount = new ChartOfAccountRepo(_context);
            Currency = new CurrencyRepo(_context);
            CostCenter = new CostCenterRepo(_context);
            CompanyTransactionKind = new CompanyTransactionKindRepo(_context);
            TransactionKind = new TransactionKindRepo(_context);
            ServiceGroup = new ServiceGroupRepo(_context);
            Service = new ServiceRepo(_context);
            CurrencyValue = new CurrencyValueRepo(_context);
            DefinitionBank = new DefinitionBankRepo(_context);
            Paper = new PaperRepo(_context);
            DefinitionOtherAccount = new DefinitionOtherAccountRepo(_context);
            DrawnBank = new DrawnBankRepo(_context);
            //--------------- Warehouse ---------------------------
            St_Warehouse = new St_WarehouseRepo(_context);
            St_WarehouseH = new St_WarehouseHRepo(_context);
            St_FactoryH = new St_FactoryHRepo(_context);
            St_SectionsOfFactoryH = new St_SectionsOfFactoryHRepo(_context);
            St_ItemGroupH = new St_ItemGroupHRepo(_context);
            St_ItemUnitH = new St_ItemUnitHRepo(_context);
            St_SubItemColorH = new St_SubItemColorHRepo(_context);
            St_BranchH = new St_BranchHRepo(_context);
            St_CompanyTransactionKind = new St_CompanyTransactionKindRepo(_context);
            St_CompanyTransactionKindSymbolSerial = new St_CompanyTransactionKindSymbolSerialRepo(_context);
            St_CompanyTransactionKindH = new St_CompanyTransactionKindHRepo(_context);
            St_CompanyTransactionKindSymbolSerialH = new St_CompanyTransactionKindSymbolSerialHRepo(_context);
            St_ItemCardH = new St_ItemCardHRepo(_context);
            St_Description = new St_DescriptionRepo(_context);
            St_ItemGallary = new St_ItemGallaryRepo(_context);
            St_DescriptionDetail = new St_DescriptionDetailRepo(_context);
            St_CategoryPrice = new St_CategoryPriceRepo(_context);
            St_MeasurementDetail = new St_MeasurementDetailRepo(_context);
            St_TransactionKind = new St_TransactionKindRepo(_context);
            St_FundingAgencyH = new St_FundingAgencyHRepo(_context);
            St_WarehouseAccount = new St_WarehouseAccountRepo(_context);
            St_AdvertisingRepresentative = new St_AdvertisingRepresentativeRepo(_context);
            St_BankH = new St_BankHRepo(_context);
            St_CountryOfOrigin = new St_CountryOfOriginRepo(_context);
            St_ManufacturerCompany = new St_ManufacturerCompanyRepo(_context);
            St_OtherAccount = new St_OtherAccountRepo(_context);
            St_OtherAccountH = new St_OtherAccountHRepo(_context);
            St_CarpenterH = new St_CarpenterHRepo(_context);
            St_SalesManH = new St_SalesManHRepo(_context);
            St_ItemUnit = new St_ItemUnitRepo(_context);
            St_BranchAccountH = new St_BranchAccountHRepo(_context);
            St_ItemCard = new St_ItemCardRepo(_context);
            St_Driver = new St_DriverRepo(_context);
            St_DelegateReceivingH = new St_DelegateReceivingHRepo(_context);
            St_ExtraExpense = new St_ExtraExpenseRepo(_context);
            St_Header = new St_HeaderRepo(_context);
            St_PurchaseOrder = new St_PurchaseOrderRepo(_context);
            St_OfferPrice = new St_OfferPriceRepo(_context);
            St_SalesOrder = new St_SalesOrderRepo(_context);
            St_TransactionKindH = new St_TransactionKindHRepo(_context);
            St_TransActionFile = new St_TransActionFileRepo(_context);

        }
        public void Complete()
        {
            _context.SaveChanges();
        }
    }
}