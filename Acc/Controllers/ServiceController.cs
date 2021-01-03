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
    public class ServiceController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        // GET: Service

        [Authorize(Roles = "CoOwner,ShowService")]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            ServiceVM Obj = new ServiceVM
            {
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency
            };
            return View(Obj);
        }

        [Authorize(Roles = "CoOwner,AddService")]
        public ActionResult AddNew()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
             ServiceVM Obj = new ServiceVM
             {
                ServiceID = _unitOfWork.Service.GetMaxSerial(UserInfo.fCompanyId),
                ServiceGroup = _unitOfWork.ServiceGroup.GetAllServiceGroup(UserInfo.fCompanyId),
                CostPrice = 0.000,
                SalePrice = 0.000,
                TaxPercentage = 0.0

             };
            return PartialView(Obj);
        }
        [HttpGet]
        public JsonResult GetAllServices()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllService = _unitOfWork.NativeSql.GetAllService(UserInfo.fCompanyId);
                if (AllService == null)
                {
                    return Json(new List<ServiceVM>(), JsonRequestBehavior.AllowGet);
                }

                return Json(AllService, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<ServiceVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        [Authorize(Roles = "CoOwner,AddService")]
        public JsonResult SaveService(Service ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                ObjToSave.ServiceID = _unitOfWork.Service.GetMaxSerial(UserInfo.fCompanyId);
                ObjToSave.InsDateTime = DateTime.Now;
                ObjToSave.InsUserID = userId;
                ObjToSave.CompanyID = UserInfo.fCompanyId;
                if (String.IsNullOrEmpty(ObjToSave.EnglishName))
                    ObjToSave.EnglishName = ObjToSave.ArabicName;

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
                _unitOfWork.Service.Add(ObjToSave);
                _unitOfWork.Complete();
                Msg.LastID = _unitOfWork.Service.GetMaxSerial(UserInfo.fCompanyId).ToString();
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

        [Authorize(Roles = "CoOwner,UpdateService")]
        public ActionResult UpdateService(string id)
        {
            int ID = 0;
            if (!String.IsNullOrEmpty(id))
            {

           
                try
                {
                    ID = int.Parse(id.ToString());
                }
                catch
                {
                    ID =0;
                }
            }

            try
            {
                if (ID != 0)
                {
                    var userId = User.Identity.GetUserId();
                    var UserInfo = _unitOfWork.User.GetUserByID(userId);
                    var sService = _unitOfWork.Service.GetServiceByID(UserInfo.fCompanyId, ID);
                    if (UserInfo == null)
                    {
                        RedirectToAction("", "");
                    }
                    ServiceVM Obj = new ServiceVM();
                    Obj.ServiceID = sService.ServiceID;
                    Obj.ServiceGroup = _unitOfWork.ServiceGroup.GetAllServiceGroup(UserInfo.fCompanyId);
                    Obj.ServiceGroupID = sService.ServiceGroupID;
                    Obj.ArabicName = sService.ArabicName;
                    Obj.EnglishName = sService.EnglishName;
                    Obj.Note = sService.Note;
                    Obj.CostPrice = sService.CostPrice;
                    Obj.SalePrice = sService.SalePrice;
                    Obj.TaxPercentage = sService.TaxPercentage;
                    return PartialView("UpdateService", Obj);
                }



                return PartialView("UpdateService", new Service());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return View("Error");
            }
        }

        [Authorize(Roles = "CoOwner,DeleteService")]
        public ActionResult DeleteService(int id)
        {
            try
            {
                if (id != 0)
                {
                    var userId = User.Identity.GetUserId();
                    var UserInfo = _unitOfWork.User.GetUserByID(userId);
                    var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                    var sService = _unitOfWork.Service.GetServiceByID(UserInfo.fCompanyId, id);
                    if (UserInfo == null)
                    {
                        RedirectToAction("", "");
                    }
                    ServiceVM Obj = new ServiceVM();
                    Obj.ServiceID = sService.ServiceID;
                    Obj.ServiceGroup = _unitOfWork.ServiceGroup.GetAllServiceGroup(UserInfo.fCompanyId);
                    Obj.ServiceGroupID = sService.ServiceGroupID;
                    Obj.ArabicName = sService.ArabicName;
                    Obj.EnglishName = sService.EnglishName;
                    Obj.Note = sService.Note;
                    Obj.CostPrice = sService.CostPrice;
                    Obj.SalePrice = sService.SalePrice;
                    Obj.TaxPercentage = sService.TaxPercentage;
                    Obj.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
                    Obj.TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency;
                    return PartialView("DeleteService", Obj);
                }
                return PartialView("DeleteService", new Service());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return View("Error");
            }
        }
        [HttpPost]
        [Authorize(Roles = "CoOwner,UpdateService")]
        public JsonResult UpdateService(Service ObjUpdate)
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
                _unitOfWork.Service.Update(ObjUpdate);
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
        [Authorize(Roles = "CoOwner,DeleteService")]
        public JsonResult DeleteService(Service ObjDelete)
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
                _unitOfWork.Service.Delete(ObjDelete);
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

        public JsonResult GetServiceByID(int id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllService = _unitOfWork.Service.GetServiceByID(UserInfo.fCompanyId, id);
                if (AllService == null)
                {
                    return Json(new  ServiceVM() , JsonRequestBehavior.AllowGet);
                }

                return Json(AllService, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new   ServiceVM() , JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public JsonResult CheckServiceByID(int id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);

            var AllService = _unitOfWork.Service.GetServiceByID(UserInfo.fCompanyId, id);
            if (AllService == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(AllService, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public JsonResult CheckService(int id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);

            var ServiceInfo = _unitOfWork.Service.GetServiceByID(UserInfo.fCompanyId, id);
            if (ServiceInfo == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(ServiceInfo.ServiceID, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public JsonResult CheckServiceBeforDelete(int id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            if (id != 0)
            {
                int ServiceGroupID = _unitOfWork.NativeSql.CheckService(UserInfo.fCompanyId, id);

                return Json(ServiceGroupID, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
    }
}