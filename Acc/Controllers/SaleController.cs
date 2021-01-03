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
    public class SaleController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public SaleController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }

        // GET: Sale

        [Authorize(Roles = "CoOwner,ShowSalesman")]

        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            return View();
        }
        [Authorize(Roles = "CoOwner,AddSalesman")]
        public ActionResult AddNew()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            Sale Obj = new Sale
            {
                SalesID = _unitOfWork.Sale.GetMaxSerial(UserInfo.fCompanyId)
            };
            return PartialView(Obj);
        }

        [HttpGet]
        public JsonResult GetAllSale()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllSale = _unitOfWork.Sale.GetAllSale(UserInfo.fCompanyId);
                if (AllSale == null)
                {
                    return Json(new List<Sale>(), JsonRequestBehavior.AllowGet);
                }

                return Json(AllSale, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<Sale>(), JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        [Authorize(Roles = "CoOwner,AddSalesman")]
        public JsonResult SaveSale(Sale ObjSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                ObjSave.SalesID = _unitOfWork.Sale.GetMaxSerial(UserInfo.fCompanyId);
                ObjSave.InsDateTime = DateTime.Now;
                ObjSave.InsUserID = userId;
                ObjSave.CompanyID = UserInfo.fCompanyId;
                
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
                _unitOfWork.Sale.Add(ObjSave);
                _unitOfWork.Complete();
                Msg.LastID = _unitOfWork.Sale.GetMaxSerial(UserInfo.fCompanyId).ToString();
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

        [Authorize(Roles = "CoOwner,UpdateSalesman")]
        public ActionResult UpdateSale(int id)
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

                    var Obj = _unitOfWork.Sale.GetSaleByID(UserInfo.fCompanyId, id);


                    return PartialView("UpdateSale", Obj);
                }



                return PartialView("UpdateSale", new Sale());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return View("Error");
            }
        }

        [Authorize(Roles = "CoOwner,DeleteSalesman")]
        public ActionResult DeleteSale(int id)
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

                    var Obj = _unitOfWork.Sale.GetSaleByID(UserInfo.fCompanyId, id);


                    return PartialView("DeleteSale", Obj);
                }



                return PartialView("DeleteSale", new Sale());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return View("Error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "CoOwner,UpdateSalesman")]
        public JsonResult UpdateSale(Sale ObjUpdate)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);

                ObjUpdate.CompanyID = UserInfo.fCompanyId;
                ObjUpdate.InsDateTime = DateTime.Now;
                ObjUpdate.InsUserID = userId;
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
                _unitOfWork.Sale.Update(ObjUpdate);
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
        [Authorize(Roles = "CoOwner,DeleteSalesman")]
        public JsonResult DeleteSale(Sale ObjDelete)
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
                _unitOfWork.Sale.Delete(ObjDelete);
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
        public JsonResult CheckSaleBeforDelete(int id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            if (id != 0)
            {
                int SaleID = _unitOfWork.NativeSql.CheckSale(UserInfo.fCompanyId, id);

                return Json(SaleID, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetSaleManName(int id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            if (id != 0)
            {
                var SaleManObj = _unitOfWork.Sale.GetSaleByID(UserInfo.fCompanyId, id);

                if (SaleManObj == null)
                {
                    SaleManObj.SalesName = "";
                };

                return Json(SaleManObj.SalesName, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}