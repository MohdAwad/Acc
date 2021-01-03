using Acc.Helpers;
using Acc.Models;
using Acc.Persistence;
using Acc.Repositories;
using Acc.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Acc.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private readonly IUnitOfWork _unitOfWork;


        public AccountController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }

        [HttpPost]
        [AllowAnonymous]
        //   [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateUser(UserAccountVM UserData)
        {
            MsgUnit Msg = new MsgUnit();
            ApplicationUser model = UserData.ApplicationUser;
            //  ObjToSave.AddBYUserID = userId;

            var UserId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetUserByID(UserId);



            int e = UserData.Email.IndexOf('@');
            int Co = UserInfo.Email.IndexOf('@');
            if (e < 0)
            {
                model.Email = UserData.Email + UserInfo.Email.Substring(Co);
            }
            else
            {
                //Msg.Msg = Resources.Resource.EmailMustBe;
                //Msg.Code = 0;
                //// return RedirectToAction("Index", "Users");
                //return Json(Msg, JsonRequestBehavior.AllowGet);
            }
            if (!ModelState.IsValid)
            {
                string Err = " ";
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (ModelError error in errors)
                {
                    Err = Err + error.ErrorMessage + "  ";
                }

                Msg.Msg = Resources.Resource.SomthingWentWrong + " " + Err;
                Msg.Code = 0;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }

            //   if (ModelState.IsValid)
            try
            {



                var user = new ApplicationUser();
                 

                user = model;

                user.UserName = model.Email;
                user.Email = model.Email;
                user.UserType = model.UserType;
                user.fCompanyId = UserInfo.fCompanyId;


                //
                var result = await UserManager.CreateAsync(user, UserData.Password);
                if (result.Succeeded)
                {
                  
                   
                    string[] roleNames = {
                "Admin", "CoOwner", "CoUser" ,
                //"ShowChartOfAccount","AddAccount","DeleteAccount",
                //"UpdateAccountName","UpdateAccountKid","UpdateAcountType","StopAccount",


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

                //---TransAction--//
               "ShowTransActionTrans","AddTransActionTrans","DeleteTransActionTrans","UpdateTransActionTrans","CopyTransActionTrans","PrintTransActionTrans",  "ExportTransActionTrans" ,"UnExportTransActionTrans", "AttachTransActionTrans",

               "ShowReceiptVoucherBank","AddReceiptVoucherBank","DeleteReceiptVoucherBank","CopyReceiptVoucherBank","UpdateReceiptVoucherBank","PrintReceiptVoucherBank",  "ExportReceiptVoucherBank" ,"UnExportReceiptVoucherBank", "AttachReceiptVoucherBank",

               "ShowTempPrepaid","AddTempPrepaid","DeleteTempPrepaid","CopyTempPrepaid","UpdateTempPrepaid","PrintTempPrepaid", "AccumulativeTempPrepaid", "AttachTempPrepaid",

               "ShowDebitNote","AddDebitNote","DeleteDebitNote","CopyDebitNote","UpdateDebitNote","PrintDebitNote", "ExportDebitNote", "UnExportDebitNote",  "AttachDebitNote",

               "ShowServiceBill","AddServiceBill","DeleteServiceBill","CopyServiceBill","UpdateServiceBill","PrintServiceBill", "AttachServiceBill", "ExportServiceBill", "UnExportServiceBill",

              // "ShowReceiptVoucherBankReceivedCheque","AddReceiptVoucherBankReceivedCheque","DeleteReceiptVoucherBankReceivedCheque","CopyReceiptVoucherBankReceivedCheque","UpdateReceiptVoucherBankReceivedCheque","PrintReceiptVoucherBankReceivedCheque",

               "ShowOpeningBalance","AddOpeningBalance","DeleteOpeningBalance","CopyOpeningBalance","UpdateOpeningBalance", "PrintOpeningBalance",

               "ShowReceiptVoucherCash","AddReceiptVoucherCash","DeleteReceiptVoucherCash","CopyReceiptVoucherCash","UpdateReceiptVoucherCash", "PrintReceiptVoucherCash", "ExportReceiptVoucherCash" ,"UnExportReceiptVoucherCash", "AttachReceiptVoucherCash",

               "ShowTempRevenueReceived","AddTempRevenueReceived","DeleteTempRevenueReceived","CopyTempRevenueReceived","UpdateTempRevenueReceived","PrintTempRevenueReceived", "AccumulativeTempRevenueReceived","AttachTempRevenueReceived",

               "ShowCreditNote","AddCreditNote","DeleteCreditNote","CopyCreditNote","UpdateCreditNote","PrintCreditNote", "ExportCreditNote", "UnExportCreditNote", "AttachCreditNote",

              // "ShowPaymentVoucherBankPaymentCheque","AddPaymentVoucherBankPaymentCheque","DeletePaymentVoucherBankPaymentCheque","CopyPaymentVoucherBankPaymentCheque","UpdatePaymentVoucherBankPaymentCheque","PrintPaymentVoucherBankPaymentCheque",

               "ShowPaymentVoucherBank","AddPaymentVoucherBank","DeletePaymentVoucherBank","CopyPaymentVoucherBank","UpdatePaymentVoucherBank","PrintPaymentVoucherBank", "ExportPaymentVoucherBank" ,"UnExportPaymentVoucherBank","AttachPaymentVoucherBank",

               "ShowPaymentVoucherCash","AddPaymentVoucherCash","DeletePaymentVoucherCash","CopyPaymentVoucherCash","UpdatePaymentVoucherCash","PrintPaymentVoucherCash", "ExportPaymentVoucherCash" ,"UnExportPaymentVoucherCash", "AttachPaymentVoucherCash",

               "ShowDepositInTheBank","AddDepositInTheBank","DeleteDepositInTheBank","CopyDepositInTheBank","UpdateDepositInTheBank","PrintDepositInTheBank" ,"ExportDepositInTheBank" ,"UnExportDepositInTheBank" ,"AttachDepositInTheBank",

               "ShowReturnACheque" , "ShowTransferFromFundUC" ,"ShowDrawingUC" ,"ShowPaymentchequeUC" , "ShowTransferFromFundCD" ,"ShowReturnAChequeCD" ,"ShowTransferFromFundToChequeE" ,"ShowPaymentChequeEndorsement" , "ShowReturnChequeUnderCollection" ,"ShowReturnChequeEndorsement", "ShowFundChequesDrawnFromUC" , "ShowReturnedChequeFund" , "ShowCourtFund",
               "ShowPostdatedCheques" , "ShowReturnPostdatedCheques" , "ShowReturnPaidPostdatedCheques" ,

               "ShowPaymentVoucherCashMultiAccount" , "AddPaymentVoucherCashMultiAccount" , "DeletePaymentVoucherCashMultiAccount" ,"UpdatePaymentVoucherCashMultiAccount" , "CopyPaymentVoucherCashMultiAccount","PrintPaymentVoucherCashMultiAccount" , "ExportPaymentVoucherCashMultiAccount", "UnExportPaymentVoucherCashMultiAccount", "AttachPaymentVoucherCashMultiAccount",
               "ShowReceiptVoucherCashMultiAccount" , "AddReceiptVoucherCashMultiAccount" ,"DeleteReceiptVoucherCashMultiAccount" ,"UpdateReceiptVoucherCashMultiAccount" , "CopyReceiptVoucherCashMultiAccount" ,"PrintReceiptVoucherCashMultiAccount" , "ExportReceiptVoucherCashMultiAccount" ,"UnExportReceiptVoucherCashMultiAccount" ,"AttachReceiptVoucherCashMultiAccount",
                "WorkWithStock","Hiajneh"



            };
                   // await UserManager.AddToRoleAsync(user.Id, "CoUser");
                    string[] sUserRole = { };
                    List<string> UserRolList = new List<string>();
                    var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                    UserRolList.Add("CoUser");
                    if (CoInfo.WorkWithStock == 1)
                    {
                       UserRolList.Add( "WorkWithStock");
                    }
                    if (CoInfo.Hiajneh == 1)
                    {
                       UserRolList.Add( "Hiajneh");
                    }
             
                 
                  
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
                           UserRolList.Add( "UpdateCompany");



                        if ((model.ShowUser) && (roleName == "ShowUser"))
                           UserRolList.Add( "ShowUser");
                        if ((model.AddUser) && (roleName == "AddUser"))
                           UserRolList.Add( "AddUser");
                        if ((model.UpdateUser) && (roleName == "UpdateUser"))
                           UserRolList.Add( "UpdateUser");
                        if ((model.DeleteUser) && (roleName == "DeleteUser"))
                           UserRolList.Add( "DeleteUser");
                        if ((model.PrintUser) && (roleName == "PrintUser"))
                           UserRolList.Add( "PrintUser");


                        if ((model.ShowCurrancy) && (roleName == "ShowCurrancy"))
                           UserRolList.Add( "ShowCurrancy");
                        if ((model.AddCurrancy) && (roleName == "AddCurrancy"))
                           UserRolList.Add( "AddCurrancy");
                        if ((model.UpdateCurrancy) && (roleName == "UpdateCurrancy"))
                           UserRolList.Add( "UpdateCurrancy");
                        if ((model.DeleteCurrancy) && (roleName == "DeleteCurrancy"))
                           UserRolList.Add( "DeleteCurrancy");
                        if ((model.PrintCurrancy) && (roleName == "PrintCurrancy"))
                           UserRolList.Add( "PrintCurrancy");




                        if ((model.ShowCurrancyValue) && (roleName == "ShowCurrancyValue"))
                           UserRolList.Add( "ShowCurrancyValue");
                        if ((model.AddCurrancyValue) && (roleName == "AddCurrancyValue"))
                           UserRolList.Add( "AddCurrancyValue");
                        if ((model.PrintCurrancyValue) && (roleName == "PrintCurrancyValue"))
                           UserRolList.Add( "PrintCurrancyValue");



                        if ((model.ShowSalesman) && (roleName == "ShowSalesman"))
                           UserRolList.Add( "ShowSalesman");
                        if ((model.AddSalesman) && (roleName == "AddSalesman"))
                           UserRolList.Add( "AddSalesman");
                        if ((model.UpdateSalesman) && (roleName == "UpdateSalesman"))
                           UserRolList.Add( "UpdateSalesman");
                        if ((model.DeleteSalesman) && (roleName == "DeleteSalesman"))
                           UserRolList.Add( "DeleteSalesman");
                        if ((model.PrintSalesman) && (roleName == "PrintSalesman"))
                           UserRolList.Add( "PrintSalesman");


                        
                        if ((model.ShowTransaction) && (roleName == "ShowTransaction"))
                           UserRolList.Add( "ShowTransaction");
                        if ((model.AddTransction) && (roleName == "AddTransction"))
                           UserRolList.Add( "AddTransction");
                        if ((model.UpdateTransction) && (roleName == "UpdateTransction"))
                           UserRolList.Add( "UpdateTransction");
                        if ((model.DeleteTransction) && (roleName == "DeleteTransction"))
                           UserRolList.Add( "DeleteTransction");
                        if ((model.PrintTransction) && (roleName == "PrintTransction"))
                           UserRolList.Add( "PrintTransction");

                        



                        if ((model.ShowServicegroup) && (roleName == "ShowServicegroup"))
                           UserRolList.Add( "ShowServicegroup");
                        if ((model.AddServicegroup) && (roleName == "AddServicegroup"))
                           UserRolList.Add( "AddServicegroup");
                        if ((model.UpdateServicegroup) && (roleName == "UpdateServicegroup"))
                           UserRolList.Add( "UpdateServicegroup");
                        if ((model.DeleteServicegroup) && (roleName == "DeleteServicegroup"))
                           UserRolList.Add( "DeleteServicegroup");
                        if ((model.PrintServicegroup) && (roleName == "PrintServicegroup"))
                           UserRolList.Add( "PrintServicegroup");




                        if ((model.ShowBankswithdrawnfrom) && (roleName == "ShowBankswithdrawnfrom"))
                           UserRolList.Add( "ShowBankswithdrawnfrom");
                        if ((model.AddBankswithdrawnfrom) && (roleName == "AddBankswithdrawnfrom"))
                           UserRolList.Add( "AddBankswithdrawnfrom");
                        if ((model.UpdateBankswithdrawnfrom) && (roleName == "UpdateBankswithdrawnfrom"))
                           UserRolList.Add( "UpdateBankswithdrawnfrom");
                        if ((model.PrintBankswithdrawnfrom) && (roleName == "PrintBankswithdrawnfrom"))
                           UserRolList.Add( "PrintBankswithdrawnfrom");



                        if ((model.ShowBank) && (roleName == "ShowBank"))
                           UserRolList.Add( "ShowBank");
                        if ((model.AddBank) && (roleName == "AddBank"))
                           UserRolList.Add( "AddBank");
                        if ((model.UpdateBank) && (roleName == "UpdateBank"))
                           UserRolList.Add( "UpdateBank");
                        if ((model.DeleteBank) && (roleName == "DeleteBank"))
                           UserRolList.Add( "DeleteBank");
                        if ((model.PrintBank) && (roleName == "PrintBank"))
                           UserRolList.Add( "PrintBank");


                        if ((model.ShowOtheraccount) && (roleName == "ShowOtheraccount"))
                           UserRolList.Add( "ShowOtheraccount");
                        if ((model.AddOtheraccount) && (roleName == "AddOtheraccount"))
                           UserRolList.Add( "AddOtheraccount");
                        if ((model.UpdateOtheraccount) && (roleName == "UpdateOtheraccount"))
                           UserRolList.Add( "UpdateOtheraccount");
                        if ((model.DeleteOtheraccount) && (roleName == "DeleteOtheraccount"))
                           UserRolList.Add( "DeleteOtheraccount");
                        if ((model.PrintOtheraccount) && (roleName == "PrintOtheraccount"))
                           UserRolList.Add( "PrintOtheraccount");


                        if ((model.ShowManagement) && (roleName == "ShowManagement"))
                           UserRolList.Add( "ShowManagement");
                        if ((model.AddManagement) && (roleName == "AddManagement"))
                           UserRolList.Add( "AddManagement");
                        if ((model.UpdateManagement) && (roleName == "UpdateManagement"))
                           UserRolList.Add( "UpdateManagement");
                        if ((model.DeleteManagement) && (roleName == "DeleteManagement"))
                           UserRolList.Add( "DeleteManagement");
                        if ((model.PrintManagement) && (roleName == "PrintManagement"))
                           UserRolList.Add( "PrintManagement");


                        if ((model.ShowDepartment) && (roleName == "ShowDepartment"))
                           UserRolList.Add( "ShowDepartment");
                        if ((model.AddDepartment) && (roleName == "AddDepartment"))
                           UserRolList.Add( "AddDepartment");
                        if ((model.UpdateDepartment) && (roleName == "UpdateDepartment"))
                           UserRolList.Add( "UpdateDepartment");
                        if ((model.DeleteDepartment) && (roleName == "DeleteDepartment"))
                           UserRolList.Add( "DeleteDepartment");
                        if ((model.PrintDepartment) && (roleName == "PrintDepartment"))
                           UserRolList.Add( "PrintDepartment");



                        if ((model.ShowSections) && (roleName == "ShowSections"))
                           UserRolList.Add( "ShowSections");
                        if ((model.AddSections) && (roleName == "AddSections"))
                           UserRolList.Add( "AddSections");
                        if ((model.UpdateSections) && (roleName == "UpdateSections"))
                           UserRolList.Add( "UpdateSections");
                        if ((model.DeleteSections) && (roleName == "DeleteSections"))
                           UserRolList.Add( "DeleteSections");
                        if ((model.PrintSections) && (roleName == "PrintSections"))
                           UserRolList.Add( "PrintSections");





                        if ((model.ShowLocations) && (roleName == "ShowLocations"))
                           UserRolList.Add( "ShowLocations");
                        if ((model.AddSections) && (roleName == "AddLocations"))
                           UserRolList.Add( "AddLocations");
                        if ((model.UpdateLocations) && (roleName == "UpdateLocations"))
                           UserRolList.Add( "UpdateLocations");
                        if ((model.DeleteLocations) && (roleName == "DeleteLocations"))
                           UserRolList.Add( "DeleteLocations");
                        if ((model.PrintSections) && (roleName == "PrintLocations"))
                           UserRolList.Add( "PrintLocations");


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
                           UserRolList.Add( "ShowTrustee");
                        if ((model.AddTrustee) && (roleName == "AddTrustee"))
                           UserRolList.Add( "AddTrustee");
                        if ((model.UpdateTrustee) && (roleName == "UpdateTrustee"))
                           UserRolList.Add( "UpdateTrustee");
                        if ((model.DeleteTrustee) && (roleName == "DeleteTrustee"))
                           UserRolList.Add( "DeleteTrustee");
                        if ((model.PrintTrustee) && (roleName == "PrintTrustee"))
                           UserRolList.Add( "PrintTrustee");



                        if ((model.ShowSupplierbanks) && (roleName == "ShowSupplierbanks"))
                           UserRolList.Add( "ShowSupplierbanks");
                        if ((model.AddSupplierbanks) && (roleName == "AddSupplierbanks"))
                           UserRolList.Add( "AddSupplierbanks");
                        if ((model.UpdateSupplierbanks) && (roleName == "UpdateSupplierbanks"))
                           UserRolList.Add( "UpdateSupplierbanks");
                        if ((model.DeleteSupplierbanks) && (roleName == "DeleteSupplierbanks"))
                           UserRolList.Add( "DeleteSupplierbanks");
                        if ((model.PrintSupplierbanks) && (roleName == "PrintSupplierbanks"))
                           UserRolList.Add( "PrintSupplierbanks");


                        if ((model.ShowSuppliercountries) && (roleName == "ShowSuppliercountries"))
                           UserRolList.Add( "ShowSuppliercountries");
                        if ((model.AddSuppliercountries) && (roleName == "AddSuppliercountries"))
                           UserRolList.Add( "AddSuppliercountries");
                        if ((model.UpdateSuppliercountries) && (roleName == "UpdateSuppliercountries"))
                           UserRolList.Add( "UpdateSuppliercountries");
                        if ((model.DeleteSuppliercountries) && (roleName == "DeleteSuppliercountries"))
                           UserRolList.Add( "DeleteSuppliercountries");
                        if ((model.PrintSuppliercountries) && (roleName == "PrintSuppliercountries"))
                           UserRolList.Add( "PrintSuppliercountries");


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
                           UserRolList.Add( "ShowSupplierbankcountries");
                        if ((model.AddSupplierbankcountries) && (roleName == "AddSupplierbankcountries"))
                           UserRolList.Add( "AddSupplierbankcountries");
                        if ((model.UpdateSupplierbankcountries) && (roleName == "UpdateSupplierbankcountries"))
                           UserRolList.Add( "UpdateSupplierbankcountries");
                        if ((model.DeleteSupplierbankcountries) && (roleName == "DeleteSupplierbankcountries"))
                           UserRolList.Add( "DeleteSupplierbankcountries");
                        if ((model.PrintSupplierbankcountries) && (roleName == "PrintSupplierbankcountries"))
                           UserRolList.Add( "PrintSupplierbankcountries");



                        if ((model.ShowSupplierbankbranches) && (roleName == "ShowSupplierbankbranches"))
                           UserRolList.Add( "ShowSupplierbankbranches");
                        if ((model.AddSupplierbankbranches) && (roleName == "AddSupplierbankbranches"))
                           UserRolList.Add( "AddSupplierbankbranches");
                        if ((model.UpdateSupplierbankbranches) && (roleName == "UpdateSupplierbankbranches"))
                           UserRolList.Add( "UpdateSupplierbankbranches");
                        if ((model.DeleteSupplierbankbranches) && (roleName == "DeleteSupplierbankbranches"))
                           UserRolList.Add( "DeleteSupplierbankbranches");
                        if ((model.PrintSupplierbankbranches) && (roleName == "PrintSupplierbankbranches"))
                           UserRolList.Add( "PrintSupplierbankbranches");


                        if ((model.ShowSuppliercity) && (roleName == "ShowSuppliercity"))
                           UserRolList.Add( "ShowSuppliercity");
                        if ((model.AddSuppliercity) && (roleName == "AddSuppliercity"))
                           UserRolList.Add( "AddSuppliercity");
                        if ((model.UpdateSuppliercity) && (roleName == "UpdateSuppliercity"))
                           UserRolList.Add( "UpdateSuppliercity");
                        if ((model.DeleteSuppliercity) && (roleName == "DeleteSuppliercity"))
                           UserRolList.Add( "DeleteSuppliercity");
                        if ((model.PrintSuppliercity) && (roleName == "PrintSuppliercity"))
                           UserRolList.Add( "PrintSuppliercity");




                        if ((model.ShowSuppliersbankcity) && (roleName == "ShowSuppliersbankcity"))
                           UserRolList.Add( "ShowSuppliersbankcity");
                        if ((model.AddSuppliersbankcity) && (roleName == "AddSuppliersbankcity"))
                           UserRolList.Add( "AddSuppliersbankcity");
                        if ((model.UpdateSuppliersbankcity) && (roleName == "UpdateSuppliersbankcity"))
                           UserRolList.Add( "UpdateSuppliersbankcity");
                        if ((model.DeleteSuppliersbankcity) && (roleName == "DeleteSuppliersbankcity"))
                           UserRolList.Add( "DeleteSuppliersbankcity");
                        if ((model.PrintSuppliersbankcity) && (roleName == "PrintSuppliersbankcity"))
                           UserRolList.Add( "PrintSuppliersbankcity");


                        if ((model.ShowCustomercity) && (roleName == "ShowCustomercity"))
                           UserRolList.Add( "ShowCustomercity");
                        if ((model.AddCustomercity) && (roleName == "AddCustomercity"))
                           UserRolList.Add( "AddCustomercity");
                        if ((model.UpdateCustomercity) && (roleName == "UpdateCustomercity"))
                           UserRolList.Add( "UpdateCustomercity");
                        if ((model.DeleteCustomercity) && (roleName == "DeleteCustomercity"))
                           UserRolList.Add( "DeleteCustomercity");
                        if ((model.PrintCustomercity) && (roleName == "PrintCustomercity"))
                           UserRolList.Add( "PrintCustomercity");


                        if ((model.ShowCustomerarea) && (roleName == "ShowCustomerarea"))
                           UserRolList.Add( "ShowCustomerarea");
                        if ((model.AddCustomerarea) && (roleName == "AddCustomerarea"))
                           UserRolList.Add( "AddCustomerarea");
                        if ((model.UpdateCustomerarea) && (roleName == "UpdateCustomerarea"))
                           UserRolList.Add( "UpdateCustomerarea");
                        if ((model.DeleteCustomerarea) && (roleName == "DeleteCustomerarea"))
                           UserRolList.Add( "DeleteCustomerarea");
                        if ((model.PrintCustomerarea) && (roleName == "PrintCustomerarea"))
                           UserRolList.Add( "PrintCustomerarea");



                        if ((model.ShowService) && (roleName == "ShowService"))
                           UserRolList.Add( "ShowService");
                        if ((model.AddService) && (roleName == "AddService"))
                           UserRolList.Add( "AddService");
                        if ((model.UpdateService) && (roleName == "UpdateService"))
                           UserRolList.Add( "UpdateService");
                        if ((model.DeleteService) && (roleName == "DeleteService"))
                           UserRolList.Add( "DeleteService");
                        if ((model.PrintService) && (roleName == "PrintService"))
                           UserRolList.Add( "PrintService");

 


                        ///----operation***//
                        ///
                        if ((model.ShowTransferbetweenaccounts) && (roleName == "ShowTransferbetweenaccounts"))
                           UserRolList.Add( "ShowTransferbetweenaccounts");
                        if ((model.TransferTransferbetweenaccounts) && (roleName == "TransferTransferbetweenaccounts"))
                           UserRolList.Add( "TransferTransferbetweenaccounts");



                        if ((model.ShowFreezunfreezmonthtransaction) && (roleName == "ShowFreezunfreezmonthtransaction"))
                           UserRolList.Add( "ShowFreezunfreezmonthtransaction");
                        if ((model.FreezFreezunfreezmonthtransaction) && (roleName == "FreezFreezunfreezmonthtransaction"))
                           UserRolList.Add( "FreezFreezunfreezmonthtransaction");
                        if ((model.unFreezFreezunfreezmonthtransaction) && (roleName == "unFreezFreezunfreezmonthtransaction"))
                           UserRolList.Add( "unFreezFreezunfreezmonthtransaction");



                        if ((model.ShowEstimatedbudget) && (roleName == "ShowEstimatedbudget"))
                           UserRolList.Add( "ShowEstimatedbudget");
                        if ((model.AddEstimatedbudget) && (roleName == "AddEstimatedbudget"))
                           UserRolList.Add( "AddEstimatedbudget");

                        ///---------//
                        ///

                        //--Accuont---


                        if ((model.ShowChartofaccount) && (roleName == "ShowChartofaccount"))
                           UserRolList.Add( "ShowChartofaccount");
                        if ((model.AddmainaccountChartofaccount) && (roleName == "AddmainaccountChartofaccount"))
                           UserRolList.Add( "AddmainaccountChartofaccount");
                        if ((model.UpdateChartofaccoun) && (roleName == "UpdateChartofaccoun"))
                           UserRolList.Add( "UpdateChartofaccoun");
                        if ((model.DeleteChartofaccoun) && (roleName == "DeleteChartofaccoun"))
                           UserRolList.Add( "DeleteChartofaccoun");








                        if ((model.AddsubaccountChartofaccount) && (roleName == "AddsubaccountChartofaccount"))
                           UserRolList.Add( "AddsubaccountChartofaccount");
                        if ((model.UpdatesubaccountChartofaccount) && (roleName == "UpdatesubaccountChartofaccount"))
                           UserRolList.Add( "UpdatesubaccountChartofaccount");
                        if ((model.DeletesubaccountChartofaccount) && (roleName == "DeletesubaccountChartofaccount"))
                           UserRolList.Add( "DeletesubaccountChartofaccount");




                        if ((model.ShowChartofcostcenter) && (roleName == "ShowChartofcostcenter"))
                           UserRolList.Add( "ShowChartofcostcenter");
                        if ((model.AddmainaccountChartofcostcenter) && (roleName == "AddmainaccountChartofcostcenter"))
                           UserRolList.Add( "AddmainaccountChartofcostcenter");
                        if ((model.UpdateChartofcostcenter) && (roleName == "UpdateChartofcostcenter"))
                           UserRolList.Add( "UpdateChartofcostcenter");
                        if ((model.DeleteChartofcostcenter) && (roleName == "DeleteChartofcostcenter"))
                           UserRolList.Add( "DeleteChartofcostcenter");





                        if ((model.AddsubChartofcostcenter) && (roleName == "AddsubChartofcostcenter"))
                           UserRolList.Add( "AddsubChartofcostcenter");
                        if ((model.UpdatesubChartofcostcenter) && (roleName == "UpdatesubChartofcostcenter"))
                           UserRolList.Add( "UpdatesubChartofcostcenter");
                        if ((model.DeletesubChartofcostcenter) && (roleName == "DeletesubChartofcostcenter"))
                           UserRolList.Add( "DeletesubChartofcostcenter");




                        if ((model.Showcustomeraccount) && (roleName == "Showcustomeraccount"))
                           UserRolList.Add( "Showcustomeraccount");
                        if ((model.Addcustomeraccount) && (roleName == "Addcustomeraccount"))
                           UserRolList.Add( "Addcustomeraccount");
                        if ((model.Updatecustomeraccount) && (roleName == "Updatecustomeraccount"))
                           UserRolList.Add( "Updatecustomeraccount");
                        if ((model.Deletecustomeraccount) && (roleName == "Deletecustomeraccount"))
                           UserRolList.Add( "Deletecustomeraccount");





                        if ((model.ShowSupplieraccount) && (roleName == "ShowSupplieraccount"))
                           UserRolList.Add( "ShowSupplieraccount");
                        if ((model.AddSupplieraccount) && (roleName == "AddSupplieraccount"))
                           UserRolList.Add("AddSupplieraccount");
                        if ((model.UpdateSupplieraccount) && (roleName == "UpdateSupplieraccount"))
                           UserRolList.Add("UpdateSupplieraccount");
                        if ((model.DeleteSupplieraccount) && (roleName == "DeleteSupplieraccount"))
                           UserRolList.Add( "DeleteSupplieraccount");




                        //--Assset--//

                        if ((model.ShowAssetType) && (roleName == "ShowAssetType"))
                           UserRolList.Add( "ShowAssetType");
                        if ((model.AddAssetType) && (roleName == "AddAssetType"))
                           UserRolList.Add( "AddAssetType");
                        if ((model.UpdateAssetType) && (roleName == "UpdateAssetType"))
                           UserRolList.Add( "UpdateAssetType");
                        if ((model.DeleteAssetType) && (roleName == "DeleteAssetType"))
                           UserRolList.Add( "DeleteAssetType");
                        if ((model.PrintAssetType) && (roleName == "PrintAssetType"))
                           UserRolList.Add( "PrintAssetType");



                        if ((model.ShowCalculationOfConsumption) && (roleName == "ShowCalculationOfConsumption"))
                           UserRolList.Add( "ShowCalculationOfConsumption");
                        if ((model.AddCalculationOfConsumption) && (roleName == "AddCalculationOfConsumption"))
                           UserRolList.Add( "AddCalculationOfConsumption");


                        if ((model.ShowAsset) && (roleName == "ShowAsset"))
                           UserRolList.Add( "ShowAsset");
                        if ((model.AddAsset) && (roleName == "AddAsset"))
                           UserRolList.Add( "AddAsset");
                        if ((model.UpdateAsset) && (roleName == "UpdateAsset"))
                           UserRolList.Add( "UpdateAsset");
                        if ((model.DeleteAsset) && (roleName == "DeleteAsset"))
                           UserRolList.Add( "DeleteAsset");
                        if ((model.PrintAsset) && (roleName == "PrintAsset"))
                           UserRolList.Add( "PrintAsset");


                        if ((model.ShowAssetToAcc) && (roleName == "ShowAssetToAcc"))
                           UserRolList.Add( "ShowAssetToAcc");
                        if ((model.AddAssetToAcc) && (roleName == "AddAssetToAcc"))
                           UserRolList.Add( "AddAssetToAcc");

                        if ((model.ShowDefinitionAsset) && (roleName == "ShowDefinitionAsset"))
                           UserRolList.Add( "ShowDefinitionAsset");
                        if ((model.AddDefinitionAsset) && (roleName == "AddDefinitionAsset"))
                           UserRolList.Add( "AddDefinitionAsset");



                        if ((model.ShowSaleAsset) && (roleName == "ShowSaleAsset"))
                           UserRolList.Add( "ShowSaleAsset");
                        if ((model.AddShowSaleAsset) && (roleName == "AddShowSaleAsset"))
                           UserRolList.Add( "AddShowSaleAsset");



                        if ((model.ShowDeliveryAssetTrustee) && (roleName == "ShowDeliveryAssetTrustee"))
                           UserRolList.Add( "ShowDeliveryAssetTrustee");
                        if ((model.AddDeliveryAssetTrustee) && (roleName == "AddDeliveryAssetTrustee"))
                           UserRolList.Add( "AddDeliveryAssetTrustee");

                        if ((model.ShowConsumAsset) && (roleName == "ShowConsumAsset"))
                           UserRolList.Add( "ShowConsumAsset");
                        if ((model.AddConsumAsset) && (roleName == "AddConsumAsset"))
                           UserRolList.Add( "AddConsumAsset");


                        if ((model.ShowAssetMaintenance) && (roleName == "ShowAssetMaintenance"))
                           UserRolList.Add( "ShowAssetMaintenance");
                        if ((model.AddAssetMaintenance) && (roleName == "AddAssetMaintenance"))
                           UserRolList.Add( "AddAssetMaintenance");
                        if ((model.UpdateAssetMaintenance) && (roleName == "UpdateAssetMaintenance"))
                           UserRolList.Add( "UpdateAssetMaintenance");
                        if ((model.DeleteAssetMaintenance) && (roleName == "DeleteAssetMaintenance"))
                           UserRolList.Add( "DeleteAssetMaintenance");

                        //--End Asset--//

                        //--Report----//

                        if ((model.RepAccountStatement) && (roleName == "RepAccountStatement"))
                           UserRolList.Add( "RepAccountStatement");

                        if ((model.RepTrialBalance) && (roleName == "RepTrialBalance"))
                           UserRolList.Add( "RepTrialBalance");

                        if ((model.RepAccountingDetailsReport) && (roleName == "RepAccountingDetailsReport"))
                           UserRolList.Add( "RepAccountingDetailsReport");

                        if ((model.RepChequesReport) && (roleName == "RepChequesReport"))
                           UserRolList.Add( "RepChequesReport");

                        if ((model.RepCustSuppBalances) && (roleName == "RepCustSuppBalances"))
                           UserRolList.Add( "RepCustSuppBalances");

                        if ((model.RepTrialBalanceYearly) && (roleName == "RepTrialBalanceYearly"))
                           UserRolList.Add( "RepTrialBalanceYearly");

                        if ((model.RepTransActionDetails) && (roleName == "RepTransActionDetails"))
                           UserRolList.Add( "RepTransActionDetails");

                        if ((model.RepTrankingChequesReport) && (roleName == "RepTrankingChequesReport"))
                           UserRolList.Add( "RepTrankingChequesReport");

                        if ((model.RepSalesReport) && (roleName == "RepSalesReport"))
                           UserRolList.Add( "RepSalesReport");

                        if ((model.RepExpenseAnalysisDetailReport) && (roleName == "RepExpenseAnalysisDetailReport"))
                           UserRolList.Add( "RepExpenseAnalysisDetailReport");

                        if ((model.RepSearchForTrans) && (roleName == "RepSearchForTrans"))
                           UserRolList.Add( "RepSearchForTrans");

                        if ((model.RepPaymentChequesReport) && (roleName == "RepPaymentChequesReport"))
                           UserRolList.Add( "RepPaymentChequesReport");

                        if ((model.RepServiceReport) && (roleName == "RepServiceReport"))
                           UserRolList.Add( "RepServiceReport");

                        if ((model.RepCustomerBalancesReport) && (roleName == "RepCustomerBalancesReport"))
                           UserRolList.Add( "RepCustomerBalancesReport");

                        if ((model.RepPaymTrankingChequesReport) && (roleName == "RepPaymTrankingChequesReport"))
                           UserRolList.Add( "RepPaymTrankingChequesReport");

                        if ((model.RepServiceTaxReport) && (roleName == "RepServiceTaxReport"))
                           UserRolList.Add( "RepServiceTaxReport");

                        if ((model.RepSupplierBalancesReport) && (roleName == "RepSupplierBalancesReport"))
                           UserRolList.Add( "RepSupplierBalancesReport");

                        if ((model.RepExpenseAnalysisReport) && (roleName == "RepExpenseAnalysisReport"))
                           UserRolList.Add( "RepExpenseAnalysisReport");

                        if ((model.RepProfitlossreport) && (roleName == "RepProfitlossreport"))
                           UserRolList.Add( "RepProfitlossreport");

                        if ((model.RepTempPrepaidReport) && (roleName == "RepTempPrepaidReport"))
                           UserRolList.Add( "RepTempPrepaidReport");

                        if ((model.RepPivotReportAccounts) && (roleName == "RepPivotReportAccounts"))
                           UserRolList.Add( "RepPivotReportAccounts");

                        if ((model.RepAssetsReport) && (roleName == "RepAssetsReport"))
                           UserRolList.Add( "RepAssetsReport");

                        if ((model.RepTempRevenueReport) && (roleName == "RepTempRevenueReport"))
                           UserRolList.Add( "RepTempRevenueReport");

                        if ((model.RepAssetDepreciationReport) && (roleName == "RepAssetDepreciationReport"))
                           UserRolList.Add( "RepAssetDepreciationReport");


                        if ((model.RepEstimatedBudgetForAccount) && (roleName == "RepEstimatedBudgetForAccount"))
                           UserRolList.Add( "RepEstimatedBudgetForAccount");

                        if ((model.RepConsumptionByTypeReport) && (roleName == "RepConsumptionByTypeReport"))
                           UserRolList.Add( "RepConsumptionByTypeReport");

                        if ((model.RepEstimatedBudgetForAccountAll) && (roleName == "RepEstimatedBudgetForAccountAll"))
                           UserRolList.Add( "RepEstimatedBudgetForAccountAll");

                        if ((model.RepDefinitionAssetSiteReport) && (roleName == "RepDefinitionAssetSiteReport"))
                           UserRolList.Add( "RepDefinitionAssetSiteReport");

                        if ((model.RepEstimatedBudgetForAccountYear) && (roleName == "RepEstimatedBudgetForAccountYear"))
                           UserRolList.Add( "RepEstimatedBudgetForAccountYear");

                        if ((model.RepSoldAsset) && (roleName == "RepSoldAsset"))
                           UserRolList.Add( "RepSoldAsset");

                        if ((model.RepEstimatedBudgetForCostCenters) && (roleName == "RepEstimatedBudgetForCostCenters"))
                           UserRolList.Add( "RepEstimatedBudgetForCostCenters");

                        if ((model.RepConsumAssetReport) && (roleName == "RepConsumAssetReport"))
                           UserRolList.Add( "RepConsumAssetReport");

                        if ((model.RepEstimatedBudgetCostAllMonth) && (roleName == "RepEstimatedBudgetCostAllMonth"))
                           UserRolList.Add( "RepEstimatedBudgetCostAllMonth");

                        if ((model.RepMaintenanceAssetReport) && (roleName == "RepMaintenanceAssetReport"))
                           UserRolList.Add( "RepMaintenanceAssetReport");

                        if ((model.RepEstimatedBudgetForCostCenterYear) && (roleName == "RepEstimatedBudgetForCostCenterYear"))
                           UserRolList.Add( "DeleteAssetMaintenance");

                        if ((model.RepAssetsTransferMovements) && (roleName == "RepAssetsTransferMovements"))
                           UserRolList.Add( "RepAssetsTransferMovements");


                        ///////////////
                        if ((model.PrintRepAccountStatement) && (roleName == "PrintRepAccountStatement"))
                           UserRolList.Add( "PrintRepAccountStatement");




                        if ((model.PrintRepTrialBalance) && (roleName == "PrintRepTrialBalance"))
                           UserRolList.Add( "PrintRepTrialBalance");



                        if ((model.PrintRepAccountingDetailsPrintReport) && (roleName == "PrintRepAccountingDetailsPrintReport"))
                           UserRolList.Add( "PrintRepAccountingDetailsPrintReport");




                        if ((model.PrintRepChequesPrintReport) && (roleName == "PrintRepChequesPrintReport"))
                           UserRolList.Add( "PrintRepChequesPrintReport");

                   

                        if ((model.PrintRepCustSuppBalances) && (roleName == "PrintRepCustSuppBalances"))
                           UserRolList.Add( "PrintRepCustSuppBalances");



                        if ((model.PrintRepTrialBalanceYearly) && (roleName == "PrintRepTrialBalanceYearly"))
                           UserRolList.Add( "PrintRepTrialBalanceYearly");




                        if ((model.PrintRepTransActionDetails) && (roleName == "PrintRepTransActionDetails"))
                           UserRolList.Add( "PrintRepTransActionDetails");



                        if ((model.PrintRepTrankingChequesPrintReport) && (roleName == "PrintRepTrankingChequesPrintReport"))
                           UserRolList.Add( "PrintRepTrankingChequesPrintReport");



                        if ((model.PrintRepSalesPrintReport) && (roleName == "PrintRepSalesPrintReport"))
                           UserRolList.Add( "PrintRepSalesPrintReport");




                        if ((model.PrintRepExpenseAnalysisDetailPrintReport) && (roleName == "PrintRepExpenseAnalysisDetailPrintReport"))
                           UserRolList.Add( "PrintRepExpenseAnalysisDetailPrintReport");


                        if ((model.PrintRepSearchForTrans) && (roleName == "PrintRepSearchForTrans"))
                           UserRolList.Add( "PrintRepSearchForTrans");



                        if ((model.PrintRepPaymentChequesPrintReport) && (roleName == "PrintRepPaymentChequesPrintReport"))
                           UserRolList.Add( "PrintRepPaymentChequesPrintReport");





                        if ((model.PrintRepServicePrintReport) && (roleName == "PrintRepServicePrintReport"))
                           UserRolList.Add( "PrintRepServicePrintReport");


                       
                        if ((model.PrintRepCustomerBalancesPrintReport) && (roleName == "PrintRepCustomerBalancesPrintReport"))
                           UserRolList.Add( "PrintRepCustomerBalancesPrintReport");



                        if ((model.PrintRepPaymTrankingChequesPrintReport) && (roleName == "PrintRepPaymTrankingChequesPrintReport"))
                           UserRolList.Add( "PrintRepPaymTrankingChequesPrintReport");





                        if ((model.PrintRepServiceTaxPrintReport) && (roleName == "PrintRepServiceTaxPrintReport"))
                           UserRolList.Add( "PrintRepServiceTaxPrintReport");





                        if ((model.PrintRepSupplierBalancesPrintReport) && (roleName == "PrintRepSupplierBalancesPrintReport"))
                           UserRolList.Add( "PrintRepSupplierBalancesPrintReport");



                        if ((model.PrintRepExpenseAnalysisPrintReport) && (roleName == "PrintRepExpenseAnalysisPrintReport"))
                           UserRolList.Add( "PrintRepExpenseAnalysisPrintReport");



                        if ((model.PrintRepProfitlossPrintReport) && (roleName == "PrintRepProfitlossPrintReport"))
                           UserRolList.Add( "PrintRepProfitlossPrintReport");


                       

                        if ((model.PrintRepTempPPrintRepaidPrintReport) && (roleName == "PrintRepTempPPrintRepaidPrintReport"))
                           UserRolList.Add( "PrintRepTempPPrintRepaidPrintReport");

                        


                        if ((model.PrintRepPivotPrintReportAccounts) && (roleName == "PrintRepPivotPrintReportAccounts"))
                           UserRolList.Add( "PrintRepPivotPrintReportAccounts");





                        if ((model.PrintRepAssetsPrintReport) && (roleName == "PrintRepAssetsPrintReport"))
                           UserRolList.Add( "PrintRepAssetsPrintReport");





                        if ((model.PrintRepTempRevenuePrintReport) && (roleName == "PrintRepTempRevenuePrintReport"))
                           UserRolList.Add( "PrintRepTempRevenuePrintReport");


                        

                        if ((model.PrintRepAssetDepreciationPrintReport) && (roleName == "PrintRepAssetDepreciationPrintReport"))
                           UserRolList.Add( "PrintRepAssetDepreciationPrintReport");

                       

                        if ((model.PrintRepEstimatedBudgetForAccount) && (roleName == "PrintRepEstimatedBudgetForAccount"))
                           UserRolList.Add( "PrintRepEstimatedBudgetForAccount");



                        if ((model.PrintRepConsumptionByTypePrintReport) && (roleName == "PrintRepConsumptionByTypePrintReport"))
                           UserRolList.Add( "PrintRepConsumptionByTypePrintReport");



                        if ((model.PrintRepEstimatedBudgetForAccountAll) && (roleName == "PrintRepEstimatedBudgetForAccountAll"))
                           UserRolList.Add( "PrintRepEstimatedBudgetForAccountAll");


                        if ((model.PrintRepDefinitionAssetSitePrintReport) && (roleName == "PrintRepDefinitionAssetSitePrintReport"))
                           UserRolList.Add( "PrintRepDefinitionAssetSitePrintReport");




                        if ((model.PrintRepEstimatedBudgetForAccountYear) && (roleName == "PrintRepEstimatedBudgetForAccountYear"))
                           UserRolList.Add( "PrintRepEstimatedBudgetForAccountYear");




                        if ((model.PrintRepSoldAsset) && (roleName == "PrintRepSoldAsset"))
                           UserRolList.Add( "PrintRepSoldAsset");




                        if ((model.PrintRepEstimatedBudgetForCostCenters) && (roleName == "PrintRepEstimatedBudgetForCostCenters"))
                           UserRolList.Add( "PrintRepEstimatedBudgetForCostCenters");





                        if ((model.PrintRepConsumAssetPrintReport) && (roleName == "PrintRepConsumAssetPrintReport"))
                           UserRolList.Add( "PrintRepConsumAssetPrintReport");




                        if ((model.PrintRepEstimatedBudgetCostAllMonth) && (roleName == "PrintRepEstimatedBudgetCostAllMonth"))
                           UserRolList.Add( "PrintRepEstimatedBudgetCostAllMonth");


                        if ((model.PrintRepMaintenanceAssetPrintReport) && (roleName == "PrintRepMaintenanceAssetPrintReport"))
                           UserRolList.Add( "PrintRepMaintenanceAssetPrintReport");




                        if ((model.PrintRepEstimatedBudgetForCostCenterYear) && (roleName == "PrintRepEstimatedBudgetForCostCenterYear"))
                           UserRolList.Add( "PrintRepEstimatedBudgetForCostCenterYear");

                        if ((model.PrintRepAssetsTransferMovements) && (roleName == "PrintRepAssetsTransferMovements"))
                           UserRolList.Add( "PrintRepAssetsTransferMovements");

                        /////////////

                        //ابدعتي

                        //--End Report---//


                        ///----TransAction---//

                        if ((model.ShowTransActionTrans) && (roleName == "ShowTransActionTrans"))
                           UserRolList.Add( "ShowTransActionTrans");
                        if ((model.AddTransActionTrans) && (roleName == "AddTransActionTrans"))
                           UserRolList.Add( "AddTransActionTrans");
                        if ((model.DeleteTransActionTrans) && (roleName == "DeleteTransActionTrans"))
                           UserRolList.Add( "DeleteTransActionTrans");
                        if ((model.UpdateTransActionTrans) && (roleName == "UpdateTransActionTrans"))
                           UserRolList.Add( "UpdateTransActionTrans");
                        if ((model.CopyTransActionTrans) && (roleName == "CopyTransActionTrans"))
                           UserRolList.Add( "CopyTransActionTrans");
                        if ((model.PrintTransActionTrans) && (roleName == "PrintTransActionTrans"))
                           UserRolList.Add( "PrintTransActionTrans");

                        if ((model.ExportTransActionTrans) && (roleName == "ExportTransActionTrans"))
                           UserRolList.Add( "ExportTransActionTrans");

                        if ((model.UnExportTransActionTrans) && (roleName == "UnExportTransActionTrans"))
                           UserRolList.Add( "UnExportTransActionTrans");


                        if ((model.AttachTransActionTrans) && (roleName == "AttachTransActionTrans"))
                           UserRolList.Add( "AttachTransActionTrans");



                        if ((model.ShowReceiptVoucherBank) && (roleName == "ShowReceiptVoucherBank"))
                           UserRolList.Add( "ShowReceiptVoucherBank");


                        if ((model.AddReceiptVoucherBank) && (roleName == "AddReceiptVoucherBank"))
                           UserRolList.Add( "AddReceiptVoucherBank");


                        if ((model.DeleteReceiptVoucherBank) && (roleName == "DeleteReceiptVoucherBank"))
                           UserRolList.Add( "DeleteReceiptVoucherBank");


                        if ((model.UpdateReceiptVoucherBank) && (roleName == "UpdateReceiptVoucherBank"))
                           UserRolList.Add( "UpdateReceiptVoucherBank");


                        if ((model.CopyReceiptVoucherBank) && (roleName == "CopyReceiptVoucherBank"))
                           UserRolList.Add( "CopyReceiptVoucherBank");

                        if ((model.PrintReceiptVoucherBank) && (roleName == "PrintReceiptVoucherBank"))
                           UserRolList.Add( "PrintReceiptVoucherBank");

                        if ((model.ExportReceiptVoucherBank) && (roleName == "ExportReceiptVoucherBank"))
                           UserRolList.Add( "ExportReceiptVoucherBank");


                        if ((model.UnExportReceiptVoucherBank) && (roleName == "UnExportReceiptVoucherBank"))
                           UserRolList.Add( "UnExportReceiptVoucherBank");


                        if ((model.AttachReceiptVoucherBank) && (roleName == "AttachReceiptVoucherBank"))
                           UserRolList.Add( "AttachReceiptVoucherBank");



                        if ((model.ShowTempPrepaid) && (roleName == "ShowTempPrepaid"))
                           UserRolList.Add( "ShowTempPrepaid");


                        if ((model.AddTempPrepaid) && (roleName == "AddTempPrepaid"))
                           UserRolList.Add( "AddTempPrepaid");


                        if ((model.DeleteTempPrepaid) && (roleName == "DeleteTempPrepaid"))
                           UserRolList.Add( "DeleteTempPrepaid");


                        if ((model.UpdateTempPrepaid) && (roleName == "UpdateTempPrepaid"))
                           UserRolList.Add( "UpdateTempPrepaid");


                        if ((model.CopyTempPrepaid) && (roleName == "CopyTempPrepaid"))
                           UserRolList.Add( "CopyTempPrepaid");

                        if ((model.PrintTempPrepaid) && (roleName == "PrintTempPrepaid"))
                           UserRolList.Add( "PrintTempPrepaid");

                         if ((model.AccumulativeTempPrepaid) && (roleName == "AccumulativeTempPrepaid"))
                           UserRolList.Add( "AccumulativeTempPrepaid");

                         
                         if ((model.AttachTempPrepaid) && (roleName == "AttachTempPrepaid"))
                           UserRolList.Add( "AttachTempPrepaid");










                        if ((model.ShowDebitNote) && (roleName == "ShowDebitNote"))
                           UserRolList.Add( "ShowDebitNote");


                        if ((model.AddDebitNote) && (roleName == "AddDebitNote"))
                           UserRolList.Add( "AddDebitNote");


                        if ((model.DeleteDebitNote) && (roleName == "DeleteDebitNote"))
                           UserRolList.Add( "DeleteDebitNote");


                        if ((model.UpdateDebitNote) && (roleName == "UpdateDebitNote"))
                           UserRolList.Add( "UpdateDebitNote");


                        if ((model.CopyDebitNote) && (roleName == "CopyDebitNote"))
                           UserRolList.Add( "CopyDebitNote");

                        if ((model.PrintDebitNote) && (roleName == "PrintDebitNote"))
                           UserRolList.Add( "PrintDebitNote");

                        if ((model.ExportDebitNote) && (roleName == "ExportDebitNote"))
                           UserRolList.Add( "ExportDebitNote");

                        if ((model.UnExportDebitNote) && (roleName == "UnExportDebitNote"))
                           UserRolList.Add( "UnExportDebitNote");

                        if ((model.AttachDebitNote) && (roleName == "AttachDebitNote"))
                           UserRolList.Add( "AttachDebitNote");









                        if ((model.ShowServiceBill) && (roleName == "ShowServiceBill"))
                           UserRolList.Add( "ShowServiceBill");


                        if ((model.AddServiceBill) && (roleName == "AddServiceBill"))
                           UserRolList.Add( "AddServiceBill");


                        if ((model.DeleteServiceBill) && (roleName == "DeleteServiceBill"))
                           UserRolList.Add( "DeleteServiceBill");


                        if ((model.UpdateServiceBill) && (roleName == "UpdateServiceBill"))
                           UserRolList.Add( "UpdateServiceBill");


                        if ((model.CopyServiceBill) && (roleName == "CopyServiceBill"))
                           UserRolList.Add( "CopyServiceBill");

                        if ((model.PrintServiceBill) && (roleName == "PrintServiceBill"))
                           UserRolList.Add( "PrintServiceBill");


                        if ((model.AttachServiceBill) && (roleName == "AttachServiceBill"))
                           UserRolList.Add( "AttachServiceBill");

                        if ((model.ExportServiceBill) && (roleName == "ExportServiceBill"))
                           UserRolList.Add( "ExportServiceBill");

                        if ((model.UnExportServiceBill) && (roleName == "UnExportServiceBill"))
                           UserRolList.Add( "UnExportServiceBill");


                       

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
                           UserRolList.Add( "ShowOpeningBalance");


                        if ((model.AddOpeningBalance) && (roleName == "AddOpeningBalance"))
                           UserRolList.Add( "AddOpeningBalance");


                        if ((model.DeleteOpeningBalance) && (roleName == "DeleteOpeningBalance"))
                           UserRolList.Add( "DeleteOpeningBalance");


                        if ((model.UpdateOpeningBalance) && (roleName == "UpdateOpeningBalance"))
                           UserRolList.Add( "UpdateOpeningBalance");


                        if ((model.CopyOpeningBalance) && (roleName == "CopyOpeningBalance"))
                           UserRolList.Add( "CopyOpeningBalance");

                        if ((model.PrintOpeningBalance) && (roleName == "PrintOpeningBalance"))
                           UserRolList.Add( "PrintOpeningBalance");




                        if ((model.ShowReceiptVoucherCash) && (roleName == "ShowReceiptVoucherCash"))
                           UserRolList.Add( "ShowReceiptVoucherCash");


                        if ((model.AddReceiptVoucherCash) && (roleName == "AddReceiptVoucherCash"))
                           UserRolList.Add( "AddReceiptVoucherCash");


                        if ((model.DeleteReceiptVoucherCash) && (roleName == "DeleteReceiptVoucherCash"))
                           UserRolList.Add( "DeleteReceiptVoucherCash");


                        if ((model.UpdateReceiptVoucherCash) && (roleName == "UpdateReceiptVoucherCash"))
                           UserRolList.Add( "UpdateReceiptVoucherCash");


                        if ((model.CopyReceiptVoucherCash) && (roleName == "CopyReceiptVoucherCash"))
                           UserRolList.Add( "CopyReceiptVoucherCash");

                        if ((model.PrintReceiptVoucherCash) && (roleName == "PrintReceiptVoucherCash"))
                           UserRolList.Add( "PrintReceiptVoucherCash");

                        if ((model.ExportReceiptVoucherCash) && (roleName == "ExportReceiptVoucherCash"))
                           UserRolList.Add( "ExportReceiptVoucherCash");


                        if ((model.UnExportReceiptVoucherCash) && (roleName == "UnExportReceiptVoucherCash"))
                           UserRolList.Add( "UnExportReceiptVoucherCash");




                        if ((model.AttachReceiptVoucherCash) && (roleName == "AttachReceiptVoucherCash"))
                           UserRolList.Add( "AttachReceiptVoucherCash");

                        if ((model.ShowTempRevenueReceived) && (roleName == "ShowTempRevenueReceived"))
                           UserRolList.Add( "ShowTempRevenueReceived");


                        if ((model.AddTempRevenueReceived) && (roleName == "AddTempRevenueReceived"))
                           UserRolList.Add( "AddTempRevenueReceived");


                        if ((model.DeleteTempRevenueReceived) && (roleName == "DeleteTempRevenueReceived"))
                           UserRolList.Add( "DeleteTempRevenueReceived");


                        if ((model.UpdateTempRevenueReceived) && (roleName == "UpdateTempRevenueReceived"))
                           UserRolList.Add( "UpdateTempRevenueReceived");


                        if ((model.CopyTempRevenueReceived) && (roleName == "CopyTempRevenueReceived"))
                           UserRolList.Add( "CopyTempRevenueReceived");

                        if ((model.PrintTempRevenueReceived) && (roleName == "PrintTempRevenueReceived"))
                           UserRolList.Add( "PrintTempRevenueReceived");

                        if ((model.AccumulativeTempRevenueReceived) && (roleName == "AccumulativeTempRevenueReceived"))
                           UserRolList.Add( "AccumulativeTempRevenueReceived");

                             if ((model.AttachTempRevenueReceived) && (roleName == "AttachTempRevenueReceived"))
                           UserRolList.Add( "AttachTempRevenueReceived");



                        if ((model.ShowCreditNote) && (roleName == "ShowCreditNote"))
                           UserRolList.Add( "ShowCreditNote");


                        if ((model.AddCreditNote) && (roleName == "AddCreditNote"))
                           UserRolList.Add( "AddCreditNote");


                        if ((model.DeleteCreditNote) && (roleName == "DeleteCreditNote"))
                           UserRolList.Add( "DeleteCreditNote");


                        if ((model.UpdateCreditNote) && (roleName == "UpdateCreditNote"))
                           UserRolList.Add( "UpdateCreditNote");


                        if ((model.CopyCreditNote) && (roleName == "CopyCreditNote"))
                           UserRolList.Add( "CopyCreditNote");

                        if ((model.PrintCreditNote) && (roleName == "PrintCreditNote"))
                           UserRolList.Add( "PrintCreditNote");

                        if ((model.ExportCreditNote) && (roleName == "ExportCreditNote"))
                           UserRolList.Add( "ExportCreditNote");

                        if ((model.UnExportCreditNote) && (roleName == "UnExportCreditNote"))
                           UserRolList.Add( "UnExportCreditNote");


                        if ((model.AttachCreditNote) && (roleName == "AttachCreditNote"))
                           UserRolList.Add( "AttachCreditNote");

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
                           UserRolList.Add( "ShowPaymentVoucherBank");


                        if ((model.AddPaymentVoucherBank) && (roleName == "AddPaymentVoucherBank"))
                           UserRolList.Add( "AddPaymentVoucherBank");


                        if ((model.DeletePaymentVoucherBank) && (roleName == "DeletePaymentVoucherBank"))
                           UserRolList.Add( "DeletePaymentVoucherBank");


                        if ((model.UpdatePaymentVoucherBank) && (roleName == "UpdatePaymentVoucherBank"))
                           UserRolList.Add( "UpdatePaymentVoucherBank");


                        if ((model.CopyPaymentVoucherBank) && (roleName == "CopyPaymentVoucherBank"))
                           UserRolList.Add( "CopyPaymentVoucherBank");

                        if ((model.PrintPaymentVoucherBank) && (roleName == "PrintPaymentVoucherBank"))
                           UserRolList.Add( "PrintPaymentVoucherBank");

                        if ((model.ExportPaymentVoucherBank) && (roleName == "ExportPaymentVoucherBank"))
                           UserRolList.Add( "ExportPaymentVoucherBank");

                        if ((model.UnExportPaymentVoucherBank) && (roleName == "UnExportPaymentVoucherBank"))
                           UserRolList.Add( "UnExportPaymentVoucherBank");


                        if ((model.AttachPaymentVoucherBank) && (roleName == "AttachPaymentVoucherBank"))
                           UserRolList.Add( "AttachPaymentVoucherBank");


                        if ((model.ShowPaymentVoucherCash) && (roleName == "ShowPaymentVoucherCash"))
                           UserRolList.Add( "ShowPaymentVoucherCash");


                        if ((model.AddPaymentVoucherCash) && (roleName == "AddPaymentVoucherCash"))
                           UserRolList.Add( "AddPaymentVoucherCash");


                        if ((model.DeletePaymentVoucherCash) && (roleName == "DeletePaymentVoucherCash"))
                           UserRolList.Add( "DeletePaymentVoucherCash");


                        if ((model.UpdatePaymentVoucherCash) && (roleName == "UpdatePaymentVoucherCash"))
                           UserRolList.Add( "UpdatePaymentVoucherCash");


                        if ((model.CopyPaymentVoucherCash) && (roleName == "CopyPaymentVoucherCash"))
                           UserRolList.Add( "CopyPaymentVoucherCash");

                        if ((model.PrintPaymentVoucherCash) && (roleName == "PrintPaymentVoucherCash"))
                           UserRolList.Add( "PrintPaymentVoucherCash");


                        if ((model.ExportPaymentVoucherCash) && (roleName == "ExportPaymentVoucherCash"))
                           UserRolList.Add( "ExportPaymentVoucherCash");




                        if ((model.UnExportPaymentVoucherCash) && (roleName == "UnExportPaymentVoucherCash"))
                           UserRolList.Add( "UnExportPaymentVoucherCash");


                        if ((model.AttachPaymentVoucherCash) && (roleName == "AttachPaymentVoucherCash"))
                           UserRolList.Add( "AttachPaymentVoucherCash");



                        if ((model.ShowDepositInTheBank) && (roleName == "ShowDepositInTheBank"))
                           UserRolList.Add( "ShowDepositInTheBank");


                        if ((model.AddDepositInTheBank) && (roleName == "AddDepositInTheBank"))
                           UserRolList.Add( "AddDepositInTheBank");


                        if ((model.DeleteDepositInTheBank) && (roleName == "DeleteDepositInTheBank"))
                           UserRolList.Add( "DeleteDepositInTheBank");


                        if ((model.UpdateDepositInTheBank) && (roleName == "UpdateDepositInTheBank"))
                           UserRolList.Add( "UpdateDepositInTheBank");


                        if ((model.CopyDepositInTheBank) && (roleName == "CopyDepositInTheBank"))
                           UserRolList.Add( "CopyDepositInTheBank");

                        if ((model.PrintDepositInTheBank) && (roleName == "PrintDepositInTheBank"))
                           UserRolList.Add( "PrintDepositInTheBank");


                        if ((model.ExportDepositInTheBank) && (roleName == "ExportDepositInTheBank"))
                           UserRolList.Add( "ExportDepositInTheBank");




                        if ((model.UnExportDepositInTheBank) && (roleName == "UnExportDepositInTheBank"))
                           UserRolList.Add( "UnExportDepositInTheBank");


                        if ((model.AttachDepositInTheBank) && (roleName == "AttachDepositInTheBank"))
                           UserRolList.Add( "AttachDepositInTheBank");


                        //------------------
                        if ((model.ShowReturnACheque) && (roleName == "ShowReturnACheque"))
                           UserRolList.Add( "ShowReturnACheque");


                        if ((model.ShowTransferFromFundUC) && (roleName == "ShowTransferFromFundUC"))
                           UserRolList.Add( "ShowTransferFromFundUC");


                        if ((model.ShowDrawingUC) && (roleName == "ShowDrawingUC"))
                           UserRolList.Add( "ShowDrawingUC");


                        if ((model.ShowPaymentchequeUC) && (roleName == "ShowPaymentchequeUC"))
                           UserRolList.Add( "ShowPaymentchequeUC");


                        if ((model.ShowTransferFromFundCD) && (roleName == "ShowTransferFromFundCD"))
                           UserRolList.Add( "ShowTransferFromFundCD");

                        if ((model.ShowReturnAChequeCD) && (roleName == "ShowReturnAChequeCD"))
                           UserRolList.Add( "ShowReturnAChequeCD");


                        if ((model.ShowTransferFromFundToChequeE) && (roleName == "ShowTransferFromFundToChequeE"))
                           UserRolList.Add( "ShowTransferFromFundToChequeE");




                        if ((model.ShowPaymentChequeEndorsement) && (roleName == "ShowPaymentChequeEndorsement"))
                           UserRolList.Add( "ShowPaymentChequeEndorsement");



                        if ((model.ShowReturnChequeUnderCollection) && (roleName == "ShowReturnChequeUnderCollection"))
                           UserRolList.Add( "ShowReturnChequeUnderCollection");


                        if ((model.ShowReturnChequeEndorsement) && (roleName == "ShowReturnChequeEndorsement"))
                           UserRolList.Add( "ShowReturnChequeEndorsement");

                        if ((model.ShowFundChequesDrawnFromUC) && (roleName == "ShowFundChequesDrawnFromUC"))
                           UserRolList.Add( "ShowFundChequesDrawnFromUC");


                        if ((model.ShowReturnedChequeFund) && (roleName == "ShowReturnedChequeFund"))
                           UserRolList.Add( "ShowReturnedChequeFund");


                        if ((model.ShowCourtFund) && (roleName == "ShowCourtFund"))
                           UserRolList.Add( "ShowCourtFund");


                        if ((model.ShowPostdatedCheques) && (roleName == "ShowPostdatedCheques"))
                           UserRolList.Add( "ShowPostdatedCheques");


                        if ((model.ShowReturnPostdatedCheques) && (roleName == "ShowReturnPostdatedCheques"))
                           UserRolList.Add( "ShowReturnPostdatedCheques");

                        if ((model.ShowReturnPaidPostdatedCheques) && (roleName == "ShowReturnPaidPostdatedCheques"))
                           UserRolList.Add( "ShowReturnPaidPostdatedCheques");



                        if ((model.ShowReceiptVoucherCashMultiAccount) && (roleName == "ShowReceiptVoucherCashMultiAccount"))
                           UserRolList.Add( "ShowReceiptVoucherCashMultiAccount");
                        if ((model.AddReceiptVoucherCashMultiAccount) && (roleName == "AddReceiptVoucherCashMultiAccount"))
                           UserRolList.Add( "AddReceiptVoucherCashMultiAccount");
                        if ((model.DeleteReceiptVoucherCashMultiAccount) && (roleName == "DeleteReceiptVoucherCashMultiAccount"))
                           UserRolList.Add( "DeleteReceiptVoucherCashMultiAccount");
                        if ((model.UpdateReceiptVoucherCashMultiAccount) && (roleName == "UpdateReceiptVoucherCashMultiAccount"))
                           UserRolList.Add( "UpdateReceiptVoucherCashMultiAccount");
                        if ((model.CopyReceiptVoucherCashMultiAccount) && (roleName == "CopyReceiptVoucherCashMultiAccount"))
                           UserRolList.Add( "CopyReceiptVoucherCashMultiAccount");
                        if ((model.PrintReceiptVoucherCashMultiAccount) && (roleName == "PrintReceiptVoucherCashMultiAccount"))
                           UserRolList.Add( "PrintReceiptVoucherCashMultiAccount");

                        if ((model.ExportReceiptVoucherCashMultiAccount) && (roleName == "ExportReceiptVoucherCashMultiAccount"))
                           UserRolList.Add( "ExportReceiptVoucherCashMultiAccount");

                        if ((model.UnExportReceiptVoucherCashMultiAccount) && (roleName == "UnExportReceiptVoucherCashMultiAccount"))
                           UserRolList.Add( "UnExportReceiptVoucherCashMultiAccount");


                        if ((model.AttachReceiptVoucherCashMultiAccount) && (roleName == "AttachReceiptVoucherCashMultiAccount"))
                           UserRolList.Add( "AttachReceiptVoucherCashMultiAccount");


                        if ((model.ShowPaymentVoucherCashMultiAccount) && (roleName == "ShowPaymentVoucherCashMultiAccount"))
                           UserRolList.Add( "ShowPaymentVoucherCashMultiAccount");
                        if ((model.AddPaymentVoucherCashMultiAccount) && (roleName == "AddPaymentVoucherCashMultiAccount"))
                           UserRolList.Add( "AddPaymentVoucherCashMultiAccount");
                        if ((model.DeletePaymentVoucherCashMultiAccount) && (roleName == "DeletePaymentVoucherCashMultiAccount"))
                           UserRolList.Add( "DeletePaymentVoucherCashMultiAccount");
                        if ((model.UpdatePaymentVoucherCashMultiAccount) && (roleName == "UpdatePaymentVoucherCashMultiAccount"))
                           UserRolList.Add( "UpdatePaymentVoucherCashMultiAccount");
                        if ((model.CopyPaymentVoucherCashMultiAccount) && (roleName == "CopyPaymentVoucherCashMultiAccount"))
                           UserRolList.Add( "CopyPaymentVoucherCashMultiAccount");
                        if ((model.PrintPaymentVoucherCashMultiAccount) && (roleName == "PrintPaymentVoucherCashMultiAccount"))
                           UserRolList.Add( "PrintPaymentVoucherCashMultiAccount");

                        if ((model.ExportPaymentVoucherCashMultiAccount) && (roleName == "ExportPaymentVoucherCashMultiAccount"))
                           UserRolList.Add( "ExportPaymentVoucherCashMultiAccount");

                        if ((model.UnExportPaymentVoucherCashMultiAccount) && (roleName == "UnExportPaymentVoucherCashMultiAccount"))
                           UserRolList.Add( "UnExportPaymentVoucherCashMultiAccount");


                        if ((model.AttachPaymentVoucherCashMultiAccount) && (roleName == "AttachPaymentVoucherCashMultiAccount"))
                           UserRolList.Add( "AttachPaymentVoucherCashMultiAccount");







                        //--EndTransAction--//







                        //if ((model.UpdateAccountName) && (roleName == "UpdateAccountName"))
                        //   UserRolList.Add( "UpdateAccountName");

                        //if ((model.UpdateAccountKid) && (roleName == "UpdateAccountKid"))
                        //   UserRolList.Add( "UpdateAccountKid");

                        //if ((model.UpdateAcountType) && (roleName == "UpdateAcountType"))
                        //   UserRolList.Add( "UpdateAcountType");

                        //if ((model.StopAccount) && (roleName == "StopAccount"))
                        //   UserRolList.Add( "StopAccount");


                    }
                    _unitOfWork.Complete();

                    //var duplicateKeys = UserRolList.GroupBy(x => x)
                    //    .Where(group => group.Count() > 1)
                    //    .Select(group => group.Key);

                    List<string> disUserRolList = UserRolList.Distinct().ToList();
                    await UserManager.AddToRolesAsync(user.Id, disUserRolList.ToArray());

                    Msg.Msg = Resources.Resource.AddedSuccessfully;
                    Msg.Code = 1;
                    // return RedirectToAction("Index", "Users");
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }

                AddErrors(result);
                Msg.Msg = Resources.Resource.SomthingWentWrong + " " + result.Errors.FirstOrDefault().ToString();
                Msg.Code = 0;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }

            // If we got this far, something failed, redisplay form
            //   return View(model);
            catch (Exception ex)
            {
                string Err = " ";
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (ModelError error in errors)
                {
                    Err = Err + error.ErrorMessage + "  ";
                }
                Err = Err + " " + ex.Message;

                Msg.Msg = Resources.Resource.SomthingWentWrong + " " + Err;
                Msg.Code = 0;
                return Json(Msg, JsonRequestBehavior.AllowGet);

            }

        }
        [HttpPost]
        [AllowAnonymous]
        // [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterJson(RegisterViewModel model)
        {
            MsgUnit Msg = new MsgUnit();
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    fCompanyId = 0,

                    UserType = 1,
                    AccountStatus = 1,
                    RealPass = model.Password
                };



                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, "CoOwner");
                  
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);


                    Msg.Msg = Resources.Resource.YourrequesthasBeenSuccessfullySent;
                    Msg.Code = 1;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }


                string Err = " ";
                //var errors = result.SelectMany();
                foreach (string error in result.Errors)
                {
                    Err = Err + error + " * ";
                }

                Msg.Msg = Resources.Resource.SomthingWentWrong + " * " + Err;
                Msg.Code = 0;
                return Json(Msg, JsonRequestBehavior.AllowGet);


            }
            else
            {
                string Err = " ";
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (ModelError error in errors)
                {
                    Err = Err + error.ErrorMessage + "  ";
                }

                Msg.Msg = Resources.Resource.SomthingWentWrong + " " + Err;
                Msg.Code = 0;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }




            // If we got this far, something failed, redisplay form
            // return View(model);
        }


        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            LoginViewModel UserInfo = new LoginViewModel();
            try
            {
            
                if (Request.Cookies["SeniorsAccLogin"] != null)
                {
                    string username = Request.Cookies["SeniorsAccLogin"].Values["email"];
                    var userdata = _unitOfWork.User.GetUserEmailID(username);
                    if (userdata != null)
                    {
                        UserInfo.Email = userdata.Email;
                        UserInfo.Password = userdata.RealPass;
                    }


                }

                var AccYear = _unitOfWork.NativeSql.GetAllYear();
                UserInfo.AccYear = AccYear;

            }
            catch(Exception ex)
            {
                string errpr = ex.Message;
            }


            ViewBag.ReturnUrl = returnUrl;
            return View(UserInfo);
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
      //  [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            


            if (!ModelState.IsValid)
            {
                var AccYear = _unitOfWork.NativeSql.GetAllYear();
                model.AccYear = AccYear;
                return View(model);
            }

            var User = _unitOfWork.User.GetUserEmailID(model.Email);
            if (User != null)
            {
                if (User.AccountStatus == 1)
                {
                    ModelState.AddModelError("", "Account Deactivated");
                    var AccYear = _unitOfWork.NativeSql.GetAllYear();
                    model.AccYear = AccYear;
                    return View(model);
                }

            }
            if (model.Year == 0)
            {
                ModelState.AddModelError("", "Please Select Year");
                var AccYear = _unitOfWork.NativeSql.GetAllYear();
                model.AccYear = AccYear;
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
        
            switch (result)
            {
                case SignInStatus.Success:

                   
                    if (model.RememberMe)
                    {
                        HttpCookie cookie = Request.Cookies["SeniorsAccLogin"];
                        if (cookie == null)
                        {
                              cookie = new HttpCookie("SeniorsAccLogin");
                        }
                            
                        cookie.Values.Add("email", model.Email);
                        cookie.Expires = DateTime.Now.AddDays(30);
                        cookie.SameSite = (SameSiteMode.Lax);
                        Response.Cookies.Add(cookie);
                    }
                    _unitOfWork.User.ChangeCurrActiveYear(model.Email, model.Year);
                    _unitOfWork.Complete();


                    return RedirectToLocal(returnUrl);

                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    var AccYear = _unitOfWork.NativeSql.GetAllYear();
                    model.AccYear = AccYear;
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}