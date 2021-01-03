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
    public class St_DescriptionDetailController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public St_DescriptionDetailController()
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
        public JsonResult GetAllSt_DescriptionDetail()
        {
                try
                {
                    var userId = User.Identity.GetUserId();
                    var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                    var AllSt_DescriptionDetail = _unitOfWork.NativeSql.GetAllSt_DescriptionDetail(UserInfo.fCompanyId);
                    if (AllSt_DescriptionDetail == null)
                    {
                        return Json(new List<St_DescriptionDetailVM>(), JsonRequestBehavior.AllowGet);
                    }

                    return Json(AllSt_DescriptionDetail, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message.ToString();
                    return Json(new List<St_DescriptionDetailVM>(), JsonRequestBehavior.AllowGet);
                }

        }
        public ActionResult Add()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            St_DescriptionDetailVM Obj = new St_DescriptionDetailVM
            {
                St_Description = _unitOfWork.St_Description.GetAllSt_Description(UserInfo.fCompanyId),
                DescriptionID = 1
            };
            return PartialView(Obj);
        }
        [HttpGet]
        public JsonResult GetMax(int id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var DescriptionDetailNo = _unitOfWork.St_DescriptionDetail.GetMaxSerial(UserInfo.fCompanyId, id);
            return Json(DescriptionDetailNo, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveSt_DescriptionDetail(St_DescriptionDetailVM ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var St_DescriptionDetailObj = new St_DescriptionDetail();
                St_DescriptionDetailObj.InsDateTime = DateTime.Now;
                St_DescriptionDetailObj.InsUserID = userId;
                St_DescriptionDetailObj.CompanyID = UserInfo.fCompanyId;
                St_DescriptionDetailObj.DescriptionID = ObjToSave.DescriptionID;
                St_DescriptionDetailObj.DescriptionDetailID = _unitOfWork.St_DescriptionDetail.GetMaxSerial(UserInfo.fCompanyId, St_DescriptionDetailObj.DescriptionID);
                if (String.IsNullOrEmpty(ObjToSave.EnglishName))
                    ObjToSave.EnglishName = ObjToSave.ArabicName;

                St_DescriptionDetailObj.ArabicName = ObjToSave.ArabicName;
                St_DescriptionDetailObj.EnglishName = ObjToSave.EnglishName;

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
                _unitOfWork.St_DescriptionDetail.Add(St_DescriptionDetailObj);
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
        public ActionResult Update(int id,int id2)
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

                    var Obj = _unitOfWork.St_DescriptionDetail.GetSt_DescriptionDetailByID(UserInfo.fCompanyId, id, id2);
                    var St_DescriptionDetailObj = new St_DescriptionDetailVM();
                    St_DescriptionDetailObj.St_Description = _unitOfWork.St_Description.GetAllSt_Description(UserInfo.fCompanyId);
                    St_DescriptionDetailObj.DescriptionID = Obj.DescriptionID;
                    St_DescriptionDetailObj.DescriptionDetailID = Obj.DescriptionDetailID;
                    St_DescriptionDetailObj.ArabicName = Obj.ArabicName;
                    St_DescriptionDetailObj.EnglishName = Obj.EnglishName;


                    return PartialView("Update", St_DescriptionDetailObj);
                }



                return PartialView("Update", new St_DescriptionDetailVM());
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

                    var Obj = _unitOfWork.St_DescriptionDetail.GetSt_DescriptionDetailByID(UserInfo.fCompanyId, id, id2);
                    var St_DescriptionDetailObj = new St_DescriptionDetailVM();
                    St_DescriptionDetailObj.St_Description = _unitOfWork.St_Description.GetAllSt_Description(UserInfo.fCompanyId);
                    St_DescriptionDetailObj.DescriptionID = Obj.DescriptionID;
                    St_DescriptionDetailObj.DescriptionDetailID = Obj.DescriptionDetailID;
                    St_DescriptionDetailObj.ArabicName = Obj.ArabicName;
                    St_DescriptionDetailObj.EnglishName = Obj.EnglishName;


                    return PartialView("Delete", St_DescriptionDetailObj);
                }



                return PartialView("Delete", new St_DescriptionDetailVM());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return View("Error");
            }
        }
        [HttpPost]
        public JsonResult UpdateSt_DescriptionDetail(St_DescriptionDetailVM ObjUpdate)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var St_DescriptionDetailObj = new St_DescriptionDetail();
                St_DescriptionDetailObj.InsDateTime = DateTime.Now;
                St_DescriptionDetailObj.InsUserID = userId;
                St_DescriptionDetailObj.CompanyID = UserInfo.fCompanyId;
                St_DescriptionDetailObj.DescriptionID = ObjUpdate.DescriptionID;
                St_DescriptionDetailObj.DescriptionDetailID = ObjUpdate.DescriptionDetailID;
                if (String.IsNullOrEmpty(ObjUpdate.EnglishName))
                    ObjUpdate.EnglishName = ObjUpdate.ArabicName;

                St_DescriptionDetailObj.ArabicName = ObjUpdate.ArabicName;
                St_DescriptionDetailObj.EnglishName = ObjUpdate.EnglishName;
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
                _unitOfWork.St_DescriptionDetail.Update(St_DescriptionDetailObj);
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
        public JsonResult DeleteSt_DescriptionDetail(St_DescriptionDetailVM ObjDelete)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);

                ObjDelete.CompanyID = UserInfo.fCompanyId;
                var St_DescriptionDetailObj = new St_DescriptionDetail();
                St_DescriptionDetailObj.CompanyID = UserInfo.fCompanyId;
                St_DescriptionDetailObj.DescriptionID = ObjDelete.DescriptionID;
                St_DescriptionDetailObj.DescriptionDetailID = ObjDelete.DescriptionDetailID;
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
                _unitOfWork.St_DescriptionDetail.Delete(St_DescriptionDetailObj);
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
            St_DescriptionDetailVM Obj = new St_DescriptionDetailVM
            {
                St_Description = _unitOfWork.St_Description.GetAllSt_Description(UserInfo.fCompanyId),
                DescriptionID = id
            };
            return PartialView(Obj);
        }
        [HttpGet]
        public JsonResult GetSt_DescriptionDetailByDescriptioID(int id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllSt_DescriptionDetail = _unitOfWork.St_DescriptionDetail.GetSt_DescriptionDetailBySt_Description(UserInfo.fCompanyId, id);
                if (AllSt_DescriptionDetail == null)
                {
                    return Json(new List<St_DescriptionDetail>(), JsonRequestBehavior.AllowGet);
                }

                return Json(AllSt_DescriptionDetail, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<St_DescriptionDetail>(), JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public JsonResult CheckSt_DescriptionDetailBeforDelete(int id,int id2)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            if (id != 0)
            {
                int DescriptionDetailID = _unitOfWork.NativeSql.CheckSt_DescriptionDetailBeforDelete(UserInfo.fCompanyId, id, id2);

                return Json(DescriptionDetailID, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
    }
}