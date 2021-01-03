using Acc.Models;
using Acc.Persistence;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Acc.Startup))]
namespace Acc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
        }
        private async void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            IdentityResult roleResult;

            // In Startup iam creating first Admin Role and creating a default Admin User    
            
            
            if (!roleManager.RoleExists("Admin"))
            {

                // first we create Admin rool   
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);
                //IdentityResult roleResult;
                //Here we create a Admin super user who will maintain the website                  

                var user = new ApplicationUser();
                user.UserName = "catnipsoft";
                user.Email = "mohammad.alnanaa@gmail.com";

                string userPWD = "Matrix__90$$";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");

                }
            }
            string[] roleNames = {
                "Admin", "CoOwner", "CoUser" ,
                

                "ShowCompany","UpdateCompany", //Company Screen
				 "ShowUser","AddUser" ,"UpdateUser","DeleteUser","PrintUser", //User Screen
                 "ShowCurrancy","AddCurrancy" ,"UpdateCurrancy","DeleteCurrancy","PrintCurrancy",//Currancy Screen
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
                
        //--//                             
        //--end company
	       //Operations//
           
                     "ShowTransferbetweenaccounts","TransferTransferbetweenaccounts", //Transferbetweenaccounts Screen      
                     "ShowFreezunfreezmonthtransaction","FreezFreezunfreezmonthtransaction","unFreezFreezunfreezmonthtransaction", //Freezunfreezmonthtransaction Screen   
                     "ShowEstimatedbudget","AddEstimatedbudget", //Estimatedbudget Screen
               //--end Operations


               //ِAccounts//

                      "ShowChartofaccount","AddmainaccountChartofaccount","UpdateChartofaccoun","DeleteChartofaccoun" , //Chartofaccoun Screen  
                      //"AddsubaccountChartofaccount","UpdatesubaccountChartofaccount","DeletesubaccountChartofaccount",
                      "ShowChartofcostcenter","AddmainaccountChartofcostcenter","UpdateChartofcostcenter","DeleteChartofcostcenter",
                      //"AddsubChartofcostcenter","UpdatesubChartofcostcenter","DeletesubChartofcostcenter",
                      "Showcustomeraccount","Addcustomeraccount", "Updatecustomeraccount","Deletecustomeraccount",//Newcustomeraccount Screen      
                      "ShowSupplieraccount","AddSupplieraccount","UpdateSupplieraccount","DeleteSupplieraccount", //NewSupplieraccount Screen  

          
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

                //-------------//


                //-- test nagggggggam--//
                //ابدعتي

                //--transaction--//

               "ShowTransActionTrans","AddTransActionTrans","DeleteTransActionTrans","UpdateTransActionTrans","CopyTransActionTrans","PrintTransActionTrans",  "ExportTransActionTrans" ,"UnExportTransActionTrans", "AttachTransActionTrans",

               "ShowReceiptVoucherBank","AddReceiptVoucherBank","DeleteReceiptVoucherBank","CopyReceiptVoucherBank","UpdateReceiptVoucherBank","PrintReceiptVoucherBank",  "ExportReceiptVoucherBank" ,"UnExportReceiptVoucherBank", "AttachReceiptVoucherBank",

               "ShowTempPrepaid","AddTempPrepaid","DeleteTempPrepaid","CopyTempPrepaid","UpdateTempPrepaid","PrintTempPrepaid", "AccumulativeTempPrepaid", "AttachTempPrepaid",

               "ShowDebitNote","AddDebitNote","DeleteDebitNote","CopyDebitNote","UpdateDebitNote","PrintDebitNote", "ExportDebitNote", "UnExportDebitNote",  "AttachDebitNote",

               "ShowServiceBill","AddServiceBill","DeleteServiceBill","CopyServiceBill","UpdateServiceBill","PrintServiceBill", "AttachServiceBill", "ExportServiceBill", "UnExportServiceBill" ,

              // "ShowReceiptVoucherBankReceivedCheque","AddReceiptVoucherBankReceivedCheque","DeleteReceiptVoucherBankReceivedCheque","CopyReceiptVoucherBankReceivedCheque","UpdateReceiptVoucherBankReceivedCheque","PrintReceiptVoucherBankReceivedCheque",

               "ShowOpeningBalance","AddOpeningBalance","DeleteOpeningBalance","CopyOpeningBalance","UpdateOpeningBalance", "PrintOpeningBalance",

               "ShowReceiptVoucherCash","AddReceiptVoucherCash","DeleteReceiptVoucherCash","CopyReceiptVoucherCash","UpdateReceiptVoucherCash", "PrintReceiptVoucherCash", "ExportReceiptVoucherCash" ,"UnExportReceiptVoucherCash", "AttachReceiptVoucherCash",

               "ShowTempRevenueReceived","AddTempRevenueReceived","DeleteTempRevenueReceived","CopyTempRevenueReceived","UpdateTempRevenueReceived","PrintTempRevenueReceived", "AccumulativeTempRevenueReceived", "AttachTempRevenueReceived",

               "ShowCreditNote","AddCreditNote","DeleteCreditNote","CopyCreditNote","UpdateCreditNote","PrintCreditNote", "ExportCreditNote", "UnExportCreditNote", "AttachCreditNote",

             //  "ShowPaymentVoucherBankPaymentCheque","AddPaymentVoucherBankPaymentCheque","DeletePaymentVoucherBankPaymentCheque","CopyPaymentVoucherBankPaymentCheque","UpdatePaymentVoucherBankPaymentCheque","PrintPaymentVoucherBankPaymentCheque",

               "ShowPaymentVoucherBank","AddPaymentVoucherBank","DeletePaymentVoucherBank","CopyPaymentVoucherBank","UpdatePaymentVoucherBank","PrintPaymentVoucherBank", "ExportPaymentVoucherBank" ,"UnExportPaymentVoucherBank","AttachPaymentVoucherBank",

               "ShowPaymentVoucherCash","AddPaymentVoucherCash","DeletePaymentVoucherCash","CopyPaymentVoucherCash","UpdatePaymentVoucherCash","PrintPaymentVoucherCash", "ExportPaymentVoucherCash" ,"UnExportPaymentVoucherCash", "AttachPaymentVoucherCash",

               "ShowDepositInTheBank","AddDepositInTheBank","DeleteDepositInTheBank","CopyDepositInTheBank","UpdateDepositInTheBank","PrintDepositInTheBank" ,"ExportDepositInTheBank" ,"UnExportDepositInTheBank" ,"AttachDepositInTheBank" ,

               "ShowReturnACheque" , "ShowTransferFromFundUC" ,"ShowDrawingUC" ,"ShowPaymentchequeUC" , "ShowTransferFromFundCD" ,"ShowReturnAChequeCD" ,"ShowTransferFromFundToChequeE" ,"ShowPaymentChequeEndorsement" ,"ShowReturnChequeUnderCollection", "ShowReturnChequeEndorsement", "ShowFundChequesDrawnFromUC" , "ShowReturnedChequeFund" , "ShowCourtFund",
               "ShowPostdatedCheques" , "ShowReturnPostdatedCheques" , "ShowReturnPaidPostdatedCheques",


               "ShowPaymentVoucherCashMultiAccount" , "AddPaymentVoucherCashMultiAccount" , "DeletePaymentVoucherCashMultiAccount" ,"UpdatePaymentVoucherCashMultiAccount" , "CopyPaymentVoucherCashMultiAccount","PrintPaymentVoucherCashMultiAccount" , "ExportPaymentVoucherCashMultiAccount", "UnExportPaymentVoucherCashMultiAccount", "AttachPaymentVoucherCashMultiAccount",
               "ShowReceiptVoucherCashMultiAccount" , "AddReceiptVoucherCashMultiAccount" ,"DeleteReceiptVoucherCashMultiAccount" ,"UpdateReceiptVoucherCashMultiAccount" , "CopyReceiptVoucherCashMultiAccount" ,"PrintReceiptVoucherCashMultiAccount" , "ExportReceiptVoucherCashMultiAccount" ,"UnExportReceiptVoucherCashMultiAccount" ,"AttachReceiptVoucherCashMultiAccount",





               //---Spical Permision ----//

                 "WorkWithStock","Hiajneh"



            };
            foreach (var roleName in roleNames)
            {
                // creating the roles and seeding them to the database
                
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }





        }
    }
}
