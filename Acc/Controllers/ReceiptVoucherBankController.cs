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
    public class ReceiptVoucherBankController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public ReceiptVoucherBankController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        [Authorize(Roles = "CoOwner,ShowReceiptVoucherBank")]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetReceiptVoucherBankFromTransactionKind(UserInfo.fCompanyId);
            var SaleObj = _unitOfWork.Sale.GetAllSale(UserInfo.fCompanyId);
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
            var ReceiptBankFilter = new ReceiptBankFilterVM
            {
                FromDate = FromDate,
                ToDate = ToDate,
                CompanyTransactionKind = CompanyTransactionKindObj,
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                Sale = SaleObj,
                CompanyYear = UserInfo.CurrYear
            };
            return View(ReceiptBankFilter);
        }
        [HttpPost]
        public JsonResult GetAllReceiptVoucherBank(ReceiptBankFilterVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllReceiptBank = _unitOfWork.NativeSql.GetAllReceiptBankFromHeader(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                if (AllReceiptBank == null)
                {
                    return Json(new List<ReceiptBankFilterVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.VoucherNumber))
                {
                    AllReceiptBank = AllReceiptBank.Where(m => m.VoucherNumber == Obj.VoucherNumber).ToList();
                }
                if (Obj.CurrencyID != 0)
                {
                    AllReceiptBank = AllReceiptBank.Where(m => m.CurrencyID == Obj.CurrencyID).ToList();
                }
                if (Obj.SaleID != 0)
                {
                    AllReceiptBank = AllReceiptBank.Where(m => m.SaleID == Obj.SaleID).ToList();
                }
                if (Obj.CompanyTransactionKindNo != 0)
                {
                    AllReceiptBank = AllReceiptBank.Where(m => m.CompanyTransactionKindNo == Obj.CompanyTransactionKindNo).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.PaidAccountNumber))
                {
                    AllReceiptBank = AllReceiptBank.Where(m => m.PaidAccountNumber == Obj.PaidAccountNumber).ToList();
                }
                return Json(AllReceiptBank, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<ReceiptBankFilterVM>(), JsonRequestBehavior.AllowGet);
            }

        }

        [Authorize(Roles = "CoOwner,AddReceiptVoucherBank")]
        public ActionResult AddNew()
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var HeadrObj = new Header();
            HeadrObj.VoucherDate = DateTime.Now;
            var TransactionDebitObj = new Transaction();
            var TransactionCreditObj = new Transaction();
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetReceiptVoucherBankFromTransactionKind(UserInfo.fCompanyId);
            var TransFixedVM = new TransactionFixedVM
            {
                Header = HeadrObj,
                TransactionDebit = TransactionDebitObj,
                TransactionCredit = TransactionCreditObj,
                CompanyTransactionKind = CompanyTransactionKindObj,
                CompanyTransactionKindID = 1,
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                CurrencyID = 1,
                SaleMan = _unitOfWork.Sale.GetAllSale(UserInfo.fCompanyId),
                WorkWithCostCenter = Company.WorkWithCostCenter,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                CurrentYear = UserInfo.CurrYear
            };
            return View(TransFixedVM);
        }
        public JsonResult SaveReceiptVoucherBank(TransactionFixedVM ObjToSave)
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
                SaveHeader.CompanyTransactionKindNo = ObjToSave.CompanyTransactionKindID;
                SaveHeader.CompanyYear = ObjToSave.Header.CompanyYear;
                SaveHeader.TransactionKindNo = _unitOfWork.CompanyTransactionKind.GetTransKindForCompanyTransactionKind(UserInfo.fCompanyId, ObjToSave.CompanyTransactionKindID);
                SaveHeader.CompanyID = UserInfo.fCompanyId;
                SaveHeader.TotalCredit = ObjToSave.Amount;
                SaveHeader.TotalDebit = ObjToSave.Amount;
                SaveHeader.SaleID = ObjToSave.SaleManNo;
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

                SaveHeader.CompanyYear = UserInfo.CurrYear;
                SaveHeader.FCurrencyID = ObjToSave.CurrencyID;

                var ObjComapnyTransactionKind = _unitOfWork.CompanyTransactionKind.GetCompanyTransactionKindByID(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo);
                if (ObjComapnyTransactionKind.AutoSerial)
                {
                    SaveHeader.VoucherNumber = _unitOfWork.Header.GetMaxVHByKind(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo, SaveHeader.TransactionKindNo, SaveHeader.CompanyYear).ToString();
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

                SaveHeader.RowCount = 2;
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
                TransactionCredit.AccountNumber = ObjToSave.ToAccountNumber;
                TransactionCredit.CostCenter = ObjToSave.ToCostCenter;
                TransactionCredit.Debit = 0;
                TransactionCredit.Credit = ObjToSave.Amount;
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
                TransactionCredit.RowNumber = 2;
                foreach (var SavePaper in ObjToSave.Paper)
                {
                    if (!String.IsNullOrWhiteSpace(SavePaper.ChequeNumber))
                    {
                        try {
                            SavePaper.CompanyID = UserInfo.fCompanyId;
                            SavePaper.CompanyYear = SaveHeader.CompanyYear;
                            SavePaper.StockCode = "*";
                            SavePaper.BranchCode = "*";
                            SavePaper.CompanyTransactionKindNo = SaveHeader.CompanyTransactionKindNo;
                            SavePaper.TransactionKindNo = SaveHeader.TransactionKindNo;
                            SavePaper.VoucherDate = SaveHeader.VoucherDate;
                            SavePaper.VoucherNumber = SaveHeader.VoucherNumber;
                            SavePaper.VHI = SaveHeader.VHI;
                            SavePaper.ChequeCase = 1;
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
                            SavePaper.AccountNumberSecond = ObjToSave.ToAccountNumber;
                            SavePaper.Remark = SaveHeader.Note;
                            SavePaper.FCurrencyID = SaveHeader.FCurrencyID;
                            SavePaper.ConversionFactor = SaveHeader.ConversionFactor;
                            SavePaper.InsUserID = SaveHeader.InsUserID;
                            SavePaper.InsDateTime = DateTime.Now;
                            SavePaper.SaleID = ObjToSave.SaleManNo;
                            _unitOfWork.Paper.Add(SavePaper);
                        }
                        catch (Exception ex)
                        {
                            Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                            Msg.Code = 0;
                            return Json(Msg, JsonRequestBehavior.AllowGet);

                        }
                    }
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
                _unitOfWork.Header.Add(SaveHeader);
                _unitOfWork.Transaction.Add(TransactionDebit);
                _unitOfWork.Transaction.Add(TransactionCredit);
                _unitOfWork.Complete();
                Msg.Code = 1;
                Msg.Msg = Resources.Resource.AddedSuccessfully;

                Msg.Year = SaveHeader.CompanyYear;
                Msg.FCompanyId = SaveHeader.CompanyID;
                Msg.VoucherNumber = SaveHeader.VoucherNumber;
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
        [Authorize(Roles = "CoOwner,AttachReceiptVoucherBank")]
        public ActionResult ShowAttach(int id,string id2, string id3, string id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            TransActionFileVM Obj = new TransActionFileVM
            {
                Year = id,
                CompanyId= UserInfo.fCompanyId,
                VoucherNumber = id2,
                TransactionKindNo = id4,
                CompanyTransactionKindNo = id3

            };
            return View("ShowAttach", Obj);

        }

        [ValidateInput(false)]
        public ActionResult GridViewPapers(string id, string id2, string id3,string id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            if (!String.IsNullOrEmpty(id))
            {
                var TransObj = _unitOfWork.NativeSql.GetPapersToReceiptVoucherBankInChequeFund(id, int.Parse(id2), UserInfo.fCompanyId, int.Parse(id3), int.Parse(id4));
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
                var TransObj = _unitOfWork.NativeSql.GetPapersToReceiptVoucherBankInChequeFundCopy(id, int.Parse(id2), UserInfo.fCompanyId, int.Parse(id3), int.Parse(id4));
                return PartialView("GridViewPapersCopy", TransObj);
            }
            else
            {
                var TransObj = new List<TransactionFixedVM>();
                return PartialView("GridViewPapersCopy", TransObj);
            }


        }
        [HttpGet]
        public ActionResult Detail(string id, int id2, int id3 , int id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var HeaderObj = _unitOfWork.Header.GetHeaderDataById(id, UserInfo.fCompanyId, id2, id3 , id4);
            var CompanyTransactionObj = _unitOfWork.CompanyTransactionKind.GetCompanyTransactionKindByID(UserInfo.fCompanyId, HeaderObj.CompanyTransactionKindNo);
            var CurrencyObj = _unitOfWork.Currency.GetCurrencyByID(UserInfo.fCompanyId, HeaderObj.FCurrencyID);
            var SalesManObj = _unitOfWork.Sale.GetSaleByID(UserInfo.fCompanyId, HeaderObj.SaleID);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var TransactionDebitObj = _unitOfWork.NativeSql.GetTransactionsDetailDebit(id, UserInfo.fCompanyId, id2, 1 , id4);
            var TransactionCreditObj = _unitOfWork.NativeSql.GetTransactionsDetailCredit(id, UserInfo.fCompanyId, id2, 2 , id4);
            int iRowNumber = HeaderObj.RowCount;
            var TransFixedVM = new TransactionFixedVM { };
            TransFixedVM.Header = HeaderObj;
            TransFixedVM.TransactionDebit = TransactionDebitObj;
            TransFixedVM.TransactionCredit = TransactionCreditObj;
            if (HeaderObj.SaleID == 0)
            {
                TransFixedVM.SaleManName = "";
            }
            else
            {
                TransFixedVM.SaleManName = SalesManObj.SalesName;
            }
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
            TransFixedVM.AccountCreditName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionCreditObj.AccountNumber);
            TransFixedVM.CostCenterDebitName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionDebitObj.CostCenter);
            TransFixedVM.CostCenterCreditName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionCreditObj.CostCenter);
            TransFixedVM.Amount = Math.Round(TransactionCreditObj.Credit, 3);
            TransFixedVM.TotalAmount = Math.Round(HeaderObj.TotalDebit, 3);
            TransFixedVM.ForeignAmount = Math.Round(TransactionCreditObj.CreditForeign, 3);
            TransFixedVM.TotalForeign = Math.Round(HeaderObj.TotalDebitForeign, 3);
            TransFixedVM.TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency;
            TransFixedVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            return View(TransFixedVM);
        }
        [HttpGet]
        public JsonResult GetPapersToReceiptVoucherBank(string id, string id2, string id3,string id4)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var AllPapersToReceiptVoucherBank = _unitOfWork.NativeSql.GetPapersToReceiptVoucherBank(id, int.Parse(id2), UserInfo.fCompanyId, int.Parse(id3), int.Parse(id4));

            if (AllPapersToReceiptVoucherBank == null)
            {
                return Json(new TransactionFixedVM(), JsonRequestBehavior.AllowGet);
            }
            return Json(AllPapersToReceiptVoucherBank, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [Authorize(Roles = "CoOwner,DeleteReceiptVoucherBank")]
        public ActionResult Delete(string id, int id2, int id3 , int id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var HeaderObj = _unitOfWork.Header.GetHeaderDataById(id, UserInfo.fCompanyId, id2, id3 , id4);
            var CompanyTransactionObj = _unitOfWork.CompanyTransactionKind.GetCompanyTransactionKindByID(UserInfo.fCompanyId, HeaderObj.CompanyTransactionKindNo);
            var CurrencyObj = _unitOfWork.Currency.GetCurrencyByID(UserInfo.fCompanyId, HeaderObj.FCurrencyID);
            var SalesManObj = _unitOfWork.Sale.GetSaleByID(UserInfo.fCompanyId, HeaderObj.SaleID);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var TransactionDebitObj = _unitOfWork.NativeSql.GetTransactionsDetailDebit(id, UserInfo.fCompanyId, id2, 1 , id4);
            var TransactionCreditObj = _unitOfWork.NativeSql.GetTransactionsDetailCredit(id, UserInfo.fCompanyId, id2, 2 , id4);
            int iRowNumber = HeaderObj.RowCount;
            var TransFixedVM = new TransactionFixedVM { };
            TransFixedVM.Header = HeaderObj;
            TransFixedVM.TransactionDebit = TransactionDebitObj;
            TransFixedVM.TransactionCredit = TransactionCreditObj;
            if (HeaderObj.SaleID == 0)
            {
                TransFixedVM.SaleManName = "";
            }
            else
            {
                TransFixedVM.SaleManName = SalesManObj.SalesName;
            }
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
            TransFixedVM.AccountCreditName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionCreditObj.AccountNumber);
            TransFixedVM.CostCenterDebitName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionDebitObj.CostCenter);
            TransFixedVM.CostCenterCreditName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionCreditObj.CostCenter);
            TransFixedVM.Amount = Math.Round(TransactionCreditObj.Credit, 3);
            TransFixedVM.TotalAmount = Math.Round(HeaderObj.TotalDebit, 3);
            TransFixedVM.ForeignAmount = Math.Round(TransactionCreditObj.CreditForeign, 3);
            TransFixedVM.TotalForeign = Math.Round(HeaderObj.TotalDebitForeign, 3);
            TransFixedVM.TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency;
            TransFixedVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            return View(TransFixedVM);
        }
        [Authorize(Roles = "CoOwner,ExportReceiptVoucherBank")]
        public ActionResult Export()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetReceiptVoucherBankFromTransactionKind(UserInfo.fCompanyId);
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
            var ReceiptBankFilter = new ReceiptBankFilterVM
            {
                FromDate = FromDate,
                ToDate = ToDate,
                CompanyTransactionKind = CompanyTransactionKindObj,
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency
        };
            return View(ReceiptBankFilter);
        }
        [HttpPost]
        public JsonResult GetAllReceiptVoucherBankUnExport(ReceiptBankFilterVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllReceiptBank = _unitOfWork.NativeSql.GetAllReceiptBankFromHeaderUnExport(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                if (AllReceiptBank == null)
                {
                    return Json(new List<ReceiptBankFilterVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.VoucherNumber))
                {
                    AllReceiptBank = AllReceiptBank.Where(m => m.VoucherNumber == Obj.VoucherNumber).ToList();
                }
                if (Obj.CurrencyID != 0)
                {
                    AllReceiptBank = AllReceiptBank.Where(m => m.CurrencyID == Obj.CurrencyID).ToList();
                }
                if (Obj.CompanyTransactionKindNo != 0)
                {
                    AllReceiptBank = AllReceiptBank.Where(m => m.CompanyTransactionKindNo == Obj.CompanyTransactionKindNo).ToList();
                }
                int iRow = 0;
                foreach (var iRowCount in AllReceiptBank)
                {
                    iRowCount.iRowTable = iRow;
                    iRow = iRow + 1;
                }
                return Json(AllReceiptBank, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<ReceiptBankFilterVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [Authorize(Roles = "CoOwner,UnExportReceiptVoucherBank")]
        public ActionResult UnExport()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetReceiptVoucherBankFromTransactionKind(UserInfo.fCompanyId);
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
            var ReceiptBankFilter = new ReceiptBankFilterVM
            {
                FromDate = FromDate,
                ToDate = ToDate,
                CompanyTransactionKind = CompanyTransactionKindObj,
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency
            };
            return View(ReceiptBankFilter);
        }
        [HttpPost]
        public JsonResult GetAllReceiptVoucherBankExport(ReceiptBankFilterVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllReceiptBank = _unitOfWork.NativeSql.GetAllReceiptBankFromHeaderExport(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                if (AllReceiptBank == null)
                {
                    return Json(new List<ReceiptBankFilterVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.VoucherNumber))
                {
                    AllReceiptBank = AllReceiptBank.Where(m => m.VoucherNumber == Obj.VoucherNumber).ToList();
                }
                if (Obj.CurrencyID != 0)
                {
                    AllReceiptBank = AllReceiptBank.Where(m => m.CurrencyID == Obj.CurrencyID).ToList();
                }
                if (Obj.CompanyTransactionKindNo != 0)
                {
                    AllReceiptBank = AllReceiptBank.Where(m => m.CompanyTransactionKindNo == Obj.CompanyTransactionKindNo).ToList();
                }
                int iRow = 0;
                foreach (var iRowCount in AllReceiptBank)
                {
                    iRowCount.iRowTable = iRow;
                    iRow = iRow + 1;
                }
                return Json(AllReceiptBank, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<ReceiptBankFilterVM>(), JsonRequestBehavior.AllowGet);
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
        [Authorize(Roles = "CoOwner,UpdateReceiptVoucherBank")]
        public ActionResult Update(string id, int id2, int id3 , int id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            var HeaderObj = _unitOfWork.Header.GetHeaderDataById(id, UserInfo.fCompanyId, id2, id3 , id4);
            var CompanyTransactionKindObj = _unitOfWork.CompanyTransactionKind.GetCompanyTransactionKindByID(UserInfo.fCompanyId, HeaderObj.CompanyTransactionKindNo);
            var CompanyTransactionKindID = HeaderObj.CompanyTransactionKindNo;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var TransactionDebitObj = _unitOfWork.NativeSql.GetTransactionsDetailDebit(id, UserInfo.fCompanyId, id2, 1 ,id4);
            var TransactionCreditObj = _unitOfWork.NativeSql.GetTransactionsDetailCredit(id, UserInfo.fCompanyId, id2, 2 , id4);
            int iRowNumber = HeaderObj.RowCount;
            var TransFixedVM = new TransactionFixedVM { };
            TransFixedVM.Header = HeaderObj;
            TransFixedVM.TransactionDebit = TransactionDebitObj;
            TransFixedVM.TransactionCredit = TransactionCreditObj;
            TransFixedVM.CompanyTransactionKindID = CompanyTransactionKindID;
            if (HeaderObj.SaleID == 0)
            {
                TransFixedVM.SaleMan = _unitOfWork.Sale.GetAllSale(UserInfo.fCompanyId);
            }
            else
            {
                TransFixedVM.SaleMan = _unitOfWork.Sale.GetAllSale(UserInfo.fCompanyId);
                TransFixedVM.SaleManNo = HeaderObj.SaleID;
            }
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
            TransFixedVM.AccountCreditName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionCreditObj.AccountNumber);
            TransFixedVM.CostCenterDebitName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionDebitObj.CostCenter);
            TransFixedVM.CostCenterCreditName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionCreditObj.CostCenter);
            TransFixedVM.Amount = Math.Round(TransactionCreditObj.Credit, 3);
            TransFixedVM.TotalAmount = Math.Round(HeaderObj.TotalDebit, 3);
            TransFixedVM.ForeignAmount = Math.Round(TransactionCreditObj.CreditForeign, 3);
            TransFixedVM.TotalForeign = Math.Round(HeaderObj.TotalDebitForeign, 3);
            TransFixedVM.TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency;
            TransFixedVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            return View(TransFixedVM);
        }
        [HttpPost]
        public JsonResult UpdateReceiptVoucherBank(TransactionFixedVM ObjToUpdate)
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
                UpdateHeader.CompanyYear = ObjToUpdate.Header.CompanyYear;
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

                UpdateHeader.CompanyYear = ObjToUpdate.Header.CompanyYear;
                UpdateHeader.FCurrencyID = ObjToUpdate.CurrencyID;
                UpdateHeader.RowCount = 2;
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

                var TransactionCredit = new Transaction();
                TransactionCredit.CompanyID = UserInfo.fCompanyId;
                TransactionCredit.CompanyYear = UpdateHeader.CompanyYear;
                TransactionCredit.CompanyTransactionKindNo = UpdateHeader.CompanyTransactionKindNo;
                TransactionCredit.TransactionKindNo = UpdateHeader.TransactionKindNo;
                TransactionCredit.VoucherDate = UpdateHeader.VoucherDate;
                TransactionCredit.VoucherNumber = UpdateHeader.VoucherNumber;
                TransactionCredit.InsUserID = UpdateHeader.InsUserID;
                TransactionCredit.VHI = UpdateHeader.VHI;
                TransactionCredit.InsDateTime = DateTime.Now;
                TransactionCredit.AccountNumber = ObjToUpdate.ToAccountNumber;
                TransactionCredit.CostCenter = ObjToUpdate.ToCostCenter;
                TransactionCredit.Debit = 0;
                TransactionCredit.Credit = ObjToUpdate.Amount;
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
                TransactionCredit.RowNumber = 2;
                _unitOfWork.NativeSql.DeletePaper(UserInfo.fCompanyId, UpdateHeader.VoucherNumber,
                    UpdateHeader.CompanyTransactionKindNo, UpdateHeader.TransactionKindNo, UpdateHeader.CompanyYear);
                foreach (var SavePaper in ObjToUpdate.Paper)
                {
                    if (!String.IsNullOrWhiteSpace(SavePaper.ChequeNumber))
                    {
                        try
                        {
                            SavePaper.CompanyID = UserInfo.fCompanyId;
                            SavePaper.CompanyYear = UpdateHeader.CompanyYear;
                            SavePaper.StockCode = "*";
                            SavePaper.BranchCode = "*";
                            SavePaper.CompanyTransactionKindNo = UpdateHeader.CompanyTransactionKindNo;
                            SavePaper.TransactionKindNo = UpdateHeader.TransactionKindNo;
                            SavePaper.VoucherDate = UpdateHeader.VoucherDate;
                            SavePaper.VoucherNumber = UpdateHeader.VoucherNumber;
                            SavePaper.VHI = UpdateHeader.VHI;
                            SavePaper.ChequeCase = 1;
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
                            SavePaper.AccountNumberSecond = ObjToUpdate.ToAccountNumber;
                            SavePaper.FCurrencyID = UpdateHeader.FCurrencyID;
                            SavePaper.ConversionFactor = UpdateHeader.ConversionFactor;
                            SavePaper.Remark = UpdateHeader.Note;
                            SavePaper.InsUserID = UpdateHeader.InsUserID;
                            SavePaper.InsDateTime = DateTime.Now;
                            SavePaper.SaleID = ObjToUpdate.SaleManNo;
                            _unitOfWork.Paper.Add(SavePaper);
                        }
                        catch (Exception ex)
                        {
                            Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                            Msg.Code = 0;
                            return Json(Msg, JsonRequestBehavior.AllowGet);

                        }
                    }
                   
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
                _unitOfWork.Header.Update(UpdateHeader);
                _unitOfWork.Transaction.Update(TransactionDebit);
                _unitOfWork.Transaction.Update(TransactionCredit);
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
        public ActionResult UpdateVoucher(string id, int id2, int id3 , int id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            var HeaderObj = _unitOfWork.Header.GetHeaderDataById(id, UserInfo.fCompanyId, id2, id3 , id4);
            var CompanyTransactionKindObj = _unitOfWork.CompanyTransactionKind.GetCompanyTransactionKindByID(UserInfo.fCompanyId, HeaderObj.CompanyTransactionKindNo);
            var CompanyTransactionKindID = HeaderObj.CompanyTransactionKindNo;
            var CurrencyObj = _unitOfWork.Currency.GetCurrencyByID(UserInfo.fCompanyId, HeaderObj.FCurrencyID);
            var SalesManObj = _unitOfWork.Sale.GetSaleByID(UserInfo.fCompanyId, HeaderObj.SaleID);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var TransactionDebitObj = _unitOfWork.NativeSql.GetTransactionsDetailDebit(id, UserInfo.fCompanyId, id2, 1 ,id4);
            var TransactionCreditObj = _unitOfWork.NativeSql.GetTransactionsDetailCredit(id, UserInfo.fCompanyId, id2, 2 , id4);
            int iRowNumber = HeaderObj.RowCount;
            var TransFixedVM = new TransactionFixedVM { };
            TransFixedVM.Header = HeaderObj;
            TransFixedVM.TransactionDebit = TransactionDebitObj;
            TransFixedVM.TransactionCredit = TransactionCreditObj;
            TransFixedVM.CompanyTransactionKindID = CompanyTransactionKindID;
            if (HeaderObj.SaleID == 0)
            {
                TransFixedVM.SaleManName = "";
            }
            else
            {
                TransFixedVM.SaleManName = SalesManObj.SalesName;
            }
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
            double SumPaperUsed = _unitOfWork.NativeSql.GetSumPaperUsed(UserInfo.fCompanyId, HeaderObj.VoucherNumber, HeaderObj.CompanyTransactionKindNo, HeaderObj.TransactionKindNo, HeaderObj.CompanyYear);
            TransFixedVM.CurrencyID = HeaderObj.FCurrencyID;
            TransFixedVM.CurrencyNewValue = Math.Round(HeaderObj.ConversionFactor, 3);
            TransFixedVM.WorkWithCostCenter = Company.WorkWithCostCenter;
            TransFixedVM.AccountDebitName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionDebitObj.AccountNumber);
            TransFixedVM.AccountCreditName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionCreditObj.AccountNumber);
            TransFixedVM.CostCenterDebitName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionDebitObj.CostCenter);
            TransFixedVM.CostCenterCreditName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionCreditObj.CostCenter);
            TransFixedVM.Amount = Math.Round(TransactionCreditObj.Credit - SumPaperUsed, 3);
            TransFixedVM.TotalAmount = Math.Round(HeaderObj.TotalDebit - SumPaperUsed, 3);
            TransFixedVM.TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency;
            TransFixedVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            if (HeaderObj.FCurrencyID == 1)
            {
            TransFixedVM.ForeignAmount = Math.Round(TransactionCreditObj.CreditForeign, 3);
            TransFixedVM.TotalForeign = Math.Round(HeaderObj.TotalDebitForeign, 3);
            }
            else
            {
                TransFixedVM.ForeignAmount = Math.Round(TransactionCreditObj.CreditForeign - (SumPaperUsed / HeaderObj.ConversionFactor), 3);
                TransFixedVM.TotalForeign = Math.Round(HeaderObj.TotalDebitForeign - (SumPaperUsed / HeaderObj.ConversionFactor), 3);
            }
            return View(TransFixedVM);
        }
        [HttpPost]
        public JsonResult UpdateReceiptVoucherBankUsedPaper(TransactionFixedVM ObjToUpdate)
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
                _unitOfWork.NativeSql.DeletePaperNotUsed(UserInfo.fCompanyId, UpdateHeader.VoucherNumber,UpdateHeader.CompanyTransactionKindNo, UpdateHeader.TransactionKindNo, UpdateHeader.CompanyYear);
                double SumPaperUsed = _unitOfWork.NativeSql.GetSumPaperUsed(UserInfo.fCompanyId, UpdateHeader.VoucherNumber, UpdateHeader.CompanyTransactionKindNo, UpdateHeader.TransactionKindNo, UpdateHeader.CompanyYear);
                int CountPaperUsed = _unitOfWork.NativeSql.GetCountPaperUsed(UserInfo.fCompanyId, UpdateHeader.VoucherNumber, UpdateHeader.CompanyTransactionKindNo, UpdateHeader.TransactionKindNo, UpdateHeader.CompanyYear);
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

                UpdateHeader.CompanyYear = ObjToUpdate.Header.CompanyYear;
                UpdateHeader.FCurrencyID = ObjToUpdate.CurrencyID;
                UpdateHeader.RowCount = 2;
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

                var TransactionCredit = new Transaction();
                TransactionCredit.CompanyID = UserInfo.fCompanyId;
                TransactionCredit.CompanyYear = UpdateHeader.CompanyYear;
                TransactionCredit.CompanyTransactionKindNo = UpdateHeader.CompanyTransactionKindNo;
                TransactionCredit.TransactionKindNo = UpdateHeader.TransactionKindNo;
                TransactionCredit.VoucherDate = UpdateHeader.VoucherDate;
                TransactionCredit.VoucherNumber = UpdateHeader.VoucherNumber;
                TransactionCredit.InsUserID = UpdateHeader.InsUserID;
                TransactionCredit.VHI = UpdateHeader.VHI;
                TransactionCredit.InsDateTime = DateTime.Now;
                TransactionCredit.AccountNumber = ObjToUpdate.ToAccountNumber;
                TransactionCredit.CostCenter = ObjToUpdate.ToCostCenter;
                TransactionCredit.Debit = 0;
                TransactionCredit.Credit = ObjToUpdate.Amount + SumPaperUsed;
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
                TransactionCredit.RowNumber = 2;
                var iRow = 0;
                var UpdateRowNumber = _unitOfWork.NativeSql.GetRowNumberPaperUsed(UserInfo.fCompanyId, UpdateHeader.VoucherNumber,
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
                foreach (var SavePaper in ObjToUpdate.Paper)
                {
                    if (!String.IsNullOrWhiteSpace(SavePaper.ChequeNumber))
                    {
                        try
                        {
                            iRow = iRow + 1;
                            SavePaper.CompanyID = UserInfo.fCompanyId;
                            SavePaper.CompanyYear = UpdateHeader.CompanyYear;
                            SavePaper.StockCode = "*";
                            SavePaper.BranchCode = "*";
                            SavePaper.CompanyTransactionKindNo = UpdateHeader.CompanyTransactionKindNo;
                            SavePaper.TransactionKindNo = UpdateHeader.TransactionKindNo;
                            SavePaper.VoucherDate = UpdateHeader.VoucherDate;
                            SavePaper.VoucherNumber = UpdateHeader.VoucherNumber;
                            SavePaper.VHI = UpdateHeader.VHI;
                            SavePaper.ChequeCase = 1;
                            SavePaper.OldVoucherNumber = "-1";
                            SavePaper.OldCompanyTransactionKindNo = -1;
                            SavePaper.OldTransactionKindNo = -1;
                            SavePaper.OldVHI = -1;
                            SavePaper.RowNumber = iRow;
                            SavePaper.sChequeDate = SavePaper.ChequeDate.ToString();
                            SavePaper.OriginalVoucherNumber = UpdateHeader.VoucherNumber;
                            SavePaper.OriginalCompanyTransactionKindNo = UpdateHeader.CompanyTransactionKindNo;
                            SavePaper.OriginalTransactionKindNo = UpdateHeader.TransactionKindNo;
                            SavePaper.OriginalVHI = UpdateHeader.VHI;
                            SavePaper.AccountNumberFirst = ObjToUpdate.FromAccountNumber;
                            SavePaper.AccountNumberSecond = ObjToUpdate.ToAccountNumber;
                            SavePaper.FCurrencyID = UpdateHeader.FCurrencyID;
                            SavePaper.ConversionFactor = UpdateHeader.ConversionFactor;
                            SavePaper.Remark = UpdateHeader.Note;
                            SavePaper.InsUserID = UpdateHeader.InsUserID;
                            SavePaper.InsDateTime = DateTime.Now;
                            SavePaper.SaleID = ObjToUpdate.SaleManNo;
                            _unitOfWork.Paper.Add(SavePaper);
                        }
                        catch (Exception ex)
                        {
                            Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                            Msg.Code = 0;
                            return Json(Msg, JsonRequestBehavior.AllowGet);

                        }
                    }
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
                _unitOfWork.Header.Update(UpdateHeader);
                _unitOfWork.Transaction.Update(TransactionDebit);
                _unitOfWork.Transaction.Update(TransactionCredit);
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
        [Authorize(Roles = "CoOwner,CopyReceiptVoucherBank")]
        public ActionResult Copy(string id, int id2, int id3 , int id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            var HeaderObj = _unitOfWork.Header.GetHeaderDataById(id, UserInfo.fCompanyId, id2, id3 , id4);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetReceiptVoucherBankFromTransactionKind(UserInfo.fCompanyId);
            var CompanyTransactionKindID = HeaderObj.CompanyTransactionKindNo;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var TransactionDebitObj = _unitOfWork.NativeSql.GetTransactionsDetailDebit(id, UserInfo.fCompanyId, id2, 1 , id4);
            var TransactionCreditObj = _unitOfWork.NativeSql.GetTransactionsDetailCredit(id, UserInfo.fCompanyId, id2, 2 , id4);
            int iRowNumber = HeaderObj.RowCount;
            var TransFixedVM = new TransactionFixedVM { };
            TransFixedVM.Header = HeaderObj;
            TransFixedVM.TransactionDebit = TransactionDebitObj;
            TransFixedVM.TransactionCredit = TransactionCreditObj;
            TransFixedVM.CompanyTransactionKind = CompanyTransactionKindObj;
            TransFixedVM.CompanyTransactionKindID = CompanyTransactionKindID;
            if (HeaderObj.SaleID == 0)
            {
                TransFixedVM.SaleMan = _unitOfWork.Sale.GetAllSale(UserInfo.fCompanyId);
            }
            else
            {
                TransFixedVM.SaleMan = _unitOfWork.Sale.GetAllSale(UserInfo.fCompanyId);
                TransFixedVM.SaleManNo = HeaderObj.SaleID;
            }
            TransFixedVM.Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId);
            TransFixedVM.CurrencyID = HeaderObj.FCurrencyID;
            TransFixedVM.CurrencyNewValue = Math.Round(HeaderObj.ConversionFactor, 3);
            TransFixedVM.WorkWithCostCenter = Company.WorkWithCostCenter;
            TransFixedVM.AccountDebitName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionDebitObj.AccountNumber);
            TransFixedVM.AccountCreditName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionCreditObj.AccountNumber);
            TransFixedVM.CostCenterDebitName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionDebitObj.CostCenter);
            TransFixedVM.CostCenterCreditName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionCreditObj.CostCenter);
            TransFixedVM.Amount = Math.Round(TransactionCreditObj.Credit, 3);
            TransFixedVM.TotalAmount = Math.Round(HeaderObj.TotalDebit, 3);
            TransFixedVM.ForeignAmount = Math.Round(TransactionCreditObj.CreditForeign, 3);
            TransFixedVM.TotalForeign = Math.Round(HeaderObj.TotalDebitForeign, 3);
            TransFixedVM.TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency;
            TransFixedVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            return View(TransFixedVM);
        }
        public ActionResult ReceivedCheque()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var PaperCount = _unitOfWork.NativeSql.GetCountAllChequeInFund(UserInfo.fCompanyId);
            var PaperAmount = _unitOfWork.NativeSql.GetSumAllChequeInFund(UserInfo.fCompanyId);
            var UnderCollectionPaperCount = _unitOfWork.NativeSql.GetCountAllUbderCollectionCheque(UserInfo.fCompanyId);
            var UnderCollectionPaperAmount = _unitOfWork.NativeSql.GetSumAllUbderCollectionCheque(UserInfo.fCompanyId);
            var EndorsementPaperCount = _unitOfWork.NativeSql.GetCountAllEndorsementPaperCount(UserInfo.fCompanyId);
            var EndorsementPaperAmount = _unitOfWork.NativeSql.GetSumAllEndorsementPaperCount(UserInfo.fCompanyId);
            var PaymentUnderCollectionPaperCount = _unitOfWork.NativeSql.GetCountAllPaymentUbderCollectionCheque(UserInfo.fCompanyId);
            var PaymentUnderCollectionPaperAmount = _unitOfWork.NativeSql.GetSumAllPaymentUbderCollectionCheque(UserInfo.fCompanyId);
            var ClearingDepositPaperCount = _unitOfWork.NativeSql.GetCountAllClearingDepositCheque(UserInfo.fCompanyId);
            var ClearingDepositPaperAmount = _unitOfWork.NativeSql.GetSumAllClearingDepositCheque(UserInfo.fCompanyId);
            var DrawingUnderCollectionPaperCount = _unitOfWork.NativeSql.GetCountAllDrawingUnderCollectionCheque(UserInfo.fCompanyId);
            var DrawingUnderCollectionPaperAmount = _unitOfWork.NativeSql.GetSumAllDrawingUnderCollectionCheque(UserInfo.fCompanyId);
            var ReturnedChequePaperCount = _unitOfWork.NativeSql.GetCountAllReturnedCheque(UserInfo.fCompanyId);
            var ReturnedChequePaperAmount = _unitOfWork.NativeSql.GetSumAllReturnedCheque(UserInfo.fCompanyId);
            var CourtFundPaperCount = _unitOfWork.NativeSql.GetCountAllCourtFundCheque(UserInfo.fCompanyId);
            var CourtFundPaperAmount = _unitOfWork.NativeSql.GetSumAllCourtFundCheque(UserInfo.fCompanyId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var PaperFilter = new PaperFilterVM
            {
                PaperCount = PaperCount,
                UnderCollectionPaperCount = UnderCollectionPaperCount,
                EndorsementPaperCount = EndorsementPaperCount,
                PaymentUnderCollectionPaperCount = PaymentUnderCollectionPaperCount,
                ClearingDepositPaperCount = ClearingDepositPaperCount,
                DrawingUnderCollectionPaperCount = DrawingUnderCollectionPaperCount,
                ReturnedChequePaperCount = ReturnedChequePaperCount,
                CourtFundPaperCount = CourtFundPaperCount,
                PaperAmount = PaperAmount,
                UnderCollectionPaperAmount = UnderCollectionPaperAmount,
                EndorsementPaperAmount = EndorsementPaperAmount,
                PaymentUnderCollectionPaperAmount = PaymentUnderCollectionPaperAmount,
                ClearingDepositPaperAmount = ClearingDepositPaperAmount,
                DrawingUnderCollectionPaperAmount = DrawingUnderCollectionPaperAmount,
                ReturnedChequePaperAmount = ReturnedChequePaperAmount,
                CourtFundPaperAmount = CourtFundPaperAmount,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency
        };
            return View(PaperFilter);
        }

        [Authorize(Roles = "CoOwner,ShowTransferFromFundUC")]
        public ActionResult TransferFromFundToUnderCollection()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetReceiptVoucherBankFromTransactionKind(UserInfo.fCompanyId);
            var CompanyTransactionKindToTransferObj = _unitOfWork.NativeSql.GetReceiptBankTransactionCompanyTransactionKind(UserInfo.fCompanyId);
            var FromDate = _unitOfWork.NativeSql.GetFromDateChequeInFund(UserInfo.fCompanyId);
            var ToDate = _unitOfWork.NativeSql.GetToDateChequeInFund(UserInfo.fCompanyId);
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
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                CurrentYear = UserInfo.CurrYear
        };
            return View(PaperFilter);
        }
        [HttpPost]
        public JsonResult GetAllTransferFromFundToUnderCollection(PaperFilterVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);

                var AllPaper = _unitOfWork.NativeSql.GetAllTransferFromFundToUnderCollection(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
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
                    AllPaper = AllPaper.Where(m=> m.IsBill == 1 ? m.BillCustomerNumber == Obj.AccountNumberSecond : m.AccountNumberSecond == Obj.AccountNumberSecond).ToList();
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
        public JsonResult SaveReceiptTransferToUnderCollection(PaperFilterVM ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var SaveHeader = ObjToSave.Header;
                SaveHeader.InsDateTime = DateTime.Now;
                SaveHeader.InsUserID = userId;
                SaveHeader.CompanyTransactionKindNo = ObjToSave.CompanyTransactionKindToTransferNo;
                SaveHeader.CompanyYear = UserInfo.CurrYear;
                var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                SaveHeader.TransactionKindNo = _unitOfWork.CompanyTransactionKind.GetTransKindForCompanyTransactionKind(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo);
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
                SaveHeader.CompanyID = UserInfo.fCompanyId;
                SaveHeader.SaleID = 0;
                SaveHeader.Exported = 1;
                if (ObjToSave.CurrencyID == 0)
                {
                    ObjToSave.CurrencyID = 1;
                }
                SaveHeader.FCurrencyID = ObjToSave.CurrencyID;
                if (Resources.Resource.CurLang == "Arb")
                {
                    SaveHeader.Note = "محول من صندوق الشيكات";
                }
                else
                {
                    SaveHeader.Note = "Transfer from cheque box";
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
                        TransNote = " رقم شيك محول لرسم التحصيل : " + UpdatePaper.ChequeNumber + " - " + " تاريخ الحق : " + dDateAfterRemoveTime.ToString("dd/MM/yyyy") + " - " + " الحساب المدفوع منه : " +
                            _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, UpdatePaper.AccountNumberSecond);
                    }
                    else
                    {
                        TransNote = " Cheque Number Transfer To Under Collection : " + UpdatePaper.ChequeNumber + " - " + " Cheque Date : " + dDateAfterRemoveTime.ToString("dd/MM/yyyy") + " - " + " Paid Account : " +
                            _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, UpdatePaper.AccountNumberSecond);
                    }
                    UpdatePaper.RowNumber = UpdatePaper.RowNumber;
                    UpdatePaper.CompanyYear = UpdatePaper.CompanyYear;
                    UpdatePaper.CompanyID = UserInfo.fCompanyId;
                    UpdatePaper.OldVoucherNumber = UpdatePaper.VoucherNumber;
                    UpdatePaper.OldCompanyTransactionKindNo = UpdatePaper.CompanyTransactionKindNo;
                    UpdatePaper.OldTransactionKindNo = _unitOfWork.CompanyTransactionKind.GetTransKindForCompanyTransactionKind(UserInfo.fCompanyId, UpdatePaper.OldCompanyTransactionKindNo);
                    UpdatePaper.OldVHI = UpdatePaper.VHI;
                    UpdatePaper.AccountNumberFourth = UpdatePaper.AccountNumberSecond;
                    UpdatePaper.AccountNumberFifth = ObjToSave.AccountNumberFifth;
                    UpdatePaper.CompanyTransactionKindNo = SaveHeader.CompanyTransactionKindNo;
                    UpdatePaper.TransactionKindNo = SaveHeader.TransactionKindNo;
                    UpdatePaper.VoucherDate = SaveHeader.VoucherDate;
                    UpdatePaper.VoucherNumber = SaveHeader.VoucherNumber;
                    UpdatePaper.VHI = SaveHeader.VHI;
                    UpdatePaper.ChequeCase = 2;
                    UpdatePaper.AccountNumberSecond = ObjToSave.UnderCollectionAccountNumber;                    
                    UpdatePaper.Remark = SaveHeader.Note;
                    UpdatePaper.InsUserID = SaveHeader.InsUserID;
                    UpdatePaper.InsDateTime = DateTime.Now;
                    Amount = Amount + UpdatePaper.ChequeAmount;
                    _unitOfWork.NativeSql.SaveTracingPaper(UpdatePaper.CompanyID, UpdatePaper.CompanyYear, UpdatePaper.RowNumber, UpdatePaper.OldVoucherNumber, 
                                                           UpdatePaper.OldCompanyTransactionKindNo, UpdatePaper.OldTransactionKindNo,UpdatePaper.ChequeNumber, UpdatePaper.sChequeDate,
                                                           UpdatePaper.ChequeAmount, UpdatePaper.AccountNumberThird);
                    _unitOfWork.NativeSql.UpdatePaper(UpdatePaper.CompanyID, UpdatePaper.CompanyYear, UpdatePaper.RowNumber, UpdatePaper.OldVoucherNumber, UpdatePaper.OldCompanyTransactionKindNo,
                        UpdatePaper.OldTransactionKindNo, UpdatePaper.OldVHI, UpdatePaper.CompanyTransactionKindNo, UpdatePaper.TransactionKindNo, UpdatePaper.VoucherDate, UpdatePaper.VoucherNumber,
                        UpdatePaper.VHI, UpdatePaper.ChequeCase, UpdatePaper.AccountNumberSecond,UpdatePaper.AccountNumberFourth, UpdatePaper.Remark,
                        UpdatePaper.InsUserID, UpdatePaper.InsDateTime, UpdatePaper.AccountNumberFifth,UpdatePaper.ChequeNumber,UpdatePaper.sChequeDate,
                        UpdatePaper.ChequeAmount, UpdatePaper.AccountNumberThird);
                    _unitOfWork.Complete();

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
                    TransactionDebit.AccountNumber = ObjToSave.UnderCollectionAccountNumber;
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
                    TransactionCredit.AccountNumber = ObjToSave.AccountNumberFirst;
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

        [Authorize(Roles = "CoOwner,ShowReturnACheque")]
        public ActionResult ReturnAChequeFromTheChequeBox()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetReceiptVoucherBankFromTransactionKind(UserInfo.fCompanyId);
            var CompanyTransactionKindToTransferObj = _unitOfWork.NativeSql.GetReceiptBankTransactionCompanyTransactionKind(UserInfo.fCompanyId);
            var FromDate = _unitOfWork.NativeSql.GetFromDateChequeInFund(UserInfo.fCompanyId);
            var ToDate = _unitOfWork.NativeSql.GetToDateChequeInFund(UserInfo.fCompanyId);
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
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                CurrentYear = UserInfo.CurrYear
            };
            return View(PaperFilter);
        }
        public JsonResult GetAllReturnAChequeFromTheChequeBox(PaperFilterVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);

                var AllPaper = _unitOfWork.NativeSql.GetAllReturnAChequeFromTheChequeBox(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
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
                    AllPaper = AllPaper.Where(m => m.IsBill == 1 ? m.BillCustomerNumber == Obj.AccountNumberSecond : m.AccountNumberSecond == Obj.AccountNumberSecond).ToList();
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
        public JsonResult SaveReturnAChequeFromTheChequeBox(PaperFilterVM ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var SaveHeader = ObjToSave.Header;
                SaveHeader.InsDateTime = DateTime.Now;
                SaveHeader.InsUserID = userId;
                SaveHeader.CompanyTransactionKindNo = ObjToSave.CompanyTransactionKindToTransferNo;
                SaveHeader.CompanyYear = UserInfo.CurrYear;
                var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                SaveHeader.TransactionKindNo = _unitOfWork.CompanyTransactionKind.GetTransKindForCompanyTransactionKind(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo);
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
                SaveHeader.CompanyID = UserInfo.fCompanyId;
                SaveHeader.SaleID = 0;
                SaveHeader.Exported = 1;
                if (ObjToSave.CurrencyID == 0)
                {
                    ObjToSave.CurrencyID = 1;
                }
                SaveHeader.FCurrencyID = ObjToSave.CurrencyID;
                if (Resources.Resource.CurLang == "Arb")
                {
                    SaveHeader.Note = "معاد من صندوق شيكات";
                }
                else
                {
                    SaveHeader.Note = "Returned from cheque box";
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
                        TransNote = " رقم شيك معاد من صندوق الشيكات : " + UpdatePaper.ChequeNumber + " - " + " تاريخ الحق : " + dDateAfterRemoveTime.ToString("dd/MM/yyyy") + " - " + " الحساب المدفوع منه : " +
                            _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, UpdatePaper.AccountNumberSecond);
                    }
                    else
                    {
                        TransNote = " Cheque Number Returned from cheque box : " + UpdatePaper.ChequeNumber + " - " + " Cheque Date : " + dDateAfterRemoveTime.ToString("dd/MM/yyyy") + " - " + " Paid Account : " +
                            _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, UpdatePaper.AccountNumberSecond);
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
                    UpdatePaper.ChequeCase = 18;
                    UpdatePaper.Remark = SaveHeader.Note;
                    UpdatePaper.InsUserID = SaveHeader.InsUserID;
                    UpdatePaper.InsDateTime = DateTime.Now;
                    Amount = Amount + UpdatePaper.ChequeAmount;
                    _unitOfWork.NativeSql.SaveTracingPaper(UpdatePaper.CompanyID, UpdatePaper.CompanyYear, UpdatePaper.RowNumber, UpdatePaper.OldVoucherNumber,
                                                           UpdatePaper.OldCompanyTransactionKindNo, UpdatePaper.OldTransactionKindNo, UpdatePaper.ChequeNumber, UpdatePaper.sChequeDate,
                                                           UpdatePaper.ChequeAmount, UpdatePaper.AccountNumberThird);
                    _unitOfWork.NativeSql.UpdatePaperReturnedFromChequeBox(UpdatePaper.CompanyID, UpdatePaper.CompanyYear, UpdatePaper.RowNumber, UpdatePaper.OldVoucherNumber, UpdatePaper.OldCompanyTransactionKindNo,
                        UpdatePaper.OldTransactionKindNo, UpdatePaper.OldVHI, UpdatePaper.CompanyTransactionKindNo, UpdatePaper.TransactionKindNo, UpdatePaper.VoucherDate, UpdatePaper.VoucherNumber,
                        UpdatePaper.VHI, UpdatePaper.ChequeCase,UpdatePaper.Remark,UpdatePaper.InsUserID, UpdatePaper.InsDateTime,UpdatePaper.ChequeNumber, UpdatePaper.sChequeDate,
                        UpdatePaper.ChequeAmount, UpdatePaper.AccountNumberThird);
                    _unitOfWork.Complete();

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
                    TransactionCredit.AccountNumber = ObjToSave.AccountNumberFirst;
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
        [Authorize(Roles = "CoOwner,ShowTransferFromFundCD")]
        public ActionResult TransferFromFundToClearingDeposit()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetReceiptVoucherBankFromTransactionKind(UserInfo.fCompanyId);
            var CompanyTransactionKindToTransferObj = _unitOfWork.NativeSql.GetReceiptBankTransactionCompanyTransactionKind(UserInfo.fCompanyId);
            var FromDate = _unitOfWork.NativeSql.GetFromDateChequeInFund(UserInfo.fCompanyId);
            var ToDate = _unitOfWork.NativeSql.GetToDateChequeInFund(UserInfo.fCompanyId);
            int year = DateTime.Now.Year;
            var HeadrObj = new Header();
            HeadrObj.VoucherDate = DateTime.Now;
            var TransactionDebitObj = new Transaction();
            var TransactionCreditObj = new Transaction();
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var PaperFilter = new PaperFilterVM
            {
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                CompanyTransactionKind = CompanyTransactionKindObj,
                CompanyTransactionKindToTransfer = CompanyTransactionKindToTransferObj,
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                FCurrencyID = 1,
                Header = HeadrObj,
                TransactionDebit = TransactionDebitObj,
                TransactionCredit = TransactionCreditObj,
                WorkWithCostCenter = Company.WorkWithCostCenter,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                CurrentYear = UserInfo.CurrYear
            };
            return View(PaperFilter);
        }
        [HttpPost]
        public JsonResult GetAllTransferFromFundToClearingDeposit(PaperFilterVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllPaper = _unitOfWork.NativeSql.GetAllTransferFromFundToClearingDeposit(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
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
                    AllPaper = AllPaper.Where(m => m.IsBill == 1 ? m.BillCustomerNumber == Obj.AccountNumberSecond : m.AccountNumberSecond == Obj.AccountNumberSecond).ToList();
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
        public JsonResult SaveReceiptTransferToClearingDeposit(PaperFilterVM ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var SaveHeader = ObjToSave.Header;
                SaveHeader.InsDateTime = DateTime.Now;
                SaveHeader.InsUserID = userId;
                SaveHeader.CompanyTransactionKindNo = ObjToSave.CompanyTransactionKindToTransferNo;
                SaveHeader.CompanyYear = UserInfo.CurrYear;
                var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                SaveHeader.TransactionKindNo = _unitOfWork.CompanyTransactionKind.GetTransKindForCompanyTransactionKind(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo);
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
                SaveHeader.CompanyID = UserInfo.fCompanyId;
                SaveHeader.SaleID = 0;
                SaveHeader.Exported = 1;
                if (ObjToSave.CurrencyID == 0)
                {
                    ObjToSave.CurrencyID = 1;
                }

                SaveHeader.FCurrencyID = ObjToSave.CurrencyID;
                SaveHeader.VHI = SaveHeader.VHI;
                if (Resources.Resource.CurLang == "Arb")
                {
                    SaveHeader.Note = "إيداع مقاصة";
                }
                else
                {
                    SaveHeader.Note = "Clearing Deposit";
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
                        TransNote = " رقم الشيك المحول إيداع مقاصة : " + UpdatePaper.ChequeNumber + " - " + " تاريخ الحق : " + dDateAfterRemoveTime.ToString("dd/MM/yyyy") + " - " + " الحساب المدفوع منه : " +
                            _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, UpdatePaper.AccountNumberSecond);
                    }
                    else
                    {
                        TransNote = " Cheque Number Transfer To Clearing Deposit : " + UpdatePaper.ChequeNumber + " - " + " Cheque Date : " + dDateAfterRemoveTime.ToString("dd/MM/yyyy") + " - " + " Paid Account : " +
                            _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, UpdatePaper.AccountNumberSecond);
                    }
                    UpdatePaper.RowNumber = UpdatePaper.RowNumber;
                    UpdatePaper.CompanyYear = UpdatePaper.CompanyYear;
                    UpdatePaper.CompanyID = UserInfo.fCompanyId;
                    UpdatePaper.OldVoucherNumber = UpdatePaper.VoucherNumber;
                    UpdatePaper.OldCompanyTransactionKindNo = UpdatePaper.CompanyTransactionKindNo;
                    UpdatePaper.OldTransactionKindNo = _unitOfWork.CompanyTransactionKind.GetTransKindForCompanyTransactionKind(UserInfo.fCompanyId, UpdatePaper.OldCompanyTransactionKindNo);
                    UpdatePaper.OldVHI = UpdatePaper.VHI;
                    UpdatePaper.AccountNumberFourth = UpdatePaper.AccountNumberSecond;
                    UpdatePaper.AccountNumberFifth = ObjToSave.ClearingDepositAccountNumber;
                    UpdatePaper.CompanyTransactionKindNo = SaveHeader.CompanyTransactionKindNo;
                    UpdatePaper.TransactionKindNo = SaveHeader.TransactionKindNo;
                    UpdatePaper.VoucherDate = SaveHeader.VoucherDate;
                    UpdatePaper.VoucherNumber = SaveHeader.VoucherNumber;
                    UpdatePaper.VHI = SaveHeader.VHI;
                    UpdatePaper.ChequeCase = 4;
                    UpdatePaper.AccountNumberSecond = ObjToSave.ClearingDepositAccountNumber;
                    UpdatePaper.Remark = SaveHeader.Note;
                    UpdatePaper.InsUserID = SaveHeader.InsUserID;
                    UpdatePaper.InsDateTime = DateTime.Now;
                    Amount = Amount + UpdatePaper.ChequeAmount;
                    _unitOfWork.NativeSql.SaveTracingPaper(UpdatePaper.CompanyID, UpdatePaper.CompanyYear, UpdatePaper.RowNumber, UpdatePaper.OldVoucherNumber,
                                                           UpdatePaper.OldCompanyTransactionKindNo, UpdatePaper.OldTransactionKindNo, UpdatePaper.ChequeNumber, UpdatePaper.sChequeDate,
                                                           UpdatePaper.ChequeAmount, UpdatePaper.AccountNumberThird);
                    _unitOfWork.NativeSql.UpdatePaper(UpdatePaper.CompanyID, UpdatePaper.CompanyYear, UpdatePaper.RowNumber, UpdatePaper.OldVoucherNumber, UpdatePaper.OldCompanyTransactionKindNo,
                        UpdatePaper.OldTransactionKindNo, UpdatePaper.OldVHI, UpdatePaper.CompanyTransactionKindNo, UpdatePaper.TransactionKindNo, UpdatePaper.VoucherDate, UpdatePaper.VoucherNumber,
                        UpdatePaper.VHI, UpdatePaper.ChequeCase, UpdatePaper.AccountNumberSecond, UpdatePaper.AccountNumberFourth, UpdatePaper.Remark,
                        UpdatePaper.InsUserID, UpdatePaper.InsDateTime, UpdatePaper.AccountNumberFifth,UpdatePaper.ChequeNumber, UpdatePaper.sChequeDate,
                        UpdatePaper.ChequeAmount, UpdatePaper.AccountNumberThird);
                    _unitOfWork.Complete();

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
                    TransactionDebit.AccountNumber = ObjToSave.ClearingDepositAccountNumber;
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
                    TransactionCredit.AccountNumber = ObjToSave.AccountNumberFirst;
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
        [Authorize(Roles = "CoOwner,ShowTransferFromFundToChequeE")]
        public ActionResult TransferFromFundToChequeEndorsement()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetReceiptVoucherBankFromTransactionKind(UserInfo.fCompanyId);
            var CompanyTransactionKindToTransferObj = _unitOfWork.NativeSql.GetReceiptBankTransactionCompanyTransactionKind(UserInfo.fCompanyId);
            var FromDate = _unitOfWork.NativeSql.GetFromDateChequeInFund(UserInfo.fCompanyId);
            var ToDate = _unitOfWork.NativeSql.GetToDateChequeInFund(UserInfo.fCompanyId);
            int year = DateTime.Now.Year;
            var HeadrObj = new Header();
            HeadrObj.VoucherDate = DateTime.Now;
            var TransactionDebitObj = new Transaction();
            var TransactionCreditObj = new Transaction();
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var PaperFilter = new PaperFilterVM
            {
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                CompanyTransactionKind = CompanyTransactionKindObj,
                CompanyTransactionKindToTransfer = CompanyTransactionKindToTransferObj,
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                FCurrencyID = 1,
                Header = HeadrObj,
                TransactionDebit = TransactionDebitObj,
                TransactionCredit = TransactionCreditObj,
                WorkWithCostCenter = Company.WorkWithCostCenter,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                CurrentYear = UserInfo.CurrYear
            };
            return View(PaperFilter);
        }
        [HttpPost]
        public JsonResult GetAllTransferFromFundToChequeEndorsement(PaperFilterVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllPaper = _unitOfWork.NativeSql.GetAllTransferFromFundToChequeEndorsement(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
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
                    AllPaper = AllPaper.Where(m => m.IsBill == 1 ? m.BillCustomerNumber == Obj.AccountNumberSecond : m.AccountNumberSecond == Obj.AccountNumberSecond).ToList();
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
        public JsonResult SaveReceiptTransferToChequeEndorsement(PaperFilterVM ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var SaveHeader = ObjToSave.Header;
                SaveHeader.InsDateTime = DateTime.Now;
                SaveHeader.InsUserID = userId;
                SaveHeader.CompanyTransactionKindNo = ObjToSave.CompanyTransactionKindToTransferNo;
                SaveHeader.CompanyYear = UserInfo.CurrYear;
                var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                SaveHeader.TransactionKindNo = _unitOfWork.CompanyTransactionKind.GetTransKindForCompanyTransactionKind(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo);
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
                SaveHeader.CompanyID = UserInfo.fCompanyId;
                SaveHeader.SaleID = 0;
                SaveHeader.Exported = 1;
                if (ObjToSave.CurrencyID == 0)
                {
                    ObjToSave.CurrencyID = 1;
                }

                SaveHeader.FCurrencyID = ObjToSave.CurrencyID;
                SaveHeader.VHI = SaveHeader.VHI;
                if (Resources.Resource.CurLang == "Arb")
                {
                    SaveHeader.Note = "مجير من صندوق الشيكات";
                }
                else
                {
                    SaveHeader.Note = "Endorsement from cheque box";
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
                        TransNote = " رقم الشيك المجير : " + UpdatePaper.ChequeNumber + " - " + " تاريخ الحق : " + dDateAfterRemoveTime.ToString("dd/MM/yyyy") + " - " + " الحساب المدفوع منه : " +
                            _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, UpdatePaper.AccountNumberSecond);
                    }
                    else
                    {
                        TransNote = " Cheque Number Endorsement : " + UpdatePaper.ChequeNumber + " - " + " Cheque Date : " + dDateAfterRemoveTime.ToString("dd/MM/yyyy") + " - " + " Paid Account : " +
                            _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, UpdatePaper.AccountNumberSecond);
                    }
                    UpdatePaper.RowNumber = UpdatePaper.RowNumber;
                    UpdatePaper.CompanyYear = UpdatePaper.CompanyYear;
                    UpdatePaper.CompanyID = UserInfo.fCompanyId;
                    UpdatePaper.OldVoucherNumber = UpdatePaper.VoucherNumber;
                    UpdatePaper.OldCompanyTransactionKindNo = UpdatePaper.CompanyTransactionKindNo;
                    UpdatePaper.OldTransactionKindNo = _unitOfWork.CompanyTransactionKind.GetTransKindForCompanyTransactionKind(UserInfo.fCompanyId, UpdatePaper.OldCompanyTransactionKindNo);
                    UpdatePaper.OldVHI = UpdatePaper.VHI;
                    UpdatePaper.AccountNumberFourth = UpdatePaper.AccountNumberSecond;
                    UpdatePaper.AccountNumberFifth = "";
                    UpdatePaper.CompanyTransactionKindNo = SaveHeader.CompanyTransactionKindNo;
                    UpdatePaper.TransactionKindNo = SaveHeader.TransactionKindNo;
                    UpdatePaper.VoucherDate = SaveHeader.VoucherDate;
                    UpdatePaper.VoucherNumber = SaveHeader.VoucherNumber;
                    UpdatePaper.VHI = SaveHeader.VHI;
                    UpdatePaper.ChequeCase = 5;
                    UpdatePaper.AccountNumberSecond = ObjToSave.ChequeEndorsementAccountNumber;
                    UpdatePaper.Remark = SaveHeader.Note;
                    UpdatePaper.InsUserID = SaveHeader.InsUserID;
                    UpdatePaper.InsDateTime = DateTime.Now;
                    Amount = Amount + UpdatePaper.ChequeAmount;
                    _unitOfWork.NativeSql.SaveTracingPaper(UpdatePaper.CompanyID, UpdatePaper.CompanyYear, UpdatePaper.RowNumber, UpdatePaper.OldVoucherNumber,
                                                           UpdatePaper.OldCompanyTransactionKindNo, UpdatePaper.OldTransactionKindNo, UpdatePaper.ChequeNumber, UpdatePaper.sChequeDate,
                                                           UpdatePaper.ChequeAmount, UpdatePaper.AccountNumberThird);
                    _unitOfWork.NativeSql.UpdatePaper(UpdatePaper.CompanyID, UpdatePaper.CompanyYear, UpdatePaper.RowNumber, UpdatePaper.OldVoucherNumber, UpdatePaper.OldCompanyTransactionKindNo,
                        UpdatePaper.OldTransactionKindNo, UpdatePaper.OldVHI, UpdatePaper.CompanyTransactionKindNo, UpdatePaper.TransactionKindNo, UpdatePaper.VoucherDate, UpdatePaper.VoucherNumber,
                        UpdatePaper.VHI, UpdatePaper.ChequeCase, UpdatePaper.AccountNumberSecond, UpdatePaper.AccountNumberFourth, UpdatePaper.Remark,
                        UpdatePaper.InsUserID, UpdatePaper.InsDateTime, UpdatePaper.AccountNumberFifth, UpdatePaper.ChequeNumber, UpdatePaper.sChequeDate,
                        UpdatePaper.ChequeAmount, UpdatePaper.AccountNumberThird);
                    _unitOfWork.Complete();

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
                    TransactionDebit.AccountNumber = ObjToSave.ChequeEndorsementAccountNumber;
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
                    TransactionCredit.AccountNumber = ObjToSave.AccountNumberFirst;
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
        [Authorize(Roles = "CoOwner,ShowPaymentchequeUC")]
        public ActionResult PaymentChequeUnderCollection()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetReceiptBankTransactionCompanyTransactionKind(UserInfo.fCompanyId);
            var CompanyTransactionKindToTransferObj = _unitOfWork.NativeSql.GetReceiptBankTransactionCompanyTransactionKind(UserInfo.fCompanyId);
            var FromDate = _unitOfWork.NativeSql.GetFromDateUbderCollectionCheque(UserInfo.fCompanyId);
            var ToDate = _unitOfWork.NativeSql.GetToDateUbderCollectionCheque(UserInfo.fCompanyId);
            int year = DateTime.Now.Year;
            var HeadrObj = new Header();
            HeadrObj.VoucherDate = DateTime.Now;
            var TransactionDebitObj = new Transaction();
            var TransactionCreditObj = new Transaction();
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var PaperFilter = new PaperFilterVM
            {
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                CompanyTransactionKind = CompanyTransactionKindObj,
                CompanyTransactionKindToTransfer = CompanyTransactionKindToTransferObj,
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                FCurrencyID = 1,
                Header = HeadrObj,
                TransactionDebit = TransactionDebitObj,
                TransactionCredit = TransactionCreditObj,
                WorkWithCostCenter = Company.WorkWithCostCenter,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                CurrentYear = UserInfo.CurrYear
            };
            return View(PaperFilter);
        }
        [HttpPost]
        public JsonResult GetAllPaymentChequeUnderCollection(PaperFilterVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllPaper = _unitOfWork.NativeSql.GetAllPaymentChequeUnderCollection(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
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
                if (!String.IsNullOrEmpty(Obj.AccountNumberFourth))
                {
                    AllPaper = AllPaper.Where(m => m.IsBill == 1 ? m.BillCustomerNumber == Obj.AccountNumberFourth : m.AccountNumberFourth == Obj.AccountNumberFourth).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.AccountNumberFifth))
                {
                    AllPaper = AllPaper.Where(m => m.AccountNumberFifth == Obj.AccountNumberFifth).ToList();
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
        public JsonResult SavePaymentChequeUnderCollection(PaperFilterVM ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var SaveHeader = ObjToSave.Header;
                SaveHeader.InsDateTime = DateTime.Now;
                SaveHeader.InsUserID = userId;
                SaveHeader.CompanyTransactionKindNo = ObjToSave.CompanyTransactionKindToTransferNo;
                SaveHeader.CompanyYear = UserInfo.CurrYear;
                var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                SaveHeader.TransactionKindNo = _unitOfWork.CompanyTransactionKind.GetTransKindForCompanyTransactionKind(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo);
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
                SaveHeader.CompanyID = UserInfo.fCompanyId;
                SaveHeader.SaleID = 0;
                SaveHeader.Exported = 1;
                if (ObjToSave.CurrencyID == 0)
                {
                    ObjToSave.CurrencyID = 1;
                }

                SaveHeader.FCurrencyID = ObjToSave.CurrencyID;
                SaveHeader.VHI = SaveHeader.VHI;
                if (Resources.Resource.CurLang == "Arb")
                {
                    SaveHeader.Note = "محصل من شيكات برسم التحصيل";
                }
                else
                {
                    SaveHeader.Note = "Payment Under Collection Cheque";
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
                        TransNote = " تحصيل شيك رقم : " + UpdatePaper.ChequeNumber + " - " + " تاريخ الحق : " + dDateAfterRemoveTime.ToString("dd/MM/yyyy") + " - " + " الحساب المدفوع منه : " +
                            _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, UpdatePaper.AccountNumberFourth);
                    }
                    else
                    {
                        TransNote = " Payment Cheque Number : " + UpdatePaper.ChequeNumber + " - " + " Cheque Date : " + dDateAfterRemoveTime.ToString("dd/MM/yyyy") + " - " + " Paid Account : " +
                            _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, UpdatePaper.AccountNumberFourth);
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
                    UpdatePaper.ChequeCase = 3;
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
                    TransactionDebit.AccountNumber = UpdatePaper.AccountNumberFifth;
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
                    TransactionCredit.AccountNumber = UpdatePaper.AccountNumberSecond;
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

                    UpdatePaper.AccountNumberSecond = UpdatePaper.AccountNumberFifth;
                    UpdatePaper.AccountNumberFifth = "";
                    _unitOfWork.NativeSql.SaveTracingPaper(UpdatePaper.CompanyID, UpdatePaper.CompanyYear, UpdatePaper.RowNumber, UpdatePaper.OldVoucherNumber,
                                                           UpdatePaper.OldCompanyTransactionKindNo, UpdatePaper.OldTransactionKindNo, UpdatePaper.ChequeNumber, UpdatePaper.sChequeDate,
                                                           UpdatePaper.ChequeAmount, UpdatePaper.AccountNumberThird);
                    _unitOfWork.NativeSql.UpdatePaymentPaperUnderCollection(UpdatePaper.CompanyID, UpdatePaper.CompanyYear, UpdatePaper.RowNumber, UpdatePaper.OldVoucherNumber, UpdatePaper.OldCompanyTransactionKindNo,
                        UpdatePaper.OldTransactionKindNo, UpdatePaper.OldVHI, UpdatePaper.CompanyTransactionKindNo, UpdatePaper.TransactionKindNo, UpdatePaper.VoucherDate, UpdatePaper.VoucherNumber,
                        UpdatePaper.VHI, UpdatePaper.ChequeCase, UpdatePaper.AccountNumberSecond, UpdatePaper.Remark, UpdatePaper.InsUserID, UpdatePaper.InsDateTime, 
                        UpdatePaper.AccountNumberFifth, UpdatePaper.ChequeNumber, UpdatePaper.sChequeDate,UpdatePaper.ChequeAmount, UpdatePaper.AccountNumberThird);
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
        [Authorize(Roles = "CoOwner,ShowPaymentChequeEndorsement")]
        public ActionResult PaymentChequeEndorsement()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetReceiptBankTransactionCompanyTransactionKind(UserInfo.fCompanyId);
            var FromDate = _unitOfWork.NativeSql.GetFromDateEndorsementCheque(UserInfo.fCompanyId);
            var ToDate = _unitOfWork.NativeSql.GetToDateEndorsementCheque(UserInfo.fCompanyId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var PaperFilter = new PaperFilterVM
            {
                FromDate = FromDate,
                ToDate = ToDate,
                CompanyTransactionKind = CompanyTransactionKindObj,
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                FCurrencyID = 1,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency
            };
            return View(PaperFilter);
        }
        [HttpPost]
        public JsonResult GetAllPaymentChequeEndorsement(PaperFilterVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllPaper = _unitOfWork.NativeSql.GetAllPaymentChequeEndorsement(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
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
                if (!String.IsNullOrEmpty(Obj.AccountNumberFourth))
                {
                    AllPaper = AllPaper.Where(m => m.IsBill == 1 ? m.BillCustomerNumber == Obj.AccountNumberFourth : m.AccountNumberFourth == Obj.AccountNumberFourth).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.AccountNumberSecond))
                {
                    AllPaper = AllPaper.Where(m => m.AccountNumberSecond == Obj.AccountNumberSecond).ToList();
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
        public JsonResult SavePaymentChequeEndorsement(PaperFilterVM ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var Note = "";
                if (Resources.Resource.CurLang == "Arb")
                {
                     Note = "مسدد من شيكات مجيرة";
                }
                else
                {
                    Note = "Payment Endorsement Cheque";
                }
                foreach (var UpdatePaper in ObjToSave.Paper)
                {
                    UpdatePaper.RowNumber = UpdatePaper.RowNumber;
                    UpdatePaper.CompanyYear = UpdatePaper.CompanyYear;
                    UpdatePaper.CompanyID = UserInfo.fCompanyId;
                    UpdatePaper.OldVoucherNumber = UpdatePaper.VoucherNumber;
                    UpdatePaper.OldCompanyTransactionKindNo = UpdatePaper.CompanyTransactionKindNo;
                    UpdatePaper.OldTransactionKindNo = _unitOfWork.CompanyTransactionKind.GetTransKindForCompanyTransactionKind(UserInfo.fCompanyId, UpdatePaper.OldCompanyTransactionKindNo);
                    UpdatePaper.OldVHI = UpdatePaper.VHI;
                    UpdatePaper.ChequeCase = 11;
                    UpdatePaper.Remark = Note;
                    UpdatePaper.InsUserID = userId;
                    UpdatePaper.InsDateTime = DateTime.Now;
                    _unitOfWork.NativeSql.SaveTracingPaper(UpdatePaper.CompanyID, UpdatePaper.CompanyYear, UpdatePaper.RowNumber, UpdatePaper.OldVoucherNumber,
                                                           UpdatePaper.OldCompanyTransactionKindNo, UpdatePaper.OldTransactionKindNo, UpdatePaper.ChequeNumber, UpdatePaper.sChequeDate,
                                                           UpdatePaper.ChequeAmount, UpdatePaper.AccountNumberThird);
                    _unitOfWork.NativeSql.UpdatePaymentPaperEndorsement(UpdatePaper.CompanyID, UpdatePaper.CompanyYear, UpdatePaper.RowNumber, UpdatePaper.OldVoucherNumber, UpdatePaper.OldCompanyTransactionKindNo,
                        UpdatePaper.OldTransactionKindNo, UpdatePaper.OldVHI,UpdatePaper.ChequeCase, UpdatePaper.Remark, UpdatePaper.InsUserID, UpdatePaper.InsDateTime, 
                        UpdatePaper.ChequeNumber, UpdatePaper.sChequeDate, UpdatePaper.ChequeAmount, UpdatePaper.AccountNumberThird);
                    _unitOfWork.Complete();

                }

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
        [Authorize(Roles = "CoOwner,ShowDrawingUC")]
        public ActionResult DrawingChequeUnderCollection()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetReceiptBankTransactionCompanyTransactionKind(UserInfo.fCompanyId);
            var CompanyTransactionKindToTransferObj = _unitOfWork.NativeSql.GetReceiptBankTransactionCompanyTransactionKind(UserInfo.fCompanyId);
            var FromDate = _unitOfWork.NativeSql.GetFromDateUbderCollectionCheque(UserInfo.fCompanyId);
            var ToDate = _unitOfWork.NativeSql.GetToDateUbderCollectionCheque(UserInfo.fCompanyId);
            int year = DateTime.Now.Year;
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
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                CurrentYear = UserInfo.CurrYear
            };
            return View(PaperFilter);
        }
        [HttpPost]
        public JsonResult GetAllDrawingChequeUnderCollection(PaperFilterVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllPaper = _unitOfWork.NativeSql.GetAllDrawingChequeUnderCollection(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
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
                if (!String.IsNullOrEmpty(Obj.AccountNumberFourth))
                {
                    AllPaper = AllPaper.Where(m => m.IsBill == 1 ? m.BillCustomerNumber == Obj.AccountNumberFourth : m.AccountNumberFourth == Obj.AccountNumberFourth).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.AccountNumberFifth))
                {
                    AllPaper = AllPaper.Where(m => m.AccountNumberFifth == Obj.AccountNumberFifth).ToList();
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
        public JsonResult SaveDrawingChequeUnderCollection(PaperFilterVM ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var SaveHeader = ObjToSave.Header;
                SaveHeader.InsDateTime = DateTime.Now;
                SaveHeader.InsUserID = userId;
                SaveHeader.CompanyTransactionKindNo = ObjToSave.CompanyTransactionKindToTransferNo;
                SaveHeader.CompanyYear = UserInfo.CurrYear;
                var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                SaveHeader.TransactionKindNo = _unitOfWork.CompanyTransactionKind.GetTransKindForCompanyTransactionKind(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo);
                var ObjComapnyTransactionKind = _unitOfWork.CompanyTransactionKind.GetCompanyTransactionKindByID(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo);
                if (ObjComapnyTransactionKind.AutoSerial)
                {
                    SaveHeader.VoucherNumber = _unitOfWork.Header.GetMaxVHByKind(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo, SaveHeader.TransactionKindNo, SaveHeader.CompanyYear).ToString();
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
                if (ObjToSave.CurrencyID == 0)
                {
                    ObjToSave.CurrencyID = 1;
                }

                SaveHeader.FCurrencyID = ObjToSave.CurrencyID;
                SaveHeader.VHI = SaveHeader.VHI;
                if (Resources.Resource.CurLang == "Arb")
                {
                    SaveHeader.Note = "مسحوب من شيكات برسم التحصيل";
                }
                else
                {
                    SaveHeader.Note = "Drawing Cheque Under Collection";
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
                        TransNote = " سحب شيك رقم : " + UpdatePaper.ChequeNumber + " - " + " تاريخ الحق : " + dDateAfterRemoveTime.ToString("dd/MM/yyyy") + " - " + " الحساب المدفوع منه : " +
                            _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, UpdatePaper.AccountNumberFourth);
                    }
                    else
                    {
                        TransNote = " Drawing Cheque Number : " + UpdatePaper.ChequeNumber + " - " + " Cheque Date : " + dDateAfterRemoveTime.ToString("dd/MM/yyyy") + " - " + " Paid Account : " +
                            _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, UpdatePaper.AccountNumberFourth);
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
                    UpdatePaper.ChequeCase = 7;
                    UpdatePaper.Remark = SaveHeader.Note;
                    UpdatePaper.InsUserID = SaveHeader.InsUserID;
                    UpdatePaper.InsDateTime = DateTime.Now;
                    UpdatePaper.UnderReturnNote = ObjToSave.ReturnNote;
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
                    TransactionDebit.AccountNumber = UpdatePaper.AccountNumberFourth;
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
                    TransactionCredit.AccountNumber = UpdatePaper.AccountNumberSecond;
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
                    _unitOfWork.NativeSql.UpdateDrawingPaperUnderCollection(UpdatePaper.CompanyID, UpdatePaper.CompanyYear, UpdatePaper.RowNumber, UpdatePaper.OldVoucherNumber, UpdatePaper.OldCompanyTransactionKindNo,
                        UpdatePaper.OldTransactionKindNo, UpdatePaper.OldVHI, UpdatePaper.CompanyTransactionKindNo, UpdatePaper.TransactionKindNo, UpdatePaper.VoucherDate, UpdatePaper.VoucherNumber,
                        UpdatePaper.VHI, UpdatePaper.ChequeCase,UpdatePaper.Remark, UpdatePaper.InsUserID, UpdatePaper.InsDateTime, 
                        UpdatePaper.UnderReturnNote, UpdatePaper.ChequeNumber, UpdatePaper.sChequeDate, UpdatePaper.ChequeAmount, UpdatePaper.AccountNumberThird);
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

        [Authorize(Roles = "CoOwner,ShowReturnChequeUnderCollection")]
        public ActionResult ReturnChequeUnderCollection()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetReceiptBankTransactionCompanyTransactionKind(UserInfo.fCompanyId);
            var CompanyTransactionKindToTransferObj = _unitOfWork.NativeSql.GetReceiptBankTransactionCompanyTransactionKind(UserInfo.fCompanyId);
            var FromDate = _unitOfWork.NativeSql.GetFromDatePaymentUbderCollectionCheque(UserInfo.fCompanyId);
            var ToDate = _unitOfWork.NativeSql.GetToDatePaymentUbderCollectionCheque(UserInfo.fCompanyId);
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
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                CurrentYear = UserInfo.CurrYear
            };
            return View(PaperFilter);
        }
        [HttpPost]
        public JsonResult GetAllReturnChequeUnderCollection(PaperFilterVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllPaper = _unitOfWork.NativeSql.GetAllReturnChequeUnderCollection(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
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
                if (!String.IsNullOrEmpty(Obj.AccountNumberFourth))
                {
                    AllPaper = AllPaper.Where(m => m.IsBill == 1 ? m.BillCustomerNumber == Obj.AccountNumberFourth : m.AccountNumberFourth == Obj.AccountNumberFourth).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.AccountNumberSecond))
                {
                    AllPaper = AllPaper.Where(m => m.AccountNumberSecond == Obj.AccountNumberSecond).ToList();
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
        public JsonResult SaveReturnChequeUnderCollection(PaperFilterVM ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var SaveHeader = ObjToSave.Header;
                SaveHeader.InsDateTime = DateTime.Now;
                SaveHeader.InsUserID = userId;
                SaveHeader.CompanyTransactionKindNo = ObjToSave.CompanyTransactionKindToTransferNo;
                SaveHeader.CompanyYear = UserInfo.CurrYear;
                var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                SaveHeader.TransactionKindNo = _unitOfWork.CompanyTransactionKind.GetTransKindForCompanyTransactionKind(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo);
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
                SaveHeader.CompanyID = UserInfo.fCompanyId;
                SaveHeader.SaleID = 0;
                SaveHeader.Exported = 1;
                if (ObjToSave.CurrencyID == 0)
                {
                    ObjToSave.CurrencyID = 1;
                }

                SaveHeader.FCurrencyID = ObjToSave.CurrencyID;
                SaveHeader.VHI = SaveHeader.VHI;
                if (Resources.Resource.CurLang == "Arb")
                {
                    SaveHeader.Note = "شيك معاد من رسم التحصيل";
                }
                else
                {
                    SaveHeader.Note = "Return Cheque Under Collection";
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
                        TransNote = " رقم الشيك المعاد من رسم التحصيل : " + UpdatePaper.ChequeNumber + " - " + " تاريخ الحق : " + dDateAfterRemoveTime.ToString("dd/MM/yyyy") + " - "  + " الحساب المدفوع منه : " +
                            _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, UpdatePaper.AccountNumberFourth);
                    }
                    else
                    {
                        TransNote = " Cheque Number Return Under Collection : " + UpdatePaper.ChequeNumber + " - " + " Cheque Date : " + dDateAfterRemoveTime.ToString("dd/MM/yyyy") + " - " + " Paid Account : " +
                            _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, UpdatePaper.AccountNumberFourth);
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
                    UpdatePaper.ChequeCase = 6;
                    UpdatePaper.Remark = SaveHeader.Note;
                    UpdatePaper.InsUserID = SaveHeader.InsUserID;
                    UpdatePaper.InsDateTime = DateTime.Now;
                    UpdatePaper.ReturnNote = ObjToSave.ReturnNote;
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
                    TransactionDebit.AccountNumber = UpdatePaper.AccountNumberFourth;
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
                    TransactionCredit.AccountNumber = UpdatePaper.AccountNumberSecond;
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
                    _unitOfWork.NativeSql.UpdateReturnPaperUnderCollection(UpdatePaper.CompanyID, UpdatePaper.CompanyYear, UpdatePaper.RowNumber, UpdatePaper.OldVoucherNumber, UpdatePaper.OldCompanyTransactionKindNo,
                        UpdatePaper.OldTransactionKindNo, UpdatePaper.OldVHI, UpdatePaper.CompanyTransactionKindNo, UpdatePaper.TransactionKindNo, UpdatePaper.VoucherDate, UpdatePaper.VoucherNumber,
                        UpdatePaper.VHI, UpdatePaper.ChequeCase, UpdatePaper.Remark, UpdatePaper.InsUserID, UpdatePaper.InsDateTime, UpdatePaper.ReturnNote, 
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
        [Authorize(Roles = "CoOwner,ShowReturnAChequeCD")]
        public ActionResult ReturnChequeClearingDeposit()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetReceiptBankTransactionCompanyTransactionKind(UserInfo.fCompanyId);
            var CompanyTransactionKindToTransferObj = _unitOfWork.NativeSql.GetReceiptBankTransactionCompanyTransactionKind(UserInfo.fCompanyId);
            var FromDate = _unitOfWork.NativeSql.GetFromDateClearingDepositCheque(UserInfo.fCompanyId);
            var ToDate = _unitOfWork.NativeSql.GetToDateClearingDepositCheque(UserInfo.fCompanyId);
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
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                CurrentYear = UserInfo.CurrYear
            };
            return View(PaperFilter);
        }
        [HttpPost]
        public JsonResult GetAllReturnChequeClearingDeposit(PaperFilterVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllPaper = _unitOfWork.NativeSql.GetAllReturnChequeClearingDeposit(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
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
                if (!String.IsNullOrEmpty(Obj.AccountNumberFourth))
                {
                    AllPaper = AllPaper.Where(m => m.IsBill == 1 ? m.BillCustomerNumber == Obj.AccountNumberFourth : m.AccountNumberFourth == Obj.AccountNumberFourth).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.AccountNumberSecond))
                {
                    AllPaper = AllPaper.Where(m => m.AccountNumberSecond == Obj.AccountNumberSecond).ToList();
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
        public JsonResult SaveReturnChequeClearingDeposit(PaperFilterVM ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var SaveHeader = ObjToSave.Header;
                SaveHeader.InsDateTime = DateTime.Now;
                SaveHeader.InsUserID = userId;
                SaveHeader.CompanyTransactionKindNo = ObjToSave.CompanyTransactionKindToTransferNo;
                SaveHeader.CompanyYear = UserInfo.CurrYear;
                var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                SaveHeader.TransactionKindNo = _unitOfWork.CompanyTransactionKind.GetTransKindForCompanyTransactionKind(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo);
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
                SaveHeader.CompanyID = UserInfo.fCompanyId;
                SaveHeader.SaleID = 0;
                SaveHeader.Exported = 1;
                if (ObjToSave.CurrencyID == 0)
                {
                    ObjToSave.CurrencyID = 1;
                }

                SaveHeader.FCurrencyID = ObjToSave.CurrencyID;
                SaveHeader.VHI = SaveHeader.VHI;
                if (Resources.Resource.CurLang == "Arb")
                {
                    SaveHeader.Note = "شيك معاد من إيداع مقاصة";
                }
                else
                {
                    SaveHeader.Note = "Return Cheque Clearing Deposit";
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
                        TransNote = " رقم الشيك معاد إيداع مقاصة : " + UpdatePaper.ChequeNumber + " - " + " تاريخ الحق : " + dDateAfterRemoveTime.ToString("dd/MM/yyyy") + " - " + " الحساب المدفوع منه : " +
                            _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, UpdatePaper.AccountNumberFourth);
                    }
                    else
                    {
                        TransNote = " Cheque Number Return Clearing Deposit : " + UpdatePaper.ChequeNumber + " - " + " Cheque Date : " + dDateAfterRemoveTime.ToString("dd/MM/yyyy") + " - " + " Paid Account : " +
                            _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, UpdatePaper.AccountNumberFourth);
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
                    UpdatePaper.ChequeCase = 6;
                    UpdatePaper.Remark = SaveHeader.Note;
                    UpdatePaper.InsUserID = SaveHeader.InsUserID;
                    UpdatePaper.InsDateTime = DateTime.Now;
                    UpdatePaper.ReturnNote = ObjToSave.ReturnNote;
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
                    TransactionDebit.AccountNumber = UpdatePaper.AccountNumberFourth;
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
                    TransactionCredit.AccountNumber = UpdatePaper.AccountNumberSecond;
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
                    _unitOfWork.NativeSql.UpdateReturngPaperClearingDeposit(UpdatePaper.CompanyID, UpdatePaper.CompanyYear, UpdatePaper.RowNumber, UpdatePaper.OldVoucherNumber, UpdatePaper.OldCompanyTransactionKindNo,
                        UpdatePaper.OldTransactionKindNo, UpdatePaper.OldVHI, UpdatePaper.CompanyTransactionKindNo, UpdatePaper.TransactionKindNo, UpdatePaper.VoucherDate, UpdatePaper.VoucherNumber,
                        UpdatePaper.VHI, UpdatePaper.ChequeCase, UpdatePaper.Remark, UpdatePaper.InsUserID, UpdatePaper.InsDateTime, UpdatePaper.ReturnNote, 
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
        [Authorize(Roles = "CoOwner,ShowReturnChequeEndorsement")]
        public ActionResult ReturnChequeEndorsement()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetReceiptBankTransactionCompanyTransactionKind(UserInfo.fCompanyId);
            var CompanyTransactionKindToTransferObj = _unitOfWork.NativeSql.GetReceiptBankTransactionCompanyTransactionKind(UserInfo.fCompanyId);
            var FromDate = _unitOfWork.NativeSql.GetFromDateEndorsementCheque(UserInfo.fCompanyId);
            var ToDate = _unitOfWork.NativeSql.GetToDateEndorsementCheque(UserInfo.fCompanyId);
            int year = DateTime.Now.Year;
            var HeadrObj = new Header();
            HeadrObj.VoucherDate = DateTime.Now;
            var TransactionDebitObj = new Transaction();
            var TransactionCreditObj = new Transaction();
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var PaperFilter = new PaperFilterVM
            {
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                CompanyTransactionKind = CompanyTransactionKindObj,
                CompanyTransactionKindToTransfer = CompanyTransactionKindToTransferObj,
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                FCurrencyID = 1,
                Header = HeadrObj,
                TransactionDebit = TransactionDebitObj,
                TransactionCredit = TransactionCreditObj,
                WorkWithCostCenter = Company.WorkWithCostCenter,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                CurrentYear = UserInfo.CurrYear
            };
            return View(PaperFilter);
        }
        [HttpPost]
        public JsonResult GetAllReturnChequeEndorsement(PaperFilterVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllPaper = _unitOfWork.NativeSql.GetAllReturnChequeEndorsement(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
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
                if (!String.IsNullOrEmpty(Obj.AccountNumberFourth))
                {
                    AllPaper = AllPaper.Where(m => m.IsBill == 1 ? m.BillCustomerNumber == Obj.AccountNumberFourth : m.AccountNumberFourth == Obj.AccountNumberFourth).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.AccountNumberSecond))
                {
                    AllPaper = AllPaper.Where(m => m.AccountNumberSecond == Obj.AccountNumberSecond).ToList();
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
        public JsonResult SaveReturnChequeEndorsement(PaperFilterVM ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var SaveHeader = ObjToSave.Header;
                SaveHeader.InsDateTime = DateTime.Now;
                SaveHeader.InsUserID = userId;
                SaveHeader.CompanyTransactionKindNo = ObjToSave.CompanyTransactionKindToTransferNo;
                SaveHeader.CompanyYear = UserInfo.CurrYear;
                var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                SaveHeader.TransactionKindNo = _unitOfWork.CompanyTransactionKind.GetTransKindForCompanyTransactionKind(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo);
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
                SaveHeader.CompanyID = UserInfo.fCompanyId;
                SaveHeader.SaleID = 0;
                SaveHeader.Exported = 1;
                if (ObjToSave.CurrencyID == 0)
                {
                    ObjToSave.CurrencyID = 1;
                }

                SaveHeader.FCurrencyID = ObjToSave.CurrencyID;
                SaveHeader.VHI = SaveHeader.VHI;
                if (Resources.Resource.CurLang == "Arb")
                {
                    SaveHeader.Note = "شيك مجير معاد";
                }
                else
                {
                    SaveHeader.Note = "Return Cheque Endorsement";
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
                        TransNote = " رقم شيك مجير معاد : " + UpdatePaper.ChequeNumber + " - " + " تاريخ الحق : " + dDateAfterRemoveTime.ToString("dd/MM/yyyy") + " - " + " الحساب المدفوع منه : " +
                            _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, UpdatePaper.AccountNumberFourth);
                    }
                    else
                    {
                        TransNote = " Cheque Number Return Endorsement: " + UpdatePaper.ChequeNumber + " - " + " Cheque Date : " + dDateAfterRemoveTime.ToString("dd/MM/yyyy") + " - " + " Paid Account : " +
                            _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, UpdatePaper.AccountNumberFourth);
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
                    UpdatePaper.ChequeCase = 10;
                    UpdatePaper.Remark = SaveHeader.Note;
                    UpdatePaper.InsUserID = SaveHeader.InsUserID;
                    UpdatePaper.InsDateTime = DateTime.Now;
                    UpdatePaper.EndorReturnNote = ObjToSave.ReturnNote;
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
                    TransactionDebit.AccountNumber = UpdatePaper.AccountNumberFourth;
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
                    TransactionCredit.AccountNumber = UpdatePaper.AccountNumberSecond;
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
                    _unitOfWork.NativeSql.UpdateReturngPaperEndorsement(UpdatePaper.CompanyID, UpdatePaper.CompanyYear, UpdatePaper.RowNumber, UpdatePaper.OldVoucherNumber, UpdatePaper.OldCompanyTransactionKindNo,
                        UpdatePaper.OldTransactionKindNo, UpdatePaper.OldVHI, UpdatePaper.CompanyTransactionKindNo, UpdatePaper.TransactionKindNo, UpdatePaper.VoucherDate, UpdatePaper.VoucherNumber,
                        UpdatePaper.VHI, UpdatePaper.ChequeCase, UpdatePaper.Remark, UpdatePaper.InsUserID, UpdatePaper.InsDateTime, UpdatePaper.EndorReturnNote, 
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
        [Authorize(Roles = "CoOwner,ShowFundChequesDrawnFromUC")]
        public ActionResult FundChequesDrawnFromUnderCollection()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetReceiptBankTransactionCompanyTransactionKind(UserInfo.fCompanyId);
            var CompanyTransactionKindToTransferObj = _unitOfWork.NativeSql.GetReceiptBankTransactionCompanyTransactionKind(UserInfo.fCompanyId);
            var FromDate = _unitOfWork.NativeSql.GetFromDateDrawingUnderCollectionCheque(UserInfo.fCompanyId);
            var ToDate = _unitOfWork.NativeSql.GetToDateDrawingUnderCollectionCheque(UserInfo.fCompanyId);
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
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                CurrentYear = UserInfo.CurrYear
            };
            return View(PaperFilter);
        }
        [HttpPost] 
        public JsonResult GetAllFundChequesDrawnFromUnderCollection(PaperFilterVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllPaper = _unitOfWork.NativeSql.GetAllFundChequesDrawnFromUnderCollection(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
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
                if (!String.IsNullOrEmpty(Obj.AccountNumberFourth))
                {
                    AllPaper = AllPaper.Where(m => m.IsBill == 1 ? m.BillCustomerNumber == Obj.AccountNumberFourth : m.AccountNumberFourth == Obj.AccountNumberFourth).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.AccountNumberFifth))
                {
                    AllPaper = AllPaper.Where(m => m.AccountNumberFifth == Obj.AccountNumberFifth).ToList();
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
        public JsonResult SaveReturnToPaidAccount(PaperFilterVM ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var Note = "";
                if (Resources.Resource.CurLang == "Arb")
                {
                    Note = "إعادة شيك الى حساب المدفوع منه";
                }
                else
                {
                    Note = "Return Cheque To Paid Account";
                }
                foreach (var UpdatePaper in ObjToSave.Paper)
                {
                    UpdatePaper.RowNumber = UpdatePaper.RowNumber;
                    UpdatePaper.CompanyYear = UpdatePaper.CompanyYear;
                    UpdatePaper.CompanyID = UserInfo.fCompanyId;
                    UpdatePaper.OldVoucherNumber = UpdatePaper.VoucherNumber;
                    UpdatePaper.OldCompanyTransactionKindNo = UpdatePaper.CompanyTransactionKindNo;
                    UpdatePaper.OldTransactionKindNo = _unitOfWork.CompanyTransactionKind.GetTransKindForCompanyTransactionKind(UserInfo.fCompanyId, UpdatePaper.OldCompanyTransactionKindNo);
                    UpdatePaper.OldVHI = UpdatePaper.VHI;
                    UpdatePaper.ChequeCase = 9;
                    UpdatePaper.Remark = Note;
                    UpdatePaper.InsUserID = userId;
                    UpdatePaper.InsDateTime = DateTime.Now;
                    UpdatePaper.UnderReturnCustomerNote = ObjToSave.ReturnNote;
                    _unitOfWork.NativeSql.SaveTracingPaper(UpdatePaper.CompanyID, UpdatePaper.CompanyYear, UpdatePaper.RowNumber, UpdatePaper.OldVoucherNumber,
                                                           UpdatePaper.OldCompanyTransactionKindNo, UpdatePaper.OldTransactionKindNo, UpdatePaper.ChequeNumber, UpdatePaper.sChequeDate,
                                                           UpdatePaper.ChequeAmount, UpdatePaper.AccountNumberThird);
                    _unitOfWork.NativeSql.UpdateReturnToPaidAccount(UpdatePaper.CompanyID, UpdatePaper.CompanyYear, UpdatePaper.RowNumber, UpdatePaper.OldVoucherNumber, UpdatePaper.OldCompanyTransactionKindNo,
                        UpdatePaper.OldTransactionKindNo, UpdatePaper.OldVHI, UpdatePaper.ChequeCase, UpdatePaper.Remark, UpdatePaper.InsUserID, UpdatePaper.InsDateTime,
                        UpdatePaper.ChequeNumber, UpdatePaper.sChequeDate, UpdatePaper.ChequeAmount, UpdatePaper.AccountNumberThird, UpdatePaper.UnderReturnCustomerNote);
                    _unitOfWork.Complete();

                }

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
        public JsonResult SaveReturnToUnderCollection(PaperFilterVM ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var SaveHeader = ObjToSave.Header;
                SaveHeader.InsDateTime = DateTime.Now;
                SaveHeader.InsUserID = userId;
                SaveHeader.CompanyTransactionKindNo = ObjToSave.CompanyTransactionKindToTransferNo;
                SaveHeader.CompanyYear = UserInfo.CurrYear;
                var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                SaveHeader.TransactionKindNo = _unitOfWork.CompanyTransactionKind.GetTransKindForCompanyTransactionKind(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo);
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
                SaveHeader.CompanyID = UserInfo.fCompanyId;
                SaveHeader.SaleID = 0;
                SaveHeader.Exported = 1;
                if (ObjToSave.CurrencyID == 0)
                {
                    ObjToSave.CurrencyID = 1;
                }

                SaveHeader.FCurrencyID = ObjToSave.CurrencyID;

                SaveHeader.VHI = SaveHeader.VHI;
                if (Resources.Resource.CurLang == "Arb")
                {
                    SaveHeader.Note = "معاد تحويله الى رسم التحصيل";
                }
                else
                {
                    SaveHeader.Note = "Return Transfer To Under Collection";
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
                        TransNote = " رقم شيك معاد تحويله لرسم التحصيل : " + UpdatePaper.ChequeNumber + " - " + " تاريخ الحق : " + dDateAfterRemoveTime.ToString("dd/MM/yyyy") + " - " + " الحساب المدفوع منه : " +
                            _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, UpdatePaper.AccountNumberFourth);
                    }
                    else
                    {
                        TransNote = " Cheque Number Return Transfer To Under Collection : " + UpdatePaper.ChequeNumber + " - " + " Cheque Date : " + dDateAfterRemoveTime.ToString("dd/MM/yyyy") + " - " + " Paid Account : " +
                            _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, UpdatePaper.AccountNumberFourth);
                    }
                    UpdatePaper.RowNumber = UpdatePaper.RowNumber;
                    UpdatePaper.CompanyYear = UpdatePaper.CompanyYear;
                    UpdatePaper.CompanyID = UserInfo.fCompanyId;
                    UpdatePaper.OldVoucherNumber = UpdatePaper.VoucherNumber;
                    UpdatePaper.OldCompanyTransactionKindNo = UpdatePaper.CompanyTransactionKindNo;
                    UpdatePaper.OldTransactionKindNo = _unitOfWork.CompanyTransactionKind.GetTransKindForCompanyTransactionKind(UserInfo.fCompanyId, UpdatePaper.OldCompanyTransactionKindNo);
                    UpdatePaper.OldVHI = UpdatePaper.VHI;
                    UpdatePaper.AccountNumberFourth = UpdatePaper.AccountNumberFourth;
                    UpdatePaper.AccountNumberFifth = ObjToSave.BankUnderCollectionAccountNumber;
                    UpdatePaper.CompanyTransactionKindNo = SaveHeader.CompanyTransactionKindNo;
                    UpdatePaper.TransactionKindNo = SaveHeader.TransactionKindNo;
                    UpdatePaper.VoucherDate = SaveHeader.VoucherDate;
                    UpdatePaper.VoucherNumber = SaveHeader.VoucherNumber;
                    UpdatePaper.VHI = SaveHeader.VHI;
                    UpdatePaper.ChequeCase = 2;
                    UpdatePaper.AccountNumberSecond = ObjToSave.UnderCollectionAccountNumber;
                    UpdatePaper.Remark = SaveHeader.Note;
                    UpdatePaper.InsUserID = SaveHeader.InsUserID;
                    UpdatePaper.InsDateTime = DateTime.Now;
                    Amount = Amount + UpdatePaper.ChequeAmount;
                    _unitOfWork.NativeSql.SaveTracingPaper(UpdatePaper.CompanyID, UpdatePaper.CompanyYear, UpdatePaper.RowNumber, UpdatePaper.OldVoucherNumber,
                                                           UpdatePaper.OldCompanyTransactionKindNo, UpdatePaper.OldTransactionKindNo, UpdatePaper.ChequeNumber, UpdatePaper.sChequeDate,
                                                           UpdatePaper.ChequeAmount, UpdatePaper.AccountNumberThird);
                    _unitOfWork.NativeSql.UpdatePaper(UpdatePaper.CompanyID, UpdatePaper.CompanyYear, UpdatePaper.RowNumber, UpdatePaper.OldVoucherNumber, UpdatePaper.OldCompanyTransactionKindNo,
                        UpdatePaper.OldTransactionKindNo, UpdatePaper.OldVHI, UpdatePaper.CompanyTransactionKindNo, UpdatePaper.TransactionKindNo, UpdatePaper.VoucherDate, UpdatePaper.VoucherNumber,
                        UpdatePaper.VHI, UpdatePaper.ChequeCase, UpdatePaper.AccountNumberSecond, UpdatePaper.AccountNumberFourth, UpdatePaper.Remark,
                        UpdatePaper.InsUserID, UpdatePaper.InsDateTime, UpdatePaper.AccountNumberFifth, UpdatePaper.ChequeNumber, UpdatePaper.sChequeDate,
                        UpdatePaper.ChequeAmount, UpdatePaper.AccountNumberThird);
                    _unitOfWork.Complete();

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
                    TransactionDebit.AccountNumber = ObjToSave.UnderCollectionAccountNumber;
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
                    TransactionCredit.AccountNumber = UpdatePaper.AccountNumberFourth;
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
        public JsonResult SaveReturnToClearingDeposit(PaperFilterVM ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var SaveHeader = ObjToSave.Header;
                SaveHeader.InsDateTime = DateTime.Now;
                SaveHeader.InsUserID = userId;
                SaveHeader.CompanyTransactionKindNo = ObjToSave.CompanyTransactionKindToTransferNo;
                SaveHeader.CompanyYear = UserInfo.CurrYear;
                var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                SaveHeader.TransactionKindNo = _unitOfWork.CompanyTransactionKind.GetTransKindForCompanyTransactionKind(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo);
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
                SaveHeader.CompanyID = UserInfo.fCompanyId;
                SaveHeader.SaleID = 0;
                SaveHeader.Exported = 1;
                if (ObjToSave.CurrencyID == 0)
                {
                    ObjToSave.CurrencyID = 1;
                }

                SaveHeader.FCurrencyID = ObjToSave.CurrencyID;
                SaveHeader.VHI = SaveHeader.VHI;
                if (Resources.Resource.CurLang == "Arb")
                {
                    SaveHeader.Note = "معاد تحويله إيداع مقاصة";
                }
                else
                {
                    SaveHeader.Note = "Return Transfer To Clearing Deposit";
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
                        TransNote = " رقم الشيك معاد تحويله إيداع مقاصة : " + UpdatePaper.ChequeNumber + " - " + " تاريخ الحق : " + dDateAfterRemoveTime.ToString("dd/MM/yyyy") + " - " + " الحساب المدفوع منه : " +
                            _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, UpdatePaper.AccountNumberFourth);
                    }
                    else
                    {
                        TransNote = " Cheque Number Return Transfer To Clearing Deposit : " + UpdatePaper.ChequeNumber + " - " + " Cheque Date : " + dDateAfterRemoveTime.ToString("dd/MM/yyyy") + " - " + " Paid Account : " +
                            _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, UpdatePaper.AccountNumberFourth);
                    }
                    UpdatePaper.RowNumber = UpdatePaper.RowNumber;
                    UpdatePaper.CompanyYear = UpdatePaper.CompanyYear;
                    UpdatePaper.CompanyID = UserInfo.fCompanyId;
                    UpdatePaper.OldVoucherNumber = UpdatePaper.VoucherNumber;
                    UpdatePaper.OldCompanyTransactionKindNo = UpdatePaper.CompanyTransactionKindNo;
                    UpdatePaper.OldTransactionKindNo = _unitOfWork.CompanyTransactionKind.GetTransKindForCompanyTransactionKind(UserInfo.fCompanyId, UpdatePaper.OldCompanyTransactionKindNo);
                    UpdatePaper.OldVHI = UpdatePaper.VHI;
                    UpdatePaper.AccountNumberFourth = UpdatePaper.AccountNumberFourth;
                    UpdatePaper.AccountNumberFifth = ObjToSave.ClearingDepositAccountNumber;
                    UpdatePaper.CompanyTransactionKindNo = SaveHeader.CompanyTransactionKindNo;
                    UpdatePaper.TransactionKindNo = SaveHeader.TransactionKindNo;
                    UpdatePaper.VoucherDate = SaveHeader.VoucherDate;
                    UpdatePaper.VoucherNumber = SaveHeader.VoucherNumber;
                    UpdatePaper.VHI = SaveHeader.VHI;
                    UpdatePaper.ChequeCase = 4;
                    UpdatePaper.AccountNumberSecond = ObjToSave.ClearingDepositAccountNumber;
                    UpdatePaper.Remark = SaveHeader.Note;
                    UpdatePaper.InsUserID = SaveHeader.InsUserID;
                    UpdatePaper.InsDateTime = DateTime.Now;
                    Amount = Amount + UpdatePaper.ChequeAmount;
                    _unitOfWork.NativeSql.SaveTracingPaper(UpdatePaper.CompanyID, UpdatePaper.CompanyYear, UpdatePaper.RowNumber, UpdatePaper.OldVoucherNumber,
                                                           UpdatePaper.OldCompanyTransactionKindNo, UpdatePaper.OldTransactionKindNo, UpdatePaper.ChequeNumber, UpdatePaper.sChequeDate,
                                                           UpdatePaper.ChequeAmount, UpdatePaper.AccountNumberThird);
                    _unitOfWork.NativeSql.UpdatePaper(UpdatePaper.CompanyID, UpdatePaper.CompanyYear, UpdatePaper.RowNumber, UpdatePaper.OldVoucherNumber, UpdatePaper.OldCompanyTransactionKindNo,
                        UpdatePaper.OldTransactionKindNo, UpdatePaper.OldVHI, UpdatePaper.CompanyTransactionKindNo, UpdatePaper.TransactionKindNo, UpdatePaper.VoucherDate, UpdatePaper.VoucherNumber,
                        UpdatePaper.VHI, UpdatePaper.ChequeCase, UpdatePaper.AccountNumberSecond, UpdatePaper.AccountNumberFourth, UpdatePaper.Remark,
                        UpdatePaper.InsUserID, UpdatePaper.InsDateTime, UpdatePaper.AccountNumberFifth, UpdatePaper.ChequeNumber, UpdatePaper.sChequeDate,
                        UpdatePaper.ChequeAmount, UpdatePaper.AccountNumberThird);
                    _unitOfWork.Complete();

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
                    TransactionDebit.AccountNumber = ObjToSave.ClearingDepositAccountNumber;
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
                    TransactionCredit.AccountNumber = UpdatePaper.AccountNumberFourth;
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
        [Authorize(Roles = "CoOwner,ShowReturnedChequeFund")]
        public ActionResult ReturnedChequeFund()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetReceiptBankTransactionCompanyTransactionKind(UserInfo.fCompanyId);
            var CompanyTransactionKindToTransferObj = _unitOfWork.NativeSql.GetReceiptBankTransactionCompanyTransactionKind(UserInfo.fCompanyId);
            var FromDate = _unitOfWork.NativeSql.GetFromDateReturnedCheque(UserInfo.fCompanyId);
            var ToDate = _unitOfWork.NativeSql.GetToDateReturnedCheque(UserInfo.fCompanyId);
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
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                CurrentYear = UserInfo.CurrYear
            };
            return View(PaperFilter);
        }
        [HttpPost]
        public JsonResult GetAllReturnedChequeFund(PaperFilterVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllPaper = _unitOfWork.NativeSql.GetAllReturnedChequeFund(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
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
                if (!String.IsNullOrEmpty(Obj.AccountNumberFourth))
                {
                    AllPaper = AllPaper.Where(m => m.IsBill == 1 ? m.BillCustomerNumber == Obj.AccountNumberFourth : m.AccountNumberFourth == Obj.AccountNumberFourth).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.AccountNumberFifth))
                {
                    AllPaper = AllPaper.Where(m => m.AccountNumberFifth == Obj.AccountNumberFifth).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.AccountNumberSecond))
                {
                    AllPaper = AllPaper.Where(m => m.AccountNumberSecond == Obj.AccountNumberSecond).ToList();
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
        public JsonResult SaveReciptReturnToPaidAccount(PaperFilterVM ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var Note = "";
                if (Resources.Resource.CurLang == "Arb")
                {
                    Note = "إعادة شيك الى حساب المدفوع منه";
                }
                else
                {
                    Note = "Return Cheque To Paid Account";
                }
                foreach (var UpdatePaper in ObjToSave.Paper)
                {
                    UpdatePaper.RowNumber = UpdatePaper.RowNumber;
                    UpdatePaper.CompanyYear = UpdatePaper.CompanyYear;
                    UpdatePaper.CompanyID = UserInfo.fCompanyId;
                    UpdatePaper.OldVoucherNumber = UpdatePaper.VoucherNumber;
                    UpdatePaper.OldCompanyTransactionKindNo = UpdatePaper.CompanyTransactionKindNo;
                    UpdatePaper.OldTransactionKindNo = _unitOfWork.CompanyTransactionKind.GetTransKindForCompanyTransactionKind(UserInfo.fCompanyId, UpdatePaper.OldCompanyTransactionKindNo);
                    UpdatePaper.OldVHI = UpdatePaper.VHI;
                    UpdatePaper.ChequeCase = 8;
                    UpdatePaper.Remark = Note;
                    UpdatePaper.InsUserID = userId;
                    UpdatePaper.InsDateTime = DateTime.Now;
                    UpdatePaper.ReturnCustomerNote = ObjToSave.ReturnNote;
                    _unitOfWork.NativeSql.SaveTracingPaper(UpdatePaper.CompanyID, UpdatePaper.CompanyYear, UpdatePaper.RowNumber, UpdatePaper.OldVoucherNumber,
                                                           UpdatePaper.OldCompanyTransactionKindNo, UpdatePaper.OldTransactionKindNo, UpdatePaper.ChequeNumber, UpdatePaper.sChequeDate,
                                                           UpdatePaper.ChequeAmount, UpdatePaper.AccountNumberThird);
                    _unitOfWork.NativeSql.UpdateReturnToPaidAccount1(UpdatePaper.CompanyID, UpdatePaper.CompanyYear, UpdatePaper.RowNumber, UpdatePaper.OldVoucherNumber, UpdatePaper.OldCompanyTransactionKindNo,
                        UpdatePaper.OldTransactionKindNo, UpdatePaper.OldVHI, UpdatePaper.ChequeCase, UpdatePaper.Remark, UpdatePaper.InsUserID, UpdatePaper.InsDateTime,
                        UpdatePaper.ChequeNumber, UpdatePaper.sChequeDate, UpdatePaper.ChequeAmount, UpdatePaper.AccountNumberThird, UpdatePaper.ReturnCustomerNote);
                    _unitOfWork.Complete();

                }

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
        public JsonResult SaveTransferToCourt(PaperFilterVM ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var Note = "";
                if (Resources.Resource.CurLang == "Arb")
                {
                    Note = "شيك محول الى المحكمة";
                }
                else
                {
                    Note = "Cheque Transfer To Court";
                }
                foreach (var UpdatePaper in ObjToSave.Paper)
                {
                    UpdatePaper.RowNumber = UpdatePaper.RowNumber;
                    UpdatePaper.CompanyYear = UpdatePaper.CompanyYear;
                    UpdatePaper.CompanyID = UserInfo.fCompanyId;
                    UpdatePaper.OldVoucherNumber = UpdatePaper.VoucherNumber;
                    UpdatePaper.OldCompanyTransactionKindNo = UpdatePaper.CompanyTransactionKindNo;
                    UpdatePaper.OldTransactionKindNo = _unitOfWork.CompanyTransactionKind.GetTransKindForCompanyTransactionKind(UserInfo.fCompanyId, UpdatePaper.OldCompanyTransactionKindNo);
                    UpdatePaper.OldVHI = UpdatePaper.VHI;
                    UpdatePaper.ChequeCase = 12;
                    UpdatePaper.Remark = Note;
                    UpdatePaper.InsUserID = userId;
                    UpdatePaper.InsDateTime = DateTime.Now;
                    UpdatePaper.CaseNumber = ObjToSave.CaseNumber;
                    _unitOfWork.NativeSql.SaveTracingPaper(UpdatePaper.CompanyID, UpdatePaper.CompanyYear, UpdatePaper.RowNumber, UpdatePaper.OldVoucherNumber,
                                                           UpdatePaper.OldCompanyTransactionKindNo, UpdatePaper.OldTransactionKindNo, UpdatePaper.ChequeNumber, UpdatePaper.sChequeDate,
                                                           UpdatePaper.ChequeAmount, UpdatePaper.AccountNumberThird);
                    _unitOfWork.NativeSql.UpdateTransferToCourt(UpdatePaper.CompanyID, UpdatePaper.CompanyYear, UpdatePaper.RowNumber, UpdatePaper.OldVoucherNumber, UpdatePaper.OldCompanyTransactionKindNo,
                        UpdatePaper.OldTransactionKindNo, UpdatePaper.OldVHI, UpdatePaper.ChequeCase, UpdatePaper.Remark, UpdatePaper.InsUserID, UpdatePaper.InsDateTime,
                        UpdatePaper.ChequeNumber, UpdatePaper.sChequeDate, UpdatePaper.ChequeAmount, UpdatePaper.AccountNumberThird, UpdatePaper.CaseNumber);
                    _unitOfWork.Complete();

                }

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
        public JsonResult SaveReturnToChequeEndorsement(PaperFilterVM ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var SaveHeader = ObjToSave.Header;
                SaveHeader.InsDateTime = DateTime.Now;
                SaveHeader.InsUserID = userId;
                SaveHeader.CompanyTransactionKindNo = ObjToSave.CompanyTransactionKindToTransferNo;
                SaveHeader.CompanyYear = UserInfo.CurrYear;
                var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                SaveHeader.TransactionKindNo = _unitOfWork.CompanyTransactionKind.GetTransKindForCompanyTransactionKind(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo);
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
                SaveHeader.CompanyID = UserInfo.fCompanyId;
                SaveHeader.SaleID = 0;
                SaveHeader.Exported = 1;
                if (ObjToSave.CurrencyID == 0)
                {
                    ObjToSave.CurrencyID = 1;
                }

                SaveHeader.FCurrencyID = ObjToSave.CurrencyID;
                SaveHeader.VHI = SaveHeader.VHI;
                if (Resources.Resource.CurLang == "Arb")
                {
                    SaveHeader.Note = "معاد تحويله تجير";
                }
                else
                {
                    SaveHeader.Note = "Return Transfer To Endorsement";
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
                        TransNote = " رقم الشيك معاد تحويله لمجير : " + UpdatePaper.ChequeNumber + " - " + " تاريخ الحق : " + dDateAfterRemoveTime.ToString("dd/MM/yyyy") + " - " + " الحساب المدفوع منه : " +
                            _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, UpdatePaper.AccountNumberFourth);
                    }
                    else
                    {
                        TransNote = " Cheque Number Return Transfer To Endorsement : " + UpdatePaper.ChequeNumber + " - " + " Cheque Date : " + dDateAfterRemoveTime.ToString("dd/MM/yyyy") + " - " + " Paid Account : " +
                            _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, UpdatePaper.AccountNumberFourth);
                    }
                    UpdatePaper.RowNumber = UpdatePaper.RowNumber;
                    UpdatePaper.CompanyYear = UpdatePaper.CompanyYear;
                    UpdatePaper.CompanyID = UserInfo.fCompanyId;
                    UpdatePaper.OldVoucherNumber = UpdatePaper.VoucherNumber;
                    UpdatePaper.OldCompanyTransactionKindNo = UpdatePaper.CompanyTransactionKindNo;
                    UpdatePaper.OldTransactionKindNo = _unitOfWork.CompanyTransactionKind.GetTransKindForCompanyTransactionKind(UserInfo.fCompanyId, UpdatePaper.OldCompanyTransactionKindNo);
                    UpdatePaper.OldVHI = UpdatePaper.VHI;
                    UpdatePaper.AccountNumberFourth = UpdatePaper.AccountNumberFourth;
                    UpdatePaper.AccountNumberFifth = "";
                    UpdatePaper.CompanyTransactionKindNo = SaveHeader.CompanyTransactionKindNo;
                    UpdatePaper.TransactionKindNo = SaveHeader.TransactionKindNo;
                    UpdatePaper.VoucherDate = SaveHeader.VoucherDate;
                    UpdatePaper.VoucherNumber = SaveHeader.VoucherNumber;
                    UpdatePaper.VHI = SaveHeader.VHI;
                    UpdatePaper.ChequeCase = 5;
                    UpdatePaper.AccountNumberSecond = ObjToSave.ChequeEndorsementAccountNumber;
                    UpdatePaper.Remark = SaveHeader.Note;
                    UpdatePaper.InsUserID = SaveHeader.InsUserID;
                    UpdatePaper.InsDateTime = DateTime.Now;
                    Amount = Amount + UpdatePaper.ChequeAmount;
                    _unitOfWork.NativeSql.SaveTracingPaper(UpdatePaper.CompanyID, UpdatePaper.CompanyYear, UpdatePaper.RowNumber, UpdatePaper.OldVoucherNumber,
                                                           UpdatePaper.OldCompanyTransactionKindNo, UpdatePaper.OldTransactionKindNo, UpdatePaper.ChequeNumber, UpdatePaper.sChequeDate,
                                                           UpdatePaper.ChequeAmount, UpdatePaper.AccountNumberThird);
                    _unitOfWork.NativeSql.UpdatePaper(UpdatePaper.CompanyID, UpdatePaper.CompanyYear, UpdatePaper.RowNumber, UpdatePaper.OldVoucherNumber, UpdatePaper.OldCompanyTransactionKindNo,
                        UpdatePaper.OldTransactionKindNo, UpdatePaper.OldVHI, UpdatePaper.CompanyTransactionKindNo, UpdatePaper.TransactionKindNo, UpdatePaper.VoucherDate, UpdatePaper.VoucherNumber,
                        UpdatePaper.VHI, UpdatePaper.ChequeCase, UpdatePaper.AccountNumberSecond, UpdatePaper.AccountNumberFourth, UpdatePaper.Remark,
                        UpdatePaper.InsUserID, UpdatePaper.InsDateTime, UpdatePaper.AccountNumberFifth, UpdatePaper.ChequeNumber, UpdatePaper.sChequeDate,
                        UpdatePaper.ChequeAmount, UpdatePaper.AccountNumberThird);
                    _unitOfWork.Complete();

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
                    TransactionDebit.AccountNumber = ObjToSave.ChequeEndorsementAccountNumber;
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
                    TransactionCredit.AccountNumber = UpdatePaper.AccountNumberFourth;
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
        [Authorize(Roles = "CoOwner,ShowCourtFund")]
        public ActionResult CourtFund()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var FromDate = _unitOfWork.NativeSql.GetFromDateCourtFundCheque(UserInfo.fCompanyId);
            var ToDate = _unitOfWork.NativeSql.GetToDateCourtFundCheque(UserInfo.fCompanyId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var PaperFilter = new PaperFilterVM
            {
                FromDate = FromDate,
                ToDate = ToDate,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency
            };
            return View(PaperFilter);
        }
        [HttpPost]
        public JsonResult GetAllCourtFund(PaperFilterVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllPaper = _unitOfWork.NativeSql.GetAllCourtFund(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                if (AllPaper == null)
                {
                    return Json(new List<PaperFilterVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.ChequeNumber))
                {
                    AllPaper = AllPaper.Where(m => m.ChequeNumber == Obj.ChequeNumber).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.AccountNumberFourth))
                {
                    AllPaper = AllPaper.Where(m => m.IsBill == 1 ? m.BillCustomerNumber == Obj.AccountNumberFourth : m.AccountNumberFourth == Obj.AccountNumberFourth).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.CaseNumber))
                {
                    AllPaper = AllPaper.Where(m => m.CaseNumber == Obj.CaseNumber).ToList();
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
        public JsonResult SaveCompromise(PaperFilterVM ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var Note = "";
                if (Resources.Resource.CurLang == "Arb")
                {
                    Note = "تمت التسوية بالمحكمة";
                }
                else
                {
                    Note = "Court Compromise";
                }
                foreach (var UpdatePaper in ObjToSave.Paper)
                {
                    UpdatePaper.RowNumber = UpdatePaper.RowNumber;
                    UpdatePaper.CompanyYear = UpdatePaper.CompanyYear;
                    UpdatePaper.CompanyID = UserInfo.fCompanyId;
                    UpdatePaper.OldVoucherNumber = UpdatePaper.VoucherNumber;
                    UpdatePaper.OldCompanyTransactionKindNo = UpdatePaper.CompanyTransactionKindNo;
                    UpdatePaper.OldTransactionKindNo = _unitOfWork.CompanyTransactionKind.GetTransKindForCompanyTransactionKind(UserInfo.fCompanyId, UpdatePaper.OldCompanyTransactionKindNo);
                    UpdatePaper.OldVHI = UpdatePaper.VHI;
                    UpdatePaper.ChequeCase = 13;
                    UpdatePaper.Remark = Note;
                    UpdatePaper.InsUserID = userId;
                    UpdatePaper.InsDateTime = DateTime.Now;
                    _unitOfWork.NativeSql.SaveTracingPaper(UpdatePaper.CompanyID, UpdatePaper.CompanyYear, UpdatePaper.RowNumber, UpdatePaper.OldVoucherNumber,
                                                           UpdatePaper.OldCompanyTransactionKindNo, UpdatePaper.OldTransactionKindNo, UpdatePaper.ChequeNumber, UpdatePaper.sChequeDate,
                                                           UpdatePaper.ChequeAmount, UpdatePaper.AccountNumberThird);
                    _unitOfWork.NativeSql.UpdateCompromisePaper(UpdatePaper.CompanyID, UpdatePaper.CompanyYear, UpdatePaper.RowNumber, UpdatePaper.OldVoucherNumber, UpdatePaper.OldCompanyTransactionKindNo,
                        UpdatePaper.OldTransactionKindNo, UpdatePaper.OldVHI, UpdatePaper.ChequeCase, UpdatePaper.Remark, UpdatePaper.InsUserID, UpdatePaper.InsDateTime,
                        UpdatePaper.ChequeNumber, UpdatePaper.sChequeDate, UpdatePaper.ChequeAmount, UpdatePaper.AccountNumberThird);
                    _unitOfWork.Complete();

                }

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
        public JsonResult UpdateCaseNumber(PaperFilterVM ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                string UpdateCaseNumber = ObjToSave.UpdateCaseNumber;
                foreach (var UpdatePaper in ObjToSave.Paper)
                {
                    UpdatePaper.RowNumber = UpdatePaper.RowNumber;
                    UpdatePaper.CompanyYear = UpdatePaper.CompanyYear;
                    UpdatePaper.CompanyID = UserInfo.fCompanyId;
                    UpdatePaper.OldVoucherNumber = UpdatePaper.VoucherNumber;
                    UpdatePaper.OldCompanyTransactionKindNo = UpdatePaper.CompanyTransactionKindNo;
                    UpdatePaper.OldTransactionKindNo = _unitOfWork.CompanyTransactionKind.GetTransKindForCompanyTransactionKind(UserInfo.fCompanyId, UpdatePaper.OldCompanyTransactionKindNo);
                    UpdatePaper.OldVHI = UpdatePaper.VHI;
                    UpdatePaper.CaseNumber = UpdateCaseNumber;
                    UpdatePaper.InsUserID = userId;
                    UpdatePaper.InsDateTime = DateTime.Now;

                    _unitOfWork.NativeSql.UpdateCaseNumber(UpdatePaper.CompanyID, UpdatePaper.CompanyYear, UpdatePaper.RowNumber, UpdatePaper.OldVoucherNumber, UpdatePaper.OldCompanyTransactionKindNo,
                        UpdatePaper.OldTransactionKindNo, UpdatePaper.OldVHI, UpdatePaper.CaseNumber, UpdatePaper.InsUserID, UpdatePaper.InsDateTime,
                        UpdatePaper.ChequeNumber, UpdatePaper.sChequeDate, UpdatePaper.ChequeAmount, UpdatePaper.AccountNumberThird);
                    _unitOfWork.Complete();

                }

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
        [Authorize(Roles = "CoOwner,RepChequesReport")]
        public ActionResult ChequesReport()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var FromDate = _unitOfWork.NativeSql.GetFromDateAllCheque(UserInfo.fCompanyId);
            var ToDate = _unitOfWork.NativeSql.GetToDateAllCheque(UserInfo.fCompanyId);
            var SaleObj = _unitOfWork.Sale.GetAllSale(UserInfo.fCompanyId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var PaperFilter = new PaperFilterVM
            {
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                Sale = SaleObj,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency
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
                var AllPaper = _unitOfWork.NativeSql.GetAllCheques(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                List<PaperFilterVM> AllData = new List<PaperFilterVM>();
                List<PaperFilterVM> Ch1 = new List<PaperFilterVM>();
                List<PaperFilterVM> Ch2 = new List<PaperFilterVM>();
                List<PaperFilterVM> Ch3 = new List<PaperFilterVM>();
                List<PaperFilterVM> Ch4 = new List<PaperFilterVM>();
                List<PaperFilterVM> Ch5 = new List<PaperFilterVM>();
                List<PaperFilterVM> Ch6 = new List<PaperFilterVM>();
                List<PaperFilterVM> Ch7 = new List<PaperFilterVM>();
                List<PaperFilterVM> Ch8 = new List<PaperFilterVM>();
                List<PaperFilterVM> Ch9 = new List<PaperFilterVM>();
                List<PaperFilterVM> Ch10 = new List<PaperFilterVM>();
                List<PaperFilterVM> Ch11 = new List<PaperFilterVM>();
                List<PaperFilterVM> Ch12 = new List<PaperFilterVM>();
                List<PaperFilterVM> Ch13 = new List<PaperFilterVM>();
                if (AllPaper == null)
                {
                    return Json(new List<PaperFilterVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.ChequeNumber))
                {
                    AllPaper = AllPaper.Where(m => m.ChequeNumber == Obj.ChequeNumber).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.PaidInAccountNumber))
                {
                    AllPaper = AllPaper.Where(m => m.IsBill == 1 ? m.BillCustomerNumber == Obj.PaidInAccountNumber : m.PaidInAccountNumber == Obj.PaidInAccountNumber).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.BankOrEndorsementAccountNumber))
                {
                    AllPaper = AllPaper.Where(m => m.BankOrEndorsementAccountNumber == Obj.BankOrEndorsementAccountNumber).ToList();
                }
                if (Obj.SaleID != 0)
                {
                    AllPaper = AllPaper.Where(m => m.SaleID == Obj.SaleID).ToList();
                }
                if (Obj.ChequeFund)
                {
                    Ch1 = AllPaper.Where(m => m.ChequeCase == 1).ToList();
                }
                if (Obj.FundDrawnFromUnderCollection)
                {
                    Ch2 = AllPaper.Where(m => m.ChequeCase == 7).ToList();
                }
                if (Obj.ReturnedChequeFundCheque)
                {
                    Ch3 = AllPaper.Where(m => m.ChequeCase == 6 || m.ChequeCase == 10).ToList();
                }
                if (Obj.CourtFundCheque)
                {
                    Ch4 = AllPaper.Where(m => m.ChequeCase == 12).ToList();
                }
                if (Obj.UnderCollection)
                {
                    Ch5 = AllPaper.Where(m => m.ChequeCase == 2).ToList();
                }
                if (Obj.PaymentUnderCollection)
                {
                    Ch6 = AllPaper.Where(m => m.ChequeCase == 3).ToList();
                }
                if (Obj.ClearingDepositCheque)
                {
                    Ch7 = AllPaper.Where(m => m.ChequeCase == 4).ToList();
                }
                if (Obj.EndorsementCheque)
                {
                    Ch8 = AllPaper.Where(m => m.ChequeCase == 5).ToList();
                }
                if (Obj.PaymentEndorsement)
                {
                    Ch9 = AllPaper.Where(m => m.ChequeCase == 11).ToList();
                }
                if (Obj.ReturnDrawingChequeToCustomer)
                {
                    Ch10 = AllPaper.Where(m => m.ChequeCase == 9).ToList();
                }
                if (Obj.ReturnChequeToCustomer)
                {
                    Ch11 = AllPaper.Where(m => m.ChequeCase == 8).ToList();
                }
                if (Obj.CompromiseInCourt)
                {
                    Ch12 = AllPaper.Where(m => m.ChequeCase == 13).ToList();
                }
                if (Obj.ReturnFromChequeBox)
                {
                    Ch13 = AllPaper.Where(m => m.ChequeCase == 18).ToList();
                }

                if (Ch1.Count > 0)
                {
                    foreach(var c in Ch1)
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
                if (Ch9.Count > 0)
                {
                    foreach (var c in Ch9)
                    {
                        AllData.Add(c);
                    }
                }
                if (Ch10.Count > 0)
                {
                    foreach (var c in Ch10)
                    {
                        AllData.Add(c);
                    }
                }
                if (Ch11.Count > 0)
                {
                    foreach (var c in Ch11)
                    {
                        AllData.Add(c);
                    }
                }
                if (Ch12.Count > 0)
                {
                    foreach (var c in Ch12)
                    {
                        AllData.Add(c);
                    }
                }
                if (Ch13.Count > 0)
                {
                    foreach (var c in Ch13)
                    {
                        AllData.Add(c);
                    }
                }
                if (Obj.ChequeFund == false && Obj.FundDrawnFromUnderCollection == false && Obj.ReturnedChequeFundCheque == false && Obj.CourtFundCheque == false &&
                    Obj.UnderCollection == false && Obj.PaymentUnderCollection == false && Obj.ClearingDepositCheque == false && Obj.EndorsementCheque == false &&
                    Obj.PaymentEndorsement == false && Obj.ReturnDrawingChequeToCustomer == false && Obj.ReturnChequeToCustomer == false && Obj.CompromiseInCourt == false && Obj.ReturnFromChequeBox == false)
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
        [Authorize(Roles = "CoOwner,RepTrankingChequesReport")]
        public ActionResult TrankingChequesReport()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var FromDate = _unitOfWork.NativeSql.GetFromDateAllCheque(UserInfo.fCompanyId);
            var ToDate = _unitOfWork.NativeSql.GetToDateAllCheque(UserInfo.fCompanyId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var PaperFilter = new PaperFilterVM
            {
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency

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
                var AllPaper = _unitOfWork.NativeSql.GetTrankingChequesReport(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                if (AllPaper == null)
                {
                    return Json(new List<PaperFilterVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.ChequeNumber))
                {
                    AllPaper = AllPaper.Where(m => m.ChequeNumber == Obj.ChequeNumber).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.PaidInAccountNumber))
                {
                    AllPaper = AllPaper.Where(m => m.IsBill == 1 ? m.BillCustomerNumber == Obj.PaidInAccountNumber : m.PaidInAccountNumber == Obj.PaidInAccountNumber).ToList();
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
            var PaperFilterVM = new PaperFilterVM {
                ChequeNumber = id,
                OriginalVoucherNumber = id2,
                OriginalCompanyTransactionKindNo = id3,
                ChequeAmount = id4,
                RowNumber = id5,
                AccountNumberThird = id6,
                OriginalTransactionKindNo = id7,
                CompanyYear = id8,
                PaidInAccountNumber = id9,
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
                var ShowPaperDetails = _unitOfWork.NativeSql.GetChequeDetails(UserInfo.fCompanyId, id, id2, id3, id4, id5, id6, id7, id8, id9);
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
