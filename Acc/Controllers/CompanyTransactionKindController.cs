using Acc.Helpers;
using Acc.Models;
using Acc.Persistence;
using Acc.Repositories;
using Acc.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;


namespace Acc.Controllers
{
    [Authorize]
    public class CompanyTransactionKindController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyTransactionKindController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        [Authorize(Roles = "CoOwner,ShowTransaction")]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            return View();
        }
        [Authorize(Roles = "CoOwner,AddTransaction")]
        public ActionResult AddNew()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            CompanyTransationKindVM Obj = new CompanyTransationKindVM
            {
                CompanyTransactionKindID = _unitOfWork.CompanyTransactionKind.GetMaxSerial(UserInfo.fCompanyId),
                TransactionKind = _unitOfWork.TransactionKind.GetAllTransactionKind(),
                AutoSerial = true
            };
            return PartialView(Obj);
        }
        [HttpGet]
        public JsonResult GetAllCoampnyTransactionKind()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllCompanyTransactionKind = _unitOfWork.NativeSql.GetAllCompanyTransactionKind(UserInfo.fCompanyId);
                if (AllCompanyTransactionKind == null)
                {
                    return Json(new List<CompanyTransationKindVM>(), JsonRequestBehavior.AllowGet);
                }

                return Json(AllCompanyTransactionKind, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<CompanyTransationKindVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        [Authorize(Roles = "CoOwner,AddTransaction")]
        public JsonResult SaveCompanyTransactionKind(CompanyTransactionKind ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                ObjToSave.CompanyTransactionKindID = _unitOfWork.CompanyTransactionKind.GetMaxSerial(UserInfo.fCompanyId);
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
                if (ObjToSave.MonthlySerial)
                {
                    var Obj = new CompanyTransactionKindMonthlySerial();
                    for (int i = 1; i <= 12; i++)
                    {
                        Obj.CompanyID = ObjToSave.CompanyID;
                        Obj.MonthID = i;
                        Obj.LastSerial = 0;
                        Obj.CompanyTransactionKindID = ObjToSave.CompanyTransactionKindID;
                        _unitOfWork.CompanyTransactionKindMonthlySerial.Add(Obj);
                        _unitOfWork.Complete();
                    }
                    
                }
                _unitOfWork.CompanyTransactionKind.Add(ObjToSave);
                _unitOfWork.Complete();
                Msg.LastID = _unitOfWork.CompanyTransactionKind.GetMaxSerial(UserInfo.fCompanyId).ToString();
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

        [Authorize(Roles = "CoOwner,UpdateTransaction")]
        public ActionResult UpdateCompanyTransactionKind(int id)
        {
            try
            {
                if (id != 0)
                {
                    var userId = User.Identity.GetUserId();
                    var UserInfo = _unitOfWork.User.GetUserByID(userId);
                    var sCompanyTransactionKind = _unitOfWork.CompanyTransactionKind.GetCompanyTransactionKindByID(UserInfo.fCompanyId, id);
                    if (UserInfo == null)
                    {
                        RedirectToAction("", "");
                    }
                    CompanyTransationKindVM Obj = new CompanyTransationKindVM();
                    Obj.CompanyTransactionKindID = sCompanyTransactionKind.CompanyTransactionKindID;
                    Obj.TransactionKind = _unitOfWork.TransactionKind.GetAllTransactionKind();
                    Obj.TransactionKindID = sCompanyTransactionKind.TransactionKindID;
                    Obj.ArabicName = sCompanyTransactionKind.ArabicName;
                    Obj.EnglishName = sCompanyTransactionKind.EnglishName;
                    Obj.AutoSerial = sCompanyTransactionKind.AutoSerial;
                    Obj.MonthlySerial = sCompanyTransactionKind.MonthlySerial;
                    Obj.Symbol = sCompanyTransactionKind.Symbol;
                    Obj.Year = sCompanyTransactionKind.Year;
                    Obj.Month = sCompanyTransactionKind.Month;
                    Obj.Serial = sCompanyTransactionKind.Serial;
                    if (Obj.MonthlySerial)
                    {
                        string SerialNumber = "";
                        DateTime ExampleYear = DateTime.Now;
                        string lastTwoDigitsOfYear = ExampleYear.ToString("yy");
                        for (int i = 1; i <= Obj.Serial; i++)
                        {
                            if (i < Obj.Serial)
                            {
                                SerialNumber = SerialNumber + "0";
                            }
                            else if (i == Obj.Serial)
                            {
                                SerialNumber = SerialNumber + "1";
                            }
                        }
                        Obj.Example = Obj.Symbol + lastTwoDigitsOfYear + "01" + SerialNumber;
                    }

                    return PartialView("UpdateCompanyTransactionKind", Obj);
                }
                return PartialView("UpdateCompanyTransactionKind", new CompanyTransactionKind());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return View("Error");
            }
        }
        [Authorize(Roles = "CoOwner,DeleteTransaction")]
        public ActionResult DeleteCompanyTransactionKind(int id)
        {
            try
            {
                if (id != 0)
                {
                    var userId = User.Identity.GetUserId();
                    var UserInfo = _unitOfWork.User.GetUserByID(userId);
                    var sCompanyTransactionKind = _unitOfWork.CompanyTransactionKind.GetCompanyTransactionKindByID(UserInfo.fCompanyId, id);
                    if (UserInfo == null)
                    {
                        RedirectToAction("", "");
                    }
                    CompanyTransationKindVM Obj = new CompanyTransationKindVM();
                    Obj.CompanyTransactionKindID = sCompanyTransactionKind.CompanyTransactionKindID;
                    Obj.TransactionKind = _unitOfWork.TransactionKind.GetAllTransactionKind();
                    Obj.TransactionKindID = sCompanyTransactionKind.TransactionKindID;
                    Obj.ArabicName = sCompanyTransactionKind.ArabicName;
                    Obj.EnglishName = sCompanyTransactionKind.EnglishName;
                    Obj.AutoSerial = sCompanyTransactionKind.AutoSerial;
                    Obj.MonthlySerial = sCompanyTransactionKind.MonthlySerial;
                    Obj.Symbol = sCompanyTransactionKind.Symbol;
                    Obj.Year = sCompanyTransactionKind.Year;
                    Obj.Month = sCompanyTransactionKind.Month;
                    Obj.Serial = sCompanyTransactionKind.Serial;
                    if (Obj.MonthlySerial)
                    {
                        string SerialNumber = "";
                        DateTime ExampleYear = DateTime.Now;
                        string lastTwoDigitsOfYear = ExampleYear.ToString("yy");
                        for (int i = 1; i <= Obj.Serial; i++)
                        {
                            if (i < Obj.Serial)
                            {
                                SerialNumber = SerialNumber + "0";
                            }
                            else if (i == Obj.Serial)
                            {
                                SerialNumber = SerialNumber + "1";
                            }
                        }
                        Obj.Example = Obj.Symbol + lastTwoDigitsOfYear + "01" + SerialNumber;
                    }

                    return PartialView("DeleteCompanyTransactionKind", Obj);
                }
                return PartialView("DeleteCompanyTransactionKind", new CompanyTransactionKind());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return View("Error");
            }
        }
        [HttpPost]
        [Authorize(Roles = "CoOwner,UpdateTransaction")]
        public JsonResult UpdateCompanyTransactionKind(CompanyTransactionKind ObjUpdate)
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
                if (ObjUpdate.MonthlySerial)
                {
                    var Obj = new CompanyTransactionKindMonthlySerial();
                    var CheckIfSave = _unitOfWork.CompanyTransactionKindMonthlySerial.CheckIfSave(ObjUpdate.CompanyID, ObjUpdate.CompanyTransactionKindID);
                    if (CheckIfSave == null)
                    {
                        _unitOfWork.CompanyTransactionKindMonthlySerial.Delete(ObjUpdate.CompanyID, ObjUpdate.CompanyTransactionKindID);
                        _unitOfWork.Complete();
                        for (int i = 1; i <= 12; i++)
                        {
                            Obj.CompanyID = ObjUpdate.CompanyID;
                            Obj.MonthID = i;
                            Obj.LastSerial = 0;
                            Obj.CompanyTransactionKindID = ObjUpdate.CompanyTransactionKindID;
                            _unitOfWork.CompanyTransactionKindMonthlySerial.Add(Obj);
                            _unitOfWork.Complete();
                        }
                    }
                }
                else {
                    var Obj = new CompanyTransactionKindMonthlySerial();
                    var CheckIfSave = _unitOfWork.CompanyTransactionKindMonthlySerial.CheckIfSave(ObjUpdate.CompanyID, ObjUpdate.CompanyTransactionKindID);
                    if (CheckIfSave != null)
                    {
                        _unitOfWork.CompanyTransactionKindMonthlySerial.Delete(ObjUpdate.CompanyID, ObjUpdate.CompanyTransactionKindID);
                        _unitOfWork.Complete();
                    }
                }
                _unitOfWork.CompanyTransactionKind.Update(ObjUpdate);
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
        [Authorize(Roles = "CoOwner,DeleteTransaction")]
        public JsonResult DeleteCompanyTransactionKind(CompanyTransactionKind ObjDelete)
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
                if (ObjDelete.MonthlySerial)
                {
                    var Obj = new CompanyTransactionKindMonthlySerial();
                    _unitOfWork.CompanyTransactionKindMonthlySerial.Delete(ObjDelete.CompanyID, ObjDelete.CompanyTransactionKindID);
                }
                _unitOfWork.CompanyTransactionKind.Delete(ObjDelete);
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
        public JsonResult CheckCompanyTransactionKindBeforDelete(int id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            if (id != 0)
            {
                int CompanyTransactionKindID = _unitOfWork.NativeSql.CheckCompanyTransactionKind(UserInfo.fCompanyId, id);

                return Json(CompanyTransactionKindID, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult CheckIfMonthlySerial(int id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            if (id != 0)
            {
                int CompanyTransactionKindID = _unitOfWork.NativeSql.CheckIfMonthlySerial(UserInfo.fCompanyId, id);

                return Json(CompanyTransactionKindID, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
    }
}