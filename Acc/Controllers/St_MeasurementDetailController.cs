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
    public class St_MeasurementDetailController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public St_MeasurementDetailController()
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
        public JsonResult GetAllSt_MeasurementDetail()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllSt_MeasurementDetail = _unitOfWork.NativeSql.GetAllSt_MeasurementDetail(UserInfo.fCompanyId);
                if (AllSt_MeasurementDetail == null)
                {
                    return Json(new List<St_MeasurementDetailVM>(), JsonRequestBehavior.AllowGet);
                }

                return Json(AllSt_MeasurementDetail, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<St_MeasurementDetailVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult Add()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            St_MeasurementDetailVM Obj = new St_MeasurementDetailVM
            {
                St_Measurement = _unitOfWork.NativeSql.GetAllSt_Measurement(UserInfo.fCompanyId),
                MeasurementID = 1
            };
            return PartialView(Obj);
        }
        [HttpGet]
        public JsonResult GetMax(int id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var MeasurementDetailNo = _unitOfWork.St_MeasurementDetail.GetMaxSerial(UserInfo.fCompanyId, id);
            return Json(MeasurementDetailNo, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveSt_MeasurementDetail(St_MeasurementDetailVM ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var St_MeasurementDetailObj = new St_MeasurementDetail();
                St_MeasurementDetailObj.InsDateTime = DateTime.Now;
                St_MeasurementDetailObj.InsUserID = userId;
                St_MeasurementDetailObj.CompanyID = UserInfo.fCompanyId;
                St_MeasurementDetailObj.MeasurementID = ObjToSave.MeasurementID;
                St_MeasurementDetailObj.MeasurementDetailID = _unitOfWork.St_MeasurementDetail.GetMaxSerial(UserInfo.fCompanyId, St_MeasurementDetailObj.MeasurementID);
                if (String.IsNullOrEmpty(ObjToSave.EnglishName))
                    ObjToSave.EnglishName = ObjToSave.ArabicName;

                St_MeasurementDetailObj.ArabicName = ObjToSave.ArabicName;
                St_MeasurementDetailObj.EnglishName = ObjToSave.EnglishName;

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
                _unitOfWork.St_MeasurementDetail.Add(St_MeasurementDetailObj);
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
        public ActionResult Update(int id, int id2)
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

                    var Obj = _unitOfWork.St_MeasurementDetail.GetSt_MeasurementDetailByID(UserInfo.fCompanyId, id, id2);
                    var St_MeasurementDetailObj = new St_MeasurementDetailVM();
                    St_MeasurementDetailObj.St_Measurement = _unitOfWork.NativeSql.GetAllSt_Measurement(UserInfo.fCompanyId);
                    St_MeasurementDetailObj.MeasurementID = Obj.MeasurementID;
                    St_MeasurementDetailObj.MeasurementDetailID = Obj.MeasurementDetailID;
                    St_MeasurementDetailObj.ArabicName = Obj.ArabicName;
                    St_MeasurementDetailObj.EnglishName = Obj.EnglishName;


                    return PartialView("Update", St_MeasurementDetailObj);
                }



                return PartialView("Update", new St_MeasurementDetailVM());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return View("Error");
            }
        }
        public ActionResult Delete(int id, int id2)
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

                    var Obj = _unitOfWork.St_MeasurementDetail.GetSt_MeasurementDetailByID(UserInfo.fCompanyId, id, id2);
                    var St_MeasurementDetailObj = new St_MeasurementDetailVM();
                    St_MeasurementDetailObj.St_Measurement = _unitOfWork.NativeSql.GetAllSt_Measurement(UserInfo.fCompanyId);
                    St_MeasurementDetailObj.MeasurementID = Obj.MeasurementID;
                    St_MeasurementDetailObj.MeasurementDetailID = Obj.MeasurementDetailID;
                    St_MeasurementDetailObj.ArabicName = Obj.ArabicName;
                    St_MeasurementDetailObj.EnglishName = Obj.EnglishName;


                    return PartialView("Delete", St_MeasurementDetailObj);
                }



                return PartialView("Delete", new St_MeasurementDetailVM());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return View("Error");
            }
        }
        [HttpPost]
        public JsonResult UpdateSt_MeasurementDetail(St_MeasurementDetailVM ObjUpdate)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var St_MeasurementDetailObj = new St_MeasurementDetail();
                St_MeasurementDetailObj.InsDateTime = DateTime.Now;
                St_MeasurementDetailObj.InsUserID = userId;
                St_MeasurementDetailObj.CompanyID = UserInfo.fCompanyId;
                St_MeasurementDetailObj.MeasurementID = ObjUpdate.MeasurementID;
                St_MeasurementDetailObj.MeasurementDetailID = ObjUpdate.MeasurementDetailID;
                if (String.IsNullOrEmpty(ObjUpdate.EnglishName))
                    ObjUpdate.EnglishName = ObjUpdate.ArabicName;

                St_MeasurementDetailObj.ArabicName = ObjUpdate.ArabicName;
                St_MeasurementDetailObj.EnglishName = ObjUpdate.EnglishName;
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
                _unitOfWork.St_MeasurementDetail.Update(St_MeasurementDetailObj);
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
        public JsonResult DeleteSt_MeasurementDetail(St_MeasurementDetailVM ObjDelete)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);

                ObjDelete.CompanyID = UserInfo.fCompanyId;
                var St_MeasurementDetailObj = new St_MeasurementDetail();
                St_MeasurementDetailObj.CompanyID = UserInfo.fCompanyId;
                St_MeasurementDetailObj.MeasurementID = ObjDelete.MeasurementID;
                St_MeasurementDetailObj.MeasurementDetailID = ObjDelete.MeasurementDetailID;
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
                _unitOfWork.St_MeasurementDetail.Delete(St_MeasurementDetailObj);
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
        public ActionResult AddToItemCard(int id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            St_MeasurementDetailVM Obj = new St_MeasurementDetailVM
            {
                St_Measurement = _unitOfWork.NativeSql.GetAllSt_Measurement(UserInfo.fCompanyId),
                MeasurementID = id
            };
            return PartialView(Obj);
        }
        [HttpGet]
        public JsonResult GetSt_MeasurementDetailByMeasurementID(int id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllSt_MeasurementDetail = _unitOfWork.St_MeasurementDetail.GetSt_MeasurementDetailBySt_Measurement(UserInfo.fCompanyId, id);
                if (AllSt_MeasurementDetail == null)
                {
                    return Json(new List<St_MeasurementDetail>(), JsonRequestBehavior.AllowGet);
                }

                return Json(AllSt_MeasurementDetail, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<St_MeasurementDetail>(), JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public JsonResult CheckSt_MeasurementDetailBeforDelete(int id, int id2)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            if (id != 0)
            {
                int MeasurementDetailID = _unitOfWork.NativeSql.CheckSt_MeasurementDetailBeforDelete(UserInfo.fCompanyId, id, id2);

                return Json(MeasurementDetailID, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

    }
}