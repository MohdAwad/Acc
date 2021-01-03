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
    public class TransActionFileController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransActionFileController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }

        [HttpPost]
        public JsonResult SaveTransActionFile(TransActionFile ObjSave)
        {

            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                ObjSave.Id = _unitOfWork.TransActionFile.GetMaxSerial(UserInfo.fCompanyId);


                ObjSave.CompanyId = UserInfo.fCompanyId;

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
                _unitOfWork.TransActionFile.Add(ObjSave);
                _unitOfWork.Complete();
                Msg.LastID = _unitOfWork.TransActionFile.GetMaxSerial(UserInfo.fCompanyId).ToString();
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

        [HttpGet]
        public JsonResult GetAllTransActionFile(int id , string id2 , string id3 , string id4)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var Type = _unitOfWork.TransActionFile.GetType(UserInfo.fCompanyId);

                int year = id;
                string VocherNumber = id2;
                string TransactionKindNo = id3;
                string CompanyTransactionKindNo = id4;

                var TransAllFiles = _unitOfWork.TransActionFile.GetAllTransactionFile(year,VocherNumber,TransactionKindNo, CompanyTransactionKindNo);
                if (TransAllFiles == null)
                {
                    return Json(new List<TransActionFile>(), JsonRequestBehavior.AllowGet);
                }

                return Json(TransAllFiles, JsonRequestBehavior.AllowGet);
            }
            
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<TransActionFile>(), JsonRequestBehavior.AllowGet);
            }

        }


        public JsonResult UpdateTransActionFile(TransActionFile ObjUpdate)
        {


            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
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
                _unitOfWork.TransActionFile.Update(ObjUpdate);
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