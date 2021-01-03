using Acc.Helpers;
using Acc.Models;
using Acc.Persistence;
using Acc.Repositories;
using Acc.ViewModels;
using DevExpress.DataAccess.Sql.DataApi;
using DevExpress.PivotGrid.Criteria;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Acc.Controllers
{
    [Authorize]
    public class St_PurchaseOrderLocalController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public St_PurchaseOrderLocalController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            int ComapnyTransactionKindNo = _unitOfWork.St_CompanyTransactionKind.GetSt_CompanyTransactionKindBySt_TransKindNo(UserInfo.fCompanyId, 513);
            int year = DateTime.Now.Year;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var St_PurchaseOrderVM = new St_PurchaseOrderVM
            {
                FromDate = new DateTime(year, 1, 1),
                ToDate = new DateTime(year, 12, 31),
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency
            };
            return View(St_PurchaseOrderVM);
        }
        [HttpPost]
        public JsonResult GetAllSt_PurchaseOrder(St_PurchaseOrderVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllGetAllSt_PurchaseOrder = _unitOfWork.NativeSql.GetAllGetAllSt_PurchaseOrder(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                if (AllGetAllSt_PurchaseOrder == null)
                {
                    return Json(new List<St_PurchaseOrderVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.VoucherNumber))
                {
                    AllGetAllSt_PurchaseOrder = AllGetAllSt_PurchaseOrder.Where(m => m.VoucherNumber == Obj.VoucherNumber).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.SupplierAccountNumber))
                {
                    AllGetAllSt_PurchaseOrder = AllGetAllSt_PurchaseOrder.Where(m => m.SupplierAccountNumber == Obj.SupplierAccountNumber).ToList();
                }
                if (Obj.TaxType != 0)
                {
                    AllGetAllSt_PurchaseOrder = AllGetAllSt_PurchaseOrder.Where(m => m.TaxType == Obj.TaxType).ToList();
                }
                if (Obj.VoucherCase != 0)
                {
                    AllGetAllSt_PurchaseOrder = AllGetAllSt_PurchaseOrder.Where(m => m.VoucherCase == Obj.VoucherCase).ToList();
                }
                if (Obj.LinkWithVoucher != 0)
                {
                    AllGetAllSt_PurchaseOrder = AllGetAllSt_PurchaseOrder.Where(m => m.LinkWithVoucher == Obj.LinkWithVoucher).ToList();
                }
                return Json(AllGetAllSt_PurchaseOrder, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<St_PurchaseOrderVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public JsonResult GetMaxVoucher(int id,int id2,string id3)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            if (id != 0)
            {
                var ObjComapnyTransactionKind = _unitOfWork.St_CompanyTransactionKind.GetSt_CompanyTransactionKindByIDAndStockCode(UserInfo.fCompanyId, id, id3);
                if (ObjComapnyTransactionKind.AutoSerial)
                {
                    var TransactionKindNo = _unitOfWork.St_CompanyTransactionKind.GetSt_TransKindForSt_CompanyTransactionKind(UserInfo.fCompanyId, id);
                    var VHI = _unitOfWork.St_PurchaseOrder.GetMaxVoucher(UserInfo.fCompanyId, id, TransactionKindNo, id2).ToString();
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
        public ActionResult Add()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            int ComapnyTransactionKindNo = _unitOfWork.St_CompanyTransactionKind.GetSt_CompanyTransactionKindBySt_TransKindNo(UserInfo.fCompanyId,513);
            St_PurchaseOrderVM Obj = new St_PurchaseOrderVM
            {
                CompanyTransactionKindNo = ComapnyTransactionKindNo,
                CompanyYear = UserInfo.CurrYear,
                TransactionKindNo = 513,
                CurrencyID = 1,
                ConversionFactor = 1,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                VoucherDate = DateTime.Now,
        };
            return View(Obj);
        }
        [ValidateInput(false)]
        public ActionResult GridViewItems(string id, string id2, string id3, string id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            if (!String.IsNullOrEmpty(id))
            {
                var St_PurchaseOrderTransactionObj = _unitOfWork.NativeSql.GetSt_PurchaseOrderTransactionLocal(id, int.Parse(id2), UserInfo.fCompanyId, int.Parse(id3), int.Parse(id4));
                return PartialView("GridViewItems", St_PurchaseOrderTransactionObj);
            }
            else
            {
                var St_PurchaseOrderTransactionObj = new List<St_PurchaseOrderVM>();
                return PartialView("GridViewItems", St_PurchaseOrderTransactionObj);
            }


        }
        public JsonResult Save(St_PurchaseOrderVM ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                var SaveHeader = new St_PurchaseOrderHeader();
                SaveHeader.InsDateTime = DateTime.Now;
                SaveHeader.InsUserID = userId;
                SaveHeader.CompanyTransactionKindNo = ObjToSave.CompanyTransactionKindNo;
                SaveHeader.TransactionKindNo = ObjToSave.TransactionKindNo;
                SaveHeader.CompanyID = UserInfo.fCompanyId;
                SaveHeader.CompanyYear = ObjToSave.CompanyYear;
                SaveHeader.TaxType = ObjToSave.TaxType;
                SaveHeader.VoucherCase = ObjToSave.VoucherCase;
                SaveHeader.VoucherDate = ObjToSave.VoucherDate;
                SaveHeader.CurrencyID = ObjToSave.CurrencyID;
                SaveHeader.ConversionFactor = ObjToSave.ConversionFactor;
                SaveHeader.SupplierAccountNumber = ObjToSave.SupplierAccountNumber;
                SaveHeader.TotalQuantity = ObjToSave.TotalQuantity;
                SaveHeader.NetTotal = ObjToSave.NetTotal;
                SaveHeader.Remark = ObjToSave.Remark;
                SaveHeader.Hint = ObjToSave.Hint;
                SaveHeader.LinkWithVoucher = 0;
                var ObjComapnyTransactionKind = _unitOfWork.St_CompanyTransactionKind.GetSt_CompanyTransactionKindByID(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo);
                if (ObjComapnyTransactionKind.AutoSerial)
                {
                    SaveHeader.VoucherNumber = _unitOfWork.St_PurchaseOrder.GetMaxVoucher(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo, SaveHeader.TransactionKindNo, ObjToSave.CompanyYear).ToString();
                    SaveHeader.VHI = int.Parse(SaveHeader.VoucherNumber);
                }
                else
                {
                    string SerialNumber = "";
                    int Serial = ObjComapnyTransactionKind.Serial;
                    string Symbol = ObjComapnyTransactionKind.Symbol;
                    int LengthLastSerial = _unitOfWork.St_CompanyTransactionKindSymbolSerial.GetMaxSerial(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo, "*").ToString().Length;
                    Serial = Serial - LengthLastSerial;
                    for (int i = 0; i <= Serial; i++)
                    {
                        if (i < Serial)
                        {
                            SerialNumber = SerialNumber + "0";
                        }
                        else if (i == Serial)
                        {
                            SerialNumber = SerialNumber + _unitOfWork.St_CompanyTransactionKindSymbolSerial.GetMaxSerial(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo, "*").ToString();
                        }
                    }
                    SaveHeader.VoucherNumber = Symbol + SerialNumber;
                    SaveHeader.VHI = _unitOfWork.St_CompanyTransactionKindSymbolSerial.GetMaxSerial(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo,  "*");
                    _unitOfWork.St_CompanyTransactionKindSymbolSerial.Update(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo, "*");
                }
                int iRow = 0;
                foreach (var SaveItems in ObjToSave.St_PurchaseOrderTransaction)
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
                            SaveItems.VoucherDate = SaveHeader.VoucherDate;
                            SaveItems.VoucherNumber = SaveHeader.VoucherNumber;
                            SaveItems.VHI = SaveHeader.VHI;
                            SaveItems.IsDeleted = 0;
                            SaveItems.ItemCode = SaveItems.ItemCode;
                            SaveItems.SimilarItemCode = SaveItems.SimilarItemCode;
                            SaveItems.Quantity = SaveItems.Quantity;
                            SaveItems.Price = SaveItems.Price;
                            SaveItems.Total = SaveItems.Quantity * SaveItems.Price;
                            SaveItems.LinkVoucherNumber = "";
                            SaveItems.LinkWithVoucher = 0;
                            SaveItems.UsingQuantity =0;
                            SaveItems.RowNumber = iRow;
                            SaveItems.InsDateTime = DateTime.Now;
                            SaveItems.InsUserID = userId;
                            _unitOfWork.St_PurchaseOrder.AddSt_PurchaseOrderTransaction(SaveItems);
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
                SaveHeader.RowCount = iRow;
                _unitOfWork.St_PurchaseOrder.AddSt_PurchaseOrderHeader(SaveHeader);
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
        public ActionResult Update(string id, int id2, int id3, int id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            var HeaderObj = _unitOfWork.St_PurchaseOrder.GetSt_PurchaseOrderHeaderDataById(id, UserInfo.fCompanyId, id2, id3, id4);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var St_PurchaseOrderVM = new St_PurchaseOrderVM { };
            St_PurchaseOrderVM.CompanyTransactionKindNo = HeaderObj.CompanyTransactionKindNo;
            St_PurchaseOrderVM.TransactionKindNo = HeaderObj.TransactionKindNo;
            St_PurchaseOrderVM.CompanyYear = HeaderObj.CompanyYear;
            St_PurchaseOrderVM.TaxType = HeaderObj.TaxType;
            St_PurchaseOrderVM.VoucherCase = HeaderObj.VoucherCase;
            St_PurchaseOrderVM.VoucherDate = HeaderObj.VoucherDate;
            St_PurchaseOrderVM.CurrencyID = HeaderObj.CurrencyID;
            St_PurchaseOrderVM.ConversionFactor = HeaderObj.ConversionFactor;
            St_PurchaseOrderVM.SupplierAccountNumber = HeaderObj.SupplierAccountNumber;
            St_PurchaseOrderVM.TotalQuantity = HeaderObj.TotalQuantity;
            St_PurchaseOrderVM.NetTotal = HeaderObj.NetTotal;
            St_PurchaseOrderVM.SupplierAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, HeaderObj.SupplierAccountNumber);
            St_PurchaseOrderVM.Remark = HeaderObj.Remark;
            St_PurchaseOrderVM.Hint = HeaderObj.Hint;
            St_PurchaseOrderVM.VoucherNumber = HeaderObj.VoucherNumber;
            St_PurchaseOrderVM.VHI = HeaderObj.VHI;
            St_PurchaseOrderVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            return View(St_PurchaseOrderVM);
        }
        public JsonResult UpdateSt_PurchaseOrder(St_PurchaseOrderVM ObjToUpdate)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                var UpdateHeader = new St_PurchaseOrderHeader();
                UpdateHeader.InsDateTime = DateTime.Now;
                UpdateHeader.InsUserID = userId;
                UpdateHeader.CompanyTransactionKindNo = ObjToUpdate.CompanyTransactionKindNo;
                UpdateHeader.TransactionKindNo = ObjToUpdate.TransactionKindNo;
                UpdateHeader.CompanyID = UserInfo.fCompanyId;
                UpdateHeader.CompanyYear = ObjToUpdate.CompanyYear;
                UpdateHeader.TaxType = ObjToUpdate.TaxType;
                UpdateHeader.VoucherCase = ObjToUpdate.VoucherCase;
                UpdateHeader.VoucherDate = ObjToUpdate.VoucherDate;
                UpdateHeader.CurrencyID = ObjToUpdate.CurrencyID;
                UpdateHeader.ConversionFactor = ObjToUpdate.ConversionFactor;
                UpdateHeader.SupplierAccountNumber = ObjToUpdate.SupplierAccountNumber;
                UpdateHeader.TotalQuantity = ObjToUpdate.TotalQuantity;
                UpdateHeader.NetTotal = ObjToUpdate.NetTotal;
                UpdateHeader.Remark = ObjToUpdate.Remark;
                UpdateHeader.Hint = ObjToUpdate.Hint;
                UpdateHeader.LinkWithVoucher = 0;
                UpdateHeader.VoucherNumber = ObjToUpdate.VoucherNumber;
                UpdateHeader.VHI = ObjToUpdate.VHI;
                _unitOfWork.St_PurchaseOrder.DeleteSt_PurchaseOrderHeader(UpdateHeader);
                _unitOfWork.NativeSql.DeleteSt_PurchaseOrderTransactionLocal(ObjToUpdate.VoucherNumber, ObjToUpdate.CompanyTransactionKindNo, UserInfo.fCompanyId, 
                    ObjToUpdate.TransactionKindNo, ObjToUpdate.CompanyYear);
                _unitOfWork.Complete();
                int iRow = 0;
                foreach (var UpdateItems in ObjToUpdate.St_PurchaseOrderTransaction)
                {
                    if (!String.IsNullOrWhiteSpace(UpdateItems.ItemCode))
                    {
                        try
                        {
                            iRow = iRow + 1;
                            UpdateItems.CompanyID = UserInfo.fCompanyId;
                            UpdateItems.CompanyYear = UpdateHeader.CompanyYear;
                            UpdateItems.CompanyTransactionKindNo = UpdateHeader.CompanyTransactionKindNo;
                            UpdateItems.TransactionKindNo = UpdateHeader.TransactionKindNo;
                            UpdateItems.VoucherDate = UpdateHeader.VoucherDate;
                            UpdateItems.VoucherNumber = UpdateHeader.VoucherNumber;
                            UpdateItems.VHI = UpdateHeader.VHI;
                            UpdateItems.IsDeleted = 0;
                            UpdateItems.ItemCode = UpdateItems.ItemCode;
                            UpdateItems.SimilarItemCode = UpdateItems.SimilarItemCode;
                            UpdateItems.Quantity = UpdateItems.Quantity;
                            UpdateItems.Price = UpdateItems.Price;
                            UpdateItems.Total = UpdateItems.Price * UpdateItems.Quantity;
                            UpdateItems.LinkVoucherNumber = "";
                            UpdateItems.LinkWithVoucher = 0;
                            UpdateItems.UsingQuantity = 0;
                            UpdateItems.RowNumber = iRow;
                            UpdateItems.InsDateTime = DateTime.Now;
                            UpdateItems.InsUserID = userId;
                            _unitOfWork.St_PurchaseOrder.AddSt_PurchaseOrderTransaction(UpdateItems);
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
                UpdateHeader.RowCount = iRow;
                _unitOfWork.St_PurchaseOrder.AddSt_PurchaseOrderHeader(UpdateHeader);
                _unitOfWork.Complete();
                Msg.Code = 1;
                Msg.Msg = Resources.Resource.UpdatedSuccessfully;

                Msg.Year = UpdateHeader.CompanyYear;
                Msg.FCompanyId = UpdateHeader.CompanyID;
                Msg.VoucherNumber = UpdateHeader.VoucherNumber;
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
        public ActionResult Copy(string id, int id2, int id3, int id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            var HeaderObj = _unitOfWork.St_PurchaseOrder.GetSt_PurchaseOrderHeaderDataById(id, UserInfo.fCompanyId, id2, id3, id4);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var St_PurchaseOrderVM = new St_PurchaseOrderVM { };
            St_PurchaseOrderVM.CompanyTransactionKindNo = HeaderObj.CompanyTransactionKindNo;
            St_PurchaseOrderVM.TransactionKindNo = HeaderObj.TransactionKindNo;
            St_PurchaseOrderVM.CompanyYear = HeaderObj.CompanyYear;
            St_PurchaseOrderVM.TaxType = HeaderObj.TaxType;
            St_PurchaseOrderVM.VoucherCase = HeaderObj.VoucherCase;
            St_PurchaseOrderVM.VoucherDate = HeaderObj.VoucherDate;
            St_PurchaseOrderVM.CurrencyID = HeaderObj.CurrencyID;
            St_PurchaseOrderVM.ConversionFactor = HeaderObj.ConversionFactor;
            St_PurchaseOrderVM.SupplierAccountNumber = HeaderObj.SupplierAccountNumber;
            St_PurchaseOrderVM.TotalQuantity = HeaderObj.TotalQuantity;
            St_PurchaseOrderVM.NetTotal = HeaderObj.NetTotal;
            St_PurchaseOrderVM.SupplierAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, HeaderObj.SupplierAccountNumber);
            St_PurchaseOrderVM.Remark = HeaderObj.Remark;
            St_PurchaseOrderVM.Hint = HeaderObj.Hint;
            St_PurchaseOrderVM.VoucherNumber = HeaderObj.VoucherNumber;
            St_PurchaseOrderVM.VHI = HeaderObj.VHI;
            St_PurchaseOrderVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            return View(St_PurchaseOrderVM);
        }
        public ActionResult Detail(string id, int id2, int id3, int id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            var HeaderObj = _unitOfWork.St_PurchaseOrder.GetSt_PurchaseOrderHeaderDataById(id, UserInfo.fCompanyId, id2, id3, id4);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var St_PurchaseOrderVM = new St_PurchaseOrderVM { };
            St_PurchaseOrderVM.CompanyTransactionKindNo = HeaderObj.CompanyTransactionKindNo;
            St_PurchaseOrderVM.TransactionKindNo = HeaderObj.TransactionKindNo;
            St_PurchaseOrderVM.CompanyYear = HeaderObj.CompanyYear;
            St_PurchaseOrderVM.TaxType = HeaderObj.TaxType;
            St_PurchaseOrderVM.VoucherCase = HeaderObj.VoucherCase;
            St_PurchaseOrderVM.VoucherDate = HeaderObj.VoucherDate;
            St_PurchaseOrderVM.CurrencyID = HeaderObj.CurrencyID;
            St_PurchaseOrderVM.ConversionFactor = HeaderObj.ConversionFactor;
            St_PurchaseOrderVM.SupplierAccountNumber = HeaderObj.SupplierAccountNumber;
            St_PurchaseOrderVM.TotalQuantity = HeaderObj.TotalQuantity;
            St_PurchaseOrderVM.NetTotal = HeaderObj.NetTotal;
            St_PurchaseOrderVM.SupplierAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, HeaderObj.SupplierAccountNumber);
            St_PurchaseOrderVM.Remark = HeaderObj.Remark;
            St_PurchaseOrderVM.Hint = HeaderObj.Hint;
            St_PurchaseOrderVM.VoucherNumber = HeaderObj.VoucherNumber;
            St_PurchaseOrderVM.VHI = HeaderObj.VHI;
            St_PurchaseOrderVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            return View(St_PurchaseOrderVM);
        }
        public ActionResult Delete(string id, int id2, int id3, int id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            var HeaderObj = _unitOfWork.St_PurchaseOrder.GetSt_PurchaseOrderHeaderDataById(id, UserInfo.fCompanyId, id2, id3, id4);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var St_PurchaseOrderVM = new St_PurchaseOrderVM { };
            St_PurchaseOrderVM.CompanyTransactionKindNo = HeaderObj.CompanyTransactionKindNo;
            St_PurchaseOrderVM.TransactionKindNo = HeaderObj.TransactionKindNo;
            St_PurchaseOrderVM.CompanyYear = HeaderObj.CompanyYear;
            St_PurchaseOrderVM.TaxType = HeaderObj.TaxType;
            St_PurchaseOrderVM.VoucherCase = HeaderObj.VoucherCase;
            St_PurchaseOrderVM.VoucherDate = HeaderObj.VoucherDate;
            St_PurchaseOrderVM.CurrencyID = HeaderObj.CurrencyID;
            St_PurchaseOrderVM.ConversionFactor = HeaderObj.ConversionFactor;
            St_PurchaseOrderVM.SupplierAccountNumber = HeaderObj.SupplierAccountNumber;
            St_PurchaseOrderVM.TotalQuantity = HeaderObj.TotalQuantity;
            St_PurchaseOrderVM.NetTotal = HeaderObj.NetTotal;
            St_PurchaseOrderVM.SupplierAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, HeaderObj.SupplierAccountNumber);
            St_PurchaseOrderVM.Remark = HeaderObj.Remark;
            St_PurchaseOrderVM.Hint = HeaderObj.Hint;
            St_PurchaseOrderVM.VoucherNumber = HeaderObj.VoucherNumber;
            St_PurchaseOrderVM.VHI = HeaderObj.VHI;
            St_PurchaseOrderVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            return View(St_PurchaseOrderVM);
        }
        [HttpGet]
        public JsonResult GetItems(string id, string id2, string id3, string id4)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var AllItems = _unitOfWork.NativeSql.GetSt_PurchaseOrderTransactionLocal(id, int.Parse(id2), UserInfo.fCompanyId, int.Parse(id3), int.Parse(id4));

            if (AllItems == null)
            {
                return Json(new TransactionFixedVM(), JsonRequestBehavior.AllowGet);
            }
            return Json(AllItems, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteSt_PurchaseOrder(St_PurchaseOrderVM ObjToDelete)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                var DeleteHeader = new St_PurchaseOrderHeader();
                DeleteHeader.CompanyTransactionKindNo = ObjToDelete.CompanyTransactionKindNo;
                DeleteHeader.TransactionKindNo = ObjToDelete.TransactionKindNo;
                DeleteHeader.CompanyID = UserInfo.fCompanyId;
                DeleteHeader.CompanyYear = ObjToDelete.CompanyYear;
                DeleteHeader.VoucherNumber = ObjToDelete.VoucherNumber;
                _unitOfWork.St_PurchaseOrder.DeleteSt_PurchaseOrderHeader(DeleteHeader);
                _unitOfWork.NativeSql.DeleteSt_PurchaseOrderTransactionLocal(ObjToDelete.VoucherNumber, ObjToDelete.CompanyTransactionKindNo, UserInfo.fCompanyId,
                    ObjToDelete.TransactionKindNo, ObjToDelete.CompanyYear);
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
        public ActionResult Test()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            int ComapnyTransactionKindNo = _unitOfWork.St_CompanyTransactionKind.GetSt_CompanyTransactionKindBySt_TransKindNo(UserInfo.fCompanyId, 513);
            St_PurchaseOrderVM Obj = new St_PurchaseOrderVM
            {
                CompanyTransactionKindNo = ComapnyTransactionKindNo,
                CompanyYear = UserInfo.CurrYear,
                TransactionKindNo = 513,
                CurrencyID = 1,
                ConversionFactor = 1,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                VoucherDate = DateTime.Now,
            };
            return View(Obj);
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