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
    public class St_ItemCardController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public St_ItemCardController()
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
            var St_ItemCardObj = new St_ItemCardVM
            {
                St_ItemUnit = _unitOfWork.St_ItemUnit.GetAllItemUnit(UserInfo.fCompanyId),
                St_CountryOfOrigin = _unitOfWork.St_CountryOfOrigin.GetAllSt_CountryOfOrigin(UserInfo.fCompanyId),
                St_ManufacturerCompany = _unitOfWork.St_ManufacturerCompany.GetAllSt_ManufacturerCompany(UserInfo.fCompanyId),
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
            return View(St_ItemCardObj);
        }
        [HttpPost]
        public JsonResult GetAllSt_ItemCard(St_ItemCardVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllSt_ItemCard = _unitOfWork.NativeSql.GetAllSt_ItemCardFilter(UserInfo.fCompanyId, Obj.ItemCode);
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
        public ActionResult Add()
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var St_ItemCardVMObj = new St_ItemCardVM
            {
                St_ItemUnit = _unitOfWork.St_ItemUnit.GetAllItemUnit(UserInfo.fCompanyId),
                St_ManufacturerCompany = _unitOfWork.St_ManufacturerCompany.GetAllSt_ManufacturerCompany(UserInfo.fCompanyId),
                St_CountryOfOrigin = _unitOfWork.St_CountryOfOrigin.GetAllSt_CountryOfOrigin(UserInfo.fCompanyId),
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
                St_MeasurementDetailLength = _unitOfWork.St_MeasurementDetail.GetSt_MeasurementDetailBySt_Measurement(UserInfo.fCompanyId, 1),
                St_MeasurementDetailWidth = _unitOfWork.St_MeasurementDetail.GetSt_MeasurementDetailBySt_Measurement(UserInfo.fCompanyId, 2),
                St_MeasurementDetailHeight = _unitOfWork.St_MeasurementDetail.GetSt_MeasurementDetailBySt_Measurement(UserInfo.fCompanyId, 3),
                St_MeasurementDetailSize = _unitOfWork.St_MeasurementDetail.GetSt_MeasurementDetailBySt_Measurement(UserInfo.fCompanyId, 4),
                St_MeasurementDetailWeight = _unitOfWork.St_MeasurementDetail.GetSt_MeasurementDetailBySt_Measurement(UserInfo.fCompanyId, 5),
                St_MeasurementDetailUnit = _unitOfWork.St_MeasurementDetail.GetSt_MeasurementDetailBySt_Measurement(UserInfo.fCompanyId, 6),
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
                CategoriePrice_1Name = _unitOfWork.St_CategoryPrice.GetSt_CategoryPriceNameByID(UserInfo.fCompanyId, 1),
                CategoriePrice_2Name = _unitOfWork.St_CategoryPrice.GetSt_CategoryPriceNameByID(UserInfo.fCompanyId, 2),
                CategoriePrice_3Name = _unitOfWork.St_CategoryPrice.GetSt_CategoryPriceNameByID(UserInfo.fCompanyId, 3),
                CategoriePrice_4Name = _unitOfWork.St_CategoryPrice.GetSt_CategoryPriceNameByID(UserInfo.fCompanyId, 4),
                CategoriePrice_5Name = _unitOfWork.St_CategoryPrice.GetSt_CategoryPriceNameByID(UserInfo.fCompanyId, 5),
                CategoriePrice_6Name = _unitOfWork.St_CategoryPrice.GetSt_CategoryPriceNameByID(UserInfo.fCompanyId, 6),
                CategoriePrice_7Name = _unitOfWork.St_CategoryPrice.GetSt_CategoryPriceNameByID(UserInfo.fCompanyId, 7),
                CategoriePrice_8Name = _unitOfWork.St_CategoryPrice.GetSt_CategoryPriceNameByID(UserInfo.fCompanyId, 8),
                CategoriePrice_9Name = _unitOfWork.St_CategoryPrice.GetSt_CategoryPriceNameByID(UserInfo.fCompanyId, 9),
                CategoriePrice_10Name = _unitOfWork.St_CategoryPrice.GetSt_CategoryPriceNameByID(UserInfo.fCompanyId, 10),
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency
            };
            return View(St_ItemCardVMObj);
        }
        [ValidateInput(false)]
        public ActionResult GridViewSimilarItem(string id)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            if (!String.IsNullOrEmpty(id))
            {
                var ItemCardObj = _unitOfWork.NativeSql.GetSimilarItemByItemCard(id, UserInfo.fCompanyId);
                return PartialView("GridViewSimilarItem", ItemCardObj);
            }
            else
            {
                var ItemCardObj = new List<St_ItemCardVM>();
                return PartialView("GridViewSimilarItem", ItemCardObj);
            }
        }
        [ValidateInput(false)]
        public ActionResult GridViewSimilarItemCheck(string id)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            if (!String.IsNullOrEmpty(id))
            {
                var ItemCardObj = _unitOfWork.NativeSql.GetSimilarItemByItemCard(id, UserInfo.fCompanyId);
                return PartialView("GridViewSimilarItemCheck", ItemCardObj);
            }
            else
            {
                var ItemCardObj = new List<St_ItemCardVM>();
                return PartialView("GridViewSimilarItemCheck", ItemCardObj);
            }


        }
        [ValidateInput(false)]
        public ActionResult GridViewAlternativeItem(string id)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            if (!String.IsNullOrEmpty(id))
            {
                var ItemCardObj = _unitOfWork.NativeSql.GetAlternativeItemByItemCard(id, UserInfo.fCompanyId);
                return PartialView("GridViewAlternativeItem", ItemCardObj);
            }
            else
            {
                var ItemCardObj = new List<St_ItemCardVM>();
                return PartialView("GridViewAlternativeItem", ItemCardObj);
            }


        }
        [ValidateInput(false)]
        public ActionResult GridViewItemOffer1(string id)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            if (!String.IsNullOrEmpty(id))
            {
                var ItemCardObj = _unitOfWork.NativeSql.GetItemOffer1ByItemCard(id, UserInfo.fCompanyId);
                return PartialView("GridViewItemOffer1", ItemCardObj);
            }
            else
            {
                var ItemCardObj = new List<St_ItemCardVM>();
                return PartialView("GridViewItemOffer1", ItemCardObj);
            }


        }
        [ValidateInput(false)]
        public ActionResult GridViewItemOffer2(string id)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            if (!String.IsNullOrEmpty(id))
            {
                var ItemCardObj = _unitOfWork.NativeSql.GetItemOffer2ByItemCard(id, UserInfo.fCompanyId);
                return PartialView("GridViewItemOffer2", ItemCardObj);
            }
            else
            {
                var ItemCardObj = new List<St_ItemCardVM>();
                return PartialView("GridViewItemOffer2", ItemCardObj);
            }


        }
        [ValidateInput(false)]
        public ActionResult GridViewOtherItemUnit(string id)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            if (!String.IsNullOrEmpty(id))
            {
                var ItemCardObj = _unitOfWork.NativeSql.GetOtherItemUnitByItemCard(id, UserInfo.fCompanyId);
                return PartialView("GridViewOtherItemUnit", ItemCardObj);
            }
            else
            {
                var ItemCardObj = new List<St_ItemCardVM>();
                return PartialView("GridViewOtherItemUnit", ItemCardObj);
            }


        }
        [ValidateInput(false)]
        public ActionResult GridViewOtherItemUnitCheck(string id)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            if (!String.IsNullOrEmpty(id))
            {
                var ItemCardObj = _unitOfWork.NativeSql.GetOtherItemUnitByItemCard(id, UserInfo.fCompanyId);
                return PartialView("GridViewOtherItemUnitCheck", ItemCardObj);
            }
            else
            {
                var ItemCardObj = new List<St_ItemCardVM>();
                return PartialView("GridViewOtherItemUnitCheck", ItemCardObj);
            }


        }
        [HttpGet]
        public JsonResult CheckItemCode(string id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);

            var ItemCardInfo = _unitOfWork.NativeSql.CheckItemCode(UserInfo.fCompanyId, id).FirstOrDefault();
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
        public JsonResult CheckItemOtherUnit(string id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);

            var ItemOtherUnit = _unitOfWork.NativeSql.GetAllSt_ItemOtherUnit(id,UserInfo.fCompanyId).FirstOrDefault();
            if (ItemOtherUnit == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(ItemOtherUnit, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public JsonResult CheckIfItemCodeExisting(string id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            string ItemCode = _unitOfWork.NativeSql.CheckIfItemCodeExisting(UserInfo.fCompanyId, id);
            return Json(ItemCode, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult CheckIfItemCodeExistingInSimilar(string id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            string ItemCode = _unitOfWork.NativeSql.CheckIfItemCodeExistingInSimilar(UserInfo.fCompanyId, id);
            return Json(ItemCode, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveSt_ItemCard(St_ItemCardVM ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var saveSt_ItemCard = new St_ItemCard();
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
                saveSt_ItemCard.ItemCode = ObjToSave.ItemCode;
                saveSt_ItemCard.MinimumItemNo = ObjToSave.MinimumItemNo;
                saveSt_ItemCard.MaximumItemNo = ObjToSave.MaximumItemNo;
                saveSt_ItemCard.TaxRate = ObjToSave.TaxRate;
                saveSt_ItemCard.TaxRateNo = ObjToSave.TaxRateNo;
                saveSt_ItemCard.TaxTypeNo = ObjToSave.TaxTypeNo;
                saveSt_ItemCard.SupplierAccountNumber = ObjToSave.SupplierAccountNumber;
                saveSt_ItemCard.ManufacturerCompanyNo = ObjToSave.ManufacturerCompanyNo;
                saveSt_ItemCard.CountryOfOriginNo = ObjToSave.CountryOfOriginNo;
                saveSt_ItemCard.SalePrice = ObjToSave.SalePrice;
                saveSt_ItemCard.PointOfSalePrice = ObjToSave.PointOfSalePrice;
                saveSt_ItemCard.MinimumSaleAmount = ObjToSave.MinimumSaleAmount;
                saveSt_ItemCard.LocalCost = ObjToSave.LocalCost;
                saveSt_ItemCard.ForeignCost = ObjToSave.ForeignCost;
                saveSt_ItemCard.TheTargetMonthlyAmount = ObjToSave.TheTargetMonthlyAmount;
                saveSt_ItemCard.StopItem = ObjToSave.StopItem;
                saveSt_ItemCard.StoppingItemFromSelling = ObjToSave.StoppingItemFromSelling;
                saveSt_ItemCard.StoppingItemFromBuying = ObjToSave.StoppingItemFromBuying;
                saveSt_ItemCard.StoppingItemFromPointOfSale = ObjToSave.StoppingItemFromPointOfSale;
                saveSt_ItemCard.ItemServicesWithoutAnInventory = ObjToSave.ItemServicesWithoutAnInventory;
                saveSt_ItemCard.TrackAnExpirationDate = ObjToSave.TrackAnExpirationDate;
                saveSt_ItemCard.TrackSequence = ObjToSave.TrackSequence;
                saveSt_ItemCard.TrackSequenceUponInput = ObjToSave.TrackSequenceUponInput;
                saveSt_ItemCard.TrackSequenceUponOutput = ObjToSave.TrackSequenceUponOutput;
                saveSt_ItemCard.TrackCustoms = ObjToSave.TrackCustoms;
                saveSt_ItemCard.Smoke = ObjToSave.Smoke;
                saveSt_ItemCard.NotRelatedToTheUnitAbove = ObjToSave.NotRelatedToTheUnitAbove;
                saveSt_ItemCard.ScaleItem = ObjToSave.ScaleItem;
                saveSt_ItemCard.ItemSalesAndPurchases = ObjToSave.ItemSalesAndPurchases;
                saveSt_ItemCard.OfferType = ObjToSave.OfferType;
                saveSt_ItemCard.ItemUnitNo = ObjToSave.ItemUnitNo;
                saveSt_ItemCard.ItemNatureNo = ObjToSave.ItemNatureNo;
                saveSt_ItemCard.ItemUnitNo = ObjToSave.ItemUnitNo;
                saveSt_ItemCard.Categorie_1 = ObjToSave.Categorie_1;
                saveSt_ItemCard.Categorie_2 = ObjToSave.Categorie_2;
                saveSt_ItemCard.Categorie_3 = ObjToSave.Categorie_3;
                saveSt_ItemCard.Categorie_4 = ObjToSave.Categorie_4;
                saveSt_ItemCard.Categorie_5 = ObjToSave.Categorie_5;
                saveSt_ItemCard.Categorie_6 = ObjToSave.Categorie_6;
                saveSt_ItemCard.Categorie_7 = ObjToSave.Categorie_7;
                saveSt_ItemCard.Categorie_8 = ObjToSave.Categorie_8;
                saveSt_ItemCard.Categorie_9 = ObjToSave.Categorie_9;
                saveSt_ItemCard.Categorie_10 = ObjToSave.Categorie_10;
                saveSt_ItemCard.Categorie_11 = ObjToSave.Categorie_11;
                saveSt_ItemCard.Categorie_12 = ObjToSave.Categorie_12;
                saveSt_ItemCard.Categorie_13 = ObjToSave.Categorie_13;
                saveSt_ItemCard.Categorie_14 = ObjToSave.Categorie_14;
                saveSt_ItemCard.Categorie_15 = ObjToSave.Categorie_15;
                saveSt_ItemCard.LengthNo = ObjToSave.LengthNo;
                saveSt_ItemCard.WidthNo = ObjToSave.WidthNo;
                saveSt_ItemCard.HeightNo = ObjToSave.HeightNo;
                saveSt_ItemCard.SizeNo = ObjToSave.SizeNo;
                saveSt_ItemCard.WeightNo = ObjToSave.WeightNo;
                saveSt_ItemCard.UnitNo = ObjToSave.UnitNo;
                saveSt_ItemCard.CBM = ObjToSave.CBM;
                saveSt_ItemCard.CategoriePrice_1 = ObjToSave.CategoriePrice_1;
                saveSt_ItemCard.CategoriePrice_2 = ObjToSave.CategoriePrice_2;
                saveSt_ItemCard.CategoriePrice_3 = ObjToSave.CategoriePrice_3;
                saveSt_ItemCard.CategoriePrice_4 = ObjToSave.CategoriePrice_4;
                saveSt_ItemCard.CategoriePrice_5 = ObjToSave.CategoriePrice_5;
                saveSt_ItemCard.CategoriePrice_6 = ObjToSave.CategoriePrice_6;
                saveSt_ItemCard.CategoriePrice_7 = ObjToSave.CategoriePrice_7;
                saveSt_ItemCard.CategoriePrice_8 = ObjToSave.CategoriePrice_8;
                saveSt_ItemCard.CategoriePrice_9 = ObjToSave.CategoriePrice_9;
                saveSt_ItemCard.CategoriePrice_10 = ObjToSave.CategoriePrice_10;
                saveSt_ItemCard.ItemLogo = ObjToSave.ItemLogo;
                var iRow = 1;
                var SaveMainItemCodeInSt_SimilarItem = new St_SimilarItem();
                SaveMainItemCodeInSt_SimilarItem.CompanyID = UserInfo.fCompanyId;
                SaveMainItemCodeInSt_SimilarItem.ItemCode = saveSt_ItemCard.ItemCode;
                SaveMainItemCodeInSt_SimilarItem.SimilarItemCode = saveSt_ItemCard.ItemCode;
                SaveMainItemCodeInSt_SimilarItem.SimilarItemUnitCode = saveSt_ItemCard.ItemUnitNo;
                SaveMainItemCodeInSt_SimilarItem.RowNumber = iRow;
                SaveMainItemCodeInSt_SimilarItem.SimilarItemType = 1;
                _unitOfWork.St_ItemCard.AddSimilarItem(SaveMainItemCodeInSt_SimilarItem);
                if (ObjToSave.St_SimilarItem != null)
                {
                    foreach (var SaveSt_SimilarItem in ObjToSave.St_SimilarItem)
                    {
                        iRow = iRow + 1;
                        SaveSt_SimilarItem.CompanyID = UserInfo.fCompanyId;
                        SaveSt_SimilarItem.ItemCode = saveSt_ItemCard.ItemCode;
                        SaveSt_SimilarItem.SimilarItemCode = SaveSt_SimilarItem.SimilarItemCode;
                        SaveSt_SimilarItem.SimilarItemUnitCode = saveSt_ItemCard.ItemUnitNo;
                        SaveSt_SimilarItem.RowNumber = iRow;
                        SaveSt_SimilarItem.SimilarItemType = 2;
                        _unitOfWork.St_ItemCard.AddSimilarItem(SaveSt_SimilarItem);
                    }
                }
                if (ObjToSave.St_AlternativeItem != null)
                {
                    foreach (var SaveSt_AlternativeItem in ObjToSave.St_AlternativeItem)
                    {
                        SaveSt_AlternativeItem.CompanyID = UserInfo.fCompanyId;
                        SaveSt_AlternativeItem.ItemCode = saveSt_ItemCard.ItemCode;
                        SaveSt_AlternativeItem.AlternativeItemCode = SaveSt_AlternativeItem.AlternativeItemCode;
                        SaveSt_AlternativeItem.RowNumber = SaveSt_AlternativeItem.RowNumber;
                        _unitOfWork.St_ItemCard.AddAlternativeItem(SaveSt_AlternativeItem);
                    }
                }
                if (ObjToSave.St_ItemOtherUnit != null)
                {
                    foreach (var SaveSt_ItemOtherUnit in ObjToSave.St_ItemOtherUnit)
                    {
                        iRow = iRow + 1;
                        var SaveSt_ItemOtherUnitBarcodeInSt_SimilarItem = new St_SimilarItem();
                        SaveSt_ItemOtherUnit.CompanyID = UserInfo.fCompanyId;
                        SaveSt_ItemOtherUnit.ItemCode = saveSt_ItemCard.ItemCode;
                        SaveSt_ItemOtherUnit.OtherUnitNumber = SaveSt_ItemOtherUnit.OtherUnitNumber;
                        SaveSt_ItemOtherUnit.OtherUnitQuantity = SaveSt_ItemOtherUnit.OtherUnitQuantity;
                        SaveSt_ItemOtherUnit.OtherUnitSalePrice = SaveSt_ItemOtherUnit.OtherUnitSalePrice;
                        SaveSt_ItemOtherUnit.OtherUnitPurchasePrice = SaveSt_ItemOtherUnit.OtherUnitPurchasePrice;
                        SaveSt_ItemOtherUnit.OtherUnitBarcode = SaveSt_ItemOtherUnit.OtherUnitBarcode;
                        SaveSt_ItemOtherUnit.RowNumber = SaveSt_ItemOtherUnit.RowNumber;
                        SaveSt_ItemOtherUnitBarcodeInSt_SimilarItem.CompanyID = UserInfo.fCompanyId;
                        SaveSt_ItemOtherUnitBarcodeInSt_SimilarItem.ItemCode = saveSt_ItemCard.ItemCode;
                        SaveSt_ItemOtherUnitBarcodeInSt_SimilarItem.SimilarItemCode = SaveSt_ItemOtherUnit.OtherUnitBarcode;
                        SaveSt_ItemOtherUnitBarcodeInSt_SimilarItem.SimilarItemUnitCode = SaveSt_ItemOtherUnit.OtherUnitNumber;
                        SaveSt_ItemOtherUnitBarcodeInSt_SimilarItem.RowNumber = iRow;
                        SaveSt_ItemOtherUnitBarcodeInSt_SimilarItem.SimilarItemType = 3;
                        _unitOfWork.St_ItemCard.AddSimilarItem(SaveSt_ItemOtherUnitBarcodeInSt_SimilarItem);
                        _unitOfWork.St_ItemCard.AddItemOtherUnit(SaveSt_ItemOtherUnit);
                    }
                }
                if (saveSt_ItemCard.OfferType == 1)
                {
                    if (ObjToSave.St_ItemOffer != null)
                    {
                        foreach (var SaveSt_ItemOffer in ObjToSave.St_ItemOffer)
                        {
                            SaveSt_ItemOffer.CompanyID = UserInfo.fCompanyId;
                            SaveSt_ItemOffer.ItemCode = saveSt_ItemCard.ItemCode;
                            SaveSt_ItemOffer.ItemOfferQuantitySold = SaveSt_ItemOffer.ItemOfferQuantitySold;
                            SaveSt_ItemOffer.ItemOfferBonus = SaveSt_ItemOffer.ItemOfferBonus;
                            SaveSt_ItemOffer.ItemOfferUnitPrice = 0;
                            SaveSt_ItemOffer.ItemOfferTotalPrice = 0;
                            SaveSt_ItemOffer.RowNumber = SaveSt_ItemOffer.RowNumber;
                            _unitOfWork.St_ItemCard.AddItemOffer(SaveSt_ItemOffer);
                        }
                    }
                    else {
                        saveSt_ItemCard.OfferType = 3;
                    }
                }
                else if (saveSt_ItemCard.OfferType == 2)
                {
                    if (ObjToSave.St_ItemOffer != null)
                    {
                        foreach (var SaveSt_ItemOffer in ObjToSave.St_ItemOffer)
                        {
                            SaveSt_ItemOffer.CompanyID = UserInfo.fCompanyId;
                            SaveSt_ItemOffer.ItemCode = saveSt_ItemCard.ItemCode;
                            SaveSt_ItemOffer.ItemOfferQuantitySold = SaveSt_ItemOffer.ItemOfferQuantitySold;
                            SaveSt_ItemOffer.ItemOfferBonus = 0;
                            SaveSt_ItemOffer.ItemOfferUnitPrice = SaveSt_ItemOffer.ItemOfferUnitPrice;
                            SaveSt_ItemOffer.ItemOfferTotalPrice = SaveSt_ItemOffer.ItemOfferTotalPrice;
                            SaveSt_ItemOffer.RowNumber = SaveSt_ItemOffer.RowNumber;
                            _unitOfWork.St_ItemCard.AddItemOffer(SaveSt_ItemOffer);
                        }
                    }
                    else
                    {
                        saveSt_ItemCard.OfferType = 3;
                    }
                }
                iRow = 0;
                if (ObjToSave.St_ItemWarehouse != null)
                {
                    foreach (var SaveItemWarehouse in ObjToSave.St_ItemWarehouse)
                    {
                        iRow = iRow + 1;
                        SaveItemWarehouse.CompanyID = UserInfo.fCompanyId;
                        SaveItemWarehouse.ItemCode = saveSt_ItemCard.ItemCode;
                        SaveItemWarehouse.StockCode = SaveItemWarehouse.StockCode;
                        SaveItemWarehouse.StockMinimumItemNo = 0;
                        SaveItemWarehouse.StockMaximumItemNo = 0;
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
                _unitOfWork.St_ItemCard.AddItemCard(saveSt_ItemCard);
                _unitOfWork.Complete();
                Msg.Code = 1;
                Msg.Msg = Resources.Resource.AddedSuccessfully;
                Msg.LastID = "0";
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
        public JsonResult GetAllSt_WarehouseByItemCode(string id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllSt_Warehouse = _unitOfWork.NativeSql.GetAllSt_WarehouseByItemCode(UserInfo.fCompanyId, id);
                if (AllSt_Warehouse == null)
                {
                    return Json(new List<St_WarehouseVM>(), JsonRequestBehavior.AllowGet);
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
                return Json(new List<St_WarehouseHVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult Update(string id)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var St_ItemCardObj = _unitOfWork.St_ItemCard.GetSt_ItemCardById(id, UserInfo.fCompanyId);
            var St_ItemCardVMObj = new St_ItemCardVM
            {
                St_ItemUnit = _unitOfWork.St_ItemUnit.GetAllItemUnit(UserInfo.fCompanyId),
                St_ManufacturerCompany = _unitOfWork.St_ManufacturerCompany.GetAllSt_ManufacturerCompany(UserInfo.fCompanyId),
                St_CountryOfOrigin = _unitOfWork.St_CountryOfOrigin.GetAllSt_CountryOfOrigin(UserInfo.fCompanyId),
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
                St_MeasurementDetailLength = _unitOfWork.St_MeasurementDetail.GetSt_MeasurementDetailBySt_Measurement(UserInfo.fCompanyId, 1),
                St_MeasurementDetailWidth = _unitOfWork.St_MeasurementDetail.GetSt_MeasurementDetailBySt_Measurement(UserInfo.fCompanyId, 2),
                St_MeasurementDetailHeight = _unitOfWork.St_MeasurementDetail.GetSt_MeasurementDetailBySt_Measurement(UserInfo.fCompanyId, 3),
                St_MeasurementDetailSize = _unitOfWork.St_MeasurementDetail.GetSt_MeasurementDetailBySt_Measurement(UserInfo.fCompanyId, 4),
                St_MeasurementDetailWeight = _unitOfWork.St_MeasurementDetail.GetSt_MeasurementDetailBySt_Measurement(UserInfo.fCompanyId, 5),
                St_MeasurementDetailUnit = _unitOfWork.St_MeasurementDetail.GetSt_MeasurementDetailBySt_Measurement(UserInfo.fCompanyId, 6),
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
                CategoriePrice_1Name = _unitOfWork.St_CategoryPrice.GetSt_CategoryPriceNameByID(UserInfo.fCompanyId, 1),
                CategoriePrice_2Name = _unitOfWork.St_CategoryPrice.GetSt_CategoryPriceNameByID(UserInfo.fCompanyId, 2),
                CategoriePrice_3Name = _unitOfWork.St_CategoryPrice.GetSt_CategoryPriceNameByID(UserInfo.fCompanyId, 3),
                CategoriePrice_4Name = _unitOfWork.St_CategoryPrice.GetSt_CategoryPriceNameByID(UserInfo.fCompanyId, 4),
                CategoriePrice_5Name = _unitOfWork.St_CategoryPrice.GetSt_CategoryPriceNameByID(UserInfo.fCompanyId, 5),
                CategoriePrice_6Name = _unitOfWork.St_CategoryPrice.GetSt_CategoryPriceNameByID(UserInfo.fCompanyId, 6),
                CategoriePrice_7Name = _unitOfWork.St_CategoryPrice.GetSt_CategoryPriceNameByID(UserInfo.fCompanyId, 7),
                CategoriePrice_8Name = _unitOfWork.St_CategoryPrice.GetSt_CategoryPriceNameByID(UserInfo.fCompanyId, 8),
                CategoriePrice_9Name = _unitOfWork.St_CategoryPrice.GetSt_CategoryPriceNameByID(UserInfo.fCompanyId, 9),
                CategoriePrice_10Name = _unitOfWork.St_CategoryPrice.GetSt_CategoryPriceNameByID(UserInfo.fCompanyId, 10),
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                ItemCode = St_ItemCardObj.ItemCode,
                ArabicName = St_ItemCardObj.ArabicName,
                EnglishName = St_ItemCardObj.EnglishName,
                MinimumItemNo = St_ItemCardObj.MinimumItemNo,
                MaximumItemNo = St_ItemCardObj.MaximumItemNo,
                OpeningDate = St_ItemCardObj.OpeningDate,
                SupplierAccountNumber = St_ItemCardObj.SupplierAccountNumber,
                SupplierAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, St_ItemCardObj.SupplierAccountNumber),
                ManufacturerCompanyNo = St_ItemCardObj.ManufacturerCompanyNo,
                CountryOfOriginNo = St_ItemCardObj.CountryOfOriginNo,
                ItemUnitNo = St_ItemCardObj.ItemUnitNo,
                ItemNatureNo = St_ItemCardObj.ItemNatureNo,
                Categorie_1 = St_ItemCardObj.Categorie_1,
                Categorie_2 = St_ItemCardObj.Categorie_2,
                Categorie_3 = St_ItemCardObj.Categorie_3,
                Categorie_4 = St_ItemCardObj.Categorie_4,
                Categorie_5 = St_ItemCardObj.Categorie_5,
                Categorie_6 = St_ItemCardObj.Categorie_6,
                Categorie_7 = St_ItemCardObj.Categorie_7,
                Categorie_8 = St_ItemCardObj.Categorie_8,
                Categorie_9 = St_ItemCardObj.Categorie_9,
                Categorie_10 = St_ItemCardObj.Categorie_10,
                Categorie_11 = St_ItemCardObj.Categorie_11,
                Categorie_12 = St_ItemCardObj.Categorie_12,
                Categorie_13 = St_ItemCardObj.Categorie_13,
                Categorie_14 = St_ItemCardObj.Categorie_14,
                Categorie_15 = St_ItemCardObj.Categorie_15,
                LengthNo = St_ItemCardObj.LengthNo,
                WidthNo = St_ItemCardObj.WidthNo,
                HeightNo = St_ItemCardObj.HeightNo,
                SizeNo = St_ItemCardObj.SizeNo,
                WeightNo = St_ItemCardObj.WeightNo,
                UnitNo = St_ItemCardObj.UnitNo,
                CBM = St_ItemCardObj.CBM,
                CategoriePrice_1 = St_ItemCardObj.CategoriePrice_1,
                CategoriePrice_2 = St_ItemCardObj.CategoriePrice_2,
                CategoriePrice_3 = St_ItemCardObj.CategoriePrice_3,
                CategoriePrice_4 = St_ItemCardObj.CategoriePrice_4,
                CategoriePrice_5 = St_ItemCardObj.CategoriePrice_5,
                CategoriePrice_6 = St_ItemCardObj.CategoriePrice_6,
                CategoriePrice_7 = St_ItemCardObj.CategoriePrice_7,
                CategoriePrice_8 = St_ItemCardObj.CategoriePrice_8,
                CategoriePrice_9 = St_ItemCardObj.CategoriePrice_9,
                CategoriePrice_10 = St_ItemCardObj.CategoriePrice_10,
                StopItem = St_ItemCardObj.StopItem,
                StoppingItemFromSelling = St_ItemCardObj.StoppingItemFromSelling,
                StoppingItemFromBuying = St_ItemCardObj.StoppingItemFromBuying,
                StoppingItemFromPointOfSale = St_ItemCardObj.StoppingItemFromPointOfSale,
                ItemServicesWithoutAnInventory = St_ItemCardObj.ItemServicesWithoutAnInventory,
                TrackAnExpirationDate = St_ItemCardObj.TrackAnExpirationDate,
                TrackSequence = St_ItemCardObj.TrackSequence,
                TrackSequenceUponInput = St_ItemCardObj.TrackSequenceUponInput,
                TrackSequenceUponOutput = St_ItemCardObj.TrackSequenceUponOutput,
                TrackCustoms = St_ItemCardObj.TrackCustoms,
                Smoke = St_ItemCardObj.Smoke,
                NotRelatedToTheUnitAbove = St_ItemCardObj.NotRelatedToTheUnitAbove,
                ScaleItem = St_ItemCardObj.ScaleItem,
                ItemSalesAndPurchases = St_ItemCardObj.ItemSalesAndPurchases,
                OfferType = St_ItemCardObj.OfferType,
                SalePrice = St_ItemCardObj.SalePrice,
                PointOfSalePrice = St_ItemCardObj.PointOfSalePrice,
                TaxTypeNo = St_ItemCardObj.TaxTypeNo,
                TaxRateNo = St_ItemCardObj.TaxRateNo,
                LocalCost = St_ItemCardObj.LocalCost,
                ForeignCost = St_ItemCardObj.ForeignCost,
                CostRate = St_ItemCardObj.CostRate,
                TheTargetMonthlyAmount = St_ItemCardObj.TheTargetMonthlyAmount,
                MinimumSaleAmount = St_ItemCardObj.MinimumSaleAmount,
                TheNumberOfDaysTheCardIsOpened = _unitOfWork.NativeSql.GetOpeningDate(UserInfo.fCompanyId, St_ItemCardObj.ItemCode),
                QuantityAvailable = 0,
                LastLocalPurchasePrice = 0,
                LastForeignPurchasePrice = 0,
                TotalQuantitySold = 0,
                TotalValueSold = 0,
                ItemLogo = St_ItemCardObj.ItemLogo
            };
            return View(St_ItemCardVMObj);
        }
        [HttpPost]
        public JsonResult UpdateSt_ItemCard(St_ItemCardVM ObjToUpdate)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var updateSt_ItemCard = new St_ItemCard();
                updateSt_ItemCard.InsDateTime = DateTime.Now;
                updateSt_ItemCard.OpeningDate = ObjToUpdate.OpeningDate;
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
                updateSt_ItemCard.ItemCode = ObjToUpdate.ItemCode;
                updateSt_ItemCard.MinimumItemNo = ObjToUpdate.MinimumItemNo;
                updateSt_ItemCard.MaximumItemNo = ObjToUpdate.MaximumItemNo;
                updateSt_ItemCard.TaxRate = ObjToUpdate.TaxRate;
                updateSt_ItemCard.TaxRateNo = ObjToUpdate.TaxRateNo;
                updateSt_ItemCard.TaxTypeNo = ObjToUpdate.TaxTypeNo;
                updateSt_ItemCard.SupplierAccountNumber = ObjToUpdate.SupplierAccountNumber;
                updateSt_ItemCard.ManufacturerCompanyNo = ObjToUpdate.ManufacturerCompanyNo;
                updateSt_ItemCard.CountryOfOriginNo = ObjToUpdate.CountryOfOriginNo;
                updateSt_ItemCard.SalePrice = ObjToUpdate.SalePrice;
                updateSt_ItemCard.PointOfSalePrice = ObjToUpdate.PointOfSalePrice;
                updateSt_ItemCard.MinimumSaleAmount = ObjToUpdate.MinimumSaleAmount;
                updateSt_ItemCard.LocalCost = ObjToUpdate.LocalCost;
                updateSt_ItemCard.ForeignCost = ObjToUpdate.ForeignCost;
                updateSt_ItemCard.TheTargetMonthlyAmount = ObjToUpdate.TheTargetMonthlyAmount;
                updateSt_ItemCard.StopItem = ObjToUpdate.StopItem;
                updateSt_ItemCard.StoppingItemFromSelling = ObjToUpdate.StoppingItemFromSelling;
                updateSt_ItemCard.StoppingItemFromBuying = ObjToUpdate.StoppingItemFromBuying;
                updateSt_ItemCard.StoppingItemFromPointOfSale = ObjToUpdate.StoppingItemFromPointOfSale;
                updateSt_ItemCard.ItemServicesWithoutAnInventory = ObjToUpdate.ItemServicesWithoutAnInventory;
                updateSt_ItemCard.TrackAnExpirationDate = ObjToUpdate.TrackAnExpirationDate;
                updateSt_ItemCard.TrackSequence = ObjToUpdate.TrackSequence;
                updateSt_ItemCard.TrackSequenceUponInput = ObjToUpdate.TrackSequenceUponInput;
                updateSt_ItemCard.TrackSequenceUponOutput = ObjToUpdate.TrackSequenceUponOutput;
                updateSt_ItemCard.TrackCustoms = ObjToUpdate.TrackCustoms;
                updateSt_ItemCard.Smoke = ObjToUpdate.Smoke;
                updateSt_ItemCard.NotRelatedToTheUnitAbove = ObjToUpdate.NotRelatedToTheUnitAbove;
                updateSt_ItemCard.ScaleItem = ObjToUpdate.ScaleItem;
                updateSt_ItemCard.ItemSalesAndPurchases = ObjToUpdate.ItemSalesAndPurchases;
                updateSt_ItemCard.OfferType = ObjToUpdate.OfferType;
                updateSt_ItemCard.ItemUnitNo = ObjToUpdate.ItemUnitNo;
                updateSt_ItemCard.ItemNatureNo = ObjToUpdate.ItemNatureNo;
                updateSt_ItemCard.ItemUnitNo = ObjToUpdate.ItemUnitNo;
                updateSt_ItemCard.Categorie_1 = ObjToUpdate.Categorie_1;
                updateSt_ItemCard.Categorie_2 = ObjToUpdate.Categorie_2;
                updateSt_ItemCard.Categorie_3 = ObjToUpdate.Categorie_3;
                updateSt_ItemCard.Categorie_4 = ObjToUpdate.Categorie_4;
                updateSt_ItemCard.Categorie_5 = ObjToUpdate.Categorie_5;
                updateSt_ItemCard.Categorie_6 = ObjToUpdate.Categorie_6;
                updateSt_ItemCard.Categorie_7 = ObjToUpdate.Categorie_7;
                updateSt_ItemCard.Categorie_8 = ObjToUpdate.Categorie_8;
                updateSt_ItemCard.Categorie_9 = ObjToUpdate.Categorie_9;
                updateSt_ItemCard.Categorie_10 = ObjToUpdate.Categorie_10;
                updateSt_ItemCard.Categorie_11 = ObjToUpdate.Categorie_11;
                updateSt_ItemCard.Categorie_12 = ObjToUpdate.Categorie_12;
                updateSt_ItemCard.Categorie_13 = ObjToUpdate.Categorie_13;
                updateSt_ItemCard.Categorie_14 = ObjToUpdate.Categorie_14;
                updateSt_ItemCard.Categorie_15 = ObjToUpdate.Categorie_15;
                updateSt_ItemCard.LengthNo = ObjToUpdate.LengthNo;
                updateSt_ItemCard.WidthNo = ObjToUpdate.WidthNo;
                updateSt_ItemCard.HeightNo = ObjToUpdate.HeightNo;
                updateSt_ItemCard.SizeNo = ObjToUpdate.SizeNo;
                updateSt_ItemCard.WeightNo = ObjToUpdate.WeightNo;
                updateSt_ItemCard.UnitNo = ObjToUpdate.UnitNo;
                updateSt_ItemCard.CBM = ObjToUpdate.CBM;
                updateSt_ItemCard.CategoriePrice_1 = ObjToUpdate.CategoriePrice_1;
                updateSt_ItemCard.CategoriePrice_2 = ObjToUpdate.CategoriePrice_2;
                updateSt_ItemCard.CategoriePrice_3 = ObjToUpdate.CategoriePrice_3;
                updateSt_ItemCard.CategoriePrice_4 = ObjToUpdate.CategoriePrice_4;
                updateSt_ItemCard.CategoriePrice_5 = ObjToUpdate.CategoriePrice_5;
                updateSt_ItemCard.CategoriePrice_6 = ObjToUpdate.CategoriePrice_6;
                updateSt_ItemCard.CategoriePrice_7 = ObjToUpdate.CategoriePrice_7;
                updateSt_ItemCard.CategoriePrice_8 = ObjToUpdate.CategoriePrice_8;
                updateSt_ItemCard.CategoriePrice_9 = ObjToUpdate.CategoriePrice_9;
                updateSt_ItemCard.CategoriePrice_10 = ObjToUpdate.CategoriePrice_10;
                updateSt_ItemCard.ItemLogo = ObjToUpdate.ItemLogo;

                _unitOfWork.NativeSql.DeleteSt_ItemCard(UserInfo.fCompanyId, updateSt_ItemCard.ItemCode);
                _unitOfWork.NativeSql.DeleteSt_SimilarItem(UserInfo.fCompanyId, updateSt_ItemCard.ItemCode);
                _unitOfWork.NativeSql.DeleteSt_ItemWarehouse(UserInfo.fCompanyId, updateSt_ItemCard.ItemCode);
                _unitOfWork.NativeSql.DeleteSt_AlternativeItem(UserInfo.fCompanyId, updateSt_ItemCard.ItemCode);
                _unitOfWork.NativeSql.DeleteSt_ItemOtherUnit(UserInfo.fCompanyId, updateSt_ItemCard.ItemCode);
                _unitOfWork.NativeSql.DeleteSt_ItemOffer(UserInfo.fCompanyId, updateSt_ItemCard.ItemCode);
                _unitOfWork.Complete();
                var iRow = 1;
                var UpdateMainItemCodeInSt_SimilarItem = new St_SimilarItem();
                UpdateMainItemCodeInSt_SimilarItem.CompanyID = UserInfo.fCompanyId;
                UpdateMainItemCodeInSt_SimilarItem.ItemCode = updateSt_ItemCard.ItemCode;
                UpdateMainItemCodeInSt_SimilarItem.SimilarItemCode = updateSt_ItemCard.ItemCode;
                UpdateMainItemCodeInSt_SimilarItem.SimilarItemUnitCode = updateSt_ItemCard.ItemUnitNo;
                UpdateMainItemCodeInSt_SimilarItem.RowNumber = iRow;
                UpdateMainItemCodeInSt_SimilarItem.SimilarItemType = 1;
                _unitOfWork.St_ItemCard.AddSimilarItem(UpdateMainItemCodeInSt_SimilarItem);
                if (ObjToUpdate.St_SimilarItem != null)
                {
                    foreach (var UpdateSt_SimilarItem in ObjToUpdate.St_SimilarItem)
                    {
                        if (UpdateSt_SimilarItem.SimilarItemType != 3)
                        {
                            iRow = iRow + 1;
                            UpdateSt_SimilarItem.CompanyID = UserInfo.fCompanyId;
                            UpdateSt_SimilarItem.ItemCode = updateSt_ItemCard.ItemCode;
                            UpdateSt_SimilarItem.SimilarItemCode = UpdateSt_SimilarItem.SimilarItemCode;
                            UpdateSt_SimilarItem.SimilarItemUnitCode = updateSt_ItemCard.ItemUnitNo;
                            UpdateSt_SimilarItem.RowNumber = iRow;
                            UpdateSt_SimilarItem.SimilarItemType = 2;
                            _unitOfWork.St_ItemCard.AddSimilarItem(UpdateSt_SimilarItem);
                        }
                    }
                }
                if (ObjToUpdate.St_AlternativeItem != null)
                {
                    foreach (var UpdateSt_AlternativeItem in ObjToUpdate.St_AlternativeItem)
                    {
                        UpdateSt_AlternativeItem.CompanyID = UserInfo.fCompanyId;
                        UpdateSt_AlternativeItem.ItemCode = updateSt_ItemCard.ItemCode;
                        UpdateSt_AlternativeItem.AlternativeItemCode = UpdateSt_AlternativeItem.AlternativeItemCode;
                        UpdateSt_AlternativeItem.RowNumber = UpdateSt_AlternativeItem.RowNumber;
                        _unitOfWork.St_ItemCard.AddAlternativeItem(UpdateSt_AlternativeItem);
                    }
                }
                if (ObjToUpdate.St_ItemOtherUnit != null)
                {
                    foreach (var UpdateSt_ItemOtherUnit in ObjToUpdate.St_ItemOtherUnit)
                    {
                        iRow = iRow + 1;
                        var updateSt_ItemOtherUnitBarcodeInSt_SimilarItem = new St_SimilarItem();
                        UpdateSt_ItemOtherUnit.CompanyID = UserInfo.fCompanyId;
                        UpdateSt_ItemOtherUnit.ItemCode = updateSt_ItemCard.ItemCode;
                        UpdateSt_ItemOtherUnit.OtherUnitNumber = UpdateSt_ItemOtherUnit.OtherUnitNumber;
                        UpdateSt_ItemOtherUnit.OtherUnitQuantity = UpdateSt_ItemOtherUnit.OtherUnitQuantity;
                        UpdateSt_ItemOtherUnit.OtherUnitSalePrice = UpdateSt_ItemOtherUnit.OtherUnitSalePrice;
                        UpdateSt_ItemOtherUnit.OtherUnitPurchasePrice = UpdateSt_ItemOtherUnit.OtherUnitPurchasePrice;
                        UpdateSt_ItemOtherUnit.OtherUnitBarcode = UpdateSt_ItemOtherUnit.OtherUnitBarcode;
                        UpdateSt_ItemOtherUnit.RowNumber = UpdateSt_ItemOtherUnit.RowNumber;
                        updateSt_ItemOtherUnitBarcodeInSt_SimilarItem.CompanyID = UserInfo.fCompanyId;
                        updateSt_ItemOtherUnitBarcodeInSt_SimilarItem.ItemCode = updateSt_ItemCard.ItemCode;
                        updateSt_ItemOtherUnitBarcodeInSt_SimilarItem.SimilarItemCode = UpdateSt_ItemOtherUnit.OtherUnitBarcode;
                        updateSt_ItemOtherUnitBarcodeInSt_SimilarItem.SimilarItemUnitCode = UpdateSt_ItemOtherUnit.OtherUnitNumber;
                        updateSt_ItemOtherUnitBarcodeInSt_SimilarItem.RowNumber = iRow;
                        updateSt_ItemOtherUnitBarcodeInSt_SimilarItem.SimilarItemType = 3;
                        _unitOfWork.St_ItemCard.AddSimilarItem(updateSt_ItemOtherUnitBarcodeInSt_SimilarItem);
                        _unitOfWork.St_ItemCard.AddItemOtherUnit(UpdateSt_ItemOtherUnit);
                    }
                }
                if (updateSt_ItemCard.OfferType == 1)
                {
                    if (ObjToUpdate.St_ItemOffer != null)
                    {
                        foreach (var UpdateSt_ItemOffer in ObjToUpdate.St_ItemOffer)
                        {
                            UpdateSt_ItemOffer.CompanyID = UserInfo.fCompanyId;
                            UpdateSt_ItemOffer.ItemCode = updateSt_ItemCard.ItemCode;
                            UpdateSt_ItemOffer.ItemOfferQuantitySold = UpdateSt_ItemOffer.ItemOfferQuantitySold;
                            UpdateSt_ItemOffer.ItemOfferBonus = UpdateSt_ItemOffer.ItemOfferBonus;
                            UpdateSt_ItemOffer.ItemOfferUnitPrice = 0;
                            UpdateSt_ItemOffer.ItemOfferTotalPrice = 0;
                            UpdateSt_ItemOffer.RowNumber = UpdateSt_ItemOffer.RowNumber;
                            _unitOfWork.St_ItemCard.AddItemOffer(UpdateSt_ItemOffer);
                        }
                    }
                    else
                    {
                        updateSt_ItemCard.OfferType = 3;
                    }
                }
                else if (updateSt_ItemCard.OfferType == 2)
                {
                    if (ObjToUpdate.St_ItemOffer != null)
                    {
                        foreach (var UpdateSt_ItemOffer in ObjToUpdate.St_ItemOffer)
                        {
                            UpdateSt_ItemOffer.CompanyID = UserInfo.fCompanyId;
                            UpdateSt_ItemOffer.ItemCode = updateSt_ItemCard.ItemCode;
                            UpdateSt_ItemOffer.ItemOfferQuantitySold = UpdateSt_ItemOffer.ItemOfferQuantitySold;
                            UpdateSt_ItemOffer.ItemOfferBonus = 0;
                            UpdateSt_ItemOffer.ItemOfferUnitPrice = UpdateSt_ItemOffer.ItemOfferUnitPrice;
                            UpdateSt_ItemOffer.ItemOfferTotalPrice = UpdateSt_ItemOffer.ItemOfferTotalPrice;
                            UpdateSt_ItemOffer.RowNumber = UpdateSt_ItemOffer.RowNumber;
                            _unitOfWork.St_ItemCard.AddItemOffer(UpdateSt_ItemOffer);
                        }
                    }
                    else
                    {
                        updateSt_ItemCard.OfferType = 3;
                    }
                }
                iRow = 0;
                if (ObjToUpdate.St_ItemWarehouse != null)
                {
                    foreach (var UpdateItemWarehouse in ObjToUpdate.St_ItemWarehouse)
                    {
                        iRow = iRow + 1;
                        UpdateItemWarehouse.CompanyID = UserInfo.fCompanyId;
                        UpdateItemWarehouse.ItemCode = updateSt_ItemCard.ItemCode;
                        UpdateItemWarehouse.StockCode = UpdateItemWarehouse.StockCode;
                        UpdateItemWarehouse.StockMinimumItemNo = UpdateItemWarehouse.StockMinimumItemNo;
                        UpdateItemWarehouse.StockMaximumItemNo = UpdateItemWarehouse.StockMaximumItemNo;
                        UpdateItemWarehouse.RowNumber = iRow;
                        _unitOfWork.St_ItemCard.AddItemWarehous(UpdateItemWarehouse);
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
                _unitOfWork.St_ItemCard.AddItemCard(updateSt_ItemCard);
                _unitOfWork.Complete();
                Msg.Code = 1;
                Msg.Msg = Resources.Resource.AddedSuccessfully;
                Msg.LastID = "0";
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
        public JsonResult GetAllSt_WarehouseByItemCodeView(string id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllSt_WarehouseH = _unitOfWork.NativeSql.GetAllSt_WarehouseByItemCodeView(UserInfo.fCompanyId, id);
                if (AllSt_WarehouseH == null)
                {
                    return Json(new List<St_ItemCardVM>(), JsonRequestBehavior.AllowGet);
                }
                return Json(AllSt_WarehouseH, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<St_ItemCardVM>(), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult GetAllSt_SimilarItemByItemCodeView(string id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllSt_SubItemColor = _unitOfWork.NativeSql.GetAllSt_SimilarItemByItemCodeView(UserInfo.fCompanyId, id);
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
        public JsonResult GetAllSt_AlternativeItemByItemCodeView(string id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllSt_SubItemColor = _unitOfWork.NativeSql.GetAllSt_AlternativeItemByItemCodeView(UserInfo.fCompanyId, id);
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
        public JsonResult GetAllSt_ItemOtherUnitByItemCodeView(string id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllSt_SubItemColor = _unitOfWork.NativeSql.GetAllSt_ItemOtherUnitByItemCodeView(UserInfo.fCompanyId, id);
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
        public JsonResult GetAllSt_ItemOffer1ByItemCodeView(string id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllSt_SubItemColor = _unitOfWork.NativeSql.GetAllSt_ItemOffer1ByItemCodeView(UserInfo.fCompanyId, id);
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
        public JsonResult GetAllSt_ItemOffer2ByItemCodeView(string id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllSt_SubItemColor = _unitOfWork.NativeSql.GetAllSt_ItemOffer2ByItemCodeView(UserInfo.fCompanyId, id);
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
            var St_ItemCardObj = _unitOfWork.St_ItemCard.GetSt_ItemCardById(id, UserInfo.fCompanyId);
            var St_ItemCardVMObj = new St_ItemCardVM
            {
                St_ItemUnit = _unitOfWork.St_ItemUnit.GetAllItemUnit(UserInfo.fCompanyId),
                St_ManufacturerCompany = _unitOfWork.St_ManufacturerCompany.GetAllSt_ManufacturerCompany(UserInfo.fCompanyId),
                St_CountryOfOrigin = _unitOfWork.St_CountryOfOrigin.GetAllSt_CountryOfOrigin(UserInfo.fCompanyId),
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
                St_MeasurementDetailLength = _unitOfWork.St_MeasurementDetail.GetSt_MeasurementDetailBySt_Measurement(UserInfo.fCompanyId, 1),
                St_MeasurementDetailWidth = _unitOfWork.St_MeasurementDetail.GetSt_MeasurementDetailBySt_Measurement(UserInfo.fCompanyId, 2),
                St_MeasurementDetailHeight = _unitOfWork.St_MeasurementDetail.GetSt_MeasurementDetailBySt_Measurement(UserInfo.fCompanyId, 3),
                St_MeasurementDetailSize = _unitOfWork.St_MeasurementDetail.GetSt_MeasurementDetailBySt_Measurement(UserInfo.fCompanyId, 4),
                St_MeasurementDetailWeight = _unitOfWork.St_MeasurementDetail.GetSt_MeasurementDetailBySt_Measurement(UserInfo.fCompanyId, 5),
                St_MeasurementDetailUnit = _unitOfWork.St_MeasurementDetail.GetSt_MeasurementDetailBySt_Measurement(UserInfo.fCompanyId, 6),
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
                CategoriePrice_1Name = _unitOfWork.St_CategoryPrice.GetSt_CategoryPriceNameByID(UserInfo.fCompanyId, 1),
                CategoriePrice_2Name = _unitOfWork.St_CategoryPrice.GetSt_CategoryPriceNameByID(UserInfo.fCompanyId, 2),
                CategoriePrice_3Name = _unitOfWork.St_CategoryPrice.GetSt_CategoryPriceNameByID(UserInfo.fCompanyId, 3),
                CategoriePrice_4Name = _unitOfWork.St_CategoryPrice.GetSt_CategoryPriceNameByID(UserInfo.fCompanyId, 4),
                CategoriePrice_5Name = _unitOfWork.St_CategoryPrice.GetSt_CategoryPriceNameByID(UserInfo.fCompanyId, 5),
                CategoriePrice_6Name = _unitOfWork.St_CategoryPrice.GetSt_CategoryPriceNameByID(UserInfo.fCompanyId, 6),
                CategoriePrice_7Name = _unitOfWork.St_CategoryPrice.GetSt_CategoryPriceNameByID(UserInfo.fCompanyId, 7),
                CategoriePrice_8Name = _unitOfWork.St_CategoryPrice.GetSt_CategoryPriceNameByID(UserInfo.fCompanyId, 8),
                CategoriePrice_9Name = _unitOfWork.St_CategoryPrice.GetSt_CategoryPriceNameByID(UserInfo.fCompanyId, 9),
                CategoriePrice_10Name = _unitOfWork.St_CategoryPrice.GetSt_CategoryPriceNameByID(UserInfo.fCompanyId, 10),
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                ItemCode = St_ItemCardObj.ItemCode,
                ArabicName = St_ItemCardObj.ArabicName,
                EnglishName = St_ItemCardObj.EnglishName,
                MinimumItemNo = St_ItemCardObj.MinimumItemNo,
                MaximumItemNo = St_ItemCardObj.MaximumItemNo,
                SupplierAccountNumber = St_ItemCardObj.SupplierAccountNumber,
                SupplierAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, St_ItemCardObj.SupplierAccountNumber),
                ManufacturerCompanyNo = St_ItemCardObj.ManufacturerCompanyNo,
                CountryOfOriginNo = St_ItemCardObj.CountryOfOriginNo,
                ItemUnitNo = St_ItemCardObj.ItemUnitNo,
                ItemNatureNo = St_ItemCardObj.ItemNatureNo,
                Categorie_1 = St_ItemCardObj.Categorie_1,
                Categorie_2 = St_ItemCardObj.Categorie_2,
                Categorie_3 = St_ItemCardObj.Categorie_3,
                Categorie_4 = St_ItemCardObj.Categorie_4,
                Categorie_5 = St_ItemCardObj.Categorie_5,
                Categorie_6 = St_ItemCardObj.Categorie_6,
                Categorie_7 = St_ItemCardObj.Categorie_7,
                Categorie_8 = St_ItemCardObj.Categorie_8,
                Categorie_9 = St_ItemCardObj.Categorie_9,
                Categorie_10 = St_ItemCardObj.Categorie_10,
                Categorie_11 = St_ItemCardObj.Categorie_11,
                Categorie_12 = St_ItemCardObj.Categorie_12,
                Categorie_13 = St_ItemCardObj.Categorie_13,
                Categorie_14 = St_ItemCardObj.Categorie_14,
                Categorie_15 = St_ItemCardObj.Categorie_15,
                LengthNo = St_ItemCardObj.LengthNo,
                WidthNo = St_ItemCardObj.WidthNo,
                HeightNo = St_ItemCardObj.HeightNo,
                SizeNo = St_ItemCardObj.SizeNo,
                WeightNo = St_ItemCardObj.WeightNo,
                UnitNo = St_ItemCardObj.UnitNo,
                CBM = St_ItemCardObj.CBM,
                CategoriePrice_1 = St_ItemCardObj.CategoriePrice_1,
                CategoriePrice_2 = St_ItemCardObj.CategoriePrice_2,
                CategoriePrice_3 = St_ItemCardObj.CategoriePrice_3,
                CategoriePrice_4 = St_ItemCardObj.CategoriePrice_4,
                CategoriePrice_5 = St_ItemCardObj.CategoriePrice_5,
                CategoriePrice_6 = St_ItemCardObj.CategoriePrice_6,
                CategoriePrice_7 = St_ItemCardObj.CategoriePrice_7,
                CategoriePrice_8 = St_ItemCardObj.CategoriePrice_8,
                CategoriePrice_9 = St_ItemCardObj.CategoriePrice_9,
                CategoriePrice_10 = St_ItemCardObj.CategoriePrice_10,
                StopItem = St_ItemCardObj.StopItem,
                StoppingItemFromSelling = St_ItemCardObj.StoppingItemFromSelling,
                StoppingItemFromBuying = St_ItemCardObj.StoppingItemFromBuying,
                StoppingItemFromPointOfSale = St_ItemCardObj.StoppingItemFromPointOfSale,
                ItemServicesWithoutAnInventory = St_ItemCardObj.ItemServicesWithoutAnInventory,
                TrackAnExpirationDate = St_ItemCardObj.TrackAnExpirationDate,
                TrackSequence = St_ItemCardObj.TrackSequence,
                TrackSequenceUponInput = St_ItemCardObj.TrackSequenceUponInput,
                TrackSequenceUponOutput = St_ItemCardObj.TrackSequenceUponOutput,
                TrackCustoms = St_ItemCardObj.TrackCustoms,
                Smoke = St_ItemCardObj.Smoke,
                NotRelatedToTheUnitAbove = St_ItemCardObj.NotRelatedToTheUnitAbove,
                ScaleItem = St_ItemCardObj.ScaleItem,
                ItemSalesAndPurchases = St_ItemCardObj.ItemSalesAndPurchases,
                OfferType = St_ItemCardObj.OfferType,
                SalePrice = St_ItemCardObj.SalePrice,
                PointOfSalePrice = St_ItemCardObj.PointOfSalePrice,
                TaxTypeNo = St_ItemCardObj.TaxTypeNo,
                TaxRateNo = St_ItemCardObj.TaxRateNo,
                LocalCost = St_ItemCardObj.LocalCost,
                ForeignCost = St_ItemCardObj.ForeignCost,
                CostRate = St_ItemCardObj.CostRate,
                TheTargetMonthlyAmount = St_ItemCardObj.TheTargetMonthlyAmount,
                MinimumSaleAmount = St_ItemCardObj.MinimumSaleAmount,
                TheNumberOfDaysTheCardIsOpened = _unitOfWork.NativeSql.GetOpeningDate(UserInfo.fCompanyId, St_ItemCardObj.ItemCode),
                QuantityAvailable = 0,
                LastLocalPurchasePrice = 0,
                LastForeignPurchasePrice = 0,
                TotalQuantitySold = 0,
                TotalValueSold = 0,
                ItemLogo = St_ItemCardObj.ItemLogo
            };
            return View(St_ItemCardVMObj);
        }
        [HttpPost]
        public JsonResult DeleteSt_ItemCard(St_ItemCardVM ObjToDelete)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var deleteSt_ItemCard = new St_ItemCard();

                deleteSt_ItemCard.ItemCode = ObjToDelete.ItemCode;

                _unitOfWork.NativeSql.DeleteSt_ItemCard(UserInfo.fCompanyId, deleteSt_ItemCard.ItemCode);
                _unitOfWork.NativeSql.DeleteSt_SimilarItem(UserInfo.fCompanyId, deleteSt_ItemCard.ItemCode);
                _unitOfWork.NativeSql.DeleteSt_ItemWarehouse(UserInfo.fCompanyId, deleteSt_ItemCard.ItemCode);
                _unitOfWork.NativeSql.DeleteSt_AlternativeItem(UserInfo.fCompanyId, deleteSt_ItemCard.ItemCode);
                _unitOfWork.NativeSql.DeleteSt_ItemOtherUnit(UserInfo.fCompanyId, deleteSt_ItemCard.ItemCode);
                _unitOfWork.NativeSql.DeleteSt_ItemOffer(UserInfo.fCompanyId, deleteSt_ItemCard.ItemCode);
                _unitOfWork.NativeSql.DeleteSt_ItemAdvertisingRepresentative(UserInfo.fCompanyId, deleteSt_ItemCard.ItemCode);
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
        [HttpGet]
        public JsonResult CheckItemCodeBeforDelete(string id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            if (id != "")
            {
                string ItemCode = _unitOfWork.NativeSql.CheckItemCodeBeforDelete(UserInfo.fCompanyId, id);

                return Json(ItemCode, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}