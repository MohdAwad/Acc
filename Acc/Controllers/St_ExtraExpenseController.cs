using Acc.Helpers;
using Acc.Models;
using Acc.Persistence;
using Acc.Repositories;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Acc.Controllers
{
    [Authorize]
    public class St_ExtraExpenseController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public St_ExtraExpenseController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            return View();
        }
        public JsonResult GetAllExtraExpense()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllExtraExpense = _unitOfWork.St_ExtraExpense.GetAllSt_ExtraExpense(UserInfo.fCompanyId);
                if (AllExtraExpense == null)
                {
                    return Json(new List<St_ExtraExpense>(), JsonRequestBehavior.AllowGet);
                }

                return Json(AllExtraExpense, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<St_ExtraExpense>(), JsonRequestBehavior.AllowGet);
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

                    var Obj = _unitOfWork.St_ExtraExpense.GetSt_ExtraExpenseByID(UserInfo.fCompanyId, id);


                    return PartialView("Update", Obj);
                }
                return PartialView("Update", new St_ExtraExpense());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return View("Error");
            }
        }
        public JsonResult UpdateExtraExpense(St_ExtraExpense ObjUpdate)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);

                ObjUpdate.CompanyID = UserInfo.fCompanyId;
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
                _unitOfWork.St_ExtraExpense.Update(ObjUpdate);
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
        [HttpGet]
        public JsonResult GetExtraExpenseName(int id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var ExtraExpenseName = _unitOfWork.St_ExtraExpense.GetSt_ExtraExpenseNameByID(UserInfo.fCompanyId, id);
            return Json(ExtraExpenseName, JsonRequestBehavior.AllowGet);
        }
    }
}