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
    public class CostCenterController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public CostCenterController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        public ActionResult TestDev()
        {



            return View();
        }

        [Authorize(Roles = "CoOwner,AddmainaccountChartofcostcenter")]
        public ActionResult CreateAccount()
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            CostCenter Obj = new CostCenter();
            
            Obj.CostChart = CoInfo.CostChart;
            Obj.CostZero = CoInfo.CostChartZero;
      

            return View(Obj);
        }
        [HttpGet]
        public JsonResult GetChartTree()
        {
            try
            {


                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllCahrt = _unitOfWork.CostCenter.GetAllCostCenter(UserInfo.fCompanyId);
                List<ChartTreeVM> ChartList = new List<ChartTreeVM>();
                foreach (var c in AllCahrt)
                {
                    ChartTreeVM ObjNew = new ChartTreeVM();
                    ObjNew.id = c.CostNumber;
                    if (String.IsNullOrEmpty(c.CostFather))
                    {
                        ObjNew.parent = "#";
                    }
                    else
                        ObjNew.parent = c.CostFather;

                    ObjNew.AccountName = c.ArabicName;
                    ObjNew.text = c.CostNumber + "-" + c.ArabicName;

                    ChartList.Add(ObjNew);

                }
                return Json(ChartList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<ChartTreeVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CostCenter()
        {
            return View();
        }
        [Authorize(Roles = "CoOwner,AddmainaccountChartofcostcenter")]
        public ActionResult AddNewFatherAccount(int id)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            CostCenter Obj = new CostCenter();
         
            Obj.CostChart = CoInfo.CostChart;
            Obj.CostZero = CoInfo.CostChartZero;
           
            Obj.CostLevel = 1;
            Obj.LevelZero = FunctionUnit.GetLevelChart(CoInfo.CostChart, Obj.CostLevel);

            //Obj.LevelZero = Obj.LevelZero.Count().ToString();

            return PartialView("AddNewFatherAccount", Obj);
        }
        [Authorize(Roles = "CoOwner,AddmainaccountChartofcostcenter")]
        public ActionResult AddNewChildAccount(string id)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var FatherAccount = _unitOfWork.CostCenter.GetCostCenterById(CoInfo.CompanyID, id);

            CostCenter Obj = new CostCenter();
         
            Obj.CostChart = CoInfo.CostChart;
            Obj.CostZero = CoInfo.CostChartZero;
             
            Obj.CostLevel = FatherAccount.CostLevel + 1;
            Obj.CostFather = FatherAccount.CostNumber;
            Obj.CostFatherName = FatherAccount.ArabicName;
            Obj.CostNumber = "";// _unitOfWork.NativeSql.GetMaxCostNumberChild(CoInfo.CompanyID, Obj.CostFather).ToString();
       

            Obj.LevelZero = FunctionUnit.GetLevelChart(CoInfo.CostChart, Obj.CostLevel);

            //Obj.LevelZero = Obj.LevelZero.Count().ToString();

            if (String.IsNullOrEmpty(Obj.LevelZero))
            {
                ViewBag.Error = Resources.Resource.LastAccountSub;

                return PartialView("Error");
            }
            else
                return PartialView("AddNewChildAccount", Obj);






        }

        [Authorize(Roles = "CoOwner,DeleteChartofcostcenter")]
        public ActionResult DeleteAccount(string id)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var Account = _unitOfWork.CostCenter.GetCostCenterById(CoInfo.CompanyID, id);
            CostCenter Obj = new CostCenter();
            Obj.ArabicName = Account.ArabicName;
            Obj.EnglishName = Account.EnglishName;
            Obj.CostNumber = Account.CostNumber;
            Obj.Note = Account.Note;
            Obj.StoppedCost = Account.StoppedCost;
            return PartialView("DeleteAccount", Obj);
        }
        [HttpPost]
        [Authorize(Roles = "CoOwner,DeleteChartofcostcenter")]
        public JsonResult DeleteAccount(CostCenter ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();


            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            ObjToSave.InsDateTime = DateTime.Now;
            //  ObjToSave.i = UserID;

            ObjToSave.CompanyID = UserInfo.fCompanyId;
            var userId = User.Identity.GetUserId();
            ObjToSave.CompanyYear = UserInfo.CurrYear;



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
            string CheckCostCenter = _unitOfWork.NativeSql.CheckCostCenter(UserInfo.fCompanyId, ObjToSave.CostNumber);
            try
            {
                if (CheckCostCenter != "")
                {

                    Msg.Msg = Resources.Resource.TheCostCenterContainsTransactions;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }
                else if (_unitOfWork.NativeSql.GetCostChild(UserInfo.fCompanyId, ObjToSave.CostNumber).Count() > 0)
                {
                    Msg.Msg = Resources.Resource.TheCostCenterContainsbranches;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }

                

                _unitOfWork.CostCenter.Delete(ObjToSave);
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
        [Authorize(Roles = "CoOwner,UpdateChartofcostcenter")]
        public ActionResult UpdateAccount(string id)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var Account = _unitOfWork.CostCenter.GetCostCenterById(CoInfo.CompanyID, id);

            CostCenter Obj = new CostCenter();
          
            Obj.ArabicName = Account.ArabicName;
            Obj.EnglishName = Account.EnglishName;
            Obj.CostNumber = Account.CostNumber;
            Obj.Note = Account.Note;
            Obj.StoppedCost = Account.StoppedCost;
          



            return PartialView("UpdateAccount", Obj);



        }
        [HttpPost]
        [Authorize(Roles = "CoOwner,UpdateChartofcostcenter")]
        public JsonResult UpdateAccount(CostCenter ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            ObjToSave.InsDateTime = DateTime.Now;
            if (String.IsNullOrEmpty(ObjToSave.EnglishName))
                ObjToSave.EnglishName = ObjToSave.ArabicName;
            ObjToSave.CompanyID = UserInfo.fCompanyId;
            var userId = User.Identity.GetUserId();
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
                _unitOfWork.CostCenter.Update(ObjToSave);
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
        [Authorize(Roles = "CoOwner,AddmainaccountChartofcostcenter")]
        public JsonResult SaveChildAccount(CostCenter ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            ObjToSave.InsDateTime = DateTime.Now;
            ObjToSave.CompanyID = UserInfo.fCompanyId;
            var userId = User.Identity.GetUserId();
            ObjToSave.CompanyYear = UserInfo.CurrYear;
            ObjToSave.CostNumber = FunctionUnit.RemoveAccountDash(ObjToSave.CostNumber);
            ObjToSave.IsFirstLevel = false;
            ObjToSave.CostNumber = ObjToSave.CostNumber.PadLeft(ObjToSave.LevelZero.Count(), '0');
            ObjToSave.OrignalCost = ObjToSave.CostNumber;
            ObjToSave.CostNumber = ObjToSave.CostFather + ObjToSave.CostNumber;
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
            var CostInfo = _unitOfWork.CostCenter.GetCostByID(UserInfo.fCompanyId, ObjToSave.CostNumber);
            try
            {
                if (CostInfo != null)
                {

                    Msg.Msg = Resources.Resource.TheCostCenterIsExist;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }
                _unitOfWork.CostCenter.Add(ObjToSave);
                _unitOfWork.Complete();
                Msg.LastID = "";
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
        [HttpPost]
        [Authorize(Roles = "CoOwner,AddmainaccountChartofcostcenter")]
        public JsonResult Save(CostCenter ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();


            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            ObjToSave.InsDateTime = DateTime.Now;
            //  ObjToSave.i = UserID;
      
            ObjToSave.CompanyID = UserInfo.fCompanyId;
            var userId = User.Identity.GetUserId();
            ObjToSave.CompanyYear = UserInfo.CurrYear;
            ObjToSave. CostFather = FunctionUnit.RemoveAccountDash(ObjToSave.CostFather);
            ObjToSave.CostNumber = FunctionUnit.RemoveAccountDash(ObjToSave.CostNumber);
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
                _unitOfWork.CostCenter.Add(ObjToSave);
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
        [HttpPost]
        [Authorize(Roles = "CoOwner,AddmainaccountChartofcostcenter")]
        public JsonResult SaveFatherAccount(CostCenter ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();


            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            ObjToSave.InsDateTime = DateTime.Now;
            //  ObjToSave.i = UserID;
         
            ObjToSave.CompanyID = UserInfo.fCompanyId;
            var userId = User.Identity.GetUserId();
            ObjToSave.CompanyYear = UserInfo.CurrYear;
            ObjToSave.CostFather = "0";
            ObjToSave.CostNumber = FunctionUnit.RemoveAccountDash(ObjToSave.CostNumber);
            ObjToSave.IsFirstLevel = true;
            ObjToSave.CostLevel = 1;

            ObjToSave.CostNumber = ObjToSave. CostNumber.PadLeft(ObjToSave.LevelZero.Count(), '0');
            ObjToSave.OrignalCost = ObjToSave.CostNumber;

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
            var CostInfo = _unitOfWork.CostCenter.GetCostByID(UserInfo.fCompanyId, ObjToSave.CostNumber);
            try
            {
                if (CostInfo != null)
                {

                    Msg.Msg = Resources.Resource.TheCostCenterIsExist;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }
                _unitOfWork.CostCenter.Add(ObjToSave);
                _unitOfWork.Complete();

                Msg.LastID = "";// _unitOfWork.NativeSql.GetMaxCostNumberFather(ObjToSave.CompanyID).ToString();



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
        Acc.Persistence.ApplicationDbContext db = new Acc.Persistence.ApplicationDbContext();
        [ValidateInput(false)]
        public ActionResult TreeListPartial()
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            var model = _unitOfWork.CostCenter.GetAllCostCenter(UserInfo.fCompanyId);
            return PartialView("_TreeListPartial", model.ToList());
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult TreeListPartialLoad()
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            var model = _unitOfWork.CostCenter.GetAllCostCenter(UserInfo.fCompanyId);
            return PartialView("_TreeListPartial", model.ToList());
        }
        [HttpGet]
        public JsonResult CheckCostCenter(string id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);

            var CostInfo = _unitOfWork.CostCenter.GetCostByID(UserInfo.fCompanyId, id);
            if (CostInfo == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(CostInfo.CostNumber, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public JsonResult GetCostCenterInfo(string id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);

            var CostInfo = _unitOfWork.CostCenter.GetCostByID(UserInfo.fCompanyId, id);
            if (CostInfo == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(CostInfo, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public JsonResult CheckCostCenterInfo(string id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);

            var CostInfo = _unitOfWork.NativeSql.CheckTransActionCostCenter(UserInfo.fCompanyId, id);
            if (CostInfo == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(CostInfo, JsonRequestBehavior.AllowGet);
            }

        }
    }
}