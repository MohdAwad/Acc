using Acc.Helpers;
using Acc.Models;
using Acc.Persistence;
using Acc.Repositories;
using Acc.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace Acc.Controllers
{
    [Authorize]
    public class SupplierCityController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public SupplierCityController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        // GET: SupplierCity
        [Authorize(Roles = "CoOwner,ShowSuppliercity")]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            return View();
        }
        [HttpGet]
        public JsonResult GetAllSupplierCity()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllSupplierCity = _unitOfWork.NativeSql.GetAllSupplierCity(UserInfo.fCompanyId);
                if (AllSupplierCity == null)
                {
                    return Json(new List<SupplierCityVM>(), JsonRequestBehavior.AllowGet);
                }

                return Json(AllSupplierCity, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<SupplierCityVM>(), JsonRequestBehavior.AllowGet);
            }

        }

        [Authorize(Roles = "CoOwner,AddSuppliercity")]
        public ActionResult AddNew()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            SupplierCityVM Obj = new SupplierCityVM
            {
                SupplierCityID = _unitOfWork.SupplierCity.GetMaxSerial(UserInfo.fCompanyId),
                SupplierCountry = _unitOfWork.SupplierCountry.GetAllSupplierCountry(UserInfo.fCompanyId),
                SupplierCountryID = 1
            };
            return PartialView(Obj);
        }
        [HttpPost]
        [Authorize(Roles = "CoOwner,AddSuppliercity")]
        public JsonResult SaveSupplierCity(SupplierCityVM ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var SupplierCityObj = new SupplierCity();
                SupplierCityObj.InsDateTime = DateTime.Now;
                SupplierCityObj.InsUserID = userId;
                SupplierCityObj.CompanyID = UserInfo.fCompanyId;
                SupplierCityObj.SupplierCityID = ObjToSave.SupplierCityID;
                SupplierCityObj.SupplierCountryID = ObjToSave.SupplierCountryID;
                if (String.IsNullOrEmpty(ObjToSave.EnglishName))
                    ObjToSave.EnglishName = ObjToSave.ArabicName;
                SupplierCityObj.ArabicName = ObjToSave.ArabicName;
                SupplierCityObj.EnglishName = ObjToSave.EnglishName;

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
                _unitOfWork.SupplierCity.Add(SupplierCityObj);
                _unitOfWork.Complete();
                _unitOfWork.SupplierCity.GetMaxSerial(UserInfo.fCompanyId).ToString();
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

        [Authorize(Roles = "CoOwner,UpdateSuppliercity")]
        public ActionResult UpdateSupplierCity(int id)
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

                    var Obj = _unitOfWork.NativeSql.GetSupplierCityByID(UserInfo.fCompanyId, id);
                    var SupplierCityObj = new SupplierCityVM { };
                    SupplierCityObj.SupplierCountry = _unitOfWork.SupplierCountry.GetAllSupplierCountry(UserInfo.fCompanyId);
                    SupplierCityObj.SupplierCityID = Obj.SupplierCityID;
                    SupplierCityObj.SupplierCountryID = Obj.SupplierCountryID;
                    SupplierCityObj.ArabicName = Obj.ArabicName;
                    SupplierCityObj.EnglishName = Obj.EnglishName;


                    return PartialView("UpdateSupplierCity", SupplierCityObj);
                }



                return PartialView("UpdateSupplierCity", new SupplierCityVM());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return View("Error");
            }
        }

        [Authorize(Roles = "CoOwner,DeleteSuppliercity")]
        public ActionResult DeleteSupplierCity(int id)
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

                    var Obj = _unitOfWork.NativeSql.GetSupplierCityByID(UserInfo.fCompanyId, id);
                    var SupplierCityObj = new SupplierCityVM { };
                    SupplierCityObj.SupplierCityID = Obj.SupplierCityID;
                    SupplierCityObj.SupplierCountryID = Obj.SupplierCountryID;
                    SupplierCityObj.ArabicName = Obj.ArabicName;
                    SupplierCityObj.EnglishName = Obj.EnglishName;
                    SupplierCityObj.SupplierCountryName = _unitOfWork.NativeSql.GetSupplierCountryName(UserInfo.fCompanyId, SupplierCityObj.SupplierCountryID);


                    return PartialView("DeleteSupplierCity", SupplierCityObj);
                }



                return PartialView("DeleteSupplierCity", new SupplierCity());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return View("Error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "CoOwner,UpdateSuppliercity")]
        public JsonResult UpdateSupplierCity(SupplierCityVM ObjUpdate)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var SupplierCityObj = new SupplierCity();
                SupplierCityObj.InsDateTime = DateTime.Now;
                SupplierCityObj.InsUserID = userId;
                SupplierCityObj.CompanyID = UserInfo.fCompanyId;
                SupplierCityObj.SupplierCityID = ObjUpdate.SupplierCityID;
                SupplierCityObj.SupplierCountryID = ObjUpdate.SupplierCountryID;
                if (String.IsNullOrEmpty(ObjUpdate.EnglishName))
                    ObjUpdate.EnglishName = ObjUpdate.ArabicName;
                SupplierCityObj.ArabicName = ObjUpdate.ArabicName;
                SupplierCityObj.EnglishName = ObjUpdate.EnglishName;
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
                _unitOfWork.SupplierCity.Update(SupplierCityObj);
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
        [Authorize(Roles = "CoOwner,DeleteSuppliercity")]
        public JsonResult DeleteSupplierCity(SupplierCityVM ObjDelete)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);

                ObjDelete.CompanyID = UserInfo.fCompanyId;
                var SupplierCityObj = new SupplierCity();
                SupplierCityObj.CompanyID = UserInfo.fCompanyId;
                SupplierCityObj.SupplierCityID = ObjDelete.SupplierCityID;

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
                _unitOfWork.SupplierCity.Delete(SupplierCityObj);
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
        public JsonResult CheckSupplierCityBeforDelete(int id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            if (id != 0)
            {
                int SupplierCityID = _unitOfWork.NativeSql.CheckSupplierCity(UserInfo.fCompanyId, id);

                return Json(SupplierCityID, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
    }
}