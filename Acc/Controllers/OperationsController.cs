using Acc.Helpers;
using Acc.Persistence;
using Acc.Repositories;
using Acc.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Acc.Controllers
{
    [Authorize]
    public class OperationsController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public OperationsController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        // GET: Operations
        
        public ActionResult OperationDash()
        {
            return View();
        }
        [Authorize(Roles = "CoOwner,ShowTransferbetweenaccounts")]
        public ActionResult Transferbetweenaccounts()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "CoOwner,TransferTransferbetweenaccounts")]
        public JsonResult TransferAccounts(TransferAccountVM ObjSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                int CurrYear = UserInfo.CurrYear;
                if (!ModelState.IsValid)
                {
                    string Err = " ";
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    foreach (ModelError error in errors)
                    {
                        Err = Err + error.ErrorMessage + " * ";
                    }

                    Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + Err;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);

                }

                var FromAccountInfo = _unitOfWork.ChartOfAccount.GetAccountByID(UserInfo.fCompanyId, ObjSave.FromAccountNumber);
                var ToAccountInfo = _unitOfWork.ChartOfAccount.GetAccountByID(UserInfo.fCompanyId, ObjSave.ToAccountNumber);
                bool IshaveOpen = false;
                if(FromAccountInfo!=null && ToAccountInfo != null)
                {

                    if (FromAccountInfo.OpeningBalanceDebit !=0 || FromAccountInfo.OpeningBalanceCredit != 0)
                    {
                        if(ToAccountInfo.OpeningBalanceDebit != 0 || ToAccountInfo.OpeningBalanceCredit != 0)
                        {
                            IshaveOpen = true;
                        }

                        ToAccountInfo.OpeningBalanceCredit = FromAccountInfo.OpeningBalanceCredit + ToAccountInfo.OpeningBalanceCredit;
                        ToAccountInfo.OpeningBalanceDebit = FromAccountInfo.OpeningBalanceDebit + ToAccountInfo.OpeningBalanceDebit;
                        _unitOfWork.NativeSql.UpdateOpeinigBalance(UserInfo.fCompanyId, ObjSave.ToAccountNumber, ToAccountInfo.OpeningBalanceDebit, ToAccountInfo.OpeningBalanceCredit, CurrYear);


                        FromAccountInfo.OpeningBalanceDebit = 0;
                        FromAccountInfo.OpeningBalanceCredit = 0;
                        _unitOfWork.NativeSql.UpdateOpeinigBalance(UserInfo.fCompanyId, ObjSave.FromAccountNumber, FromAccountInfo.OpeningBalanceDebit, FromAccountInfo.OpeningBalanceCredit, CurrYear);

                        if(IshaveOpen)
                        {
                            _unitOfWork.NativeSql.DeleteOpeinigBalance(UserInfo.fCompanyId, ObjSave.FromAccountNumber, CurrYear);
                            _unitOfWork.NativeSql.UpdateTransActionOpeinigBalance(UserInfo.fCompanyId, ObjSave.ToAccountNumber, ToAccountInfo.OpeningBalanceDebit, ToAccountInfo.OpeningBalanceCredit, CurrYear);



                        }
                        else
                        {
                            _unitOfWork.NativeSql.TransferTRansActionOpeinig(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear);


                        }


                    }

                    _unitOfWork.NativeSql.TransferTRansAction(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear);
                    
                    _unitOfWork.NativeSql.TransferTRansActionServiceBill(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear);

                    _unitOfWork.NativeSql.TransferAssetMaintenance(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber);
                    _unitOfWork.NativeSql.TransferAccreditationInformation(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber);

                    _unitOfWork.NativeSql.TransferSt_CarpenterH(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber);
                    _unitOfWork.NativeSql.TransferSt_DelegateReceivingH(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber);

  
                    _unitOfWork.NativeSql.TransferO_TransactionH(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear);
                    _unitOfWork.NativeSql.TransferO_TransactionHistoryH(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear);


                    _unitOfWork.NativeSql.TransferEstimatedBudget(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear, 1);
                    _unitOfWork.NativeSql.TransferEstimatedBudget(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear, 2);

                    _unitOfWork.NativeSql.TransferPapers(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear, 2);
                    _unitOfWork.NativeSql.TransferPapers(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear, 3);
                    _unitOfWork.NativeSql.TransferPapers(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear, 4);
                    _unitOfWork.NativeSql.TransferPapers(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear, 5);
                    _unitOfWork.NativeSql.TransferPapers(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear, 6);

                    _unitOfWork.NativeSql.TransferCompany(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear, 1);
                    _unitOfWork.NativeSql.TransferCompany(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear, 2);
                    _unitOfWork.NativeSql.TransferCompany(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear, 3);
                    _unitOfWork.NativeSql.TransferCompany(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear, 4);

                    _unitOfWork.NativeSql.TransferTracingPapers(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear, 1);
                    _unitOfWork.NativeSql.TransferTracingPapers(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear, 2);
                    _unitOfWork.NativeSql.TransferTracingPapers(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear, 3);
                    _unitOfWork.NativeSql.TransferTracingPapers(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear, 4);
                    _unitOfWork.NativeSql.TransferTracingPapers(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear, 5);
                    _unitOfWork.NativeSql.TransferTracingPapers(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear, 6);

                    _unitOfWork.NativeSql.TransferCostCenters(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear, 1);
                    _unitOfWork.NativeSql.TransferCostCenters(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear, 2);
                    _unitOfWork.NativeSql.TransferCostCenters(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear, 3);
                    _unitOfWork.NativeSql.TransferCostCenters(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear, 4);
                    _unitOfWork.NativeSql.TransferCostCenters(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear, 5);

                    _unitOfWork.NativeSql.TransferO_PaperH(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear, 1);
                    _unitOfWork.NativeSql.TransferO_PaperH(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear, 2);
                    _unitOfWork.NativeSql.TransferO_PaperH(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear, 3);
                    _unitOfWork.NativeSql.TransferO_PaperH(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear, 4);
                    _unitOfWork.NativeSql.TransferO_PaperH(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear, 5);
                    _unitOfWork.NativeSql.TransferO_PaperH(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear, 6);

                    _unitOfWork.NativeSql.TransferO_ReceiptBillHeaderH(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear, 1);
                    _unitOfWork.NativeSql.TransferO_ReceiptBillHeaderH(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear, 2);

                    _unitOfWork.NativeSql.TransferO_ReceiptBillHeaderHistoryH(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear, 1);
                    _unitOfWork.NativeSql.TransferO_ReceiptBillHeaderHistoryH(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear, 1);

                    _unitOfWork.NativeSql.TransferO_HeaderH(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear, 1);
                    _unitOfWork.NativeSql.TransferO_HeaderH(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear, 2);
                    _unitOfWork.NativeSql.TransferO_HeaderH(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear, 3);

                    _unitOfWork.NativeSql.TransferO_HeaderH(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear, 3);

                    _unitOfWork.NativeSql.TransferHeaders(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear, 1);
                    _unitOfWork.NativeSql.TransferHeaders(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear, 2);

                    _unitOfWork.NativeSql.TransferHeaderServiceBill(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear, 1);
                    _unitOfWork.NativeSql.TransferHeaderServiceBill(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear, 2);
                    _unitOfWork.NativeSql.TransferHeaderServiceBill(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear, 3);
                    _unitOfWork.NativeSql.TransferHeaderServiceBill(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear, 4);
                    _unitOfWork.NativeSql.TransferHeaderServiceBill(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear, 5);
                    _unitOfWork.NativeSql.TransferHeaderServiceBill(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear, 6);
                    _unitOfWork.NativeSql.TransferHeaderServiceBill(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear, 7);

                    _unitOfWork.NativeSql.TransferAsset(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber,  1);
                    _unitOfWork.NativeSql.TransferAsset(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 2);

                    _unitOfWork.NativeSql.TempPrepaidAndRevenueReceivedDetailFrom(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear , 1);
                    _unitOfWork.NativeSql.TempPrepaidAndRevenueReceivedDetailFrom(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear , 2);

                    _unitOfWork.NativeSql.TempPrepaidAndRevenueReceivedDetailTo(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear ,1);
                    _unitOfWork.NativeSql.TempPrepaidAndRevenueReceivedDetailTo(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, CurrYear ,2);

                    _unitOfWork.NativeSql.TransferDefinitionBank(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 1);
                    _unitOfWork.NativeSql.TransferDefinitionBank(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 2);
                    _unitOfWork.NativeSql.TransferDefinitionBank(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 3);
                    _unitOfWork.NativeSql.TransferDefinitionBank(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 4);

                    _unitOfWork.NativeSql.TransferSupplierInformations(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 1);
                    _unitOfWork.NativeSql.TransferSupplierInformations(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 2);

                    _unitOfWork.NativeSql.TransferOtherAccount(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 1);
                    _unitOfWork.NativeSql.TransferOtherAccount(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 2);
                    _unitOfWork.NativeSql.TransferOtherAccount(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 3);
                    _unitOfWork.NativeSql.TransferOtherAccount(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 4);
                    _unitOfWork.NativeSql.TransferOtherAccount(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 5);
                    _unitOfWork.NativeSql.TransferOtherAccount(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 6);
                    _unitOfWork.NativeSql.TransferOtherAccount(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 7);
                    _unitOfWork.NativeSql.TransferOtherAccount(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 8);
                    _unitOfWork.NativeSql.TransferOtherAccount(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 9);
                    _unitOfWork.NativeSql.TransferOtherAccount(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 10);
                    _unitOfWork.NativeSql.TransferOtherAccount(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 11);
                    _unitOfWork.NativeSql.TransferOtherAccount(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 12);

                    //--------
                    _unitOfWork.NativeSql.TransferSt_BankH(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber);
                    _unitOfWork.NativeSql.TransferSt_FundingAgencyH(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber);
                    _unitOfWork.NativeSql.TransferSt_BranchExpenseAccountH(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber);
                    _unitOfWork.NativeSql.TransferSt_ItemCard(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber);
                    _unitOfWork.NativeSql.TransferSt_ItemCardH(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber);
                    _unitOfWork.NativeSql.TransferSt_BranchOtherExpenseAccountH(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber);
                    _unitOfWork.NativeSql.TransferSt_HeaderH(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber);
                    _unitOfWork.NativeSql.TransferSt_SalesManH(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber);
                    _unitOfWork.NativeSql.TransferSt_PurchaseOrderHeader(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber , CurrYear);

                    _unitOfWork.NativeSql.TransferSt_BranchAccountH(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 1);
                    _unitOfWork.NativeSql.TransferSt_BranchAccountH(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 2);
                    _unitOfWork.NativeSql.TransferSt_BranchAccountH(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 3);
                    _unitOfWork.NativeSql.TransferSt_BranchAccountH(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 4);
                    _unitOfWork.NativeSql.TransferSt_BranchAccountH(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 5);
                    _unitOfWork.NativeSql.TransferSt_BranchAccountH(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 6);
                    _unitOfWork.NativeSql.TransferSt_BranchAccountH(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 7);
                    _unitOfWork.NativeSql.TransferSt_BranchAccountH(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 8);
                    _unitOfWork.NativeSql.TransferSt_BranchAccountH(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 9);
                    _unitOfWork.NativeSql.TransferSt_BranchAccountH(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 10);
                    _unitOfWork.NativeSql.TransferSt_BranchAccountH(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 11);

                    _unitOfWork.NativeSql.TransferSt_OtherAccount(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 1);
                    _unitOfWork.NativeSql.TransferSt_OtherAccount(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 2);
                    _unitOfWork.NativeSql.TransferSt_OtherAccount(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 3);
                    _unitOfWork.NativeSql.TransferSt_OtherAccount(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 4);
                    _unitOfWork.NativeSql.TransferSt_OtherAccount(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 5);
                    _unitOfWork.NativeSql.TransferSt_OtherAccount(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 6);
                    _unitOfWork.NativeSql.TransferSt_OtherAccount(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 7);



                    _unitOfWork.NativeSql.TransferSt_OtherAccountH(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 1);
                    _unitOfWork.NativeSql.TransferSt_OtherAccountH(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 2);
                    _unitOfWork.NativeSql.TransferSt_OtherAccountH(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 3);
                    _unitOfWork.NativeSql.TransferSt_OtherAccountH(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 4);
                    _unitOfWork.NativeSql.TransferSt_OtherAccountH(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 5);
                    _unitOfWork.NativeSql.TransferSt_OtherAccountH(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 6);
                    _unitOfWork.NativeSql.TransferSt_OtherAccountH(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 7);


                    _unitOfWork.NativeSql.TransferSt_Warehouse(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 1);
                    _unitOfWork.NativeSql.TransferSt_Warehouse(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 2);

                    _unitOfWork.NativeSql.TransferSt_WarehouseAccount(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 1);
                    _unitOfWork.NativeSql.TransferSt_WarehouseAccount(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 2);
                    _unitOfWork.NativeSql.TransferSt_WarehouseAccount(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 3);
                    _unitOfWork.NativeSql.TransferSt_WarehouseAccount(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 4);
                    _unitOfWork.NativeSql.TransferSt_WarehouseAccount(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 5);
                    _unitOfWork.NativeSql.TransferSt_WarehouseAccount(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 6);
                    _unitOfWork.NativeSql.TransferSt_WarehouseAccount(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 7);
                    _unitOfWork.NativeSql.TransferSt_WarehouseAccount(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 8);
                    _unitOfWork.NativeSql.TransferSt_WarehouseAccount(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 9);
                    _unitOfWork.NativeSql.TransferSt_WarehouseAccount(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 10);


                    _unitOfWork.NativeSql.TransferSt_Header(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 1);
                    _unitOfWork.NativeSql.TransferSt_Header(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 2);
                    _unitOfWork.NativeSql.TransferSt_Header(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 3);
                    _unitOfWork.NativeSql.TransferSt_Header(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 4);
                    _unitOfWork.NativeSql.TransferSt_Header(UserInfo.fCompanyId, ObjSave.FromAccountNumber, ObjSave.ToAccountNumber, 5);



                    _unitOfWork.Complete();
                }


                
                Msg.Code = 1;
                Msg.Msg = Resources.Resource.OperationCompletedSuccessfully;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                Msg.Code = 0;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }

        }
    }
}