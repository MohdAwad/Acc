﻿using Acc.Helpers;
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
    public class ReceiptVoucherCashMultiAccountController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public ReceiptVoucherCashMultiAccountController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        [Authorize(Roles = "CoOwner,ShowReceiptVoucherCashMultiAccount")]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetReceiptVoucherCashMultiAccountFromTransactionKind(UserInfo.fCompanyId);
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
            var ReceiptCashFilter = new ReceiptCashFilterVM
            {
                FromDate = FromDate,
                ToDate = FromDate,
                CompanyTransactionKind = CompanyTransactionKindObj,
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                CompanyYear = UserInfo.CurrYear
            };
            return View(ReceiptCashFilter);
        }
        [HttpPost]
        public JsonResult GetAllReceiptVoucherCashMultiAccount(ReceiptCashFilterVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllReceiptCashMultiAccount = _unitOfWork.NativeSql.GetAllReceiptCashMultiAccountFromHeader(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                if (AllReceiptCashMultiAccount == null)
                {
                    return Json(new List<ReceiptCashFilterVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.VoucherNumber))
                {
                    AllReceiptCashMultiAccount = AllReceiptCashMultiAccount.Where(m => m.VoucherNumber == Obj.VoucherNumber).ToList();
                }
                if (Obj.CurrencyID != 0)
                {
                    AllReceiptCashMultiAccount = AllReceiptCashMultiAccount.Where(m => m.CurrencyID == Obj.CurrencyID).ToList();
                }
                if (Obj.CompanyTransactionKindNo != 0)
                {
                    AllReceiptCashMultiAccount = AllReceiptCashMultiAccount.Where(m => m.CompanyTransactionKindNo == Obj.CompanyTransactionKindNo).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.FundAccountNumber))
                {
                    AllReceiptCashMultiAccount = AllReceiptCashMultiAccount.Where(m => m.FundAccountNumber == Obj.FundAccountNumber).ToList();
                }
                return Json(AllReceiptCashMultiAccount, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<ReceiptCashFilterVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [Authorize(Roles = "CoOwner,AddReceiptVoucherCashMultiAccount")]
        public ActionResult AddNew()
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var HeadrObj = new Header();
            HeadrObj.VoucherDate = DateTime.Now;
            var TransactionDebitObj = new Transaction();
            var TransactionCreditObj = new List<Transaction>();
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetReceiptVoucherCashMultiAccountFromTransactionKind(UserInfo.fCompanyId);
            var TransFixedVM = new TransactionFixedVM
            {
                Header = HeadrObj,
                TransactionDebit = TransactionDebitObj,
                TransactionCreditList = TransactionCreditObj,
                CompanyTransactionKind = CompanyTransactionKindObj,
                CompanyTransactionKindID = 1,
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                CurrencyID = 1,
                WorkWithCostCenter = Company.WorkWithCostCenter,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                CurrentYear = UserInfo.CurrYear
            };
            return View(TransFixedVM);
        }
        [ValidateInput(false)]
        public ActionResult GridViewTransActionPartial(string id, string id2, string id3, string id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            ViewBag.WorkWithCostCenter = Company.WorkWithCostCenter;
            if (!String.IsNullOrEmpty(id))
            {
                var TransObj = _unitOfWork.NativeSql.GetTransactionsDetailReceiptVoucherCashMultiCredit(id, int.Parse(id2), UserInfo.fCompanyId, int.Parse(id3), int.Parse(id4));
                return PartialView("GridViewTransActionPartial", TransObj);
            }
            else
            {
                var TransObj = new List<TransActionModelVM>();
                return PartialView("GridViewTransActionPartial", TransObj);
            }
        }
        public JsonResult SaveReceiptVoucherCashMultiAccount(TransactionFixedVM ObjToSave)
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

                SaveHeader.CompanyYear = UserInfo.CurrYear;
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
                var TransactionCreditList = ObjToSave.TransactionCreditList;
                var iRow = 1;
                foreach (var TransactionCreditSave in TransactionCreditList)
                {
                    try
                    {
                        iRow = iRow + 1;
                        TransactionCreditSave.CompanyID = UserInfo.fCompanyId;
                        TransactionCreditSave.CompanyYear = SaveHeader.CompanyYear;
                        TransactionCreditSave.CompanyTransactionKindNo = SaveHeader.CompanyTransactionKindNo;
                        TransactionCreditSave.TransactionKindNo = SaveHeader.TransactionKindNo;
                        TransactionCreditSave.VoucherDate = SaveHeader.VoucherDate;
                        TransactionCreditSave.VoucherNumber = SaveHeader.VoucherNumber;
                        TransactionCreditSave.InsUserID = SaveHeader.InsUserID;
                        TransactionCreditSave.VHI = SaveHeader.VHI;
                        TransactionCreditSave.InsDateTime = DateTime.Now;
                        TransactionCreditSave.AccountNumber = TransactionCreditSave.AccountNumber;
                        TransactionCreditSave.CostCenter = TransactionCreditSave.CostCenter;
                        TransactionCreditSave.Debit = 0;
                        TransactionCreditSave.Credit = TransactionCreditSave.Credit;
                        TransactionCreditSave.DebitForeign = 0;
                        if (SaveHeader.ConversionFactor == 1)
                        {
                            TransactionCreditSave.CreditForeign = 0;
                        }
                        else
                        {
                            TransactionCreditSave.CreditForeign = TransactionCreditSave.Credit / SaveHeader.ConversionFactor;
                        };
                        TransactionCreditSave.CreditDebitForeign = TransactionCreditSave.CreditForeign;
                        TransactionCreditSave.RowNumber = iRow;
                        TransactionCreditSave.Note = TransactionCreditSave.Note;
                        _unitOfWork.Transaction.Add(TransactionCreditSave);
                        _unitOfWork.Complete();
                    }
                    catch (Exception ex)
                    {
                        Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                        Msg.Code = 0;
                        return Json(Msg, JsonRequestBehavior.AllowGet);

                    }
                }
                SaveHeader.RowCount = iRow;
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
        [Authorize(Roles = "CoOwner,UpdateReceiptVoucherCashMultiAccount")]
        public ActionResult UpdateReceiptVoucherCashMultiAccount(string id, int id2, int id3 , int id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var HeaderObj = _unitOfWork.Header.GetHeaderDataById(id, UserInfo.fCompanyId, id2, id3 , id4);
            var CompanyTransactionKindObj = _unitOfWork.CompanyTransactionKind.GetCompanyTransactionKindByID(UserInfo.fCompanyId, HeaderObj.CompanyTransactionKindNo);
            var CompanyTransactionKindID = HeaderObj.CompanyTransactionKindNo;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var TransactionDebitObj = _unitOfWork.NativeSql.GetTransactionsDetailDebit(id, UserInfo.fCompanyId, id2, 1, id4);
            var TransactionCreditListObj = new List<Transaction>();
            int iRowNumber = HeaderObj.RowCount;
            var TransFixedVM = new TransactionFixedVM { };
            TransFixedVM.Header = HeaderObj;
            TransFixedVM.TransactionDebit = TransactionDebitObj;
            TransFixedVM.CompanyTransactionKindID = CompanyTransactionKindID;
            if (Resources.Resource.CurLang == "Arb")
            {
                TransFixedVM.CompanyTransactionKindName = CompanyTransactionKindObj.ArabicName;
            }
            else
            {
                TransFixedVM.CompanyTransactionKindName = CompanyTransactionKindObj.EnglishName;
            }
            TransFixedVM.TransactionCreditList = TransactionCreditListObj;
            TransFixedVM.Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId);
            TransFixedVM.CurrencyID = HeaderObj.FCurrencyID;
            TransFixedVM.CurrencyNewValue = Math.Round(HeaderObj.ConversionFactor, 3);
            TransFixedVM.WorkWithCostCenter = Company.WorkWithCostCenter;
            TransFixedVM.AccountDebitName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionDebitObj.AccountNumber);
            TransFixedVM.CostCenterDebitName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionDebitObj.CostCenter);
            TransFixedVM.Amount = Math.Round(HeaderObj.TotalDebit, 3);
            TransFixedVM.TotalAmount = Math.Round(HeaderObj.TotalDebit, 3);
            TransFixedVM.ForeignAmount = Math.Round(HeaderObj.TotalDebitForeign, 3);
            TransFixedVM.TotalForeign = Math.Round(HeaderObj.TotalDebitForeign, 3);
            TransFixedVM.TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency;
            TransFixedVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            return View(TransFixedVM);
        }
        [HttpPost]
        public JsonResult UpdateReceiptVoucherCashMultiAccount(TransactionFixedVM ObjToUpdate)
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

                UpdateHeader.CompanyYear = ObjToUpdate.Header.CompanyYear;
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

                var TransactionCreditList = ObjToUpdate.TransactionCreditList;
                var iRow = 1;
                _unitOfWork.NativeSql.DeleteTransActionCredit(UpdateHeader.CompanyID, UpdateHeader.VoucherNumber,
                    UpdateHeader.CompanyTransactionKindNo, UpdateHeader.TransactionKindNo, UpdateHeader.CompanyYear);
                foreach (var TransactionCreditSave in TransactionCreditList)
                {
                    try
                    {
                        iRow = iRow + 1;
                        TransactionCreditSave.CompanyID = UserInfo.fCompanyId;
                        TransactionCreditSave.CompanyYear = UpdateHeader.CompanyYear;
                        TransactionCreditSave.CompanyTransactionKindNo = UpdateHeader.CompanyTransactionKindNo;
                        TransactionCreditSave.TransactionKindNo = UpdateHeader.TransactionKindNo;
                        TransactionCreditSave.VoucherDate = UpdateHeader.VoucherDate;
                        TransactionCreditSave.VoucherNumber = UpdateHeader.VoucherNumber;
                        TransactionCreditSave.InsUserID = UpdateHeader.InsUserID;
                        TransactionCreditSave.VHI = UpdateHeader.VHI;
                        TransactionCreditSave.InsDateTime = DateTime.Now;
                        TransactionCreditSave.AccountNumber = TransactionCreditSave.AccountNumber;
                        TransactionCreditSave.CostCenter = TransactionCreditSave.CostCenter;
                        TransactionCreditSave.Debit = 0;
                        TransactionCreditSave.Credit = TransactionCreditSave.Credit;
                        TransactionCreditSave.DebitForeign = 0;
                        if (UpdateHeader.ConversionFactor == 1)
                        {
                            TransactionCreditSave.CreditForeign = 0;
                        }
                        else
                        {
                            TransactionCreditSave.CreditForeign = TransactionCreditSave.Credit / UpdateHeader.ConversionFactor;
                        };
                        TransactionCreditSave.CreditDebitForeign = TransactionCreditSave.CreditForeign;
                        TransactionCreditSave.RowNumber = iRow;
                        TransactionCreditSave.Note = TransactionCreditSave.Note;
                        _unitOfWork.Transaction.Add(TransactionCreditSave);
                        _unitOfWork.Complete();
                    }
                    catch (Exception ex)
                    {
                        Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                        Msg.Code = 0;
                        return Json(Msg, JsonRequestBehavior.AllowGet);

                    }
                }
                UpdateHeader.RowCount = iRow;

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
        [Authorize(Roles = "CoOwner,CopyReceiptVoucherCashMultiAccount")]
        public ActionResult CopyReceiptVoucherCashMultiAccount(string id, int id2, int id3 , int id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            var HeaderObj = _unitOfWork.Header.GetHeaderDataById(id, UserInfo.fCompanyId, id2, id3 , id4);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetReceiptVoucherCashMultiAccountFromTransactionKind(UserInfo.fCompanyId);
            var CompanyTransactionKindID = HeaderObj.CompanyTransactionKindNo;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var TransactionDebitObj = _unitOfWork.NativeSql.GetTransactionsDetailDebit(id, UserInfo.fCompanyId, id2, 1 , id4);
            var TransactionCreditListObj = new List<Transaction>();
            int iRowNumber = HeaderObj.RowCount;
            var TransFixedVM = new TransactionFixedVM { };
            TransFixedVM.Header = HeaderObj;
            TransFixedVM.TransactionDebit = TransactionDebitObj;
            TransFixedVM.CompanyTransactionKind = CompanyTransactionKindObj;
            TransFixedVM.CompanyTransactionKindID = CompanyTransactionKindID;
            TransFixedVM.TransactionCreditList = TransactionCreditListObj;
            TransFixedVM.Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId);
            TransFixedVM.CurrencyID = HeaderObj.FCurrencyID;
            TransFixedVM.CurrencyNewValue = Math.Round(HeaderObj.ConversionFactor, 3);
            TransFixedVM.WorkWithCostCenter = Company.WorkWithCostCenter;
            TransFixedVM.AccountDebitName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionDebitObj.AccountNumber);
            TransFixedVM.CostCenterDebitName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionDebitObj.CostCenter);
            TransFixedVM.Amount = Math.Round(HeaderObj.TotalDebit, 3);
            TransFixedVM.TotalAmount = Math.Round(HeaderObj.TotalDebit, 3);
            TransFixedVM.ForeignAmount = Math.Round(HeaderObj.TotalDebitForeign, 3);
            TransFixedVM.TotalForeign = Math.Round(HeaderObj.TotalDebitForeign, 3);
            TransFixedVM.TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency;
            TransFixedVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            return View(TransFixedVM);
        }
        [Authorize(Roles = "CoOwner,AttachReceiptVoucherCashMultiAccount")]
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
        public ActionResult DetailReceiptVoucherCashMultiAccount(string id, int id2, int id3 , int id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            var HeaderObj = _unitOfWork.Header.GetHeaderDataById(id, UserInfo.fCompanyId, id2, id3 , id4);
            var CompanyTransactionObj = _unitOfWork.CompanyTransactionKind.GetCompanyTransactionKindByID(UserInfo.fCompanyId, HeaderObj.CompanyTransactionKindNo);
            var CurrencyObj = _unitOfWork.Currency.GetCurrencyByID(UserInfo.fCompanyId, HeaderObj.FCurrencyID);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var TransactionDebitObj = _unitOfWork.NativeSql.GetTransactionsDetailDebit(id, UserInfo.fCompanyId, id2, 1 , id4);
            int iRowNumber = HeaderObj.RowCount;
            var TransFixedVM = new TransactionFixedVM { };
            TransFixedVM.Header = HeaderObj;
            TransFixedVM.TransactionDebit = TransactionDebitObj;
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
            TransFixedVM.Amount = Math.Round(HeaderObj.TotalDebit, 3);
            TransFixedVM.TotalAmount = Math.Round(HeaderObj.TotalDebit, 3);
            TransFixedVM.ForeignAmount = Math.Round(HeaderObj.TotalDebitForeign, 3);
            TransFixedVM.TotalForeign = Math.Round(HeaderObj.TotalDebitForeign, 3);
            TransFixedVM.TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency;
            TransFixedVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            return View(TransFixedVM);
        }
        [HttpGet]
        public JsonResult GetTransactionCredit(string id, string id2, string id3 , int id4)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var AllTransactionsDetailCredit = _unitOfWork.NativeSql.GetAllTransactionsDetailCredit(id, int.Parse(id2), UserInfo.fCompanyId, int.Parse(id3) , id4);

            if (AllTransactionsDetailCredit == null)
            {
                return Json(new TransactionFixedVM(), JsonRequestBehavior.AllowGet);
            }
            return Json(AllTransactionsDetailCredit, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "CoOwner,DeleteReceiptVoucherCashMultiAccount")]
        public ActionResult DeleteReceiptVoucherCashMultiAccount(string id, int id2, int id3 , int id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            var HeaderObj = _unitOfWork.Header.GetHeaderDataById(id, UserInfo.fCompanyId, id2, id3, id4);
            var CompanyTransactionObj = _unitOfWork.CompanyTransactionKind.GetCompanyTransactionKindByID(UserInfo.fCompanyId, HeaderObj.CompanyTransactionKindNo);
            var CurrencyObj = _unitOfWork.Currency.GetCurrencyByID(UserInfo.fCompanyId, HeaderObj.FCurrencyID);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var TransactionDebitObj = _unitOfWork.NativeSql.GetTransactionsDetailDebit(id, UserInfo.fCompanyId, id2, 1 , id4);
            int iRowNumber = HeaderObj.RowCount;
            var TransFixedVM = new TransactionFixedVM { };
            TransFixedVM.Header = HeaderObj;
            TransFixedVM.TransactionDebit = TransactionDebitObj;
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
            TransFixedVM.Amount = Math.Round(HeaderObj.TotalDebit, 3);
            TransFixedVM.TotalAmount = Math.Round(HeaderObj.TotalDebit, 3);
            TransFixedVM.ForeignAmount = Math.Round(HeaderObj.TotalDebitForeign, 3);
            TransFixedVM.TotalForeign = Math.Round(HeaderObj.TotalDebitForeign, 3);
            TransFixedVM.TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency;
            TransFixedVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            return View(TransFixedVM);
        }
        [Authorize(Roles = "CoOwner,ExportReceiptVoucherCashMultiAccount")]
        public ActionResult ExportReceiptCashMultiAccount()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetReceiptVoucherCashMultiAccountFromTransactionKind(UserInfo.fCompanyId);
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
            var ReceiptCashFilter = new ReceiptCashFilterVM
            {
                FromDate = FromDate,
                ToDate = ToDate,
                CompanyTransactionKind = CompanyTransactionKindObj,
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                CompanyYear = UserInfo.CurrYear
            };
            return View(ReceiptCashFilter);
        }
        [HttpPost]
        public JsonResult GetAllReceiptVoucherCashMultiAccountUnExport(ReceiptCashFilterVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllReceiptCash = _unitOfWork.NativeSql.GetAllReceiptCashMultiAccountFromHeaderUnExport(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                if (AllReceiptCash == null)
                {
                    return Json(new List<ReceiptCashFilterVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.VoucherNumber))
                {
                    AllReceiptCash = AllReceiptCash.Where(m => m.VoucherNumber == Obj.VoucherNumber).ToList();
                }
                if (Obj.CurrencyID != 0)
                {
                    AllReceiptCash = AllReceiptCash.Where(m => m.CurrencyID == Obj.CurrencyID).ToList();
                }
                if (Obj.CompanyTransactionKindNo != 0)
                {
                    AllReceiptCash = AllReceiptCash.Where(m => m.CompanyTransactionKindNo == Obj.CompanyTransactionKindNo).ToList();
                }
                int iRow = 0;
                foreach (var iRowCount in AllReceiptCash)
                {
                    iRowCount.iRowTable = iRow;
                    iRow = iRow + 1;
                }
                return Json(AllReceiptCash, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<ReceiptCashFilterVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [Authorize(Roles = "CoOwner,UnExportReceiptVoucherCashMultiAccount")]
        public ActionResult UnExportReceiptCashMultiAccount()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetReceiptVoucherCashMultiAccountFromTransactionKind(UserInfo.fCompanyId);
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
            var ReceiptCashFilter = new ReceiptCashFilterVM
            {
                FromDate = FromDate,
                ToDate = ToDate,
                CompanyTransactionKind = CompanyTransactionKindObj,
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                CompanyYear = UserInfo.CurrYear
            };
            return View(ReceiptCashFilter);
        }
        [HttpPost]
        public JsonResult GetAllReceiptVoucherCashMultiAccountExport(ReceiptCashFilterVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllReceiptCash = _unitOfWork.NativeSql.GetAllReceiptCashMultiAccountFromHeaderExport(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                if (AllReceiptCash == null)
                {
                    return Json(new List<ReceiptCashFilterVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.VoucherNumber))
                {
                    AllReceiptCash = AllReceiptCash.Where(m => m.VoucherNumber == Obj.VoucherNumber).ToList();
                }
                if (Obj.CurrencyID != 0)
                {
                    AllReceiptCash = AllReceiptCash.Where(m => m.CurrencyID == Obj.CurrencyID).ToList();
                }
                if (Obj.CompanyTransactionKindNo != 0)
                {
                    AllReceiptCash = AllReceiptCash.Where(m => m.CompanyTransactionKindNo == Obj.CompanyTransactionKindNo).ToList();
                }
                int iRow = 0;
                foreach (var iRowCount in AllReceiptCash)
                {
                    iRowCount.iRowTable = iRow;
                    iRow = iRow + 1;
                }
                return Json(AllReceiptCash, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<ReceiptCashFilterVM>(), JsonRequestBehavior.AllowGet);
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
    }
}