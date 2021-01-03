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
    public class PaymentVoucherBankController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public PaymentVoucherBankController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }

        [Authorize(Roles = "CoOwner,ShowPaymentVoucherBank")]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetPaymentVoucherBankFromTransactionKind(UserInfo.fCompanyId);
            int year = DateTime.Now.Year;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            DateTime Today = DateTime.Now;
            DateTime FromDate = DateTime.Now;
            DateTime ToDate = DateTime.Now;
            int CurrentYear = Today.Year;
            if (CurrentYear == UserInfo.CurrYear)
            {
                FromDate = DateTime.Now;
                ToDate = DateTime.Now;
            }
            else if (CurrentYear < UserInfo.CurrYear || CurrentYear > UserInfo.CurrYear)
            {
                FromDate = new DateTime(UserInfo.CurrYear, 1, 1);
                ToDate = new DateTime(UserInfo.CurrYear, 1, 1);
            }
            var PaymentBankFilter = new PaymentBankFilterVM
            {
                FromDate = FromDate,
                ToDate = ToDate,
                CompanyTransactionKind = CompanyTransactionKindObj,
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                CompanyYear = UserInfo.CurrYear
            };
            return View(PaymentBankFilter);
        }
        [HttpPost]
        public JsonResult GetAllPaymentVoucherBank(PaymentBankFilterVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllPaymentBank = _unitOfWork.NativeSql.GetAllPaymentBankFromHeader(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                if (AllPaymentBank == null)
                {
                    return Json(new List<PaymentBankFilterVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.VoucherNumber))
                {
                    AllPaymentBank = AllPaymentBank.Where(m => m.VoucherNumber == Obj.VoucherNumber).ToList();
                }
                if (Obj.CurrencyID != 0)
                {
                    AllPaymentBank = AllPaymentBank.Where(m => m.CurrencyID == Obj.CurrencyID).ToList();
                }
                if (Obj.CompanyTransactionKindNo != 0)
                {
                    AllPaymentBank = AllPaymentBank.Where(m => m.CompanyTransactionKindNo == Obj.CompanyTransactionKindNo).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.PaidAccountNumber))
                {
                    AllPaymentBank = AllPaymentBank.Where(m => m.PaidAccountNumber == Obj.PaidAccountNumber).ToList();
                }
                return Json(AllPaymentBank, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<PaymentBankFilterVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [ValidateInput(false)]
        public ActionResult GridViewPapers(string id, string id2, string id3, string id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            if (!String.IsNullOrEmpty(id))
            {
                var TransObj = _unitOfWork.NativeSql.GetPapersToPaymentVoucherBankPosted(id, int.Parse(id2), UserInfo.fCompanyId, int.Parse(id3), int.Parse(id4));
                return PartialView("GridViewPapers", TransObj);
            }
            else
            {
                var TransObj = new List<TransactionFixedVM>();
                return PartialView("GridViewPapers", TransObj);
            }


        }
        [ValidateInput(false)]
        public ActionResult GridViewPapersCopy(string id, string id2, string id3, string id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            if (!String.IsNullOrEmpty(id))
            {
                var TransObj = _unitOfWork.NativeSql.GetPapersToPaymentVoucherBankPostedCopy(id, int.Parse(id2), UserInfo.fCompanyId, int.Parse(id3), int.Parse(id4));
                return PartialView("GridViewPapersCopy", TransObj);
            }
            else
            {
                var TransObj = new List<TransactionFixedVM>();
                return PartialView("GridViewPapersCopy", TransObj);
            }


        }
        [Authorize(Roles = "CoOwner,AddPaymentVoucherBank")]
        public ActionResult AddNew()
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var HeadrObj = new Header();
            HeadrObj.VoucherDate = DateTime.Now;
            var TransactionDebitObj = new Transaction();
            var TransactionCreditObj = new Transaction();
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetPaymentVoucherBankFromTransactionKind(UserInfo.fCompanyId);
            var TransFixedVM = new TransactionFixedVM
            {
                Header = HeadrObj,
                TransactionDebit = TransactionDebitObj,
                TransactionCredit = TransactionCreditObj,
                CompanyTransactionKind = CompanyTransactionKindObj,
                CompanyTransactionKindID = 1,
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                CurrencyID = 1,
                WorkWithCostCenter = Company.WorkWithCostCenter,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                Sirs = true,
                CurrentYear = UserInfo.CurrYear
            };
            return View(TransFixedVM);
        }
        public JsonResult SavePaymentVoucherBank(TransactionFixedVM ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                var SaveHeader = ObjToSave.Header;
                SaveHeader.InsDateTime = DateTime.Now;
                SaveHeader.InsUserID = userId;
                SaveHeader.CompanyYear = UserInfo.CurrYear;
                SaveHeader.CompanyTransactionKindNo = ObjToSave.CompanyTransactionKindID;
                SaveHeader.TransactionKindNo = _unitOfWork.CompanyTransactionKind.GetTransKindForCompanyTransactionKind(UserInfo.fCompanyId, ObjToSave.CompanyTransactionKindID);
                SaveHeader.CompanyID = UserInfo.fCompanyId;
                SaveHeader.TotalCredit = ObjToSave.Amount;
                SaveHeader.TotalDebit = ObjToSave.Amount;
                SaveHeader.SaleID = 0;
                double TotalDebitForeign = 0;
                if (SaveHeader.ConversionFactor == 0)
                {
                    SaveHeader.ConversionFactor = 1;
                };
                if (SaveHeader.ConversionFactor == 1)
                {
                    TotalDebitForeign = 0;
                }
                else
                {
                    TotalDebitForeign = SaveHeader.TotalDebit / SaveHeader.ConversionFactor;
                };
                SaveHeader.TotalDebitForeign = TotalDebitForeign;
                SaveHeader.TotalCreditForeign = TotalDebitForeign;

                
                SaveHeader.FCurrencyID = ObjToSave.CurrencyID;
                var ObjComapnyTransactionKind = _unitOfWork.CompanyTransactionKind.GetCompanyTransactionKindByID(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo);
                if (ObjComapnyTransactionKind.AutoSerial)
                {
                    SaveHeader.VoucherNumber = _unitOfWork.Header.GetMaxVHByKind(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo, SaveHeader.TransactionKindNo , SaveHeader.CompanyYear).ToString();
                    SaveHeader.VHI = int.Parse(SaveHeader.VoucherNumber);
                }
                else
                {
                    string SerialNumber = "";
                    int Serial = ObjComapnyTransactionKind.Serial;
                    string Symbol = ObjComapnyTransactionKind.Symbol;
                    DateTime dDate = SaveHeader.VoucherDate;
                    string YearNo = dDate.ToString("yy");
                    string sMonth = dDate.ToString("MM");
                    int MonthNo = Int16.Parse(sMonth);
                    int LengthLastSerial = _unitOfWork.CompanyTransactionKindMonthlySerial.GetMaxSerial(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo, MonthNo).ToString().Length;
                    Serial = Serial - LengthLastSerial;
                    for (int i = 0; i <= Serial; i++)
                    {
                        if (i < Serial)
                        {
                            SerialNumber = SerialNumber + "0";
                        }
                        else if (i == Serial)
                        {
                            SerialNumber = SerialNumber + _unitOfWork.CompanyTransactionKindMonthlySerial.GetMaxSerial(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo, MonthNo).ToString();
                        }
                    }
                    SaveHeader.VoucherNumber = Symbol + YearNo + sMonth + SerialNumber;
                    SaveHeader.VHI = _unitOfWork.CompanyTransactionKindMonthlySerial.GetMaxSerial(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo, MonthNo);
                    _unitOfWork.CompanyTransactionKindMonthlySerial.Update(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo, MonthNo);
                }
                if (Company.DirectExportTheTransactions)
                {
                    SaveHeader.Exported = 1;
                }
                else
                {
                    SaveHeader.Exported = 0;
                }
                var TransactionDebit = new Transaction();
                TransactionDebit.CompanyID = UserInfo.fCompanyId;
                TransactionDebit.CompanyYear = SaveHeader.CompanyYear;
                TransactionDebit.CompanyTransactionKindNo = SaveHeader.CompanyTransactionKindNo;
                TransactionDebit.TransactionKindNo = SaveHeader.TransactionKindNo;
                TransactionDebit.VoucherDate = SaveHeader.VoucherDate;
                TransactionDebit.VoucherNumber = SaveHeader.VoucherNumber;
                TransactionDebit.InsUserID = SaveHeader.InsUserID;
                TransactionDebit.VHI = SaveHeader.VHI;
                TransactionDebit.InsDateTime = DateTime.Now;
                TransactionDebit.AccountNumber = ObjToSave.FromAccountNumber;
                TransactionDebit.CostCenter = ObjToSave.FromCostCenter;
                TransactionDebit.Debit = ObjToSave.Amount;
                TransactionDebit.Credit = 0;
                if (SaveHeader.ConversionFactor == 1)
                {
                    TransactionDebit.DebitForeign = 0;
                }
                else
                {
                    TransactionDebit.DebitForeign = TransactionDebit.Debit / SaveHeader.ConversionFactor;
                };
                TransactionDebit.CreditForeign = 0;
                TransactionDebit.CreditDebitForeign = TransactionDebit.DebitForeign;
                TransactionDebit.Note = SaveHeader.Note;
                TransactionDebit.RowNumber = 1;
                _unitOfWork.Transaction.Add(TransactionDebit);
                var CreditRowNumber = 1;
                foreach (var SavePaper in ObjToSave.Paper)
                {
                    if (SavePaper.ChequeNumber == "" && SavePaper.ChequeNumber == null)
                    {
                        continue;
                    }
                    CreditRowNumber = CreditRowNumber + 1;
                    var TransactionCredit = new Transaction();
                    TransactionCredit.CompanyID = UserInfo.fCompanyId;
                    TransactionCredit.CompanyYear = SaveHeader.CompanyYear;
                    TransactionCredit.CompanyTransactionKindNo = SaveHeader.CompanyTransactionKindNo;
                    TransactionCredit.TransactionKindNo = SaveHeader.TransactionKindNo;
                    TransactionCredit.VoucherDate = SaveHeader.VoucherDate;
                    TransactionCredit.VoucherNumber = SaveHeader.VoucherNumber;
                    TransactionCredit.InsUserID = SaveHeader.InsUserID;
                    TransactionCredit.VHI = SaveHeader.VHI;
                    TransactionCredit.InsDateTime = DateTime.Now;
                    TransactionCredit.AccountNumber = SavePaper.AccountNumberSecond;
                    TransactionCredit.CostCenter = ObjToSave.ToCostCenter;
                    TransactionCredit.Debit = 0;
                    TransactionCredit.Credit = SavePaper.ChequeAmount;
                    TransactionCredit.DebitForeign = 0;
                    if (SaveHeader.ConversionFactor == 1)
                    {
                        TransactionCredit.CreditForeign = 0;
                    }
                    else
                    {
                        TransactionCredit.CreditForeign = TransactionCredit.Credit / SaveHeader.ConversionFactor;
                    };
                    TransactionCredit.CreditDebitForeign = TransactionCredit.CreditForeign;
                    TransactionCredit.Note = SaveHeader.Note;
                    TransactionCredit.RowNumber = CreditRowNumber;
                    _unitOfWork.Transaction.Add(TransactionCredit);
                    SavePaper.CompanyID = UserInfo.fCompanyId;
                    SavePaper.CompanyYear = SaveHeader.CompanyYear;
                    SavePaper.StockCode = "*";
                    SavePaper.BranchCode = "*";
                    SavePaper.CompanyTransactionKindNo = SaveHeader.CompanyTransactionKindNo;
                    SavePaper.TransactionKindNo = SaveHeader.TransactionKindNo;
                    SavePaper.VoucherDate = SaveHeader.VoucherDate;
                    SavePaper.VoucherNumber = SaveHeader.VoucherNumber;
                    SavePaper.VHI = SaveHeader.VHI;
                    SavePaper.ChequeCase = 14;
                    SavePaper.OldVoucherNumber = "-1";
                    SavePaper.OldCompanyTransactionKindNo = -1;
                    SavePaper.OldTransactionKindNo = -1;
                    SavePaper.OldVHI = -1;
                    SavePaper.sChequeDate = SavePaper.ChequeDate.ToString();
                    SavePaper.OriginalVoucherNumber = SaveHeader.VoucherNumber;
                    SavePaper.OriginalCompanyTransactionKindNo = SaveHeader.CompanyTransactionKindNo;
                    SavePaper.OriginalTransactionKindNo = SaveHeader.TransactionKindNo;
                    SavePaper.OriginalVHI = SaveHeader.VHI;
                    SavePaper.AccountNumberFirst = ObjToSave.FromAccountNumber;
                    SavePaper.AccountNumberSecond = SavePaper.AccountNumberSecond;
                    SavePaper.AccountNumberThird = SavePaper.AccountNumberThird;
                    SavePaper.Remark = SaveHeader.Note;
                    SavePaper.Sirs = ObjToSave.Sirs;
                    SavePaper.Mr = ObjToSave.Mr;
                    SavePaper.Mrs = ObjToSave.Mrs;
                    SavePaper.Co = ObjToSave.Co;
                    SavePaper.First = ObjToSave.First;
                    SavePaper.FCurrencyID = SaveHeader.FCurrencyID;
                    SavePaper.ConversionFactor = SaveHeader.ConversionFactor;
                    SavePaper.InsUserID = SaveHeader.InsUserID;
                    SavePaper.InsDateTime = DateTime.Now;
                    _unitOfWork.Paper.Add(SavePaper);
                }

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
                SaveHeader.RowCount = CreditRowNumber;
                _unitOfWork.Header.Add(SaveHeader);
                _unitOfWork.Complete();
                Msg.Code = 1;
                Msg.Msg = Resources.Resource.AddedSuccessfully;

                Msg.Year = SaveHeader.CompanyYear;
                Msg.FCompanyId = SaveHeader.CompanyID;
                Msg.VoucherNumber = SaveHeader.VoucherNumber.ToString();
                Msg.TransactionKindNo = SaveHeader.TransactionKindNo.ToString();
                Msg.CompanyTransactionKindNo = SaveHeader.CompanyTransactionKindNo.ToString();

                return Json(Msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                Msg.Code = 0;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }

        }
        [Authorize(Roles = "CoOwner,AttachPaymentVoucherBank")]
        public ActionResult ShowAttach(int id, string id2, string id3, string id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            TransActionFileVM Obj = new TransActionFileVM
            {
                Year = id,
                CompanyId = UserInfo.fCompanyId,
                VoucherNumber = id2,
                TransactionKindNo = id4,
                CompanyTransactionKindNo = id3

            };
            return View("ShowAttach", Obj);

        }
        [HttpGet]
        public ActionResult Detail(string id, int id2, int id3 , int id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            var HeaderObj = _unitOfWork.Header.GetHeaderDataById(id, UserInfo.fCompanyId, id2, id3 , id4);
            var CompanyTransactionObj = _unitOfWork.CompanyTransactionKind.GetCompanyTransactionKindByID(UserInfo.fCompanyId, HeaderObj.CompanyTransactionKindNo);
            var CurrencyObj = _unitOfWork.Currency.GetCurrencyByID(UserInfo.fCompanyId, HeaderObj.FCurrencyID);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var TransactionDebitObj = _unitOfWork.NativeSql.GetTransactionsDetailDebit(id, UserInfo.fCompanyId, id2, 1 , id4);
            var TransactionCreditObj = _unitOfWork.NativeSql.GetTransactionsDetailCredit(id, UserInfo.fCompanyId, id2, 2 , id4);
            int iRowNumber = HeaderObj.RowCount;
            var TransFixedVM = new TransactionFixedVM { };
            TransFixedVM.Header = HeaderObj;
            TransFixedVM.TransactionDebit = TransactionDebitObj;
            TransFixedVM.TransactionCredit = TransactionCreditObj;
            if (Resources.Resource.CurLang == "Arb")
            {
                TransFixedVM.CompanyTransactionKindName = CompanyTransactionObj.ArabicName;
                TransFixedVM.CurrencyName = CurrencyObj.ArabicName;
            }
            else
            {
                TransFixedVM.CompanyTransactionKindName = CompanyTransactionObj.EnglishName;
                TransFixedVM.CurrencyName = CurrencyObj.EnglishName;
            }
            TransFixedVM.CurrencyNewValue = Math.Round(HeaderObj.ConversionFactor, 3);
            TransFixedVM.WorkWithCostCenter = Company.WorkWithCostCenter;
            TransFixedVM.AccountDebitName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionDebitObj.AccountNumber);
            TransFixedVM.CostCenterDebitName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionDebitObj.CostCenter);
            TransFixedVM.CostCenterCreditName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionCreditObj.CostCenter);
            TransFixedVM.Amount = Math.Round(TransactionDebitObj.Debit, 3);
            TransFixedVM.TotalAmount = Math.Round(HeaderObj.TotalDebit, 3);
            TransFixedVM.ForeignAmount = Math.Round(TransactionDebitObj.DebitForeign, 3);
            TransFixedVM.TotalForeign = Math.Round(HeaderObj.TotalDebitForeign, 3);
            TransFixedVM.TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency;
            TransFixedVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            return View(TransFixedVM);
        }
        [HttpGet]
        public JsonResult GetPapersToPaymentVoucherBank(string id, string id2, string id3, string id4)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var AllPapersToPaymnetVoucherBank = _unitOfWork.NativeSql.GetPapersToPaymentVoucherBank(id, int.Parse(id2), UserInfo.fCompanyId, int.Parse(id3), int.Parse(id4));

            if (AllPapersToPaymnetVoucherBank == null)
            {
                return Json(new TransactionFixedVM(), JsonRequestBehavior.AllowGet);
            }
            return Json(AllPapersToPaymnetVoucherBank, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [Authorize(Roles = "CoOwner,DeletePaymentVoucherBank")]
        public ActionResult Delete(string id, int id2, int id3 , int id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            var HeaderObj = _unitOfWork.Header.GetHeaderDataById(id, UserInfo.fCompanyId, id2, id3 ,id4);
            var CompanyTransactionObj = _unitOfWork.CompanyTransactionKind.GetCompanyTransactionKindByID(UserInfo.fCompanyId, HeaderObj.CompanyTransactionKindNo);
            var CurrencyObj = _unitOfWork.Currency.GetCurrencyByID(UserInfo.fCompanyId, HeaderObj.FCurrencyID);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var TransactionDebitObj = _unitOfWork.NativeSql.GetTransactionsDetailDebit(id, UserInfo.fCompanyId, id2, 1 , id4);
            var TransactionCreditObj = _unitOfWork.NativeSql.GetTransactionsDetailCredit(id, UserInfo.fCompanyId, id2,2 , id4);
            int iRowNumber = HeaderObj.RowCount;
            var TransFixedVM = new TransactionFixedVM { };
            TransFixedVM.Header = HeaderObj;
            TransFixedVM.TransactionDebit = TransactionDebitObj;
            TransFixedVM.TransactionCredit = TransactionCreditObj;
            if (Resources.Resource.CurLang == "Arb")
            {
                TransFixedVM.CompanyTransactionKindName = CompanyTransactionObj.ArabicName;
                TransFixedVM.CurrencyName = CurrencyObj.ArabicName;
            }
            else
            {
                TransFixedVM.CompanyTransactionKindName = CompanyTransactionObj.EnglishName;
                TransFixedVM.CurrencyName = CurrencyObj.EnglishName;
            }
            TransFixedVM.CurrencyNewValue = Math.Round(HeaderObj.ConversionFactor, 3);
            TransFixedVM.WorkWithCostCenter = Company.WorkWithCostCenter;
            TransFixedVM.AccountDebitName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionDebitObj.AccountNumber);
            TransFixedVM.CostCenterDebitName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionDebitObj.CostCenter);
            TransFixedVM.CostCenterCreditName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionCreditObj.CostCenter);
            TransFixedVM.Amount = Math.Round(TransactionDebitObj.Debit, 3);
            TransFixedVM.TotalAmount = Math.Round(HeaderObj.TotalDebit, 3);
            TransFixedVM.ForeignAmount = Math.Round(TransactionDebitObj.DebitForeign, 3);
            TransFixedVM.TotalForeign = Math.Round(HeaderObj.TotalDebitForeign, 3);
            TransFixedVM.TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency;
            TransFixedVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            return View(TransFixedVM);
        }
        [Authorize(Roles = "CoOwner,ExportPaymentVoucherBank")]
        public ActionResult Export()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetPaymentVoucherBankFromTransactionKind(UserInfo.fCompanyId);
            int year = DateTime.Now.Year;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            DateTime Today = DateTime.Now;
            DateTime FromDate = DateTime.Now;
            DateTime ToDate = DateTime.Now;
            int CurrentYear = Today.Year;
            if (CurrentYear == UserInfo.CurrYear)
            {
                FromDate = DateTime.Now;
                ToDate = DateTime.Now;
            }
            else if (CurrentYear < UserInfo.CurrYear || CurrentYear > UserInfo.CurrYear)
            {
                FromDate = new DateTime(UserInfo.CurrYear, 1, 1);
                ToDate = new DateTime(UserInfo.CurrYear, 1, 1);
            }
            var PaymentBankFilter = new PaymentBankFilterVM
            {
                FromDate = FromDate,
                ToDate = ToDate,
                CompanyTransactionKind = CompanyTransactionKindObj,
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                CompanyYear = UserInfo.CurrYear
        };
            return View(PaymentBankFilter);
        }
        [HttpPost]
        public JsonResult GetAllPaymentVoucherBankUnExport(PaymentBankFilterVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllPaymentBank = _unitOfWork.NativeSql.GetAllPaymentBankFromHeaderUnExport(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                if (AllPaymentBank == null)
                {
                    return Json(new List<PaymentBankFilterVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.VoucherNumber))
                {
                    AllPaymentBank = AllPaymentBank.Where(m => m.VoucherNumber == Obj.VoucherNumber).ToList();
                }
                if (Obj.CurrencyID != 0)
                {
                    AllPaymentBank = AllPaymentBank.Where(m => m.CurrencyID == Obj.CurrencyID).ToList();
                }
                if (Obj.CompanyTransactionKindNo != 0)
                {
                    AllPaymentBank = AllPaymentBank.Where(m => m.CompanyTransactionKindNo == Obj.CompanyTransactionKindNo).ToList();
                }
                int iRow = 0;
                foreach (var iRowCount in AllPaymentBank)
                {
                    iRowCount.iRowTable = iRow;
                    iRow = iRow + 1;
                }
                return Json(AllPaymentBank, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<PaymentBankFilterVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [Authorize(Roles = "CoOwner,UnExportPaymentVoucherBank")]
        public ActionResult UnExport()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetPaymentVoucherBankFromTransactionKind(UserInfo.fCompanyId);
            int year = DateTime.Now.Year;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            DateTime Today = DateTime.Now;
            DateTime FromDate = DateTime.Now;
            DateTime ToDate = DateTime.Now;
            int CurrentYear = Today.Year;
            if (CurrentYear == UserInfo.CurrYear)
            {
                FromDate = DateTime.Now;
                ToDate = DateTime.Now;
            }
            else if (CurrentYear < UserInfo.CurrYear || CurrentYear > UserInfo.CurrYear)
            {
                FromDate = new DateTime(UserInfo.CurrYear, 1, 1);
                ToDate = new DateTime(UserInfo.CurrYear, 1, 1);
            }
            var PaymentBankFilter = new PaymentBankFilterVM
            {
                FromDate = FromDate,
                ToDate = ToDate,
                CompanyTransactionKind = CompanyTransactionKindObj,
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                CompanyYear = UserInfo.CurrYear
            };
            return View(PaymentBankFilter);
        }
        [HttpPost]
        public JsonResult GetAllPaymentVoucherBankExport(PaymentBankFilterVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllPaymentBank = _unitOfWork.NativeSql.GetAllPaymentBankFromHeaderExport(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                if (AllPaymentBank == null)
                {
                    return Json(new List<PaymentBankFilterVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.VoucherNumber))
                {
                    AllPaymentBank = AllPaymentBank.Where(m => m.VoucherNumber == Obj.VoucherNumber).ToList();
                }
                if (Obj.CurrencyID != 0)
                {
                    AllPaymentBank = AllPaymentBank.Where(m => m.CurrencyID == Obj.CurrencyID).ToList();
                }
                if (Obj.CompanyTransactionKindNo != 0)
                {
                    AllPaymentBank = AllPaymentBank.Where(m => m.CompanyTransactionKindNo == Obj.CompanyTransactionKindNo).ToList();
                }
                int iRow = 0;
                foreach (var iRowCount in AllPaymentBank)
                {
                    iRowCount.iRowTable = iRow;
                    iRow = iRow + 1;
                }
                return Json(AllPaymentBank, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<PaymentBankFilterVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public JsonResult UpdateToExport(ExportAndUnExportVM ObjToUpdate)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                foreach (var UpdateHeader in ObjToUpdate.Header)
                {
                    UpdateHeader.Exported = 1;
                    UpdateHeader.CompanyID = UserInfo.fCompanyId;
                    _unitOfWork.Header.UpdateToExportAndUnExport(UpdateHeader);
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
        [HttpPost]
        public JsonResult UpdateToUnExport(ExportAndUnExportVM ObjToUpdate)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                foreach (var UpdateHeader in ObjToUpdate.Header)
                {
                    UpdateHeader.Exported = 0;
                    UpdateHeader.CompanyID = UserInfo.fCompanyId;
                    _unitOfWork.Header.UpdateToExportAndUnExport(UpdateHeader);
                    _unitOfWork.Complete();
                }
                Msg.Code = 1;
                Msg.Msg = Resources.Resource.UnExportSuccessfully;
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
        [Authorize(Roles = "CoOwner,UpdatePaymentVoucherBank")]
        public ActionResult Update(string id, int id2, int id3 , int id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            var HeaderObj = _unitOfWork.Header.GetHeaderDataById(id, UserInfo.fCompanyId, id2, id3 , id4);
            var CompanyTransactionKindObj = _unitOfWork.CompanyTransactionKind.GetCompanyTransactionKindByID(UserInfo.fCompanyId, HeaderObj.CompanyTransactionKindNo);
            var CompanyTransactionKindID = HeaderObj.CompanyTransactionKindNo;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var TransactionDebitObj = _unitOfWork.NativeSql.GetTransactionsDetailDebit(id, UserInfo.fCompanyId, id2, 1 , id4);
            var TransactionCreditObj = _unitOfWork.NativeSql.GetTransactionsDetailCredit(id, UserInfo.fCompanyId, id2, 2 , id4);
            var PostedPaper = _unitOfWork.NativeSql.GetPostedPrintPapersCase(id, id2, UserInfo.fCompanyId, id3, id4);
            int iRowNumber = HeaderObj.RowCount;
            var TransFixedVM = new TransactionFixedVM { };
            TransFixedVM.Header = HeaderObj;
            TransFixedVM.TransactionDebit = TransactionDebitObj;
            TransFixedVM.TransactionCredit = TransactionCreditObj;
            TransFixedVM.CompanyTransactionKindID = CompanyTransactionKindID;
            if (Resources.Resource.CurLang == "Arb")
            {
                TransFixedVM.CompanyTransactionKindName = CompanyTransactionKindObj.ArabicName;
            }
            else
            {
                TransFixedVM.CompanyTransactionKindName = CompanyTransactionKindObj.EnglishName;
            }
            TransFixedVM.Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId);
            TransFixedVM.CurrencyID = HeaderObj.FCurrencyID;
            TransFixedVM.CurrencyNewValue = Math.Round(HeaderObj.ConversionFactor, 3);
            TransFixedVM.WorkWithCostCenter = Company.WorkWithCostCenter;
            TransFixedVM.AccountDebitName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionDebitObj.AccountNumber);
            TransFixedVM.CostCenterDebitName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionDebitObj.CostCenter);
            TransFixedVM.CostCenterCreditName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionCreditObj.CostCenter);
            TransFixedVM.Amount = Math.Round(TransactionDebitObj.Debit, 3);
            TransFixedVM.TotalAmount = Math.Round(HeaderObj.TotalDebit, 3);
            TransFixedVM.ForeignAmount = Math.Round(TransactionDebitObj.DebitForeign, 3);
            TransFixedVM.TotalForeign = Math.Round(HeaderObj.TotalDebitForeign, 3);
            TransFixedVM.TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency;
            TransFixedVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            if (PostedPaper.Sirs)
            {
                TransFixedVM.Sirs = true;
                TransFixedVM.Mr = false;
                TransFixedVM.Mrs = false;
                TransFixedVM.Co = false;
                TransFixedVM.First = false;
            }
            else if (PostedPaper.Mr)
            {
                TransFixedVM.Sirs = false;
                TransFixedVM.Mr = true;
                TransFixedVM.Mrs = false;
                TransFixedVM.Co = false;
                TransFixedVM.First = false;
            }
            else if (PostedPaper.Mrs)
            {
                TransFixedVM.Sirs = false;
                TransFixedVM.Mr = false;
                TransFixedVM.Mrs = true;
                TransFixedVM.Co = false;
                TransFixedVM.First = false;
            }
            else if (PostedPaper.Co)
            {
                TransFixedVM.Sirs = false;
                TransFixedVM.Mr = false;
                TransFixedVM.Mrs = false;
                TransFixedVM.Co = true;
                TransFixedVM.First = false;
            }
            else if (PostedPaper.First)
            {
                TransFixedVM.Sirs = false;
                TransFixedVM.Mr = false;
                TransFixedVM.Mrs = false;
                TransFixedVM.Co = false;
                TransFixedVM.First = true;
            }
            return View(TransFixedVM);
        }
        public JsonResult UpdatePaymentVoucherBank(TransactionFixedVM ObjToUpdate)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);

                var UpdateHeader = ObjToUpdate.Header;
                UpdateHeader.InsDateTime = DateTime.Now;
                UpdateHeader.VoucherNumber = ObjToUpdate.Header.VoucherNumber;
                UpdateHeader.VoucherDate = ObjToUpdate.Header.VoucherDate;
                UpdateHeader.CompanyTransactionKindNo = ObjToUpdate.CompanyTransactionKindID;
                UpdateHeader.TransactionKindNo = _unitOfWork.CompanyTransactionKind.GetTransKindForCompanyTransactionKind(UserInfo.fCompanyId, ObjToUpdate.CompanyTransactionKindID);
                UpdateHeader.InsUserID = userId;
                UpdateHeader.CompanyID = UserInfo.fCompanyId;
                UpdateHeader.TotalCredit = ObjToUpdate.Amount;
                UpdateHeader.TotalDebit = ObjToUpdate.Amount;
                UpdateHeader.SaleID = ObjToUpdate.SaleManNo;
                double TotalDebitForeign = 0;
                if (UpdateHeader.ConversionFactor == 0)
                {
                    UpdateHeader.ConversionFactor = 1;
                };
                if (UpdateHeader.ConversionFactor == 1)
                {
                    TotalDebitForeign = 0;
                }
                else
                {
                    TotalDebitForeign = UpdateHeader.TotalDebit / UpdateHeader.ConversionFactor;
                };
                UpdateHeader.TotalDebitForeign = TotalDebitForeign;
                UpdateHeader.TotalCreditForeign = TotalDebitForeign;

                UpdateHeader.CompanyYear = ObjToUpdate.CompanyYear;
                UpdateHeader.FCurrencyID = ObjToUpdate.CurrencyID;
                var TransactionDebit = new Transaction();
                TransactionDebit.CompanyID = UserInfo.fCompanyId;
                TransactionDebit.CompanyYear = UpdateHeader.CompanyYear;
                TransactionDebit.CompanyTransactionKindNo = UpdateHeader.CompanyTransactionKindNo;
                TransactionDebit.TransactionKindNo = UpdateHeader.TransactionKindNo;
                TransactionDebit.VoucherDate = UpdateHeader.VoucherDate;
                TransactionDebit.VoucherNumber = UpdateHeader.VoucherNumber;
                TransactionDebit.InsUserID = UpdateHeader.InsUserID;
                TransactionDebit.VHI = UpdateHeader.VHI;
                TransactionDebit.InsDateTime = DateTime.Now;
                TransactionDebit.AccountNumber = ObjToUpdate.FromAccountNumber;
                TransactionDebit.CostCenter = ObjToUpdate.FromCostCenter;
                TransactionDebit.Debit = ObjToUpdate.Amount;
                TransactionDebit.Credit = 0;
                if (UpdateHeader.ConversionFactor == 1)
                {
                    TransactionDebit.DebitForeign = 0;
                }
                else
                {
                    TransactionDebit.DebitForeign = TransactionDebit.Debit / UpdateHeader.ConversionFactor;
                };
                TransactionDebit.CreditForeign = 0;
                TransactionDebit.CreditDebitForeign = TransactionDebit.DebitForeign;
                TransactionDebit.Note = UpdateHeader.Note;
                TransactionDebit.RowNumber = 1;

                _unitOfWork.Transaction.Update(TransactionDebit);

                _unitOfWork.NativeSql.DeletePaper(UserInfo.fCompanyId, UpdateHeader.VoucherNumber,
                    UpdateHeader.CompanyTransactionKindNo, UpdateHeader.TransactionKindNo, UpdateHeader.CompanyYear);
                _unitOfWork.NativeSql.DeleteTransActionCredit(UserInfo.fCompanyId, UpdateHeader.VoucherNumber,
                   UpdateHeader.CompanyTransactionKindNo, UpdateHeader.TransactionKindNo, UpdateHeader.CompanyYear);
                var CreditRowNumber = 1;
                foreach (var SavePaper in ObjToUpdate.Paper)
                {
                    if (SavePaper.ChequeNumber == "" && SavePaper.ChequeNumber == null)
                    {
                        continue;
                    }
                    CreditRowNumber = CreditRowNumber + 1;
                    Transaction TransactionCredit = new Transaction();
                    TransactionCredit.CompanyID = UserInfo.fCompanyId;
                    TransactionCredit.CompanyYear = UpdateHeader.CompanyYear;
                    TransactionCredit.CompanyTransactionKindNo = UpdateHeader.CompanyTransactionKindNo;
                    TransactionCredit.TransactionKindNo = UpdateHeader.TransactionKindNo;
                    TransactionCredit.VoucherDate = UpdateHeader.VoucherDate;
                    TransactionCredit.VoucherNumber = UpdateHeader.VoucherNumber;
                    TransactionCredit.InsUserID = UpdateHeader.InsUserID;
                    TransactionCredit.VHI = UpdateHeader.VHI;
                    TransactionCredit.InsDateTime = DateTime.Now;
                    TransactionCredit.AccountNumber = SavePaper.AccountNumberSecond;
                    TransactionCredit.CostCenter = ObjToUpdate.ToCostCenter;
                    TransactionCredit.Debit = 0;
                    TransactionCredit.Credit = SavePaper.ChequeAmount;
                    TransactionCredit.DebitForeign = 0;
                    if (UpdateHeader.ConversionFactor == 1)
                    {
                        TransactionCredit.CreditForeign = 0;
                    }
                    else
                    {
                        TransactionCredit.CreditForeign = TransactionCredit.Credit / UpdateHeader.ConversionFactor;
                    };
                    TransactionCredit.CreditDebitForeign = TransactionCredit.CreditForeign;
                    TransactionCredit.Note = UpdateHeader.Note;
                    TransactionCredit.RowNumber = CreditRowNumber;
                    _unitOfWork.Transaction.Add(TransactionCredit);

                    SavePaper.CompanyID = UserInfo.fCompanyId;
                    SavePaper.CompanyYear = UpdateHeader.CompanyYear;
                    SavePaper.StockCode = "*";
                    SavePaper.BranchCode = "*";
                    SavePaper.CompanyTransactionKindNo = UpdateHeader.CompanyTransactionKindNo;
                    SavePaper.TransactionKindNo = UpdateHeader.TransactionKindNo;
                    SavePaper.VoucherDate = UpdateHeader.VoucherDate;
                    SavePaper.VoucherNumber = UpdateHeader.VoucherNumber;
                    SavePaper.VHI = UpdateHeader.VHI;
                    SavePaper.ChequeCase = 14;
                    SavePaper.OldVoucherNumber = "-1";
                    SavePaper.OldCompanyTransactionKindNo = -1;
                    SavePaper.OldTransactionKindNo = -1;
                    SavePaper.OldVHI = -1;
                    SavePaper.sChequeDate = SavePaper.ChequeDate.ToString();
                    SavePaper.OriginalVoucherNumber = UpdateHeader.VoucherNumber;
                    SavePaper.OriginalCompanyTransactionKindNo = UpdateHeader.CompanyTransactionKindNo;
                    SavePaper.OriginalTransactionKindNo = UpdateHeader.TransactionKindNo;
                    SavePaper.OriginalVHI = UpdateHeader.VHI;
                    SavePaper.AccountNumberFirst = ObjToUpdate.FromAccountNumber;
                    SavePaper.AccountNumberSecond = SavePaper.AccountNumberSecond;
                    SavePaper.AccountNumberThird = SavePaper.AccountNumberThird;
                    SavePaper.Remark = UpdateHeader.Note;
                    SavePaper.Sirs = ObjToUpdate.Sirs;
                    SavePaper.Mr = ObjToUpdate.Mr;
                    SavePaper.Mrs = ObjToUpdate.Mrs;
                    SavePaper.Co = ObjToUpdate.Co;
                    SavePaper.First = ObjToUpdate.First;
                    SavePaper.FCurrencyID = UpdateHeader.FCurrencyID;
                    SavePaper.ConversionFactor = UpdateHeader.ConversionFactor;
                    SavePaper.InsUserID = UpdateHeader.InsUserID;
                    SavePaper.InsDateTime = DateTime.Now;
                    _unitOfWork.Paper.Add(SavePaper);
                }
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
                UpdateHeader.RowCount = CreditRowNumber;
                _unitOfWork.Header.Update(UpdateHeader);
                _unitOfWork.Complete();
                Msg.Code = 1;
                Msg.Msg = Resources.Resource.UpdatedSuccessfully;

                Msg.Year = UpdateHeader.CompanyYear;
                Msg.FCompanyId = UpdateHeader.CompanyID;
                Msg.VoucherNumber = UpdateHeader.VoucherNumber.ToString();
                Msg.TransactionKindNo = UpdateHeader.TransactionKindNo.ToString();
                Msg.CompanyTransactionKindNo = UpdateHeader.CompanyTransactionKindNo.ToString();
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                Msg.Code = 0;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult UpdateVoucher(string id, int id2, int id3 , int id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var HeaderObj = _unitOfWork.Header.GetHeaderDataById(id, UserInfo.fCompanyId, id2, id3 , id4);
            var CompanyTransactionKindObj = _unitOfWork.CompanyTransactionKind.GetCompanyTransactionKindByID(UserInfo.fCompanyId, HeaderObj.CompanyTransactionKindNo);
            var CompanyTransactionKindID = HeaderObj.CompanyTransactionKindNo;
            var CurrencyObj = _unitOfWork.Currency.GetCurrencyByID(UserInfo.fCompanyId, HeaderObj.FCurrencyID);
            var PostedPaper = _unitOfWork.NativeSql.GetPostedPrintPapersCase(id, id2, UserInfo.fCompanyId, id3, id4);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var TransactionDebitObj = _unitOfWork.NativeSql.GetTransactionsDetailDebit(id, UserInfo.fCompanyId, id2, 1, id4);
            var TransactionCreditObj = _unitOfWork.NativeSql.GetTransactionsDetailCredit(id, UserInfo.fCompanyId, id2, 2 ,id4);
            int iRowNumber = HeaderObj.RowCount;
            var TransFixedVM = new TransactionFixedVM { };
            TransFixedVM.Header = HeaderObj;
            TransFixedVM.TransactionDebit = TransactionDebitObj;
            TransFixedVM.TransactionCredit = TransactionCreditObj;
            TransFixedVM.CompanyTransactionKindID = CompanyTransactionKindID;
            if (Resources.Resource.CurLang == "Arb")
            {
                TransFixedVM.CompanyTransactionKindName = CompanyTransactionKindObj.ArabicName;
                TransFixedVM.CurrencyName = CurrencyObj.ArabicName;
            }
            else
            {
                TransFixedVM.CompanyTransactionKindName = CompanyTransactionKindObj.EnglishName;
                TransFixedVM.CurrencyName = CurrencyObj.EnglishName;
            }
            double SumPaperUsed = _unitOfWork.NativeSql.GetSumPaymnetPaperUsed(UserInfo.fCompanyId, HeaderObj.VoucherNumber, HeaderObj.CompanyTransactionKindNo, HeaderObj.TransactionKindNo, HeaderObj.CompanyYear);
            TransFixedVM.CurrencyID = HeaderObj.FCurrencyID;
            TransFixedVM.CurrencyNewValue = Math.Round(HeaderObj.ConversionFactor, 3);
            TransFixedVM.WorkWithCostCenter = Company.WorkWithCostCenter;
            TransFixedVM.AccountDebitName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionDebitObj.AccountNumber);
            TransFixedVM.CostCenterDebitName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionDebitObj.CostCenter);
            TransFixedVM.CostCenterCreditName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionCreditObj.CostCenter);
            TransFixedVM.Amount = Math.Round(TransactionDebitObj.Debit - SumPaperUsed, 3);
            TransFixedVM.TotalAmount = Math.Round(HeaderObj.TotalDebit - SumPaperUsed, 3);
            TransFixedVM.TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency;
            TransFixedVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            if (HeaderObj.FCurrencyID == 1)
            {
                TransFixedVM.ForeignAmount = Math.Round(TransactionDebitObj.DebitForeign, 3);
                TransFixedVM.TotalForeign = Math.Round(HeaderObj.TotalDebitForeign, 3);
            }
            else
            {
                TransFixedVM.ForeignAmount = Math.Round(TransactionDebitObj.DebitForeign - (SumPaperUsed / HeaderObj.ConversionFactor), 3);
                TransFixedVM.TotalForeign = Math.Round(HeaderObj.TotalDebitForeign - (SumPaperUsed / HeaderObj.ConversionFactor), 3);
            }
            if (PostedPaper.Sirs)
            {
                TransFixedVM.Sirs = true;
                TransFixedVM.Mr = false;
                TransFixedVM.Mrs = false;
                TransFixedVM.Co = false;
                TransFixedVM.First = false;
            }
            else if (PostedPaper.Mr)
            {
                TransFixedVM.Sirs = false;
                TransFixedVM.Mr = true;
                TransFixedVM.Mrs = false;
                TransFixedVM.Co = false;
                TransFixedVM.First = false;
            }
            else if (PostedPaper.Mrs)
            {
                TransFixedVM.Sirs = false;
                TransFixedVM.Mr = false;
                TransFixedVM.Mrs = true;
                TransFixedVM.Co = false;
                TransFixedVM.First = false;
            }
            else if (PostedPaper.Co)
            {
                TransFixedVM.Sirs = false;
                TransFixedVM.Mr = false;
                TransFixedVM.Mrs = false;
                TransFixedVM.Co = true;
                TransFixedVM.First = false;
            }
            else if (PostedPaper.First)
            {
                TransFixedVM.Sirs = false;
                TransFixedVM.Mr = false;
                TransFixedVM.Mrs = false;
                TransFixedVM.Co = false;
                TransFixedVM.First = true;
            }
            return View(TransFixedVM);
        }
        [HttpPost]
        public JsonResult UpdatePaymentVoucherBankUsedPaper(TransactionFixedVM ObjToUpdate)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);

                var UpdateHeader = ObjToUpdate.Header;
                UpdateHeader.InsDateTime = DateTime.Now;
                UpdateHeader.VoucherNumber = ObjToUpdate.Header.VoucherNumber;
                UpdateHeader.VoucherDate = ObjToUpdate.Header.VoucherDate;
                UpdateHeader.CompanyTransactionKindNo = ObjToUpdate.CompanyTransactionKindID;
                UpdateHeader.CompanyYear = ObjToUpdate.CompanyYear;
                UpdateHeader.TransactionKindNo = _unitOfWork.CompanyTransactionKind.GetTransKindForCompanyTransactionKind(UserInfo.fCompanyId, ObjToUpdate.CompanyTransactionKindID);
                _unitOfWork.NativeSql.DeletePaymentPaperNotUsedInTransaction(UserInfo.fCompanyId, UpdateHeader.VoucherNumber, UpdateHeader.CompanyTransactionKindNo, UpdateHeader.TransactionKindNo, UpdateHeader.CompanyYear);
                _unitOfWork.NativeSql.DeletePaymentPaperNotUsed(UserInfo.fCompanyId, UpdateHeader.VoucherNumber, UpdateHeader.CompanyTransactionKindNo, UpdateHeader.TransactionKindNo, UpdateHeader.CompanyYear);
                double SumPaperUsed = _unitOfWork.NativeSql.GetSumPaymnetPaperUsed(UserInfo.fCompanyId, UpdateHeader.VoucherNumber, UpdateHeader.CompanyTransactionKindNo, UpdateHeader.TransactionKindNo, UpdateHeader.CompanyYear);
                int CountPaperUsed = _unitOfWork.NativeSql.GetCountPaymentPaperUsed(UserInfo.fCompanyId, UpdateHeader.VoucherNumber, UpdateHeader.CompanyTransactionKindNo, UpdateHeader.TransactionKindNo, UpdateHeader.CompanyYear);
                UpdateHeader.InsUserID = userId;
                UpdateHeader.CompanyID = UserInfo.fCompanyId;
                UpdateHeader.TotalCredit = ObjToUpdate.Amount + SumPaperUsed; 
                UpdateHeader.TotalDebit = ObjToUpdate.Amount + SumPaperUsed; 
                UpdateHeader.SaleID = ObjToUpdate.SaleManNo;
                double TotalDebitForeign = 0;
                if (UpdateHeader.ConversionFactor == 0)
                {
                    UpdateHeader.ConversionFactor = 1;
                };
                if (UpdateHeader.ConversionFactor == 1)
                {
                    TotalDebitForeign = 0;
                }
                else
                {
                    TotalDebitForeign = UpdateHeader.TotalDebit / UpdateHeader.ConversionFactor;
                };
                UpdateHeader.TotalDebitForeign = TotalDebitForeign;
                UpdateHeader.TotalCreditForeign = TotalDebitForeign;

                UpdateHeader.FCurrencyID = ObjToUpdate.CurrencyID;
                var TransactionDebit = new Transaction();
                TransactionDebit.CompanyID = UserInfo.fCompanyId;
                TransactionDebit.CompanyYear = UpdateHeader.CompanyYear;
                TransactionDebit.CompanyTransactionKindNo = UpdateHeader.CompanyTransactionKindNo;
                TransactionDebit.TransactionKindNo = UpdateHeader.TransactionKindNo;
                TransactionDebit.VoucherDate = UpdateHeader.VoucherDate;
                TransactionDebit.VoucherNumber = UpdateHeader.VoucherNumber;
                TransactionDebit.InsUserID = UpdateHeader.InsUserID;
                TransactionDebit.VHI = UpdateHeader.VHI;
                TransactionDebit.InsDateTime = DateTime.Now;
                TransactionDebit.AccountNumber = ObjToUpdate.FromAccountNumber;
                TransactionDebit.CostCenter = ObjToUpdate.FromCostCenter;
                TransactionDebit.Debit = ObjToUpdate.Amount + SumPaperUsed;
                TransactionDebit.Credit = 0;
                if (UpdateHeader.ConversionFactor == 1)
                {
                    TransactionDebit.DebitForeign = 0;
                }
                else
                {
                    TransactionDebit.DebitForeign = TransactionDebit.Debit / UpdateHeader.ConversionFactor;
                };
                TransactionDebit.CreditForeign = 0;
                TransactionDebit.CreditDebitForeign = TransactionDebit.DebitForeign;
                TransactionDebit.Note = UpdateHeader.Note;
                TransactionDebit.RowNumber = 1;

                _unitOfWork.Transaction.Update(TransactionDebit);
                var CreditRowNumber = 1;
                var iRow = 0;

                var UpdateTransactionRowNumber = _unitOfWork.NativeSql.GetRowNumberPaymentPaperUsedInTransaction(UserInfo.fCompanyId, UpdateHeader.VoucherNumber,
                    UpdateHeader.CompanyTransactionKindNo, UpdateHeader.TransactionKindNo, UpdateHeader.CompanyYear);
                var UpdateRowNumber = _unitOfWork.NativeSql.GetRowNumberPaymentPaperUsed(UserInfo.fCompanyId, UpdateHeader.VoucherNumber,
                    UpdateHeader.CompanyTransactionKindNo, UpdateHeader.TransactionKindNo, UpdateHeader.CompanyYear);

                _unitOfWork.NativeSql.DeleteTransActionCredit(UserInfo.fCompanyId, UpdateHeader.VoucherNumber,
                   UpdateHeader.CompanyTransactionKindNo, UpdateHeader.TransactionKindNo, UpdateHeader.CompanyYear);
                _unitOfWork.NativeSql.DeletePaper(UserInfo.fCompanyId, UpdateHeader.VoucherNumber,
                    UpdateHeader.CompanyTransactionKindNo, UpdateHeader.TransactionKindNo, UpdateHeader.CompanyYear);
                

                foreach (var SavePaper in UpdateRowNumber)
                {
                    if (SavePaper.ChequeNumber == "" && SavePaper.ChequeNumber == null)
                    {
                        continue;
                    }
                    iRow = iRow + 1;
                    SavePaper.RowNumber = iRow;
                    _unitOfWork.Paper.Add(SavePaper);
                }
                foreach (var SaveTransaction in UpdateTransactionRowNumber)
                {
                    if (SaveTransaction.VoucherNumber == "" && SaveTransaction.VoucherNumber == null)
                    {
                        continue;
                    }
                    CreditRowNumber = CreditRowNumber + 1;
                    SaveTransaction.RowNumber = CreditRowNumber;
                    _unitOfWork.Transaction.Add(SaveTransaction);
                }

                foreach (var SavePaper in ObjToUpdate.Paper)
                {
                    if (SavePaper.ChequeNumber == "" && SavePaper.ChequeNumber == null)
                    {
                        continue;
                    }
                    CreditRowNumber = CreditRowNumber + 1;
                    iRow = iRow + 1;
                    Transaction TransactionCredit = new Transaction();
                    TransactionCredit.CompanyID = UserInfo.fCompanyId;
                    TransactionCredit.CompanyYear = UpdateHeader.CompanyYear;
                    TransactionCredit.CompanyTransactionKindNo = UpdateHeader.CompanyTransactionKindNo;
                    TransactionCredit.TransactionKindNo = UpdateHeader.TransactionKindNo;
                    TransactionCredit.VoucherDate = UpdateHeader.VoucherDate;
                    TransactionCredit.VoucherNumber = UpdateHeader.VoucherNumber;
                    TransactionCredit.InsUserID = UpdateHeader.InsUserID;
                    TransactionCredit.VHI = UpdateHeader.VHI;
                    TransactionCredit.InsDateTime = DateTime.Now;
                    TransactionCredit.AccountNumber = SavePaper.AccountNumberSecond;
                    TransactionCredit.CostCenter = ObjToUpdate.ToCostCenter;
                    TransactionCredit.Debit = 0;
                    TransactionCredit.Credit = SavePaper.ChequeAmount;
                    TransactionCredit.DebitForeign = 0;
                    if (UpdateHeader.ConversionFactor == 1)
                    {
                        TransactionCredit.CreditForeign = 0;
                    }
                    else
                    {
                        TransactionCredit.CreditForeign = TransactionCredit.Credit / UpdateHeader.ConversionFactor;
                    };
                    TransactionCredit.CreditDebitForeign = TransactionCredit.CreditForeign;
                    TransactionCredit.Note = UpdateHeader.Note;
                    TransactionCredit.RowNumber = CreditRowNumber;
                    _unitOfWork.Transaction.Add(TransactionCredit);

                    SavePaper.CompanyID = UserInfo.fCompanyId;
                    SavePaper.CompanyYear = UpdateHeader.CompanyYear;
                    SavePaper.StockCode = "*";
                    SavePaper.BranchCode = "*";
                    SavePaper.CompanyTransactionKindNo = UpdateHeader.CompanyTransactionKindNo;
                    SavePaper.TransactionKindNo = UpdateHeader.TransactionKindNo;
                    SavePaper.VoucherDate = UpdateHeader.VoucherDate;
                    SavePaper.VoucherNumber = UpdateHeader.VoucherNumber;
                    SavePaper.VHI = UpdateHeader.VHI;
                    SavePaper.ChequeCase = 14;
                    SavePaper.OldVoucherNumber = "-1";
                    SavePaper.OldCompanyTransactionKindNo = -1;
                    SavePaper.OldTransactionKindNo = -1;
                    SavePaper.OldVHI = -1;
                    SavePaper.sChequeDate = SavePaper.ChequeDate.ToString();
                    SavePaper.OriginalVoucherNumber = UpdateHeader.VoucherNumber;
                    SavePaper.OriginalCompanyTransactionKindNo = UpdateHeader.CompanyTransactionKindNo;
                    SavePaper.OriginalTransactionKindNo = UpdateHeader.TransactionKindNo;
                    SavePaper.OriginalVHI = UpdateHeader.VHI;
                    SavePaper.AccountNumberFirst = ObjToUpdate.FromAccountNumber;
                    SavePaper.AccountNumberSecond = SavePaper.AccountNumberSecond;
                    SavePaper.AccountNumberThird = SavePaper.AccountNumberThird;
                    SavePaper.Remark = UpdateHeader.Note;
                    SavePaper.Sirs = ObjToUpdate.Sirs;
                    SavePaper.Mr = ObjToUpdate.Mr;
                    SavePaper.Mrs = ObjToUpdate.Mrs;
                    SavePaper.Co = ObjToUpdate.Co;
                    SavePaper.First = ObjToUpdate.First;
                    SavePaper.FCurrencyID = UpdateHeader.FCurrencyID;
                    SavePaper.ConversionFactor = UpdateHeader.ConversionFactor;
                    SavePaper.InsUserID = UpdateHeader.InsUserID;
                    SavePaper.InsDateTime = DateTime.Now;
                    SavePaper.RowNumber = iRow;
                    _unitOfWork.Paper.Add(SavePaper);
                }
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
                UpdateHeader.RowCount = CreditRowNumber;
                _unitOfWork.Header.Update(UpdateHeader);
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
        [Authorize(Roles = "CoOwner,CopyPaymentVoucherBank")]
        public ActionResult Copy(string id, int id2, int id3 , int id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            var HeaderObj = _unitOfWork.Header.GetHeaderDataById(id, UserInfo.fCompanyId, id2, id3 , id4);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetPaymentVoucherBankFromTransactionKind(UserInfo.fCompanyId);
            var CompanyTransactionKindID = HeaderObj.CompanyTransactionKindNo;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var TransactionDebitObj = _unitOfWork.NativeSql.GetTransactionsDetailDebit(id, UserInfo.fCompanyId, id2, 1 ,id4);
            var TransactionCreditObj = _unitOfWork.NativeSql.GetTransactionsDetailCredit(id, UserInfo.fCompanyId, id2, 2 , id4);
            var PostedPaper = _unitOfWork.NativeSql.GetPostedPrintPapersCase(id, id2, UserInfo.fCompanyId, id3, HeaderObj.CompanyYear);
            int iRowNumber = HeaderObj.RowCount;
            var TransFixedVM = new TransactionFixedVM { };
            TransFixedVM.Header = HeaderObj;
            TransFixedVM.TransactionDebit = TransactionDebitObj;
            TransFixedVM.TransactionCredit = TransactionCreditObj;
            TransFixedVM.CompanyTransactionKind = CompanyTransactionKindObj;
            TransFixedVM.CompanyTransactionKindID = CompanyTransactionKindID;
            TransFixedVM.Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId);
            TransFixedVM.CurrencyID = HeaderObj.FCurrencyID;
            TransFixedVM.CurrencyNewValue = Math.Round(HeaderObj.ConversionFactor, 3);
            TransFixedVM.WorkWithCostCenter = Company.WorkWithCostCenter;
            TransFixedVM.AccountDebitName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionDebitObj.AccountNumber);
            TransFixedVM.CostCenterDebitName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionDebitObj.CostCenter);
            TransFixedVM.CostCenterCreditName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionCreditObj.CostCenter);
            TransFixedVM.Amount = Math.Round(TransactionDebitObj.Debit, 3);
            TransFixedVM.TotalAmount = Math.Round(HeaderObj.TotalDebit, 3);
            TransFixedVM.ForeignAmount = Math.Round(TransactionDebitObj.DebitForeign, 3);
            TransFixedVM.TotalForeign = Math.Round(HeaderObj.TotalDebitForeign, 3);
            TransFixedVM.TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency;
            TransFixedVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            if (PostedPaper.Sirs)
            {
                TransFixedVM.Sirs = true;
                TransFixedVM.Mr = false;
                TransFixedVM.Mrs = false;
                TransFixedVM.Co = false;
                TransFixedVM.First = false;
            }
            else if (PostedPaper.Mr)
            {
                TransFixedVM.Sirs = false;
                TransFixedVM.Mr = true;
                TransFixedVM.Mrs = false;
                TransFixedVM.Co = false;
                TransFixedVM.First = false;
            }
            else if (PostedPaper.Mrs)
            {
                TransFixedVM.Sirs = false;
                TransFixedVM.Mr = false;
                TransFixedVM.Mrs = true;
                TransFixedVM.Co = false;
                TransFixedVM.First = false;
            }
            else if (PostedPaper.Co)
            {
                TransFixedVM.Sirs = false;
                TransFixedVM.Mr = false;
                TransFixedVM.Mrs = false;
                TransFixedVM.Co = true;
                TransFixedVM.First = false;
            }
            else if (PostedPaper.First)
            {
                TransFixedVM.Sirs = false;
                TransFixedVM.Mr = false;
                TransFixedVM.Mrs = false;
                TransFixedVM.Co = false;
                TransFixedVM.First = true;
            }
            return View(TransFixedVM);
        }
        public ActionResult PaymentCheque()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var PostdatedChequeCount = _unitOfWork.NativeSql.GetCountAllPostdatedCheque(UserInfo.fCompanyId);
            var PostdatedChequeAmount = _unitOfWork.NativeSql.GetSumAllPostdatedCheque(UserInfo.fCompanyId);
            var ReturnPostdatedChequeCount = _unitOfWork.NativeSql.GetCountAllReturnPostdatedCheque(UserInfo.fCompanyId);
            var ReturnPostdatedChequeAmount = _unitOfWork.NativeSql.GetSumAlllReturnPostdatedCheque(UserInfo.fCompanyId);
            var ReturnPaidPostdatedChequeCount = _unitOfWork.NativeSql.GetCountAllReturnPaidPostdatedCheque(UserInfo.fCompanyId);
            var ReturnPaidPostdatedChequeAmount = _unitOfWork.NativeSql.GetSumAlllReturnPaidPostdatedCheque(UserInfo.fCompanyId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var PaperFilter = new PaperFilterVM
            {
                PostdatedChequeCount = PostdatedChequeCount,
                PostdatedChequeAmount = PostdatedChequeAmount,
                ReturnPostdatedChequeCount = ReturnPostdatedChequeCount,
                ReturnPostdatedChequeAmount = ReturnPostdatedChequeAmount,
                ReturnPaidPostdatedChequeCount = ReturnPaidPostdatedChequeCount,
                ReturnPaidPostdatedChequeAmount = ReturnPaidPostdatedChequeAmount,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency
        };
            return View(PaperFilter);
        }

        [Authorize(Roles = "CoOwner,ShowPostdatedCheques")]
        public ActionResult PostdatedCheques()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetPaymentVoucherBankFromTransactionKind(UserInfo.fCompanyId);
            var CompanyTransactionKindToTransferObj = _unitOfWork.NativeSql.GetPaymentBankTransactionCompanyTransactionKind(UserInfo.fCompanyId);
            var FromDate = _unitOfWork.NativeSql.GetFromDatePostdatedCheques(UserInfo.fCompanyId);
            var ToDate = _unitOfWork.NativeSql.GetToDatePostdatedCheques(UserInfo.fCompanyId);
            var HeadrObj = new Header();
            HeadrObj.VoucherDate = DateTime.Now;
            var TransactionDebitObj = new Transaction();
            var TransactionCreditObj = new Transaction();
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var PaperFilter = new PaperFilterVM
            {
                FromDate = FromDate,
                ToDate = ToDate,
                CompanyTransactionKind = CompanyTransactionKindObj,
                CompanyTransactionKindToTransfer = CompanyTransactionKindToTransferObj,
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                FCurrencyID = 1,
                Header = HeadrObj,
                TransactionDebit = TransactionDebitObj,
                TransactionCredit = TransactionCreditObj,
                WorkWithCostCenter = Company.WorkWithCostCenter,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency
            };
            return View(PaperFilter);
        }
        [HttpPost]
        public JsonResult GetAllPostdatedCheque(PaperFilterVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllPaper = _unitOfWork.NativeSql.GetAllPostdatedCheque(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                if (AllPaper == null)
                {
                    return Json(new List<PaperFilterVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.VoucherNumber))
                {
                    AllPaper = AllPaper.Where(m => m.VoucherNumber == Obj.VoucherNumber).ToList();
                }
                if (Obj.CompanyTransactionKindNo != 0)
                {
                    AllPaper = AllPaper.Where(m => m.CompanyTransactionKindNo == Obj.CompanyTransactionKindNo).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.ChequeNumber))
                {
                    AllPaper = AllPaper.Where(m => m.ChequeNumber == Obj.ChequeNumber).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.AccountNumberSecond))
                {
                    AllPaper = AllPaper.Where(m => m.AccountNumberSecond == Obj.AccountNumberSecond).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.AccountNumberThird))
                {
                    AllPaper = AllPaper.Where(m => m.AccountNumberThird == Obj.AccountNumberThird).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.AccountNumberFirst))
                {
                    AllPaper = AllPaper.Where(m => m.AccountNumberFirst == Obj.AccountNumberFirst).ToList();
                }
                if (Obj.FCurrencyID != 0)
                {
                    AllPaper = AllPaper.Where(m => m.FCurrencyID == Obj.FCurrencyID).ToList();
                }
                int iRow = 0;
                foreach (var iRowCount in AllPaper)
                {
                    iRowCount.iRowTable = iRow;
                    iRow = iRow + 1;
                }
                return Json(AllPaper, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<PaperFilterVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult SavePostdatedCheque(PaperFilterVM ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var SaveHeader = ObjToSave.Header;
                SaveHeader.InsDateTime = DateTime.Now;
                SaveHeader.InsUserID = userId;
                var CurrYear = UserInfo.CurrYear;
                SaveHeader.CompanyTransactionKindNo = ObjToSave.CompanyTransactionKindToTransferNo;
                var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                SaveHeader.TransactionKindNo = _unitOfWork.CompanyTransactionKind.GetTransKindForCompanyTransactionKind(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo);
                var ObjComapnyTransactionKind = _unitOfWork.CompanyTransactionKind.GetCompanyTransactionKindByID(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo);
                if (ObjComapnyTransactionKind.AutoSerial)
                {
                    SaveHeader.VoucherNumber = _unitOfWork.Header.GetMaxVHByKind(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo, SaveHeader.TransactionKindNo, CurrYear).ToString();
                    SaveHeader.VHI = int.Parse(SaveHeader.VoucherNumber);
                }
                else
                {
                    string SerialNumber = "";
                    int Serial = ObjComapnyTransactionKind.Serial;
                    string Symbol = ObjComapnyTransactionKind.Symbol;
                    DateTime dDate = SaveHeader.VoucherDate;
                    string YearNo = dDate.ToString("yy");
                    string sMonth = dDate.ToString("MM");
                    int MonthNo = Int16.Parse(sMonth);
                    int LengthLastSerial = _unitOfWork.CompanyTransactionKindMonthlySerial.GetMaxSerial(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo, MonthNo).ToString().Length;
                    Serial = Serial - LengthLastSerial;
                    for (int i = 0; i <= Serial; i++)
                    {
                        if (i < Serial)
                        {
                            SerialNumber = SerialNumber + "0";
                        }
                        else if (i == Serial)
                        {
                            SerialNumber = SerialNumber + _unitOfWork.CompanyTransactionKindMonthlySerial.GetMaxSerial(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo, MonthNo).ToString();
                        }
                    }
                    SaveHeader.VoucherNumber = Symbol + YearNo + sMonth + SerialNumber;
                    SaveHeader.VHI = _unitOfWork.CompanyTransactionKindMonthlySerial.GetMaxSerial(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo, MonthNo);
                    _unitOfWork.CompanyTransactionKindMonthlySerial.Update(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo, MonthNo);
                }
                SaveHeader.CompanyID = UserInfo.fCompanyId;
                SaveHeader.SaleID = 0;
                SaveHeader.Exported = 1;
                SaveHeader.CompanyYear = CurrYear;
                if (ObjToSave.CurrencyID == 0)
                {
                    ObjToSave.CurrencyID = 1;
                }

                SaveHeader.FCurrencyID = ObjToSave.CurrencyID;
                SaveHeader.VHI = SaveHeader.VHI;
                if (Resources.Resource.CurLang == "Arb")
                {
                    SaveHeader.Note = "مسدد من شيكات اجلة الدفع";
                }
                else
                {
                    SaveHeader.Note = "Payment Postdated Cheque";
                }
                double Amount = 0;
                int iRow = 0;

                foreach (var UpdatePaper in ObjToSave.Paper)
                {
                    string sDate = "";
                    var TransactionDebit = new Transaction();
                    var TransactionCredit = new Transaction();
                    sDate = UpdatePaper.sChequeDate;
                    DateTime dDate = Convert.ToDateTime(sDate);
                    var dDateAfterRemoveTime = dDate.Date;

                    string TransNote = "";
                    if (Resources.Resource.CurLang == "Arb")
                    {
                        TransNote = " تسديد شيك رقم : " + UpdatePaper.ChequeNumber + " - " + " تاريخ الحق : " + dDateAfterRemoveTime.ToString("dd/MM/yyyy") + " - " + " الحساب المدفوع له : " +
                            _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, UpdatePaper.AccountNumberFirst);
                    }
                    else
                    {
                        TransNote = " Payment Cheque Number : " + UpdatePaper.ChequeNumber + " - " + " Cheque Date : " + dDateAfterRemoveTime.ToString("dd/MM/yyyy") + " - " + " Paid To Account : " +
                            _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, UpdatePaper.AccountNumberFirst);
                    }
                    UpdatePaper.RowNumber = UpdatePaper.RowNumber;
                    UpdatePaper.CompanyYear = UpdatePaper.CompanyYear;
                    UpdatePaper.CompanyID = UserInfo.fCompanyId;
                    UpdatePaper.OldVoucherNumber = UpdatePaper.VoucherNumber;
                    UpdatePaper.OldCompanyTransactionKindNo = UpdatePaper.CompanyTransactionKindNo;
                    UpdatePaper.OldTransactionKindNo = _unitOfWork.CompanyTransactionKind.GetTransKindForCompanyTransactionKind(UserInfo.fCompanyId, UpdatePaper.OldCompanyTransactionKindNo);
                    UpdatePaper.OldVHI = UpdatePaper.VHI;
                    UpdatePaper.CompanyTransactionKindNo = SaveHeader.CompanyTransactionKindNo;
                    UpdatePaper.TransactionKindNo = SaveHeader.TransactionKindNo;
                    UpdatePaper.VoucherDate = SaveHeader.VoucherDate;
                    UpdatePaper.VoucherNumber = SaveHeader.VoucherNumber;
                    UpdatePaper.VHI = SaveHeader.VHI;
                    UpdatePaper.ChequeCase = 15;
                    UpdatePaper.Remark = SaveHeader.Note;
                    UpdatePaper.InsUserID = SaveHeader.InsUserID;
                    UpdatePaper.InsDateTime = DateTime.Now;
                    Amount = Amount + UpdatePaper.ChequeAmount;

                    iRow = iRow + 1;
                    TransactionDebit.CompanyID = UserInfo.fCompanyId;
                    TransactionDebit.CompanyYear = SaveHeader.CompanyYear;
                    TransactionDebit.CompanyTransactionKindNo = SaveHeader.CompanyTransactionKindNo;
                    TransactionDebit.TransactionKindNo = SaveHeader.TransactionKindNo;
                    TransactionDebit.VoucherDate = SaveHeader.VoucherDate;
                    TransactionDebit.VoucherNumber = SaveHeader.VoucherNumber;
                    TransactionDebit.InsUserID = SaveHeader.InsUserID;
                    TransactionDebit.VHI = SaveHeader.VHI;
                    TransactionDebit.InsDateTime = DateTime.Now;
                    TransactionDebit.AccountNumber = UpdatePaper.AccountNumberSecond;
                    TransactionDebit.CostCenter = ObjToSave.FromCostCenter;
                    TransactionDebit.Debit = UpdatePaper.ChequeAmount;
                    TransactionDebit.Credit = 0;
                    if (SaveHeader.ConversionFactor == 0)
                    {
                        SaveHeader.ConversionFactor = 1;
                    };
                    if (SaveHeader.ConversionFactor == 1)
                    {
                        TransactionDebit.DebitForeign = 0;
                    }
                    else
                    {
                        TransactionDebit.DebitForeign = TransactionDebit.Debit / SaveHeader.ConversionFactor;
                    };
                    TransactionDebit.CreditForeign = 0;
                    TransactionDebit.CreditDebitForeign = TransactionDebit.DebitForeign;

                    TransactionDebit.Note = TransNote;
                    TransactionDebit.RowNumber = iRow;
                    iRow = iRow + 1;
                    TransactionCredit.CompanyID = UserInfo.fCompanyId;
                    TransactionCredit.CompanyYear = SaveHeader.CompanyYear;
                    TransactionCredit.CompanyTransactionKindNo = SaveHeader.CompanyTransactionKindNo;
                    TransactionCredit.TransactionKindNo = SaveHeader.TransactionKindNo;
                    TransactionCredit.VoucherDate = SaveHeader.VoucherDate;
                    TransactionCredit.VoucherNumber = SaveHeader.VoucherNumber;
                    TransactionCredit.InsUserID = SaveHeader.InsUserID;
                    TransactionCredit.VHI = SaveHeader.VHI;
                    TransactionCredit.InsDateTime = DateTime.Now;
                    TransactionCredit.AccountNumber = UpdatePaper.AccountNumberThird;
                    TransactionCredit.CostCenter = ObjToSave.ToCostCenter;
                    TransactionCredit.Debit = 0;
                    TransactionCredit.Credit = UpdatePaper.ChequeAmount;
                    TransactionCredit.DebitForeign = 0;
                    if (SaveHeader.ConversionFactor == 0)
                    {
                        SaveHeader.ConversionFactor = 1;
                    };
                    if (SaveHeader.ConversionFactor == 1)
                    {
                        TransactionCredit.CreditForeign = 0;
                    }
                    else
                    {
                        TransactionCredit.CreditForeign = TransactionCredit.Credit / SaveHeader.ConversionFactor;
                    };
                    TransactionCredit.CreditDebitForeign = TransactionCredit.CreditForeign;
                    TransactionCredit.Note = TransNote;
                    TransactionCredit.RowNumber = iRow;

                    _unitOfWork.NativeSql.SaveTracingPaper(UpdatePaper.CompanyID, UpdatePaper.CompanyYear, UpdatePaper.RowNumber, UpdatePaper.OldVoucherNumber,
                                                           UpdatePaper.OldCompanyTransactionKindNo, UpdatePaper.OldTransactionKindNo, UpdatePaper.ChequeNumber, UpdatePaper.sChequeDate,
                                                           UpdatePaper.ChequeAmount, UpdatePaper.AccountNumberThird);
                    _unitOfWork.NativeSql.UpdatePostdatedCheque(UpdatePaper.CompanyID, UpdatePaper.CompanyYear, UpdatePaper.RowNumber, UpdatePaper.OldVoucherNumber, UpdatePaper.OldCompanyTransactionKindNo,
                        UpdatePaper.OldTransactionKindNo, UpdatePaper.OldVHI, UpdatePaper.CompanyTransactionKindNo, UpdatePaper.TransactionKindNo, UpdatePaper.VoucherDate, UpdatePaper.VoucherNumber,
                        UpdatePaper.VHI, UpdatePaper.ChequeCase, UpdatePaper.Remark, UpdatePaper.InsUserID, UpdatePaper.InsDateTime,
                        UpdatePaper.ChequeNumber, UpdatePaper.sChequeDate, UpdatePaper.ChequeAmount, UpdatePaper.AccountNumberThird);
                    _unitOfWork.Complete();

                    _unitOfWork.Transaction.Add(TransactionDebit);
                    _unitOfWork.Transaction.Add(TransactionCredit);
                    _unitOfWork.Complete();
                }

                SaveHeader.TotalCredit = Amount;
                SaveHeader.TotalDebit = Amount;
                double TotalDebitForeign = 0;
                if (SaveHeader.ConversionFactor == 0)
                {
                    SaveHeader.ConversionFactor = 1;
                };
                if (SaveHeader.ConversionFactor == 1)
                {
                    TotalDebitForeign = 0;
                }
                else
                {
                    TotalDebitForeign = SaveHeader.TotalDebit / SaveHeader.ConversionFactor;
                };
                SaveHeader.TotalDebitForeign = TotalDebitForeign;
                SaveHeader.TotalCreditForeign = TotalDebitForeign;
                SaveHeader.RowCount = iRow;

                _unitOfWork.Header.Add(SaveHeader);
                _unitOfWork.Complete();

                Msg.Code = 1;
                Msg.Msg = Resources.Resource.AddedSuccessfully;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                Msg.Code = 0;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "CoOwner,ShowReturnPostdatedCheques")]
        public ActionResult ReturnPostdatedCheques()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetPaymentVoucherBankFromTransactionKind(UserInfo.fCompanyId);
            var CompanyTransactionKindToTransferObj = _unitOfWork.NativeSql.GetPaymentBankTransactionCompanyTransactionKind(UserInfo.fCompanyId);
            var FromDate = _unitOfWork.NativeSql.GetFromDateReturnPostdatedCheques(UserInfo.fCompanyId);
            var ToDate = _unitOfWork.NativeSql.GetToDateReturnPostdatedCheques(UserInfo.fCompanyId);
            var HeadrObj = new Header();
            HeadrObj.VoucherDate = DateTime.Now;
            var TransactionDebitObj = new Transaction();
            var TransactionCreditObj = new Transaction();
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var PaperFilter = new PaperFilterVM
            {
                FromDate = FromDate,
                ToDate = ToDate,
                CompanyTransactionKind = CompanyTransactionKindObj,
                CompanyTransactionKindToTransfer = CompanyTransactionKindToTransferObj,
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                FCurrencyID = 1,
                Header = HeadrObj,
                TransactionDebit = TransactionDebitObj,
                TransactionCredit = TransactionCreditObj,
                WorkWithCostCenter = Company.WorkWithCostCenter,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency
            };
            return View(PaperFilter);
        }
        [HttpPost]
        public JsonResult GetAllReturnPostdatedCheque(PaperFilterVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllPaper = _unitOfWork.NativeSql.GetAllReturnPostdatedCheque(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                if (AllPaper == null)
                {
                    return Json(new List<PaperFilterVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.VoucherNumber))
                {
                    AllPaper = AllPaper.Where(m => m.VoucherNumber == Obj.VoucherNumber).ToList();
                }
                if (Obj.CompanyTransactionKindNo != 0)
                {
                    AllPaper = AllPaper.Where(m => m.CompanyTransactionKindNo == Obj.CompanyTransactionKindNo).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.ChequeNumber))
                {
                    AllPaper = AllPaper.Where(m => m.ChequeNumber == Obj.ChequeNumber).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.AccountNumberSecond))
                {
                    AllPaper = AllPaper.Where(m => m.AccountNumberSecond == Obj.AccountNumberSecond).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.AccountNumberThird))
                {
                    AllPaper = AllPaper.Where(m => m.AccountNumberThird == Obj.AccountNumberThird).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.AccountNumberFirst))
                {
                    AllPaper = AllPaper.Where(m => m.AccountNumberFirst == Obj.AccountNumberFirst).ToList();
                }
                if (Obj.FCurrencyID != 0)
                {
                    AllPaper = AllPaper.Where(m => m.FCurrencyID == Obj.FCurrencyID).ToList();
                }
                int iRow = 0;
                foreach (var iRowCount in AllPaper)
                {
                    iRowCount.iRowTable = iRow;
                    iRow = iRow + 1;
                }
                return Json(AllPaper, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<PaperFilterVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult SaveReturnPostdatedCheque(PaperFilterVM ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var SaveHeader = ObjToSave.Header;
                SaveHeader.InsDateTime = DateTime.Now;
                SaveHeader.InsUserID = userId;
                var CurrYear = UserInfo.CurrYear;
                SaveHeader.CompanyTransactionKindNo = ObjToSave.CompanyTransactionKindToTransferNo;
                var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                SaveHeader.TransactionKindNo = _unitOfWork.CompanyTransactionKind.GetTransKindForCompanyTransactionKind(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo);
                var ObjComapnyTransactionKind = _unitOfWork.CompanyTransactionKind.GetCompanyTransactionKindByID(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo);
                if (ObjComapnyTransactionKind.AutoSerial)
                {
                    SaveHeader.VoucherNumber = _unitOfWork.Header.GetMaxVHByKind(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo, SaveHeader.TransactionKindNo , CurrYear).ToString();
                    SaveHeader.VHI = int.Parse(SaveHeader.VoucherNumber);
                }
                else
                {
                    string SerialNumber = "";
                    int Serial = ObjComapnyTransactionKind.Serial;
                    string Symbol = ObjComapnyTransactionKind.Symbol;
                    DateTime dDate = SaveHeader.VoucherDate;
                    string YearNo = dDate.ToString("yy");
                    string sMonth = dDate.ToString("MM");
                    int MonthNo = Int16.Parse(sMonth);
                    int LengthLastSerial = _unitOfWork.CompanyTransactionKindMonthlySerial.GetMaxSerial(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo, MonthNo).ToString().Length;
                    Serial = Serial - LengthLastSerial;
                    for (int i = 0; i <= Serial; i++)
                    {
                        if (i < Serial)
                        {
                            SerialNumber = SerialNumber + "0";
                        }
                        else if (i == Serial)
                        {
                            SerialNumber = SerialNumber + _unitOfWork.CompanyTransactionKindMonthlySerial.GetMaxSerial(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo, MonthNo).ToString();
                        }
                    }
                    SaveHeader.VoucherNumber = Symbol + YearNo + sMonth + SerialNumber;
                    SaveHeader.VHI = _unitOfWork.CompanyTransactionKindMonthlySerial.GetMaxSerial(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo, MonthNo);
                    _unitOfWork.CompanyTransactionKindMonthlySerial.Update(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo, MonthNo);
                }
                SaveHeader.CompanyID = UserInfo.fCompanyId;
                SaveHeader.SaleID = 0;
                SaveHeader.Exported = 1;
                SaveHeader.CompanyYear = CurrYear;
                if (ObjToSave.CurrencyID == 0)
                {
                    ObjToSave.CurrencyID = 1;
                }

                SaveHeader.FCurrencyID = ObjToSave.CurrencyID;
                SaveHeader.VHI = SaveHeader.VHI;
                if (Resources.Resource.CurLang == "Arb")
                {
                    SaveHeader.Note = "إرجاع شيكات اجلة";
                }
                else
                {
                    SaveHeader.Note = "Return Postdated Cheque";
                }
                double Amount = 0;
                int iRow = 0;

                foreach (var UpdatePaper in ObjToSave.Paper)
                {
                    string sDate = "";
                    var TransactionDebit = new Transaction();
                    var TransactionCredit = new Transaction();
                    sDate = UpdatePaper.sChequeDate;
                    DateTime dDate = Convert.ToDateTime(sDate);
                    var dDateAfterRemoveTime = dDate.Date;

                    string TransNote = "";
                    if (Resources.Resource.CurLang == "Arb")
                    {
                        TransNote = " إرجاع شيك رقم : " + UpdatePaper.ChequeNumber + " - " + " تاريخ الحق : " + dDateAfterRemoveTime.ToString("dd/MM/yyyy") + " - " + " الحساب المدفوع له : " +
                            _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, UpdatePaper.AccountNumberFirst);
                    }
                    else
                    {
                        TransNote = " Return Cheque Number : " + UpdatePaper.ChequeNumber + " - " + " Cheque Date : " + dDateAfterRemoveTime.ToString("dd/MM/yyyy") + " - " + " Paid To Account : " +
                            _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, UpdatePaper.AccountNumberFirst);
                    }
                    UpdatePaper.RowNumber = UpdatePaper.RowNumber;
                    UpdatePaper.CompanyYear = UpdatePaper.CompanyYear;
                    UpdatePaper.CompanyID = UserInfo.fCompanyId;
                    UpdatePaper.OldVoucherNumber = UpdatePaper.VoucherNumber;
                    UpdatePaper.PaymentReturnNote = ObjToSave.ReturnNote;
                    UpdatePaper.OldCompanyTransactionKindNo = UpdatePaper.CompanyTransactionKindNo;
                    UpdatePaper.OldTransactionKindNo = _unitOfWork.CompanyTransactionKind.GetTransKindForCompanyTransactionKind(UserInfo.fCompanyId, UpdatePaper.OldCompanyTransactionKindNo);
                    UpdatePaper.OldVHI = UpdatePaper.VHI;
                    UpdatePaper.CompanyTransactionKindNo = SaveHeader.CompanyTransactionKindNo;
                    UpdatePaper.TransactionKindNo = SaveHeader.TransactionKindNo;
                    UpdatePaper.VoucherDate = SaveHeader.VoucherDate;
                    UpdatePaper.VoucherNumber = SaveHeader.VoucherNumber;
                    UpdatePaper.VHI = SaveHeader.VHI;
                    UpdatePaper.ChequeCase = 16;
                    UpdatePaper.Remark = SaveHeader.Note;
                    UpdatePaper.InsUserID = SaveHeader.InsUserID;
                    UpdatePaper.InsDateTime = DateTime.Now;
                    Amount = Amount + UpdatePaper.ChequeAmount;

                    iRow = iRow + 1;
                    TransactionDebit.CompanyID = UserInfo.fCompanyId;
                    TransactionDebit.CompanyYear = SaveHeader.CompanyYear;
                    TransactionDebit.CompanyTransactionKindNo = SaveHeader.CompanyTransactionKindNo;
                    TransactionDebit.TransactionKindNo = SaveHeader.TransactionKindNo;
                    TransactionDebit.VoucherDate = SaveHeader.VoucherDate;
                    TransactionDebit.VoucherNumber = SaveHeader.VoucherNumber;
                    TransactionDebit.InsUserID = SaveHeader.InsUserID;
                    TransactionDebit.VHI = SaveHeader.VHI;
                    TransactionDebit.InsDateTime = DateTime.Now;
                    TransactionDebit.AccountNumber = UpdatePaper.AccountNumberSecond;
                    TransactionDebit.CostCenter = ObjToSave.FromCostCenter;
                    TransactionDebit.Debit = UpdatePaper.ChequeAmount;
                    TransactionDebit.Credit = 0;
                    if (SaveHeader.ConversionFactor == 0)
                    {
                        SaveHeader.ConversionFactor = 1;
                    };
                    if (SaveHeader.ConversionFactor == 1)
                    {
                        TransactionDebit.DebitForeign = 0;
                    }
                    else
                    {
                        TransactionDebit.DebitForeign = TransactionDebit.Debit / SaveHeader.ConversionFactor;
                    };
                    TransactionDebit.CreditForeign = 0;
                    TransactionDebit.CreditDebitForeign = TransactionDebit.DebitForeign;

                    TransactionDebit.Note = TransNote;
                    TransactionDebit.RowNumber = iRow;
                    iRow = iRow + 1;
                    TransactionCredit.CompanyID = UserInfo.fCompanyId;
                    TransactionCredit.CompanyYear = SaveHeader.CompanyYear;
                    TransactionCredit.CompanyTransactionKindNo = SaveHeader.CompanyTransactionKindNo;
                    TransactionCredit.TransactionKindNo = SaveHeader.TransactionKindNo;
                    TransactionCredit.VoucherDate = SaveHeader.VoucherDate;
                    TransactionCredit.VoucherNumber = SaveHeader.VoucherNumber;
                    TransactionCredit.InsUserID = SaveHeader.InsUserID;
                    TransactionCredit.VHI = SaveHeader.VHI;
                    TransactionCredit.InsDateTime = DateTime.Now;
                    TransactionCredit.AccountNumber = UpdatePaper.AccountNumberFirst;
                    TransactionCredit.CostCenter = ObjToSave.ToCostCenter;
                    TransactionCredit.Debit = 0;
                    TransactionCredit.Credit = UpdatePaper.ChequeAmount;
                    TransactionCredit.DebitForeign = 0;
                    if (SaveHeader.ConversionFactor == 0)
                    {
                        SaveHeader.ConversionFactor = 1;
                    };
                    if (SaveHeader.ConversionFactor == 1)
                    {
                        TransactionCredit.CreditForeign = 0;
                    }
                    else
                    {
                        TransactionCredit.CreditForeign = TransactionCredit.Credit / SaveHeader.ConversionFactor;
                    };
                    TransactionCredit.CreditDebitForeign = TransactionCredit.CreditForeign;
                    TransactionCredit.Note = TransNote;
                    TransactionCredit.RowNumber = iRow;
                    _unitOfWork.NativeSql.SaveTracingPaper(UpdatePaper.CompanyID, UpdatePaper.CompanyYear, UpdatePaper.RowNumber, UpdatePaper.OldVoucherNumber,
                                                           UpdatePaper.OldCompanyTransactionKindNo, UpdatePaper.OldTransactionKindNo, UpdatePaper.ChequeNumber, UpdatePaper.sChequeDate,
                                                           UpdatePaper.ChequeAmount, UpdatePaper.AccountNumberThird);
                    _unitOfWork.NativeSql.UpdateReturnPostdatedCheque(UpdatePaper.CompanyID, UpdatePaper.CompanyYear, UpdatePaper.RowNumber, UpdatePaper.OldVoucherNumber, UpdatePaper.OldCompanyTransactionKindNo,
                        UpdatePaper.OldTransactionKindNo, UpdatePaper.OldVHI, UpdatePaper.CompanyTransactionKindNo, UpdatePaper.TransactionKindNo, UpdatePaper.VoucherDate, UpdatePaper.VoucherNumber,
                        UpdatePaper.VHI, UpdatePaper.ChequeCase, UpdatePaper.Remark, UpdatePaper.InsUserID, UpdatePaper.InsDateTime,
                        UpdatePaper.ChequeNumber, UpdatePaper.sChequeDate, UpdatePaper.ChequeAmount, UpdatePaper.AccountNumberThird, UpdatePaper.PaymentReturnNote);
                    _unitOfWork.Complete();

                    _unitOfWork.Transaction.Add(TransactionDebit);
                    _unitOfWork.Transaction.Add(TransactionCredit);
                    _unitOfWork.Complete();
                }

                SaveHeader.TotalCredit = Amount;
                SaveHeader.TotalDebit = Amount;
                double TotalDebitForeign = 0;
                if (SaveHeader.ConversionFactor == 0)
                {
                    SaveHeader.ConversionFactor = 1;
                };
                if (SaveHeader.ConversionFactor == 1)
                {
                    TotalDebitForeign = 0;
                }
                else
                {
                    TotalDebitForeign = SaveHeader.TotalDebit / SaveHeader.ConversionFactor;
                };
                SaveHeader.TotalDebitForeign = TotalDebitForeign;
                SaveHeader.TotalCreditForeign = TotalDebitForeign;
                SaveHeader.RowCount = iRow;

                _unitOfWork.Header.Add(SaveHeader);
                _unitOfWork.Complete();

                Msg.Code = 1;
                Msg.Msg = Resources.Resource.AddedSuccessfully; 
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                Msg.Code = 0;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "CoOwner,ShowReturnPaidPostdatedCheques")]
        public ActionResult ReturnPaidPostdatedCheques()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetPaymentVoucherBankFromTransactionKind(UserInfo.fCompanyId);
            var CompanyTransactionKindToTransferObj = _unitOfWork.NativeSql.GetPaymentBankTransactionCompanyTransactionKind(UserInfo.fCompanyId);
            var FromDate = _unitOfWork.NativeSql.GetFromDateReturnPaidPostdatedCheques(UserInfo.fCompanyId);
            var ToDate = _unitOfWork.NativeSql.GetToDateReturnPaidPostdatedCheques(UserInfo.fCompanyId);
            var HeadrObj = new Header();
            HeadrObj.VoucherDate = DateTime.Now;
            var TransactionDebitObj = new Transaction();
            var TransactionCreditObj = new Transaction();
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var PaperFilter = new PaperFilterVM
            {
                FromDate = FromDate,
                ToDate = ToDate,
                CompanyTransactionKind = CompanyTransactionKindObj,
                CompanyTransactionKindToTransfer = CompanyTransactionKindToTransferObj,
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                FCurrencyID = 1,
                Header = HeadrObj,
                TransactionDebit = TransactionDebitObj,
                TransactionCredit = TransactionCreditObj,
                WorkWithCostCenter = Company.WorkWithCostCenter,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency
            };
            return View(PaperFilter);
        }
        [HttpPost]
        public JsonResult GetAllReturnPaidPostdatedCheque(PaperFilterVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllPaper = _unitOfWork.NativeSql.GetAllReturnPaidPostdatedCheque(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                if (AllPaper == null)
                {
                    return Json(new List<PaperFilterVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.VoucherNumber))
                {
                    AllPaper = AllPaper.Where(m => m.VoucherNumber == Obj.VoucherNumber).ToList();
                }
                if (Obj.CompanyTransactionKindNo != 0)
                {
                    AllPaper = AllPaper.Where(m => m.CompanyTransactionKindNo == Obj.CompanyTransactionKindNo).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.ChequeNumber))
                {
                    AllPaper = AllPaper.Where(m => m.ChequeNumber == Obj.ChequeNumber).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.AccountNumberSecond))
                {
                    AllPaper = AllPaper.Where(m => m.AccountNumberSecond == Obj.AccountNumberSecond).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.AccountNumberThird))
                {
                    AllPaper = AllPaper.Where(m => m.AccountNumberThird == Obj.AccountNumberThird).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.AccountNumberFirst))
                {
                    AllPaper = AllPaper.Where(m => m.AccountNumberFirst == Obj.AccountNumberFirst).ToList();
                }
                if (Obj.FCurrencyID != 0)
                {
                    AllPaper = AllPaper.Where(m => m.FCurrencyID == Obj.FCurrencyID).ToList();
                }
                int iRow = 0;
                foreach (var iRowCount in AllPaper)
                {
                    iRowCount.iRowTable = iRow;
                    iRow = iRow + 1;
                }
                return Json(AllPaper, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<PaperFilterVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult SaveReturnPaidPostdatedCheque(PaperFilterVM ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var SaveHeader = ObjToSave.Header;
                SaveHeader.InsDateTime = DateTime.Now;
                SaveHeader.InsUserID = userId;
                var CurrYear = UserInfo.CurrYear;
                SaveHeader.CompanyTransactionKindNo = ObjToSave.CompanyTransactionKindToTransferNo;
                var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                SaveHeader.TransactionKindNo = _unitOfWork.CompanyTransactionKind.GetTransKindForCompanyTransactionKind(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo);
                var ObjComapnyTransactionKind = _unitOfWork.CompanyTransactionKind.GetCompanyTransactionKindByID(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo);
                if (ObjComapnyTransactionKind.AutoSerial)
                {
                    SaveHeader.VoucherNumber = _unitOfWork.Header.GetMaxVHByKind(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo, SaveHeader.TransactionKindNo , CurrYear).ToString();
                    SaveHeader.VHI = int.Parse(SaveHeader.VoucherNumber);
                }
                else
                {
                    string SerialNumber = "";
                    int Serial = ObjComapnyTransactionKind.Serial;
                    string Symbol = ObjComapnyTransactionKind.Symbol;
                    DateTime dDate = SaveHeader.VoucherDate;
                    string YearNo = dDate.ToString("yy");
                    string sMonth = dDate.ToString("MM");
                    int MonthNo = Int16.Parse(sMonth);
                    int LengthLastSerial = _unitOfWork.CompanyTransactionKindMonthlySerial.GetMaxSerial(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo, MonthNo).ToString().Length;
                    Serial = Serial - LengthLastSerial;
                    for (int i = 0; i <= Serial; i++)
                    {
                        if (i < Serial)
                        {
                            SerialNumber = SerialNumber + "0";
                        }
                        else if (i == Serial)
                        {
                            SerialNumber = SerialNumber + _unitOfWork.CompanyTransactionKindMonthlySerial.GetMaxSerial(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo, MonthNo).ToString();
                        }
                    }
                    SaveHeader.VoucherNumber = Symbol + YearNo + sMonth + SerialNumber;
                    SaveHeader.VHI = _unitOfWork.CompanyTransactionKindMonthlySerial.GetMaxSerial(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo, MonthNo);
                    _unitOfWork.CompanyTransactionKindMonthlySerial.Update(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo, MonthNo);
                }
                SaveHeader.CompanyID = UserInfo.fCompanyId;
                SaveHeader.SaleID = 0;
                SaveHeader.Exported = 1;
                SaveHeader.CompanyYear = CurrYear;
                if (ObjToSave.CurrencyID == 0)
                {
                    ObjToSave.CurrencyID = 1;
                }

                SaveHeader.FCurrencyID = ObjToSave.CurrencyID;
                SaveHeader.VHI = SaveHeader.VHI;
                if (Resources.Resource.CurLang == "Arb")
                {
                    SaveHeader.Note = "إرجاع شيك اجل مسدد";
                }
                else
                {
                    SaveHeader.Note = "Return Paid Postdated Cheque";
                }
                double Amount = 0;
                int iRow = 0;

                foreach (var UpdatePaper in ObjToSave.Paper)
                {
                    string sDate = "";
                    var TransactionDebit = new Transaction();
                    var TransactionCredit = new Transaction();
                    sDate = UpdatePaper.sChequeDate;
                    DateTime dDate = Convert.ToDateTime(sDate);
                    var dDateAfterRemoveTime = dDate.Date;

                    string TransNote = "";
                    if (Resources.Resource.CurLang == "Arb")
                    {
                        TransNote = " إرجاع شيك مسدد رقم  : " + UpdatePaper.ChequeNumber + " - " + " تاريخ الحق : " + dDateAfterRemoveTime.ToString("dd/MM/yyyy") + " - " + " الحساب المدفوع له : " +
                            _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, UpdatePaper.AccountNumberFirst);
                    }
                    else
                    {
                        TransNote = " Return Paid Cheque Number : " + UpdatePaper.ChequeNumber + " - " + " Cheque Date : " + dDateAfterRemoveTime.ToString("dd/MM/yyyy") + " - " + " Paid To Account : " +
                            _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, UpdatePaper.AccountNumberFirst);
                    }
                    UpdatePaper.RowNumber = UpdatePaper.RowNumber;
                    UpdatePaper.CompanyYear = UpdatePaper.CompanyYear;
                    UpdatePaper.CompanyID = UserInfo.fCompanyId;
                    UpdatePaper.OldVoucherNumber = UpdatePaper.VoucherNumber;
                    UpdatePaper.PaymentPaidReturnNote = ObjToSave.ReturnNote;
                    UpdatePaper.OldCompanyTransactionKindNo = UpdatePaper.CompanyTransactionKindNo;
                    UpdatePaper.OldTransactionKindNo = _unitOfWork.CompanyTransactionKind.GetTransKindForCompanyTransactionKind(UserInfo.fCompanyId, UpdatePaper.OldCompanyTransactionKindNo);
                    UpdatePaper.OldVHI = UpdatePaper.VHI;
                    UpdatePaper.CompanyTransactionKindNo = SaveHeader.CompanyTransactionKindNo;
                    UpdatePaper.TransactionKindNo = SaveHeader.TransactionKindNo;
                    UpdatePaper.VoucherDate = SaveHeader.VoucherDate;
                    UpdatePaper.VoucherNumber = SaveHeader.VoucherNumber;
                    UpdatePaper.VHI = SaveHeader.VHI;
                    UpdatePaper.ChequeCase = 17;
                    UpdatePaper.Remark = SaveHeader.Note;
                    UpdatePaper.InsUserID = SaveHeader.InsUserID;
                    UpdatePaper.InsDateTime = DateTime.Now;
                    Amount = Amount + UpdatePaper.ChequeAmount;

                    iRow = iRow + 1;
                    TransactionDebit.CompanyID = UserInfo.fCompanyId;
                    TransactionDebit.CompanyYear = SaveHeader.CompanyYear;
                    TransactionDebit.CompanyTransactionKindNo = SaveHeader.CompanyTransactionKindNo;
                    TransactionDebit.TransactionKindNo = SaveHeader.TransactionKindNo;
                    TransactionDebit.VoucherDate = SaveHeader.VoucherDate;
                    TransactionDebit.VoucherNumber = SaveHeader.VoucherNumber;
                    TransactionDebit.InsUserID = SaveHeader.InsUserID;
                    TransactionDebit.VHI = SaveHeader.VHI;
                    TransactionDebit.InsDateTime = DateTime.Now;
                    TransactionDebit.AccountNumber = UpdatePaper.AccountNumberThird;
                    TransactionDebit.CostCenter = ObjToSave.FromCostCenter;
                    TransactionDebit.Debit = UpdatePaper.ChequeAmount;
                    TransactionDebit.Credit = 0;
                    if (SaveHeader.ConversionFactor == 0)
                    {
                        SaveHeader.ConversionFactor = 1;
                    };
                    if (SaveHeader.ConversionFactor == 1)
                    {
                        TransactionDebit.DebitForeign = 0;
                    }
                    else
                    {
                        TransactionDebit.DebitForeign = TransactionDebit.Debit / SaveHeader.ConversionFactor;
                    };
                    TransactionDebit.CreditForeign = 0;
                    TransactionDebit.CreditDebitForeign = TransactionDebit.DebitForeign;

                    TransactionDebit.Note = TransNote;
                    TransactionDebit.RowNumber = iRow;
                    iRow = iRow + 1;
                    TransactionCredit.CompanyID = UserInfo.fCompanyId;
                    TransactionCredit.CompanyYear = SaveHeader.CompanyYear;
                    TransactionCredit.CompanyTransactionKindNo = SaveHeader.CompanyTransactionKindNo;
                    TransactionCredit.TransactionKindNo = SaveHeader.TransactionKindNo;
                    TransactionCredit.VoucherDate = SaveHeader.VoucherDate;
                    TransactionCredit.VoucherNumber = SaveHeader.VoucherNumber;
                    TransactionCredit.InsUserID = SaveHeader.InsUserID;
                    TransactionCredit.VHI = SaveHeader.VHI;
                    TransactionCredit.InsDateTime = DateTime.Now;
                    TransactionCredit.AccountNumber = UpdatePaper.AccountNumberFirst;
                    TransactionCredit.CostCenter = ObjToSave.ToCostCenter;
                    TransactionCredit.Debit = 0;
                    TransactionCredit.Credit = UpdatePaper.ChequeAmount;
                    TransactionCredit.DebitForeign = 0;
                    if (SaveHeader.ConversionFactor == 0)
                    {
                        SaveHeader.ConversionFactor = 1;
                    };
                    if (SaveHeader.ConversionFactor == 1)
                    {
                        TransactionCredit.CreditForeign = 0;
                    }
                    else
                    {
                        TransactionCredit.CreditForeign = TransactionCredit.Credit / SaveHeader.ConversionFactor;
                    };
                    TransactionCredit.CreditDebitForeign = TransactionCredit.CreditForeign;
                    TransactionCredit.Note = TransNote;
                    TransactionCredit.RowNumber = iRow;
                    _unitOfWork.NativeSql.SaveTracingPaper(UpdatePaper.CompanyID, UpdatePaper.CompanyYear, UpdatePaper.RowNumber, UpdatePaper.OldVoucherNumber,
                                                           UpdatePaper.OldCompanyTransactionKindNo, UpdatePaper.OldTransactionKindNo, UpdatePaper.ChequeNumber, UpdatePaper.sChequeDate,
                                                           UpdatePaper.ChequeAmount, UpdatePaper.AccountNumberThird);
                    _unitOfWork.NativeSql.UpdateReturnPaidPostdatedCheque(UpdatePaper.CompanyID, UpdatePaper.CompanyYear, UpdatePaper.RowNumber, UpdatePaper.OldVoucherNumber, UpdatePaper.OldCompanyTransactionKindNo,
                        UpdatePaper.OldTransactionKindNo, UpdatePaper.OldVHI, UpdatePaper.CompanyTransactionKindNo, UpdatePaper.TransactionKindNo, UpdatePaper.VoucherDate, UpdatePaper.VoucherNumber,
                        UpdatePaper.VHI, UpdatePaper.ChequeCase, UpdatePaper.Remark, UpdatePaper.InsUserID, UpdatePaper.InsDateTime,
                        UpdatePaper.ChequeNumber, UpdatePaper.sChequeDate, UpdatePaper.ChequeAmount, UpdatePaper.AccountNumberThird,UpdatePaper.PaymentPaidReturnNote);
                    _unitOfWork.Complete();

                    _unitOfWork.Transaction.Add(TransactionDebit);
                    _unitOfWork.Transaction.Add(TransactionCredit);
                    _unitOfWork.Complete();
                }

                SaveHeader.TotalCredit = Amount;
                SaveHeader.TotalDebit = Amount;
                double TotalDebitForeign = 0;
                if (SaveHeader.ConversionFactor == 0)
                {
                    SaveHeader.ConversionFactor = 1;
                };
                if (SaveHeader.ConversionFactor == 1)
                {
                    TotalDebitForeign = 0;
                }
                else
                {
                    TotalDebitForeign = SaveHeader.TotalDebit / SaveHeader.ConversionFactor;
                };
                SaveHeader.TotalDebitForeign = TotalDebitForeign;
                SaveHeader.TotalCreditForeign = TotalDebitForeign;
                SaveHeader.RowCount = iRow;

                _unitOfWork.Header.Add(SaveHeader);
                _unitOfWork.Complete();

                Msg.Code = 1;
                Msg.Msg = Resources.Resource.AddedSuccessfully;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                Msg.Code = 0;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "CoOwner,RepPaymentChequesReport")]
        public ActionResult ChequesReport()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var FromDate = _unitOfWork.NativeSql.GetFromDateAllPostdatedCheque(UserInfo.fCompanyId);
            var ToDate = _unitOfWork.NativeSql.GetToDateAllPostdatedCheque(UserInfo.fCompanyId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var PaperFilter = new PaperFilterVM
            {
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency
            };
            return View(PaperFilter);
        }
        [HttpPost]
        public JsonResult GetAllChequesReport(PaperFilterVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllPaper = _unitOfWork.NativeSql.GetAllPaymentCheques(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                List<PaperFilterVM> AllData = new List<PaperFilterVM>();
                List<PaperFilterVM> Ch14 = new List<PaperFilterVM>();
                List<PaperFilterVM> Ch15 = new List<PaperFilterVM>();
                List<PaperFilterVM> Ch16 = new List<PaperFilterVM>();
                List<PaperFilterVM> Ch17 = new List<PaperFilterVM>();
                if (AllPaper == null)
                {
                    return Json(new List<PaperFilterVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.ChequeNumber))
                {
                    AllPaper = AllPaper.Where(m => m.ChequeNumber == Obj.ChequeNumber).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.PaidToAccountNumber))
                {
                    AllPaper = AllPaper.Where(m => m.PaidToAccountNumber == Obj.PaidToAccountNumber).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.BankAccountNumber))
                {
                    AllPaper = AllPaper.Where(m => m.BankAccountNumber == Obj.BankAccountNumber).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.PostdatedAccountNumber))
                {
                    AllPaper = AllPaper.Where(m => m.PostdatedAccountNumber == Obj.PostdatedAccountNumber).ToList();
                }
                if (Obj.PostdatedCheques)
                {
                    Ch14 = AllPaper.Where(m => m.ChequeCase == 14).ToList();
                }
                if (Obj.PaidPostdatedCheque)
                {
                    Ch15 = AllPaper.Where(m => m.ChequeCase == 15).ToList();
                }
                if (Obj.ReturnPostdatedCheque)
                {
                    Ch16 = AllPaper.Where(m => m.ChequeCase == 16).ToList();
                }
                if (Obj.ReturnPaidPostdatedCheque)
                {
                    Ch17 = AllPaper.Where(m => m.ChequeCase == 17).ToList();
                }

                if (Ch14.Count > 0)
                {
                    foreach (var c in Ch14)
                    {
                        AllData.Add(c);
                    }
                }
                if (Ch15.Count > 0)
                {
                    foreach (var c in Ch15)
                    {
                        AllData.Add(c);
                    }
                }
                if (Ch16.Count > 0)
                {
                    foreach (var c in Ch16)
                    {
                        AllData.Add(c);
                    }
                }
                if (Ch17.Count > 0)
                {
                    foreach (var c in Ch17)
                    {
                        AllData.Add(c);
                    }
                }

                if (Obj.PostdatedCheques == false && Obj.PaidPostdatedCheque == false && Obj.ReturnPostdatedCheque == false && Obj.ReturnPaidPostdatedCheque == false)
                {
                    return Json(AllPaper, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(AllData, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<PaperFilterVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [Authorize(Roles = "CoOwner,RepPaymTrankingChequesReport")]
        public ActionResult TrankingChequesReport()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var FromDate = _unitOfWork.NativeSql.GetFromDateAllPostdatedCheque(UserInfo.fCompanyId);
            var ToDate = _unitOfWork.NativeSql.GetToDateAllPostdatedCheque(UserInfo.fCompanyId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var PaperFilter = new PaperFilterVM
            {
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency
            };
            return View(PaperFilter);
        }
        [HttpPost]
        public JsonResult GetTrankingChequesReport(PaperFilterVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllPaper = _unitOfWork.NativeSql.GetTrankingPaymentChequesReport(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                if (AllPaper == null)
                {
                    return Json(new List<PaperFilterVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.ChequeNumber))
                {
                    AllPaper = AllPaper.Where(m => m.ChequeNumber == Obj.ChequeNumber).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.PaidToAccountNumber))
                {
                    AllPaper = AllPaper.Where(m => m.PaidToAccountNumber == Obj.PaidToAccountNumber).ToList();
                }
                return Json(AllPaper, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<PaperFilterVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public ActionResult ShowPaperDetails(string id, string id2, int id3, double id4, int id5, string id6, int id7, int id8, string id9)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var PaperFilterVM = new PaperFilterVM
            {
                ChequeNumber = id,
                OriginalVoucherNumber = id2,
                OriginalCompanyTransactionKindNo = id3,
                ChequeAmount = id4,
                RowNumber = id5,
                AccountNumberThird = id6,
                OriginalTransactionKindNo = id7,
                CompanyYear = id8,
                PaidToAccountNumber = id9,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency
            };
            return PartialView(PaperFilterVM);
        }
        [HttpGet]
        public JsonResult GetPaperDetails(string id, string id2, int id3, double id4, int id5, string id6, int id7, int id8, string id9)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var ShowPaperDetails = _unitOfWork.NativeSql.GetPaymentChequeDetails(UserInfo.fCompanyId, id, id2, id3, id4, id5, id6, id7, id8, id9);
                return Json(ShowPaperDetails, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<PaperFilterVM>(), JsonRequestBehavior.AllowGet);
            }

        }

    }
}