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
    public class CustomerAreaController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerAreaController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        // GET: CustomerArea

        [Authorize(Roles = "CoOwner,ShowCustomerarea")]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            return View();
        }
        [HttpGet]
        public JsonResult GetAllCustomerArea()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllCustomerArea = _unitOfWork.NativeSql.GetAllCustomerArea(UserInfo.fCompanyId);
                if (AllCustomerArea == null)
                {
                    return Json(new List<CustomerAreaVM>(), JsonRequestBehavior.AllowGet);
                }

                return Json(AllCustomerArea, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<CustomerAreaVM>(), JsonRequestBehavior.AllowGet);
            }

        }

        [Authorize(Roles = "CoOwner,AddCustomerarea")]
        public ActionResult AddNew()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            CustomerAreaVM Obj = new CustomerAreaVM
            {
                CustomerAreaID = _unitOfWork.CustomerArea.GetMaxSerial(UserInfo.fCompanyId),
                CustomerCity = _unitOfWork.CustomerCity.GetAllCustomerCity(UserInfo.fCompanyId),
                CustomerCityID = 1
            };
            return PartialView(Obj);
        }
        [HttpPost]
        [Authorize(Roles = "CoOwner,AddCustomerarea")]
        public JsonResult SaveCustomerArea(CustomerAreaVM ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var CustomerAreaObj = new CustomerArea();
                CustomerAreaObj.InsDateTime = DateTime.Now;
                CustomerAreaObj.InsUserID = userId;
                CustomerAreaObj.CompanyID = UserInfo.fCompanyId;
                CustomerAreaObj.CustomerAreaID = ObjToSave.CustomerAreaID;
                CustomerAreaObj.CustomerCityID = ObjToSave.CustomerCityID;
                if (String.IsNullOrEmpty(ObjToSave.EnglishName))
                    ObjToSave.EnglishName = ObjToSave.ArabicName;
                CustomerAreaObj.ArabicName = ObjToSave.ArabicName;
                CustomerAreaObj.EnglishName = ObjToSave.EnglishName;

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
                _unitOfWork.CustomerArea.Add(CustomerAreaObj);
                _unitOfWork.Complete();
                Msg.LastID = _unitOfWork.CustomerArea.GetMaxSerial(UserInfo.fCompanyId).ToString();
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

        [Authorize(Roles = "CoOwner,UpdateCustomerarea")]
        public ActionResult UpdateCustomerArea(int id)
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
                    
                    var Obj = _unitOfWork.NativeSql.GetCustomerAreaByID(UserInfo.fCompanyId, id);
                    var CustomerAreaObj = new CustomerAreaVM { };
                    CustomerAreaObj.CustomerCity = _unitOfWork.CustomerCity.GetAllCustomerCity(UserInfo.fCompanyId);
                    CustomerAreaObj.CustomerAreaID = Obj.CustomerAreaID;
                    CustomerAreaObj.CustomerCityID = Obj.CustomerCityID;
                    CustomerAreaObj.ArabicName = Obj.ArabicName;
                    CustomerAreaObj.EnglishName = Obj.EnglishName;


                    return PartialView("UpdateCustomerArea", CustomerAreaObj);
                }



                return PartialView("UpdateCustomerArea", new CustomerAreaVM());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return View("Error");
            }
        }

        [Authorize(Roles = "CoOwner,DeleteCustomerarea")]
        public ActionResult DeleteCustomerArea(int id)
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

                    var Obj = _unitOfWork.NativeSql.GetCustomerAreaByID(UserInfo.fCompanyId, id);
                    var CustomerAreaObj = new CustomerAreaVM { };
                    CustomerAreaObj.CustomerAreaID = Obj.CustomerAreaID;
                    CustomerAreaObj.CustomerCityID = Obj.CustomerCityID;
                    CustomerAreaObj.ArabicName = Obj.ArabicName;
                    CustomerAreaObj.EnglishName = Obj.EnglishName;
                    CustomerAreaObj.CustomerCityName = _unitOfWork.NativeSql.GetCustomerCityName(UserInfo.fCompanyId, CustomerAreaObj.CustomerCityID);


                    return PartialView("DeleteCustomerArea", CustomerAreaObj);
                }



                return PartialView("DeleteCustomerArea", new CustomerArea());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return View("Error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "CoOwner,UpdateCustomerarea")]
        public JsonResult UpdateCustomerArea(CustomerAreaVM ObjUpdate)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var CustomerAreaObj = new CustomerArea();
                CustomerAreaObj.InsDateTime = DateTime.Now;
                CustomerAreaObj.InsUserID = userId;
                CustomerAreaObj.CompanyID = UserInfo.fCompanyId;
                CustomerAreaObj.CustomerAreaID = ObjUpdate.CustomerAreaID;
                CustomerAreaObj.CustomerCityID = ObjUpdate.CustomerCityID;
                if (String.IsNullOrEmpty(ObjUpdate.EnglishName))
                    ObjUpdate.EnglishName = ObjUpdate.ArabicName;
                CustomerAreaObj.ArabicName = ObjUpdate.ArabicName;
                CustomerAreaObj.EnglishName = ObjUpdate.EnglishName;
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
                _unitOfWork.CustomerArea.Update(CustomerAreaObj);
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
        [Authorize(Roles = "CoOwner,DeleteCustomerarea")]
        public JsonResult DeleteCustomerArea(CustomerAreaVM ObjDelete)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);

                ObjDelete.CompanyID = UserInfo.fCompanyId;
                var CustomerAreaObj = new CustomerArea();
                CustomerAreaObj.CompanyID = UserInfo.fCompanyId;
                CustomerAreaObj.CustomerAreaID = ObjDelete.CustomerAreaID;

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
                _unitOfWork.CustomerArea.Delete(CustomerAreaObj);
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
        public JsonResult CheckCustomerAreaBeforDelete(int id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            if (id != 0)
            {
                int CustomerAreaID = _unitOfWork.NativeSql.CheckCustomerArea(UserInfo.fCompanyId, id);

                return Json(CustomerAreaID, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
    }
}