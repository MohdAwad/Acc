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
    public class SupplierInformationController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public SupplierInformationController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }

        [Authorize(Roles = "CoOwner,ShowCreditNote")]

        public ActionResult Index()
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var SupplierFatherAccountObj = _unitOfWork.NativeSql.GetClientFatherAccount(CoInfo.CompanyID);
            var FatherAccountNumber = _unitOfWork.NativeSql.GetClientFatherAccount(CoInfo.CompanyID).First();
            var FatherAccount = _unitOfWork.ChartOfAccount.GetChartOfAccountById(CoInfo.CompanyID, FatherAccountNumber.AccountNumber);
            ChartOfAccountSupplierVM Obj = new ChartOfAccountSupplierVM();
            Obj.SupplierFatherAccount = SupplierFatherAccountObj;
            Obj.AccountType = _unitOfWork.AccountType.GetAllAccountType();
            Obj.AccountChart = CoInfo.AccountChart;
            Obj.AccountChartZero = CoInfo.AccountChartZero;
            Obj.AccountTypeID = FatherAccount.AccountTypeID;
            Obj.AccountKind = 2;
            Obj.AccountLevel = FatherAccount.AccountLevel + 1;
            Obj.AccountFather = FatherAccount.AccountNumber;
            Obj.AccountFatherName = FatherAccount.ArabicName;
            Obj.LevelZero = FunctionUnit.GetLevelChart(CoInfo.AccountChart, Obj.AccountLevel);
            int LevelZeroLength = Obj.LevelZero.Length;
            int AccountNumberLength = _unitOfWork.NativeSql.GetMaxChartNumberChild(CoInfo.CompanyID, Obj.AccountFather).ToString().Length;
            string NewAccountNumber = "";
            if (LevelZeroLength == AccountNumberLength)
            {
                Obj.AccountNumber = _unitOfWork.NativeSql.GetMaxChartNumberChild(CoInfo.CompanyID, Obj.AccountFather).ToString();
            }
            else
            {
                LevelZeroLength = LevelZeroLength - AccountNumberLength;
                for (int i = 1; i <= LevelZeroLength; i++)
                {
                    NewAccountNumber = NewAccountNumber + "0";
                }
                Obj.AccountNumber = NewAccountNumber + _unitOfWork.NativeSql.GetMaxChartNumberChild(CoInfo.CompanyID, Obj.AccountFather).ToString();
            }
            Obj.Currency = _unitOfWork.Currency.GetAllCurrency(CoInfo.CompanyID);
            Obj.CurrencyID = FatherAccount.CurrencyID;
            Obj.SupplierCountry = _unitOfWork.SupplierCountry.GetAllSupplierCountry(CoInfo.CompanyID);
            Obj.SupplierCountryBank = _unitOfWork.SupplierCountryBank.GetAllSupplierCountryBank(CoInfo.CompanyID);
            Obj.SupplierBank = _unitOfWork.SupplierBank.GetAllSupplierBank(CoInfo.CompanyID);
            Obj.SupplierCity = _unitOfWork.SupplierCity.GetAllSupplierCityByCountryID(CoInfo.CompanyID, 0);
            Obj.SupplierCityBank = _unitOfWork.SupplierCityBank.GetAllSupplierCityBankByCountryBankID(CoInfo.CompanyID, 0);
            Obj.SupplierBranchBank = _unitOfWork.SupplierBranchBank.GetAllSupplierBranchBankByBankID(CoInfo.CompanyID, 0);
            Obj.SupplierType = _unitOfWork.NativeSql.GetAllSupplierType();

            if (String.IsNullOrEmpty(Obj.LevelZero))
            {
                ViewBag.Error = Resources.Resource.LastAccountSub;

                return PartialView("Error");
            }
            Obj.Sale = _unitOfWork.Sale.GetAllSale(UserInfo.fCompanyId);
            Obj.FSalesID = FatherAccount.SalesID;
            return View(Obj);
        }
        [HttpGet]
        public JsonResult GetMaxFatherAccount(string id)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var FatherAccount = _unitOfWork.ChartOfAccount.GetChartOfAccountById(CoInfo.CompanyID, id);
            if (id != "")
            {
                ChartOfAccountMaxVM Obj = new ChartOfAccountMaxVM();
                Obj.AccountLevel = FatherAccount.AccountLevel + 1;
                Obj.LevelZero = FunctionUnit.GetLevelChart(CoInfo.AccountChart, Obj.AccountLevel);
                int LevelZeroLength = Obj.LevelZero.Length;
                int AccountNumberLength = _unitOfWork.NativeSql.GetMaxChartNumberChild(CoInfo.CompanyID, id).ToString().Length;
                string NewAccountNumber = "";
                if (LevelZeroLength == AccountNumberLength)
                {
                    Obj.AccountNumber = _unitOfWork.NativeSql.GetMaxChartNumberChild(CoInfo.CompanyID, id).ToString();
                }
                else
                {
                    LevelZeroLength = LevelZeroLength - AccountNumberLength;
                    for (int i = 1; i <= LevelZeroLength; i++)
                    {
                        NewAccountNumber = NewAccountNumber + "0";
                    }
                    Obj.AccountNumber = NewAccountNumber + _unitOfWork.NativeSql.GetMaxChartNumberChild(CoInfo.CompanyID, id).ToString();
                }
                return Json(Obj, JsonRequestBehavior.AllowGet);
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }
    }
}