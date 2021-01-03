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

    public class St_AdvertisingRepresentativeController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public St_AdvertisingRepresentativeController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            return View();
        }
        public ActionResult AddNew()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            St_AdvertisingRepresentative Obj = new St_AdvertisingRepresentative
            {
                AdvertisingRepresentativeID = _unitOfWork.St_AdvertisingRepresentative.GetMaxSerial(UserInfo.fCompanyId)
            };
            return PartialView(Obj);
        }
        public JsonResult GetAllSt_AdvertisingRepresentative()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllAdvertisingRepresentative = _unitOfWork.St_AdvertisingRepresentative.GetAllAdvertisingRepresentative(UserInfo.fCompanyId);
                if (AllAdvertisingRepresentative == null)
                {
                    return Json(new List<St_AdvertisingRepresentative>(), JsonRequestBehavior.AllowGet);
                }

                return Json(AllAdvertisingRepresentative, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<St_AdvertisingRepresentative>(), JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult SaveSt_AdvertisingRepresentative(St_AdvertisingRepresentative ObjSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                ObjSave.AdvertisingRepresentativeID = _unitOfWork.St_AdvertisingRepresentative.GetMaxSerial(UserInfo.fCompanyId);
                ObjSave.InsDateTime = DateTime.Now;
                ObjSave.InsUserID = userId;
                ObjSave.CompanyID = UserInfo.fCompanyId;
                if (String.IsNullOrEmpty(ObjSave.EnglishName))
                    ObjSave.EnglishName = ObjSave.ArabicName;
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
                _unitOfWork.St_AdvertisingRepresentative.Add(ObjSave);
                _unitOfWork.Complete();
                Msg.LastID = _unitOfWork.St_AdvertisingRepresentative.GetMaxSerial(UserInfo.fCompanyId).ToString();
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
        public ActionResult Update(int id)
        {
            try
            {
                if (id != 0)
                {
                    var userId = User.Identity.GetUserId();

                    var UserInfo = _unitOfWork.User.GetUserByID(userId);
                    if (UserInfo == null)
                    {
                        RedirectToAction("", "");
                    }

                    var Obj = _unitOfWork.St_AdvertisingRepresentative.GetSt_AdvertisingRepresentativeByID(UserInfo.fCompanyId, id);


                    return PartialView("Update", Obj);
                }



                return PartialView("Update", new St_AdvertisingRepresentative());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return View("Error");
            }
        }
        public ActionResult Delete(int id)
        {
            try
            {


                if (id != 0)
                {
                    var userId = User.Identity.GetUserId();

                    var UserInfo = _unitOfWork.User.GetUserByID(userId);
                    if (UserInfo == null)
                    {
                        RedirectToAction("", "");
                    }

                    var Obj = _unitOfWork.St_AdvertisingRepresentative.GetSt_AdvertisingRepresentativeByID(UserInfo.fCompanyId, id);


                    return PartialView("Delete", Obj);
                }



                return PartialView("Delete", new St_AdvertisingRepresentative());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return View("Error");
            }
        }
        public JsonResult UpdateSt_AdvertisingRepresentative(St_AdvertisingRepresentative ObjUpdate)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);

                ObjUpdate.CompanyID = UserInfo.fCompanyId;
                ObjUpdate.InsDateTime = DateTime.Now;
                ObjUpdate.InsUserID = userId;
                if (String.IsNullOrEmpty(ObjUpdate.EnglishName))
                    ObjUpdate.EnglishName = ObjUpdate.ArabicName;
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
                _unitOfWork.St_AdvertisingRepresentative.Update(ObjUpdate);
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
        public JsonResult DeleteSt_AdvertisingRepresentative(St_AdvertisingRepresentative ObjDelete)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);

                ObjDelete.CompanyID = UserInfo.fCompanyId;

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
                _unitOfWork.St_AdvertisingRepresentative.Delete(ObjDelete);
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
        public ActionResult SaveSt_ItemAdvertisingRepresentative()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var St_ItemAdvertisingRepresentativeObj = new St_ItemCardVM
            {
                St_ItemUnit = _unitOfWork.St_ItemUnit.GetAllItemUnit(UserInfo.fCompanyId),
                St_CountryOfOrigin = _unitOfWork.St_CountryOfOrigin.GetAllSt_CountryOfOrigin(UserInfo.fCompanyId),
                St_ManufacturerCompany = _unitOfWork.St_ManufacturerCompany.GetAllSt_ManufacturerCompany(UserInfo.fCompanyId),
                St_AdvertisingRepresentative = _unitOfWork.St_AdvertisingRepresentative.GetAllAdvertisingRepresentative(UserInfo.fCompanyId),
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
            return View(St_ItemAdvertisingRepresentativeObj);
        }
        [HttpPost]
        public JsonResult GetAllSt_ItemCardByAdvertisingRepresentative(St_ItemCardVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllSt_ItemCard = _unitOfWork.NativeSql.GetAllSt_ItemCardFilterByAdvertisingRepresentative(UserInfo.fCompanyId, Obj.ItemCode, Obj.AdvertisingRepresentativeID);
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
        public JsonResult SaveSt_ItemAdvertisingRepresentative(St_ItemCardVM ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                _unitOfWork.NativeSql.DeleteSt_ItemAdvertisingRepresentativeByAdvertisingRepresentativeID(UserInfo.fCompanyId, ObjToSave.AdvertisingRepresentativeID);
                var iRow = 0;
                if (ObjToSave.St_ItemAdvertisingRepresentative != null)
                {
                    foreach (var SaveItemAdvertisingRepresentative in ObjToSave.St_ItemAdvertisingRepresentative)
                    {
                        iRow = iRow + 1;
                        SaveItemAdvertisingRepresentative.CompanyID = UserInfo.fCompanyId;
                        SaveItemAdvertisingRepresentative.ItemCode = SaveItemAdvertisingRepresentative.ItemCode;
                        SaveItemAdvertisingRepresentative.AdvertisingRepresentativeID = ObjToSave.AdvertisingRepresentativeID;
                        SaveItemAdvertisingRepresentative.RowNumber = iRow;
                        _unitOfWork.St_ItemCard.AddItemAdvertisingRepresentative(SaveItemAdvertisingRepresentative);
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

    }
}