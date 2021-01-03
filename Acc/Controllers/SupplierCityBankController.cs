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
    public class SupplierCityBankController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public SupplierCityBankController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        // GET: SupplierCityBank

        [Authorize(Roles = "CoOwner,ShowSuppliersbankcity")]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            return View();
        }
        [HttpGet]
        public JsonResult GetAllSupplierCityBank()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllSupplierCityBank = _unitOfWork.NativeSql.GetAllSupplierCityBank(UserInfo.fCompanyId);
                if (AllSupplierCityBank == null)
                {
                    return Json(new List<SupplierCityBankVM>(), JsonRequestBehavior.AllowGet);
                }

                return Json(AllSupplierCityBank, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<SupplierCityBankVM>(), JsonRequestBehavior.AllowGet);
            }

        }

        [Authorize(Roles = "CoOwner,AddSuppliersbankcity")]
        public ActionResult AddNew()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            SupplierCityBankVM Obj = new SupplierCityBankVM
            {
                SupplierCityBankID = _unitOfWork.SupplierCityBank.GetMaxSerial(UserInfo.fCompanyId),
                SupplierCountryBank = _unitOfWork.SupplierCountryBank.GetAllSupplierCountryBank(UserInfo.fCompanyId),
                SupplierCountryBankID = 1
            };
            return PartialView(Obj);
        }
        [HttpPost]
        [Authorize(Roles = "CoOwner,AddSuppliersbankcity")]
        public JsonResult SaveSupplierCityBank(SupplierCityBankVM ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var SupplierCityBankObj = new SupplierCityBank();
                SupplierCityBankObj.InsDateTime = DateTime.Now;
                SupplierCityBankObj.InsUserID = userId;
                SupplierCityBankObj.CompanyID = UserInfo.fCompanyId;
                SupplierCityBankObj.SupplierCityBankID = ObjToSave.SupplierCityBankID;
                SupplierCityBankObj.SupplierCountryBankID = ObjToSave.SupplierCountryBankID;
                if (String.IsNullOrEmpty(ObjToSave.EnglishName))
                    ObjToSave.EnglishName = ObjToSave.ArabicName;
                SupplierCityBankObj.ArabicName = ObjToSave.ArabicName;
                SupplierCityBankObj.EnglishName = ObjToSave.EnglishName;

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
                _unitOfWork.SupplierCityBank.Add(SupplierCityBankObj);
                _unitOfWork.Complete();
                _unitOfWork.SupplierCityBank.GetMaxSerial(UserInfo.fCompanyId).ToString();
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

        [Authorize(Roles = "CoOwner,UpdateSuppliersbankcity")]
        public ActionResult UpdateSupplierCityBank(int id)
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

                    var Obj = _unitOfWork.NativeSql.GetSupplierCityBankByID(UserInfo.fCompanyId, id);
                    var SupplierCityBankObj = new SupplierCityBankVM { };
                    SupplierCityBankObj.SupplierCountryBank = _unitOfWork.SupplierCountryBank.GetAllSupplierCountryBank(UserInfo.fCompanyId);
                    SupplierCityBankObj.SupplierCityBankID = Obj.SupplierCityBankID;
                    SupplierCityBankObj.SupplierCountryBankID = Obj.SupplierCountryBankID;
                    SupplierCityBankObj.ArabicName = Obj.ArabicName;
                    SupplierCityBankObj.EnglishName = Obj.EnglishName;


                    return PartialView("UpdateSupplierCityBank", SupplierCityBankObj);
                }



                return PartialView("UpdateSupplierCityBank", new SupplierCityBankVM());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return View("Error");
            }
        }

        [Authorize(Roles = "CoOwner,DeleteSuppliersbankcity")]
        public ActionResult DeleteSupplierCityBank(int id)
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

                    var Obj = _unitOfWork.NativeSql.GetSupplierCityBankByID(UserInfo.fCompanyId, id);
                    var SupplierCityBankObj = new SupplierCityBankVM { };
                    SupplierCityBankObj.SupplierCityBankID = Obj.SupplierCityBankID;
                    SupplierCityBankObj.SupplierCountryBankID = Obj.SupplierCountryBankID;
                    SupplierCityBankObj.ArabicName = Obj.ArabicName;
                    SupplierCityBankObj.EnglishName = Obj.EnglishName;
                    SupplierCityBankObj.SupplierCountryBankName = _unitOfWork.NativeSql.GetSupplierCountryBankName(UserInfo.fCompanyId, SupplierCityBankObj.SupplierCountryBankID);


                    return PartialView("DeleteSupplierCityBank", SupplierCityBankObj);
                }



                return PartialView("DeleteSupplierCityBank", new SupplierCityBank());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return View("Error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "CoOwner,UpdateSuppliersbankcity")]
        public JsonResult UpdateSupplierCityBank(SupplierCityBankVM ObjUpdate)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var SupplierCityBankObj = new SupplierCityBank();
                SupplierCityBankObj.InsDateTime = DateTime.Now;
                SupplierCityBankObj.InsUserID = userId;
                SupplierCityBankObj.CompanyID = UserInfo.fCompanyId;
                SupplierCityBankObj.SupplierCityBankID = ObjUpdate.SupplierCityBankID;
                SupplierCityBankObj.SupplierCountryBankID = ObjUpdate.SupplierCountryBankID;
                if (String.IsNullOrEmpty(ObjUpdate.EnglishName))
                    ObjUpdate.EnglishName = ObjUpdate.ArabicName;
                SupplierCityBankObj.ArabicName = ObjUpdate.ArabicName;
                SupplierCityBankObj.EnglishName = ObjUpdate.EnglishName;
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
                _unitOfWork.SupplierCityBank.Update(SupplierCityBankObj);
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
        [Authorize(Roles = "CoOwner,DeleteSuppliersbankcity")]
        public JsonResult DeleteSupplierCityBank(SupplierCityBankVM ObjDelete)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);

                ObjDelete.CompanyID = UserInfo.fCompanyId;
                var SupplierCityBankObj = new SupplierCityBank();
                SupplierCityBankObj.CompanyID = UserInfo.fCompanyId;
                SupplierCityBankObj.SupplierCityBankID = ObjDelete.SupplierCityBankID;

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
                _unitOfWork.SupplierCityBank.Delete(SupplierCityBankObj);
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
        public JsonResult CheckSupplierCityBankBeforDelete(int id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            if (id != 0)
            {
                int SupplierCityBankID = _unitOfWork.NativeSql.CheckSupplierCityBank(UserInfo.fCompanyId, id);

                return Json(SupplierCityBankID, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
    }
}