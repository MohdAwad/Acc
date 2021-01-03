using Acc.Helpers;
using Acc.Models;
using Acc.Persistence;
using Acc.Repositories;
using Acc.ViewModels;
using DevExpress.XtraEditors.Filtering.Templates;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Acc.Controllers
{
    [Authorize]
    public class St_ItemCardHController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public St_ItemCardHController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        public ActionResult AttachGallary(string id)
        {
            St_ItemGallary Obj = new St_ItemGallary
            {
                ItemCode = id
            };
            
            return View(Obj);
        }
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var St_ItemCardHObj = new St_ItemCardHVM
            {
                St_ItemGroupH = _unitOfWork.St_ItemGroupH.GetAllSt_ItemGroupH(UserInfo.fCompanyId),
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency
            };
            return View(St_ItemCardHObj);
        }
        [HttpPost]
        public JsonResult GetAllSt_ItemCardH(St_ItemCardHVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllSt_ItemCardH = _unitOfWork.NativeSql.GetAllSt_ItemCardHFilter(UserInfo.fCompanyId);
                if (AllSt_ItemCardH == null)
                {
                    return Json(new List<St_ItemCardHVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.ItemCode))
                {
                    AllSt_ItemCardH = AllSt_ItemCardH.Where(m => m.ItemCode.Contains(Obj.ItemCode)).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.ItemName))
                {
                    AllSt_ItemCardH = AllSt_ItemCardH.Where(m => m.ItemName.Contains(Obj.ItemName)).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.SupplierAccountNumber))
                {
                    AllSt_ItemCardH = AllSt_ItemCardH.Where(m => m.SupplierAccountNumber == Obj.SupplierAccountNumber).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.GroupCode))
                {
                    AllSt_ItemCardH = AllSt_ItemCardH.Where(m => m.GroupCode == Obj.GroupCode).ToList();
                }
                if (Obj.ItemCaseInt != 0)
                {
                    AllSt_ItemCardH = AllSt_ItemCardH.Where(m => m.StopItem == Convert.ToBoolean(Obj.ItemCaseInt -1)).ToList();
                }
                if (Obj.ItemTypeNo != 0)
                {
                    AllSt_ItemCardH = AllSt_ItemCardH.Where(m => m.ItemTypeNo == Obj.ItemTypeNo).ToList();
                }
                return Json(AllSt_ItemCardH, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<St_ItemCardHVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult Add()
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var St_ItemCardHVMObj = new St_ItemCardHVM
            {
                St_FactoryH = _unitOfWork.St_FactoryH.GetAllSt_FactoryH(UserInfo.fCompanyId),
                St_ItemGroupH = _unitOfWork.St_ItemGroupH.GetAllSt_ItemGroupH(UserInfo.fCompanyId),
                St_ItemUnitH = _unitOfWork.St_ItemUnitH.GetAllSt_ItemUnitH(UserInfo.fCompanyId),
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency

            };
            return View(St_ItemCardHVMObj);
        }
        [ValidateInput(false)]
        public ActionResult GridViewRelatedItem(string id)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            if (!String.IsNullOrEmpty(id))
            {
                var ItemCardObj = _unitOfWork.NativeSql.GetRelatedItemByItemCardH(id,UserInfo.fCompanyId);
                return PartialView("GridViewRelatedItem", ItemCardObj);
            }
            else
            {
                var ItemCardObj = new List<St_ItemCardHVM>();
                return PartialView("GridViewRelatedItem", ItemCardObj);
            }


        }
        [ValidateInput(false)]
        public ActionResult GridViewSimilarItem(string id)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            if (!String.IsNullOrEmpty(id))
            {
                var ItemCardObj = _unitOfWork.NativeSql.GetSimilarItemByItemCardH(id, UserInfo.fCompanyId);
                return PartialView("GridViewSimilarItem", ItemCardObj);
            }
            else
            {
                var ItemCardObj = new List<St_ItemCardHVM>();
                return PartialView("GridViewSimilarItem", ItemCardObj);
            }


        }
        [ValidateInput(false)]
        public ActionResult GridViewManufacturingStage(string id)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            if (!String.IsNullOrEmpty(id))
            {
                var ItemCardObj = _unitOfWork.NativeSql.GetManufacturingStageByItemCardH(id, UserInfo.fCompanyId);
                return PartialView("GridViewManufacturingStage", ItemCardObj);
            }
            else
            {
                var ItemCardObj = new List<St_ItemCardHVM>();
                return PartialView("GridViewManufacturingStage", ItemCardObj);
            }


        }
        [ValidateInput(false)]
        public ActionResult GridViewRawMaterial(string id)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            if (!String.IsNullOrEmpty(id))
            {
                var ItemCardObj = _unitOfWork.NativeSql.GetRawMaterialByItemCardH(id, UserInfo.fCompanyId);
                return PartialView("GridViewRawMaterial", ItemCardObj);
            }
            else
            {
                var ItemCardObj = new List<St_ItemCardHVM>();
                return PartialView("GridViewRawMaterial", ItemCardObj);
            }


        }
        [HttpGet]
        public JsonResult CheckItemCodeH(string id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);

            var ItemCardInfo = _unitOfWork.NativeSql.CheckItemCodeH(UserInfo.fCompanyId, id).FirstOrDefault();
            if (ItemCardInfo == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(ItemCardInfo, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public JsonResult GetMaxItemCard(string id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var ItemSerial = _unitOfWork.St_ItemCardH.GetMaxSerial(UserInfo.fCompanyId, id).ToString();
            return Json(ItemSerial, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveSt_ItemCardH(St_ItemCardHVM ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var saveSt_ItemCard = new St_ItemCardH();
                saveSt_ItemCard.InsDateTime = DateTime.Now;
                saveSt_ItemCard.OpeningDate = DateTime.Now;
                saveSt_ItemCard.InsUserID = userId;
                saveSt_ItemCard.CompanyID = UserInfo.fCompanyId;
                saveSt_ItemCard.ArabicName = ObjToSave.ArabicName;
                if (String.IsNullOrEmpty(ObjToSave.EnglishName))
                {
                    saveSt_ItemCard.EnglishName = ObjToSave.ArabicName;
                }
                else
                {
                    saveSt_ItemCard.EnglishName = ObjToSave.EnglishName;
                }
                saveSt_ItemCard.GroupCode = ObjToSave.GroupCode;
                saveSt_ItemCard.NumberOfPieces = ObjToSave.NumberOfPieces;
                saveSt_ItemCard.CBM = ObjToSave.CBM;
                saveSt_ItemCard.ReOrderLimit = ObjToSave.ReOrderLimit;
                saveSt_ItemCard.OrderQuantity = ObjToSave.OrderQuantity;
                saveSt_ItemCard.ItemUnitNo = ObjToSave.ItemUnitNo;
                saveSt_ItemCard.SupplierAccountNumber = ObjToSave.SupplierAccountNumber;
                saveSt_ItemCard.StyleNo = ObjToSave.StyleNo;
                saveSt_ItemCard.ItemLevelNo = ObjToSave.ItemLevelNo;
                saveSt_ItemCard.StopItem = ObjToSave.StopItem;
                saveSt_ItemCard.ShowOnline = ObjToSave.ShowOnline;
                saveSt_ItemCard.QuantityOnline = ObjToSave.QuantityOnline;
                saveSt_ItemCard.SaleOfOfferedArticleIsPermitted = ObjToSave.SaleOfOfferedArticleIsPermitted;
                saveSt_ItemCard.ItemTypeNo = ObjToSave.ItemTypeNo;
                saveSt_ItemCard.FactoryNo = ObjToSave.FactoryNo;
                saveSt_ItemCard.NumberOfWorkingDays = ObjToSave.NumberOfWorkingDays;
                saveSt_ItemCard.NumberOfStages = ObjToSave.NumberOfStages;
                saveSt_ItemCard.WageRate = ObjToSave.WageRate;
                saveSt_ItemCard.ApprovingTheWarehouseQuantityLessThan = ObjToSave.ApprovingTheWarehouseQuantityLessThan;
                saveSt_ItemCard.QuantityManufacturing = ObjToSave.QuantityManufacturing;
                saveSt_ItemCard.FabricChangeIsAllowed = ObjToSave.FabricChangeIsAllowed;
                saveSt_ItemCard.AllowWoodToChangeColor = ObjToSave.AllowWoodToChangeColor;
                saveSt_ItemCard.FactoryNotes = ObjToSave.FactoryNotes;
                saveSt_ItemCard.SalePrice = ObjToSave.SalePrice;
                saveSt_ItemCard.TaxRate = ObjToSave.TaxRate;
                saveSt_ItemCard.TaxTypeNo = ObjToSave.TaxTypeNo;
                saveSt_ItemCard.LocalCost = ObjToSave.LocalCost;
                saveSt_ItemCard.ForeignCost = ObjToSave.ForeignCost;
                saveSt_ItemCard.TotalCost = ObjToSave.TotalCost;
                saveSt_ItemCard.TotalQuantity = ObjToSave.TotalQuantity;
                saveSt_ItemCard.NetTotal = ObjToSave.NetTotal;
                saveSt_ItemCard.TheTargetMonthlyAmount = ObjToSave.TheTargetMonthlyAmount;
                saveSt_ItemCard.ItemSerial = _unitOfWork.St_ItemCardH.GetMaxSerial(UserInfo.fCompanyId, saveSt_ItemCard.GroupCode);
                saveSt_ItemCard.ItemLogo = ObjToSave.ItemLogo;
                int SerialLength = 5 - saveSt_ItemCard.ItemSerial.ToString().Length;
                string SerialNumber = "";
                for (int i = 0; i <= SerialLength; i++)
                {
                    if (i < SerialLength)
                    {
                        SerialNumber = SerialNumber + "0";
                    }
                    else if (i == SerialLength)
                    {
                        SerialNumber = saveSt_ItemCard.GroupCode + SerialNumber + saveSt_ItemCard.ItemSerial.ToString();
                    }
                }
                saveSt_ItemCard.ItemCode = SerialNumber;
                if (ObjToSave.St_RelatedItemH != null)
                {
                    foreach (var SaveSt_RelatedItem in ObjToSave.St_RelatedItemH)
                    {
                        SaveSt_RelatedItem.CompanyID = UserInfo.fCompanyId;
                        SaveSt_RelatedItem.ItemCode = saveSt_ItemCard.ItemCode;
                        SaveSt_RelatedItem.RelatedItemCode = SaveSt_RelatedItem.RelatedItemCode;
                        SaveSt_RelatedItem.RowNumber = SaveSt_RelatedItem.RowNumber;
                        _unitOfWork.St_ItemCardH.AddRelatedItem(SaveSt_RelatedItem);
                    }
                }
                if (ObjToSave.St_SimilarItemH != null)
                {
                    foreach (var SaveSt_SimilarItem in ObjToSave.St_SimilarItemH)
                    {
                        SaveSt_SimilarItem.CompanyID = UserInfo.fCompanyId;
                        SaveSt_SimilarItem.ItemCode = saveSt_ItemCard.ItemCode;
                        SaveSt_SimilarItem.SimilarItemCode = SaveSt_SimilarItem.SimilarItemCode;
                        SaveSt_SimilarItem.RowNumber = SaveSt_SimilarItem.RowNumber;
                        _unitOfWork.St_ItemCardH.AddSimilarItem(SaveSt_SimilarItem);
                    }
                }
            
                if (saveSt_ItemCard.ItemTypeNo == 3)
                {
                    foreach (var SaveSt_ManufacturingStage in ObjToSave.St_ManufacturingStageH)
                    {
                        SaveSt_ManufacturingStage.CompanyID = UserInfo.fCompanyId;
                        SaveSt_ManufacturingStage.ItemCode = saveSt_ItemCard.ItemCode;
                        SaveSt_ManufacturingStage.FactoryNo = SaveSt_ManufacturingStage.FactoryNo;
                        SaveSt_ManufacturingStage.RowNumber = SaveSt_ManufacturingStage.RowNumber;
                        SaveSt_ManufacturingStage.NumberOfDays = SaveSt_ManufacturingStage.NumberOfDays;
                        _unitOfWork.St_ItemCardH.AddManufacturingStage(SaveSt_ManufacturingStage);
                    }
                    if (ObjToSave.St_RawMaterialH != null)
                    {
                        foreach (var SaveSt_RawMaterial in ObjToSave.St_RawMaterialH)
                        {
                            SaveSt_RawMaterial.CompanyID = UserInfo.fCompanyId;
                            SaveSt_RawMaterial.ItemCode = saveSt_ItemCard.ItemCode;
                            SaveSt_RawMaterial.RawMaterialCode = SaveSt_RawMaterial.RawMaterialCode;
                            SaveSt_RawMaterial.RowNumber = SaveSt_RawMaterial.RowNumber;
                            SaveSt_RawMaterial.Quantity = SaveSt_RawMaterial.Quantity;
                            SaveSt_RawMaterial.Cost = SaveSt_RawMaterial.Cost;
                            SaveSt_RawMaterial.Total = SaveSt_RawMaterial.Total;
                            _unitOfWork.St_ItemCardH.AddRawMaterial(SaveSt_RawMaterial);
                        }
                    }
                }
                int iRow = 0;
                if (saveSt_ItemCard.ItemTypeNo == 1)
                {
                    if (ObjToSave.St_SubColorsItemH != null) {
                        foreach (var SaveSubColorsItem in ObjToSave.St_SubColorsItemH)
                        {
                            iRow = iRow + 1;
                            SaveSubColorsItem.CompanyID = UserInfo.fCompanyId;
                            SaveSubColorsItem.ItemCode = saveSt_ItemCard.ItemCode;
                            SaveSubColorsItem.SubItemColorCode = SaveSubColorsItem.SubItemColorCode;
                            SaveSubColorsItem.RowNumber = iRow;
                            _unitOfWork.St_ItemCardH.AddSubColorsItem(SaveSubColorsItem);
                        }
                    }      
                }
                iRow = 0;
                if (ObjToSave.St_ItemWarehouseH != null)
                {
                    foreach (var SaveItemWarehouse in ObjToSave.St_ItemWarehouseH)
                    {
                        iRow = iRow + 1;
                        SaveItemWarehouse.CompanyID = UserInfo.fCompanyId;
                        SaveItemWarehouse.ItemCode = saveSt_ItemCard.ItemCode;
                        SaveItemWarehouse.StockCode = SaveItemWarehouse.StockCode;
                        SaveItemWarehouse.RowNumber = iRow;
                        _unitOfWork.St_ItemCardH.AddItemWarehous(SaveItemWarehouse);
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
                _unitOfWork.St_ItemCardH.AddItemCard(saveSt_ItemCard);
                _unitOfWork.Complete();
                Msg.Code = 1;
                Msg.Msg = Resources.Resource.AddedSuccessfully;
                Msg.LastID = saveSt_ItemCard.ItemCode;
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
        public JsonResult GetAllSt_WarehouseHByItemCode(string id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllSt_WarehouseH = _unitOfWork.NativeSql.GetAllSt_WarehouseHByItemCode(UserInfo.fCompanyId, id);
                if (AllSt_WarehouseH == null)
                {
                    return Json(new List<St_WarehouseHVM>(), JsonRequestBehavior.AllowGet);
                }
                int iRow = 0;
                foreach (var iRowCount in AllSt_WarehouseH)
                {
                    iRowCount.iRowTable = iRow;
                    iRow = iRow + 1;
                }
                return Json(AllSt_WarehouseH, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<St_WarehouseHVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public JsonResult GetAllSt_SubColorByItemCode(string id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllSt_SubItemColor = _unitOfWork.NativeSql.GetAllSt_SubColorHByItemCode(UserInfo.fCompanyId, id);
                if (AllSt_SubItemColor == null)
                {
                    return Json(new List<St_SubItemColorHVM>(), JsonRequestBehavior.AllowGet);
                }
                int iRow = 0;
                foreach (var iRowCount in AllSt_SubItemColor)
                {
                    iRowCount.iRowTable = iRow;
                    iRow = iRow + 1;
                }
                return Json(AllSt_SubItemColor, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<St_SubItemColorHVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult Update(string id)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var St_ItemCardObj = _unitOfWork.St_ItemCardH.GetSt_ItemCardById(id, UserInfo.fCompanyId);
            var St_ItemCardHVMObj = new St_ItemCardHVM
            {
                St_FactoryH = _unitOfWork.St_FactoryH.GetAllSt_FactoryH(UserInfo.fCompanyId),
                St_ItemGroupH = _unitOfWork.St_ItemGroupH.GetAllSt_ItemGroupH(UserInfo.fCompanyId),
                St_ItemUnitH = _unitOfWork.St_ItemUnitH.GetAllSt_ItemUnitH(UserInfo.fCompanyId),
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                ItemSerial = St_ItemCardObj.ItemSerial,
                ItemCode = St_ItemCardObj.ItemCode,
                GroupCode = St_ItemCardObj.GroupCode,
                ArabicName = St_ItemCardObj.ArabicName,
                EnglishName = St_ItemCardObj.EnglishName,
                NumberOfPieces = St_ItemCardObj.NumberOfPieces,
                ReOrderLimit = St_ItemCardObj.ReOrderLimit,
                OrderQuantity = St_ItemCardObj.OrderQuantity,
                CBM = St_ItemCardObj.CBM,
                ItemUnitNo = St_ItemCardObj.ItemUnitNo,
                StyleNo = St_ItemCardObj.StyleNo,
                ItemLevelNo = St_ItemCardObj.ItemLevelNo,
                ItemTypeNo = St_ItemCardObj.ItemTypeNo,
                SupplierAccountNumber = St_ItemCardObj.SupplierAccountNumber,
                SupplierAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, St_ItemCardObj.SupplierAccountNumber),
                ShowOnline = St_ItemCardObj.ShowOnline,
                QuantityOnline = St_ItemCardObj.QuantityOnline,
                StopItem = St_ItemCardObj.StopItem,
                SaleOfOfferedArticleIsPermitted = St_ItemCardObj.SaleOfOfferedArticleIsPermitted,
                FabricChangeIsAllowed = St_ItemCardObj.FabricChangeIsAllowed,
                AllowWoodToChangeColor = St_ItemCardObj.AllowWoodToChangeColor,
                ApprovingTheWarehouseQuantityLessThan = St_ItemCardObj.ApprovingTheWarehouseQuantityLessThan,
                QuantityManufacturing = St_ItemCardObj.QuantityManufacturing,
                SalePrice = St_ItemCardObj.SalePrice,
                TaxTypeNo = St_ItemCardObj.TaxTypeNo,
                TaxRate = St_ItemCardObj.TaxRate,
                LocalCost = St_ItemCardObj.LocalCost,
                ForeignCost = St_ItemCardObj.ForeignCost,
                CostRate = St_ItemCardObj.CostRate,
                TheTargetMonthlyAmount = St_ItemCardObj.TheTargetMonthlyAmount,
                TheNumberOfDaysTheCardIsOpened = _unitOfWork.NativeSql.GetOpeningDateH(UserInfo.fCompanyId, St_ItemCardObj.ItemCode),
                LastLocalPurchasePrice = 0,
                LastForeignPurchasePrice = 0,
                TotalQuantitySold = 0,
                TotalValueSold = 0,
                FactoryNo = St_ItemCardObj.FactoryNo,
                FactoryNotes = St_ItemCardObj.FactoryNotes,
                WageRate = St_ItemCardObj.WageRate,
                NumberOfWorkingDays = St_ItemCardObj.NumberOfWorkingDays,
                NumberOfStages = St_ItemCardObj.NumberOfStages,
                NetTotal = St_ItemCardObj.NetTotal,
                TotalQuantity = St_ItemCardObj.TotalQuantity,
                TotalCost = St_ItemCardObj.TotalCost,
                ItemLogo = St_ItemCardObj.ItemLogo
            };
            return View(St_ItemCardHVMObj);
        }
        [HttpPost]
        public JsonResult UpdateSt_ItemCardH(St_ItemCardHVM ObjToUpdate)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var updateSt_ItemCard = new St_ItemCardH();
                updateSt_ItemCard.InsDateTime = DateTime.Now;
                updateSt_ItemCard.OpeningDate = DateTime.Now;
                updateSt_ItemCard.InsUserID = userId;
                updateSt_ItemCard.CompanyID = UserInfo.fCompanyId;
                updateSt_ItemCard.ArabicName = ObjToUpdate.ArabicName;
                if (String.IsNullOrEmpty(ObjToUpdate.EnglishName))
                {
                    updateSt_ItemCard.EnglishName = ObjToUpdate.ArabicName;
                }
                else
                {
                    updateSt_ItemCard.EnglishName = ObjToUpdate.EnglishName;
                }
                updateSt_ItemCard.GroupCode = ObjToUpdate.GroupCode;
                updateSt_ItemCard.NumberOfPieces = ObjToUpdate.NumberOfPieces;
                updateSt_ItemCard.ReOrderLimit = ObjToUpdate.ReOrderLimit;
                updateSt_ItemCard.OrderQuantity = ObjToUpdate.OrderQuantity;
                updateSt_ItemCard.CBM = ObjToUpdate.CBM;
                updateSt_ItemCard.ItemUnitNo = ObjToUpdate.ItemUnitNo;
                updateSt_ItemCard.SupplierAccountNumber = ObjToUpdate.SupplierAccountNumber;
                updateSt_ItemCard.StyleNo = ObjToUpdate.StyleNo;
                updateSt_ItemCard.ItemLevelNo = ObjToUpdate.ItemLevelNo;
                updateSt_ItemCard.StopItem = ObjToUpdate.StopItem;
                updateSt_ItemCard.ShowOnline = ObjToUpdate.ShowOnline;
                updateSt_ItemCard.QuantityOnline = ObjToUpdate.QuantityOnline;
                updateSt_ItemCard.SaleOfOfferedArticleIsPermitted = ObjToUpdate.SaleOfOfferedArticleIsPermitted;
                updateSt_ItemCard.ItemTypeNo = ObjToUpdate.ItemTypeNo;
                updateSt_ItemCard.FactoryNo = ObjToUpdate.FactoryNo;
                updateSt_ItemCard.NumberOfWorkingDays = ObjToUpdate.NumberOfWorkingDays;
                updateSt_ItemCard.NumberOfStages = ObjToUpdate.NumberOfStages;
                updateSt_ItemCard.WageRate = ObjToUpdate.WageRate;
                updateSt_ItemCard.ApprovingTheWarehouseQuantityLessThan = ObjToUpdate.ApprovingTheWarehouseQuantityLessThan;
                updateSt_ItemCard.QuantityManufacturing = ObjToUpdate.QuantityManufacturing;
                updateSt_ItemCard.FabricChangeIsAllowed = ObjToUpdate.FabricChangeIsAllowed;
                updateSt_ItemCard.AllowWoodToChangeColor = ObjToUpdate.AllowWoodToChangeColor;
                updateSt_ItemCard.FactoryNotes = ObjToUpdate.FactoryNotes;
                updateSt_ItemCard.SalePrice = ObjToUpdate.SalePrice;
                updateSt_ItemCard.TaxRate = ObjToUpdate.TaxRate;
                updateSt_ItemCard.TaxTypeNo = ObjToUpdate.TaxTypeNo;
                updateSt_ItemCard.LocalCost = ObjToUpdate.LocalCost;
                updateSt_ItemCard.ForeignCost = ObjToUpdate.ForeignCost;
                updateSt_ItemCard.TotalCost = ObjToUpdate.TotalCost;
                updateSt_ItemCard.TotalQuantity = ObjToUpdate.TotalQuantity;
                updateSt_ItemCard.NetTotal = ObjToUpdate.NetTotal;
                updateSt_ItemCard.TheTargetMonthlyAmount = ObjToUpdate.TheTargetMonthlyAmount;
                updateSt_ItemCard.ItemSerial = ObjToUpdate.ItemSerial;
                updateSt_ItemCard.ItemLogo = ObjToUpdate.ItemLogo;
                updateSt_ItemCard.ItemCode = ObjToUpdate.ItemCode;

                _unitOfWork.NativeSql.DeleteSt_ItemCardH(UserInfo.fCompanyId, updateSt_ItemCard.ItemCode);
                _unitOfWork.NativeSql.DeleteSt_SimilarItemH(UserInfo.fCompanyId, updateSt_ItemCard.ItemCode);
                _unitOfWork.NativeSql.DeleteSt_RelatedItemH(UserInfo.fCompanyId, updateSt_ItemCard.ItemCode);
                _unitOfWork.NativeSql.DeleteSt_ManufacturingStageH(UserInfo.fCompanyId, updateSt_ItemCard.ItemCode);
                _unitOfWork.NativeSql.DeleteSt_RawMaterialH(UserInfo.fCompanyId, updateSt_ItemCard.ItemCode);
                _unitOfWork.NativeSql.DeleteSt_SubColorsItemH(UserInfo.fCompanyId, updateSt_ItemCard.ItemCode);
                _unitOfWork.NativeSql.DeleteSt_ItemWarehouseH(UserInfo.fCompanyId, updateSt_ItemCard.ItemCode);
                _unitOfWork.Complete();

                if (ObjToUpdate.St_RelatedItemH != null)
                {
                    foreach (var UpdateSt_RelatedItem in ObjToUpdate.St_RelatedItemH)
                    {
                        UpdateSt_RelatedItem.CompanyID = UserInfo.fCompanyId;
                        UpdateSt_RelatedItem.ItemCode = updateSt_ItemCard.ItemCode;
                        UpdateSt_RelatedItem.RelatedItemCode = UpdateSt_RelatedItem.RelatedItemCode;
                        UpdateSt_RelatedItem.RowNumber = UpdateSt_RelatedItem.RowNumber;
                        _unitOfWork.St_ItemCardH.AddRelatedItem(UpdateSt_RelatedItem);
                    }
                }
                if (ObjToUpdate.St_SimilarItemH != null)
                {
                    foreach (var UpdateSt_SimilarItem in ObjToUpdate.St_SimilarItemH)
                    {
                        UpdateSt_SimilarItem.CompanyID = UserInfo.fCompanyId;
                        UpdateSt_SimilarItem.ItemCode = updateSt_ItemCard.ItemCode;
                        UpdateSt_SimilarItem.SimilarItemCode = UpdateSt_SimilarItem.SimilarItemCode;
                        UpdateSt_SimilarItem.RowNumber = UpdateSt_SimilarItem.RowNumber;
                        _unitOfWork.St_ItemCardH.AddSimilarItem(UpdateSt_SimilarItem);
                    }
                }
                if (updateSt_ItemCard.ItemTypeNo == 3)
                {
                    foreach (var UpdateSt_ManufacturingStage in ObjToUpdate.St_ManufacturingStageH)
                    {
                        UpdateSt_ManufacturingStage.CompanyID = UserInfo.fCompanyId;
                        UpdateSt_ManufacturingStage.ItemCode = updateSt_ItemCard.ItemCode;
                        UpdateSt_ManufacturingStage.FactoryNo = UpdateSt_ManufacturingStage.FactoryNo;
                        UpdateSt_ManufacturingStage.RowNumber = UpdateSt_ManufacturingStage.RowNumber;
                        UpdateSt_ManufacturingStage.NumberOfDays = UpdateSt_ManufacturingStage.NumberOfDays;
                        _unitOfWork.St_ItemCardH.AddManufacturingStage(UpdateSt_ManufacturingStage);
                    }
                    if (ObjToUpdate.St_RawMaterialH != null)
                    {
                        foreach (var UpdateSt_RawMaterial in ObjToUpdate.St_RawMaterialH)
                        {
                            UpdateSt_RawMaterial.CompanyID = UserInfo.fCompanyId;
                            UpdateSt_RawMaterial.ItemCode = updateSt_ItemCard.ItemCode;
                            UpdateSt_RawMaterial.RawMaterialCode = UpdateSt_RawMaterial.RawMaterialCode;
                            UpdateSt_RawMaterial.RowNumber = UpdateSt_RawMaterial.RowNumber;
                            UpdateSt_RawMaterial.Quantity = UpdateSt_RawMaterial.Quantity;
                            UpdateSt_RawMaterial.Cost = UpdateSt_RawMaterial.Cost;
                            UpdateSt_RawMaterial.Total = UpdateSt_RawMaterial.Total;
                            _unitOfWork.St_ItemCardH.AddRawMaterial(UpdateSt_RawMaterial);
                        }
                    }
                }
                int iRow = 0;
                if (ObjToUpdate.ItemTypeNo == 1)
                {
                    if (ObjToUpdate.St_SubColorsItemH != null)
                    {
                        foreach (var UpdateSubColorsItem in ObjToUpdate.St_SubColorsItemH)
                        {
                            iRow = iRow + 1;
                            UpdateSubColorsItem.CompanyID = UserInfo.fCompanyId;
                            UpdateSubColorsItem.ItemCode = updateSt_ItemCard.ItemCode;
                            UpdateSubColorsItem.SubItemColorCode = UpdateSubColorsItem.SubItemColorCode;
                            UpdateSubColorsItem.RowNumber = iRow;
                            _unitOfWork.St_ItemCardH.AddSubColorsItem(UpdateSubColorsItem);
                        }
                    }
                }
                iRow = 0;
                if (ObjToUpdate.St_ItemWarehouseH != null)
                {
                    foreach (var UpdateItemWarehouse in ObjToUpdate.St_ItemWarehouseH)
                    {
                        iRow = iRow + 1;
                        UpdateItemWarehouse.CompanyID = UserInfo.fCompanyId;
                        UpdateItemWarehouse.ItemCode = updateSt_ItemCard.ItemCode;
                        UpdateItemWarehouse.StockCode = UpdateItemWarehouse.StockCode;
                        UpdateItemWarehouse.RowNumber = iRow;
                        _unitOfWork.St_ItemCardH.AddItemWarehous(UpdateItemWarehouse);
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
                _unitOfWork.St_ItemCardH.AddItemCard(updateSt_ItemCard);
                _unitOfWork.Complete();
                Msg.Code = 1;
                Msg.Msg = Resources.Resource.UpdatedSuccessfully;
                Msg.LastID = updateSt_ItemCard.ItemCode;
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
        public JsonResult GetAllSt_WarehouseHByItemCodeView(string id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllSt_WarehouseH = _unitOfWork.NativeSql.GetAllSt_WarehouseHByItemCodeView(UserInfo.fCompanyId, id);
                if (AllSt_WarehouseH == null)
                {
                    return Json(new List<St_ItemCardHVM>(), JsonRequestBehavior.AllowGet);
                }
                return Json(AllSt_WarehouseH, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<St_ItemCardHVM>(), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult GetAllSt_SubColorByItemCodeView(string id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllSt_SubItemColor = _unitOfWork.NativeSql.GetAllSt_SubColorHByItemCodeView(UserInfo.fCompanyId, id);
                if (AllSt_SubItemColor == null)
                {
                    return Json(new List<St_ItemCardHVM>(), JsonRequestBehavior.AllowGet);
                }
                return Json(AllSt_SubItemColor, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<St_ItemCardHVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public JsonResult GetAllSt_RelatedItemByItemCodeView(string id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllSt_SubItemColor = _unitOfWork.NativeSql.GetAllSt_RelatedItemHByItemCodeView(UserInfo.fCompanyId, id);
                if (AllSt_SubItemColor == null)
                {
                    return Json(new List<St_ItemCardHVM>(), JsonRequestBehavior.AllowGet);
                }
                return Json(AllSt_SubItemColor, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<St_ItemCardHVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public JsonResult GetAllSt_SimilarItemByItemCodeView(string id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllSt_SubItemColor = _unitOfWork.NativeSql.GetAllSt_SimilarItemHByItemCodeView(UserInfo.fCompanyId, id);
                if (AllSt_SubItemColor == null)
                {
                    return Json(new List<St_ItemCardHVM>(), JsonRequestBehavior.AllowGet);
                }
                return Json(AllSt_SubItemColor, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<St_ItemCardHVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public JsonResult GetAllSt_ManufacturingStagesByItemCodeView(string id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllSt_SubItemColor = _unitOfWork.NativeSql.GetAllSt_ManufacturingStagesHByItemCodeView(UserInfo.fCompanyId, id);
                if (AllSt_SubItemColor == null)
                {
                    return Json(new List<St_ItemCardHVM>(), JsonRequestBehavior.AllowGet);
                }
                return Json(AllSt_SubItemColor, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<St_ItemCardHVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public JsonResult GetAllSt_RawMaterialByItemCodeView(string id)  
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllSt_SubItemColor = _unitOfWork.NativeSql.GetAllSt_RawMaterialHByItemCodeView(UserInfo.fCompanyId, id);
                if (AllSt_SubItemColor == null)
                {
                    return Json(new List<St_ItemCardHVM>(), JsonRequestBehavior.AllowGet);
                }
                return Json(AllSt_SubItemColor, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<St_ItemCardHVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult Delete(string id)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var St_ItemCardObj = _unitOfWork.St_ItemCardH.GetSt_ItemCardById(id, UserInfo.fCompanyId);
            var St_ItemCardHVMObj = new St_ItemCardHVM
            {
                St_FactoryH = _unitOfWork.St_FactoryH.GetAllSt_FactoryH(UserInfo.fCompanyId),
                St_ItemGroupH = _unitOfWork.St_ItemGroupH.GetAllSt_ItemGroupH(UserInfo.fCompanyId),
                St_ItemUnitH = _unitOfWork.St_ItemUnitH.GetAllSt_ItemUnitH(UserInfo.fCompanyId),
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                ItemSerial = St_ItemCardObj.ItemSerial,
                ItemCode = St_ItemCardObj.ItemCode,
                GroupCode = St_ItemCardObj.GroupCode,
                ArabicName = St_ItemCardObj.ArabicName,
                EnglishName = St_ItemCardObj.EnglishName,
                NumberOfPieces = St_ItemCardObj.NumberOfPieces,
                ReOrderLimit = St_ItemCardObj.ReOrderLimit,
                OrderQuantity = St_ItemCardObj.OrderQuantity,
                CBM = St_ItemCardObj.CBM,
                ItemUnitNo = St_ItemCardObj.ItemUnitNo,
                StyleNo = St_ItemCardObj.StyleNo,
                ItemLevelNo = St_ItemCardObj.ItemLevelNo,
                ItemTypeNo = St_ItemCardObj.ItemTypeNo,
                SupplierAccountNumber = St_ItemCardObj.SupplierAccountNumber,
                SupplierAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, St_ItemCardObj.SupplierAccountNumber),
                ShowOnline = St_ItemCardObj.ShowOnline,
                QuantityOnline = St_ItemCardObj.QuantityOnline,
                StopItem = St_ItemCardObj.StopItem,
                SaleOfOfferedArticleIsPermitted = St_ItemCardObj.SaleOfOfferedArticleIsPermitted,
                FabricChangeIsAllowed = St_ItemCardObj.FabricChangeIsAllowed,
                AllowWoodToChangeColor = St_ItemCardObj.AllowWoodToChangeColor,
                ApprovingTheWarehouseQuantityLessThan = St_ItemCardObj.ApprovingTheWarehouseQuantityLessThan,
                QuantityManufacturing = St_ItemCardObj.QuantityManufacturing,
                SalePrice = St_ItemCardObj.SalePrice,
                TaxTypeNo = St_ItemCardObj.TaxTypeNo,
                TaxRate = St_ItemCardObj.TaxRate,
                LocalCost = St_ItemCardObj.LocalCost,
                ForeignCost = St_ItemCardObj.ForeignCost,
                CostRate = St_ItemCardObj.CostRate,
                TheTargetMonthlyAmount = St_ItemCardObj.TheTargetMonthlyAmount,
                TheNumberOfDaysTheCardIsOpened = _unitOfWork.NativeSql.GetOpeningDateH(UserInfo.fCompanyId, St_ItemCardObj.ItemCode),
                LastLocalPurchasePrice = 0,
                LastForeignPurchasePrice = 0,
                TotalQuantitySold = 0,
                TotalValueSold = 0,
                FactoryNo = St_ItemCardObj.FactoryNo,
                FactoryNotes = St_ItemCardObj.FactoryNotes,
                WageRate = St_ItemCardObj.WageRate,
                NumberOfWorkingDays = St_ItemCardObj.NumberOfWorkingDays,
                NumberOfStages = St_ItemCardObj.NumberOfStages,
                NetTotal = St_ItemCardObj.NetTotal,
                TotalQuantity = St_ItemCardObj.TotalQuantity,
                TotalCost = St_ItemCardObj.TotalCost,
                ItemLogo = St_ItemCardObj.ItemLogo
            };
            return View(St_ItemCardHVMObj);
        }
        [HttpPost]
        public JsonResult DeleteSt_ItemCardH(St_ItemCardHVM ObjToDelete)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var deleteSt_ItemCard = new St_ItemCardH();

                deleteSt_ItemCard.ItemCode = ObjToDelete.ItemCode;

                _unitOfWork.NativeSql.DeleteSt_ItemCardH(UserInfo.fCompanyId, deleteSt_ItemCard.ItemCode);
                _unitOfWork.NativeSql.DeleteSt_SimilarItemH(UserInfo.fCompanyId, deleteSt_ItemCard.ItemCode);
                _unitOfWork.NativeSql.DeleteSt_RelatedItemH(UserInfo.fCompanyId, deleteSt_ItemCard.ItemCode);
                _unitOfWork.NativeSql.DeleteSt_ManufacturingStageH(UserInfo.fCompanyId, deleteSt_ItemCard.ItemCode);
                _unitOfWork.NativeSql.DeleteSt_RawMaterialH(UserInfo.fCompanyId, deleteSt_ItemCard.ItemCode);
                _unitOfWork.NativeSql.DeleteSt_SubColorsItemH(UserInfo.fCompanyId, deleteSt_ItemCard.ItemCode);
                _unitOfWork.NativeSql.DeleteSt_ItemWarehouseH(UserInfo.fCompanyId, deleteSt_ItemCard.ItemCode);
                _unitOfWork.NativeSql.DeleteSt_ItemGallary(UserInfo.fCompanyId, deleteSt_ItemCard.ItemCode);



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

                                string path = Path.Combine(new DirectoryInfo(string.Format(@"{0}ItemGallary\{1}", base.Server.MapPath(@"\"), deleteSt_ItemCard.ItemCode)).ToString());
                string[] files = Directory.GetFiles(path);
                foreach (string file in files)
                {
                    System.IO.File.Delete(file);
                   
                }

                Msg.Code = 1;
                Msg.Msg = Resources.Resource.DeletedSuccessfully;
                Msg.LastID = deleteSt_ItemCard.ItemCode;
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