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
    public class St_WarehouseController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public St_WarehouseController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            int RegisterCase = _unitOfWork.NativeSql.GetSt_RegisterValueByRegisterID(UserInfo.fCompanyId,1);
            var St_WarehouseVM = new St_WarehouseVM
            {
                WorkWithCostCenter = Company.WorkWithCostCenter,
                InventoryType = RegisterCase
            };
            return View(St_WarehouseVM);
        }
        public ActionResult Add()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            int RegisterCase = _unitOfWork.NativeSql.GetSt_RegisterValueByRegisterID(UserInfo.fCompanyId, 1);
            St_WarehouseVM Obj = new St_WarehouseVM
            {
                WorkWithCostCenter = Company.WorkWithCostCenter,
                InventoryType = RegisterCase
            };
            return PartialView(Obj);
        }
        [HttpGet]
        public JsonResult GetAllSt_Warehouse()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllSt_Warehouse = _unitOfWork.NativeSql.GetAllSt_Warehouse(UserInfo.fCompanyId);
                if (AllSt_Warehouse == null)
                {
                    return Json(new List<St_Warehouse>(), JsonRequestBehavior.AllowGet);
                }
                int iRow = 0;
                foreach (var iRowCount in AllSt_Warehouse)
                {
                    iRowCount.iRowTable = iRow;
                    iRow = iRow + 1;
                }
                return Json(AllSt_Warehouse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<St_WarehouseVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult Save(St_WarehouseVM ObjSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var ObjSaveWarehouse = new St_Warehouse();
                ObjSaveWarehouse.InsDateTime = DateTime.Now;
                ObjSaveWarehouse.InsUserID = userId;
                ObjSaveWarehouse.CompanyID = UserInfo.fCompanyId;
                ObjSaveWarehouse.StockCode = ObjSave.StockCode;
                ObjSaveWarehouse.AccountNumber = ObjSave.AccountNumber;
                ObjSaveWarehouse.CostCenterNumber = ObjSave.CostCenterNumber;
                ObjSaveWarehouse.Telephone = ObjSave.Telephone;
                ObjSaveWarehouse.Address = ObjSave.Address;
                ObjSaveWarehouse.ArabicName = ObjSave.ArabicName;
                if (String.IsNullOrEmpty(ObjSave.EnglishName))
                {
                    ObjSaveWarehouse.EnglishName = ObjSaveWarehouse.ArabicName;
                }
                else
                {
                    ObjSaveWarehouse.EnglishName = ObjSave.EnglishName;
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

                var St_TransactionAllStockTransaction = _unitOfWork.NativeSql.GetSt_TransactionKindAllStockTransaction(UserInfo.fCompanyId);
                if (St_TransactionAllStockTransaction.Count() == 0)
                {
                    St_TransactionAllStockTransaction = _unitOfWork.NativeSql.GetSt_TransactionKind();
                    foreach (var SaveSt_TransactionAllStockTransaction in St_TransactionAllStockTransaction)
                    {
                        var St_CompanyTransationKindObj = new St_CompanyTransactionKind();
                        St_CompanyTransationKindObj.CompanyID = UserInfo.fCompanyId;
                        St_CompanyTransationKindObj.St_CompanyTransactionKindID = _unitOfWork.St_CompanyTransactionKind.GetMaxSerial(UserInfo.fCompanyId);
                        St_CompanyTransationKindObj.St_TransactionKindID = SaveSt_TransactionAllStockTransaction.St_TransactionKindID;
                        St_CompanyTransationKindObj.StockCode = "*";
                        St_CompanyTransationKindObj.AutoSerial = true;
                        St_CompanyTransationKindObj.SymbolSerial = false;
                        St_CompanyTransationKindObj.Symbol = "";
                        St_CompanyTransationKindObj.Serial = 0;
                        St_CompanyTransationKindObj.InsUserID = userId;
                        St_CompanyTransationKindObj.InsDateTime = DateTime.Now;
                        _unitOfWork.St_CompanyTransactionKind.Add(St_CompanyTransationKindObj);
                        _unitOfWork.Complete();
                    }
                }
                var St_TransactionAllWithoutStockTransaction = _unitOfWork.NativeSql.GetSt_TransactionKindWithoutAllStockTransaction();
                foreach (var SaveSt_TransactionWithoutAllStockTransaction in St_TransactionAllWithoutStockTransaction)
                {
                    var St_CompanyTransationKindObj = new St_CompanyTransactionKind();
                    St_CompanyTransationKindObj.CompanyID = UserInfo.fCompanyId;
                    St_CompanyTransationKindObj.St_CompanyTransactionKindID = _unitOfWork.St_CompanyTransactionKind.GetMaxSerial(UserInfo.fCompanyId);
                    St_CompanyTransationKindObj.St_TransactionKindID = SaveSt_TransactionWithoutAllStockTransaction.St_TransactionKindID;
                    St_CompanyTransationKindObj.StockCode = ObjSaveWarehouse.StockCode;
                    St_CompanyTransationKindObj.AutoSerial = true;
                    St_CompanyTransationKindObj.SymbolSerial = false;
                    St_CompanyTransationKindObj.Symbol = "";
                    St_CompanyTransationKindObj.Serial = 0;
                    St_CompanyTransationKindObj.InsUserID = userId;
                    St_CompanyTransationKindObj.InsDateTime = DateTime.Now;
                    _unitOfWork.St_CompanyTransactionKind.Add(St_CompanyTransationKindObj);
                    _unitOfWork.Complete();
                }
                _unitOfWork.St_Warehouse.Add(ObjSaveWarehouse);
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
        public ActionResult Update(string id)
        {
            try
            {
                if (id != "")
                {
                    var userId = User.Identity.GetUserId();
                    var UserInfo = _unitOfWork.User.GetUserByID(userId);
                    var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                    int RegisterCase = _unitOfWork.NativeSql.GetSt_RegisterValueByRegisterID(UserInfo.fCompanyId, 1);
                    if (UserInfo == null)
                    {
                        RedirectToAction("", "");
                    }

                    var Obj = _unitOfWork.St_Warehouse.GetSt_WarehouseByID(UserInfo.fCompanyId, id);
                    var St_WarehouseObj = new St_WarehouseVM { };
                    St_WarehouseObj.StockCode = Obj.StockCode;
                    St_WarehouseObj.ArabicName = Obj.ArabicName;
                    St_WarehouseObj.EnglishName = Obj.EnglishName;
                    St_WarehouseObj.Telephone = Obj.Telephone;
                    St_WarehouseObj.Address = Obj.Address;
                    St_WarehouseObj.AccountNumber = Obj.AccountNumber;
                    St_WarehouseObj.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, St_WarehouseObj.AccountNumber);
                    St_WarehouseObj.CostCenterNumber = Obj.CostCenterNumber;
                    St_WarehouseObj.CostCenterName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, St_WarehouseObj.CostCenterNumber);
                    St_WarehouseObj.WorkWithCostCenter = Company.WorkWithCostCenter;
                    St_WarehouseObj.InventoryType = RegisterCase;
                    return PartialView("Update", St_WarehouseObj);
                }
                return PartialView("Update", new St_WarehouseVM());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return View("Error");
            }
        }
        public ActionResult Delete(string id)
        {
            try
            {
                if (id != "")
                {
                    var userId = User.Identity.GetUserId();
                    var UserInfo = _unitOfWork.User.GetUserByID(userId);
                    var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                    int RegisterCase = _unitOfWork.NativeSql.GetSt_RegisterValueByRegisterID(UserInfo.fCompanyId, 1);
                    if (UserInfo == null)
                    {
                        RedirectToAction("", "");
                    }

                    var Obj = _unitOfWork.St_Warehouse.GetSt_WarehouseByID(UserInfo.fCompanyId, id);
                    var St_WarehouseObj = new St_WarehouseVM { };
                    St_WarehouseObj.StockCode = Obj.StockCode;
                    St_WarehouseObj.ArabicName = Obj.ArabicName;
                    St_WarehouseObj.EnglishName = Obj.EnglishName;
                    St_WarehouseObj.Telephone = Obj.Telephone;
                    St_WarehouseObj.Address = Obj.Address;
                    St_WarehouseObj.AccountNumber = Obj.AccountNumber;
                    St_WarehouseObj.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, St_WarehouseObj.AccountNumber);
                    St_WarehouseObj.CostCenterNumber = Obj.CostCenterNumber;
                    St_WarehouseObj.CostCenterName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, St_WarehouseObj.CostCenterNumber);
                    St_WarehouseObj.WorkWithCostCenter = Company.WorkWithCostCenter;
                    St_WarehouseObj.InventoryType = RegisterCase;
                    return PartialView("Delete", St_WarehouseObj);
                }
                return PartialView("Delete", new St_WarehouseVM());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return View("Error");
            }
        }
        [HttpPost]
        public JsonResult Update(St_WarehouseVM ObjUpdate)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var ObjUpdateWarehouse = new St_Warehouse();
                ObjUpdateWarehouse.InsDateTime = DateTime.Now;
                ObjUpdateWarehouse.InsUserID = userId;
                ObjUpdateWarehouse.CompanyID = UserInfo.fCompanyId;
                ObjUpdateWarehouse.StockCode = ObjUpdate.StockCode;
                ObjUpdateWarehouse.AccountNumber = ObjUpdate.AccountNumber;
                ObjUpdateWarehouse.CostCenterNumber = ObjUpdate.CostCenterNumber;
                ObjUpdateWarehouse.Telephone = ObjUpdate.Telephone;
                ObjUpdateWarehouse.Address = ObjUpdate.Address;
                ObjUpdateWarehouse.ArabicName = ObjUpdate.ArabicName;
                if (String.IsNullOrEmpty(ObjUpdate.EnglishName))
                {
                    ObjUpdateWarehouse.EnglishName = ObjUpdateWarehouse.ArabicName;
                }
                else
                {
                    ObjUpdateWarehouse.EnglishName = ObjUpdate.EnglishName;
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
                _unitOfWork.St_Warehouse.Update(ObjUpdateWarehouse);
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
        [HttpPost]
        public JsonResult Delete(St_WarehouseVM ObjDelete)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);

                ObjDelete.CompanyID = UserInfo.fCompanyId;
                var ObjDeleteSt_Warehouse = new St_Warehouse();
                ObjDeleteSt_Warehouse.CompanyID = UserInfo.fCompanyId;
                ObjDeleteSt_Warehouse.StockCode = ObjDelete.StockCode;
                var ObjDeleteSt_WarehouseAccount = new St_WarehouseAccount();
                ObjDeleteSt_Warehouse.CompanyID = UserInfo.fCompanyId;
                ObjDeleteSt_Warehouse.StockCode = ObjDelete.StockCode;
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
                _unitOfWork.NativeSql.DeleteSt_CompanyTransactionKind(ObjDeleteSt_Warehouse.CompanyID,ObjDeleteSt_Warehouse.StockCode);
                _unitOfWork.NativeSql.DeleteSt_ItemWarehouseByStockCode(ObjDeleteSt_Warehouse.CompanyID, ObjDeleteSt_Warehouse.StockCode);
                _unitOfWork.St_WarehouseAccount.Delete(ObjDeleteSt_WarehouseAccount);
                _unitOfWork.St_Warehouse.Delete(ObjDeleteSt_Warehouse);
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
        public JsonResult CheckIfStockCodeExisting(string id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            string StockCode = _unitOfWork.St_Warehouse.CheckIfStockCodeExisting(UserInfo.fCompanyId, id);
            return Json(StockCode, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveSt_ItemWarehouse()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var St_ItemWarehouseObj = new St_ItemCardVM
            {
                St_ItemUnit = _unitOfWork.St_ItemUnit.GetAllItemUnit(UserInfo.fCompanyId),
                St_CountryOfOrigin = _unitOfWork.St_CountryOfOrigin.GetAllSt_CountryOfOrigin(UserInfo.fCompanyId),
                St_ManufacturerCompany = _unitOfWork.St_ManufacturerCompany.GetAllSt_ManufacturerCompany(UserInfo.fCompanyId),
                St_Warehouse = _unitOfWork.St_Warehouse.GetAllSt_Warehouse(UserInfo.fCompanyId),
                St_DescriptionDetail1 = _unitOfWork.St_DescriptionDetail.GetSt_DescriptionDetailBySt_Description(UserInfo.fCompanyId, 1),
                St_DescriptionDetail2 = _unitOfWork.St_DescriptionDetail.GetSt_DescriptionDetailBySt_Description(UserInfo.fCompanyId, 2),
                St_DescriptionDetail3 = _unitOfWork.St_DescriptionDetail.GetSt_DescriptionDetailBySt_Description(UserInfo.fCompanyId, 3),
                St_DescriptionDetail4 = _unitOfWork.St_DescriptionDetail.GetSt_DescriptionDetailBySt_Description(UserInfo.fCompanyId, 4),
                St_DescriptionDetail5 = _unitOfWork.St_DescriptionDetail.GetSt_DescriptionDetailBySt_Description(UserInfo.fCompanyId, 5),
                St_DescriptionDetail6 = _unitOfWork.St_DescriptionDetail.GetSt_DescriptionDetailBySt_Description(UserInfo.fCompanyId, 6),
                St_DescriptionDetail7 = _unitOfWork.St_DescriptionDetail.GetSt_DescriptionDetailBySt_Description(UserInfo.fCompanyId, 7),
                St_DescriptionDetail8 = _unitOfWork.St_DescriptionDetail.GetSt_DescriptionDetailBySt_Description(UserInfo.fCompanyId, 8),
                St_DescriptionDetail9 = _unitOfWork.St_DescriptionDetail.GetSt_DescriptionDetailBySt_Description(UserInfo.fCompanyId, 9),
                St_DescriptionDetail10 = _unitOfWork.St_DescriptionDetail.GetSt_DescriptionDetailBySt_Description(UserInfo.fCompanyId, 10),
                St_DescriptionDetail11 = _unitOfWork.St_DescriptionDetail.GetSt_DescriptionDetailBySt_Description(UserInfo.fCompanyId, 11),
                St_DescriptionDetail12 = _unitOfWork.St_DescriptionDetail.GetSt_DescriptionDetailBySt_Description(UserInfo.fCompanyId, 12),
                St_DescriptionDetail13 = _unitOfWork.St_DescriptionDetail.GetSt_DescriptionDetailBySt_Description(UserInfo.fCompanyId, 13),
                St_DescriptionDetail14 = _unitOfWork.St_DescriptionDetail.GetSt_DescriptionDetailBySt_Description(UserInfo.fCompanyId, 14),
                St_DescriptionDetail15 = _unitOfWork.St_DescriptionDetail.GetSt_DescriptionDetailBySt_Description(UserInfo.fCompanyId, 15),
                Categorie_1Name = _unitOfWork.St_Description.GetSt_DescriptionNameByID(UserInfo.fCompanyId, 1),
                Categorie_2Name = _unitOfWork.St_Description.GetSt_DescriptionNameByID(UserInfo.fCompanyId, 2),
                Categorie_3Name = _unitOfWork.St_Description.GetSt_DescriptionNameByID(UserInfo.fCompanyId, 3),
                Categorie_4Name = _unitOfWork.St_Description.GetSt_DescriptionNameByID(UserInfo.fCompanyId, 4),
                Categorie_5Name = _unitOfWork.St_Description.GetSt_DescriptionNameByID(UserInfo.fCompanyId, 5),
                Categorie_6Name = _unitOfWork.St_Description.GetSt_DescriptionNameByID(UserInfo.fCompanyId, 6),
                Categorie_7Name = _unitOfWork.St_Description.GetSt_DescriptionNameByID(UserInfo.fCompanyId, 7),
                Categorie_8Name = _unitOfWork.St_Description.GetSt_DescriptionNameByID(UserInfo.fCompanyId, 8),
                Categorie_9Name = _unitOfWork.St_Description.GetSt_DescriptionNameByID(UserInfo.fCompanyId, 9),
                Categorie_10Name = _unitOfWork.St_Description.GetSt_DescriptionNameByID(UserInfo.fCompanyId, 10),
                Categorie_11Name = _unitOfWork.St_Description.GetSt_DescriptionNameByID(UserInfo.fCompanyId, 11),
                Categorie_12Name = _unitOfWork.St_Description.GetSt_DescriptionNameByID(UserInfo.fCompanyId, 12),
                Categorie_13Name = _unitOfWork.St_Description.GetSt_DescriptionNameByID(UserInfo.fCompanyId, 13),
                Categorie_14Name = _unitOfWork.St_Description.GetSt_DescriptionNameByID(UserInfo.fCompanyId, 14),
                Categorie_15Name = _unitOfWork.St_Description.GetSt_DescriptionNameByID(UserInfo.fCompanyId, 15),
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency
            };
            return View(St_ItemWarehouseObj);
        }
        [HttpPost]
        public JsonResult GetAllSt_ItemCardByWarehouse(St_ItemCardVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllSt_ItemCard = _unitOfWork.NativeSql.GetAllSt_ItemCardFilterByWarehouse(UserInfo.fCompanyId, Obj.ItemCode,Obj.StockCode);
                if (AllSt_ItemCard == null)
                {
                    return Json(new List<St_ItemCardVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.ItemName))
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.ItemName.Contains(Obj.ItemName)).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.SupplierAccountNumber))
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.SupplierAccountNumber == Obj.SupplierAccountNumber).ToList();
                }
                if (Obj.ItemUnitNo != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.ItemUnitNo == Obj.ItemUnitNo).ToList();
                }
                if (Obj.CountryOfOriginNo != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.CountryOfOriginNo == Obj.CountryOfOriginNo).ToList();
                }
                if (Obj.ManufacturerCompanyNo != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.ManufacturerCompanyNo == Obj.ManufacturerCompanyNo).ToList();
                }
                if (Obj.ItemCaseInt != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.StopItem == Convert.ToBoolean(Obj.ItemCaseInt - 1)).ToList();
                }
                if (Obj.ItemNatureNo != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.ItemNatureNo == Obj.ItemNatureNo).ToList();
                }
                if (Obj.Categorie_1 != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.Categorie_1 == Obj.Categorie_1).ToList();
                }
                if (Obj.Categorie_2 != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.Categorie_2 == Obj.Categorie_2).ToList();
                }
                if (Obj.Categorie_3 != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.Categorie_3 == Obj.Categorie_3).ToList();
                }
                if (Obj.Categorie_4 != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.Categorie_4 == Obj.Categorie_4).ToList();
                }
                if (Obj.Categorie_5 != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.Categorie_5 == Obj.Categorie_5).ToList();
                }
                if (Obj.Categorie_6 != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.Categorie_6 == Obj.Categorie_6).ToList();
                }
                if (Obj.Categorie_7 != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.Categorie_7 == Obj.Categorie_7).ToList();
                }
                if (Obj.Categorie_8 != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.Categorie_8 == Obj.Categorie_8).ToList();
                }
                if (Obj.Categorie_9 != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.Categorie_9 == Obj.Categorie_9).ToList();
                }
                if (Obj.Categorie_10 != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.Categorie_10 == Obj.Categorie_10).ToList();
                }
                if (Obj.Categorie_11 != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.Categorie_11 == Obj.Categorie_11).ToList();
                }
                if (Obj.Categorie_12 != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.Categorie_12 == Obj.Categorie_12).ToList();
                }
                if (Obj.Categorie_13 != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.Categorie_13 == Obj.Categorie_13).ToList();
                }
                if (Obj.Categorie_14 != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.Categorie_14 == Obj.Categorie_14).ToList();
                }
                if (Obj.Categorie_15 != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.Categorie_15 == Obj.Categorie_15).ToList();
                }
                int iRow = 0;
                foreach (var iRowCount in AllSt_ItemCard)
                {
                    iRowCount.iRowTable = iRow;
                    iRow = iRow + 1;
                }
                return Json(AllSt_ItemCard, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<St_ItemCardVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public JsonResult SaveSt_ItemWarehouse(St_ItemCardVM ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                _unitOfWork.NativeSql.DeleteSt_ItemWarehouseByStockCode(UserInfo.fCompanyId, ObjToSave.StockCode);

                var iRow = 0;
                if (ObjToSave.St_ItemWarehouse != null)
                {
                    foreach (var SaveItemWarehouse in ObjToSave.St_ItemWarehouse)
                    {
                        iRow = iRow + 1;
                        SaveItemWarehouse.CompanyID = UserInfo.fCompanyId;
                        SaveItemWarehouse.ItemCode = SaveItemWarehouse.ItemCode;
                        SaveItemWarehouse.StockCode = ObjToSave.StockCode;
                        SaveItemWarehouse.StockMinimumItemNo = SaveItemWarehouse.StockMinimumItemNo;
                        SaveItemWarehouse.StockMaximumItemNo = SaveItemWarehouse.StockMaximumItemNo;
                        SaveItemWarehouse.RowNumber = iRow;
                        _unitOfWork.St_ItemCard.AddItemWarehous(SaveItemWarehouse);
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
        public ActionResult DetermineWarehouseQuantities()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var St_ItemWarehouseObj = new St_ItemCardVM
            {
                St_ItemUnit = _unitOfWork.St_ItemUnit.GetAllItemUnit(UserInfo.fCompanyId),
                St_CountryOfOrigin = _unitOfWork.St_CountryOfOrigin.GetAllSt_CountryOfOrigin(UserInfo.fCompanyId),
                St_ManufacturerCompany = _unitOfWork.St_ManufacturerCompany.GetAllSt_ManufacturerCompany(UserInfo.fCompanyId),
                St_Warehouse = _unitOfWork.St_Warehouse.GetAllSt_Warehouse(UserInfo.fCompanyId),
                St_DescriptionDetail1 = _unitOfWork.St_DescriptionDetail.GetSt_DescriptionDetailBySt_Description(UserInfo.fCompanyId, 1),
                St_DescriptionDetail2 = _unitOfWork.St_DescriptionDetail.GetSt_DescriptionDetailBySt_Description(UserInfo.fCompanyId, 2),
                St_DescriptionDetail3 = _unitOfWork.St_DescriptionDetail.GetSt_DescriptionDetailBySt_Description(UserInfo.fCompanyId, 3),
                St_DescriptionDetail4 = _unitOfWork.St_DescriptionDetail.GetSt_DescriptionDetailBySt_Description(UserInfo.fCompanyId, 4),
                St_DescriptionDetail5 = _unitOfWork.St_DescriptionDetail.GetSt_DescriptionDetailBySt_Description(UserInfo.fCompanyId, 5),
                St_DescriptionDetail6 = _unitOfWork.St_DescriptionDetail.GetSt_DescriptionDetailBySt_Description(UserInfo.fCompanyId, 6),
                St_DescriptionDetail7 = _unitOfWork.St_DescriptionDetail.GetSt_DescriptionDetailBySt_Description(UserInfo.fCompanyId, 7),
                St_DescriptionDetail8 = _unitOfWork.St_DescriptionDetail.GetSt_DescriptionDetailBySt_Description(UserInfo.fCompanyId, 8),
                St_DescriptionDetail9 = _unitOfWork.St_DescriptionDetail.GetSt_DescriptionDetailBySt_Description(UserInfo.fCompanyId, 9),
                St_DescriptionDetail10 = _unitOfWork.St_DescriptionDetail.GetSt_DescriptionDetailBySt_Description(UserInfo.fCompanyId, 10),
                St_DescriptionDetail11 = _unitOfWork.St_DescriptionDetail.GetSt_DescriptionDetailBySt_Description(UserInfo.fCompanyId, 11),
                St_DescriptionDetail12 = _unitOfWork.St_DescriptionDetail.GetSt_DescriptionDetailBySt_Description(UserInfo.fCompanyId, 12),
                St_DescriptionDetail13 = _unitOfWork.St_DescriptionDetail.GetSt_DescriptionDetailBySt_Description(UserInfo.fCompanyId, 13),
                St_DescriptionDetail14 = _unitOfWork.St_DescriptionDetail.GetSt_DescriptionDetailBySt_Description(UserInfo.fCompanyId, 14),
                St_DescriptionDetail15 = _unitOfWork.St_DescriptionDetail.GetSt_DescriptionDetailBySt_Description(UserInfo.fCompanyId, 15),
                Categorie_1Name = _unitOfWork.St_Description.GetSt_DescriptionNameByID(UserInfo.fCompanyId, 1),
                Categorie_2Name = _unitOfWork.St_Description.GetSt_DescriptionNameByID(UserInfo.fCompanyId, 2),
                Categorie_3Name = _unitOfWork.St_Description.GetSt_DescriptionNameByID(UserInfo.fCompanyId, 3),
                Categorie_4Name = _unitOfWork.St_Description.GetSt_DescriptionNameByID(UserInfo.fCompanyId, 4),
                Categorie_5Name = _unitOfWork.St_Description.GetSt_DescriptionNameByID(UserInfo.fCompanyId, 5),
                Categorie_6Name = _unitOfWork.St_Description.GetSt_DescriptionNameByID(UserInfo.fCompanyId, 6),
                Categorie_7Name = _unitOfWork.St_Description.GetSt_DescriptionNameByID(UserInfo.fCompanyId, 7),
                Categorie_8Name = _unitOfWork.St_Description.GetSt_DescriptionNameByID(UserInfo.fCompanyId, 8),
                Categorie_9Name = _unitOfWork.St_Description.GetSt_DescriptionNameByID(UserInfo.fCompanyId, 9),
                Categorie_10Name = _unitOfWork.St_Description.GetSt_DescriptionNameByID(UserInfo.fCompanyId, 10),
                Categorie_11Name = _unitOfWork.St_Description.GetSt_DescriptionNameByID(UserInfo.fCompanyId, 11),
                Categorie_12Name = _unitOfWork.St_Description.GetSt_DescriptionNameByID(UserInfo.fCompanyId, 12),
                Categorie_13Name = _unitOfWork.St_Description.GetSt_DescriptionNameByID(UserInfo.fCompanyId, 13),
                Categorie_14Name = _unitOfWork.St_Description.GetSt_DescriptionNameByID(UserInfo.fCompanyId, 14),
                Categorie_15Name = _unitOfWork.St_Description.GetSt_DescriptionNameByID(UserInfo.fCompanyId, 15),
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency
            };
            return View(St_ItemWarehouseObj);
        }
        [HttpPost]
        public JsonResult GetAllSt_ItemCardFilterByWarehouseToDetermineQuantity(St_ItemCardVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllSt_ItemCard = _unitOfWork.NativeSql.GetAllSt_ItemCardFilterByWarehouseToDetermineQuantity(UserInfo.fCompanyId, Obj.ItemCode, Obj.StockCode);
                if (AllSt_ItemCard == null)
                {
                    return Json(new List<St_ItemCardVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.ItemName))
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.ItemName.Contains(Obj.ItemName)).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.SupplierAccountNumber))
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.SupplierAccountNumber == Obj.SupplierAccountNumber).ToList();
                }
                if (Obj.ItemUnitNo != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.ItemUnitNo == Obj.ItemUnitNo).ToList();
                }
                if (Obj.CountryOfOriginNo != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.CountryOfOriginNo == Obj.CountryOfOriginNo).ToList();
                }
                if (Obj.ManufacturerCompanyNo != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.ManufacturerCompanyNo == Obj.ManufacturerCompanyNo).ToList();
                }
                if (Obj.ItemCaseInt != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.StopItem == Convert.ToBoolean(Obj.ItemCaseInt - 1)).ToList();
                }
                if (Obj.ItemNatureNo != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.ItemNatureNo == Obj.ItemNatureNo).ToList();
                }
                if (Obj.Categorie_1 != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.Categorie_1 == Obj.Categorie_1).ToList();
                }
                if (Obj.Categorie_2 != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.Categorie_2 == Obj.Categorie_2).ToList();
                }
                if (Obj.Categorie_3 != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.Categorie_3 == Obj.Categorie_3).ToList();
                }
                if (Obj.Categorie_4 != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.Categorie_4 == Obj.Categorie_4).ToList();
                }
                if (Obj.Categorie_5 != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.Categorie_5 == Obj.Categorie_5).ToList();
                }
                if (Obj.Categorie_6 != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.Categorie_6 == Obj.Categorie_6).ToList();
                }
                if (Obj.Categorie_7 != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.Categorie_7 == Obj.Categorie_7).ToList();
                }
                if (Obj.Categorie_8 != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.Categorie_8 == Obj.Categorie_8).ToList();
                }
                if (Obj.Categorie_9 != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.Categorie_9 == Obj.Categorie_9).ToList();
                }
                if (Obj.Categorie_10 != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.Categorie_10 == Obj.Categorie_10).ToList();
                }
                if (Obj.Categorie_11 != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.Categorie_11 == Obj.Categorie_11).ToList();
                }
                if (Obj.Categorie_12 != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.Categorie_12 == Obj.Categorie_12).ToList();
                }
                if (Obj.Categorie_13 != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.Categorie_13 == Obj.Categorie_13).ToList();
                }
                if (Obj.Categorie_14 != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.Categorie_14 == Obj.Categorie_14).ToList();
                }
                if (Obj.Categorie_15 != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.Categorie_15 == Obj.Categorie_15).ToList();
                }
                return Json(AllSt_ItemCard, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<St_ItemCardVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult UpdateDetermineWarehouseQuantities(string id,string id2)
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
                    var Obj = _unitOfWork.NativeSql.GetSt_ItemWarehouseByItemAndStock(UserInfo.fCompanyId, id,id2);
                    var St_ItemCardVM = new St_ItemCardVM { };
                    St_ItemCardVM.UpdateItemCode = Obj.ItemCode;
                    St_ItemCardVM.StockCode = Obj.StockCode;
                    St_ItemCardVM.ItemName = Obj.ItemName;
                    St_ItemCardVM.StockName = Obj.StockName;
                    St_ItemCardVM.StockMaximumItemNo = Obj.StockMaximumItemNo;
                    St_ItemCardVM.StockMinimumItemNo = Obj.StockMinimumItemNo;
                    return PartialView("UpdateDetermineWarehouseQuantities", St_ItemCardVM);
                }
                return PartialView("UpdateDetermineWarehouseQuantities", new St_ItemCardVM());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return View("Error");
            }
        }
        [HttpPost]
        public JsonResult UpdateDetermineWarehouseQuantities(St_ItemCardVM ObjUpdate)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var ObjUpdateSt_ItemWarehouse = new St_ItemWarehouse();
                ObjUpdateSt_ItemWarehouse.CompanyID = UserInfo.fCompanyId;
                ObjUpdateSt_ItemWarehouse.StockCode = ObjUpdate.StockCode;
                ObjUpdateSt_ItemWarehouse.ItemCode = ObjUpdate.UpdateItemCode;
                ObjUpdateSt_ItemWarehouse.StockMinimumItemNo = ObjUpdate.StockMinimumItemNo;
                ObjUpdateSt_ItemWarehouse.StockMaximumItemNo = ObjUpdate.StockMaximumItemNo;
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
                _unitOfWork.St_ItemCard.UpdateItemWarehous(ObjUpdateSt_ItemWarehouse);
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
        public ActionResult UpdateAllDetermineWarehouseQuantities()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            St_ItemCardVM Obj = new St_ItemCardVM();
            return PartialView(Obj);
        }
        [HttpPost]
        public JsonResult UpdateAllDetermineWarehouseQuantities(St_ItemCardVM ObjUpdate)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                foreach (var ObjUpdateSt_ItemWarehouse in ObjUpdate.St_ItemWarehouse)
                {
                    ObjUpdateSt_ItemWarehouse.CompanyID = UserInfo.fCompanyId;
                    ObjUpdateSt_ItemWarehouse.StockCode = ObjUpdate.StockCode;
                    ObjUpdateSt_ItemWarehouse.ItemCode = ObjUpdateSt_ItemWarehouse.ItemCode;
                    ObjUpdateSt_ItemWarehouse.StockMinimumItemNo = ObjUpdate.StockMinimumItemNo;
                    ObjUpdateSt_ItemWarehouse.StockMaximumItemNo = ObjUpdate.StockMaximumItemNo;
                    _unitOfWork.St_ItemCard.UpdateItemWarehous(ObjUpdateSt_ItemWarehouse);
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
        public JsonResult CheckWarehouseBeforDelete(string id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            if (id != "")
            {
                string WarehouseCode = _unitOfWork.NativeSql.CheckWarehouseBeforDelete(UserInfo.fCompanyId, id);
                if (WarehouseCode == null)
                {
                    WarehouseCode = "";
                }
                return Json(WarehouseCode, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

    }
}