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
    public class CurrencyValueController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public CurrencyValueController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        // GET: CurrencyValue

        [Authorize(Roles = "CoOwner,ShowCurrancyValue")]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            return View();
        }

        [Authorize(Roles = "CoOwner,AddCurrancyValue")]

        public ActionResult AddNew()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            CurrencyValueVM Obj = new CurrencyValueVM
            {
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId)
            };
            return PartialView(Obj);
        }

        [HttpGet]
        public    JsonResult  GetCurrencyValueById(int id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            if (id != 0)
            {
                var repo =   _unitOfWork.NativeSql.GetCurrencyValueByID(UserInfo.fCompanyId, id);
                if (repo == null)
                {
                    repo = new CurrencyValueVM
                    {
                        ConversionFactor = 0
                    };
                };

                return Json(repo.ConversionFactor, JsonRequestBehavior.AllowGet);
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAllCurrencyValue()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllCurrencyValue = _unitOfWork.NativeSql.GetAllCurrencyValue(UserInfo.fCompanyId);
                if (AllCurrencyValue == null)
                {
                    return Json(new List<CurrencyValueVM>(), JsonRequestBehavior.AllowGet);
                }

                foreach( var c in AllCurrencyValue)
                {
                    c.sInsDateTime = c.InsDateTime.ToString("dd/MM/yyyy");
                }
                return Json(AllCurrencyValue, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<CurrencyValueVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]

        public JsonResult SaveCurrencyValue(CurrencyValue ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                ObjToSave.InsDateTime = DateTime.Now;
                ObjToSave.InsUserID = userId;
                ObjToSave.CompanyID = UserInfo.fCompanyId;

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
                _unitOfWork.CurrencyValue.Add(ObjToSave);
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
    }
}