using Acc.Models;
using Acc.Repositories;
using Acc.ViewModels;
using DevExpress.XtraPrinting.Export;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Acc.Persistence
{
    public class UserRepo : IUserRepo
    {
        private readonly ApplicationDbContext _context;

        public UserRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public void ChangeCurrActiveYear(string UserName, int Year)
        {
            var UserInfo = _context.Users.SingleOrDefault(m => m.UserName == UserName);
            if (UserInfo != null)
            {
                UserInfo.CurrYear = Year;
            }

        }
        public ApplicationUser GetMyInfo(string UserID)
        {
            return _context.Users.SingleOrDefault(m => m.Id == UserID);
        }

        public void UpdateCompanyID(string UserID, int CompanyID)
        {
            var UserInfo = _context.Users.SingleOrDefault(m => m.Id == UserID);
            if (UserInfo != null)
            {
                UserInfo.fCompanyId = CompanyID;
            }

        }
        public void UpdateUserPers(ApplicationUser Obj)
        {
           

            var user = _context.Users.FirstOrDefault(m => m.fCompanyId == Obj.fCompanyId && m.Id == Obj.Id);

            if (user != null)
            {
                var CoInfo = _context.Companys.FirstOrDefault(m => m.CompanyID == user.fCompanyId);
                if (CoInfo.WorkWithStock == 1)
                {
                    user.WorkWithStock = true;
                   
                }
                else
                {
                    user.WorkWithStock = false;
                }

                if (CoInfo.Hiajneh == 1)
                {
                    user.Hiajneh = true;

                }
                else
                {
                    user.Hiajneh = false;
                }

                

                user.ShowCompany = Obj.ShowCompany;
                user.AddCompany = Obj.AddCompany;
                user.UpdateCompany = Obj.UpdateCompany;
                user.DeleteCompany = Obj.DeleteCompany;
                user.PrintCompany = Obj.PrintCompany;
                user.ShowUser = Obj.ShowUser;
                user.AddUser = Obj.AddUser;
                user.UpdateUser = Obj.UpdateUser;
                user.DeleteUser = Obj.DeleteUser;
                user.PrintUser = Obj.PrintUser;
                user.ShowCurrancy = Obj.ShowCurrancy;
                user.AddCurrancy = Obj.AddCurrancy;
                user.UpdateCurrancy = Obj.UpdateCurrancy;
                user.DeleteCurrancy = Obj.DeleteCurrancy;
                user.PrintCurrancy = Obj.PrintCurrancy;
                user.ShowCurrancyValue = Obj.ShowCurrancyValue;
                user.AddCurrancyValue = Obj.AddCurrancyValue;
                user.PrintCurrancyValue = Obj.PrintCurrancyValue;
                user.ShowSalesman = Obj.ShowSalesman;
                user.AddSalesman = Obj.AddSalesman;
                user.UpdateSalesman = Obj.UpdateSalesman;
                user.DeleteSalesman = Obj.DeleteSalesman;
                user.PrintSalesman = Obj.PrintSalesman;

                user.ShowTransaction = Obj.ShowTransaction;
                user.AddTransaction = Obj.AddTransaction;
                user.UpdateTransaction = Obj.UpdateTransaction;
                user.DeleteTransaction = Obj.DeleteTransaction;
                user.PrintTransaction = Obj.PrintTransaction;
              


                user.ShowServicegroup = Obj.ShowServicegroup;
                user.AddServicegroup = Obj.AddServicegroup;
                user.UpdateServicegroup = Obj.UpdateServicegroup;
                user.DeleteServicegroup = Obj.DeleteServicegroup;
                user.PrintServicegroup = Obj.PrintServicegroup;
                user.ShowBankswithdrawnfrom = Obj.ShowBankswithdrawnfrom;
                user.AddBankswithdrawnfrom = Obj.AddBankswithdrawnfrom;
                user.UpdateBankswithdrawnfrom = Obj.UpdateBankswithdrawnfrom;
                user.PrintBankswithdrawnfrom = Obj.PrintBankswithdrawnfrom;
                user.ShowBank = Obj.ShowBank;
                user.AddBank = Obj.AddBank;
                user.UpdateBank = Obj.UpdateBank;
                user.DeleteBank = Obj.DeleteBank;
                user.PrintBank = Obj.PrintBank;
                user.ShowOtheraccount = Obj.ShowOtheraccount;
                user.AddOtheraccount = Obj.AddOtheraccount;
                user.UpdateOtheraccount = Obj.UpdateOtheraccount;
                user.DeleteOtheraccount = Obj.DeleteOtheraccount;
                user.PrintOtheraccount = Obj.PrintOtheraccount;
                user.ShowManagement = Obj.ShowManagement;
                user.AddManagement = Obj.AddManagement;
                user.UpdateManagement = Obj.UpdateManagement;
                user.DeleteManagement = Obj.DeleteManagement;
                user.PrintManagement = Obj.PrintManagement;
                user.ShowDepartment = Obj.ShowDepartment;
                user.AddDepartment = Obj.AddDepartment;
                user.UpdateDepartment = Obj.UpdateDepartment;
                user.DeleteDepartment = Obj.DeleteDepartment;
                user.PrintDepartment = Obj.PrintDepartment;
                user.ShowSections = Obj.ShowSections;
                user.AddSections = Obj.AddSections;
                user.UpdateSections = Obj.UpdateSections;
                user.DeleteSections = Obj.DeleteSections;
                user.PrintSections = Obj.PrintSections;
                user.ShowLocations = Obj.ShowLocations;
                user.AddLocations = Obj.AddLocations;
                user.UpdateLocations = Obj.UpdateLocations;
                user.DeleteLocations = Obj.DeleteLocations;
                user.PrintLocations = Obj.PrintLocations;
                user.ShowTrustee = Obj.ShowTrustee;
                user.AddTrustee = Obj.AddTrustee;
                user.UpdateTrustee = Obj.UpdateTrustee;
                user.DeleteTrustee = Obj.DeleteTrustee;
                user.PrintTrustee = Obj.PrintTrustee;
                user.ShowSupplierbanks = Obj.ShowSupplierbanks;
                user.AddSupplierbanks = Obj.AddSupplierbanks;
                user.UpdateSupplierbanks = Obj.UpdateSupplierbanks;
                user.DeleteSupplierbanks = Obj.DeleteSupplierbanks;
                user.PrintSupplierbanks = Obj.PrintSupplierbanks;
                user.ShowSuppliercountries = Obj.ShowSuppliercountries;
                user.AddSuppliercountries = Obj.AddSuppliercountries;
                user.UpdateSuppliercountries = Obj.UpdateSuppliercountries;
                user.DeleteSuppliercountries = Obj.DeleteSuppliercountries;
                user.PrintSuppliercountries = Obj.PrintSuppliercountries;
                user.ShowSupplierbankcountries = Obj.ShowSupplierbankcountries;
                user.AddSupplierbankcountries = Obj.AddSupplierbankcountries;
                user.UpdateSupplierbankcountries = Obj.UpdateSupplierbankcountries;
                user.DeleteSupplierbankcountries = Obj.DeleteSupplierbankcountries;
                user.PrintSupplierbankcountries = Obj.PrintSupplierbankcountries;
                user.ShowSupplierbankbranches = Obj.ShowSupplierbankbranches;
                user.AddSupplierbankbranches = Obj.AddSupplierbankbranches;
                user.UpdateSupplierbankbranches = Obj.UpdateSupplierbankbranches;
                user.DeleteSupplierbankbranches = Obj.DeleteSupplierbankbranches;
                user.PrintSupplierbankbranches = Obj.PrintSupplierbankbranches;
                user.ShowSuppliercity = Obj.ShowSuppliercity;
                user.AddSuppliercity = Obj.AddSuppliercity;
                user.UpdateSuppliercity = Obj.UpdateSuppliercity;
                user.DeleteSuppliercity = Obj.DeleteSuppliercity;
                user.PrintSuppliercity = Obj.PrintSuppliercity;
                user.ShowSuppliersbankcity = Obj.ShowSuppliersbankcity;
                user.AddSuppliersbankcity = Obj.AddSuppliersbankcity;
                user.UpdateSuppliersbankcity = Obj.UpdateSuppliersbankcity;
                user.DeleteSuppliersbankcity = Obj.DeleteSuppliersbankcity;
                user.PrintSuppliersbankcity = Obj.PrintSuppliersbankcity;
                user.ShowCustomercity = Obj.ShowCustomercity;
                user.AddCustomercity = Obj.AddCustomercity;
                user.UpdateCustomercity = Obj.UpdateCustomercity;
                user.DeleteCustomercity = Obj.DeleteCustomercity;
                user.PrintCustomercity = Obj.PrintCustomercity;
                user.ShowCustomerarea = Obj.ShowCustomerarea;
                user.AddCustomerarea = Obj.AddCustomerarea;
                user.UpdateCustomerarea = Obj.UpdateCustomerarea;
                user.DeleteCustomerarea = Obj.DeleteCustomerarea;
                user.PrintCustomerarea = Obj.PrintCustomerarea;
                user.ShowTransferbetweenaccounts = Obj.ShowTransferbetweenaccounts;
                user.TransferTransferbetweenaccounts = Obj.TransferTransferbetweenaccounts;
                user.ShowFreezunfreezmonthtransaction = Obj.ShowFreezunfreezmonthtransaction;
                user.FreezFreezunfreezmonthtransaction = Obj.FreezFreezunfreezmonthtransaction;
                user.unFreezFreezunfreezmonthtransaction = Obj.unFreezFreezunfreezmonthtransaction;
                user.ShowEstimatedbudget = Obj.ShowEstimatedbudget;
                user.AddEstimatedbudget = Obj.AddEstimatedbudget;

                user.ShowChartofaccount = Obj.ShowChartofaccount;
                user.AddmainaccountChartofaccount = Obj.AddmainaccountChartofaccount;
                user.UpdateChartofaccoun = Obj.UpdateChartofaccoun;
                user.DeleteChartofaccoun = Obj.DeleteChartofaccoun;
                user.AddsubaccountChartofaccount = Obj.AddsubaccountChartofaccount;
                user.UpdatesubaccountChartofaccount = Obj.UpdatesubaccountChartofaccount;
                user.DeletesubaccountChartofaccount = Obj.DeletesubaccountChartofaccount;
                user.ShowChartofcostcenter = Obj.ShowChartofcostcenter;
                user.AddmainaccountChartofcostcenter = Obj.AddmainaccountChartofcostcenter;
                user.UpdateChartofcostcenter = Obj.UpdateChartofcostcenter;
                user.DeleteChartofcostcenter = Obj.DeleteChartofcostcenter;
                user.AddsubChartofcostcenter = Obj.AddsubChartofcostcenter;
                user.UpdatesubChartofcostcenter = Obj.UpdatesubChartofcostcenter;
                user.DeletesubChartofcostcenter = Obj.DeletesubChartofcostcenter;
                user.Showcustomeraccount = Obj.Showcustomeraccount;
                user.Addcustomeraccount = Obj.Addcustomeraccount;
                user.Updatecustomeraccount = Obj.Updatecustomeraccount;
                user.Deletecustomeraccount = Obj.Deletecustomeraccount;
                user.ShowSupplieraccount = Obj.ShowSupplieraccount;
                user.AddSupplieraccount = Obj.AddSupplieraccount;
                user.UpdateSupplieraccount = Obj.UpdateSupplieraccount;
                user.DeleteSupplieraccount = Obj.DeleteSupplieraccount;
                user.ShowService = Obj.ShowService;
                user.AddService = Obj.AddService;
                user.UpdateService = Obj.UpdateService;
                user.DeleteService = Obj.DeleteService;
                user.PrintService = Obj.PrintService;
                user.RepAccountStatement = Obj.RepAccountStatement;
                user.RepTrialBalance = Obj.RepTrialBalance;
                user.RepAccountingDetailsReport = Obj.RepAccountingDetailsReport;
                user.RepChequesReport = Obj.RepChequesReport;
                user.RepCustSuppBalances = Obj.RepCustSuppBalances;
                user.RepTrialBalanceYearly = Obj.RepTrialBalanceYearly;
                user.RepTransActionDetails = Obj.RepTransActionDetails;
                user.RepTrankingChequesReport = Obj.RepTrankingChequesReport;
                user.RepSalesReport = Obj.RepSalesReport;
                user.RepExpenseAnalysisDetailReport = Obj.RepExpenseAnalysisDetailReport;
                user.RepSearchForTrans = Obj.RepSearchForTrans;
                user.RepPaymentChequesReport = Obj.RepPaymentChequesReport;
                user.RepServiceReport = Obj.RepServiceReport;
                user.RepCustomerBalancesReport = Obj.RepCustomerBalancesReport;
                user.RepPaymTrankingChequesReport = Obj.RepPaymTrankingChequesReport;
                user.RepServiceTaxReport = Obj.RepServiceTaxReport;
                user.RepSupplierBalancesReport = Obj.RepSupplierBalancesReport;
                user.RepExpenseAnalysisReport = Obj.RepExpenseAnalysisReport;
                user.RepProfitlossreport = Obj.RepProfitlossreport;
                user.RepTempPrepaidReport = Obj.RepTempPrepaidReport;
                user.RepPivotReportAccounts = Obj.RepPivotReportAccounts;
                user.RepAssetsReport = Obj.RepAssetsReport;
                user.RepTempRevenueReport = Obj.RepTempRevenueReport;

                user.RepAssetDepreciationReport = Obj.RepAssetDepreciationReport;
                user.RepEstimatedBudgetForAccount = Obj.RepEstimatedBudgetForAccount;
                user.RepConsumptionByTypeReport = Obj.RepConsumptionByTypeReport;
                user.RepEstimatedBudgetForAccountAll = Obj.RepEstimatedBudgetForAccountAll;
                user.RepDefinitionAssetSiteReport = Obj.RepDefinitionAssetSiteReport;
                user.RepEstimatedBudgetForAccountYear = Obj.RepEstimatedBudgetForAccountYear;
                user.RepSoldAsset = Obj.RepSoldAsset;
                user.RepEstimatedBudgetForCostCenters = Obj.RepEstimatedBudgetForCostCenters;
                user.RepConsumAssetReport = Obj.RepConsumAssetReport;
                user.RepEstimatedBudgetCostAllMonth = Obj.RepEstimatedBudgetCostAllMonth;
                user.RepMaintenanceAssetReport = Obj.RepMaintenanceAssetReport;
                user.RepEstimatedBudgetForCostCenterYear = Obj.RepEstimatedBudgetForCostCenterYear;
                user.RepAssetsTransferMovements = Obj.RepAssetsTransferMovements;
                user.ShowAssetType = Obj.ShowAssetType;
                user.AddAssetType = Obj.AddAssetType;
                user.UpdateAssetType = Obj.UpdateAssetType;
                user.DeleteAssetType = Obj.DeleteAssetType;
                user.PrintAssetType = Obj.PrintAssetType;
                user.ShowCalculationOfConsumption = Obj.ShowCalculationOfConsumption;
                user.AddCalculationOfConsumption = Obj.AddCalculationOfConsumption;
                user.ShowAsset = Obj.ShowAsset;
                user.AddAsset = Obj.AddAsset;
                user.UpdateAsset = Obj.UpdateAsset;
                user.DeleteAsset = Obj.DeleteAsset;
                user.PrintAsset = Obj.PrintAsset;
                user.ShowAssetToAcc = Obj.ShowAssetToAcc;
                user.AddAssetToAcc = Obj.AddAssetToAcc;
                user.ShowDeliveryAssetTrustee = Obj.ShowDeliveryAssetTrustee;
                user.AddDeliveryAssetTrustee = Obj.AddDeliveryAssetTrustee;
                user.ShowConsumAsset = Obj.ShowConsumAsset;
                user.AddConsumAsset = Obj.AddConsumAsset;
                user.ShowDefinitionAsset = Obj.ShowDefinitionAsset;
                user.AddDefinitionAsset = Obj.AddDefinitionAsset;
                user.ShowSaleAsset = Obj.ShowSaleAsset;
                user.AddShowSaleAsset = Obj.AddShowSaleAsset;
                user.ShowAssetMaintenance = Obj.ShowAssetMaintenance;
                user.AddAssetMaintenance = Obj.AddAssetMaintenance;
                user.UpdateAssetMaintenance = Obj.UpdateAssetMaintenance;
                user.DeleteAssetMaintenance = Obj.DeleteAssetMaintenance;

                user.PrintRepAccountStatement = Obj.PrintRepAccountStatement;

                user.PrintRepTrialBalance = Obj.PrintRepTrialBalance;

                user.PrintRepAccountingDetailsPrintReport = Obj.PrintRepAccountingDetailsPrintReport;

                user.PrintRepChequesPrintReport = Obj.PrintRepChequesPrintReport;

                user.PrintRepCustSuppBalances = Obj.PrintRepCustSuppBalances;

                user.PrintRepTrialBalanceYearly = Obj.PrintRepTrialBalanceYearly;

                user.PrintRepTransActionDetails = Obj.PrintRepTransActionDetails;

                user.PrintRepTrankingChequesPrintReport = Obj.PrintRepTrankingChequesPrintReport;

                user.PrintRepSalesPrintReport = Obj.PrintRepSalesPrintReport;

                user.PrintRepExpenseAnalysisDetailPrintReport = Obj.PrintRepExpenseAnalysisDetailPrintReport;

                user.PrintRepSearchForTrans = Obj.PrintRepSearchForTrans;

                user.PrintRepPaymentChequesPrintReport = Obj.PrintRepPaymentChequesPrintReport;

                   user.PrintRepServicePrintReport = Obj.PrintRepServicePrintReport;

                user.PrintRepCustomerBalancesPrintReport = Obj.PrintRepCustomerBalancesPrintReport;

                user.PrintRepExpenseAnalysisPrintReport = Obj.PrintRepExpenseAnalysisPrintReport;

                 user.PrintRepPaymTrankingChequesPrintReport = Obj.PrintRepPaymTrankingChequesPrintReport;

                 user.PrintRepTempPPrintRepaidPrintReport = Obj.PrintRepTempPPrintRepaidPrintReport;

                user.PrintRepServiceTaxPrintReport = Obj.PrintRepServiceTaxPrintReport;

                user.PrintRepSupplierBalancesPrintReport = Obj.PrintRepSupplierBalancesPrintReport;



                 user.PrintRepProfitlossPrintReport = Obj.PrintRepProfitlossPrintReport;



                user.PrintRepPivotPrintReportAccounts = Obj.PrintRepPivotPrintReportAccounts;

                user.PrintRepAssetsPrintReport = Obj.PrintRepAssetsPrintReport;

                user.PrintRepDefinitionAssetSitePrintReport = Obj.PrintRepDefinitionAssetSitePrintReport;

                user.PrintRepTempRevenuePrintReport = Obj.PrintRepTempRevenuePrintReport;

                user.PrintRepAssetDepreciationPrintReport = Obj.PrintRepAssetDepreciationPrintReport;

                user.PrintRepEstimatedBudgetForAccount = Obj.PrintRepEstimatedBudgetForAccount;

                user.PrintRepConsumAssetPrintReport = Obj.PrintRepConsumAssetPrintReport;


                user.PrintRepConsumptionByTypePrintReport = Obj.PrintRepConsumptionByTypePrintReport;

                user.PrintRepEstimatedBudgetForAccountAll = Obj.PrintRepEstimatedBudgetForAccountAll;



                user.PrintRepSoldAsset = Obj.PrintRepSoldAsset;





                user.PrintRepEstimatedBudgetForCostCenters = Obj.PrintRepEstimatedBudgetForCostCenters;



                user.PrintRepEstimatedBudgetCostAllMonth = Obj.PrintRepEstimatedBudgetCostAllMonth;



                user.PrintRepMaintenanceAssetPrintReport = Obj.PrintRepMaintenanceAssetPrintReport;



                user.PrintRepEstimatedBudgetForCostCenterYear = Obj.PrintRepEstimatedBudgetForCostCenterYear;


                user.PrintRepAssetsTransferMovements = Obj.PrintRepAssetsTransferMovements;

                user.PrintRepEstimatedBudgetForAccountYear = Obj.PrintRepEstimatedBudgetForAccountYear;


                user.ShowTransActionTrans = Obj.ShowTransActionTrans;
                user.AddTransActionTrans = Obj.AddTransActionTrans;
                user.DeleteTransActionTrans = Obj.DeleteTransActionTrans;
                user.UpdateTransActionTrans = Obj.UpdateTransActionTrans;
                user.CopyTransActionTrans = Obj.CopyTransActionTrans;
                user.PrintTransActionTrans = Obj.PrintTransActionTrans;
                user.ExportTransActionTrans = Obj.ExportTransActionTrans;
                user.UnExportTransActionTrans = Obj.UnExportTransActionTrans;
                user.AttachTransActionTrans= Obj.AttachTransActionTrans;



                user.ShowReceiptVoucherBank = Obj.ShowReceiptVoucherBank;
                user.AddReceiptVoucherBank = Obj.AddReceiptVoucherBank;
                user.DeleteReceiptVoucherBank = Obj.DeleteReceiptVoucherBank;
                user.CopyReceiptVoucherBank = Obj.CopyReceiptVoucherBank;
                user.UpdateReceiptVoucherBank = Obj.UpdateReceiptVoucherBank;
                user.PrintReceiptVoucherBank = Obj.PrintReceiptVoucherBank;
                user.ExportReceiptVoucherBank = Obj.ExportReceiptVoucherBank;
                user.UnExportReceiptVoucherBank = Obj.ExportReceiptVoucherBank;
                user.AttachReceiptVoucherBank = Obj.AttachReceiptVoucherBank;


                user.ShowTempPrepaid = Obj.ShowTempPrepaid;
                user.AddTempPrepaid = Obj.AddTempPrepaid;
                user.DeleteTempPrepaid = Obj.DeleteTempPrepaid;
                user.CopyTempPrepaid = Obj.CopyTempPrepaid;
                user.UpdateTempPrepaid = Obj.UpdateTempPrepaid;
                user.PrintTempPrepaid = Obj.PrintTempPrepaid;
                user.AccumulativeTempPrepaid = Obj.AccumulativeTempPrepaid;
                user.AttachTempPrepaid = Obj.AttachTempPrepaid;




                user.ShowDebitNote = Obj.ShowDebitNote;
                user.AddDebitNote = Obj.AddDebitNote;
                user.DeleteDebitNote = Obj.DeleteDebitNote;
                user.CopyDebitNote = Obj.CopyDebitNote;
                user.UpdateDebitNote = Obj.UpdateDebitNote;
                user.PrintDebitNote = Obj.PrintDebitNote;
                user.ExportDebitNote = Obj.ExportDebitNote;
                user.UnExportDebitNote = Obj.UnExportDebitNote;
                user.AttachDebitNote = Obj.AttachDebitNote;




                user.ShowServiceBill = Obj.ShowServiceBill;
                user.AddServiceBill = Obj.AddServiceBill;
                user.DeleteServiceBill = Obj.DeleteServiceBill;
                user.CopyServiceBill = Obj.CopyServiceBill;
                user.UpdateServiceBill = Obj.UpdateServiceBill;
                user.PrintServiceBill = Obj.PrintServiceBill;
                user.ExportServiceBill = Obj.ExportServiceBill;
                user.UnExportServiceBill = Obj.UnExportServiceBill;

                user.AttachServiceBill = Obj.AttachServiceBill;



               // user.ShowReceiptVoucherBankReceivedCheque = Obj.ShowReceiptVoucherBankReceivedCheque;

                //user.AddReceiptVoucherBankReceivedCheque = Obj.AddReceiptVoucherBankReceivedCheque;
                //user.AddReceiptVoucherBankReceivedCheque = Obj.AddReceiptVoucherBankReceivedCheque;
                //user.DeleteReceiptVoucherBankReceivedCheque = Obj.DeleteReceiptVoucherBankReceivedCheque;
                //user.CopyReceiptVoucherBankReceivedCheque = Obj.CopyReceiptVoucherBankReceivedCheque;
                //user.UpdateReceiptVoucherBankReceivedCheque = Obj.UpdateReceiptVoucherBankReceivedCheque;
                //user.PrintReceiptVoucherBankReceivedCheque = Obj.PrintReceiptVoucherBankReceivedCheque;

                user.ShowOpeningBalance = Obj.ShowOpeningBalance;
                user.AddOpeningBalance = Obj.AddOpeningBalance;
                user.DeleteOpeningBalance = Obj.DeleteOpeningBalance;
                user.CopyOpeningBalance = Obj.CopyOpeningBalance;
                user.UpdateOpeningBalance = Obj.UpdateOpeningBalance;
                user.PrintOpeningBalance = Obj.PrintOpeningBalance;

                user.ShowReceiptVoucherCash = Obj.ShowReceiptVoucherCash;
                user.AddReceiptVoucherCash = Obj.AddReceiptVoucherCash;
                user.DeleteReceiptVoucherCash = Obj.DeleteReceiptVoucherCash;
                user.CopyReceiptVoucherCash = Obj.CopyReceiptVoucherCash;
                user.UpdateReceiptVoucherCash = Obj.UpdateReceiptVoucherCash;
                user.PrintReceiptVoucherCash = Obj.PrintReceiptVoucherCash;
                user.ExportReceiptVoucherCash = Obj.ExportReceiptVoucherCash;
                user.UnExportReceiptVoucherCash = Obj.UnExportReceiptVoucherCash;
                user.AttachReceiptVoucherCash = Obj.AttachReceiptVoucherCash;




                user.ShowTempRevenueReceived = Obj.ShowTempRevenueReceived;
                user.AddTempRevenueReceived = Obj.AddTempRevenueReceived;
                user.DeleteTempRevenueReceived = Obj.DeleteTempRevenueReceived;
                user.CopyTempRevenueReceived = Obj.CopyTempRevenueReceived;
                user.UpdateTempRevenueReceived = Obj.UpdateTempRevenueReceived;
                user.PrintTempRevenueReceived = Obj.PrintTempRevenueReceived;
                user.AccumulativeTempRevenueReceived = Obj.AccumulativeTempRevenueReceived;
                user.AttachTempRevenueReceived = Obj.AttachTempRevenueReceived;
                

                user.ShowCreditNote = Obj.ShowCreditNote;
                user.AddCreditNote = Obj.AddCreditNote;
                user.DeleteCreditNote = Obj.DeleteCreditNote;
                user.CopyCreditNote = Obj.CopyCreditNote;
                user.UpdateCreditNote = Obj.UpdateCreditNote;
                user.PrintCreditNote = Obj.PrintCreditNote;
                user.ExportCreditNote = Obj.ExportCreditNote;
                user.UnExportCreditNote = Obj.UnExportCreditNote;
                user.AttachCreditNote = Obj.AttachCreditNote;





              //  user.ShowPaymentVoucherBankPaymentCheque = Obj.ShowPaymentVoucherBankPaymentCheque;
                //user.AddPaymentVoucherBankPaymentCheque = Obj.AddPaymentVoucherBankPaymentCheque;
                //user.DeletePaymentVoucherBankPaymentCheque = Obj.DeletePaymentVoucherBankPaymentCheque;
                //user.CopyPaymentVoucherBankPaymentCheque = Obj.CopyPaymentVoucherBankPaymentCheque;
                //user.UpdatePaymentVoucherBankPaymentCheque = Obj.UpdatePaymentVoucherBankPaymentCheque;
                //user.PrintPaymentVoucherBankPaymentCheque = Obj.PrintPaymentVoucherBankPaymentCheque;




                user.ShowPaymentVoucherBank = Obj.ShowPaymentVoucherBank;
                user.AddPaymentVoucherBank = Obj.AddPaymentVoucherBank;
                user.DeletePaymentVoucherBank = Obj.DeletePaymentVoucherBank;
                user.CopyPaymentVoucherBank = Obj.CopyPaymentVoucherBank;
                user.UpdatePaymentVoucherBank = Obj.UpdatePaymentVoucherBank;
                user.PrintPaymentVoucherBank = Obj.PrintPaymentVoucherBank;
                user.ExportPaymentVoucherBank = Obj.ExportPaymentVoucherBank;

                user.UnExportPaymentVoucherBank = Obj.UnExportPaymentVoucherBank;
                user.AttachPaymentVoucherBank = Obj.AttachPaymentVoucherBank;







                user.ShowPaymentVoucherCash = Obj.ShowPaymentVoucherCash;
                user.AddPaymentVoucherCash = Obj.AddPaymentVoucherCash;
                user.DeletePaymentVoucherCash = Obj.DeletePaymentVoucherCash;
                user.CopyPaymentVoucherCash = Obj.CopyPaymentVoucherCash;
                user.UpdatePaymentVoucherCash = Obj.UpdatePaymentVoucherCash;
                user.PrintPaymentVoucherCash = Obj.PrintPaymentVoucherCash;
                user.ExportPaymentVoucherCash = Obj.ExportPaymentVoucherCash;
                user.UnExportPaymentVoucherCash = Obj.UnExportPaymentVoucherCash;
                user.AttachPaymentVoucherCash = Obj.AttachPaymentVoucherCash;





                user.ShowDepositInTheBank = Obj.ShowDepositInTheBank;
                user.AddDepositInTheBank = Obj.AddDepositInTheBank;
                user.DeleteDepositInTheBank = Obj.DeleteDepositInTheBank;
                user.CopyDepositInTheBank = Obj.CopyDepositInTheBank;
                user.UpdateDepositInTheBank = Obj.UpdateDepositInTheBank;
                user.PrintDepositInTheBank = Obj.PrintDepositInTheBank;
                user.ExportDepositInTheBank = Obj.ExportDepositInTheBank;

                user.UnExportDepositInTheBank = Obj.UnExportDepositInTheBank;

                user.AttachDepositInTheBank = Obj.AttachDepositInTheBank;

                user.ShowReturnACheque = Obj.ShowReturnACheque;
                user.ShowTransferFromFundUC = Obj.ShowTransferFromFundUC;
                user.ShowDrawingUC = Obj.ShowDrawingUC;
                user.ShowPaymentchequeUC = Obj.ShowPaymentchequeUC;
                user.ShowTransferFromFundCD = Obj.ShowTransferFromFundCD;
                user.ShowReturnAChequeCD = Obj.ShowReturnAChequeCD;
                user.ShowTransferFromFundToChequeE = Obj.ShowTransferFromFundToChequeE;
                user.ShowPaymentChequeEndorsement = Obj.ShowPaymentChequeEndorsement;
                user.ShowReturnChequeUnderCollection = Obj.ShowReturnChequeUnderCollection;
                user.ShowReturnChequeEndorsement = Obj.ShowReturnChequeEndorsement;
                user.ShowFundChequesDrawnFromUC = Obj.ShowFundChequesDrawnFromUC;
                user.ShowReturnedChequeFund = Obj.ShowReturnedChequeFund;
                user.ShowCourtFund = Obj.ShowCourtFund;
                user.ShowPostdatedCheques = Obj.ShowPostdatedCheques;
                user.ShowReturnPostdatedCheques = Obj.ShowReturnPostdatedCheques;
                user.ShowReturnPaidPostdatedCheques = Obj.ShowReturnPaidPostdatedCheques;

                user.ShowReceiptVoucherCashMultiAccount = Obj.ShowReceiptVoucherCashMultiAccount;
                user.AddReceiptVoucherCashMultiAccount = Obj.AddReceiptVoucherCashMultiAccount;
                user.DeleteReceiptVoucherCashMultiAccount = Obj.DeleteReceiptVoucherCashMultiAccount;
                user.UpdateReceiptVoucherCashMultiAccount = Obj.UpdateReceiptVoucherCashMultiAccount;
                user.CopyReceiptVoucherCashMultiAccount = Obj.CopyReceiptVoucherCashMultiAccount;
                user.PrintReceiptVoucherCashMultiAccount = Obj.PrintReceiptVoucherCashMultiAccount;
                user.ExportReceiptVoucherCashMultiAccount = Obj.ExportReceiptVoucherCashMultiAccount;
                user.UnExportReceiptVoucherCashMultiAccount = Obj.UnExportReceiptVoucherCashMultiAccount;
                user.AttachReceiptVoucherCashMultiAccount = Obj.AttachReceiptVoucherCashMultiAccount;



                user.ShowPaymentVoucherCashMultiAccount = Obj.ShowPaymentVoucherCashMultiAccount;
                user.AddPaymentVoucherCashMultiAccount = Obj.AddPaymentVoucherCashMultiAccount;
                user.DeletePaymentVoucherCashMultiAccount = Obj.DeletePaymentVoucherCashMultiAccount;
                user.UpdatePaymentVoucherCashMultiAccount = Obj.UpdatePaymentVoucherCashMultiAccount;
                user.CopyPaymentVoucherCashMultiAccount = Obj.CopyPaymentVoucherCashMultiAccount;
                user.PrintPaymentVoucherCashMultiAccount = Obj.PrintPaymentVoucherCashMultiAccount;
                user.ExportPaymentVoucherCashMultiAccount = Obj.ExportPaymentVoucherCashMultiAccount;
                user.UnExportPaymentVoucherCashMultiAccount = Obj.UnExportPaymentVoucherCashMultiAccount;
                user.AttachPaymentVoucherCashMultiAccount = Obj.AttachPaymentVoucherCashMultiAccount;


            }

        }
        public async Task<bool> RemoveUserPermission(ApplicationUser model)
        {
            var store = new UserStore<ApplicationUser>(_context);
            var UserManager = new ApplicationUserManager(store);

            var user = _context.Users.FirstOrDefault(m => m.fCompanyId == model.fCompanyId && m.Id == model.Id);

            if (user != null)
            {


                user = model;


               
                string[] roleNames = {
               
                "ShowCompany","UpdateCompany", //Company Screen
				 "ShowUser","AddUser" ,"UpdateUser","DeleteUser","PrintUser", //User Screen
                 "ShowCurrancy","AddCurrancy" ,"UpdateCurrancy","DeleteCurrancy","PrintCurrancy", //Currancy Screen
                 "ShowCurrancyValue","AddCurrancyValue","PrintCurrancyValue", //CurrancyValue Screen
                 "ShowSalesman","AddSalesman" ,"UpdateSalesman","DeleteSalesman","PrintSalesman", //Salesman Screen
                 "ShowTransaction","AddTransaction" ,"UpdateTransaction","DeleteTransaction","PrintTransaction", //Transaction Screen
                 "ShowServicegroup","AddServicegroup" ,"UpdateServicegroup","DeleteServicegroup","PrintServicegroup", //Servicegroup Screen
                 "ShowBankswithdrawnfrom","AddBankswithdrawnfrom" ,"UpdateBankswithdrawnfrom","PrintBankswithdrawnfrom", //Bankswithdrawn Screen
                 "ShowBank","AddBank" ,"UpdateBank","DeleteBank","PrintBank", //Bank Screen
                 "ShowOtheraccount","AddOtheraccount" ,"UpdateOtheraccount","DeleteOtheraccount","PrintOtheraccount", //Otheraccount Screen
                 "ShowManagement","AddManagement" ,"UpdateManagement","DeleteManagement","PrintManagement", //Management Screen
                 "ShowDepartment","AddDepartment" ,"UpdateDepartment","DeleteDepartment","PrintDepartment", //Department Screen
                 "ShowSections","AddSections" ,"UpdateSections","DeleteSections","PrintSections", //Sections Screen
                 "ShowLocations","AddLocations" ,"UpdateLocations","DeleteLocations","PrintLocations", //Locations Screen
                 "ShowTrustee","AddTrustee" ,"UpdateTrustee","DeleteTrustee","PrintTrustee", //Trustee Screen
                 "ShowSupplierbanks","AddSupplierbanks" ,"UpdateSupplierbanks","DeleteSupplierbanks","PrintSupplierbanks", //Supplierbanks Screen
                 "ShowSuppliercountries","AddSuppliercountries" ,"UpdateSuppliercountries","DeleteSuppliercountries","PrintSuppliercountries", //Suppliercountries Screen
                 "ShowSupplierbankcountries","AddSupplierbankcountries" ,"UpdateSupplierbankcountries","DeleteSupplierbankcountries","PrintSupplierbankcountries", //Supplierbankcountries Screen                                                                               
                 "ShowSupplierbankbranches","AddSupplierbankbranches" ,"UpdateSupplierbankbranches","DeleteSupplierbankbranches","PrintSupplierbankbranches", //Supplierbankbranches Screen      
                 "ShowSuppliercity","AddSuppliercity" ,"UpdateSuppliercity","DeleteSuppliercity","PrintSuppliercity", //Suppliercity Screen      
                 "ShowSuppliersbankcity","AddSuppliersbankcity" ,"UpdateSuppliersbankcity","DeleteSuppliersbankcity","PrintSuppliersbankcity", //Suppliersbankcity Screen      
                 "ShowCustomercity","AddCustomercity" ,"UpdateCustomercity","DeleteCustomercity","PrintCustomercity", //Customercity Screen      
                 "ShowCustomerarea","AddCustomerarea" ,"UpdateCustomerarea","DeleteCustomerarea","PrintCustomerarea", //Customerarea Screen  
                  "ShowService","AddService" ,"UpdateService","DeleteService","PrintService",
                
                                     //--end company
	       //Operations//
           
                     "ShowTransferbetweenaccounts","TransferTransferbetweenaccounts", //Transferbetweenaccounts Screen      
                     "ShowFreezunfreezmonthtransaction","FreezFreezunfreezmonthtransaction","unFreezFreezunfreezmonthtransaction", //Freezunfreezmonthtransaction Screen   
                     "ShowEstimatedbudget","AddEstimatedbudget", //Estimatedbudget Screen
               //--end Operations


               //ِAccounts//

                      "ShowChartofaccount","AddmainaccountChartofaccount","UpdateChartofaccoun","DeleteChartofaccoun" , //Chartofaccoun Screen  
                      "AddsubaccountChartofaccount","UpdatesubaccountChartofaccount","DeletesubaccountChartofaccount",
                      "ShowChartofcostcenter","AddmainaccountChartofcostcenter","UpdateChartofcostcenter","DeleteChartofcostcenter",
                      "AddsubChartofcostcenter","UpdatesubChartofcostcenter","DeletesubChartofcostcenter",
                      "Showcustomeraccount","Addcustomeraccount", "Updatecustomeraccount","Deletecustomeraccount",//Newcustomeraccount Screen      
                      "ShowSupplieraccount","AddSupplieraccount","UpdateSupplieraccount","DeleteSupplieraccount", //NewSupplieraccount Screen  

            //--end Accounts
                      
  //-----Report---//
                "RepAccountStatement","RepTrialBalance", "RepAccountingDetailsReport","RepChequesReport", "RepCustSuppBalances",
                "RepTrialBalanceYearly", "RepTransActionDetails","RepTrankingChequesReport",
                "RepSalesReport","RepExpenseAnalysisDetailReport", "RepSearchForTrans","RepPaymentChequesReport",
                "RepServiceReport","RepCustomerBalancesReport", "RepPaymTrankingChequesReport",
                "RepServiceTaxReport", "RepSupplierBalancesReport",
                "RepExpenseAnalysisReport",
                "RepProfitlossreport",
                "RepTempPrepaidReport", "RepPivotReportAccounts","RepAssetsReport",
                "RepTempRevenueReport","RepAssetDepreciationReport",
                "RepEstimatedBudgetForAccount","RepConsumptionByTypeReport",
                "RepEstimatedBudgetForAccountAll","RepDefinitionAssetSiteReport",
                "RepEstimatedBudgetForAccountYear","RepSoldAsset",
                "RepEstimatedBudgetForCostCenters", "RepConsumAssetReport",
                "RepEstimatedBudgetCostAllMonth", "RepMaintenanceAssetReport",
                "RepEstimatedBudgetForCostCenterYear", "RepAssetsTransferMovements",

                //---Asset--//
                "ShowAssetType","AddAssetType","UpdateAssetType","DeleteAssetType","PrintAssetType",
                "ShowCalculationOfConsumption","AddCalculationOfConsumption",
                "ShowAsset","AddAsset","UpdateAsset","DeleteAsset","PrintAsset",
                "ShowAssetToAcc","AddAssetToAcc",
                "ShowDefinitionAsset","AddDefinitionAsset",
                "ShowSaleAsset","AddShowSaleAsset",
                "ShowDeliveryAssetTrustee","AddDeliveryAssetTrustee",
                "ShowConsumAsset","AddConsumAsset",
                "ShowAssetMaintenance", "AddAssetMaintenance","UpdateAssetMaintenance","DeleteAssetMaintenance","WorkWithStock","Hiajneh"




            };
                var userid = model.Id;
                var roles = await UserManager.GetRolesAsync(userid);
                await UserManager.RemoveFromRolesAsync(userid, roles.ToArray());
                //foreach (var roleName in roleNames)
                //{
                //    await UserManager.RemoveFromRoleAsync(user.Id, roleName);
                //}
               
      

            }
            return true;
        }

        public async Task<bool> UpdateUserPermission(ApplicationUser model)
        {
            var store = new UserStore<ApplicationUser>(_context);
            var UserManager = new ApplicationUserManager(store);

            var user = _context.Users.FirstOrDefault(m => m.fCompanyId == model.fCompanyId && m.Id == model.Id);

            if (user != null)
            {


                user = model;



                string[] roleNames = {
                 "CoUser",
                "ShowCompany","UpdateCompany", //Company Screen
				 "ShowUser","AddUser" ,"UpdateUser","DeleteUser","PrintUser", //User Screen
                 "ShowCurrancy","AddCurrancy" ,"UpdateCurrancy","DeleteCurrancy","PrintCurrancy", //Currancy Screen
                 "ShowCurrancyValue","AddCurrancyValue","PrintCurrancyValue", //CurrancyValue Screen
                 "ShowSalesman","AddSalesman" ,"UpdateSalesman","DeleteSalesman","PrintSalesman", //Salesman Screen
                 "ShowTransaction","AddTransaction" ,"UpdateTransaction","DeleteTransaction","PrintTransaction", //Transaction Screen
                 "ShowServicegroup","AddServicegroup" ,"UpdateServicegroup","DeleteServicegroup","PrintServicegroup", //Servicegroup Screen
                 "ShowBankswithdrawnfrom","AddBankswithdrawnfrom" ,"UpdateBankswithdrawnfrom","PrintBankswithdrawnfrom", //Bankswithdrawn Screen
                 "ShowBank","AddBank" ,"UpdateBank","DeleteBank","PrintBank", //Bank Screen
                 "ShowOtheraccount","AddOtheraccount" ,"UpdateOtheraccount","DeleteOtheraccount","PrintOtheraccount", //Otheraccount Screen
                 "ShowManagement","AddManagement" ,"UpdateManagement","DeleteManagement","PrintManagement", //Management Screen
                 "ShowDepartment","AddDepartment" ,"UpdateDepartment","DeleteDepartment","PrintDepartment", //Department Screen
                 "ShowSections","AddSections" ,"UpdateSections","DeleteSections","PrintSections", //Sections Screen
                 "ShowLocations","AddLocations" ,"UpdateLocations","DeleteLocations","PrintLocations", //Locations Screen
                 "ShowTrustee","AddTrustee" ,"UpdateTrustee","DeleteTrustee","PrintTrustee", //Trustee Screen
                 "ShowSupplierbanks","AddSupplierbanks" ,"UpdateSupplierbanks","DeleteSupplierbanks","PrintSupplierbanks", //Supplierbanks Screen
                 "ShowSuppliercountries","AddSuppliercountries" ,"UpdateSuppliercountries","DeleteSuppliercountries","PrintSuppliercountries", //Suppliercountries Screen
                 "ShowSupplierbankcountries","AddSupplierbankcountries" ,"UpdateSupplierbankcountries","DeleteSupplierbankcountries","PrintSupplierbankcountries", //Supplierbankcountries Screen                                                                               
                 "ShowSupplierbankbranches","AddSupplierbankbranches" ,"UpdateSupplierbankbranches","DeleteSupplierbankbranches","PrintSupplierbankbranches", //Supplierbankbranches Screen      
                 "ShowSuppliercity","AddSuppliercity" ,"UpdateSuppliercity","DeleteSuppliercity","PrintSuppliercity", //Suppliercity Screen      
                 "ShowSuppliersbankcity","AddSuppliersbankcity" ,"UpdateSuppliersbankcity","DeleteSuppliersbankcity","PrintSuppliersbankcity", //Suppliersbankcity Screen      
                 "ShowCustomercity","AddCustomercity" ,"UpdateCustomercity","DeleteCustomercity","PrintCustomercity", //Customercity Screen      
                 "ShowCustomerarea","AddCustomerarea" ,"UpdateCustomerarea","DeleteCustomerarea","PrintCustomerarea", //Customerarea Screen  
                  "ShowService","AddService" ,"UpdateService","DeleteService","PrintService",
                
                                     //--end company
	       //Operations//
           
                     "ShowTransferbetweenaccounts","TransferTransferbetweenaccounts", //Transferbetweenaccounts Screen      
                     "ShowFreezunfreezmonthtransaction","FreezFreezunfreezmonthtransaction","unFreezFreezunfreezmonthtransaction", //Freezunfreezmonthtransaction Screen   
                     "ShowEstimatedbudget","AddEstimatedbudget", //Estimatedbudget Screen
               //--end Operations


               //ِAccounts//

                      "ShowChartofaccount","AddmainaccountChartofaccount","UpdateChartofaccoun","DeleteChartofaccoun" , //Chartofaccoun Screen  
                      "AddsubaccountChartofaccount","UpdatesubaccountChartofaccount","DeletesubaccountChartofaccount",
                      "ShowChartofcostcenter","AddmainaccountChartofcostcenter","UpdateChartofcostcenter","DeleteChartofcostcenter",
                      "AddsubChartofcostcenter","UpdatesubChartofcostcenter","DeletesubChartofcostcenter",
                      "Showcustomeraccount","Addcustomeraccount", "Updatecustomeraccount","Deletecustomeraccount",//Newcustomeraccount Screen      
                      "ShowSupplieraccount","AddSupplieraccount","UpdateSupplieraccount","DeleteSupplieraccount", //NewSupplieraccount Screen  

            //--end Accounts
                      
  //-----Report---//
                "RepAccountStatement","RepTrialBalance", "RepAccountingDetailsReport","RepChequesReport", "RepCustSuppBalances",
                "RepTrialBalanceYearly", "RepTransActionDetails","RepTrankingChequesReport",
                "RepSalesReport","RepExpenseAnalysisDetailReport", "RepSearchForTrans","RepPaymentChequesReport",
                "RepServiceReport","RepCustomerBalancesReport", "RepPaymTrankingChequesReport",
                "RepServiceTaxReport", "RepSupplierBalancesReport",
                "RepExpenseAnalysisReport",
                "RepProfitlossreport",
                "RepTempPrepaidReport", "RepPivotReportAccounts","RepAssetsReport",
                "RepTempRevenueReport","RepAssetDepreciationReport",
                "RepEstimatedBudgetForAccount","RepConsumptionByTypeReport",
                "RepEstimatedBudgetForAccountAll","RepDefinitionAssetSiteReport",
                "RepEstimatedBudgetForAccountYear","RepSoldAsset",
                "RepEstimatedBudgetForCostCenters", "RepConsumAssetReport",
                "RepEstimatedBudgetCostAllMonth", "RepMaintenanceAssetReport",
                "RepEstimatedBudgetForCostCenterYear", "RepAssetsTransferMovements",




                 "PrintRepAccountStatement","PrintRepTrialBalance", "PrintRepAccountingDetailsPrintReport","PrintRepChequesPrintReport", "PrintRepCustSuppBalances",
                "PrintRepTrialBalanceYearly", "PrintRepTransActionDetails","PrintRepTrankingChequesPrintReport",
                "PrintRepSalesPrintReport","PrintRepExpenseAnalysisDetailPrintReport", "PrintRepSearchForTrans","PrintRepPaymentChequesPrintReport",
                "PrintRepServicePrintReport","PrintRepCustomerBalancesPrintReport", "PrintRepPaymTrankingChequesPrintReport",
                "PrintRepServiceTaxPrintReport", "PrintRepSupplierBalancesPrintReport",
                "PrintRepExpenseAnalysisPrintReport",
                "PrintRepProfitlossPrintReport",
                "PrintRepTempPPrintRepaidPrintReport", "PrintRepPivotPrintReportAccounts","PrintRepAssetsPrintReport",
                "PrintRepTempRevenuePrintReport","PrintRepAssetDepreciationPrintReport",
                "PrintRepEstimatedBudgetForAccount","PrintRepConsumptionByTypePrintReport",
                "PrintRepEstimatedBudgetForAccountAll","PrintRepDefinitionAssetSitePrintReport",
                "PrintRepEstimatedBudgetForAccountYear","PrintRepSoldAsset",
                "PrintRepEstimatedBudgetForCostCenters", "PrintRepConsumAssetPrintReport",
                "PrintRepEstimatedBudgetCostAllMonth", "PrintRepMaintenanceAssetPrintReport",
                "PrintRepEstimatedBudgetForCostCenterYear", "PrintRepAssetsTransferMovements",



                //---Asset--//
                "ShowAssetType","AddAssetType","UpdateAssetType","DeleteAssetType","PrintAssetType",
                "ShowCalculationOfConsumption","AddCalculationOfConsumption",
                "ShowAsset","AddAsset","UpdateAsset","DeleteAsset","PrintAsset",
                "ShowAssetToAcc","AddAssetToAcc",
                "ShowDefinitionAsset","AddDefinitionAsset",
                "ShowSaleAsset","AddShowSaleAsset",
                "ShowDeliveryAssetTrustee","AddDeliveryAssetTrustee",
                "ShowConsumAsset","AddConsumAsset",
                "ShowAssetMaintenance", "AddAssetMaintenance","UpdateAssetMaintenance","DeleteAssetMaintenance",


                //----TransAction--//

                 "ShowTransActionTrans","AddTransActionTrans","DeleteTransActionTrans","UpdateTransActionTrans","CopyTransActionTrans","PrintTransActionTrans",  "ExportTransActionTrans" ,"UnExportTransActionTrans", "AttachTransActionTrans",

               "ShowReceiptVoucherBank","AddReceiptVoucherBank","DeleteReceiptVoucherBank","CopyReceiptVoucherBank","UpdateReceiptVoucherBank","PrintReceiptVoucherBank",  "ExportReceiptVoucherBank" ,"UnExportReceiptVoucherBank", "AttachReceiptVoucherBank",

               "ShowTempPrepaid","AddTempPrepaid","DeleteTempPrepaid","CopyTempPrepaid","UpdateTempPrepaid","PrintTempPrepaid", "AccumulativeTempPrepaid","AttachTempPrepaid",

               "ShowDebitNote","AddDebitNote","DeleteDebitNote","CopyDebitNote","UpdateDebitNote","PrintDebitNote", "ExportDebitNote", "UnExportDebitNote",  "AttachDebitNote",

               "ShowServiceBill","AddServiceBill","DeleteServiceBill","CopyServiceBill","UpdateServiceBill","PrintServiceBill", "AttachServiceBill", "ExportServiceBill", "UnExportServiceBill",

              // "ShowReceiptVoucherBankReceivedCheque","AddReceiptVoucherBankReceivedCheque","DeleteReceiptVoucherBankReceivedCheque","CopyReceiptVoucherBankReceivedCheque","UpdateReceiptVoucherBankReceivedCheque","PrintReceiptVoucherBankReceivedCheque",

               "ShowOpeningBalance","AddOpeningBalance","DeleteReceiptVoucherCash","CopyOpeningBalance","UpdateOpeningBalance", "PrintOpeningBalance",

               "ShowReceiptVoucherCash","AddReceiptVoucherCash","DeleteReceiptVoucherCash","CopyReceiptVoucherCash","UpdateReceiptVoucherCash", "PrintReceiptVoucherCash", "ExportReceiptVoucherCash" ,"UnExportReceiptVoucherCash", "AttachReceiptVoucherCash",

               "ShowTempRevenueReceived","AddTempRevenueReceived","DeleteTempRevenueReceived","CopyTempRevenueReceived","UpdateTempRevenueReceived","PrintTempRevenueReceived", "AccumulativeTempRevenueReceived","AttachTempRevenueReceived",

               "ShowCreditNote","AddCreditNote","DeleteCreditNote","CopyCreditNote","UpdateCreditNote","PrintCreditNote", "ExportCreditNote", "UnExportCreditNote", "AttachCreditNote",

               //"ShowPaymentVoucherBankPaymentCheque","AddPaymentVoucherBankPaymentCheque","DeletePaymentVoucherBankPaymentCheque","CopyPaymentVoucherBankPaymentCheque","UpdatePaymentVoucherBankPaymentCheque","PrintPaymentVoucherBankPaymentCheque",

               "ShowPaymentVoucherBank","AddPaymentVoucherBank","DeletePaymentVoucherBank","CopyPaymentVoucherBank","UpdatePaymentVoucherBank","PrintPaymentVoucherBank", "ExportPaymentVoucherBank" ,"UnExportPaymentVoucherBank","AttachPaymentVoucherBank",

               "ShowPaymentVoucherCash","AddPaymentVoucherCash","DeletePaymentVoucherCash","CopyPaymentVoucherCash","UpdatePaymentVoucherCash","PrintPaymentVoucherCash", "ExportPaymentVoucherCash" ,"UnExportPaymentVoucherCash", "AttachPaymentVoucherCash",

               "ShowDepositInTheBank","AddDepositInTheBank","DeleteDepositInTheBank","CopyDepositInTheBank","UpdateDepositInTheBank","PrintDepositInTheBank" ,"ExportDepositInTheBank" ,"UnExportDepositInTheBank" ,"AttachDepositInTheBank",



               "ShowReturnACheque" , "ShowTransferFromFundUC" ,"ShowDrawingUC" ,"ShowPaymentchequeUC" , "ShowTransferFromFundCD" ,"ShowReturnAChequeCD" ,"ShowTransferFromFundToChequeE" ,"ShowPaymentChequeEndorsement" ,"ShowReturnChequeUnderCollection", "ShowReturnChequeEndorsement", "ShowFundChequesDrawnFromUC" , "ShowReturnedChequeFund" , "ShowCourtFund",
               "ShowPostdatedCheques" , "ShowReturnPostdatedCheques" , "ShowReturnPaidPostdatedCheques",


               "ShowPaymentVoucherCashMultiAccount" , "AddPaymentVoucherCashMultiAccount" , "DeletePaymentVoucherCashMultiAccount" ,"UpdatePaymentVoucherCashMultiAccount" , "CopyPaymentVoucherCashMultiAccount","PrintPaymentVoucherCashMultiAccount" , "ExportPaymentVoucherCashMultiAccount", "UnExportPaymentVoucherCashMultiAccount", "AttachPaymentVoucherCashMultiAccount",
               "ShowReceiptVoucherCashMultiAccount" , "AddReceiptVoucherCashMultiAccount" ,"DeleteReceiptVoucherCashMultiAccount" ,"UpdateReceiptVoucherCashMultiAccount" , "CopyReceiptVoucherCashMultiAccount" ,"PrintReceiptVoucherCashMultiAccount" , "ExportReceiptVoucherCashMultiAccount" ,"UnExportReceiptVoucherCashMultiAccount" ,"AttachReceiptVoucherCashMultiAccount",
               "WorkWithStock","Hiajneh"



            };

              var  userid = user.Id;
                var roles = await UserManager.GetRolesAsync(userid);
                await UserManager.RemoveFromRolesAsync(userid, roles.ToArray());

                string[] sUserRole = { };
                List<string> UserRolList = new List<string>();
                UserRolList.Add("CoUser");
                //await UserManager.AddToRoleAsync(userid, "CoUser");
                var CoInfo = _context.Companys.FirstOrDefault(m => m.CompanyID == user.fCompanyId);
                if (CoInfo.WorkWithStock == 1)
                {
                    UserRolList.Add("WorkWithStock");
                }
                if (CoInfo.Hiajneh == 1)
                {
                    UserRolList.Add("Hiajneh");
                }
                //if (CoInfo.WorkWithStock == 1)
                //{
                //    await UserManager.AddToRoleAsync(user.Id, "WorkWithStock");
                //}
                //if (CoInfo.Hiajneh == 1)
                //{
                //    await UserManager.AddToRoleAsync(user.Id, "Hiajneh");
                //}


                foreach (var roleName in roleNames)
                {
                    //string nameOfTestVariable = MemberInfoGetting.GetMemberName(() => roleName);
                    //var src.GetType().GetProperty(propName).GetValue(src, null);

                    //if ((model.roleName) && (roleName == nameOfTestVariable))
                    //   UserRolList.Add( nameOfTestVariable);
                    //await UserManager.AddToRolesAsync("", theArray);

                    if ((model.ShowCompany) && (roleName == "ShowCompany"))
                        UserRolList.Add("ShowCompany");
                    if ((model.UpdateCompany) && (roleName == "UpdateCompany"))
                        UserRolList.Add("UpdateCompany");



                    if ((model.ShowUser) && (roleName == "ShowUser"))
                        UserRolList.Add("ShowUser");
                    if ((model.AddUser) && (roleName == "AddUser"))
                        UserRolList.Add("AddUser");
                    if ((model.UpdateUser) && (roleName == "UpdateUser"))
                        UserRolList.Add("UpdateUser");
                    if ((model.DeleteUser) && (roleName == "DeleteUser"))
                        UserRolList.Add("DeleteUser");
                    if ((model.PrintUser) && (roleName == "PrintUser"))
                        UserRolList.Add("PrintUser");


                    if ((model.ShowCurrancy) && (roleName == "ShowCurrancy"))
                        UserRolList.Add("ShowCurrancy");
                    if ((model.AddCurrancy) && (roleName == "AddCurrancy"))
                        UserRolList.Add("AddCurrancy");
                    if ((model.UpdateCurrancy) && (roleName == "UpdateCurrancy"))
                        UserRolList.Add("UpdateCurrancy");
                    if ((model.DeleteCurrancy) && (roleName == "DeleteCurrancy"))
                        UserRolList.Add("DeleteCurrancy");
                    if ((model.PrintCurrancy) && (roleName == "PrintCurrancy"))
                        UserRolList.Add("PrintCurrancy");




                    if ((model.ShowCurrancyValue) && (roleName == "ShowCurrancyValue"))
                        UserRolList.Add("ShowCurrancyValue");
                    if ((model.AddCurrancyValue) && (roleName == "AddCurrancyValue"))
                        UserRolList.Add("AddCurrancyValue");
                    if ((model.PrintCurrancyValue) && (roleName == "PrintCurrancyValue"))
                        UserRolList.Add("PrintCurrancyValue");



                    if ((model.ShowSalesman) && (roleName == "ShowSalesman"))
                        UserRolList.Add("ShowSalesman");
                    if ((model.AddSalesman) && (roleName == "AddSalesman"))
                        UserRolList.Add("AddSalesman");
                    if ((model.UpdateSalesman) && (roleName == "UpdateSalesman"))
                        UserRolList.Add("UpdateSalesman");
                    if ((model.DeleteSalesman) && (roleName == "DeleteSalesman"))
                        UserRolList.Add("DeleteSalesman");
                    if ((model.PrintSalesman) && (roleName == "PrintSalesman"))
                        UserRolList.Add("PrintSalesman");



                    if ((model.ShowTransaction) && (roleName == "ShowTransaction"))
                        UserRolList.Add("ShowTransaction");
                    if ((model.AddTransction) && (roleName == "AddTransction"))
                        UserRolList.Add("AddTransction");
                    if ((model.UpdateTransction) && (roleName == "UpdateTransction"))
                        UserRolList.Add("UpdateTransction");
                    if ((model.DeleteTransction) && (roleName == "DeleteTransction"))
                        UserRolList.Add("DeleteTransction");
                    if ((model.PrintTransction) && (roleName == "PrintTransction"))
                        UserRolList.Add("PrintTransction");





                    if ((model.ShowServicegroup) && (roleName == "ShowServicegroup"))
                        UserRolList.Add("ShowServicegroup");
                    if ((model.AddServicegroup) && (roleName == "AddServicegroup"))
                        UserRolList.Add("AddServicegroup");
                    if ((model.UpdateServicegroup) && (roleName == "UpdateServicegroup"))
                        UserRolList.Add("UpdateServicegroup");
                    if ((model.DeleteServicegroup) && (roleName == "DeleteServicegroup"))
                        UserRolList.Add("DeleteServicegroup");
                    if ((model.PrintServicegroup) && (roleName == "PrintServicegroup"))
                        UserRolList.Add("PrintServicegroup");




                    if ((model.ShowBankswithdrawnfrom) && (roleName == "ShowBankswithdrawnfrom"))
                        UserRolList.Add("ShowBankswithdrawnfrom");
                    if ((model.AddBankswithdrawnfrom) && (roleName == "AddBankswithdrawnfrom"))
                        UserRolList.Add("AddBankswithdrawnfrom");
                    if ((model.UpdateBankswithdrawnfrom) && (roleName == "UpdateBankswithdrawnfrom"))
                        UserRolList.Add("UpdateBankswithdrawnfrom");
                    if ((model.PrintBankswithdrawnfrom) && (roleName == "PrintBankswithdrawnfrom"))
                        UserRolList.Add("PrintBankswithdrawnfrom");



                    if ((model.ShowBank) && (roleName == "ShowBank"))
                        UserRolList.Add("ShowBank");
                    if ((model.AddBank) && (roleName == "AddBank"))
                        UserRolList.Add("AddBank");
                    if ((model.UpdateBank) && (roleName == "UpdateBank"))
                        UserRolList.Add("UpdateBank");
                    if ((model.DeleteBank) && (roleName == "DeleteBank"))
                        UserRolList.Add("DeleteBank");
                    if ((model.PrintBank) && (roleName == "PrintBank"))
                        UserRolList.Add("PrintBank");


                    if ((model.ShowOtheraccount) && (roleName == "ShowOtheraccount"))
                        UserRolList.Add("ShowOtheraccount");
                    if ((model.AddOtheraccount) && (roleName == "AddOtheraccount"))
                        UserRolList.Add("AddOtheraccount");
                    if ((model.UpdateOtheraccount) && (roleName == "UpdateOtheraccount"))
                        UserRolList.Add("UpdateOtheraccount");
                    if ((model.DeleteOtheraccount) && (roleName == "DeleteOtheraccount"))
                        UserRolList.Add("DeleteOtheraccount");
                    if ((model.PrintOtheraccount) && (roleName == "PrintOtheraccount"))
                        UserRolList.Add("PrintOtheraccount");


                    if ((model.ShowManagement) && (roleName == "ShowManagement"))
                        UserRolList.Add("ShowManagement");
                    if ((model.AddManagement) && (roleName == "AddManagement"))
                        UserRolList.Add("AddManagement");
                    if ((model.UpdateManagement) && (roleName == "UpdateManagement"))
                        UserRolList.Add("UpdateManagement");
                    if ((model.DeleteManagement) && (roleName == "DeleteManagement"))
                        UserRolList.Add("DeleteManagement");
                    if ((model.PrintManagement) && (roleName == "PrintManagement"))
                        UserRolList.Add("PrintManagement");


                    if ((model.ShowDepartment) && (roleName == "ShowDepartment"))
                        UserRolList.Add("ShowDepartment");
                    if ((model.AddDepartment) && (roleName == "AddDepartment"))
                        UserRolList.Add("AddDepartment");
                    if ((model.UpdateDepartment) && (roleName == "UpdateDepartment"))
                        UserRolList.Add("UpdateDepartment");
                    if ((model.DeleteDepartment) && (roleName == "DeleteDepartment"))
                        UserRolList.Add("DeleteDepartment");
                    if ((model.PrintDepartment) && (roleName == "PrintDepartment"))
                        UserRolList.Add("PrintDepartment");



                    if ((model.ShowSections) && (roleName == "ShowSections"))
                        UserRolList.Add("ShowSections");
                    if ((model.AddSections) && (roleName == "AddSections"))
                        UserRolList.Add("AddSections");
                    if ((model.UpdateSections) && (roleName == "UpdateSections"))
                        UserRolList.Add("UpdateSections");
                    if ((model.DeleteSections) && (roleName == "DeleteSections"))
                        UserRolList.Add("DeleteSections");
                    if ((model.PrintSections) && (roleName == "PrintSections"))
                        UserRolList.Add("PrintSections");





                    if ((model.ShowLocations) && (roleName == "ShowLocations"))
                        UserRolList.Add("ShowLocations");
                    if ((model.AddSections) && (roleName == "AddLocations"))
                        UserRolList.Add("AddLocations");
                    if ((model.UpdateLocations) && (roleName == "UpdateLocations"))
                        UserRolList.Add("UpdateLocations");
                    if ((model.DeleteLocations) && (roleName == "DeleteLocations"))
                        UserRolList.Add("DeleteLocations");
                    if ((model.PrintSections) && (roleName == "PrintLocations"))
                        UserRolList.Add("PrintLocations");


                    //if ((model.ShowLocations) && (roleName == "ShowLocations"))
                    //   UserRolList.Add( "ShowLocations");
                    //if ((model.AddSections) && (roleName == "AddLocations"))
                    //   UserRolList.Add( "AddLocations");
                    //if ((model.UpdateLocations) && (roleName == "UpdateLocations"))
                    //   UserRolList.Add( "UpdateLocations");
                    //if ((model.DeleteLocations) && (roleName == "DeleteLocations"))
                    //   UserRolList.Add( "DeleteLocations");
                    //if ((model.PrintSections) && (roleName == "PrintLocations"))
                    //   UserRolList.Add( "PrintLocations");




                    if ((model.ShowTrustee) && (roleName == "ShowTrustee"))
                        UserRolList.Add("ShowTrustee");
                    if ((model.AddTrustee) && (roleName == "AddTrustee"))
                        UserRolList.Add("AddTrustee");
                    if ((model.UpdateTrustee) && (roleName == "UpdateTrustee"))
                        UserRolList.Add("UpdateTrustee");
                    if ((model.DeleteTrustee) && (roleName == "DeleteTrustee"))
                        UserRolList.Add("DeleteTrustee");
                    if ((model.PrintTrustee) && (roleName == "PrintTrustee"))
                        UserRolList.Add("PrintTrustee");



                    if ((model.ShowSupplierbanks) && (roleName == "ShowSupplierbanks"))
                        UserRolList.Add("ShowSupplierbanks");
                    if ((model.AddSupplierbanks) && (roleName == "AddSupplierbanks"))
                        UserRolList.Add("AddSupplierbanks");
                    if ((model.UpdateSupplierbanks) && (roleName == "UpdateSupplierbanks"))
                        UserRolList.Add("UpdateSupplierbanks");
                    if ((model.DeleteSupplierbanks) && (roleName == "DeleteSupplierbanks"))
                        UserRolList.Add("DeleteSupplierbanks");
                    if ((model.PrintSupplierbanks) && (roleName == "PrintSupplierbanks"))
                        UserRolList.Add("PrintSupplierbanks");


                    if ((model.ShowSuppliercountries) && (roleName == "ShowSuppliercountries"))
                        UserRolList.Add("ShowSuppliercountries");
                    if ((model.AddSuppliercountries) && (roleName == "AddSuppliercountries"))
                        UserRolList.Add("AddSuppliercountries");
                    if ((model.UpdateSuppliercountries) && (roleName == "UpdateSuppliercountries"))
                        UserRolList.Add("UpdateSuppliercountries");
                    if ((model.DeleteSuppliercountries) && (roleName == "DeleteSuppliercountries"))
                        UserRolList.Add("DeleteSuppliercountries");
                    if ((model.PrintSuppliercountries) && (roleName == "PrintSuppliercountries"))
                        UserRolList.Add("PrintSuppliercountries");


                    //if ((model.ShowSuppliercountries) && (roleName == "ShowSuppliercountries"))
                    //   UserRolList.Add( "ShowSuppliercountries");
                    //if ((model.AddSuppliercountries) && (roleName == "AddSuppliercountries"))
                    //   UserRolList.Add( "AddSuppliercountries");
                    //if ((model.UpdateSuppliercountries) && (roleName == "UpdateSuppliercountries"))
                    //   UserRolList.Add( "UpdateSuppliercountries");
                    //if ((model.DeleteSuppliercountries) && (roleName == "DeleteSuppliercountries"))
                    //   UserRolList.Add( "DeleteSuppliercountries");
                    //if ((model.PrintSuppliercountries) && (roleName == "PrintSuppliercountries"))
                    //   UserRolList.Add( "PrintSuppliercountries");



                    if ((model.ShowSupplierbankcountries) && (roleName == "ShowSupplierbankcountries"))
                        UserRolList.Add("ShowSupplierbankcountries");
                    if ((model.AddSupplierbankcountries) && (roleName == "AddSupplierbankcountries"))
                        UserRolList.Add("AddSupplierbankcountries");
                    if ((model.UpdateSupplierbankcountries) && (roleName == "UpdateSupplierbankcountries"))
                        UserRolList.Add("UpdateSupplierbankcountries");
                    if ((model.DeleteSupplierbankcountries) && (roleName == "DeleteSupplierbankcountries"))
                        UserRolList.Add("DeleteSupplierbankcountries");
                    if ((model.PrintSupplierbankcountries) && (roleName == "PrintSupplierbankcountries"))
                        UserRolList.Add("PrintSupplierbankcountries");



                    if ((model.ShowSupplierbankbranches) && (roleName == "ShowSupplierbankbranches"))
                        UserRolList.Add("ShowSupplierbankbranches");
                    if ((model.AddSupplierbankbranches) && (roleName == "AddSupplierbankbranches"))
                        UserRolList.Add("AddSupplierbankbranches");
                    if ((model.UpdateSupplierbankbranches) && (roleName == "UpdateSupplierbankbranches"))
                        UserRolList.Add("UpdateSupplierbankbranches");
                    if ((model.DeleteSupplierbankbranches) && (roleName == "DeleteSupplierbankbranches"))
                        UserRolList.Add("DeleteSupplierbankbranches");
                    if ((model.PrintSupplierbankbranches) && (roleName == "PrintSupplierbankbranches"))
                        UserRolList.Add("PrintSupplierbankbranches");


                    if ((model.ShowSuppliercity) && (roleName == "ShowSuppliercity"))
                        UserRolList.Add("ShowSuppliercity");
                    if ((model.AddSuppliercity) && (roleName == "AddSuppliercity"))
                        UserRolList.Add("AddSuppliercity");
                    if ((model.UpdateSuppliercity) && (roleName == "UpdateSuppliercity"))
                        UserRolList.Add("UpdateSuppliercity");
                    if ((model.DeleteSuppliercity) && (roleName == "DeleteSuppliercity"))
                        UserRolList.Add("DeleteSuppliercity");
                    if ((model.PrintSuppliercity) && (roleName == "PrintSuppliercity"))
                        UserRolList.Add("PrintSuppliercity");




                    if ((model.ShowSuppliersbankcity) && (roleName == "ShowSuppliersbankcity"))
                        UserRolList.Add("ShowSuppliersbankcity");
                    if ((model.AddSuppliersbankcity) && (roleName == "AddSuppliersbankcity"))
                        UserRolList.Add("AddSuppliersbankcity");
                    if ((model.UpdateSuppliersbankcity) && (roleName == "UpdateSuppliersbankcity"))
                        UserRolList.Add("UpdateSuppliersbankcity");
                    if ((model.DeleteSuppliersbankcity) && (roleName == "DeleteSuppliersbankcity"))
                        UserRolList.Add("DeleteSuppliersbankcity");
                    if ((model.PrintSuppliersbankcity) && (roleName == "PrintSuppliersbankcity"))
                        UserRolList.Add("PrintSuppliersbankcity");


                    if ((model.ShowCustomercity) && (roleName == "ShowCustomercity"))
                        UserRolList.Add("ShowCustomercity");
                    if ((model.AddCustomercity) && (roleName == "AddCustomercity"))
                        UserRolList.Add("AddCustomercity");
                    if ((model.UpdateCustomercity) && (roleName == "UpdateCustomercity"))
                        UserRolList.Add("UpdateCustomercity");
                    if ((model.DeleteCustomercity) && (roleName == "DeleteCustomercity"))
                        UserRolList.Add("DeleteCustomercity");
                    if ((model.PrintCustomercity) && (roleName == "PrintCustomercity"))
                        UserRolList.Add("PrintCustomercity");


                    if ((model.ShowCustomerarea) && (roleName == "ShowCustomerarea"))
                        UserRolList.Add("ShowCustomerarea");
                    if ((model.AddCustomerarea) && (roleName == "AddCustomerarea"))
                        UserRolList.Add("AddCustomerarea");
                    if ((model.UpdateCustomerarea) && (roleName == "UpdateCustomerarea"))
                        UserRolList.Add("UpdateCustomerarea");
                    if ((model.DeleteCustomerarea) && (roleName == "DeleteCustomerarea"))
                        UserRolList.Add("DeleteCustomerarea");
                    if ((model.PrintCustomerarea) && (roleName == "PrintCustomerarea"))
                        UserRolList.Add("PrintCustomerarea");



                    if ((model.ShowService) && (roleName == "ShowService"))
                        UserRolList.Add("ShowService");
                    if ((model.AddService) && (roleName == "AddService"))
                        UserRolList.Add("AddService");
                    if ((model.UpdateService) && (roleName == "UpdateService"))
                        UserRolList.Add("UpdateService");
                    if ((model.DeleteService) && (roleName == "DeleteService"))
                        UserRolList.Add("DeleteService");
                    if ((model.PrintService) && (roleName == "PrintService"))
                        UserRolList.Add("PrintService");




                    ///----operation***//
                    ///
                    if ((model.ShowTransferbetweenaccounts) && (roleName == "ShowTransferbetweenaccounts"))
                        UserRolList.Add("ShowTransferbetweenaccounts");
                    if ((model.TransferTransferbetweenaccounts) && (roleName == "TransferTransferbetweenaccounts"))
                        UserRolList.Add("TransferTransferbetweenaccounts");



                    if ((model.ShowFreezunfreezmonthtransaction) && (roleName == "ShowFreezunfreezmonthtransaction"))
                        UserRolList.Add("ShowFreezunfreezmonthtransaction");
                    if ((model.FreezFreezunfreezmonthtransaction) && (roleName == "FreezFreezunfreezmonthtransaction"))
                        UserRolList.Add("FreezFreezunfreezmonthtransaction");
                    if ((model.unFreezFreezunfreezmonthtransaction) && (roleName == "unFreezFreezunfreezmonthtransaction"))
                        UserRolList.Add("unFreezFreezunfreezmonthtransaction");



                    if ((model.ShowEstimatedbudget) && (roleName == "ShowEstimatedbudget"))
                        UserRolList.Add("ShowEstimatedbudget");
                    if ((model.AddEstimatedbudget) && (roleName == "AddEstimatedbudget"))
                        UserRolList.Add("AddEstimatedbudget");

                    ///---------//
                    ///

                    //--Accuont---


                    if ((model.ShowChartofaccount) && (roleName == "ShowChartofaccount"))
                        UserRolList.Add("ShowChartofaccount");
                    if ((model.AddmainaccountChartofaccount) && (roleName == "AddmainaccountChartofaccount"))
                        UserRolList.Add("AddmainaccountChartofaccount");
                    if ((model.UpdateChartofaccoun) && (roleName == "UpdateChartofaccoun"))
                        UserRolList.Add("UpdateChartofaccoun");
                    if ((model.DeleteChartofaccoun) && (roleName == "DeleteChartofaccoun"))
                        UserRolList.Add("DeleteChartofaccoun");








                    if ((model.AddsubaccountChartofaccount) && (roleName == "AddsubaccountChartofaccount"))
                        UserRolList.Add("AddsubaccountChartofaccount");
                    if ((model.UpdatesubaccountChartofaccount) && (roleName == "UpdatesubaccountChartofaccount"))
                        UserRolList.Add("UpdatesubaccountChartofaccount");
                    if ((model.DeletesubaccountChartofaccount) && (roleName == "DeletesubaccountChartofaccount"))
                        UserRolList.Add("DeletesubaccountChartofaccount");




                    if ((model.ShowChartofcostcenter) && (roleName == "ShowChartofcostcenter"))
                        UserRolList.Add("ShowChartofcostcenter");
                    if ((model.AddmainaccountChartofcostcenter) && (roleName == "AddmainaccountChartofcostcenter"))
                        UserRolList.Add("AddmainaccountChartofcostcenter");
                    if ((model.UpdateChartofcostcenter) && (roleName == "UpdateChartofcostcenter"))
                        UserRolList.Add("UpdateChartofcostcenter");
                    if ((model.DeleteChartofcostcenter) && (roleName == "DeleteChartofcostcenter"))
                        UserRolList.Add("DeleteChartofcostcenter");





                    if ((model.AddsubChartofcostcenter) && (roleName == "AddsubChartofcostcenter"))
                        UserRolList.Add("AddsubChartofcostcenter");
                    if ((model.UpdatesubChartofcostcenter) && (roleName == "UpdatesubChartofcostcenter"))
                        UserRolList.Add("UpdatesubChartofcostcenter");
                    if ((model.DeletesubChartofcostcenter) && (roleName == "DeletesubChartofcostcenter"))
                        UserRolList.Add("DeletesubChartofcostcenter");




                    if ((model.Showcustomeraccount) && (roleName == "Showcustomeraccount"))
                        UserRolList.Add("Showcustomeraccount");
                    if ((model.Addcustomeraccount) && (roleName == "Addcustomeraccount"))
                        UserRolList.Add("Addcustomeraccount");
                    if ((model.Updatecustomeraccount) && (roleName == "Updatecustomeraccount"))
                        UserRolList.Add("Updatecustomeraccount");
                    if ((model.Deletecustomeraccount) && (roleName == "Deletecustomeraccount"))
                        UserRolList.Add("Deletecustomeraccount");





                    if ((model.ShowSupplieraccount) && (roleName == "ShowSupplieraccount"))
                        UserRolList.Add("ShowSupplieraccount");
                    if ((model.AddSupplieraccount) && (roleName == "AddSupplieraccount"))
                        UserRolList.Add("AddSupplieraccount");
                    if ((model.UpdateSupplieraccount) && (roleName == "UpdateSupplieraccount"))
                        UserRolList.Add("UpdateSupplieraccount");
                    if ((model.DeleteSupplieraccount) && (roleName == "DeleteSupplieraccount"))
                        UserRolList.Add("DeleteSupplieraccount");




                    //--Assset--//

                    if ((model.ShowAssetType) && (roleName == "ShowAssetType"))
                        UserRolList.Add("ShowAssetType");
                    if ((model.AddAssetType) && (roleName == "AddAssetType"))
                        UserRolList.Add("AddAssetType");
                    if ((model.UpdateAssetType) && (roleName == "UpdateAssetType"))
                        UserRolList.Add("UpdateAssetType");
                    if ((model.DeleteAssetType) && (roleName == "DeleteAssetType"))
                        UserRolList.Add("DeleteAssetType");
                    if ((model.PrintAssetType) && (roleName == "PrintAssetType"))
                        UserRolList.Add("PrintAssetType");



                    if ((model.ShowCalculationOfConsumption) && (roleName == "ShowCalculationOfConsumption"))
                        UserRolList.Add("ShowCalculationOfConsumption");
                    if ((model.AddCalculationOfConsumption) && (roleName == "AddCalculationOfConsumption"))
                        UserRolList.Add("AddCalculationOfConsumption");


                    if ((model.ShowAsset) && (roleName == "ShowAsset"))
                        UserRolList.Add("ShowAsset");
                    if ((model.AddAsset) && (roleName == "AddAsset"))
                        UserRolList.Add("AddAsset");
                    if ((model.UpdateAsset) && (roleName == "UpdateAsset"))
                        UserRolList.Add("UpdateAsset");
                    if ((model.DeleteAsset) && (roleName == "DeleteAsset"))
                        UserRolList.Add("DeleteAsset");
                    if ((model.PrintAsset) && (roleName == "PrintAsset"))
                        UserRolList.Add("PrintAsset");


                    if ((model.ShowAssetToAcc) && (roleName == "ShowAssetToAcc"))
                        UserRolList.Add("ShowAssetToAcc");
                    if ((model.AddAssetToAcc) && (roleName == "AddAssetToAcc"))
                        UserRolList.Add("AddAssetToAcc");

                    if ((model.ShowDefinitionAsset) && (roleName == "ShowDefinitionAsset"))
                        UserRolList.Add("ShowDefinitionAsset");
                    if ((model.AddDefinitionAsset) && (roleName == "AddDefinitionAsset"))
                        UserRolList.Add("AddDefinitionAsset");



                    if ((model.ShowSaleAsset) && (roleName == "ShowSaleAsset"))
                        UserRolList.Add("ShowSaleAsset");
                    if ((model.AddShowSaleAsset) && (roleName == "AddShowSaleAsset"))
                        UserRolList.Add("AddShowSaleAsset");



                    if ((model.ShowDeliveryAssetTrustee) && (roleName == "ShowDeliveryAssetTrustee"))
                        UserRolList.Add("ShowDeliveryAssetTrustee");
                    if ((model.AddDeliveryAssetTrustee) && (roleName == "AddDeliveryAssetTrustee"))
                        UserRolList.Add("AddDeliveryAssetTrustee");

                    if ((model.ShowConsumAsset) && (roleName == "ShowConsumAsset"))
                        UserRolList.Add("ShowConsumAsset");
                    if ((model.AddConsumAsset) && (roleName == "AddConsumAsset"))
                        UserRolList.Add("AddConsumAsset");


                    if ((model.ShowAssetMaintenance) && (roleName == "ShowAssetMaintenance"))
                        UserRolList.Add("ShowAssetMaintenance");
                    if ((model.AddAssetMaintenance) && (roleName == "AddAssetMaintenance"))
                        UserRolList.Add("AddAssetMaintenance");
                    if ((model.UpdateAssetMaintenance) && (roleName == "UpdateAssetMaintenance"))
                        UserRolList.Add("UpdateAssetMaintenance");
                    if ((model.DeleteAssetMaintenance) && (roleName == "DeleteAssetMaintenance"))
                        UserRolList.Add("DeleteAssetMaintenance");

                    //--End Asset--//

                    //--Report----//

                    if ((model.RepAccountStatement) && (roleName == "RepAccountStatement"))
                        UserRolList.Add("RepAccountStatement");

                    if ((model.RepTrialBalance) && (roleName == "RepTrialBalance"))
                        UserRolList.Add("RepTrialBalance");

                    if ((model.RepAccountingDetailsReport) && (roleName == "RepAccountingDetailsReport"))
                        UserRolList.Add("RepAccountingDetailsReport");

                    if ((model.RepChequesReport) && (roleName == "RepChequesReport"))
                        UserRolList.Add("RepChequesReport");

                    if ((model.RepCustSuppBalances) && (roleName == "RepCustSuppBalances"))
                        UserRolList.Add("RepCustSuppBalances");

                    if ((model.RepTrialBalanceYearly) && (roleName == "RepTrialBalanceYearly"))
                        UserRolList.Add("RepTrialBalanceYearly");

                    if ((model.RepTransActionDetails) && (roleName == "RepTransActionDetails"))
                        UserRolList.Add("RepTransActionDetails");

                    if ((model.RepTrankingChequesReport) && (roleName == "RepTrankingChequesReport"))
                        UserRolList.Add("RepTrankingChequesReport");

                    if ((model.RepSalesReport) && (roleName == "RepSalesReport"))
                        UserRolList.Add("RepSalesReport");

                    if ((model.RepExpenseAnalysisDetailReport) && (roleName == "RepExpenseAnalysisDetailReport"))
                        UserRolList.Add("RepExpenseAnalysisDetailReport");

                    if ((model.RepSearchForTrans) && (roleName == "RepSearchForTrans"))
                        UserRolList.Add("RepSearchForTrans");

                    if ((model.RepPaymentChequesReport) && (roleName == "RepPaymentChequesReport"))
                        UserRolList.Add("RepPaymentChequesReport");

                    if ((model.RepServiceReport) && (roleName == "RepServiceReport"))
                        UserRolList.Add("RepServiceReport");

                    if ((model.RepCustomerBalancesReport) && (roleName == "RepCustomerBalancesReport"))
                        UserRolList.Add("RepCustomerBalancesReport");

                    if ((model.RepPaymTrankingChequesReport) && (roleName == "RepPaymTrankingChequesReport"))
                        UserRolList.Add("RepPaymTrankingChequesReport");

                    if ((model.RepServiceTaxReport) && (roleName == "RepServiceTaxReport"))
                        UserRolList.Add("RepServiceTaxReport");

                    if ((model.RepSupplierBalancesReport) && (roleName == "RepSupplierBalancesReport"))
                        UserRolList.Add("RepSupplierBalancesReport");

                    if ((model.RepExpenseAnalysisReport) && (roleName == "RepExpenseAnalysisReport"))
                        UserRolList.Add("RepExpenseAnalysisReport");

                    if ((model.RepProfitlossreport) && (roleName == "RepProfitlossreport"))
                        UserRolList.Add("RepProfitlossreport");

                    if ((model.RepTempPrepaidReport) && (roleName == "RepTempPrepaidReport"))
                        UserRolList.Add("RepTempPrepaidReport");

                    if ((model.RepPivotReportAccounts) && (roleName == "RepPivotReportAccounts"))
                        UserRolList.Add("RepPivotReportAccounts");

                    if ((model.RepAssetsReport) && (roleName == "RepAssetsReport"))
                        UserRolList.Add("RepAssetsReport");

                    if ((model.RepTempRevenueReport) && (roleName == "RepTempRevenueReport"))
                        UserRolList.Add("RepTempRevenueReport");

                    if ((model.RepAssetDepreciationReport) && (roleName == "RepAssetDepreciationReport"))
                        UserRolList.Add("RepAssetDepreciationReport");


                    if ((model.RepEstimatedBudgetForAccount) && (roleName == "RepEstimatedBudgetForAccount"))
                        UserRolList.Add("RepEstimatedBudgetForAccount");

                    if ((model.RepConsumptionByTypeReport) && (roleName == "RepConsumptionByTypeReport"))
                        UserRolList.Add("RepConsumptionByTypeReport");

                    if ((model.RepEstimatedBudgetForAccountAll) && (roleName == "RepEstimatedBudgetForAccountAll"))
                        UserRolList.Add("RepEstimatedBudgetForAccountAll");

                    if ((model.RepDefinitionAssetSiteReport) && (roleName == "RepDefinitionAssetSiteReport"))
                        UserRolList.Add("RepDefinitionAssetSiteReport");

                    if ((model.RepEstimatedBudgetForAccountYear) && (roleName == "RepEstimatedBudgetForAccountYear"))
                        UserRolList.Add("RepEstimatedBudgetForAccountYear");

                    if ((model.RepSoldAsset) && (roleName == "RepSoldAsset"))
                        UserRolList.Add("RepSoldAsset");

                    if ((model.RepEstimatedBudgetForCostCenters) && (roleName == "RepEstimatedBudgetForCostCenters"))
                        UserRolList.Add("RepEstimatedBudgetForCostCenters");

                    if ((model.RepConsumAssetReport) && (roleName == "RepConsumAssetReport"))
                        UserRolList.Add("RepConsumAssetReport");

                    if ((model.RepEstimatedBudgetCostAllMonth) && (roleName == "RepEstimatedBudgetCostAllMonth"))
                        UserRolList.Add("RepEstimatedBudgetCostAllMonth");

                    if ((model.RepMaintenanceAssetReport) && (roleName == "RepMaintenanceAssetReport"))
                        UserRolList.Add("RepMaintenanceAssetReport");

                    if ((model.RepEstimatedBudgetForCostCenterYear) && (roleName == "RepEstimatedBudgetForCostCenterYear"))
                        UserRolList.Add("DeleteAssetMaintenance");

                    if ((model.RepAssetsTransferMovements) && (roleName == "RepAssetsTransferMovements"))
                        UserRolList.Add("RepAssetsTransferMovements");


                    ///////////////
                    if ((model.PrintRepAccountStatement) && (roleName == "PrintRepAccountStatement"))
                        UserRolList.Add("PrintRepAccountStatement");




                    if ((model.PrintRepTrialBalance) && (roleName == "PrintRepTrialBalance"))
                        UserRolList.Add("PrintRepTrialBalance");



                    if ((model.PrintRepAccountingDetailsPrintReport) && (roleName == "PrintRepAccountingDetailsPrintReport"))
                        UserRolList.Add("PrintRepAccountingDetailsPrintReport");




                    if ((model.PrintRepChequesPrintReport) && (roleName == "PrintRepChequesPrintReport"))
                        UserRolList.Add("PrintRepChequesPrintReport");



                    if ((model.PrintRepCustSuppBalances) && (roleName == "PrintRepCustSuppBalances"))
                        UserRolList.Add("PrintRepCustSuppBalances");



                    if ((model.PrintRepTrialBalanceYearly) && (roleName == "PrintRepTrialBalanceYearly"))
                        UserRolList.Add("PrintRepTrialBalanceYearly");




                    if ((model.PrintRepTransActionDetails) && (roleName == "PrintRepTransActionDetails"))
                        UserRolList.Add("PrintRepTransActionDetails");



                    if ((model.PrintRepTrankingChequesPrintReport) && (roleName == "PrintRepTrankingChequesPrintReport"))
                        UserRolList.Add("PrintRepTrankingChequesPrintReport");



                    if ((model.PrintRepSalesPrintReport) && (roleName == "PrintRepSalesPrintReport"))
                        UserRolList.Add("PrintRepSalesPrintReport");




                    if ((model.PrintRepExpenseAnalysisDetailPrintReport) && (roleName == "PrintRepExpenseAnalysisDetailPrintReport"))
                        UserRolList.Add("PrintRepExpenseAnalysisDetailPrintReport");


                    if ((model.PrintRepSearchForTrans) && (roleName == "PrintRepSearchForTrans"))
                        UserRolList.Add("PrintRepSearchForTrans");



                    if ((model.PrintRepPaymentChequesPrintReport) && (roleName == "PrintRepPaymentChequesPrintReport"))
                        UserRolList.Add("PrintRepPaymentChequesPrintReport");





                    if ((model.PrintRepServicePrintReport) && (roleName == "PrintRepServicePrintReport"))
                        UserRolList.Add("PrintRepServicePrintReport");



                    if ((model.PrintRepCustomerBalancesPrintReport) && (roleName == "PrintRepCustomerBalancesPrintReport"))
                        UserRolList.Add("PrintRepCustomerBalancesPrintReport");



                    if ((model.PrintRepPaymTrankingChequesPrintReport) && (roleName == "PrintRepPaymTrankingChequesPrintReport"))
                        UserRolList.Add("PrintRepPaymTrankingChequesPrintReport");





                    if ((model.PrintRepServiceTaxPrintReport) && (roleName == "PrintRepServiceTaxPrintReport"))
                        UserRolList.Add("PrintRepServiceTaxPrintReport");





                    if ((model.PrintRepSupplierBalancesPrintReport) && (roleName == "PrintRepSupplierBalancesPrintReport"))
                        UserRolList.Add("PrintRepSupplierBalancesPrintReport");



                    if ((model.PrintRepExpenseAnalysisPrintReport) && (roleName == "PrintRepExpenseAnalysisPrintReport"))
                        UserRolList.Add("PrintRepExpenseAnalysisPrintReport");



                    if ((model.PrintRepProfitlossPrintReport) && (roleName == "PrintRepProfitlossPrintReport"))
                        UserRolList.Add("PrintRepProfitlossPrintReport");




                    if ((model.PrintRepTempPPrintRepaidPrintReport) && (roleName == "PrintRepTempPPrintRepaidPrintReport"))
                        UserRolList.Add("PrintRepTempPPrintRepaidPrintReport");




                    if ((model.PrintRepPivotPrintReportAccounts) && (roleName == "PrintRepPivotPrintReportAccounts"))
                        UserRolList.Add("PrintRepPivotPrintReportAccounts");





                    if ((model.PrintRepAssetsPrintReport) && (roleName == "PrintRepAssetsPrintReport"))
                        UserRolList.Add("PrintRepAssetsPrintReport");





                    if ((model.PrintRepTempRevenuePrintReport) && (roleName == "PrintRepTempRevenuePrintReport"))
                        UserRolList.Add("PrintRepTempRevenuePrintReport");




                    if ((model.PrintRepAssetDepreciationPrintReport) && (roleName == "PrintRepAssetDepreciationPrintReport"))
                        UserRolList.Add("PrintRepAssetDepreciationPrintReport");



                    if ((model.PrintRepEstimatedBudgetForAccount) && (roleName == "PrintRepEstimatedBudgetForAccount"))
                        UserRolList.Add("PrintRepEstimatedBudgetForAccount");



                    if ((model.PrintRepConsumptionByTypePrintReport) && (roleName == "PrintRepConsumptionByTypePrintReport"))
                        UserRolList.Add("PrintRepConsumptionByTypePrintReport");



                    if ((model.PrintRepEstimatedBudgetForAccountAll) && (roleName == "PrintRepEstimatedBudgetForAccountAll"))
                        UserRolList.Add("PrintRepEstimatedBudgetForAccountAll");


                    if ((model.PrintRepDefinitionAssetSitePrintReport) && (roleName == "PrintRepDefinitionAssetSitePrintReport"))
                        UserRolList.Add("PrintRepDefinitionAssetSitePrintReport");




                    if ((model.PrintRepEstimatedBudgetForAccountYear) && (roleName == "PrintRepEstimatedBudgetForAccountYear"))
                        UserRolList.Add("PrintRepEstimatedBudgetForAccountYear");




                    if ((model.PrintRepSoldAsset) && (roleName == "PrintRepSoldAsset"))
                        UserRolList.Add("PrintRepSoldAsset");




                    if ((model.PrintRepEstimatedBudgetForCostCenters) && (roleName == "PrintRepEstimatedBudgetForCostCenters"))
                        UserRolList.Add("PrintRepEstimatedBudgetForCostCenters");





                    if ((model.PrintRepConsumAssetPrintReport) && (roleName == "PrintRepConsumAssetPrintReport"))
                        UserRolList.Add("PrintRepConsumAssetPrintReport");




                    if ((model.PrintRepEstimatedBudgetCostAllMonth) && (roleName == "PrintRepEstimatedBudgetCostAllMonth"))
                        UserRolList.Add("PrintRepEstimatedBudgetCostAllMonth");


                    if ((model.PrintRepMaintenanceAssetPrintReport) && (roleName == "PrintRepMaintenanceAssetPrintReport"))
                        UserRolList.Add("PrintRepMaintenanceAssetPrintReport");




                    if ((model.PrintRepEstimatedBudgetForCostCenterYear) && (roleName == "PrintRepEstimatedBudgetForCostCenterYear"))
                        UserRolList.Add("PrintRepEstimatedBudgetForCostCenterYear");

                    if ((model.PrintRepAssetsTransferMovements) && (roleName == "PrintRepAssetsTransferMovements"))
                        UserRolList.Add("PrintRepAssetsTransferMovements");

                    /////////////

                    //ابدعتي

                    //--End Report---//


                    ///----TransAction---//

                    if ((model.ShowTransActionTrans) && (roleName == "ShowTransActionTrans"))
                        UserRolList.Add("ShowTransActionTrans");
                    if ((model.AddTransActionTrans) && (roleName == "AddTransActionTrans"))
                        UserRolList.Add("AddTransActionTrans");
                    if ((model.DeleteTransActionTrans) && (roleName == "DeleteTransActionTrans"))
                        UserRolList.Add("DeleteTransActionTrans");
                    if ((model.UpdateTransActionTrans) && (roleName == "UpdateTransActionTrans"))
                        UserRolList.Add("UpdateTransActionTrans");
                    if ((model.CopyTransActionTrans) && (roleName == "CopyTransActionTrans"))
                        UserRolList.Add("CopyTransActionTrans");
                    if ((model.PrintTransActionTrans) && (roleName == "PrintTransActionTrans"))
                        UserRolList.Add("PrintTransActionTrans");

                    if ((model.ExportTransActionTrans) && (roleName == "ExportTransActionTrans"))
                        UserRolList.Add("ExportTransActionTrans");

                    if ((model.UnExportTransActionTrans) && (roleName == "UnExportTransActionTrans"))
                        UserRolList.Add("UnExportTransActionTrans");


                    if ((model.AttachTransActionTrans) && (roleName == "AttachTransActionTrans"))
                        UserRolList.Add("AttachTransActionTrans");



                    if ((model.ShowReceiptVoucherBank) && (roleName == "ShowReceiptVoucherBank"))
                        UserRolList.Add("ShowReceiptVoucherBank");


                    if ((model.AddReceiptVoucherBank) && (roleName == "AddReceiptVoucherBank"))
                        UserRolList.Add("AddReceiptVoucherBank");


                    if ((model.DeleteReceiptVoucherBank) && (roleName == "DeleteReceiptVoucherBank"))
                        UserRolList.Add("DeleteReceiptVoucherBank");


                    if ((model.UpdateReceiptVoucherBank) && (roleName == "UpdateReceiptVoucherBank"))
                        UserRolList.Add("UpdateReceiptVoucherBank");


                    if ((model.CopyReceiptVoucherBank) && (roleName == "CopyReceiptVoucherBank"))
                        UserRolList.Add("CopyReceiptVoucherBank");

                    if ((model.PrintReceiptVoucherBank) && (roleName == "PrintReceiptVoucherBank"))
                        UserRolList.Add("PrintReceiptVoucherBank");

                    if ((model.ExportReceiptVoucherBank) && (roleName == "ExportReceiptVoucherBank"))
                        UserRolList.Add("ExportReceiptVoucherBank");


                    if ((model.UnExportReceiptVoucherBank) && (roleName == "UnExportReceiptVoucherBank"))
                        UserRolList.Add("UnExportReceiptVoucherBank");


                    if ((model.AttachReceiptVoucherBank) && (roleName == "AttachReceiptVoucherBank"))
                        UserRolList.Add("AttachReceiptVoucherBank");



                    if ((model.ShowTempPrepaid) && (roleName == "ShowTempPrepaid"))
                        UserRolList.Add("ShowTempPrepaid");


                    if ((model.AddTempPrepaid) && (roleName == "AddTempPrepaid"))
                        UserRolList.Add("AddTempPrepaid");


                    if ((model.DeleteTempPrepaid) && (roleName == "DeleteTempPrepaid"))
                        UserRolList.Add("DeleteTempPrepaid");


                    if ((model.UpdateTempPrepaid) && (roleName == "UpdateTempPrepaid"))
                        UserRolList.Add("UpdateTempPrepaid");


                    if ((model.CopyTempPrepaid) && (roleName == "CopyTempPrepaid"))
                        UserRolList.Add("CopyTempPrepaid");

                    if ((model.PrintTempPrepaid) && (roleName == "PrintTempPrepaid"))
                        UserRolList.Add("PrintTempPrepaid");

                    if ((model.AccumulativeTempPrepaid) && (roleName == "AccumulativeTempPrepaid"))
                        UserRolList.Add("AccumulativeTempPrepaid");


                    if ((model.AttachTempPrepaid) && (roleName == "AttachTempPrepaid"))
                        UserRolList.Add("AttachTempPrepaid");










                    if ((model.ShowDebitNote) && (roleName == "ShowDebitNote"))
                        UserRolList.Add("ShowDebitNote");


                    if ((model.AddDebitNote) && (roleName == "AddDebitNote"))
                        UserRolList.Add("AddDebitNote");


                    if ((model.DeleteDebitNote) && (roleName == "DeleteDebitNote"))
                        UserRolList.Add("DeleteDebitNote");


                    if ((model.UpdateDebitNote) && (roleName == "UpdateDebitNote"))
                        UserRolList.Add("UpdateDebitNote");


                    if ((model.CopyDebitNote) && (roleName == "CopyDebitNote"))
                        UserRolList.Add("CopyDebitNote");

                    if ((model.PrintDebitNote) && (roleName == "PrintDebitNote"))
                        UserRolList.Add("PrintDebitNote");

                    if ((model.ExportDebitNote) && (roleName == "ExportDebitNote"))
                        UserRolList.Add("ExportDebitNote");

                    if ((model.UnExportDebitNote) && (roleName == "UnExportDebitNote"))
                        UserRolList.Add("UnExportDebitNote");

                    if ((model.AttachDebitNote) && (roleName == "AttachDebitNote"))
                        UserRolList.Add("AttachDebitNote");









                    if ((model.ShowServiceBill) && (roleName == "ShowServiceBill"))
                        UserRolList.Add("ShowServiceBill");


                    if ((model.AddServiceBill) && (roleName == "AddServiceBill"))
                        UserRolList.Add("AddServiceBill");


                    if ((model.DeleteServiceBill) && (roleName == "DeleteServiceBill"))
                        UserRolList.Add("DeleteServiceBill");


                    if ((model.UpdateServiceBill) && (roleName == "UpdateServiceBill"))
                        UserRolList.Add("UpdateServiceBill");


                    if ((model.CopyServiceBill) && (roleName == "CopyServiceBill"))
                        UserRolList.Add("CopyServiceBill");

                    if ((model.PrintServiceBill) && (roleName == "PrintServiceBill"))
                        UserRolList.Add("PrintServiceBill");


                    if ((model.AttachServiceBill) && (roleName == "AttachServiceBill"))
                        UserRolList.Add("AttachServiceBill");

                    if ((model.ExportServiceBill) && (roleName == "ExportServiceBill"))
                        UserRolList.Add("ExportServiceBill");

                    if ((model.UnExportServiceBill) && (roleName == "UnExportServiceBill"))
                        UserRolList.Add("UnExportServiceBill");




                    //if ((model.ShowReceiptVoucherBankReceivedCheque) && (roleName == "ShowReceiptVoucherBankReceivedCheque"))
                    //   UserRolList.Add( "ShowReceiptVoucherBankReceivedCheque");


                    //if ((model.AddReceiptVoucherBankReceivedCheque) && (roleName == "AddReceiptVoucherBankReceivedCheque"))
                    //   UserRolList.Add( "AddReceiptVoucherBankReceivedCheque");


                    //if ((model.DeleteReceiptVoucherBankReceivedCheque) && (roleName == "DeleteReceiptVoucherBankReceivedCheque"))
                    //   UserRolList.Add( "DeleteReceiptVoucherBankReceivedCheque");


                    //if ((model.UpdateReceiptVoucherBankReceivedCheque) && (roleName == "UpdateReceiptVoucherBankReceivedCheque"))
                    //   UserRolList.Add( "UpdateReceiptVoucherBankReceivedCheque");


                    //if ((model.CopyReceiptVoucherBankReceivedCheque) && (roleName == "CopyReceiptVoucherBankReceivedCheque"))
                    //   UserRolList.Add( "CopyReceiptVoucherBankReceivedCheque");

                    //if ((model.PrintReceiptVoucherBankReceivedCheque) && (roleName == "PrintReceiptVoucherBankReceivedCheque"))
                    //   UserRolList.Add( "PrintReceiptVoucherBankReceivedCheque");





                    if ((model.ShowOpeningBalance) && (roleName == "ShowOpeningBalance"))
                        UserRolList.Add("ShowOpeningBalance");


                    if ((model.AddOpeningBalance) && (roleName == "AddOpeningBalance"))
                        UserRolList.Add("AddOpeningBalance");


                    if ((model.DeleteOpeningBalance) && (roleName == "DeleteOpeningBalance"))
                        UserRolList.Add("DeleteOpeningBalance");


                    if ((model.UpdateOpeningBalance) && (roleName == "UpdateOpeningBalance"))
                        UserRolList.Add("UpdateOpeningBalance");


                    if ((model.CopyOpeningBalance) && (roleName == "CopyOpeningBalance"))
                        UserRolList.Add("CopyOpeningBalance");

                    if ((model.PrintOpeningBalance) && (roleName == "PrintOpeningBalance"))
                        UserRolList.Add("PrintOpeningBalance");




                    if ((model.ShowReceiptVoucherCash) && (roleName == "ShowReceiptVoucherCash"))
                        UserRolList.Add("ShowReceiptVoucherCash");


                    if ((model.AddReceiptVoucherCash) && (roleName == "AddReceiptVoucherCash"))
                        UserRolList.Add("AddReceiptVoucherCash");


                    if ((model.DeleteReceiptVoucherCash) && (roleName == "DeleteReceiptVoucherCash"))
                        UserRolList.Add("DeleteReceiptVoucherCash");


                    if ((model.UpdateReceiptVoucherCash) && (roleName == "UpdateReceiptVoucherCash"))
                        UserRolList.Add("UpdateReceiptVoucherCash");


                    if ((model.CopyReceiptVoucherCash) && (roleName == "CopyReceiptVoucherCash"))
                        UserRolList.Add("CopyReceiptVoucherCash");

                    if ((model.PrintReceiptVoucherCash) && (roleName == "PrintReceiptVoucherCash"))
                        UserRolList.Add("PrintReceiptVoucherCash");

                    if ((model.ExportReceiptVoucherCash) && (roleName == "ExportReceiptVoucherCash"))
                        UserRolList.Add("ExportReceiptVoucherCash");


                    if ((model.UnExportReceiptVoucherCash) && (roleName == "UnExportReceiptVoucherCash"))
                        UserRolList.Add("UnExportReceiptVoucherCash");




                    if ((model.AttachReceiptVoucherCash) && (roleName == "AttachReceiptVoucherCash"))
                        UserRolList.Add("AttachReceiptVoucherCash");

                    if ((model.ShowTempRevenueReceived) && (roleName == "ShowTempRevenueReceived"))
                        UserRolList.Add("ShowTempRevenueReceived");


                    if ((model.AddTempRevenueReceived) && (roleName == "AddTempRevenueReceived"))
                        UserRolList.Add("AddTempRevenueReceived");


                    if ((model.DeleteTempRevenueReceived) && (roleName == "DeleteTempRevenueReceived"))
                        UserRolList.Add("DeleteTempRevenueReceived");


                    if ((model.UpdateTempRevenueReceived) && (roleName == "UpdateTempRevenueReceived"))
                        UserRolList.Add("UpdateTempRevenueReceived");


                    if ((model.CopyTempRevenueReceived) && (roleName == "CopyTempRevenueReceived"))
                        UserRolList.Add("CopyTempRevenueReceived");

                    if ((model.PrintTempRevenueReceived) && (roleName == "PrintTempRevenueReceived"))
                        UserRolList.Add("PrintTempRevenueReceived");

                    if ((model.AccumulativeTempRevenueReceived) && (roleName == "AccumulativeTempRevenueReceived"))
                        UserRolList.Add("AccumulativeTempRevenueReceived");

                    if ((model.AttachTempRevenueReceived) && (roleName == "AttachTempRevenueReceived"))
                        UserRolList.Add("AttachTempRevenueReceived");



                    if ((model.ShowCreditNote) && (roleName == "ShowCreditNote"))
                        UserRolList.Add("ShowCreditNote");


                    if ((model.AddCreditNote) && (roleName == "AddCreditNote"))
                        UserRolList.Add("AddCreditNote");


                    if ((model.DeleteCreditNote) && (roleName == "DeleteCreditNote"))
                        UserRolList.Add("DeleteCreditNote");


                    if ((model.UpdateCreditNote) && (roleName == "UpdateCreditNote"))
                        UserRolList.Add("UpdateCreditNote");


                    if ((model.CopyCreditNote) && (roleName == "CopyCreditNote"))
                        UserRolList.Add("CopyCreditNote");

                    if ((model.PrintCreditNote) && (roleName == "PrintCreditNote"))
                        UserRolList.Add("PrintCreditNote");

                    if ((model.ExportCreditNote) && (roleName == "ExportCreditNote"))
                        UserRolList.Add("ExportCreditNote");

                    if ((model.UnExportCreditNote) && (roleName == "UnExportCreditNote"))
                        UserRolList.Add("UnExportCreditNote");


                    if ((model.AttachCreditNote) && (roleName == "AttachCreditNote"))
                        UserRolList.Add("AttachCreditNote");

                    //if ((model.ShowPaymentVoucherBankPaymentCheque) && (roleName == "ShowPaymentVoucherBankPaymentCheque"))
                    //   UserRolList.Add( "ShowPaymentVoucherBankPaymentCheque");


                    //if ((model.AddPaymentVoucherBankPaymentCheque) && (roleName == "AddPaymentVoucherBankPaymentCheque"))
                    //   UserRolList.Add( "AddPaymentVoucherBankPaymentCheque");


                    //if ((model.DeletePaymentVoucherBankPaymentCheque) && (roleName == "DeletePaymentVoucherBankPaymentCheque"))
                    //   UserRolList.Add( "DeletePaymentVoucherBankPaymentCheque");


                    //if ((model.UpdatePaymentVoucherBankPaymentCheque) && (roleName == "UpdatePaymentVoucherBankPaymentCheque"))
                    //   UserRolList.Add( "UpdatePaymentVoucherBankPaymentCheque");


                    //if ((model.CopyPaymentVoucherBankPaymentCheque) && (roleName == "CopyPaymentVoucherBankPaymentCheque"))
                    //   UserRolList.Add( "CopyPaymentVoucherBankPaymentCheque");

                    //if ((model.PrintPaymentVoucherBankPaymentCheque) && (roleName == "PrintPaymentVoucherBankPaymentCheque"))
                    //   UserRolList.Add( "PrintPaymentVoucherBankPaymentCheque");







                    if ((model.ShowPaymentVoucherBank) && (roleName == "ShowPaymentVoucherBank"))
                        UserRolList.Add("ShowPaymentVoucherBank");


                    if ((model.AddPaymentVoucherBank) && (roleName == "AddPaymentVoucherBank"))
                        UserRolList.Add("AddPaymentVoucherBank");


                    if ((model.DeletePaymentVoucherBank) && (roleName == "DeletePaymentVoucherBank"))
                        UserRolList.Add("DeletePaymentVoucherBank");


                    if ((model.UpdatePaymentVoucherBank) && (roleName == "UpdatePaymentVoucherBank"))
                        UserRolList.Add("UpdatePaymentVoucherBank");


                    if ((model.CopyPaymentVoucherBank) && (roleName == "CopyPaymentVoucherBank"))
                        UserRolList.Add("CopyPaymentVoucherBank");

                    if ((model.PrintPaymentVoucherBank) && (roleName == "PrintPaymentVoucherBank"))
                        UserRolList.Add("PrintPaymentVoucherBank");

                    if ((model.ExportPaymentVoucherBank) && (roleName == "ExportPaymentVoucherBank"))
                        UserRolList.Add("ExportPaymentVoucherBank");

                    if ((model.UnExportPaymentVoucherBank) && (roleName == "UnExportPaymentVoucherBank"))
                        UserRolList.Add("UnExportPaymentVoucherBank");


                    if ((model.AttachPaymentVoucherBank) && (roleName == "AttachPaymentVoucherBank"))
                        UserRolList.Add("AttachPaymentVoucherBank");


                    if ((model.ShowPaymentVoucherCash) && (roleName == "ShowPaymentVoucherCash"))
                        UserRolList.Add("ShowPaymentVoucherCash");


                    if ((model.AddPaymentVoucherCash) && (roleName == "AddPaymentVoucherCash"))
                        UserRolList.Add("AddPaymentVoucherCash");


                    if ((model.DeletePaymentVoucherCash) && (roleName == "DeletePaymentVoucherCash"))
                        UserRolList.Add("DeletePaymentVoucherCash");


                    if ((model.UpdatePaymentVoucherCash) && (roleName == "UpdatePaymentVoucherCash"))
                        UserRolList.Add("UpdatePaymentVoucherCash");


                    if ((model.CopyPaymentVoucherCash) && (roleName == "CopyPaymentVoucherCash"))
                        UserRolList.Add("CopyPaymentVoucherCash");

                    if ((model.PrintPaymentVoucherCash) && (roleName == "PrintPaymentVoucherCash"))
                        UserRolList.Add("PrintPaymentVoucherCash");


                    if ((model.ExportPaymentVoucherCash) && (roleName == "ExportPaymentVoucherCash"))
                        UserRolList.Add("ExportPaymentVoucherCash");




                    if ((model.UnExportPaymentVoucherCash) && (roleName == "UnExportPaymentVoucherCash"))
                        UserRolList.Add("UnExportPaymentVoucherCash");


                    if ((model.AttachPaymentVoucherCash) && (roleName == "AttachPaymentVoucherCash"))
                        UserRolList.Add("AttachPaymentVoucherCash");



                    if ((model.ShowDepositInTheBank) && (roleName == "ShowDepositInTheBank"))
                        UserRolList.Add("ShowDepositInTheBank");


                    if ((model.AddDepositInTheBank) && (roleName == "AddDepositInTheBank"))
                        UserRolList.Add("AddDepositInTheBank");


                    if ((model.DeleteDepositInTheBank) && (roleName == "DeleteDepositInTheBank"))
                        UserRolList.Add("DeleteDepositInTheBank");


                    if ((model.UpdateDepositInTheBank) && (roleName == "UpdateDepositInTheBank"))
                        UserRolList.Add("UpdateDepositInTheBank");


                    if ((model.CopyDepositInTheBank) && (roleName == "CopyDepositInTheBank"))
                        UserRolList.Add("CopyDepositInTheBank");

                    if ((model.PrintDepositInTheBank) && (roleName == "PrintDepositInTheBank"))
                        UserRolList.Add("PrintDepositInTheBank");


                    if ((model.ExportDepositInTheBank) && (roleName == "ExportDepositInTheBank"))
                        UserRolList.Add("ExportDepositInTheBank");




                    if ((model.UnExportDepositInTheBank) && (roleName == "UnExportDepositInTheBank"))
                        UserRolList.Add("UnExportDepositInTheBank");


                    if ((model.AttachDepositInTheBank) && (roleName == "AttachDepositInTheBank"))
                        UserRolList.Add("AttachDepositInTheBank");


                    //------------------
                    if ((model.ShowReturnACheque) && (roleName == "ShowReturnACheque"))
                        UserRolList.Add("ShowReturnACheque");


                    if ((model.ShowTransferFromFundUC) && (roleName == "ShowTransferFromFundUC"))
                        UserRolList.Add("ShowTransferFromFundUC");


                    if ((model.ShowDrawingUC) && (roleName == "ShowDrawingUC"))
                        UserRolList.Add("ShowDrawingUC");


                    if ((model.ShowPaymentchequeUC) && (roleName == "ShowPaymentchequeUC"))
                        UserRolList.Add("ShowPaymentchequeUC");


                    if ((model.ShowTransferFromFundCD) && (roleName == "ShowTransferFromFundCD"))
                        UserRolList.Add("ShowTransferFromFundCD");

                    if ((model.ShowReturnAChequeCD) && (roleName == "ShowReturnAChequeCD"))
                        UserRolList.Add("ShowReturnAChequeCD");


                    if ((model.ShowTransferFromFundToChequeE) && (roleName == "ShowTransferFromFundToChequeE"))
                        UserRolList.Add("ShowTransferFromFundToChequeE");




                    if ((model.ShowPaymentChequeEndorsement) && (roleName == "ShowPaymentChequeEndorsement"))
                        UserRolList.Add("ShowPaymentChequeEndorsement");



                    if ((model.ShowReturnChequeUnderCollection) && (roleName == "ShowReturnChequeUnderCollection"))
                        UserRolList.Add("ShowReturnChequeUnderCollection");


                    if ((model.ShowReturnChequeEndorsement) && (roleName == "ShowReturnChequeEndorsement"))
                        UserRolList.Add("ShowReturnChequeEndorsement");

                    if ((model.ShowFundChequesDrawnFromUC) && (roleName == "ShowFundChequesDrawnFromUC"))
                        UserRolList.Add("ShowFundChequesDrawnFromUC");


                    if ((model.ShowReturnedChequeFund) && (roleName == "ShowReturnedChequeFund"))
                        UserRolList.Add("ShowReturnedChequeFund");


                    if ((model.ShowCourtFund) && (roleName == "ShowCourtFund"))
                        UserRolList.Add("ShowCourtFund");


                    if ((model.ShowPostdatedCheques) && (roleName == "ShowPostdatedCheques"))
                        UserRolList.Add("ShowPostdatedCheques");


                    if ((model.ShowReturnPostdatedCheques) && (roleName == "ShowReturnPostdatedCheques"))
                        UserRolList.Add("ShowReturnPostdatedCheques");

                    if ((model.ShowReturnPaidPostdatedCheques) && (roleName == "ShowReturnPaidPostdatedCheques"))
                        UserRolList.Add("ShowReturnPaidPostdatedCheques");



                    if ((model.ShowReceiptVoucherCashMultiAccount) && (roleName == "ShowReceiptVoucherCashMultiAccount"))
                        UserRolList.Add("ShowReceiptVoucherCashMultiAccount");
                    if ((model.AddReceiptVoucherCashMultiAccount) && (roleName == "AddReceiptVoucherCashMultiAccount"))
                        UserRolList.Add("AddReceiptVoucherCashMultiAccount");
                    if ((model.DeleteReceiptVoucherCashMultiAccount) && (roleName == "DeleteReceiptVoucherCashMultiAccount"))
                        UserRolList.Add("DeleteReceiptVoucherCashMultiAccount");
                    if ((model.UpdateReceiptVoucherCashMultiAccount) && (roleName == "UpdateReceiptVoucherCashMultiAccount"))
                        UserRolList.Add("UpdateReceiptVoucherCashMultiAccount");
                    if ((model.CopyReceiptVoucherCashMultiAccount) && (roleName == "CopyReceiptVoucherCashMultiAccount"))
                        UserRolList.Add("CopyReceiptVoucherCashMultiAccount");
                    if ((model.PrintReceiptVoucherCashMultiAccount) && (roleName == "PrintReceiptVoucherCashMultiAccount"))
                        UserRolList.Add("PrintReceiptVoucherCashMultiAccount");

                    if ((model.ExportReceiptVoucherCashMultiAccount) && (roleName == "ExportReceiptVoucherCashMultiAccount"))
                        UserRolList.Add("ExportReceiptVoucherCashMultiAccount");

                    if ((model.UnExportReceiptVoucherCashMultiAccount) && (roleName == "UnExportReceiptVoucherCashMultiAccount"))
                        UserRolList.Add("UnExportReceiptVoucherCashMultiAccount");


                    if ((model.AttachReceiptVoucherCashMultiAccount) && (roleName == "AttachReceiptVoucherCashMultiAccount"))
                        UserRolList.Add("AttachReceiptVoucherCashMultiAccount");


                    if ((model.ShowPaymentVoucherCashMultiAccount) && (roleName == "ShowPaymentVoucherCashMultiAccount"))
                        UserRolList.Add("ShowPaymentVoucherCashMultiAccount");
                    if ((model.AddPaymentVoucherCashMultiAccount) && (roleName == "AddPaymentVoucherCashMultiAccount"))
                        UserRolList.Add("AddPaymentVoucherCashMultiAccount");
                    if ((model.DeletePaymentVoucherCashMultiAccount) && (roleName == "DeletePaymentVoucherCashMultiAccount"))
                        UserRolList.Add("DeletePaymentVoucherCashMultiAccount");
                    if ((model.UpdatePaymentVoucherCashMultiAccount) && (roleName == "UpdatePaymentVoucherCashMultiAccount"))
                        UserRolList.Add("UpdatePaymentVoucherCashMultiAccount");
                    if ((model.CopyPaymentVoucherCashMultiAccount) && (roleName == "CopyPaymentVoucherCashMultiAccount"))
                        UserRolList.Add("CopyPaymentVoucherCashMultiAccount");
                    if ((model.PrintPaymentVoucherCashMultiAccount) && (roleName == "PrintPaymentVoucherCashMultiAccount"))
                        UserRolList.Add("PrintPaymentVoucherCashMultiAccount");

                    if ((model.ExportPaymentVoucherCashMultiAccount) && (roleName == "ExportPaymentVoucherCashMultiAccount"))
                        UserRolList.Add("ExportPaymentVoucherCashMultiAccount");

                    if ((model.UnExportPaymentVoucherCashMultiAccount) && (roleName == "UnExportPaymentVoucherCashMultiAccount"))
                        UserRolList.Add("UnExportPaymentVoucherCashMultiAccount");


                    if ((model.AttachPaymentVoucherCashMultiAccount) && (roleName == "AttachPaymentVoucherCashMultiAccount"))
                        UserRolList.Add("AttachPaymentVoucherCashMultiAccount");


 


                }
              //  _unitOfWork.Complete();

                //var duplicateKeys = UserRolList.GroupBy(x => x)
                //    .Where(group => group.Count() > 1)
                //    .Select(group => group.Key);
                try
                {
                    List<string> disUserRolList = UserRolList.Distinct().ToList();
                   var r= await UserManager.AddToRolesAsync(user.Id, disUserRolList.ToArray());
                }
                catch(Exception ex)
                {
                    string s = ex.Message;
                }
               
               


            }
            return true;
        }

        public void AddModify(ApplicationUser ObjToSave)
        { 
            var User = _context.Users.FirstOrDefault(m => m.Id == ObjToSave.Id && (m.fCompanyId == ObjToSave.fCompanyId || m.fCompanyId == 0));
            if (User != null)
            {


                User.AccountStatus = ObjToSave.AccountStatus;
                User.fCompanyId = ObjToSave.fCompanyId;
                User.PasswordHash = ObjToSave.PasswordHash;




            }
            else
            {
                _context.Users.Add(ObjToSave);
                //  UserManager.AddToRole(user.Id, "CoUser");
            }
        }


        public void DeActivate(ApplicationUser ObjToSave)
        {
            if (_context.Users.SingleOrDefault(m => m.fCompanyId == ObjToSave.fCompanyId && m.Id == ObjToSave.Id) != null)
            {
                var User = _context.Users.FirstOrDefault(m => m.Id == ObjToSave.Id);
                if (User != null)
                {
                    User.AccountStatus = ObjToSave.AccountStatus;
                }
            }

        }

        public void Delete(ApplicationUser ObjToSave)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ApplicationUser> GetAllUsers(int CoId)
        {
            return _context.Users.Where(m => m.fCompanyId == CoId).ToList();
        }

        public ApplicationUser GetUserByIDAndCo(int CoId, string UId)
        {
            return _context.Users.SingleOrDefault(m => m.fCompanyId == CoId && m.Id == UId);
        }
        public ApplicationUser GetUserByID(string UId)
        {
            return _context.Users.SingleOrDefault(m => m.Id == UId);
        }

        public ApplicationUser GetUserEmailID(string UId)
        {
            return _context.Users.SingleOrDefault(m => m.Email == UId);
        }

        public ApplicationUser GetUserByEmpIdAndCo(int CoId, string UId)
        {
            return _context.Users.SingleOrDefault(m => m.fCompanyId == CoId && m.UserId == UId);
        }

        public ApplicationUser GetUserEmailIDForEmailValidation(string UId, string Email)
        {
            return _context.Users.SingleOrDefault(m => m.Email == Email && m.Id != UId);
        }

        public void AddModifyFromCreateCompnay(ApplicationUser ObjToSave)
        {
            var User = _context.Users.FirstOrDefault(m => m.Id == ObjToSave.Id);
            if (User != null)
            {


                User.AccountStatus = ObjToSave.AccountStatus;
                User.fCompanyId = ObjToSave.fCompanyId;
                User.PasswordHash = ObjToSave.PasswordHash;



            }
            else
            {
                _context.Users.Add(ObjToSave);
                //  UserManager.AddToRole(user.Id, "CoUser");
            }
        }

        public ApplicationUser GetUserByEmailAndPassword(string Email, string Passord)
        {
            return _context.Users.SingleOrDefault(m => m.Email == Email && m.PasswordHash == Passord);
        }

        public void ChangePass(ApplicationUser ObjToSave)
        {//&& m.FCoID == ObjToSave.FCoID
            var User = _context.Users.FirstOrDefault(m => m.Id == ObjToSave.Id);
            if (User != null)
            {


                User.PasswordHash = ObjToSave.PasswordHash;
                User.RealPass = ObjToSave.RealPass;


            }

        }
        public ApplicationUser LoginMobile(string UserName, string Password)
        {
            return _context.Users.SingleOrDefault(m => m.Email == UserName && m.RealPass == Password);
        }


    }
}