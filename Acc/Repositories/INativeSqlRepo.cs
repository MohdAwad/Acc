using Acc.Models;
using Acc.ViewModels;
using System;
using System.Collections.Generic;

namespace Acc.Repositories
{
    public interface INativeSqlRepo
    {
        //******//
        //ResttingAccountVM GetAllAcountToRessttingByAccount(string AccountNo, int Year);
        //IEnumerable<ResttingAccountVM> GetAllAcountToResstting(string AccountType, int Type,int Year);
            //****//
        IEnumerable<AccYear> GetAllYear();
        double GetTotalBankFund(int CompanyID,int year);
        IEnumerable<ExpenseTotalVM> GetTotlExpense(int CompanyID, DateTime Fromdate, DateTime ToDate );
        double GetAccountTypeTotalForAcc(int CompanyID, DateTime Fromdate, DateTime ToDate,string AccNo);
        IEnumerable<AccountTTotalVM> GetAccountTypeTotal(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<TransActionFilter> GetServiceBillHeaders(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<MutilUseSearchVM> GetAllTransActionAccountSalesGain(int CompanyID);
        MutilUseSearchVM CheckAllTransActionAccountSalesGain(int CompanyID , string AccountNumber);
        IEnumerable<DefinitionAssetSiteVM> GetAllDefinitionAsset(int CompanyID);
        DefinitionAssetSiteVM CheckAllDefinitionAsset(int CompanyID, string AccountNumber);
        IEnumerable<St_WarehouseSearchHVM> GetAllSt_ItemGroupH(int CompanyID);
        IEnumerable<St_WarehouseSearchHVM> GetAllSt_ItemCardH(int CompanyID);
        IEnumerable<St_ItemCardHVM> GetAllSt_ItemCardHFilter(int CompanyID);
        IEnumerable<St_ItemCardVM> GetAllSt_ItemCardFilter(int CompanyID,string ItemCode);
        IEnumerable<St_ItemCardVM> GetAllSt_ItemCardFilterByWarehouse(int CompanyID, string ItemCode,string StockCode);
        IEnumerable<St_ItemCardVM> GetAllSt_ItemCardFilterByAdvertisingRepresentative(int CompanyID, string ItemCode, int AdvertisingRepresentativeID);
        IEnumerable<St_ItemCardVM> GetAllSt_ItemCardFilterByWarehouseToDetermineQuantity(int CompanyID, string ItemCode, string StockCode);
        IEnumerable<St_ItemCardVM> GetAllSt_ItemCardFilterByWarehouseToInitialInventory(int CompanyID, string ItemCode, string StockCode);
        IEnumerable<MutilUseSearchVM> GetAllTransActionAccountPurchase(int CompanyID);
        MutilUseSearchVM CheckAllTransActionAccountPurchase(int CompanyID , string AccountNumber);
        IEnumerable<CompanyTransationKindVM> GetAllCompanyTransactionKind(int CompanyID);
        IEnumerable<St_CompanyTransationKindVM> GetAllSt_CompanyTransactionKind(int CompanyID);
        IEnumerable<St_CompanyTransationKindHVM> GetAllSt_CompanyTransactionKindH(int CompanyID);
        IEnumerable<AssetTransActionVM> GetAllAssetTRanAction(int CompanyId, int VouchrNo);
        IEnumerable<SupplierType> GetAllSupplierType();
        void TransferTRansActionOpeinig(int CompanyID, string FromAccount, string ToAccount, int Year);

        void UpdateOpeinigBalance(int CompanyID, string AccountNumber, double OpeningBalanceDebit, double OpeningBalanceCredit, int Year);
        void UpdateTransActionOpeinigBalance(int CompanyID, string AccountNumber, double OpeningBalanceDebit, double OpeningBalanceCredit, int Year);

        void DeleteOpeinigBalance(int CompanyID, string AccountNumber,   int Year);

        void TransferTRansAction(int CompanyID, string FromAccount, string ToAccount, int Year);
        void TransferAccreditationInformation(int CompanyID, string FromAccount, string ToAccount);
        void TransferSt_CarpenterH(int CompanyID, string FromAccount, string ToAccount);
        void TransferSt_DelegateReceivingH(int CompanyID, string FromAccount, string ToAccount);
        void TransferSt_SalesManH(int CompanyID, string FromAccount, string ToAccount);
        void TransferSt_PurchaseOrderHeader(int CompanyID, string FromAccount, string ToAccount, int Year);
        void TransferTRansActionServiceBill(int CompanyID, string FromAccount, string ToAccount, int Year);
        void TransferAssetMaintenance(int CompanyID, string FromAccount, string ToAccount);
        void TransferEstimatedBudget(int CompanyID, string FromAccount, string ToAccount, int Year , int level);
        void TransferPapers(int CompanyID, string FromAccount, string ToAccount, int Year,int level);      
        void TransferCompany(int CompanyID, string FromAccount, string ToAccount, int Year,int level);      
        void TransferTracingPapers(int CompanyID, string FromAccount, string ToAccount, int Year, int level);
        void TransferHeaderServiceBill(int CompanyID, string FromAccount, string ToAccount, int Year, int level);
        void TransferO_HeaderH(int CompanyID, string FromAccount, string ToAccount, int Year, int level);
        void TransferO_HeaderHistoryH(int CompanyID, string FromAccount, string ToAccount, int Year, int level);
        void TransferHeaders(int CompanyID, string FromAccount, string ToAccount, int Year, int level);
        void TransferO_PaperH(int CompanyID, string FromAccount, string ToAccount, int Year, int level);
        void TempPrepaidAndRevenueReceivedDetailFrom(int CompanyID, string FromAccount, string ToAccount, int Year , int level);
        void TransferO_TransactionH(int CompanyID, string FromAccount, string ToAccount, int Year );
        void TransferO_TransactionHistoryH(int CompanyID, string FromAccount, string ToAccount, int Year );
        void TransferSt_BankH(int CompanyID, string FromAccount, string ToAccount);
        void TransferSt_FundingAgencyH(int CompanyID, string FromAccount, string ToAccount);
        void TransferSt_BranchOtherExpenseAccountH(int CompanyID, string FromAccount, string ToAccount);
        void TransferSt_HeaderH(int CompanyID, string FromAccount, string ToAccount);
        void TransferCostCenters(int CompanyID, string FromAccount, string ToAccount, int Year, int level);
        void TransferSt_Header(int CompanyID, string FromAccount, string ToAccount, int level);
        void TransferSt_ItemCard(int CompanyID, string FromAccount, string ToAccount);
        void TransferSt_ItemCardH(int CompanyID, string FromAccount, string ToAccount);
        void TransferSt_BranchExpenseAccountH(int CompanyID, string FromAccount, string ToAccount);
        void TransferSt_BranchAccountH(int CompanyID, string FromAccount, string ToAccount, int level);
        void TransferSt_WarehouseAccount(int CompanyID, string FromAccount, string ToAccount, int level);
        void TransferSt_OtherAccount(int CompanyID, string FromAccount, string ToAccount, int level);
        void TransferSt_OtherAccountH(int CompanyID, string FromAccount, string ToAccount, int level);
        void TransferSt_Warehouse(int CompanyID, string FromAccount, string ToAccount, int level);
        void TempPrepaidAndRevenueReceivedDetailTo(int CompanyID, string FromAccount, string ToAccount, int Year , int level);
        void TransferAsset(int CompanyID, string FromAccount, string ToAccount, int level);
        void UpdateOpeiningBalance(int CompanyID,int Year, string AccountNo, double OpeningBalanceDebit, double OpeningBalanceCredit);
         void TransferDefinitionBank(int CompanyID, string FromAccount, string ToAccount, int level);
         void TransferSupplierInformations(int CompanyID, string FromAccount, string ToAccount, int level);
         void TransferOtherAccount(int CompanyID, string FromAccount, string ToAccount, int level);
        void TransferO_ReceiptBillHeaderH(int CompanyID, string FromAccount, string ToAccount, int Year, int level);
        void TransferO_ReceiptBillHeaderHistoryH(int CompanyID, string FromAccount, string ToAccount, int Year, int level);
        IEnumerable<TransactionTotVM> TransactionByAccKind(int CompanyID, DateTime FromDate, DateTime toDate,int AccKind);
        IEnumerable<CustSuppRepVM> GetAccountsInfoByKind(int CompanyID,int AccountKind);
        IEnumerable<ChartOfAccount> GetTransActionAccountForBalance(int CompanyID);
        IEnumerable<DrawnBankVM> GetDrawnBank(int CompanyID);
        IEnumerable<TransActionDataVM> GetTransActionDatasReport(int CompanyID,Boolean ByDate,DateTime fromDate,DateTime ToDate);
        IEnumerable<TransReportDetailVM> GetTransActionDatasReportDetail(int CompanyID, DateTime fromDate, DateTime ToDate);
        IEnumerable<AccountLevelVM> GetAccountLevelVMs(int CompanyID);
        IEnumerable<ChartOfAccount> GetChartOfAccountByLevel(int CompanyID);
        IEnumerable<ChartOfAccount> GetChartOfAccountByLevelAndCity(int CompanyID,int CityId,int AreaId);
        IEnumerable<ChartOfAccount> GetChartOfAccountByCostCenterID(int CompanyID,string CostCenterID,int BudgetType);
        IEnumerable<CostCenter> GetCostCenterByLevel(int CompanyID);
        IEnumerable<ChartOfAccount> GetAccountChild(int CompanyID, string AccountNo);
        IEnumerable<CostCenter> GetCostChild(int CompanyID, string CostNumber);
        IEnumerable<TrialBalanceVM> GetTransactionForTrial(int CompanyID,DateTime FromDate,DateTime toDate,bool ByCostCenter,string CostCenter,bool PartOfNumber,int PartType);
        IEnumerable<TrialBalanceVM> GetTransactionForTrialForDash(int CompanyID, DateTime FromDate, DateTime toDate, bool ByCostCenter, string CostCenter, bool PartOfNumber, int PartType);
        IEnumerable<TrialBalanceVM> GetTransactionForTrialCost(int CompanyID, DateTime FromDate, DateTime toDate);
        IEnumerable<TrialBalanceVM> GetTotalTransactionByCostCenterID(int CompanyID, DateTime FromDate, DateTime toDate );
        IEnumerable<TrialBalanceVM> GetTotalTransactionByCostCenter(int CompanyID, DateTime FromDate, DateTime toDate );
        IEnumerable<Transaction> GetTransactionForPivotAcc(int CompanyID, DateTime FromDate, DateTime toDate);
        IEnumerable<TrialBalanceVM> GetTotCreditDebitForTrial(int CompanyID, DateTime FromDate, DateTime toDate, bool ByCostCenter, string CostCenter, bool PartOfNumber, int PartType);
        IEnumerable<TrialBalanceVM> GetTotCreditDebitForTrialForDash(int CompanyID, DateTime FromDate, DateTime toDate, bool ByCostCenter, string CostCenter, bool PartOfNumber, int PartType);
        IEnumerable<TrialBalanceVM> GetTransactionForTrialYearly(int CompanyID, DateTime FromDate, DateTime toDate, bool ByCostCenter, string CostCenter, bool PartOfNumber, int PartType);
        IEnumerable<TrialBalanceVM> GetTransactionForCostMonthly(int CompanyID, DateTime FromDate, DateTime toDate);
        IEnumerable<TrialBalanceVM> GetTransactionForCostMonthlyByID(int CompanyID, DateTime FromDate, DateTime toDate,string CostCenterID);
        IEnumerable<TrialBalanceVM> GetTransactionForTrialOpen(int CompanyID, DateTime FromDate, DateTime toDate);
        IEnumerable<TrialBalanceVM> GetTotCreditDebitForTrialOpen(int CompanyID, DateTime FromDate, DateTime toDate);
        IEnumerable<TrialBalanceVM> GetTotCreditDebitForCostCenterOpen(int CompanyID, DateTime FromDate, DateTime toDate);
        void DeleteTransActionTrans(int CompanyID, string VoucherNumber, int CompanyTransactionKindNo, int TransactionKindNo, int CompanyYear);
        void DeleteSt_Transaction(int CompanyID, string VoucherNumber, int CompanyTransactionKindNo, int TransactionKindNo, int CompanyYear, string StockCode);
        void DeletePaper(int CompanyID, string VoucherNumber, int CompanyTransactionKindNo, int TransactionKindNo, int CompanyYear);
        void DeletePaperNotUsed(int CompanyID, string VoucherNumber, int CompanyTransactionKindNo, int TransactionKindNo, int CompanyYear);
        void DeletePaymentPaperNotUsed(int CompanyID, string VoucherNumber, int CompanyTransactionKindNo, int TransactionKindNo, int CompanyYear);
        void DeletePaymentPaperNotUsedInTransaction(int CompanyID, string VoucherNumber, int CompanyTransactionKindNo, int TransactionKindNo, int CompanyYear);
        double GetSumPaperUsed(int CompanyID, string VoucherNumber, int CompanyTransactionKindNo, int TransactionKindNo, int CompanyYear);
        double GetSumPaymnetPaperUsed(int CompanyID, string VoucherNumber, int CompanyTransactionKindNo, int TransactionKindNo, int CompanyYear);
        int GetCountPaperUsed(int CompanyID, string VoucherNumber, int CompanyTransactionKindNo, int TransactionKindNo, int CompanyYear);
        int GetCountPaymentPaperUsed(int CompanyID, string VoucherNumber, int CompanyTransactionKindNo, int TransactionKindNo, int CompanyYear);
        IEnumerable<Paper> GetRowNumberPaperUsed(int CompanyID, string VoucherNumber, int CompanyTransactionKindNo, int TransactionKindNo, int CompanyYear);
        IEnumerable<Paper> GetRowNumberPaymentPaperUsed(int CompanyID, string VoucherNumber, int CompanyTransactionKindNo, int TransactionKindNo, int CompanyYear);
        IEnumerable<Transaction> GetRowNumberPaymentPaperUsedInTransaction(int CompanyID, string VoucherNumber, int CompanyTransactionKindNo, int TransactionKindNo, int CompanyYear);
        void DeleteTempPrepaidAndRevenueReceivedDetails(int CompanyID, string VoucherNumber, int CompanyTransactionKindNo, int TransactionKindNo, int CompanyYear);
        void DeleteTempPrepaidAndRevenueReceivedDetailsWithoutExport(int CompanyID, string VoucherNumber, int CompanyTransactionKindNo, int TransactionKindNo, int CompanyYear);
        void DeleteTransActionDebit(int CompanyID, string VoucherNumber, int CompanyTransactionKindNo, int TransactionKindNo, int CompanyYear);
        void DeleteTransActionCredit(int CompanyID, string VoucherNumber, int CompanyTransactionKindNo, int TransactionKindNo, int CompanyYear);
        IEnumerable<TransActionFilter> GetHeadersForReport(int CompanyID, DateTime Fromdate, DateTime ToDate); 
        int GetMaxChartNumberFather(int CoId);
        int GetCountNumberOfPaymnetTempPrepaidAndRevenueReceived(int CompanyID, string VoucherNumber, int CompanyTransactionKindNo, int TransactionKindNo, int CompanyYear);
        double GetSumTotalTempPrepaidAndRevenueReceived(int CompanyID, string VoucherNumber, int CompanyTransactionKindNo, int TransactionKindNo, int CompanyYear);
        int GetMaxChartNumberChild(int CoId, string AccountNumber);
        int GetMaxCostNumberFather(int CoId);
        int GetMaxCostNumberChild(int CoId, string AccountNumber);
        int CheckCostChartExist(int CompanyID);
        int AccountChartExist(int CompanyID);
        ChartOfAccountClientVM GetAccountClientInfo(int CompanyID, string AccountNumber);
        TempPrepaidAndRevenueReceived GetTempPrepaidAndRevenueReceivedData(string VoucherNumber, int CompanyID, int CompanyTransactionKindNo, int TransactionKindNo);
        DefinitionAssetSiteVM CheckDefinitionAssetSite(int CompanyID, int SerialID);
        ChartOfAccountSupplierVM GetAccountSupplierInfo(int CompanyID, string AccountNumber);
        ChartOfAccountAccreditationVM GetAccountAccreditation(int CompanyID, string AccountNumber);
        IEnumerable<CurrencyValueVM> GetAllCurrencyValue(int CompanyID);
        IEnumerable<CustomerAreaVM> GetAllCustomerArea(int CompanyID);
        CustomerAreaVM GetCustomerAreaByID(int CompanyID,int CustomerAreaID);
        SupplierCityVM GetCountryIDByCityID(int CompanyID, int SuuplierCityID);
        SupplierCityBankVM GetBankCountryIDByBankCityID(int CompanyID, int SuuplierCityBankID);
        SupplierBranchBankVM GetBankIDByBranchBankID(int CompanyID, int BranchBankID);
        IEnumerable<SupplierBranchBankVM> GetAllSupplierBranchBank(int CompanyID);
        SupplierBranchBankVM GetSupplierBranchBankByID(int CompanyID, int SupplierBranchBankID);
        IEnumerable<SupplierCityVM> GetAllSupplierCity(int CompanyID);
        SupplierCityVM GetSupplierCityByID(int CompanyID, int SupplierCityID);
        IEnumerable<SupplierCityBankVM> GetAllSupplierCityBank(int CompanyID);
        SupplierCityBankVM GetSupplierCityBankByID(int CompanyID, int SupplierCityBankID);
        IEnumerable<CompanyTransactionKind> GetCompanyTransactionKind(int CompanyID);
        IEnumerable<CompanyTransactionKind> GetPrepaidAndRevenuCompanyTransactionKind(int CompanyID);
        IEnumerable<CompanyTransactionKind> GetReceiptBankTransactionCompanyTransactionKind(int CompanyID);
        IEnumerable<CompanyTransactionKind> GetPaymentBankTransactionCompanyTransactionKind(int CompanyID);
        IEnumerable<CompanyTransactionKind> GetCompanyTransactionKindAll(int CompanyID);
        CurrencyValueVM GetCurrencyValueByID(int CompanyID, int CurrencyID);
        IEnumerable<MutilUseSearchVM> GetTransActionAccount(int CompanyID);
        MutilUseSearchVM CheckTransActionAccount(int CompanyID, string AccountNumber);
        St_WarehouseSearchVM CheckSt_ItemCard(int CompanyID, string ItemCode);
        
        MutilUseSearchVM CheckFatherTransActionAccount(int CompanyID, string AccountNumber);
        DrawnBanksFilterVM CheckDrawnBank(int CompanyID, int BankID);
        IEnumerable<MutilUseSearchVM> GetAllDefinitionExpense(int CompanyID,string Expense);
        IEnumerable<MutilUseSearchVM> GetAllDefinitionPaidExpense(int CompanyID,string PaidExpense);
        MutilUseSearchVM CheckAllDefinitionPaidExpense(int CompanyID,string PaidExpense , string AccountNumber);
        IEnumerable<MutilUseSearchVM> GetAllDefinitionFund(int CompanyID,string CashFund);
        MutilUseSearchVM CheckAllDefinitionFund(int CompanyID,string CashFund , string AccountNumber);
        IEnumerable<MutilUseSearchVM> GetAllDefinitionRevenue(int CompanyID,string Revenues);
        MutilUseSearchVM CheckAllDefinitionRevenue(int CompanyID,string Revenues , string AccountNumber);
        MutilUseSearchVM CheckAllDefinitionExpense(int CompanyID,string Revenues , string AccountNumber);
        IEnumerable<MutilUseSearchVM> GetAllDefinitionRevenueReceived(int CompanyID,string RevenuesReceived);
        MutilUseSearchVM CheckAllDefinitionRevenueReceived(int CompanyID, string RevenuesReceived, string AccountNumber);
        IEnumerable<MutilUseSearchVM> GetAllDefinitionTax(int CompanyID);
        MutilUseSearchVM CheckAllDefinitionTax(int CompanyID ,string AccountNumber);
        IEnumerable<MutilUseSearchVM> GetAllDefinitionChequeFund(int CompanyID,string ChequeFund);
        MutilUseSearchVM CheckAllDefinitionChequeFund(int CompanyID,string ChequeFund , string AccountNumber);
        IEnumerable<MutilUseSearchVM> GetAllTransActionAccount(int CompanyID);
        MutilUseSearchVM CheckAllAccountNumber(int CompanyID, string AccountNumber);
        IEnumerable<MutilUseSearchVM> GetCustomerAccount(int CompanyID);
        MutilUseSearchVM CheckCustomerAccount(int CompanyID , string AccountNumber);
        IEnumerable<MutilUseSearchVM> GetCustomerFatherAccount(int CompanyID);
        IEnumerable<MutilUseSearchVM> GetClientAccount(int CompanyID);
        MutilUseSearchVM CheckClientAccount(int CompanyID, string AccountNumber);
        IEnumerable<MutilUseSearchVM> GetClientFatherAccount(int CompanyID);
        IEnumerable<MutilUseSearchVM> GetCustomerAndClientAccount(int CompanyID);
        MutilUseSearchVM CheckCustomerAndClientAccount(int CompanyID , string AccountNumber);
        IEnumerable<ServiceVM> GetAllService(int CompanyID);
        ServiceVM CheckAllService(int CompanyID , string AccountNumber);
        IEnumerable<MutilUseSearchVM> GetFatherAccount(int CompanyID);
        IEnumerable<CompanyTransactionKind> GetReceiptVoucherCashFromTransactionKind(int CompanyID);
        IEnumerable<CompanyTransactionKind> GetReceiptVoucherCashMultiAccountFromTransactionKind(int CompanyID);
        IEnumerable<CompanyTransactionKind> GetPaymentVoucherCashMultiAccountFromTransactionKind(int CompanyID);
        IEnumerable<CompanyTransactionKind> GetReceiptVoucherBankFromTransactionKind(int CompanyID);
        IEnumerable<CompanyTransactionKind> GetPaymentVoucherBankFromTransactionKind(int CompanyID);
        IEnumerable<CompanyTransactionKind> GetDepositInTheBankFromTransactionKind(int CompanyID);
        IEnumerable<CompanyTransactionKind> GetPrepaidFromTransactionKind(int CompanyID);
        IEnumerable<CompanyTransactionKind> GetRevenueFromTransactionKind(int CompanyID);
        IEnumerable<CompanyTransactionKind> GetAllTransactionKind(int CompanyID);
        IEnumerable<MutilUseSearchVM> GetTransActionCostCenter(int CompanyID);
        MutilUseSearchVM CheckTransActionCostCenter(int CompanyID,string CostCenter);
        IEnumerable<CompanyTransactionKind> GetPaymentVoucherCashFromTransactionKind(int CompanyID);
        IEnumerable<CompanyTransactionKind> GetDebitNoteFromTransactionKind(int CompanyID);
        IEnumerable<CompanyTransactionKind> GetCreditNoteFromTransactionKind(int CompanyID);
        IEnumerable<DebitNoteFilterVM> GetAllDebitNoteFromHeader(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<DepositInTheBankFilter> GetAllDepositInTheBankFromHeader(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<TempPrepaidAndRevenueReceivedFilterVM> GetAllTempPrepaid(int CompanyID, DateTime Fromdate, DateTime ToDate,DateTime FromDateFirstPayment,DateTime ToDateFirstPayment);
        IEnumerable<TempPrepaidAndRevenueReceivedFilterVM> GetAllTempRevenue(int CompanyID, DateTime Fromdate, DateTime ToDate, DateTime FromDateFirstPayment, DateTime ToDateFirstPayment);
        IEnumerable<DebitNoteFilterVM> GetAllDebitNoteFromHeaderUnExport(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<DebitNoteFilterVM> GetAllDebitNoteFromHeaderExport(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<DepositInTheBankFilter> GetAllDepoistInTheBankFromHeaderUnExport(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<TempPrepaidAndRevenueReceivedFilterVM> GetAlllCollectionPrepaid(int CompanyID, DateTime FromDate, DateTime ToDate);
        IEnumerable<TempPrepaidAndRevenueReceivedFilterVM> GetAlllCollectionRevenue(int CompanyID, DateTime FromDate, DateTime ToDate);
        IEnumerable<DepositInTheBankFilter> GetAllDepoistInTheBankFromHeaderExport(int CompanyID, DateTime Fromdate, DateTime ToDate);
        string GetAccountName(int CompanyID,string AccountNumber);
        string GetCostCenterName(int CompanyID, string CostCenterNumber);
        string GetCustomerCityName(int CompanyID, int CustomerCityID);
        string GetSupplierBankName(int CompanyID, int SupplierBankID);
        string GetSupplierCountryName(int CompanyID, int SupplierCountryID);
        string GetSupplierCountryBankName(int CompanyID, int SupplierCountryBankID);
        ChartOfAccount GetAccountInfo(int CompanyID, string AccNumber, int Year);
        Transaction GetTransactionsDetailDebit(string VoucherNumber, int CompanyID, int CompanyTransactionKindNo,int iRowNumber , int CompanyYear);
        TransactionFixedVM GetPostedPrintPapersCase(string VoucherNumber, int CompanyTransactionKindNo, int CompanyID, int TransactionKindNo, int CompanyYear);
        Transaction GetTransactionsDetailDebitByTransKind(string VoucherNumber, int CompanyID, int CompanyTransactionKindNo, int iRowNumber, int TransactionKindNo, int CompanyYear);
        Transaction GetTransactionsDetailCredit(string VoucherNumber, int CompanyID, int CompanyTransactionKindNo, int iRowNumber , int CompanyYear);
        Transaction GetTransactionsDetailCreditByTransKind(string VoucherNumber, int CompanyID, int CompanyTransactionKindNo, int iRowNumber, int TransactionKindNo);
        IEnumerable<TransactionGridVM> GetTransactionsDetailDepositInTheBankCredit(string VoucherNumber, int CompanyTransactionKindNo, int CompanyID, int TransactionKindNo, int CompanyYear);
        IEnumerable<TransactionFixedVM> GetAllTransactionsDetailCredit(string VoucherNumber, int CompanyTransactionKindNo, int CompanyID, int TransactionKindNo , int CompanyYear);
        IEnumerable<TransactionFixedVM> GetAllTransactionsDetailDebit(string VoucherNumber, int CompanyTransactionKindNo, int CompanyID, int TransactionKindNo , int CompanyYear);
        IEnumerable<TransactionGridVM> GetTransactionsDetailReceiptVoucherCashMultiCredit(string VoucherNumber, int CompanyTransactionKindNo, int CompanyID, int TransactionKindNo, int CompanyYear);
        IEnumerable<TransactionGridVM> GetTransactionsDetailPaymentVoucherCashMultiDebit(string VoucherNumber, int CompanyTransactionKindNo, int CompanyID, int TransactionKindNo, int CompanyYear);
        IEnumerable<TransactionFixedVM> GetPapersToReceiptVoucherBank(string VoucherNumber, int CompanyTransactionKindNo, int CompanyID, int TransactionKindNo, int CompanyYear);
        IEnumerable<TransactionFixedVM> GetPapersToPaymentVoucherBank(string VoucherNumber, int CompanyTransactionKindNo, int CompanyID, int TransactionKindNo, int CompanyYear);
        IEnumerable<TransactionFixedVM> GetPapersToReceiptVoucherBankInChequeFund(string VoucherNumber, int CompanyTransactionKindNo, int CompanyID, int TransactionKindNo, int CompanyYear);
        IEnumerable<TransactionFixedVM> GetPapersToReceiptVoucherBankInChequeFundCopy(string VoucherNumber, int CompanyTransactionKindNo, int CompanyID, int TransactionKindNo, int CompanyYear);
        IEnumerable<TransactionFixedVM> GetPapersToPaymentVoucherBankPosted(string VoucherNumber, int CompanyTransactionKindNo, int CompanyID, int TransactionKindNo, int CompanyYear);
        IEnumerable<TransactionFixedVM> GetPapersToPaymentVoucherBankPostedCopy(string VoucherNumber, int CompanyTransactionKindNo, int CompanyID, int TransactionKindNo, int CompanyYear);
        IEnumerable<TempPrepaidAndRevenueReceivedVM> GetTempPrepaidAndRevenueReceivedDetailsNotExport(string VoucherNumber, int CompanyTransactionKindNo, int CompanyID, int TransactionKindNo , int CompanyYear);
        IEnumerable<TempPrepaidAndRevenueReceivedVM> GetTempPrepaidAndRevenueReceivedDetails(string VoucherNumber, int CompanyTransactionKindNo, int CompanyID, int TransactionKindNo , int CompanyYear);
        IEnumerable<TempPrepaidAndRevenueReceivedDetail> GetRowNumberTempPrepaidAndRevenueReceived(string VoucherNumber, int CompanyID, int CompanyTransactionKindNo, int TransactionKindNo,int CompanyYear);
        IEnumerable<TempPrepaidAndRevenueReceivedVM> GetTempPrepaidAndRevenueReceivedDetalsData(string VoucherNumber, int CompanyTransactionKindNo, int CompanyID, int TransactionKindNo);
        Transaction GetTransactionsDetailTax(string VoucherNumber, int CompanyID, int CompanyTransactionKindNo, int iRowNumber,int CompanyYear);
        IEnumerable<Transaction> GetAccountStatement(int CompanyID, int Year, string AccNumber, DateTime FromDate, DateTime ToDate);
        IEnumerable<Transaction> GetOpeningBalanceTrans(int CompanyID, int Year, string AccNumber, DateTime FromDate, DateTime ToDate);
        IEnumerable<CreditNoteFilterVM> GetAllCreditNoteFromHeader(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<CreditNoteFilterVM> GetAllCreditNoteFromHeaderUnExport(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<CreditNoteFilterVM> GetAllCreditNoteFromHeaderExport(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<ReceiptCashFilterVM> GetAllReceiptCashFromHeader(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<ReceiptCashFilterVM> GetAllReceiptCashFromHeaderUnExport(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<ServiceBillFilterVM> GetAllServiceBillUnExport(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<ReceiptCashFilterVM> GetAllReceiptCashFromHeaderExport(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<ServiceBillFilterVM> GetAllServiceBillExport(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<ReceiptCashFilterVM> GetAllReceiptCashMultiAccountFromHeader(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<ReceiptCashFilterVM> GetAllReceiptCashMultiAccountFromHeaderUnExport(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<ReceiptCashFilterVM> GetAllReceiptCashMultiAccountFromHeaderExport(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<PaymentCashFilterVM> GetAllPaymentCashMultiAccountFromHeader(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<PaymentCashFilterVM> GetAllPaymentCashMultiAccountFromHeaderUnExport(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<PaymentCashFilterVM> GetAllPaymentCashMultiAccountFromHeaderExport(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<ReceiptBankFilterVM> GetAllReceiptBankFromHeaderUnExport(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<ReceiptBankFilterVM> GetAllReceiptBankFromHeaderExport(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<PaymentBankFilterVM> GetAllPaymentBankFromHeaderUnExport(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<PaymentBankFilterVM> GetAllPaymentBankFromHeaderExport(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<PaymentCashFilterVM> GetAllPaymentCashFromHeader(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<PaymentCashFilterVM> GetAllPaymentCashFromHeaderUnExport(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<PaymentCashFilterVM> GetAllPaymentCashFromHeaderExport(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<TransActionVM> GetAllTransactionFromHeaderUnExport(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<TransActionVM> GetAllTransactionFromHeaderExport(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<ReceiptBankFilterVM> GetAllReceiptBankFromHeader(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<PaymentBankFilterVM> GetAllPaymentBankFromHeader(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<DefinitionBanksFilterVM> GetAllDefinitionBank(int CompanyID);
        DefinitionBanksFilterVM CheckAllDefinitionBank(int CompanyID , string AccountNumber);
        IEnumerable<DefinitionBanksFilterVM> GetAllBankAccountNumber(int CompanyID);
        DefinitionBanksFilterVM CheckAllBankAccountNumber(int CompanyID,string AccountNumber);
        IEnumerable<DrawnBanksFilterVM> GetAllDrawnBank(int CompanyID);
        DrawnBanksFilterVM CheckAllDrawnBank(int CompanyID , string AccountNumber);
        IEnumerable<St_WarehouseSearchHVM> GetAllSt_FactoryH(int CompanyID);
        IEnumerable<St_WarehouseSearchVM> GetAllSt_ItemUnit(int CompanyID);
        IEnumerable<PaperFilterVM> GetAllTransferFromFundToUnderCollection(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<PaperFilterVM> GetAllReturnAChequeFromTheChequeBox(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<PaperFilterVM> GetAllTransferFromFundToClearingDeposit(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<PaperFilterVM> GetAllTransferFromFundToChequeEndorsement(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<PaperFilterVM> GetAllPaymentChequeUnderCollection(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<PaperFilterVM> GetAllPaymentChequeEndorsement(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<PaperFilterVM> GetAllDrawingChequeUnderCollection(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<PaperFilterVM> GetAllReturnChequeUnderCollection(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<PaperFilterVM> GetAllReturnChequeClearingDeposit(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<PaperFilterVM> GetAllReturnChequeEndorsement(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<PaperFilterVM> GetAllFundChequesDrawnFromUnderCollection(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<PaperFilterVM> GetAllReturnedChequeFund(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<PaperFilterVM> GetAllCourtFund(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<PaperFilterVM> GetAllPostdatedCheque(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<PaperFilterVM> GetAllReturnPostdatedCheque(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<PaperFilterVM> GetAllReturnPaidPostdatedCheque(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<PaperFilterVM> GetAllCheques(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<PaperFilterVM> GetAllPaymentCheques(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<PaperFilterVM> GetTrankingChequesReport(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<PaperFilterVM> GetTrankingPaymentChequesReport(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<PaperFilterVM> GetChequeDetails(int CompanyID, string ChequeNumber, string OriginalVoucherNumber, int OriginalCompanyTransactionKind,
                                                    double ChequeAmount, int RowNumber, string AccountNumberThird, int OriginalTransactionKindNo, int CompanyYear, string PaidInAccountNumber);
        IEnumerable<PaperFilterVM> GetPaymentChequeDetails(int CompanyID, string ChequeNumber, string OriginalVoucherNumber, int OriginalCompanyTransactionKind,
                                                    double ChequeAmount, int RowNumber, string AccountNumberThird, int OriginalTransactionKindNo, int CompanyYear, string PaidToAccountNumber);
        IEnumerable<AccountingDetailsReportVM> GetVoucherDetails(int CompanyID, string VoucherNumber, int TransactionKindNo,int CompanyTransactionKindNo, int CompanyYear);
        IEnumerable<AccountingDetailsReportVM> GetServiceDetails(int CompanyID, string VoucherNumber, int TransactionKindNo, int CompanyTransactionKindNo, int CompanyYear);
        IEnumerable<AccountingDetailsReportVM> GetAccountingDetailsReport(int CompanyID, Boolean DateApproval, DateTime FromDate, DateTime ToDate);
        IEnumerable<SalesReportVM> GetSalesReport(int CompanyID,DateTime FromDate, DateTime ToDate);
        IEnumerable<ServiceTaxReportVM> GetServiceTaxReport(int CompanyID,DateTime FromDate, DateTime ToDate);
        IEnumerable<ServiceTaxReportVM> GetServiceTaxReportTaxExempt(int CompanyID, DateTime FromDate, DateTime ToDate);
        IEnumerable<ServiceTaxReportVM> GetServiceTaxReportDetailed(int CompanyID, DateTime FromDate, DateTime ToDate);
        IEnumerable<ServiceTaxReportVM> GetServiceTaxReportDetailedTaxExempt(int CompanyID, DateTime FromDate, DateTime ToDate);
        IEnumerable<ServiceReportVM> GetServiceReport(int CompanyID, DateTime FromDate, DateTime ToDate);
        IEnumerable<DefinitionAssetSiteVM> GetAssetSiteReport(int CompanyID, Boolean ApproveDeliveryDate,Boolean ApproveTransferDate, DateTime FromTransferDate, 
            DateTime ToTransferDate, DateTime FromDeliveryDate, DateTime ToDeliveryDate);
        IEnumerable<DefinitionAssetSiteVM> GetAssetsTransferMovementsReport(int CompanyID, Boolean ApproveDeliveryDate, Boolean ApproveTransferDate, DateTime FromTransferDate,
            DateTime ToTransferDate, DateTime FromDeliveryDate, DateTime ToDeliveryDate);
        IEnumerable<DefinitionAssetSiteVM> GetAssetsReport(int CompanyID, Boolean ApproveCombinedtDate, Boolean ApproveConsStartDate, DateTime FromCombinedtDate,
            DateTime ToCombinedtDate, DateTime FromConsStartDate, DateTime ToConsStartDate);
        IEnumerable<DefinitionAssetSiteVM> GetConsumptionByTypeReport(int CompanyID,Boolean ApproveConsStartDate,DateTime FromConsStartDate, DateTime ToConsStartDate);
        IEnumerable<DefinitionAssetSiteVM> GetAllAssetsHeader(int CompanyID, DateTime FromVouchrDate, DateTime ToVouchrDate);
        IEnumerable<DefinitionAssetSiteVM> GetAssetDepreciationTransactionReport(int CompanyID, int VouchrNo,Boolean ApproveLastConsumptionDate,Boolean ApproveConsumptionDate,
        DateTime FromLastConsumptionDate, DateTime ToLastConsumptionDate, DateTime FromConsumptionDate, DateTime ToConsumptionDate);
        IEnumerable<ServiceReportVM> GetServiceReportDetailed(int CompanyID, DateTime FromDate, DateTime ToDate);
        int GetCountAllChequeInFund(int CompanyID);
        int GetCountAllUbderCollectionCheque(int CompanyID);
        int GetCountAllEndorsementPaperCount(int CompanyID);
        int GetCountAllPaymentUbderCollectionCheque(int CompanyID);
        int GetCountAllClearingDepositCheque(int CompanyID);
        int GetCountAllDrawingUnderCollectionCheque(int CompanyID);
        int GetCountAllReturnedCheque(int CompanyID);
        int GetCountAllCourtFundCheque(int CompanyID);
        int GetCountAllPostdatedCheque(int CompanyID);
        int GetCountAllReturnPostdatedCheque(int CompanyID);
        int GetCountAllReturnPaidPostdatedCheque(int CompanyID);
        string GetSumAllChequeInFund(int CompanyID);
        string GetSumAllUbderCollectionCheque(int CompanyID);
        string GetSumAllEndorsementPaperCount(int CompanyID);
        string GetSumAllPaymentUbderCollectionCheque(int CompanyID);
        string GetSumAllClearingDepositCheque(int CompanyID);
        string GetSumAllDrawingUnderCollectionCheque(int CompanyID);
        string GetSumAllReturnedCheque(int CompanyID);
        string GetSumAllCourtFundCheque(int CompanyID);
        string GetSumAllPostdatedCheque(int CompanyID);
        string GetSumAlllReturnPostdatedCheque(int CompanyID);
        string GetSumAlllReturnPaidPostdatedCheque(int CompanyID);
        DateTime GetFromDateChequeInFund(int CompanyID);
        DateTime GetToDateChequeInFund(int CompanyID);
        DateTime GetFromDateUbderCollectionCheque(int CompanyID);
        DateTime GetToDateUbderCollectionCheque(int CompanyID);
        DateTime GetFromDateEndorsementCheque(int CompanyID);
        DateTime GetToDateEndorsementCheque(int CompanyID);
        DateTime GetFromDatePaymentUbderCollectionCheque(int CompanyID);
        DateTime GetToDatePaymentUbderCollectionCheque(int CompanyID);
        DateTime GetFromDateClearingDepositCheque(int CompanyID);
        DateTime GetToDateClearingDepositCheque(int CompanyID);
        DateTime GetFromDateDrawingUnderCollectionCheque(int CompanyID);
        DateTime GetToDateDrawingUnderCollectionCheque(int CompanyID);
        DateTime GetFromDateReturnedCheque(int CompanyID);
        DateTime GetToDateReturnedCheque(int CompanyID);
        DateTime GetFromDateCourtFundCheque(int CompanyID);
        DateTime GetToDateCourtFundCheque(int CompanyID);
        DateTime GetFromDateAllCheque(int CompanyID);
        DateTime GetToDateAllCheque(int CompanyID);
        DateTime GetFromDatePostdatedCheques(int CompanyID);
        DateTime GetToDatePostdatedCheques(int CompanyID);
        DateTime GetFromDateReturnPostdatedCheques(int CompanyID);
        DateTime GetToDateReturnPostdatedCheques(int CompanyID);
        DateTime GetFromDateReturnPaidPostdatedCheques(int CompanyID);
        DateTime GetToDateReturnPaidPostdatedCheques(int CompanyID);
        DateTime GetFromDateAllPostdatedCheque(int CompanyID);
        DateTime GetToDateAllPostdatedCheque(int CompanyID);
        IEnumerable<TransActionServiceBillVM> GetTransActionServiceBillsData(int CompanyID, int VoucherNumber, int CompanyTransactionKindNo, int TransactionKindNo, int CompanyYear);
        void SaveTracingPaper(int CompanyID, int CompanyYear, int RowNumber, string OldVoucherNumber, int OldCompanyTransactionKindNo, int OldTransactionKindNo,
                                   string ChequeNumber, string sChequeDate, double ChequeAmount, string AccountNumberThird);
        void UpdatePaper(int CompanyID, int CompanyYear, int RowNumber, string OldVoucherNumber, int OldCompanyTransactionKindNo, int OldTransactionKindNo, int OldVHI, int CompanyTransactionKindNo,
                        int TransactionKindNo, DateTime VoucherDate, string VoucherNumber, int VHI, int ChequeCase, string AccountNumberSecond, 
                        string AccountNumberFourth, string Remark, string InsUserID, DateTime InsDateTime,string AccountNumberFifth,
                        string ChequeNumber, string sChequeDate, double ChequeAmount, string AccountNumberThird);
        void UpdatePaperReturnedFromChequeBox(int CompanyID, int CompanyYear, int RowNumber, string OldVoucherNumber, int OldCompanyTransactionKindNo, int OldTransactionKindNo, int OldVHI, int CompanyTransactionKindNo,
                        int TransactionKindNo, DateTime VoucherDate, string VoucherNumber, int VHI, int ChequeCase,string Remark, string InsUserID, DateTime InsDateTime,
                        string ChequeNumber, string sChequeDate, double ChequeAmount, string AccountNumberThird);
        void UpdatePaymentPaperUnderCollection(int CompanyID, int CompanyYear, int RowNumber, string OldVoucherNumber, int OldCompanyTransactionKindNo, int OldTransactionKindNo, int OldVHI, int CompanyTransactionKindNo,
                        int TransactionKindNo, DateTime VoucherDate, string VoucherNumber, int VHI, int ChequeCase, string AccountNumberSecond,
                        string Remark, string InsUserID, DateTime InsDateTime, string AccountNumberFifth, 
                        string ChequeNumber, string sChequeDate, double ChequeAmount, string AccountNumberThird);
        void UpdatePaymentPaperEndorsement(int CompanyID, int CompanyYear, int RowNumber, string OldVoucherNumber, int OldCompanyTransactionKindNo,
                                               int OldTransactionKindNo, int OldVHI, int ChequeCase, string Remark, string InsUserID, DateTime InsDateTime,
                                               string ChequeNumber, string sChequeDate, double ChequeAmount, string AccountNumberThird);
        void UpdateDrawingPaperUnderCollection(int CompanyID, int CompanyYear, int RowNumber, string OldVoucherNumber, int OldCompanyTransactionKindNo, int OldTransactionKindNo, int OldVHI, int CompanyTransactionKindNo,
                        int TransactionKindNo, DateTime VoucherDate, string VoucherNumber, int VHI, int ChequeCase,
                        string Remark, string InsUserID, DateTime InsDateTime, string UnderReturnNote, string ChequeNumber, string sChequeDate, double ChequeAmount, string AccountNumberThird);
        void UpdateReturnPaperUnderCollection(int CompanyID, int CompanyYear, int RowNumber, string OldVoucherNumber, int OldCompanyTransactionKindNo, int OldTransactionKindNo, int OldVHI, int CompanyTransactionKindNo,
                        int TransactionKindNo, DateTime VoucherDate, string VoucherNumber, int VHI, int ChequeCase,
                        string Remark, string InsUserID, DateTime InsDateTime, string ReturnNote, string ChequeNumber, string sChequeDate, double ChequeAmount, string AccountNumberThird);
        void UpdateReturngPaperClearingDeposit(int CompanyID, int CompanyYear, int RowNumber, string OldVoucherNumber, int OldCompanyTransactionKindNo, int OldTransactionKindNo, int OldVHI, int CompanyTransactionKindNo,
                        int TransactionKindNo, DateTime VoucherDate, string VoucherNumber, int VHI, int ChequeCase,
                        string Remark, string InsUserID, DateTime InsDateTime, string ReturnNote, string ChequeNumber, string sChequeDate, double ChequeAmount, string AccountNumberThird);
        void UpdateReturngPaperEndorsement(int CompanyID, int CompanyYear, int RowNumber, string OldVoucherNumber, int OldCompanyTransactionKindNo, int OldTransactionKindNo, int OldVHI, int CompanyTransactionKindNo,
                        int TransactionKindNo, DateTime VoucherDate, string VoucherNumber, int VHI, int ChequeCase,
                        string Remark, string InsUserID, DateTime InsDateTime, string EndorReturnNote, string ChequeNumber, string sChequeDate, double ChequeAmount, string AccountNumberThird);
        void UpdateReturnToPaidAccount(int CompanyID, int CompanyYear, int RowNumber, string OldVoucherNumber, int OldCompanyTransactionKindNo,
                                               int OldTransactionKindNo, int OldVHI, int ChequeCase, string Remark, string InsUserID, DateTime InsDateTime,
                                               string ChequeNumber, string sChequeDate, double ChequeAmount, string AccountNumberThird,string UnderReturnCustomerNote);
        void UpdateReturnToPaidAccount1(int CompanyID, int CompanyYear, int RowNumber, string OldVoucherNumber, int OldCompanyTransactionKindNo,
                                               int OldTransactionKindNo, int OldVHI, int ChequeCase, string Remark, string InsUserID, DateTime InsDateTime,
                                               string ChequeNumber, string sChequeDate, double ChequeAmount, string AccountNumberThird, string ReturnCustomerNote);
        void UpdateTransferToCourt(int CompanyID, int CompanyYear, int RowNumber, string OldVoucherNumber, int OldCompanyTransactionKindNo,
                                               int OldTransactionKindNo, int OldVHI, int ChequeCase, string Remark, string InsUserID, DateTime InsDateTime,
                                               string ChequeNumber, string sChequeDate, double ChequeAmount, string AccountNumberThird, string CaseNumber);
        void UpdateCompromisePaper(int CompanyID, int CompanyYear, int RowNumber, string OldVoucherNumber, int OldCompanyTransactionKindNo,
                                               int OldTransactionKindNo, int OldVHI, int ChequeCase, string Remark, string InsUserID, DateTime InsDateTime,
                                               string ChequeNumber, string sChequeDate, double ChequeAmount, string AccountNumberThird);
        void UpdateCaseNumber(int CompanyID, int CompanyYear, int RowNumber, string OldVoucherNumber, int OldCompanyTransactionKindNo,
                                               int OldTransactionKindNo, int OldVHI, string CaseNumber, string InsUserID, DateTime InsDateTime,
                                               string ChequeNumber, string sChequeDate, double ChequeAmount, string AccountNumberThird);
        void UpdatePostdatedCheque(int CompanyID, int CompanyYear, int RowNumber, string OldVoucherNumber, int OldCompanyTransactionKindNo, int OldTransactionKindNo, int OldVHI,
                                int CompanyTransactionKindNo, int TransactionKindNo, DateTime VoucherDate, string VoucherNumber, int VHI, int ChequeCase,
                                string Remark, string InsUserID, DateTime InsDateTime, string ChequeNumber, string sChequeDate, double ChequeAmount, string AccountNumberThird);
        void UpdateReturnPaidPostdatedCheque(int CompanyID, int CompanyYear, int RowNumber, string OldVoucherNumber, int OldCompanyTransactionKindNo, int OldTransactionKindNo, int OldVHI,
                                int CompanyTransactionKindNo, int TransactionKindNo, DateTime VoucherDate, string VoucherNumber, int VHI, int ChequeCase,
                                string Remark, string InsUserID, DateTime InsDateTime, string ChequeNumber, string sChequeDate, double ChequeAmount, string AccountNumberThird, string PaymentPaidReturnNote);
        void UpdateReturnPostdatedCheque(int CompanyID, int CompanyYear, int RowNumber, string OldVoucherNumber, int OldCompanyTransactionKindNo, int OldTransactionKindNo, int OldVHI,
                                int CompanyTransactionKindNo, int TransactionKindNo, DateTime VoucherDate, string VoucherNumber, int VHI, int ChequeCase,
                                string Remark, string InsUserID, DateTime InsDateTime, string ChequeNumber, string sChequeDate, double ChequeAmount, string AccountNumberThird, string PaymentReturnNote);
        IEnumerable<TempPrepaidAndRevenueReceivedVM> GetTempRevenueReport(int CompanyID, Boolean All, Boolean Collector, Boolean NotCollected, DateTime FromCollectionDate, DateTime ToCollectionDate);
        IEnumerable<TempPrepaidAndRevenueReceivedVM> GetTempPrepaidReport(int CompanyID, Boolean All, Boolean Collector, Boolean NotCollected, DateTime FromCollectionDate, DateTime ToCollectionDate);
        void DeleteDefinitionAsset(int CompanyID, int AssetID);
        int CheckCurrency(int CompanyID, int CurrencyID);
        int CheckSale(int CompanyID, int SaleID);
        int CheckCompanyTransactionKind(int CompanyID, int CompanyTransactionKindID);
        int CheckIfMonthlySerial(int CompanyID, int CompanyTransactionKindID);
        int CheckServiceGroup(int CompanyID, int ServiceGroupID);
        int CheckService(int CompanyID, int ServiceID);
        string CheckDefinitionBank(int CompanyID, string AccountNumber);
        int CheckAssetAdministration(int CompanyID, int AdministrationID);
        int CheckAssetCircle(int CompanyID, int CircleID);
        int CheckAssetSection(int CompanyID, int SectionID);
        int CheckAssetSite(int CompanyID, int SiteID);
        int CheckAssetTrustee(int CompanyID, int TrusteeID);
        int CheckCustomerArea(int CompanyID, int AreaID);
        int CheckCustomerCity(int CompanyID, int CityID);
        int CheckSupplierBank(int CompanyID, int SupplierBankID);
        int CheckSupplierBranchBank(int CompanyID, int SupplierBranchBankID);
        int CheckSupplierCityBank(int CompanyID, int SupplierCityBankID);
        int CheckSupplierCity(int CompanyID, int SupplierCityID);
        int CheckSupplierCountryBank(int CompanyID, int CheckSupplierCountryBankID);
        int CheckSupplierCountry(int CompanyID, int SupplierCountryID);
        int CheckCustomerCityInArea(int CompanyID, int CustomerCityID);
        int CheckSupplierBankInBranchBank(int CompanyID, int SupplierBankID);
        int CheckSupplierCountryBankInCityBank(int CompanyID, int SupplierCountryBankID);
        int CheckSupplierCountryInCity(int CompanyID, int SupplierCountryID);
        int CheckAssetType(int CompanyID, int AssetTypeID);
        int CheckAsset(int CompanyID, int AssetID);
        string CheckAccountNumber(int CompanyID, string AccountNumber);
        string CheckCostCenter(int CompanyID, string CostNumber);
        IEnumerable<FreezeTransactionVM> GetAllFreezeTransaction(int CompanyID);
        double GetTotalUnpaidChequesReceived(int CompanyID, DateTime Fromdate, DateTime ToDate, string AccNo);
        IEnumerable<PaperFilterVM> GetUnpaidChequesReceived(int CompanyID, DateTime Fromdate, DateTime ToDate, string Acc);
        IEnumerable<PaperFilterVM> GetUnpaidFundCheques(int CompanyID, DateTime dToday);
        IEnumerable<PaperFilterVM> GetUnpaidUnderCollectionCheques(int CompanyID, DateTime dToday);
        double GetTotalUnpaidChequesPayment(int CompanyID, DateTime Fromdate, DateTime ToDate, string AccNo);
        IEnumerable<PaperFilterVM> GetUnpaidChequesPayment(int CompanyID, DateTime Fromdate, DateTime ToDate, string Acc);
        IEnumerable<PaperFilterVM> GetUnpaidPostdatedCheque(int CompanyID, DateTime dToday);
        AssetVM GetAssetByID(int CompanyID, int AssetID);
        IEnumerable<DefinitionBanksFilterVM> CheckBankAccountNumber(int CompanyID, string BankAccountNumber);
        IEnumerable<St_SectionsOfFactoryHVM> GetAllSt_SectionsOfFactoryH(int CompanyID);
        St_SectionsOfFactoryHVM GetSt_SectionsOfFactoryHByID(int CompanyID, int SectionsOfFactoryID);
        string GetSt_FactoryHName(int CompanyID, int FactoryID);
        int GetSt_RegisterValueByRegisterID(int CompanyID, int RegisterID);
        IEnumerable<St_WarehouseVM> GetAllSt_Warehouse(int CompanyID);
        IEnumerable<St_WarehouseHVM> GetAllSt_WarehouseH(int CompanyID);
        IEnumerable<St_WarehouseHVM> GetAllSt_WarehouseHByItemCode(int CompanyID,string ItemCode);
        IEnumerable<St_WarehouseVM> GetAllSt_WarehouseByItemCode(int CompanyID, string ItemCode);
        IEnumerable<St_ItemCardHVM> GetAllSt_WarehouseHByItemCodeView(int CompanyID, string ItemCode);
        IEnumerable<St_ItemCardVM> GetAllSt_WarehouseByItemCodeView(int CompanyID, string ItemCode);
        IEnumerable<St_SubItemColorHVM> GetAllSt_SubItemColorH(int CompanyID);
        IEnumerable<St_SubItemColorHVM> GetAllSt_SubColorHByItemCode(int CompanyID, string ItemCode);
        IEnumerable<St_ItemCardHVM> GetAllSt_SubColorHByItemCodeView(int CompanyID, string ItemCode);
        IEnumerable<St_ItemCardHVM> GetAllSt_RelatedItemHByItemCodeView(int CompanyID, string ItemCode);
        IEnumerable<St_ItemCardHVM> GetAllSt_SimilarItemHByItemCodeView(int CompanyID, string ItemCode);
        IEnumerable<St_ItemCardHVM> GetAllSt_ManufacturingStagesHByItemCodeView(int CompanyID, string ItemCode);
        IEnumerable<St_ItemCardHVM> GetAllSt_RawMaterialHByItemCodeView(int CompanyID, string ItemCode);
        IEnumerable<St_ItemCardVM> GetAllSt_SimilarItemByItemCodeView(int CompanyID, string ItemCode);
        IEnumerable<St_ItemCardVM> GetAllSt_AlternativeItemByItemCodeView(int CompanyID, string ItemCode);
        IEnumerable<St_ItemCardVM> GetAllSt_ItemOtherUnitByItemCodeView(int CompanyID, string ItemCode);
        IEnumerable<St_ItemCardVM> GetAllSt_ItemOffer1ByItemCodeView(int CompanyID, string ItemCode);
        IEnumerable<St_ItemCardVM> GetAllSt_ItemOffer2ByItemCodeView(int CompanyID, string ItemCode);
        IEnumerable<St_BranchHVM> GetAllSt_BranchH(int CompanyID);
        St_BranchHVM GetSt_BranchHByID(int CompanyID, string BranchCode);
        string GetSt_WarehouseName(int CompanyID, string StockCode);
        string GetSt_WarehouseHName(int CompanyID, string StockCode);
        int CheckSt_FactoryHBeforDelete(int CompanyID, int FactoryID);
        string CheckSt_WarehouseH(int CompanyID, string StockCode);
        IEnumerable<St_ItemCardHVM> GetRelatedItemByItemCardH(string ItemCode, int CompanyID);
        IEnumerable<St_WarehouseSearchHVM> CheckItemCodeH(int CompanyID, string ItemCode);
        IEnumerable<St_WarehouseSearchVM> CheckItemCode(int CompanyID, string ItemCode);
        IEnumerable<St_WarehouseSearchVM> CheckItemUnit(int CompanyID, int ItemUnitCode);
        IEnumerable<St_ItemCardHVM> GetSimilarItemByItemCardH(string ItemCode, int CompanyID);
        IEnumerable<St_ItemCardHVM> GetSubColorsItemByItemCardH(string ItemCode, int CompanyID);
        IEnumerable<St_ItemCardHVM> GetManufacturingStageByItemCardH(string ItemCode, int CompanyID);
        IEnumerable<St_ItemCardHVM> GetRawMaterialByItemCardH(string ItemCode, int CompanyID);
        St_WarehouseSearchHVM CheckSt_FactoryH(int CompanyID, int BankID);
        int GetOpeningDateH(int CompanyID, string ItemCode);
        int GetOpeningDate(int CompanyID, string ItemCode);
        void DeleteSt_ItemCardH(int CompanyID, string ItemCode);
        void DeleteSt_SimilarItemH(int CompanyID, string ItemCode);
        void DeleteSt_RelatedItemH(int CompanyID, string ItemCode);
        void DeleteSt_ManufacturingStageH(int CompanyID, string ItemCode);
        void DeleteSt_RawMaterialH(int CompanyID, string ItemCode);
        void DeleteSt_SubColorsItemH(int CompanyID, string ItemCode);
        void DeleteSt_ItemWarehouseH(int CompanyID, string ItemCode);
        void DeleteSt_ItemWarehouseHByStockCode(int CompanyID, string StockCode);
        void DeleteSt_ItemGallary(int CompanyID, string ItemCode);
        void DeleteSt_ItemCard(int CompanyID, string ItemCode);
        void DeleteSt_SimilarItem(int CompanyID, string ItemCode);
        void DeleteSt_ItemWarehouse(int CompanyID, string ItemCode);
        void DeleteSt_ItemWarehouseByStockCode(int CompanyID, string StockCode);
        void DeleteSt_ItemAdvertisingRepresentativeByAdvertisingRepresentativeID(int CompanyID, int AdvertisingRepresentativeID);
        void DeleteSt_AlternativeItem(int CompanyID, string ItemCode);
        void DeleteSt_ItemOtherUnit(int CompanyID, string ItemCode);
        void DeleteSt_ItemOffer(int CompanyID, string ItemCode);
        void DeleteSt_ItemAdvertisingRepresentative(int CompanyID, string ItemCode);
        int CheckDateIsFreezeDate(int CompanyID,DateTime dDate);
        IEnumerable<St_DescriptionDetailVM> GetAllSt_DescriptionDetail(int CompanyID);
        IEnumerable<St_MeasurementDetailVM> GetAllSt_MeasurementDetail(int CompanyID);
        IEnumerable<St_Measurement> GetAllSt_Measurement(int CompanyID);
        void DeleteSt_CompanyTransactionKind(int CompanyID, string StockCode);
        void DeleteSt_CompanyTransactionKindH(int CompanyID, string StockCode);
        string GetSt_TransactionKindName(int St_TransactionKindID);
        string GetSt_TransactionKindHName(int St_TransactionKindID);
        IEnumerable<St_TransactionKindHVM> GetSt_TransactionKindHAllStockTransaction(int CompanyID);
        IEnumerable<St_TransactionKindHVM> GetSt_TransactionKindHWithoutAllStockTransaction();
        IEnumerable<St_TransactionKindHVM> GetSt_TransactionKindH();
        IEnumerable<St_TransactionKindVM> GetSt_TransactionKindAllStockTransaction(int CompanyID);
        IEnumerable<St_TransactionKindVM> GetSt_TransactionKindWithoutAllStockTransaction();
        IEnumerable<St_TransactionKindVM> GetSt_TransactionKind();
        St_WarehouseAccountVM GetSt_WarehouseAccountByStock(int CompanyID, string StockCode);
        IEnumerable<St_ItemCardHVM> GetSt_ItemWarehouseH(int CompanyID);
        IEnumerable<St_BankHVM> GetAllBank(int CompanyID);
        IEnumerable<St_CarpenterHVM> GetAllSt_CarpenterH(int CompanyID);
        IEnumerable<St_DelegateReceivingHVM> GetAllSt_DelegateReceivingH(int CompanyID);
        IEnumerable<St_SalesManHVM> GetAllSt_SalesManH(int CompanyID);
        IEnumerable<St_BranchAccountHVM> GetBranchExpensesAccountH(string BranchCode, int CompanyID);
        IEnumerable<St_BranchAccountHVM> GetBranchOtherExpensesAccountH(string BranchCode, int CompanyID);
        void DeleteSt_BranchExpenseAccountH(int CompanyID, string BranchCode);
        void DeleteSt_BranchOtherExpenseAccountH(int CompanyID, string BranchCode);
        IEnumerable<St_BranchAccountHVM> GetAllSt_BranchAccountH(int CompanyID);
        IEnumerable<TransActionVM> GetTransactionsDetail(string VoucherNumber, int CompanyTransactionKindNo, int CompanyID, int TransactionKindNo,int CompanyYear);
        IEnumerable<St_ItemCardVM> GetSimilarItemByItemCard(string ItemCode, int CompanyID);
        IEnumerable<St_ItemCardVM> GetAlternativeItemByItemCard(string ItemCode, int CompanyID);
        IEnumerable<St_ItemCardVM> GetItemOffer1ByItemCard(string ItemCode, int CompanyID);
        IEnumerable<St_ItemCardVM> GetItemOffer2ByItemCard(string ItemCode, int CompanyID);
        IEnumerable<St_ItemCardVM> GetOtherItemUnitByItemCard(string ItemCode, int CompanyID);
        IEnumerable<St_WarehouseSearchVM> GetAllSt_ItemCard(int CompanyID);
        IEnumerable<St_WarehouseSearchVM> GetSt_ItemCard(int CompanyID);
        string CheckIfItemCodeExisting(int CompanyID, string ItemCode);
        string CheckIfItemCodeExistingInSimilar(int CompanyID, string ItemCode);
        St_ItemCardVM GetSt_ItemWarehouseByItemAndStock(int CompanyID, string ItemCode,string StockCode);
        St_ItemCardVM GetSt_InitialInventoryByItemAndStock(int CompanyID, string ItemCode, string StockCode);
        IEnumerable<St_PurchaseOrderVM> GetSt_PurchaseOrderTransactionLocal(string VoucherNumber, int CompanyTransactionKindNo, int CompanyID, int TransactionKindNo, int CompanyYear);
        IEnumerable<St_PurchaseOrderVM> GetAllGetAllSt_PurchaseOrder(int CompanyID, DateTime Fromdate, DateTime ToDate);
        void DeleteSt_PurchaseOrderTransactionLocal(string VoucherNumber, int CompanyTransactionKindNo, int CompanyID, int TransactionKindNo, int CompanyYear);
        IEnumerable<St_HeaderVM> GetSt_Transacation(string VoucherNumber, int CompanyTransactionKindNo, int CompanyID, int TransactionKindNo, int CompanyYear, string StockCode);
        int CheckSt_CompanyTransactionKind(int CompanyID, int CompanyTransactionKindID);
        IEnumerable<St_HeaderVM> GetAllSt_PurchaseVoucher(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<St_HeaderVM> GetAllSt_ReturnPurchaseVoucher(int CompanyID, DateTime Fromdate, DateTime ToDate);
        St_ItemOtherUnitVM GetSt_ItemOtherUnit(string ItemCode,int CompanyID, int OtherUnitNumber);
        IEnumerable<St_ItemOtherUnitVM> GetAllSt_ItemOtherUnitByItem(string ItemCode, int CompanyID);
        IEnumerable<St_ItemOtherUnitVM> GetAllSt_ItemOtherUnit(string ItemCode, int CompanyID);
        IEnumerable<St_HeaderVM> GetAllSt_SaleVoucher(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<St_OfferPriceVM> GetAllSt_OfferPrice(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<St_SaleOrderVM> GetAllSt_SaleOrder(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<St_OfferPriceVM> GetSt_OfferPriceTransaction(string VoucherNumber, int CompanyTransactionKindNo, int CompanyID, int TransactionKindNo, int CompanyYear);
        IEnumerable<St_SaleOrderVM> GetSt_SaleOrderTransaction(string VoucherNumber, int CompanyTransactionKindNo, int CompanyID, int TransactionKindNo, int CompanyYear);

        void DeleteSt_OfferPriceTransaction(string VoucherNumber, int CompanyTransactionKindNo, int CompanyID, int TransactionKindNo, int CompanyYear);

        void DeleteSt_SaleOrderTransaction(string VoucherNumber, int CompanyTransactionKindNo, int CompanyID, int TransactionKindNo, int CompanyYear);

        IEnumerable<St_HeaderVM> GetAllSt_InVoucher(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<St_HeaderVM> GetAllSt_OutVoucher(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<St_HeaderVM> GetAllSt_ReturnSaleVoucher(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<St_HeaderVM> GetAllSt_UnExport(int CompanyID, DateTime Fromdate, DateTime ToDate,int TransactionKindNo);
        IEnumerable<St_HeaderVM> GetAllSt_Export(int CompanyID, DateTime Fromdate, DateTime ToDate, int TransactionKindNo);
        IEnumerable<St_HeaderVM> GetAllSt_SpoiledVoucher(int CompanyID, DateTime Fromdate, DateTime ToDate);
        IEnumerable<St_HeaderVM> GetAllSt_TransferVoucher(int CompanyID, DateTime Fromdate, DateTime ToDate);
        int CheckSt_DescriptionDetailBeforDelete(int CompanyID, int DescriptionDetailID,int DescriptionID);
        int CheckItemUnitBeforDelete(int CompanyID, int ItemUnitNo);
        int CheckCountryOfOriginBeforDelete(int CompanyID, int CountryOfOriginNo);
        int CheckManufacturerCompanyBeforDelete(int CompanyID, int ManufacturerCompanyNo);
        string CheckItemCodeBeforDelete(int CompanyID, string ItemCode);
        int CheckSt_MeasurementDetailBeforDelete(int CompanyID, int MeasurementDetailID, int MeasurementID);
        string CheckWarehouseBeforDelete(int CompanyID, string StockCode);
        int CheckDriverBeforDelete(int CompanyID, int DriverID);
        IEnumerable<St_ItemTransactionReportVM> GetAverageCostByStockCode(int CompanyID, string ItemCode, string StockCode, DateTime VoucherDate);
        IEnumerable<St_ItemTransactionReportVM> GetAverageCost(int CompanyID, string ItemCode, DateTime VoucherDate);
        IEnumerable<St_ItemTransactionReportVM> GetAverageCostByStockCodeBetweenDate(int CompanyID, string ItemCode, string StockCode, DateTime FromDate, DateTime ToDate);
        IEnumerable<St_ItemTransactionReportVM> GetAverageCostBetweenDate(int CompanyID, string ItemCode, DateTime FromDate, DateTime ToDate);
        IEnumerable<St_AvaregCostHVM> GetAverageCostH(int CompanyID, string ItemCode, DateTime VoucherDate);
        void SaveSt_TempCalculateAverageCost(string ItemCode, DateTime VoucherDate, string VoucherNumber, int TransactionKindNo, string StockCode,
        double TotalCostLocal, double QuantityInputOutput, double SumTotalCostLocal, double SumQuantityInputOutput, double CostPieceLocal, double AverageCost,string InsUserID,DateTime InsDateTime,
        double PricePieceLocalAfterDiscount, double TotalLocalAfterDiscount, double QuantityIn,double QuantityOut,string TransactionKindName,string StockName,string Remark,string FromStockName,string ToStockName,string AccountName);
        void SaveSt_TempCalculateAverageCostH(string ItemCode, DateTime VoucherDate, string VoucherNumber, int TransactionKindNo, string StockCode, string BranchCode,
        double TotalCostLocal, double QuantityInputOutput, double SumTotalCostLocal, double SumQuantityInputOutput, double CostPieceLocal, double AverageCost, string InsUserID, DateTime InsDateTime);
        void DeleteSt_TempCalculateAverageCost(string InsUserID);
        void DeleteSt_TempCalculateAverageCostH(string InsUserID);
        IEnumerable<St_ItemTransactionReportVM> GetTempAverageCost(string UserNumber,DateTime FromDate,DateTime ToDate);



        //******//
        ResttingAccountVM GetAllAcountToRessttingByAccount(string AccountNo, int Year);
        IEnumerable<ResttingAccountVM> GetAllAcountToResstting(string AccountType, int Type, int Year);


        IEnumerable<TrialBalanceVM> GetOpenBalance(int CompanyID, DateTime FromDate, DateTime toDate);
        //****//

    }
}