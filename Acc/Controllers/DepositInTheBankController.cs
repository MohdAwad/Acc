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
    public class DepositInTheBankController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public DepositInTheBankController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }

        [Authorize(Roles = "CoOwner, AddDepositInTheBank")]
        public ActionResult Create()
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var HeadrObj = new Header();
            HeadrObj.VoucherDate = DateTime.Now;
            var TransObj = new List<Transaction>();
            var CurrYear = UserInfo.CurrYear;
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetDepositInTheBankFromTransactionKind(UserInfo.fCompanyId);
            var TransactionKindNo = (int)EnumTransKind.DepositInBank;
            var CompanyTransactionKindID = 0;
            HeadrObj.VoucherNumber = _unitOfWork.Header.GetMaxVHByKind(UserInfo.fCompanyId, CompanyTransactionKindID, TransactionKindNo , CurrYear).ToString();
            HeadrObj.VHI = int.Parse(HeadrObj.VoucherNumber);
            var TransactionDebitObj = new Transaction();
            var TransVM = new TransActionBankVM
            {
                Header = HeadrObj,
                Transaction = TransObj,
                TransactionDebit = TransactionDebitObj,
                CompanyTransactionKind = CompanyTransactionKindObj,
                CompanyTransactionKindID = CompanyTransactionKindID,
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                CurrencyID = 1,
                WorkWithCostCenter = Company.WorkWithCostCenter,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency,
                CurrentYear = UserInfo.CurrYear

            };
            return View(TransVM);
        }
        [Authorize(Roles = "CoOwner,UpdateDepositInTheBank")]
        public ActionResult Update(string id,int id2,int id3 , int id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);

            var HeaderObj = _unitOfWork.Header.GetHeaderDataByTransId(id, UserInfo.fCompanyId, id2, id3, id4);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetDepositInTheBankFromTransactionKind(UserInfo.fCompanyId);
            var CompanyTransactionKindID = HeaderObj.CompanyTransactionKindNo;
            var TransactionDebitObj = _unitOfWork.NativeSql.GetTransactionsDetailDebitByTransKind(id, UserInfo.fCompanyId, id2, 1, id3, id4);
            var TransObj = new List<Transaction>();
            var TransVM = new TransActionBankVM
            {
                Header = HeaderObj,
                TransactionDebit = TransactionDebitObj,
                Transaction = TransObj,
                CompanyTransactionKind = CompanyTransactionKindObj,
                CompanyTransactionKindID = CompanyTransactionKindID,
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                CurrencyID = HeaderObj.FCurrencyID,
                CurrencyNewValue = Math.Round(HeaderObj.ConversionFactor, 3),
                BankName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionDebitObj.AccountNumber),
                BankCostCenterName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionDebitObj.CostCenter),
                Amount = Math.Round(TransactionDebitObj.Debit, 3),
                ForeignAmount = Math.Round(TransactionDebitObj.DebitForeign, 3),
                WorkWithCostCenter = Company.WorkWithCostCenter,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency

            };

            return View(TransVM);
        }
        [Authorize(Roles = "CoOwner,CopyDepositInTheBank")]
        public ActionResult Copy(string id, int id2, int id3 , int id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var HeaderObj = _unitOfWork.Header.GetHeaderDataByTransId(id, UserInfo.fCompanyId, id2, id3 , id4);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetDepositInTheBankFromTransactionKind(UserInfo.fCompanyId);
            var CompanyTransactionKindID = HeaderObj.CompanyTransactionKindNo;
            var TransactionDebitObj = _unitOfWork.NativeSql.GetTransactionsDetailDebitByTransKind(id, UserInfo.fCompanyId, id2, 1, id3, id4);
            var TransObj = new List<Transaction>();
            var TransVM = new TransActionBankVM
            {
                Header = HeaderObj,
                TransactionDebit = TransactionDebitObj,
                Transaction = TransObj,
                CompanyTransactionKind = CompanyTransactionKindObj,
                CompanyTransactionKindID = CompanyTransactionKindID,
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                CurrencyID = HeaderObj.FCurrencyID,
                CurrencyNewValue = Math.Round(HeaderObj.ConversionFactor, 3),
                BankName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionDebitObj.AccountNumber),
                BankCostCenterName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionDebitObj.CostCenter),
                Amount = Math.Round(TransactionDebitObj.Debit, 3),
                ForeignAmount = Math.Round(TransactionDebitObj.DebitForeign, 3),
                WorkWithCostCenter = Company.WorkWithCostCenter,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency

            };

            return View(TransVM);
        }
        [Authorize(Roles = "CoOwner,ShowDepositInTheBank")]
        public ActionResult index()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            int year = DateTime.Now.Year;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetDepositInTheBankFromTransactionKind(UserInfo.fCompanyId);
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
            var DepositInTheBankFilter = new DepositInTheBankFilter
            {
                FromDate = FromDate,
                ToDate = ToDate,
                CompanyTransactionKind = CompanyTransactionKindObj,
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                WorkWithCostCenter = Company.WorkWithCostCenter,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency
            };
            return View(DepositInTheBankFilter);
        }
        [HttpGet]
        public ActionResult Detail(string id, int id2, int id3 , int id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var HeaderObj = _unitOfWork.Header.GetHeaderDataByTransId(id, UserInfo.fCompanyId, id2, id3, id4);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetDepositInTheBankFromTransactionKind(UserInfo.fCompanyId);
            var CompanyTransactionKindID = HeaderObj.CompanyTransactionKindNo;
            var CurrencyObj = _unitOfWork.Currency.GetCurrencyByID(UserInfo.fCompanyId, HeaderObj.FCurrencyID);
            var TransactionDebitObj = _unitOfWork.NativeSql.GetTransactionsDetailDebitByTransKind(id, UserInfo.fCompanyId, id2, 1, id3, id4);
            var TransVM = new TransActionBankVM { };
            TransVM.Header = HeaderObj;
            TransVM.TransactionDebit = TransactionDebitObj;
            TransVM.CompanyTransactionKind = CompanyTransactionKindObj;
            TransVM.CompanyTransactionKindID = CompanyTransactionKindID;
            TransVM.Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId);
            TransVM.CurrencyID = HeaderObj.FCurrencyID;
            TransVM.CurrencyNewValue = Math.Round(HeaderObj.ConversionFactor, 3);
            TransVM.BankName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionDebitObj.AccountNumber);
            TransVM.BankCostCenterName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionDebitObj.CostCenter);
            TransVM.Amount = Math.Round(TransactionDebitObj.Debit, 3);
            TransVM.ForeignAmount = Math.Round(TransactionDebitObj.DebitForeign, 3);
            TransVM.WorkWithCostCenter = Company.WorkWithCostCenter;
            TransVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            TransVM.TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency;
            if (Resources.Resource.CurLang == "Arb")
            {
                TransVM.CurrencyName = CurrencyObj.ArabicName;
            }
            else
            {
                TransVM.CurrencyName = CurrencyObj.EnglishName;
            }
            return View(TransVM);
        }
        [HttpGet]
        [Authorize(Roles = "CoOwner, DeleteDepositInTheBank")]
        public ActionResult Delete(string id, int id2, int id3 , int id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var HeaderObj = _unitOfWork.Header.GetHeaderDataByTransId(id, UserInfo.fCompanyId, id2, id3 , id4);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetDepositInTheBankFromTransactionKind(UserInfo.fCompanyId);
            var CompanyTransactionKindID = HeaderObj.CompanyTransactionKindNo;
            var CurrencyObj = _unitOfWork.Currency.GetCurrencyByID(UserInfo.fCompanyId, HeaderObj.FCurrencyID);
            var TransactionDebitObj = _unitOfWork.NativeSql.GetTransactionsDetailDebitByTransKind(id, UserInfo.fCompanyId, id2, 1, id3, id4);
            var TransVM = new TransActionBankVM { };
            TransVM.Header = HeaderObj;
            TransVM.TransactionDebit = TransactionDebitObj;
            TransVM.CompanyTransactionKind = CompanyTransactionKindObj;
            TransVM.CompanyTransactionKindID = CompanyTransactionKindID;
            TransVM.Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId);
            TransVM.CurrencyID = HeaderObj.FCurrencyID;
            TransVM.CurrencyNewValue = Math.Round(HeaderObj.ConversionFactor, 3);
            TransVM.BankName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionDebitObj.AccountNumber);
            TransVM.BankCostCenterName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionDebitObj.CostCenter);
            TransVM.Amount = Math.Round(TransactionDebitObj.Debit, 3);
            TransVM.ForeignAmount = Math.Round(TransactionDebitObj.DebitForeign, 3);
            TransVM.WorkWithCostCenter = Company.WorkWithCostCenter;
            TransVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            TransVM.TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency;
            if (Resources.Resource.CurLang == "Arb")
            {
                TransVM.CurrencyName = CurrencyObj.ArabicName;
            }
            else
            {
                TransVM.CurrencyName = CurrencyObj.EnglishName;
            }
            return View(TransVM);
        }
        [HttpPost]
        public JsonResult DeleteTransActionTrans(TransActionDeleteVM ObjSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                ObjSave.CompanyID = UserInfo.fCompanyId;
                _unitOfWork.NativeSql.DeleteTransActionTrans(UserInfo.fCompanyId, ObjSave.VoucherNumber,
                 ObjSave.CompanyTransactionKindNo, ObjSave.TransactionKindNo, ObjSave.CompanyYear);
                _unitOfWork.Header.Delete(ObjSave);
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
        [HttpPost]
        public JsonResult GetDepositInTheBank(DepositInTheBankFilter Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllDepositInTheBank = _unitOfWork.NativeSql.GetAllDepositInTheBankFromHeader(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                if (AllDepositInTheBank == null)
                {
                    return Json(new List<DepositInTheBankFilter>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.VoucherNumber))
                {
                    AllDepositInTheBank = AllDepositInTheBank.Where(m => m.VoucherNumber == Obj.VoucherNumber).ToList();
                }
                if (Obj.CompanyTransactionKindNo != 0)
                {
                    AllDepositInTheBank = AllDepositInTheBank.Where(m => m.CompanyTransactionKindNo == Obj.CompanyTransactionKindNo).ToList();
                }
                if (Obj.CurrencyID != 0)
                {
                    AllDepositInTheBank = AllDepositInTheBank.Where(m => m.CurrencyID == Obj.CurrencyID).ToList();
                }
                return Json(AllDepositInTheBank, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<DepositInTheBankFilter>(), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult GetDepositInTheBankTrans(string id, string id2, string id3 , int id4)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var AllDepositInTheBankTrans = _unitOfWork.NativeSql.GetTransactionsDetailDepositInTheBankCredit(id, int.Parse(id2), UserInfo.fCompanyId, int.Parse(id3), id4);

            if (AllDepositInTheBankTrans == null)
            {
                return Json(new TransactionGridVM(), JsonRequestBehavior.AllowGet);
            }
            return Json(AllDepositInTheBankTrans, JsonRequestBehavior.AllowGet);
        }
        [ValidateInput(false)]
        public ActionResult GridViewTransActionPartial(string id,string id2,string id3)
        {
          
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var CompanyYear = UserInfo.CurrYear;
            ViewBag.WorkWithCostCenter = Company.WorkWithCostCenter;

            if (!String.IsNullOrEmpty(id))
            {
                var TransObj = _unitOfWork.NativeSql.GetTransactionsDetailDepositInTheBankCredit(id,int.Parse(id2), UserInfo.fCompanyId, int.Parse(id3) , CompanyYear);
                return PartialView("GridViewTransActionPartial", TransObj);
            }
            else
            {
                var TransObj = new List<TransActionModelVM>();
                return PartialView("GridViewTransActionPartial", TransObj);
            }

             
        }
        [HttpPost]
        public JsonResult SaveNewTransAction(TransActionBankVM ObjSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);

                var TranAction = ObjSave.Transaction;
                var TransactionDebit = ObjSave.TransactionDebit;
                var HeaderTOSave = ObjSave.Header;
                HeaderTOSave.CompanyTransactionKindNo = ObjSave.CompanyTransactionKindID;
                HeaderTOSave.TransactionKindNo = _unitOfWork.CompanyTransactionKind.GetTransKindForCompanyTransactionKind(UserInfo.fCompanyId, ObjSave.CompanyTransactionKindID);
                HeaderTOSave.InsDateTime = DateTime.Now;
                HeaderTOSave.InsUserID = userId;
                HeaderTOSave.CompanyID = UserInfo.fCompanyId;
                HeaderTOSave.TotalCredit = ObjSave.Amount; 
                HeaderTOSave.TotalDebit = ObjSave.Amount;
                double TotalDebitForeign = 0;
                if (HeaderTOSave.ConversionFactor == 0)
                {
                    HeaderTOSave.ConversionFactor = 1;
                };
                if (HeaderTOSave.ConversionFactor == 1)
                {
                    TotalDebitForeign = 0;
                }
                else
                {
                    TotalDebitForeign = HeaderTOSave.TotalDebit / HeaderTOSave.ConversionFactor;
                };
                HeaderTOSave.TotalDebitForeign = TotalDebitForeign;
                HeaderTOSave.TotalCreditForeign = TotalDebitForeign;
                HeaderTOSave.CompanyYear = UserInfo.CurrYear;
                var ObjComapnyTransactionKind = _unitOfWork.CompanyTransactionKind.GetCompanyTransactionKindByID(UserInfo.fCompanyId, HeaderTOSave.CompanyTransactionKindNo);
                if (ObjComapnyTransactionKind.AutoSerial)
                {
                    HeaderTOSave.VoucherNumber = _unitOfWork.Header.GetMaxVHByKind(UserInfo.fCompanyId, HeaderTOSave.CompanyTransactionKindNo, HeaderTOSave.TransactionKindNo, HeaderTOSave.CompanyYear).ToString();
                    HeaderTOSave.VHI = int.Parse(HeaderTOSave.VoucherNumber);
                }
                else
                {
                    string SerialNumber = "";
                    int Serial = ObjComapnyTransactionKind.Serial;
                    string Symbol = ObjComapnyTransactionKind.Symbol;
                    DateTime dDate = HeaderTOSave.VoucherDate;
                    string YearNo = dDate.ToString("yy");
                    string sMonth = dDate.ToString("MM");
                    int MonthNo = Int16.Parse(sMonth);
                    int LengthLastSerial = _unitOfWork.CompanyTransactionKindMonthlySerial.GetMaxSerial(UserInfo.fCompanyId, HeaderTOSave.CompanyTransactionKindNo, MonthNo).ToString().Length;
                    Serial = Serial - LengthLastSerial;
                    for (int i = 0; i <= Serial; i++)
                    {
                        if (i < Serial)
                        {
                            SerialNumber = SerialNumber + "0";
                        }
                        else if (i == Serial)
                        {
                            SerialNumber = SerialNumber + _unitOfWork.CompanyTransactionKindMonthlySerial.GetMaxSerial(UserInfo.fCompanyId, HeaderTOSave.CompanyTransactionKindNo, MonthNo).ToString();
                        }
                    }
                    HeaderTOSave.VoucherNumber = Symbol + YearNo + sMonth + SerialNumber;
                    HeaderTOSave.VHI = _unitOfWork.CompanyTransactionKindMonthlySerial.GetMaxSerial(UserInfo.fCompanyId, HeaderTOSave.CompanyTransactionKindNo, MonthNo);
                    _unitOfWork.CompanyTransactionKindMonthlySerial.Update(UserInfo.fCompanyId, HeaderTOSave.CompanyTransactionKindNo, MonthNo);
                }
                if (Company.DirectExportTheTransactions)
                {
                    HeaderTOSave.Exported = 1;
                }
                else
                {
                    HeaderTOSave.Exported = 0;
                }
                TransactionDebit.CompanyID = UserInfo.fCompanyId;
                TransactionDebit.CompanyYear = HeaderTOSave.CompanyYear;
                TransactionDebit.CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo;
                TransactionDebit.TransactionKindNo = HeaderTOSave.TransactionKindNo;
                TransactionDebit.VoucherDate = HeaderTOSave.VoucherDate;
                TransactionDebit.VoucherNumber = HeaderTOSave.VoucherNumber;
                TransactionDebit.InsUserID = HeaderTOSave.InsUserID;
                TransactionDebit.VHI = HeaderTOSave.VHI;
                TransactionDebit.InsDateTime = DateTime.Now;
                TransactionDebit.AccountNumber = TransactionDebit.AccountNumber;
                TransactionDebit.CostCenter = TransactionDebit.CostCenter;
                TransactionDebit.Debit = ObjSave.Amount;
                TransactionDebit.Credit = 0;
                if (HeaderTOSave.ConversionFactor == 1)
                {
                    TransactionDebit.DebitForeign = 0;
                }
                else
                {
                    TransactionDebit.DebitForeign = TransactionDebit.Debit / HeaderTOSave.ConversionFactor;
                };
                TransactionDebit.CreditForeign = 0;
                TransactionDebit.CreditDebitForeign = TransactionDebit.DebitForeign;
                TransactionDebit.Note = HeaderTOSave.Note;
                TransactionDebit.RowNumber = 1;
                _unitOfWork.Transaction.Add(TransactionDebit);
                _unitOfWork.Complete();
                var iRow = 1;
                foreach (var Trans in TranAction)
                {
                    if (Company.WorkWithCostCenter)
                    {
                        if (!String.IsNullOrWhiteSpace(Trans.AccountNumber) && !String.IsNullOrWhiteSpace(Trans.CostCenter))
                        {
                            try
                            {
                                iRow = iRow + 1;
                                Trans.CompanyID = UserInfo.fCompanyId;
                                Trans.CompanyYear = HeaderTOSave.CompanyYear;
                                Trans.CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo;
                                Trans.TransactionKindNo = HeaderTOSave.TransactionKindNo;
                                Trans.VoucherDate = HeaderTOSave.VoucherDate;
                                Trans.VoucherNumber = HeaderTOSave.VoucherNumber;
                                Trans.InsUserID = HeaderTOSave.InsUserID;
                                Trans.VHI = HeaderTOSave.VHI;
                                Trans.InsDateTime = DateTime.Now;
                                Trans.RowNumber = iRow;
                                Trans.Debit = 0;
                                Trans.Credit = Trans.Credit;
                                Trans.AccountNumber = Trans.AccountNumber;
                                Trans.CostCenter = Trans.CostCenter;
                                if (HeaderTOSave.ConversionFactor == 1)
                                {
                                    Trans.CreditForeign = 0;
                                }
                                else
                                {
                                    Trans.CreditForeign = Trans.Credit / HeaderTOSave.ConversionFactor;
                                };
                                Trans.DebitForeign = 0;
                                Trans.CreditDebitForeign = Trans.CreditForeign;
                                Trans.CreditForeign = Trans.CreditDebitForeign;
                                _unitOfWork.Transaction.Add(Trans);
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
                        if (!String.IsNullOrWhiteSpace(Trans.AccountNumber))
                        {
                            try
                            {
                                iRow = iRow + 1;
                                Trans.CompanyID = UserInfo.fCompanyId;
                                Trans.CompanyYear = HeaderTOSave.CompanyYear;
                                Trans.CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo;
                                Trans.TransactionKindNo = HeaderTOSave.TransactionKindNo;
                                Trans.VoucherDate = HeaderTOSave.VoucherDate;
                                Trans.VoucherNumber = HeaderTOSave.VoucherNumber;
                                Trans.InsUserID = HeaderTOSave.InsUserID;
                                Trans.VHI = HeaderTOSave.VHI;
                                Trans.InsDateTime = DateTime.Now;
                                Trans.RowNumber = iRow;
                                Trans.Debit = 0;
                                Trans.Credit = Trans.Credit;
                                Trans.AccountNumber = Trans.AccountNumber;
                                Trans.CostCenter = Trans.CostCenter;
                                if (HeaderTOSave.ConversionFactor == 1)
                                {
                                    Trans.CreditForeign = 0;
                                }
                                else
                                {
                                    Trans.CreditForeign = Trans.Credit / HeaderTOSave.ConversionFactor;
                                };
                                Trans.DebitForeign = 0;
                                Trans.CreditDebitForeign = Trans.CreditForeign;
                                Trans.CreditForeign = Trans.CreditDebitForeign;
                                _unitOfWork.Transaction.Add(Trans);
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
                HeaderTOSave.RowCount = iRow;
                _unitOfWork.Header.Add(HeaderTOSave);
                _unitOfWork.Complete();
                var OldCurrency = _unitOfWork.NativeSql.GetCurrencyValueByID(UserInfo.fCompanyId, ObjSave.CurrencyID);
                if (OldCurrency == null)
                {
                    var NewCurr = new CurrencyValue
                    {
                        CurrencyID = ObjSave.CurrencyID,
                        CompanyID = UserInfo.fCompanyId,
                        ConversionFactor = ObjSave.CurrencyNewValue,
                        InsDateTime = DateTime.Now,
                        InsUserID = userId
                    };
                    _unitOfWork.CurrencyValue.Add(NewCurr);
                    _unitOfWork.Complete();

                }
                else
                if (OldCurrency.ConversionFactor != ObjSave.CurrencyNewValue)
                {
                    var NewCurr = new CurrencyValue
                    {
                        CurrencyID = ObjSave.CurrencyID,
                        CompanyID = UserInfo.fCompanyId,
                        ConversionFactor = ObjSave.CurrencyNewValue,
                        InsDateTime = DateTime.Now,
                        InsUserID = userId
                    };
                    _unitOfWork.CurrencyValue.Add(NewCurr);
                    _unitOfWork.Complete();
                }
                Msg.LastID = _unitOfWork.Header.GetMaxVHByKind(UserInfo.fCompanyId, HeaderTOSave.CompanyTransactionKindNo, HeaderTOSave.TransactionKindNo, HeaderTOSave.CompanyYear).ToString(); ;
                Msg.Code = 1;

                Msg.Msg = Resources.Resource.AddedSuccessfully;
                Msg.Year = HeaderTOSave.CompanyYear;
                Msg.FCompanyId = HeaderTOSave.CompanyID;
                Msg.VoucherNumber = HeaderTOSave.VoucherNumber.ToString();
                Msg.TransactionKindNo = HeaderTOSave.TransactionKindNo.ToString();
                Msg.CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo.ToString();

                return Json(Msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                Msg.Code = 0;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }

        }
        [Authorize(Roles = "CoOwner, AttachDepositInTheBank")]
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
        [HttpPost]
        public JsonResult UpdateTransActionTrans(TransActionBankVM ObjUpdate)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                var TranAction = ObjUpdate.Transaction;
                var TransactionDebit = ObjUpdate.TransactionDebit;
                var HeaderTOUpdate = ObjUpdate.Header;
                HeaderTOUpdate.CompanyTransactionKindNo = ObjUpdate.CompanyTransactionKindID;
                HeaderTOUpdate.TransactionKindNo = _unitOfWork.CompanyTransactionKind.GetTransKindForCompanyTransactionKind(UserInfo.fCompanyId, ObjUpdate.CompanyTransactionKindID);
                HeaderTOUpdate.InsDateTime = DateTime.Now;
                HeaderTOUpdate.InsUserID = userId;
                HeaderTOUpdate.CompanyID = UserInfo.fCompanyId;
                HeaderTOUpdate.TotalCredit = ObjUpdate.Amount;
                HeaderTOUpdate.TotalDebit = ObjUpdate.Amount;
                double TotalDebitForeign = 0;
                if (HeaderTOUpdate.ConversionFactor == 0)
                {
                    HeaderTOUpdate.ConversionFactor = 1;
                };
                if (HeaderTOUpdate.ConversionFactor == 1)
                {
                    TotalDebitForeign = 0;
                }
                else
                {
                    TotalDebitForeign = HeaderTOUpdate.TotalDebit / HeaderTOUpdate.ConversionFactor;
                };
                HeaderTOUpdate.TotalDebitForeign = TotalDebitForeign;
                HeaderTOUpdate.TotalCreditForeign = TotalDebitForeign;
                HeaderTOUpdate.CompanyYear = ObjUpdate.CurrentYear;
                HeaderTOUpdate.VoucherNumber = HeaderTOUpdate.VoucherNumber;
                HeaderTOUpdate.VHI = HeaderTOUpdate.VHI;

                TransactionDebit.CompanyID = UserInfo.fCompanyId;
                TransactionDebit.CompanyYear = HeaderTOUpdate.CompanyYear;
                TransactionDebit.CompanyTransactionKindNo = HeaderTOUpdate.CompanyTransactionKindNo;
                TransactionDebit.TransactionKindNo = HeaderTOUpdate.TransactionKindNo;
                TransactionDebit.VoucherDate = HeaderTOUpdate.VoucherDate;
                TransactionDebit.VoucherNumber = HeaderTOUpdate.VoucherNumber;
                TransactionDebit.InsUserID = HeaderTOUpdate.InsUserID;
                TransactionDebit.VHI = HeaderTOUpdate.VHI;
                TransactionDebit.InsDateTime = DateTime.Now;
                TransactionDebit.AccountNumber = TransactionDebit.AccountNumber;
                TransactionDebit.CostCenter = TransactionDebit.CostCenter;
                TransactionDebit.Debit = ObjUpdate.Amount;
                TransactionDebit.Credit = 0;
                if (HeaderTOUpdate.ConversionFactor == 1)
                {
                    TransactionDebit.DebitForeign = 0;
                }
                else
                {
                    TransactionDebit.DebitForeign = TransactionDebit.Debit / HeaderTOUpdate.ConversionFactor;
                };
                TransactionDebit.CreditForeign = 0;
                TransactionDebit.CreditDebitForeign = TransactionDebit.DebitForeign;
                TransactionDebit.Note = HeaderTOUpdate.Note;
                TransactionDebit.RowNumber = 1;
                _unitOfWork.Transaction.Update(TransactionDebit);
                _unitOfWork.Complete();
                var iRow = 1;
                _unitOfWork.NativeSql.DeleteTransActionCredit(HeaderTOUpdate.CompanyID, HeaderTOUpdate.VoucherNumber,
                    HeaderTOUpdate.CompanyTransactionKindNo, HeaderTOUpdate.TransactionKindNo, HeaderTOUpdate.CompanyYear);
                foreach (var Trans in TranAction)
                {
                    if (Company.WorkWithCostCenter)
                    {
                        if (!String.IsNullOrWhiteSpace(Trans.AccountNumber) && !String.IsNullOrWhiteSpace(Trans.CostCenter))
                        {
                            try
                            {
                                iRow = iRow + 1;
                                Trans.CompanyID = UserInfo.fCompanyId;
                                Trans.CompanyYear = HeaderTOUpdate.CompanyYear;
                                Trans.CompanyTransactionKindNo = HeaderTOUpdate.CompanyTransactionKindNo;
                                Trans.TransactionKindNo = HeaderTOUpdate.TransactionKindNo;
                                Trans.VoucherDate = HeaderTOUpdate.VoucherDate;
                                Trans.VoucherNumber = HeaderTOUpdate.VoucherNumber;
                                Trans.InsUserID = HeaderTOUpdate.InsUserID;
                                Trans.VHI = HeaderTOUpdate.VHI;
                                Trans.InsDateTime = DateTime.Now;
                                Trans.RowNumber = iRow;
                                Trans.Debit = 0;
                                Trans.Credit = Trans.Credit;
                                Trans.AccountNumber = Trans.AccountNumber;
                                Trans.CostCenter = Trans.CostCenter;
                                if (HeaderTOUpdate.ConversionFactor == 1)
                                {
                                    Trans.CreditForeign = 0;
                                }
                                else
                                {
                                    Trans.CreditForeign = Trans.Credit / HeaderTOUpdate.ConversionFactor;
                                };
                                Trans.DebitForeign = 0;
                                Trans.CreditDebitForeign = Trans.CreditForeign;
                                Trans.CreditForeign = Trans.CreditDebitForeign;
                                _unitOfWork.Transaction.Add(Trans);
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
                        if (!String.IsNullOrWhiteSpace(Trans.AccountNumber))
                        {
                            try
                            {
                                iRow = iRow + 1;
                                Trans.CompanyID = UserInfo.fCompanyId;
                                Trans.CompanyYear = HeaderTOUpdate.CompanyYear;
                                Trans.CompanyTransactionKindNo = HeaderTOUpdate.CompanyTransactionKindNo;
                                Trans.TransactionKindNo = HeaderTOUpdate.TransactionKindNo;
                                Trans.VoucherDate = HeaderTOUpdate.VoucherDate;
                                Trans.VoucherNumber = HeaderTOUpdate.VoucherNumber;
                                Trans.InsUserID = HeaderTOUpdate.InsUserID;
                                Trans.VHI = HeaderTOUpdate.VHI;
                                Trans.InsDateTime = DateTime.Now;
                                Trans.RowNumber = iRow;
                                Trans.Debit = 0;
                                Trans.Credit = Trans.Credit;
                                Trans.AccountNumber = Trans.AccountNumber;
                                Trans.CostCenter = Trans.CostCenter;
                                if (HeaderTOUpdate.ConversionFactor == 1)
                                {
                                    Trans.CreditForeign = 0;
                                }
                                else
                                {
                                    Trans.CreditForeign = Trans.Credit / HeaderTOUpdate.ConversionFactor;
                                };
                                Trans.DebitForeign = 0;
                                Trans.CreditDebitForeign = Trans.CreditForeign;
                                Trans.CreditForeign = Trans.CreditDebitForeign;
                                _unitOfWork.Transaction.Add(Trans);
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
                HeaderTOUpdate.RowCount = iRow;
                _unitOfWork.Header.Update(HeaderTOUpdate);
                _unitOfWork.Complete();
                var OldCurrency = _unitOfWork.NativeSql.GetCurrencyValueByID(UserInfo.fCompanyId, ObjUpdate.CurrencyID);
                if (OldCurrency == null)
                {
                    var NewCurr = new CurrencyValue
                    {
                        CurrencyID = ObjUpdate.CurrencyID,
                        CompanyID = UserInfo.fCompanyId,
                        ConversionFactor = ObjUpdate.CurrencyNewValue,
                        InsDateTime = DateTime.Now,
                        InsUserID = userId
                    };
                    _unitOfWork.CurrencyValue.Add(NewCurr);
                    _unitOfWork.Complete();

                }
                else
                if (OldCurrency.ConversionFactor != ObjUpdate.CurrencyNewValue)
                {
                    var NewCurr = new CurrencyValue
                    {
                        CurrencyID = ObjUpdate.CurrencyID,
                        CompanyID = UserInfo.fCompanyId,
                        ConversionFactor = ObjUpdate.CurrencyNewValue,
                        InsDateTime = DateTime.Now,
                        InsUserID = userId
                    };
                    _unitOfWork.CurrencyValue.Add(NewCurr);
                    _unitOfWork.Complete();
                }
                Msg.LastID = _unitOfWork.Header.GetMaxVHByKind(UserInfo.fCompanyId, HeaderTOUpdate.CompanyTransactionKindNo, HeaderTOUpdate.TransactionKindNo, HeaderTOUpdate.CompanyYear).ToString(); 
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
        [Authorize(Roles = "CoOwner, ExportDepositInTheBank")]
        public ActionResult Export()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetDepositInTheBankFromTransactionKind(UserInfo.fCompanyId);
            int year = DateTime.Now.Year;
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
            var DepositInTheBankFilter = new DepositInTheBankFilter
            {
                FromDate = FromDate,
                ToDate = ToDate,
                CompanyTransactionKind = CompanyTransactionKindObj,
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency
            };
            return View(DepositInTheBankFilter);
        }
        [HttpPost]
        public JsonResult GetAllUnExport(DepositInTheBankFilter Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllDepositInTheBank = _unitOfWork.NativeSql.GetAllDepoistInTheBankFromHeaderUnExport(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                if (AllDepositInTheBank == null)
                {
                    return Json(new List<DepositInTheBankFilter>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.VoucherNumber))
                {
                    AllDepositInTheBank = AllDepositInTheBank.Where(m => m.VoucherNumber == Obj.VoucherNumber).ToList();
                }
                if (Obj.CurrencyID != 0)
                {
                    AllDepositInTheBank = AllDepositInTheBank.Where(m => m.CurrencyID == Obj.CurrencyID).ToList();
                }
                if (Obj.CompanyTransactionKindNo != 0)
                {
                    AllDepositInTheBank = AllDepositInTheBank.Where(m => m.CompanyTransactionKindNo == Obj.CompanyTransactionKindNo).ToList();
                }
                int iRow = 0;
                foreach (var iRowCount in AllDepositInTheBank)
                {
                    iRowCount.iRowTable = iRow;
                    iRow = iRow + 1;
                }
                return Json(AllDepositInTheBank, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<DebitNoteFilterVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [Authorize(Roles = "CoOwner, UnExportDepositInTheBank")]
        public ActionResult UnExport()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetDepositInTheBankFromTransactionKind(UserInfo.fCompanyId);
            int year = DateTime.Now.Year;
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
            var DepositInTheBankFilter = new DepositInTheBankFilter
            {
                FromDate = new DateTime(year, 1, 1),
                ToDate = new DateTime(year, 12, 31),
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                CompanyTransactionKind = CompanyTransactionKindObj,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency
            };
            return View(DepositInTheBankFilter);
        }
        [HttpPost]
        public JsonResult GetAllExport(DebitNoteFilterVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllDepositInTheBank = _unitOfWork.NativeSql.GetAllDepoistInTheBankFromHeaderExport(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                if (AllDepositInTheBank == null)
                {
                    return Json(new List<DepositInTheBankFilter>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.VoucherNumber))
                {
                    AllDepositInTheBank = AllDepositInTheBank.Where(m => m.VoucherNumber == Obj.VoucherNumber).ToList();
                }
                if (Obj.CurrencyID != 0)
                {
                    AllDepositInTheBank = AllDepositInTheBank.Where(m => m.CurrencyID == Obj.CurrencyID).ToList();
                }
                if (Obj.CompanyTransactionKindNo != 0)
                {
                    AllDepositInTheBank = AllDepositInTheBank.Where(m => m.CompanyTransactionKindNo == Obj.CompanyTransactionKindNo).ToList();
                }
                int iRow = 0;
                foreach (var iRowCount in AllDepositInTheBank)
                {
                    iRowCount.iRowTable = iRow;
                    iRow = iRow + 1;
                }
                return Json(AllDepositInTheBank, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<DebitNoteFilterVM>(), JsonRequestBehavior.AllowGet);
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