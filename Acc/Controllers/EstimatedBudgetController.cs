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
    public class EstimatedBudgetController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public EstimatedBudgetController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        // GET: EstimatedBudget

        [Authorize(Roles = "CoOwner,ShowEstimatedbudget")]
        public ActionResult Index()
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            EstimatedBudgetVM Obj = new EstimatedBudgetVM
            {
                WorkWithCostCenter = Company.WorkWithCostCenter
            };
            return View(Obj);
        }

        [HttpPost]
        [Authorize(Roles = "CoOwner,AddEstimatedbudget")]
        public JsonResult Save(EstimatedBudget ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();


            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            int CurrYear = UserInfo.CurrYear;

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

            try

            {

                if (ObjToSave.BudgetType == 1)
                {
                    if (String.IsNullOrEmpty(ObjToSave.AccountNumber))
                    {
                        Msg.Code = 0;
                        Msg.Msg = Resources.Resource.Pleaseentertheaccountnumber;
                        return Json(Msg, JsonRequestBehavior.AllowGet);

                    }

                }
                else if (ObjToSave.BudgetType == 2)
                {
                    if (String.IsNullOrEmpty(ObjToSave.CostCenterNumber))
                    {
                        Msg.Code = 0;
                        Msg.Msg = Resources.Resource.PleaseentertheCostCenter;
                        return Json(Msg, JsonRequestBehavior.AllowGet);

                    }  

                }
               else if (ObjToSave.BudgetType == 3)
                {
                    if (String.IsNullOrEmpty(ObjToSave.CostCenterNumber))
                    {
                        Msg.Code = 0;
                        Msg.Msg = Resources.Resource.PleaseentertheCostCenter;
                        return Json(Msg, JsonRequestBehavior.AllowGet);

                    }
                    else
                    if (String.IsNullOrEmpty(ObjToSave.AccountNumber))
                    {
                        Msg.Code = 0;
                        Msg.Msg = Resources.Resource.Pleaseentertheaccountnumber;
                        return Json(Msg, JsonRequestBehavior.AllowGet);

                    }
                }


                if (!String.IsNullOrEmpty(ObjToSave.AccountNumber))
                {
                    var Acc = _unitOfWork.ChartOfAccount.GetAccountByID(UserInfo.fCompanyId, ObjToSave.AccountNumber);

                    if (Acc == null)
                    {
                        Msg.Msg = Resources.Resource.ThisAccountNoDoseNotExist + " : " + ObjToSave.AccountNumber;
                        Msg.Code = 0;
                        return Json(Msg, JsonRequestBehavior.AllowGet);
                    }
                }
                if (!String.IsNullOrEmpty(ObjToSave.CostCenterNumber))
                {
                    var Acc = _unitOfWork.CostCenter.GetCostCenterById(UserInfo.fCompanyId, ObjToSave.CostCenterNumber);

                    if (Acc == null)
                    {
                        Msg.Msg = Resources.Resource.ThisAccountNoDoseNotExist + " : " + ObjToSave.CostCenterNumber;
                        Msg.Code = 0;
                        return Json(Msg, JsonRequestBehavior.AllowGet);
                    }
                }

                ObjToSave.CompanyID = UserInfo.fCompanyId;
                ObjToSave.CompanyYear = CurrYear;
 
                _unitOfWork.EstimatedBudget.Add(ObjToSave);
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
        [HttpGet]
        public JsonResult GetEstimatedBudget(string id,int id2)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                int CurrYear = UserInfo.CurrYear;
                var D = _unitOfWork.EstimatedBudget.GetEstimatedBudgetForAcc(UserInfo.fCompanyId, CurrYear, id,id2);
                if (D != null)
                {
                    var AccInfo = _unitOfWork.ChartOfAccount.GetAccountByID(UserInfo.fCompanyId, D.AccountNumber);
                    if (AccInfo != null)
                    {
                        if (Resources.Resource.CurLang == "Arb")
                        {
                            D.AccountName = AccInfo.ArabicName;
                        }
                        else
                        {
                            D.AccountName = AccInfo.EnglishName;
                        }
                        
                    }
                    
                   if(!String.IsNullOrEmpty(D.CostCenterNumber))
                    {
                        var CostName = _unitOfWork.CostCenter.GetCostByID(UserInfo.fCompanyId, D.CostCenterNumber);
                        if (CostName != null)
                        {
                            if (Resources.Resource.CurLang == "Arb")
                            {
                                D.CostName = AccInfo.ArabicName;
                            }
                            else
                            {
                                D.CostName = AccInfo.EnglishName;
                            }
                        }
                    }
                    
                    return Json(D, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new EstimatedBudget(), JsonRequestBehavior.AllowGet);
                }



                
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new EstimatedBudget(), JsonRequestBehavior.AllowGet);
            }

        }
    }
}