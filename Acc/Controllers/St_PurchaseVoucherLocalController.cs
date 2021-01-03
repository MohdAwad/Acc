using Acc.Helpers;
using Acc.Models;
using Acc.Persistence;
using Acc.Repositories;
using Acc.ViewModels;
using DevExpress.DataAccess.Sql.DataApi;
using DevExpress.PivotGrid.Criteria;
using DevExpress.Web.Internal;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Acc.Controllers
{
    [Authorize]
    public class St_PurchaseVoucherLocalController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public St_PurchaseVoucherLocalController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            int year = DateTime.Now.Year;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var St_Warehouse = _unitOfWork.St_Warehouse.GetAllSt_Warehouse(UserInfo.fCompanyId);
            var St_HeaderVM = new St_HeaderVM
            {
                St_Warehouse = St_Warehouse,
                FromDate = new DateTime(year, 1, 1),
                ToDate = new DateTime(year, 12, 31),
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency
            };
            return View(St_HeaderVM);
        }
        [HttpPost]
        public JsonResult GetAllSt_PurchaseVoucher(St_HeaderVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllSt_PurchaseVoucher = _unitOfWork.NativeSql.GetAllSt_PurchaseVoucher(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                if (AllSt_PurchaseVoucher == null)
                {
                    return Json(new List<St_HeaderVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.VoucherNumber))
                {
                    AllSt_PurchaseVoucher = AllSt_PurchaseVoucher.Where(m => m.VoucherNumber == Obj.VoucherNumber).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.AccountNumber))
                {
                    AllSt_PurchaseVoucher = AllSt_PurchaseVoucher.Where(m => m.AccountNumber == Obj.AccountNumber).ToList();
                }
                if (Obj.TaxType != 0)
                {
                    AllSt_PurchaseVoucher = AllSt_PurchaseVoucher.Where(m => m.TaxType == Obj.TaxType).ToList();
                }
                if (Obj.VoucherCase != 0)
                {
                    AllSt_PurchaseVoucher = AllSt_PurchaseVoucher.Where(m => m.VoucherCase == Obj.VoucherCase).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.StockCode))
                {
                    AllSt_PurchaseVoucher = AllSt_PurchaseVoucher.Where(m => m.StockCode == Obj.StockCode).ToList();
                }
                return Json(AllSt_PurchaseVoucher, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<St_HeaderVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public JsonResult GetMaxVoucher(int id, int id2, string id3)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            if (id != 0)
            {
                var ObjComapnyTransactionKind = _unitOfWork.St_CompanyTransactionKind.GetSt_CompanyTransactionKindByIDAndStockCode(UserInfo.fCompanyId, id, id3);
                if (ObjComapnyTransactionKind.AutoSerial)
                {
                    var TransactionKindNo = _unitOfWork.St_CompanyTransactionKind.GetSt_TransKindForSt_CompanyTransactionKind(UserInfo.fCompanyId, id);
                    var VHI = _unitOfWork.St_Header.GetMaxVoucher(UserInfo.fCompanyId, id, TransactionKindNo, id2,id3).ToString();
                    return Json(VHI, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string SerialNumber = "";
                    int Serial = ObjComapnyTransactionKind.Serial;
                    string Symbol = ObjComapnyTransactionKind.Symbol;
                    int LengthLastSerial = _unitOfWork.St_CompanyTransactionKindSymbolSerial.GetMaxSerial(UserInfo.fCompanyId, id, id3).ToString().Length;
                    Serial = Serial - LengthLastSerial;
                    for (int i = 0; i <= Serial; i++)
                    {
                        if (i < Serial)
                        {
                            SerialNumber = SerialNumber + "0";
                        }
                        else if (i == Serial)
                        {
                            SerialNumber = SerialNumber + _unitOfWork.St_CompanyTransactionKindSymbolSerial.GetMaxSerial(UserInfo.fCompanyId, id, id3).ToString();
                        }
                    }
                    var VHI = Symbol + SerialNumber;
                    return Json(VHI, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetCompanyTransactionKindNo(int id, string id2)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            if (id != 0)
            {
                int ComapnyTransactionKindNo = _unitOfWork.St_CompanyTransactionKind.GetSt_CompanyTransactionKind(UserInfo.fCompanyId, id, id2);
                return Json(ComapnyTransactionKindNo, JsonRequestBehavior.AllowGet);
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetWarehouseAccount(string id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            if (id != "")
            {
                var AllWarehouseAccount = _unitOfWork.St_WarehouseAccount.GetSt_WarehouseAccountByID(UserInfo.fCompanyId, id);
                return Json(AllWarehouseAccount, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAccountName(string id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            if (id != "")
            {
                string AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, id);
                return Json(AccountName, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var ObjGet = _unitOfWork.St_OtherAccount.GetSt_OtherAccountByID(UserInfo.fCompanyId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            FixAccountInfoVM ObjFix = new FixAccountInfoVM
            {
                AccountNo = "",
                AccountName = ""
            };
            if (ObjGet != null)
            {
                ObjFix.AccountNo = ObjGet.PurchaseTaxAccountNumber;
                ObjFix.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.PurchaseTaxAccountNumber);
            }
            var St_Warehouse = _unitOfWork.St_Warehouse.GetAllSt_Warehouse(UserInfo.fCompanyId);
            St_HeaderVM Obj = new St_HeaderVM
            {
                VoucherDate = DateTime.Now,
                DueDate = DateTime.Now,
                TaxAccountNumber = ObjFix.AccountNo,
                TaxAccountName = ObjFix.AccountName,
                OrignailTaxAccountNumber = ObjFix.AccountNo,
                OrignailTaxAccountName = ObjFix.AccountName,
                St_Warehouse = St_Warehouse,
                CurrencyID = 1,
                ConversionFactor = 1,
                TransactionKindNo = 501,
                CompanyYear = UserInfo.CurrYear,
                WorkWithCostCenter = Company.WorkWithCostCenter,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency
            };
            return View(Obj);
        }
        [ValidateInput(false)]
        public ActionResult GridViewItems(string id, string id2, string id3, string id4,string id5)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            if (!String.IsNullOrEmpty(id))
            {
                var St_TransactionObj = _unitOfWork.NativeSql.GetSt_Transacation(id, int.Parse(id2), UserInfo.fCompanyId, int.Parse(id3), int.Parse(id4), id5);
                return PartialView("GridViewItems", St_TransactionObj);
            }
            else
            {
                var St_TransactionObj = new List<St_HeaderVM>();
                return PartialView("GridViewItems", St_TransactionObj);
            }


        }
        public JsonResult Save(St_HeaderVM ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                var SaveHeader = new St_Header();
                SaveHeader.InsDateTime = DateTime.Now;
                SaveHeader.InsUserID = userId;
                SaveHeader.CompanyTransactionKindNo = ObjToSave.CompanyTransactionKindNo;
                SaveHeader.TransactionKindNo = ObjToSave.TransactionKindNo;
                SaveHeader.StockCode = ObjToSave.StockCode;
                SaveHeader.AccountNumber = ObjToSave.AccountNumber;
                SaveHeader.CompanyID = UserInfo.fCompanyId;
                SaveHeader.CompanyYear = ObjToSave.CompanyYear;
                SaveHeader.TaxType = ObjToSave.TaxType;
                SaveHeader.VoucherCase = ObjToSave.VoucherCase;
                SaveHeader.VoucherDate = ObjToSave.VoucherDate;
                SaveHeader.DueDate = ObjToSave.DueDate;
                SaveHeader.CurrencyID = ObjToSave.CurrencyID;
                SaveHeader.ConversionFactor = ObjToSave.ConversionFactor;
                SaveHeader.Remark = ObjToSave.Remark;
                SaveHeader.Hint = ObjToSave.Hint;
                SaveHeader.OrderNumber = ObjToSave.OrderNumber;
                SaveHeader.OriginalVoucherNumber = ObjToSave.OriginalVoucherNumber;
                SaveHeader.NetTotalLocalBeforDiscount = ObjToSave.NetTotalLocalBeforDiscount;
                SaveHeader.NetTotalLineDiscountLocal = ObjToSave.NetTotalLineDiscountLocal;
                SaveHeader.NetTotalLocalAfterLineDiscount = ObjToSave.NetTotalLocalAfterLineDiscount;
                SaveHeader.NetTotalTaxAfterLineDiscountLocal = ObjToSave.NetTotalTaxAfterLineDiscountLocal;
                SaveHeader.NetTotalAfterLineDiscountBeforDiscountAllLocal = ObjToSave.NetTotalAfterLineDiscountBeforDiscountAllLocal;
                SaveHeader.NetTotalDiscountLocal = ObjToSave.NetTotalDiscountLocal;
                SaveHeader.DiscountPercentage = ObjToSave.DiscountPercentage;
                SaveHeader.NetTotalLocalAfterDiscount = ObjToSave.NetTotalLocalAfterDiscount;
                SaveHeader.NetTotalTaxLocal = ObjToSave.NetTotalTaxLocal;
                SaveHeader.NetTotalLocal = ObjToSave.NetTotalLocal;
                if (Company.DirectExportTheTransactions)
                {
                    SaveHeader.Exported = 1;
                }
                else
                {
                    SaveHeader.Exported = 0;
                }
                var ObjComapnyTransactionKind = _unitOfWork.St_CompanyTransactionKind.GetSt_CompanyTransactionKindByIDAndStockCode(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo,SaveHeader.StockCode);
                if (ObjComapnyTransactionKind.AutoSerial)
                {
                    SaveHeader.VoucherNumber = _unitOfWork.St_Header.GetMaxVoucher(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo, SaveHeader.TransactionKindNo, ObjToSave.CompanyYear, SaveHeader.StockCode).ToString();
                    SaveHeader.VHI = int.Parse(SaveHeader.VoucherNumber);
                }
                else
                {
                    string SerialNumber = "";
                    int Serial = ObjComapnyTransactionKind.Serial;
                    string Symbol = ObjComapnyTransactionKind.Symbol;
                    int LengthLastSerial = _unitOfWork.St_CompanyTransactionKindSymbolSerial.GetMaxSerial(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo, SaveHeader.StockCode).ToString().Length;
                    Serial = Serial - LengthLastSerial;
                    for (int i = 0; i <= Serial; i++)
                    {
                        if (i < Serial)
                        {
                            SerialNumber = SerialNumber + "0";
                        }
                        else if (i == Serial)
                        {
                            SerialNumber = SerialNumber + _unitOfWork.St_CompanyTransactionKindSymbolSerial.GetMaxSerial(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo, SaveHeader.StockCode).ToString();
                        }
                    }
                    SaveHeader.VoucherNumber = Symbol + SerialNumber;
                    SaveHeader.VHI = _unitOfWork.St_CompanyTransactionKindSymbolSerial.GetMaxSerial(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo, SaveHeader.StockCode);
                    _unitOfWork.St_CompanyTransactionKindSymbolSerial.Update(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo, SaveHeader.StockCode);
                }
                int iRow = 0;
                foreach (var SaveItems in ObjToSave.St_Transaction)
                {
                    if (!String.IsNullOrWhiteSpace(SaveItems.ItemCode))
                    {
                        try
                        {
                            iRow = iRow + 1;
                            SaveItems.CompanyID = UserInfo.fCompanyId;
                            SaveItems.CompanyYear = SaveHeader.CompanyYear;
                            SaveItems.CompanyTransactionKindNo = SaveHeader.CompanyTransactionKindNo;
                            SaveItems.TransactionKindNo = SaveHeader.TransactionKindNo;
                            SaveItems.StockCode = SaveHeader.StockCode;
                            SaveItems.VoucherDate = SaveHeader.VoucherDate;
                            SaveItems.VoucherNumber = SaveHeader.VoucherNumber;
                            SaveItems.VHI = SaveHeader.VHI;
                            SaveItems.IsDeleted = 0;
                            SaveItems.ItemCode = SaveItems.ItemCode;
                            SaveItems.SimilarItemCode = SaveItems.SimilarItemCode;
                            if (SaveItems.SimilarItemCode == "" || SaveItems.SimilarItemCode == null)
                            {
                                SaveItems.SimilarItemCode = SaveItems.ItemCode;
                            }
                            SaveItems.Quantity = SaveItems.Quantity;
                            SaveItems.Bonus = SaveItems.Bonus;
                            SaveItems.QuantityInputOutput = SaveItems.Quantity;
                            SaveItems.BonusInputOutput = SaveItems.Bonus;
                            SaveItems.TotalLocalBeforDiscount = SaveItems.TotalLocalBeforDiscount;
                            SaveItems.TotalLineDiscountLocal = SaveItems.TotalLineDiscountLocal;
                            SaveItems.TotalLocalAfterLineDiscount = SaveItems.TotalLocalAfterLineDiscount;
                            SaveItems.LineDiscountPercentage = SaveItems.LineDiscountPercentage;
                            SaveItems.TotalTaxAfterLineDiscountLocal = SaveItems.TotalTaxAfterLineDiscountLocal;
                            SaveItems.TotalAfterLineDiscountBeforDiscountAllLocal = SaveItems.TotalAfterLineDiscountBeforDiscountAllLocal;
                            SaveItems.TotalDiscountLocal = SaveItems.TotalDiscountLocal;
                            SaveItems.DiscountPercentage = SaveHeader.DiscountPercentage;
                            SaveItems.TotalLocalAfterDiscount = SaveItems.TotalLocalAfterDiscount;
                            SaveItems.TotalTaxLocal = SaveItems.TotalTaxLocal;
                            SaveItems.TotalLocal = SaveItems.TotalLocal;
                            SaveItems.TotalCostLocal = SaveItems.TotalCostLocal;
                            SaveItems.PricePieceLocalBeforDiscount = SaveItems.PricePieceLocalBeforDiscount;
                            SaveItems.PricePieceLineDiscountLocal = SaveItems.PricePieceLineDiscountLocal;
                            SaveItems.PricePieceLocalAfterLineDiscount = SaveItems.PricePieceLocalAfterLineDiscount;
                            SaveItems.PricePieceTaxAfterLineDiscountLocal = SaveItems.PricePieceTaxAfterLineDiscountLocal;
                            SaveItems.PricePieceAfterLineDiscountBeforDiscountAllLocal = SaveItems.PricePieceAfterLineDiscountBeforDiscountAllLocal;
                            SaveItems.PricePieceDiscountLocal = SaveItems.PricePieceDiscountLocal;
                            SaveItems.PricePieceLocalAfterDiscount = SaveItems.PricePieceLocalAfterDiscount;
                            SaveItems.PricePieceTaxLocal = SaveItems.PricePieceTaxLocal;
                            SaveItems.PricePieceTotalLocal = SaveItems.PricePieceTotalLocal;
                            SaveItems.CostPieceLocal = SaveItems.CostPieceLocal;
                            SaveItems.TaxRate = SaveItems.TaxRate;
                            SaveItems.TaxType = SaveHeader.TaxType;
                            SaveItems.RowNumber = iRow;
                            SaveItems.Remark = SaveHeader.Remark;
                            SaveItems.Hint = SaveHeader.Hint;
                            SaveItems.ExpierDate = (DateTime.Now).AddYears(30);
                            SaveItems.InsDateTime = DateTime.Now;
                            SaveItems.InsUserID = userId;
                            _unitOfWork.St_Header.AddTransaction(SaveItems);
                        }
                        catch (Exception ex)
                        {
                            Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                            Msg.Code = 0;
                            return Json(Msg, JsonRequestBehavior.AllowGet);

                        }
                    }
                }

                var Header = new Header();
                Header.TransactionKindNo = ObjToSave.TransactionKindNo;
                Header.CompanyTransactionKindNo = ObjToSave.CompanyTransactionKindNo;
                Header.InsDateTime = DateTime.Now;
                Header.InsUserID = userId;
                Header.CompanyID = UserInfo.fCompanyId;
                Header.TotalCredit = ObjToSave.NetTotalLocal;
                Header.TotalDebit = ObjToSave.NetTotalLocal;
                Header.TotalDebitForeign = 0;
                Header.TotalCreditForeign = 0;
                Header.CompanyYear = ObjToSave.CompanyYear;
                Header.VoucherNumber = SaveHeader.VoucherNumber;
                Header.VHI = SaveHeader.VHI;
                Header.Note = ObjToSave.Remark;
                Header.VoucherDate = ObjToSave.VoucherDate;
                Header.FCurrencyID = ObjToSave.CurrencyID;
                Header.ConversionFactor = ObjToSave.ConversionFactor;
                if (Company.DirectExportTheTransactions)
                {
                    Header.Exported = 1;
                }
                else
                {
                    Header.Exported = 0;
                }

                if (ObjToSave.TaxType != 1)
                {
                    Header.RowCount = 2;
                }
                else
                {
                    Header.RowCount = 3;
                }
                Header.SaleID = 0;
                _unitOfWork.Header.Add(Header);
                Transaction ObjCredit = new Transaction();
                ObjCredit.AccountNumber = ObjToSave.CreditAccountNumber;
                ObjCredit.CostCenter = ObjToSave.CreditCostNumber;
                ObjCredit.CompanyTransactionKindNo = ObjToSave.CompanyTransactionKindNo;
                ObjCredit.TransactionKindNo = ObjToSave.TransactionKindNo;
                ObjCredit.CompanyYear = ObjToSave.CompanyYear;
                ObjCredit.CompanyID = UserInfo.fCompanyId;
                ObjCredit.Debit = 0;
                ObjCredit.DebitForeign = 0;
                ObjCredit.Credit = ObjToSave.NetTotalLocal;
                ObjCredit.CreditForeign = 0;
                ObjCredit.CreditDebitForeign = 0;
                ObjCredit.Exported = 0;
                ObjCredit.InsDateTime = DateTime.Now;
                ObjCredit.InsUserID = userId;
                ObjCredit.Note = ObjToSave.Remark;
                ObjCredit.RowNumber = 1;
                ObjCredit.VHI = SaveHeader.VHI;
                ObjCredit.VoucherDate = ObjToSave.VoucherDate;
                ObjCredit.VoucherNumber = SaveHeader.VoucherNumber;

                Transaction ObjDebit = new Transaction();
                ObjDebit.AccountNumber = ObjToSave.DebitAccountNumber;
                ObjDebit.CostCenter = ObjToSave.DebitCostNumber;
                ObjDebit.CompanyTransactionKindNo = ObjToSave.CompanyTransactionKindNo;
                ObjDebit.TransactionKindNo = ObjToSave.TransactionKindNo;
                ObjDebit.CompanyYear = ObjToSave.CompanyYear;
                ObjDebit.CompanyID = UserInfo.fCompanyId;
                ObjDebit.Debit = ObjToSave.NetTotalLocalAfterDiscount;
                ObjDebit.DebitForeign = 0;
                ObjDebit.CreditDebitForeign = 0;
                ObjDebit.Credit = 0;
                ObjDebit.CreditForeign = 0;
                ObjDebit.Exported = 0;
                ObjDebit.InsDateTime = DateTime.Now;
                ObjDebit.InsUserID = userId;
                ObjDebit.Note = ObjToSave.Remark;
                ObjDebit.RowNumber = 2;
                ObjDebit.VHI = SaveHeader.VHI;
                ObjDebit.VoucherDate = ObjToSave.VoucherDate;
                ObjDebit.VoucherNumber = SaveHeader.VoucherNumber;

                _unitOfWork.Transaction.Add(ObjDebit);
                _unitOfWork.Transaction.Add(ObjCredit);

                if (ObjToSave.TaxType == 1)
                {
                    Transaction ObjTax = new Transaction();
                    ObjTax.AccountNumber = ObjToSave.TaxAccountNumber;
                    ObjTax.CostCenter = ObjToSave.TaxCostNumber;
                    ObjTax.CompanyTransactionKindNo = ObjToSave.CompanyTransactionKindNo;
                    ObjTax.TransactionKindNo = ObjToSave.TransactionKindNo;
                    ObjTax.CompanyYear = ObjToSave.CompanyYear;
                    ObjTax.CompanyID = UserInfo.fCompanyId;
                    ObjTax.Debit = ObjToSave.NetTotalTaxLocal;
                    ObjTax.DebitForeign = 0;
                    ObjTax.CreditDebitForeign = 0;
                    ObjTax.Credit = 0;
                    ObjTax.CreditForeign = 0;
                    ObjTax.Exported = 0;
                    ObjTax.InsDateTime = DateTime.Now;
                    ObjTax.InsUserID = userId;
                    ObjTax.Note = ObjToSave.Remark;
                    ObjTax.RowNumber = 3;
                    ObjTax.VHI = SaveHeader.VHI;
                    ObjTax.VoucherDate = ObjToSave.VoucherDate;
                    ObjTax.VoucherNumber = SaveHeader.VoucherNumber;
                    _unitOfWork.Transaction.Add(ObjTax);
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
                SaveHeader.RowCount = iRow;
                _unitOfWork.St_Header.AddHeader(SaveHeader);
                _unitOfWork.Complete();
                Msg.Code = 1;
                Msg.Msg = Resources.Resource.AddedSuccessfully;

                Msg.Year = SaveHeader.CompanyYear;
                Msg.FCompanyId = SaveHeader.CompanyID;
                Msg.VoucherNumber = SaveHeader.VoucherNumber;
                Msg.TransactionKindNo = SaveHeader.TransactionKindNo.ToString();
                Msg.CompanyTransactionKindNo = SaveHeader.CompanyTransactionKindNo.ToString();
                Msg.StockCode = SaveHeader.StockCode;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                Msg.Code = 0;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult Update(string id, int id2, int id3, int id4, string id5)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var St_Warehouse = _unitOfWork.St_Warehouse.GetAllSt_Warehouse(UserInfo.fCompanyId);
            var St_HeaderObj = _unitOfWork.St_Header.GetSt_HeaderDataById(id, UserInfo.fCompanyId, id2, id3, id4, id5);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var HeaderObj = _unitOfWork.Header.GetHeaderDataById(id, UserInfo.fCompanyId, id2, id3, id4);
            var TransactionCreditObj  = _unitOfWork.NativeSql.GetTransactionsDetailCredit(id, UserInfo.fCompanyId, id2, 1, id4);
            var TransactionDebitObj = _unitOfWork.NativeSql.GetTransactionsDetailDebit(id, UserInfo.fCompanyId, id2, 2, id4);
            var TransactionTaxObj = _unitOfWork.NativeSql.GetTransactionsDetailTax(id, UserInfo.fCompanyId, id2, 3, id4);
            var ObjGet = _unitOfWork.St_OtherAccount.GetSt_OtherAccountByID(UserInfo.fCompanyId);
            FixAccountInfoVM ObjFix = new FixAccountInfoVM
            {
                AccountNo = "",
                AccountName = ""
            };
            if (ObjGet != null)
            {
                ObjFix.AccountNo = ObjGet.PurchaseTaxAccountNumber;
                ObjFix.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.PurchaseTaxAccountNumber);
            }
            var St_HeaderVM = new St_HeaderVM { };
            St_HeaderVM.St_Warehouse = St_Warehouse;
            St_HeaderVM.StockCode = St_HeaderObj.StockCode;
            St_HeaderVM.CompanyTransactionKindNo = St_HeaderObj.CompanyTransactionKindNo;
            St_HeaderVM.TransactionKindNo = St_HeaderObj.TransactionKindNo;
            St_HeaderVM.CompanyYear = St_HeaderObj.CompanyYear;
            St_HeaderVM.TaxType = St_HeaderObj.TaxType;
            St_HeaderVM.VoucherCase = St_HeaderObj.VoucherCase;
            St_HeaderVM.VoucherDate = St_HeaderObj.VoucherDate;
            St_HeaderVM.DueDate = St_HeaderObj.DueDate;
            St_HeaderVM.CurrencyID = St_HeaderObj.CurrencyID;
            St_HeaderVM.ConversionFactor = St_HeaderObj.ConversionFactor;
            St_HeaderVM.AccountNumber = St_HeaderObj.AccountNumber;
            St_HeaderVM.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, St_HeaderObj.AccountNumber);
            St_HeaderVM.OriginalVoucherNumber = St_HeaderObj.OriginalVoucherNumber;
            St_HeaderVM.OrderNumber = St_HeaderObj.OrderNumber;
            St_HeaderVM.NetTotalLocalBeforDiscount = St_HeaderObj.NetTotalLocalBeforDiscount;
            St_HeaderVM.NetTotalLineDiscountLocal = St_HeaderObj.NetTotalLineDiscountLocal;
            St_HeaderVM.NetTotalLocalAfterLineDiscount = St_HeaderObj.NetTotalLocalAfterLineDiscount;
            St_HeaderVM.NetTotalTaxAfterLineDiscountLocal = St_HeaderObj.NetTotalTaxAfterLineDiscountLocal;
            St_HeaderVM.NetTotalAfterLineDiscountBeforDiscountAllLocal = St_HeaderObj.NetTotalAfterLineDiscountBeforDiscountAllLocal;
            St_HeaderVM.NetTotalDiscountLocal = St_HeaderObj.NetTotalDiscountLocal;
            St_HeaderVM.DiscountPercentage = St_HeaderObj.DiscountPercentage;
            St_HeaderVM.NetTotalLocalAfterDiscount = St_HeaderObj.NetTotalLocalAfterDiscount;
            St_HeaderVM.NetTotalTaxLocal = St_HeaderObj.NetTotalTaxLocal;
            St_HeaderVM.NetTotalLocal = St_HeaderObj.NetTotalLocal;
            St_HeaderVM.Remark = St_HeaderObj.Remark;
            St_HeaderVM.Hint = St_HeaderObj.Hint;
            St_HeaderVM.VoucherNumber = St_HeaderObj.VoucherNumber;
            St_HeaderVM.VHI = St_HeaderObj.VHI;
            St_HeaderVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            St_HeaderVM.WorkWithCostCenter = Company.WorkWithCostCenter;
            St_HeaderVM.DebitAccountNumber = TransactionDebitObj.AccountNumber;
            St_HeaderVM.CreditAccountNumber = TransactionCreditObj.AccountNumber;
            St_HeaderVM.DebitCostNumber = TransactionDebitObj.CostCenter;
            St_HeaderVM.CreditCostNumber = TransactionCreditObj.CostCenter;
            St_HeaderVM.DebitAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionDebitObj.AccountNumber);
            St_HeaderVM.CreditAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionCreditObj.AccountNumber);
            St_HeaderVM.DebitCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionDebitObj.CostCenter);
            St_HeaderVM.CreditCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionCreditObj.CostCenter);
            St_HeaderVM.OrignailTaxAccountNumber = ObjFix.AccountNo;
            St_HeaderVM.OrignailTaxAccountName = ObjFix.AccountName;
            if (HeaderObj.RowCount > 2)
            {
                St_HeaderVM.TaxAccountNumber = TransactionTaxObj.AccountNumber;
                St_HeaderVM.TaxCostNumber = TransactionTaxObj.CostCenter;
                St_HeaderVM.TaxCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionTaxObj.CostCenter);
                St_HeaderVM.TaxAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionTaxObj.AccountNumber);
            }
            return View(St_HeaderVM);
        }
        public JsonResult UpdateSt_PurchaseVoucher(St_HeaderVM ObjToUpdate)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                
                var SaveHeader = new St_Header();
                SaveHeader.InsDateTime = DateTime.Now;
                SaveHeader.InsUserID = userId;
                SaveHeader.CompanyTransactionKindNo = ObjToUpdate.CompanyTransactionKindNo;
                SaveHeader.TransactionKindNo = ObjToUpdate.TransactionKindNo;
                SaveHeader.StockCode = ObjToUpdate.StockCode;
                SaveHeader.AccountNumber = ObjToUpdate.AccountNumber;
                SaveHeader.CompanyID = UserInfo.fCompanyId;
                SaveHeader.CompanyYear = ObjToUpdate.CompanyYear;
                SaveHeader.TaxType = ObjToUpdate.TaxType;
                SaveHeader.VoucherCase = ObjToUpdate.VoucherCase;
                SaveHeader.VoucherDate = ObjToUpdate.VoucherDate;
                SaveHeader.DueDate = ObjToUpdate.DueDate;
                SaveHeader.CurrencyID = ObjToUpdate.CurrencyID;
                SaveHeader.ConversionFactor = ObjToUpdate.ConversionFactor;
                SaveHeader.Remark = ObjToUpdate.Remark;
                SaveHeader.Hint = ObjToUpdate.Hint;
                SaveHeader.OrderNumber = ObjToUpdate.OrderNumber;
                SaveHeader.OriginalVoucherNumber = ObjToUpdate.OriginalVoucherNumber;
                SaveHeader.NetTotalLocalBeforDiscount = ObjToUpdate.NetTotalLocalBeforDiscount;
                SaveHeader.NetTotalLineDiscountLocal = ObjToUpdate.NetTotalLineDiscountLocal;
                SaveHeader.NetTotalLocalAfterLineDiscount = ObjToUpdate.NetTotalLocalAfterLineDiscount;
                SaveHeader.NetTotalTaxAfterLineDiscountLocal = ObjToUpdate.NetTotalTaxAfterLineDiscountLocal;
                SaveHeader.NetTotalAfterLineDiscountBeforDiscountAllLocal = ObjToUpdate.NetTotalAfterLineDiscountBeforDiscountAllLocal;
                SaveHeader.NetTotalDiscountLocal = ObjToUpdate.NetTotalDiscountLocal;
                SaveHeader.DiscountPercentage = ObjToUpdate.DiscountPercentage;
                SaveHeader.NetTotalLocalAfterDiscount = ObjToUpdate.NetTotalLocalAfterDiscount;
                SaveHeader.NetTotalTaxLocal = ObjToUpdate.NetTotalTaxLocal;
                SaveHeader.NetTotalLocal = ObjToUpdate.NetTotalLocal;
                SaveHeader.VoucherNumber = ObjToUpdate.VoucherNumber;
                SaveHeader.VHI = ObjToUpdate.VHI;
                int iRow = 0;
                var TransActionDeleteVM = new TransActionDeleteVM();
                TransActionDeleteVM.CompanyYear = SaveHeader.CompanyYear;
                TransActionDeleteVM.CompanyID = SaveHeader.CompanyID;
                TransActionDeleteVM.VoucherNumber = SaveHeader.VoucherNumber;
                TransActionDeleteVM.CompanyTransactionKindNo = SaveHeader.CompanyTransactionKindNo;
                TransActionDeleteVM.TransactionKindNo = SaveHeader.TransactionKindNo;
                _unitOfWork.Header.Delete(TransActionDeleteVM);
                _unitOfWork.St_Header.DeleteHeader(SaveHeader);
                _unitOfWork.NativeSql.DeleteTransActionTrans(SaveHeader.CompanyID, SaveHeader.VoucherNumber, SaveHeader.CompanyTransactionKindNo, SaveHeader.TransactionKindNo, SaveHeader.CompanyYear);
                _unitOfWork.NativeSql.DeleteSt_Transaction(SaveHeader.CompanyID, SaveHeader.VoucherNumber, SaveHeader.CompanyTransactionKindNo, SaveHeader.TransactionKindNo, SaveHeader.CompanyYear,SaveHeader.StockCode);
                _unitOfWork.Complete();
                foreach (var SaveItems in ObjToUpdate.St_Transaction)
                {
                    if (!String.IsNullOrWhiteSpace(SaveItems.ItemCode))
                    {
                        try
                        {
                            iRow = iRow + 1;
                            SaveItems.CompanyID = UserInfo.fCompanyId;
                            SaveItems.CompanyYear = SaveHeader.CompanyYear;
                            SaveItems.CompanyTransactionKindNo = SaveHeader.CompanyTransactionKindNo;
                            SaveItems.TransactionKindNo = SaveHeader.TransactionKindNo;
                            SaveItems.StockCode = SaveHeader.StockCode;
                            SaveItems.VoucherDate = SaveHeader.VoucherDate;
                            SaveItems.VoucherNumber = SaveHeader.VoucherNumber;
                            SaveItems.VHI = SaveHeader.VHI;
                            SaveItems.IsDeleted = 0;
                            SaveItems.ItemCode = SaveItems.ItemCode;
                            SaveItems.SimilarItemCode = SaveItems.SimilarItemCode;
                            if (SaveItems.SimilarItemCode == "" || SaveItems.SimilarItemCode == null)
                            {
                                SaveItems.SimilarItemCode = SaveItems.ItemCode;
                            }
                            SaveItems.Quantity = SaveItems.Quantity;
                            SaveItems.Bonus = SaveItems.Bonus;
                            SaveItems.QuantityInputOutput = SaveItems.Quantity;
                            SaveItems.BonusInputOutput = SaveItems.Bonus;
                            SaveItems.TotalLocalBeforDiscount = SaveItems.TotalLocalBeforDiscount;
                            SaveItems.TotalLineDiscountLocal = SaveItems.TotalLineDiscountLocal;
                            SaveItems.TotalLocalAfterLineDiscount = SaveItems.TotalLocalAfterLineDiscount;
                            SaveItems.LineDiscountPercentage = SaveItems.LineDiscountPercentage;
                            SaveItems.TotalTaxAfterLineDiscountLocal = SaveItems.TotalTaxAfterLineDiscountLocal;
                            SaveItems.TotalAfterLineDiscountBeforDiscountAllLocal = SaveItems.TotalAfterLineDiscountBeforDiscountAllLocal;
                            SaveItems.TotalDiscountLocal = SaveItems.TotalDiscountLocal;
                            SaveItems.DiscountPercentage = SaveHeader.DiscountPercentage;
                            SaveItems.TotalLocalAfterDiscount = SaveItems.TotalLocalAfterDiscount;
                            SaveItems.TotalTaxLocal = SaveItems.TotalTaxLocal;
                            SaveItems.TotalLocal = SaveItems.TotalLocal;
                            SaveItems.TotalCostLocal = SaveItems.TotalCostLocal;
                            SaveItems.PricePieceLocalBeforDiscount = SaveItems.PricePieceLocalBeforDiscount;
                            SaveItems.PricePieceLineDiscountLocal = SaveItems.PricePieceLineDiscountLocal;
                            SaveItems.PricePieceLocalAfterLineDiscount = SaveItems.PricePieceLocalAfterLineDiscount;
                            SaveItems.PricePieceTaxAfterLineDiscountLocal = SaveItems.PricePieceTaxAfterLineDiscountLocal;
                            SaveItems.PricePieceAfterLineDiscountBeforDiscountAllLocal = SaveItems.PricePieceAfterLineDiscountBeforDiscountAllLocal;
                            SaveItems.PricePieceDiscountLocal = SaveItems.PricePieceDiscountLocal;
                            SaveItems.PricePieceLocalAfterDiscount = SaveItems.PricePieceLocalAfterDiscount;
                            SaveItems.PricePieceTaxLocal = SaveItems.PricePieceTaxLocal;
                            SaveItems.PricePieceTotalLocal = SaveItems.PricePieceTotalLocal;
                            SaveItems.CostPieceLocal = SaveItems.CostPieceLocal;
                            SaveItems.TaxRate = SaveItems.TaxRate;
                            SaveItems.TaxType = SaveHeader.TaxType;
                            SaveItems.RowNumber = iRow;
                            SaveItems.Remark = SaveHeader.Remark;
                            SaveItems.Hint = SaveHeader.Hint;
                            SaveItems.ExpierDate = (DateTime.Now).AddYears(30);
                            SaveItems.InsDateTime = DateTime.Now;
                            SaveItems.InsUserID = userId;
                            _unitOfWork.St_Header.AddTransaction(SaveItems);
                        }
                        catch (Exception ex)
                        {
                            Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                            Msg.Code = 0;
                            return Json(Msg, JsonRequestBehavior.AllowGet);

                        }
                    }
                }

                var Header = new Header();
                Header.TransactionKindNo = ObjToUpdate.TransactionKindNo;
                Header.CompanyTransactionKindNo = ObjToUpdate.CompanyTransactionKindNo;
                Header.InsDateTime = DateTime.Now;
                Header.InsUserID = userId;
                Header.CompanyID = UserInfo.fCompanyId;
                Header.TotalCredit = ObjToUpdate.NetTotalLocal;
                Header.TotalDebit = ObjToUpdate.NetTotalLocal;
                Header.TotalDebitForeign = 0;
                Header.TotalCreditForeign = 0;
                Header.CompanyYear = ObjToUpdate.CompanyYear;
                Header.VoucherNumber = SaveHeader.VoucherNumber;
                Header.VHI = SaveHeader.VHI;
                Header.Note = ObjToUpdate.Remark;
                Header.VoucherDate = ObjToUpdate.VoucherDate;
                Header.FCurrencyID = ObjToUpdate.CurrencyID;
                Header.ConversionFactor = ObjToUpdate.ConversionFactor;
                if (ObjToUpdate.TaxType != 1)
                {
                    Header.RowCount = 2;
                }
                else
                {
                    Header.RowCount = 3;
                }
                Header.SaleID = 0;
                _unitOfWork.Header.Add(Header);
                Transaction ObjCredit = new Transaction();
                ObjCredit.AccountNumber = ObjToUpdate.CreditAccountNumber;
                ObjCredit.CostCenter = ObjToUpdate.CreditCostNumber;
                ObjCredit.CompanyTransactionKindNo = ObjToUpdate.CompanyTransactionKindNo;
                ObjCredit.TransactionKindNo = ObjToUpdate.TransactionKindNo;
                ObjCredit.CompanyYear = ObjToUpdate.CompanyYear;
                ObjCredit.CompanyID = UserInfo.fCompanyId;
                ObjCredit.Debit = 0;
                ObjCredit.DebitForeign = 0;
                ObjCredit.Credit = ObjToUpdate.NetTotalLocal;
                ObjCredit.CreditForeign = 0;
                ObjCredit.CreditDebitForeign = 0;
                ObjCredit.Exported = 0;
                ObjCredit.InsDateTime = DateTime.Now;
                ObjCredit.InsUserID = userId;
                ObjCredit.Note = ObjToUpdate.Remark;
                ObjCredit.RowNumber = 1;
                ObjCredit.VHI = SaveHeader.VHI;
                ObjCredit.VoucherDate = ObjToUpdate.VoucherDate;
                ObjCredit.VoucherNumber = SaveHeader.VoucherNumber;

                Transaction ObjDebit = new Transaction();
                ObjDebit.AccountNumber = ObjToUpdate.DebitAccountNumber;
                ObjDebit.CostCenter = ObjToUpdate.DebitCostNumber;
                ObjDebit.CompanyTransactionKindNo = ObjToUpdate.CompanyTransactionKindNo;
                ObjDebit.TransactionKindNo = ObjToUpdate.TransactionKindNo;
                ObjDebit.CompanyYear = ObjToUpdate.CompanyYear;
                ObjDebit.CompanyID = UserInfo.fCompanyId;
                ObjDebit.Debit = ObjToUpdate.NetTotalLocalAfterDiscount;
                ObjDebit.DebitForeign = 0;
                ObjDebit.CreditDebitForeign = 0;
                ObjDebit.Credit = 0;
                ObjDebit.CreditForeign = 0;
                ObjDebit.Exported = 0;
                ObjDebit.InsDateTime = DateTime.Now;
                ObjDebit.InsUserID = userId;
                ObjDebit.Note = ObjToUpdate.Remark;
                ObjDebit.RowNumber = 2;
                ObjDebit.VHI = SaveHeader.VHI;
                ObjDebit.VoucherDate = ObjToUpdate.VoucherDate;
                ObjDebit.VoucherNumber = SaveHeader.VoucherNumber;

                _unitOfWork.Transaction.Add(ObjDebit);
                _unitOfWork.Transaction.Add(ObjCredit);

                if (ObjToUpdate.TaxType == 1)
                {
                    Transaction ObjTax = new Transaction();
                    ObjTax.AccountNumber = ObjToUpdate.TaxAccountNumber;
                    ObjTax.CostCenter = ObjToUpdate.TaxCostNumber;
                    ObjTax.CompanyTransactionKindNo = ObjToUpdate.CompanyTransactionKindNo;
                    ObjTax.TransactionKindNo = ObjToUpdate.TransactionKindNo;
                    ObjTax.CompanyYear = ObjToUpdate.CompanyYear;
                    ObjTax.CompanyID = UserInfo.fCompanyId;
                    ObjTax.Debit = ObjToUpdate.NetTotalTaxLocal;
                    ObjTax.DebitForeign = 0;
                    ObjTax.CreditDebitForeign = 0;
                    ObjTax.Credit = 0;
                    ObjTax.CreditForeign = 0;
                    ObjTax.Exported = 0;
                    ObjTax.InsDateTime = DateTime.Now;
                    ObjTax.InsUserID = userId;
                    ObjTax.Note = ObjToUpdate.Remark;
                    ObjTax.RowNumber = 3;
                    ObjTax.VHI = SaveHeader.VHI;
                    ObjTax.VoucherDate = ObjToUpdate.VoucherDate;
                    ObjTax.VoucherNumber = SaveHeader.VoucherNumber;
                    _unitOfWork.Transaction.Add(ObjTax);
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
                SaveHeader.RowCount = iRow;
                _unitOfWork.St_Header.AddHeader(SaveHeader);
                _unitOfWork.Complete();
                Msg.Code = 1;
                Msg.Msg = Resources.Resource.UpdatedSuccessfully;

                Msg.Year = SaveHeader.CompanyYear;
                Msg.FCompanyId = SaveHeader.CompanyID;
                Msg.VoucherNumber = SaveHeader.VoucherNumber;
                Msg.TransactionKindNo = SaveHeader.TransactionKindNo.ToString();
                Msg.CompanyTransactionKindNo = SaveHeader.CompanyTransactionKindNo.ToString();
                Msg.StockCode = SaveHeader.StockCode;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                Msg.Code = 0;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Copy(string id, int id2, int id3, int id4, string id5)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var St_Warehouse = _unitOfWork.St_Warehouse.GetAllSt_Warehouse(UserInfo.fCompanyId);
            var St_HeaderObj = _unitOfWork.St_Header.GetSt_HeaderDataById(id, UserInfo.fCompanyId, id2, id3, id4, id5);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var HeaderObj = _unitOfWork.Header.GetHeaderDataById(id, UserInfo.fCompanyId, id2, id3,id4);
            var TransactionCreditObj = _unitOfWork.NativeSql.GetTransactionsDetailCredit(id, UserInfo.fCompanyId, id2, 1, id4);
            var TransactionDebitObj = _unitOfWork.NativeSql.GetTransactionsDetailDebit(id, UserInfo.fCompanyId, id2, 2, id4);
            var TransactionTaxObj = _unitOfWork.NativeSql.GetTransactionsDetailTax(id, UserInfo.fCompanyId, id2, 3, id4);
            var ObjGet = _unitOfWork.St_OtherAccount.GetSt_OtherAccountByID(UserInfo.fCompanyId);
            FixAccountInfoVM ObjFix = new FixAccountInfoVM
            {
                AccountNo = "",
                AccountName = ""
            };
            if (ObjGet != null)
            {
                ObjFix.AccountNo = ObjGet.PurchaseTaxAccountNumber;
                ObjFix.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.PurchaseTaxAccountNumber);
            }
            var St_HeaderVM = new St_HeaderVM { };
            St_HeaderVM.St_Warehouse = St_Warehouse;
            St_HeaderVM.StockCode = St_HeaderObj.StockCode;
            St_HeaderVM.CompanyTransactionKindNo = St_HeaderObj.CompanyTransactionKindNo;
            St_HeaderVM.TransactionKindNo = St_HeaderObj.TransactionKindNo;
            St_HeaderVM.CompanyYear = St_HeaderObj.CompanyYear;
            St_HeaderVM.TaxType = St_HeaderObj.TaxType;
            St_HeaderVM.VoucherCase = St_HeaderObj.VoucherCase;
            St_HeaderVM.VoucherDate = St_HeaderObj.VoucherDate;
            St_HeaderVM.DueDate = St_HeaderObj.DueDate;
            St_HeaderVM.CurrencyID = St_HeaderObj.CurrencyID;
            St_HeaderVM.ConversionFactor = St_HeaderObj.ConversionFactor;
            St_HeaderVM.AccountNumber = St_HeaderObj.AccountNumber;
            St_HeaderVM.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, St_HeaderObj.AccountNumber);
            St_HeaderVM.OriginalVoucherNumber = St_HeaderObj.OriginalVoucherNumber;
            St_HeaderVM.OrderNumber = St_HeaderObj.OrderNumber;
            St_HeaderVM.NetTotalLocalBeforDiscount = St_HeaderObj.NetTotalLocalBeforDiscount;
            St_HeaderVM.NetTotalLineDiscountLocal = St_HeaderObj.NetTotalLineDiscountLocal;
            St_HeaderVM.NetTotalLocalAfterLineDiscount = St_HeaderObj.NetTotalLocalAfterLineDiscount;
            St_HeaderVM.NetTotalTaxAfterLineDiscountLocal = St_HeaderObj.NetTotalTaxAfterLineDiscountLocal;
            St_HeaderVM.NetTotalAfterLineDiscountBeforDiscountAllLocal = St_HeaderObj.NetTotalAfterLineDiscountBeforDiscountAllLocal;
            St_HeaderVM.NetTotalDiscountLocal = St_HeaderObj.NetTotalDiscountLocal;
            St_HeaderVM.DiscountPercentage = St_HeaderObj.DiscountPercentage;
            St_HeaderVM.NetTotalLocalAfterDiscount = St_HeaderObj.NetTotalLocalAfterDiscount;
            St_HeaderVM.NetTotalTaxLocal = St_HeaderObj.NetTotalTaxLocal;
            St_HeaderVM.NetTotalLocal = St_HeaderObj.NetTotalLocal;
            St_HeaderVM.Remark = St_HeaderObj.Remark;
            St_HeaderVM.Hint = St_HeaderObj.Hint;
            St_HeaderVM.VoucherNumber = St_HeaderObj.VoucherNumber;
            St_HeaderVM.VHI = St_HeaderObj.VHI;
            St_HeaderVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            St_HeaderVM.WorkWithCostCenter = Company.WorkWithCostCenter;
            St_HeaderVM.DebitAccountNumber = TransactionDebitObj.AccountNumber;
            St_HeaderVM.CreditAccountNumber = TransactionCreditObj.AccountNumber;
            St_HeaderVM.DebitCostNumber = TransactionDebitObj.CostCenter;
            St_HeaderVM.CreditCostNumber = TransactionCreditObj.CostCenter;
            St_HeaderVM.DebitAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionDebitObj.AccountNumber);
            St_HeaderVM.CreditAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionCreditObj.AccountNumber);
            St_HeaderVM.DebitCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionDebitObj.CostCenter);
            St_HeaderVM.CreditCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionCreditObj.CostCenter);
            St_HeaderVM.OrignailTaxAccountNumber = ObjFix.AccountNo;
            St_HeaderVM.OrignailTaxAccountName = ObjFix.AccountName;
            if (HeaderObj.RowCount > 2)
            {
                St_HeaderVM.TaxAccountNumber = TransactionTaxObj.AccountNumber;
                St_HeaderVM.TaxCostNumber = TransactionTaxObj.CostCenter;
                St_HeaderVM.TaxCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionTaxObj.CostCenter);
                St_HeaderVM.TaxAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionTaxObj.AccountNumber);
            }
            return View(St_HeaderVM);
        }
        public ActionResult Detail(string id, int id2, int id3, int id4, string id5)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var St_Warehouse = _unitOfWork.St_Warehouse.GetAllSt_Warehouse(UserInfo.fCompanyId);
            var St_HeaderObj = _unitOfWork.St_Header.GetSt_HeaderDataById(id, UserInfo.fCompanyId, id2, id3, id4, id5);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var HeaderObj = _unitOfWork.Header.GetHeaderDataById(id, UserInfo.fCompanyId, id2, id3, id4);
            var TransactionCreditObj = _unitOfWork.NativeSql.GetTransactionsDetailCredit(id, UserInfo.fCompanyId, id2, 1, id4);
            var TransactionDebitObj = _unitOfWork.NativeSql.GetTransactionsDetailDebit(id, UserInfo.fCompanyId, id2, 2, id4);
            var TransactionTaxObj = _unitOfWork.NativeSql.GetTransactionsDetailTax(id, UserInfo.fCompanyId, id2, 3, id4);
            var ObjGet = _unitOfWork.St_OtherAccount.GetSt_OtherAccountByID(UserInfo.fCompanyId);
            FixAccountInfoVM ObjFix = new FixAccountInfoVM
            {
                AccountNo = "",
                AccountName = ""
            };
            if (ObjGet != null)
            {
                ObjFix.AccountNo = ObjGet.PurchaseTaxAccountNumber;
                ObjFix.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.PurchaseTaxAccountNumber);
            }
            var St_HeaderVM = new St_HeaderVM { };
            St_HeaderVM.St_Warehouse = St_Warehouse;
            St_HeaderVM.StockCode = St_HeaderObj.StockCode;
            St_HeaderVM.CompanyTransactionKindNo = St_HeaderObj.CompanyTransactionKindNo;
            St_HeaderVM.TransactionKindNo = St_HeaderObj.TransactionKindNo;
            St_HeaderVM.CompanyYear = St_HeaderObj.CompanyYear;
            St_HeaderVM.TaxType = St_HeaderObj.TaxType;
            St_HeaderVM.VoucherCase = St_HeaderObj.VoucherCase;
            St_HeaderVM.VoucherDate = St_HeaderObj.VoucherDate;
            St_HeaderVM.DueDate = St_HeaderObj.DueDate;
            St_HeaderVM.CurrencyID = St_HeaderObj.CurrencyID;
            St_HeaderVM.ConversionFactor = St_HeaderObj.ConversionFactor;
            St_HeaderVM.AccountNumber = St_HeaderObj.AccountNumber;
            St_HeaderVM.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, St_HeaderObj.AccountNumber);
            St_HeaderVM.OriginalVoucherNumber = St_HeaderObj.OriginalVoucherNumber;
            St_HeaderVM.OrderNumber = St_HeaderObj.OrderNumber;
            St_HeaderVM.NetTotalLocalBeforDiscount = St_HeaderObj.NetTotalLocalBeforDiscount;
            St_HeaderVM.NetTotalLineDiscountLocal = St_HeaderObj.NetTotalLineDiscountLocal;
            St_HeaderVM.NetTotalLocalAfterLineDiscount = St_HeaderObj.NetTotalLocalAfterLineDiscount;
            St_HeaderVM.NetTotalTaxAfterLineDiscountLocal = St_HeaderObj.NetTotalTaxAfterLineDiscountLocal;
            St_HeaderVM.NetTotalAfterLineDiscountBeforDiscountAllLocal = St_HeaderObj.NetTotalAfterLineDiscountBeforDiscountAllLocal;
            St_HeaderVM.NetTotalDiscountLocal = St_HeaderObj.NetTotalDiscountLocal;
            St_HeaderVM.DiscountPercentage = St_HeaderObj.DiscountPercentage;
            St_HeaderVM.NetTotalLocalAfterDiscount = St_HeaderObj.NetTotalLocalAfterDiscount;
            St_HeaderVM.NetTotalTaxLocal = St_HeaderObj.NetTotalTaxLocal;
            St_HeaderVM.NetTotalLocal = St_HeaderObj.NetTotalLocal;
            St_HeaderVM.Remark = St_HeaderObj.Remark;
            St_HeaderVM.Hint = St_HeaderObj.Hint;
            St_HeaderVM.VoucherNumber = St_HeaderObj.VoucherNumber;
            St_HeaderVM.VHI = St_HeaderObj.VHI;
            St_HeaderVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            St_HeaderVM.WorkWithCostCenter = Company.WorkWithCostCenter;
            St_HeaderVM.DebitAccountNumber = TransactionDebitObj.AccountNumber;
            St_HeaderVM.CreditAccountNumber = TransactionCreditObj.AccountNumber;
            St_HeaderVM.DebitCostNumber = TransactionDebitObj.CostCenter;
            St_HeaderVM.CreditCostNumber = TransactionCreditObj.CostCenter;
            St_HeaderVM.DebitAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionDebitObj.AccountNumber);
            St_HeaderVM.CreditAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionCreditObj.AccountNumber);
            St_HeaderVM.DebitCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionDebitObj.CostCenter);
            St_HeaderVM.CreditCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionCreditObj.CostCenter);
            St_HeaderVM.OrignailTaxAccountNumber = ObjFix.AccountNo;
            St_HeaderVM.OrignailTaxAccountName = ObjFix.AccountName;
            if (HeaderObj.RowCount > 2)
            {
                St_HeaderVM.TaxAccountNumber = TransactionTaxObj.AccountNumber;
                St_HeaderVM.TaxCostNumber = TransactionTaxObj.CostCenter;
                St_HeaderVM.TaxCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionTaxObj.CostCenter);
                St_HeaderVM.TaxAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionTaxObj.AccountNumber);
            }
            return View(St_HeaderVM);
        }
        public ActionResult Delete(string id, int id2, int id3, int id4, string id5)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var St_Warehouse = _unitOfWork.St_Warehouse.GetAllSt_Warehouse(UserInfo.fCompanyId);
            var St_HeaderObj = _unitOfWork.St_Header.GetSt_HeaderDataById(id, UserInfo.fCompanyId, id2, id3, id4, id5);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var HeaderObj = _unitOfWork.Header.GetHeaderDataById(id, UserInfo.fCompanyId, id2, id3, id4);
            var TransactionCreditObj = _unitOfWork.NativeSql.GetTransactionsDetailCredit(id, UserInfo.fCompanyId, id2, 1, id4);
            var TransactionDebitObj = _unitOfWork.NativeSql.GetTransactionsDetailDebit(id, UserInfo.fCompanyId, id2, 2, id4);
            var TransactionTaxObj = _unitOfWork.NativeSql.GetTransactionsDetailTax(id, UserInfo.fCompanyId, id2, 3, id4);
            var ObjGet = _unitOfWork.St_OtherAccount.GetSt_OtherAccountByID(UserInfo.fCompanyId);
            FixAccountInfoVM ObjFix = new FixAccountInfoVM
            {
                AccountNo = "",
                AccountName = ""
            };
            if (ObjGet != null)
            {
                ObjFix.AccountNo = ObjGet.PurchaseTaxAccountNumber;
                ObjFix.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.PurchaseTaxAccountNumber);
            }
            var St_HeaderVM = new St_HeaderVM { };
            St_HeaderVM.St_Warehouse = St_Warehouse;
            St_HeaderVM.StockCode = St_HeaderObj.StockCode;
            St_HeaderVM.CompanyTransactionKindNo = St_HeaderObj.CompanyTransactionKindNo;
            St_HeaderVM.TransactionKindNo = St_HeaderObj.TransactionKindNo;
            St_HeaderVM.CompanyYear = St_HeaderObj.CompanyYear;
            St_HeaderVM.TaxType = St_HeaderObj.TaxType;
            St_HeaderVM.VoucherCase = St_HeaderObj.VoucherCase;
            St_HeaderVM.VoucherDate = St_HeaderObj.VoucherDate;
            St_HeaderVM.DueDate = St_HeaderObj.DueDate;
            St_HeaderVM.CurrencyID = St_HeaderObj.CurrencyID;
            St_HeaderVM.ConversionFactor = St_HeaderObj.ConversionFactor;
            St_HeaderVM.AccountNumber = St_HeaderObj.AccountNumber;
            St_HeaderVM.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, St_HeaderObj.AccountNumber);
            St_HeaderVM.OriginalVoucherNumber = St_HeaderObj.OriginalVoucherNumber;
            St_HeaderVM.OrderNumber = St_HeaderObj.OrderNumber;
            St_HeaderVM.NetTotalLocalBeforDiscount = St_HeaderObj.NetTotalLocalBeforDiscount;
            St_HeaderVM.NetTotalLineDiscountLocal = St_HeaderObj.NetTotalLineDiscountLocal;
            St_HeaderVM.NetTotalLocalAfterLineDiscount = St_HeaderObj.NetTotalLocalAfterLineDiscount;
            St_HeaderVM.NetTotalTaxAfterLineDiscountLocal = St_HeaderObj.NetTotalTaxAfterLineDiscountLocal;
            St_HeaderVM.NetTotalAfterLineDiscountBeforDiscountAllLocal = St_HeaderObj.NetTotalAfterLineDiscountBeforDiscountAllLocal;
            St_HeaderVM.NetTotalDiscountLocal = St_HeaderObj.NetTotalDiscountLocal;
            St_HeaderVM.DiscountPercentage = St_HeaderObj.DiscountPercentage;
            St_HeaderVM.NetTotalLocalAfterDiscount = St_HeaderObj.NetTotalLocalAfterDiscount;
            St_HeaderVM.NetTotalTaxLocal = St_HeaderObj.NetTotalTaxLocal;
            St_HeaderVM.NetTotalLocal = St_HeaderObj.NetTotalLocal;
            St_HeaderVM.Remark = St_HeaderObj.Remark;
            St_HeaderVM.Hint = St_HeaderObj.Hint;
            St_HeaderVM.VoucherNumber = St_HeaderObj.VoucherNumber;
            St_HeaderVM.VHI = St_HeaderObj.VHI;
            St_HeaderVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            St_HeaderVM.WorkWithCostCenter = Company.WorkWithCostCenter;
            St_HeaderVM.DebitAccountNumber = TransactionDebitObj.AccountNumber;
            St_HeaderVM.CreditAccountNumber = TransactionCreditObj.AccountNumber;
            St_HeaderVM.DebitCostNumber = TransactionDebitObj.CostCenter;
            St_HeaderVM.CreditCostNumber = TransactionCreditObj.CostCenter;
            St_HeaderVM.DebitAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionDebitObj.AccountNumber);
            St_HeaderVM.CreditAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionCreditObj.AccountNumber);
            St_HeaderVM.DebitCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionDebitObj.CostCenter);
            St_HeaderVM.CreditCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionCreditObj.CostCenter);
            St_HeaderVM.OrignailTaxAccountNumber = ObjFix.AccountNo;
            St_HeaderVM.OrignailTaxAccountName = ObjFix.AccountName;
            if (HeaderObj.RowCount > 2)
            {
                St_HeaderVM.TaxAccountNumber = TransactionTaxObj.AccountNumber;
                St_HeaderVM.TaxCostNumber = TransactionTaxObj.CostCenter;
                St_HeaderVM.TaxCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionTaxObj.CostCenter);
                St_HeaderVM.TaxAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionTaxObj.AccountNumber);
            }
            return View(St_HeaderVM);
        }
        [HttpGet]
        public JsonResult GetItems(string id, string id2, string id3, string id4, string id5)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var AllItems = _unitOfWork.NativeSql.GetSt_Transacation(id, int.Parse(id2), UserInfo.fCompanyId, int.Parse(id3), int.Parse(id4), id5);

            if (AllItems == null)
            {
                return Json(new St_HeaderVM(), JsonRequestBehavior.AllowGet);
            }
            return Json(AllItems, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteSt_PurchaseVoucher(St_HeaderVM ObjToDelete)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                var DeleteHeader = new St_Header();
                DeleteHeader.CompanyTransactionKindNo = ObjToDelete.CompanyTransactionKindNo;
                DeleteHeader.TransactionKindNo = ObjToDelete.TransactionKindNo;
                DeleteHeader.CompanyID = UserInfo.fCompanyId;
                DeleteHeader.CompanyYear = ObjToDelete.CompanyYear;
                DeleteHeader.VoucherNumber = ObjToDelete.VoucherNumber;
                DeleteHeader.StockCode = ObjToDelete.StockCode;
                var TransActionDeleteVM = new TransActionDeleteVM();
                TransActionDeleteVM.CompanyYear = DeleteHeader.CompanyYear;
                TransActionDeleteVM.CompanyID = DeleteHeader.CompanyID;
                TransActionDeleteVM.VoucherNumber = DeleteHeader.VoucherNumber;
                TransActionDeleteVM.CompanyTransactionKindNo = DeleteHeader.CompanyTransactionKindNo;
                TransActionDeleteVM.TransactionKindNo = DeleteHeader.TransactionKindNo;
                TransActionDeleteVM.VoucherNumber = DeleteHeader.VoucherNumber;
                _unitOfWork.Header.Delete(TransActionDeleteVM);
                _unitOfWork.St_Header.DeleteHeader(DeleteHeader);
                _unitOfWork.NativeSql.DeleteTransActionTrans(DeleteHeader.CompanyID, DeleteHeader.VoucherNumber, DeleteHeader.CompanyTransactionKindNo, DeleteHeader.TransactionKindNo, DeleteHeader.CompanyYear);
                _unitOfWork.NativeSql.DeleteSt_Transaction(DeleteHeader.CompanyID, DeleteHeader.VoucherNumber, DeleteHeader.CompanyTransactionKindNo, DeleteHeader.TransactionKindNo, DeleteHeader.CompanyYear, DeleteHeader.StockCode);
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
        public ActionResult St_ItemOtherUnit(string id)
        {
            try
            {
                if (id != "")
                {
                    var userId = User.Identity.GetUserId();
                    var UserInfo = _unitOfWork.User.GetUserByID(userId);
                    var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                    if (UserInfo == null)
                    {
                        RedirectToAction("", "");
                    }
                    var AllItemOtherUnitObj = new St_ItemOtherUnitVM { };
                    AllItemOtherUnitObj.St_ItemUnit = _unitOfWork.NativeSql.GetAllSt_ItemOtherUnitByItem(id, UserInfo.fCompanyId);
                    AllItemOtherUnitObj.ItemCode = id;
                    AllItemOtherUnitObj.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
                    return PartialView("St_ItemOtherUnit", AllItemOtherUnitObj);
                }



                return PartialView("St_ItemOtherUnit", new St_ItemOtherUnitVM());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return View("Error");
            }
        }
        [HttpGet]
        public JsonResult GetSt_ItemOtherUnit(string id,int id2)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);

            var GetAllSt_ItemOtherUnitObj = _unitOfWork.NativeSql.GetSt_ItemOtherUnit(id, UserInfo.fCompanyId, id2);
            if (GetAllSt_ItemOtherUnitObj == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(GetAllSt_ItemOtherUnitObj, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Export()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            int year = DateTime.Now.Year;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var St_Warehouse = _unitOfWork.St_Warehouse.GetAllSt_Warehouse(UserInfo.fCompanyId);
            var St_HeaderVM = new St_HeaderVM
            {
                St_Warehouse = St_Warehouse,
                FromDate = new DateTime(year, 1, 1),
                ToDate = new DateTime(year, 12, 31),
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency
            };
            return View(St_HeaderVM);
        }
        [HttpPost]
        public JsonResult GetAllUnExport(St_HeaderVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllUnExport = _unitOfWork.NativeSql.GetAllSt_UnExport(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate, 501);
                if (AllUnExport == null)
                {
                    return Json(new List<St_HeaderVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.VoucherNumber))
                {
                    AllUnExport = AllUnExport.Where(m => m.VoucherNumber == Obj.VoucherNumber).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.StockCode))
                {
                    AllUnExport = AllUnExport.Where(m => m.StockCode == Obj.StockCode).ToList();
                }
                int iRow = 0;
                foreach (var iRowCount in AllUnExport)
                {
                    iRowCount.iRowTable = iRow;
                    iRow = iRow + 1;
                }
                return Json(AllUnExport, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<St_HeaderVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult UnExport()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            int year = DateTime.Now.Year;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var St_Warehouse = _unitOfWork.St_Warehouse.GetAllSt_Warehouse(UserInfo.fCompanyId);
            var St_HeaderVM = new St_HeaderVM
            {
                St_Warehouse = St_Warehouse,
                FromDate = new DateTime(year, 1, 1),
                ToDate = new DateTime(year, 12, 31),
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency
            };
            return View(St_HeaderVM);
        }
        [HttpPost]
        public JsonResult GetAllExport(St_HeaderVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllUnExport = _unitOfWork.NativeSql.GetAllSt_Export(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate, 501);
                if (AllUnExport == null)
                {
                    return Json(new List<St_HeaderVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.VoucherNumber))
                {
                    AllUnExport = AllUnExport.Where(m => m.VoucherNumber == Obj.VoucherNumber).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.StockCode))
                {
                    AllUnExport = AllUnExport.Where(m => m.StockCode == Obj.StockCode).ToList();
                }
                int iRow = 0;
                foreach (var iRowCount in AllUnExport)
                {
                    iRowCount.iRowTable = iRow;
                    iRow = iRow + 1;
                }
                return Json(AllUnExport, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<St_HeaderVM>(), JsonRequestBehavior.AllowGet);
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
                foreach (var UpdateSt_Header in ObjToUpdate.St_Header)
                {
                    UpdateSt_Header.Exported = 1;
                    UpdateSt_Header.CompanyID = UserInfo.fCompanyId;
                    _unitOfWork.St_Header.UpdateToExportAndUnExport(UpdateSt_Header);
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
                foreach (var UpdateSt_Header in ObjToUpdate.St_Header)
                {
                    UpdateSt_Header.Exported = 0;
                    UpdateSt_Header.CompanyID = UserInfo.fCompanyId;
                    _unitOfWork.St_Header.UpdateToExportAndUnExport(UpdateSt_Header);
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


        public ActionResult ShowAttach(int id, string id2, string id3, string id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            St_TransActionFileVM Obj = new St_TransActionFileVM
            {
                Year = id,
                CompanyId = UserInfo.fCompanyId,
                VoucherNumber = id2,
                TransactionKindNo = id4,
                CompanyTransactionKindNo = id3

            };
            return View("ShowAttach", Obj);

        }

    }
}