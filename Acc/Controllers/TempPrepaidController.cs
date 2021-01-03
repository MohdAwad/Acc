using Acc.Helpers;
using Acc.Models;
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
    public class TempPrepaidController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public TempPrepaidController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }

        [Authorize(Roles = "CoOwner,ShowTempPrepaid")]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            int year = DateTime.Now.Year;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var TempPrepaidAndRevenueReceivedFilter = new TempPrepaidAndRevenueReceivedFilterVM
            {
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                FromDateFirstPayment = DateTime.Now,
                ToDateFirstPayment = DateTime.Now,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency
            };
            return View(TempPrepaidAndRevenueReceivedFilter);
        }

        [Authorize(Roles = "CoOwner,AddTempPrepaid")]

        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var TempPrepaidAndRevenueReceivedObj = new TempPrepaidAndRevenueReceived();
            TempPrepaidAndRevenueReceivedObj.VoucherDate = DateTime.Now;
            TempPrepaidAndRevenueReceivedObj.DateFirstPayment = DateTime.Now;
            var TempPrepaidAndRevenueReceivedDetailObj = new List<TempPrepaidAndRevenueReceivedDetail>();
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetPrepaidFromTransactionKind(UserInfo.fCompanyId);
            var TransactionKindNo = (int)EnumTransKind.PrepaidExp;
            var CurrYear = UserInfo.CurrYear;
            var CompanyTransactionKindID = 0;
            TempPrepaidAndRevenueReceivedObj.VoucherNumber = _unitOfWork.TempPrepaidAndRevenueReceived.GetMaxVHByKind(UserInfo.fCompanyId, CompanyTransactionKindID, TransactionKindNo , CurrYear).ToString();
            TempPrepaidAndRevenueReceivedObj.VHI = int.Parse(TempPrepaidAndRevenueReceivedObj.VoucherNumber);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var TempPrepaidAndRevenueReceivedVM = new TempPrepaidAndRevenueReceivedVM
            {
                TempPrepaidAndRevenueReceived = TempPrepaidAndRevenueReceivedObj,
                TempPrepaidAndRevenueReceivedDetail = TempPrepaidAndRevenueReceivedDetailObj,
                CompanyTransactionKind = CompanyTransactionKindObj,
                CompanyTransactionKindID = CompanyTransactionKindID,
                WorkWithCostCenter = Company.WorkWithCostCenter,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency
            };
            return View(TempPrepaidAndRevenueReceivedVM);
        }
        [ValidateInput(false)]
        public ActionResult GridViewTempPrepaidPartial(string id, string id2, string id3 , int id4)
        {

            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
   
            if (!String.IsNullOrEmpty(id))
            {
                var TempPrepaidAndRevenueReceivedObj = _unitOfWork.NativeSql.GetTempPrepaidAndRevenueReceivedDetailsNotExport(id, int.Parse(id2), UserInfo.fCompanyId, int.Parse(id3) , id4);
                return PartialView("GridViewTempPrepaidPartial", TempPrepaidAndRevenueReceivedObj);
            }
            else
            {
                var TempPrepaidAndRevenueReceivedObj = new List<TempPrepaidAndRevenueReceivedVM>();
                return PartialView("GridViewTempPrepaidPartial", TempPrepaidAndRevenueReceivedObj);
            }

        }
        [ValidateInput(false)]
        public ActionResult GridViewTempPrepaidPartialCopy(string id, string id2, string id3 , int id4)
        {

            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            if (!String.IsNullOrEmpty(id))
            {
                var TempPrepaidAndRevenueReceivedObj = _unitOfWork.NativeSql.GetTempPrepaidAndRevenueReceivedDetails(id, int.Parse(id2), UserInfo.fCompanyId, int.Parse(id3) , id4);
                return PartialView("GridViewTempPrepaidPartialCopy", TempPrepaidAndRevenueReceivedObj);
            }
            else
            {
                var TempPrepaidAndRevenueReceivedObj = new List<TempPrepaidAndRevenueReceivedVM>();
                return PartialView("GridViewTempPrepaidPartialCopy", TempPrepaidAndRevenueReceivedObj);
            }

        }
        [HttpPost]
        public JsonResult SaveTempPrepaid(TempPrepaidAndRevenueReceivedVM ObjSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);

                var TempPrepaidAndRevenueReceivedDetail = ObjSave.TempPrepaidAndRevenueReceivedDetail;

                var TempPrepaidToSave = ObjSave.TempPrepaidAndRevenueReceived;
                TempPrepaidToSave.InsDateTime = DateTime.Now;
                TempPrepaidToSave.InsUserID = userId;
                TempPrepaidToSave.TransactionKindNo = (int)EnumTransKind.PrepaidExp;
                TempPrepaidToSave.CompanyTransactionKindNo = 0;
                TempPrepaidToSave.CompanyID = UserInfo.fCompanyId;
                TempPrepaidToSave.Total = TempPrepaidToSave.Total;
                TempPrepaidToSave.NumberOfPayments = TempPrepaidToSave.NumberOfPayments;
                TempPrepaidToSave.DateFirstPayment = TempPrepaidToSave.DateFirstPayment;
                TempPrepaidToSave.VoucherDate = TempPrepaidToSave.VoucherDate;
                TempPrepaidToSave.CompanyYear = UserInfo.CurrYear;
                TempPrepaidToSave.VoucherNumber = _unitOfWork.TempPrepaidAndRevenueReceived.GetMaxVHByKind(UserInfo.fCompanyId, TempPrepaidToSave.CompanyTransactionKindNo, TempPrepaidToSave.TransactionKindNo , TempPrepaidToSave.CompanyYear).ToString();
                TempPrepaidToSave.VHI = int.Parse(TempPrepaidToSave.VoucherNumber);
                var iRow = 0;
                foreach (var TempPrepaidDetailsToSave in TempPrepaidAndRevenueReceivedDetail)
                {
                    if (Company.WorkWithCostCenter)
                    {
                        if (!String.IsNullOrWhiteSpace(ObjSave.ExpenseAccountNumber) && !String.IsNullOrWhiteSpace(ObjSave.PrepaidExpenseAccountNumber)
                            && !String.IsNullOrWhiteSpace(ObjSave.ExpenseCostNumber) && !String.IsNullOrWhiteSpace(ObjSave.PrepaidExpenseCostNumber))
                        {
                            try
                            {
                                iRow = iRow + 1;
                                TempPrepaidDetailsToSave.CompanyID = UserInfo.fCompanyId;
                                TempPrepaidDetailsToSave.CompanyYear = TempPrepaidToSave.CompanyYear;
                                TempPrepaidDetailsToSave.CompanyTransactionKindNo = TempPrepaidToSave.CompanyTransactionKindNo;
                                TempPrepaidDetailsToSave.TransactionKindNo = TempPrepaidToSave.TransactionKindNo;
                                TempPrepaidDetailsToSave.VoucherDate = TempPrepaidToSave.VoucherDate;
                                TempPrepaidDetailsToSave.VoucherNumber = TempPrepaidToSave.VoucherNumber;
                                TempPrepaidDetailsToSave.InsUserID = TempPrepaidToSave.InsUserID;
                                TempPrepaidDetailsToSave.VHI = TempPrepaidToSave.VHI;
                                TempPrepaidDetailsToSave.InsDateTime = DateTime.Now;
                                TempPrepaidDetailsToSave.RowNumber = iRow;
                                TempPrepaidDetailsToSave.FromAccountNumber = ObjSave.ExpenseAccountNumber;
                                TempPrepaidDetailsToSave.ToAccountNumber = ObjSave.PrepaidExpenseAccountNumber;
                                TempPrepaidDetailsToSave.FromCostCenter = ObjSave.ExpenseCostNumber;
                                TempPrepaidDetailsToSave.ToCostCenter = ObjSave.PrepaidExpenseCostNumber;
                                _unitOfWork.TempPrepaidAndRevenueReceivedDetail.Add(TempPrepaidDetailsToSave);
                                _unitOfWork.Complete();
                            }
                            catch (Exception ex)
                            {
                                Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                                Msg.Code = 0;
                                return Json(Msg, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        if (!String.IsNullOrWhiteSpace(ObjSave.ExpenseAccountNumber) && !String.IsNullOrWhiteSpace(ObjSave.PrepaidExpenseAccountNumber))
                        {
                            try
                            {
                                iRow = iRow + 1;
                                TempPrepaidDetailsToSave.CompanyID = UserInfo.fCompanyId;
                                TempPrepaidDetailsToSave.CompanyYear = TempPrepaidToSave.CompanyYear;
                                TempPrepaidDetailsToSave.CompanyTransactionKindNo = TempPrepaidToSave.CompanyTransactionKindNo;
                                TempPrepaidDetailsToSave.TransactionKindNo = TempPrepaidToSave.TransactionKindNo;
                                TempPrepaidDetailsToSave.VoucherDate = TempPrepaidToSave.VoucherDate;
                                TempPrepaidDetailsToSave.VoucherNumber = TempPrepaidToSave.VoucherNumber;
                                TempPrepaidDetailsToSave.InsUserID = TempPrepaidToSave.InsUserID;
                                TempPrepaidDetailsToSave.VHI = TempPrepaidToSave.VHI;
                                TempPrepaidDetailsToSave.InsDateTime = DateTime.Now;
                                TempPrepaidDetailsToSave.RowNumber = iRow;
                                TempPrepaidDetailsToSave.FromAccountNumber = ObjSave.ExpenseAccountNumber;
                                TempPrepaidDetailsToSave.ToAccountNumber = ObjSave.PrepaidExpenseAccountNumber;
                                _unitOfWork.TempPrepaidAndRevenueReceivedDetail.Add(TempPrepaidDetailsToSave);
                                _unitOfWork.Complete();
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
                TempPrepaidToSave.RowCount = iRow;
                _unitOfWork.TempPrepaidAndRevenueReceived.Add(TempPrepaidToSave);
                _unitOfWork.Complete();
                Msg.LastID = _unitOfWork.TempPrepaidAndRevenueReceived.GetMaxVHByKind(UserInfo.fCompanyId, TempPrepaidToSave.CompanyTransactionKindNo, TempPrepaidToSave.TransactionKindNo , TempPrepaidToSave.CompanyYear).ToString(); ;
                Msg.Code = 1;
                Msg.Msg = Resources.Resource.AddedSuccessfully;


                Msg.Year = TempPrepaidToSave.CompanyYear;
                Msg.VoucherNumber = TempPrepaidToSave.VoucherNumber;
                Msg.TransactionKindNo = TempPrepaidToSave.TransactionKindNo.ToString();
                Msg.CompanyTransactionKindNo = TempPrepaidToSave.CompanyTransactionKindNo.ToString();


                return Json(Msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                Msg.Code = 0;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }

        }

        [Authorize(Roles = "CoOwner,AttachTempPrepaid")]

        public ActionResult ShowAttach(int id, string id2, string id3, string id4)
        {

            TransActionFileVM Obj = new TransActionFileVM
            {
                Year = id,
                VoucherNumber = id2,
                TransactionKindNo = id3,
                CompanyTransactionKindNo = id4
            };

            return View("ShowAttach", Obj);

        }
            
        [HttpPost]
        public JsonResult GetPrepaid(TempPrepaidAndRevenueReceivedFilterVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllTempPrepaidAndRevenueReceive = _unitOfWork.NativeSql.GetAllTempPrepaid(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate,Obj.FromDateFirstPayment,Obj.ToDateFirstPayment);
                if (AllTempPrepaidAndRevenueReceive == null)
                {
                    return Json(new List<TempPrepaidAndRevenueReceivedFilterVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.VoucherNumber))
                {
                    AllTempPrepaidAndRevenueReceive = AllTempPrepaidAndRevenueReceive.Where(m => m.VoucherNumber == Obj.VoucherNumber).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.ExpenseAccountNumber))
                {
                    AllTempPrepaidAndRevenueReceive = AllTempPrepaidAndRevenueReceive.Where(m => m.ExpenseAccountNumber == Obj.ExpenseAccountNumber).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.PrepaidExpenseAccountNumber))
                {
                    AllTempPrepaidAndRevenueReceive = AllTempPrepaidAndRevenueReceive.Where(m => m.PrepaidExpenseAccountNumber == Obj.PrepaidExpenseAccountNumber).ToList();
                }
                return Json(AllTempPrepaidAndRevenueReceive, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<TempPrepaidAndRevenueReceivedFilterVM>(), JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "CoOwner,AccumulativeTempPrepaid")]
        public ActionResult Collection()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var CurrYear = UserInfo.CurrYear ;
            DateTime now = DateTime.Now;
            var startDateInMonth = new DateTime(now.Year, now.Month, 1);
            var endDateOnMonth = startDateInMonth.AddMonths(1).AddDays(-1);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetPrepaidAndRevenuCompanyTransactionKind(UserInfo.fCompanyId);
            var TempPrepaidAndRevenueReceivedFilter = new TempPrepaidAndRevenueReceivedFilterVM
            {
                FromCollectionDate = startDateInMonth,
                ToCollectionDate = endDateOnMonth,
                VoucherDate = endDateOnMonth,
                WorkWithCostCenter = Company.WorkWithCostCenter,
                CompanyTransactionKind = CompanyTransactionKindObj,
                CompanyTransactionKindID = 1,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency
            };
            return View(TempPrepaidAndRevenueReceivedFilter);
        }
        [HttpPost]
        public JsonResult GetAllCollection(TempPrepaidAndRevenueReceivedFilterVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllTempPrepaid = _unitOfWork.NativeSql.GetAlllCollectionPrepaid(UserInfo.fCompanyId, Obj.FromCollectionDate, Obj.ToCollectionDate);
                if (AllTempPrepaid == null)
                {
                    return Json(new List<TempPrepaidAndRevenueReceivedFilterVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.ExpenseAccountNumber))
                {
                    AllTempPrepaid = AllTempPrepaid.Where(m => m.ExpenseAccountNumber == Obj.ExpenseAccountNumber).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.PrepaidExpenseAccountNumber))
                {
                    AllTempPrepaid = AllTempPrepaid.Where(m => m.PrepaidExpenseAccountNumber == Obj.PrepaidExpenseAccountNumber).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.ExpenseCostNumber))
                {
                    AllTempPrepaid = AllTempPrepaid.Where(m => m.ExpenseCostNumber == Obj.ExpenseCostNumber).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.PrepaidExpenseCostNumber))
                {
                    AllTempPrepaid = AllTempPrepaid.Where(m => m.PrepaidExpenseCostNumber == Obj.PrepaidExpenseCostNumber).ToList();
                }
                int iRow = 0;
                foreach (var iRowCount in AllTempPrepaid)
                {
                    iRowCount.iRowTable = iRow;
                    iRow = iRow + 1;
                }
                return Json(AllTempPrepaid, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<TempPrepaidAndRevenueReceivedFilterVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public JsonResult SaveCollection(TempPrepaidAndRevenueReceivedFilterVM ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                int CurrYear = UserInfo.CurrYear;
                int iRow = 0;
                double SumAmount = 0;
                int CompanyTransactionKindNo = ObjToSave.CompanyTransactionKindID;
                int TransactionKindNo = _unitOfWork.CompanyTransactionKind.GetTransKindForCompanyTransactionKind(UserInfo.fCompanyId, ObjToSave.CompanyTransactionKindID);
                var ObjComapnyTransactionKind = _unitOfWork.CompanyTransactionKind.GetCompanyTransactionKindByID(UserInfo.fCompanyId, CompanyTransactionKindNo);
                string VoucherNumber = "";
                int VHI = 0;
                if (ObjComapnyTransactionKind.AutoSerial)
                {
                    VoucherNumber = _unitOfWork.Header.GetMaxVHByKind(UserInfo.fCompanyId, CompanyTransactionKindNo, TransactionKindNo , CurrYear).ToString();
                    VHI = int.Parse(VoucherNumber);
                }
                else
                {
                    string SerialNumber = "";
                    int Serial = ObjComapnyTransactionKind.Serial;
                    string Symbol = ObjComapnyTransactionKind.Symbol;
                    DateTime dDate = ObjToSave.VoucherDate;
                    string YearNo = dDate.ToString("yy");
                    string sMonth = dDate.ToString("MM");
                    int MonthNo = Int16.Parse(sMonth);
                    int LengthLastSerial = _unitOfWork.CompanyTransactionKindMonthlySerial.GetMaxSerial(UserInfo.fCompanyId, CompanyTransactionKindNo, MonthNo).ToString().Length;
                    Serial = Serial - LengthLastSerial;
                    for (int i = 0; i <= Serial; i++)
                    {
                        if (i < Serial)
                        {
                            SerialNumber = SerialNumber + "0";
                        }
                        else if (i == Serial)
                        {
                            SerialNumber = SerialNumber + _unitOfWork.CompanyTransactionKindMonthlySerial.GetMaxSerial(UserInfo.fCompanyId, CompanyTransactionKindNo, MonthNo).ToString();
                        }
                    }
                    VoucherNumber = Symbol + YearNo + sMonth + SerialNumber;
                    VHI = _unitOfWork.CompanyTransactionKindMonthlySerial.GetMaxSerial(UserInfo.fCompanyId, CompanyTransactionKindNo, MonthNo);
                    _unitOfWork.CompanyTransactionKindMonthlySerial.Update(UserInfo.fCompanyId, CompanyTransactionKindNo, MonthNo);
                }
                foreach (var SaveDebitTransaction in ObjToSave.Transaction)
                {
                    Transaction ObjTransactionDebit = new Transaction();
                    iRow = iRow + 1;
                    ObjTransactionDebit.CompanyTransactionKindNo = CompanyTransactionKindNo;
                    ObjTransactionDebit.TransactionKindNo = TransactionKindNo;
                    ObjTransactionDebit.VoucherNumber = VoucherNumber;
                    ObjTransactionDebit.VoucherDate = ObjToSave.VoucherDate;
                    ObjTransactionDebit.VHI = VHI;
                    ObjTransactionDebit.CompanyID = UserInfo.fCompanyId;
                    ObjTransactionDebit.CompanyYear = CurrYear;
                    ObjTransactionDebit.RowNumber = iRow;
                    ObjTransactionDebit.AccountNumber = SaveDebitTransaction.ExpenseAccountNumber;
                    ObjTransactionDebit.CostCenter = SaveDebitTransaction.ExpenseCostNumber;
                    ObjTransactionDebit.Debit = SaveDebitTransaction.Amount;
                    SumAmount = SumAmount + SaveDebitTransaction.Amount;
                    ObjTransactionDebit.Credit = 0;
                    ObjTransactionDebit.DebitForeign = 0;
                    ObjTransactionDebit.CreditForeign = 0;
                    ObjTransactionDebit.CreditDebitForeign = 0;
                    ObjTransactionDebit.CreditDebitForeign = 0;
                    ObjTransactionDebit.Note = SaveDebitTransaction.Note;
                    ObjTransactionDebit.InsDateTime = DateTime.Now;
                    ObjTransactionDebit.InsUserID = userId;
                    _unitOfWork.Transaction.Add(ObjTransactionDebit);
                    _unitOfWork.Complete();
                }
                foreach (var SaveCreditTransaction in ObjToSave.Transaction)
                {
                    Transaction ObjTransactionCredit = new Transaction();
                    iRow = iRow + 1;
                    ObjTransactionCredit.CompanyTransactionKindNo = CompanyTransactionKindNo;
                    ObjTransactionCredit.TransactionKindNo = TransactionKindNo;
                    ObjTransactionCredit.VoucherNumber = VoucherNumber;
                    ObjTransactionCredit.VoucherDate = ObjToSave.VoucherDate;
                    ObjTransactionCredit.VHI = VHI;
                    ObjTransactionCredit.CompanyID = UserInfo.fCompanyId;
                    ObjTransactionCredit.CompanyYear = CurrYear;
                    ObjTransactionCredit.RowNumber = iRow;
                    ObjTransactionCredit.AccountNumber = SaveCreditTransaction.PrepaidExpenseAccountNumber;
                    ObjTransactionCredit.CostCenter = SaveCreditTransaction.PrepaidExpenseCostNumber;
                    ObjTransactionCredit.Debit = 0;
                    ObjTransactionCredit.Credit = SaveCreditTransaction.Amount;
                    ObjTransactionCredit.DebitForeign = 0;
                    ObjTransactionCredit.CreditForeign = 0;
                    ObjTransactionCredit.CreditDebitForeign = 0;
                    ObjTransactionCredit.CreditDebitForeign = 0;
                    ObjTransactionCredit.Note = SaveCreditTransaction.Note;
                    ObjTransactionCredit.InsDateTime = DateTime.Now;
                    ObjTransactionCredit.InsUserID = userId;
                    _unitOfWork.Transaction.Add(ObjTransactionCredit);
                    _unitOfWork.Complete();
                }

                var SaveHeader = new Header();
                SaveHeader.CompanyTransactionKindNo = CompanyTransactionKindNo;
                SaveHeader.TransactionKindNo = TransactionKindNo;
                SaveHeader.VoucherNumber = VoucherNumber;
                SaveHeader.VoucherDate = ObjToSave.VoucherDate;
                SaveHeader.VHI = VHI;
                SaveHeader.CompanyID = UserInfo.fCompanyId;
                SaveHeader.CompanyYear = CurrYear;
                SaveHeader.TotalDebit = SumAmount;
                SaveHeader.TotalCredit = SumAmount;
                SaveHeader.TotalDebitForeign = 0;
                SaveHeader.TotalCreditForeign = 0;
                SaveHeader.ConversionFactor = 1;
                SaveHeader.Note = "";
                SaveHeader.RowCount = iRow;
                SaveHeader.Exported = 1;
                SaveHeader.IsBill = 0;
                SaveHeader.InsDateTime = DateTime.Now;
                SaveHeader.InsUserID = userId;
                SaveHeader.FCurrencyID = 1;
                SaveHeader.SaleID = 0;
                _unitOfWork.Header.Add(SaveHeader);
                _unitOfWork.Complete();
                foreach (var UpdateTempPrepaid in ObjToSave.TempPrepaidAndRevenueReceivedDetail)
                {
                    UpdateTempPrepaid.Exported = 1;
                    UpdateTempPrepaid.CompanyID = UserInfo.fCompanyId;
                    _unitOfWork.TempPrepaidAndRevenueReceivedDetail.UpdateToExport(UpdateTempPrepaid);
                    _unitOfWork.Complete();
                }
                Msg.Code = 1;
                Msg.Msg = Resources.Resource.ExportSuccessfully;



                return Json(Msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                Msg.Code = 0;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public ActionResult Detail(string id, int id2, int id3 , int id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            var TempPrepaidObj = _unitOfWork.TempPrepaidAndRevenueReceived.GetTempPrepaidAndRevenueReceivedData(id, UserInfo.fCompanyId, id2, id3 , id4);
            var TempPrepaidDetailsObj = _unitOfWork.TempPrepaidAndRevenueReceivedDetail.GetTempPrepaidAndRevenueReceivedDetailData(id, UserInfo.fCompanyId, id2, id3, id4);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var TempPrepaidVM = new TempPrepaidAndRevenueReceivedVM { };
            TempPrepaidVM.WorkWithCostCenter = Company.WorkWithCostCenter;
            TempPrepaidVM.TempPrepaidAndRevenueReceived = TempPrepaidObj;
            TempPrepaidVM.ExpenseAccountNumber = TempPrepaidDetailsObj.FromAccountNumber;
            TempPrepaidVM.PrepaidExpenseAccountNumber = TempPrepaidDetailsObj.ToAccountNumber;
            TempPrepaidVM.ExpenseCostNumber = TempPrepaidDetailsObj.FromCostCenter;
            TempPrepaidVM.PrepaidExpenseCostNumber = TempPrepaidDetailsObj.ToCostCenter;
            TempPrepaidVM.ExpenseAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TempPrepaidVM.ExpenseAccountNumber);
            TempPrepaidVM.PrepaidExpenseAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TempPrepaidVM.PrepaidExpenseAccountNumber);
            TempPrepaidVM.ExpenseCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TempPrepaidVM.ExpenseCostNumber);
            TempPrepaidVM.PrepaidExpenseCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TempPrepaidVM.PrepaidExpenseCostNumber);
            TempPrepaidVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            TempPrepaidVM.TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency;
            return View(TempPrepaidVM);
        }
        [HttpGet]
        public JsonResult GetTempPrepaid(string id, string id2, string id3)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var TempPrepaidDetailsObj = _unitOfWork.NativeSql.GetTempPrepaidAndRevenueReceivedDetalsData(id, int.Parse(id2), UserInfo.fCompanyId, int.Parse(id3));

            if (TempPrepaidDetailsObj == null)
            {
                return Json(new TempPrepaidAndRevenueReceivedVM(), JsonRequestBehavior.AllowGet);
            }
            return Json(TempPrepaidDetailsObj, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]

        [Authorize(Roles = "CoOwner,DeleteTempPrepaid")]

        public ActionResult Delete(string id, int id2, int id3 , int id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            var TempPrepaidObj = _unitOfWork.TempPrepaidAndRevenueReceived.GetTempPrepaidAndRevenueReceivedData(id, UserInfo.fCompanyId, id2, id3 , id4);
            var TempPrepaidDetailsObj = _unitOfWork.TempPrepaidAndRevenueReceivedDetail.GetTempPrepaidAndRevenueReceivedDetailData(id, UserInfo.fCompanyId, id2, id3 , id4);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var TempPrepaidVM = new TempPrepaidAndRevenueReceivedVM { };
            TempPrepaidVM.WorkWithCostCenter = Company.WorkWithCostCenter;
            TempPrepaidVM.TempPrepaidAndRevenueReceived = TempPrepaidObj;
            TempPrepaidVM.ExpenseAccountNumber = TempPrepaidDetailsObj.FromAccountNumber;
            TempPrepaidVM.PrepaidExpenseAccountNumber = TempPrepaidDetailsObj.ToAccountNumber;
            TempPrepaidVM.ExpenseCostNumber = TempPrepaidDetailsObj.FromCostCenter;
            TempPrepaidVM.PrepaidExpenseCostNumber = TempPrepaidDetailsObj.ToCostCenter;
            TempPrepaidVM.ExpenseAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TempPrepaidVM.ExpenseAccountNumber);
            TempPrepaidVM.PrepaidExpenseAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TempPrepaidVM.PrepaidExpenseAccountNumber);
            TempPrepaidVM.ExpenseCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TempPrepaidVM.ExpenseCostNumber);
            TempPrepaidVM.PrepaidExpenseCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TempPrepaidVM.PrepaidExpenseCostNumber);
            TempPrepaidVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            TempPrepaidVM.TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency;
            return View(TempPrepaidVM);
        }
        [HttpPost]
        public JsonResult DeleteTempPrepaid(TempPrepaidAndRevenueReceivedVM ObjDelete)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                ObjDelete.CompanyID = UserInfo.fCompanyId;
                ObjDelete.TransactionKindNo = 17;
                ObjDelete.VoucherNumber = ObjDelete.VoucherNumber;
                ObjDelete.CompanyTransactionKindNo = 0;
                ObjDelete.CompanyYear = ObjDelete.CompanyYear;

                _unitOfWork.TempPrepaidAndRevenueReceived.Delete(ObjDelete);
                _unitOfWork.NativeSql.DeleteTempPrepaidAndRevenueReceivedDetails(ObjDelete.CompanyID, ObjDelete.VoucherNumber, 
                    ObjDelete.CompanyTransactionKindNo, ObjDelete.TransactionKindNo, ObjDelete.CompanyYear);

                _unitOfWork.Complete();

                Msg.Code = 1;
                Msg.Msg = Resources.Resource.DeletedSuccessfully;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                Msg.Code = 0;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]

        [Authorize(Roles = "CoOwner,UpdateTempPrepaid")]

        public ActionResult Update(string id, int id2, int id3 , int id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            var TempPrepaidObj = _unitOfWork.TempPrepaidAndRevenueReceived.GetTempPrepaidAndRevenueReceivedData(id, UserInfo.fCompanyId, id2, id3 , id4);
            var TempPrepaidDetailsObj = _unitOfWork.TempPrepaidAndRevenueReceivedDetail.GetTempPrepaidAndRevenueReceivedDetailData(id, UserInfo.fCompanyId, id2, id3 , id4);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var TempPrepaidVM = new TempPrepaidAndRevenueReceivedVM { };
            TempPrepaidVM.WorkWithCostCenter = Company.WorkWithCostCenter;
            TempPrepaidVM.TempPrepaidAndRevenueReceived = TempPrepaidObj;
            TempPrepaidVM.ExpenseAccountNumber = TempPrepaidDetailsObj.FromAccountNumber;
            TempPrepaidVM.PrepaidExpenseAccountNumber = TempPrepaidDetailsObj.ToAccountNumber;
            TempPrepaidVM.ExpenseCostNumber = TempPrepaidDetailsObj.FromCostCenter;
            TempPrepaidVM.PrepaidExpenseCostNumber = TempPrepaidDetailsObj.ToCostCenter;
            TempPrepaidVM.ExpenseAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TempPrepaidVM.ExpenseAccountNumber);
            TempPrepaidVM.PrepaidExpenseAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TempPrepaidVM.PrepaidExpenseAccountNumber);
            TempPrepaidVM.ExpenseCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TempPrepaidVM.ExpenseCostNumber);
            TempPrepaidVM.PrepaidExpenseCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TempPrepaidVM.PrepaidExpenseCostNumber);
            TempPrepaidVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            TempPrepaidVM.TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency;
            TempPrepaidVM.CompanyYear = TempPrepaidDetailsObj.CompanyYear;
            return View(TempPrepaidVM);
        }
        [HttpGet]

        [Authorize(Roles = "CoOwner,CopyTempPrepaid")]

        public ActionResult Copy(string id, int id2, int id3 , int id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            var TempPrepaidObj = _unitOfWork.TempPrepaidAndRevenueReceived.GetTempPrepaidAndRevenueReceivedData(id, UserInfo.fCompanyId, id2, id3 , id4);
            var TempPrepaidDetailsObj = _unitOfWork.TempPrepaidAndRevenueReceivedDetail.GetTempPrepaidAndRevenueReceivedDetailData(id, UserInfo.fCompanyId, id2, id3 , id4);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var TempPrepaidVM = new TempPrepaidAndRevenueReceivedVM { };
            TempPrepaidVM.WorkWithCostCenter = Company.WorkWithCostCenter;
            TempPrepaidVM.TempPrepaidAndRevenueReceived = TempPrepaidObj;
            TempPrepaidVM.ExpenseAccountNumber = TempPrepaidDetailsObj.FromAccountNumber;
            TempPrepaidVM.PrepaidExpenseAccountNumber = TempPrepaidDetailsObj.ToAccountNumber;
            TempPrepaidVM.ExpenseCostNumber = TempPrepaidDetailsObj.FromCostCenter;
            TempPrepaidVM.PrepaidExpenseCostNumber = TempPrepaidDetailsObj.ToCostCenter;
            TempPrepaidVM.ExpenseAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TempPrepaidVM.ExpenseAccountNumber);
            TempPrepaidVM.PrepaidExpenseAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TempPrepaidVM.PrepaidExpenseAccountNumber);
            TempPrepaidVM.ExpenseCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TempPrepaidVM.ExpenseCostNumber);
            TempPrepaidVM.PrepaidExpenseCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TempPrepaidVM.PrepaidExpenseCostNumber);
            TempPrepaidVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            TempPrepaidVM.TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency;
            return View(TempPrepaidVM);
        }
        [HttpPost]
        public JsonResult UpdateTempPrepaid(TempPrepaidAndRevenueReceivedVM ObjUpdate)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);

                var TempPrepaidAndRevenueReceivedDetail = ObjUpdate.TempPrepaidAndRevenueReceivedDetail;

                var TempPrepaidToUpdate = ObjUpdate.TempPrepaidAndRevenueReceived;
                TempPrepaidToUpdate.InsDateTime = DateTime.Now;
                TempPrepaidToUpdate.InsUserID = userId;
                TempPrepaidToUpdate.TransactionKindNo = (int)EnumTransKind.PrepaidExp;
                TempPrepaidToUpdate.CompanyTransactionKindNo = 0;
                TempPrepaidToUpdate.CompanyID = UserInfo.fCompanyId;
                TempPrepaidToUpdate.Total = TempPrepaidToUpdate.Total;
                TempPrepaidToUpdate.NumberOfPayments = TempPrepaidToUpdate.NumberOfPayments;
                TempPrepaidToUpdate.DateFirstPayment = TempPrepaidToUpdate.DateFirstPayment;
                TempPrepaidToUpdate.VoucherDate = TempPrepaidToUpdate.VoucherDate;
                TempPrepaidToUpdate.CompanyYear = TempPrepaidToUpdate.CompanyYear;
                TempPrepaidToUpdate.VoucherNumber = TempPrepaidToUpdate.VoucherNumber;
                TempPrepaidToUpdate.VHI = int.Parse(TempPrepaidToUpdate.VoucherNumber);
                var iRow = 0;
                _unitOfWork.NativeSql.DeleteTempPrepaidAndRevenueReceivedDetails(TempPrepaidToUpdate.CompanyID, TempPrepaidToUpdate.VoucherNumber,
                    TempPrepaidToUpdate.CompanyTransactionKindNo, TempPrepaidToUpdate.TransactionKindNo, TempPrepaidToUpdate.CompanyYear);
                foreach (var TempPrepaidDetailsToUpdate in TempPrepaidAndRevenueReceivedDetail)
                {
                    if (Company.WorkWithCostCenter)
                    {
                        if (!String.IsNullOrWhiteSpace(ObjUpdate.ExpenseAccountNumber) && !String.IsNullOrWhiteSpace(ObjUpdate.PrepaidExpenseAccountNumber)
                            && !String.IsNullOrWhiteSpace(ObjUpdate.ExpenseCostNumber) && !String.IsNullOrWhiteSpace(ObjUpdate.PrepaidExpenseCostNumber))
                        {
                            try
                            {
                                iRow = iRow + 1;
                                TempPrepaidDetailsToUpdate.CompanyID = UserInfo.fCompanyId;
                                TempPrepaidDetailsToUpdate.CompanyYear = TempPrepaidToUpdate.CompanyYear;
                                TempPrepaidDetailsToUpdate.CompanyTransactionKindNo = TempPrepaidToUpdate.CompanyTransactionKindNo;
                                TempPrepaidDetailsToUpdate.TransactionKindNo = TempPrepaidToUpdate.TransactionKindNo;
                                TempPrepaidDetailsToUpdate.VoucherDate = TempPrepaidToUpdate.VoucherDate;
                                TempPrepaidDetailsToUpdate.VoucherNumber = TempPrepaidToUpdate.VoucherNumber;
                                TempPrepaidDetailsToUpdate.InsUserID = TempPrepaidToUpdate.InsUserID;
                                TempPrepaidDetailsToUpdate.VHI = TempPrepaidToUpdate.VHI;
                                TempPrepaidDetailsToUpdate.InsDateTime = DateTime.Now;
                                TempPrepaidDetailsToUpdate.RowNumber = iRow;
                                TempPrepaidDetailsToUpdate.FromAccountNumber = ObjUpdate.ExpenseAccountNumber;
                                TempPrepaidDetailsToUpdate.ToAccountNumber = ObjUpdate.PrepaidExpenseAccountNumber;
                                TempPrepaidDetailsToUpdate.FromCostCenter = ObjUpdate.ExpenseCostNumber;
                                TempPrepaidDetailsToUpdate.ToCostCenter = ObjUpdate.PrepaidExpenseCostNumber;
                                _unitOfWork.TempPrepaidAndRevenueReceivedDetail.Add(TempPrepaidDetailsToUpdate);
                                _unitOfWork.Complete();
                            }
                            catch (Exception ex)
                            {
                                Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                                Msg.Code = 0;
                                return Json(Msg, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        if (!String.IsNullOrWhiteSpace(ObjUpdate.ExpenseAccountNumber) && !String.IsNullOrWhiteSpace(ObjUpdate.PrepaidExpenseAccountNumber))
                        {
                            try
                            {
                                iRow = iRow + 1;
                                TempPrepaidDetailsToUpdate.CompanyID = UserInfo.fCompanyId;
                                TempPrepaidDetailsToUpdate.CompanyYear = TempPrepaidToUpdate.CompanyYear;
                                TempPrepaidDetailsToUpdate.CompanyTransactionKindNo = TempPrepaidToUpdate.CompanyTransactionKindNo;
                                TempPrepaidDetailsToUpdate.TransactionKindNo = TempPrepaidToUpdate.TransactionKindNo;
                                TempPrepaidDetailsToUpdate.VoucherDate = TempPrepaidToUpdate.VoucherDate;
                                TempPrepaidDetailsToUpdate.VoucherNumber = TempPrepaidToUpdate.VoucherNumber;
                                TempPrepaidDetailsToUpdate.InsUserID = TempPrepaidToUpdate.InsUserID;
                                TempPrepaidDetailsToUpdate.VHI = TempPrepaidToUpdate.VHI;
                                TempPrepaidDetailsToUpdate.InsDateTime = DateTime.Now;
                                TempPrepaidDetailsToUpdate.RowNumber = iRow;
                                TempPrepaidDetailsToUpdate.FromAccountNumber = ObjUpdate.ExpenseAccountNumber;
                                TempPrepaidDetailsToUpdate.ToAccountNumber = ObjUpdate.PrepaidExpenseAccountNumber;
                                _unitOfWork.TempPrepaidAndRevenueReceivedDetail.Add(TempPrepaidDetailsToUpdate);
                                _unitOfWork.Complete();
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
                TempPrepaidToUpdate.RowCount = iRow;
                _unitOfWork.TempPrepaidAndRevenueReceived.Update(TempPrepaidToUpdate);
                _unitOfWork.Complete();
                Msg.Code = 1;
                Msg.Msg = Resources.Resource.UpdatedSuccessfully;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                Msg.Code = 0;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        [Authorize(Roles = "CoOwner,UpdateTempPrepaid")]
        public ActionResult UpdateTempPrepaid(string id, int id2, int id3 , int id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            var TempPrepaidObj = _unitOfWork.NativeSql.GetTempPrepaidAndRevenueReceivedData(id, UserInfo.fCompanyId, id2, id3);
            var TempPrepaidDetailsObj = _unitOfWork.TempPrepaidAndRevenueReceivedDetail.GetTempPrepaidAndRevenueReceivedDetailData(id, UserInfo.fCompanyId, id2, id3 , id4);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var TempPrepaidVM = new TempPrepaidAndRevenueReceivedVM { };
            TempPrepaidVM.WorkWithCostCenter = Company.WorkWithCostCenter;
            TempPrepaidVM.TempPrepaidAndRevenueReceived = TempPrepaidObj;
            TempPrepaidVM.ExpenseAccountNumber = TempPrepaidDetailsObj.FromAccountNumber;
            TempPrepaidVM.PrepaidExpenseAccountNumber = TempPrepaidDetailsObj.ToAccountNumber;
            TempPrepaidVM.ExpenseCostNumber = TempPrepaidDetailsObj.FromCostCenter;
            TempPrepaidVM.PrepaidExpenseCostNumber = TempPrepaidDetailsObj.ToCostCenter;
            TempPrepaidVM.ExpenseAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TempPrepaidVM.ExpenseAccountNumber);
            TempPrepaidVM.PrepaidExpenseAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TempPrepaidVM.PrepaidExpenseAccountNumber);
            TempPrepaidVM.ExpenseCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TempPrepaidVM.ExpenseCostNumber);
            TempPrepaidVM.PrepaidExpenseCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TempPrepaidVM.PrepaidExpenseCostNumber);
            TempPrepaidVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            TempPrepaidVM.TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency;
            TempPrepaidVM.CompanyYear = TempPrepaidDetailsObj.CompanyYear;
            return View(TempPrepaidVM);
        }
        [HttpPost]
        public JsonResult UpdateTempPrepaidWithExport(TempPrepaidAndRevenueReceivedVM ObjUpdate)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                var TempPrepaidAndRevenueReceivedDetail = ObjUpdate.TempPrepaidAndRevenueReceivedDetail;
                var TempPrepaidToUpdate = ObjUpdate.TempPrepaidAndRevenueReceived;
                TempPrepaidToUpdate.InsDateTime = DateTime.Now;
                TempPrepaidToUpdate.InsUserID = userId;
                TempPrepaidToUpdate.CompanyYear = TempPrepaidToUpdate.CompanyYear;
                TempPrepaidToUpdate.VoucherNumber = TempPrepaidToUpdate.VoucherNumber;
                TempPrepaidToUpdate.TransactionKindNo = (int)EnumTransKind.PrepaidExp;
                TempPrepaidToUpdate.CompanyTransactionKindNo = 0;
                TempPrepaidToUpdate.CompanyID = UserInfo.fCompanyId;
                int NumberOfPaymentsExported = _unitOfWork.NativeSql.GetCountNumberOfPaymnetTempPrepaidAndRevenueReceived(UserInfo.fCompanyId, TempPrepaidToUpdate.VoucherNumber,
                    TempPrepaidToUpdate.CompanyTransactionKindNo, TempPrepaidToUpdate.TransactionKindNo, TempPrepaidToUpdate.CompanyYear);
                double TotalExported = _unitOfWork.NativeSql.GetSumTotalTempPrepaidAndRevenueReceived(UserInfo.fCompanyId, TempPrepaidToUpdate.VoucherNumber,
                    TempPrepaidToUpdate.CompanyTransactionKindNo, TempPrepaidToUpdate.TransactionKindNo, TempPrepaidToUpdate.CompanyYear);
                TempPrepaidToUpdate.Total = TempPrepaidToUpdate.Total + TotalExported;
                TempPrepaidToUpdate.NumberOfPayments = TempPrepaidToUpdate.NumberOfPayments + NumberOfPaymentsExported;
                TempPrepaidToUpdate.DateFirstPayment = TempPrepaidToUpdate.DateFirstPayment;
                TempPrepaidToUpdate.VoucherDate = TempPrepaidToUpdate.VoucherDate;
                TempPrepaidToUpdate.VHI = int.Parse(TempPrepaidToUpdate.VoucherNumber);
                var iRow = 0;
                _unitOfWork.NativeSql.DeleteTempPrepaidAndRevenueReceivedDetailsWithoutExport(TempPrepaidToUpdate.CompanyID, TempPrepaidToUpdate.VoucherNumber,
                    TempPrepaidToUpdate.CompanyTransactionKindNo, TempPrepaidToUpdate.TransactionKindNo, TempPrepaidToUpdate.CompanyYear);
                var UpdateRowNumber = _unitOfWork.NativeSql.GetRowNumberTempPrepaidAndRevenueReceived(TempPrepaidToUpdate.VoucherNumber, UserInfo.fCompanyId,
                    TempPrepaidToUpdate.CompanyTransactionKindNo, TempPrepaidToUpdate.TransactionKindNo, TempPrepaidToUpdate.CompanyYear);
                _unitOfWork.NativeSql.DeleteTempPrepaidAndRevenueReceivedDetails(TempPrepaidToUpdate.CompanyID, TempPrepaidToUpdate.VoucherNumber,
                    TempPrepaidToUpdate.CompanyTransactionKindNo, TempPrepaidToUpdate.TransactionKindNo, TempPrepaidToUpdate.CompanyYear);
                foreach (var TempPrepaidDetailsToUpdate in UpdateRowNumber)
                {
                    if (Company.WorkWithCostCenter)
                    {
                        if (!String.IsNullOrWhiteSpace(ObjUpdate.ExpenseAccountNumber) && !String.IsNullOrWhiteSpace(ObjUpdate.PrepaidExpenseAccountNumber)
                            && !String.IsNullOrWhiteSpace(ObjUpdate.ExpenseCostNumber) && !String.IsNullOrWhiteSpace(ObjUpdate.PrepaidExpenseCostNumber))
                        {
                            try
                            {
                                iRow = iRow + 1;
                                TempPrepaidDetailsToUpdate.CompanyID = UserInfo.fCompanyId;
                                TempPrepaidDetailsToUpdate.CompanyYear = TempPrepaidToUpdate.CompanyYear;
                                TempPrepaidDetailsToUpdate.CompanyTransactionKindNo = TempPrepaidToUpdate.CompanyTransactionKindNo;
                                TempPrepaidDetailsToUpdate.TransactionKindNo = TempPrepaidToUpdate.TransactionKindNo;
                                TempPrepaidDetailsToUpdate.VoucherDate = TempPrepaidToUpdate.VoucherDate;
                                TempPrepaidDetailsToUpdate.VoucherNumber = TempPrepaidToUpdate.VoucherNumber;
                                TempPrepaidDetailsToUpdate.InsUserID = TempPrepaidToUpdate.InsUserID;
                                TempPrepaidDetailsToUpdate.VHI = TempPrepaidToUpdate.VHI;
                                TempPrepaidDetailsToUpdate.InsDateTime = DateTime.Now;
                                TempPrepaidDetailsToUpdate.RowNumber = iRow;
                                TempPrepaidDetailsToUpdate.FromAccountNumber = ObjUpdate.ExpenseAccountNumber;
                                TempPrepaidDetailsToUpdate.ToAccountNumber = ObjUpdate.PrepaidExpenseAccountNumber;
                                TempPrepaidDetailsToUpdate.FromCostCenter = ObjUpdate.ExpenseCostNumber;
                                TempPrepaidDetailsToUpdate.ToCostCenter = ObjUpdate.PrepaidExpenseCostNumber;
                                _unitOfWork.TempPrepaidAndRevenueReceivedDetail.Add(TempPrepaidDetailsToUpdate);
                                _unitOfWork.Complete();
                            }
                            catch (Exception ex)
                            {
                                Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                                Msg.Code = 0;
                                return Json(Msg, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        if (!String.IsNullOrWhiteSpace(ObjUpdate.ExpenseAccountNumber) && !String.IsNullOrWhiteSpace(ObjUpdate.PrepaidExpenseAccountNumber))
                        {
                            try
                            {
                                iRow = iRow + 1;
                                TempPrepaidDetailsToUpdate.CompanyID = UserInfo.fCompanyId;
                                TempPrepaidDetailsToUpdate.CompanyYear = TempPrepaidToUpdate.CompanyYear;
                                TempPrepaidDetailsToUpdate.CompanyTransactionKindNo = TempPrepaidToUpdate.CompanyTransactionKindNo;
                                TempPrepaidDetailsToUpdate.TransactionKindNo = TempPrepaidToUpdate.TransactionKindNo;
                                TempPrepaidDetailsToUpdate.VoucherDate = TempPrepaidToUpdate.VoucherDate;
                                TempPrepaidDetailsToUpdate.VoucherNumber = TempPrepaidToUpdate.VoucherNumber;
                                TempPrepaidDetailsToUpdate.InsUserID = TempPrepaidToUpdate.InsUserID;
                                TempPrepaidDetailsToUpdate.VHI = TempPrepaidToUpdate.VHI;
                                TempPrepaidDetailsToUpdate.InsDateTime = DateTime.Now;
                                TempPrepaidDetailsToUpdate.RowNumber = iRow;
                                TempPrepaidDetailsToUpdate.FromAccountNumber = ObjUpdate.ExpenseAccountNumber;
                                TempPrepaidDetailsToUpdate.ToAccountNumber = ObjUpdate.PrepaidExpenseAccountNumber;
                                _unitOfWork.TempPrepaidAndRevenueReceivedDetail.Add(TempPrepaidDetailsToUpdate);
                                _unitOfWork.Complete();
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
                foreach (var TempPrepaidDetailsToUpdate in TempPrepaidAndRevenueReceivedDetail)
                {
                    if (Company.WorkWithCostCenter)
                    {
                        if (!String.IsNullOrWhiteSpace(ObjUpdate.ExpenseAccountNumber) && !String.IsNullOrWhiteSpace(ObjUpdate.PrepaidExpenseAccountNumber)
                            && !String.IsNullOrWhiteSpace(ObjUpdate.ExpenseCostNumber) && !String.IsNullOrWhiteSpace(ObjUpdate.PrepaidExpenseCostNumber))
                        {
                            try
                            {
                                iRow = iRow + 1;
                                TempPrepaidDetailsToUpdate.CompanyID = UserInfo.fCompanyId;
                                TempPrepaidDetailsToUpdate.CompanyYear = TempPrepaidToUpdate.CompanyYear;
                                TempPrepaidDetailsToUpdate.CompanyTransactionKindNo = TempPrepaidToUpdate.CompanyTransactionKindNo;
                                TempPrepaidDetailsToUpdate.TransactionKindNo = TempPrepaidToUpdate.TransactionKindNo;
                                TempPrepaidDetailsToUpdate.VoucherDate = TempPrepaidToUpdate.VoucherDate;
                                TempPrepaidDetailsToUpdate.VoucherNumber = TempPrepaidToUpdate.VoucherNumber;
                                TempPrepaidDetailsToUpdate.InsUserID = TempPrepaidToUpdate.InsUserID;
                                TempPrepaidDetailsToUpdate.VHI = TempPrepaidToUpdate.VHI;
                                TempPrepaidDetailsToUpdate.InsDateTime = DateTime.Now;
                                TempPrepaidDetailsToUpdate.RowNumber = iRow;
                                TempPrepaidDetailsToUpdate.FromAccountNumber = ObjUpdate.ExpenseAccountNumber;
                                TempPrepaidDetailsToUpdate.ToAccountNumber = ObjUpdate.PrepaidExpenseAccountNumber;
                                TempPrepaidDetailsToUpdate.FromCostCenter = ObjUpdate.ExpenseCostNumber;
                                TempPrepaidDetailsToUpdate.ToCostCenter = ObjUpdate.PrepaidExpenseCostNumber;
                                TempPrepaidDetailsToUpdate.CollectionDate = DateTime.Parse(TempPrepaidDetailsToUpdate.CollectionDate.ToString("dd/MM/yyyy"));
                                _unitOfWork.TempPrepaidAndRevenueReceivedDetail.Add(TempPrepaidDetailsToUpdate);
                                _unitOfWork.Complete();
                            }
                            catch (Exception ex)
                            {
                                Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                                Msg.Code = 0;
                                return Json(Msg, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        if (!String.IsNullOrWhiteSpace(ObjUpdate.ExpenseAccountNumber) && !String.IsNullOrWhiteSpace(ObjUpdate.PrepaidExpenseAccountNumber))
                        {
                            try
                            {
                                iRow = iRow + 1;
                                TempPrepaidDetailsToUpdate.CompanyID = UserInfo.fCompanyId;
                                TempPrepaidDetailsToUpdate.CompanyYear = TempPrepaidToUpdate.CompanyYear;
                                TempPrepaidDetailsToUpdate.CompanyTransactionKindNo = TempPrepaidToUpdate.CompanyTransactionKindNo;
                                TempPrepaidDetailsToUpdate.TransactionKindNo = TempPrepaidToUpdate.TransactionKindNo;
                                TempPrepaidDetailsToUpdate.VoucherDate = TempPrepaidToUpdate.VoucherDate;
                                TempPrepaidDetailsToUpdate.VoucherNumber = TempPrepaidToUpdate.VoucherNumber;
                                TempPrepaidDetailsToUpdate.InsUserID = TempPrepaidToUpdate.InsUserID;
                                TempPrepaidDetailsToUpdate.VHI = TempPrepaidToUpdate.VHI;
                                TempPrepaidDetailsToUpdate.InsDateTime = DateTime.Now;
                                TempPrepaidDetailsToUpdate.RowNumber = iRow;
                                TempPrepaidDetailsToUpdate.FromAccountNumber = ObjUpdate.ExpenseAccountNumber;
                                TempPrepaidDetailsToUpdate.ToAccountNumber = ObjUpdate.PrepaidExpenseAccountNumber;
                                TempPrepaidDetailsToUpdate.CollectionDate = DateTime.Parse(TempPrepaidDetailsToUpdate.CollectionDate.ToString("dd/MM/yyyy"));
                                _unitOfWork.TempPrepaidAndRevenueReceivedDetail.Add(TempPrepaidDetailsToUpdate);
                                _unitOfWork.Complete();
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
                TempPrepaidToUpdate.RowCount = iRow;
                _unitOfWork.TempPrepaidAndRevenueReceived.Update(TempPrepaidToUpdate);
                _unitOfWork.Complete();
                Msg.Code = 1;
                Msg.Msg = Resources.Resource.UpdatedSuccessfully;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                Msg.Code = 0;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }

        }
        [Authorize(Roles = "CoOwner,RepTempPrepaidReport")]

        public ActionResult TempPrepaidReport()
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            int CurrYear = UserInfo.CurrYear;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var TempPrepaidAndRevenueReceivedVM = new TempPrepaidAndRevenueReceivedVM
            {
                FromCollectionDate = DateTime.Now,
                ToCollectionDate = DateTime.Now,
                WorkWithCostCenter = Company.WorkWithCostCenter,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency
        };
            return View(TempPrepaidAndRevenueReceivedVM);
        }
        [HttpPost]
        public JsonResult GetTempPrepaidReport(TempPrepaidAndRevenueReceivedVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllTempPrepaid = _unitOfWork.NativeSql.GetTempPrepaidReport(UserInfo.fCompanyId, Obj.All, Obj.Collector, Obj.NotCollected, Obj.FromCollectionDate, Obj.ToCollectionDate);
                if (AllTempPrepaid == null)
                {
                    return Json(new List<TempPrepaidAndRevenueReceivedVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.ExpenseAccountNumber))
                {
                    AllTempPrepaid = AllTempPrepaid.Where(m => m.ExpenseAccountNumber == Obj.ExpenseAccountNumber).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.PrepaidExpenseAccountNumber))
                {
                    AllTempPrepaid = AllTempPrepaid.Where(m => m.PrepaidExpenseAccountNumber == Obj.PrepaidExpenseAccountNumber).ToList();
                }
                return Json(AllTempPrepaid, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<TempPrepaidAndRevenueReceivedVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public JsonResult GetMaxVHIForAcc(int id , int id2)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var VHI = _unitOfWork.TempPrepaidAndRevenueReceived.GetMaxVHByKind(UserInfo.fCompanyId, id, 17 , id2).ToString();
            return Json(VHI, JsonRequestBehavior.AllowGet);
        }

    }
}