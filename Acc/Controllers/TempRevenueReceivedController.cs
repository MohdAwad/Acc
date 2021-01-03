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
    public class TempRevenueReceivedController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public TempRevenueReceivedController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }

        [Authorize(Roles = "CoOwner,ShowTempRevenueReceived")]
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
                WorkWithCostCenter = Company.WorkWithCostCenter,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency
            };
            return View(TempPrepaidAndRevenueReceivedFilter);
        }

        [Authorize(Roles = "CoOwner,AddTempRevenueReceived")]
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var TempPrepaidAndRevenueReceivedObj = new TempPrepaidAndRevenueReceived();
            TempPrepaidAndRevenueReceivedObj.VoucherDate = DateTime.Now;
            TempPrepaidAndRevenueReceivedObj.DateFirstPayment = DateTime.Now;
            var TempPrepaidAndRevenueReceivedDetailObj = new List<TempPrepaidAndRevenueReceivedDetail>();
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetRevenueFromTransactionKind(UserInfo.fCompanyId);
            var TransactionKindNo = (int)EnumTransKind.RevenueReceived;
            var CompanyYear = UserInfo.CurrYear;
            var CompanyTransactionKindID = 0;
            TempPrepaidAndRevenueReceivedObj.VoucherNumber = _unitOfWork.TempPrepaidAndRevenueReceived.GetMaxVHByKind(UserInfo.fCompanyId, CompanyTransactionKindID, TransactionKindNo ,CompanyYear).ToString();
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
        public ActionResult GridViewTempRevenuePartial(string id, string id2, string id3 , int id4)
        {

            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            if (!String.IsNullOrEmpty(id))
            {
                var TempPrepaidAndRevenueReceivedObj = _unitOfWork.NativeSql.GetTempPrepaidAndRevenueReceivedDetailsNotExport(id, int.Parse(id2), UserInfo.fCompanyId, int.Parse(id3) , id4);
                return PartialView("GridViewTempRevenuePartial", TempPrepaidAndRevenueReceivedObj);
            }
            else
            {
                var TempPrepaidAndRevenueReceivedObj = new List<TempPrepaidAndRevenueReceivedVM>();
                return PartialView("GridViewTempRevenuePartial", TempPrepaidAndRevenueReceivedObj);
            }

        }
        [ValidateInput(false)]
        public ActionResult GridViewTempRevenuePartialCopy(string id, string id2, string id3 ,int id4)
        {

            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            if (!String.IsNullOrEmpty(id))
            {
                var TempPrepaidAndRevenueReceivedObj = _unitOfWork.NativeSql.GetTempPrepaidAndRevenueReceivedDetails(id, int.Parse(id2), UserInfo.fCompanyId, int.Parse(id3), id4);
                return PartialView("GridViewTempRevenuePartialCopy", TempPrepaidAndRevenueReceivedObj);
            }
            else
            {
                var TempPrepaidAndRevenueReceivedObj = new List<TempPrepaidAndRevenueReceivedVM>();
                return PartialView("GridViewTempRevenuePartialCopy", TempPrepaidAndRevenueReceivedObj);
            }

        }
        [HttpPost]
        public JsonResult SaveTempRevenue(TempPrepaidAndRevenueReceivedVM ObjSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);

                var TempPrepaidAndRevenueReceivedDetail = ObjSave.TempPrepaidAndRevenueReceivedDetail;

                var TempRevenueToSave = ObjSave.TempPrepaidAndRevenueReceived;
                TempRevenueToSave.InsDateTime = DateTime.Now;
                TempRevenueToSave.InsUserID = userId;
                TempRevenueToSave.TransactionKindNo = (int)EnumTransKind.RevenueReceived;
                TempRevenueToSave.CompanyTransactionKindNo = 0;
                TempRevenueToSave.CompanyID = UserInfo.fCompanyId;
                TempRevenueToSave.Total = TempRevenueToSave.Total;
                TempRevenueToSave.NumberOfPayments = TempRevenueToSave.NumberOfPayments;
                TempRevenueToSave.DateFirstPayment = TempRevenueToSave.DateFirstPayment;
                TempRevenueToSave.VoucherDate = TempRevenueToSave.VoucherDate;
                TempRevenueToSave.CompanyYear = UserInfo.CurrYear;
                TempRevenueToSave.VoucherNumber = _unitOfWork.TempPrepaidAndRevenueReceived.GetMaxVHByKind(UserInfo.fCompanyId, TempRevenueToSave.CompanyTransactionKindNo, TempRevenueToSave.TransactionKindNo , TempRevenueToSave.CompanyYear).ToString();
                TempRevenueToSave.VHI = int.Parse(TempRevenueToSave.VoucherNumber);
                var iRow = 0;
                foreach (var TempRevenueDetailsToSave in TempPrepaidAndRevenueReceivedDetail)
                {
                    if (Company.WorkWithCostCenter)
                    {
                        if (!String.IsNullOrWhiteSpace(ObjSave.RevenueAccountNumber) && !String.IsNullOrWhiteSpace(ObjSave.RevenueReceivedAccountNumber)
                            && !String.IsNullOrWhiteSpace(ObjSave.RevenueReceivedCostNumber) && !String.IsNullOrWhiteSpace(ObjSave.RevenueCostNumber))
                        {
                            try
                            {
                                iRow = iRow + 1;
                                TempRevenueDetailsToSave.CompanyID = UserInfo.fCompanyId;
                                TempRevenueDetailsToSave.CompanyYear = TempRevenueToSave.CompanyYear;
                                TempRevenueDetailsToSave.CompanyTransactionKindNo = TempRevenueToSave.CompanyTransactionKindNo;
                                TempRevenueDetailsToSave.TransactionKindNo = TempRevenueToSave.TransactionKindNo;
                                TempRevenueDetailsToSave.VoucherDate = TempRevenueToSave.VoucherDate;
                                TempRevenueDetailsToSave.VoucherNumber = TempRevenueToSave.VoucherNumber;
                                TempRevenueDetailsToSave.InsUserID = TempRevenueToSave.InsUserID;
                                TempRevenueDetailsToSave.VHI = TempRevenueToSave.VHI;
                                TempRevenueDetailsToSave.InsDateTime = DateTime.Now;
                                TempRevenueDetailsToSave.RowNumber = iRow;
                                TempRevenueDetailsToSave.FromAccountNumber = ObjSave.RevenueReceivedAccountNumber;
                                TempRevenueDetailsToSave.ToAccountNumber = ObjSave.RevenueAccountNumber;
                                TempRevenueDetailsToSave.FromCostCenter = ObjSave.RevenueReceivedCostNumber;
                                TempRevenueDetailsToSave.ToCostCenter = ObjSave.RevenueCostNumber;
                                _unitOfWork.TempPrepaidAndRevenueReceivedDetail.Add(TempRevenueDetailsToSave);
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
                        if (!String.IsNullOrWhiteSpace(ObjSave.RevenueAccountNumber) && !String.IsNullOrWhiteSpace(ObjSave.RevenueReceivedAccountNumber))
                        {
                            try
                            {
                                iRow = iRow + 1;
                                TempRevenueDetailsToSave.CompanyID = UserInfo.fCompanyId;
                                TempRevenueDetailsToSave.CompanyYear = TempRevenueToSave.CompanyYear;
                                TempRevenueDetailsToSave.CompanyTransactionKindNo = TempRevenueToSave.CompanyTransactionKindNo;
                                TempRevenueDetailsToSave.TransactionKindNo = TempRevenueToSave.TransactionKindNo;
                                TempRevenueDetailsToSave.VoucherDate = TempRevenueToSave.VoucherDate;
                                TempRevenueDetailsToSave.VoucherNumber = TempRevenueToSave.VoucherNumber;
                                TempRevenueDetailsToSave.InsUserID = TempRevenueToSave.InsUserID;
                                TempRevenueDetailsToSave.VHI = TempRevenueToSave.VHI;
                                TempRevenueDetailsToSave.InsDateTime = DateTime.Now;
                                TempRevenueDetailsToSave.RowNumber = iRow;
                                TempRevenueDetailsToSave.FromAccountNumber = ObjSave.RevenueReceivedAccountNumber;
                                TempRevenueDetailsToSave.ToAccountNumber = ObjSave.RevenueAccountNumber;
                                _unitOfWork.TempPrepaidAndRevenueReceivedDetail.Add(TempRevenueDetailsToSave);
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
                TempRevenueToSave.RowCount = iRow;
                _unitOfWork.TempPrepaidAndRevenueReceived.Add(TempRevenueToSave);
                _unitOfWork.Complete();
                Msg.LastID = _unitOfWork.TempPrepaidAndRevenueReceived.GetMaxVHByKind(UserInfo.fCompanyId, TempRevenueToSave.CompanyTransactionKindNo, TempRevenueToSave.TransactionKindNo ,TempRevenueToSave.CompanyYear).ToString(); ;
                Msg.Code = 1;
                Msg.Msg = Resources.Resource.AddedSuccessfully;

                Msg.Year = TempRevenueToSave.CompanyYear;
                Msg.VoucherNumber = TempRevenueToSave.VoucherNumber;
                Msg.TransactionKindNo = TempRevenueToSave.TransactionKindNo.ToString();
                Msg.CompanyTransactionKindNo = TempRevenueToSave.CompanyTransactionKindNo.ToString();

                return Json(Msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                Msg.Code = 0;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }

        }



        [Authorize(Roles = "CoOwner,AattchTempRevenueReceived")]

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
        public JsonResult GetRevenue(TempPrepaidAndRevenueReceivedFilterVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllTempPrepaidAndRevenueReceive = _unitOfWork.NativeSql.GetAllTempRevenue(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate, Obj.FromDateFirstPayment, Obj.ToDateFirstPayment);
                if (AllTempPrepaidAndRevenueReceive == null)
                {
                    return Json(new List<TempPrepaidAndRevenueReceivedFilterVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.VoucherNumber))
                {
                    AllTempPrepaidAndRevenueReceive = AllTempPrepaidAndRevenueReceive.Where(m => m.VoucherNumber == Obj.VoucherNumber).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.RevenueReceivedAccountNumber))
                {
                    AllTempPrepaidAndRevenueReceive = AllTempPrepaidAndRevenueReceive.Where(m => m.RevenueReceivedAccountNumber == Obj.RevenueReceivedAccountNumber).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.RevenueAccountNumber))
                {
                    AllTempPrepaidAndRevenueReceive = AllTempPrepaidAndRevenueReceive.Where(m => m.RevenueAccountNumber == Obj.RevenueAccountNumber).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.RevenueReceivedCostNumber))
                {
                    AllTempPrepaidAndRevenueReceive = AllTempPrepaidAndRevenueReceive.Where(m => m.RevenueReceivedCostNumber == Obj.RevenueReceivedCostNumber).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.RevenueCostNumber))
                {
                    AllTempPrepaidAndRevenueReceive = AllTempPrepaidAndRevenueReceive.Where(m => m.RevenueCostNumber == Obj.RevenueCostNumber).ToList();
                }
                return Json(AllTempPrepaidAndRevenueReceive, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<TempPrepaidAndRevenueReceivedFilterVM>(), JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "CoOwner,AccumulativeTempRevenueReceived")]

        public ActionResult Collection()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            int CurrYear = UserInfo.CurrYear;
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
                var AllTempPrepaid = _unitOfWork.NativeSql.GetAlllCollectionRevenue(UserInfo.fCompanyId, Obj.FromCollectionDate, Obj.ToCollectionDate);
                if (AllTempPrepaid == null)
                {
                    return Json(new List<TempPrepaidAndRevenueReceivedFilterVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.RevenueReceivedAccountNumber))
                {
                    AllTempPrepaid = AllTempPrepaid.Where(m => m.RevenueReceivedAccountNumber == Obj.RevenueReceivedAccountNumber).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.RevenueAccountNumber))
                {
                    AllTempPrepaid = AllTempPrepaid.Where(m => m.RevenueAccountNumber == Obj.RevenueAccountNumber).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.RevenueReceivedCostNumber))
                {
                    AllTempPrepaid = AllTempPrepaid.Where(m => m.RevenueReceivedCostNumber == Obj.RevenueReceivedCostNumber).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.RevenueCostNumber))
                {
                    AllTempPrepaid = AllTempPrepaid.Where(m => m.RevenueCostNumber == Obj.RevenueCostNumber).ToList();
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
                    VoucherNumber = _unitOfWork.Header.GetMaxVHByKind(UserInfo.fCompanyId, CompanyTransactionKindNo, TransactionKindNo, CurrYear).ToString();
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
                    ObjTransactionDebit.AccountNumber = SaveDebitTransaction.RevenueReceivedAccountNumber;
                    ObjTransactionDebit.CostCenter = SaveDebitTransaction.RevenueReceivedCostNumber;
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
                    ObjTransactionCredit.AccountNumber = SaveCreditTransaction.RevenueAccountNumber;
                    ObjTransactionCredit.CostCenter = SaveCreditTransaction.RevenueCostNumber;
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

            var TempRevenueObj = _unitOfWork.TempPrepaidAndRevenueReceived.GetTempPrepaidAndRevenueReceivedData(id, UserInfo.fCompanyId, id2, id3 , id4);
            var TempRevenueDetailsObj = _unitOfWork.TempPrepaidAndRevenueReceivedDetail.GetTempPrepaidAndRevenueReceivedDetailData(id, UserInfo.fCompanyId, id2, id3 , id4);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var TempRevenueVM = new TempPrepaidAndRevenueReceivedVM { };
            TempRevenueVM.WorkWithCostCenter = Company.WorkWithCostCenter;
            TempRevenueVM.TempPrepaidAndRevenueReceived = TempRevenueObj;
            TempRevenueVM.RevenueReceivedAccountNumber = TempRevenueDetailsObj.FromAccountNumber;
            TempRevenueVM.RevenueAccountNumber = TempRevenueDetailsObj.ToAccountNumber;
            TempRevenueVM.RevenueReceivedCostNumber = TempRevenueDetailsObj.FromCostCenter;
            TempRevenueVM.RevenueCostNumber = TempRevenueDetailsObj.ToCostCenter;
            TempRevenueVM.RevenueReceivedAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TempRevenueVM.RevenueReceivedAccountNumber);
            TempRevenueVM.RevenueAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TempRevenueVM.RevenueAccountNumber);
            TempRevenueVM.RevenueReceivedCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TempRevenueVM.RevenueReceivedCostNumber);
            TempRevenueVM.RevenueCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TempRevenueVM.RevenueCostNumber);
            TempRevenueVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            TempRevenueVM.TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency;
            return View(TempRevenueVM);
        }
        [HttpGet]
        public JsonResult GetTempRevenue(string id, string id2, string id3)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var TempRevenueDetailsObj = _unitOfWork.NativeSql.GetTempPrepaidAndRevenueReceivedDetalsData(id, int.Parse(id2), UserInfo.fCompanyId, int.Parse(id3));

            if (TempRevenueDetailsObj == null)
            {
                return Json(new TempPrepaidAndRevenueReceivedVM(), JsonRequestBehavior.AllowGet);
            }
            return Json(TempRevenueDetailsObj, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [Authorize(Roles = "CoOwner,DeleteTempRevenueReceived")]

        public ActionResult Delete(string id, int id2, int id3 , int id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            var TempRevenueObj = _unitOfWork.TempPrepaidAndRevenueReceived.GetTempPrepaidAndRevenueReceivedData(id, UserInfo.fCompanyId, id2, id3 , id4);
            var TempRevenueDetailsObj = _unitOfWork.TempPrepaidAndRevenueReceivedDetail.GetTempPrepaidAndRevenueReceivedDetailData(id, UserInfo.fCompanyId, id2, id3 , id4);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var TempRevenueVM = new TempPrepaidAndRevenueReceivedVM { };
            TempRevenueVM.WorkWithCostCenter = Company.WorkWithCostCenter;
            TempRevenueVM.TempPrepaidAndRevenueReceived = TempRevenueObj;
            TempRevenueVM.RevenueReceivedAccountNumber = TempRevenueDetailsObj.FromAccountNumber;
            TempRevenueVM.RevenueAccountNumber = TempRevenueDetailsObj.ToAccountNumber;
            TempRevenueVM.RevenueReceivedCostNumber = TempRevenueDetailsObj.FromCostCenter;
            TempRevenueVM.RevenueCostNumber = TempRevenueDetailsObj.ToCostCenter;
            TempRevenueVM.RevenueReceivedAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TempRevenueVM.RevenueReceivedAccountNumber);
            TempRevenueVM.RevenueAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TempRevenueVM.RevenueAccountNumber);
            TempRevenueVM.RevenueReceivedCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TempRevenueVM.RevenueReceivedCostNumber);
            TempRevenueVM.RevenueCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TempRevenueVM.RevenueCostNumber);
            TempRevenueVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            TempRevenueVM.TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency;
            return View(TempRevenueVM);
        }
        [HttpPost]
        public JsonResult DeleteTempRevenue(TempPrepaidAndRevenueReceivedVM ObjDelete)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                ObjDelete.CompanyID = UserInfo.fCompanyId;
                ObjDelete.TransactionKindNo = 18;
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

        [Authorize(Roles = "CoOwner,UpdateTempRevenueReceived")]

        public ActionResult Update(string id, int id2, int id3 , int id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            var TempRevenueObj = _unitOfWork.TempPrepaidAndRevenueReceived.GetTempPrepaidAndRevenueReceivedData(id, UserInfo.fCompanyId, id2, id3 , id4);
            var TempRevenueDetailsObj = _unitOfWork.TempPrepaidAndRevenueReceivedDetail.GetTempPrepaidAndRevenueReceivedDetailData(id, UserInfo.fCompanyId, id2, id3 , id4);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var TempRevenueVM = new TempPrepaidAndRevenueReceivedVM { };
            TempRevenueVM.WorkWithCostCenter = Company.WorkWithCostCenter;
            TempRevenueVM.TempPrepaidAndRevenueReceived = TempRevenueObj;
            TempRevenueVM.RevenueReceivedAccountNumber = TempRevenueDetailsObj.FromAccountNumber;
            TempRevenueVM.RevenueAccountNumber = TempRevenueDetailsObj.ToAccountNumber;
            TempRevenueVM.RevenueReceivedCostNumber = TempRevenueDetailsObj.FromCostCenter;
            TempRevenueVM.RevenueCostNumber = TempRevenueDetailsObj.ToCostCenter;
            TempRevenueVM.RevenueReceivedAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TempRevenueVM.RevenueReceivedAccountNumber);
            TempRevenueVM.RevenueAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TempRevenueVM.RevenueAccountNumber);
            TempRevenueVM.RevenueReceivedCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TempRevenueVM.RevenueReceivedCostNumber);
            TempRevenueVM.RevenueCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TempRevenueVM.RevenueCostNumber);
            TempRevenueVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            TempRevenueVM.TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency;
            TempRevenueVM.CompanyYear = TempRevenueDetailsObj.CompanyYear;
            return View(TempRevenueVM);
        }
        [HttpGet]

        [Authorize(Roles = "CoOwner,CopyTempRevenueReceived")]

        public ActionResult Copy(string id, int id2, int id3 , int id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            var TempRevenueObj = _unitOfWork.TempPrepaidAndRevenueReceived.GetTempPrepaidAndRevenueReceivedData(id, UserInfo.fCompanyId, id2, id3 , id4);
            var TempRevenueDetailsObj = _unitOfWork.TempPrepaidAndRevenueReceivedDetail.GetTempPrepaidAndRevenueReceivedDetailData(id, UserInfo.fCompanyId, id2, id3 , id4);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var TempRevenueVM = new TempPrepaidAndRevenueReceivedVM { };
            TempRevenueVM.WorkWithCostCenter = Company.WorkWithCostCenter;
            TempRevenueVM.TempPrepaidAndRevenueReceived = TempRevenueObj;
            TempRevenueVM.RevenueReceivedAccountNumber = TempRevenueDetailsObj.FromAccountNumber;
            TempRevenueVM.RevenueAccountNumber = TempRevenueDetailsObj.ToAccountNumber;
            TempRevenueVM.RevenueReceivedCostNumber = TempRevenueDetailsObj.FromCostCenter;
            TempRevenueVM.RevenueCostNumber = TempRevenueDetailsObj.ToCostCenter;
            TempRevenueVM.RevenueReceivedAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TempRevenueVM.RevenueReceivedAccountNumber);
            TempRevenueVM.RevenueAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TempRevenueVM.RevenueAccountNumber);
            TempRevenueVM.RevenueReceivedCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TempRevenueVM.RevenueReceivedCostNumber);
            TempRevenueVM.RevenueCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TempRevenueVM.RevenueCostNumber);
            TempRevenueVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            TempRevenueVM.TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency;
            return View(TempRevenueVM);
        }
        [HttpPost]
        public JsonResult UpdateTempRevenue(TempPrepaidAndRevenueReceivedVM ObjUpdate)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);

                var TempPrepaidAndRevenueReceivedDetail = ObjUpdate.TempPrepaidAndRevenueReceivedDetail;

                var TempRevenueToUpdate = ObjUpdate.TempPrepaidAndRevenueReceived;
                TempRevenueToUpdate.InsDateTime = DateTime.Now;
                TempRevenueToUpdate.InsUserID = userId;
                TempRevenueToUpdate.TransactionKindNo = (int)EnumTransKind.RevenueReceived;
                TempRevenueToUpdate.CompanyTransactionKindNo = 0;
                TempRevenueToUpdate.CompanyID = UserInfo.fCompanyId;
                TempRevenueToUpdate.Total = TempRevenueToUpdate.Total;
                TempRevenueToUpdate.NumberOfPayments = TempRevenueToUpdate.NumberOfPayments;
                TempRevenueToUpdate.DateFirstPayment = TempRevenueToUpdate.DateFirstPayment;
                TempRevenueToUpdate.VoucherDate = TempRevenueToUpdate.VoucherDate;
                TempRevenueToUpdate.CompanyYear = TempRevenueToUpdate.CompanyYear;
                TempRevenueToUpdate.VoucherNumber = TempRevenueToUpdate.VoucherNumber;
                TempRevenueToUpdate.VHI = int.Parse(TempRevenueToUpdate.VoucherNumber);
                var iRow = 0;
                _unitOfWork.NativeSql.DeleteTempPrepaidAndRevenueReceivedDetails(TempRevenueToUpdate.CompanyID, TempRevenueToUpdate.VoucherNumber,
                    TempRevenueToUpdate.CompanyTransactionKindNo, TempRevenueToUpdate.TransactionKindNo, TempRevenueToUpdate.CompanyYear);
                foreach (var TempRevenueDetailsToUpdate in TempPrepaidAndRevenueReceivedDetail)
                {
                    if (Company.WorkWithCostCenter)
                    {
                        if (!String.IsNullOrWhiteSpace(ObjUpdate.RevenueReceivedAccountNumber) && !String.IsNullOrWhiteSpace(ObjUpdate.RevenueAccountNumber)
                            && !String.IsNullOrWhiteSpace(ObjUpdate.RevenueReceivedCostNumber) && !String.IsNullOrWhiteSpace(ObjUpdate.RevenueCostNumber))
                        {
                            try
                            {
                                iRow = iRow + 1;
                                TempRevenueDetailsToUpdate.CompanyID = UserInfo.fCompanyId;
                                TempRevenueDetailsToUpdate.CompanyYear = TempRevenueToUpdate.CompanyYear;
                                TempRevenueDetailsToUpdate.CompanyTransactionKindNo = TempRevenueToUpdate.CompanyTransactionKindNo;
                                TempRevenueDetailsToUpdate.TransactionKindNo = TempRevenueToUpdate.TransactionKindNo;
                                TempRevenueDetailsToUpdate.VoucherDate = TempRevenueToUpdate.VoucherDate;
                                TempRevenueDetailsToUpdate.VoucherNumber = TempRevenueToUpdate.VoucherNumber;
                                TempRevenueDetailsToUpdate.InsUserID = TempRevenueToUpdate.InsUserID;
                                TempRevenueDetailsToUpdate.VHI = TempRevenueToUpdate.VHI;
                                TempRevenueDetailsToUpdate.InsDateTime = DateTime.Now;
                                TempRevenueDetailsToUpdate.RowNumber = iRow;
                                TempRevenueDetailsToUpdate.FromAccountNumber = ObjUpdate.RevenueReceivedAccountNumber;
                                TempRevenueDetailsToUpdate.ToAccountNumber = ObjUpdate.RevenueAccountNumber;
                                TempRevenueDetailsToUpdate.FromCostCenter = ObjUpdate.RevenueReceivedCostNumber;
                                TempRevenueDetailsToUpdate.ToCostCenter = ObjUpdate.RevenueCostNumber;
                                _unitOfWork.TempPrepaidAndRevenueReceivedDetail.Add(TempRevenueDetailsToUpdate);
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
                        if (!String.IsNullOrWhiteSpace(ObjUpdate.RevenueReceivedAccountNumber) && !String.IsNullOrWhiteSpace(ObjUpdate.RevenueAccountNumber))
                        {
                            try
                            {
                                iRow = iRow + 1;
                                TempRevenueDetailsToUpdate.CompanyID = UserInfo.fCompanyId;
                                TempRevenueDetailsToUpdate.CompanyYear = TempRevenueToUpdate.CompanyYear;
                                TempRevenueDetailsToUpdate.CompanyTransactionKindNo = TempRevenueToUpdate.CompanyTransactionKindNo;
                                TempRevenueDetailsToUpdate.TransactionKindNo = TempRevenueToUpdate.TransactionKindNo;
                                TempRevenueDetailsToUpdate.VoucherDate = TempRevenueToUpdate.VoucherDate;
                                TempRevenueDetailsToUpdate.VoucherNumber = TempRevenueToUpdate.VoucherNumber;
                                TempRevenueDetailsToUpdate.InsUserID = TempRevenueToUpdate.InsUserID;
                                TempRevenueDetailsToUpdate.VHI = TempRevenueToUpdate.VHI;
                                TempRevenueDetailsToUpdate.InsDateTime = DateTime.Now;
                                TempRevenueDetailsToUpdate.RowNumber = iRow;
                                TempRevenueDetailsToUpdate.FromAccountNumber = ObjUpdate.RevenueReceivedAccountNumber;
                                TempRevenueDetailsToUpdate.ToAccountNumber = ObjUpdate.RevenueAccountNumber;
                                _unitOfWork.TempPrepaidAndRevenueReceivedDetail.Add(TempRevenueDetailsToUpdate);
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
                TempRevenueToUpdate.RowCount = iRow;
                _unitOfWork.TempPrepaidAndRevenueReceived.Update(TempRevenueToUpdate);
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
        public ActionResult UpdateTempRevenue(string id, int id2, int id3 , int id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            var TempRevenueObj = _unitOfWork.NativeSql.GetTempPrepaidAndRevenueReceivedData(id, UserInfo.fCompanyId, id2, id3);
            var TempRevenueDetailsObj = _unitOfWork.TempPrepaidAndRevenueReceivedDetail.GetTempPrepaidAndRevenueReceivedDetailData(id, UserInfo.fCompanyId, id2, id3 , id4);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var TempRevenueVM = new TempPrepaidAndRevenueReceivedVM { };
            TempRevenueVM.WorkWithCostCenter = Company.WorkWithCostCenter;
            TempRevenueVM.TempPrepaidAndRevenueReceived = TempRevenueObj;
            TempRevenueVM.RevenueReceivedAccountNumber = TempRevenueDetailsObj.FromAccountNumber;
            TempRevenueVM.RevenueAccountNumber = TempRevenueDetailsObj.ToAccountNumber;
            TempRevenueVM.RevenueReceivedCostNumber = TempRevenueDetailsObj.FromCostCenter;
            TempRevenueVM.RevenueCostNumber = TempRevenueDetailsObj.ToCostCenter;
            TempRevenueVM.RevenueReceivedAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TempRevenueVM.RevenueReceivedAccountNumber);
            TempRevenueVM.RevenueAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TempRevenueVM.RevenueAccountNumber);
            TempRevenueVM.RevenueReceivedCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TempRevenueVM.RevenueReceivedCostNumber);
            TempRevenueVM.RevenueCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TempRevenueVM.RevenueCostNumber);
            TempRevenueVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            TempRevenueVM.TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency;
            TempRevenueVM.CompanyYear = TempRevenueDetailsObj.CompanyYear;
            return View(TempRevenueVM);
        }
        [HttpPost]
        public JsonResult UpdateTempRevenueWithExport(TempPrepaidAndRevenueReceivedVM ObjUpdate)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                var TempPrepaidAndRevenueReceivedDetail = ObjUpdate.TempPrepaidAndRevenueReceivedDetail;
                var TempRevenueToUpdate = ObjUpdate.TempPrepaidAndRevenueReceived;
                TempRevenueToUpdate.InsDateTime = DateTime.Now;
                TempRevenueToUpdate.InsUserID = userId;
                TempRevenueToUpdate.CompanyYear = TempRevenueToUpdate.CompanyYear;
                TempRevenueToUpdate.VoucherNumber = TempRevenueToUpdate.VoucherNumber;
                TempRevenueToUpdate.TransactionKindNo = (int)EnumTransKind.RevenueReceived;
                TempRevenueToUpdate.CompanyTransactionKindNo = 0;
                TempRevenueToUpdate.CompanyID = UserInfo.fCompanyId;
                int NumberOfPaymentsExported = _unitOfWork.NativeSql.GetCountNumberOfPaymnetTempPrepaidAndRevenueReceived(UserInfo.fCompanyId, TempRevenueToUpdate.VoucherNumber,
                    TempRevenueToUpdate.CompanyTransactionKindNo, TempRevenueToUpdate.TransactionKindNo, TempRevenueToUpdate.CompanyYear);
                double TotalExported = _unitOfWork.NativeSql.GetSumTotalTempPrepaidAndRevenueReceived(UserInfo.fCompanyId, TempRevenueToUpdate.VoucherNumber,
                    TempRevenueToUpdate.CompanyTransactionKindNo, TempRevenueToUpdate.TransactionKindNo, TempRevenueToUpdate.CompanyYear);
                TempRevenueToUpdate.Total = TempRevenueToUpdate.Total + TotalExported;
                TempRevenueToUpdate.NumberOfPayments = TempRevenueToUpdate.NumberOfPayments + NumberOfPaymentsExported;
                TempRevenueToUpdate.DateFirstPayment = TempRevenueToUpdate.DateFirstPayment;
                TempRevenueToUpdate.VoucherDate = TempRevenueToUpdate.VoucherDate;
                TempRevenueToUpdate.VHI = int.Parse(TempRevenueToUpdate.VoucherNumber);
                var iRow = 0;
                _unitOfWork.NativeSql.DeleteTempPrepaidAndRevenueReceivedDetailsWithoutExport(TempRevenueToUpdate.CompanyID, TempRevenueToUpdate.VoucherNumber,
                    TempRevenueToUpdate.CompanyTransactionKindNo, TempRevenueToUpdate.TransactionKindNo, TempRevenueToUpdate.CompanyYear);
                var UpdateRowNumber = _unitOfWork.NativeSql.GetRowNumberTempPrepaidAndRevenueReceived(TempRevenueToUpdate.VoucherNumber, UserInfo.fCompanyId,
                    TempRevenueToUpdate.CompanyTransactionKindNo, TempRevenueToUpdate.TransactionKindNo, TempRevenueToUpdate.CompanyYear);
                _unitOfWork.NativeSql.DeleteTempPrepaidAndRevenueReceivedDetails(TempRevenueToUpdate.CompanyID, TempRevenueToUpdate.VoucherNumber,
                    TempRevenueToUpdate.CompanyTransactionKindNo, TempRevenueToUpdate.TransactionKindNo, TempRevenueToUpdate.CompanyYear);
                foreach (var TempRevenueDetailsToUpdate in UpdateRowNumber)
                {
                    if (Company.WorkWithCostCenter)
                    {
                        if (!String.IsNullOrWhiteSpace(ObjUpdate.RevenueReceivedAccountNumber) && !String.IsNullOrWhiteSpace(ObjUpdate.RevenueAccountNumber)
                            && !String.IsNullOrWhiteSpace(ObjUpdate.RevenueReceivedCostNumber) && !String.IsNullOrWhiteSpace(ObjUpdate.RevenueCostNumber))
                        {
                            try
                            {
                                iRow = iRow + 1;
                                TempRevenueDetailsToUpdate.CompanyID = UserInfo.fCompanyId;
                                TempRevenueDetailsToUpdate.CompanyYear = TempRevenueToUpdate.CompanyYear;
                                TempRevenueDetailsToUpdate.CompanyTransactionKindNo = TempRevenueToUpdate.CompanyTransactionKindNo;
                                TempRevenueDetailsToUpdate.TransactionKindNo = TempRevenueToUpdate.TransactionKindNo;
                                TempRevenueDetailsToUpdate.VoucherDate = TempRevenueToUpdate.VoucherDate;
                                TempRevenueDetailsToUpdate.VoucherNumber = TempRevenueToUpdate.VoucherNumber;
                                TempRevenueDetailsToUpdate.InsUserID = TempRevenueToUpdate.InsUserID;
                                TempRevenueDetailsToUpdate.VHI = TempRevenueToUpdate.VHI;
                                TempRevenueDetailsToUpdate.InsDateTime = DateTime.Now;
                                TempRevenueDetailsToUpdate.RowNumber = iRow;
                                TempRevenueDetailsToUpdate.FromAccountNumber = ObjUpdate.RevenueReceivedAccountNumber;
                                TempRevenueDetailsToUpdate.ToAccountNumber = ObjUpdate.RevenueAccountNumber;
                                TempRevenueDetailsToUpdate.FromCostCenter = ObjUpdate.RevenueReceivedCostNumber;
                                TempRevenueDetailsToUpdate.ToCostCenter = ObjUpdate.RevenueCostNumber;
                                _unitOfWork.TempPrepaidAndRevenueReceivedDetail.Add(TempRevenueDetailsToUpdate);
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
                        if (!String.IsNullOrWhiteSpace(ObjUpdate.RevenueReceivedAccountNumber) && !String.IsNullOrWhiteSpace(ObjUpdate.RevenueAccountNumber))
                        {
                            try
                            {
                                iRow = iRow + 1;
                                TempRevenueDetailsToUpdate.CompanyID = UserInfo.fCompanyId;
                                TempRevenueDetailsToUpdate.CompanyYear = TempRevenueToUpdate.CompanyYear;
                                TempRevenueDetailsToUpdate.CompanyTransactionKindNo = TempRevenueToUpdate.CompanyTransactionKindNo;
                                TempRevenueDetailsToUpdate.TransactionKindNo = TempRevenueToUpdate.TransactionKindNo;
                                TempRevenueDetailsToUpdate.VoucherDate = TempRevenueToUpdate.VoucherDate;
                                TempRevenueDetailsToUpdate.VoucherNumber = TempRevenueToUpdate.VoucherNumber;
                                TempRevenueDetailsToUpdate.InsUserID = TempRevenueToUpdate.InsUserID;
                                TempRevenueDetailsToUpdate.VHI = TempRevenueToUpdate.VHI;
                                TempRevenueDetailsToUpdate.InsDateTime = DateTime.Now;
                                TempRevenueDetailsToUpdate.RowNumber = iRow;
                                TempRevenueDetailsToUpdate.FromAccountNumber = ObjUpdate.RevenueReceivedAccountNumber;
                                TempRevenueDetailsToUpdate.ToAccountNumber = ObjUpdate.RevenueAccountNumber;
                                _unitOfWork.TempPrepaidAndRevenueReceivedDetail.Add(TempRevenueDetailsToUpdate);
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
                foreach (var TempRevenueDetailsToUpdate in TempPrepaidAndRevenueReceivedDetail)
                {
                    if (Company.WorkWithCostCenter)
                    {
                        if (!String.IsNullOrWhiteSpace(ObjUpdate.RevenueReceivedAccountNumber) && !String.IsNullOrWhiteSpace(ObjUpdate.RevenueAccountNumber)
                            && !String.IsNullOrWhiteSpace(ObjUpdate.RevenueReceivedCostNumber) && !String.IsNullOrWhiteSpace(ObjUpdate.RevenueCostNumber))
                        {
                            try
                            {
                                iRow = iRow + 1;
                                TempRevenueDetailsToUpdate.CompanyID = UserInfo.fCompanyId;
                                TempRevenueDetailsToUpdate.CompanyYear = TempRevenueToUpdate.CompanyYear;
                                TempRevenueDetailsToUpdate.CompanyTransactionKindNo = TempRevenueToUpdate.CompanyTransactionKindNo;
                                TempRevenueDetailsToUpdate.TransactionKindNo = TempRevenueToUpdate.TransactionKindNo;
                                TempRevenueDetailsToUpdate.VoucherDate = TempRevenueToUpdate.VoucherDate;
                                TempRevenueDetailsToUpdate.VoucherNumber = TempRevenueToUpdate.VoucherNumber;
                                TempRevenueDetailsToUpdate.InsUserID = TempRevenueToUpdate.InsUserID;
                                TempRevenueDetailsToUpdate.VHI = TempRevenueToUpdate.VHI;
                                TempRevenueDetailsToUpdate.InsDateTime = DateTime.Now;
                                TempRevenueDetailsToUpdate.RowNumber = iRow;
                                TempRevenueDetailsToUpdate.FromAccountNumber = ObjUpdate.RevenueReceivedAccountNumber;
                                TempRevenueDetailsToUpdate.ToAccountNumber = ObjUpdate.RevenueAccountNumber;
                                TempRevenueDetailsToUpdate.FromCostCenter = ObjUpdate.RevenueReceivedCostNumber;
                                TempRevenueDetailsToUpdate.ToCostCenter = ObjUpdate.RevenueCostNumber;
                                TempRevenueDetailsToUpdate.CollectionDate = DateTime.Parse(TempRevenueDetailsToUpdate.CollectionDate.ToString("dd/MM/yyyy"));
                                _unitOfWork.TempPrepaidAndRevenueReceivedDetail.Add(TempRevenueDetailsToUpdate);
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
                        if (!String.IsNullOrWhiteSpace(ObjUpdate.RevenueReceivedAccountNumber) && !String.IsNullOrWhiteSpace(ObjUpdate.RevenueAccountNumber))
                        {
                            try
                            {
                                iRow = iRow + 1;
                                TempRevenueDetailsToUpdate.CompanyID = UserInfo.fCompanyId;
                                TempRevenueDetailsToUpdate.CompanyYear = TempRevenueToUpdate.CompanyYear;
                                TempRevenueDetailsToUpdate.CompanyTransactionKindNo = TempRevenueToUpdate.CompanyTransactionKindNo;
                                TempRevenueDetailsToUpdate.TransactionKindNo = TempRevenueToUpdate.TransactionKindNo;
                                TempRevenueDetailsToUpdate.VoucherDate = TempRevenueToUpdate.VoucherDate;
                                TempRevenueDetailsToUpdate.VoucherNumber = TempRevenueToUpdate.VoucherNumber;
                                TempRevenueDetailsToUpdate.InsUserID = TempRevenueToUpdate.InsUserID;
                                TempRevenueDetailsToUpdate.VHI = TempRevenueToUpdate.VHI;
                                TempRevenueDetailsToUpdate.InsDateTime = DateTime.Now;
                                TempRevenueDetailsToUpdate.RowNumber = iRow;
                                TempRevenueDetailsToUpdate.FromAccountNumber = ObjUpdate.RevenueReceivedAccountNumber;
                                TempRevenueDetailsToUpdate.ToAccountNumber = ObjUpdate.RevenueAccountNumber;
                                TempRevenueDetailsToUpdate.CollectionDate = DateTime.Parse(TempRevenueDetailsToUpdate.CollectionDate.ToString("dd/MM/yyyy"));
                                _unitOfWork.TempPrepaidAndRevenueReceivedDetail.Add(TempRevenueDetailsToUpdate);
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
                TempRevenueToUpdate.RowCount = iRow;
                _unitOfWork.TempPrepaidAndRevenueReceived.Update(TempRevenueToUpdate);
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
        [Authorize(Roles = "CoOwner,RepTempRevenueReport")]

        public ActionResult TempRevenueReport()
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
        public JsonResult GetTempRevenueReport(TempPrepaidAndRevenueReceivedVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllTempRevenue = _unitOfWork.NativeSql.GetTempRevenueReport(UserInfo.fCompanyId, Obj.All,Obj.Collector, Obj.NotCollected, Obj.FromCollectionDate, Obj.ToCollectionDate);
                if (AllTempRevenue == null)
                {
                    return Json(new List<TempPrepaidAndRevenueReceivedVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.RevenueReceivedAccountNumber))
                {
                    AllTempRevenue = AllTempRevenue.Where(m => m.RevenueReceivedAccountNumber == Obj.RevenueReceivedAccountNumber).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.RevenueAccountNumber))
                {
                    AllTempRevenue = AllTempRevenue.Where(m => m.RevenueAccountNumber == Obj.RevenueAccountNumber).ToList();
                }
                return Json(AllTempRevenue, JsonRequestBehavior.AllowGet);
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
            var VHI = _unitOfWork.TempPrepaidAndRevenueReceived.GetMaxVHByKind(UserInfo.fCompanyId, id, 18 , id2).ToString();
            return Json(VHI, JsonRequestBehavior.AllowGet);
        }
    }
}