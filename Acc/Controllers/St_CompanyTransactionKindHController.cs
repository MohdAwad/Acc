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
    public class St_CompanyTransactionKindHController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public St_CompanyTransactionKindHController()
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
        public JsonResult GetAllSt_CoampnyTransactionKindH()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllSt_CompanyTransactionKindH = _unitOfWork.NativeSql.GetAllSt_CompanyTransactionKindH(UserInfo.fCompanyId);
                if (AllSt_CompanyTransactionKindH == null)
                {
                    return Json(new List<St_CompanyTransationKindHVM>(), JsonRequestBehavior.AllowGet);
                }

                return Json(AllSt_CompanyTransactionKindH, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<St_CompanyTransationKindHVM>(), JsonRequestBehavior.AllowGet);
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
                    var sSt_CompanyTransactionKindH = _unitOfWork.St_CompanyTransactionKindH.GetSt_CompanyTransactionKindHByID(UserInfo.fCompanyId, id);
                    St_CompanyTransationKindHVM Obj = new St_CompanyTransationKindHVM();
                    Obj.St_CompanyTransactionKindID = sSt_CompanyTransactionKindH.St_CompanyTransactionKindID;
                    Obj.St_TransactionKindID = sSt_CompanyTransactionKindH.St_TransactionKindID;
                    Obj.TransactionKindName = _unitOfWork.NativeSql.GetSt_TransactionKindHName(Obj.St_TransactionKindID);
                    Obj.StockCode = sSt_CompanyTransactionKindH.StockCode;
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
                    else
                    {
                        Obj.WarehouseName = _unitOfWork.NativeSql.GetSt_WarehouseHName(UserInfo.fCompanyId, Obj.StockCode);
                    }
                    Obj.AutoSerial = sSt_CompanyTransactionKindH.AutoSerial;
                    Obj.SymbolSerial = sSt_CompanyTransactionKindH.SymbolSerial;
                    Obj.Symbol = sSt_CompanyTransactionKindH.Symbol;
                    Obj.Serial = sSt_CompanyTransactionKindH.Serial;
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
                return PartialView("Update", new St_CompanyTransactionKindH());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return View("Error");
            }
        }
        public JsonResult UpdateSt_CompanyTransactionKindH(St_CompanyTransactionKindH ObjUpdate)
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
                if (ObjUpdate.SymbolSerial)
                {
                    var Obj = new St_CompanyTransactionKindSymbolSerialH();
                    var CheckIfSave = _unitOfWork.St_CompanyTransactionKindSymbolSerialH.CheckIfSave(ObjUpdate.CompanyID, ObjUpdate.St_CompanyTransactionKindID);
                    if (CheckIfSave == null)
                    {
                        _unitOfWork.St_CompanyTransactionKindSymbolSerialH.Delete(ObjUpdate.CompanyID, ObjUpdate.St_CompanyTransactionKindID);
                        _unitOfWork.Complete();
                        Obj.CompanyID = ObjUpdate.CompanyID;
                        Obj.StockCode = ObjUpdate.StockCode;
                        Obj.St_CompanyTransactionKindID = ObjUpdate.St_CompanyTransactionKindID;
                        Obj.LastSerial = 0;
                        _unitOfWork.St_CompanyTransactionKindSymbolSerialH.Add(Obj);
                        _unitOfWork.Complete();
                    }
                }
                else
                {
                    var Obj = new St_CompanyTransactionKindSymbolSerialH();
                    var CheckIfSave = _unitOfWork.St_CompanyTransactionKindSymbolSerialH.CheckIfSave(ObjUpdate.CompanyID, ObjUpdate.St_CompanyTransactionKindID);
                    if (CheckIfSave != null)
                    {
                        _unitOfWork.St_CompanyTransactionKindSymbolSerialH.Delete(ObjUpdate.CompanyID, ObjUpdate.St_CompanyTransactionKindID);
                        _unitOfWork.Complete();
                    }
                }
                _unitOfWork.St_CompanyTransactionKindH.Update(ObjUpdate);
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
    }
}