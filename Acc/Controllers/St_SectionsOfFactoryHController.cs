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
    public class St_SectionsOfFactoryHController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public St_SectionsOfFactoryHController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            return View();
        }
        [HttpGet]
        public JsonResult GetAllSt_SectionsOfFactoryH()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllSt_SectionsOfFactory = _unitOfWork.NativeSql.GetAllSt_SectionsOfFactoryH(UserInfo.fCompanyId);
                if (AllSt_SectionsOfFactory == null)
                {
                    return Json(new List<St_SectionsOfFactoryHVM>(), JsonRequestBehavior.AllowGet);
                }

                return Json(AllSt_SectionsOfFactory, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<St_SectionsOfFactoryHVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult Add()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            St_SectionsOfFactoryHVM Obj = new St_SectionsOfFactoryHVM
            {
                SectionsOfFactoryID = _unitOfWork.St_SectionsOfFactoryH.GetMaxSerial(UserInfo.fCompanyId),
                St_Factory = _unitOfWork.St_FactoryH.GetAllSt_FactoryH(UserInfo.fCompanyId),
                FactoryID = 1
            };
            return PartialView(Obj);
        }
        [HttpPost]
        public JsonResult Save(St_SectionsOfFactoryHVM ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var St_SectionsOfFactoryObj = new St_SectionsOfFactoryH();
                St_SectionsOfFactoryObj.InsDateTime = DateTime.Now;
                St_SectionsOfFactoryObj.InsUserID = userId;
                St_SectionsOfFactoryObj.CompanyID = UserInfo.fCompanyId;
                St_SectionsOfFactoryObj.SectionsOfFactoryID = ObjToSave.SectionsOfFactoryID;
                St_SectionsOfFactoryObj.FactoryID = ObjToSave.FactoryID;
                if (String.IsNullOrEmpty(ObjToSave.EnglishName))
                    ObjToSave.EnglishName = ObjToSave.ArabicName;
                St_SectionsOfFactoryObj.ArabicName = ObjToSave.ArabicName;
                St_SectionsOfFactoryObj.EnglishName = ObjToSave.EnglishName;

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
                _unitOfWork.St_SectionsOfFactoryH.Add(St_SectionsOfFactoryObj);
                _unitOfWork.Complete();
                Msg.LastID =  _unitOfWork.St_SectionsOfFactoryH.GetMaxSerial(UserInfo.fCompanyId).ToString();
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
                    var Obj = _unitOfWork.NativeSql.GetSt_SectionsOfFactoryHByID(UserInfo.fCompanyId, id);
                    var St_SectionsOfFactoryObj = new St_SectionsOfFactoryHVM { };
                    St_SectionsOfFactoryObj.St_Factory = _unitOfWork.St_FactoryH.GetAllSt_FactoryH(UserInfo.fCompanyId);
                    St_SectionsOfFactoryObj.SectionsOfFactoryID = Obj.SectionsOfFactoryID;
                    St_SectionsOfFactoryObj.FactoryID = Obj.FactoryID;
                    St_SectionsOfFactoryObj.ArabicName = Obj.ArabicName;
                    St_SectionsOfFactoryObj.EnglishName = Obj.EnglishName;
                    return PartialView("Update", St_SectionsOfFactoryObj);
                }
                return PartialView("Update", new St_SectionsOfFactoryHVM());
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
                    var Obj = _unitOfWork.NativeSql.GetSt_SectionsOfFactoryHByID(UserInfo.fCompanyId, id);
                    var St_SectionsOfFactoryObj = new St_SectionsOfFactoryHVM { };
                    St_SectionsOfFactoryObj.SectionsOfFactoryID = Obj.SectionsOfFactoryID;
                    St_SectionsOfFactoryObj.ArabicName = Obj.ArabicName;
                    St_SectionsOfFactoryObj.EnglishName = Obj.EnglishName;
                    St_SectionsOfFactoryObj.FactoryName = _unitOfWork.NativeSql.GetSt_FactoryHName(UserInfo.fCompanyId, Obj.FactoryID);
                    return PartialView("Delete", St_SectionsOfFactoryObj);
                }
                return PartialView("Delete", new St_SectionsOfFactoryHVM());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return View("Error");
            }
        }
        [HttpPost]
        public JsonResult Update(St_SectionsOfFactoryHVM ObjUpdate)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var St_SectionsOfFactoryObj = new St_SectionsOfFactoryH();
                St_SectionsOfFactoryObj.InsDateTime = DateTime.Now;
                St_SectionsOfFactoryObj.InsUserID = userId;
                St_SectionsOfFactoryObj.CompanyID = UserInfo.fCompanyId;
                St_SectionsOfFactoryObj.SectionsOfFactoryID = ObjUpdate.SectionsOfFactoryID;
                St_SectionsOfFactoryObj.FactoryID = ObjUpdate.FactoryID;
                if (String.IsNullOrEmpty(ObjUpdate.EnglishName))
                    ObjUpdate.EnglishName = ObjUpdate.ArabicName;
                St_SectionsOfFactoryObj.ArabicName = ObjUpdate.ArabicName;
                St_SectionsOfFactoryObj.EnglishName = ObjUpdate.EnglishName;
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
                _unitOfWork.St_SectionsOfFactoryH.Update(St_SectionsOfFactoryObj);
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
        public JsonResult Delete(St_SectionsOfFactoryHVM ObjDelete)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);

                ObjDelete.CompanyID = UserInfo.fCompanyId;
                var St_SectionsOfFactoryObj = new St_SectionsOfFactoryH();
                St_SectionsOfFactoryObj.CompanyID = UserInfo.fCompanyId;
                St_SectionsOfFactoryObj.SectionsOfFactoryID = ObjDelete.SectionsOfFactoryID;

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
                _unitOfWork.St_SectionsOfFactoryH.Delete(St_SectionsOfFactoryObj);
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
    }
}