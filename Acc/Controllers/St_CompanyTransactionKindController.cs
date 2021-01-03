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
    public class St_CompanyTransactionKindController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public St_CompanyTransactionKindController()
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
        public JsonResult GetAllSt_CoampnyTransactionKind()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllSt_CompanyTransactionKind = _unitOfWork.NativeSql.GetAllSt_CompanyTransactionKind(UserInfo.fCompanyId);
                if (AllSt_CompanyTransactionKind == null)
                {
                    return Json(new List<St_CompanyTransationKindVM>(), JsonRequestBehavior.AllowGet);
                }

                return Json(AllSt_CompanyTransactionKind, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<St_CompanyTransationKindVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult Update(int id)
        {
            try
            {
                if (id != 0)
                {
                    var userId = User.Identity.GetUserId();
                    var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                    var sSt_CompanyTransactionKind = _unitOfWork.St_CompanyTransactionKind.GetSt_CompanyTransactionKindByID(UserInfo.fCompanyId, id);
                    St_CompanyTransationKindVM Obj = new St_CompanyTransationKindVM();
                    Obj.St_CompanyTransactionKindID = sSt_CompanyTransactionKind.St_CompanyTransactionKindID;
                    Obj.St_TransactionKindID = sSt_CompanyTransactionKind.St_TransactionKindID;
                    Obj.TransactionKindName = _unitOfWork.NativeSql.GetSt_TransactionKindName(Obj.St_TransactionKindID);
                    Obj.StockCode = sSt_CompanyTransactionKind.StockCode;
                    if (Obj.StockCode == "*")
                    {
                        if (Resources.Resource.CurLang == "Arb")
                        {
                            Obj.WarehouseName = "كل المستودعات";
                        }
                        else
                        {
                            Obj.WarehouseName = "All Stores";
                        }
                    }
                    else {
                        Obj.WarehouseName = _unitOfWork.NativeSql.GetSt_WarehouseName(UserInfo.fCompanyId,Obj.StockCode);
                    }
                    Obj.AutoSerial = sSt_CompanyTransactionKind.AutoSerial;
                    Obj.SymbolSerial = sSt_CompanyTransactionKind.SymbolSerial;
                    Obj.Symbol = sSt_CompanyTransactionKind.Symbol;
                    Obj.Serial = sSt_CompanyTransactionKind.Serial;
                    if (Obj.SymbolSerial)
                    {
                        string SerialNumber = "";
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
                        Obj.Example = Obj.Symbol + SerialNumber;
                    }

                    return PartialView("Update", Obj);
                }
                return PartialView("Update", new St_CompanyTransactionKind());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return View("Error");
            }
        }
        public JsonResult UpdateSt_CompanyTransactionKind(St_CompanyTransactionKind ObjUpdate)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                ObjUpdate.InsDateTime = DateTime.Now;
                ObjUpdate.InsUserID = userId;
                ObjUpdate.CompanyID = UserInfo.fCompanyId;

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
                if (ObjUpdate.SymbolSerial)
                {
                    var Obj = new St_CompanyTransactionKindSymbolSerial();
                    var CheckIfSave = _unitOfWork.St_CompanyTransactionKindSymbolSerial.CheckIfSave(ObjUpdate.CompanyID, ObjUpdate.St_CompanyTransactionKindID);
                    if (CheckIfSave == null)
                    {
                        _unitOfWork.St_CompanyTransactionKindSymbolSerial.Delete(ObjUpdate.CompanyID, ObjUpdate.St_CompanyTransactionKindID);
                        _unitOfWork.Complete();
                        Obj.CompanyID = ObjUpdate.CompanyID;
                        Obj.StockCode = ObjUpdate.StockCode;
                        Obj.St_CompanyTransactionKindID = ObjUpdate.St_CompanyTransactionKindID;
                        Obj.LastSerial = 0;
                        _unitOfWork.St_CompanyTransactionKindSymbolSerial.Add(Obj);
                        _unitOfWork.Complete();
                    }
                }
                else
                {
                    var Obj = new St_CompanyTransactionKindSymbolSerial();
                    var CheckIfSave = _unitOfWork.St_CompanyTransactionKindSymbolSerial.CheckIfSave(ObjUpdate.CompanyID, ObjUpdate.St_CompanyTransactionKindID);
                    if (CheckIfSave != null)
                    {
                        _unitOfWork.St_CompanyTransactionKindSymbolSerial.Delete(ObjUpdate.CompanyID, ObjUpdate.St_CompanyTransactionKindID);
                        _unitOfWork.Complete();
                    }
                }
                _unitOfWork.St_CompanyTransactionKind.Update(ObjUpdate);
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
        public JsonResult CheckST_CompanyTransactionKindBeforDelete(int id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            if (id != 0)
            {
                int CompanyTransactionKindID = _unitOfWork.NativeSql.CheckSt_CompanyTransactionKind(UserInfo.fCompanyId, id);

                return Json(CompanyTransactionKindID, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
    }
}