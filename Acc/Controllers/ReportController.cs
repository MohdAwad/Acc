using DevExpress.Web.Mvc;
using Acc.Models;
using Acc.Persistence;
using Acc.Repositories;
using Acc.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.ASPxPivotGrid;
using Acc.Helpers;

namespace Acc.Controllers
{
    [Authorize]
    public class ReportController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private IEnumerable<ChartOfAccount> _TempAccountData = new List<ChartOfAccount>();

        public ReportController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        [Authorize(Roles = "CoOwner,RepAccountStatement")]
        public ActionResult AccountStatementTrial(string id,string id2,int id3)
        {
            var userId = User.Identity.GetUserId();

            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var AccountInfo = _unitOfWork.ChartOfAccount.GetAccountByID(UserInfo.fCompanyId, id);
            int CurrYear = UserInfo.CurrYear;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            if (AccountInfo == null)
            {
                AccountInfo = new ChartOfAccount();
            }
            AccountStatementSearchVM Obj = new AccountStatementSearchVM
            {
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                FromAccAccount = id,
                FromAccName = AccountInfo.ArabicName,
                WorkWithCostCenter = Company.WorkWithCostCenter,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency

            };
            if (id2 != "0")
            {
                Obj.CostCenterNumber = id2;
                Obj.ByCostCenter = true;
                var cost=_unitOfWork.CostCenter.GetCostCenterById(UserInfo.fCompanyId, Obj.CostCenterNumber);
                if (cost != null)
                {
                    Obj.CostCenterName = cost.ArabicName;
                }


                if (id3 != 0)
                {
                    Obj.CostSearchType = id3;
                    Obj.Partofthenumber = true;
                }
            }
            return View("AccountStatement", Obj);
        }
        [Authorize(Roles = "CoOwner,RepAccountStatement")]
        public ActionResult AccountStatement()
        {
            var userId = User.Identity.GetUserId();

            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            int CurrYear = UserInfo.CurrYear;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            AccountStatementSearchVM Obj = new AccountStatementSearchVM
            {
                FromDate = DateTime.Parse("01/01/" + CurrYear),
                ToDate = DateTime.Parse("31/12/" + CurrYear),
                WorkWithCostCenter = Company.WorkWithCostCenter,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency
            };
            return View(Obj);
        }
        [HttpPost]
        public JsonResult GetAccountNumber(AccountStatementSearchVM Obj)
        {
            try
            {

                var userId = User.Identity.GetUserId();

                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                int CurrYear = UserInfo.CurrYear;

                var model = _unitOfWork.NativeSql.GetAccountInfo(UserInfo.fCompanyId, Obj.FromAccAccount, CurrYear);

                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string err = ex.Message;



                return Json(new List<ChartOfAccount>(), JsonRequestBehavior.AllowGet);

            }


        }
        [HttpPost]
        public JsonResult GetAccountTransAction(AccountStatementSearchVM Obj)
        {
            try
            {
                if (!String.IsNullOrEmpty(Obj.FromAccAccount))
                {
                    var userId = User.Identity.GetUserId();
                    string UserID = User.Identity.GetUserId();
                    var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
                    int CurrYear = UserInfo.CurrYear;
                    var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                    var AccTransAction = _unitOfWork.NativeSql.GetAccountStatement(UserInfo.fCompanyId, CurrYear, Obj.FromAccAccount, Obj.FromDate, Obj.ToDate);
                    if(Obj.ByCostCenter && (!String.IsNullOrEmpty(Obj.CostCenterNumber)) ){
                       
                          if (Obj.CostSearchType == 1 && Obj.Partofthenumber)
                        {
                            AccTransAction = AccTransAction.Where(m => m.CostCenter != null).ToList();
                            AccTransAction = AccTransAction.Where(m => m.CostCenter.StartsWith( Obj.CostCenterNumber)).ToList();
                        }
                        else if (Obj.CostSearchType == 2 && Obj.Partofthenumber)
                        {
                            AccTransAction = AccTransAction.Where(m => m.CostCenter != null).ToList();
                            AccTransAction = AccTransAction.Where(m => m.CostCenter.EndsWith(Obj.CostCenterNumber)).ToList();
                        }
                        else if (Obj.CostSearchType == 3 && Obj.Partofthenumber)
                        {
                            AccTransAction = AccTransAction.Where(m => m.CostCenter != null).ToList();
                            AccTransAction = AccTransAction.Where(m => m.CostCenter.Contains(Obj.CostCenterNumber)).ToList();
                        }
                        else
                        {
                            AccTransAction = AccTransAction.Where(m => m.CostCenter != null).ToList();
                            AccTransAction = AccTransAction.Where(m => m.CostCenter == Obj.CostCenterNumber).ToList();
                        }
                      
                        
                    }
                    var CompanyTransAction = _unitOfWork.CompanyTransactionKind.GetAllCompanyTransactionKind(UserInfo.fCompanyId);
                    var TransActionKind = _unitOfWork.TransactionKind.GetAllTransactionKind();

                    var St_CompanyTransAction = _unitOfWork.St_CompanyTransactionKind.GetAllSt_CompanyTransactionKind(UserInfo.fCompanyId);
                    var St_TranActionKind = _unitOfWork.St_TransactionKind.GetAllSt_TransactionKind();

                    var St_CompanyTransAction_H = _unitOfWork.St_CompanyTransactionKindH.GetAllSt_CompanyTransactionKindH(UserInfo.fCompanyId);
                    var St_TranActionKind_H = _unitOfWork.St_TransactionKindH.GetAllSt_TransactionKindH();

                    var FromDate = Obj.FromDate;
                    var ToDate = Obj.ToDate;
                    var ObjAccountStatmentList = new List<AccountStatementVM>();
                    double LastBalance = 0;
                    double TotalDebit = 0;
                    double TotalCredit = 0;
                    double NetTotal = 0;
                    double TotalUnpaidChequesReceived = _unitOfWork.NativeSql.GetTotalUnpaidChequesReceived(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate, Obj.FromAccAccount);
                    double TotalUnpaidChequesPayment = _unitOfWork.NativeSql.GetTotalUnpaidChequesPayment(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate, Obj.FromAccAccount);

                    if (DateTime.Parse("31/12/" + (CurrYear - 1).ToString()) < Obj.FromDate) {
                        var OpeiningBalance = _unitOfWork.NativeSql.GetOpeningBalanceTrans(UserInfo.fCompanyId, CurrYear, Obj.FromAccAccount, DateTime.Parse("31/12/" + (CurrYear - 1).ToString()), DateTime.Parse("31/12/" + (CurrYear - 1).ToString()));
                        if (OpeiningBalance.Count() > 0)
                        {
                            foreach (var Day in OpeiningBalance)
                            {
                                TotalDebit = TotalDebit + Day.Debit;
                                TotalCredit = TotalCredit + Day.Credit;

                                AccountStatementVM ObjStatment = new AccountStatementVM();
                                ObjStatment.TransDate = Day.VoucherDate;
                                ObjStatment.Statment = Day.Note;
                                ObjStatment.TransName = "";
                                ObjStatment.Credit = Day.Credit * -1;
                                ObjStatment.Debit = Day.Debit;
                                ObjStatment.Balance = (LastBalance) + ((Day.Credit * -1) + Day.Debit);
                                ObjStatment.CompanyTransactionKindID = Day.CompanyTransactionKindNo;
                                ObjStatment.TransactionKindID = Day.TransactionKindNo;
                                ObjStatment.VHFI = Day.VHI;
                                ObjStatment.VoucherNumber = Day.VoucherNumber;
                                if (Company.TheDecimalPointForTheLocalCurrency == 2)
                                {
                                    ObjStatment.sCredit = String.Format("{0:n2}", (Day.Credit * -1));
                                    ObjStatment.sDebit = String.Format("{0:n2}", (Day.Debit));
                                }
                                else
                                {
                                    ObjStatment.sCredit = String.Format("{0:n3}", (Day.Credit * -1));
                                    ObjStatment.sDebit = String.Format("{0:n3}", (Day.Debit));
                                }
                                


                                if (ObjStatment.Debit < 0)
                                {
                                    if (Company.TheDecimalPointForTheLocalCurrency == 2)
                                    {
                                        ObjStatment.sDebit = "(" + String.Format("{0:n2}", (ObjStatment.Debit * -1)) + ")";
                                    }
                                    else
                                    {
                                        ObjStatment.sDebit = "(" + String.Format("{0:n3}", (ObjStatment.Debit * -1)) + ")";
                                    }
                                    
                                }
                                if (ObjStatment.Credit < 0)
                                {

                                   

                                    if (Company.TheDecimalPointForTheLocalCurrency == 2)
                                    {
                                        ObjStatment.sCredit = "" + String.Format("{0:n2}", (ObjStatment.Credit * -1)) + "";
                                    }
                                    else
                                    {
                                        ObjStatment.sCredit = "" + String.Format("{0:n3}", (ObjStatment.Credit * -1)) + "";
                                    }
                                        
                                }
                                if (ObjStatment.Debit == 0)
                                    ObjStatment.sDebit = "";
                                if (ObjStatment.Credit == 0)
                                    ObjStatment.sCredit = "";

                                if (ObjStatment.Balance == 0)
                                    ObjStatment.sBalance = "";

                                ObjStatment.TransName = "";



                                if (ObjStatment.Balance < 0)
                                {
                                    if (Company.TheDecimalPointForTheLocalCurrency == 2)
                                    {
                                        ObjStatment.sBalance = "(" + String.Format("{0:n2}", (ObjStatment.Balance * -1)) + ")";
                                    }
                                    else
                                    {
                                        ObjStatment.sBalance = "(" + String.Format("{0:n3}", (ObjStatment.Balance * -1)) + ")";
                                    }
                                    
                                }
                                else
                                {
                                    if (Company.TheDecimalPointForTheLocalCurrency == 2)
                                    {
                                        ObjStatment.sBalance = String.Format("{0:n2}", (ObjStatment.Balance));
                                    }
                                    else
                                    {
                                        ObjStatment.sBalance = String.Format("{0:n3}", (ObjStatment.Balance));
                                    }
                                    
                                }


                                LastBalance = ObjStatment.Balance;

                                ObjAccountStatmentList.Add(ObjStatment);
                            }
                        }
                    }

                    while (FromDate <= ToDate)
                    {
                        var CurrDay = AccTransAction.Where(m => m.VoucherDate >= FromDate && m.VoucherDate < FromDate.AddDays(1)).OrderBy(m => m.VHI).OrderBy(m => m.RowNumber);
                        foreach (var Day in CurrDay)
                        {
                            TotalDebit = TotalDebit + Day.Debit;
                            TotalCredit = TotalCredit + Day.Credit;

                            AccountStatementVM ObjStatment = new AccountStatementVM();
                            ObjStatment.TransDate = Day.VoucherDate;
                            ObjStatment.Statment = Day.Note;
                            ObjStatment.TransName = Day.CompanyTransactionKindNo.ToString();
                            ObjStatment.Credit = Day.Credit * -1;
                            ObjStatment.Debit = Day.Debit;
                            ObjStatment.Balance = (LastBalance) + ((Day.Credit * -1) + Day.Debit);
                            ObjStatment.CompanyTransactionKindID = Day.CompanyTransactionKindNo;
                            ObjStatment.TransactionKindID = Day.TransactionKindNo;
                            ObjStatment.VHFI = Day.VHI;
                            ObjStatment.VoucherNumber = Day.VoucherNumber;
                            if (Company.TheDecimalPointForTheLocalCurrency == 2)
                            {
                                ObjStatment.sCredit = String.Format("{0:n2}", (Day.Credit * -1));
                                ObjStatment.sDebit = String.Format("{0:n2}", (Day.Debit));
                            }
                            else
                            {
                                ObjStatment.sCredit = String.Format("{0:n3}", (Day.Credit * -1));
                                ObjStatment.sDebit = String.Format("{0:n3}", (Day.Debit));
                            }
                            



                            if (ObjStatment.Debit < 0)
                            {
                                if (Company.TheDecimalPointForTheLocalCurrency == 2)
                                {
                                    ObjStatment.sDebit = "(" + String.Format("{0:n2}", (ObjStatment.Debit * -1)) + ")";
                                }
                                else
                                {
                                    ObjStatment.sDebit = "(" + String.Format("{0:n3}", (ObjStatment.Debit * -1)) + ")";
                                }
                                
                            }
                            if (ObjStatment.Credit < 0)
                            {
                                if (Company.TheDecimalPointForTheLocalCurrency == 2)
                                {
                                    ObjStatment.sCredit = "" + String.Format("{0:n2}", (ObjStatment.Credit * -1)) + "";
                                }
                                else
                                {
                                    ObjStatment.sCredit = "" + String.Format("{0:n3}", (ObjStatment.Credit * -1)) + "";
                                }
                                
                            }
                            if (ObjStatment.Debit == 0)
                                ObjStatment.sDebit = "";
                            if (ObjStatment.Credit == 0)
                                ObjStatment.sCredit = "";

                            if (ObjStatment.Balance == 0)
                                ObjStatment.sBalance = "";

                            if (Company.Hiajneh == 1)
                            {
                                if (Day.CompanyTransactionKindNo >= 1000)
                                {
                                    var V = St_TranActionKind_H.FirstOrDefault(m => m.St_TransactionKindID == Day.TransactionKindNo);
                                    var St = St_CompanyTransAction_H.FirstOrDefault(m => m.St_TransactionKindID == Day.TransactionKindNo && m.St_CompanyTransactionKindID == Day.CompanyTransactionKindNo);
                                    if (V != null)
                                    {
                                        ObjStatment.TransName = V.ArabicName + " (" + Day.VoucherNumber + ") ";
                                    }
                                    if (St != null)
                                    {
                                        ObjStatment.TransName = "(" + St.StockCode + ")" + ObjStatment.TransName;
                                    }

                                }
                                else if (Day.CompanyTransactionKindNo == 0)
                                {
                                    var V = TransActionKind.FirstOrDefault(m => m.TransactionKindID == Day.TransactionKindNo);
                                    if (V != null)
                                    {
                                        ObjStatment.TransName = V.ArabicName + " (" + Day.VoucherNumber + ") ";
                                    }
                                }
                                else
                                {
                                    var V = CompanyTransAction.FirstOrDefault(m => m.TransactionKindID == Day.TransactionKindNo
                                      && m.CompanyTransactionKindID == Day.CompanyTransactionKindNo);
                                    if (V != null)
                                    {
                                        ObjStatment.TransName = V.ArabicName + " (" + Day.VoucherNumber + ") ";
                                    }
                                }
                            }
                            else if (Company.WorkWithStock == 1)
                            {
                                if (Day.CompanyTransactionKindNo >= 1000)
                                {
                                    var V = St_TranActionKind.FirstOrDefault(m => m.St_TransactionKindID == Day.TransactionKindNo);
                                    var St = St_CompanyTransAction.FirstOrDefault(m => m.St_TransactionKindID == Day.TransactionKindNo && m.St_CompanyTransactionKindID == Day.CompanyTransactionKindNo);
                                    if (V != null)
                                    {
                                        ObjStatment.TransName = V.ArabicName + " (" + Day.VoucherNumber + ") ";
                                    }
                                    if (St != null)
                                    {
                                        ObjStatment.TransName = "(" + St.StockCode + ")" + ObjStatment.TransName;
                                    }

                                }
                                else if (Day.CompanyTransactionKindNo == 0)
                                {
                                    var V = TransActionKind.FirstOrDefault(m => m.TransactionKindID == Day.TransactionKindNo);
                                    if (V != null)
                                    {
                                        ObjStatment.TransName = V.ArabicName + " (" + Day.VoucherNumber + ") ";
                                    }
                                }
                                else
                                {
                                    var V = CompanyTransAction.FirstOrDefault(m => m.TransactionKindID == Day.TransactionKindNo
                                      && m.CompanyTransactionKindID == Day.CompanyTransactionKindNo);
                                    if (V != null)
                                    {
                                        ObjStatment.TransName = V.ArabicName + " (" + Day.VoucherNumber + ") ";
                                    }
                                }
                            }
                            else
                            {
                                if (Day.CompanyTransactionKindNo == 0)
                                {
                                    var V = TransActionKind.FirstOrDefault(m => m.TransactionKindID == Day.TransactionKindNo);
                                    if (V != null)
                                    {
                                        ObjStatment.TransName = V.ArabicName + " (" + Day.VoucherNumber + ") ";
                                    }
                                }
                                else
                                {
                                    var V = CompanyTransAction.FirstOrDefault(m => m.TransactionKindID == Day.TransactionKindNo
                                      && m.CompanyTransactionKindID == Day.CompanyTransactionKindNo);
                                    if (V != null)
                                    {
                                        ObjStatment.TransName = V.ArabicName + " (" + Day.VoucherNumber + ") ";
                                    }
                                }
                            }
                            if (ObjStatment.Balance < 0)
                            {
                                if (Company.TheDecimalPointForTheLocalCurrency == 2)
                                {
                                    ObjStatment.sBalance = "(" + String.Format("{0:n2}", (ObjStatment.Balance * -1)) + ")";
                                }
                                else
                                {
                                    ObjStatment.sBalance = "(" + String.Format("{0:n3}", (ObjStatment.Balance * -1)) + ")";
                                }
                                
                            }
                            else
                            {
                                if (Company.TheDecimalPointForTheLocalCurrency == 2)
                                {
                                    ObjStatment.sBalance = String.Format("{0:n2}", (ObjStatment.Balance));
                                }
                                else
                                {
                                    ObjStatment.sBalance = String.Format("{0:n3}", (ObjStatment.Balance));
                                }
                                
                            }
                            LastBalance = ObjStatment.Balance;
                            ObjAccountStatmentList.Add(ObjStatment);
                        }



                        FromDate = FromDate.AddDays(1);
                    }
                    NetTotal = TotalDebit - TotalCredit;
                    AccountStatementHeaderVM ObjTot = new AccountStatementHeaderVM();
                    ObjTot.AccountStatementVM = ObjAccountStatmentList;
                    ObjTot.TotalCredit = TotalCredit;
                    ObjTot.TotalDebit = TotalDebit;
                    ObjTot.NetTotal = NetTotal;
                    ObjTot.TotalUnpaidChequesReceived = TotalUnpaidChequesReceived;
                    ObjTot.TotalUnpaidChequesPayment = TotalUnpaidChequesPayment;
                    if (Company.TheDecimalPointForTheLocalCurrency == 2)
                    {
                        ObjTot.sTotalCredit = String.Format("{0:n2}", (TotalCredit));
                        ObjTot.sTotalDebit = String.Format("{0:n2}", (TotalDebit));
                        if (NetTotal > 0)
                        {
                            ObjTot.sNetTotal = String.Format("{0:n2}", (NetTotal));
                        }
                        else
                        {
                            ObjTot.sNetTotal = "(" + String.Format("{0:n2}", (NetTotal * -1)) + ")";
                        }
                        ObjTot.sTotalCustomerCheque = String.Format("{0:n2}", (TotalUnpaidChequesReceived));
                        ObjTot.sTotalSupplierCheque = String.Format("{0:n2}", (TotalUnpaidChequesPayment));
                    }
                    else
                    {
                        ObjTot.sTotalCredit = String.Format("{0:n3}", (TotalCredit));
                        ObjTot.sTotalDebit = String.Format("{0:n3}", (TotalDebit));
                        if (NetTotal > 0)
                        {
                            ObjTot.sNetTotal = String.Format("{0:n3}", (NetTotal));
                        }
                        else
                        {
                            ObjTot.sNetTotal = "(" + String.Format("{0:n3}", (NetTotal * -1)) + ")";
                        }
                        ObjTot.sTotalCustomerCheque = String.Format("{0:n3}", (TotalUnpaidChequesReceived));
                        ObjTot.sTotalSupplierCheque = String.Format("{0:n3}", (TotalUnpaidChequesPayment));
                    }
                    



                    return Json(ObjTot, JsonRequestBehavior.AllowGet);

                }

                else
                {
                    var ObjH = new AccountStatementHeaderVM
                    {
                        AccountStatementVM = new List<AccountStatementVM>(),
                        NetTotal = 0,
                        TotalCredit = 0,
                        TotalDebit = 0,
                    };
                    return Json(ObjH, JsonRequestBehavior.AllowGet);

                }



            }
            catch (Exception ex)
            {
                string err = ex.Message;

                var ObjH = new AccountStatementHeaderVM
                {
                    AccountStatementVM = new List<AccountStatementVM>(),
                    NetTotal = 0,
                    TotalCredit = 0,
                    TotalDebit = 0,
                };

                return Json(ObjH, JsonRequestBehavior.AllowGet);

            }
        }
        [HttpGet]
        public ActionResult ShowCustomerCheque(string id, string id2, string id3)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var PaperFilterVM = new PaperFilterVM
            {
                FromAccAccount = id,
                FromDate = DateTime.Parse(id2.Substring(0, 2) + "/" + id2.Substring(2, 2) + "/" + id2.Substring(4, 4)),
                ToDate = DateTime.Parse(id3.Substring(0, 2) + "/" + id3.Substring(2, 2) + "/" + id3.Substring(4, 4)),
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency
            };
            return PartialView(PaperFilterVM);
        }
        [HttpGet]
        public JsonResult GetShowCustomerCheque(string id, string id2, string id3)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var FromDate = DateTime.Parse(id2.Substring(0, 2) + "/" + id2.Substring(2, 2) + "/" + id2.Substring(4, 4));
                var ToDate = DateTime.Parse(id3.Substring(0, 2) + "/" + id3.Substring(2, 2) + "/" + id3.Substring(4, 4));
                var ShowPaperDetails = _unitOfWork.NativeSql.GetUnpaidChequesReceived(UserInfo.fCompanyId, FromDate, ToDate, id);
                return Json(ShowPaperDetails, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<PaperFilterVM>(), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult ShowSupplierCheque(string id, string id2, string id3)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var PaperFilterVM = new PaperFilterVM
            {
                FromAccAccount = id,
                FromDate = DateTime.Parse(id2.Substring(0, 2) + "/" + id2.Substring(2, 2) + "/" + id2.Substring(4, 4)),
                ToDate = DateTime.Parse(id3.Substring(0, 2) + "/" + id3.Substring(2, 2) + "/" + id3.Substring(4, 4)),
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency
            };
            return PartialView(PaperFilterVM);
        }
        [HttpGet]
        public JsonResult GetShowSupplierCheque(string id, string id2, string id3)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var FromDate = DateTime.Parse(id2.Substring(0, 2) + "/" + id2.Substring(2, 2) + "/" + id2.Substring(4, 4));
                var ToDate = DateTime.Parse(id3.Substring(0, 2) + "/" + id3.Substring(2, 2) + "/" + id3.Substring(4, 4));
                var ShowPaperDetails = _unitOfWork.NativeSql.GetUnpaidChequesPayment(UserInfo.fCompanyId, FromDate, ToDate, id);
                return Json(ShowPaperDetails, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<PaperFilterVM>(), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetProfitlossreport(AccountLevelRepVM Obj)
        {
         
            try
            {
                ProfitLossVM Data = new ProfitLossVM();
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);

                int CurrYear = UserInfo.CurrYear;
                var AllData = _unitOfWork.NativeSql.GetAccountTypeTotal(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
             
                if (AllData.Count()!=0)
                {
                    Data.GOODSSTART = Obj.GoodsFirst;
                    Data.GOODSEND = Obj.GoodsEnd;

                    if (!string.IsNullOrEmpty(Obj.AccNo))
                    {
                        Data.GOODSSTART= _unitOfWork.NativeSql.GetAccountTypeTotalForAcc(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate, Obj.AccNo);
                    }

                    if (AllData.FirstOrDefault(m => m.AccountTypeID == 7) != null)
                        Data.SALES = AllData.FirstOrDefault(m => m.AccountTypeID == 7).Total;

                    if (AllData.FirstOrDefault(m => m.AccountTypeID == 10) != null)
                        Data.PURCHASES = AllData.FirstOrDefault(m => m.AccountTypeID == 10).Total;


                    if (AllData.FirstOrDefault(m => m.AccountTypeID == 8) != null)
                        Data.GOODSSTART = AllData.FirstOrDefault(m => m.AccountTypeID == 8).Total;


                    if (AllData.FirstOrDefault(m => m.AccountTypeID == 9) != null)
                        Data.GOODSEND = AllData.FirstOrDefault(m => m.AccountTypeID == 9).Total;


                    if (AllData.FirstOrDefault(m => m.AccountTypeID == 14) != null)
                        Data.SALESEXPENSES = AllData.FirstOrDefault(m => m.AccountTypeID == 14).Total;


                    if (AllData.FirstOrDefault(m => m.AccountTypeID == 17) != null)
                        Data.OTHEREARNING = AllData.FirstOrDefault(m => m.AccountTypeID == 17).Total;


                    if (AllData.FirstOrDefault(m => m.AccountTypeID == 15) != null)
                        Data.ADMINISTRATIONEXPENSES = AllData.FirstOrDefault(m => m.AccountTypeID == 15).Total;


                    if (AllData.FirstOrDefault(m => m.AccountTypeID == 16) != null)
                        Data.FINANCIALEXPENSES = AllData.FirstOrDefault(m => m.AccountTypeID == 16).Total;

                       Data.SALESCOST = Data.PURCHASES + Data.GOODSSTART - Data.GOODSEND;
                       Data.GrossProfit = Data.SALES + Data.SALESCOST + Data.SALESEXPENSES;
                       Data.NetTotal = Data.GrossProfit + Data.OTHEREARNING + Data.ADMINISTRATIONEXPENSES + Data.FINANCIALEXPENSES;




                }


                return Json(Data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                
                return Json(new ProfitLossVM(), JsonRequestBehavior.AllowGet);
            }

        }
        [Authorize(Roles = "CoOwner,Profitlossreport")]
        public ActionResult Profitlossreport()
        {

            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            //  var GetAllLevel = _unitOfWork.NativeSql.GetAccountLevelVMs(UserInfo.fCompanyId);

            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            String Chart = CoInfo.AccountChart;


            Chart = Chart.Trim(new Char[] { '-', '-', ' ' });

            string[] numbers = Regex.Split(Chart, @"\D+");
            var AccountLevel = new List<AccountLevelDropVM>();
            int i = 1;
            foreach (string value in numbers)
            {
                if (!string.IsNullOrEmpty(value))
                {

                    AccountLevelDropVM Obj = new AccountLevelDropVM
                    {
                        AccountLevel = i,
                        LevelName = Resources.Resource.Level + " " + i
                    };
                    AccountLevel.Add(Obj);
                    i = i + 1;
                }
            }



            int CurrYear = UserInfo.CurrYear;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);

            AccountLevelRepVM ObjAccountLevelRep = new AccountLevelRepVM
            {
                AccountLevelDropVM = AccountLevel,
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency
            };

            return View(ObjAccountLevelRep);
        }
        [Authorize(Roles = "CoOwner,RepCustomerBalancesReport")]
        public ActionResult CustomerBalancesReport()
        {

            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            //  var GetAllLevel = _unitOfWork.NativeSql.GetAccountLevelVMs(UserInfo.fCompanyId);

            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            String Chart = CoInfo.AccountChart;


            Chart = Chart.Trim(new Char[] { '-', '-', ' ' });

            string[] numbers = Regex.Split(Chart, @"\D+");
            var AccountLevel = new List<AccountLevelDropVM>();
            int i = 1;
            foreach (string value in numbers)
            {
                if (!string.IsNullOrEmpty(value))
                {

                    AccountLevelDropVM Obj = new AccountLevelDropVM
                    {
                        AccountLevel = i,
                        LevelName = Resources.Resource.Level + " " + i
                    };
                    AccountLevel.Add(Obj);
                    i = i + 1;
                }
            }
            int CurrYear = UserInfo.CurrYear;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            AccountLevelRepVM ObjAccountLevelRep = new AccountLevelRepVM
            {
                AccountLevelDropVM = AccountLevel,
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                TempCostID = "0",
                TempCostType = 0,
                AccountLevelDropVMID = i - 1,
                WorkWithCostCenter = Company.WorkWithCostCenter,
                CustomerCity = _unitOfWork.CustomerCity.GetAllCustomerCity(CoInfo.CompanyID),
                CustomerArea = _unitOfWork.CustomerArea.GetAllCustomerAreaByCityID(CoInfo.CompanyID, 0),
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency


            };



            return View(ObjAccountLevelRep);
        }
        [Authorize(Roles = "CoOwner,RepSupplierBalancesReport")]
        public ActionResult SupplierBalancesReport()
        {

            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            //  var GetAllLevel = _unitOfWork.NativeSql.GetAccountLevelVMs(UserInfo.fCompanyId);

            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            String Chart = CoInfo.AccountChart;


            Chart = Chart.Trim(new Char[] { '-', '-', ' ' });

            string[] numbers = Regex.Split(Chart, @"\D+");
            var AccountLevel = new List<AccountLevelDropVM>();
            int i = 1;
            foreach (string value in numbers)
            {
                if (!string.IsNullOrEmpty(value))
                {

                    AccountLevelDropVM Obj = new AccountLevelDropVM
                    {
                        AccountLevel = i,
                        LevelName = Resources.Resource.Level + " " + i
                    };
                    AccountLevel.Add(Obj);
                    i = i + 1;
                }
            }



            int CurrYear = UserInfo.CurrYear;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            AccountLevelRepVM ObjAccountLevelRep = new AccountLevelRepVM
            {
                AccountLevelDropVM = AccountLevel,
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                TempCostID = "0",
                TempCostType = 0,
                AccountLevelDropVMID = i - 1,
                WorkWithCostCenter = Company.WorkWithCostCenter,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency


            };



            return View(ObjAccountLevelRep);
        }
        [Authorize(Roles = "CoOwner,RepExpenseAnalysisDetailReport")]
        public ActionResult ExpenseAnalysisDetailReport()
        {

            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            //  var GetAllLevel = _unitOfWork.NativeSql.GetAccountLevelVMs(UserInfo.fCompanyId);

            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            String Chart = CoInfo.AccountChart;


            Chart = Chart.Trim(new Char[] { '-', '-', ' ' });

            string[] numbers = Regex.Split(Chart, @"\D+");
            var AccountLevel = new List<AccountLevelDropVM>();
            int i = 1;
            foreach (string value in numbers)
            {
                if (!string.IsNullOrEmpty(value))
                {

                    AccountLevelDropVM Obj = new AccountLevelDropVM
                    {
                        AccountLevel = i,
                        LevelName = Resources.Resource.Level + " " + i
                    };
                    AccountLevel.Add(Obj);
                    i = i + 1;
                }
            }



            int CurrYear = UserInfo.CurrYear;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            AccountLevelRepVM ObjAccountLevelRep = new AccountLevelRepVM
            {
                AccountLevelDropVM = AccountLevel,
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                TempCostID = "0",
                TempCostType = 0,
                AccountLevelDropVMID = i - 1,
                AccountType=_unitOfWork.AccountType.GetAllAccountType(),
                AccountTypeID=0,
                WorkWithCostCenter = Company.WorkWithCostCenter,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency
            };



            return View(ObjAccountLevelRep);
        }
        [Authorize(Roles = "CoOwner,RepExpenseAnalysisReport")]
        public ActionResult ExpenseAnalysisReport()
        {

            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            //  var GetAllLevel = _unitOfWork.NativeSql.GetAccountLevelVMs(UserInfo.fCompanyId);

            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            String Chart = CoInfo.AccountChart;

            Chart = Chart.Trim(new Char[] { '-', '-', ' ' });

            string[] numbers = Regex.Split(Chart, @"\D+");
            var AccountLevel = new List<AccountLevelDropVM>();
           
            int i = 1;
            foreach (string value in numbers)
            {
                if (!string.IsNullOrEmpty(value))
                {

                    AccountLevelDropVM Obj = new AccountLevelDropVM
                    {
                        AccountLevel = i,
                        LevelName = Resources.Resource.Level + " " + i
                    };
                    AccountLevel.Add(Obj);
                    i = i + 1;
                }
            }



            int CurrYear = UserInfo.CurrYear;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            AccountLevelRepVM ObjAccountLevelRep = new AccountLevelRepVM
            {
                AccountLevelDropVM = AccountLevel,
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                TempCostID = "0",
                TempCostType = 0,
                AccountLevelDropVMID = i - 1,
                WorkWithCostCenter = Company.WorkWithCostCenter,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency
            };



            return View(ObjAccountLevelRep);
        }
        [Authorize(Roles = "CoOwner,RepTrialBalance")]
        public ActionResult TrialBalance()
        {

            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            //  var GetAllLevel = _unitOfWork.NativeSql.GetAccountLevelVMs(UserInfo.fCompanyId);

            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            String Chart = CoInfo.AccountChart;


            Chart = Chart.Trim(new Char[] { '-', '-', ' ' });

            string[] numbers = Regex.Split(Chart, @"\D+");
            var AccountLevel = new List<AccountLevelDropVM>();
            int i = 1;
            foreach (string value in numbers)
            {
                if (!string.IsNullOrEmpty(value))
                {

                    AccountLevelDropVM Obj = new AccountLevelDropVM
                    {
                        AccountLevel = i,
                        LevelName = Resources.Resource.Level + " " + i
                    };
                    AccountLevel.Add(Obj);
                    i = i + 1;
                }
            }

         

            int CurrYear = UserInfo.CurrYear;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            AccountLevelRepVM ObjAccountLevelRep = new AccountLevelRepVM
            {
                AccountLevelDropVM = AccountLevel,
                
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                TempCostID="0",
                TempCostType=0,
                WorkWithCostCenter = Company.WorkWithCostCenter,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency


            };



            return View(ObjAccountLevelRep);
        }


        [Authorize(Roles = "CoOwner,RepTrialBalance")]
        public ActionResult BalanceSheet()
        {

            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            //  var GetAllLevel = _unitOfWork.NativeSql.GetAccountLevelVMs(UserInfo.fCompanyId);

            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            String Chart = CoInfo.AccountChart;


            Chart = Chart.Trim(new Char[] { '-', '-', ' ' });

            string[] numbers = Regex.Split(Chart, @"\D+");
            var AccountLevel = new List<AccountLevelDropVM>();
            int i = 1;
            foreach (string value in numbers)
            {
                if (!string.IsNullOrEmpty(value))
                {

                    AccountLevelDropVM Obj = new AccountLevelDropVM
                    {
                        AccountLevel = i,
                        LevelName = Resources.Resource.Level + " " + i
                    };
                    AccountLevel.Add(Obj);
                    i = i + 1;
                }
            }



            int CurrYear = UserInfo.CurrYear;

            AccountLevelRepVM ObjAccountLevelRep = new AccountLevelRepVM
            {
                AccountLevelDropVM = AccountLevel,
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                TempCostID = "0",
                TempCostType = 0,
                AccountLevelDropVMID=1
            };



            return View(ObjAccountLevelRep);
        }
        public ActionResult ReportDash()
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var ReportDashObj = new ChartDashVM();
            ReportDashObj.WorkWithCostCenter = Company.WorkWithCostCenter;
            return View(ReportDashObj);
        }
        [Authorize(Roles = "CoOwner,RepTrialBalanceYearly")]
        public ActionResult TrialBalanceYearly()
        {

            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            //  var GetAllLevel = _unitOfWork.NativeSql.GetAccountLevelVMs(UserInfo.fCompanyId);

            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            String Chart = CoInfo.AccountChart;


            Chart = Chart.Trim(new Char[] { '-', '-', ' ' });

            string[] numbers = Regex.Split(Chart, @"\D+");
            var AccountLevel = new List<AccountLevelDropVM>();
            int i = 1;
            foreach (string value in numbers)
            {
                if (!string.IsNullOrEmpty(value))
                {

                    AccountLevelDropVM Obj = new AccountLevelDropVM
                    {
                        AccountLevel = i,
                        LevelName = Resources.Resource.Level + " " + i
                    };
                    AccountLevel.Add(Obj);
                    i = i + 1;
                }
            }



            int CurrYear = UserInfo.CurrYear;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);

            AccountLevelRepVM ObjAccountLevelRep = new AccountLevelRepVM
            {
                AccountLevelDropVM = AccountLevel,
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency


            };



            return View(ObjAccountLevelRep);
        }
        [Authorize(Roles = "CoOwner,RepEstimatedBudgetForCostCenters")]
        public ActionResult EstimatedBudgetForCostCenters()
        {

            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            //  var GetAllLevel = _unitOfWork.NativeSql.GetAccountLevelVMs(UserInfo.fCompanyId);

            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            String Chart = CoInfo.AccountChart;


            Chart = Chart.Trim(new Char[] { '-', '-', ' ' });

            string[] numbers = Regex.Split(Chart, @"\D+");
            var AccountLevel = new List<AccountLevelDropVM>();
            int i = 1;
            foreach (string value in numbers)
            {
                if (!string.IsNullOrEmpty(value))
                {

                    AccountLevelDropVM Obj = new AccountLevelDropVM
                    {
                        AccountLevel = i,
                        LevelName = Resources.Resource.Level + " " + i
                    };
                    AccountLevel.Add(Obj);
                    i = i + 1;
                }
            }



            int CurrYear = UserInfo.CurrYear;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);

            AccountLevelRepVM ObjAccountLevelRep = new AccountLevelRepVM
            {
                AccountLevelDropVM = AccountLevel,
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency



            };



            return View(ObjAccountLevelRep);
        }
        [Authorize(Roles = "CoOwner,RepEstimatedBudgetCostAllMonth")]
        public ActionResult EstimatedBudgetCostAllMonth()
        {

            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            //  var GetAllLevel = _unitOfWork.NativeSql.GetAccountLevelVMs(UserInfo.fCompanyId);

            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            String Chart = CoInfo.AccountChart;


            Chart = Chart.Trim(new Char[] { '-', '-', ' ' });

            string[] numbers = Regex.Split(Chart, @"\D+");
            var AccountLevel = new List<AccountLevelDropVM>();
            int i = 1;
            foreach (string value in numbers)
            {
                if (!string.IsNullOrEmpty(value))
                {

                    AccountLevelDropVM Obj = new AccountLevelDropVM
                    {
                        AccountLevel = i,
                        LevelName = Resources.Resource.Level + " " + i
                    };
                    AccountLevel.Add(Obj);
                    i = i + 1;
                }
            }



            int CurrYear = UserInfo.CurrYear;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);

            AccountLevelRepVM ObjAccountLevelRep = new AccountLevelRepVM
            {
                AccountLevelDropVM = AccountLevel,
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency


            };



            return View(ObjAccountLevelRep);
        }
        [Authorize(Roles = "CoOwner,RepEstimatedBudgetForAccount")]
        public ActionResult EstimatedBudgetForAccount()
        {

            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            //  var GetAllLevel = _unitOfWork.NativeSql.GetAccountLevelVMs(UserInfo.fCompanyId);

            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            String Chart = CoInfo.AccountChart;


            Chart = Chart.Trim(new Char[] { '-', '-', ' ' });

            string[] numbers = Regex.Split(Chart, @"\D+");
            var AccountLevel = new List<AccountLevelDropVM>();
            int i = 1;
            foreach (string value in numbers)
            {
                if (!string.IsNullOrEmpty(value))
                {

                    AccountLevelDropVM Obj = new AccountLevelDropVM
                    {
                        AccountLevel = i,
                        LevelName = Resources.Resource.Level + " " + i
                    };
                    AccountLevel.Add(Obj);
                    i = i + 1;
                }
            }



            int CurrYear = UserInfo.CurrYear;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            AccountLevelRepVM ObjAccountLevelRep = new AccountLevelRepVM
            {
                AccountLevelDropVM = AccountLevel,
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency



            };



            return View(ObjAccountLevelRep);
        }
        [Authorize(Roles = "CoOwner,RepEstimatedBudgetForAccountAll")]
        public ActionResult EstimatedBudgetForAccountAll()
        {

            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            //  var GetAllLevel = _unitOfWork.NativeSql.GetAccountLevelVMs(UserInfo.fCompanyId);

            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            String Chart = CoInfo.AccountChart;


            Chart = Chart.Trim(new Char[] { '-', '-', ' ' });

            string[] numbers = Regex.Split(Chart, @"\D+");
            var AccountLevel = new List<AccountLevelDropVM>();
            int i = 1;
            foreach (string value in numbers)
            {
                if (!string.IsNullOrEmpty(value))
                {

                    AccountLevelDropVM Obj = new AccountLevelDropVM
                    {
                        AccountLevel = i,
                        LevelName = Resources.Resource.Level + " " + i
                    };
                    AccountLevel.Add(Obj);
                    i = i + 1;
                }
            }



            int CurrYear = UserInfo.CurrYear;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);

            AccountLevelRepVM ObjAccountLevelRep = new AccountLevelRepVM
            {
                AccountLevelDropVM = AccountLevel,
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency


            };



            return View(ObjAccountLevelRep);
        }
        [Authorize(Roles = "CoOwner,RepEstimatedBudgetForAccountYear")]
        public ActionResult EstimatedBudgetForAccountYear()
        {

            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            //  var GetAllLevel = _unitOfWork.NativeSql.GetAccountLevelVMs(UserInfo.fCompanyId);

            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            String Chart = CoInfo.AccountChart;


            Chart = Chart.Trim(new Char[] { '-', '-', ' ' });

            string[] numbers = Regex.Split(Chart, @"\D+");
            var AccountLevel = new List<AccountLevelDropVM>();
            int i = 1;
            foreach (string value in numbers)
            {
                if (!string.IsNullOrEmpty(value))
                {

                    AccountLevelDropVM Obj = new AccountLevelDropVM
                    {
                        AccountLevel = i,
                        LevelName = Resources.Resource.Level + " " + i
                    };
                    AccountLevel.Add(Obj);
                    i = i + 1;
                }
            }



            int CurrYear = UserInfo.CurrYear;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);

            AccountLevelRepVM ObjAccountLevelRep = new AccountLevelRepVM
            {
                AccountLevelDropVM = AccountLevel,
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency


            };



            return View(ObjAccountLevelRep);
        }
        [Authorize(Roles = "CoOwner,RepEstimatedBudgetForCostCenterYear")]
        public ActionResult EstimatedBudgetForCostCenterYear()
        {

            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            //  var GetAllLevel = _unitOfWork.NativeSql.GetAccountLevelVMs(UserInfo.fCompanyId);

            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            String Chart = CoInfo.AccountChart;


            Chart = Chart.Trim(new Char[] { '-', '-', ' ' });

            string[] numbers = Regex.Split(Chart, @"\D+");
            var AccountLevel = new List<AccountLevelDropVM>();
            int i = 1;
            foreach (string value in numbers)
            {
                if (!string.IsNullOrEmpty(value))
                {

                    AccountLevelDropVM Obj = new AccountLevelDropVM
                    {
                        AccountLevel = i,
                        LevelName = Resources.Resource.Level + " " + i
                    };
                    AccountLevel.Add(Obj);
                    i = i + 1;
                }
            }



            int CurrYear = UserInfo.CurrYear;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);

            AccountLevelRepVM ObjAccountLevelRep = new AccountLevelRepVM
            {
                AccountLevelDropVM = AccountLevel,
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency



            };



            return View(ObjAccountLevelRep);
        }

        [HttpPost]
        public JsonResult GetBalanceSheet(AccountLevelRepVM Obj)
        {
            List<TrialBalanceVM> TrialBalanceVMList = new List<TrialBalanceVM>();

            IEnumerable<ChartOfAccount> Accounts = new List<ChartOfAccount>();

            IEnumerable<ChartOfAccount> LessMainAccount = new List<ChartOfAccount>();
            try
            {

               
                var userId = User.Identity.GetUserId();
                string UserID = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
                var AccountYpe = _unitOfWork.AccountType.GetAllAccountType();

                var AccountData = _unitOfWork.NativeSql.GetChartOfAccountByLevel(UserInfo.fCompanyId);
                int CurrYear = UserInfo.CurrYear;

                var Date = "01/01/" + CurrYear;
                var OpenDate = "31/12/" + (CurrYear - 1).ToString();

                string TempCostID = "0";
                int TempCostType = 0;

                if (Obj.ByCostCenter)
                {
                    TempCostID = Obj.CostCenterNumber;
                    if (Obj.Partofthenumber)
                    {
                        TempCostType = Obj.CostSearchType;
                    }


                }

                IEnumerable<TrialBalanceVM> TotData = new List<TrialBalanceVM>();
                if (Obj.Partofthenumber)
                {
                    if (Obj.CostSearchType == 1)
                    {
                        Obj.CostCenterNumber = Obj.CostCenterNumber + "%";
                    }
                    else if (Obj.CostSearchType == 2)
                    {
                        Obj.CostCenterNumber = "%" + Obj.CostCenterNumber;
                    }
                    else if (Obj.CostSearchType == 3)
                    {
                        Obj.CostCenterNumber = "%" + Obj.CostCenterNumber + "%";

                    }
                }

                var TranData = _unitOfWork.NativeSql.GetTransactionForTrial(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate, Obj.ByCostCenter, Obj.CostCenterNumber, Obj.Partofthenumber, Obj.CostSearchType);
                if (DateTime.Parse(Date) != Obj.FromDate)
                    TotData = _unitOfWork.NativeSql.GetTotCreditDebitForTrial(UserInfo.fCompanyId, DateTime.Parse(Date), Obj.FromDate, Obj.ByCostCenter, Obj.CostCenterNumber, Obj.Partofthenumber, Obj.CostSearchType);
                //----------------Get Open-------------//
                IEnumerable<TrialBalanceVM> TotDataOpen = new List<TrialBalanceVM>();
                TotDataOpen = _unitOfWork.NativeSql.GetTotCreditDebitForTrialOpen(UserInfo.fCompanyId, DateTime.Parse(OpenDate), DateTime.Parse(OpenDate));
                foreach (var D in TotDataOpen)
                {
                    TotData = TotData.Append(D);

                }
                //------------------------------------//

                double TotalDebit = 0;
                double TOTCredit = 0;
                double NetCredit = 0;
                double NetDebit = 0;
                double CreditBalance = 0;
                double DebitBalance = 0;
                double CreditTransAction = 0;
                double DebitTransAction = 0;

                double AssetTotal = 0;
                double Totalliabilities = 0;
                //Get الاصول المتداوله
                List<int> AccType = new List<int> { 1 };
                TrialBalanceVMList.Add(new TrialBalanceVM
                {

                    AccountNumber = "*",
                    Name = "الاصول المتداولة"
                });
                double TotalType=0;
              
                    if (!String.IsNullOrEmpty(Obj.AccNo))
                    {

                        var MainAccount = AccountData.FirstOrDefault(m => m.AccountNumber == Obj.AccNo);
                        var MainChild = AccountData.Where(m => m.AccountFather == Obj.AccNo).OrderBy(m => m.AccountNumber).OrderBy(m => m.AccountLevel).ToList();

                        foreach (var Acc in MainChild)
                        {
                            TotalDebit = 0;
                            TOTCredit = 0;
                            NetCredit = 0;
                            NetDebit = 0;
                            CreditBalance = 0;
                            DebitBalance = 0;
                            CreditTransAction = 0;
                            DebitTransAction = 0;


                            if (AccountData.FirstOrDefault(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel) != null)//Check if Account Had Branches
                            {
                                var AllAccount = AccountData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel)
                                                .OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();//Get All Account Branches
                                int FatherLevel = Acc.AccountLevel;
                                foreach (var CurrAccLevel in AllAccount)
                                {
                                    if (CurrAccLevel.AccountLevel == FatherLevel)
                                    {
                                        break;
                                    }
                                    else
                                    {

                                        var TransActionData = TranData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                        var BalanceData = TotData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                        foreach (var D in TransActionData)
                                        {
                                            CreditTransAction += D.CreditTransAction;
                                            DebitTransAction += D.DebitTransAction;
                                        }
                                        foreach (var Balance in BalanceData)
                                        {
                                            CreditBalance += Balance.CreditBalance;
                                            DebitBalance += Balance.DebitBalance;
                                        }



                                        TotalDebit = DebitTransAction + DebitBalance;
                                        TOTCredit = CreditTransAction + CreditBalance;




                                    }
                                }




                            }
                            else
                            {
                                var TransActionData = TranData.Where(m => m.AccountNumber == Acc.AccountNumber);
                                var BalanceData = TotData.Where(m => m.AccountNumber == Acc.AccountNumber);
                                foreach (var D in TransActionData)
                                {
                                    CreditTransAction += D.CreditTransAction;
                                    DebitTransAction += D.DebitTransAction;
                                }
                                foreach (var Balance in BalanceData)
                                {
                                    CreditBalance += Balance.CreditBalance;
                                    DebitBalance += Balance.DebitBalance;
                                }



                                TotalDebit = DebitTransAction + DebitBalance;
                                TOTCredit = CreditTransAction + CreditBalance;
                            }
                            if (TotalDebit > TOTCredit)
                            {
                                NetDebit = TotalDebit - TOTCredit;
                            }
                            if (TotalDebit < TOTCredit)
                            {
                                NetCredit = TOTCredit - TotalDebit;
                            }

                            TrialBalanceVM trialBalance = new TrialBalanceVM();
                            trialBalance.AccountNumber = Acc.AccountNumber;
                            trialBalance.Name = Acc.ArabicName;

                            trialBalance.CreditBalance = CreditBalance;
                            trialBalance.CreditTransAction = CreditTransAction;
                            trialBalance.DebitBalance = DebitBalance;
                            trialBalance.DebitTransAction = DebitTransAction;
                            trialBalance.NetCredit = NetCredit;
                            trialBalance.NetDebit = NetDebit;
                            trialBalance.NetTot = NetDebit - NetCredit;
                            TotalType+= NetDebit - NetCredit;
                            trialBalance.TempCostID = TempCostID;
                            trialBalance.TempCostType = TempCostType;

                            if (!Obj.ShowZeroBalance)
                            {
                                if ((trialBalance.NetCredit - trialBalance.NetDebit) != 0)
                                {
                                    TrialBalanceVMList.Add(trialBalance);
                                }
                            }
                            else
                            {
                                if ((trialBalance.NetCredit != 0) || (trialBalance.NetDebit != 0) || (TotalDebit != 0) || (NetCredit != 0))
                                {
                                    TrialBalanceVMList.Add(trialBalance);
                                }


                            }
                        }


                    }
                    else
                    //if ((Obj.AccountLevelDropVMID > 0))
                    {


                        Accounts = AccountData.Where(m => m.AccountLevel == Obj.AccountLevelDropVMID ).OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();

                        LessMainAccount = AccountData.Where(m => m.AccountLevel < Obj.AccountLevelDropVMID ).OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();

                        IEnumerable<string> FaherAccount = LessMainAccount.Where(m => m.AccountFather != "0" ).Select(m => m.AccountFather);


                        foreach (var A in LessMainAccount)
                        {
                            if (AccountData.FirstOrDefault(m => m.AccountFather == A.AccountNumber ) != null)
                            {
                                //do nothing
                            }
                            else

                            {
                                Accounts = Accounts.Append(A);
                            }

                        }
                        Accounts = Accounts.OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();
                        // var MainChild = AccountData.Where(m => m.AccountFather == MainAcc.AccountNumber).OrderBy(m => m.AccountNumber).OrderBy(m => m.AccountLevel).ToList();
                       // foreach(var t in AccType)
                  //  {
                        foreach (var Acc in Accounts)
                        {
                            TotalDebit = 0;
                            TOTCredit = 0;
                            NetCredit = 0;
                            NetDebit = 0;
                            CreditBalance = 0;
                            DebitBalance = 0;
                            CreditTransAction = 0;
                            DebitTransAction = 0;


                            if (AccountData.FirstOrDefault(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel ) != null)
                            {
                                var AllAccount = AccountData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel )
                                                .OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();
                                int FatherLevel = Acc.AccountLevel;

                                foreach (var CurrAccLevel in AllAccount)
                                {
                                    if (CurrAccLevel.AccountLevel == FatherLevel)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        var TransActionData = TranData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                        var BalanceData = TotData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                        foreach (var D in TransActionData)
                                        {
                                            CreditTransAction += D.CreditTransAction;
                                            DebitTransAction += D.DebitTransAction;
                                        }
                                        foreach (var Balance in BalanceData)
                                        {
                                            CreditBalance += Balance.CreditBalance;
                                            DebitBalance += Balance.DebitBalance;
                                        }
                                        TotalDebit = DebitTransAction + DebitBalance;
                                        TOTCredit = CreditTransAction + CreditBalance;
                                    }
                                }
                            }
                            else
                            {
                                var TransActionData = TranData.Where(m => m.AccountNumber == Acc.AccountNumber);
                                var BalanceData = TotData.Where(m => m.AccountNumber == Acc.AccountNumber);
                                foreach (var D in TransActionData)
                                {
                                    CreditTransAction += D.CreditTransAction;
                                    DebitTransAction += D.DebitTransAction;
                                }
                                foreach (var Balance in BalanceData)
                                {
                                    CreditBalance += Balance.CreditBalance;
                                    DebitBalance += Balance.DebitBalance;
                                }

                                TotalDebit = DebitTransAction + DebitBalance;
                                TOTCredit = CreditTransAction + CreditBalance;
                            }
                            if (TotalDebit > TOTCredit)
                            {
                                NetDebit = TotalDebit - TOTCredit;
                            }
                            if (TotalDebit < TOTCredit)
                            {
                                NetCredit = TOTCredit - TotalDebit;
                            }
                            if(Acc.AccountTypeID==1  )
                            {
                                TrialBalanceVM trialBalance = new TrialBalanceVM();
                                trialBalance.AccountNumber = Acc.AccountNumber;
                                trialBalance.Name = Acc.ArabicName;
                                trialBalance.Level = Acc.AccountLevel;
                                trialBalance.CreditBalance = CreditBalance;
                                trialBalance.CreditTransAction = CreditTransAction;
                                trialBalance.DebitBalance = DebitBalance;
                                trialBalance.DebitTransAction = DebitTransAction;
                                trialBalance.NetCredit = NetCredit;
                                trialBalance.NetDebit = NetDebit;

                                trialBalance.nCreditBalance = CreditBalance;
                                trialBalance.nCreditTransAction = CreditTransAction;
                                trialBalance.nDebitBalance = DebitBalance;
                                trialBalance.nDebitTransAction = DebitTransAction;
                                trialBalance.nNetCredit = NetCredit;
                                trialBalance.nNetDebit = NetDebit;
                                trialBalance.TempCostID = TempCostID;
                                trialBalance.TempCostType = TempCostType;
                                trialBalance.NetTot = NetDebit - NetCredit;
                                TotalType += NetDebit - NetCredit;
                                if (AccountData.FirstOrDefault(m => m.AccountFather == Acc.AccountNumber ) != null)
                                {
                                    trialBalance.IsMainAccount = 1;

                                }
                                else
                                {
                                    trialBalance.IsMainAccount = 0;


                                }

                                if (!Obj.ShowZeroBalance)
                                {
                                    if ((trialBalance.NetCredit - trialBalance.NetDebit) != 0)
                                    {
                                        TrialBalanceVMList.Add(trialBalance);
                                    }
                                }
                                else
                                {
                                    if ((trialBalance.NetCredit != 0) || (trialBalance.NetDebit != 0) || (TotalDebit != 0) || (NetCredit != 0))
                                    {
                                        TrialBalanceVMList.Add(trialBalance);
                                    }


                                }
                            }



                        }
                  //  }
                       

                    }

                
                TrialBalanceVMList.Add(new TrialBalanceVM
                {

                    AccountNumber = "",
                    Name = "بضاعة اخر المدة",
                    NetTot = Obj.GoodsEnd,

                });
                TotalType += Obj.GoodsEnd;
                TrialBalanceVMList.Add(new TrialBalanceVM
                {

                    AccountNumber = "**",
                    Name = "مجموع الاصول المتداولة",
                    NetTot = TotalType,
                     
                }) ;
                AssetTotal += TotalType;
                //----------//
                AccType = new List<int> { 3 };
                TrialBalanceVMList.Add(new TrialBalanceVM
                {

                    AccountNumber = "*",
                    Name = "الموجودات الثابتة"
                });
                TotalType = 0;
                if (!String.IsNullOrEmpty(Obj.AccNo))
                {

                    var MainAccount = AccountData.FirstOrDefault(m => m.AccountNumber == Obj.AccNo);
                    var MainChild = AccountData.Where(m => m.AccountFather == Obj.AccNo).OrderBy(m => m.AccountNumber).OrderBy(m => m.AccountLevel).ToList();

                    foreach (var Acc in MainChild)
                    {
                        TotalDebit = 0;
                        TOTCredit = 0;
                        NetCredit = 0;
                        NetDebit = 0;
                        CreditBalance = 0;
                        DebitBalance = 0;
                        CreditTransAction = 0;
                        DebitTransAction = 0;


                        if (AccountData.FirstOrDefault(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel) != null)//Check if Account Had Branches
                        {
                            var AllAccount = AccountData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel)
                                            .OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();//Get All Account Branches
                            int FatherLevel = Acc.AccountLevel;
                            foreach (var CurrAccLevel in AllAccount)
                            {
                                if (CurrAccLevel.AccountLevel == FatherLevel)
                                {
                                    break;
                                }
                                else
                                {

                                    var TransActionData = TranData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                    var BalanceData = TotData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                    foreach (var D in TransActionData)
                                    {
                                        CreditTransAction += D.CreditTransAction;
                                        DebitTransAction += D.DebitTransAction;
                                    }
                                    foreach (var Balance in BalanceData)
                                    {
                                        CreditBalance += Balance.CreditBalance;
                                        DebitBalance += Balance.DebitBalance;
                                    }



                                    TotalDebit = DebitTransAction + DebitBalance;
                                    TOTCredit = CreditTransAction + CreditBalance;




                                }
                            }




                        }
                        else
                        {
                            var TransActionData = TranData.Where(m => m.AccountNumber == Acc.AccountNumber);
                            var BalanceData = TotData.Where(m => m.AccountNumber == Acc.AccountNumber);
                            foreach (var D in TransActionData)
                            {
                                CreditTransAction += D.CreditTransAction;
                                DebitTransAction += D.DebitTransAction;
                            }
                            foreach (var Balance in BalanceData)
                            {
                                CreditBalance += Balance.CreditBalance;
                                DebitBalance += Balance.DebitBalance;
                            }



                            TotalDebit = DebitTransAction + DebitBalance;
                            TOTCredit = CreditTransAction + CreditBalance;
                        }
                        if (TotalDebit > TOTCredit)
                        {
                            NetDebit = TotalDebit - TOTCredit;
                        }
                        if (TotalDebit < TOTCredit)
                        {
                            NetCredit = TOTCredit - TotalDebit;
                        }

                        TrialBalanceVM trialBalance = new TrialBalanceVM();
                        trialBalance.AccountNumber = Acc.AccountNumber;
                        trialBalance.Name = Acc.ArabicName;

                        trialBalance.CreditBalance = CreditBalance;
                        trialBalance.CreditTransAction = CreditTransAction;
                        trialBalance.DebitBalance = DebitBalance;
                        trialBalance.DebitTransAction = DebitTransAction;
                        trialBalance.NetCredit = NetCredit;
                        trialBalance.NetDebit = NetDebit;
                        trialBalance.NetTot = NetDebit - NetCredit;
                        TotalType += NetDebit - NetCredit;
                        trialBalance.TempCostID = TempCostID;
                        trialBalance.TempCostType = TempCostType;

                        if (!Obj.ShowZeroBalance)
                        {
                            if ((trialBalance.NetCredit - trialBalance.NetDebit) != 0)
                            {
                                TrialBalanceVMList.Add(trialBalance);
                            }
                        }
                        else
                        {
                            if ((trialBalance.NetCredit != 0) || (trialBalance.NetDebit != 0) || (TotalDebit != 0) || (NetCredit != 0))
                            {
                                TrialBalanceVMList.Add(trialBalance);
                            }


                        }
                    }


                }
                else
                //if ((Obj.AccountLevelDropVMID > 0))
                {


                    Accounts = AccountData.Where(m => m.AccountLevel == Obj.AccountLevelDropVMID).OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();

                    LessMainAccount = AccountData.Where(m => m.AccountLevel < Obj.AccountLevelDropVMID).OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();

                    IEnumerable<string> FaherAccount = LessMainAccount.Where(m => m.AccountFather != "0").Select(m => m.AccountFather);


                    foreach (var A in LessMainAccount)
                    {
                        if (AccountData.FirstOrDefault(m => m.AccountFather == A.AccountNumber) != null)
                        {
                            //do nothing
                        }
                        else

                        {
                            Accounts = Accounts.Append(A);
                        }

                    }
                    Accounts = Accounts.OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();
                    // var MainChild = AccountData.Where(m => m.AccountFather == MainAcc.AccountNumber).OrderBy(m => m.AccountNumber).OrderBy(m => m.AccountLevel).ToList();
                    // foreach(var t in AccType)
                    //  {
                    foreach (var Acc in Accounts)
                    {
                        TotalDebit = 0;
                        TOTCredit = 0;
                        NetCredit = 0;
                        NetDebit = 0;
                        CreditBalance = 0;
                        DebitBalance = 0;
                        CreditTransAction = 0;
                        DebitTransAction = 0;


                        if (AccountData.FirstOrDefault(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel) != null)
                        {
                            var AllAccount = AccountData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel)
                                            .OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();
                            int FatherLevel = Acc.AccountLevel;

                            foreach (var CurrAccLevel in AllAccount)
                            {
                                if (CurrAccLevel.AccountLevel == FatherLevel)
                                {
                                    break;
                                }
                                else
                                {
                                    var TransActionData = TranData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                    var BalanceData = TotData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                    foreach (var D in TransActionData)
                                    {
                                        CreditTransAction += D.CreditTransAction;
                                        DebitTransAction += D.DebitTransAction;
                                    }
                                    foreach (var Balance in BalanceData)
                                    {
                                        CreditBalance += Balance.CreditBalance;
                                        DebitBalance += Balance.DebitBalance;
                                    }
                                    TotalDebit = DebitTransAction + DebitBalance;
                                    TOTCredit = CreditTransAction + CreditBalance;
                                }
                            }
                        }
                        else
                        {
                            var TransActionData = TranData.Where(m => m.AccountNumber == Acc.AccountNumber);
                            var BalanceData = TotData.Where(m => m.AccountNumber == Acc.AccountNumber);
                            foreach (var D in TransActionData)
                            {
                                CreditTransAction += D.CreditTransAction;
                                DebitTransAction += D.DebitTransAction;
                            }
                            foreach (var Balance in BalanceData)
                            {
                                CreditBalance += Balance.CreditBalance;
                                DebitBalance += Balance.DebitBalance;
                            }

                            TotalDebit = DebitTransAction + DebitBalance;
                            TOTCredit = CreditTransAction + CreditBalance;
                        }
                        if (TotalDebit > TOTCredit)
                        {
                            NetDebit = TotalDebit - TOTCredit;
                        }
                        if (TotalDebit < TOTCredit)
                        {
                            NetCredit = TOTCredit - TotalDebit;
                        }
                        if (Acc.AccountTypeID == 3 )
                        {
                            TrialBalanceVM trialBalance = new TrialBalanceVM();
                            trialBalance.AccountNumber = Acc.AccountNumber;
                            trialBalance.Name = Acc.ArabicName;
                            trialBalance.Level = Acc.AccountLevel;
                            trialBalance.CreditBalance = CreditBalance;
                            trialBalance.CreditTransAction = CreditTransAction;
                            trialBalance.DebitBalance = DebitBalance;
                            trialBalance.DebitTransAction = DebitTransAction;
                            trialBalance.NetCredit = NetCredit;
                            trialBalance.NetDebit = NetDebit;

                            trialBalance.nCreditBalance = CreditBalance;
                            trialBalance.nCreditTransAction = CreditTransAction;
                            trialBalance.nDebitBalance = DebitBalance;
                            trialBalance.nDebitTransAction = DebitTransAction;
                            trialBalance.nNetCredit = NetCredit;
                            trialBalance.nNetDebit = NetDebit;
                            trialBalance.TempCostID = TempCostID;
                            trialBalance.TempCostType = TempCostType;
                            trialBalance.NetTot = NetDebit - NetCredit;
                            TotalType += NetDebit - NetCredit;
                            if (AccountData.FirstOrDefault(m => m.AccountFather == Acc.AccountNumber) != null)
                            {
                                trialBalance.IsMainAccount = 1;

                            }
                            else
                            {
                                trialBalance.IsMainAccount = 0;


                            }

                            if (!Obj.ShowZeroBalance)
                            {
                                if ((trialBalance.NetCredit - trialBalance.NetDebit) != 0)
                                {
                                    TrialBalanceVMList.Add(trialBalance);
                                }
                            }
                            else
                            {
                                if ((trialBalance.NetCredit != 0) || (trialBalance.NetDebit != 0) || (TotalDebit != 0) || (NetCredit != 0))
                                {
                                    TrialBalanceVMList.Add(trialBalance);
                                }


                            }
                        }



                    }
                    //  }


                }
                TrialBalanceVMList.Add(new TrialBalanceVM
                {

                    AccountNumber = "**",
                    Name = "صافي القيمة الدفترية",
                    NetTot = TotalType,

                });
                AssetTotal += TotalType;

                //----------//
                AccType = new List<int> { 2 };
                TrialBalanceVMList.Add(new TrialBalanceVM
                {

                    AccountNumber = "*",
                    Name = "الاصول الاخري"
                });
                TotalType = 0;
                if (!String.IsNullOrEmpty(Obj.AccNo))
                {

                    var MainAccount = AccountData.FirstOrDefault(m => m.AccountNumber == Obj.AccNo);
                    var MainChild = AccountData.Where(m => m.AccountFather == Obj.AccNo).OrderBy(m => m.AccountNumber).OrderBy(m => m.AccountLevel).ToList();

                    foreach (var Acc in MainChild)
                    {
                        TotalDebit = 0;
                        TOTCredit = 0;
                        NetCredit = 0;
                        NetDebit = 0;
                        CreditBalance = 0;
                        DebitBalance = 0;
                        CreditTransAction = 0;
                        DebitTransAction = 0;


                        if (AccountData.FirstOrDefault(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel) != null)//Check if Account Had Branches
                        {
                            var AllAccount = AccountData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel)
                                            .OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();//Get All Account Branches
                            int FatherLevel = Acc.AccountLevel;
                            foreach (var CurrAccLevel in AllAccount)
                            {
                                if (CurrAccLevel.AccountLevel == FatherLevel)
                                {
                                    break;
                                }
                                else
                                {

                                    var TransActionData = TranData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                    var BalanceData = TotData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                    foreach (var D in TransActionData)
                                    {
                                        CreditTransAction += D.CreditTransAction;
                                        DebitTransAction += D.DebitTransAction;
                                    }
                                    foreach (var Balance in BalanceData)
                                    {
                                        CreditBalance += Balance.CreditBalance;
                                        DebitBalance += Balance.DebitBalance;
                                    }



                                    TotalDebit = DebitTransAction + DebitBalance;
                                    TOTCredit = CreditTransAction + CreditBalance;




                                }
                            }




                        }
                        else
                        {
                            var TransActionData = TranData.Where(m => m.AccountNumber == Acc.AccountNumber);
                            var BalanceData = TotData.Where(m => m.AccountNumber == Acc.AccountNumber);
                            foreach (var D in TransActionData)
                            {
                                CreditTransAction += D.CreditTransAction;
                                DebitTransAction += D.DebitTransAction;
                            }
                            foreach (var Balance in BalanceData)
                            {
                                CreditBalance += Balance.CreditBalance;
                                DebitBalance += Balance.DebitBalance;
                            }



                            TotalDebit = DebitTransAction + DebitBalance;
                            TOTCredit = CreditTransAction + CreditBalance;
                        }
                        if (TotalDebit > TOTCredit)
                        {
                            NetDebit = TotalDebit - TOTCredit;
                        }
                        if (TotalDebit < TOTCredit)
                        {
                            NetCredit = TOTCredit - TotalDebit;
                        }

                        TrialBalanceVM trialBalance = new TrialBalanceVM();
                        trialBalance.AccountNumber = Acc.AccountNumber;
                        trialBalance.Name = Acc.ArabicName;

                        trialBalance.CreditBalance = CreditBalance;
                        trialBalance.CreditTransAction = CreditTransAction;
                        trialBalance.DebitBalance = DebitBalance;
                        trialBalance.DebitTransAction = DebitTransAction;
                        trialBalance.NetCredit = NetCredit;
                        trialBalance.NetDebit = NetDebit;
                        trialBalance.NetTot = NetDebit - NetCredit;
                        TotalType += NetDebit - NetCredit;
                        trialBalance.TempCostID = TempCostID;
                        trialBalance.TempCostType = TempCostType;

                        if (!Obj.ShowZeroBalance)
                        {
                            if ((trialBalance.NetCredit - trialBalance.NetDebit) != 0)
                            {
                                TrialBalanceVMList.Add(trialBalance);
                            }
                        }
                        else
                        {
                            if ((trialBalance.NetCredit != 0) || (trialBalance.NetDebit != 0) || (TotalDebit != 0) || (NetCredit != 0))
                            {
                                TrialBalanceVMList.Add(trialBalance);
                            }


                        }
                    }


                }
                else
                //if ((Obj.AccountLevelDropVMID > 0))
                {


                    Accounts = AccountData.Where(m => m.AccountLevel == Obj.AccountLevelDropVMID).OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();

                    LessMainAccount = AccountData.Where(m => m.AccountLevel < Obj.AccountLevelDropVMID).OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();

                    IEnumerable<string> FaherAccount = LessMainAccount.Where(m => m.AccountFather != "0").Select(m => m.AccountFather);


                    foreach (var A in LessMainAccount)
                    {
                        if (AccountData.FirstOrDefault(m => m.AccountFather == A.AccountNumber) != null)
                        {
                            //do nothing
                        }
                        else

                        {
                            Accounts = Accounts.Append(A);
                        }

                    }
                    Accounts = Accounts.OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();
                    // var MainChild = AccountData.Where(m => m.AccountFather == MainAcc.AccountNumber).OrderBy(m => m.AccountNumber).OrderBy(m => m.AccountLevel).ToList();
                    // foreach(var t in AccType)
                    //  {
                    foreach (var Acc in Accounts)
                    {
                        TotalDebit = 0;
                        TOTCredit = 0;
                        NetCredit = 0;
                        NetDebit = 0;
                        CreditBalance = 0;
                        DebitBalance = 0;
                        CreditTransAction = 0;
                        DebitTransAction = 0;


                        if (AccountData.FirstOrDefault(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel) != null)
                        {
                            var AllAccount = AccountData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel)
                                            .OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();
                            int FatherLevel = Acc.AccountLevel;

                            foreach (var CurrAccLevel in AllAccount)
                            {
                                if (CurrAccLevel.AccountLevel == FatherLevel)
                                {
                                    break;
                                }
                                else
                                {
                                    var TransActionData = TranData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                    var BalanceData = TotData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                    foreach (var D in TransActionData)
                                    {
                                        CreditTransAction += D.CreditTransAction;
                                        DebitTransAction += D.DebitTransAction;
                                    }
                                    foreach (var Balance in BalanceData)
                                    {
                                        CreditBalance += Balance.CreditBalance;
                                        DebitBalance += Balance.DebitBalance;
                                    }
                                    TotalDebit = DebitTransAction + DebitBalance;
                                    TOTCredit = CreditTransAction + CreditBalance;
                                }
                            }
                        }
                        else
                        {
                            var TransActionData = TranData.Where(m => m.AccountNumber == Acc.AccountNumber);
                            var BalanceData = TotData.Where(m => m.AccountNumber == Acc.AccountNumber);
                            foreach (var D in TransActionData)
                            {
                                CreditTransAction += D.CreditTransAction;
                                DebitTransAction += D.DebitTransAction;
                            }
                            foreach (var Balance in BalanceData)
                            {
                                CreditBalance += Balance.CreditBalance;
                                DebitBalance += Balance.DebitBalance;
                            }

                            TotalDebit = DebitTransAction + DebitBalance;
                            TOTCredit = CreditTransAction + CreditBalance;
                        }
                        if (TotalDebit > TOTCredit)
                        {
                            NetDebit = TotalDebit - TOTCredit;
                        }
                        if (TotalDebit < TOTCredit)
                        {
                            NetCredit = TOTCredit - TotalDebit;
                        }
                        if (Acc.AccountTypeID == 2)
                        {
                            TrialBalanceVM trialBalance = new TrialBalanceVM();
                            trialBalance.AccountNumber = Acc.AccountNumber;
                            trialBalance.Name = Acc.ArabicName;
                            trialBalance.Level = Acc.AccountLevel;
                            trialBalance.CreditBalance = CreditBalance;
                            trialBalance.CreditTransAction = CreditTransAction;
                            trialBalance.DebitBalance = DebitBalance;
                            trialBalance.DebitTransAction = DebitTransAction;
                            trialBalance.NetCredit = NetCredit;
                            trialBalance.NetDebit = NetDebit;

                            trialBalance.nCreditBalance = CreditBalance;
                            trialBalance.nCreditTransAction = CreditTransAction;
                            trialBalance.nDebitBalance = DebitBalance;
                            trialBalance.nDebitTransAction = DebitTransAction;
                            trialBalance.nNetCredit = NetCredit;
                            trialBalance.nNetDebit = NetDebit;
                            trialBalance.TempCostID = TempCostID;
                            trialBalance.TempCostType = TempCostType;
                            trialBalance.NetTot = NetDebit - NetCredit;
                            TotalType += NetDebit - NetCredit;
                            if (AccountData.FirstOrDefault(m => m.AccountFather == Acc.AccountNumber) != null)
                            {
                                trialBalance.IsMainAccount = 1;

                            }
                            else
                            {
                                trialBalance.IsMainAccount = 0;


                            }

                            if (!Obj.ShowZeroBalance)
                            {
                                if ((trialBalance.NetCredit - trialBalance.NetDebit) != 0)
                                {
                                    TrialBalanceVMList.Add(trialBalance);
                                }
                            }
                            else
                            {
                                if ((trialBalance.NetCredit != 0) || (trialBalance.NetDebit != 0) || (TotalDebit != 0) || (NetCredit != 0))
                                {
                                    TrialBalanceVMList.Add(trialBalance);
                                }


                            }
                        }



                    }
                    //  }


                }
                TrialBalanceVMList.Add(new TrialBalanceVM
                {

                    AccountNumber = "**",
                    Name = "مجموع الاصول الاخرى",
                    NetTot = TotalType,

                });
                AssetTotal += TotalType;
                TrialBalanceVMList.Add(new TrialBalanceVM
                {

                    AccountNumber = "**",
                    Name = "مجموع الاصول",
                    NetTot = AssetTotal,

                });

//--//
                AccType = new List<int> { 4};
                TrialBalanceVMList.Add(new TrialBalanceVM
                {

                    AccountNumber = "*",
                    Name = "المطلوبات المتداولة"
                });
                TotalType = 0;
                foreach (var t in AccType)
                {


                    if (!String.IsNullOrEmpty(Obj.AccNo))
                    {

                        var MainAccount = AccountData.FirstOrDefault(m => m.AccountNumber == Obj.AccNo);
                        var MainChild = AccountData.Where(m => m.AccountFather == Obj.AccNo).OrderBy(m => m.AccountNumber).OrderBy(m => m.AccountLevel).ToList();

                        foreach (var Acc in MainChild)
                        {
                            TotalDebit = 0;
                            TOTCredit = 0;
                            NetCredit = 0;
                            NetDebit = 0;
                            CreditBalance = 0;
                            DebitBalance = 0;
                            CreditTransAction = 0;
                            DebitTransAction = 0;


                            if (AccountData.FirstOrDefault(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel) != null)//Check if Account Had Branches
                            {
                                var AllAccount = AccountData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel)
                                                .OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();//Get All Account Branches
                                int FatherLevel = Acc.AccountLevel;
                                foreach (var CurrAccLevel in AllAccount)
                                {
                                    if (CurrAccLevel.AccountLevel == FatherLevel)
                                    {
                                        break;
                                    }
                                    else
                                    {

                                        var TransActionData = TranData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                        var BalanceData = TotData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                        foreach (var D in TransActionData)
                                        {
                                            CreditTransAction += D.CreditTransAction;
                                            DebitTransAction += D.DebitTransAction;
                                        }
                                        foreach (var Balance in BalanceData)
                                        {
                                            CreditBalance += Balance.CreditBalance;
                                            DebitBalance += Balance.DebitBalance;
                                        }



                                        TotalDebit = DebitTransAction + DebitBalance;
                                        TOTCredit = CreditTransAction + CreditBalance;




                                    }
                                }




                            }
                            else
                            {
                                var TransActionData = TranData.Where(m => m.AccountNumber == Acc.AccountNumber);
                                var BalanceData = TotData.Where(m => m.AccountNumber == Acc.AccountNumber);
                                foreach (var D in TransActionData)
                                {
                                    CreditTransAction += D.CreditTransAction;
                                    DebitTransAction += D.DebitTransAction;
                                }
                                foreach (var Balance in BalanceData)
                                {
                                    CreditBalance += Balance.CreditBalance;
                                    DebitBalance += Balance.DebitBalance;
                                }



                                TotalDebit = DebitTransAction + DebitBalance;
                                TOTCredit = CreditTransAction + CreditBalance;
                            }
                            if (TotalDebit > TOTCredit)
                            {
                                NetDebit = TotalDebit - TOTCredit;
                            }
                            if (TotalDebit < TOTCredit)
                            {
                                NetCredit = TOTCredit - TotalDebit;
                            }

                            TrialBalanceVM trialBalance = new TrialBalanceVM();
                            trialBalance.AccountNumber = Acc.AccountNumber;
                            trialBalance.Name = Acc.ArabicName;

                            trialBalance.CreditBalance = CreditBalance;
                            trialBalance.CreditTransAction = CreditTransAction;
                            trialBalance.DebitBalance = DebitBalance;
                            trialBalance.DebitTransAction = DebitTransAction;
                            trialBalance.NetCredit = NetCredit;
                            trialBalance.NetDebit = NetDebit;
                            trialBalance.NetTot = NetDebit - NetCredit;
                            TotalType += NetDebit - NetCredit;
                            trialBalance.TempCostID = TempCostID;
                            trialBalance.TempCostType = TempCostType;

                            if (!Obj.ShowZeroBalance)
                            {
                                if ((trialBalance.NetCredit - trialBalance.NetDebit) != 0)
                                {
                                    TrialBalanceVMList.Add(trialBalance);
                                }
                            }
                            else
                            {
                                if ((trialBalance.NetCredit != 0) || (trialBalance.NetDebit != 0) || (TotalDebit != 0) || (NetCredit != 0))
                                {
                                    TrialBalanceVMList.Add(trialBalance);
                                }


                            }
                        }


                    }
                    else
                    if ((Obj.AccountLevelDropVMID > 0))
                    {


                        Accounts = AccountData.Where(m => m.AccountLevel == Obj.AccountLevelDropVMID && m.AccountTypeID == t).OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();

                        LessMainAccount = AccountData.Where(m => m.AccountLevel < Obj.AccountLevelDropVMID && m.AccountTypeID == t).OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();

                        IEnumerable<string> FaherAccount = LessMainAccount.Where(m => m.AccountFather != "0" && m.AccountTypeID == t).Select(m => m.AccountFather);


                        foreach (var A in LessMainAccount)
                        {
                            if (AccountData.FirstOrDefault(m => m.AccountFather == A.AccountNumber) != null)
                            {
                                //do nothing
                            }
                            else

                            {
                                Accounts = Accounts.Append(A);
                            }

                        }
                        Accounts = Accounts.OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();
                        // var MainChild = AccountData.Where(m => m.AccountFather == MainAcc.AccountNumber).OrderBy(m => m.AccountNumber).OrderBy(m => m.AccountLevel).ToList();
                        foreach (var Acc in Accounts)
                        {
                            TotalDebit = 0;
                            TOTCredit = 0;
                            NetCredit = 0;
                            NetDebit = 0;
                            CreditBalance = 0;
                            DebitBalance = 0;
                            CreditTransAction = 0;
                            DebitTransAction = 0;


                            if (AccountData.FirstOrDefault(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel) != null)
                            {
                                var AllAccount = AccountData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel && m.AccountTypeID == t)
                                                .OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();
                                int FatherLevel = Acc.AccountLevel;

                                foreach (var CurrAccLevel in AllAccount)
                                {
                                    if (CurrAccLevel.AccountLevel == FatherLevel)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        var TransActionData = TranData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                        var BalanceData = TotData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                        foreach (var D in TransActionData)
                                        {
                                            CreditTransAction += D.CreditTransAction;
                                            DebitTransAction += D.DebitTransAction;
                                        }
                                        foreach (var Balance in BalanceData)
                                        {
                                            CreditBalance += Balance.CreditBalance;
                                            DebitBalance += Balance.DebitBalance;
                                        }
                                        TotalDebit = DebitTransAction + DebitBalance;
                                        TOTCredit = CreditTransAction + CreditBalance;
                                    }
                                }
                            }
                            else
                            {
                                var TransActionData = TranData.Where(m => m.AccountNumber == Acc.AccountNumber);
                                var BalanceData = TotData.Where(m => m.AccountNumber == Acc.AccountNumber);
                                foreach (var D in TransActionData)
                                {
                                    CreditTransAction += D.CreditTransAction;
                                    DebitTransAction += D.DebitTransAction;
                                }
                                foreach (var Balance in BalanceData)
                                {
                                    CreditBalance += Balance.CreditBalance;
                                    DebitBalance += Balance.DebitBalance;
                                }

                                TotalDebit = DebitTransAction + DebitBalance;
                                TOTCredit = CreditTransAction + CreditBalance;
                            }
                            if (TotalDebit > TOTCredit)
                            {
                                NetDebit = TotalDebit - TOTCredit;
                            }
                            if (TotalDebit < TOTCredit)
                            {
                                NetCredit = TOTCredit - TotalDebit;
                            }

                            TrialBalanceVM trialBalance = new TrialBalanceVM();
                            trialBalance.AccountNumber = Acc.AccountNumber;
                            trialBalance.Name = Acc.ArabicName;
                            trialBalance.Level = Acc.AccountLevel;
                            trialBalance.CreditBalance = CreditBalance;
                            trialBalance.CreditTransAction = CreditTransAction;
                            trialBalance.DebitBalance = DebitBalance;
                            trialBalance.DebitTransAction = DebitTransAction;
                            trialBalance.NetCredit = NetCredit;
                            trialBalance.NetDebit = NetDebit;

                            trialBalance.nCreditBalance = CreditBalance;
                            trialBalance.nCreditTransAction = CreditTransAction;
                            trialBalance.nDebitBalance = DebitBalance;
                            trialBalance.nDebitTransAction = DebitTransAction;
                            trialBalance.nNetCredit = NetCredit;
                            trialBalance.nNetDebit = NetDebit;
                            trialBalance.TempCostID = TempCostID;
                            trialBalance.TempCostType = TempCostType;
                            trialBalance.NetTot = NetDebit - NetCredit;
                            TotalType += NetDebit - NetCredit;
                            if (AccountData.FirstOrDefault(m => m.AccountFather == Acc.AccountNumber && m.AccountTypeID == t) != null)
                            {
                                trialBalance.IsMainAccount = 1;

                            }
                            else
                            {
                                trialBalance.IsMainAccount = 0;


                            }

                            if (!Obj.ShowZeroBalance)
                            {
                                if ((trialBalance.NetCredit - trialBalance.NetDebit) != 0)
                                {
                                    TrialBalanceVMList.Add(trialBalance);
                                }
                            }
                            else
                            {
                                if ((trialBalance.NetCredit != 0) || (trialBalance.NetDebit != 0) || (TotalDebit != 0) || (NetCredit != 0))
                                {
                                    TrialBalanceVMList.Add(trialBalance);
                                }


                            }
                        }

                    }

                }
                TrialBalanceVMList.Add(new TrialBalanceVM
                {

                    AccountNumber = "**",
                    Name = "مجموع المطلوبات المتداولة",
                    NetTot = TotalType,

                });
                Totalliabilities += TotalType;

                //--//
                AccType = new List<int> { 5 };
                TrialBalanceVMList.Add(new TrialBalanceVM
                {

                    AccountNumber = "*",
                    Name = "مطلوبات طويلة الاجل	"
                });
                TotalType = 0;
                foreach (var t in AccType)
                {


                    if (!String.IsNullOrEmpty(Obj.AccNo))
                    {

                        var MainAccount = AccountData.FirstOrDefault(m => m.AccountNumber == Obj.AccNo);
                        var MainChild = AccountData.Where(m => m.AccountFather == Obj.AccNo).OrderBy(m => m.AccountNumber).OrderBy(m => m.AccountLevel).ToList();

                        foreach (var Acc in MainChild)
                        {
                            TotalDebit = 0;
                            TOTCredit = 0;
                            NetCredit = 0;
                            NetDebit = 0;
                            CreditBalance = 0;
                            DebitBalance = 0;
                            CreditTransAction = 0;
                            DebitTransAction = 0;


                            if (AccountData.FirstOrDefault(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel) != null)//Check if Account Had Branches
                            {
                                var AllAccount = AccountData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel)
                                                .OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();//Get All Account Branches
                                int FatherLevel = Acc.AccountLevel;
                                foreach (var CurrAccLevel in AllAccount)
                                {
                                    if (CurrAccLevel.AccountLevel == FatherLevel)
                                    {
                                        break;
                                    }
                                    else
                                    {

                                        var TransActionData = TranData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                        var BalanceData = TotData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                        foreach (var D in TransActionData)
                                        {
                                            CreditTransAction += D.CreditTransAction;
                                            DebitTransAction += D.DebitTransAction;
                                        }
                                        foreach (var Balance in BalanceData)
                                        {
                                            CreditBalance += Balance.CreditBalance;
                                            DebitBalance += Balance.DebitBalance;
                                        }



                                        TotalDebit = DebitTransAction + DebitBalance;
                                        TOTCredit = CreditTransAction + CreditBalance;




                                    }
                                }




                            }
                            else
                            {
                                var TransActionData = TranData.Where(m => m.AccountNumber == Acc.AccountNumber);
                                var BalanceData = TotData.Where(m => m.AccountNumber == Acc.AccountNumber);
                                foreach (var D in TransActionData)
                                {
                                    CreditTransAction += D.CreditTransAction;
                                    DebitTransAction += D.DebitTransAction;
                                }
                                foreach (var Balance in BalanceData)
                                {
                                    CreditBalance += Balance.CreditBalance;
                                    DebitBalance += Balance.DebitBalance;
                                }



                                TotalDebit = DebitTransAction + DebitBalance;
                                TOTCredit = CreditTransAction + CreditBalance;
                            }
                            if (TotalDebit > TOTCredit)
                            {
                                NetDebit = TotalDebit - TOTCredit;
                            }
                            if (TotalDebit < TOTCredit)
                            {
                                NetCredit = TOTCredit - TotalDebit;
                            }

                            TrialBalanceVM trialBalance = new TrialBalanceVM();
                            trialBalance.AccountNumber = Acc.AccountNumber;
                            trialBalance.Name = Acc.ArabicName;

                            trialBalance.CreditBalance = CreditBalance;
                            trialBalance.CreditTransAction = CreditTransAction;
                            trialBalance.DebitBalance = DebitBalance;
                            trialBalance.DebitTransAction = DebitTransAction;
                            trialBalance.NetCredit = NetCredit;
                            trialBalance.NetDebit = NetDebit;
                            trialBalance.NetTot = NetDebit - NetCredit;
                            TotalType += NetDebit - NetCredit;
                            trialBalance.TempCostID = TempCostID;
                            trialBalance.TempCostType = TempCostType;

                            if (!Obj.ShowZeroBalance)
                            {
                                if ((trialBalance.NetCredit - trialBalance.NetDebit) != 0)
                                {
                                    TrialBalanceVMList.Add(trialBalance);
                                }
                            }
                            else
                            {
                                if ((trialBalance.NetCredit != 0) || (trialBalance.NetDebit != 0) || (TotalDebit != 0) || (NetCredit != 0))
                                {
                                    TrialBalanceVMList.Add(trialBalance);
                                }


                            }
                        }


                    }
                    else
                    if ((Obj.AccountLevelDropVMID > 0))
                    {


                        Accounts = AccountData.Where(m => m.AccountLevel == Obj.AccountLevelDropVMID && m.AccountTypeID == t).OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();

                        LessMainAccount = AccountData.Where(m => m.AccountLevel < Obj.AccountLevelDropVMID && m.AccountTypeID == t).OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();

                        IEnumerable<string> FaherAccount = LessMainAccount.Where(m => m.AccountFather != "0" && m.AccountTypeID == t).Select(m => m.AccountFather);


                        foreach (var A in LessMainAccount)
                        {
                            if (AccountData.FirstOrDefault(m => m.AccountFather == A.AccountNumber) != null)
                            {
                                //do nothing
                            }
                            else

                            {
                                Accounts = Accounts.Append(A);
                            }

                        }
                        Accounts = Accounts.OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();
                        // var MainChild = AccountData.Where(m => m.AccountFather == MainAcc.AccountNumber).OrderBy(m => m.AccountNumber).OrderBy(m => m.AccountLevel).ToList();
                        foreach (var Acc in Accounts)
                        {
                            TotalDebit = 0;
                            TOTCredit = 0;
                            NetCredit = 0;
                            NetDebit = 0;
                            CreditBalance = 0;
                            DebitBalance = 0;
                            CreditTransAction = 0;
                            DebitTransAction = 0;


                            if (AccountData.FirstOrDefault(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel) != null)
                            {
                                var AllAccount = AccountData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel && m.AccountTypeID == t)
                                                .OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();
                                int FatherLevel = Acc.AccountLevel;

                                foreach (var CurrAccLevel in AllAccount)
                                {
                                    if (CurrAccLevel.AccountLevel == FatherLevel)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        var TransActionData = TranData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                        var BalanceData = TotData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                        foreach (var D in TransActionData)
                                        {
                                            CreditTransAction += D.CreditTransAction;
                                            DebitTransAction += D.DebitTransAction;
                                        }
                                        foreach (var Balance in BalanceData)
                                        {
                                            CreditBalance += Balance.CreditBalance;
                                            DebitBalance += Balance.DebitBalance;
                                        }
                                        TotalDebit = DebitTransAction + DebitBalance;
                                        TOTCredit = CreditTransAction + CreditBalance;
                                    }
                                }
                            }
                            else
                            {
                                var TransActionData = TranData.Where(m => m.AccountNumber == Acc.AccountNumber);
                                var BalanceData = TotData.Where(m => m.AccountNumber == Acc.AccountNumber);
                                foreach (var D in TransActionData)
                                {
                                    CreditTransAction += D.CreditTransAction;
                                    DebitTransAction += D.DebitTransAction;
                                }
                                foreach (var Balance in BalanceData)
                                {
                                    CreditBalance += Balance.CreditBalance;
                                    DebitBalance += Balance.DebitBalance;
                                }

                                TotalDebit = DebitTransAction + DebitBalance;
                                TOTCredit = CreditTransAction + CreditBalance;
                            }
                            if (TotalDebit > TOTCredit)
                            {
                                NetDebit = TotalDebit - TOTCredit;
                            }
                            if (TotalDebit < TOTCredit)
                            {
                                NetCredit = TOTCredit - TotalDebit;
                            }

                            TrialBalanceVM trialBalance = new TrialBalanceVM();
                            trialBalance.AccountNumber = Acc.AccountNumber;
                            trialBalance.Name = Acc.ArabicName;
                            trialBalance.Level = Acc.AccountLevel;
                            trialBalance.CreditBalance = CreditBalance;
                            trialBalance.CreditTransAction = CreditTransAction;
                            trialBalance.DebitBalance = DebitBalance;
                            trialBalance.DebitTransAction = DebitTransAction;
                            trialBalance.NetCredit = NetCredit;
                            trialBalance.NetDebit = NetDebit;

                            trialBalance.nCreditBalance = CreditBalance;
                            trialBalance.nCreditTransAction = CreditTransAction;
                            trialBalance.nDebitBalance = DebitBalance;
                            trialBalance.nDebitTransAction = DebitTransAction;
                            trialBalance.nNetCredit = NetCredit;
                            trialBalance.nNetDebit = NetDebit;
                            trialBalance.TempCostID = TempCostID;
                            trialBalance.TempCostType = TempCostType;
                            trialBalance.NetTot = NetDebit - NetCredit;
                            TotalType += NetDebit - NetCredit;
                            if (AccountData.FirstOrDefault(m => m.AccountFather == Acc.AccountNumber && m.AccountTypeID == t) != null)
                            {
                                trialBalance.IsMainAccount = 1;

                            }
                            else
                            {
                                trialBalance.IsMainAccount = 0;


                            }

                            if (!Obj.ShowZeroBalance)
                            {
                                if ((trialBalance.NetCredit - trialBalance.NetDebit) != 0)
                                {
                                    TrialBalanceVMList.Add(trialBalance);
                                }
                            }
                            else
                            {
                                if ((trialBalance.NetCredit != 0) || (trialBalance.NetDebit != 0) || (TotalDebit != 0) || (NetCredit != 0))
                                {
                                    TrialBalanceVMList.Add(trialBalance);
                                }


                            }
                        }

                    }

                }
                TrialBalanceVMList.Add(new TrialBalanceVM
                {

                    AccountNumber = "**",
                    Name = "مجموع المطلوبات طويلة الاجل",
                    NetTot = TotalType,

                });
                Totalliabilities += TotalType;
                //--//
                AccType = new List<int> { 6 };
                TrialBalanceVMList.Add(new TrialBalanceVM
                {

                    AccountNumber = "*",
                    Name = "حقوق الملكية"	
                });
                TotalType = 0;
                foreach (var t in AccType)
                {


                    if (!String.IsNullOrEmpty(Obj.AccNo))
                    {

                        var MainAccount = AccountData.FirstOrDefault(m => m.AccountNumber == Obj.AccNo);
                        var MainChild = AccountData.Where(m => m.AccountFather == Obj.AccNo).OrderBy(m => m.AccountNumber).OrderBy(m => m.AccountLevel).ToList();

                        foreach (var Acc in MainChild)
                        {
                            TotalDebit = 0;
                            TOTCredit = 0;
                            NetCredit = 0;
                            NetDebit = 0;
                            CreditBalance = 0;
                            DebitBalance = 0;
                            CreditTransAction = 0;
                            DebitTransAction = 0;


                            if (AccountData.FirstOrDefault(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel) != null)//Check if Account Had Branches
                            {
                                var AllAccount = AccountData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel)
                                                .OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();//Get All Account Branches
                                int FatherLevel = Acc.AccountLevel;
                                foreach (var CurrAccLevel in AllAccount)
                                {
                                    if (CurrAccLevel.AccountLevel == FatherLevel)
                                    {
                                        break;
                                    }
                                    else
                                    {

                                        var TransActionData = TranData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                        var BalanceData = TotData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                        foreach (var D in TransActionData)
                                        {
                                            CreditTransAction += D.CreditTransAction;
                                            DebitTransAction += D.DebitTransAction;
                                        }
                                        foreach (var Balance in BalanceData)
                                        {
                                            CreditBalance += Balance.CreditBalance;
                                            DebitBalance += Balance.DebitBalance;
                                        }



                                        TotalDebit = DebitTransAction + DebitBalance;
                                        TOTCredit = CreditTransAction + CreditBalance;




                                    }
                                }




                            }
                            else
                            {
                                var TransActionData = TranData.Where(m => m.AccountNumber == Acc.AccountNumber);
                                var BalanceData = TotData.Where(m => m.AccountNumber == Acc.AccountNumber);
                                foreach (var D in TransActionData)
                                {
                                    CreditTransAction += D.CreditTransAction;
                                    DebitTransAction += D.DebitTransAction;
                                }
                                foreach (var Balance in BalanceData)
                                {
                                    CreditBalance += Balance.CreditBalance;
                                    DebitBalance += Balance.DebitBalance;
                                }



                                TotalDebit = DebitTransAction + DebitBalance;
                                TOTCredit = CreditTransAction + CreditBalance;
                            }
                            if (TotalDebit > TOTCredit)
                            {
                                NetDebit = TotalDebit - TOTCredit;
                            }
                            if (TotalDebit < TOTCredit)
                            {
                                NetCredit = TOTCredit - TotalDebit;
                            }

                            TrialBalanceVM trialBalance = new TrialBalanceVM();
                            trialBalance.AccountNumber = Acc.AccountNumber;
                            trialBalance.Name = Acc.ArabicName;

                            trialBalance.CreditBalance = CreditBalance;
                            trialBalance.CreditTransAction = CreditTransAction;
                            trialBalance.DebitBalance = DebitBalance;
                            trialBalance.DebitTransAction = DebitTransAction;
                            trialBalance.NetCredit = NetCredit;
                            trialBalance.NetDebit = NetDebit;
                            trialBalance.NetTot = NetDebit - NetCredit;
                            TotalType += NetDebit - NetCredit;
                            trialBalance.TempCostID = TempCostID;
                            trialBalance.TempCostType = TempCostType;

                            if (!Obj.ShowZeroBalance)
                            {
                                if ((trialBalance.NetCredit - trialBalance.NetDebit) != 0)
                                {
                                    TrialBalanceVMList.Add(trialBalance);
                                }
                            }
                            else
                            {
                                if ((trialBalance.NetCredit != 0) || (trialBalance.NetDebit != 0) || (TotalDebit != 0) || (NetCredit != 0))
                                {
                                    TrialBalanceVMList.Add(trialBalance);
                                }


                            }
                        }


                    }
                    else
                    if ((Obj.AccountLevelDropVMID > 0))
                    {


                        Accounts = AccountData.Where(m => m.AccountLevel == Obj.AccountLevelDropVMID && m.AccountTypeID == t).OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();

                        LessMainAccount = AccountData.Where(m => m.AccountLevel < Obj.AccountLevelDropVMID && m.AccountTypeID == t).OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();

                        IEnumerable<string> FaherAccount = LessMainAccount.Where(m => m.AccountFather != "0" && m.AccountTypeID == t).Select(m => m.AccountFather);


                        foreach (var A in LessMainAccount)
                        {
                            if (AccountData.FirstOrDefault(m => m.AccountFather == A.AccountNumber) != null)
                            {
                                //do nothing
                            }
                            else

                            {
                                Accounts = Accounts.Append(A);
                            }

                        }
                        Accounts = Accounts.OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();
                        // var MainChild = AccountData.Where(m => m.AccountFather == MainAcc.AccountNumber).OrderBy(m => m.AccountNumber).OrderBy(m => m.AccountLevel).ToList();
                        foreach (var Acc in Accounts)
                        {
                            TotalDebit = 0;
                            TOTCredit = 0;
                            NetCredit = 0;
                            NetDebit = 0;
                            CreditBalance = 0;
                            DebitBalance = 0;
                            CreditTransAction = 0;
                            DebitTransAction = 0;


                            if (AccountData.FirstOrDefault(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel) != null)
                            {
                                var AllAccount = AccountData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel && m.AccountTypeID == t)
                                                .OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();
                                int FatherLevel = Acc.AccountLevel;

                                foreach (var CurrAccLevel in AllAccount)
                                {
                                    if (CurrAccLevel.AccountLevel == FatherLevel)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        var TransActionData = TranData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                        var BalanceData = TotData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                        foreach (var D in TransActionData)
                                        {
                                            CreditTransAction += D.CreditTransAction;
                                            DebitTransAction += D.DebitTransAction;
                                        }
                                        foreach (var Balance in BalanceData)
                                        {
                                            CreditBalance += Balance.CreditBalance;
                                            DebitBalance += Balance.DebitBalance;
                                        }
                                        TotalDebit = DebitTransAction + DebitBalance;
                                        TOTCredit = CreditTransAction + CreditBalance;
                                    }
                                }
                            }
                            else
                            {
                                var TransActionData = TranData.Where(m => m.AccountNumber == Acc.AccountNumber);
                                var BalanceData = TotData.Where(m => m.AccountNumber == Acc.AccountNumber);
                                foreach (var D in TransActionData)
                                {
                                    CreditTransAction += D.CreditTransAction;
                                    DebitTransAction += D.DebitTransAction;
                                }
                                foreach (var Balance in BalanceData)
                                {
                                    CreditBalance += Balance.CreditBalance;
                                    DebitBalance += Balance.DebitBalance;
                                }

                                TotalDebit = DebitTransAction + DebitBalance;
                                TOTCredit = CreditTransAction + CreditBalance;
                            }
                            if (TotalDebit > TOTCredit)
                            {
                                NetDebit = TotalDebit - TOTCredit;
                            }
                            if (TotalDebit < TOTCredit)
                            {
                                NetCredit = TOTCredit - TotalDebit;
                            }

                            TrialBalanceVM trialBalance = new TrialBalanceVM();
                            trialBalance.AccountNumber = Acc.AccountNumber;
                            trialBalance.Name = Acc.ArabicName;
                            trialBalance.Level = Acc.AccountLevel;
                            trialBalance.CreditBalance = CreditBalance;
                            trialBalance.CreditTransAction = CreditTransAction;
                            trialBalance.DebitBalance = DebitBalance;
                            trialBalance.DebitTransAction = DebitTransAction;
                            trialBalance.NetCredit = NetCredit;
                            trialBalance.NetDebit = NetDebit;

                            trialBalance.nCreditBalance = CreditBalance;
                            trialBalance.nCreditTransAction = CreditTransAction;
                            trialBalance.nDebitBalance = DebitBalance;
                            trialBalance.nDebitTransAction = DebitTransAction;
                            trialBalance.nNetCredit = NetCredit;
                            trialBalance.nNetDebit = NetDebit;
                            trialBalance.TempCostID = TempCostID;
                            trialBalance.TempCostType = TempCostType;
                            trialBalance.NetTot = NetDebit - NetCredit;
                            TotalType += NetDebit - NetCredit;
                            if (AccountData.FirstOrDefault(m => m.AccountFather == Acc.AccountNumber && m.AccountTypeID == t) != null)
                            {
                                trialBalance.IsMainAccount = 1;

                            }
                            else
                            {
                                trialBalance.IsMainAccount = 0;


                            }

                            if (!Obj.ShowZeroBalance)
                            {
                                if ((trialBalance.NetCredit - trialBalance.NetDebit) != 0)
                                {
                                    TrialBalanceVMList.Add(trialBalance);
                                }
                            }
                            else
                            {
                                if ((trialBalance.NetCredit != 0) || (trialBalance.NetDebit != 0) || (TotalDebit != 0) || (NetCredit != 0))
                                {
                                    TrialBalanceVMList.Add(trialBalance);
                                }


                            }
                        }

                    }

                }
                TrialBalanceVMList.Add(new TrialBalanceVM
                {

                    AccountNumber = "**",
                    Name = "صافي حقوق الملكية",
                    NetTot = TotalType,

                });
                Totalliabilities += TotalType;


                TrialBalanceVMList.Add(new TrialBalanceVM
                {

                    AccountNumber = "",
                    Name = "صافي الربح / الخسارة",
                    NetTot = Obj.ProfitLostNetTotal,

                });




                Totalliabilities += Obj.ProfitLostNetTotal;
                TrialBalanceVMList.Add(new TrialBalanceVM
                {

                    AccountNumber = "**",
                    Name = "مجموع المطلوبات  ",
                    NetTot = Totalliabilities

                });

                //----------//

            }
            catch (Exception ex)
            {
                string err = ex.Message;

                //List<TrialBalanceVM> TrialBalanceVMList = new List<TrialBalanceVM>();

                //return Json(TrialBalanceVMList, JsonRequestBehavior.AllowGet);

            }

            return Json(TrialBalanceVMList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetTrialBalance(AccountLevelRepVM Obj)
        {
            try
            {

                var userId = User.Identity.GetUserId();
                string UserID = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
                var AccountData = _unitOfWork.NativeSql.GetChartOfAccountByLevel(UserInfo.fCompanyId);
                int CurrYear = UserInfo.CurrYear;
           

                  var Date = "01/01/" +CurrYear;
                var OpenDate = "31/12/" + (CurrYear-1).ToString();

                string TempCostID="0";
                int TempCostType = 0;
              
                if (Obj.ByCostCenter)
                {
                    TempCostID = Obj.CostCenterNumber;
                    if (Obj.Partofthenumber)
                    {
                        TempCostType = Obj.CostSearchType;
                    }


                }

                IEnumerable<TrialBalanceVM> TotData = new List<TrialBalanceVM>();
                if (Obj.Partofthenumber)
                {
                    if (Obj.CostSearchType == 1)
                    {
                        Obj.CostCenterNumber = Obj.CostCenterNumber + "%";
                    }
                    else if (Obj.CostSearchType == 2)
                    {
                        Obj.CostCenterNumber = "%" + Obj.CostCenterNumber;
                    }
                    else if (Obj.CostSearchType == 3)
                    {
                        Obj.CostCenterNumber = "%" + Obj.CostCenterNumber + "%";

                    }
                }

                var TranData = _unitOfWork.NativeSql.GetTransactionForTrial(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate,Obj.ByCostCenter,Obj.CostCenterNumber,Obj.Partofthenumber,Obj.CostSearchType);
                if (DateTime.Parse(Date)!= Obj.FromDate)
                  TotData = _unitOfWork.NativeSql.GetTotCreditDebitForTrial(UserInfo.fCompanyId, DateTime.Parse(Date),Obj.FromDate, Obj.ByCostCenter, Obj.CostCenterNumber, Obj.Partofthenumber, Obj.CostSearchType);

                //----------------Get Open-------------//
                IEnumerable<TrialBalanceVM> TotDataOpen = new List<TrialBalanceVM>();
                 TotDataOpen = _unitOfWork.NativeSql.GetTotCreditDebitForTrialOpen(UserInfo.fCompanyId, DateTime.Parse(OpenDate), DateTime.Parse(OpenDate));
                foreach(var D in TotDataOpen)
                {
                    TotData = TotData.Append(D);

                }
                //------------------------------------//

            

                double TotalDebit = 0;
                double TOTCredit = 0;
                double NetCredit = 0;
                double NetDebit = 0;
                double CreditBalance = 0;
                double DebitBalance = 0;
                double CreditTransAction = 0;
                double DebitTransAction = 0;

                List<TrialBalanceVM> TrialBalanceVMList = new List<TrialBalanceVM>();
                
                IEnumerable<ChartOfAccount> Accounts = new List<ChartOfAccount>();
               
                IEnumerable<ChartOfAccount> LessMainAccount = new List<ChartOfAccount>();
           
                if (!String.IsNullOrEmpty(Obj.AccNo))
                {
                   
                    var MainAccount = AccountData.FirstOrDefault(m => m.AccountNumber == Obj.AccNo);
                    var MainChild = AccountData.Where(m => m.AccountFather==Obj.AccNo).OrderBy(m => m.AccountNumber).OrderBy(m => m.AccountLevel).ToList();

                    foreach (var Acc in MainChild)
                    {
                        TotalDebit = 0;
                        TOTCredit = 0;
                        NetCredit = 0;
                        NetDebit = 0;
                        CreditBalance = 0;
                        DebitBalance = 0;
                        CreditTransAction = 0;
                        DebitTransAction = 0;


                        if (AccountData.FirstOrDefault(m=>m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel)!=null)//Check if Account Had Branches
                        {
                            var AllAccount = AccountData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel)
                                            .OrderBy(m => m.AccountNumber).ThenBy(m=>m.AccountLevel).ToList();//Get All Account Branches
                            int FatherLevel = Acc.AccountLevel;
                            foreach(var CurrAccLevel in AllAccount)
                            {
                                if(CurrAccLevel.AccountLevel == FatherLevel)
                                {
                                    break;
                                }
                                else
                                {
                                  
                                    var TransActionData = TranData.Where(m => m.AccountNumber==CurrAccLevel.AccountNumber);
                                    var BalanceData = TotData.Where(m => m.AccountNumber==CurrAccLevel.AccountNumber);
                                    foreach (var D in TransActionData)
                                    {
                                        CreditTransAction += D.CreditTransAction;
                                        DebitTransAction += D.DebitTransAction;
                                    }
                                    foreach (var Balance in BalanceData)
                                    {
                                        CreditBalance += Balance.CreditBalance;
                                        DebitBalance += Balance.DebitBalance;
                                    }
                                 


                                    TotalDebit = DebitTransAction + DebitBalance;
                                    TOTCredit = CreditTransAction + CreditBalance;

                               


                                }
                            } 




                        }
                        else
                        {
                            var TransActionData = TranData.Where(m => m.AccountNumber == Acc.AccountNumber);
                            var BalanceData = TotData.Where(m => m.AccountNumber == Acc.AccountNumber);
                            foreach (var D in TransActionData)
                            {
                                CreditTransAction += D.CreditTransAction;
                                DebitTransAction += D.DebitTransAction;
                            }
                            foreach (var Balance in BalanceData)
                            {
                                CreditBalance += Balance.CreditBalance;
                                DebitBalance += Balance.DebitBalance;
                            }
                            TotalDebit = DebitTransAction + DebitBalance;
                            TOTCredit = CreditTransAction + CreditBalance;
                        }
                        if (TotalDebit > TOTCredit)
                        {
                            NetDebit = TotalDebit - TOTCredit;
                        }
                        if (TotalDebit < TOTCredit)
                        {
                            NetCredit = TOTCredit - TotalDebit;
                        }

                        TrialBalanceVM trialBalance = new TrialBalanceVM();
                        trialBalance.AccountNumber = Acc.AccountNumber;
                        trialBalance.Name = Acc.ArabicName;

                        trialBalance.CreditBalance = CreditBalance;
                        trialBalance.CreditTransAction = CreditTransAction;
                        trialBalance.DebitBalance = DebitBalance;
                        trialBalance.DebitTransAction = DebitTransAction;
                        trialBalance.NetCredit = NetCredit;
                        trialBalance.NetDebit = NetDebit;
                        trialBalance.TempCostID = TempCostID;
                        trialBalance.TempCostType = TempCostType;



                        trialBalance.nCreditBalance = CreditBalance;
                        trialBalance.nCreditTransAction = CreditTransAction;
                        trialBalance.nDebitBalance = DebitBalance;
                        trialBalance.nDebitTransAction = DebitTransAction;
                        trialBalance.nNetCredit = NetCredit;
                        trialBalance.nNetDebit = NetDebit;


                        if (!Obj.ShowZeroBalance)
                        {
                            if ((trialBalance.NetCredit - trialBalance.NetDebit) != 0)
                            {
                                TrialBalanceVMList.Add(trialBalance);
                            }
                        }
                        else
                        {
                            if ((trialBalance.NetCredit != 0) || (trialBalance.NetDebit != 0) || (TotalDebit != 0) || (NetCredit != 0))
                            {
                                TrialBalanceVMList.Add(trialBalance);
                            }


                        }
                    }
  

                }
                else
               
                if ((Obj.AccountLevelDropVMID>0) && (!Obj.Detail))
                {
                  
                    
                    Accounts = AccountData.Where(m => m.AccountLevel == Obj.AccountLevelDropVMID).OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();
                    
                    LessMainAccount = AccountData.Where(m => m.AccountLevel < Obj.AccountLevelDropVMID).OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();

                    IEnumerable<string> FaherAccount = LessMainAccount.Where(m => m.AccountFather != "0").Select(m => m.AccountFather);
 

                    foreach (var A in LessMainAccount)
                    {
                        if (AccountData.FirstOrDefault(m => m.AccountFather == A.AccountNumber) != null)
                        {
                            //do nothing
                        }
                        else

                        {
                            Accounts = Accounts.Append(A);
                        }
                       
                    }
                    Accounts = Accounts.OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();
                    // var MainChild = AccountData.Where(m => m.AccountFather == MainAcc.AccountNumber).OrderBy(m => m.AccountNumber).OrderBy(m => m.AccountLevel).ToList();
                    foreach (var Acc in Accounts)
                        {
                            TotalDebit = 0;
                            TOTCredit = 0;
                            NetCredit = 0;
                            NetDebit = 0;
                            CreditBalance = 0;
                            DebitBalance = 0;
                            CreditTransAction = 0;
                            DebitTransAction = 0;


                            if (AccountData.FirstOrDefault(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel) != null)
                            {
                                var AllAccount = AccountData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel)
                                                .OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();
                                int FatherLevel = Acc.AccountLevel;

                                foreach (var CurrAccLevel in AllAccount)
                                {
                                    if (CurrAccLevel.AccountLevel == FatherLevel)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        var TransActionData = TranData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                        var BalanceData = TotData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                        foreach (var D in TransActionData)
                                        {
                                            CreditTransAction += D.CreditTransAction;
                                            DebitTransAction += D.DebitTransAction;
                                        }
                                        foreach (var Balance in BalanceData)
                                        {
                                            CreditBalance += Balance.CreditBalance;
                                            DebitBalance += Balance.DebitBalance;
                                        }
                                        TotalDebit = DebitTransAction + DebitBalance;
                                        TOTCredit = CreditTransAction + CreditBalance;
                                    }
                                }
                            }
                            else
                            {
                                var TransActionData = TranData.Where(m => m.AccountNumber == Acc.AccountNumber);
                                var BalanceData = TotData.Where(m => m.AccountNumber == Acc.AccountNumber);
                                foreach (var D in TransActionData)
                                {
                                    CreditTransAction += D.CreditTransAction;
                                    DebitTransAction += D.DebitTransAction;
                                }
                                foreach (var Balance in BalanceData)
                                {
                                    CreditBalance += Balance.CreditBalance;
                                    DebitBalance += Balance.DebitBalance;
                                }

                                TotalDebit = DebitTransAction + DebitBalance;
                                TOTCredit = CreditTransAction + CreditBalance;
                            }
                            if (TotalDebit > TOTCredit)
                            {
                                NetDebit = TotalDebit - TOTCredit;
                            }
                            if (TotalDebit < TOTCredit)
                            {
                                NetCredit = TOTCredit - TotalDebit;
                            }

                            TrialBalanceVM trialBalance = new TrialBalanceVM();
                            trialBalance.AccountNumber = Acc.AccountNumber;
                            trialBalance.Name = Acc.ArabicName;
                             trialBalance.Level = Acc.AccountLevel;
                            trialBalance.CreditBalance = CreditBalance;
                            trialBalance.CreditTransAction = CreditTransAction;
                            trialBalance.DebitBalance = DebitBalance;
                            trialBalance.DebitTransAction = DebitTransAction;
                            trialBalance.NetCredit = NetCredit;
                            trialBalance.NetDebit = NetDebit;

                            trialBalance.nCreditBalance = CreditBalance;
                            trialBalance.nCreditTransAction = CreditTransAction;
                            trialBalance.nDebitBalance = DebitBalance;
                            trialBalance.nDebitTransAction = DebitTransAction;
                            trialBalance.nNetCredit = NetCredit;
                            trialBalance.nNetDebit = NetDebit;
                        trialBalance.TempCostID = TempCostID;
                        trialBalance.TempCostType = TempCostType;
                        if (AccountData.FirstOrDefault(m => m.AccountFather == Acc.AccountNumber) != null)
                        {
                            trialBalance.IsMainAccount = 1;
                            
                        }
                        else
                        {
                            trialBalance.IsMainAccount = 0;
                            

                        }

                        if (!Obj.ShowZeroBalance)
                            {
                                if ((trialBalance.NetCredit - trialBalance.NetDebit) != 0)
                                {
                                    TrialBalanceVMList.Add(trialBalance);
                                }
                            }
                            else
                            {
                                if ((trialBalance.NetCredit != 0) || (trialBalance.NetDebit != 0) || (TotalDebit != 0) || (NetCredit != 0))
                                {
                                    TrialBalanceVMList.Add(trialBalance);
                                }


                            }
                        }

                }
                else if ((Obj.AccountLevelDropVMID > 0) && (Obj.Detail))
                {


                    Accounts = AccountData.Where(m => m.AccountLevel <= Obj.AccountLevelDropVMID).ToList().OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();
 
                    foreach (var Acc in Accounts)//Each Main Account
                    {
                        TotalDebit = 0;
                        TOTCredit = 0;
                        NetCredit = 0;
                        NetDebit = 0;
                        CreditBalance = 0;
                        DebitBalance = 0;
                        CreditTransAction = 0;
                        DebitTransAction = 0;

                        var MainAccount = AccountData.FirstOrDefault(m => m.AccountNumber == Acc.AccountNumber);
                            var MainChild = AccountData.Where(m => m.AccountFather == Acc.AccountNumber).OrderBy(m => m.AccountNumber).OrderBy(m => m.AccountLevel).ToList();
                        var TransActionData = new List<TrialBalanceVM>();
                        TranData = TranData.Where(m => m.AccountNumber != null).ToList();
                        if (TranData!= null && TranData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber))!=null)
                        {
                               TransActionData = TranData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber)).ToList();
                        }
                         
                        var BalanceData = TotData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber));

                        if (TransActionData != null)
                        {
                            foreach (var D in TransActionData)
                            {
                                CreditTransAction += D.CreditTransAction;
                                DebitTransAction += D.DebitTransAction;
                            }

                        }
                     
                        if (BalanceData != null)
                        {
                            foreach (var Balance in BalanceData)
                            {
                                CreditBalance += Balance.CreditBalance;
                                DebitBalance += Balance.DebitBalance;
                            }
                        }
                       



                        TotalDebit = DebitTransAction + DebitBalance;
                        TOTCredit = CreditTransAction + CreditBalance;

                        if (TotalDebit > TOTCredit)
                        {
                            NetDebit = TotalDebit - TOTCredit;
                        }
                        if (TotalDebit < TOTCredit)
                        {
                            NetCredit = TOTCredit - TotalDebit;
                        }

                        TrialBalanceVM trialBalance = new TrialBalanceVM();
                        trialBalance.AccountNumber = Acc.AccountNumber;
                        trialBalance.Name = Acc.ArabicName;

                        trialBalance.CreditBalance = CreditBalance;
                        trialBalance.CreditTransAction = CreditTransAction;
                        trialBalance.DebitBalance = DebitBalance;
                        trialBalance.DebitTransAction = DebitTransAction;
                        trialBalance.NetCredit = NetCredit;
                        trialBalance.NetDebit = NetDebit;
                        trialBalance.TempCostID = TempCostID;
                        trialBalance.TempCostType = TempCostType;
                        if ((AccountData.FirstOrDefault(m => m.AccountFather == Acc.AccountNumber) != null) && (Acc.AccountLevel < Obj.AccountLevelDropVMID ))
                        {
                            trialBalance.IsMainAccount = 1;
                            trialBalance.MainAccount = "{*}";

                          
                            trialBalance.nCreditBalance = 0;
                            trialBalance.nCreditTransAction = 0;
                            trialBalance.nDebitBalance = 0;
                            trialBalance.nDebitTransAction = 0;
                            trialBalance.nNetCredit = 0;
                            trialBalance.nNetDebit = 0;

                        }
                        else
                        {
                            trialBalance.IsMainAccount = 0;
                            trialBalance.MainAccount = "";
                            trialBalance.nCreditBalance = CreditBalance;
                            trialBalance.nCreditTransAction = CreditTransAction;
                            trialBalance.nDebitBalance = DebitBalance;
                            trialBalance.nDebitTransAction = DebitTransAction;
                            trialBalance.nNetCredit = NetCredit;
                            trialBalance.nNetDebit = NetDebit;


                        }
                        if (!Obj.ShowZeroBalance)
                        {
                            if ((trialBalance.NetCredit - trialBalance.NetDebit) != 0)
                            {
                                TrialBalanceVMList.Add(trialBalance);
                            }
                        }
                        else
                        {
                           if ((trialBalance.NetCredit != 0) || (trialBalance.NetDebit != 0) || (TotalDebit != 0) || (NetCredit != 0))
                           {
                                TrialBalanceVMList.Add(trialBalance);
                            }


                         }


                  
                             
               }
                }
                if (Obj.OrderBy == 1)
                {
                    TrialBalanceVMList = TrialBalanceVMList.OrderByDescending(m => m.NetDebit).ThenBy(m => m.NetCredit).ToList();
                }
                else if (Obj.OrderBy == 2)
                {
                    TrialBalanceVMList = TrialBalanceVMList.OrderBy(m => m.NetDebit).ThenBy(m => m.NetCredit).ToList();
                }
                return Json(TrialBalanceVMList, JsonRequestBehavior.AllowGet);
              
            }
            catch (Exception ex)
            {
                string err = ex.Message;

                List<TrialBalanceVM> TrialBalanceVMList = new List<TrialBalanceVM>();

                return Json(TrialBalanceVMList, JsonRequestBehavior.AllowGet);

            }


        }
        [HttpPost]
        public JsonResult GetTrialBalanceForSupplier(AccountLevelRepVM Obj)
        {
            try
            {

                var userId = User.Identity.GetUserId();
                string UserID = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
                var AccountData = _unitOfWork.NativeSql.GetChartOfAccountByLevel(UserInfo.fCompanyId);
                AccountData = AccountData.Where(m => m.AccountKind == 2).ToList();
                int CurrYear = UserInfo.CurrYear;

                var Date = "01/01/" + CurrYear;
                var OpenDate = "31/12/" + (CurrYear - 1).ToString();

                string TempCostID = "0";
                int TempCostType = 0;

                if (Obj.ByCostCenter)
                {
                    TempCostID = Obj.CostCenterNumber;
                    if (Obj.Partofthenumber)
                    {
                        TempCostType = Obj.CostSearchType;
                    }


                }

                IEnumerable<TrialBalanceVM> TotData = new List<TrialBalanceVM>();
                if (Obj.Partofthenumber)
                {
                    if (Obj.CostSearchType == 1)
                    {
                        Obj.CostCenterNumber = Obj.CostCenterNumber + "%";
                    }
                    else if (Obj.CostSearchType == 2)
                    {
                        Obj.CostCenterNumber = "%" + Obj.CostCenterNumber;
                    }
                    else if (Obj.CostSearchType == 3)
                    {
                        Obj.CostCenterNumber = "%" + Obj.CostCenterNumber + "%";

                    }
                }

                var TranData = _unitOfWork.NativeSql.GetTransactionForTrial(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate, Obj.ByCostCenter, Obj.CostCenterNumber, Obj.Partofthenumber, Obj.CostSearchType);
                if (DateTime.Parse(Date) != Obj.FromDate)
                    TotData = _unitOfWork.NativeSql.GetTotCreditDebitForTrial(UserInfo.fCompanyId, DateTime.Parse(Date), Obj.FromDate, Obj.ByCostCenter, Obj.CostCenterNumber, Obj.Partofthenumber, Obj.CostSearchType);
                //----------------Get Open-------------//
                IEnumerable<TrialBalanceVM> TotDataOpen = new List<TrialBalanceVM>();
                TotDataOpen = _unitOfWork.NativeSql.GetTotCreditDebitForTrialOpen(UserInfo.fCompanyId, DateTime.Parse(OpenDate), DateTime.Parse(OpenDate));
                foreach (var D in TotDataOpen)
                {
                    TotData = TotData.Append(D);

                }
                //------------------------------------//



                double TotalDebit = 0;
                double TOTCredit = 0;
                double NetCredit = 0;
                double NetDebit = 0;
                double CreditBalance = 0;
                double DebitBalance = 0;
                double CreditTransAction = 0;
                double DebitTransAction = 0;

                List<TrialBalanceVM> TrialBalanceVMList = new List<TrialBalanceVM>();

                IEnumerable<ChartOfAccount> Accounts = new List<ChartOfAccount>();

                IEnumerable<ChartOfAccount> LessMainAccount = new List<ChartOfAccount>();
                if (!String.IsNullOrEmpty(Obj.AccNo))
                {

                    var MainAccount = AccountData.FirstOrDefault(m => m.AccountNumber == Obj.AccNo);
                    var MainChild = AccountData.Where(m => m.AccountFather == Obj.AccNo).OrderBy(m => m.AccountNumber).OrderBy(m => m.AccountLevel).ToList();

                    foreach (var Acc in MainChild)
                    {
                        TotalDebit = 0;
                        TOTCredit = 0;
                        NetCredit = 0;
                        NetDebit = 0;
                        CreditBalance = 0;
                        DebitBalance = 0;
                        CreditTransAction = 0;
                        DebitTransAction = 0;


                        if (AccountData.FirstOrDefault(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel) != null)//Check if Account Had Branches
                        {
                            var AllAccount = AccountData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel)
                                            .OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();//Get All Account Branches
                            int FatherLevel = Acc.AccountLevel;
                            foreach (var CurrAccLevel in AllAccount)
                            {
                                if (CurrAccLevel.AccountLevel == FatherLevel)
                                {
                                    break;
                                }
                                else
                                {

                                    var TransActionData = TranData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                    var BalanceData = TotData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                    foreach (var D in TransActionData)
                                    {
                                        CreditTransAction += D.CreditTransAction;
                                        DebitTransAction += D.DebitTransAction;
                                    }
                                    foreach (var Balance in BalanceData)
                                    {
                                        CreditBalance += Balance.CreditBalance;
                                        DebitBalance += Balance.DebitBalance;
                                    }



                                    TotalDebit = DebitTransAction + DebitBalance;
                                    TOTCredit = CreditTransAction + CreditBalance;




                                }
                            }




                        }
                        else
                        {
                            var TransActionData = TranData.Where(m => m.AccountNumber == Acc.AccountNumber);
                            var BalanceData = TotData.Where(m => m.AccountNumber == Acc.AccountNumber);
                            foreach (var D in TransActionData)
                            {
                                CreditTransAction += D.CreditTransAction;
                                DebitTransAction += D.DebitTransAction;
                            }
                            foreach (var Balance in BalanceData)
                            {
                                CreditBalance += Balance.CreditBalance;
                                DebitBalance += Balance.DebitBalance;
                            }



                            TotalDebit = DebitTransAction + DebitBalance;
                            TOTCredit = CreditTransAction + CreditBalance;
                        }
                        if (TotalDebit > TOTCredit)
                        {
                            NetDebit = TotalDebit - TOTCredit;
                        }
                        if (TotalDebit < TOTCredit)
                        {
                            NetCredit = TOTCredit - TotalDebit;
                        }

                        TrialBalanceVM trialBalance = new TrialBalanceVM();
                        trialBalance.AccountNumber = Acc.AccountNumber;
                        trialBalance.Name = Acc.ArabicName;

                        trialBalance.CreditBalance = CreditBalance;
                        trialBalance.CreditTransAction = CreditTransAction;
                        trialBalance.DebitBalance = DebitBalance;
                        trialBalance.DebitTransAction = DebitTransAction;
                        trialBalance.NetCredit = NetCredit;
                        trialBalance.NetDebit = NetDebit;
                        trialBalance.TempCostID = TempCostID;
                        trialBalance.TempCostType = TempCostType;
                        if (!Obj.ShowZeroBalance)
                        {
                            if ((trialBalance.NetCredit - trialBalance.NetDebit) != 0)
                            {
                                TrialBalanceVMList.Add(trialBalance);
                            }
                        }
                        else
                        {
                            if ((trialBalance.NetCredit != 0) || (trialBalance.NetDebit != 0) || (TotalDebit != 0) || (NetCredit != 0))
                            {
                                TrialBalanceVMList.Add(trialBalance);
                            }


                        }
                    }


                }
                else

                if ((Obj.AccountLevelDropVMID > 0) && (!Obj.Detail))
                {


                    Accounts = AccountData.Where(m => m.AccountLevel == Obj.AccountLevelDropVMID).OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();

                    LessMainAccount = AccountData.Where(m => m.AccountLevel < Obj.AccountLevelDropVMID).OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();

                    IEnumerable<string> FaherAccount = LessMainAccount.Where(m => m.AccountFather != "0").Select(m => m.AccountFather);


                    foreach (var A in LessMainAccount)
                    {
                        if (AccountData.FirstOrDefault(m => m.AccountFather == A.AccountNumber) != null)
                        {
                            //do nothing
                        }
                        else

                        {
                            Accounts = Accounts.Append(A);
                        }

                    }
                    Accounts = Accounts.OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();
                    // var MainChild = AccountData.Where(m => m.AccountFather == MainAcc.AccountNumber).OrderBy(m => m.AccountNumber).OrderBy(m => m.AccountLevel).ToList();
                    foreach (var Acc in Accounts)
                    {
                        TotalDebit = 0;
                        TOTCredit = 0;
                        NetCredit = 0;
                        NetDebit = 0;
                        CreditBalance = 0;
                        DebitBalance = 0;
                        CreditTransAction = 0;
                        DebitTransAction = 0;


                        if (AccountData.FirstOrDefault(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel) != null)
                        {
                            var AllAccount = AccountData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel)
                                            .OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();
                            int FatherLevel = Acc.AccountLevel;

                            foreach (var CurrAccLevel in AllAccount)
                            {
                                if (CurrAccLevel.AccountLevel == FatherLevel)
                                {
                                    break;
                                }
                                else
                                {
                                    var TransActionData = TranData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                    var BalanceData = TotData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                    foreach (var D in TransActionData)
                                    {
                                        CreditTransAction += D.CreditTransAction;
                                        DebitTransAction += D.DebitTransAction;
                                    }
                                    foreach (var Balance in BalanceData)
                                    {
                                        CreditBalance += Balance.CreditBalance;
                                        DebitBalance += Balance.DebitBalance;
                                    }
                                    TotalDebit = DebitTransAction + DebitBalance;
                                    TOTCredit = CreditTransAction + CreditBalance;
                                }
                            }
                        }
                        else
                        {
                            var TransActionData = TranData.Where(m => m.AccountNumber == Acc.AccountNumber);
                            var BalanceData = TotData.Where(m => m.AccountNumber == Acc.AccountNumber);
                            foreach (var D in TransActionData)
                            {
                                CreditTransAction += D.CreditTransAction;
                                DebitTransAction += D.DebitTransAction;
                            }
                            foreach (var Balance in BalanceData)
                            {
                                CreditBalance += Balance.CreditBalance;
                                DebitBalance += Balance.DebitBalance;
                            }

                            TotalDebit = DebitTransAction + DebitBalance;
                            TOTCredit = CreditTransAction + CreditBalance;
                        }
                        if (TotalDebit > TOTCredit)
                        {
                            NetDebit = TotalDebit - TOTCredit;
                        }
                        if (TotalDebit < TOTCredit)
                        {
                            NetCredit = TOTCredit - TotalDebit;
                        }

                        TrialBalanceVM trialBalance = new TrialBalanceVM();
                        trialBalance.AccountNumber = Acc.AccountNumber;
                        trialBalance.Name = Acc.ArabicName;
                        trialBalance.Level = Acc.AccountLevel;
                        trialBalance.CreditBalance = CreditBalance;
                        trialBalance.CreditTransAction = CreditTransAction;
                        trialBalance.DebitBalance = DebitBalance;
                        trialBalance.DebitTransAction = DebitTransAction;
                        trialBalance.NetCredit = NetCredit;
                        trialBalance.NetDebit = NetDebit;

                        trialBalance.nCreditBalance = CreditBalance;
                        trialBalance.nCreditTransAction = CreditTransAction;
                        trialBalance.nDebitBalance = DebitBalance;
                        trialBalance.nDebitTransAction = DebitTransAction;
                        trialBalance.nNetCredit = NetCredit;
                        trialBalance.nNetDebit = NetDebit;
                        trialBalance.TempCostID = TempCostID;
                        trialBalance.TempCostType = TempCostType;
                        if (AccountData.FirstOrDefault(m => m.AccountFather == Acc.AccountNumber) != null)
                        {
                            trialBalance.IsMainAccount = 1;

                        }
                        else
                        {
                            trialBalance.IsMainAccount = 0;


                        }

                        if (!Obj.ShowZeroBalance)
                        {
                            if ((trialBalance.NetCredit - trialBalance.NetDebit) != 0)
                            {
                                TrialBalanceVMList.Add(trialBalance);
                            }
                        }
                        else
                        {
                            if ((trialBalance.NetCredit != 0) || (trialBalance.NetDebit != 0) || (TotalDebit != 0) || (NetCredit != 0))
                            {
                                TrialBalanceVMList.Add(trialBalance);
                            }


                        }
                    }

                }
                else if ((Obj.AccountLevelDropVMID > 0) && (Obj.Detail))
                {


                    Accounts = AccountData.Where(m => m.AccountLevel <= Obj.AccountLevelDropVMID).ToList().OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();

                    foreach (var Acc in Accounts)//Each Main Account
                    {
                        TotalDebit = 0;
                        TOTCredit = 0;
                        NetCredit = 0;
                        NetDebit = 0;
                        CreditBalance = 0;
                        DebitBalance = 0;
                        CreditTransAction = 0;
                        DebitTransAction = 0;

                        var MainAccount = AccountData.FirstOrDefault(m => m.AccountNumber == Acc.AccountNumber);
                        var MainChild = AccountData.Where(m => m.AccountFather == Acc.AccountNumber).OrderBy(m => m.AccountNumber).OrderBy(m => m.AccountLevel).ToList();

                        var TransActionData = TranData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber));
                        var BalanceData = TotData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber));
                        foreach (var D in TransActionData)
                        {
                            CreditTransAction += D.CreditTransAction;
                            DebitTransAction += D.DebitTransAction;
                        }
                        foreach (var Balance in BalanceData)
                        {
                            CreditBalance += Balance.CreditBalance;
                            DebitBalance += Balance.DebitBalance;
                        }



                        TotalDebit = DebitTransAction + DebitBalance;
                        TOTCredit = CreditTransAction + CreditBalance;

                        if (TotalDebit > TOTCredit)
                        {
                            NetDebit = TotalDebit - TOTCredit;
                        }
                        if (TotalDebit < TOTCredit)
                        {
                            NetCredit = TOTCredit - TotalDebit;
                        }

                        TrialBalanceVM trialBalance = new TrialBalanceVM();
                        trialBalance.AccountNumber = Acc.AccountNumber;
                        trialBalance.Name = Acc.ArabicName;

                        trialBalance.CreditBalance = CreditBalance;
                        trialBalance.CreditTransAction = CreditTransAction;
                        trialBalance.DebitBalance = DebitBalance;
                        trialBalance.DebitTransAction = DebitTransAction;
                        trialBalance.NetCredit = NetCredit;
                        trialBalance.NetDebit = NetDebit;
                        trialBalance.TempCostID = TempCostID;
                        trialBalance.TempCostType = TempCostType;
                        if ((AccountData.FirstOrDefault(m => m.AccountFather == Acc.AccountNumber) != null) && (Acc.AccountLevel < Obj.AccountLevelDropVMID))
                        {
                            trialBalance.IsMainAccount = 1;
                            trialBalance.MainAccount = "{*}";


                            trialBalance.nCreditBalance = 0;
                            trialBalance.nCreditTransAction = 0;
                            trialBalance.nDebitBalance = 0;
                            trialBalance.nDebitTransAction = 0;
                            trialBalance.nNetCredit = 0;
                            trialBalance.nNetDebit = 0;

                        }
                        else
                        {
                            trialBalance.IsMainAccount = 0;
                            trialBalance.MainAccount = "";
                            trialBalance.nCreditBalance = CreditBalance;
                            trialBalance.nCreditTransAction = CreditTransAction;
                            trialBalance.nDebitBalance = DebitBalance;
                            trialBalance.nDebitTransAction = DebitTransAction;
                            trialBalance.nNetCredit = NetCredit;
                            trialBalance.nNetDebit = NetDebit;


                        }
                        if (!Obj.ShowZeroBalance)
                        {
                            if ((trialBalance.NetCredit - trialBalance.NetDebit) != 0)
                            {
                                TrialBalanceVMList.Add(trialBalance);
                            }
                        }
                        else
                        {
                            if ((trialBalance.NetCredit != 0) || (trialBalance.NetDebit != 0) || (TotalDebit != 0) || (NetCredit != 0))
                            {
                                TrialBalanceVMList.Add(trialBalance);
                            }


                        }




                    }






                }




                return Json(TrialBalanceVMList, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                string err = ex.Message;

                List<TrialBalanceVM> TrialBalanceVMList = new List<TrialBalanceVM>();

                return Json(TrialBalanceVMList, JsonRequestBehavior.AllowGet);

            }


        }
        [HttpPost]
        public JsonResult GetTrialExpenseAnlysis(AccountLevelRepVM Obj)
        {
            try
            {

                var userId = User.Identity.GetUserId();
                string UserID = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
                var AccountData = _unitOfWork.NativeSql.GetChartOfAccountByLevel(UserInfo.fCompanyId);
                AccountData = AccountData.Where(m => m.AccountTypeID == 14 || m.AccountTypeID == 15 || m.AccountTypeID == 16).ToList();
                if (Obj.AccountTypeID != 0)
                {
                    AccountData = AccountData.Where(m => m.AccountTypeID == Obj.AccountTypeID).ToList();
                }

                int CurrYear = UserInfo.CurrYear;

                var Date = "01/01/" + CurrYear;
                var OpenDate = "31/12/" + (CurrYear - 1).ToString();

                string TempCostID = "0";
                int TempCostType = 0;

                if (Obj.ByCostCenter)
                {
                    TempCostID = Obj.CostCenterNumber;
                    if (Obj.Partofthenumber)
                    {
                        TempCostType = Obj.CostSearchType;
                    }


                }

                IEnumerable<TrialBalanceVM> TotData = new List<TrialBalanceVM>();
                if (Obj.Partofthenumber)
                {
                    if (Obj.CostSearchType == 1)
                    {
                        Obj.CostCenterNumber = Obj.CostCenterNumber + "%";
                    }
                    else if (Obj.CostSearchType == 2)
                    {
                        Obj.CostCenterNumber = "%" + Obj.CostCenterNumber;
                    }
                    else if (Obj.CostSearchType == 3)
                    {
                        Obj.CostCenterNumber = "%" + Obj.CostCenterNumber + "%";

                    }
                }

                var TranData = _unitOfWork.NativeSql.GetTransactionForTrial(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate, Obj.ByCostCenter, Obj.CostCenterNumber, Obj.Partofthenumber, Obj.CostSearchType);
                if (DateTime.Parse(Date) != Obj.FromDate)
                    TotData = _unitOfWork.NativeSql.GetTotCreditDebitForTrial(UserInfo.fCompanyId, DateTime.Parse(Date), Obj.FromDate, Obj.ByCostCenter, Obj.CostCenterNumber, Obj.Partofthenumber, Obj.CostSearchType);
                //----------------Get Open-------------//
                IEnumerable<TrialBalanceVM> TotDataOpen = new List<TrialBalanceVM>();
                TotDataOpen = _unitOfWork.NativeSql.GetTotCreditDebitForTrialOpen(UserInfo.fCompanyId, DateTime.Parse(OpenDate), DateTime.Parse(OpenDate));
                foreach (var D in TotDataOpen)
                {
                    TotData = TotData.Append(D);

                }
                //------------------------------------//



                double TotalDebit = 0;
                double TOTCredit = 0;
                double NetCredit = 0;
                double NetDebit = 0;
                double CreditBalance = 0;
                double DebitBalance = 0;
                double CreditTransAction = 0;
                double DebitTransAction = 0;

                List<TrialBalanceVM> TrialBalanceVMList = new List<TrialBalanceVM>();

                IEnumerable<ChartOfAccount> Accounts = new List<ChartOfAccount>();

                IEnumerable<ChartOfAccount> LessMainAccount = new List<ChartOfAccount>();
                if (!String.IsNullOrEmpty(Obj.AccNo))
                {

                    var MainAccount = AccountData.FirstOrDefault(m => m.AccountNumber == Obj.AccNo);
                    var MainChild = AccountData.Where(m => m.AccountFather == Obj.AccNo).OrderBy(m => m.AccountNumber).OrderBy(m => m.AccountLevel).ToList();

                    foreach (var Acc in MainChild)
                    {
                        TotalDebit = 0;
                        TOTCredit = 0;
                        NetCredit = 0;
                        NetDebit = 0;
                        CreditBalance = 0;
                        DebitBalance = 0;
                        CreditTransAction = 0;
                        DebitTransAction = 0;


                        if (AccountData.FirstOrDefault(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel) != null)//Check if Account Had Branches
                        {
                            var AllAccount = AccountData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel)
                                            .OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();//Get All Account Branches
                            int FatherLevel = Acc.AccountLevel;
                            foreach (var CurrAccLevel in AllAccount)
                            {
                                if (CurrAccLevel.AccountLevel == FatherLevel)
                                {
                                    break;
                                }
                                else
                                {

                                    var TransActionData = TranData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                    var BalanceData = TotData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                    foreach (var D in TransActionData)
                                    {
                                        CreditTransAction += D.CreditTransAction;
                                        DebitTransAction += D.DebitTransAction;
                                    }
                                    foreach (var Balance in BalanceData)
                                    {
                                        CreditBalance += Balance.CreditBalance;
                                        DebitBalance += Balance.DebitBalance;
                                    }



                                    TotalDebit = DebitTransAction + DebitBalance;
                                    TOTCredit = CreditTransAction + CreditBalance;




                                }
                            }




                        }
                        else
                        {
                            var TransActionData = TranData.Where(m => m.AccountNumber == Acc.AccountNumber);
                            var BalanceData = TotData.Where(m => m.AccountNumber == Acc.AccountNumber);
                            foreach (var D in TransActionData)
                            {
                                CreditTransAction += D.CreditTransAction;
                                DebitTransAction += D.DebitTransAction;
                            }
                            foreach (var Balance in BalanceData)
                            {
                                CreditBalance += Balance.CreditBalance;
                                DebitBalance += Balance.DebitBalance;
                            }



                            TotalDebit = DebitTransAction + DebitBalance;
                            TOTCredit = CreditTransAction + CreditBalance;
                        }
                        if (TotalDebit > TOTCredit)
                        {
                            NetDebit = TotalDebit - TOTCredit;
                        }
                        if (TotalDebit < TOTCredit)
                        {
                            NetCredit = TOTCredit - TotalDebit;
                        }

                        TrialBalanceVM trialBalance = new TrialBalanceVM();
                        trialBalance.AccountNumber = Acc.AccountNumber;
                        trialBalance.Name = Acc.ArabicName;

                        trialBalance.CreditBalance = CreditBalance;
                        trialBalance.CreditTransAction = CreditTransAction;
                        trialBalance.DebitBalance = DebitBalance;
                        trialBalance.DebitTransAction = DebitTransAction;
                        trialBalance.NetCredit = NetCredit;
                        trialBalance.NetDebit = NetDebit;
                        trialBalance.TempCostID = TempCostID;
                        trialBalance.TempCostType = TempCostType;

                        trialBalance.nCreditBalance = CreditBalance;
                        trialBalance.nCreditTransAction = CreditTransAction;
                        trialBalance.nDebitBalance = DebitBalance;
                        trialBalance.nDebitTransAction = DebitTransAction;
                        trialBalance.nNetCredit = NetCredit;
                        trialBalance.nNetDebit = NetDebit;


                        trialBalance.nCreditBalance = CreditBalance;
                        trialBalance.nCreditTransAction = CreditTransAction;
                        trialBalance.nDebitBalance = DebitBalance;
                        trialBalance.nDebitTransAction = DebitTransAction;
                        trialBalance.nNetCredit = NetCredit;
                        trialBalance.nNetDebit = NetDebit;

                        if (!Obj.ShowZeroBalance)
                        {
                            if ((trialBalance.NetCredit - trialBalance.NetDebit) != 0)
                            {
                                TrialBalanceVMList.Add(trialBalance);
                            }
                        }
                        else
                        {
                            if ((trialBalance.NetCredit != 0) || (trialBalance.NetDebit != 0) || (TotalDebit != 0) || (NetCredit != 0))
                            {
                                TrialBalanceVMList.Add(trialBalance);
                            }


                        }
                    }


                }
                else

                if ((Obj.AccountLevelDropVMID > 0) && (!Obj.Detail))
                {


                    Accounts = AccountData.Where(m => m.AccountLevel == Obj.AccountLevelDropVMID).OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();

                    LessMainAccount = AccountData.Where(m => m.AccountLevel < Obj.AccountLevelDropVMID).OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();

                    IEnumerable<string> FaherAccount = LessMainAccount.Where(m => m.AccountFather != "0").Select(m => m.AccountFather);


                    foreach (var A in LessMainAccount)
                    {
                        if (AccountData.FirstOrDefault(m => m.AccountFather == A.AccountNumber) != null)
                        {
                            //do nothing
                        }
                        else

                        {
                            Accounts = Accounts.Append(A);
                        }

                    }
                    Accounts = Accounts.OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();
                    // var MainChild = AccountData.Where(m => m.AccountFather == MainAcc.AccountNumber).OrderBy(m => m.AccountNumber).OrderBy(m => m.AccountLevel).ToList();
                    foreach (var Acc in Accounts)
                    {
                        TotalDebit = 0;
                        TOTCredit = 0;
                        NetCredit = 0;
                        NetDebit = 0;
                        CreditBalance = 0;
                        DebitBalance = 0;
                        CreditTransAction = 0;
                        DebitTransAction = 0;


                        if (AccountData.FirstOrDefault(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel) != null)
                        {
                            var AllAccount = AccountData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel)
                                            .OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();
                            int FatherLevel = Acc.AccountLevel;

                            foreach (var CurrAccLevel in AllAccount)
                            {
                                if (CurrAccLevel.AccountLevel == FatherLevel)
                                {
                                    break;
                                }
                                else
                                {
                                    var TransActionData = TranData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                    var BalanceData = TotData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                    foreach (var D in TransActionData)
                                    {
                                        CreditTransAction += D.CreditTransAction;
                                        DebitTransAction += D.DebitTransAction;
                                    }
                                    foreach (var Balance in BalanceData)
                                    {
                                        CreditBalance += Balance.CreditBalance;
                                        DebitBalance += Balance.DebitBalance;
                                    }
                                    TotalDebit = DebitTransAction + DebitBalance;
                                    TOTCredit = CreditTransAction + CreditBalance;
                                }
                            }
                        }
                        else
                        {
                            var TransActionData = TranData.Where(m => m.AccountNumber == Acc.AccountNumber);
                            var BalanceData = TotData.Where(m => m.AccountNumber == Acc.AccountNumber);
                            foreach (var D in TransActionData)
                            {
                                CreditTransAction += D.CreditTransAction;
                                DebitTransAction += D.DebitTransAction;
                            }
                            foreach (var Balance in BalanceData)
                            {
                                CreditBalance += Balance.CreditBalance;
                                DebitBalance += Balance.DebitBalance;
                            }

                            TotalDebit = DebitTransAction + DebitBalance;
                            TOTCredit = CreditTransAction + CreditBalance;
                        }
                        if (TotalDebit > TOTCredit)
                        {
                            NetDebit = TotalDebit - TOTCredit;
                        }
                        if (TotalDebit < TOTCredit)
                        {
                            NetCredit = TOTCredit - TotalDebit;
                        }

                        TrialBalanceVM trialBalance = new TrialBalanceVM();
                        trialBalance.AccountNumber = Acc.AccountNumber;
                        trialBalance.Name = Acc.ArabicName;
                        trialBalance.Level = Acc.AccountLevel;
                        trialBalance.CreditBalance = CreditBalance;
                        trialBalance.CreditTransAction = CreditTransAction;
                        trialBalance.DebitBalance = DebitBalance;
                        trialBalance.DebitTransAction = DebitTransAction;
                        trialBalance.NetCredit = NetCredit;
                        trialBalance.NetDebit = NetDebit;

                        trialBalance.nCreditBalance = CreditBalance;
                        trialBalance.nCreditTransAction = CreditTransAction;
                        trialBalance.nDebitBalance = DebitBalance;
                        trialBalance.nDebitTransAction = DebitTransAction;
                        trialBalance.nNetCredit = NetCredit;
                        trialBalance.nNetDebit = NetDebit;
                        trialBalance.TempCostID = TempCostID;
                        trialBalance.TempCostType = TempCostType;
                        if (AccountData.FirstOrDefault(m => m.AccountFather == Acc.AccountNumber) != null)
                        {
                            trialBalance.IsMainAccount = 1;

                        }
                        else
                        {
                            trialBalance.IsMainAccount = 0;


                        }

                        if (!Obj.ShowZeroBalance)
                        {
                            if ((trialBalance.NetCredit - trialBalance.NetDebit) != 0)
                            {
                                TrialBalanceVMList.Add(trialBalance);
                            }
                        }
                        else
                        {
                            if ((trialBalance.NetCredit != 0) || (trialBalance.NetDebit != 0) || (TotalDebit != 0) || (NetCredit != 0))
                            {
                                TrialBalanceVMList.Add(trialBalance);
                            }


                        }
                    }

                }
                else if ((Obj.AccountLevelDropVMID > 0) && (Obj.Detail))
                {


                    Accounts = AccountData.Where(m => m.AccountLevel <= Obj.AccountLevelDropVMID).ToList().OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();

                    foreach (var Acc in Accounts)//Each Main Account
                    {
                        TotalDebit = 0;
                        TOTCredit = 0;
                        NetCredit = 0;
                        NetDebit = 0;
                        CreditBalance = 0;
                        DebitBalance = 0;
                        CreditTransAction = 0;
                        DebitTransAction = 0;

                        var MainAccount = AccountData.FirstOrDefault(m => m.AccountNumber == Acc.AccountNumber);
                        var MainChild = AccountData.Where(m => m.AccountFather == Acc.AccountNumber).OrderBy(m => m.AccountNumber).OrderBy(m => m.AccountLevel).ToList();

                        var TransActionData = TranData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber));
                        var BalanceData = TotData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber));
                        foreach (var D in TransActionData)
                        {
                            CreditTransAction += D.CreditTransAction;
                            DebitTransAction += D.DebitTransAction;
                        }
                        foreach (var Balance in BalanceData)
                        {
                            CreditBalance += Balance.CreditBalance;
                            DebitBalance += Balance.DebitBalance;
                        }



                        TotalDebit = DebitTransAction + DebitBalance;
                        TOTCredit = CreditTransAction + CreditBalance;

                        if (TotalDebit > TOTCredit)
                        {
                            NetDebit = TotalDebit - TOTCredit;
                        }
                        if (TotalDebit < TOTCredit)
                        {
                            NetCredit = TOTCredit - TotalDebit;
                        }

                        TrialBalanceVM trialBalance = new TrialBalanceVM();
                        trialBalance.AccountNumber = Acc.AccountNumber;
                        trialBalance.Name = Acc.ArabicName;

                        trialBalance.CreditBalance = CreditBalance;
                        trialBalance.CreditTransAction = CreditTransAction;
                        trialBalance.DebitBalance = DebitBalance;
                        trialBalance.DebitTransAction = DebitTransAction;
                        trialBalance.NetCredit = NetCredit;
                        trialBalance.NetDebit = NetDebit;
                        trialBalance.TempCostID = TempCostID;
                        trialBalance.TempCostType = TempCostType;
                        if ((AccountData.FirstOrDefault(m => m.AccountFather == Acc.AccountNumber) != null) && (Acc.AccountLevel < Obj.AccountLevelDropVMID))
                        {
                            trialBalance.IsMainAccount = 1;
                            trialBalance.MainAccount = "{*}";


                            trialBalance.nCreditBalance = 0;
                            trialBalance.nCreditTransAction = 0;
                            trialBalance.nDebitBalance = 0;
                            trialBalance.nDebitTransAction = 0;
                            trialBalance.nNetCredit = 0;
                            trialBalance.nNetDebit = 0;

                        }
                        else
                        {
                            trialBalance.IsMainAccount = 0;
                            trialBalance.MainAccount = "";
                            trialBalance.nCreditBalance = CreditBalance;
                            trialBalance.nCreditTransAction = CreditTransAction;
                            trialBalance.nDebitBalance = DebitBalance;
                            trialBalance.nDebitTransAction = DebitTransAction;
                            trialBalance.nNetCredit = NetCredit;
                            trialBalance.nNetDebit = NetDebit;


                        }
                        if (!Obj.ShowZeroBalance)
                        {
                            if ((trialBalance.NetCredit - trialBalance.NetDebit) != 0)
                            {
                                TrialBalanceVMList.Add(trialBalance);
                            }
                        }
                        else
                        {
                            if ((trialBalance.NetCredit != 0) || (trialBalance.NetDebit != 0) || (TotalDebit != 0) || (NetCredit != 0))
                            {
                                TrialBalanceVMList.Add(trialBalance);
                            }


                        }




                    }






                }




                return Json(TrialBalanceVMList, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                string err = ex.Message;

                List<TrialBalanceVM> TrialBalanceVMList = new List<TrialBalanceVM>();

                return Json(TrialBalanceVMList, JsonRequestBehavior.AllowGet);

            }


        }
        [HttpPost]
        public JsonResult GetTrialExpenseAnlysisDetail(AccountLevelRepVM Obj)
        {
          

            try
            {
                var userId = User.Identity.GetUserId();
                string UserID = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
                var AccountData = _unitOfWork.NativeSql.GetChartOfAccountByLevel(UserInfo.fCompanyId);

                
                if (Obj.AccountTypeID != 0)
                {
                    AccountData = AccountData.Where(m => m.AccountTypeID == Obj.AccountTypeID).ToList();
                }

             
                int CurrYear = UserInfo.CurrYear;

                var Date = "01/01/" + CurrYear;
                var OpenDate = "31/12/" + (CurrYear - 1).ToString();

                string TempCostID = "0";
                int TempCostType = 0;

                if (Obj.ByCostCenter)
                {
                    TempCostID = Obj.CostCenterNumber;
                    if (Obj.Partofthenumber)
                    {
                        TempCostType = Obj.CostSearchType;
                    }


                }

                
                IEnumerable<TrialBalanceVM> TotData = new List<TrialBalanceVM>();

               
                if (Obj.Partofthenumber)
                {
                    if (Obj.CostSearchType == 1)
                    {
                        Obj.CostCenterNumber = Obj.CostCenterNumber + "%";
                    }
                    else if (Obj.CostSearchType == 2)
                    {
                        Obj.CostCenterNumber = "%" + Obj.CostCenterNumber;
                    }
                    else if (Obj.CostSearchType == 3)
                    {
                        Obj.CostCenterNumber = "%" + Obj.CostCenterNumber + "%";

                    }
                }

                var TranData = _unitOfWork.NativeSql.GetTransactionForTrial(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate, Obj.ByCostCenter, Obj.CostCenterNumber, Obj.Partofthenumber, Obj.CostSearchType);
                if (DateTime.Parse(Date) != Obj.FromDate)
                    TotData = _unitOfWork.NativeSql.GetTotCreditDebitForTrial(UserInfo.fCompanyId, DateTime.Parse(Date), Obj.FromDate, Obj.ByCostCenter, Obj.CostCenterNumber, Obj.Partofthenumber, Obj.CostSearchType);
                //----------------Get Open-------------//
                IEnumerable<TrialBalanceVM> TotDataOpen = new List<TrialBalanceVM>();
                TotDataOpen = _unitOfWork.NativeSql.GetTotCreditDebitForTrialOpen(UserInfo.fCompanyId, DateTime.Parse(OpenDate), DateTime.Parse(OpenDate));
                foreach (var D in TotDataOpen)
                {
                    TotData = TotData.Append(D);

                }
                //------------------------------------//

                
                double TotalDebit = 0;
                double TOTCredit = 0;
                double NetCredit = 0;
                double NetDebit = 0;
                double CreditBalance = 0;
                double DebitBalance = 0;
                double CreditTransAction = 0;
                double DebitTransAction = 0;

                List<TrialBalanceVM> TrialBalanceVMList = new List<TrialBalanceVM>();

                IEnumerable<ChartOfAccount> Accounts = new List<ChartOfAccount>();

                IEnumerable<ChartOfAccount> LessMainAccount = new List<ChartOfAccount>();
                if (!String.IsNullOrEmpty(Obj.AccNo))
                {

                    var MainAccount = AccountData.FirstOrDefault(m => m.AccountNumber == Obj.AccNo);
                    var MainChild = AccountData.Where(m => m.AccountFather == Obj.AccNo).OrderBy(m => m.AccountNumber).OrderBy(m => m.AccountLevel).ToList();

                    foreach (var Acc in MainChild)
                    {
                        TotalDebit = 0;
                        TOTCredit = 0;
                        NetCredit = 0;
                        NetDebit = 0;
                        CreditBalance = 0;
                        DebitBalance = 0;
                        CreditTransAction = 0;
                        DebitTransAction = 0;


                        if (AccountData.FirstOrDefault(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel) != null)//Check if Account Had Branches
                        {
                            var AllAccount = AccountData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel)
                                            .OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();//Get All Account Branches
                            int FatherLevel = Acc.AccountLevel;
                            foreach (var CurrAccLevel in AllAccount)
                            {
                                if (CurrAccLevel.AccountLevel == FatherLevel)
                                {
                                    break;
                                }
                                else
                                {

                                    var TransActionData = TranData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                    var BalanceData = TotData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                    foreach (var D in TransActionData)
                                    {
                                        CreditTransAction += D.CreditTransAction;
                                        DebitTransAction += D.DebitTransAction;
                                    }
                                    foreach (var Balance in BalanceData)
                                    {
                                        CreditBalance += Balance.CreditBalance;
                                        DebitBalance += Balance.DebitBalance;
                                    }



                                    TotalDebit = DebitTransAction + DebitBalance;
                                    TOTCredit = CreditTransAction + CreditBalance;




                                }
                            }




                        }
                        else
                        {
                            var TransActionData = TranData.Where(m => m.AccountNumber == Acc.AccountNumber);
                            var BalanceData = TotData.Where(m => m.AccountNumber == Acc.AccountNumber);
                            foreach (var D in TransActionData)
                            {
                                CreditTransAction += D.CreditTransAction;
                                DebitTransAction += D.DebitTransAction;
                            }
                            foreach (var Balance in BalanceData)
                            {
                                CreditBalance += Balance.CreditBalance;
                                DebitBalance += Balance.DebitBalance;
                            }



                            TotalDebit = DebitTransAction + DebitBalance;
                            TOTCredit = CreditTransAction + CreditBalance;
                        }
                        if (TotalDebit > TOTCredit)
                        {
                            NetDebit = TotalDebit - TOTCredit;
                        }
                        if (TotalDebit < TOTCredit)
                        {
                            NetCredit = TOTCredit - TotalDebit;
                        }

                        TrialBalanceVM trialBalance = new TrialBalanceVM();
                        trialBalance.AccountNumber = Acc.AccountNumber;
                        trialBalance.Name = Acc.ArabicName;

                        trialBalance.CreditBalance = CreditBalance;
                        trialBalance.CreditTransAction = CreditTransAction;
                        trialBalance.DebitBalance = DebitBalance;
                        trialBalance.DebitTransAction = DebitTransAction;
                        trialBalance.NetCredit = NetCredit;
                        trialBalance.NetDebit = NetDebit;
                        trialBalance.TempCostID = TempCostID;
                        trialBalance.TempCostType = TempCostType;



                        trialBalance.nCreditBalance = CreditBalance;
                        trialBalance.nCreditTransAction = CreditTransAction;
                        trialBalance.nDebitBalance = DebitBalance;
                        trialBalance.nDebitTransAction = DebitTransAction;
                        trialBalance.nNetCredit = NetCredit;
                        trialBalance.nNetDebit = NetDebit;

                        if (!Obj.ShowZeroBalance)
                        {
                            if ((trialBalance.NetCredit - trialBalance.NetDebit) != 0)
                            {
                                TrialBalanceVMList.Add(trialBalance);
                            }
                        }
                        else
                        {
                            if ((trialBalance.NetCredit != 0) || (trialBalance.NetDebit != 0) || (TotalDebit != 0) || (NetCredit != 0))
                            {
                                TrialBalanceVMList.Add(trialBalance);
                            }


                        }
                    }


                }
                else

                if ((Obj.AccountLevelDropVMID > 0) && (!Obj.Detail))
                {


                    Accounts = AccountData.Where(m => m.AccountLevel == Obj.AccountLevelDropVMID).OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();

                    LessMainAccount = AccountData.Where(m => m.AccountLevel < Obj.AccountLevelDropVMID).OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();

                    IEnumerable<string> FaherAccount = LessMainAccount.Where(m => m.AccountFather != "0").Select(m => m.AccountFather);


                    foreach (var A in LessMainAccount)
                    {
                        if (AccountData.FirstOrDefault(m => m.AccountFather == A.AccountNumber) != null)
                        {
                            //do nothing
                        }
                        else

                        {
                            Accounts = Accounts.Append(A);
                        }

                    }
                    Accounts = Accounts.OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();
                    // var MainChild = AccountData.Where(m => m.AccountFather == MainAcc.AccountNumber).OrderBy(m => m.AccountNumber).OrderBy(m => m.AccountLevel).ToList();
                    foreach (var Acc in Accounts)
                    {
                        TotalDebit = 0;
                        TOTCredit = 0;
                        NetCredit = 0;
                        NetDebit = 0;
                        CreditBalance = 0;
                        DebitBalance = 0;
                        CreditTransAction = 0;
                        DebitTransAction = 0;


                        if (AccountData.FirstOrDefault(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel) != null)
                        {
                            var AllAccount = AccountData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel)
                                            .OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();
                            int FatherLevel = Acc.AccountLevel;

                            foreach (var CurrAccLevel in AllAccount)
                            {
                                if (CurrAccLevel.AccountLevel == FatherLevel)
                                {
                                    break;
                                }
                                else
                                {
                                    var TransActionData = TranData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                    var BalanceData = TotData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                    foreach (var D in TransActionData)
                                    {
                                        CreditTransAction += D.CreditTransAction;
                                        DebitTransAction += D.DebitTransAction;
                                    }
                                    foreach (var Balance in BalanceData)
                                    {
                                        CreditBalance += Balance.CreditBalance;
                                        DebitBalance += Balance.DebitBalance;
                                    }
                                    TotalDebit = DebitTransAction + DebitBalance;
                                    TOTCredit = CreditTransAction + CreditBalance;
                                }
                            }
                        }
                        else
                        {
                            var TransActionData = TranData.Where(m => m.AccountNumber == Acc.AccountNumber);
                            var BalanceData = TotData.Where(m => m.AccountNumber == Acc.AccountNumber);
                            foreach (var D in TransActionData)
                            {
                                CreditTransAction += D.CreditTransAction;
                                DebitTransAction += D.DebitTransAction;
                            }
                            foreach (var Balance in BalanceData)
                            {
                                CreditBalance += Balance.CreditBalance;
                                DebitBalance += Balance.DebitBalance;
                            }

                            TotalDebit = DebitTransAction + DebitBalance;
                            TOTCredit = CreditTransAction + CreditBalance;
                        }
                        if (TotalDebit > TOTCredit)
                        {
                            NetDebit = TotalDebit - TOTCredit;
                        }
                        if (TotalDebit < TOTCredit)
                        {
                            NetCredit = TOTCredit - TotalDebit;
                        }

                        TrialBalanceVM trialBalance = new TrialBalanceVM();
                        trialBalance.AccountNumber = Acc.AccountNumber;
                        trialBalance.Name = Acc.ArabicName;
                        trialBalance.Level = Acc.AccountLevel;
                        trialBalance.CreditBalance = CreditBalance;
                        trialBalance.CreditTransAction = CreditTransAction;
                        trialBalance.DebitBalance = DebitBalance;
                        trialBalance.DebitTransAction = DebitTransAction;
                        trialBalance.NetCredit = NetCredit;
                        trialBalance.NetDebit = NetDebit;

                        trialBalance.nCreditBalance = CreditBalance;
                        trialBalance.nCreditTransAction = CreditTransAction;
                        trialBalance.nDebitBalance = DebitBalance;
                        trialBalance.nDebitTransAction = DebitTransAction;
                        trialBalance.nNetCredit = NetCredit;
                        trialBalance.nNetDebit = NetDebit;
                        trialBalance.TempCostID = TempCostID;
                        trialBalance.TempCostType = TempCostType;
                        if (AccountData.FirstOrDefault(m => m.AccountFather == Acc.AccountNumber) != null)
                        {
                            trialBalance.IsMainAccount = 1;

                        }
                        else
                        {
                            trialBalance.IsMainAccount = 0;


                        }

                        if (!Obj.ShowZeroBalance)
                        {
                            if ((trialBalance.NetCredit - trialBalance.NetDebit) != 0)
                            {
                                TrialBalanceVMList.Add(trialBalance);
                            }
                        }
                        else
                        {
                            if ((trialBalance.NetCredit != 0) || (trialBalance.NetDebit != 0) || (TotalDebit != 0) || (NetCredit != 0))
                            {
                                TrialBalanceVMList.Add(trialBalance);
                            }


                        }
                    }

                }
                else if ((Obj.AccountLevelDropVMID > 0) && (Obj.Detail))
                {


                    Accounts = AccountData.Where(m => m.AccountLevel <= Obj.AccountLevelDropVMID).ToList().OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();

                    foreach (var Acc in Accounts)//Each Main Account
                    {
                        TotalDebit = 0;
                        TOTCredit = 0;
                        NetCredit = 0;
                        NetDebit = 0;
                        CreditBalance = 0;
                        DebitBalance = 0;
                        CreditTransAction = 0;
                        DebitTransAction = 0;

                        var MainAccount = AccountData.FirstOrDefault(m => m.AccountNumber == Acc.AccountNumber);
                        var MainChild = AccountData.Where(m => m.AccountFather == Acc.AccountNumber).OrderBy(m => m.AccountNumber).OrderBy(m => m.AccountLevel).ToList();

                        var TransActionData = TranData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber));
                        var BalanceData = TotData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber));
                        foreach (var D in TransActionData)
                        {
                            CreditTransAction += D.CreditTransAction;
                            DebitTransAction += D.DebitTransAction;
                        }
                        foreach (var Balance in BalanceData)
                        {
                            CreditBalance += Balance.CreditBalance;
                            DebitBalance += Balance.DebitBalance;
                        }



                        TotalDebit = DebitTransAction + DebitBalance;
                        TOTCredit = CreditTransAction + CreditBalance;

                        if (TotalDebit > TOTCredit)
                        {
                            NetDebit = TotalDebit - TOTCredit;
                        }
                        if (TotalDebit < TOTCredit)
                        {
                            NetCredit = TOTCredit - TotalDebit;
                        }

                        TrialBalanceVM trialBalance = new TrialBalanceVM();
                        trialBalance.AccountNumber = Acc.AccountNumber;
                        trialBalance.Name = Acc.ArabicName;

                        trialBalance.CreditBalance = CreditBalance;
                        trialBalance.CreditTransAction = CreditTransAction;
                        trialBalance.DebitBalance = DebitBalance;
                        trialBalance.DebitTransAction = DebitTransAction;
                        trialBalance.NetCredit = NetCredit;
                        trialBalance.NetDebit = NetDebit;
                        trialBalance.TempCostID = TempCostID;
                        trialBalance.TempCostType = TempCostType;
                        if ((AccountData.FirstOrDefault(m => m.AccountFather == Acc.AccountNumber) != null) && (Acc.AccountLevel < Obj.AccountLevelDropVMID))
                        {
                            trialBalance.IsMainAccount = 1;
                            trialBalance.MainAccount = "{*}";


                            trialBalance.nCreditBalance = 0;
                            trialBalance.nCreditTransAction = 0;
                            trialBalance.nDebitBalance = 0;
                            trialBalance.nDebitTransAction = 0;
                            trialBalance.nNetCredit = 0;
                            trialBalance.nNetDebit = 0;

                        }
                        else
                        {
                            trialBalance.IsMainAccount = 0;
                            trialBalance.MainAccount = "";
                            trialBalance.nCreditBalance = CreditBalance;
                            trialBalance.nCreditTransAction = CreditTransAction;
                            trialBalance.nDebitBalance = DebitBalance;
                            trialBalance.nDebitTransAction = DebitTransAction;
                            trialBalance.nNetCredit = NetCredit;
                            trialBalance.nNetDebit = NetDebit;


                        }
                        if (!Obj.ShowZeroBalance)
                        {
                            if ((trialBalance.NetCredit - trialBalance.NetDebit) != 0)
                            {
                                TrialBalanceVMList.Add(trialBalance);
                            }
                        }
                        else
                        {
                            if ((trialBalance.NetCredit != 0) || (trialBalance.NetDebit != 0) || (TotalDebit != 0) || (NetCredit != 0))
                            {
                                TrialBalanceVMList.Add(trialBalance);
                            }


                        }




                    }






                }


                if (Obj.OrderBy == 1)
                {
                    TrialBalanceVMList = TrialBalanceVMList.OrderByDescending(m => m.NetDebit).ThenBy(m => m.NetCredit).ToList();
                }
                else if (Obj.OrderBy == 2)
                {
                    TrialBalanceVMList = TrialBalanceVMList.OrderBy(m => m.NetDebit).ThenBy(m => m.NetCredit).ToList();
                }


                return Json(TrialBalanceVMList, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                string err = ex.Message;

                List<TrialBalanceVM> TrialBalanceVMList = new List<TrialBalanceVM>();

                return Json(TrialBalanceVMList, JsonRequestBehavior.AllowGet);

            }


        }
        [HttpPost]
        public JsonResult GetTrialBalanceForCustomer(AccountLevelRepVM Obj)
        {
            try
            {

                var userId = User.Identity.GetUserId();
                string UserID = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
                IEnumerable <ChartOfAccount> AccountData = new List<ChartOfAccount>();

                if (Obj.CityID > 0)
                {
                    AccountData = _unitOfWork.NativeSql.GetChartOfAccountByLevelAndCity(UserInfo.fCompanyId,Obj.CityID,Obj.AreaID);
                }
                else
                {
                    AccountData = _unitOfWork.NativeSql.GetChartOfAccountByLevel(UserInfo.fCompanyId);
                }
               
                AccountData= AccountData.Where(m=>m.AccountKind == 1).ToList();

              

                int CurrYear = UserInfo.CurrYear;

                var Date = "01/01/" + CurrYear;
                var OpenDate = "31/12/" + (CurrYear - 1).ToString();

                string TempCostID = "0";
                int TempCostType = 0;

                if (Obj.ByCostCenter)
                {
                    TempCostID = Obj.CostCenterNumber;
                    if (Obj.Partofthenumber)
                    {
                        TempCostType = Obj.CostSearchType;
                    }


                }

                IEnumerable<TrialBalanceVM> TotData = new List<TrialBalanceVM>();
                if (Obj.Partofthenumber)
                {
                    if (Obj.CostSearchType == 1)
                    {
                        Obj.CostCenterNumber = Obj.CostCenterNumber + "%";
                    }
                    else if (Obj.CostSearchType == 2)
                    {
                        Obj.CostCenterNumber = "%" + Obj.CostCenterNumber;
                    }
                    else if (Obj.CostSearchType == 3)
                    {
                        Obj.CostCenterNumber = "%" + Obj.CostCenterNumber + "%";

                    }
                }

                var TranData = _unitOfWork.NativeSql.GetTransactionForTrial(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate, Obj.ByCostCenter, Obj.CostCenterNumber, Obj.Partofthenumber, Obj.CostSearchType);
                if (DateTime.Parse(Date) != Obj.FromDate)
                    TotData = _unitOfWork.NativeSql.GetTotCreditDebitForTrial(UserInfo.fCompanyId, DateTime.Parse(Date), Obj.FromDate, Obj.ByCostCenter, Obj.CostCenterNumber, Obj.Partofthenumber, Obj.CostSearchType);
                //----------------Get Open-------------//
                IEnumerable<TrialBalanceVM> TotDataOpen = new List<TrialBalanceVM>();
                TotDataOpen = _unitOfWork.NativeSql.GetTotCreditDebitForTrialOpen(UserInfo.fCompanyId, DateTime.Parse(OpenDate), DateTime.Parse(OpenDate));
                foreach (var D in TotDataOpen)
                {
                    TotData = TotData.Append(D);

                }
                //------------------------------------//



                double TotalDebit = 0;
                double TOTCredit = 0;
                double NetCredit = 0;
                double NetDebit = 0;
                double CreditBalance = 0;
                double DebitBalance = 0;
                double CreditTransAction = 0;
                double DebitTransAction = 0;

                List<TrialBalanceVM> TrialBalanceVMList = new List<TrialBalanceVM>();

                IEnumerable<ChartOfAccount> Accounts = new List<ChartOfAccount>();

                IEnumerable<ChartOfAccount> LessMainAccount = new List<ChartOfAccount>();
                if (!String.IsNullOrEmpty(Obj.AccNo))
                {

                    var MainAccount = AccountData.FirstOrDefault(m => m.AccountNumber == Obj.AccNo);
                    var MainChild = AccountData.Where(m => m.AccountFather == Obj.AccNo).OrderBy(m => m.AccountNumber).OrderBy(m => m.AccountLevel).ToList();

                    foreach (var Acc in MainChild)
                    {
                        TotalDebit = 0;
                        TOTCredit = 0;
                        NetCredit = 0;
                        NetDebit = 0;
                        CreditBalance = 0;
                        DebitBalance = 0;
                        CreditTransAction = 0;
                        DebitTransAction = 0;


                        if (AccountData.FirstOrDefault(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel) != null)//Check if Account Had Branches
                        {
                            var AllAccount = AccountData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel)
                                            .OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();//Get All Account Branches
                            int FatherLevel = Acc.AccountLevel;
                            foreach (var CurrAccLevel in AllAccount)
                            {
                                if (CurrAccLevel.AccountLevel == FatherLevel)
                                {
                                    break;
                                }
                                else
                                {

                                    var TransActionData = TranData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                    var BalanceData = TotData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                    foreach (var D in TransActionData)
                                    {
                                        CreditTransAction += D.CreditTransAction;
                                        DebitTransAction += D.DebitTransAction;
                                    }
                                    foreach (var Balance in BalanceData)
                                    {
                                        CreditBalance += Balance.CreditBalance;
                                        DebitBalance += Balance.DebitBalance;
                                    }



                                    TotalDebit = DebitTransAction + DebitBalance;
                                    TOTCredit = CreditTransAction + CreditBalance;




                                }
                            }




                        }
                        else
                        {
                            var TransActionData = TranData.Where(m => m.AccountNumber == Acc.AccountNumber);
                            var BalanceData = TotData.Where(m => m.AccountNumber == Acc.AccountNumber);
                            foreach (var D in TransActionData)
                            {
                                CreditTransAction += D.CreditTransAction;
                                DebitTransAction += D.DebitTransAction;
                            }
                            foreach (var Balance in BalanceData)
                            {
                                CreditBalance += Balance.CreditBalance;
                                DebitBalance += Balance.DebitBalance;
                            }



                            TotalDebit = DebitTransAction + DebitBalance;
                            TOTCredit = CreditTransAction + CreditBalance;
                        }
                        if (TotalDebit > TOTCredit)
                        {
                            NetDebit = TotalDebit - TOTCredit;
                        }
                        if (TotalDebit < TOTCredit)
                        {
                            NetCredit = TOTCredit - TotalDebit;
                        }

                        TrialBalanceVM trialBalance = new TrialBalanceVM();
                        trialBalance.AccountNumber = Acc.AccountNumber;
                        trialBalance.Name = Acc.ArabicName;

                        trialBalance.CreditBalance = CreditBalance;
                        trialBalance.CreditTransAction = CreditTransAction;
                        trialBalance.DebitBalance = DebitBalance;
                        trialBalance.DebitTransAction = DebitTransAction;
                        trialBalance.NetCredit = NetCredit;
                        trialBalance.NetDebit = NetDebit;
                        trialBalance.TempCostID = TempCostID;
                        trialBalance.TempCostType = TempCostType;

                        trialBalance.nCreditBalance = CreditBalance;
                        trialBalance.nCreditTransAction = CreditTransAction;
                        trialBalance.nDebitBalance = DebitBalance;
                        trialBalance.nDebitTransAction = DebitTransAction;
                        trialBalance.nNetCredit = NetCredit;
                        trialBalance.nNetDebit = NetDebit;


                        if (!Obj.ShowZeroBalance)
                        {
                            if ((trialBalance.NetCredit - trialBalance.NetDebit) != 0)
                            {
                                TrialBalanceVMList.Add(trialBalance);
                            }
                        }
                        else
                        {
                            if ((trialBalance.NetCredit != 0) || (trialBalance.NetDebit != 0) || (TotalDebit != 0) || (NetCredit != 0))
                            {
                                TrialBalanceVMList.Add(trialBalance);
                            }


                        }
                    }


                }
                else

                if ((Obj.AccountLevelDropVMID > 0) && (!Obj.Detail))
                {


                    Accounts = AccountData.Where(m => m.AccountLevel == Obj.AccountLevelDropVMID).OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();

                    LessMainAccount = AccountData.Where(m => m.AccountLevel < Obj.AccountLevelDropVMID).OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();

                    IEnumerable<string> FaherAccount = LessMainAccount.Where(m => m.AccountFather != "0").Select(m => m.AccountFather);


                    foreach (var A in LessMainAccount)
                    {
                        if (AccountData.FirstOrDefault(m => m.AccountFather == A.AccountNumber) != null)
                        {
                            //do nothing
                        }
                        else

                        {
                            Accounts = Accounts.Append(A);
                        }

                    }
                    Accounts = Accounts.OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();
                    // var MainChild = AccountData.Where(m => m.AccountFather == MainAcc.AccountNumber).OrderBy(m => m.AccountNumber).OrderBy(m => m.AccountLevel).ToList();
                    foreach (var Acc in Accounts)
                    {
                        TotalDebit = 0;
                        TOTCredit = 0;
                        NetCredit = 0;
                        NetDebit = 0;
                        CreditBalance = 0;
                        DebitBalance = 0;
                        CreditTransAction = 0;
                        DebitTransAction = 0;


                        if (AccountData.FirstOrDefault(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel) != null)
                        {
                            var AllAccount = AccountData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel)
                                            .OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();
                            int FatherLevel = Acc.AccountLevel;

                            foreach (var CurrAccLevel in AllAccount)
                            {
                                if (CurrAccLevel.AccountLevel == FatherLevel)
                                {
                                    break;
                                }
                                else
                                {
                                    var TransActionData = TranData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                    var BalanceData = TotData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                    foreach (var D in TransActionData)
                                    {
                                        CreditTransAction += D.CreditTransAction;
                                        DebitTransAction += D.DebitTransAction;
                                    }
                                    foreach (var Balance in BalanceData)
                                    {
                                        CreditBalance += Balance.CreditBalance;
                                        DebitBalance += Balance.DebitBalance;
                                    }
                                    TotalDebit = DebitTransAction + DebitBalance;
                                    TOTCredit = CreditTransAction + CreditBalance;
                                }
                            }
                        }
                        else
                        {
                            var TransActionData = TranData.Where(m => m.AccountNumber == Acc.AccountNumber);
                            var BalanceData = TotData.Where(m => m.AccountNumber == Acc.AccountNumber);
                            foreach (var D in TransActionData)
                            {
                                CreditTransAction += D.CreditTransAction;
                                DebitTransAction += D.DebitTransAction;
                            }
                            foreach (var Balance in BalanceData)
                            {
                                CreditBalance += Balance.CreditBalance;
                                DebitBalance += Balance.DebitBalance;
                            }

                            TotalDebit = DebitTransAction + DebitBalance;
                            TOTCredit = CreditTransAction + CreditBalance;
                        }
                        if (TotalDebit > TOTCredit)
                        {
                            NetDebit = TotalDebit - TOTCredit;
                        }
                        if (TotalDebit < TOTCredit)
                        {
                            NetCredit = TOTCredit - TotalDebit;
                        }

                        TrialBalanceVM trialBalance = new TrialBalanceVM();
                        trialBalance.AccountNumber = Acc.AccountNumber;
                        trialBalance.Name = Acc.ArabicName;
                        trialBalance.Level = Acc.AccountLevel;
                        trialBalance.CreditBalance = CreditBalance;
                        trialBalance.CreditTransAction = CreditTransAction;
                        trialBalance.DebitBalance = DebitBalance;
                        trialBalance.DebitTransAction = DebitTransAction;
                        trialBalance.NetCredit = NetCredit;
                        trialBalance.NetDebit = NetDebit;

                        trialBalance.nCreditBalance = CreditBalance;
                        trialBalance.nCreditTransAction = CreditTransAction;
                        trialBalance.nDebitBalance = DebitBalance;
                        trialBalance.nDebitTransAction = DebitTransAction;
                        trialBalance.nNetCredit = NetCredit;
                        trialBalance.nNetDebit = NetDebit;
                        trialBalance.TempCostID = TempCostID;
                        trialBalance.TempCostType = TempCostType;
                        if (AccountData.FirstOrDefault(m => m.AccountFather == Acc.AccountNumber) != null)
                        {
                            trialBalance.IsMainAccount = 1;

                        }
                        else
                        {
                            trialBalance.IsMainAccount = 0;


                        }

                        if (!Obj.ShowZeroBalance)
                        {
                            if ((trialBalance.NetCredit - trialBalance.NetDebit) != 0)
                            {
                                TrialBalanceVMList.Add(trialBalance);
                            }
                        }
                        else
                        {
                            if ((trialBalance.NetCredit != 0) || (trialBalance.NetDebit != 0) || (TotalDebit != 0) || (NetCredit != 0))
                            {
                                TrialBalanceVMList.Add(trialBalance);
                            }


                        }
                    }

                }
                else if ((Obj.AccountLevelDropVMID > 0) && (Obj.Detail))
                {


                    Accounts = AccountData.Where(m => m.AccountLevel <= Obj.AccountLevelDropVMID).ToList().OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();

                    foreach (var Acc in Accounts)//Each Main Account
                    {
                        TotalDebit = 0;
                        TOTCredit = 0;
                        NetCredit = 0;
                        NetDebit = 0;
                        CreditBalance = 0;
                        DebitBalance = 0;
                        CreditTransAction = 0;
                        DebitTransAction = 0;

                        var MainAccount = AccountData.FirstOrDefault(m => m.AccountNumber == Acc.AccountNumber);
                        var MainChild = AccountData.Where(m => m.AccountFather == Acc.AccountNumber).OrderBy(m => m.AccountNumber).OrderBy(m => m.AccountLevel).ToList();

                        var TransActionData = TranData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber));
                        var BalanceData = TotData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber));
                        foreach (var D in TransActionData)
                        {
                            CreditTransAction += D.CreditTransAction;
                            DebitTransAction += D.DebitTransAction;
                        }
                        foreach (var Balance in BalanceData)
                        {
                            CreditBalance += Balance.CreditBalance;
                            DebitBalance += Balance.DebitBalance;
                        }



                        TotalDebit = DebitTransAction + DebitBalance;
                        TOTCredit = CreditTransAction + CreditBalance;

                        if (TotalDebit > TOTCredit)
                        {
                            NetDebit = TotalDebit - TOTCredit;
                        }
                        if (TotalDebit < TOTCredit)
                        {
                            NetCredit = TOTCredit - TotalDebit;
                        }

                        TrialBalanceVM trialBalance = new TrialBalanceVM();
                        trialBalance.AccountNumber = Acc.AccountNumber;
                        trialBalance.Name = Acc.ArabicName;

                        trialBalance.CreditBalance = CreditBalance;
                        trialBalance.CreditTransAction = CreditTransAction;
                        trialBalance.DebitBalance = DebitBalance;
                        trialBalance.DebitTransAction = DebitTransAction;
                        trialBalance.NetCredit = NetCredit;
                        trialBalance.NetDebit = NetDebit;
                        trialBalance.TempCostID = TempCostID;
                        trialBalance.TempCostType = TempCostType;
                        if ((AccountData.FirstOrDefault(m => m.AccountFather == Acc.AccountNumber) != null) && (Acc.AccountLevel < Obj.AccountLevelDropVMID))
                        {
                            trialBalance.IsMainAccount = 1;
                            trialBalance.MainAccount = "{*}";


                            trialBalance.nCreditBalance = 0;
                            trialBalance.nCreditTransAction = 0;
                            trialBalance.nDebitBalance = 0;
                            trialBalance.nDebitTransAction = 0;
                            trialBalance.nNetCredit = 0;
                            trialBalance.nNetDebit = 0;

                        }
                        else
                        {
                            trialBalance.IsMainAccount = 0;
                            trialBalance.MainAccount = "";
                            trialBalance.nCreditBalance = CreditBalance;
                            trialBalance.nCreditTransAction = CreditTransAction;
                            trialBalance.nDebitBalance = DebitBalance;
                            trialBalance.nDebitTransAction = DebitTransAction;
                            trialBalance.nNetCredit = NetCredit;
                            trialBalance.nNetDebit = NetDebit;


                        }
                        if (!Obj.ShowZeroBalance)
                        {
                            if ((trialBalance.NetCredit - trialBalance.NetDebit) != 0)
                            {
                                TrialBalanceVMList.Add(trialBalance);
                            }
                        }
                        else
                        {
                            if ((trialBalance.NetCredit != 0) || (trialBalance.NetDebit != 0) || (TotalDebit != 0) || (NetCredit != 0))
                            {
                                TrialBalanceVMList.Add(trialBalance);
                            }


                        }




                    }






                }




                return Json(TrialBalanceVMList, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                string err = ex.Message;

                List<TrialBalanceVM> TrialBalanceVMList = new List<TrialBalanceVM>();

                return Json(TrialBalanceVMList, JsonRequestBehavior.AllowGet);

            }


        }
        [HttpPost]
        public JsonResult EstimatedBudgetForAccount(AccountLevelRepVM Obj)
        {
            try
            {

                var userId = User.Identity.GetUserId();
                string UserID = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
                var AccountData = _unitOfWork.NativeSql.GetChartOfAccountByLevel(UserInfo.fCompanyId);

                int CurrYear = UserInfo.CurrYear;
                Obj.Detail = true;
                var Date = "01/01/" + CurrYear;
                var OpenDate = "31/12/" + (CurrYear - 1).ToString();
                Obj.FromDate = DateTime.Parse("01/"+Obj.Month.ToString("00")+"/"+ CurrYear);
                var LastDay = DateTime.DaysInMonth(CurrYear, Obj.Month).ToString("00");
                Obj.ToDate = DateTime.Parse(LastDay+"/" + Obj.Month.ToString("00") + "/" + CurrYear);



                IEnumerable<TrialBalanceVM> TotData = new List<TrialBalanceVM>();
                if (Obj.Partofthenumber)
                {
                    if (Obj.CostSearchType == 1)
                    {
                        Obj.CostCenterNumber = Obj.CostCenterNumber + "%";
                    }
                    else if (Obj.CostSearchType == 2)
                    {
                        Obj.CostCenterNumber = "%"+Obj.CostCenterNumber ;
                    }
                    else if (Obj.CostSearchType == 3)
                    {
                        Obj.CostCenterNumber = "%"+Obj.CostCenterNumber + "%";

                    }
                }

                var TranData = _unitOfWork.NativeSql.GetTransactionForTrial(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate, Obj.ByCostCenter, Obj.CostCenterNumber, Obj.Partofthenumber, Obj.CostSearchType);
                var MonthlyData = _unitOfWork.NativeSql.GetTransactionForTrialYearly(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate, Obj.ByCostCenter, Obj.CostCenterNumber, Obj.Partofthenumber, Obj.CostSearchType);
                var BudgetData = _unitOfWork.EstimatedBudget.GetAllEstimatedBudgets(UserInfo.fCompanyId, CurrYear);

              //  if (DateTime.Parse(Date) != Obj.FromDate)
                //    TotData = _unitOfWork.NativeSql.GetTotCreditDebitForTrial(UserInfo.fCompanyId, DateTime.Parse(Date), Obj.FromDate);


                //----------------Get Open-------------//
                IEnumerable<TrialBalanceVM> TotDataOpen = new List<TrialBalanceVM>();
                TotDataOpen = _unitOfWork.NativeSql.GetTotCreditDebitForTrialOpen(UserInfo.fCompanyId, DateTime.Parse(OpenDate), DateTime.Parse(OpenDate));
                foreach (var D in TotDataOpen)
                {
                    TotData = TotData.Append(D);

                }
                //------------------------------------//


                double TotalDebit = 0;
                double TOTCredit = 0;
                double NetCredit = 0;
                double NetDebit = 0;
                double CreditBalance = 0;
                double DebitBalance = 0;
                double CreditTransAction = 0;
                double DebitTransAction = 0;

                double January = 0;
                double February = 0;
                double March = 0;
                double April = 0;
                double May = 0;
                double June = 0;
                double July = 0;
                double August = 0;
                double September = 0;
                double October = 0;
                double November = 0;
                double December = 0;

                double NetTot = 0;
                double OpenBalance = 0;
                double MonthTransaction = 0;
                double Expected = 0;
                double ChangeVaule = 0;
                double ChangePrec = 0;

                List<TrialBalanceVM> TrialBalanceVMList = new List<TrialBalanceVM>();

                IEnumerable<ChartOfAccount> Accounts = new List<ChartOfAccount>();

                IEnumerable<ChartOfAccount> LessMainAccount = new List<ChartOfAccount>();




                if ((Obj.AccountLevelDropVMID > 0) && (Obj.Detail))
                {


                    Accounts = AccountData.Where(m => m.AccountLevel <= Obj.AccountLevelDropVMID).ToList().OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();

                    foreach (var Acc in Accounts)//Each Main Account
                    {
                        TotalDebit = 0;
                        TOTCredit = 0;
                        NetCredit = 0;
                        NetDebit = 0;
                        CreditBalance = 0;
                        DebitBalance = 0;
                        CreditTransAction = 0;
                        DebitTransAction = 0;

                        January = 0;
                        February = 0;
                        March = 0;
                        April = 0;
                        May = 0;
                        June = 0;
                        July = 0;
                        August = 0;
                        September = 0;
                        October = 0;
                        November = 0;
                        December = 0;

                        NetTot = 0;

                        MonthTransaction = 0;
                        Expected = 0;
                        ChangeVaule = 0;
                        ChangePrec = 0;

                        var MainAccount = AccountData.FirstOrDefault(m => m.AccountNumber == Acc.AccountNumber);
                        var MainChild = AccountData.Where(m => m.AccountFather == Acc.AccountNumber).OrderBy(m => m.AccountNumber).OrderBy(m => m.AccountLevel).ToList();

                        var TransActionData = TranData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber));
                        var BalanceData = TotData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber));
                        var MonthlyTrans = MonthlyData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber));
                        foreach (var D in TransActionData)
                        {
                            CreditTransAction += D.CreditTransAction;
                            DebitTransAction += D.DebitTransAction;
                        }
                        foreach (var Balance in BalanceData)
                        {
                            CreditBalance += Balance.CreditBalance;
                            DebitBalance += Balance.DebitBalance;
                        }
                        foreach (var D in MonthlyTrans)
                        {
                            switch (D.Month)
                            {
                                case 1:
                                    January = January + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 2:
                                    February = February + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 3:
                                    March = March + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 4:
                                    April = April + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 5:
                                    May = May + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 6:
                                    June = June + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 7:
                                    July = July + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 8:
                                    August = August + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 9:
                                    September = September + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 10:
                                    October = October + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 11:
                                    November = November + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 12:
                                    December = December + (D.DebitTransAction - D.CreditTransAction);
                                    break;

                            }

                        }



                        TotalDebit = DebitTransAction + DebitBalance;
                        TOTCredit = CreditTransAction + CreditBalance;

                        if (TotalDebit > TOTCredit)
                        {
                            NetDebit = TotalDebit - TOTCredit;
                            NetTot = NetDebit;
                        }
                        if (TotalDebit < TOTCredit)
                        {
                            NetCredit = TOTCredit - TotalDebit;
                            NetTot = NetCredit;


                        }
                        NetTot = TotalDebit - TOTCredit;
                        OpenBalance = DebitBalance - CreditBalance;

                        TrialBalanceVM trialBalance = new TrialBalanceVM();
                        trialBalance.AccountNumber = Acc.AccountNumber;
                        trialBalance.Name = Acc.ArabicName;

                        trialBalance.CreditBalance = CreditBalance;
                        trialBalance.CreditTransAction = CreditTransAction;
                        trialBalance.DebitBalance = DebitBalance;
                        trialBalance.DebitTransAction = DebitTransAction;
                        trialBalance.NetCredit = NetCredit;
                        trialBalance.NetDebit = NetDebit;

                        trialBalance.January = January;
                        trialBalance.February = February;
                        trialBalance.March = March;
                        trialBalance.April = April;
                        trialBalance.May = May;
                        trialBalance.June = June;
                        trialBalance.July = July;
                        trialBalance.August = August;
                        trialBalance.September = September;
                        trialBalance.October = October;
                        trialBalance.November = November;
                        trialBalance.December = December;

                        trialBalance.OpenBalance = OpenBalance;
                        trialBalance.NetTot = NetTot;
                        BudgetData = BudgetData.Where(m => m.AccountNumber != null).ToList();
                             var CurrBudget = BudgetData .Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.BudgetType==1).ToList();
                        if (CurrBudget != null)
                        {
                            foreach(var D in CurrBudget)
                            {
                                switch (Obj.Month)
                                {
                                    case 1:
                                        Expected = Expected+ D.January;
                                        break;
                                    case 2:
                                        Expected = Expected + D.February;
                                        break;
                                    case 3:
                                        Expected = Expected + D.March;
                                        break;
                                    case 4:
                                        Expected = Expected + D.April;
                                        break;
                                    case 5:
                                        Expected = Expected + D.May;
                                        break;
                                    case 6:
                                        Expected = Expected + D.June;
                                        break;
                                    case 7:
                                        Expected = Expected + D.July;
                                        break;
                                    case 8:
                                        Expected = Expected + D.August;
                                        break;
                                    case 9:
                                        Expected = Expected + D.September;
                                        break;
                                    case 10:
                                        Expected = Expected + D.October;
                                        break;
                                    case 11:
                                        Expected = Expected + D.November;
                                        break;
                                    case 12:
                                        Expected = Expected + D.December;
                                        break;

                                }

                            }                             
                        }
                       
                        MonthTransaction =  NetTot ;
                        ChangeVaule =   NetTot  - Expected;
                        ChangePrec = 0;
                        if (Expected != 0)
                        {
                            if(MonthTransaction != 0 )
                            ChangePrec =  Math.Round( (ChangeVaule/ MonthTransaction) *100,3);
                        }
                     


                        trialBalance.Expected = Expected;
                        trialBalance.MonthTransaction = MonthTransaction;
                        trialBalance.ChangeVaule = ChangeVaule;
                        trialBalance.ChangePrec = ChangePrec;

                        if ((AccountData.FirstOrDefault(m => m.AccountFather == Acc.AccountNumber) != null) && (Acc.AccountLevel < Obj.AccountLevelDropVMID))
                        {
                            trialBalance.IsMainAccount = 1;
                            trialBalance.MainAccount = "{*}";
                        
                          

                            trialBalance.nCreditBalance = 0;
                            trialBalance.nCreditTransAction = 0;
                            trialBalance.nDebitBalance = 0;
                            trialBalance.nDebitTransAction = 0;
                            trialBalance.nNetCredit = 0;
                            trialBalance.nNetDebit = 0;

                        }
                        else
                        {
                            trialBalance.IsMainAccount = 0;
                            trialBalance.MainAccount = "";
                            trialBalance.nCreditBalance = CreditBalance;
                            trialBalance.nCreditTransAction = CreditTransAction;
                            trialBalance.nDebitBalance = DebitBalance;
                            trialBalance.nDebitTransAction = DebitTransAction;
                            trialBalance.nNetCredit = NetCredit;
                            trialBalance.nNetDebit = NetDebit;

                            trialBalance.nJanuary = January;
                            trialBalance.nFebruary = February;
                            trialBalance.nMarch = March;
                            trialBalance.nApril = April;
                            trialBalance.nMay = May;
                            trialBalance.nJune = June;
                            trialBalance.nJuly = July;
                            trialBalance.nAugust = August;
                            trialBalance.nSeptember = September;
                            trialBalance.nOctober = October;
                            trialBalance.nNovember = November;
                            trialBalance.nDecember = December;

                            trialBalance.nNetTot = NetTot;

                            trialBalance.nOpenBalance = OpenBalance;

                            trialBalance.nMonthTransaction = MonthTransaction;
                            trialBalance.nExpected = Expected;
                            trialBalance.nChangeVaule = ChangeVaule;
                            trialBalance.nChangePrec = ChangePrec;


                        }
                        if (!Obj.ShowEstimatedZero)
                        {
                            if ((trialBalance.Expected) != 0)
                            {
                                TrialBalanceVMList.Add(trialBalance);
                            }
                        }
                        else
                        {
                            //if ((trialBalance.NetCredit != 0) || (trialBalance.NetDebit != 0) || (TotalDebit != 0) || (NetCredit != 0))
                            //{
                                TrialBalanceVMList.Add(trialBalance);
                            //}


                        }




                    }






                }
                if (!Obj.ShowMainAccountValue)
                {
                    foreach(var trialBalance in TrialBalanceVMList.Where(m=>m.IsMainAccount==1))
                    {
                        if (trialBalance.IsMainAccount == 1)
                        {
                            trialBalance.Expected = 0;
                            trialBalance.MonthTransaction = 0;
                            trialBalance.ChangeVaule = 0;
                            trialBalance.ChangePrec = 0;
                            trialBalance.CreditBalance = 0;
                            trialBalance.CreditTransAction = 0;
                            trialBalance.DebitBalance = 0;
                            trialBalance.DebitTransAction = 0;
                            trialBalance.NetCredit = 0;
                            trialBalance.NetDebit = 0;
                        }
                     
                    }
                   
                }



                return Json(TrialBalanceVMList, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                string err = ex.Message;

                List<TrialBalanceVM> TrialBalanceVMList = new List<TrialBalanceVM>();

                return Json(TrialBalanceVMList, JsonRequestBehavior.AllowGet);

            }


        }
        [HttpPost]
        public JsonResult GetEstimatedBudgetForAccountAll(AccountLevelRepVM Obj)
        {
            try
            {

                var userId = User.Identity.GetUserId();
                string UserID = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
                var AccountData = _unitOfWork.NativeSql.GetChartOfAccountByLevel(UserInfo.fCompanyId);

                int CurrYear = UserInfo.CurrYear;
                Obj.Detail = true;
                var Date = "01/01/" + CurrYear;
                var OpenDate = "31/12/" + (CurrYear - 1).ToString();
                Obj.FromDate = DateTime.Parse("01/01/" + CurrYear);
                Obj.ToDate = DateTime.Parse("31/12/" + CurrYear);



                IEnumerable<TrialBalanceVM> TotData = new List<TrialBalanceVM>();
                if (Obj.Partofthenumber)
                {
                    if (Obj.CostSearchType == 1)
                    {
                        Obj.CostCenterNumber = Obj.CostCenterNumber + "%";
                    }
                    else if (Obj.CostSearchType == 2)
                    {
                        Obj.CostCenterNumber = "%" + Obj.CostCenterNumber;
                    }
                    else if (Obj.CostSearchType == 3)
                    {
                        Obj.CostCenterNumber = "%" + Obj.CostCenterNumber + "%";

                    }
                }
                var TranData = _unitOfWork.NativeSql.GetTransactionForTrial(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate, Obj.ByCostCenter, Obj.CostCenterNumber, Obj.Partofthenumber, Obj.CostSearchType);
                var MonthlyData = _unitOfWork.NativeSql.GetTransactionForTrialYearly(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate, Obj.ByCostCenter, Obj.CostCenterNumber, Obj.Partofthenumber, Obj.CostSearchType);
                var BudgetData = _unitOfWork.EstimatedBudget.GetAllEstimatedBudgets(UserInfo.fCompanyId, CurrYear);

              //  if (DateTime.Parse(Date) != Obj.FromDate)
               //     TotData = _unitOfWork.NativeSql.GetTotCreditDebitForTrial(UserInfo.fCompanyId, DateTime.Parse(Date), Obj.FromDate);


                //----------------Get Open-------------//
                IEnumerable<TrialBalanceVM> TotDataOpen = new List<TrialBalanceVM>();
                TotDataOpen = _unitOfWork.NativeSql.GetTotCreditDebitForTrialOpen(UserInfo.fCompanyId, DateTime.Parse(OpenDate), DateTime.Parse(OpenDate));
                foreach (var D in TotDataOpen)
                {
                    TotData = TotData.Append(D);

                }
                //------------------------------------//


                double TotalDebit = 0;
                double TOTCredit = 0;
                double NetCredit = 0;
                double NetDebit = 0;
                double CreditBalance = 0;
                double DebitBalance = 0;
                double CreditTransAction = 0;
                double DebitTransAction = 0;

                double January = 0;
                double February = 0;
                double March = 0;
                double April = 0;
                double May = 0;
                double June = 0;
                double July = 0;
                double August = 0;
                double September = 0;
                double October = 0;
                double November = 0;
                double December = 0;

                double NetTot = 0;
                double OpenBalance = 0;

                double MonthTransaction1 = 0;
                double Expected1 = 0;
                double ChangeVaule1 = 0;
                double ChangePrec1 = 0;

                double MonthTransaction2 = 0;
                double Expected2 = 0;
                double ChangeVaule2 = 0;
                double ChangePrec2 = 0;


                double MonthTransaction3 = 0;
                double Expected3 = 0;
                double ChangeVaule3 = 0;
                double ChangePrec3 = 0;


                double MonthTransaction4 = 0;
                double Expected4 = 0;
                double ChangeVaule4 = 0;
                double ChangePrec4 = 0;


                double MonthTransaction5 = 0;
                double Expected5 = 0;
                double ChangeVaule5 = 0;
                double ChangePrec5 = 0;

                double MonthTransaction6 = 0;
                double Expected6 = 0;
                double ChangeVaule6 = 0;
                double ChangePrec6 = 0;


                double MonthTransaction7 = 0;
                double Expected7 = 0;
                double ChangeVaule7 = 0;
                double ChangePrec7 = 0;


                double MonthTransaction8 = 0;
                double Expected8 = 0;
                double ChangeVaule8 = 0;
                double ChangePrec8 = 0;


                double MonthTransaction9 = 0;
                double Expected9 = 0;
                double ChangeVaule9 = 0;
                double ChangePrec9 = 0;


                double MonthTransaction10 = 0;
                double Expected10 = 0;
                double ChangeVaule10 = 0;
                double ChangePrec10 = 0;


 


                double MonthTransaction11 = 0;
                double Expected11 = 0;
                double ChangeVaule11 = 0;
                double ChangePrec11 = 0;


                double MonthTransaction12 = 0;
                double Expected12 = 0;
                double ChangeVaule12 = 0;
                double ChangePrec12 = 0;

                List<TrialBalanceVM> TrialBalanceVMList = new List<TrialBalanceVM>();

                IEnumerable<ChartOfAccount> Accounts = new List<ChartOfAccount>();

                IEnumerable<ChartOfAccount> LessMainAccount = new List<ChartOfAccount>();




                if ((Obj.AccountLevelDropVMID > 0) && (Obj.Detail))
                {


                    Accounts = AccountData.Where(m => m.AccountLevel <= Obj.AccountLevelDropVMID).ToList().OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();

                    foreach (var Acc in Accounts)//Each Main Account
                    {
                        TotalDebit = 0;
                        TOTCredit = 0;
                        NetCredit = 0;
                        NetDebit = 0;
                        CreditBalance = 0;
                        DebitBalance = 0;
                        CreditTransAction = 0;
                        DebitTransAction = 0;

                        January = 0;
                        February = 0;
                        March = 0;
                        April = 0;
                        May = 0;
                        June = 0;
                        July = 0;
                        August = 0;
                        September = 0;
                        October = 0;
                        November = 0;
                        December = 0;

                        NetTot = 0;

                        MonthTransaction1 = 0;
                        Expected1 = 0;
                        ChangeVaule1 = 0;
                        ChangePrec1 = 0;

                        MonthTransaction2 = 0;
                        Expected2 = 0;
                        ChangeVaule2 = 0;
                        ChangePrec2 = 0;


                        MonthTransaction3 = 0;
                        Expected3 = 0;
                        ChangeVaule3 = 0;
                        ChangePrec3 = 0;


                        MonthTransaction4 = 0;
                        Expected4 = 0;
                        ChangeVaule4 = 0;
                        ChangePrec4 = 0;


                        MonthTransaction5 = 0;
                        Expected5 = 0;
                        ChangeVaule5 = 0;
                        ChangePrec5 = 0;

                        MonthTransaction6 = 0;
                        Expected6 = 0;
                        ChangeVaule6 = 0;
                        ChangePrec6 = 0;


                        MonthTransaction7 = 0;
                        Expected7 = 0;
                        ChangeVaule7 = 0;
                        ChangePrec7 = 0;


                        MonthTransaction8 = 0;
                        Expected8 = 0;
                        ChangeVaule8 = 0;
                        ChangePrec8 = 0;


                        MonthTransaction9 = 0;
                        Expected9 = 0;
                        ChangeVaule9 = 0;
                        ChangePrec9 = 0;


                        MonthTransaction10 = 0;
                        Expected10 = 0;
                        ChangeVaule10 = 0;
                        ChangePrec10 = 0;



                        MonthTransaction10 = 0;
                        Expected10 = 0;
                        ChangeVaule10 = 0;
                        ChangePrec10 = 0;


                        MonthTransaction11 = 0;
                        Expected11 = 0;
                        ChangeVaule11 = 0;
                        ChangePrec11 = 0;


                        MonthTransaction12 = 0;
                        Expected12 = 0;
                        ChangeVaule12 = 0;
                        ChangePrec12 = 0;

                        var MainAccount = AccountData.FirstOrDefault(m => m.AccountNumber == Acc.AccountNumber);
                        var MainChild = AccountData.Where(m => m.AccountFather == Acc.AccountNumber).OrderBy(m => m.AccountNumber).OrderBy(m => m.AccountLevel).ToList();
                        var TransActionData = TranData.Where(m => m.AccountNumber != null);
                        var MonthlyTrans = TranData.Where(m => m.AccountNumber != null);
                        var BalanceData = TranData.Where(m => m.AccountNumber != null);
                       
                        foreach (var D in TransActionData)
                        {
                            TransActionData = TranData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber));
                            CreditTransAction += D.CreditTransAction;
                            DebitTransAction += D.DebitTransAction;
                        }
                        foreach (var Balance in BalanceData)
                        {
                            BalanceData = TotData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber));

                            CreditBalance += Balance.CreditBalance;
                            DebitBalance += Balance.DebitBalance;
                        }
                        foreach (var D in MonthlyTrans)
                        {
                            MonthlyTrans = MonthlyData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber));

                            switch (D.Month)
                            {
                                case 1:
                                    January = January + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 2:
                                    February = February + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 3:
                                    March = March + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 4:
                                    April = April + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 5:
                                    May = May + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 6:
                                    June = June + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 7:
                                    July = July + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 8:
                                    August = August + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 9:
                                    September = September + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 10:
                                    October = October + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 11:
                                    November = November + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 12:
                                    December = December + (D.DebitTransAction - D.CreditTransAction);
                                    break;

                            }

                        }



                        TotalDebit = DebitTransAction + DebitBalance;
                        TOTCredit = CreditTransAction + CreditBalance;

                        if (TotalDebit > TOTCredit)
                        {
                            NetDebit = TotalDebit - TOTCredit;
                            NetTot = NetDebit;
                        }
                        if (TotalDebit < TOTCredit)
                        {
                            NetCredit = TOTCredit - TotalDebit;
                            NetTot = NetCredit;


                        }
                        NetTot = TotalDebit - TOTCredit;
                        OpenBalance = DebitBalance - CreditBalance;

                        TrialBalanceVM trialBalance = new TrialBalanceVM();
                        trialBalance.AccountNumber = Acc.AccountNumber;
                        trialBalance.Name = Acc.ArabicName;

                        trialBalance.CreditBalance = CreditBalance;
                        trialBalance.CreditTransAction = CreditTransAction;
                        trialBalance.DebitBalance = DebitBalance;
                        trialBalance.DebitTransAction = DebitTransAction;
                        trialBalance.NetCredit = NetCredit;
                        trialBalance.NetDebit = NetDebit;

                        trialBalance.January = January;
                        trialBalance.February = February;
                        trialBalance.March = March;
                        trialBalance.April = April;
                        trialBalance.May = May;
                        trialBalance.June = June;
                        trialBalance.July = July;
                        trialBalance.August = August;
                        trialBalance.September = September;
                        trialBalance.October = October;
                        trialBalance.November = November;
                        trialBalance.December = December;

                        trialBalance.OpenBalance = OpenBalance;
                        trialBalance.NetTot = NetTot;
                        BudgetData = BudgetData.Where(m => m.AccountNumber != null).ToList();
                        var CurrBudget = BudgetData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.BudgetType == 1);
                        if (CurrBudget.Count() >0)
                        {
                            foreach (var D in CurrBudget)
                            {
                                 
                                        Expected1 = Expected1 + D.January;
                                     
                                        Expected2 = Expected2 + D.February;
                                     
                                        Expected3 = Expected3 + D.March;
                                    
                                        Expected4 = Expected4 + D.April;
                                     
                                        Expected5 = Expected5 + D.May;
                                       
                                        Expected6 = Expected6 + D.June;
                                      
                                        Expected7 = Expected7 + D.July;
                                      
                                        Expected8 = Expected8 + D.August;
                                     
                                        Expected9 = Expected9 + D.September;
                                      
                                        Expected10 = Expected10 + D.October;
                                      
                                     
                                        Expected11 = Expected11 + D.November;
                                        
                                       Expected12 = Expected12 + D.December;
                                        

                                }

                             
                        }

                        MonthTransaction1 = January ;
                        ChangeVaule1 =   January - Expected1;
                        ChangePrec1 = 0;
                        if (Expected1 != 0)
                        {
                            ChangePrec1 = 0;// Math.Round((ChangeVaule1 / MonthTransaction1) * 100, 3);
                        }

                        MonthTransaction2 =  February ;
                        ChangeVaule2 =  February  - Expected2;
                        ChangePrec2 = 0;
                        if (Expected2 != 0)
                        {
                            ChangePrec2 = 0;// Math.Round((ChangeVaule2 / MonthTransaction2) * 100, 3);
                        }


                        MonthTransaction3 = March;
                        ChangeVaule3 = March - Expected3;
                        ChangePrec3 = 0;
                        if (Expected3 != 0)
                        {
                            ChangePrec3 = 0;//  Math.Round((ChangeVaule3 / MonthTransaction3) * 100, 3);
                        }

                        MonthTransaction4 = April;
                        ChangeVaule4 = April - Expected4;
                        ChangePrec4 = 0;
                        if (Expected4 != 0)
                        {
                            ChangePrec4 = 0;// Math.Round((ChangeVaule4 / MonthTransaction4) * 100, 3);
                        }

                        MonthTransaction5 = May;
                        ChangeVaule5 =   May  - Expected5;
                        ChangePrec5 = 0;
                        if (Expected5 != 0)
                        {
                            ChangePrec5 = 0;// Math.Round((ChangeVaule5 / MonthTransaction5) * 100, 3);
                        }

                        MonthTransaction6 = June;
                        ChangeVaule6 = June - Expected6;
                        ChangePrec6 = 0;
                        if (Expected6 != 0)
                        {
                            ChangePrec6 = 0;//  Math.Round((ChangeVaule6 / MonthTransaction6) * 100, 3);
                        }

                        MonthTransaction7 =  July ;
                        ChangeVaule7 =  July  - Expected7;
                        ChangePrec7 = 0;
                        if (Expected7 != 0)
                        {
                            ChangePrec7 = 0;//  Math.Round((ChangeVaule7 / MonthTransaction7) * 100, 3);
                        }



                        MonthTransaction8 =  August ;
                        ChangeVaule8 =  August  - Expected8;
                        ChangePrec8 = 0;
                        if (Expected8 != 0)
                        {
                            ChangePrec8 = 0;//  Math.Round((ChangeVaule8 / MonthTransaction8) * 100, 3);
                        }


                        MonthTransaction9 =  September ;
                        ChangeVaule9 =   September - Expected9;
                        ChangePrec9 = 0;
                        if (Expected9 != 0)
                        {
                            ChangePrec9 = 0;//  Math.Round((ChangeVaule9 / MonthTransaction9) * 100, 3);
                        }


                        MonthTransaction10 =  October ;
                        ChangeVaule10 =  October  - Expected10;
                        ChangePrec10 = 0;
                        if (Expected10 != 0)
                        {
                            ChangePrec10 = 0;//  Math.Round((ChangeVaule10 / MonthTransaction10) * 100, 3);
                        }


                        MonthTransaction11 =   November ;
                        ChangeVaule11 =  November  - Expected11;
                        ChangePrec11 = 0;
                        if (Expected11 != 0)
                        {
                            ChangePrec11 = 0;// Math.Round((ChangeVaule11 / MonthTransaction11) * 100, 3);
                        }


                        MonthTransaction12 =  December ;
                        ChangeVaule12 =  December - Expected12;
                        ChangePrec12 = 0;
                        if (Expected12 != 0)
                        {
                            ChangePrec12 = 0;//  Math.Round((ChangeVaule12 / MonthTransaction12) * 100, 3);
                        }


                        trialBalance.Expected1 = Expected1;
                        trialBalance.MonthTransaction1 = MonthTransaction1;
                        trialBalance.ChangeVaule1 = ChangeVaule1;
                        trialBalance.ChangePrec1 = ChangePrec1;

                        trialBalance.Expected2 = Expected2;
                        trialBalance.MonthTransaction2 = MonthTransaction2;
                        trialBalance.ChangeVaule2 = ChangeVaule2;
                        trialBalance.ChangePrec2 = ChangePrec2;


                        trialBalance.Expected3 = Expected3;
                        trialBalance.MonthTransaction3 = MonthTransaction3;
                        trialBalance.ChangeVaule3 = ChangeVaule3;
                        trialBalance.ChangePrec3 = ChangePrec3;


                        trialBalance.Expected4 = Expected4;
                        trialBalance.MonthTransaction4 = MonthTransaction4;
                        trialBalance.ChangeVaule4 = ChangeVaule4;
                        trialBalance.ChangePrec4 = ChangePrec4;


                        trialBalance.Expected5 = Expected5;
                        trialBalance.MonthTransaction5 = MonthTransaction5;
                        trialBalance.ChangeVaule5 = ChangeVaule5;
                        trialBalance.ChangePrec5 = ChangePrec5;


                        trialBalance.Expected6 = Expected6;
                        trialBalance.MonthTransaction6 = MonthTransaction6;
                        trialBalance.ChangeVaule6 = ChangeVaule6;
                        trialBalance.ChangePrec6 = ChangePrec6;


                        trialBalance.Expected7 = Expected7;
                        trialBalance.MonthTransaction7 = MonthTransaction7;
                        trialBalance.ChangeVaule7 = ChangeVaule7;
                        trialBalance.ChangePrec7 = ChangePrec7;


                        trialBalance.Expected8 = Expected8;
                        trialBalance.MonthTransaction8 = MonthTransaction8;
                        trialBalance.ChangeVaule8 = ChangeVaule8;
                        trialBalance.ChangePrec8 = ChangePrec8;


                        trialBalance.Expected9 = Expected9;
                        trialBalance.MonthTransaction9 = MonthTransaction9;
                        trialBalance.ChangeVaule9 = ChangeVaule9;
                        trialBalance.ChangePrec9 = ChangePrec9;


                        trialBalance.Expected10 = Expected10;
                        trialBalance.MonthTransaction10 = MonthTransaction10;
                        trialBalance.ChangeVaule10 = ChangeVaule10;
                        trialBalance.ChangePrec10 = ChangePrec10;


                        trialBalance.Expected11 = Expected11;
                        trialBalance.MonthTransaction11 = MonthTransaction11;
                        trialBalance.ChangeVaule11 = ChangeVaule11;
                        trialBalance.ChangePrec11 = ChangePrec11;


                        trialBalance.Expected12 = Expected12;
                        trialBalance.MonthTransaction12 = MonthTransaction12;
                        trialBalance.ChangeVaule12 = ChangeVaule12;
                        trialBalance.ChangePrec12 = ChangePrec12;


                        trialBalance.Expected1 = Expected1;
                        trialBalance.MonthTransaction1 = MonthTransaction1;
                        trialBalance.ChangeVaule1 = ChangeVaule1;
                        trialBalance.ChangePrec1 = ChangePrec1;



                        trialBalance.Expected1 = Expected1;
                        trialBalance.MonthTransaction1 = MonthTransaction1;
                        trialBalance.ChangeVaule1 = ChangeVaule1;
                        trialBalance.ChangePrec1 = ChangePrec1;


                        trialBalance.Expected1 = Expected1;
                        trialBalance.MonthTransaction1 = MonthTransaction1;
                        trialBalance.ChangeVaule1 = ChangeVaule1;
                        trialBalance.ChangePrec1 = ChangePrec1;







                        if ((AccountData.FirstOrDefault(m => m.AccountFather == Acc.AccountNumber) != null) && (Acc.AccountLevel < Obj.AccountLevelDropVMID))
                        {
                            trialBalance.IsMainAccount = 1;
                            trialBalance.MainAccount = "{*}";



                            trialBalance.nCreditBalance = 0;
                            trialBalance.nCreditTransAction = 0;
                            trialBalance.nDebitBalance = 0;
                            trialBalance.nDebitTransAction = 0;
                            trialBalance.nNetCredit = 0;
                            trialBalance.nNetDebit = 0;

                        }
                        else
                        {
                            trialBalance.IsMainAccount = 0;
                            trialBalance.MainAccount = "";
                            trialBalance.nCreditBalance = CreditBalance;
                            trialBalance.nCreditTransAction = CreditTransAction;
                            trialBalance.nDebitBalance = DebitBalance;
                            trialBalance.nDebitTransAction = DebitTransAction;
                            trialBalance.nNetCredit = NetCredit;
                            trialBalance.nNetDebit = NetDebit;

                            trialBalance.nJanuary = January;
                            trialBalance.nFebruary = February;
                            trialBalance.nMarch = March;
                            trialBalance.nApril = April;
                            trialBalance.nMay = May;
                            trialBalance.nJune = June;
                            trialBalance.nJuly = July;
                            trialBalance.nAugust = August;
                            trialBalance.nSeptember = September;
                            trialBalance.nOctober = October;
                            trialBalance.nNovember = November;
                            trialBalance.nDecember = December;

                            trialBalance.nNetTot = NetTot;

                            trialBalance.nOpenBalance = OpenBalance;

                            trialBalance.nExpected1 = Expected1;
                            trialBalance.nMonthTransaction1 = MonthTransaction1;
                            trialBalance.nChangeVaule1 = ChangeVaule1;
                            trialBalance.nChangePrec1 = ChangePrec1;

                            trialBalance.nExpected2 = Expected2;
                            trialBalance.nMonthTransaction2 = MonthTransaction2;
                            trialBalance.nChangeVaule2 = ChangeVaule2;
                            trialBalance.nChangePrec2 = ChangePrec2;


                            trialBalance.nExpected3 = Expected3;
                            trialBalance.nMonthTransaction3 = MonthTransaction3;
                            trialBalance.nChangeVaule3 = ChangeVaule3;
                            trialBalance.nChangePrec3 = ChangePrec3;


                            trialBalance.nExpected4 = Expected4;
                            trialBalance.nMonthTransaction4 = MonthTransaction4;
                            trialBalance.nChangeVaule4 = ChangeVaule4;
                            trialBalance.nChangePrec4 = ChangePrec4;


                            trialBalance.nExpected5 = Expected5;
                            trialBalance.nMonthTransaction5 = MonthTransaction5;
                            trialBalance.nChangeVaule5 = ChangeVaule5;
                            trialBalance.nChangePrec5 = ChangePrec5;


                            trialBalance.nExpected6 = Expected6;
                            trialBalance.nMonthTransaction6 = MonthTransaction6;
                            trialBalance.nChangeVaule6 = ChangeVaule6;
                            trialBalance.nChangePrec6 = ChangePrec6;


                            trialBalance.nExpected7 = Expected7;
                            trialBalance.nMonthTransaction7 = MonthTransaction7;
                            trialBalance.nChangeVaule7 = ChangeVaule7;
                            trialBalance.nChangePrec7 = ChangePrec7;


                            trialBalance.nExpected8 = Expected8;
                            trialBalance.nMonthTransaction8 = MonthTransaction8;
                            trialBalance.nChangeVaule8 = ChangeVaule8;
                            trialBalance.nChangePrec8 = ChangePrec8;


                            trialBalance.nExpected9 = Expected9;
                            trialBalance.nMonthTransaction9 = MonthTransaction9;
                            trialBalance.nChangeVaule9 = ChangeVaule9;
                            trialBalance.nChangePrec9 = ChangePrec9;


                            trialBalance.nExpected10 = Expected10;
                            trialBalance.nMonthTransaction10 = MonthTransaction10;
                            trialBalance.nChangeVaule10 = ChangeVaule10;
                            trialBalance.nChangePrec10 = ChangePrec10;


                            trialBalance.nExpected11 = Expected11;
                            trialBalance.nMonthTransaction11 = MonthTransaction11;
                            trialBalance.nChangeVaule11 = ChangeVaule11;
                            trialBalance.nChangePrec11 = ChangePrec11;


                            trialBalance.nExpected12 = Expected12;
                            trialBalance.nMonthTransaction12 = MonthTransaction12;
                            trialBalance.nChangeVaule12 = ChangeVaule12;
                            trialBalance.nChangePrec12 = ChangePrec12;


                            trialBalance.nExpected1 = Expected1;
                            trialBalance.nMonthTransaction1 = MonthTransaction1;
                            trialBalance.nChangeVaule1 = ChangeVaule1;
                            trialBalance.nChangePrec1 = ChangePrec1;



                            trialBalance.nExpected1 = Expected1;
                            trialBalance.nMonthTransaction1 = MonthTransaction1;
                            trialBalance.nChangeVaule1 = ChangeVaule1;
                            trialBalance.nChangePrec1 = ChangePrec1;


                            trialBalance.nExpected1 = Expected1;
                            trialBalance.nMonthTransaction1 = MonthTransaction1;
                            trialBalance.nChangeVaule1 = ChangeVaule1;
                            trialBalance.nChangePrec1 = ChangePrec1;


                        }
                        if (!Obj.ShowEstimatedZero)
                        {
                            if ((trialBalance.Expected1+ trialBalance.Expected2 + trialBalance.Expected3 + trialBalance.Expected4 + trialBalance.Expected5+ trialBalance.Expected5 +
                                trialBalance.Expected7 + trialBalance.Expected8 + trialBalance.Expected9 + trialBalance.Expected10 + trialBalance.Expected11 + trialBalance.Expected12) != 0)
                            {
                                TrialBalanceVMList.Add(trialBalance);
                            }
                        }
                        else
                        {
                            //if ((trialBalance.NetCredit != 0) || (trialBalance.NetDebit != 0) || (TotalDebit != 0) || (NetCredit != 0))
                            //{
                                TrialBalanceVMList.Add(trialBalance);
                            //}


                        }




                    }






                }
                if (!Obj.ShowMainAccountValue)
                {
                    foreach (var trialBalance in TrialBalanceVMList.Where(m => m.IsMainAccount == 1))
                    {
                        if (trialBalance.IsMainAccount == 1)
                        {
                            trialBalance.Expected = 0;
                            trialBalance.MonthTransaction = 0;
                            trialBalance.ChangeVaule = 0;
                            trialBalance.ChangePrec = 0;
                            trialBalance.CreditBalance = 0;
                            trialBalance.CreditTransAction = 0;
                            trialBalance.DebitBalance = 0;
                            trialBalance.DebitTransAction = 0;
                            trialBalance.NetCredit = 0;
                            trialBalance.NetDebit = 0;
                        }

                    }

                }



                return Json(TrialBalanceVMList, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                string err = ex.Message;

                List<TrialBalanceVM> TrialBalanceVMList = new List<TrialBalanceVM>();

                return Json(TrialBalanceVMList, JsonRequestBehavior.AllowGet);

            }


        }
        [HttpPost]
        public JsonResult GetEstimatedBudgetForAccountYear(AccountLevelRepVM Obj)
        {
            try
            {

                var userId = User.Identity.GetUserId();
                string UserID = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
                var AccountData = _unitOfWork.NativeSql.GetChartOfAccountByLevel(UserInfo.fCompanyId);

                int CurrYear = UserInfo.CurrYear;
                Obj.Detail = true;
                var Date = "01/01/" + CurrYear;
                var OpenDate = "31/12/" + (CurrYear - 1).ToString();
                Obj.FromDate = DateTime.Parse("01/01/" + CurrYear);
                Obj.ToDate = DateTime.Parse("31/12/" + CurrYear);



                IEnumerable<TrialBalanceVM> TotData = new List<TrialBalanceVM>();
                if (Obj.Partofthenumber)
                {
                    if (Obj.CostSearchType == 1)
                    {
                        Obj.CostCenterNumber = Obj.CostCenterNumber + "%";
                    }
                    else if (Obj.CostSearchType == 2)
                    {
                        Obj.CostCenterNumber = "%" + Obj.CostCenterNumber;
                    }
                    else if (Obj.CostSearchType == 3)
                    {
                        Obj.CostCenterNumber = "%" + Obj.CostCenterNumber + "%";

                    }
                }
                var TranData = _unitOfWork.NativeSql.GetTransactionForTrial(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate, Obj.ByCostCenter, Obj.CostCenterNumber, Obj.Partofthenumber, Obj.CostSearchType);
                var MonthlyData = _unitOfWork.NativeSql.GetTransactionForTrialYearly(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate, Obj.ByCostCenter, Obj.CostCenterNumber, Obj.Partofthenumber, Obj.CostSearchType);
                var BudgetData = _unitOfWork.EstimatedBudget.GetAllEstimatedBudgets(UserInfo.fCompanyId, CurrYear);

                //  if (DateTime.Parse(Date) != Obj.FromDate)
                //     TotData = _unitOfWork.NativeSql.GetTotCreditDebitForTrial(UserInfo.fCompanyId, DateTime.Parse(Date), Obj.FromDate);


                //----------------Get Open-------------//
                IEnumerable<TrialBalanceVM> TotDataOpen = new List<TrialBalanceVM>();
                TotDataOpen = _unitOfWork.NativeSql.GetTotCreditDebitForTrialOpen(UserInfo.fCompanyId, DateTime.Parse(OpenDate), DateTime.Parse(OpenDate));
                foreach (var D in TotDataOpen)
                {
                    TotData = TotData.Append(D);

                }
                //------------------------------------//


                double TotalDebit = 0;
                double TOTCredit = 0;
                double NetCredit = 0;
                double NetDebit = 0;
                double CreditBalance = 0;
                double DebitBalance = 0;
                double CreditTransAction = 0;
                double DebitTransAction = 0;

                double January = 0;
                double February = 0;
                double March = 0;
                double April = 0;
                double May = 0;
                double June = 0;
                double July = 0;
                double August = 0;
                double September = 0;
                double October = 0;
                double November = 0;
                double December = 0;

                double NetTot = 0;
                double OpenBalance = 0;

                double MonthTransaction1 = 0;
                double Expected1 = 0;
                double ChangeVaule1 = 0;
                double ChangePrec1 = 0;

                double MonthTransaction2 = 0;
                double Expected2 = 0;
                double ChangeVaule2 = 0;
                double ChangePrec2 = 0;


                double MonthTransaction3 = 0;
                double Expected3 = 0;
                double ChangeVaule3 = 0;
                double ChangePrec3 = 0;


                double MonthTransaction4 = 0;
                double Expected4 = 0;
                double ChangeVaule4 = 0;
                double ChangePrec4 = 0;


                double MonthTransaction5 = 0;
                double Expected5 = 0;
                double ChangeVaule5 = 0;
                double ChangePrec5 = 0;

                double MonthTransaction6 = 0;
                double Expected6 = 0;
                double ChangeVaule6 = 0;
                double ChangePrec6 = 0;


                double MonthTransaction7 = 0;
                double Expected7 = 0;
                double ChangeVaule7 = 0;
                double ChangePrec7 = 0;


                double MonthTransaction8 = 0;
                double Expected8 = 0;
                double ChangeVaule8 = 0;
                double ChangePrec8 = 0;


                double MonthTransaction9 = 0;
                double Expected9 = 0;
                double ChangeVaule9 = 0;
                double ChangePrec9 = 0;


                double MonthTransaction10 = 0;
                double Expected10 = 0;
                double ChangeVaule10 = 0;
                double ChangePrec10 = 0;





                double MonthTransaction11 = 0;
                double Expected11 = 0;
                double ChangeVaule11 = 0;
                double ChangePrec11 = 0;


                double MonthTransaction12 = 0;
                double Expected12 = 0;
                double ChangeVaule12 = 0;
                double ChangePrec12 = 0;

                List<TrialBalanceVM> TrialBalanceVMList = new List<TrialBalanceVM>();

                IEnumerable<ChartOfAccount> Accounts = new List<ChartOfAccount>();

                IEnumerable<ChartOfAccount> LessMainAccount = new List<ChartOfAccount>();




                if ((Obj.AccountLevelDropVMID > 0) && (Obj.Detail))
                {


                    Accounts = AccountData.Where(m => m.AccountLevel <= Obj.AccountLevelDropVMID).ToList().OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();

                    foreach (var Acc in Accounts)//Each Main Account
                    {
                        TotalDebit = 0;
                        TOTCredit = 0;
                        NetCredit = 0;
                        NetDebit = 0;
                        CreditBalance = 0;
                        DebitBalance = 0;
                        CreditTransAction = 0;
                        DebitTransAction = 0;

                        January = 0;
                        February = 0;
                        March = 0;
                        April = 0;
                        May = 0;
                        June = 0;
                        July = 0;
                        August = 0;
                        September = 0;
                        October = 0;
                        November = 0;
                        December = 0;

                        NetTot = 0;

                        MonthTransaction1 = 0;
                        Expected1 = 0;
                        ChangeVaule1 = 0;
                        ChangePrec1 = 0;

                        MonthTransaction2 = 0;
                        Expected2 = 0;
                        ChangeVaule2 = 0;
                        ChangePrec2 = 0;


                        MonthTransaction3 = 0;
                        Expected3 = 0;
                        ChangeVaule3 = 0;
                        ChangePrec3 = 0;


                        MonthTransaction4 = 0;
                        Expected4 = 0;
                        ChangeVaule4 = 0;
                        ChangePrec4 = 0;


                        MonthTransaction5 = 0;
                        Expected5 = 0;
                        ChangeVaule5 = 0;
                        ChangePrec5 = 0;

                        MonthTransaction6 = 0;
                        Expected6 = 0;
                        ChangeVaule6 = 0;
                        ChangePrec6 = 0;


                        MonthTransaction7 = 0;
                        Expected7 = 0;
                        ChangeVaule7 = 0;
                        ChangePrec7 = 0;


                        MonthTransaction8 = 0;
                        Expected8 = 0;
                        ChangeVaule8 = 0;
                        ChangePrec8 = 0;


                        MonthTransaction9 = 0;
                        Expected9 = 0;
                        ChangeVaule9 = 0;
                        ChangePrec9 = 0;


                        MonthTransaction10 = 0;
                        Expected10 = 0;
                        ChangeVaule10 = 0;
                        ChangePrec10 = 0;



                        MonthTransaction10 = 0;
                        Expected10 = 0;
                        ChangeVaule10 = 0;
                        ChangePrec10 = 0;


                        MonthTransaction11 = 0;
                        Expected11 = 0;
                        ChangeVaule11 = 0;
                        ChangePrec11 = 0;


                        MonthTransaction12 = 0;
                        Expected12 = 0;
                        ChangeVaule12 = 0;
                        ChangePrec12 = 0;

                        var MainAccount = AccountData.FirstOrDefault(m => m.AccountNumber == Acc.AccountNumber);
                        var MainChild = AccountData.Where(m => m.AccountFather == Acc.AccountNumber).OrderBy(m => m.AccountNumber).OrderBy(m => m.AccountLevel).ToList();



                        var TransActionData = TranData.Where(m => m.AccountNumber != null);

                        var BalanceData = TotData.Where(m => m.AccountNumber != null);
                        var MonthlyTrans = MonthlyData.Where(m => m.AccountNumber != null);

                        foreach (var D in TransActionData)
                        {
                            TransActionData = TranData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber));
                            CreditTransAction += D.CreditTransAction;
                            DebitTransAction += D.DebitTransAction;
                        }
                        foreach (var Balance in BalanceData)
                        {
                            BalanceData = TotData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber));

                            CreditBalance += Balance.CreditBalance;
                            DebitBalance += Balance.DebitBalance;
                        }
                        foreach (var D in MonthlyTrans)
                        {
                            MonthlyTrans = MonthlyData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber));

                            switch (D.Month)
                            {
                                case 1:
                                    January = January + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 2:
                                    February = February + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 3:
                                    March = March + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 4:
                                    April = April + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 5:
                                    May = May + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 6:
                                    June = June + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 7:
                                    July = July + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 8:
                                    August = August + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 9:
                                    September = September + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 10:
                                    October = October + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 11:
                                    November = November + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 12:
                                    December = December + (D.DebitTransAction - D.CreditTransAction);
                                    break;

                            }

                        }



                        TotalDebit = DebitTransAction + DebitBalance;
                        TOTCredit = CreditTransAction + CreditBalance;

                        if (TotalDebit > TOTCredit)
                        {
                            NetDebit = TotalDebit - TOTCredit;
                            NetTot = NetDebit;
                        }
                        if (TotalDebit < TOTCredit)
                        {
                            NetCredit = TOTCredit - TotalDebit;
                            NetTot = NetCredit;


                        }
                        NetTot = TotalDebit - TOTCredit;
                        OpenBalance = DebitBalance - CreditBalance;

                        TrialBalanceVM trialBalance = new TrialBalanceVM();
                        trialBalance.AccountNumber = Acc.AccountNumber;
                        trialBalance.Name = Acc.ArabicName;

                        trialBalance.CreditBalance = CreditBalance;
                        trialBalance.CreditTransAction = CreditTransAction;
                        trialBalance.DebitBalance = DebitBalance;
                        trialBalance.DebitTransAction = DebitTransAction;
                        trialBalance.NetCredit = NetCredit;
                        trialBalance.NetDebit = NetDebit;

                        trialBalance.January = January;
                        trialBalance.February = February;
                        trialBalance.March = March;
                        trialBalance.April = April;
                        trialBalance.May = May;
                        trialBalance.June = June;
                        trialBalance.July = July;
                        trialBalance.August = August;
                        trialBalance.September = September;
                        trialBalance.October = October;
                        trialBalance.November = November;
                        trialBalance.December = December;

                        trialBalance.OpenBalance = OpenBalance;
                        trialBalance.NetTot = NetTot;
                        BudgetData = BudgetData.Where(m => m.AccountNumber != null).ToList();
                        var CurrBudget = BudgetData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.BudgetType == 1);
                        if (CurrBudget.Count()>0)
                        {
                            foreach (var D in CurrBudget)
                            {

                                Expected1 = Expected1 + D.January;

                                Expected2 = Expected2 + D.February;

                                Expected3 = Expected3 + D.March;

                                Expected4 = Expected4 + D.April;

                                Expected5 = Expected5 + D.May;

                                Expected6 = Expected6 + D.June;

                                Expected7 = Expected7 + D.July;

                                Expected8 = Expected8 + D.August;

                                Expected9 = Expected9 + D.September;

                                Expected10 = Expected10 + D.October;


                                Expected11 = Expected11 + D.November;

                                Expected12 = Expected12 + D.December;


                            }


                        }

                        MonthTransaction1 =  January ;
                        ChangeVaule1 =  January - Expected1;
                        ChangePrec1 = 0;
                        if (Expected1 != 0)
                        {
                            ChangePrec1 = 0;// Math.Round((ChangeVaule1 / MonthTransaction1) * 100, 3);
                        }

                        MonthTransaction2 =  February ;
                        ChangeVaule2 =  February - Expected2;
                        ChangePrec2 = 0;
                        if (Expected2 != 0)
                        {
                            ChangePrec2 = 0;// Math.Round((ChangeVaule2 / MonthTransaction2) * 100, 3);
                        }


                        MonthTransaction3 =  March ;
                        ChangeVaule3 =  March  - Expected3;
                        ChangePrec3 = 0;
                        if (Expected3 != 0)
                        {
                            ChangePrec3 = 0;//  Math.Round((ChangeVaule3 / MonthTransaction3) * 100, 3);
                        }

                        MonthTransaction4 =  April ;
                        ChangeVaule4 =  April - Expected4;
                        ChangePrec4 = 0;
                        if (Expected4 != 0)
                        {
                            ChangePrec4 = 0;// Math.Round((ChangeVaule4 / MonthTransaction4) * 100, 3);
                        }

                        MonthTransaction5 =  May ;
                        ChangeVaule5 =  (May) - Expected5;
                        ChangePrec5 = 0;
                        if (Expected5 != 0)
                        {
                            ChangePrec5 = 0;// Math.Round((ChangeVaule5 / MonthTransaction5) * 100, 3);
                        }

                        MonthTransaction6 = (June);
                        ChangeVaule6 = (June) - Expected6;
                        ChangePrec6 = 0;
                        if (Expected6 != 0)
                        {
                            ChangePrec6 = 0;//  Math.Round((ChangeVaule6 / MonthTransaction6) * 100, 3);
                        }

                        MonthTransaction7 = (July);
                        ChangeVaule7 = (July) - Expected7;
                        ChangePrec7 = 0;
                        if (Expected7 != 0)
                        {
                            ChangePrec7 = 0;//  Math.Round((ChangeVaule7 / MonthTransaction7) * 100, 3);
                        }



                        MonthTransaction8 = (August);
                        ChangeVaule8 = (August) - Expected8;
                        ChangePrec8 = 0;
                        if (Expected8 != 0)
                        {
                            ChangePrec8 = 0;//  Math.Round((ChangeVaule8 / MonthTransaction8) * 100, 3);
                        }


                        MonthTransaction9 = (September);
                        ChangeVaule9 = (September) - Expected9;
                        ChangePrec9 = 0;
                        if (Expected9 != 0)
                        {
                            ChangePrec9 = 0;//  Math.Round((ChangeVaule9 / MonthTransaction9) * 100, 3);
                        }


                        MonthTransaction10 = (October);
                        ChangeVaule10 = (October) - Expected10;
                        ChangePrec10 = 0;
                        if (Expected10 != 0)
                        {
                            ChangePrec10 = 0;//  Math.Round((ChangeVaule10 / MonthTransaction10) * 100, 3);
                        }


                        MonthTransaction11 = (November);
                        ChangeVaule11 = (November) - Expected11;
                        ChangePrec11 = 0;
                        if (Expected11 != 0)
                        {
                            ChangePrec11 = 0;// Math.Round((ChangeVaule11 / MonthTransaction11) * 100, 3);
                        }


                        MonthTransaction12 = (December);
                        ChangeVaule12 = (December) - Expected12;
                        ChangePrec12 = 0;
                        if (Expected12 != 0)
                        {
                            ChangePrec12 = 0;//  Math.Round((ChangeVaule12 / MonthTransaction12) * 100, 3);
                        }


                        trialBalance.Expected1 = Expected1;
                        trialBalance.MonthTransaction1 = MonthTransaction1;
                        trialBalance.ChangeVaule1 = ChangeVaule1;
                        trialBalance.ChangePrec1 = ChangePrec1;

                        trialBalance.Expected2 = Expected2;
                        trialBalance.MonthTransaction2 = MonthTransaction2;
                        trialBalance.ChangeVaule2 = ChangeVaule2;
                        trialBalance.ChangePrec2 = ChangePrec2;


                        trialBalance.Expected3 = Expected3;
                        trialBalance.MonthTransaction3 = MonthTransaction3;
                        trialBalance.ChangeVaule3 = ChangeVaule3;
                        trialBalance.ChangePrec3 = ChangePrec3;


                        trialBalance.Expected4 = Expected4;
                        trialBalance.MonthTransaction4 = MonthTransaction4;
                        trialBalance.ChangeVaule4 = ChangeVaule4;
                        trialBalance.ChangePrec4 = ChangePrec4;


                        trialBalance.Expected5 = Expected5;
                        trialBalance.MonthTransaction5 = MonthTransaction5;
                        trialBalance.ChangeVaule5 = ChangeVaule5;
                        trialBalance.ChangePrec5 = ChangePrec5;


                        trialBalance.Expected6 = Expected6;
                        trialBalance.MonthTransaction6 = MonthTransaction6;
                        trialBalance.ChangeVaule6 = ChangeVaule6;
                        trialBalance.ChangePrec6 = ChangePrec6;


                        trialBalance.Expected7 = Expected7;
                        trialBalance.MonthTransaction7 = MonthTransaction7;
                        trialBalance.ChangeVaule7 = ChangeVaule7;
                        trialBalance.ChangePrec7 = ChangePrec7;


                        trialBalance.Expected8 = Expected8;
                        trialBalance.MonthTransaction8 = MonthTransaction8;
                        trialBalance.ChangeVaule8 = ChangeVaule8;
                        trialBalance.ChangePrec8 = ChangePrec8;


                        trialBalance.Expected9 = Expected9;
                        trialBalance.MonthTransaction9 = MonthTransaction9;
                        trialBalance.ChangeVaule9 = ChangeVaule9;
                        trialBalance.ChangePrec9 = ChangePrec9;


                        trialBalance.Expected10 = Expected10;
                        trialBalance.MonthTransaction10 = MonthTransaction10;
                        trialBalance.ChangeVaule10 = ChangeVaule10;
                        trialBalance.ChangePrec10 = ChangePrec10;


                        trialBalance.Expected11 = Expected11;
                        trialBalance.MonthTransaction11 = MonthTransaction11;
                        trialBalance.ChangeVaule11 = ChangeVaule11;
                        trialBalance.ChangePrec11 = ChangePrec11;


                        trialBalance.Expected12 = Expected12;
                        trialBalance.MonthTransaction12 = MonthTransaction12;
                        trialBalance.ChangeVaule12 = ChangeVaule12;
                        trialBalance.ChangePrec12 = ChangePrec12;


                        trialBalance.Expected1 = Expected1;
                        trialBalance.MonthTransaction1 = MonthTransaction1;
                        trialBalance.ChangeVaule1 = ChangeVaule1;
                        trialBalance.ChangePrec1 = ChangePrec1;



                        trialBalance.Expected1 = Expected1;
                        trialBalance.MonthTransaction1 = MonthTransaction1;
                        trialBalance.ChangeVaule1 = ChangeVaule1;
                        trialBalance.ChangePrec1 = ChangePrec1;


                        trialBalance.Expected1 = Expected1;
                        trialBalance.MonthTransaction1 = MonthTransaction1;
                        trialBalance.ChangeVaule1 = ChangeVaule1;
                        trialBalance.ChangePrec1 = ChangePrec1;

                        trialBalance.ExpectedAnnually = (trialBalance.Expected1 + trialBalance.Expected2 + trialBalance.Expected3 + trialBalance.Expected4 + trialBalance.Expected5 + trialBalance.Expected5 +
                                trialBalance.Expected7 + trialBalance.Expected8 + trialBalance.Expected9 + trialBalance.Expected10 + trialBalance.Expected11 + trialBalance.Expected12);

                        trialBalance.ActualTotal =Math.Round( NetTot,4);
                        trialBalance.RemainingExpected = Math.Round( (trialBalance.ExpectedAnnually- Math.Abs(NetTot) ),4);
                        trialBalance.RemainingRatio = 0;
                        trialBalance.ExpectedAnnually =Math.Round(trialBalance.ExpectedAnnually,4);
                        if ((AccountData.FirstOrDefault(m => m.AccountFather == Acc.AccountNumber) != null) && (Acc.AccountLevel < Obj.AccountLevelDropVMID))
                        {
                            trialBalance.IsMainAccount = 1;
                            trialBalance.MainAccount = "{*}";



                            trialBalance.nCreditBalance = 0;
                            trialBalance.nCreditTransAction = 0;
                            trialBalance.nDebitBalance = 0;
                            trialBalance.nDebitTransAction = 0;
                            trialBalance.nNetCredit = 0;
                            trialBalance.nNetDebit = 0;

                        }
                        else
                        {
                            trialBalance.IsMainAccount = 0;
                            trialBalance.MainAccount = "";

                            trialBalance.nExpectedAnnually = trialBalance.ExpectedAnnually;
                            trialBalance.nActualTotal = trialBalance.ActualTotal;
                            trialBalance.nRemainingExpected = trialBalance.RemainingExpected;
                            trialBalance.nRemainingRatio = trialBalance.RemainingRatio;

                            trialBalance.nCreditBalance = CreditBalance;
                            trialBalance.nCreditTransAction = CreditTransAction;
                            trialBalance.nDebitBalance = DebitBalance;
                            trialBalance.nDebitTransAction = DebitTransAction;
                            trialBalance.nNetCredit = NetCredit;
                            trialBalance.nNetDebit = NetDebit;

                            trialBalance.nJanuary = January;
                            trialBalance.nFebruary = February;
                            trialBalance.nMarch = March;
                            trialBalance.nApril = April;
                            trialBalance.nMay = May;
                            trialBalance.nJune = June;
                            trialBalance.nJuly = July;
                            trialBalance.nAugust = August;
                            trialBalance.nSeptember = September;
                            trialBalance.nOctober = October;
                            trialBalance.nNovember = November;
                            trialBalance.nDecember = December;

                            //trialBalance.nNetTot = NetTot;

                            //trialBalance.nOpenBalance = OpenBalance;

                            //trialBalance.nExpected1 = Expected1;
                            //trialBalance.nMonthTransaction1 = MonthTransaction1;
                            //trialBalance.nChangeVaule1 = ChangeVaule1;
                            //trialBalance.nChangePrec1 = ChangePrec1;

                            //trialBalance.nExpected2 = Expected2;
                            //trialBalance.nMonthTransaction2 = MonthTransaction2;
                            //trialBalance.nChangeVaule2 = ChangeVaule2;
                            //trialBalance.nChangePrec2 = ChangePrec2;


                            //trialBalance.nExpected3 = Expected3;
                            //trialBalance.nMonthTransaction3 = MonthTransaction3;
                            //trialBalance.nChangeVaule3 = ChangeVaule3;
                            //trialBalance.nChangePrec3 = ChangePrec3;


                            //trialBalance.nExpected4 = Expected4;
                            //trialBalance.nMonthTransaction4 = MonthTransaction4;
                            //trialBalance.nChangeVaule4 = ChangeVaule4;
                            //trialBalance.nChangePrec4 = ChangePrec4;


                            //trialBalance.nExpected5 = Expected5;
                            //trialBalance.nMonthTransaction5 = MonthTransaction5;
                            //trialBalance.nChangeVaule5 = ChangeVaule5;
                            //trialBalance.nChangePrec5 = ChangePrec5;


                            //trialBalance.nExpected6 = Expected6;
                            //trialBalance.nMonthTransaction6 = MonthTransaction6;
                            //trialBalance.nChangeVaule6 = ChangeVaule6;
                            //trialBalance.nChangePrec6 = ChangePrec6;


                            //trialBalance.nExpected7 = Expected7;
                            //trialBalance.nMonthTransaction7 = MonthTransaction7;
                            //trialBalance.nChangeVaule7 = ChangeVaule7;
                            //trialBalance.nChangePrec7 = ChangePrec7;


                            //trialBalance.nExpected8 = Expected8;
                            //trialBalance.nMonthTransaction8 = MonthTransaction8;
                            //trialBalance.nChangeVaule8 = ChangeVaule8;
                            //trialBalance.nChangePrec8 = ChangePrec8;


                            //trialBalance.nExpected9 = Expected9;
                            //trialBalance.nMonthTransaction9 = MonthTransaction9;
                            //trialBalance.nChangeVaule9 = ChangeVaule9;
                            //trialBalance.nChangePrec9 = ChangePrec9;


                            //trialBalance.nExpected10 = Expected10;
                            //trialBalance.nMonthTransaction10 = MonthTransaction10;
                            //trialBalance.nChangeVaule10 = ChangeVaule10;
                            //trialBalance.nChangePrec10 = ChangePrec10;


                            //trialBalance.nExpected11 = Expected11;
                            //trialBalance.nMonthTransaction11 = MonthTransaction11;
                            //trialBalance.nChangeVaule11 = ChangeVaule11;
                            //trialBalance.nChangePrec11 = ChangePrec11;


                            //trialBalance.nExpected12 = Expected12;
                            //trialBalance.nMonthTransaction12 = MonthTransaction12;
                            //trialBalance.nChangeVaule12 = ChangeVaule12;
                            //trialBalance.nChangePrec12 = ChangePrec12;


                            //trialBalance.nExpected1 = Expected1;
                            //trialBalance.nMonthTransaction1 = MonthTransaction1;
                            //trialBalance.nChangeVaule1 = ChangeVaule1;
                            //trialBalance.nChangePrec1 = ChangePrec1;



                            //trialBalance.nExpected1 = Expected1;
                            //trialBalance.nMonthTransaction1 = MonthTransaction1;
                            //trialBalance.nChangeVaule1 = ChangeVaule1;
                            //trialBalance.nChangePrec1 = ChangePrec1;


                            //trialBalance.nExpected1 = Expected1;
                            //trialBalance.nMonthTransaction1 = MonthTransaction1;
                            //trialBalance.nChangeVaule1 = ChangeVaule1;
                            //trialBalance.nChangePrec1 = ChangePrec1;


                        }
                        if (!Obj.ShowEstimatedZero)
                        {
                            if ((trialBalance.Expected1 + trialBalance.Expected2 + trialBalance.Expected3 + trialBalance.Expected4 + trialBalance.Expected5 + trialBalance.Expected5 +
                                trialBalance.Expected7 + trialBalance.Expected8 + trialBalance.Expected9 + trialBalance.Expected10 + trialBalance.Expected11 + trialBalance.Expected12) != 0)
                            {
                                TrialBalanceVMList.Add(trialBalance);
                            }
                        }
                        else
                        {
                            //if ((trialBalance.NetCredit != 0) || (trialBalance.NetDebit != 0) || (TotalDebit != 0) || (NetCredit != 0))
                            //{
                            TrialBalanceVMList.Add(trialBalance);
                            //}


                        }




                    }






                }
                if (!Obj.ShowMainAccountValue)
                {
                    foreach (var trialBalance in TrialBalanceVMList.Where(m => m.IsMainAccount == 1))
                    {
                        if (trialBalance.IsMainAccount == 1)
                        {
                            trialBalance.Expected = 0;
                            trialBalance.MonthTransaction = 0;
                            trialBalance.ChangeVaule = 0;
                            trialBalance.ChangePrec = 0;
                            trialBalance.CreditBalance = 0;
                            trialBalance.CreditTransAction = 0;
                            trialBalance.DebitBalance = 0;
                            trialBalance.DebitTransAction = 0;
                            trialBalance.NetCredit = 0;
                            trialBalance.NetDebit = 0;


                        
                                        trialBalance.ActualTotal = 0;
                                        trialBalance.RemainingExpected = 0;
                                        trialBalance.RemainingRatio = 0;
                                        trialBalance.ExpectedAnnually = 0;

                                        trialBalance.Expected = 0;
                                        trialBalance.MonthTransaction = 0;
                                        trialBalance.ChangeVaule = 0;
                                        trialBalance.ChangePrec = 0;
                                        trialBalance.CreditBalance = 0;
                                        trialBalance.CreditTransAction = 0;
                                        trialBalance.DebitBalance = 0;
                                        trialBalance.DebitTransAction = 0;
                                        trialBalance.NetCredit = 0;
                                        trialBalance.NetDebit = 0;


                                        trialBalance.Expected1 = 0;
                                        trialBalance.MonthTransaction1 = 0;
                                        trialBalance.ChangeVaule1 = 0;
                                        trialBalance.ChangePrec1 = 0;

                                        trialBalance.Expected2 = 0;
                                        trialBalance.MonthTransaction2 = 0;
                                        trialBalance.ChangeVaule2 = 0;
                                        trialBalance.ChangePrec2 = 0;


                                        trialBalance.Expected3 = 0;
                                        trialBalance.MonthTransaction3 = 0;
                                        trialBalance.ChangeVaule3 = 0;
                                        trialBalance.ChangePrec3 = 0;


                                        trialBalance.Expected4 = 0;
                                        trialBalance.MonthTransaction4 = 0;
                                        trialBalance.ChangeVaule4 = 0;
                                        trialBalance.ChangePrec4 = 0;


                                        trialBalance.Expected5 = 0;
                                        trialBalance.MonthTransaction5 = 0;
                                        trialBalance.ChangeVaule5 = 0;
                                        trialBalance.ChangePrec5 = 0;


                                        trialBalance.Expected6 = 0;
                                        trialBalance.MonthTransaction6 = 0;
                                        trialBalance.ChangeVaule6 = 0;
                                        trialBalance.ChangePrec6 = 0;


                                        trialBalance.Expected7 = 0;
                                        trialBalance.MonthTransaction7 = 0;
                                        trialBalance.ChangeVaule7 = 0;
                                        trialBalance.ChangePrec7 = 0;


                                        trialBalance.Expected8 = 0;
                                        trialBalance.MonthTransaction8 = 0;
                                        trialBalance.ChangeVaule8 = 0;
                                        trialBalance.ChangePrec8 = 0;


                                        trialBalance.Expected9 = 0;
                                        trialBalance.MonthTransaction9 = 0;
                                        trialBalance.ChangeVaule9 = 0;
                                        trialBalance.ChangePrec9 = 0;


                                        trialBalance.Expected10 = 0;
                                        trialBalance.MonthTransaction10 = 0;
                                        trialBalance.ChangeVaule10 = 0;
                                        trialBalance.ChangePrec10 = 0;


                                        trialBalance.Expected11 = 0;
                                        trialBalance.MonthTransaction11 = 0;
                                        trialBalance.ChangeVaule11 = 0;
                                        trialBalance.ChangePrec11 = 0;


                                        trialBalance.Expected12 = 0;
                                        trialBalance.MonthTransaction12 = 0;
                                        trialBalance.ChangeVaule12 = 0;
                                        trialBalance.ChangePrec12 = 0;


                                        trialBalance.January = 0;
                                        trialBalance.February = 0;
                                        trialBalance.March = 0;
                                        trialBalance.April = 0;
                                        trialBalance.May = 0;
                                        trialBalance.June = 0;
                                        trialBalance.July = 0;
                                        trialBalance.August = 0;
                                        trialBalance.September = 0;
                                        trialBalance.October = 0;
                                        trialBalance.November = 0;
                                        trialBalance.December = 0;

                                        trialBalance.nActualTotal = 0;
                                        trialBalance.nRemainingExpected = 0;
                                        trialBalance.nRemainingRatio = 0;
                                        trialBalance.nExpectedAnnually = 0;

                                        trialBalance.nExpected = 0;
                                        trialBalance.nMonthTransaction = 0;
                                        trialBalance.nChangeVaule = 0;
                                        trialBalance.nChangePrec = 0;
                                        trialBalance.nCreditBalance = 0;
                                        trialBalance.nCreditTransAction = 0;
                                        trialBalance.nDebitBalance = 0;
                                        trialBalance.nDebitTransAction = 0;
                                        trialBalance.nNetCredit = 0;
                                        trialBalance.nNetDebit = 0;


                                        trialBalance.nExpected1 = 0;
                                        trialBalance.nMonthTransaction1 = 0;
                                        trialBalance.nChangeVaule1 = 0;
                                        trialBalance.nChangePrec1 = 0;

                                        trialBalance.nExpected2 = 0;
                                        trialBalance.nMonthTransaction2 = 0;
                                        trialBalance.nChangeVaule2 = 0;
                                        trialBalance.nChangePrec2 = 0;


                                        trialBalance.nExpected3 = 0;
                                        trialBalance.nMonthTransaction3 = 0;
                                        trialBalance.nChangeVaule3 = 0;
                                        trialBalance.nChangePrec3 = 0;


                                        trialBalance.nExpected4 = 0;
                                        trialBalance.nMonthTransaction4 = 0;
                                        trialBalance.nChangeVaule4 = 0;
                                        trialBalance.nChangePrec4 = 0;


                                        trialBalance.nExpected5 = 0;
                                        trialBalance.nMonthTransaction5 = 0;
                                        trialBalance.nChangeVaule5 = 0;
                                        trialBalance.nChangePrec5 = 0;


                                        trialBalance.nExpected6 = 0;
                                        trialBalance.nMonthTransaction6 = 0;
                                        trialBalance.nChangeVaule6 = 0;
                                        trialBalance.nChangePrec6 = 0;


                                        trialBalance.nExpected7 = 0;
                                        trialBalance.nMonthTransaction7 = 0;
                                        trialBalance.nChangeVaule7 = 0;
                                        trialBalance.nChangePrec7 = 0;


                                        trialBalance.nExpected8 = 0;
                                        trialBalance.nMonthTransaction8 = 0;
                                        trialBalance.nChangeVaule8 = 0;
                                        trialBalance.nChangePrec8 = 0;


                                        trialBalance.nExpected9 = 0;
                                        trialBalance.nMonthTransaction9 = 0;
                                        trialBalance.nChangeVaule9 = 0;
                                        trialBalance.nChangePrec9 = 0;


                                        trialBalance.nExpected10 = 0;
                                        trialBalance.nMonthTransaction10 = 0;
                                        trialBalance.nChangeVaule10 = 0;
                                        trialBalance.nChangePrec10 = 0;


                                        trialBalance.nExpected11 = 0;
                                        trialBalance.nMonthTransaction11 = 0;
                                        trialBalance.nChangeVaule11 = 0;
                                        trialBalance.nChangePrec11 = 0;


                                        trialBalance.nExpected12 = 0;
                                        trialBalance.nMonthTransaction12 = 0;
                                        trialBalance.nChangeVaule12 = 0;
                                        trialBalance.nChangePrec12 = 0;


                                        trialBalance.nJanuary = 0;
                                        trialBalance.nFebruary = 0;
                                        trialBalance.nMarch = 0;
                                        trialBalance.nApril = 0;
                                        trialBalance.nMay = 0;
                                        trialBalance.nJune = 0;
                                        trialBalance.nJuly = 0;
                                        trialBalance.nAugust = 0;
                                        trialBalance.nSeptember = 0;
                                        trialBalance.nOctober = 0;
                                        trialBalance.nNovember = 0;
                                        trialBalance.nDecember = 0;
                                   

                            

                        }

                    }

                }



                return Json(TrialBalanceVMList, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                string err = ex.Message;

                List<TrialBalanceVM> TrialBalanceVMList = new List<TrialBalanceVM>();

                return Json(TrialBalanceVMList, JsonRequestBehavior.AllowGet);

            }


        }
        [HttpPost]
        public JsonResult GetTrialBalanceYearly(AccountLevelRepVM Obj)
        {
            try
            {

                var userId = User.Identity.GetUserId();
                string UserID = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
                var AccountData = _unitOfWork.NativeSql.GetChartOfAccountByLevel(UserInfo.fCompanyId);
                
                int CurrYear = UserInfo.CurrYear;
                Obj.Detail = true;
                var Date = "01/01/" + CurrYear;
                var OpenDate = "31/12/" + (CurrYear - 1).ToString();
                Obj.FromDate=DateTime.Parse( "01/01/" + CurrYear);
                Obj.ToDate = DateTime.Parse("31/12/" + CurrYear);
                string TempCostID = "0";
                int TempCostType = 0;

                if (Obj.ByCostCenter)
                {
                    TempCostID = Obj.CostCenterNumber;
                    if (Obj.Partofthenumber)
                    {
                        TempCostType = Obj.CostSearchType;
                    }


                }



                IEnumerable<TrialBalanceVM> TotData = new List<TrialBalanceVM>();

                if (Obj.Partofthenumber)
                {
                    if (Obj.CostSearchType == 1)
                    {
                        Obj.CostCenterNumber = Obj.CostCenterNumber + "%";
                    }
                    else if (Obj.CostSearchType == 2)
                    {
                        Obj.CostCenterNumber = "%" + Obj.CostCenterNumber;
                    }
                    else if (Obj.CostSearchType == 3)
                    {
                        Obj.CostCenterNumber = "%" + Obj.CostCenterNumber + "%";

                    }
                }
                var TranData = _unitOfWork.NativeSql.GetTransactionForTrial(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate, Obj.ByCostCenter, Obj.CostCenterNumber, Obj.Partofthenumber, Obj.CostSearchType);
                var MonthlyData=  _unitOfWork.NativeSql.GetTransactionForTrialYearly(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate, Obj.ByCostCenter, Obj.CostCenterNumber, Obj.Partofthenumber, Obj.CostSearchType);

                if (DateTime.Parse(Date) != Obj.FromDate)
                    TotData = _unitOfWork.NativeSql.GetTotCreditDebitForTrial(UserInfo.fCompanyId, DateTime.Parse(Date), Obj.FromDate, Obj.ByCostCenter, Obj.CostCenterNumber, Obj.Partofthenumber, Obj.CostSearchType);


                //----------------Get Open-------------//
                IEnumerable<TrialBalanceVM> TotDataOpen = new List<TrialBalanceVM>();
                TotDataOpen = _unitOfWork.NativeSql.GetTotCreditDebitForTrialOpen(UserInfo.fCompanyId, DateTime.Parse(OpenDate), DateTime.Parse(OpenDate));
                foreach (var D in TotDataOpen)
                {
                    TotData = TotData.Append(D);

                }
                //------------------------------------//


                double TotalDebit = 0;
                double TOTCredit = 0;
                double NetCredit = 0;
                double NetDebit = 0;
                double CreditBalance = 0;
                double DebitBalance = 0;
                double CreditTransAction = 0;
                double DebitTransAction = 0;

                double January = 0;
                double February = 0;
                double March = 0;
                double April = 0;
                double May = 0;
                double June = 0;
                double July = 0;
                double August = 0;
                double September = 0;
                double October = 0;
                double November = 0;
                double December = 0;

                double NetTot = 0;
                double OpenBalance = 0;
                List<TrialBalanceVM> TrialBalanceVMList = new List<TrialBalanceVM>();

                IEnumerable<ChartOfAccount> Accounts = new List<ChartOfAccount>();

                IEnumerable<ChartOfAccount> LessMainAccount = new List<ChartOfAccount>();
                
                

                
                if ((Obj.AccountLevelDropVMID > 0) && (Obj.Detail))
                {


                    Accounts = AccountData.Where(m => m.AccountLevel <= Obj.AccountLevelDropVMID).ToList().OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();

                    foreach (var Acc in Accounts)//Each Main Account
                    {
                        TotalDebit = 0;
                        TOTCredit = 0;
                        NetCredit = 0;
                        NetDebit = 0;
                        CreditBalance = 0;
                        DebitBalance = 0;
                        CreditTransAction = 0;
                        DebitTransAction = 0;

                        January = 0;
                        February = 0;
                        March = 0;
                        April = 0;
                        May = 0;
                        June = 0;
                        July = 0;
                        August = 0;
                        September = 0;
                        October = 0;
                        November = 0;
                        December = 0;

                        NetTot =0;

                        var MainAccount = AccountData.FirstOrDefault(m => m.AccountNumber == Acc.AccountNumber);
                        var MainChild = AccountData.Where(m => m.AccountFather == Acc.AccountNumber).OrderBy(m => m.AccountNumber).OrderBy(m => m.AccountLevel).ToList();

                      
                        var TransActionData = TranData.Where(m => m.AccountNumber != null);

                        var BalanceData = TotData.Where(m => m.AccountNumber != null);
                        var MonthlyTrans= MonthlyData.Where(m => m.AccountNumber != null);
                        foreach (var D in TransActionData)
                        {
                            TransActionData = TranData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber));
                            CreditTransAction += D.CreditTransAction;
                            DebitTransAction += D.DebitTransAction;
                        }
                        foreach (var Balance in BalanceData)
                        {
                             BalanceData = TotData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber));
                            CreditBalance += Balance.CreditBalance;
                            DebitBalance += Balance.DebitBalance;
                        }
                        foreach (var D in MonthlyTrans)
                        {
                             MonthlyTrans = MonthlyData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber));

                            switch (D.Month)
                            {
                                case 1:
                                    January = January + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 2:
                                    February = February + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 3:
                                    March = March + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 4:
                                    April = April + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 5:
                                    May = May + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 6:
                                    June = June + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 7:
                                    July = July + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 8:
                                    August = August + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 9:
                                    September = September + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 10:
                                    October = October + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 11:
                                    November = November + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 12:
                                    December = December + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                               
                            }
                           
                        }



                        TotalDebit = DebitTransAction + DebitBalance;
                        TOTCredit = CreditTransAction + CreditBalance;

                        if (TotalDebit > TOTCredit)
                        {
                            NetDebit = TotalDebit - TOTCredit;
                            NetTot=NetDebit;
                        }
                        if (TotalDebit < TOTCredit)
                        {
                            NetCredit = TOTCredit - TotalDebit;
                            NetTot = NetCredit;


                        }
                        NetTot = TotalDebit - TOTCredit;
                        OpenBalance = DebitBalance - CreditBalance;

                        TrialBalanceVM trialBalance = new TrialBalanceVM();
                        trialBalance.AccountNumber = Acc.AccountNumber;
                        trialBalance.Name = Acc.ArabicName;

                        trialBalance.CreditBalance = CreditBalance;
                        trialBalance.CreditTransAction = CreditTransAction;
                        trialBalance.DebitBalance = DebitBalance;
                        trialBalance.DebitTransAction = DebitTransAction;
                        trialBalance.NetCredit = NetCredit;
                        trialBalance.NetDebit = NetDebit;

                        trialBalance.January = January;
                        trialBalance.February = February;
                        trialBalance.March = March;
                        trialBalance.April = April;
                        trialBalance.May = May;
                        trialBalance.June = June;
                        trialBalance.July = July;
                        trialBalance.August = August;
                        trialBalance.September = September;
                        trialBalance.October = October;
                        trialBalance.November = November;
                        trialBalance.December = December;

                        trialBalance.OpenBalance = OpenBalance;
                        trialBalance.NetTot = NetTot;
                        trialBalance.TempCostID = TempCostID;
                        trialBalance.TempCostType = TempCostType;
                        //  NetTot = January + February + March + April + May + June + July + August + September + October + November + December+ OpenBalance;

                        if ((AccountData.FirstOrDefault(m => m.AccountFather == Acc.AccountNumber) != null) && (Acc.AccountLevel < Obj.AccountLevelDropVMID))
                        {
                            trialBalance.IsMainAccount = 1;
                            trialBalance.MainAccount = "{*}";


                            trialBalance.nCreditBalance = 0;
                            trialBalance.nCreditTransAction = 0;
                            trialBalance.nDebitBalance = 0;
                            trialBalance.nDebitTransAction = 0;
                            trialBalance.nNetCredit = 0;
                            trialBalance.nNetDebit = 0;

                        }
                        else
                        {
                            trialBalance.IsMainAccount = 0;
                            trialBalance.MainAccount = "";
                            trialBalance.nCreditBalance = CreditBalance;
                            trialBalance.nCreditTransAction = CreditTransAction;
                            trialBalance.nDebitBalance = DebitBalance;
                            trialBalance.nDebitTransAction = DebitTransAction;
                            trialBalance.nNetCredit = NetCredit;
                            trialBalance.nNetDebit = NetDebit;

                            trialBalance.nJanuary = January;
                            trialBalance.nFebruary = February;
                            trialBalance.nMarch = March;
                            trialBalance.nApril = April;
                            trialBalance.nMay = May;
                            trialBalance.nJune = June;
                            trialBalance.nJuly = July;
                            trialBalance.nAugust = August;
                            trialBalance.nSeptember = September;
                            trialBalance.nOctober = October;
                            trialBalance.nNovember = November;
                            trialBalance.nDecember = December;

                            trialBalance.nNetTot = NetTot;

                            trialBalance.nOpenBalance = OpenBalance;


                        }
                        if (!Obj.ShowZeroBalance)
                        {
                            if ((trialBalance.NetCredit - trialBalance.NetDebit) != 0)
                            {
                                TrialBalanceVMList.Add(trialBalance);
                            }
                        }
                        else
                        {
                            if ((trialBalance.NetCredit != 0) || (trialBalance.NetDebit != 0) || (TotalDebit != 0) || (NetCredit != 0))
                            {
                                TrialBalanceVMList.Add(trialBalance);
                            }


                        }




                    }






                }




                return Json(TrialBalanceVMList, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                string err = ex.Message;

                List<TrialBalanceVM> TrialBalanceVMList = new List<TrialBalanceVM>();

                return Json(TrialBalanceVMList, JsonRequestBehavior.AllowGet);

            }


        }
        [Authorize(Roles = "CoOwner,RepTransActionDetails")]
        public ActionResult TransActionDetails()
        {

            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            //  var GetAllLevel = _unitOfWork.NativeSql.GetAccountLevelVMs(UserInfo.fCompanyId);

            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);




            int CurrYear = UserInfo.CurrYear;

            TransReportVM Obj = new TransReportVM
            {

                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                

            };
            ViewData["FromDate"]  = DateTime.Parse("01/"+ DateTime.Now.Month.ToString("00") +"/"+ CurrYear);
            ViewData["ToDate"]  = DateTime.Now;



            return View(Obj);
        }
        [Authorize(Roles = "CoOwner,RepSearchForTrans")]
        public ActionResult SearchForTrans()
        {

            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            //  var GetAllLevel = _unitOfWork.NativeSql.GetAccountLevelVMs(UserInfo.fCompanyId);

            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
         
             


            int CurrYear = UserInfo.CurrYear;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);

            TransReportVM Obj = new TransReportVM
            {
                
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                DebitCredit=0,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency


            };



            return View(Obj);
        }
        [HttpPost]
        public JsonResult GetTransAction(TransReportVM Obj)
        {
            try
            {


                var userId = User.Identity.GetUserId();
                string UserID = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
                int CurrYear = UserInfo.CurrYear;
                var TransData = _unitOfWork.NativeSql.GetTransActionDatasReport(UserInfo.fCompanyId, Obj.ByDate, Obj.FromDate, Obj.ToDate);

                var CompanyTransAction = _unitOfWork.CompanyTransactionKind.GetAllCompanyTransactionKind(UserInfo.fCompanyId);
                var TransActionKind = _unitOfWork.TransactionKind.GetAllTransactionKind();

                List<TransActionDataVM> ObjAllData = new List<TransActionDataVM>();
                if (Obj.DebitCredit == 0)
                {
                    TransData = TransData.Where(m => m.Debit !=0 ).ToList();
                }
                else
                {
                    TransData = TransData.Where(m => m.Credit !=0 ).ToList();
                }
                if (Obj.ByAcc)
                {
                    TransData = TransData.Where(m => m.AccountNumber == Obj.AccountNumber).ToList();
                }

                if (Obj.ByValue)
                {
                    if (Obj.DebitCredit == 0)
                    {
                        TransData = TransData.Where(m => m.Debit >= Obj.FromValue && m.Debit <= Obj.ToValue).ToList();
                    }
                    else
                    {
                        TransData = TransData.Where(m => m.Credit >= Obj.FromValue && m.Credit <= Obj.ToValue).ToList();
                    }
                }
                if (Obj.ByNote)
                {
                    TransData = TransData.Where(m => m.Note != null).ToList();
                    TransData = TransData.Where(m => m.Note.Contains(Obj.Note)).ToList();
                }
                foreach (var D in TransData)
                {
                    if (D.CompanyTransactionKindNo == 0)
                    {
                        var V = TransActionKind.FirstOrDefault(m => m.TransactionKindID == D.TransactionKindNo);
                        if (V != null)
                        {
                            D.TransName = V.ArabicName;
                        }


                    } //else
                    {
                        var V = CompanyTransAction.FirstOrDefault(m => m.TransactionKindID == D.TransactionKindNo
                          && m.CompanyTransactionKindID == D.CompanyTransactionKindNo);
                        if (V != null)
                        {
                            D.TransName = V.ArabicName;
                        }


                    }

                    if (Obj.DebitCredit == 0)
                    {
                        D.Value = D.Debit;
                    }
                    else
                    {
                        D.Value = D.Credit;
                    }
                    ObjAllData.Add(D);
                }

                return Json(ObjAllData, JsonRequestBehavior.AllowGet);

            }



            catch (Exception ex)
            {
                string err = ex.Message;



                return Json(new List<TransActionDataVM>(), JsonRequestBehavior.AllowGet);

            }

        }
        [HttpPost]
        public JsonResult GetEstimatedBudgetForCostCenter(AccountLevelRepVM Obj)
        {
            try
            {

                var userId = User.Identity.GetUserId();
                string UserID = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
                var AccountData = _unitOfWork.NativeSql.GetCostCenterByLevel(UserInfo.fCompanyId);

                int CurrYear = UserInfo.CurrYear;
                Obj.Detail = true;
                var Date = "01/01/" + CurrYear;
                var OpenDate = "31/12/" + (CurrYear - 1).ToString();
                Obj.FromDate = DateTime.Parse("01/" + Obj.Month.ToString("00") + "/" + CurrYear);
                var LastDay = DateTime.DaysInMonth(CurrYear, Obj.Month).ToString("00");
                Obj.ToDate = DateTime.Parse(LastDay + "/" + Obj.Month.ToString("00") + "/" + CurrYear);
                if (Obj.Partofthenumber)
                {
                    if (Obj.CostSearchType == 1)
                    {
                        Obj.CostCenterNumber = Obj.CostCenterNumber + "%";
                    }
                    else if (Obj.CostSearchType == 2)
                    {
                        Obj.CostCenterNumber = "%" + Obj.CostCenterNumber;
                    }
                    else if (Obj.CostSearchType == 3)
                    {
                        Obj.CostCenterNumber = "%" + Obj.CostCenterNumber + "%";

                    }
                }


                IEnumerable<TrialBalanceVM> TotData = new List<TrialBalanceVM>();
                var TranData = _unitOfWork.NativeSql.GetTransactionForTrialCost(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                var MonthlyData = _unitOfWork.NativeSql.GetTransactionForCostMonthly(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                var BudgetData = _unitOfWork.EstimatedBudget.GetAllEstimatedBudgets(UserInfo.fCompanyId, CurrYear);

                //  if (DateTime.Parse(Date) != Obj.FromDate)
                //    TotData = _unitOfWork.NativeSql.GetTotCreditDebitForTrial(UserInfo.fCompanyId, DateTime.Parse(Date), Obj.FromDate);


                //----------------Get Open-------------//
                IEnumerable<TrialBalanceVM> TotDataOpen = new List<TrialBalanceVM>();
                TotDataOpen = _unitOfWork.NativeSql.GetTotCreditDebitForTrialOpen(UserInfo.fCompanyId, DateTime.Parse(OpenDate), DateTime.Parse(OpenDate));
                foreach (var D in TotDataOpen)
                {
                    TotData = TotData.Append(D);

                }
                //------------------------------------//


                double TotalDebit = 0;
                double TOTCredit = 0;
                double NetCredit = 0;
                double NetDebit = 0;
                double CreditBalance = 0;
                double DebitBalance = 0;
                double CreditTransAction = 0;
                double DebitTransAction = 0;

                double January = 0;
                double February = 0;
                double March = 0;
                double April = 0;
                double May = 0;
                double June = 0;
                double July = 0;
                double August = 0;
                double September = 0;
                double October = 0;
                double November = 0;
                double December = 0;

                double NetTot = 0;
                double OpenBalance = 0;
                double MonthTransaction = 0;
                double Expected = 0;
                double ChangeVaule = 0;
                double ChangePrec = 0;

                List<TrialBalanceVM> TrialBalanceVMList = new List<TrialBalanceVM>();

                IEnumerable<CostCenter> Accounts = new List<CostCenter>();

                IEnumerable<ChartOfAccount> LessMainAccount = new List<ChartOfAccount>();




                if ((Obj.AccountLevelDropVMID > 0) && (!Obj.ShowOnlyaccountswithcostcenter))
                {


                    Accounts = AccountData.Where(m => m.CostLevel <= Obj.AccountLevelDropVMID).ToList().OrderBy(m => m.CostNumber).ThenBy(m => m.CostLevel).ToList();

                    foreach (var Acc in Accounts)//Each Main Account
                    {
                        TotalDebit = 0;
                        TOTCredit = 0;
                        NetCredit = 0;
                        NetDebit = 0;
                        CreditBalance = 0;
                        DebitBalance = 0;
                        CreditTransAction = 0;
                        DebitTransAction = 0;

                        January = 0;
                        February = 0;
                        March = 0;
                        April = 0;
                        May = 0;
                        June = 0;
                        July = 0;
                        August = 0;
                        September = 0;
                        October = 0;
                        November = 0;
                        December = 0;

                        NetTot = 0;

                        MonthTransaction = 0;
                        Expected = 0;
                        ChangeVaule = 0;
                        ChangePrec = 0;

                        var MainAccount = AccountData.FirstOrDefault(m => m.CostNumber == Acc.CostNumber);
                        var MainChild = AccountData.Where(m => m.CostFather == Acc.CostNumber).OrderBy(m => m.CostNumber).OrderBy(m => m.CostLevel).ToList();
                      
                        var TransActionData = TranData.Where(m => m.CostCenter.StartsWith(Acc.CostNumber));
                       var BalanceData = TotData.Where(m => m.CostCenter != null);
                        var MonthlyTrans = MonthlyData.Where(m => m.CostCenter.StartsWith(Acc.CostNumber));
                        foreach (var D in TransActionData)
                        {
                            CreditTransAction += D.CreditTransAction;
                            DebitTransAction += D.DebitTransAction;
                        }
                        foreach (var Balance in BalanceData)
                        {
                            BalanceData = TotData.Where(m => m.CostCenter.StartsWith(Acc.CostNumber));

                            CreditBalance += Balance.CreditBalance;
                            DebitBalance += Balance.DebitBalance;
                        }
                        foreach (var D in MonthlyTrans)
                        {
                            switch (D.Month)
                            {
                                case 1:
                                    January = January + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 2:
                                    February = February + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 3:
                                    March = March + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 4:
                                    April = April + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 5:
                                    May = May + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 6:
                                    June = June + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 7:
                                    July = July + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 8:
                                    August = August + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 9:
                                    September = September + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 10:
                                    October = October + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 11:
                                    November = November + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 12:
                                    December = December + (D.DebitTransAction - D.CreditTransAction);
                                    break;

                            }

                        }



                        TotalDebit = DebitTransAction + DebitBalance;
                        TOTCredit = CreditTransAction + CreditBalance;

                        if (TotalDebit > TOTCredit)
                        {
                            NetDebit = TotalDebit - TOTCredit;
                            NetTot = NetDebit;
                        }
                        if (TotalDebit < TOTCredit)
                        {
                            NetCredit = TOTCredit - TotalDebit;
                            NetTot = NetCredit;


                        }
                        NetTot = TotalDebit - TOTCredit;
                        OpenBalance = DebitBalance - CreditBalance;

                        TrialBalanceVM trialBalance = new TrialBalanceVM();
                        trialBalance.AccountNumber = Acc.CostNumber;
                        trialBalance.Name = Acc.ArabicName;

                        trialBalance.CreditBalance = CreditBalance;
                        trialBalance.CreditTransAction = CreditTransAction;
                        trialBalance.DebitBalance = DebitBalance;
                        trialBalance.DebitTransAction = DebitTransAction;
                        trialBalance.NetCredit = NetCredit;
                        trialBalance.NetDebit = NetDebit;

                        trialBalance.January = January;
                        trialBalance.February = February;
                        trialBalance.March = March;
                        trialBalance.April = April;
                        trialBalance.May = May;
                        trialBalance.June = June;
                        trialBalance.July = July;
                        trialBalance.August = August;
                        trialBalance.September = September;
                        trialBalance.October = October;
                        trialBalance.November = November;
                        trialBalance.December = December;

                        trialBalance.OpenBalance = OpenBalance;
                        trialBalance.NetTot = NetTot;
                        BudgetData = BudgetData.Where(m => m.CostCenterNumber != null).ToList();
                        var CurrBudget = BudgetData.Where(m => m.CostCenterNumber.StartsWith(Acc.CostNumber) && m.BudgetType == 2);
                        if ( CurrBudget.Count() > 0)
                        {
                            foreach (var D in CurrBudget)
                            {
                                switch (Obj.Month)
                                {
                                    case 1:
                                        Expected = Expected + D.January;
                                        break;
                                    case 2:
                                        Expected = Expected + D.February;
                                        break;
                                    case 3:
                                        Expected = Expected + D.March;
                                        break;
                                    case 4:
                                        Expected = Expected + D.April;
                                        break;
                                    case 5:
                                        Expected = Expected + D.May;
                                        break;
                                    case 6:
                                        Expected = Expected + D.June;
                                        break;
                                    case 7:
                                        Expected = Expected + D.July;
                                        break;
                                    case 8:
                                        Expected = Expected + D.August;
                                        break;
                                    case 9:
                                        Expected = Expected + D.September;
                                        break;
                                    case 10:
                                        Expected = Expected + D.October;
                                        break;
                                    case 11:
                                        Expected = Expected + D.November;
                                        break;
                                    case 12:
                                        Expected = Expected + D.December;
                                        break;

                                }

                            }
                        }

                        MonthTransaction = NetTot;
                        ChangeVaule = NetTot - Expected;
                        ChangePrec = 0;
                        if (Expected != 0)
                        {
                            if (MonthTransaction != 0)
                                ChangePrec = Math.Round((ChangeVaule / MonthTransaction) * 100, 3);
                        }



                        trialBalance.Expected = Expected;
                        trialBalance.MonthTransaction = MonthTransaction;
                        trialBalance.ChangeVaule = ChangeVaule;
                        trialBalance.ChangePrec = ChangePrec;

                        if ((AccountData.FirstOrDefault(m => m.CostFather == Acc.CostNumber) != null) && (Acc.CostLevel < Obj.AccountLevelDropVMID))
                        {
                            trialBalance.IsMainAccount = 1;
                            trialBalance.MainAccount = "{*}";



                            trialBalance.nCreditBalance = 0;
                            trialBalance.nCreditTransAction = 0;
                            trialBalance.nDebitBalance = 0;
                            trialBalance.nDebitTransAction = 0;
                            trialBalance.nNetCredit = 0;
                            trialBalance.nNetDebit = 0;

                        }
                        else
                        {
                            trialBalance.IsMainAccount = 0;
                            trialBalance.MainAccount = "";
                            trialBalance.nCreditBalance = CreditBalance;
                            trialBalance.nCreditTransAction = CreditTransAction;
                            trialBalance.nDebitBalance = DebitBalance;
                            trialBalance.nDebitTransAction = DebitTransAction;
                            trialBalance.nNetCredit = NetCredit;
                            trialBalance.nNetDebit = NetDebit;

                            trialBalance.nJanuary = January;
                            trialBalance.nFebruary = February;
                            trialBalance.nMarch = March;
                            trialBalance.nApril = April;
                            trialBalance.nMay = May;
                            trialBalance.nJune = June;
                            trialBalance.nJuly = July;
                            trialBalance.nAugust = August;
                            trialBalance.nSeptember = September;
                            trialBalance.nOctober = October;
                            trialBalance.nNovember = November;
                            trialBalance.nDecember = December;

                            trialBalance.nNetTot = NetTot;

                            trialBalance.nOpenBalance = OpenBalance;

                            trialBalance.nMonthTransaction = MonthTransaction;
                            trialBalance.nExpected = Expected;
                            trialBalance.nChangeVaule = ChangeVaule;
                            trialBalance.nChangePrec = ChangePrec;


                        }
                        if (!Obj.ShowEstimatedZero)
                        {
                            if ((trialBalance.Expected) != 0)
                            {
                                TrialBalanceVMList.Add(trialBalance);
                            }
                        }
                        else
                        {
                            //if ((trialBalance.NetCredit != 0) || (trialBalance.NetDebit != 0) || (TotalDebit != 0) || (NetCredit != 0))
                            //{
                            TrialBalanceVMList.Add(trialBalance);
                            //}


                        }




                    }






                }
                else if((Obj.ShowOnlyaccountswithcostcenter) && !String.IsNullOrEmpty(Obj.AccNo))
                {
                   
                    var AccountByCost = _unitOfWork.NativeSql.GetChartOfAccountByCostCenterID(UserInfo.fCompanyId, Obj.AccNo, 3);
                      TranData = _unitOfWork.NativeSql.GetTransactionForTrial(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate, Obj.ByCostCenter, Obj.CostCenterNumber, Obj.Partofthenumber, Obj.CostSearchType);
                      MonthlyData = _unitOfWork.NativeSql.GetTransactionForTrialYearly(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate, Obj.ByCostCenter, Obj.CostCenterNumber, Obj.Partofthenumber, Obj.CostSearchType);
                      BudgetData = _unitOfWork.EstimatedBudget.GetAllEstimatedBudgets(UserInfo.fCompanyId, CurrYear);
                    //  Accounts = AccountData.Where(m => m.CostLevel <= Obj.AccountLevelDropVMID).ToList().OrderBy(m => m.CostNumber).ThenBy(m => m.CostLevel).ToList();

                    foreach (var Acc in AccountByCost)//Each Main Account
                    {
                        TotalDebit = 0;
                        TOTCredit = 0;
                        NetCredit = 0;
                        NetDebit = 0;
                        CreditBalance = 0;
                        DebitBalance = 0;
                        CreditTransAction = 0;
                        DebitTransAction = 0;

                        January = 0;
                        February = 0;
                        March = 0;
                        April = 0;
                        May = 0;
                        June = 0;
                        July = 0;
                        August = 0;
                        September = 0;
                        October = 0;
                        November = 0;
                        December = 0;

                        NetTot = 0;

                        MonthTransaction = 0;
                        Expected = 0;
                        ChangeVaule = 0;
                        ChangePrec = 0;

                      
                        var TransActionData = TranData.Where(m => m.AccountNumber==Acc.AccountNumber);
                        var BalanceData = TotData.Where(m => m.AccountNumber == Acc.AccountNumber);
                        var MonthlyTrans = MonthlyData.Where(m => m.AccountNumber == Acc.AccountNumber);
                        foreach (var D in TransActionData)
                        {
                            CreditTransAction += D.CreditTransAction;
                            DebitTransAction += D.DebitTransAction;
                        }
                        foreach (var Balance in BalanceData)
                        {
                            CreditBalance += Balance.CreditBalance;
                            DebitBalance += Balance.DebitBalance;
                        }
                        foreach (var D in MonthlyTrans)
                        {
                            switch (D.Month)
                            {
                                case 1:
                                    January = January + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 2:
                                    February = February + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 3:
                                    March = March + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 4:
                                    April = April + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 5:
                                    May = May + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 6:
                                    June = June + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 7:
                                    July = July + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 8:
                                    August = August + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 9:
                                    September = September + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 10:
                                    October = October + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 11:
                                    November = November + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 12:
                                    December = December + (D.DebitTransAction - D.CreditTransAction);
                                    break;

                            }

                        }



                        TotalDebit = DebitTransAction + DebitBalance;
                        TOTCredit = CreditTransAction + CreditBalance;

                        if (TotalDebit > TOTCredit)
                        {
                            NetDebit = TotalDebit - TOTCredit;
                            NetTot = NetDebit;
                        }
                        if (TotalDebit < TOTCredit)
                        {
                            NetCredit = TOTCredit - TotalDebit;
                            NetTot = NetCredit;


                        }
                        NetTot = TotalDebit - TOTCredit;
                        OpenBalance = DebitBalance - CreditBalance;

                        TrialBalanceVM trialBalance = new TrialBalanceVM();
                        trialBalance.AccountNumber = Acc.AccountNumber;
                        trialBalance.Name = Acc.ArabicName;

                        trialBalance.CreditBalance = CreditBalance;
                        trialBalance.CreditTransAction = CreditTransAction;
                        trialBalance.DebitBalance = DebitBalance;
                        trialBalance.DebitTransAction = DebitTransAction;
                        trialBalance.NetCredit = NetCredit;
                        trialBalance.NetDebit = NetDebit;

                        trialBalance.January = January;
                        trialBalance.February = February;
                        trialBalance.March = March;
                        trialBalance.April = April;
                        trialBalance.May = May;
                        trialBalance.June = June;
                        trialBalance.July = July;
                        trialBalance.August = August;
                        trialBalance.September = September;
                        trialBalance.October = October;
                        trialBalance.November = November;
                        trialBalance.December = December;

                        trialBalance.OpenBalance = OpenBalance;
                        trialBalance.NetTot = NetTot;
                        BudgetData = BudgetData.Where(m => m.CostCenterNumber != null).ToList();
                        var CurrBudget = BudgetData.Where(m => m.AccountNumber == Acc.AccountNumber && m.BudgetType == 3);
                        if (CurrBudget.Count() > 0)
                        {
                            foreach (var D in CurrBudget)
                            {
                                switch (Obj.Month)
                                {
                                    case 1:
                                        Expected = Expected + D.January;
                                        break;
                                    case 2:
                                        Expected = Expected + D.February;
                                        break;
                                    case 3:
                                        Expected = Expected + D.March;
                                        break;
                                    case 4:
                                        Expected = Expected + D.April;
                                        break;
                                    case 5:
                                        Expected = Expected + D.May;
                                        break;
                                    case 6:
                                        Expected = Expected + D.June;
                                        break;
                                    case 7:
                                        Expected = Expected + D.July;
                                        break;
                                    case 8:
                                        Expected = Expected + D.August;
                                        break;
                                    case 9:
                                        Expected = Expected + D.September;
                                        break;
                                    case 10:
                                        Expected = Expected + D.October;
                                        break;
                                    case 11:
                                        Expected = Expected + D.November;
                                        break;
                                    case 12:
                                        Expected = Expected + D.December;
                                        break;

                                }

                            }
                        }

                        MonthTransaction = NetTot;
                        ChangeVaule = NetTot - Expected;
                        ChangePrec = 0;
                        if (Expected != 0)
                        {
                            if (MonthTransaction != 0)
                                ChangePrec = Math.Round((ChangeVaule / MonthTransaction) * 100, 3);
                        }



                        trialBalance.Expected = Expected;
                        trialBalance.MonthTransaction = MonthTransaction;
                        trialBalance.ChangeVaule = ChangeVaule;
                        trialBalance.ChangePrec = ChangePrec;

                        if (false/*(AccountData.FirstOrDefault(m => m.CostFather == Acc.CostNumber) != null) && (Acc.CostLevel < Obj.AccountLevelDropVMID)*/)
                        {
                            trialBalance.IsMainAccount = 1;
                            trialBalance.MainAccount = "{*}";



                            trialBalance.nCreditBalance = 0;
                            trialBalance.nCreditTransAction = 0;
                            trialBalance.nDebitBalance = 0;
                            trialBalance.nDebitTransAction = 0;
                            trialBalance.nNetCredit = 0;
                            trialBalance.nNetDebit = 0;

                        }
                        else
                        {
                            trialBalance.IsMainAccount = 0;
                            trialBalance.MainAccount = "";
                            trialBalance.nCreditBalance = CreditBalance;
                            trialBalance.nCreditTransAction = CreditTransAction;
                            trialBalance.nDebitBalance = DebitBalance;
                            trialBalance.nDebitTransAction = DebitTransAction;
                            trialBalance.nNetCredit = NetCredit;
                            trialBalance.nNetDebit = NetDebit;

                            trialBalance.nJanuary = January;
                            trialBalance.nFebruary = February;
                            trialBalance.nMarch = March;
                            trialBalance.nApril = April;
                            trialBalance.nMay = May;
                            trialBalance.nJune = June;
                            trialBalance.nJuly = July;
                            trialBalance.nAugust = August;
                            trialBalance.nSeptember = September;
                            trialBalance.nOctober = October;
                            trialBalance.nNovember = November;
                            trialBalance.nDecember = December;

                            trialBalance.nNetTot = NetTot;

                            trialBalance.nOpenBalance = OpenBalance;

                            trialBalance.nMonthTransaction = MonthTransaction;
                            trialBalance.nExpected = Expected;
                            trialBalance.nChangeVaule = ChangeVaule;
                            trialBalance.nChangePrec = ChangePrec;


                        }
                        if (!Obj.ShowEstimatedZero)
                        {
                            if ((trialBalance.Expected) != 0)
                            {
                                TrialBalanceVMList.Add(trialBalance);
                            }
                        }
                        else
                        {
                            //if ((trialBalance.NetCredit != 0) || (trialBalance.NetDebit != 0) || (TotalDebit != 0) || (NetCredit != 0))
                            //{
                            TrialBalanceVMList.Add(trialBalance);
                            //}


                        }




                    }

                }
                if (!Obj.ShowMainAccountValue)
                {
                    foreach (var trialBalance in TrialBalanceVMList.Where(m => m.IsMainAccount == 1))
                    {
                        if (trialBalance.IsMainAccount == 1)
                        {
                            trialBalance.Expected = 0;
                            trialBalance.MonthTransaction = 0;
                            trialBalance.ChangeVaule = 0;
                            trialBalance.ChangePrec = 0;
                            trialBalance.CreditBalance = 0;
                            trialBalance.CreditTransAction = 0;
                            trialBalance.DebitBalance = 0;
                            trialBalance.DebitTransAction = 0;
                            trialBalance.NetCredit = 0;
                            trialBalance.NetDebit = 0;
                        }

                    }

                }



                return Json(TrialBalanceVMList, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                string err = ex.Message;

                List<TrialBalanceVM> TrialBalanceVMList = new List<TrialBalanceVM>();

                return Json(TrialBalanceVMList, JsonRequestBehavior.AllowGet);

            }


        }
        [HttpPost]
        public JsonResult GetEstimatedBudgetCostAllMonth(AccountLevelRepVM Obj)
        {
            try
            {

                var userId = User.Identity.GetUserId();
                string UserID = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
                var AccountData = _unitOfWork.NativeSql.GetCostCenterByLevel(UserInfo.fCompanyId);
                 

                int CurrYear = UserInfo.CurrYear;
                Obj.Detail = true;
                var Date = "01/01/" + CurrYear;
                var OpenDate = "31/12/" + (CurrYear - 1).ToString();
                Obj.FromDate = DateTime.Parse("01/01/" + CurrYear);
                Obj.ToDate = DateTime.Parse("31/12/" + CurrYear);



                IEnumerable<TrialBalanceVM> TotData = new List<TrialBalanceVM>();
                if (Obj.Partofthenumber)
                {
                    if (Obj.CostSearchType == 1)
                    {
                        Obj.CostCenterNumber = Obj.CostCenterNumber + "%";
                    }
                    else if (Obj.CostSearchType == 2)
                    {
                        Obj.CostCenterNumber = "%" + Obj.CostCenterNumber;
                    }
                    else if (Obj.CostSearchType == 3)
                    {
                        Obj.CostCenterNumber = "%" + Obj.CostCenterNumber + "%";

                    }
                }
                var TranData = _unitOfWork.NativeSql.GetTransactionForTrialCost(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                var MonthlyData = _unitOfWork.NativeSql.GetTransactionForCostMonthly(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                var BudgetData = _unitOfWork.EstimatedBudget.GetAllEstimatedBudgets(UserInfo.fCompanyId, CurrYear);

                //  if (DateTime.Parse(Date) != Obj.FromDate)
                //     TotData = _unitOfWork.NativeSql.GetTotCreditDebitForTrial(UserInfo.fCompanyId, DateTime.Parse(Date), Obj.FromDate);


                //----------------Get Open-------------//
                IEnumerable<TrialBalanceVM> TotDataOpen = new List<TrialBalanceVM>();
                TotDataOpen = _unitOfWork.NativeSql.GetTotCreditDebitForTrialOpen(UserInfo.fCompanyId, DateTime.Parse(OpenDate), DateTime.Parse(OpenDate));
                foreach (var D in TotDataOpen)
                {
                    TotData = TotData.Append(D);

                }
                //------------------------------------//


                double TotalDebit = 0;
                double TOTCredit = 0;
                double NetCredit = 0;
                double NetDebit = 0;
                double CreditBalance = 0;
                double DebitBalance = 0;
                double CreditTransAction = 0;
                double DebitTransAction = 0;

                double January = 0;
                double February = 0;
                double March = 0;
                double April = 0;
                double May = 0;
                double June = 0;
                double July = 0;
                double August = 0;
                double September = 0;
                double October = 0;
                double November = 0;
                double December = 0;

                double NetTot = 0;
                double OpenBalance = 0;

                double MonthTransaction1 = 0;
                double Expected1 = 0;
                double ChangeVaule1 = 0;
                double ChangePrec1 = 0;

                double MonthTransaction2 = 0;
                double Expected2 = 0;
                double ChangeVaule2 = 0;
                double ChangePrec2 = 0;


                double MonthTransaction3 = 0;
                double Expected3 = 0;
                double ChangeVaule3 = 0;
                double ChangePrec3 = 0;


                double MonthTransaction4 = 0;
                double Expected4 = 0;
                double ChangeVaule4 = 0;
                double ChangePrec4 = 0;


                double MonthTransaction5 = 0;
                double Expected5 = 0;
                double ChangeVaule5 = 0;
                double ChangePrec5 = 0;

                double MonthTransaction6 = 0;
                double Expected6 = 0;
                double ChangeVaule6 = 0;
                double ChangePrec6 = 0;


                double MonthTransaction7 = 0;
                double Expected7 = 0;
                double ChangeVaule7 = 0;
                double ChangePrec7 = 0;


                double MonthTransaction8 = 0;
                double Expected8 = 0;
                double ChangeVaule8 = 0;
                double ChangePrec8 = 0;


                double MonthTransaction9 = 0;
                double Expected9 = 0;
                double ChangeVaule9 = 0;
                double ChangePrec9 = 0;


                double MonthTransaction10 = 0;
                double Expected10 = 0;
                double ChangeVaule10 = 0;
                double ChangePrec10 = 0;





                double MonthTransaction11 = 0;
                double Expected11 = 0;
                double ChangeVaule11 = 0;
                double ChangePrec11 = 0;


                double MonthTransaction12 = 0;
                double Expected12 = 0;
                double ChangeVaule12 = 0;
                double ChangePrec12 = 0;

                List<TrialBalanceVM> TrialBalanceVMList = new List<TrialBalanceVM>();

                IEnumerable<CostCenter> Accounts = new List<CostCenter>();

                IEnumerable<ChartOfAccount> LessMainAccount = new List<ChartOfAccount>();




                if ((Obj.AccountLevelDropVMID > 0) && (!Obj.ShowOnlyaccountswithcostcenter))
                {

                    Accounts = AccountData.Where(m => m.CostLevel <= Obj.AccountLevelDropVMID).ToList().OrderBy(m => m.CostNumber).ThenBy(m => m.CostLevel).ToList();

                    foreach (var Acc in Accounts)//Each Main Account
                    {
                        TotalDebit = 0;
                        TOTCredit = 0;
                        NetCredit = 0;
                        NetDebit = 0;
                        CreditBalance = 0;
                        DebitBalance = 0;
                        CreditTransAction = 0;
                        DebitTransAction = 0;

                        January = 0;
                        February = 0;
                        March = 0;
                        April = 0;
                        May = 0;
                        June = 0;
                        July = 0;
                        August = 0;
                        September = 0;
                        October = 0;
                        November = 0;
                        December = 0;

                        NetTot = 0;

                        MonthTransaction1 = 0;
                        Expected1 = 0;
                        ChangeVaule1 = 0;
                        ChangePrec1 = 0;

                        MonthTransaction2 = 0;
                        Expected2 = 0;
                        ChangeVaule2 = 0;
                        ChangePrec2 = 0;


                        MonthTransaction3 = 0;
                        Expected3 = 0;
                        ChangeVaule3 = 0;
                        ChangePrec3 = 0;


                        MonthTransaction4 = 0;
                        Expected4 = 0;
                        ChangeVaule4 = 0;
                        ChangePrec4 = 0;


                        MonthTransaction5 = 0;
                        Expected5 = 0;
                        ChangeVaule5 = 0;
                        ChangePrec5 = 0;

                        MonthTransaction6 = 0;
                        Expected6 = 0;
                        ChangeVaule6 = 0;
                        ChangePrec6 = 0;


                        MonthTransaction7 = 0;
                        Expected7 = 0;
                        ChangeVaule7 = 0;
                        ChangePrec7 = 0;


                        MonthTransaction8 = 0;
                        Expected8 = 0;
                        ChangeVaule8 = 0;
                        ChangePrec8 = 0;


                        MonthTransaction9 = 0;
                        Expected9 = 0;
                        ChangeVaule9 = 0;
                        ChangePrec9 = 0;


                        MonthTransaction10 = 0;
                        Expected10 = 0;
                        ChangeVaule10 = 0;
                        ChangePrec10 = 0;



                        MonthTransaction10 = 0;
                        Expected10 = 0;
                        ChangeVaule10 = 0;
                        ChangePrec10 = 0;


                        MonthTransaction11 = 0;
                        Expected11 = 0;
                        ChangeVaule11 = 0;
                        ChangePrec11 = 0;


                        MonthTransaction12 = 0;
                        Expected12 = 0;
                        ChangeVaule12 = 0;
                        ChangePrec12 = 0;

                        //  var MainAccount = AccountData.FirstOrDefault(m => m.AccountNumber == Acc.AccountNumber);
                        //   var MainChild = AccountData.Where(m => m.AccountFather == Acc.AccountNumber).OrderBy(m => m.AccountNumber).OrderBy(m => m.AccountLevel).ToList();


                        var TransActionData = TranData.Where(m => m.CostCenter.StartsWith(Acc.CostNumber));
                        var BalanceData = TotData.Where(m => m.CostCenter != null);
                        var MonthlyTrans = MonthlyData.Where(m => m.CostCenter.StartsWith(Acc.CostNumber));
                        foreach (var D in TransActionData)
                        {
                            CreditTransAction += D.CreditTransAction;
                            DebitTransAction += D.DebitTransAction;
                        }

                        if(BalanceData != null) {

                            foreach (var Balance in BalanceData)
                            {
                                 BalanceData = TotData.Where(m => m.CostCenter.StartsWith(Acc.CostNumber));
                                CreditBalance += Balance.CreditBalance;
                                DebitBalance += Balance.DebitBalance;
                            }
                        }
                      
                        foreach (var D in MonthlyTrans)
                        {
                            switch (D.Month)
                            {
                                case 1:
                                    January = January + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 2:
                                    February = February + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 3:
                                    March = March + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 4:
                                    April = April + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 5:
                                    May = May + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 6:
                                    June = June + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 7:
                                    July = July + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 8:
                                    August = August + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 9:
                                    September = September + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 10:
                                    October = October + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 11:
                                    November = November + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 12:
                                    December = December + (D.DebitTransAction - D.CreditTransAction);
                                    break;

                            }

                        }



                        TotalDebit = DebitTransAction + DebitBalance;
                        TOTCredit = CreditTransAction + CreditBalance;

                        if (TotalDebit > TOTCredit)
                        {
                            NetDebit = TotalDebit - TOTCredit;
                            NetTot = NetDebit;
                        }
                        if (TotalDebit < TOTCredit)
                        {
                            NetCredit = TOTCredit - TotalDebit;
                            NetTot = NetCredit;


                        }
                        NetTot = TotalDebit - TOTCredit;
                        OpenBalance = DebitBalance - CreditBalance;

                        TrialBalanceVM trialBalance = new TrialBalanceVM();
                        trialBalance.AccountNumber = Acc.CostNumber;
                        trialBalance.Name = Acc.ArabicName;

                        trialBalance.CreditBalance = CreditBalance;
                        trialBalance.CreditTransAction = CreditTransAction;
                        trialBalance.DebitBalance = DebitBalance;
                        trialBalance.DebitTransAction = DebitTransAction;
                        trialBalance.NetCredit = NetCredit;
                        trialBalance.NetDebit = NetDebit;

                        trialBalance.January = January;
                        trialBalance.February = February;
                        trialBalance.March = March;
                        trialBalance.April = April;
                        trialBalance.May = May;
                        trialBalance.June = June;
                        trialBalance.July = July;
                        trialBalance.August = August;
                        trialBalance.September = September;
                        trialBalance.October = October;
                        trialBalance.November = November;
                        trialBalance.December = December;

                        trialBalance.OpenBalance = OpenBalance;
                        trialBalance.NetTot = NetTot;
                        BudgetData = BudgetData.Where(m => m.CostCenterNumber != null).ToList();
                        var CurrBudget = BudgetData.Where(m => m.CostCenterNumber.StartsWith(Acc.CostNumber) && m.BudgetType == 2);
                        if (CurrBudget.Count() > 0)
                        {
                            foreach (var D in CurrBudget)
                            {

                                Expected1 = Expected1 + D.January;

                                Expected2 = Expected2 + D.February;

                                Expected3 = Expected3 + D.March;

                                Expected4 = Expected4 + D.April;

                                Expected5 = Expected5 + D.May;

                                Expected6 = Expected6 + D.June;

                                Expected7 = Expected7 + D.July;

                                Expected8 = Expected8 + D.August;

                                Expected9 = Expected9 + D.September;

                                Expected10 = Expected10 + D.October;


                                Expected11 = Expected11 + D.November;

                                Expected12 = Expected12 + D.December;


                            }


                        }

                        MonthTransaction1 = January;
                        ChangeVaule1 = January - Expected1;
                        ChangePrec1 = 0;
                        if (Expected1 != 0)
                        {
                            ChangePrec1 = 0;// Math.Round((ChangeVaule1 / MonthTransaction1) * 100, 3);
                        }

                        MonthTransaction2 = February;
                        ChangeVaule2 = February - Expected2;
                        ChangePrec2 = 0;
                        if (Expected2 != 0)
                        {
                            ChangePrec2 = 0;// Math.Round((ChangeVaule2 / MonthTransaction2) * 100, 3);
                        }


                        MonthTransaction3 = March;
                        ChangeVaule3 = March - Expected3;
                        ChangePrec3 = 0;
                        if (Expected3 != 0)
                        {
                            ChangePrec3 = 0;//  Math.Round((ChangeVaule3 / MonthTransaction3) * 100, 3);
                        }

                        MonthTransaction4 = April;
                        ChangeVaule4 = April - Expected4;
                        ChangePrec4 = 0;
                        if (Expected4 != 0)
                        {
                            ChangePrec4 = 0;// Math.Round((ChangeVaule4 / MonthTransaction4) * 100, 3);
                        }

                        MonthTransaction5 = May;
                        ChangeVaule5 = May - Expected5;
                        ChangePrec5 = 0;
                        if (Expected5 != 0)
                        {
                            ChangePrec5 = 0;// Math.Round((ChangeVaule5 / MonthTransaction5) * 100, 3);
                        }

                        MonthTransaction6 = June;
                        ChangeVaule6 = June - Expected6;
                        ChangePrec6 = 0;
                        if (Expected6 != 0)
                        {
                            ChangePrec6 = 0;//  Math.Round((ChangeVaule6 / MonthTransaction6) * 100, 3);
                        }

                        MonthTransaction7 = July;
                        ChangeVaule7 = July - Expected7;
                        ChangePrec7 = 0;
                        if (Expected7 != 0)
                        {
                            ChangePrec7 = 0;//  Math.Round((ChangeVaule7 / MonthTransaction7) * 100, 3);
                        }



                        MonthTransaction8 = August;
                        ChangeVaule8 = August - Expected8;
                        ChangePrec8 = 0;
                        if (Expected8 != 0)
                        {
                            ChangePrec8 = 0;//  Math.Round((ChangeVaule8 / MonthTransaction8) * 100, 3);
                        }


                        MonthTransaction9 = September;
                        ChangeVaule9 = September - Expected9;
                        ChangePrec9 = 0;
                        if (Expected9 != 0)
                        {
                            ChangePrec9 = 0;//  Math.Round((ChangeVaule9 / MonthTransaction9) * 100, 3);
                        }


                        MonthTransaction10 = October;
                        ChangeVaule10 = October - Expected10;
                        ChangePrec10 = 0;
                        if (Expected10 != 0)
                        {
                            ChangePrec10 = 0;//  Math.Round((ChangeVaule10 / MonthTransaction10) * 100, 3);
                        }


                        MonthTransaction11 = November;
                        ChangeVaule11 = November - Expected11;
                        ChangePrec11 = 0;
                        if (Expected11 != 0)
                        {
                            ChangePrec11 = 0;// Math.Round((ChangeVaule11 / MonthTransaction11) * 100, 3);
                        }


                        MonthTransaction12 = December;
                        ChangeVaule12 = December - Expected12;
                        ChangePrec12 = 0;
                        if (Expected12 != 0)
                        {
                            ChangePrec12 = 0;//  Math.Round((ChangeVaule12 / MonthTransaction12) * 100, 3);
                        }


                        trialBalance.Expected1 = Expected1;
                        trialBalance.MonthTransaction1 = MonthTransaction1;
                        trialBalance.ChangeVaule1 = ChangeVaule1;
                        trialBalance.ChangePrec1 = ChangePrec1;

                        trialBalance.Expected2 = Expected2;
                        trialBalance.MonthTransaction2 = MonthTransaction2;
                        trialBalance.ChangeVaule2 = ChangeVaule2;
                        trialBalance.ChangePrec2 = ChangePrec2;


                        trialBalance.Expected3 = Expected3;
                        trialBalance.MonthTransaction3 = MonthTransaction3;
                        trialBalance.ChangeVaule3 = ChangeVaule3;
                        trialBalance.ChangePrec3 = ChangePrec3;


                        trialBalance.Expected4 = Expected4;
                        trialBalance.MonthTransaction4 = MonthTransaction4;
                        trialBalance.ChangeVaule4 = ChangeVaule4;
                        trialBalance.ChangePrec4 = ChangePrec4;


                        trialBalance.Expected5 = Expected5;
                        trialBalance.MonthTransaction5 = MonthTransaction5;
                        trialBalance.ChangeVaule5 = ChangeVaule5;
                        trialBalance.ChangePrec5 = ChangePrec5;


                        trialBalance.Expected6 = Expected6;
                        trialBalance.MonthTransaction6 = MonthTransaction6;
                        trialBalance.ChangeVaule6 = ChangeVaule6;
                        trialBalance.ChangePrec6 = ChangePrec6;


                        trialBalance.Expected7 = Expected7;
                        trialBalance.MonthTransaction7 = MonthTransaction7;
                        trialBalance.ChangeVaule7 = ChangeVaule7;
                        trialBalance.ChangePrec7 = ChangePrec7;


                        trialBalance.Expected8 = Expected8;
                        trialBalance.MonthTransaction8 = MonthTransaction8;
                        trialBalance.ChangeVaule8 = ChangeVaule8;
                        trialBalance.ChangePrec8 = ChangePrec8;


                        trialBalance.Expected9 = Expected9;
                        trialBalance.MonthTransaction9 = MonthTransaction9;
                        trialBalance.ChangeVaule9 = ChangeVaule9;
                        trialBalance.ChangePrec9 = ChangePrec9;


                        trialBalance.Expected10 = Expected10;
                        trialBalance.MonthTransaction10 = MonthTransaction10;
                        trialBalance.ChangeVaule10 = ChangeVaule10;
                        trialBalance.ChangePrec10 = ChangePrec10;


                        trialBalance.Expected11 = Expected11;
                        trialBalance.MonthTransaction11 = MonthTransaction11;
                        trialBalance.ChangeVaule11 = ChangeVaule11;
                        trialBalance.ChangePrec11 = ChangePrec11;


                        trialBalance.Expected12 = Expected12;
                        trialBalance.MonthTransaction12 = MonthTransaction12;
                        trialBalance.ChangeVaule12 = ChangeVaule12;
                        trialBalance.ChangePrec12 = ChangePrec12;


                        trialBalance.Expected1 = Expected1;
                        trialBalance.MonthTransaction1 = MonthTransaction1;
                        trialBalance.ChangeVaule1 = ChangeVaule1;
                        trialBalance.ChangePrec1 = ChangePrec1;



                        trialBalance.Expected1 = Expected1;
                        trialBalance.MonthTransaction1 = MonthTransaction1;
                        trialBalance.ChangeVaule1 = ChangeVaule1;
                        trialBalance.ChangePrec1 = ChangePrec1;


                        trialBalance.Expected1 = Expected1;
                        trialBalance.MonthTransaction1 = MonthTransaction1;
                        trialBalance.ChangeVaule1 = ChangeVaule1;
                        trialBalance.ChangePrec1 = ChangePrec1;







                        if ((AccountData.FirstOrDefault(m => m.CostFather == Acc.CostNumber) != null) && (Acc.CostLevel < Obj.AccountLevelDropVMID))
                        {
                            trialBalance.IsMainAccount = 1;
                            trialBalance.MainAccount = "{*}";



                            trialBalance.nCreditBalance = 0;
                            trialBalance.nCreditTransAction = 0;
                            trialBalance.nDebitBalance = 0;
                            trialBalance.nDebitTransAction = 0;
                            trialBalance.nNetCredit = 0;
                            trialBalance.nNetDebit = 0;

                        }
                        else
                        {
                            trialBalance.IsMainAccount = 0;
                            trialBalance.MainAccount = "";
                            trialBalance.nCreditBalance = CreditBalance;
                            trialBalance.nCreditTransAction = CreditTransAction;
                            trialBalance.nDebitBalance = DebitBalance;
                            trialBalance.nDebitTransAction = DebitTransAction;
                            trialBalance.nNetCredit = NetCredit;
                            trialBalance.nNetDebit = NetDebit;

                            trialBalance.nJanuary = January;
                            trialBalance.nFebruary = February;
                            trialBalance.nMarch = March;
                            trialBalance.nApril = April;
                            trialBalance.nMay = May;
                            trialBalance.nJune = June;
                            trialBalance.nJuly = July;
                            trialBalance.nAugust = August;
                            trialBalance.nSeptember = September;
                            trialBalance.nOctober = October;
                            trialBalance.nNovember = November;
                            trialBalance.nDecember = December;

                            trialBalance.nNetTot = NetTot;

                            trialBalance.nOpenBalance = OpenBalance;

                            trialBalance.nExpected1 = Expected1;
                            trialBalance.nMonthTransaction1 = MonthTransaction1;
                            trialBalance.nChangeVaule1 = ChangeVaule1;
                            trialBalance.nChangePrec1 = ChangePrec1;

                            trialBalance.nExpected2 = Expected2;
                            trialBalance.nMonthTransaction2 = MonthTransaction2;
                            trialBalance.nChangeVaule2 = ChangeVaule2;
                            trialBalance.nChangePrec2 = ChangePrec2;


                            trialBalance.nExpected3 = Expected3;
                            trialBalance.nMonthTransaction3 = MonthTransaction3;
                            trialBalance.nChangeVaule3 = ChangeVaule3;
                            trialBalance.nChangePrec3 = ChangePrec3;


                            trialBalance.nExpected4 = Expected4;
                            trialBalance.nMonthTransaction4 = MonthTransaction4;
                            trialBalance.nChangeVaule4 = ChangeVaule4;
                            trialBalance.nChangePrec4 = ChangePrec4;


                            trialBalance.nExpected5 = Expected5;
                            trialBalance.nMonthTransaction5 = MonthTransaction5;
                            trialBalance.nChangeVaule5 = ChangeVaule5;
                            trialBalance.nChangePrec5 = ChangePrec5;


                            trialBalance.nExpected6 = Expected6;
                            trialBalance.nMonthTransaction6 = MonthTransaction6;
                            trialBalance.nChangeVaule6 = ChangeVaule6;
                            trialBalance.nChangePrec6 = ChangePrec6;


                            trialBalance.nExpected7 = Expected7;
                            trialBalance.nMonthTransaction7 = MonthTransaction7;
                            trialBalance.nChangeVaule7 = ChangeVaule7;
                            trialBalance.nChangePrec7 = ChangePrec7;


                            trialBalance.nExpected8 = Expected8;
                            trialBalance.nMonthTransaction8 = MonthTransaction8;
                            trialBalance.nChangeVaule8 = ChangeVaule8;
                            trialBalance.nChangePrec8 = ChangePrec8;


                            trialBalance.nExpected9 = Expected9;
                            trialBalance.nMonthTransaction9 = MonthTransaction9;
                            trialBalance.nChangeVaule9 = ChangeVaule9;
                            trialBalance.nChangePrec9 = ChangePrec9;


                            trialBalance.nExpected10 = Expected10;
                            trialBalance.nMonthTransaction10 = MonthTransaction10;
                            trialBalance.nChangeVaule10 = ChangeVaule10;
                            trialBalance.nChangePrec10 = ChangePrec10;


                            trialBalance.nExpected11 = Expected11;
                            trialBalance.nMonthTransaction11 = MonthTransaction11;
                            trialBalance.nChangeVaule11 = ChangeVaule11;
                            trialBalance.nChangePrec11 = ChangePrec11;


                            trialBalance.nExpected12 = Expected12;
                            trialBalance.nMonthTransaction12 = MonthTransaction12;
                            trialBalance.nChangeVaule12 = ChangeVaule12;
                            trialBalance.nChangePrec12 = ChangePrec12;


                            trialBalance.nExpected1 = Expected1;
                            trialBalance.nMonthTransaction1 = MonthTransaction1;
                            trialBalance.nChangeVaule1 = ChangeVaule1;
                            trialBalance.nChangePrec1 = ChangePrec1;



                            trialBalance.nExpected1 = Expected1;
                            trialBalance.nMonthTransaction1 = MonthTransaction1;
                            trialBalance.nChangeVaule1 = ChangeVaule1;
                            trialBalance.nChangePrec1 = ChangePrec1;


                            trialBalance.nExpected1 = Expected1;
                            trialBalance.nMonthTransaction1 = MonthTransaction1;
                            trialBalance.nChangeVaule1 = ChangeVaule1;
                            trialBalance.nChangePrec1 = ChangePrec1;


                        }
                        if (!Obj.ShowEstimatedZero)
                        {
                            if ((trialBalance.Expected1 + trialBalance.Expected2 + trialBalance.Expected3 + trialBalance.Expected4 + trialBalance.Expected5 + trialBalance.Expected5 +
                                trialBalance.Expected7 + trialBalance.Expected8 + trialBalance.Expected9 + trialBalance.Expected10 + trialBalance.Expected11 + trialBalance.Expected12) != 0)
                            {
                                TrialBalanceVMList.Add(trialBalance);
                            }
                        }
                        else
                        {
                            //if ((trialBalance.NetCredit != 0) || (trialBalance.NetDebit != 0) || (TotalDebit != 0) || (NetCredit != 0))
                            //{
                            TrialBalanceVMList.Add(trialBalance);
                            //}


                        }




                    }

                }
                else if ((Obj.ShowOnlyaccountswithcostcenter) && !String.IsNullOrEmpty(Obj.AccNo))
                {
                    var AccountByCost = _unitOfWork.NativeSql.GetChartOfAccountByCostCenterID(UserInfo.fCompanyId, Obj.AccNo, 3);
                    TranData = _unitOfWork.NativeSql.GetTransactionForTrial(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate, Obj.ByCostCenter, Obj.CostCenterNumber, Obj.Partofthenumber, Obj.CostSearchType);
                    MonthlyData = _unitOfWork.NativeSql.GetTransactionForTrialYearly(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate, Obj.ByCostCenter, Obj.CostCenterNumber, Obj.Partofthenumber, Obj.CostSearchType);
                    BudgetData = _unitOfWork.EstimatedBudget.GetAllEstimatedBudgets(UserInfo.fCompanyId, CurrYear);

                    foreach (var Acc in AccountByCost)//Each Main Account
                    {
                        TotalDebit = 0;
                        TOTCredit = 0;
                        NetCredit = 0;
                        NetDebit = 0;
                        CreditBalance = 0;
                        DebitBalance = 0;
                        CreditTransAction = 0;
                        DebitTransAction = 0;

                        January = 0;
                        February = 0;
                        March = 0;
                        April = 0;
                        May = 0;
                        June = 0;
                        July = 0;
                        August = 0;
                        September = 0;
                        October = 0;
                        November = 0;
                        December = 0;

                        NetTot = 0;

                        MonthTransaction1 = 0;
                        Expected1 = 0;
                        ChangeVaule1 = 0;
                        ChangePrec1 = 0;

                        MonthTransaction2 = 0;
                        Expected2 = 0;
                        ChangeVaule2 = 0;
                        ChangePrec2 = 0;


                        MonthTransaction3 = 0;
                        Expected3 = 0;
                        ChangeVaule3 = 0;
                        ChangePrec3 = 0;


                        MonthTransaction4 = 0;
                        Expected4 = 0;
                        ChangeVaule4 = 0;
                        ChangePrec4 = 0;


                        MonthTransaction5 = 0;
                        Expected5 = 0;
                        ChangeVaule5 = 0;
                        ChangePrec5 = 0;

                        MonthTransaction6 = 0;
                        Expected6 = 0;
                        ChangeVaule6 = 0;
                        ChangePrec6 = 0;


                        MonthTransaction7 = 0;
                        Expected7 = 0;
                        ChangeVaule7 = 0;
                        ChangePrec7 = 0;


                        MonthTransaction8 = 0;
                        Expected8 = 0;
                        ChangeVaule8 = 0;
                        ChangePrec8 = 0;


                        MonthTransaction9 = 0;
                        Expected9 = 0;
                        ChangeVaule9 = 0;
                        ChangePrec9 = 0;


                        MonthTransaction10 = 0;
                        Expected10 = 0;
                        ChangeVaule10 = 0;
                        ChangePrec10 = 0;



                        MonthTransaction10 = 0;
                        Expected10 = 0;
                        ChangeVaule10 = 0;
                        ChangePrec10 = 0;


                        MonthTransaction11 = 0;
                        Expected11 = 0;
                        ChangeVaule11 = 0;
                        ChangePrec11 = 0;


                        MonthTransaction12 = 0;
                        Expected12 = 0;
                        ChangeVaule12 = 0;
                        ChangePrec12 = 0;

                        //  var MainAccount = AccountData.FirstOrDefault(m => m.AccountNumber == Acc.AccountNumber);
                        //   var MainChild = AccountData.Where(m => m.AccountFather == Acc.AccountNumber).OrderBy(m => m.AccountNumber).OrderBy(m => m.AccountLevel).ToList();

                        var TransActionData = TranData.Where(m => m.AccountNumber == Acc.AccountNumber);
                        var BalanceData = TotData.Where(m => m.AccountNumber == Acc.AccountNumber);
                        var MonthlyTrans = MonthlyData.Where(m => m.AccountNumber == Acc.AccountNumber);
                        foreach (var D in TransActionData)
                        {
                            CreditTransAction += D.CreditTransAction;
                            DebitTransAction += D.DebitTransAction;
                        }
                        foreach (var Balance in BalanceData)
                        {
                            CreditBalance += Balance.CreditBalance;
                            DebitBalance += Balance.DebitBalance;
                        }
                        foreach (var D in MonthlyTrans)
                        {
                            switch (D.Month)
                            {
                                case 1:
                                    January = January + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 2:
                                    February = February + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 3:
                                    March = March + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 4:
                                    April = April + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 5:
                                    May = May + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 6:
                                    June = June + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 7:
                                    July = July + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 8:
                                    August = August + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 9:
                                    September = September + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 10:
                                    October = October + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 11:
                                    November = November + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 12:
                                    December = December + (D.DebitTransAction - D.CreditTransAction);
                                    break;

                            }

                        }



                        TotalDebit = DebitTransAction + DebitBalance;
                        TOTCredit = CreditTransAction + CreditBalance;

                        if (TotalDebit > TOTCredit)
                        {
                            NetDebit = TotalDebit - TOTCredit;
                            NetTot = NetDebit;
                        }
                        if (TotalDebit < TOTCredit)
                        {
                            NetCredit = TOTCredit - TotalDebit;
                            NetTot = NetCredit;


                        }
                        NetTot = TotalDebit - TOTCredit;
                        OpenBalance = DebitBalance - CreditBalance;

                        TrialBalanceVM trialBalance = new TrialBalanceVM();
                        trialBalance.AccountNumber = Acc.AccountNumber;
                        trialBalance.Name = Acc.ArabicName;

                        trialBalance.CreditBalance = CreditBalance;
                        trialBalance.CreditTransAction = CreditTransAction;
                        trialBalance.DebitBalance = DebitBalance;
                        trialBalance.DebitTransAction = DebitTransAction;
                        trialBalance.NetCredit = NetCredit;
                        trialBalance.NetDebit = NetDebit;

                        trialBalance.January = January;
                        trialBalance.February = February;
                        trialBalance.March = March;
                        trialBalance.April = April;
                        trialBalance.May = May;
                        trialBalance.June = June;
                        trialBalance.July = July;
                        trialBalance.August = August;
                        trialBalance.September = September;
                        trialBalance.October = October;
                        trialBalance.November = November;
                        trialBalance.December = December;

                        trialBalance.OpenBalance = OpenBalance;
                        trialBalance.NetTot = NetTot;
                        BudgetData = BudgetData.Where(m => m.CostCenterNumber != null).ToList();
                        var CurrBudget = BudgetData.Where(m => m.AccountNumber == Acc.AccountNumber && m.BudgetType == 3);
                        if (CurrBudget.Count() > 0)
                        {
                            foreach (var D in CurrBudget)
                            {

                                Expected1 = Expected1 + D.January;

                                Expected2 = Expected2 + D.February;

                                Expected3 = Expected3 + D.March;

                                Expected4 = Expected4 + D.April;

                                Expected5 = Expected5 + D.May;

                                Expected6 = Expected6 + D.June;

                                Expected7 = Expected7 + D.July;

                                Expected8 = Expected8 + D.August;

                                Expected9 = Expected9 + D.September;

                                Expected10 = Expected10 + D.October;


                                Expected11 = Expected11 + D.November;

                                Expected12 = Expected12 + D.December;


                            }


                        }

                        MonthTransaction1 = January;
                        ChangeVaule1 = January - Expected1;
                        ChangePrec1 = 0;
                        if (Expected1 != 0)
                        {
                            ChangePrec1 = 0;// Math.Round((ChangeVaule1 / MonthTransaction1) * 100, 3);
                        }

                        MonthTransaction2 = February;
                        ChangeVaule2 = February - Expected2;
                        ChangePrec2 = 0;
                        if (Expected2 != 0)
                        {
                            ChangePrec2 = 0;// Math.Round((ChangeVaule2 / MonthTransaction2) * 100, 3);
                        }


                        MonthTransaction3 = March;
                        ChangeVaule3 = March - Expected3;
                        ChangePrec3 = 0;
                        if (Expected3 != 0)
                        {
                            ChangePrec3 = 0;//  Math.Round((ChangeVaule3 / MonthTransaction3) * 100, 3);
                        }

                        MonthTransaction4 = April;
                        ChangeVaule4 = April - Expected4;
                        ChangePrec4 = 0;
                        if (Expected4 != 0)
                        {
                            ChangePrec4 = 0;// Math.Round((ChangeVaule4 / MonthTransaction4) * 100, 3);
                        }

                        MonthTransaction5 = May;
                        ChangeVaule5 = May - Expected5;
                        ChangePrec5 = 0;
                        if (Expected5 != 0)
                        {
                            ChangePrec5 = 0;// Math.Round((ChangeVaule5 / MonthTransaction5) * 100, 3);
                        }

                        MonthTransaction6 = June;
                        ChangeVaule6 = June - Expected6;
                        ChangePrec6 = 0;
                        if (Expected6 != 0)
                        {
                            ChangePrec6 = 0;//  Math.Round((ChangeVaule6 / MonthTransaction6) * 100, 3);
                        }

                        MonthTransaction7 = July;
                        ChangeVaule7 = July - Expected7;
                        ChangePrec7 = 0;
                        if (Expected7 != 0)
                        {
                            ChangePrec7 = 0;//  Math.Round((ChangeVaule7 / MonthTransaction7) * 100, 3);
                        }



                        MonthTransaction8 = August;
                        ChangeVaule8 = August - Expected8;
                        ChangePrec8 = 0;
                        if (Expected8 != 0)
                        {
                            ChangePrec8 = 0;//  Math.Round((ChangeVaule8 / MonthTransaction8) * 100, 3);
                        }


                        MonthTransaction9 = September;
                        ChangeVaule9 = September - Expected9;
                        ChangePrec9 = 0;
                        if (Expected9 != 0)
                        {
                            ChangePrec9 = 0;//  Math.Round((ChangeVaule9 / MonthTransaction9) * 100, 3);
                        }


                        MonthTransaction10 = October;
                        ChangeVaule10 = October - Expected10;
                        ChangePrec10 = 0;
                        if (Expected10 != 0)
                        {
                            ChangePrec10 = 0;//  Math.Round((ChangeVaule10 / MonthTransaction10) * 100, 3);
                        }


                        MonthTransaction11 = November;
                        ChangeVaule11 = November - Expected11;
                        ChangePrec11 = 0;
                        if (Expected11 != 0)
                        {
                            ChangePrec11 = 0;// Math.Round((ChangeVaule11 / MonthTransaction11) * 100, 3);
                        }


                        MonthTransaction12 = December;
                        ChangeVaule12 = December - Expected12;
                        ChangePrec12 = 0;
                        if (Expected12 != 0)
                        {
                            ChangePrec12 = 0;//  Math.Round((ChangeVaule12 / MonthTransaction12) * 100, 3);
                        }


                        trialBalance.Expected1 = Expected1;
                        trialBalance.MonthTransaction1 = MonthTransaction1;
                        trialBalance.ChangeVaule1 = ChangeVaule1;
                        trialBalance.ChangePrec1 = ChangePrec1;

                        trialBalance.Expected2 = Expected2;
                        trialBalance.MonthTransaction2 = MonthTransaction2;
                        trialBalance.ChangeVaule2 = ChangeVaule2;
                        trialBalance.ChangePrec2 = ChangePrec2;


                        trialBalance.Expected3 = Expected3;
                        trialBalance.MonthTransaction3 = MonthTransaction3;
                        trialBalance.ChangeVaule3 = ChangeVaule3;
                        trialBalance.ChangePrec3 = ChangePrec3;


                        trialBalance.Expected4 = Expected4;
                        trialBalance.MonthTransaction4 = MonthTransaction4;
                        trialBalance.ChangeVaule4 = ChangeVaule4;
                        trialBalance.ChangePrec4 = ChangePrec4;


                        trialBalance.Expected5 = Expected5;
                        trialBalance.MonthTransaction5 = MonthTransaction5;
                        trialBalance.ChangeVaule5 = ChangeVaule5;
                        trialBalance.ChangePrec5 = ChangePrec5;


                        trialBalance.Expected6 = Expected6;
                        trialBalance.MonthTransaction6 = MonthTransaction6;
                        trialBalance.ChangeVaule6 = ChangeVaule6;
                        trialBalance.ChangePrec6 = ChangePrec6;


                        trialBalance.Expected7 = Expected7;
                        trialBalance.MonthTransaction7 = MonthTransaction7;
                        trialBalance.ChangeVaule7 = ChangeVaule7;
                        trialBalance.ChangePrec7 = ChangePrec7;


                        trialBalance.Expected8 = Expected8;
                        trialBalance.MonthTransaction8 = MonthTransaction8;
                        trialBalance.ChangeVaule8 = ChangeVaule8;
                        trialBalance.ChangePrec8 = ChangePrec8;


                        trialBalance.Expected9 = Expected9;
                        trialBalance.MonthTransaction9 = MonthTransaction9;
                        trialBalance.ChangeVaule9 = ChangeVaule9;
                        trialBalance.ChangePrec9 = ChangePrec9;


                        trialBalance.Expected10 = Expected10;
                        trialBalance.MonthTransaction10 = MonthTransaction10;
                        trialBalance.ChangeVaule10 = ChangeVaule10;
                        trialBalance.ChangePrec10 = ChangePrec10;


                        trialBalance.Expected11 = Expected11;
                        trialBalance.MonthTransaction11 = MonthTransaction11;
                        trialBalance.ChangeVaule11 = ChangeVaule11;
                        trialBalance.ChangePrec11 = ChangePrec11;


                        trialBalance.Expected12 = Expected12;
                        trialBalance.MonthTransaction12 = MonthTransaction12;
                        trialBalance.ChangeVaule12 = ChangeVaule12;
                        trialBalance.ChangePrec12 = ChangePrec12;


                        trialBalance.Expected1 = Expected1;
                        trialBalance.MonthTransaction1 = MonthTransaction1;
                        trialBalance.ChangeVaule1 = ChangeVaule1;
                        trialBalance.ChangePrec1 = ChangePrec1;



                        trialBalance.Expected1 = Expected1;
                        trialBalance.MonthTransaction1 = MonthTransaction1;
                        trialBalance.ChangeVaule1 = ChangeVaule1;
                        trialBalance.ChangePrec1 = ChangePrec1;


                        trialBalance.Expected1 = Expected1;
                        trialBalance.MonthTransaction1 = MonthTransaction1;
                        trialBalance.ChangeVaule1 = ChangeVaule1;
                        trialBalance.ChangePrec1 = ChangePrec1;







                        if (false/*(AccountData.FirstOrDefault(m => m.CostFather == Acc.CostNumber) != null) && (Acc.CostLevel < Obj.AccountLevelDropVMID)*/)
                        {
                            trialBalance.IsMainAccount = 1;
                            trialBalance.MainAccount = "{*}";



                            trialBalance.nCreditBalance = 0;
                            trialBalance.nCreditTransAction = 0;
                            trialBalance.nDebitBalance = 0;
                            trialBalance.nDebitTransAction = 0;
                            trialBalance.nNetCredit = 0;
                            trialBalance.nNetDebit = 0;

                        }
                        else
                        {
                            trialBalance.IsMainAccount = 0;
                            trialBalance.MainAccount = "";
                            trialBalance.nCreditBalance = CreditBalance;
                            trialBalance.nCreditTransAction = CreditTransAction;
                            trialBalance.nDebitBalance = DebitBalance;
                            trialBalance.nDebitTransAction = DebitTransAction;
                            trialBalance.nNetCredit = NetCredit;
                            trialBalance.nNetDebit = NetDebit;

                            trialBalance.nJanuary = January;
                            trialBalance.nFebruary = February;
                            trialBalance.nMarch = March;
                            trialBalance.nApril = April;
                            trialBalance.nMay = May;
                            trialBalance.nJune = June;
                            trialBalance.nJuly = July;
                            trialBalance.nAugust = August;
                            trialBalance.nSeptember = September;
                            trialBalance.nOctober = October;
                            trialBalance.nNovember = November;
                            trialBalance.nDecember = December;

                            trialBalance.nNetTot = NetTot;

                            trialBalance.nOpenBalance = OpenBalance;

                            trialBalance.nExpected1 = Expected1;
                            trialBalance.nMonthTransaction1 = MonthTransaction1;
                            trialBalance.nChangeVaule1 = ChangeVaule1;
                            trialBalance.nChangePrec1 = ChangePrec1;

                            trialBalance.nExpected2 = Expected2;
                            trialBalance.nMonthTransaction2 = MonthTransaction2;
                            trialBalance.nChangeVaule2 = ChangeVaule2;
                            trialBalance.nChangePrec2 = ChangePrec2;


                            trialBalance.nExpected3 = Expected3;
                            trialBalance.nMonthTransaction3 = MonthTransaction3;
                            trialBalance.nChangeVaule3 = ChangeVaule3;
                            trialBalance.nChangePrec3 = ChangePrec3;


                            trialBalance.nExpected4 = Expected4;
                            trialBalance.nMonthTransaction4 = MonthTransaction4;
                            trialBalance.nChangeVaule4 = ChangeVaule4;
                            trialBalance.nChangePrec4 = ChangePrec4;


                            trialBalance.nExpected5 = Expected5;
                            trialBalance.nMonthTransaction5 = MonthTransaction5;
                            trialBalance.nChangeVaule5 = ChangeVaule5;
                            trialBalance.nChangePrec5 = ChangePrec5;


                            trialBalance.nExpected6 = Expected6;
                            trialBalance.nMonthTransaction6 = MonthTransaction6;
                            trialBalance.nChangeVaule6 = ChangeVaule6;
                            trialBalance.nChangePrec6 = ChangePrec6;


                            trialBalance.nExpected7 = Expected7;
                            trialBalance.nMonthTransaction7 = MonthTransaction7;
                            trialBalance.nChangeVaule7 = ChangeVaule7;
                            trialBalance.nChangePrec7 = ChangePrec7;


                            trialBalance.nExpected8 = Expected8;
                            trialBalance.nMonthTransaction8 = MonthTransaction8;
                            trialBalance.nChangeVaule8 = ChangeVaule8;
                            trialBalance.nChangePrec8 = ChangePrec8;


                            trialBalance.nExpected9 = Expected9;
                            trialBalance.nMonthTransaction9 = MonthTransaction9;
                            trialBalance.nChangeVaule9 = ChangeVaule9;
                            trialBalance.nChangePrec9 = ChangePrec9;


                            trialBalance.nExpected10 = Expected10;
                            trialBalance.nMonthTransaction10 = MonthTransaction10;
                            trialBalance.nChangeVaule10 = ChangeVaule10;
                            trialBalance.nChangePrec10 = ChangePrec10;


                            trialBalance.nExpected11 = Expected11;
                            trialBalance.nMonthTransaction11 = MonthTransaction11;
                            trialBalance.nChangeVaule11 = ChangeVaule11;
                            trialBalance.nChangePrec11 = ChangePrec11;


                            trialBalance.nExpected12 = Expected12;
                            trialBalance.nMonthTransaction12 = MonthTransaction12;
                            trialBalance.nChangeVaule12 = ChangeVaule12;
                            trialBalance.nChangePrec12 = ChangePrec12;


                            trialBalance.nExpected1 = Expected1;
                            trialBalance.nMonthTransaction1 = MonthTransaction1;
                            trialBalance.nChangeVaule1 = ChangeVaule1;
                            trialBalance.nChangePrec1 = ChangePrec1;



                            trialBalance.nExpected1 = Expected1;
                            trialBalance.nMonthTransaction1 = MonthTransaction1;
                            trialBalance.nChangeVaule1 = ChangeVaule1;
                            trialBalance.nChangePrec1 = ChangePrec1;


                            trialBalance.nExpected1 = Expected1;
                            trialBalance.nMonthTransaction1 = MonthTransaction1;
                            trialBalance.nChangeVaule1 = ChangeVaule1;
                            trialBalance.nChangePrec1 = ChangePrec1;


                        }
                        if (!Obj.ShowEstimatedZero)
                        {
                            if ((trialBalance.Expected1 + trialBalance.Expected2 + trialBalance.Expected3 + trialBalance.Expected4 + trialBalance.Expected5 + trialBalance.Expected5 +
                                trialBalance.Expected7 + trialBalance.Expected8 + trialBalance.Expected9 + trialBalance.Expected10 + trialBalance.Expected11 + trialBalance.Expected12) != 0)
                            {
                                TrialBalanceVMList.Add(trialBalance);
                            }
                        }
                        else
                        {
                            //if ((trialBalance.NetCredit != 0) || (trialBalance.NetDebit != 0) || (TotalDebit != 0) || (NetCredit != 0))
                            //{
                            TrialBalanceVMList.Add(trialBalance);
                            //}


                        }




                    }

                }
                    if (!Obj.ShowMainAccountValue)
                {
                    foreach (var trialBalance in TrialBalanceVMList.Where(m => m.IsMainAccount == 1))
                    {
                        if (trialBalance.IsMainAccount == 1)
                        {
                            trialBalance.Expected = 0;
                            trialBalance.MonthTransaction = 0;
                            trialBalance.ChangeVaule = 0;
                            trialBalance.ChangePrec = 0;
                            trialBalance.CreditBalance = 0;
                            trialBalance.CreditTransAction = 0;
                            trialBalance.DebitBalance = 0;
                            trialBalance.DebitTransAction = 0;
                            trialBalance.NetCredit = 0;
                            trialBalance.NetDebit = 0;
                        }

                    }

                }



                return Json(TrialBalanceVMList, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                string err = ex.Message;

                List<TrialBalanceVM> TrialBalanceVMList = new List<TrialBalanceVM>();

                return Json(TrialBalanceVMList, JsonRequestBehavior.AllowGet);

            }


        }
        [HttpPost]
        public JsonResult GetEstimatedBudgetForCostYear(AccountLevelRepVM Obj)
        {
            try
            {

                var userId = User.Identity.GetUserId();
                string UserID = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
                var AccountData = _unitOfWork.NativeSql.GetCostCenterByLevel(UserInfo.fCompanyId);

                int CurrYear = UserInfo.CurrYear;
                Obj.Detail = true;
                var Date = "01/01/" + CurrYear;
                var OpenDate = "31/12/" + (CurrYear - 1).ToString();
                Obj.FromDate = DateTime.Parse("01/01/" + CurrYear);
                Obj.ToDate = DateTime.Parse("31/12/" + CurrYear);



                IEnumerable<TrialBalanceVM> TotData = new List<TrialBalanceVM>();
                var TranData = _unitOfWork.NativeSql.GetTransactionForTrialCost(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                var MonthlyData = _unitOfWork.NativeSql.GetTransactionForCostMonthly(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                var BudgetData = _unitOfWork.EstimatedBudget.GetAllEstimatedBudgets(UserInfo.fCompanyId, CurrYear);

                //  if (DateTime.Parse(Date) != Obj.FromDate)
                //     TotData = _unitOfWork.NativeSql.GetTotCreditDebitForTrial(UserInfo.fCompanyId, DateTime.Parse(Date), Obj.FromDate);


                //----------------Get Open-------------//
                IEnumerable<TrialBalanceVM> TotDataOpen = new List<TrialBalanceVM>();
                TotDataOpen = _unitOfWork.NativeSql.GetTotCreditDebitForTrialOpen(UserInfo.fCompanyId, DateTime.Parse(OpenDate), DateTime.Parse(OpenDate));
                foreach (var D in TotDataOpen)
                {
                    TotData = TotData.Append(D);

                }
                //------------------------------------//


                double TotalDebit = 0;
                double TOTCredit = 0;
                double NetCredit = 0;
                double NetDebit = 0;
                double CreditBalance = 0;
                double DebitBalance = 0;
                double CreditTransAction = 0;
                double DebitTransAction = 0;

                double January = 0;
                double February = 0;
                double March = 0;
                double April = 0;
                double May = 0;
                double June = 0;
                double July = 0;
                double August = 0;
                double September = 0;
                double October = 0;
                double November = 0;
                double December = 0;

                double NetTot = 0;
                double OpenBalance = 0;

                double MonthTransaction1 = 0;
                double Expected1 = 0;
                double ChangeVaule1 = 0;
                double ChangePrec1 = 0;

                double MonthTransaction2 = 0;
                double Expected2 = 0;
                double ChangeVaule2 = 0;
                double ChangePrec2 = 0;


                double MonthTransaction3 = 0;
                double Expected3 = 0;
                double ChangeVaule3 = 0;
                double ChangePrec3 = 0;


                double MonthTransaction4 = 0;
                double Expected4 = 0;
                double ChangeVaule4 = 0;
                double ChangePrec4 = 0;


                double MonthTransaction5 = 0;
                double Expected5 = 0;
                double ChangeVaule5 = 0;
                double ChangePrec5 = 0;

                double MonthTransaction6 = 0;
                double Expected6 = 0;
                double ChangeVaule6 = 0;
                double ChangePrec6 = 0;


                double MonthTransaction7 = 0;
                double Expected7 = 0;
                double ChangeVaule7 = 0;
                double ChangePrec7 = 0;


                double MonthTransaction8 = 0;
                double Expected8 = 0;
                double ChangeVaule8 = 0;
                double ChangePrec8 = 0;


                double MonthTransaction9 = 0;
                double Expected9 = 0;
                double ChangeVaule9 = 0;
                double ChangePrec9 = 0;


                double MonthTransaction10 = 0;
                double Expected10 = 0;
                double ChangeVaule10 = 0;
                double ChangePrec10 = 0;





                double MonthTransaction11 = 0;
                double Expected11 = 0;
                double ChangeVaule11 = 0;
                double ChangePrec11 = 0;


                double MonthTransaction12 = 0;
                double Expected12 = 0;
                double ChangeVaule12 = 0;
                double ChangePrec12 = 0;

                List<TrialBalanceVM> TrialBalanceVMList = new List<TrialBalanceVM>();

                IEnumerable<CostCenter> Accounts = new List<CostCenter>();

                IEnumerable<ChartOfAccount> LessMainAccount = new List<ChartOfAccount>();




                if ((Obj.AccountLevelDropVMID > 0) && (!Obj.ShowOnlyaccountswithcostcenter))
                {

                    Accounts = AccountData.Where(m => m.CostLevel <= Obj.AccountLevelDropVMID).ToList().OrderBy(m => m.CostNumber).ThenBy(m => m.CostLevel).ToList();

                    foreach (var Acc in Accounts)//Each Main Account
                    {
                        TotalDebit = 0;
                        TOTCredit = 0;
                        NetCredit = 0;
                        NetDebit = 0;
                        CreditBalance = 0;
                        DebitBalance = 0;
                        CreditTransAction = 0;
                        DebitTransAction = 0;

                        January = 0;
                        February = 0;
                        March = 0;
                        April = 0;
                        May = 0;
                        June = 0;
                        July = 0;
                        August = 0;
                        September = 0;
                        October = 0;
                        November = 0;
                        December = 0;

                        NetTot = 0;

                        MonthTransaction1 = 0;
                        Expected1 = 0;
                        ChangeVaule1 = 0;
                        ChangePrec1 = 0;

                        MonthTransaction2 = 0;
                        Expected2 = 0;
                        ChangeVaule2 = 0;
                        ChangePrec2 = 0;


                        MonthTransaction3 = 0;
                        Expected3 = 0;
                        ChangeVaule3 = 0;
                        ChangePrec3 = 0;


                        MonthTransaction4 = 0;
                        Expected4 = 0;
                        ChangeVaule4 = 0;
                        ChangePrec4 = 0;


                        MonthTransaction5 = 0;
                        Expected5 = 0;
                        ChangeVaule5 = 0;
                        ChangePrec5 = 0;

                        MonthTransaction6 = 0;
                        Expected6 = 0;
                        ChangeVaule6 = 0;
                        ChangePrec6 = 0;


                        MonthTransaction7 = 0;
                        Expected7 = 0;
                        ChangeVaule7 = 0;
                        ChangePrec7 = 0;


                        MonthTransaction8 = 0;
                        Expected8 = 0;
                        ChangeVaule8 = 0;
                        ChangePrec8 = 0;


                        MonthTransaction9 = 0;
                        Expected9 = 0;
                        ChangeVaule9 = 0;
                        ChangePrec9 = 0;


                        MonthTransaction10 = 0;
                        Expected10 = 0;
                        ChangeVaule10 = 0;
                        ChangePrec10 = 0;



                        MonthTransaction10 = 0;
                        Expected10 = 0;
                        ChangeVaule10 = 0;
                        ChangePrec10 = 0;


                        MonthTransaction11 = 0;
                        Expected11 = 0;
                        ChangeVaule11 = 0;
                        ChangePrec11 = 0;


                        MonthTransaction12 = 0;
                        Expected12 = 0;
                        ChangeVaule12 = 0;
                        ChangePrec12 = 0;


                        var TransActionData = TranData.Where(m => m.CostCenter.StartsWith(Acc.CostNumber));
                       var BalanceData = TotData.Where(m => m.CostCenter != null);
                        var MonthlyTrans = MonthlyData.Where(m => m.CostCenter.StartsWith(Acc.CostNumber));
                        foreach (var D in TransActionData)
                        {
                            CreditTransAction += D.CreditTransAction;
                            DebitTransAction += D.DebitTransAction;
                        }

                        if (BalanceData != null) {

                            foreach (var Balance in BalanceData)
                            {
                                BalanceData = TotData.Where(m => m.CostCenter.StartsWith(Acc.CostNumber));
                                CreditBalance += Balance.CreditBalance;
                                DebitBalance += Balance.DebitBalance;
                            }
                        }
                        
                        foreach (var D in MonthlyTrans)
                        {
                            switch (D.Month)
                            {
                                case 1:
                                    January = January + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 2:
                                    February = February + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 3:
                                    March = March + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 4:
                                    April = April + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 5:
                                    May = May + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 6:
                                    June = June + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 7:
                                    July = July + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 8:
                                    August = August + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 9:
                                    September = September + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 10:
                                    October = October + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 11:
                                    November = November + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 12:
                                    December = December + (D.DebitTransAction - D.CreditTransAction);
                                    break;

                            }

                        }



                        TotalDebit = DebitTransAction + DebitBalance;
                        TOTCredit = CreditTransAction + CreditBalance;

                        if (TotalDebit > TOTCredit)
                        {
                            NetDebit = TotalDebit - TOTCredit;
                            NetTot = NetDebit;
                        }
                        if (TotalDebit < TOTCredit)
                        {
                            NetCredit = TOTCredit - TotalDebit;
                            NetTot = NetCredit;


                        }
                        NetTot = TotalDebit - TOTCredit;
                        OpenBalance = DebitBalance - CreditBalance;

                        TrialBalanceVM trialBalance = new TrialBalanceVM();
                        trialBalance.AccountNumber = Acc.CostNumber;
                        trialBalance.Name = Acc.ArabicName;

                        trialBalance.CreditBalance = CreditBalance;
                        trialBalance.CreditTransAction = CreditTransAction;
                        trialBalance.DebitBalance = DebitBalance;
                        trialBalance.DebitTransAction = DebitTransAction;
                        trialBalance.NetCredit = NetCredit;
                        trialBalance.NetDebit = NetDebit;

                        trialBalance.January = January;
                        trialBalance.February = February;
                        trialBalance.March = March;
                        trialBalance.April = April;
                        trialBalance.May = May;
                        trialBalance.June = June;
                        trialBalance.July = July;
                        trialBalance.August = August;
                        trialBalance.September = September;
                        trialBalance.October = October;
                        trialBalance.November = November;
                        trialBalance.December = December;

                        trialBalance.OpenBalance = OpenBalance;
                        trialBalance.NetTot = NetTot;
                        BudgetData = BudgetData.Where(m => m.CostCenterNumber != null).ToList();
                        var CurrBudget = BudgetData.Where(m => m.CostCenterNumber.StartsWith(Acc.CostNumber) && m.BudgetType == 2);
                        if (CurrBudget.Count() > 0)
                        {
                            foreach (var D in CurrBudget)
                            {

                                Expected1 = Expected1 + D.January;

                                Expected2 = Expected2 + D.February;

                                Expected3 = Expected3 + D.March;

                                Expected4 = Expected4 + D.April;

                                Expected5 = Expected5 + D.May;

                                Expected6 = Expected6 + D.June;

                                Expected7 = Expected7 + D.July;

                                Expected8 = Expected8 + D.August;

                                Expected9 = Expected9 + D.September;

                                Expected10 = Expected10 + D.October;


                                Expected11 = Expected11 + D.November;

                                Expected12 = Expected12 + D.December;


                            }


                        }

                        MonthTransaction1 = January;
                        ChangeVaule1 = January - Expected1;
                        ChangePrec1 = 0;
                        if (Expected1 != 0)
                        {
                            ChangePrec1 = 0;// Math.Round((ChangeVaule1 / MonthTransaction1) * 100, 3);
                        }

                        MonthTransaction2 = February;
                        ChangeVaule2 = February - Expected2;
                        ChangePrec2 = 0;
                        if (Expected2 != 0)
                        {
                            ChangePrec2 = 0;// Math.Round((ChangeVaule2 / MonthTransaction2) * 100, 3);
                        }


                        MonthTransaction3 = March;
                        ChangeVaule3 = March - Expected3;
                        ChangePrec3 = 0;
                        if (Expected3 != 0)
                        {
                            ChangePrec3 = 0;//  Math.Round((ChangeVaule3 / MonthTransaction3) * 100, 3);
                        }

                        MonthTransaction4 = April;
                        ChangeVaule4 = April - Expected4;
                        ChangePrec4 = 0;
                        if (Expected4 != 0)
                        {
                            ChangePrec4 = 0;// Math.Round((ChangeVaule4 / MonthTransaction4) * 100, 3);
                        }

                        MonthTransaction5 = May;
                        ChangeVaule5 = (May) - Expected5;
                        ChangePrec5 = 0;
                        if (Expected5 != 0)
                        {
                            ChangePrec5 = 0;// Math.Round((ChangeVaule5 / MonthTransaction5) * 100, 3);
                        }

                        MonthTransaction6 = (June);
                        ChangeVaule6 = (June) - Expected6;
                        ChangePrec6 = 0;
                        if (Expected6 != 0)
                        {
                            ChangePrec6 = 0;//  Math.Round((ChangeVaule6 / MonthTransaction6) * 100, 3);
                        }

                        MonthTransaction7 = (July);
                        ChangeVaule7 = (July) - Expected7;
                        ChangePrec7 = 0;
                        if (Expected7 != 0)
                        {
                            ChangePrec7 = 0;//  Math.Round((ChangeVaule7 / MonthTransaction7) * 100, 3);
                        }



                        MonthTransaction8 = (August);
                        ChangeVaule8 = (August) - Expected8;
                        ChangePrec8 = 0;
                        if (Expected8 != 0)
                        {
                            ChangePrec8 = 0;//  Math.Round((ChangeVaule8 / MonthTransaction8) * 100, 3);
                        }


                        MonthTransaction9 = (September);
                        ChangeVaule9 = (September) - Expected9;
                        ChangePrec9 = 0;
                        if (Expected9 != 0)
                        {
                            ChangePrec9 = 0;//  Math.Round((ChangeVaule9 / MonthTransaction9) * 100, 3);
                        }


                        MonthTransaction10 = (October);
                        ChangeVaule10 = (October) - Expected10;
                        ChangePrec10 = 0;
                        if (Expected10 != 0)
                        {
                            ChangePrec10 = 0;//  Math.Round((ChangeVaule10 / MonthTransaction10) * 100, 3);
                        }


                        MonthTransaction11 = (November);
                        ChangeVaule11 = (November) - Expected11;
                        ChangePrec11 = 0;
                        if (Expected11 != 0)
                        {
                            ChangePrec11 = 0;// Math.Round((ChangeVaule11 / MonthTransaction11) * 100, 3);
                        }


                        MonthTransaction12 = (December);
                        ChangeVaule12 = (December) - Expected12;
                        ChangePrec12 = 0;
                        if (Expected12 != 0)
                        {
                            ChangePrec12 = 0;//  Math.Round((ChangeVaule12 / MonthTransaction12) * 100, 3);
                        }


                        trialBalance.Expected1 = Expected1;
                        trialBalance.MonthTransaction1 = MonthTransaction1;
                        trialBalance.ChangeVaule1 = ChangeVaule1;
                        trialBalance.ChangePrec1 = ChangePrec1;

                        trialBalance.Expected2 = Expected2;
                        trialBalance.MonthTransaction2 = MonthTransaction2;
                        trialBalance.ChangeVaule2 = ChangeVaule2;
                        trialBalance.ChangePrec2 = ChangePrec2;


                        trialBalance.Expected3 = Expected3;
                        trialBalance.MonthTransaction3 = MonthTransaction3;
                        trialBalance.ChangeVaule3 = ChangeVaule3;
                        trialBalance.ChangePrec3 = ChangePrec3;


                        trialBalance.Expected4 = Expected4;
                        trialBalance.MonthTransaction4 = MonthTransaction4;
                        trialBalance.ChangeVaule4 = ChangeVaule4;
                        trialBalance.ChangePrec4 = ChangePrec4;


                        trialBalance.Expected5 = Expected5;
                        trialBalance.MonthTransaction5 = MonthTransaction5;
                        trialBalance.ChangeVaule5 = ChangeVaule5;
                        trialBalance.ChangePrec5 = ChangePrec5;


                        trialBalance.Expected6 = Expected6;
                        trialBalance.MonthTransaction6 = MonthTransaction6;
                        trialBalance.ChangeVaule6 = ChangeVaule6;
                        trialBalance.ChangePrec6 = ChangePrec6;


                        trialBalance.Expected7 = Expected7;
                        trialBalance.MonthTransaction7 = MonthTransaction7;
                        trialBalance.ChangeVaule7 = ChangeVaule7;
                        trialBalance.ChangePrec7 = ChangePrec7;


                        trialBalance.Expected8 = Expected8;
                        trialBalance.MonthTransaction8 = MonthTransaction8;
                        trialBalance.ChangeVaule8 = ChangeVaule8;
                        trialBalance.ChangePrec8 = ChangePrec8;


                        trialBalance.Expected9 = Expected9;
                        trialBalance.MonthTransaction9 = MonthTransaction9;
                        trialBalance.ChangeVaule9 = ChangeVaule9;
                        trialBalance.ChangePrec9 = ChangePrec9;


                        trialBalance.Expected10 = Expected10;
                        trialBalance.MonthTransaction10 = MonthTransaction10;
                        trialBalance.ChangeVaule10 = ChangeVaule10;
                        trialBalance.ChangePrec10 = ChangePrec10;


                        trialBalance.Expected11 = Expected11;
                        trialBalance.MonthTransaction11 = MonthTransaction11;
                        trialBalance.ChangeVaule11 = ChangeVaule11;
                        trialBalance.ChangePrec11 = ChangePrec11;


                        trialBalance.Expected12 = Expected12;
                        trialBalance.MonthTransaction12 = MonthTransaction12;
                        trialBalance.ChangeVaule12 = ChangeVaule12;
                        trialBalance.ChangePrec12 = ChangePrec12;

 


                       

                        trialBalance.ExpectedAnnually = (trialBalance.Expected1 + trialBalance.Expected2 + trialBalance.Expected3 + trialBalance.Expected4 + trialBalance.Expected5 + trialBalance.Expected5 +
                                trialBalance.Expected7 + trialBalance.Expected8 + trialBalance.Expected9 + trialBalance.Expected10 + trialBalance.Expected11 + trialBalance.Expected12);

                        trialBalance.ActualTotal = Math.Round(NetTot, 4);
                        trialBalance.RemainingExpected = Math.Round((trialBalance.ExpectedAnnually - Math.Abs(NetTot)), 4);
                        trialBalance.RemainingRatio = 0;
                        trialBalance.ExpectedAnnually = Math.Round(trialBalance.ExpectedAnnually, 4);
                        if ((AccountData.FirstOrDefault(m => m.CostFather == Acc.CostNumber) != null) && (Acc.CostLevel < Obj.AccountLevelDropVMID))
                        {
                            trialBalance.IsMainAccount = 1;
                            trialBalance.MainAccount = "{*}";



                            trialBalance.nCreditBalance = 0;
                            trialBalance.nCreditTransAction = 0;
                            trialBalance.nDebitBalance = 0;
                            trialBalance.nDebitTransAction = 0;
                            trialBalance.nNetCredit = 0;
                            trialBalance.nNetDebit = 0;

                        }
                        else
                        {
                            trialBalance.IsMainAccount = 0;
                            trialBalance.MainAccount = "";

                            trialBalance.nExpectedAnnually = trialBalance.ExpectedAnnually;
                            trialBalance.nActualTotal = trialBalance.ActualTotal;
                            trialBalance.nRemainingExpected = trialBalance.RemainingExpected;
                            trialBalance.nRemainingRatio = trialBalance.RemainingRatio;

                            trialBalance.nCreditBalance = CreditBalance;
                            trialBalance.nCreditTransAction = CreditTransAction;
                            trialBalance.nDebitBalance = DebitBalance;
                            trialBalance.nDebitTransAction = DebitTransAction;
                            trialBalance.nNetCredit = NetCredit;
                            trialBalance.nNetDebit = NetDebit;

                            trialBalance.nJanuary = January;
                            trialBalance.nFebruary = February;
                            trialBalance.nMarch = March;
                            trialBalance.nApril = April;
                            trialBalance.nMay = May;
                            trialBalance.nJune = June;
                            trialBalance.nJuly = July;
                            trialBalance.nAugust = August;
                            trialBalance.nSeptember = September;
                            trialBalance.nOctober = October;
                            trialBalance.nNovember = November;
                            trialBalance.nDecember = December;
 


                        }
                        if (!Obj.ShowEstimatedZero)
                        {
                            if ((trialBalance.Expected1 + trialBalance.Expected2 + trialBalance.Expected3 + trialBalance.Expected4 + trialBalance.Expected5 + trialBalance.Expected5 +
                                trialBalance.Expected7 + trialBalance.Expected8 + trialBalance.Expected9 + trialBalance.Expected10 + trialBalance.Expected11 + trialBalance.Expected12) != 0)
                            {
                                TrialBalanceVMList.Add(trialBalance);
                            }
                        }
                        else
                        {
                            //if ((trialBalance.NetCredit != 0) || (trialBalance.NetDebit != 0) || (TotalDebit != 0) || (NetCredit != 0))
                            //{
                            TrialBalanceVMList.Add(trialBalance);
                            //}


                        }




                    }

                }
                else if ((Obj.ShowOnlyaccountswithcostcenter) && !String.IsNullOrEmpty(Obj.AccNo))
                {
                    var AccountByCost = _unitOfWork.NativeSql.GetChartOfAccountByCostCenterID(UserInfo.fCompanyId, Obj.AccNo, 3);
                    TranData = _unitOfWork.NativeSql.GetTransactionForTrial(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate, Obj.ByCostCenter, Obj.CostCenterNumber, Obj.Partofthenumber, Obj.CostSearchType);
                    MonthlyData = _unitOfWork.NativeSql.GetTransactionForTrialYearly(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate, Obj.ByCostCenter, Obj.CostCenterNumber, Obj.Partofthenumber, Obj.CostSearchType);
                    BudgetData = _unitOfWork.EstimatedBudget.GetAllEstimatedBudgets(UserInfo.fCompanyId, CurrYear);

                    foreach (var Acc in AccountByCost)//Each Main Account
                    {
                        TotalDebit = 0;
                        TOTCredit = 0;
                        NetCredit = 0;
                        NetDebit = 0;
                        CreditBalance = 0;
                        DebitBalance = 0;
                        CreditTransAction = 0;
                        DebitTransAction = 0;

                        January = 0;
                        February = 0;
                        March = 0;
                        April = 0;
                        May = 0;
                        June = 0;
                        July = 0;
                        August = 0;
                        September = 0;
                        October = 0;
                        November = 0;
                        December = 0;

                        NetTot = 0;

                        MonthTransaction1 = 0;
                        Expected1 = 0;
                        ChangeVaule1 = 0;
                        ChangePrec1 = 0;

                        MonthTransaction2 = 0;
                        Expected2 = 0;
                        ChangeVaule2 = 0;
                        ChangePrec2 = 0;


                        MonthTransaction3 = 0;
                        Expected3 = 0;
                        ChangeVaule3 = 0;
                        ChangePrec3 = 0;


                        MonthTransaction4 = 0;
                        Expected4 = 0;
                        ChangeVaule4 = 0;
                        ChangePrec4 = 0;


                        MonthTransaction5 = 0;
                        Expected5 = 0;
                        ChangeVaule5 = 0;
                        ChangePrec5 = 0;

                        MonthTransaction6 = 0;
                        Expected6 = 0;
                        ChangeVaule6 = 0;
                        ChangePrec6 = 0;


                        MonthTransaction7 = 0;
                        Expected7 = 0;
                        ChangeVaule7 = 0;
                        ChangePrec7 = 0;


                        MonthTransaction8 = 0;
                        Expected8 = 0;
                        ChangeVaule8 = 0;
                        ChangePrec8 = 0;


                        MonthTransaction9 = 0;
                        Expected9 = 0;
                        ChangeVaule9 = 0;
                        ChangePrec9 = 0;


                        MonthTransaction10 = 0;
                        Expected10 = 0;
                        ChangeVaule10 = 0;
                        ChangePrec10 = 0;



                        MonthTransaction10 = 0;
                        Expected10 = 0;
                        ChangeVaule10 = 0;
                        ChangePrec10 = 0;


                        MonthTransaction11 = 0;
                        Expected11 = 0;
                        ChangeVaule11 = 0;
                        ChangePrec11 = 0;


                        MonthTransaction12 = 0;
                        Expected12 = 0;
                        ChangeVaule12 = 0;
                        ChangePrec12 = 0;

                        var TransActionData = TranData.Where(m => m.AccountNumber == Acc.AccountNumber);
                        var BalanceData = TotData.Where(m => m.AccountNumber == Acc.AccountNumber);
                        var MonthlyTrans = MonthlyData.Where(m => m.AccountNumber == Acc.AccountNumber);
                        foreach (var D in TransActionData)
                        {
                            CreditTransAction += D.CreditTransAction;
                            DebitTransAction += D.DebitTransAction;
                        }
                        foreach (var Balance in BalanceData)
                        {
                            CreditBalance += Balance.CreditBalance;
                            DebitBalance += Balance.DebitBalance;
                        }
                        foreach (var D in MonthlyTrans)
                        {
                            switch (D.Month)
                            {
                                case 1:
                                    January = January + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 2:
                                    February = February + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 3:
                                    March = March + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 4:
                                    April = April + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 5:
                                    May = May + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 6:
                                    June = June + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 7:
                                    July = July + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 8:
                                    August = August + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 9:
                                    September = September + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 10:
                                    October = October + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 11:
                                    November = November + (D.DebitTransAction - D.CreditTransAction);
                                    break;
                                case 12:
                                    December = December + (D.DebitTransAction - D.CreditTransAction);
                                    break;

                            }

                        }



                        TotalDebit = DebitTransAction + DebitBalance;
                        TOTCredit = CreditTransAction + CreditBalance;

                        if (TotalDebit > TOTCredit)
                        {
                            NetDebit = TotalDebit - TOTCredit;
                            NetTot = NetDebit;
                        }
                        if (TotalDebit < TOTCredit)
                        {
                            NetCredit = TOTCredit - TotalDebit;
                            NetTot = NetCredit;


                        }
                        NetTot = TotalDebit - TOTCredit;
                        OpenBalance = DebitBalance - CreditBalance;

                        TrialBalanceVM trialBalance = new TrialBalanceVM();
                        trialBalance.AccountNumber = Acc.AccountNumber;
                        trialBalance.Name = Acc.ArabicName;

                        trialBalance.CreditBalance = CreditBalance;
                        trialBalance.CreditTransAction = CreditTransAction;
                        trialBalance.DebitBalance = DebitBalance;
                        trialBalance.DebitTransAction = DebitTransAction;
                        trialBalance.NetCredit = NetCredit;
                        trialBalance.NetDebit = NetDebit;

                        trialBalance.January = January;
                        trialBalance.February = February;
                        trialBalance.March = March;
                        trialBalance.April = April;
                        trialBalance.May = May;
                        trialBalance.June = June;
                        trialBalance.July = July;
                        trialBalance.August = August;
                        trialBalance.September = September;
                        trialBalance.October = October;
                        trialBalance.November = November;
                        trialBalance.December = December;

                        trialBalance.OpenBalance = OpenBalance;
                        trialBalance.NetTot = NetTot;
                        BudgetData = BudgetData.Where(m => m.CostCenterNumber != null).ToList();
                        var CurrBudget = BudgetData.Where(m => m.AccountNumber == Acc.AccountNumber && m.BudgetType == 3);
                        if (CurrBudget.Count() > 0)
                        {
                            foreach (var D in CurrBudget)
                            {

                                Expected1 = Expected1 + D.January;

                                Expected2 = Expected2 + D.February;

                                Expected3 = Expected3 + D.March;

                                Expected4 = Expected4 + D.April;

                                Expected5 = Expected5 + D.May;

                                Expected6 = Expected6 + D.June;

                                Expected7 = Expected7 + D.July;

                                Expected8 = Expected8 + D.August;

                                Expected9 = Expected9 + D.September;

                                Expected10 = Expected10 + D.October;


                                Expected11 = Expected11 + D.November;

                                Expected12 = Expected12 + D.December;


                            }


                        }

                        MonthTransaction1 = January;
                        ChangeVaule1 = January - Expected1;
                        ChangePrec1 = 0;
                        if (Expected1 != 0)
                        {
                            ChangePrec1 = 0;// Math.Round((ChangeVaule1 / MonthTransaction1) * 100, 3);
                        }

                        MonthTransaction2 = February;
                        ChangeVaule2 = February - Expected2;
                        ChangePrec2 = 0;
                        if (Expected2 != 0)
                        {
                            ChangePrec2 = 0;// Math.Round((ChangeVaule2 / MonthTransaction2) * 100, 3);
                        }


                        MonthTransaction3 = March;
                        ChangeVaule3 = March - Expected3;
                        ChangePrec3 = 0;
                        if (Expected3 != 0)
                        {
                            ChangePrec3 = 0;//  Math.Round((ChangeVaule3 / MonthTransaction3) * 100, 3);
                        }

                        MonthTransaction4 = April;
                        ChangeVaule4 = April - Expected4;
                        ChangePrec4 = 0;
                        if (Expected4 != 0)
                        {
                            ChangePrec4 = 0;// Math.Round((ChangeVaule4 / MonthTransaction4) * 100, 3);
                        }

                        MonthTransaction5 = May;
                        ChangeVaule5 = (May) - Expected5;
                        ChangePrec5 = 0;
                        if (Expected5 != 0)
                        {
                            ChangePrec5 = 0;// Math.Round((ChangeVaule5 / MonthTransaction5) * 100, 3);
                        }

                        MonthTransaction6 = (June);
                        ChangeVaule6 = (June) - Expected6;
                        ChangePrec6 = 0;
                        if (Expected6 != 0)
                        {
                            ChangePrec6 = 0;//  Math.Round((ChangeVaule6 / MonthTransaction6) * 100, 3);
                        }

                        MonthTransaction7 = (July);
                        ChangeVaule7 = (July) - Expected7;
                        ChangePrec7 = 0;
                        if (Expected7 != 0)
                        {
                            ChangePrec7 = 0;//  Math.Round((ChangeVaule7 / MonthTransaction7) * 100, 3);
                        }



                        MonthTransaction8 = (August);
                        ChangeVaule8 = (August) - Expected8;
                        ChangePrec8 = 0;
                        if (Expected8 != 0)
                        {
                            ChangePrec8 = 0;//  Math.Round((ChangeVaule8 / MonthTransaction8) * 100, 3);
                        }


                        MonthTransaction9 = (September);
                        ChangeVaule9 = (September) - Expected9;
                        ChangePrec9 = 0;
                        if (Expected9 != 0)
                        {
                            ChangePrec9 = 0;//  Math.Round((ChangeVaule9 / MonthTransaction9) * 100, 3);
                        }


                        MonthTransaction10 = (October);
                        ChangeVaule10 = (October) - Expected10;
                        ChangePrec10 = 0;
                        if (Expected10 != 0)
                        {
                            ChangePrec10 = 0;//  Math.Round((ChangeVaule10 / MonthTransaction10) * 100, 3);
                        }


                        MonthTransaction11 = (November);
                        ChangeVaule11 = (November) - Expected11;
                        ChangePrec11 = 0;
                        if (Expected11 != 0)
                        {
                            ChangePrec11 = 0;// Math.Round((ChangeVaule11 / MonthTransaction11) * 100, 3);
                        }


                        MonthTransaction12 = (December);
                        ChangeVaule12 = (December) - Expected12;
                        ChangePrec12 = 0;
                        if (Expected12 != 0)
                        {
                            ChangePrec12 = 0;//  Math.Round((ChangeVaule12 / MonthTransaction12) * 100, 3);
                        }


                        trialBalance.Expected1 = Expected1;
                        trialBalance.MonthTransaction1 = MonthTransaction1;
                        trialBalance.ChangeVaule1 = ChangeVaule1;
                        trialBalance.ChangePrec1 = ChangePrec1;

                        trialBalance.Expected2 = Expected2;
                        trialBalance.MonthTransaction2 = MonthTransaction2;
                        trialBalance.ChangeVaule2 = ChangeVaule2;
                        trialBalance.ChangePrec2 = ChangePrec2;


                        trialBalance.Expected3 = Expected3;
                        trialBalance.MonthTransaction3 = MonthTransaction3;
                        trialBalance.ChangeVaule3 = ChangeVaule3;
                        trialBalance.ChangePrec3 = ChangePrec3;


                        trialBalance.Expected4 = Expected4;
                        trialBalance.MonthTransaction4 = MonthTransaction4;
                        trialBalance.ChangeVaule4 = ChangeVaule4;
                        trialBalance.ChangePrec4 = ChangePrec4;


                        trialBalance.Expected5 = Expected5;
                        trialBalance.MonthTransaction5 = MonthTransaction5;
                        trialBalance.ChangeVaule5 = ChangeVaule5;
                        trialBalance.ChangePrec5 = ChangePrec5;


                        trialBalance.Expected6 = Expected6;
                        trialBalance.MonthTransaction6 = MonthTransaction6;
                        trialBalance.ChangeVaule6 = ChangeVaule6;
                        trialBalance.ChangePrec6 = ChangePrec6;


                        trialBalance.Expected7 = Expected7;
                        trialBalance.MonthTransaction7 = MonthTransaction7;
                        trialBalance.ChangeVaule7 = ChangeVaule7;
                        trialBalance.ChangePrec7 = ChangePrec7;


                        trialBalance.Expected8 = Expected8;
                        trialBalance.MonthTransaction8 = MonthTransaction8;
                        trialBalance.ChangeVaule8 = ChangeVaule8;
                        trialBalance.ChangePrec8 = ChangePrec8;


                        trialBalance.Expected9 = Expected9;
                        trialBalance.MonthTransaction9 = MonthTransaction9;
                        trialBalance.ChangeVaule9 = ChangeVaule9;
                        trialBalance.ChangePrec9 = ChangePrec9;


                        trialBalance.Expected10 = Expected10;
                        trialBalance.MonthTransaction10 = MonthTransaction10;
                        trialBalance.ChangeVaule10 = ChangeVaule10;
                        trialBalance.ChangePrec10 = ChangePrec10;


                        trialBalance.Expected11 = Expected11;
                        trialBalance.MonthTransaction11 = MonthTransaction11;
                        trialBalance.ChangeVaule11 = ChangeVaule11;
                        trialBalance.ChangePrec11 = ChangePrec11;


                        trialBalance.Expected12 = Expected12;
                        trialBalance.MonthTransaction12 = MonthTransaction12;
                        trialBalance.ChangeVaule12 = ChangeVaule12;
                        trialBalance.ChangePrec12 = ChangePrec12;


                        trialBalance.Expected1 = Expected1;
                        trialBalance.MonthTransaction1 = MonthTransaction1;
                        trialBalance.ChangeVaule1 = ChangeVaule1;
                        trialBalance.ChangePrec1 = ChangePrec1;



                        trialBalance.Expected1 = Expected1;
                        trialBalance.MonthTransaction1 = MonthTransaction1;
                        trialBalance.ChangeVaule1 = ChangeVaule1;
                        trialBalance.ChangePrec1 = ChangePrec1;


                        trialBalance.Expected1 = Expected1;
                        trialBalance.MonthTransaction1 = MonthTransaction1;
                        trialBalance.ChangeVaule1 = ChangeVaule1;
                        trialBalance.ChangePrec1 = ChangePrec1;

                        trialBalance.ExpectedAnnually = (trialBalance.Expected1 + trialBalance.Expected2 + trialBalance.Expected3 + trialBalance.Expected4 + trialBalance.Expected5 + trialBalance.Expected5 +
                                trialBalance.Expected7 + trialBalance.Expected8 + trialBalance.Expected9 + trialBalance.Expected10 + trialBalance.Expected11 + trialBalance.Expected12);

                        trialBalance.ActualTotal = Math.Round(NetTot, 4);
                        trialBalance.RemainingExpected = Math.Round((trialBalance.ExpectedAnnually - Math.Abs(NetTot)), 4);
                        trialBalance.RemainingRatio = 0;
                        trialBalance.ExpectedAnnually = Math.Round(trialBalance.ExpectedAnnually, 4);
                        if (false/*(AccountData.FirstOrDefault(m => m.CostFather == Acc.CostNumber) != null) && (Acc.CostLevel < Obj.AccountLevelDropVMID)*/)
                        {
                            trialBalance.IsMainAccount = 1;
                            trialBalance.MainAccount = "{*}";



                            trialBalance.nCreditBalance = 0;
                            trialBalance.nCreditTransAction = 0;
                            trialBalance.nDebitBalance = 0;
                            trialBalance.nDebitTransAction = 0;
                            trialBalance.nNetCredit = 0;
                            trialBalance.nNetDebit = 0;

                        }
                        else
                        {
                            trialBalance.IsMainAccount = 0;
                            trialBalance.MainAccount = "";
                            trialBalance.MainAccount = "{*}";
                            trialBalance.nExpectedAnnually = trialBalance.ExpectedAnnually;
                            trialBalance.nActualTotal = trialBalance.ActualTotal;
                            trialBalance.nRemainingExpected = trialBalance.RemainingExpected;
                            trialBalance.nRemainingRatio = trialBalance.RemainingRatio;

                            trialBalance.nCreditBalance = CreditBalance;
                            trialBalance.nCreditTransAction = CreditTransAction;
                            trialBalance.nDebitBalance = DebitBalance;
                            trialBalance.nDebitTransAction = DebitTransAction;
                            trialBalance.nNetCredit = NetCredit;
                            trialBalance.nNetDebit = NetDebit;

                            trialBalance.nJanuary = January;
                            trialBalance.nFebruary = February;
                            trialBalance.nMarch = March;
                            trialBalance.nApril = April;
                            trialBalance.nMay = May;
                            trialBalance.nJune = June;
                            trialBalance.nJuly = July;
                            trialBalance.nAugust = August;
                            trialBalance.nSeptember = September;
                            trialBalance.nOctober = October;
                            trialBalance.nNovember = November;
                            trialBalance.nDecember = December;



                        }
                        if (!Obj.ShowEstimatedZero)
                        {
                            if ((trialBalance.Expected1 + trialBalance.Expected2 + trialBalance.Expected3 + trialBalance.Expected4 + trialBalance.Expected5 + trialBalance.Expected5 +
                                trialBalance.Expected7 + trialBalance.Expected8 + trialBalance.Expected9 + trialBalance.Expected10 + trialBalance.Expected11 + trialBalance.Expected12) != 0)
                            {
                                TrialBalanceVMList.Add(trialBalance);
                            }
                        }
                        else
                        {
                            //if ((trialBalance.NetCredit != 0) || (trialBalance.NetDebit != 0) || (TotalDebit != 0) || (NetCredit != 0))
                            //{
                            TrialBalanceVMList.Add(trialBalance);
                            //}


                        }




                    }

                }
                if (!Obj.ShowMainAccountValue)
                {
                    foreach (var trialBalance in TrialBalanceVMList.Where(m => m.IsMainAccount == 1))
                    {
                        if (trialBalance.IsMainAccount == 1)
                        {
                            trialBalance.ActualTotal =0;
                            trialBalance.RemainingExpected = 0;
                            trialBalance.RemainingRatio = 0;
                            trialBalance.ExpectedAnnually =0;

                            trialBalance.Expected = 0;
                            trialBalance.MonthTransaction = 0;
                            trialBalance.ChangeVaule = 0;
                            trialBalance.ChangePrec = 0;
                            trialBalance.CreditBalance = 0;
                            trialBalance.CreditTransAction = 0;
                            trialBalance.DebitBalance = 0;
                            trialBalance.DebitTransAction = 0;
                            trialBalance.NetCredit = 0;
                            trialBalance.NetDebit = 0;


                            trialBalance.Expected1 = 0;
                            trialBalance.MonthTransaction1 = 0;
                            trialBalance.ChangeVaule1 = 0;
                            trialBalance.ChangePrec1 = 0;

                            trialBalance.Expected2 = 0;
                            trialBalance.MonthTransaction2 = 0;
                            trialBalance.ChangeVaule2 = 0;
                            trialBalance.ChangePrec2 = 0;


                            trialBalance.Expected3 = 0;
                            trialBalance.MonthTransaction3 = 0;
                            trialBalance.ChangeVaule3 = 0;
                            trialBalance.ChangePrec3 = 0;


                            trialBalance.Expected4 = 0;
                            trialBalance.MonthTransaction4 = 0;
                            trialBalance.ChangeVaule4 = 0;
                            trialBalance.ChangePrec4 = 0;


                            trialBalance.Expected5 = 0;
                            trialBalance.MonthTransaction5 = 0;
                            trialBalance.ChangeVaule5 = 0;
                            trialBalance.ChangePrec5 = 0;


                            trialBalance.Expected6 = 0;
                            trialBalance.MonthTransaction6 = 0;
                            trialBalance.ChangeVaule6 = 0;
                            trialBalance.ChangePrec6 = 0;


                            trialBalance.Expected7 = 0;
                            trialBalance.MonthTransaction7 = 0;
                            trialBalance.ChangeVaule7 = 0;
                            trialBalance.ChangePrec7 = 0;


                            trialBalance.Expected8 = 0;
                            trialBalance.MonthTransaction8 = 0;
                            trialBalance.ChangeVaule8 = 0;
                            trialBalance.ChangePrec8 = 0;


                            trialBalance.Expected9 = 0;
                            trialBalance.MonthTransaction9 = 0;
                            trialBalance.ChangeVaule9 = 0;
                            trialBalance.ChangePrec9 = 0;


                            trialBalance.Expected10 = 0;
                            trialBalance.MonthTransaction10 = 0;
                            trialBalance.ChangeVaule10 = 0;
                            trialBalance.ChangePrec10 = 0;


                            trialBalance.Expected11 = 0;
                            trialBalance.MonthTransaction11 = 0;
                            trialBalance.ChangeVaule11 = 0;
                            trialBalance.ChangePrec11 = 0;


                            trialBalance.Expected12 = 0;
                            trialBalance.MonthTransaction12 = 0;
                            trialBalance.ChangeVaule12 = 0;
                            trialBalance.ChangePrec12 = 0;


                            trialBalance.January = 0;
                            trialBalance.February = 0;
                            trialBalance.March = 0;
                            trialBalance.April = 0;
                            trialBalance.May = 0;
                            trialBalance.June = 0;
                            trialBalance.July = 0;
                            trialBalance.August = 0;
                            trialBalance.September = 0;
                            trialBalance.October = 0;
                            trialBalance.November = 0;
                            trialBalance.December = 0;

                            trialBalance.nActualTotal = 0;
                            trialBalance.nRemainingExpected = 0;
                            trialBalance.nRemainingRatio = 0;
                            trialBalance.nExpectedAnnually = 0;

                            trialBalance.nExpected = 0;
                            trialBalance.nMonthTransaction = 0;
                            trialBalance.nChangeVaule = 0;
                            trialBalance.nChangePrec = 0;
                            trialBalance.nCreditBalance = 0;
                            trialBalance.nCreditTransAction = 0;
                            trialBalance.nDebitBalance = 0;
                            trialBalance.nDebitTransAction = 0;
                            trialBalance.nNetCredit = 0;
                            trialBalance.nNetDebit = 0;


                            trialBalance.nExpected1 = 0;
                            trialBalance.nMonthTransaction1 = 0;
                            trialBalance.nChangeVaule1 = 0;
                            trialBalance.nChangePrec1 = 0;

                            trialBalance.nExpected2 = 0;
                            trialBalance.nMonthTransaction2 = 0;
                            trialBalance.nChangeVaule2 = 0;
                            trialBalance.nChangePrec2 = 0;


                            trialBalance.nExpected3 = 0;
                            trialBalance.nMonthTransaction3 = 0;
                            trialBalance.nChangeVaule3 = 0;
                            trialBalance.nChangePrec3 = 0;


                            trialBalance.nExpected4 = 0;
                            trialBalance.nMonthTransaction4 = 0;
                            trialBalance.nChangeVaule4 = 0;
                            trialBalance.nChangePrec4 = 0;


                            trialBalance.nExpected5 = 0;
                            trialBalance.nMonthTransaction5 = 0;
                            trialBalance.nChangeVaule5 = 0;
                            trialBalance.nChangePrec5 = 0;


                            trialBalance.nExpected6 = 0;
                            trialBalance.nMonthTransaction6 = 0;
                            trialBalance.nChangeVaule6 = 0;
                            trialBalance.nChangePrec6 = 0;


                            trialBalance.nExpected7 = 0;
                            trialBalance.nMonthTransaction7 = 0;
                            trialBalance.nChangeVaule7 = 0;
                            trialBalance.nChangePrec7 = 0;


                            trialBalance.nExpected8 = 0;
                            trialBalance.nMonthTransaction8 = 0;
                            trialBalance.nChangeVaule8 = 0;
                            trialBalance.nChangePrec8 = 0;


                            trialBalance.nExpected9 = 0;
                            trialBalance.nMonthTransaction9 = 0;
                            trialBalance.nChangeVaule9 = 0;
                            trialBalance.nChangePrec9 = 0;


                            trialBalance.nExpected10 = 0;
                            trialBalance.nMonthTransaction10 = 0;
                            trialBalance.nChangeVaule10 = 0;
                            trialBalance.nChangePrec10 = 0;


                            trialBalance.nExpected11 = 0;
                            trialBalance.nMonthTransaction11 = 0;
                            trialBalance.nChangeVaule11 = 0;
                            trialBalance.nChangePrec11 = 0;


                            trialBalance.nExpected12 = 0;
                            trialBalance.nMonthTransaction12 = 0;
                            trialBalance.nChangeVaule12 = 0;
                            trialBalance.nChangePrec12 = 0;


                            trialBalance.nJanuary = 0;
                            trialBalance.nFebruary = 0;
                            trialBalance.nMarch = 0;
                            trialBalance.nApril = 0;
                            trialBalance.nMay = 0;
                            trialBalance.nJune = 0;
                            trialBalance.nJuly = 0;
                            trialBalance.nAugust = 0;
                            trialBalance.nSeptember = 0;
                            trialBalance.nOctober = 0;
                            trialBalance.nNovember = 0;
                            trialBalance.nDecember = 0;
                        }

                    }

                }



                return Json(TrialBalanceVMList, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                string err = ex.Message;

                List<TrialBalanceVM> TrialBalanceVMList = new List<TrialBalanceVM>();

                return Json(TrialBalanceVMList, JsonRequestBehavior.AllowGet);

            }


        }
        [ValidateInput(false)]
        [Authorize(Roles = "CoOwner,RepTransActionDetails")]
        public ActionResult GetTransActionDetails( )
        {
            var userId = User.Identity.GetUserId();
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            int CurrYear = UserInfo.CurrYear;
            DateTime FromDate= DateTime.Parse("01/01/"+ CurrYear);
            DateTime ToDate = DateTime.Parse("31/12/"+ CurrYear);
            try
            {
                FromDate = !string.IsNullOrEmpty(Request.Params["FromDate"]) ? DateTime.Parse(Request.Params["FromDate"]) : DateTime.Parse("01/01/2020");
                ToDate = !string.IsNullOrEmpty(Request.Params["ToDate"]) ? DateTime.Parse(Request.Params["ToDate"]) : DateTime.Parse("31/12/2020");
            }
            catch
            {

            }


            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            ViewBag.WorkWithCostCenter = Company.WorkWithCostCenter;
            ViewBag.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            ViewBag.TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency;
            var CompanyTransAction = _unitOfWork.CompanyTransactionKind.GetAllCompanyTransactionKind(UserInfo.fCompanyId);
            var TransActionKind = _unitOfWork.TransactionKind.GetAllTransactionKind();

            List<TransReportDetailVM> ObjAllData = new List<TransReportDetailVM>();
            var TransData = _unitOfWork.NativeSql.GetTransActionDatasReportDetail(UserInfo.fCompanyId,  FromDate, ToDate);
            int  RowID = 1;
            foreach(var D in TransData)
            {
                D.iRowID =  RowID;
                if (D.CompanyTransactionKindNo == 0)
                {
                    var V = TransActionKind.FirstOrDefault(m => m.TransactionKindID == D.TransactionKindNo);
                    if (V != null)
                    {
                        if (Resources.Resource.CurLang == "Arb")
                        { 
                            D.TransName = V.ArabicName; 
                        }
                        else 
                           { 
                            D.TransName = V.EnglishName; 
                        }  
                    }
                } 
                {
                    var V = CompanyTransAction.FirstOrDefault(m => m.TransactionKindID == D.TransactionKindNo
                      && m.CompanyTransactionKindID == D.CompanyTransactionKindNo);
                    if (V != null)
                    {
                        if (Resources.Resource.CurLang == "Arb")
                        {
                            D.TransName = V.ArabicName;
                        }
                        else
                        {
                            D.TransName = V.EnglishName;
                        }
                    }


                }
                ObjAllData.Add(D);
                 RowID =  RowID + 1;


            }

            return PartialView("_GridViewPartial", ObjAllData);
        }
        
        public ActionResult ReceivableAge()
        {
            var userId = User.Identity.GetUserId();

            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            int CurrYear = UserInfo.CurrYear;

            ReceivableAgeFilter Obj = new ReceivableAgeFilter
            {
                
                ToDate = DateTime.Now,
                NumberOfDays=7
            };
            return View(Obj);
        }
        [Authorize(Roles = "CoOwner,RepPivotReportAccounts")]
        public ActionResult PivotReportAccounts()
        {

            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            //  var GetAllLevel = _unitOfWork.NativeSql.GetAccountLevelVMs(UserInfo.fCompanyId);

            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            String Chart = CoInfo.AccountChart;


            Chart = Chart.Trim(new Char[] { '-', '-', ' ' });

            string[] numbers = Regex.Split(Chart, @"\D+");
            var AccountLevel = new List<AccountLevelDropVM>();
            int i = 1;
            foreach (string value in numbers)
            {
                if (!string.IsNullOrEmpty(value))
                {

                    AccountLevelDropVM Obj = new AccountLevelDropVM
                    {
                        AccountLevel = i,
                        LevelName = Resources.Resource.Level + " " + i
                    };
                    AccountLevel.Add(Obj);
                    i = i + 1;
                }
            }



            int CurrYear = UserInfo.CurrYear;

            AccountLevelRepVM ObjAccountLevelRep = new AccountLevelRepVM
            {
                AccountLevelDropVM = AccountLevel,
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                AccountLevelDropVMID = 1,
                PivotMonthTypeVM = new List<PivotMonthTypeVM>(),
                RepMonthType = 0
            };



            return View(ObjAccountLevelRep);
        }
        [ValidateInput(false)]
        public ActionResult PivotAccountsGridPartial()
        {
            AccountLevelRepVM Obj = new AccountLevelRepVM();
            var userId = User.Identity.GetUserId();
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            int CurrYear = UserInfo.CurrYear;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            ViewBag.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            ViewBag.WorkWithCostCenter = Company.WorkWithCostCenter;
            Obj.FromDate = DateTime.Parse("01/01/" + CurrYear);
            Obj.ToDate = DateTime.Parse("31/12/" + CurrYear);
            Obj.AccountLevelDropVMID = 1;
            Obj.RepMonthType = 0;
 

            var CompanyTransAction = _unitOfWork.CompanyTransactionKind.GetAllCompanyTransactionKind(UserInfo.fCompanyId);
            var TransActionKind = _unitOfWork.TransactionKind.GetAllTransactionKind();
            var AccountType = _unitOfWork.AccountType.GetAllAccountType();
            var CostCenter = _unitOfWork.CostCenter.GetAllCostCenter(UserInfo.fCompanyId);
            try
            {
                Obj.FromDate = !string.IsNullOrEmpty(Request.Params["FromDate"]) ? DateTime.Parse(Request.Params["FromDate"]) : DateTime.Now;
                Obj.ToDate = !string.IsNullOrEmpty(Request.Params["ToDate"]) ? DateTime.Parse(Request.Params["ToDate"]) : DateTime.Now;
                Obj.AccountLevelDropVMID = !string.IsNullOrEmpty(Request.Params["AccountLevelDropVMID"]) ? int.Parse(Request.Params["AccountLevelDropVMID"]) : 1;
                Obj.RepMonthType = !string.IsNullOrEmpty(Request.Params["RepMonthType"]) ? int.Parse(Request.Params["RepMonthType"]) : 0;

            }
            catch
            {

            }
            try
            {


                var AccountData = _unitOfWork.NativeSql.GetChartOfAccountByLevel(UserInfo.fCompanyId);


                var Date = "01/01/" + CurrYear;
                var OpenDate = "31/12/" + (CurrYear - 1).ToString();

                IEnumerable<TrialBalanceVM> TotData = new List<TrialBalanceVM>();
                var TranData = _unitOfWork.NativeSql.GetTransactionForPivotAcc(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);


                double TotalDebit = 0;
                double TOTCredit = 0;
                double NetCredit = 0;
                double NetDebit = 0;
                double CreditBalance = 0;
                double DebitBalance = 0;
                double CreditTransAction = 0;
                double DebitTransAction = 0;

                List<TransActionPivotVM> TransActionPivotVMList = new List<TransActionPivotVM>();

                IEnumerable<ChartOfAccount> Accounts = new List<ChartOfAccount>();

                IEnumerable<ChartOfAccount> LessMainAccount = new List<ChartOfAccount>();
                if (!String.IsNullOrEmpty(Obj.AccNo))
                {

                    var MainAccount = AccountData.FirstOrDefault(m => m.AccountNumber == Obj.AccNo);
                    var MainChild = AccountData.Where(m => m.AccountFather == Obj.AccNo).OrderBy(m => m.AccountNumber).OrderBy(m => m.AccountLevel).ToList();

                    foreach (var Acc in MainChild)
                    {
                        TotalDebit = 0;
                        TOTCredit = 0;
                        NetCredit = 0;
                        NetDebit = 0;
                        CreditBalance = 0;
                        DebitBalance = 0;
                        CreditTransAction = 0;
                        DebitTransAction = 0;


                        if (AccountData.FirstOrDefault(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel) != null)//Check if Account Had Branches
                        {
                            var AllAccount = AccountData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel)
                                            .OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();//Get All Account Branches
                            int FatherLevel = Acc.AccountLevel;
                            foreach (var CurrAccLevel in AllAccount)
                            {
                                if (CurrAccLevel.AccountLevel == FatherLevel)
                                {
                                    break;
                                }
                                else
                                {

                                    var TransActionData = TranData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                    var BalanceData = TotData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                    foreach (var D in TransActionData)
                                    {
                                        TransActionPivotVM ObjData = new TransActionPivotVM();
                                        ObjData.AccountNumber = Acc.AccountNumber;
                                        ObjData.Name = "[" + Acc.AccountNumber + "]" + Acc.ArabicName;
                                        ObjData.Debit = D.Debit;
                                        ObjData.Credit = D.Credit;
                                        ObjData.NetTot = D.Debit - D.Credit;
                                        ObjData.VoucherDate = D.VoucherDate;
                                        ObjData.CompanyTransactionKindNo = D.CompanyTransactionKindNo;
                                        ObjData.TransName = D.CompanyTransactionKindNo.ToString();
                                        ObjData.sVoucherDate = D.VoucherDate.Year.ToString() + "/" + D.VoucherDate.Month.ToString("00");
                                        ObjData.AccountType = AccountType.FirstOrDefault(m => m.AccountTypeID == Acc.AccountTypeID).ArabicName;
                                        ObjData.CostCenterNumber = D.CostCenter;

                                        var CostName = CostCenter.FirstOrDefault(m => m.CostNumber == D.CostCenter);
                                        if (CostName != null)
                                            ObjData.CostCenterName = "[" + D.CostCenter + "]" + CostName.ArabicName;
                                        if (D.CompanyTransactionKindNo == 0)
                                        {
                                            var V = TransActionKind.FirstOrDefault(m => m.TransactionKindID == D.TransactionKindNo);
                                            if (V != null)
                                            {
                                                ObjData.TransName = V.ArabicName;
                                            }


                                        }
                                        else
                                        {
                                            var V = CompanyTransAction.FirstOrDefault(m => m.TransactionKindID == D.TransactionKindNo
                                              && m.CompanyTransactionKindID == D.CompanyTransactionKindNo);
                                            if (V != null)
                                            {
                                                ObjData.TransName = V.ArabicName;
                                            }


                                        }


                                        if (Obj.RepMonthType == 1)
                                        {
                                            if (D.VoucherDate.Month <= 3)
                                            {
                                                ObjData.sVoucherDate = Resources.Resource.Quart1 + "/" + D.VoucherDate.Year.ToString();
                                            }
                                            else if (D.VoucherDate.Month <= 6)
                                            {
                                                ObjData.sVoucherDate = Resources.Resource.Quart2 + "/" + D.VoucherDate.Year.ToString();
                                            }
                                            else if (D.VoucherDate.Month <= 9)
                                            {
                                                ObjData.sVoucherDate = Resources.Resource.Quart3 + "/" + D.VoucherDate.Year.ToString();
                                            }
                                            else if (D.VoucherDate.Month <= 12)
                                            {
                                                ObjData.sVoucherDate = Resources.Resource.Quart3 + "/" + D.VoucherDate.Year.ToString();
                                            }
                                        }

                                        else if (Obj.RepMonthType == 2)
                                        {
                                            if (D.VoucherDate.Month <= 6)
                                            {
                                                ObjData.sVoucherDate = Resources.Resource.Half1 + "/" + D.VoucherDate.Year.ToString();
                                            }
                                            else if (D.VoucherDate.Month <= 12)
                                            {
                                                ObjData.sVoucherDate = Resources.Resource.Half2 + "/" + D.VoucherDate.Year.ToString();
                                            }
                                        }



                                        TransActionPivotVMList.Add(ObjData);
                                    }


                                }
                            }




                        }
                        else
                        {
                            var TransActionData = TranData.Where(m => m.AccountNumber == Acc.AccountNumber);
                            var BalanceData = TotData.Where(m => m.AccountNumber == Acc.AccountNumber);
                            foreach (var D in TransActionData)
                            {
                                TransActionPivotVM ObjData = new TransActionPivotVM();
                                ObjData.AccountNumber = Acc.AccountNumber;
                                ObjData.Name = "[" + Acc.AccountNumber + "]" + Acc.ArabicName;
                                ObjData.Debit = D.Debit;
                                ObjData.Credit = D.Credit;
                                ObjData.NetTot = D.Debit - D.Credit;
                                ObjData.VoucherDate = D.VoucherDate;
                                ObjData.CompanyTransactionKindNo = D.CompanyTransactionKindNo;
                                ObjData.TransName = D.CompanyTransactionKindNo.ToString();
                                ObjData.sVoucherDate = D.VoucherDate.Year.ToString() + "/" + D.VoucherDate.Month.ToString("00");
                                ObjData.AccountType = AccountType.FirstOrDefault(m => m.AccountTypeID == Acc.AccountTypeID).ArabicName;
                                var CostName = CostCenter.FirstOrDefault(m => m.CostNumber == D.CostCenter);
                                if (CostName != null)
                                    ObjData.CostCenterName = "[" + D.CostCenter + "]" + CostName.ArabicName;
                                if (D.CompanyTransactionKindNo == 0)
                                {
                                    var V = TransActionKind.FirstOrDefault(m => m.TransactionKindID == D.TransactionKindNo);
                                    if (V != null)
                                    {
                                        ObjData.TransName = V.ArabicName;
                                    }


                                }
                                else
                                {
                                    var V = CompanyTransAction.FirstOrDefault(m => m.TransactionKindID == D.TransactionKindNo
                                      && m.CompanyTransactionKindID == D.CompanyTransactionKindNo);
                                    if (V != null)
                                    {
                                        ObjData.TransName = V.ArabicName;
                                    }


                                }


                                if (Obj.RepMonthType == 1)
                                {
                                    if (D.VoucherDate.Month <= 3)
                                    {
                                        ObjData.sVoucherDate = Resources.Resource.Quart1 + "/" + D.VoucherDate.Year.ToString();
                                    }
                                    else if (D.VoucherDate.Month <= 6)
                                    {
                                        ObjData.sVoucherDate = Resources.Resource.Quart2 + "/" + D.VoucherDate.Year.ToString();
                                    }
                                    else if (D.VoucherDate.Month <= 9)
                                    {
                                        ObjData.sVoucherDate = Resources.Resource.Quart3 + "/" + D.VoucherDate.Year.ToString();
                                    }
                                    else if (D.VoucherDate.Month <= 12)
                                    {
                                        ObjData.sVoucherDate = Resources.Resource.Quart3 + "/" + D.VoucherDate.Year.ToString();
                                    }
                                }

                                else if (Obj.RepMonthType == 2)
                                {
                                    if (D.VoucherDate.Month <= 6)
                                    {
                                        ObjData.sVoucherDate = Resources.Resource.Half1 + "/" + D.VoucherDate.Year.ToString();
                                    }
                                    else if (D.VoucherDate.Month <= 12)
                                    {
                                        ObjData.sVoucherDate = Resources.Resource.Half2 + "/" + D.VoucherDate.Year.ToString();
                                    }
                                }
                                TransActionPivotVMList.Add(ObjData);
                            }
                        }



                    }


                }
                else

                if ((Obj.AccountLevelDropVMID > 0) && (!Obj.Detail))
                {


                    Accounts = AccountData.Where(m => m.AccountLevel == Obj.AccountLevelDropVMID).OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();

                    LessMainAccount = AccountData.Where(m => m.AccountLevel < Obj.AccountLevelDropVMID).OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();

                    IEnumerable<string> FaherAccount = LessMainAccount.Where(m => m.AccountFather != "0").Select(m => m.AccountFather);


                    foreach (var A in LessMainAccount)
                    {
                        if (AccountData.FirstOrDefault(m => m.AccountFather == A.AccountNumber) != null)
                        {
                            //do nothing
                        }
                        else

                        {
                            Accounts = Accounts.Append(A);
                        }

                    }
                    Accounts = Accounts.OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();
                    // var MainChild = AccountData.Where(m => m.AccountFather == MainAcc.AccountNumber).OrderBy(m => m.AccountNumber).OrderBy(m => m.AccountLevel).ToList();
                    foreach (var Acc in Accounts)
                    {
                        TotalDebit = 0;
                        TOTCredit = 0;
                        NetCredit = 0;
                        NetDebit = 0;
                        CreditBalance = 0;
                        DebitBalance = 0;
                        CreditTransAction = 0;
                        DebitTransAction = 0;


                        if (AccountData.FirstOrDefault(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel) != null)
                        {
                            var AllAccount = AccountData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel)
                                            .OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();
                            int FatherLevel = Acc.AccountLevel;

                            foreach (var CurrAccLevel in AllAccount)
                            {
                                if (CurrAccLevel.AccountLevel == FatherLevel)
                                {
                                    break;
                                }
                                else
                                {
                                    var TransActionData = TranData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                    var BalanceData = TotData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                    foreach (var D in TransActionData)
                                    {
                                        TransActionPivotVM ObjData = new TransActionPivotVM();
                                        ObjData.AccountNumber = Acc.AccountNumber;

                                        ObjData.Name = "[" + Acc.AccountNumber + "]" + Acc.ArabicName;
                                        ObjData.Debit = D.Debit;
                                        ObjData.Credit = D.Credit;
                                        ObjData.NetTot = D.Debit - D.Credit;
                                        ObjData.VoucherDate = D.VoucherDate;
                                        ObjData.CompanyTransactionKindNo = D.CompanyTransactionKindNo;
                                        ObjData.TransName = D.CompanyTransactionKindNo.ToString();
                                        ObjData.sVoucherDate = D.VoucherDate.Year.ToString() + "/" + D.VoucherDate.Month.ToString("00");
                                        ObjData.AccountType = AccountType.FirstOrDefault(m => m.AccountTypeID == Acc.AccountTypeID).ArabicName;
                                        if (D.CompanyTransactionKindNo == 0)
                                        {
                                            var V = TransActionKind.FirstOrDefault(m => m.TransactionKindID == D.TransactionKindNo);
                                            if (V != null)
                                            {
                                                ObjData.TransName = V.ArabicName;
                                            }


                                        }
                                        else
                                        {
                                            var V = CompanyTransAction.FirstOrDefault(m => m.TransactionKindID == D.TransactionKindNo
                                              && m.CompanyTransactionKindID == D.CompanyTransactionKindNo);
                                            if (V != null)
                                            {
                                                ObjData.TransName = V.ArabicName;
                                            }


                                        }

                                        if (Obj.RepMonthType == 1)
                                        {
                                            if (D.VoucherDate.Month <= 3)
                                            {
                                                ObjData.sVoucherDate = Resources.Resource.Quart1 + "/" + D.VoucherDate.Year.ToString();
                                            }
                                            else if (D.VoucherDate.Month <= 6)
                                            {
                                                ObjData.sVoucherDate = Resources.Resource.Quart2 + "/" + D.VoucherDate.Year.ToString();
                                            }
                                            else if (D.VoucherDate.Month <= 9)
                                            {
                                                ObjData.sVoucherDate = Resources.Resource.Quart3 + "/" + D.VoucherDate.Year.ToString();
                                            }
                                            else if (D.VoucherDate.Month <= 12)
                                            {
                                                ObjData.sVoucherDate = Resources.Resource.Quart3 + "/" + D.VoucherDate.Year.ToString();
                                            }
                                        }

                                        else if (Obj.RepMonthType == 2)
                                        {
                                            if (D.VoucherDate.Month <= 6)
                                            {
                                                ObjData.sVoucherDate = Resources.Resource.Half1 + "/" + D.VoucherDate.Year.ToString();
                                            }
                                            else if (D.VoucherDate.Month <= 12)
                                            {
                                                ObjData.sVoucherDate = Resources.Resource.Half2 + "/" + D.VoucherDate.Year.ToString();
                                            }
                                        }
                                        TransActionPivotVMList.Add(ObjData);
                                    }
                                    //foreach (var Balance in BalanceData)
                                    //{
                                    //    CreditBalance += Balance.CreditBalance;
                                    //    DebitBalance += Balance.DebitBalance;
                                    //}
                                    //TotalDebit = DebitTransAction + DebitBalance;
                                    //TOTCredit = CreditTransAction + CreditBalance;
                                }
                            }
                        }
                        else
                        {
                            var TransActionData = TranData.Where(m => m.AccountNumber == Acc.AccountNumber);
                            var BalanceData = TotData.Where(m => m.AccountNumber == Acc.AccountNumber);
                            foreach (var D in TransActionData)
                            {
                                TransActionPivotVM ObjData = new TransActionPivotVM();
                                ObjData.AccountNumber = Acc.AccountNumber;
                                ObjData.Name = "[" + Acc.AccountNumber + "]" + Acc.ArabicName;
                                ObjData.Debit = D.Debit;
                                ObjData.Credit = D.Credit;
                                ObjData.NetTot = D.Debit - D.Credit;
                                ObjData.VoucherDate = D.VoucherDate;
                                ObjData.CompanyTransactionKindNo = D.CompanyTransactionKindNo;
                                ObjData.TransName = D.CompanyTransactionKindNo.ToString();
                                ObjData.sVoucherDate = D.VoucherDate.Year.ToString() + "/" + D.VoucherDate.Month.ToString("00");
                                ObjData.AccountType = AccountType.FirstOrDefault(m => m.AccountTypeID == Acc.AccountTypeID).ArabicName;
                                if (D.CompanyTransactionKindNo == 0)
                                {
                                    var V = TransActionKind.FirstOrDefault(m => m.TransactionKindID == D.TransactionKindNo);
                                    if (V != null)
                                    {
                                        ObjData.TransName = V.ArabicName;
                                    }


                                }
                                else
                                {
                                    var V = CompanyTransAction.FirstOrDefault(m => m.TransactionKindID == D.TransactionKindNo
                                      && m.CompanyTransactionKindID == D.CompanyTransactionKindNo);
                                    if (V != null)
                                    {
                                        ObjData.TransName = V.ArabicName;
                                    }


                                }


                                if (Obj.RepMonthType == 1)
                                {
                                    if (D.VoucherDate.Month <= 3)
                                    {
                                        ObjData.sVoucherDate = Resources.Resource.Quart1 + "/" + D.VoucherDate.Year.ToString();
                                    }
                                    else if (D.VoucherDate.Month <= 6)
                                    {
                                        ObjData.sVoucherDate = Resources.Resource.Quart2 + "/" + D.VoucherDate.Year.ToString();
                                    }
                                    else if (D.VoucherDate.Month <= 9)
                                    {
                                        ObjData.sVoucherDate = Resources.Resource.Quart3 + "/" + D.VoucherDate.Year.ToString();
                                    }
                                    else if (D.VoucherDate.Month <= 12)
                                    {
                                        ObjData.sVoucherDate = Resources.Resource.Quart3 + "/" + D.VoucherDate.Year.ToString();
                                    }
                                }

                                else if (Obj.RepMonthType == 2)
                                {
                                    if (D.VoucherDate.Month <= 6)
                                    {
                                        ObjData.sVoucherDate = Resources.Resource.Half1 + "/" + D.VoucherDate.Year.ToString();
                                    }
                                    else if (D.VoucherDate.Month <= 12)
                                    {
                                        ObjData.sVoucherDate = Resources.Resource.Half2 + "/" + D.VoucherDate.Year.ToString();
                                    }
                                }
                                TransActionPivotVMList.Add(ObjData);
                            }

                        }





                    }

                }




                return PartialView("_PivotAccountsGridPartial", TransActionPivotVMList);



            }
            catch (Exception ex)
            {
                string err = ex.Message;

                List<TransActionPivotVM> TransActionPivotVMList = new List<TransActionPivotVM>();
                return PartialView("_PivotAccountsGridPartial", TransActionPivotVMList);


            }



        }
        public ActionResult ExportToXLSX_DataAware()
        {
            AccountLevelRepVM Obj = new AccountLevelRepVM();
            var userId = User.Identity.GetUserId();
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            int CurrYear = UserInfo.CurrYear;
            Obj.FromDate = DateTime.Parse("01/01/" + CurrYear);
            Obj.ToDate = DateTime.Parse("31/12/" + CurrYear);
            Obj.AccountLevelDropVMID = 1;

            var CompanyTransAction = _unitOfWork.CompanyTransactionKind.GetAllCompanyTransactionKind(UserInfo.fCompanyId);
            var TransActionKind = _unitOfWork.TransactionKind.GetAllTransactionKind();
            List<TransActionPivotVM> TransActionPivotVMList = new List<TransActionPivotVM>();
            try
            {
                Obj.FromDate = !string.IsNullOrEmpty(Request.Params["FromDate"]) ? DateTime.Parse(Request.Params["FromDate"]) : DateTime.Now;
                Obj.ToDate = !string.IsNullOrEmpty(Request.Params["ToDate"]) ? DateTime.Parse(Request.Params["ToDate"]) : DateTime.Now;
                Obj.AccountLevelDropVMID = !string.IsNullOrEmpty(Request.Params["AccountLevelDropVMID"]) ? int.Parse(Request.Params["AccountLevelDropVMID"]) : 1;
            }
            catch
            {

            }
            try
            {


                var AccountData = _unitOfWork.NativeSql.GetChartOfAccountByLevel(UserInfo.fCompanyId);


                var Date = "01/01/" + CurrYear;
                var OpenDate = "31/12/" + (CurrYear - 1).ToString();

                IEnumerable<TrialBalanceVM> TotData = new List<TrialBalanceVM>();
                var TranData = _unitOfWork.NativeSql.GetTransactionForPivotAcc(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);


                double TotalDebit = 0;
                double TOTCredit = 0;
                double NetCredit = 0;
                double NetDebit = 0;
                double CreditBalance = 0;
                double DebitBalance = 0;
                double CreditTransAction = 0;
                double DebitTransAction = 0;


                IEnumerable<ChartOfAccount> Accounts = new List<ChartOfAccount>();

                IEnumerable<ChartOfAccount> LessMainAccount = new List<ChartOfAccount>();
                if (!String.IsNullOrEmpty(Obj.AccNo))
                {

                    var MainAccount = AccountData.FirstOrDefault(m => m.AccountNumber == Obj.AccNo);
                    var MainChild = AccountData.Where(m => m.AccountFather == Obj.AccNo).OrderBy(m => m.AccountNumber).OrderBy(m => m.AccountLevel).ToList();

                    foreach (var Acc in MainChild)
                    {
                        TotalDebit = 0;
                        TOTCredit = 0;
                        NetCredit = 0;
                        NetDebit = 0;
                        CreditBalance = 0;
                        DebitBalance = 0;
                        CreditTransAction = 0;
                        DebitTransAction = 0;


                        if (AccountData.FirstOrDefault(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel) != null)//Check if Account Had Branches
                        {
                            var AllAccount = AccountData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel)
                                            .OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();//Get All Account Branches
                            int FatherLevel = Acc.AccountLevel;
                            foreach (var CurrAccLevel in AllAccount)
                            {
                                if (CurrAccLevel.AccountLevel == FatherLevel)
                                {
                                    break;
                                }
                                else
                                {

                                    var TransActionData = TranData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                    var BalanceData = TotData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                    foreach (var D in TransActionData)
                                    {
                                        TransActionPivotVM ObjData = new TransActionPivotVM();
                                        ObjData.AccountNumber = Acc.AccountNumber;
                                        ObjData.Name = "[" + Acc.AccountNumber + "]" + Acc.ArabicName;
                                        ObjData.Debit = D.Debit;
                                        ObjData.Credit = D.Credit;
                                        ObjData.NetTot = D.Debit - D.Credit;
                                        ObjData.VoucherDate = D.VoucherDate;
                                        ObjData.CompanyTransactionKindNo = D.CompanyTransactionKindNo;
                                        ObjData.TransName = D.CompanyTransactionKindNo.ToString();
                                        ObjData.sVoucherDate = D.VoucherDate.Year.ToString() + "/" + D.VoucherDate.Month.ToString("00");
                                        if (D.CompanyTransactionKindNo == 0)
                                        {
                                            var V = TransActionKind.FirstOrDefault(m => m.TransactionKindID == D.TransactionKindNo);
                                            if (V != null)
                                            {
                                                ObjData.TransName = V.ArabicName;
                                            }


                                        }
                                        else
                                        {
                                            var V = CompanyTransAction.FirstOrDefault(m => m.TransactionKindID == D.TransactionKindNo
                                              && m.CompanyTransactionKindID == D.CompanyTransactionKindNo);
                                            if (V != null)
                                            {
                                                ObjData.TransName = V.ArabicName;
                                            }


                                        }

                                        TransActionPivotVMList.Add(ObjData);
                                    }


                                }
                            }




                        }
                        else
                        {
                            var TransActionData = TranData.Where(m => m.AccountNumber == Acc.AccountNumber);
                            var BalanceData = TotData.Where(m => m.AccountNumber == Acc.AccountNumber);
                            foreach (var D in TransActionData)
                            {
                                TransActionPivotVM ObjData = new TransActionPivotVM();
                                ObjData.AccountNumber = Acc.AccountNumber;
                                ObjData.Name = "[" + Acc.AccountNumber + "]" + Acc.ArabicName;
                                ObjData.Debit = D.Debit;
                                ObjData.Credit = D.Credit;
                                ObjData.NetTot = D.Debit - D.Credit;
                                ObjData.VoucherDate = D.VoucherDate;
                                ObjData.CompanyTransactionKindNo = D.CompanyTransactionKindNo;
                                ObjData.TransName = D.CompanyTransactionKindNo.ToString();
                                ObjData.sVoucherDate = D.VoucherDate.Year.ToString() + "/" + D.VoucherDate.Month.ToString("00");
                                if (D.CompanyTransactionKindNo == 0)
                                {
                                    var V = TransActionKind.FirstOrDefault(m => m.TransactionKindID == D.TransactionKindNo);
                                    if (V != null)
                                    {
                                        ObjData.TransName = V.ArabicName;
                                    }


                                }
                                else
                                {
                                    var V = CompanyTransAction.FirstOrDefault(m => m.TransactionKindID == D.TransactionKindNo
                                      && m.CompanyTransactionKindID == D.CompanyTransactionKindNo);
                                    if (V != null)
                                    {
                                        ObjData.TransName = V.ArabicName;
                                    }


                                }
                                TransActionPivotVMList.Add(ObjData);
                            }
                        }



                    }


                }
                else

                if ((Obj.AccountLevelDropVMID > 0) && (!Obj.Detail))
                {


                    Accounts = AccountData.Where(m => m.AccountLevel == Obj.AccountLevelDropVMID).OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();

                    LessMainAccount = AccountData.Where(m => m.AccountLevel < Obj.AccountLevelDropVMID).OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();

                    IEnumerable<string> FaherAccount = LessMainAccount.Where(m => m.AccountFather != "0").Select(m => m.AccountFather);


                    foreach (var A in LessMainAccount)
                    {
                        if (AccountData.FirstOrDefault(m => m.AccountFather == A.AccountNumber) != null)
                        {
                            //do nothing
                        }
                        else

                        {
                            Accounts = Accounts.Append(A);
                        }

                    }
                    Accounts = Accounts.OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();
                    // var MainChild = AccountData.Where(m => m.AccountFather == MainAcc.AccountNumber).OrderBy(m => m.AccountNumber).OrderBy(m => m.AccountLevel).ToList();
                    foreach (var Acc in Accounts)
                    {
                        TotalDebit = 0;
                        TOTCredit = 0;
                        NetCredit = 0;
                        NetDebit = 0;
                        CreditBalance = 0;
                        DebitBalance = 0;
                        CreditTransAction = 0;
                        DebitTransAction = 0;


                        if (AccountData.FirstOrDefault(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel) != null)
                        {
                            var AllAccount = AccountData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel)
                                            .OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();
                            int FatherLevel = Acc.AccountLevel;

                            foreach (var CurrAccLevel in AllAccount)
                            {
                                if (CurrAccLevel.AccountLevel == FatherLevel)
                                {
                                    break;
                                }
                                else
                                {
                                    var TransActionData = TranData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                    var BalanceData = TotData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                    foreach (var D in TransActionData)
                                    {
                                        TransActionPivotVM ObjData = new TransActionPivotVM();
                                        ObjData.AccountNumber = Acc.AccountNumber;

                                        ObjData.Name = "[" + Acc.AccountNumber + "]" + Acc.ArabicName;
                                        ObjData.Debit = D.Debit;
                                        ObjData.Credit = D.Credit;
                                        ObjData.NetTot = D.Debit - D.Credit;
                                        ObjData.VoucherDate = D.VoucherDate;
                                        ObjData.CompanyTransactionKindNo = D.CompanyTransactionKindNo;
                                        ObjData.TransName = D.CompanyTransactionKindNo.ToString();
                                        ObjData.sVoucherDate = D.VoucherDate.Year.ToString() + "/" + D.VoucherDate.Month.ToString("00");
                                        if (D.CompanyTransactionKindNo == 0)
                                        {
                                            var V = TransActionKind.FirstOrDefault(m => m.TransactionKindID == D.TransactionKindNo);
                                            if (V != null)
                                            {
                                                ObjData.TransName = V.ArabicName;
                                            }


                                        }
                                        else
                                        {
                                            var V = CompanyTransAction.FirstOrDefault(m => m.TransactionKindID == D.TransactionKindNo
                                              && m.CompanyTransactionKindID == D.CompanyTransactionKindNo);
                                            if (V != null)
                                            {
                                                ObjData.TransName = V.ArabicName;
                                            }


                                        }
                                        TransActionPivotVMList.Add(ObjData);
                                    }
                                    //foreach (var Balance in BalanceData)
                                    //{
                                    //    CreditBalance += Balance.CreditBalance;
                                    //    DebitBalance += Balance.DebitBalance;
                                    //}
                                    //TotalDebit = DebitTransAction + DebitBalance;
                                    //TOTCredit = CreditTransAction + CreditBalance;
                                }
                            }
                        }
                        else
                        {
                            var TransActionData = TranData.Where(m => m.AccountNumber == Acc.AccountNumber);
                            var BalanceData = TotData.Where(m => m.AccountNumber == Acc.AccountNumber);
                            foreach (var D in TransActionData)
                            {
                                TransActionPivotVM ObjData = new TransActionPivotVM();
                                ObjData.AccountNumber = Acc.AccountNumber;
                                ObjData.Name = "[" + Acc.AccountNumber + "]" + Acc.ArabicName;
                                ObjData.Debit = D.Debit;
                                ObjData.Credit = D.Credit;
                                ObjData.NetTot = D.Debit - D.Credit;
                                ObjData.VoucherDate = D.VoucherDate;
                                ObjData.CompanyTransactionKindNo = D.CompanyTransactionKindNo;
                                ObjData.TransName = D.CompanyTransactionKindNo.ToString();
                                ObjData.sVoucherDate = D.VoucherDate.Year.ToString() + "/" + D.VoucherDate.Month.ToString("00");
                                if (D.CompanyTransactionKindNo == 0)
                                {
                                    var V = TransActionKind.FirstOrDefault(m => m.TransactionKindID == D.TransactionKindNo);
                                    if (V != null)
                                    {
                                        ObjData.TransName = V.ArabicName;
                                    }


                                }
                                else
                                {
                                    var V = CompanyTransAction.FirstOrDefault(m => m.TransactionKindID == D.TransactionKindNo
                                      && m.CompanyTransactionKindID == D.CompanyTransactionKindNo);
                                    if (V != null)
                                    {
                                        ObjData.TransName = V.ArabicName;
                                    }


                                }
                                TransActionPivotVMList.Add(ObjData);
                            }

                        }





                    }

                }








            }
            catch (Exception ex)
            {
                string err = ex.Message;

                TransActionPivotVMList = new List<TransActionPivotVM>();


            }
            return PivotGridExtension.ExportToXlsx(Acc.PivotGridHelper.PivotGridHelper.Settings, TransActionPivotVMList  /*Models.NwindModel.GetData()*/, Acc.PivotGridHelper.PivotGridHelper.XlsxOptions);
        }
        public ActionResult ExportToXLSX_WYSIWYG()
        {
            AccountLevelRepVM Obj = new AccountLevelRepVM();
            var userId = User.Identity.GetUserId();
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            int CurrYear = UserInfo.CurrYear;

            Obj.FromDate = DateTime.Parse("01/01/" + CurrYear);
            Obj.ToDate = DateTime.Parse("31/12/" + CurrYear);
            Obj.AccountLevelDropVMID = 1;
            Obj.RepMonthType = 0;
           var CompanyTransAction = _unitOfWork.CompanyTransactionKind.GetAllCompanyTransactionKind(UserInfo.fCompanyId);
            var TransActionKind = _unitOfWork.TransactionKind.GetAllTransactionKind();
            var AccountType = _unitOfWork.AccountType.GetAllAccountType();
            var CostCenter = _unitOfWork.CostCenter.GetAllCostCenter(UserInfo.fCompanyId);
            if (CostCenter.Count() == 0) 
                CostCenter = new List<CostCenter>();
            List<TransActionPivotVM> TransActionPivotVMList = new List<TransActionPivotVM>();
            try
            {
                Obj.FromDate = !string.IsNullOrEmpty(Request.Params["FromDate"]) ? DateTime.Parse(Request.Params["FromDate"]) : DateTime.Now;
                Obj.ToDate = !string.IsNullOrEmpty(Request.Params["ToDate"]) ? DateTime.Parse(Request.Params["ToDate"]) : DateTime.Now;
                Obj.AccountLevelDropVMID = !string.IsNullOrEmpty(Request.Params["AccountLevelDropVMID"]) ? int.Parse(Request.Params["AccountLevelDropVMID"]) : 1;
                Obj.RepMonthType = !string.IsNullOrEmpty(Request.Params["RepMonthType"]) ? int.Parse(Request.Params["RepMonthType"]) : 0;
            }
            catch
            {

            }
            try
            {


                var AccountData = _unitOfWork.NativeSql.GetChartOfAccountByLevel(UserInfo.fCompanyId);


                var Date = "01/01/" + CurrYear;
                var OpenDate = "31/12/" + (CurrYear - 1).ToString();

                IEnumerable<TrialBalanceVM> TotData = new List<TrialBalanceVM>();
                var TranData = _unitOfWork.NativeSql.GetTransactionForPivotAcc(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);


                double TotalDebit = 0;
                double TOTCredit = 0;
                double NetCredit = 0;
                double NetDebit = 0;
                double CreditBalance = 0;
                double DebitBalance = 0;
                double CreditTransAction = 0;
                double DebitTransAction = 0;

         
                IEnumerable<ChartOfAccount> Accounts = new List<ChartOfAccount>();

                IEnumerable<ChartOfAccount> LessMainAccount = new List<ChartOfAccount>();
                if (!String.IsNullOrEmpty(Obj.AccNo))
                {

                    var MainAccount = AccountData.FirstOrDefault(m => m.AccountNumber == Obj.AccNo);
                    var MainChild = AccountData.Where(m => m.AccountFather == Obj.AccNo).OrderBy(m => m.AccountNumber).OrderBy(m => m.AccountLevel).ToList();

                    foreach (var Acc in MainChild)
                    {
                        TotalDebit = 0;
                        TOTCredit = 0;
                        NetCredit = 0;
                        NetDebit = 0;
                        CreditBalance = 0;
                        DebitBalance = 0;
                        CreditTransAction = 0;
                        DebitTransAction = 0;


                        if (AccountData.FirstOrDefault(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel) != null)//Check if Account Had Branches
                        {
                            var AllAccount = AccountData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel)
                                            .OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();//Get All Account Branches
                            int FatherLevel = Acc.AccountLevel;
                            foreach (var CurrAccLevel in AllAccount)
                            {
                                if (CurrAccLevel.AccountLevel == FatherLevel)
                                {
                                    break;
                                }
                                else
                                {

                                    var TransActionData = TranData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                    var BalanceData = TotData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                    foreach (var D in TransActionData)
                                    {
                                        TransActionPivotVM ObjData = new TransActionPivotVM();
                                        ObjData.AccountNumber = Acc.AccountNumber;
                                        ObjData.Name = "[" + Acc.AccountNumber + "]" + Acc.ArabicName;
                                        ObjData.Debit = D.Debit;
                                        ObjData.Credit = D.Credit;
                                        ObjData.NetTot = D.Debit - D.Credit;
                                        ObjData.VoucherDate = D.VoucherDate;
                                        ObjData.CompanyTransactionKindNo = D.CompanyTransactionKindNo;
                                        ObjData.TransName = D.CompanyTransactionKindNo.ToString();
                                        ObjData.sVoucherDate = D.VoucherDate.Year.ToString() + "/" + D.VoucherDate.Month.ToString("00");
                                        ObjData.AccountType = AccountType.FirstOrDefault(m => m.AccountTypeID == Acc.AccountTypeID).ArabicName;
                                        var CostName = CostCenter.FirstOrDefault(m => m.CostNumber == D.CostCenter);
                                        if (CostName != null)
                                            ObjData.CostCenterName = "[" + D.CostCenter + "]" + CostName.ArabicName;
                                        if (D.CompanyTransactionKindNo == 0)
                                        {
                                            var V = TransActionKind.FirstOrDefault(m => m.TransactionKindID == D.TransactionKindNo);
                                            if (V != null)
                                            {
                                                ObjData.TransName = V.ArabicName;
                                            }


                                        }
                                        else
                                        {
                                            var V = CompanyTransAction.FirstOrDefault(m => m.TransactionKindID == D.TransactionKindNo
                                              && m.CompanyTransactionKindID == D.CompanyTransactionKindNo);
                                            if (V != null)
                                            {
                                                ObjData.TransName = V.ArabicName;
                                            }


                                        }

                                        if (Obj.RepMonthType == 1)
                                        {
                                            if (D.VoucherDate.Month <= 3)
                                            {
                                                ObjData.sVoucherDate = Resources.Resource.Quart1 + "/" + D.VoucherDate.Year.ToString();
                                            }
                                            else if (D.VoucherDate.Month <= 6)
                                            {
                                                ObjData.sVoucherDate = Resources.Resource.Quart2 + "/" + D.VoucherDate.Year.ToString();
                                            }
                                            else if (D.VoucherDate.Month <= 9)
                                            {
                                                ObjData.sVoucherDate = Resources.Resource.Quart3 + "/" + D.VoucherDate.Year.ToString();
                                            }
                                            else if (D.VoucherDate.Month <= 12)
                                            {
                                                ObjData.sVoucherDate = Resources.Resource.Quart3 + "/" + D.VoucherDate.Year.ToString();
                                            }
                                        }

                                        else if (Obj.RepMonthType == 2)
                                        {
                                            if (D.VoucherDate.Month <= 6)
                                            {
                                                ObjData.sVoucherDate = Resources.Resource.Half1 + "/" + D.VoucherDate.Year.ToString();
                                            }
                                            else if (D.VoucherDate.Month <= 12)
                                            {
                                                ObjData.sVoucherDate = Resources.Resource.Half2 + "/" + D.VoucherDate.Year.ToString();
                                            }
                                        }

                                        TransActionPivotVMList.Add(ObjData);
                                    }


                                }
                            }




                        }
                        else
                        {
                            var TransActionData = TranData.Where(m => m.AccountNumber == Acc.AccountNumber);
                            var BalanceData = TotData.Where(m => m.AccountNumber == Acc.AccountNumber);
                            foreach (var D in TransActionData)
                            {
                                TransActionPivotVM ObjData = new TransActionPivotVM();
                                ObjData.AccountNumber = Acc.AccountNumber;
                                ObjData.Name = "[" + Acc.AccountNumber + "]" + Acc.ArabicName;
                                ObjData.Debit = D.Debit;
                                ObjData.Credit = D.Credit;
                                ObjData.NetTot = D.Debit - D.Credit;
                                ObjData.VoucherDate = D.VoucherDate;
                                ObjData.CompanyTransactionKindNo = D.CompanyTransactionKindNo;
                                ObjData.TransName = D.CompanyTransactionKindNo.ToString();
                                ObjData.sVoucherDate = D.VoucherDate.Year.ToString() + "/" + D.VoucherDate.Month.ToString("00");
                                ObjData.AccountType = AccountType.FirstOrDefault(m => m.AccountTypeID == Acc.AccountTypeID).ArabicName;

                                var CostName = CostCenter.FirstOrDefault(m => m.CostNumber == D.CostCenter);
                                if (CostName != null)
                                    ObjData.CostCenterName = "[" + D.CostCenter + "]" + CostName.ArabicName;
                                if (D.CompanyTransactionKindNo == 0)
                                {
                                    var V = TransActionKind.FirstOrDefault(m => m.TransactionKindID == D.TransactionKindNo);
                                    if (V != null)
                                    {
                                        ObjData.TransName = V.ArabicName;
                                    }


                                }
                                else
                                {
                                    var V = CompanyTransAction.FirstOrDefault(m => m.TransactionKindID == D.TransactionKindNo
                                      && m.CompanyTransactionKindID == D.CompanyTransactionKindNo);
                                    if (V != null)
                                    {
                                        ObjData.TransName = V.ArabicName;
                                    }


                                }

                                if (Obj.RepMonthType == 1)
                                {
                                    if (D.VoucherDate.Month <= 3)
                                    {
                                        ObjData.sVoucherDate = Resources.Resource.Quart1 + "/" + D.VoucherDate.Year.ToString();
                                    }
                                    else if (D.VoucherDate.Month <= 6)
                                    {
                                        ObjData.sVoucherDate = Resources.Resource.Quart2 + "/" + D.VoucherDate.Year.ToString();
                                    }
                                    else if (D.VoucherDate.Month <= 9)
                                    {
                                        ObjData.sVoucherDate = Resources.Resource.Quart3 + "/" + D.VoucherDate.Year.ToString();
                                    }
                                    else if (D.VoucherDate.Month <= 12)
                                    {
                                        ObjData.sVoucherDate = Resources.Resource.Quart3 + "/" + D.VoucherDate.Year.ToString();
                                    }
                                }

                                else if (Obj.RepMonthType == 2)
                                {
                                    if (D.VoucherDate.Month <= 6)
                                    {
                                        ObjData.sVoucherDate = Resources.Resource.Half1 + "/" + D.VoucherDate.Year.ToString();
                                    }
                                    else if (D.VoucherDate.Month <= 12)
                                    {
                                        ObjData.sVoucherDate = Resources.Resource.Half2 + "/" + D.VoucherDate.Year.ToString();
                                    }
                                }
                                TransActionPivotVMList.Add(ObjData);
                            }
                        }



                    }


                }
                else

                if ((Obj.AccountLevelDropVMID > 0) && (!Obj.Detail))
                {


                    Accounts = AccountData.Where(m => m.AccountLevel == Obj.AccountLevelDropVMID).OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();

                    LessMainAccount = AccountData.Where(m => m.AccountLevel < Obj.AccountLevelDropVMID).OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();

                    IEnumerable<string> FaherAccount = LessMainAccount.Where(m => m.AccountFather != "0").Select(m => m.AccountFather);


                    foreach (var A in LessMainAccount)
                    {
                        if (AccountData.FirstOrDefault(m => m.AccountFather == A.AccountNumber) != null)
                        {
                            //do nothing
                        }
                        else

                        {
                            Accounts = Accounts.Append(A);
                        }

                    }
                    Accounts = Accounts.OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();
                    // var MainChild = AccountData.Where(m => m.AccountFather == MainAcc.AccountNumber).OrderBy(m => m.AccountNumber).OrderBy(m => m.AccountLevel).ToList();
                    foreach (var Acc in Accounts)
                    {
                        TotalDebit = 0;
                        TOTCredit = 0;
                        NetCredit = 0;
                        NetDebit = 0;
                        CreditBalance = 0;
                        DebitBalance = 0;
                        CreditTransAction = 0;
                        DebitTransAction = 0;


                        if (AccountData.FirstOrDefault(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel) != null)
                        {
                            var AllAccount = AccountData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel)
                                            .OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();
                            int FatherLevel = Acc.AccountLevel;

                            foreach (var CurrAccLevel in AllAccount)
                            {
                                if (CurrAccLevel.AccountLevel == FatherLevel)
                                {
                                    break;
                                }
                                else
                                {
                                    var TransActionData = TranData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                    var BalanceData = TotData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                    foreach (var D in TransActionData)
                                    {
                                        TransActionPivotVM ObjData = new TransActionPivotVM();
                                        ObjData.AccountNumber = Acc.AccountNumber;

                                        ObjData.Name = "[" + Acc.AccountNumber + "]" + Acc.ArabicName;
                                        ObjData.Debit = D.Debit;
                                        ObjData.Credit = D.Credit;
                                        ObjData.NetTot = D.Debit - D.Credit;
                                        ObjData.VoucherDate = D.VoucherDate;
                                        ObjData.CompanyTransactionKindNo = D.CompanyTransactionKindNo;
                                        ObjData.TransName = D.CompanyTransactionKindNo.ToString();
                                        ObjData.sVoucherDate = D.VoucherDate.Year.ToString() + "/" + D.VoucherDate.Month.ToString("00");
                                        ObjData.AccountType = AccountType.FirstOrDefault(m => m.AccountTypeID == Acc.AccountTypeID).ArabicName;
                                        var CostName = CostCenter.FirstOrDefault(m => m.CostNumber == D.CostCenter);
                                        if (CostName != null)
                                            ObjData.CostCenterName = "[" + D.CostCenter + "]" + CostName.ArabicName;
                                        if (D.CompanyTransactionKindNo == 0)
                                        {
                                            var V = TransActionKind.FirstOrDefault(m => m.TransactionKindID == D.TransactionKindNo);
                                            if (V != null)
                                            {
                                                ObjData.TransName = V.ArabicName;
                                            }


                                        }
                                        else
                                        {
                                            var V = CompanyTransAction.FirstOrDefault(m => m.TransactionKindID == D.TransactionKindNo
                                              && m.CompanyTransactionKindID == D.CompanyTransactionKindNo);
                                            if (V != null)
                                            {
                                                ObjData.TransName = V.ArabicName;
                                            }


                                        }

                                        if (Obj.RepMonthType == 1)
                                        {
                                            if (D.VoucherDate.Month <= 3)
                                            {
                                                ObjData.sVoucherDate = Resources.Resource.Quart1 + "/" + D.VoucherDate.Year.ToString();
                                            }
                                            else if (D.VoucherDate.Month <= 6)
                                            {
                                                ObjData.sVoucherDate = Resources.Resource.Quart2 + "/" + D.VoucherDate.Year.ToString();
                                            }
                                            else if (D.VoucherDate.Month <= 9)
                                            {
                                                ObjData.sVoucherDate = Resources.Resource.Quart3 + "/" + D.VoucherDate.Year.ToString();
                                            }
                                            else if (D.VoucherDate.Month <= 12)
                                            {
                                                ObjData.sVoucherDate = Resources.Resource.Quart3 + "/" + D.VoucherDate.Year.ToString();
                                            }
                                        }

                                        else if (Obj.RepMonthType == 2)
                                        {
                                            if (D.VoucherDate.Month <= 6)
                                            {
                                                ObjData.sVoucherDate = Resources.Resource.Half1 + "/" + D.VoucherDate.Year.ToString();
                                            }
                                            else if (D.VoucherDate.Month <= 12)
                                            {
                                                ObjData.sVoucherDate = Resources.Resource.Half1 + "/" + D.VoucherDate.Year.ToString();
                                            }
                                        }
                                        TransActionPivotVMList.Add(ObjData);
                                    }
                                    //foreach (var Balance in BalanceData)
                                    //{
                                    //    CreditBalance += Balance.CreditBalance;
                                    //    DebitBalance += Balance.DebitBalance;
                                    //}
                                    //TotalDebit = DebitTransAction + DebitBalance;
                                    //TOTCredit = CreditTransAction + CreditBalance;
                                }
                            }
                        }
                        else
                        {
                            var TransActionData = TranData.Where(m => m.AccountNumber == Acc.AccountNumber);
                            var BalanceData = TotData.Where(m => m.AccountNumber == Acc.AccountNumber);
                            foreach (var D in TransActionData)
                            {
                                TransActionPivotVM ObjData = new TransActionPivotVM();
                                ObjData.AccountNumber = Acc.AccountNumber;
                                ObjData.Name = "[" + Acc.AccountNumber + "]" + Acc.ArabicName;
                                ObjData.Debit = D.Debit;
                                ObjData.Credit = D.Credit;
                                ObjData.NetTot = D.Debit - D.Credit;
                                ObjData.VoucherDate = D.VoucherDate;
                                ObjData.CompanyTransactionKindNo = D.CompanyTransactionKindNo;
                                ObjData.TransName = D.CompanyTransactionKindNo.ToString();
                                ObjData.sVoucherDate = D.VoucherDate.Year.ToString() + "/" + D.VoucherDate.Month.ToString("00");
                                ObjData.AccountType = AccountType.FirstOrDefault(m => m.AccountTypeID == Acc.AccountTypeID).ArabicName;
                                var CostName = CostCenter.FirstOrDefault(m => m.CostNumber == D.CostCenter);
                                if (CostName != null)
                                    ObjData.CostCenterName = "[" + D.CostCenter + "]" + CostName.ArabicName;
                                if (D.CompanyTransactionKindNo == 0)
                                {
                                    var V = TransActionKind.FirstOrDefault(m => m.TransactionKindID == D.TransactionKindNo);
                                    if (V != null)
                                    {
                                        ObjData.TransName = V.ArabicName;
                                    }


                                }
                                else
                                {
                                    var V = CompanyTransAction.FirstOrDefault(m => m.TransactionKindID == D.TransactionKindNo
                                      && m.CompanyTransactionKindID == D.CompanyTransactionKindNo);
                                    if (V != null)
                                    {
                                        ObjData.TransName = V.ArabicName;
                                    }


                                }

                                if (Obj.RepMonthType == 1)
                                {
                                    if (D.VoucherDate.Month <= 3)
                                    {
                                        ObjData.sVoucherDate = Resources.Resource.Quart1 + "/" + D.VoucherDate.Year.ToString();
                                    }
                                    else if (D.VoucherDate.Month <= 6)
                                    {
                                        ObjData.sVoucherDate = Resources.Resource.Quart2 + "/" + D.VoucherDate.Year.ToString();
                                    }
                                    else if (D.VoucherDate.Month <= 9)
                                    {
                                        ObjData.sVoucherDate = Resources.Resource.Quart3 + "/" + D.VoucherDate.Year.ToString();
                                    }
                                    else if (D.VoucherDate.Month <= 12)
                                    {
                                        ObjData.sVoucherDate = Resources.Resource.Quart3 + "/" + D.VoucherDate.Year.ToString();
                                    }
                                }

                                else if (Obj.RepMonthType == 2)
                                {
                                    if (D.VoucherDate.Month <= 6)
                                    {
                                        ObjData.sVoucherDate = Resources.Resource.Half1 + "/" + D.VoucherDate.Year.ToString();
                                    }
                                    else if (D.VoucherDate.Month <= 12)
                                    {
                                        ObjData.sVoucherDate = Resources.Resource.Half2 + "/" + D.VoucherDate.Year.ToString();
                                    }
                                }
                                TransActionPivotVMList.Add(ObjData);
                            }

                        }





                    }

                }




               



            }
            catch (Exception ex)
            {
                string err = ex.Message;

                 TransActionPivotVMList = new List<TransActionPivotVM>();
               

            }
            PivotXlsxExportOptions options = new PivotXlsxExportOptions();
            options.ExportType = DevExpress.Export.ExportType.WYSIWYG;
            
            return PivotGridExtension.ExportToXlsx(Acc.PivotGridHelper.PivotGridHelper.Settings, TransActionPivotVMList, options);
        }
        public ActionResult ExportToPDF()
        {
            AccountLevelRepVM Obj = new AccountLevelRepVM();
            var userId = User.Identity.GetUserId();
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            int CurrYear = UserInfo.CurrYear;
            Obj.FromDate = DateTime.Parse("01/01/" + CurrYear);
            Obj.ToDate = DateTime.Parse("31/12/" + CurrYear);
            Obj.AccountLevelDropVMID = 1;

            var CompanyTransAction = _unitOfWork.CompanyTransactionKind.GetAllCompanyTransactionKind(UserInfo.fCompanyId);
            var TransActionKind = _unitOfWork.TransactionKind.GetAllTransactionKind();
            List<TransActionPivotVM> TransActionPivotVMList = new List<TransActionPivotVM>();
            try
            {
                Obj.FromDate = !string.IsNullOrEmpty(Request.Params["FromDate"]) ? DateTime.Parse(Request.Params["FromDate"]) : DateTime.Now;
                Obj.ToDate = !string.IsNullOrEmpty(Request.Params["ToDate"]) ? DateTime.Parse(Request.Params["ToDate"]) : DateTime.Now;
                Obj.AccountLevelDropVMID = !string.IsNullOrEmpty(Request.Params["AccountLevelDropVMID"]) ? int.Parse(Request.Params["AccountLevelDropVMID"]) : 1;
            }
            catch
            {

            }
            try
            {


                var AccountData = _unitOfWork.NativeSql.GetChartOfAccountByLevel(UserInfo.fCompanyId);


                var Date = "01/01/" + CurrYear;
                var OpenDate = "31/12/" + (CurrYear - 1).ToString();

                IEnumerable<TrialBalanceVM> TotData = new List<TrialBalanceVM>();
                var TranData = _unitOfWork.NativeSql.GetTransactionForPivotAcc(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);


                double TotalDebit = 0;
                double TOTCredit = 0;
                double NetCredit = 0;
                double NetDebit = 0;
                double CreditBalance = 0;
                double DebitBalance = 0;
                double CreditTransAction = 0;
                double DebitTransAction = 0;


                IEnumerable<ChartOfAccount> Accounts = new List<ChartOfAccount>();

                IEnumerable<ChartOfAccount> LessMainAccount = new List<ChartOfAccount>();
                if (!String.IsNullOrEmpty(Obj.AccNo))
                {

                    var MainAccount = AccountData.FirstOrDefault(m => m.AccountNumber == Obj.AccNo);
                    var MainChild = AccountData.Where(m => m.AccountFather == Obj.AccNo).OrderBy(m => m.AccountNumber).OrderBy(m => m.AccountLevel).ToList();

                    foreach (var Acc in MainChild)
                    {
                        TotalDebit = 0;
                        TOTCredit = 0;
                        NetCredit = 0;
                        NetDebit = 0;
                        CreditBalance = 0;
                        DebitBalance = 0;
                        CreditTransAction = 0;
                        DebitTransAction = 0;


                        if (AccountData.FirstOrDefault(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel) != null)//Check if Account Had Branches
                        {
                            var AllAccount = AccountData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel)
                                            .OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();//Get All Account Branches
                            int FatherLevel = Acc.AccountLevel;
                            foreach (var CurrAccLevel in AllAccount)
                            {
                                if (CurrAccLevel.AccountLevel == FatherLevel)
                                {
                                    break;
                                }
                                else
                                {

                                    var TransActionData = TranData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                    var BalanceData = TotData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                    foreach (var D in TransActionData)
                                    {
                                        TransActionPivotVM ObjData = new TransActionPivotVM();
                                        ObjData.AccountNumber = Acc.AccountNumber;
                                        ObjData.Name = "[" + Acc.AccountNumber + "]" + Acc.ArabicName;
                                        ObjData.Debit = D.Debit;
                                        ObjData.Credit = D.Credit;
                                        ObjData.NetTot = D.Debit - D.Credit;
                                        ObjData.VoucherDate = D.VoucherDate;
                                        ObjData.CompanyTransactionKindNo = D.CompanyTransactionKindNo;
                                        ObjData.TransName = D.CompanyTransactionKindNo.ToString();
                                        ObjData.sVoucherDate = D.VoucherDate.Year.ToString() + "/" + D.VoucherDate.Month.ToString("00");
                                        if (D.CompanyTransactionKindNo == 0)
                                        {
                                            var V = TransActionKind.FirstOrDefault(m => m.TransactionKindID == D.TransactionKindNo);
                                            if (V != null)
                                            {
                                                ObjData.TransName = V.ArabicName;
                                            }


                                        }
                                        else
                                        {
                                            var V = CompanyTransAction.FirstOrDefault(m => m.TransactionKindID == D.TransactionKindNo
                                              && m.CompanyTransactionKindID == D.CompanyTransactionKindNo);
                                            if (V != null)
                                            {
                                                ObjData.TransName = V.ArabicName;
                                            }


                                        }

                                        TransActionPivotVMList.Add(ObjData);
                                    }


                                }
                            }




                        }
                        else
                        {
                            var TransActionData = TranData.Where(m => m.AccountNumber == Acc.AccountNumber);
                            var BalanceData = TotData.Where(m => m.AccountNumber == Acc.AccountNumber);
                            foreach (var D in TransActionData)
                            {
                                TransActionPivotVM ObjData = new TransActionPivotVM();
                                ObjData.AccountNumber = Acc.AccountNumber;
                                ObjData.Name = "[" + Acc.AccountNumber + "]" + Acc.ArabicName;
                                ObjData.Debit = D.Debit;
                                ObjData.Credit = D.Credit;
                                ObjData.NetTot = D.Debit - D.Credit;
                                ObjData.VoucherDate = D.VoucherDate;
                                ObjData.CompanyTransactionKindNo = D.CompanyTransactionKindNo;
                                ObjData.TransName = D.CompanyTransactionKindNo.ToString();
                                ObjData.sVoucherDate = D.VoucherDate.Year.ToString() + "/" + D.VoucherDate.Month.ToString("00");
                                if (D.CompanyTransactionKindNo == 0)
                                {
                                    var V = TransActionKind.FirstOrDefault(m => m.TransactionKindID == D.TransactionKindNo);
                                    if (V != null)
                                    {
                                        ObjData.TransName = V.ArabicName;
                                    }


                                }
                                else
                                {
                                    var V = CompanyTransAction.FirstOrDefault(m => m.TransactionKindID == D.TransactionKindNo
                                      && m.CompanyTransactionKindID == D.CompanyTransactionKindNo);
                                    if (V != null)
                                    {
                                        ObjData.TransName = V.ArabicName;
                                    }


                                }
                                TransActionPivotVMList.Add(ObjData);
                            }
                        }



                    }


                }
                else

                if ((Obj.AccountLevelDropVMID > 0) && (!Obj.Detail))
                {


                    Accounts = AccountData.Where(m => m.AccountLevel == Obj.AccountLevelDropVMID).OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();

                    LessMainAccount = AccountData.Where(m => m.AccountLevel < Obj.AccountLevelDropVMID).OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();

                    IEnumerable<string> FaherAccount = LessMainAccount.Where(m => m.AccountFather != "0").Select(m => m.AccountFather);


                    foreach (var A in LessMainAccount)
                    {
                        if (AccountData.FirstOrDefault(m => m.AccountFather == A.AccountNumber) != null)
                        {
                            //do nothing
                        }
                        else

                        {
                            Accounts = Accounts.Append(A);
                        }

                    }
                    Accounts = Accounts.OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();
                    // var MainChild = AccountData.Where(m => m.AccountFather == MainAcc.AccountNumber).OrderBy(m => m.AccountNumber).OrderBy(m => m.AccountLevel).ToList();
                    foreach (var Acc in Accounts)
                    {
                        TotalDebit = 0;
                        TOTCredit = 0;
                        NetCredit = 0;
                        NetDebit = 0;
                        CreditBalance = 0;
                        DebitBalance = 0;
                        CreditTransAction = 0;
                        DebitTransAction = 0;


                        if (AccountData.FirstOrDefault(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel) != null)
                        {
                            var AllAccount = AccountData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel)
                                            .OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();
                            int FatherLevel = Acc.AccountLevel;

                            foreach (var CurrAccLevel in AllAccount)
                            {
                                if (CurrAccLevel.AccountLevel == FatherLevel)
                                {
                                    break;
                                }
                                else
                                {
                                    var TransActionData = TranData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                    var BalanceData = TotData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                    foreach (var D in TransActionData)
                                    {
                                        TransActionPivotVM ObjData = new TransActionPivotVM();
                                        ObjData.AccountNumber = Acc.AccountNumber;

                                        ObjData.Name = "[" + Acc.AccountNumber + "]" + Acc.ArabicName;
                                        ObjData.Debit = D.Debit;
                                        ObjData.Credit = D.Credit;
                                        ObjData.NetTot = D.Debit - D.Credit;
                                        ObjData.VoucherDate = D.VoucherDate;
                                        ObjData.CompanyTransactionKindNo = D.CompanyTransactionKindNo;
                                        ObjData.TransName = D.CompanyTransactionKindNo.ToString();
                                        ObjData.sVoucherDate = D.VoucherDate.Year.ToString() + "/" + D.VoucherDate.Month.ToString("00");
                                        if (D.CompanyTransactionKindNo == 0)
                                        {
                                            var V = TransActionKind.FirstOrDefault(m => m.TransactionKindID == D.TransactionKindNo);
                                            if (V != null)
                                            {
                                                ObjData.TransName = V.ArabicName;
                                            }


                                        }
                                        else
                                        {
                                            var V = CompanyTransAction.FirstOrDefault(m => m.TransactionKindID == D.TransactionKindNo
                                              && m.CompanyTransactionKindID == D.CompanyTransactionKindNo);
                                            if (V != null)
                                            {
                                                ObjData.TransName = V.ArabicName;
                                            }


                                        }
                                        TransActionPivotVMList.Add(ObjData);
                                    }
                                    //foreach (var Balance in BalanceData)
                                    //{
                                    //    CreditBalance += Balance.CreditBalance;
                                    //    DebitBalance += Balance.DebitBalance;
                                    //}
                                    //TotalDebit = DebitTransAction + DebitBalance;
                                    //TOTCredit = CreditTransAction + CreditBalance;
                                }
                            }
                        }
                        else
                        {
                            var TransActionData = TranData.Where(m => m.AccountNumber == Acc.AccountNumber);
                            var BalanceData = TotData.Where(m => m.AccountNumber == Acc.AccountNumber);
                            foreach (var D in TransActionData)
                            {
                                TransActionPivotVM ObjData = new TransActionPivotVM();
                                ObjData.AccountNumber = Acc.AccountNumber;
                                ObjData.Name = "[" + Acc.AccountNumber + "]" + Acc.ArabicName;
                                ObjData.Debit = D.Debit;
                                ObjData.Credit = D.Credit;
                                ObjData.NetTot = D.Debit - D.Credit;
                                ObjData.VoucherDate = D.VoucherDate;
                                ObjData.CompanyTransactionKindNo = D.CompanyTransactionKindNo;
                                ObjData.TransName = D.CompanyTransactionKindNo.ToString();
                                ObjData.sVoucherDate = D.VoucherDate.Year.ToString() + "/" + D.VoucherDate.Month.ToString("00");
                                if (D.CompanyTransactionKindNo == 0)
                                {
                                    var V = TransActionKind.FirstOrDefault(m => m.TransactionKindID == D.TransactionKindNo);
                                    if (V != null)
                                    {
                                        ObjData.TransName = V.ArabicName;
                                    }


                                }
                                else
                                {
                                    var V = CompanyTransAction.FirstOrDefault(m => m.TransactionKindID == D.TransactionKindNo
                                      && m.CompanyTransactionKindID == D.CompanyTransactionKindNo);
                                    if (V != null)
                                    {
                                        ObjData.TransName = V.ArabicName;
                                    }


                                }
                                TransActionPivotVMList.Add(ObjData);
                            }

                        }





                    }

                }








            }
            catch (Exception ex)
            {
                string err = ex.Message;

                TransActionPivotVMList = new List<TransActionPivotVM>();


            }
            
            return PivotGridExtension.ExportToPdf(Acc.PivotGridHelper.PivotGridHelper.Settings, TransActionPivotVMList);
        }
        [Authorize(Roles = "CoOwner,RepCustSuppBalances")]
        public ActionResult CustSuppBalances()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            int CurrYear = UserInfo.CurrYear;

            ClientSuplierFilterVM Obj = new ClientSuplierFilterVM
            {

                ToDate = DateTime.Now
            };
            return View(Obj);
        }
        [ValidateInput(false)]
        public ActionResult CustSuppGridViewPartial()
        {

            var userId = User.Identity.GetUserId();

            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            int CurrYear = UserInfo.CurrYear;
            var FromDate = "01/01/" + CurrYear;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            ViewBag.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            ClientSuplierFilterVM Obj = new ClientSuplierFilterVM();
            Obj.ToDate = DateTime.Now;
            Obj.ClientSup = 1;
            Obj.Sales = false;
            try
            {
                
                Obj.ToDate = !string.IsNullOrEmpty(Request.Params["ToDate"]) ? DateTime.Parse(Request.Params["ToDate"]) : DateTime.Now;
                Obj.ClientSup = !string.IsNullOrEmpty(Request.Params["ClientSup"]) ? int.Parse(Request.Params["ClientSup"]) : 1;
                Obj.Sales = !string.IsNullOrEmpty(Request.Params["Sales"]) ? bool.Parse(Request.Params["Sales"]) : false;

            }
            catch
            {

            }

            var AccountData = _unitOfWork.NativeSql.GetAccountsInfoByKind(UserInfo.fCompanyId,Obj.ClientSup);
            var TransData= _unitOfWork.NativeSql.TransactionByAccKind(UserInfo.fCompanyId,DateTime.Parse(FromDate), Obj.ToDate,Obj.ClientSup);
            List<CustSuppRepVM> ObjList = new List<CustSuppRepVM>();
            foreach(var Acc in AccountData)
            {
                CustSuppRepVM ObjC = new CustSuppRepVM();
                ObjC.AccountNumber = Acc.AccountNumber;
                ObjC.AccountName = Acc.AccountName;
                ObjC.AccountTypeID = Acc.AccountTypeID;
                ObjC.AccountKind = Acc.AccountKind;
                ObjC.SalesID = Acc.SalesID;
                ObjC.Email = Acc.Email;
                ObjC.Telephone = Acc.Telephone;
                ObjC.Mobile = Acc.Mobile;
                ObjC.TeleFax = Acc.TeleFax;
                if (Obj.ClientSup == 1)
                {
                    ObjC.SaleName = Acc.SaleName;
                    ObjC.CustomerArea = Acc.CustomerArea;
                    ObjC.CustomerCity = Acc.CustomerCity;
                    ObjC.StreetName = Acc.StreetName;
                    ObjC.SupplierCity = "";
                    ObjC.SupplierCountry = "";
                    ObjC.NameOfPersonInCharge = "";
                }
                else
                {
                    ObjC.SaleName = "";
                    ObjC.CustomerArea = "";
                    ObjC.CustomerCity = "";
                    ObjC.StreetName = "";
                    ObjC.SupplierCity = Acc.SupplierCity;
                    ObjC.SupplierCountry = Acc.SupplierCountry;
                    ObjC.NameOfPersonInCharge = Acc.NameOfPersonInCharge;
                }
                if ((TransData.FirstOrDefault(m => m.AccountNumber == Acc.AccountNumber) != null))
                {
                    var D = TransData.FirstOrDefault(m => m.AccountNumber == Acc.AccountNumber);
                    ObjC.Balance = (Acc.OpeningBalanceDebit + D.Debit) - (Acc.OpeningBalanceCredit + D.Credit);
                }
                ObjList.Add(ObjC);
            }


      
            return PartialView("_CustSuppGridViewPartial", ObjList);
        }
        [Authorize(Roles = "CoOwner,RepAccountingDetailsReport")]
        public ActionResult AccountingDetailsReport()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            int CurrYear = UserInfo.CurrYear;
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetAllTransactionKind(UserInfo.fCompanyId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            AccountingDetailsReportVM Obj = new AccountingDetailsReportVM
            {
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                CompanyTransactionKind = CompanyTransactionKindObj,

                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency


            };
            return View(Obj);
        }
        [HttpPost]
        public JsonResult GetAccountingDetailsReport(AccountingDetailsReportVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllAccountingDetails = _unitOfWork.NativeSql.GetAccountingDetailsReport(UserInfo.fCompanyId, Obj.DateApproval, Obj.FromDate, Obj.ToDate);
                List<AccountingDetailsReportVM> AllData = new List<AccountingDetailsReportVM>();
                List<AccountingDetailsReportVM> Ch1 = new List<AccountingDetailsReportVM>();
                List<AccountingDetailsReportVM> Ch2 = new List<AccountingDetailsReportVM>();
                List<AccountingDetailsReportVM> Ch3 = new List<AccountingDetailsReportVM>();
                List<AccountingDetailsReportVM> Ch4 = new List<AccountingDetailsReportVM>();
                if (AllAccountingDetails == null)
                {
                    return Json(new List<AccountingDetailsReportVM>(), JsonRequestBehavior.AllowGet);
                }
                if (Obj.CompanyTransactionKindNo != 0)
                {
                    AllAccountingDetails = AllAccountingDetails.Where(m => m.CompanyTransactionKindNo == Obj.CompanyTransactionKindNo).ToList();
                }
                if (Obj.AllExportTransaction)
                {
                    Ch1 = AllAccountingDetails.Where(m => m.Exported == 1).ToList();
                }
                if (Obj.AllUnExportTransaction)
                {
                    Ch2 = AllAccountingDetails.Where(m => m.Exported == 0).ToList();
                }
                if (Obj.VoucherApproval)
                {
                    Ch3 = AllAccountingDetails.Where(m => m.VHI >= Obj.FromVoucherNumber && m.VHI <= Obj.ToVoucherNumber).ToList();
                }
                if (Obj.NoteApproval)
                {
                    Ch4 = AllAccountingDetails.Where(m => m.Note.Contains(Obj.SearchNote)).ToList();
                }
                if (Ch1.Count > 0)
                {
                    foreach (var c in Ch1)
                    {
                        AllData.Add(c);
                    }
                }
                if (Ch2.Count > 0)
                {
                    foreach (var c in Ch2)
                    {
                        AllData.Add(c);
                    }
                }
                if (Ch3.Count > 0)
                {
                    foreach (var c in Ch3)
                    {
                        AllData.Add(c);
                    }
                }
                if (Ch4.Count > 0)
                {
                    foreach (var c in Ch4)
                    {
                        AllData.Add(c);
                    }
                }
                if (Obj.AllExportTransaction == false && Obj.AllUnExportTransaction == false && Obj.VoucherApproval == false && Obj.NoteApproval == false)
                {
                    return Json(AllAccountingDetails, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(AllData, JsonRequestBehavior.AllowGet);
                } 
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<AccountingDetailsReportVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public ActionResult ShowVoucherDetails(string id, int id2, int id3, int id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var AccountingDetailsReportVM = new AccountingDetailsReportVM
            {
                VoucherNumber = id,
                TransactionKindNo = id2,
                CompanyTransactionKindNo = id3,
                CompanyYear = id4,
                WorkWithCostCenter = Company.WorkWithCostCenter,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency
            };
            return PartialView(AccountingDetailsReportVM);
        }
        [HttpGet]
        public JsonResult GetVoucherDetails(string id, int id2, int id3, int id4)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var ShowVoucherDetails = _unitOfWork.NativeSql.GetVoucherDetails(UserInfo.fCompanyId, id, id2, id3, id4);
                return Json(ShowVoucherDetails, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<AccountingDetailsReportVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public JsonResult GetServiceDetails(string id, int id2, int id3, int id4)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var ShowVoucherDetails = _unitOfWork.NativeSql.GetServiceDetails(UserInfo.fCompanyId, id, id2, id3, id4);
                return Json(ShowVoucherDetails, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<AccountingDetailsReportVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [Authorize(Roles = "CoOwner,RepSalesReport")]
        public ActionResult SalesReport()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            int CurrYear = UserInfo.CurrYear;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            SalesReportVM Obj = new SalesReportVM
            {
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                SaleMan = _unitOfWork.Sale.GetAllSale(UserInfo.fCompanyId),
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency
            };
            return View(Obj);
        }
        [HttpPost]
        public JsonResult GetSalesReport(SalesReportVM Obj)
        {
            try
            {
                
                
                    var userId = User.Identity.GetUserId();
                    var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                    var AllSalesReport = _unitOfWork.NativeSql.GetSalesReport(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                    List<SalesReportVM> AllData = new List<SalesReportVM>();
                    List<SalesReportVM> Ch1 = new List<SalesReportVM>();
                    List<SalesReportVM> Ch2 = new List<SalesReportVM>();
                    List<SalesReportVM> Ch3 = new List<SalesReportVM>();
                    List<SalesReportVM> Ch4 = new List<SalesReportVM>();
                    List<SalesReportVM> Ch5 = new List<SalesReportVM>();
                    List<SalesReportVM> Ch6 = new List<SalesReportVM>();
                    List<SalesReportVM> Ch7 = new List<SalesReportVM>();
                    List<SalesReportVM> Ch8 = new List<SalesReportVM>();
                    if (AllSalesReport == null)
                    {
                        return Json(new List<SalesReportVM>(), JsonRequestBehavior.AllowGet);
                    }
                    if (!String.IsNullOrEmpty(Obj.VoucherNumber))
                    {
                        AllSalesReport = AllSalesReport.Where(m => m.VoucherNumber == Obj.VoucherNumber).ToList();
                    }
                    if (!String.IsNullOrEmpty(Obj.AccountNumber))
                    {
                        AllSalesReport = AllSalesReport.Where(m => m.AccountNumber == Obj.AccountNumber).ToList();
                    }
                    if (Obj.SaleID != 0)
                    {
                        AllSalesReport = AllSalesReport.Where(m => m.SaleID == Obj.SaleID).ToList();
                    }
                    if (Obj.ReceiptCash)
                    {
                        Ch1 = AllSalesReport.Where(m => m.TransactionKindNo == 3).ToList();
                    }
                    if (Obj.ReceiptBank)
                    {
                        Ch2 = AllSalesReport.Where(m => m.TransactionKindNo == 2).ToList();
                    }
                    if (Obj.DebitNote)
                    {
                        Ch3 = AllSalesReport.Where(m => m.TransactionKindNo == 6).ToList();
                    }
                    if (Obj.SaleService)
                    {
                        Ch4 = AllSalesReport.Where(m => m.TransactionKindNo == 10).ToList();
                    }
                    if (Obj.SaleMultiService)
                    {
                        Ch5 = AllSalesReport.Where(m => m.TransactionKindNo == 11).ToList();
                    }
                    if (Obj.ReturnService)
                    {
                        Ch6 = AllSalesReport.Where(m => m.TransactionKindNo == 19).ToList();
                    }
                    if (Obj.ReturnMultiService)
                    {
                        Ch7 = AllSalesReport.Where(m => m.TransactionKindNo == 20).ToList();
                    }
                    if (Obj.ReceiptVoucherCashMultiAccount)
                    {
                        Ch7 = AllSalesReport.Where(m => m.TransactionKindNo == 23).ToList();
                    }
                    if (Ch1.Count > 0)
                    {
                        foreach (var c in Ch1)
                        {
                            AllData.Add(c);
                        }
                    }
                    if (Ch2.Count > 0)
                    {
                        foreach (var c in Ch2)
                        {
                            AllData.Add(c);
                        }
                    }
                    if (Ch3.Count > 0)
                    {
                        foreach (var c in Ch3)
                        {
                            AllData.Add(c);
                        }
                    }
                    if (Ch4.Count > 0)
                    {
                        foreach (var c in Ch4)
                        {
                            AllData.Add(c);
                        }
                    }
                    if (Ch5.Count > 0)
                    {
                        foreach (var c in Ch5)
                        {
                            AllData.Add(c);
                        }
                    }
                    if (Ch6.Count > 0)
                    {
                        foreach (var c in Ch6)
                        {
                            AllData.Add(c);
                        }
                    }
                    if (Ch7.Count > 0)
                    {
                        foreach (var c in Ch7)
                        {
                            AllData.Add(c);
                        }
                    }
                    if (Ch8.Count > 0)
                    {
                        foreach (var c in Ch8)
                        {
                            AllData.Add(c);
                        }
                    }
                    if (Obj.ReceiptCash == false && Obj.ReceiptBank == false && Obj.DebitNote == false && Obj.SaleService == false &&
                        Obj.SaleMultiService == false && Obj.ReturnService == false && Obj.ReturnMultiService == false && Obj.ReceiptVoucherCashMultiAccount == false)
                    {
                        return Json(AllSalesReport, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(AllData, JsonRequestBehavior.AllowGet);
                    }
                

              

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<SalesReportVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [Authorize(Roles = "CoOwner,RepServiceTaxReport")]
        public ActionResult ServiceTaxReport()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            int CurrYear = UserInfo.CurrYear;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            ServiceTaxReportVM Obj = new ServiceTaxReportVM
            {
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,

                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency

            };
            return View(Obj);
        }
        [HttpPost]
        public JsonResult GetServiceTaxReport(ServiceTaxReportVM Obj)
        {
            try
            {

                
                
                    var userId = User.Identity.GetUserId();
                    var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                    var AllServiceTaxReport = _unitOfWork.NativeSql.GetServiceTaxReport(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                    List<ServiceTaxReportVM> AllData = new List<ServiceTaxReportVM>();
                    List<ServiceTaxReportVM> Ch1 = new List<ServiceTaxReportVM>();
                    List<ServiceTaxReportVM> Ch2 = new List<ServiceTaxReportVM>();
                    List<ServiceTaxReportVM> Ch3 = new List<ServiceTaxReportVM>();
                    List<ServiceTaxReportVM> Ch4 = new List<ServiceTaxReportVM>();
                    List<ServiceTaxReportVM> Ch5 = new List<ServiceTaxReportVM>();
                    List<ServiceTaxReportVM> Ch6 = new List<ServiceTaxReportVM>();
                    List<ServiceTaxReportVM> Ch7 = new List<ServiceTaxReportVM>();
                    List<ServiceTaxReportVM> Ch8 = new List<ServiceTaxReportVM>();

                    if (AllServiceTaxReport == null)
                    {
                        return Json(new List<ServiceTaxReportVM>(), JsonRequestBehavior.AllowGet);
                    }
                    if (!String.IsNullOrEmpty(Obj.AccountNumber))
                    {
                        AllServiceTaxReport = AllServiceTaxReport.Where(m => m.AccountNumber == Obj.AccountNumber).ToList();
                    }
                    if (Obj.ServiceNo != 0)
                    {
                        AllServiceTaxReport = AllServiceTaxReport.Where(m => m.ServiceNo == Obj.ServiceNo).ToList();
                    }

                    if (Obj.SaleService)
                    {
                        Ch1 = AllServiceTaxReport.Where(m => m.TransactionKindNo == 10).ToList();
                    }
                    if (Obj.SaleMultiService)
                    {
                        Ch2 = AllServiceTaxReport.Where(m => m.TransactionKindNo == 11).ToList();
                    }
                    if (Obj.ReturnService)
                    {
                        Ch3 = AllServiceTaxReport.Where(m => m.TransactionKindNo == 19).ToList();
                    }
                    if (Obj.ReturnMultiService)
                    {
                        Ch4 = AllServiceTaxReport.Where(m => m.TransactionKindNo == 20).ToList();
                    }
                    if (Obj.PurchaseService)
                    {
                        Ch5 = AllServiceTaxReport.Where(m => m.TransactionKindNo == 12).ToList();
                    }
                    if (Obj.PurchaseMultiService)
                    {
                        Ch6 = AllServiceTaxReport.Where(m => m.TransactionKindNo == 13).ToList();
                    }
                    if (Obj.ReturnPurchaseService)
                    {
                        Ch7 = AllServiceTaxReport.Where(m => m.TransactionKindNo == 21).ToList();
                    }
                    if (Obj.ReturnPurchaseMultiService)
                    {
                        Ch8 = AllServiceTaxReport.Where(m => m.TransactionKindNo == 22).ToList();
                    }
                    if (Ch1.Count > 0)
                    {
                        foreach (var c in Ch1)
                        {
                            AllData.Add(c);
                        }
                    }
                    if (Ch2.Count > 0)
                    {
                        foreach (var c in Ch2)
                        {
                            AllData.Add(c);
                        }
                    }
                    if (Ch3.Count > 0)
                    {
                        foreach (var c in Ch3)
                        {
                            AllData.Add(c);
                        }
                    }
                    if (Ch4.Count > 0)
                    {
                        foreach (var c in Ch4)
                        {
                            AllData.Add(c);
                        }
                    }
                    if (Ch5.Count > 0)
                    {
                        foreach (var c in Ch5)
                        {
                            AllData.Add(c);
                        }
                    }
                    if (Ch6.Count > 0)
                    {
                        foreach (var c in Ch6)
                        {
                            AllData.Add(c);
                        }
                    }
                    if (Ch7.Count > 0)
                    {
                        foreach (var c in Ch7)
                        {
                            AllData.Add(c);
                        }
                    }
                    if (Ch8.Count > 0)
                    {
                        foreach (var c in Ch8)
                        {
                            AllData.Add(c);
                        }
                    }
                    if (Obj.SaleService == false && Obj.SaleMultiService == false && Obj.ReturnService == false && Obj.ReturnMultiService == false &&
                        Obj.PurchaseService == false && Obj.PurchaseMultiService == false && Obj.ReturnPurchaseService == false && Obj.ReturnPurchaseMultiService == false)
                    {
                        return Json(AllServiceTaxReport, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(AllData, JsonRequestBehavior.AllowGet);
                    }
                
              
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<ServiceTaxReportVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public JsonResult GetServiceTaxReportTaxExempt(ServiceTaxReportVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllServiceTaxReport = _unitOfWork.NativeSql.GetServiceTaxReportTaxExempt(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                List<ServiceTaxReportVM> AllData = new List<ServiceTaxReportVM>();
                List<ServiceTaxReportVM> Ch1 = new List<ServiceTaxReportVM>();
                List<ServiceTaxReportVM> Ch2 = new List<ServiceTaxReportVM>();
                List<ServiceTaxReportVM> Ch3 = new List<ServiceTaxReportVM>();
                List<ServiceTaxReportVM> Ch4 = new List<ServiceTaxReportVM>();
                List<ServiceTaxReportVM> Ch5 = new List<ServiceTaxReportVM>();
                List<ServiceTaxReportVM> Ch6 = new List<ServiceTaxReportVM>();
                List<ServiceTaxReportVM> Ch7 = new List<ServiceTaxReportVM>();
                List<ServiceTaxReportVM> Ch8 = new List<ServiceTaxReportVM>();

                if (AllServiceTaxReport == null)
                {
                    return Json(new List<ServiceTaxReportVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.AccountNumber))
                {
                    AllServiceTaxReport = AllServiceTaxReport.Where(m => m.AccountNumber == Obj.AccountNumber).ToList();
                }
                //if (Obj.ServiceNo != 0)
                //{
                //    AllServiceTaxReport = AllServiceTaxReport.Where(m => m.ServiceNo == Obj.ServiceNo).ToList();
                //}
                if (Obj.SaleService)
                {
                    Ch1 = AllServiceTaxReport.Where(m => m.TransactionKindNo == 10).ToList();
                }
                if (Obj.SaleMultiService)
                {
                    Ch2 = AllServiceTaxReport.Where(m => m.TransactionKindNo == 11).ToList();
                }
                if (Obj.ReturnService)
                {
                    Ch3 = AllServiceTaxReport.Where(m => m.TransactionKindNo == 19).ToList();
                }
                if (Obj.ReturnMultiService)
                {
                    Ch4 = AllServiceTaxReport.Where(m => m.TransactionKindNo == 20).ToList();
                }
                if (Obj.PurchaseService)
                {
                    Ch5 = AllServiceTaxReport.Where(m => m.TransactionKindNo == 12).ToList();
                }
                if (Obj.PurchaseMultiService)
                {
                    Ch6 = AllServiceTaxReport.Where(m => m.TransactionKindNo == 13).ToList();
                }
                if (Obj.ReturnPurchaseService)
                {
                    Ch7 = AllServiceTaxReport.Where(m => m.TransactionKindNo == 21).ToList();
                }
                if (Obj.ReturnPurchaseMultiService)
                {
                    Ch8 = AllServiceTaxReport.Where(m => m.TransactionKindNo == 22).ToList();
                }
                if (Ch1.Count > 0)
                {
                    foreach (var c in Ch1)
                    {
                        AllData.Add(c);
                    }
                }
                if (Ch2.Count > 0)
                {
                    foreach (var c in Ch2)
                    {
                        AllData.Add(c);
                    }
                }
                if (Ch3.Count > 0)
                {
                    foreach (var c in Ch3)
                    {
                        AllData.Add(c);
                    }
                }
                if (Ch4.Count > 0)
                {
                    foreach (var c in Ch4)
                    {
                        AllData.Add(c);
                    }
                }
                if (Ch5.Count > 0)
                {
                    foreach (var c in Ch5)
                    {
                        AllData.Add(c);
                    }
                }
                if (Ch6.Count > 0)
                {
                    foreach (var c in Ch6)
                    {
                        AllData.Add(c);
                    }
                }
                if (Ch7.Count > 0)
                {
                    foreach (var c in Ch7)
                    {
                        AllData.Add(c);
                    }
                }
                if (Ch8.Count > 0)
                {
                    foreach (var c in Ch8)
                    {
                        AllData.Add(c);
                    }
                }
                if (Obj.SaleService == false && Obj.SaleMultiService == false && Obj.ReturnService == false && Obj.ReturnMultiService == false &&
                    Obj.PurchaseService == false && Obj.PurchaseMultiService == false && Obj.ReturnPurchaseService == false && Obj.ReturnPurchaseMultiService == false)
                {
                    return Json(AllServiceTaxReport, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(AllData, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<ServiceTaxReportVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public JsonResult GetServiceTaxReportDetailed(ServiceTaxReportVM Obj)
        {
            try
                {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllServiceTaxReport = _unitOfWork.NativeSql.GetServiceTaxReportDetailed(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                List<ServiceTaxReportVM> AllData = new List<ServiceTaxReportVM>();
                List<ServiceTaxReportVM> Ch1 = new List<ServiceTaxReportVM>();
                List<ServiceTaxReportVM> Ch2 = new List<ServiceTaxReportVM>();
                List<ServiceTaxReportVM> Ch3 = new List<ServiceTaxReportVM>();
                List<ServiceTaxReportVM> Ch4 = new List<ServiceTaxReportVM>();
                List<ServiceTaxReportVM> Ch5 = new List<ServiceTaxReportVM>();
                List<ServiceTaxReportVM> Ch6 = new List<ServiceTaxReportVM>();
                List<ServiceTaxReportVM> Ch7 = new List<ServiceTaxReportVM>();
                List<ServiceTaxReportVM> Ch8 = new List<ServiceTaxReportVM>();

                if (AllServiceTaxReport == null)
                {
                    return Json(new List<ServiceTaxReportVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.AccountNumber))
                {
                    AllServiceTaxReport = AllServiceTaxReport.Where(m => m.AccountNumber == Obj.AccountNumber).ToList();
                }
                if (Obj.ServiceNo != 0)
                {
                    AllServiceTaxReport = AllServiceTaxReport.Where(m => m.ServiceNo == Obj.ServiceNo).ToList();
                }
                if (Obj.SaleService)
                {
                    Ch1 = AllServiceTaxReport.Where(m => m.TransactionKindNo == 10).ToList();
                }
                if (Obj.SaleMultiService)
                {
                    Ch2 = AllServiceTaxReport.Where(m => m.TransactionKindNo == 11).ToList();
                }
                if (Obj.ReturnService)
                {
                    Ch3 = AllServiceTaxReport.Where(m => m.TransactionKindNo == 19).ToList();
                }
                if (Obj.ReturnMultiService)
                {
                    Ch4 = AllServiceTaxReport.Where(m => m.TransactionKindNo == 20).ToList();
                }
                if (Obj.PurchaseService)
                {
                    Ch5 = AllServiceTaxReport.Where(m => m.TransactionKindNo == 12).ToList();
                }
                if (Obj.PurchaseMultiService)
                {
                    Ch6 = AllServiceTaxReport.Where(m => m.TransactionKindNo == 13).ToList();
                }
                if (Obj.ReturnPurchaseService)
                {
                    Ch7 = AllServiceTaxReport.Where(m => m.TransactionKindNo == 21).ToList();
                }
                if (Obj.ReturnPurchaseMultiService)
                {
                    Ch8 = AllServiceTaxReport.Where(m => m.TransactionKindNo == 22).ToList();
                }
                if (Ch1.Count > 0)
                {
                    foreach (var c in Ch1)
                    {
                        AllData.Add(c);
                    }
                }
                if (Ch2.Count > 0)
                {
                    foreach (var c in Ch2)
                    {
                        AllData.Add(c);
                    }
                }
                if (Ch3.Count > 0)
                {
                    foreach (var c in Ch3)
                    {
                        AllData.Add(c);
                    }
                }
                if (Ch4.Count > 0)
                {
                    foreach (var c in Ch4)
                    {
                        AllData.Add(c);
                    }
                }
                if (Ch5.Count > 0)
                {
                    foreach (var c in Ch5)
                    {
                        AllData.Add(c);
                    }
                }
                if (Ch6.Count > 0)
                {
                    foreach (var c in Ch6)
                    {
                        AllData.Add(c);
                    }
                }
                if (Ch7.Count > 0)
                {
                    foreach (var c in Ch7)
                    {
                        AllData.Add(c);
                    }
                }
                if (Ch8.Count > 0)
                {
                    foreach (var c in Ch8)
                    {
                        AllData.Add(c);
                    }
                }
                if (Obj.SaleService == false && Obj.SaleMultiService == false && Obj.ReturnService == false && Obj.ReturnMultiService == false &&
                    Obj.PurchaseService == false && Obj.PurchaseMultiService == false && Obj.ReturnPurchaseService == false && Obj.ReturnPurchaseMultiService == false)
                {
                    return Json(AllServiceTaxReport, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(AllData, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<ServiceTaxReportVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public JsonResult GetServiceTaxReportDetailedTaxExempt(ServiceTaxReportVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllServiceTaxReport = _unitOfWork.NativeSql.GetServiceTaxReportDetailedTaxExempt(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                List<ServiceTaxReportVM> AllData = new List<ServiceTaxReportVM>();
                List<ServiceTaxReportVM> Ch1 = new List<ServiceTaxReportVM>();
                List<ServiceTaxReportVM> Ch2 = new List<ServiceTaxReportVM>();
                List<ServiceTaxReportVM> Ch3 = new List<ServiceTaxReportVM>();
                List<ServiceTaxReportVM> Ch4 = new List<ServiceTaxReportVM>();
                List<ServiceTaxReportVM> Ch5 = new List<ServiceTaxReportVM>();
                List<ServiceTaxReportVM> Ch6 = new List<ServiceTaxReportVM>();
                List<ServiceTaxReportVM> Ch7 = new List<ServiceTaxReportVM>();
                List<ServiceTaxReportVM> Ch8 = new List<ServiceTaxReportVM>();

                if (AllServiceTaxReport == null)
                {
                    return Json(new List<ServiceTaxReportVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.AccountNumber))
                {
                    AllServiceTaxReport = AllServiceTaxReport.Where(m => m.AccountNumber == Obj.AccountNumber).ToList();
                }
                if (Obj.ServiceNo != 0)
                {
                    AllServiceTaxReport = AllServiceTaxReport.Where(m => m.ServiceNo == Obj.ServiceNo).ToList();
                }
                if (Obj.SaleService)
                {
                    Ch1 = AllServiceTaxReport.Where(m => m.TransactionKindNo == 10).ToList();
                }
                if (Obj.SaleMultiService)
                {
                    Ch2 = AllServiceTaxReport.Where(m => m.TransactionKindNo == 11).ToList();
                }
                if (Obj.ReturnService)
                {
                    Ch3 = AllServiceTaxReport.Where(m => m.TransactionKindNo == 19).ToList();
                }
                if (Obj.ReturnMultiService)
                {
                    Ch4 = AllServiceTaxReport.Where(m => m.TransactionKindNo == 20).ToList();
                }
                if (Obj.PurchaseService)
                {
                    Ch5 = AllServiceTaxReport.Where(m => m.TransactionKindNo == 12).ToList();
                }
                if (Obj.PurchaseMultiService)
                {
                    Ch6 = AllServiceTaxReport.Where(m => m.TransactionKindNo == 13).ToList();
                }
                if (Obj.ReturnPurchaseService)
                {
                    Ch7 = AllServiceTaxReport.Where(m => m.TransactionKindNo == 21).ToList();
                }
                if (Obj.ReturnPurchaseMultiService)
                {
                    Ch8 = AllServiceTaxReport.Where(m => m.TransactionKindNo == 22).ToList();
                }
                if (Ch1.Count > 0)
                {
                    foreach (var c in Ch1)
                    {
                        AllData.Add(c);
                    }
                }
                if (Ch2.Count > 0)
                {
                    foreach (var c in Ch2)
                    {
                        AllData.Add(c);
                    }
                }
                if (Ch3.Count > 0)
                {
                    foreach (var c in Ch3)
                    {
                        AllData.Add(c);
                    }
                }
                if (Ch4.Count > 0)
                {
                    foreach (var c in Ch4)
                    {
                        AllData.Add(c);
                    }
                }
                if (Ch5.Count > 0)
                {
                    foreach (var c in Ch5)
                    {
                        AllData.Add(c);
                    }
                }
                if (Ch6.Count > 0)
                {
                    foreach (var c in Ch6)
                    {
                        AllData.Add(c);
                    }
                }
                if (Ch7.Count > 0)
                {
                    foreach (var c in Ch7)
                    {
                        AllData.Add(c);
                    }
                }
                if (Ch8.Count > 0)
                {
                    foreach (var c in Ch8)
                    {
                        AllData.Add(c);
                    }
                }
                if (Obj.SaleService == false && Obj.SaleMultiService == false && Obj.ReturnService == false && Obj.ReturnMultiService == false &&
                    Obj.PurchaseService == false && Obj.PurchaseMultiService == false && Obj.ReturnPurchaseService == false && Obj.ReturnPurchaseMultiService == false)
                {
                    return Json(AllServiceTaxReport, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(AllData, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<ServiceTaxReportVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [Authorize(Roles = "CoOwner,RepServiceReport")]
        public ActionResult ServiceReport()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            int CurrYear = UserInfo.CurrYear;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            ServiceReportVM Obj = new ServiceReportVM
            {
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                SaleMan = _unitOfWork.Sale.GetAllSale(UserInfo.fCompanyId),
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency

            };
            return View(Obj);
        }
        [HttpPost]
        public JsonResult GetServiceReport(ServiceReportVM Obj)
        {
            try
            {
                
                    var userId = User.Identity.GetUserId();
                    var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                    var AllServiceReportVM = _unitOfWork.NativeSql.GetServiceReport(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                    List<ServiceReportVM> AllData = new List<ServiceReportVM>();
                    List<ServiceReportVM> Ch1 = new List<ServiceReportVM>();
                    List<ServiceReportVM> Ch2 = new List<ServiceReportVM>();
                    List<ServiceReportVM> Ch3 = new List<ServiceReportVM>();
                    List<ServiceReportVM> Ch4 = new List<ServiceReportVM>();
                    List<ServiceReportVM> Ch5 = new List<ServiceReportVM>();
                    List<ServiceReportVM> Ch6 = new List<ServiceReportVM>();
                    List<ServiceReportVM> Ch7 = new List<ServiceReportVM>();
                    List<ServiceReportVM> Ch8 = new List<ServiceReportVM>();

                    if (AllServiceReportVM == null)
                    {
                        return Json(new List<ServiceReportVM>(), JsonRequestBehavior.AllowGet);
                    }
                    if (Obj.SaleID != 0)
                    {
                        AllServiceReportVM = AllServiceReportVM.Where(m => m.SaleID == Obj.SaleID).ToList();
                    }
                    if (Obj.BillID != 0)
                    {
                        AllServiceReportVM = AllServiceReportVM.Where(m => m.BillID == Obj.BillID).ToList();
                    }
                    if (!String.IsNullOrEmpty(Obj.AccountNumber))
                    {
                        AllServiceReportVM = AllServiceReportVM.Where(m => m.AccountNumber == Obj.AccountNumber).ToList();
                    }
                    if (Obj.ServiceNo != 0)
                    {
                        AllServiceReportVM = AllServiceReportVM.Where(m => m.ServiceNo == Obj.ServiceNo).ToList();
                    }

                    if (Obj.SaleService)
                    {
                        Ch1 = AllServiceReportVM.Where(m => m.TransactionKindNo == 10).ToList();
                    }
                    if (Obj.SaleMultiService)
                    {
                        Ch2 = AllServiceReportVM.Where(m => m.TransactionKindNo == 11).ToList();
                    }
                    if (Obj.ReturnService)
                    {
                        Ch3 = AllServiceReportVM.Where(m => m.TransactionKindNo == 19).ToList();
                    }
                    if (Obj.ReturnMultiService)
                    {
                        Ch4 = AllServiceReportVM.Where(m => m.TransactionKindNo == 20).ToList();
                    }
                    if (Obj.PurchaseService)
                    {
                        Ch5 = AllServiceReportVM.Where(m => m.TransactionKindNo == 12).ToList();
                    }
                    if (Obj.PurchaseMultiService)
                    {
                        Ch6 = AllServiceReportVM.Where(m => m.TransactionKindNo == 13).ToList();
                    }
                    if (Obj.ReturnPurchaseService)
                    {
                        Ch7 = AllServiceReportVM.Where(m => m.TransactionKindNo == 21).ToList();
                    }
                    if (Obj.ReturnPurchaseMultiService)
                    {
                        Ch8 = AllServiceReportVM.Where(m => m.TransactionKindNo == 22).ToList();
                    }
                    if (Ch1.Count > 0)
                    {
                        foreach (var c in Ch1)
                        {
                            AllData.Add(c);
                        }
                    }
                    if (Ch2.Count > 0)
                    {
                        foreach (var c in Ch2)
                        {
                            AllData.Add(c);
                        }
                    }
                    if (Ch3.Count > 0)
                    {
                        foreach (var c in Ch3)
                        {
                            AllData.Add(c);
                        }
                    }
                    if (Ch4.Count > 0)
                    {
                        foreach (var c in Ch4)
                        {
                            AllData.Add(c);
                        }
                    }
                    if (Ch5.Count > 0)
                    {
                        foreach (var c in Ch5)
                        {
                            AllData.Add(c);
                        }
                    }
                    if (Ch6.Count > 0)
                    {
                        foreach (var c in Ch6)
                        {
                            AllData.Add(c);
                        }
                    }
                    if (Ch7.Count > 0)
                    {
                        foreach (var c in Ch7)
                        {
                            AllData.Add(c);
                        }
                    }
                    if (Ch8.Count > 0)
                    {
                        foreach (var c in Ch8)
                        {
                            AllData.Add(c);
                        }
                    }
                    if (Obj.SaleService == false && Obj.SaleMultiService == false && Obj.ReturnService == false && Obj.ReturnMultiService == false &&
                        Obj.PurchaseService == false && Obj.PurchaseMultiService == false && Obj.ReturnPurchaseService == false && Obj.ReturnPurchaseMultiService == false)
                    {
                        return Json(AllServiceReportVM, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(AllData, JsonRequestBehavior.AllowGet);
                    }
                

               

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<ServiceReportVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public JsonResult GetServiceReportDetailed(ServiceReportVM Obj)
        {
            try
            {
              
                
                    var userId = User.Identity.GetUserId();
                    var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                    var AllServiceReportVM = _unitOfWork.NativeSql.GetServiceReportDetailed(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);



                    List<ServiceReportVM> AllData = new List<ServiceReportVM>();
                    List<ServiceReportVM> Ch1 = new List<ServiceReportVM>();
                    List<ServiceReportVM> Ch2 = new List<ServiceReportVM>();
                    List<ServiceReportVM> Ch3 = new List<ServiceReportVM>();
                    List<ServiceReportVM> Ch4 = new List<ServiceReportVM>();
                    List<ServiceReportVM> Ch5 = new List<ServiceReportVM>();
                    List<ServiceReportVM> Ch6 = new List<ServiceReportVM>();
                    List<ServiceReportVM> Ch7 = new List<ServiceReportVM>();
                    List<ServiceReportVM> Ch8 = new List<ServiceReportVM>();

                    if (AllServiceReportVM == null)
                    {
                        return Json(new List<ServiceReportVM>(), JsonRequestBehavior.AllowGet);
                    }
                    if (Obj.SaleID != 0)
                    {
                        AllServiceReportVM = AllServiceReportVM.Where(m => m.SaleID == Obj.SaleID).ToList();
                    }
                    if (Obj.ServiceNo != 0)
                    {
                        AllServiceReportVM = AllServiceReportVM.Where(m => m.ServiceNo == Obj.ServiceNo).ToList();
                    }
                    if (Obj.BillID != 0)
                    {
                        AllServiceReportVM = AllServiceReportVM.Where(m => m.BillID == Obj.BillID).ToList();
                    }
                    if (!String.IsNullOrEmpty(Obj.AccountNumber))
                    {
                        AllServiceReportVM = AllServiceReportVM.Where(m => m.AccountNumber == Obj.AccountNumber).ToList();
                    }
                    if (Obj.SaleService)
                    {
                        Ch1 = AllServiceReportVM.Where(m => m.TransactionKindNo == 10).ToList();
                    }
                    if (Obj.SaleMultiService)
                    {
                        Ch2 = AllServiceReportVM.Where(m => m.TransactionKindNo == 11).ToList();
                    }
                    if (Obj.ReturnService)
                    {
                        Ch3 = AllServiceReportVM.Where(m => m.TransactionKindNo == 19).ToList();
                    }
                    if (Obj.ReturnMultiService)
                    {
                        Ch4 = AllServiceReportVM.Where(m => m.TransactionKindNo == 20).ToList();
                    }
                    if (Obj.PurchaseService)
                    {
                        Ch5 = AllServiceReportVM.Where(m => m.TransactionKindNo == 12).ToList();
                    }
                    if (Obj.PurchaseMultiService)
                    {
                        Ch6 = AllServiceReportVM.Where(m => m.TransactionKindNo == 13).ToList();
                    }
                    if (Obj.ReturnPurchaseService)
                    {
                        Ch7 = AllServiceReportVM.Where(m => m.TransactionKindNo == 21).ToList();
                    }
                    if (Obj.ReturnPurchaseMultiService)
                    {
                        Ch8 = AllServiceReportVM.Where(m => m.TransactionKindNo == 22).ToList();
                    }
                    if (Ch1.Count > 0)
                    {
                        foreach (var c in Ch1)
                        {
                            AllData.Add(c);
                        }
                    }
                    if (Ch2.Count > 0)
                    {
                        foreach (var c in Ch2)
                        {
                            AllData.Add(c);
                        }
                    }
                    if (Ch3.Count > 0)
                    {
                        foreach (var c in Ch3)
                        {
                            AllData.Add(c);
                        }
                    }
                    if (Ch4.Count > 0)
                    {
                        foreach (var c in Ch4)
                        {
                            AllData.Add(c);
                        }
                    }
                    if (Ch5.Count > 0)
                    {
                        foreach (var c in Ch5)
                        {
                            AllData.Add(c);
                        }
                    }
                    if (Ch6.Count > 0)
                    {
                        foreach (var c in Ch6)
                        {
                            AllData.Add(c);
                        }
                    }
                    if (Ch7.Count > 0)
                    {
                        foreach (var c in Ch7)
                        {
                            AllData.Add(c);
                        }
                    }
                    if (Ch8.Count > 0)
                    {
                        foreach (var c in Ch8)
                        {
                            AllData.Add(c);
                        }
                    }
                    if (Obj.SaleService == false && Obj.SaleMultiService == false && Obj.ReturnService == false && Obj.ReturnMultiService == false &&
                        Obj.PurchaseService == false && Obj.PurchaseMultiService == false && Obj.ReturnPurchaseService == false && Obj.ReturnPurchaseMultiService == false)
                    {
                        return Json(AllServiceReportVM, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(AllData, JsonRequestBehavior.AllowGet);
                    }
                
                   
               
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<ServiceReportVM>(), JsonRequestBehavior.AllowGet);
            }

        }

    }
}