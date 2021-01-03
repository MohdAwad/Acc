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
    public class ChartOfAccountController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public ChartOfAccountController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        public ActionResult TestDev()
        {
            return View();
        }

        [Authorize(Roles = "CoOwner,AddmainaccountChartofaccount")]
        public ActionResult CreateAccount()
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            ChartOfAccountVM Obj = new ChartOfAccountVM();
            Obj.AccountType = _unitOfWork.AccountType.GetAllAccountType();
            Obj.AccountChart = CoInfo.AccountChart;
            Obj.AccountChartZero = CoInfo.AccountChartZero;
            Obj.AccountTypeID = 1;
            Obj.Sale = _unitOfWork.Sale.GetAllSale(UserInfo.fCompanyId);
            Obj.FSalesID =1;

            return View(Obj);
        }
        [HttpGet]
        public JsonResult GetChartTree()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllCahrt = _unitOfWork.ChartOfAccount.GetAllChartOfAccount(UserInfo.fCompanyId);
                List<ChartTreeVM> ChartList = new List<ChartTreeVM>();
                foreach (var c in AllCahrt)
                {
                    ChartTreeVM ObjNew = new ChartTreeVM();
                    ObjNew.id = c.AccountNumber;
                    if (String.IsNullOrEmpty(c.AccountFather))
                    {
                        ObjNew.parent = "#";
                    }
                    else
                        ObjNew.parent = c.AccountFather;
                    ObjNew.AccountName = c.ArabicName;
                    ObjNew.text = c.AccountNumber + "-" + c.ArabicName;

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

        [Authorize(Roles = "CoOwner,ShowChartOfAccount")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "CoOwner,ShowChartOfAccount")]
        public ActionResult ChartOfAccount()
        {
            return View();
        }

        [Authorize(Roles = "CoOwner,AddmainaccountChartofaccount")]
        public ActionResult AddNewFatherAccount(int id)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            if (id == 1)
            {
                ChartOfAccountClientVM Obj = new ChartOfAccountClientVM();
                Obj.AccountType = _unitOfWork.AccountType.GetAllAccountType();
                Obj.AccountTypeID = 1;
                Obj.AccountChart = CoInfo.AccountChart;
                Obj.AccountChartZero = CoInfo.AccountChartZero;
                Obj.AccountTypeID = 1;
                Obj.AccountLevel = 1;
                Obj.LevelZero = FunctionUnit.GetLevelChart(CoInfo.AccountChart, Obj.AccountLevel);
                int LevelZeroLength = Obj.LevelZero.Length;
                int AccountNumberLength = _unitOfWork.NativeSql.GetMaxChartNumberFather(CoInfo.CompanyID).ToString().Length;
                string NewAccountNumber = "";
                if (LevelZeroLength == AccountNumberLength)
                {
                    Obj.AccountNumber = _unitOfWork.NativeSql.GetMaxChartNumberFather(CoInfo.CompanyID).ToString();
                }
                else
                {
                    LevelZeroLength = LevelZeroLength - AccountNumberLength;
                    for (int i = 1; i <= LevelZeroLength; i++)
                    {
                        NewAccountNumber = NewAccountNumber + "0";
                    }
                    Obj.AccountNumber = NewAccountNumber + _unitOfWork.NativeSql.GetMaxChartNumberFather(CoInfo.CompanyID).ToString();
                }
                Obj.Currency = _unitOfWork.Currency.GetAllCurrency(CoInfo.CompanyID);
                Obj.CurrencyID = 1;
                Obj.AccountKind = id;
                Obj.Sale = _unitOfWork.Sale.GetAllSale(UserInfo.fCompanyId);
                Obj.FSalesID = 1;
                Obj.CustomerCity = _unitOfWork.CustomerCity.GetAllCustomerCity(CoInfo.CompanyID);
                Obj.CustomerArea = _unitOfWork.CustomerArea.GetAllCustomerAreaByCityID(CoInfo.CompanyID,0);
                return PartialView("AddNewFatherAccountClient", Obj);
            }
            else if(id==2)
            {
                ChartOfAccountSupplierVM Obj = new ChartOfAccountSupplierVM();
                Obj.AccountType = _unitOfWork.AccountType.GetAllAccountType();
                Obj.AccountTypeID = 1;
                Obj.AccountChart = CoInfo.AccountChart;
                Obj.AccountChartZero = CoInfo.AccountChartZero;
                Obj.AccountTypeID = 2;
                Obj.AccountLevel = 1;
                Obj.LevelZero = FunctionUnit.GetLevelChart(CoInfo.AccountChart, Obj.AccountLevel);
                int LevelZeroLength = Obj.LevelZero.Length;
                int AccountNumberLength = _unitOfWork.NativeSql.GetMaxChartNumberFather(CoInfo.CompanyID).ToString().Length;
                string NewAccountNumber = "";
                if (LevelZeroLength == AccountNumberLength)
                {
                    Obj.AccountNumber = _unitOfWork.NativeSql.GetMaxChartNumberFather(CoInfo.CompanyID).ToString();
                }
                else
                {
                    LevelZeroLength = LevelZeroLength - AccountNumberLength;
                    for (int i = 1; i <= LevelZeroLength; i++)
                    {
                        NewAccountNumber = NewAccountNumber + "0";
                    }
                    Obj.AccountNumber = NewAccountNumber + _unitOfWork.NativeSql.GetMaxChartNumberFather(CoInfo.CompanyID).ToString();
                }
                Obj.Currency = _unitOfWork.Currency.GetAllCurrency(CoInfo.CompanyID);
                Obj.CurrencyID = 1;
                Obj.AccountKind = id;
                Obj.Sale = _unitOfWork.Sale.GetAllSale(UserInfo.fCompanyId);
                Obj.FSalesID = 1;
                Obj.SupplierCountry = _unitOfWork.SupplierCountry.GetAllSupplierCountry(CoInfo.CompanyID);
                Obj.SupplierCountryBank = _unitOfWork.SupplierCountryBank.GetAllSupplierCountryBank(CoInfo.CompanyID);
                Obj.SupplierBank = _unitOfWork.SupplierBank.GetAllSupplierBank(CoInfo.CompanyID);
                Obj.SupplierCity = _unitOfWork.SupplierCity.GetAllSupplierCityByCountryID(CoInfo.CompanyID,0);
                Obj.SupplierCityBank = _unitOfWork.SupplierCityBank.GetAllSupplierCityBankByCountryBankID(CoInfo.CompanyID,0);
                Obj.SupplierBranchBank = _unitOfWork.SupplierBranchBank.GetAllSupplierBranchBankByBankID(CoInfo.CompanyID,0);
                Obj.SupplierType = _unitOfWork.NativeSql.GetAllSupplierType();
                return PartialView("AddNewFatherAccountSupplier", Obj);
            }
            else if (id == 3)
            {
                ChartOfAccountAccreditationVM Obj = new ChartOfAccountAccreditationVM();
                Obj.AccountType = _unitOfWork.AccountType.GetAllAccountType();
                Obj.AccountTypeID = 1;
                Obj.AccountChart = CoInfo.AccountChart;
                Obj.AccountChartZero = CoInfo.AccountChartZero;
                Obj.AccountTypeID = 3;
                Obj.AccountLevel = 1;
                Obj.LevelZero = FunctionUnit.GetLevelChart(CoInfo.AccountChart, Obj.AccountLevel);
                int LevelZeroLength = Obj.LevelZero.Length;
                int AccountNumberLength = _unitOfWork.NativeSql.GetMaxChartNumberFather(CoInfo.CompanyID).ToString().Length;
                string NewAccountNumber = "";
                if (LevelZeroLength == AccountNumberLength)
                {
                    Obj.AccountNumber = _unitOfWork.NativeSql.GetMaxChartNumberFather(CoInfo.CompanyID).ToString();
                }
                else
                {
                    LevelZeroLength = LevelZeroLength - AccountNumberLength;
                    for (int i = 1; i <= LevelZeroLength; i++)
                    {
                        NewAccountNumber = NewAccountNumber + "0";
                    }
                    Obj.AccountNumber = NewAccountNumber + _unitOfWork.NativeSql.GetMaxChartNumberFather(CoInfo.CompanyID).ToString();
                }
                Obj.Currency = _unitOfWork.Currency.GetAllCurrency(CoInfo.CompanyID);
                Obj.CurrencyID = 1;
                Obj.AccountKind = id;
                Obj.DateOfIssuanceOfCredit = DateTime.Now;
                Obj.LastShipmentDate = DateTime.Now;
                Obj.ValidityOfAccreditation = DateTime.Now;
                Obj.Sale = _unitOfWork.Sale.GetAllSale(UserInfo.fCompanyId);
                Obj.FSalesID = 1;
                return PartialView("AddNewFatherAccountAccreditation", Obj);
            }
            else
            {
                ChartOfAccountVM Obj = new ChartOfAccountVM();
                Obj.AccountType = _unitOfWork.AccountType.GetAllAccountType();
                Obj.AccountTypeID = 1;
                Obj.AccountChart = CoInfo.AccountChart;
                Obj.AccountChartZero = CoInfo.AccountChartZero;
                Obj.AccountLevel = 1;
                Obj.LevelZero = FunctionUnit.GetLevelChart(CoInfo.AccountChart, Obj.AccountLevel);
                int LevelZeroLength = Obj.LevelZero.Length;
                int AccountNumberLength = _unitOfWork.NativeSql.GetMaxChartNumberFather(CoInfo.CompanyID).ToString().Length;
                string NewAccountNumber = "";
                if (LevelZeroLength == AccountNumberLength)
                {
                    Obj.AccountNumber = _unitOfWork.NativeSql.GetMaxChartNumberFather(CoInfo.CompanyID).ToString();
                }
                else
                {
                    LevelZeroLength = LevelZeroLength - AccountNumberLength;
                    for (int i = 1; i <= LevelZeroLength; i++)
                    {
                        NewAccountNumber = NewAccountNumber + "0";
                    }
                    Obj.AccountNumber = NewAccountNumber + _unitOfWork.NativeSql.GetMaxChartNumberFather(CoInfo.CompanyID).ToString();
                }
                Obj.Currency = _unitOfWork.Currency.GetAllCurrency(CoInfo.CompanyID);
                Obj.CurrencyID = 1;
                Obj.AccountKind = id;
                Obj.Sale = _unitOfWork.Sale.GetAllSale(UserInfo.fCompanyId);
                Obj.FSalesID = 1;
                return PartialView("AddNewFatherAccount", Obj);

            } 
        }

        [Authorize(Roles = "CoOwner,AddmainaccountChartofaccount")]
        public ActionResult AddNewChildAccount(string id,string id2)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var FatherAccount = _unitOfWork.ChartOfAccount.GetChartOfAccountById(CoInfo.CompanyID, id);
            id2 = FatherAccount.AccountKind.ToString();
            if (_unitOfWork.Transaction.CheckAccountTrans(UserInfo.fCompanyId, id) != null)
            {
                ViewBag.Error = Resources.Resource.TheAccountContainsTransactions;
                return PartialView("Error");
            }
            if (id2 == "1")
            {
                ChartOfAccountClientVM Obj = new ChartOfAccountClientVM();
                Obj.AccountType = _unitOfWork.AccountType.GetAllAccountType();
                Obj.AccountChart = CoInfo.AccountChart;
                Obj.AccountChartZero = CoInfo.AccountChartZero;
                Obj.AccountTypeID = FatherAccount.AccountTypeID;
                Obj.AccountKind = 1;
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
                Obj.CustomerCity = _unitOfWork.CustomerCity.GetAllCustomerCity(CoInfo.CompanyID);
                Obj.CustomerArea = _unitOfWork.CustomerArea.GetAllCustomerAreaByCityID(CoInfo.CompanyID, 0);
                Obj.CurrencyID = FatherAccount.CurrencyID;
                if (String.IsNullOrEmpty(Obj.LevelZero))
                {
                    ViewBag.Error = Resources.Resource.LastAccountSub;

                    return PartialView("Error");
                }
                Obj.Sale = _unitOfWork.Sale.GetAllSale(UserInfo.fCompanyId);
                Obj.FSalesID = FatherAccount.SalesID;
                return PartialView("AddNewChildAccountClient", Obj);
            }
            else if (id2 == "2")
            {
                ChartOfAccountSupplierVM Obj = new ChartOfAccountSupplierVM();
                Obj.AccountType = _unitOfWork.AccountType.GetAllAccountType();
                Obj.AccountChart = CoInfo.AccountChart;
                Obj.AccountChartZero = CoInfo.AccountChartZero;
                Obj.AccountTypeID = FatherAccount.AccountTypeID;
                Obj.AccountKind = 2;//2FatherAccount.AccountKind;
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
                return PartialView("AddNewChildAccountSupplier", Obj);

            }
            else if (id2 == "3")
            {
                ChartOfAccountAccreditationVM Obj = new ChartOfAccountAccreditationVM();
                Obj.AccountType = _unitOfWork.AccountType.GetAllAccountType();
                Obj.AccountChart = CoInfo.AccountChart;
                Obj.AccountChartZero = CoInfo.AccountChartZero;
                Obj.AccountTypeID = FatherAccount.AccountTypeID;
                Obj.AccountKind = 3;//FatherAccount.AccountKind;
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
                Obj.DateOfIssuanceOfCredit = DateTime.Now;
                Obj.LastShipmentDate = DateTime.Now;
                Obj.ValidityOfAccreditation = DateTime.Now;

                if (String.IsNullOrEmpty(Obj.LevelZero))
                {
                    ViewBag.Error = Resources.Resource.LastAccountSub;

                    return PartialView("Error");
                }
                Obj.Sale = _unitOfWork.Sale.GetAllSale(UserInfo.fCompanyId);
                Obj.FSalesID = FatherAccount.SalesID;
                return PartialView("AddNewChildAccountAccreditation", Obj);

            }
            else
            {
                ChartOfAccountVM Obj = new ChartOfAccountVM();
                Obj.AccountType = _unitOfWork.AccountType.GetAllAccountType();
                Obj.AccountChart = CoInfo.AccountChart;
                Obj.AccountChartZero = CoInfo.AccountChartZero;
                Obj.AccountTypeID = FatherAccount.AccountTypeID;
                Obj.AccountKind = 0;
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

                if (String.IsNullOrEmpty(Obj.LevelZero))
                {
                    ViewBag.Error = Resources.Resource.LastAccountSub;

                    return PartialView("Error");
                }
                Obj.Sale = _unitOfWork.Sale.GetAllSale(UserInfo.fCompanyId);
                Obj.FSalesID = FatherAccount.SalesID;
                return PartialView("AddNewChildAccount", Obj);

            }
        }
        [Authorize(Roles = "CoOwner,DeleteChartofaccoun")]
        public ActionResult DeleteAccount(string id)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var Account = _unitOfWork.ChartOfAccount.GetChartOfAccountById(CoInfo.CompanyID, id);

            if (Account.AccountKind == 1)
            {
                ChartOfAccountVM Obj = new ChartOfAccountVM();
                Obj.AccountType = _unitOfWork.AccountType.GetAllAccountType();
                Obj.AccountTypeID = Account.AccountTypeID;
                Obj.AccountKind = Account.AccountKind;
                Obj.ArabicName = Account.ArabicName;
                Obj.EnglishName = Account.EnglishName;
                Obj.AccountNumber = Account.AccountNumber;
                Obj.Note = Account.Note;
                Obj.StoppedAccount = Account.StoppedAccount;
                Obj.Currency = _unitOfWork.Currency.GetAllCurrency(CoInfo.CompanyID);
                Obj.CurrencyID = Account.CurrencyID;
                Obj.Sale = _unitOfWork.Sale.GetAllSale(UserInfo.fCompanyId);
                Obj.FSalesID = Account.SalesID;
                return PartialView("DeleteAccount", Obj);
            }
            else if (Account.AccountKind == 2)
            {
                ChartOfAccountVM Obj = new ChartOfAccountVM();
                Obj.AccountType = _unitOfWork.AccountType.GetAllAccountType();
                Obj.AccountTypeID = Account.AccountTypeID;
                Obj.AccountKind = Account.AccountKind;
                Obj.ArabicName = Account.ArabicName;
                Obj.EnglishName = Account.EnglishName;
                Obj.AccountNumber = Account.AccountNumber;
                Obj.Note = Account.Note;
                Obj.StoppedAccount = Account.StoppedAccount;
                Obj.Currency = _unitOfWork.Currency.GetAllCurrency(CoInfo.CompanyID);
                Obj.CurrencyID = Account.CurrencyID;
                Obj.Sale = _unitOfWork.Sale.GetAllSale(UserInfo.fCompanyId);
                Obj.FSalesID = Account.SalesID;
                return PartialView("DeleteAccount", Obj);
            }
            else if (Account.AccountKind == 3)
            {
                ChartOfAccountVM Obj = new ChartOfAccountVM();
                Obj.AccountType = _unitOfWork.AccountType.GetAllAccountType();
                Obj.AccountTypeID = Account.AccountTypeID;
                Obj.AccountKind = Account.AccountKind;
                Obj.ArabicName = Account.ArabicName;
                Obj.EnglishName = Account.EnglishName;
                Obj.AccountNumber = Account.AccountNumber;
                Obj.Note = Account.Note;
                Obj.StoppedAccount = Account.StoppedAccount;
                Obj.Currency = _unitOfWork.Currency.GetAllCurrency(CoInfo.CompanyID);
                Obj.CurrencyID = Account.CurrencyID;
                Obj.Sale = _unitOfWork.Sale.GetAllSale(UserInfo.fCompanyId);
                Obj.FSalesID = Account.SalesID;

                return PartialView("DeleteAccount", Obj);
            }
            else
            {
                ChartOfAccountVM Obj = new ChartOfAccountVM();
                Obj.AccountType = _unitOfWork.AccountType.GetAllAccountType();
                Obj.AccountTypeID = Account.AccountTypeID;
                Obj.AccountKind = Account.AccountKind;
                Obj.ArabicName = Account.ArabicName;
                Obj.EnglishName = Account.EnglishName;
                Obj.AccountNumber = Account.AccountNumber;
                Obj.Note = Account.Note;
                Obj.StoppedAccount = Account.StoppedAccount;
                Obj.Currency = _unitOfWork.Currency.GetAllCurrency(CoInfo.CompanyID);
                Obj.CurrencyID = Account.CurrencyID;
                Obj.Sale = _unitOfWork.Sale.GetAllSale(UserInfo.fCompanyId);
                Obj.FSalesID = Account.SalesID;
                return PartialView("DeleteAccount", Obj);
            }
        }

        [Authorize(Roles = "CoOwner,UpdateChartofaccoun")]
        public ActionResult UpdateAccount(string id)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var Account = _unitOfWork.ChartOfAccount.GetChartOfAccountById(CoInfo.CompanyID, id);
            if (Account.AccountKind == 1)
            {
                ChartOfAccountClientVM Obj = new ChartOfAccountClientVM();
                Obj = _unitOfWork.NativeSql.GetAccountClientInfo(Account.CompanyID, Account.AccountNumber);

                Obj.AccountType = _unitOfWork.AccountType.GetAllAccountType();
                Obj.AccountTypeID = Account.AccountTypeID;
                Obj.AccountKind = Account.AccountKind;
                Obj.ArabicName = Account.ArabicName;
                Obj.EnglishName = Account.EnglishName;
                Obj.AccountNumber = Account.AccountNumber;
                Obj.Note = Account.Note;
                Obj.StoppedAccount = Account.StoppedAccount;
                Obj.Currency = _unitOfWork.Currency.GetAllCurrency(CoInfo.CompanyID);
                Obj.CurrencyID = Account.CurrencyID;
                Obj.Sale = _unitOfWork.Sale.GetAllSale(UserInfo.fCompanyId);
                Obj.CustomerCity = _unitOfWork.CustomerCity.GetAllCustomerCity(CoInfo.CompanyID);
                if (Obj.CityID > 0)
                {
                    Obj.CustomerArea = _unitOfWork.CustomerArea.GetAllCustomerAreaByCityID(CoInfo.CompanyID, Obj.CityID);
                 
                }
                else
                {
                    Obj.CustomerArea = _unitOfWork.CustomerArea.GetAllCustomerAreaByCityID(CoInfo.CompanyID, 0);
                }
                Obj.FSalesID = Account.SalesID;
                return PartialView("UpdateAccountClient", Obj);
            }
            else if (Account.AccountKind == 2)
            {
                ChartOfAccountSupplierVM Obj = new ChartOfAccountSupplierVM();

                Obj = _unitOfWork.NativeSql.GetAccountSupplierInfo(Account.CompanyID, Account.AccountNumber);

                Obj.AccountType = _unitOfWork.AccountType.GetAllAccountType();
                Obj.AccountTypeID = Account.AccountTypeID;
                Obj.AccountKind = Account.AccountKind;
                Obj.ArabicName = Account.ArabicName;
                Obj.EnglishName = Account.EnglishName;
                Obj.AccountNumber = Account.AccountNumber;
                Obj.Note = Account.Note;
                Obj.StoppedAccount = Account.StoppedAccount;
                Obj.Currency = _unitOfWork.Currency.GetAllCurrency(CoInfo.CompanyID);
                Obj.CurrencyID = Account.CurrencyID;
                Obj.Sale = _unitOfWork.Sale.GetAllSale(UserInfo.fCompanyId);
                Obj.FSalesID = Account.SalesID;
                Obj.SupplierCountry = _unitOfWork.SupplierCountry.GetAllSupplierCountry(CoInfo.CompanyID);
                Obj.SupplierCountryBank = _unitOfWork.SupplierCountryBank.GetAllSupplierCountryBank(CoInfo.CompanyID);
                Obj.SupplierBank = _unitOfWork.SupplierBank.GetAllSupplierBank(CoInfo.CompanyID);
                if (Obj.SupplierCityID > 0)
                {
                    Obj.SupplierCity = _unitOfWork.SupplierCity.GetAllSupplierCityByCountryID(CoInfo.CompanyID, Obj.SupplierCityID);
                }
                else {
                    Obj.SupplierCity = _unitOfWork.SupplierCity.GetAllSupplierCityByCountryID(CoInfo.CompanyID, 0);
                }
                if (Obj.SupplierCityBankID > 0)
                {
                    Obj.SupplierCityBank = _unitOfWork.SupplierCityBank.GetAllSupplierCityBankByCountryBankID(CoInfo.CompanyID, Obj.SupplierCityBankID);
                }
                else
                {
                    Obj.SupplierCityBank = _unitOfWork.SupplierCityBank.GetAllSupplierCityBankByCountryBankID(CoInfo.CompanyID, 0);
                }
                if (Obj.SupplierBranchBankID > 0)
                {
                    Obj.SupplierBranchBank = _unitOfWork.SupplierBranchBank.GetAllSupplierBranchBankByBankID(CoInfo.CompanyID, Obj.SupplierBranchBankID);
                }
                else
                {
                    Obj.SupplierBranchBank = _unitOfWork.SupplierBranchBank.GetAllSupplierBranchBankByBankID(CoInfo.CompanyID, 0);
                }
                
                Obj.SupplierType = _unitOfWork.NativeSql.GetAllSupplierType();
                return PartialView("UpdateAccountSupplier", Obj);
            }
            else if (Account.AccountKind == 3)
            {
                ChartOfAccountAccreditationVM Obj = new ChartOfAccountAccreditationVM();
                Obj = _unitOfWork.NativeSql.GetAccountAccreditation(Account.CompanyID, Account.AccountNumber);
                Obj.AccountType = _unitOfWork.AccountType.GetAllAccountType();
                Obj.AccountTypeID = Account.AccountTypeID;
                Obj.AccountKind = Account.AccountKind;
                Obj.ArabicName = Account.ArabicName;
                Obj.EnglishName = Account.EnglishName;
                Obj.AccountNumber = Account.AccountNumber;
                Obj.Note = Account.Note;
                Obj.StoppedAccount = Account.StoppedAccount;
                Obj.Currency = _unitOfWork.Currency.GetAllCurrency(CoInfo.CompanyID);
                Obj.CurrencyID = Account.CurrencyID;
                Obj.Sale = _unitOfWork.Sale.GetAllSale(UserInfo.fCompanyId);
                Obj.FSalesID = Account.SalesID;

                return PartialView("UpdateAccountAccreditation", Obj);
            } 
            else
            {
                ChartOfAccountVM Obj = new ChartOfAccountVM();
                Obj.AccountType = _unitOfWork.AccountType.GetAllAccountType();
                Obj.AccountTypeID = Account.AccountTypeID;
                Obj.AccountKind = Account.AccountKind;
                Obj.ArabicName = Account.ArabicName;
                Obj.EnglishName = Account.EnglishName;
                Obj.AccountNumber = Account.AccountNumber;
                Obj.Note = Account.Note;
                Obj.StoppedAccount = Account.StoppedAccount;
                Obj.Currency = _unitOfWork.Currency.GetAllCurrency(CoInfo.CompanyID);
                Obj.CurrencyID = Account.CurrencyID;
                Obj.Sale = _unitOfWork.Sale.GetAllSale(UserInfo.fCompanyId);
                Obj.FSalesID = Account.SalesID;

                return PartialView("UpdateAccount", Obj);
            }
        }
        [HttpGet]
        public JsonResult GetAccountName(string  id)
        {
            if (!String.IsNullOrWhiteSpace(id))
            {
                string[] words = id.Split(' ');
                id = words[0];
            }
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var AccountInfo = _unitOfWork.ChartOfAccount.GetAccountByID(UserInfo.fCompanyId, id);
            if (AccountInfo == null)
            {
                AccountInfo = new ChartOfAccount
                {
                    AccountNumber = "",
                    ArabicName = ""
                };
            }
              return Json(AccountInfo, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize(Roles = "CoOwner,UpdateChartofaccoun")]
        public JsonResult UpdateAccount(ChartOfAccount ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            ObjToSave.InsDateTime = DateTime.Now;
            ObjToSave.CurrencyID = ObjToSave.CurrencyID;
            ObjToSave.CompanyID = UserInfo.fCompanyId;
            var userId = User.Identity.GetUserId();
            if (String.IsNullOrEmpty(ObjToSave.EnglishName))
                ObjToSave.EnglishName = ObjToSave.ArabicName;
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
            try
            {
                _unitOfWork.ChartOfAccount.Update(ObjToSave);
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
        [Authorize(Roles = "CoOwner,DeleteChartofaccoun")]
        public JsonResult DeleteAccount(ChartOfAccount ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            ObjToSave.InsDateTime = DateTime.Now;
            //  ObjToSave.i = UserID;
            ObjToSave.CurrencyID = ObjToSave.CurrencyID;
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
            string CheckAccountNumber = _unitOfWork.NativeSql.CheckAccountNumber(UserInfo.fCompanyId, ObjToSave.AccountNumber);
            try
            {
                if ( CheckAccountNumber != "" )
                {

                    Msg.Msg = Resources.Resource.TheAccountContainsTransactions;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }
                else if (_unitOfWork.NativeSql.GetAccountChild(UserInfo.fCompanyId, ObjToSave.AccountNumber).Count() > 0){
                    Msg.Msg = Resources.Resource.TheAccountContainsbranches;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }

                var CustomerObj = new CustomerInformation { };
                CustomerObj.AccountNumber = ObjToSave.AccountNumber;
                CustomerObj.CompanyID = ObjToSave.CompanyID;

                var SupplierObj = new SupplierInformation { };
                SupplierObj.AccountNumber = ObjToSave.AccountNumber;
                SupplierObj.CompanyID = ObjToSave.CompanyID;

                _unitOfWork.ChartOfAccount.Delete(ObjToSave);
                _unitOfWork.CustomerInformation.Delete(CustomerObj);
                _unitOfWork.SupplierInformation.Delete(SupplierObj);
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
        [HttpPost]
        [Authorize(Roles = "CoOwner,AddmainaccountChartofaccount")]
        public JsonResult SaveChildAccount(ChartOfAccount ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            ObjToSave.InsDateTime = DateTime.Now;
            ObjToSave.CurrencyID = ObjToSave.CurrencyID;
            ObjToSave.CompanyID = UserInfo.fCompanyId;
            var userId = User.Identity.GetUserId();
            ObjToSave.CompanyYear = UserInfo.CurrYear;
            ObjToSave.AccountNumber = FunctionUnit.RemoveAccountDash(ObjToSave.AccountNumber);
            ObjToSave.IsFirstLevel = false;
            ObjToSave.AccountNumber = ObjToSave.AccountNumber.PadLeft(ObjToSave.LevelZero.Count(), '0');
            ObjToSave.OrignalAccount = ObjToSave.AccountNumber;
            ObjToSave.AccountNumber = ObjToSave.AccountFather + ObjToSave.AccountNumber;
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
            var AccountInfo = _unitOfWork.ChartOfAccount.GetAccountByID(UserInfo.fCompanyId, ObjToSave.AccountNumber);
            try
            {
                if (AccountInfo != null)
                {

                    Msg.Msg = Resources.Resource.TheAccountNumberIsExist;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }
                _unitOfWork.ChartOfAccount.Add(ObjToSave);
                _unitOfWork.Complete();
                ObjToSave.LevelZero = FunctionUnit.GetLevelChart(CoInfo.AccountChart, ObjToSave.AccountLevel);
                int LevelZeroLength = ObjToSave.LevelZero.Length;
                int AccountNumberLength = _unitOfWork.NativeSql.GetMaxChartNumberChild(CoInfo.CompanyID, ObjToSave.AccountFather).ToString().Length;
                string NewAccountNumber = "";
                if (LevelZeroLength == AccountNumberLength)
                {
                    Msg.LastID = _unitOfWork.NativeSql.GetMaxChartNumberChild(CoInfo.CompanyID, ObjToSave.AccountFather).ToString();
                }
                else
                {
                    LevelZeroLength = LevelZeroLength - AccountNumberLength;
                    for (int i = 1; i <= LevelZeroLength; i++)
                    {
                        NewAccountNumber = NewAccountNumber + "0";
                    }
                    Msg.LastID = NewAccountNumber + _unitOfWork.NativeSql.GetMaxChartNumberChild(CoInfo.CompanyID, ObjToSave.AccountFather).ToString();
                }
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
        [Authorize(Roles = "CoOwner,AddmainaccountChartofaccount")]
        public JsonResult Save(ChartOfAccount ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            ObjToSave.InsDateTime = DateTime.Now;
            ObjToSave.CurrencyID = ObjToSave.CurrencyID;
            ObjToSave.CompanyID = UserInfo.fCompanyId;
            var userId = User.Identity.GetUserId();
            ObjToSave.CompanyYear = UserInfo.CurrYear;
            ObjToSave.AccountFather = FunctionUnit.RemoveAccountDash(ObjToSave.AccountFather);
            ObjToSave.AccountNumber = FunctionUnit.RemoveAccountDash(ObjToSave.AccountNumber);

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
            var AccountInfo = _unitOfWork.ChartOfAccount.GetAccountByID(UserInfo.fCompanyId, ObjToSave.AccountNumber);
            try
            {
                if (AccountInfo != null)
                {

                    Msg.Msg = Resources.Resource.TheAccountNumberIsExist;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }
                _unitOfWork.ChartOfAccount.Add(ObjToSave);
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
        [Authorize(Roles = "CoOwner,AddmainaccountChartofaccount")]
        public JsonResult SaveFatherAccount(ChartOfAccount ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            ObjToSave.InsDateTime = DateTime.Now;
            ObjToSave.CurrencyID = ObjToSave.CurrencyID;
            ObjToSave.CompanyID = UserInfo.fCompanyId;
            var userId = User.Identity.GetUserId();
            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            ObjToSave.CompanyYear = UserInfo.CurrYear;
            ObjToSave.AccountFather = "0";
            ObjToSave.AccountNumber = FunctionUnit.RemoveAccountDash(ObjToSave.AccountNumber);
            ObjToSave.IsFirstLevel = true;
            ObjToSave.AccountLevel = 1;
            ObjToSave.AccountNumber = ObjToSave.AccountNumber.PadLeft(ObjToSave.LevelZero.Count(), '0');
            ObjToSave.OrignalAccount = ObjToSave.AccountNumber;
            ObjToSave.SalesID = ObjToSave.SalesID;
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
            var AccountInfo = _unitOfWork.ChartOfAccount.GetAccountByID(UserInfo.fCompanyId, ObjToSave.AccountNumber);
            try
            {
                if (AccountInfo != null)
                {

                    Msg.Msg = Resources.Resource.TheAccountNumberIsExist;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }
                _unitOfWork.ChartOfAccount.Add(ObjToSave);
                _unitOfWork.Complete();
                ObjToSave.LevelZero = FunctionUnit.GetLevelChart(CoInfo.AccountChart, ObjToSave.AccountLevel);
                int LevelZeroLength = ObjToSave.LevelZero.Length;
                int AccountNumberLength = _unitOfWork.NativeSql.GetMaxChartNumberFather(ObjToSave.CompanyID).ToString().Length;
                string NewAccountNumber = "";
                if (LevelZeroLength == AccountNumberLength)
                {
                    Msg.LastID = _unitOfWork.NativeSql.GetMaxChartNumberFather(ObjToSave.CompanyID).ToString();
                }
                else
                {
                    LevelZeroLength = LevelZeroLength - AccountNumberLength;
                    for (int i = 1; i <= LevelZeroLength; i++)
                    {
                        NewAccountNumber = NewAccountNumber + "0";
                    }
                    Msg.LastID = NewAccountNumber + _unitOfWork.NativeSql.GetMaxChartNumberFather(ObjToSave.CompanyID).ToString();
                }
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
        [Authorize(Roles = "CoOwner,AddmainaccountChartofaccount")]
        public JsonResult SaveFatherAccountClient(ChartOfAccountClientVM ObjVM)
        {
            MsgUnit Msg = new MsgUnit();
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            ChartOfAccount ObjToSave = new ChartOfAccount();
            ObjToSave.AccountFather     = ObjVM.AccountFather;
            ObjToSave.AccountFatherName = ObjVM.AccountFatherName;
            ObjToSave.AccountKind       = ObjVM.AccountKind;
            ObjToSave.AccountLevel      = ObjVM.AccountLevel;
            ObjToSave.AccountNumber     = ObjVM.AccountNumber;
            ObjToSave.AccountTypeID     = ObjVM.AccountTypeID;
            ObjToSave.ArabicName        = ObjVM.ArabicName;
            ObjToSave.EnglishName       = ObjVM.EnglishName;
            ObjToSave.Note              = ObjVM.Note;
            ObjToSave.LevelZero         = ObjVM.LevelZero;
            ObjToSave.CurrencyID        = ObjVM.CurrencyID;
            ObjToSave.InsDateTime = DateTime.Now;
            ObjToSave.CurrencyID = ObjVM.CurrencyID;
            ObjToSave.CompanyID = UserInfo.fCompanyId;
            var userId = User.Identity.GetUserId();
            ObjToSave.CompanyYear = UserInfo.CurrYear;
            ObjToSave.AccountFather = "0";
            ObjToSave.AccountNumber = FunctionUnit.RemoveAccountDash(ObjToSave.AccountNumber);
            ObjToSave.IsFirstLevel = true;
            ObjToSave.AccountLevel = 1;
            ObjToSave.AccountNumber = ObjToSave.AccountNumber.PadLeft(ObjToSave.LevelZero.Count(), '0');
            ObjToSave.OrignalAccount = ObjToSave.AccountNumber;
            if (String.IsNullOrEmpty(ObjToSave.EnglishName))
                ObjToSave.EnglishName = ObjToSave.ArabicName;
            ObjToSave.SalesID = ObjVM.FSalesID;
            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
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
            var AccountInfo = _unitOfWork.ChartOfAccount.GetAccountByID(UserInfo.fCompanyId, ObjToSave.AccountNumber);
            try
            {
                if (AccountInfo != null)
                {

                    Msg.Msg = Resources.Resource.TheAccountNumberIsExist;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }
                _unitOfWork.ChartOfAccount.Add(ObjToSave);
                _unitOfWork.Complete();
                ObjToSave.LevelZero = FunctionUnit.GetLevelChart(CoInfo.AccountChart, ObjToSave.AccountLevel);
                int LevelZeroLength = ObjToSave.LevelZero.Length;
                int AccountNumberLength = _unitOfWork.NativeSql.GetMaxChartNumberFather(ObjToSave.CompanyID).ToString().Length;
                string NewAccountNumber = "";
                if (LevelZeroLength == AccountNumberLength)
                {
                    Msg.LastID = _unitOfWork.NativeSql.GetMaxChartNumberFather(ObjToSave.CompanyID).ToString();
                }
                else
                {
                    LevelZeroLength = LevelZeroLength - AccountNumberLength;
                    for (int i = 1; i <= LevelZeroLength; i++)
                    {
                        NewAccountNumber = NewAccountNumber + "0";
                    }
                    Msg.LastID = NewAccountNumber + _unitOfWork.NativeSql.GetMaxChartNumberFather(ObjToSave.CompanyID).ToString();
                }
                CustomerInformation ObjCust = new CustomerInformation
                {
                    AccountNumber = ObjToSave.AccountNumber,
                    AreaID = ObjVM.AreaID,
                    AuthorizedSignatory = ObjVM.AuthorizedSignatory,
                    BuildingNo = ObjVM.BuildingNo,
                    CityID = ObjVM.CityID,
                    CommercialRecord = ObjVM.CommercialRecord,
                    CompanyID = ObjToSave.CompanyID,
                    DebitLimit = ObjVM.DebitLimit,
                    DebitPeriod = ObjVM.DebitPeriod,
                    DiscountPercentage = ObjVM.DiscountPercentage,
                    Email = ObjVM.Email,
                    FloorNo = ObjVM.FloorNo,
                    KnownTo = ObjVM.KnownTo,
                    Mobile = ObjVM.Mobile,
                    NationalNumberOfTheFacility = ObjVM.NationalNumberOfTheFacility,
                    NextTo = ObjVM.NextTo,
                    PaymnetMethodTypeID = ObjVM.PaymnetMethodTypeID,
                    ProfessionLicence = ObjVM.ProfessionLicence,
                    StreetName = ObjVM.StreetName,
                    TeleFax = ObjVM.TeleFax,
                    Telephone = ObjVM.Telephone,
                    TradeName = ObjVM.TradeName,
                    Website = ObjVM.Website,
                    TaxCaseID = ObjVM.TaxCaseID,
                    UpperLimitForUncollectedCheques = ObjVM.UpperLimitForUncollectedCheques,
                    InsDateTime = DateTime.Now,
                    InsUserID = UserID  
                };

                _unitOfWork.CustomerInformation.Add(ObjCust);
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
        [Authorize(Roles = "CoOwner,AddmainaccountChartofaccount")]
        public JsonResult SaveChildAccountClient(ChartOfAccountClientVM ObjVM)
        {
            MsgUnit Msg = new MsgUnit();
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            ChartOfAccount ObjToSave = new ChartOfAccount();
            ObjToSave.AccountFather = ObjVM.AccountFather;
            ObjToSave.AccountTypeID = ObjVM.AccountTypeID;
            ObjToSave.AccountKind = ObjVM.AccountKind;
            ObjToSave.AccountNumber = ObjVM.AccountNumber;
            ObjToSave.ArabicName = ObjVM.ArabicName;
            ObjToSave.EnglishName = ObjVM.EnglishName;
            ObjToSave.Note = ObjVM.Note;
            ObjToSave.LevelZero = ObjVM.LevelZero;
            ObjToSave.AccountLevel = ObjVM.AccountLevel;
            ObjToSave.AccountFather = ObjVM.AccountFather;
            ObjToSave.AccountFatherName = ObjVM.AccountFatherName;
            ObjToSave.CurrencyID = ObjVM.CurrencyID;
            ObjToSave.InsDateTime = DateTime.Now;
            ObjToSave.CompanyID = UserInfo.fCompanyId;
            var userId = User.Identity.GetUserId();
            ObjToSave.CompanyYear = UserInfo.CurrYear;
            ObjToSave.AccountNumber = FunctionUnit.RemoveAccountDash(ObjToSave.AccountNumber);
            ObjToSave.IsFirstLevel = false;
            ObjToSave.SalesID = ObjVM.FSalesID;
            ObjToSave.AccountNumber = ObjToSave.AccountNumber.PadLeft(ObjToSave.LevelZero.Count(), '0');
            ObjToSave.OrignalAccount = ObjToSave.AccountNumber;
            ObjToSave.AccountNumber = ObjToSave.AccountFather + ObjToSave.AccountNumber;
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
            var AccountInfo = _unitOfWork.ChartOfAccount.GetAccountByID(UserInfo.fCompanyId, ObjToSave.AccountNumber);
            try
            {
                if (AccountInfo != null)
                {

                    Msg.Msg = Resources.Resource.TheAccountNumberIsExist;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }
                _unitOfWork.ChartOfAccount.Add(ObjToSave);
                _unitOfWork.Complete();
                ObjToSave.LevelZero = FunctionUnit.GetLevelChart(CoInfo.AccountChart, ObjToSave.AccountLevel);
                int LevelZeroLength = ObjToSave.LevelZero.Length;
                int AccountNumberLength = _unitOfWork.NativeSql.GetMaxChartNumberChild(CoInfo.CompanyID, ObjToSave.AccountFather).ToString().Length;
                string NewAccountNumber = "";
                if (LevelZeroLength == AccountNumberLength)
                {
                    Msg.LastID = _unitOfWork.NativeSql.GetMaxChartNumberChild(CoInfo.CompanyID, ObjToSave.AccountFather).ToString();
                }
                else
                {
                    LevelZeroLength = LevelZeroLength - AccountNumberLength;
                    for (int i = 1; i <= LevelZeroLength; i++)
                    {
                        NewAccountNumber = NewAccountNumber + "0";
                    }
                    Msg.LastID = NewAccountNumber + _unitOfWork.NativeSql.GetMaxChartNumberChild(CoInfo.CompanyID, ObjToSave.AccountFather).ToString();
                }
                CustomerInformation ObjCust = new CustomerInformation
                {
                    AccountNumber = ObjToSave.AccountNumber,
                    AreaID = ObjVM.AreaID,
                    AuthorizedSignatory = ObjVM.AuthorizedSignatory,
                    BuildingNo = ObjVM.BuildingNo,
                    CityID = ObjVM.CityID,
                    CommercialRecord = ObjVM.CommercialRecord,
                    CompanyID = ObjToSave.CompanyID,
                    DebitLimit = ObjVM.DebitLimit,
                    DebitPeriod = ObjVM.DebitPeriod,
                    DiscountPercentage = ObjVM.DiscountPercentage,
                    Email = ObjVM.Email,
                    FloorNo = ObjVM.FloorNo,
                    KnownTo = ObjVM.KnownTo,
                    Mobile = ObjVM.Mobile,
                    NationalNumberOfTheFacility = ObjVM.NationalNumberOfTheFacility,
                    NextTo = ObjVM.NextTo,
                    PaymnetMethodTypeID = ObjVM.PaymnetMethodTypeID,
                    TaxCaseID = ObjVM.TaxCaseID,
                    UpperLimitForUncollectedCheques = ObjVM.UpperLimitForUncollectedCheques,
                    ProfessionLicence = ObjVM.ProfessionLicence,
                    StreetName = ObjVM.StreetName,
                    TeleFax = ObjVM.TeleFax,
                    Telephone = ObjVM.Telephone,
                    TradeName = ObjVM.TradeName,
                    Website = ObjVM.Website,
                    InsDateTime = DateTime.Now,
                    InsUserID = UserID
                };
                _unitOfWork.CustomerInformation.Add(ObjCust);
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
        [Authorize(Roles = "CoOwner,UpdateChartofaccoun")]
        public JsonResult UpdateAccountClient(ChartOfAccountClientVM ObjVM)
        {
            MsgUnit Msg = new MsgUnit();
            ChartOfAccount ObjToSave = new ChartOfAccount();
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            ObjToSave.AccountTypeID = ObjVM.AccountTypeID;
            ObjToSave.AccountKind = ObjVM.AccountKind;
            ObjToSave.AccountNumber = ObjVM.AccountNumber;
            ObjToSave.ArabicName = ObjVM.ArabicName;
            if (String.IsNullOrEmpty(ObjVM.EnglishName))
                ObjToSave.EnglishName = ObjToSave.ArabicName;
            else
                ObjToSave.EnglishName = ObjVM.EnglishName;
            ObjToSave.Note = ObjVM.Note;
            ObjToSave.StoppedAccount = ObjVM.StoppedAccount;
            ObjToSave.CurrencyID = ObjVM.CurrencyID;
            ObjToSave.InsDateTime = DateTime.Now;          
            ObjToSave.CompanyID = UserInfo.fCompanyId;
            var userId = User.Identity.GetUserId();
            ObjToSave.CompanyYear =UserInfo.CurrYear;
            ObjToSave.SalesID = ObjVM.FSalesID;
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
                _unitOfWork.ChartOfAccount.Update(ObjToSave);
                _unitOfWork.Complete();
                if (ObjToSave.AccountKind == 1)
                {
                    CustomerInformation ObjCust = new CustomerInformation
                    {
                        AccountNumber = ObjToSave.AccountNumber,
                        AreaID = ObjVM.AreaID,
                        AuthorizedSignatory = ObjVM.AuthorizedSignatory,
                        BuildingNo = ObjVM.BuildingNo,
                        CityID = ObjVM.CityID,
                        CommercialRecord = ObjVM.CommercialRecord,
                        CompanyID = ObjToSave.CompanyID,
                        DebitLimit = ObjVM.DebitLimit,
                        DebitPeriod = ObjVM.DebitPeriod,
                        DiscountPercentage = ObjVM.DiscountPercentage,
                        Email = ObjVM.Email,
                        FloorNo = ObjVM.FloorNo,
                        KnownTo = ObjVM.KnownTo,
                        Mobile = ObjVM.Mobile,
                        NationalNumberOfTheFacility = ObjVM.NationalNumberOfTheFacility,
                        NextTo = ObjVM.NextTo,
                        PaymnetMethodTypeID = ObjVM.PaymnetMethodTypeID,
                        ProfessionLicence = ObjVM.ProfessionLicence,
                        StreetName = ObjVM.StreetName,
                        TeleFax = ObjVM.TeleFax,
                        TaxCaseID = ObjVM.TaxCaseID,
                        UpperLimitForUncollectedCheques = ObjVM.UpperLimitForUncollectedCheques,
                        Telephone = ObjVM.Telephone,
                        TradeName = ObjVM.TradeName,
                        Website = ObjVM.Website,
                        InsDateTime = DateTime.Now,
                        InsUserID = UserID

                    };
                    _unitOfWork.CustomerInformation.Add(ObjCust);
                    _unitOfWork.Complete();
                }
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
        [Authorize(Roles = "CoOwner,AddmainaccountChartofaccount")]
        public JsonResult SaveFatherAccountSupplier(ChartOfAccountSupplierVM ObjVM)
        {
            MsgUnit Msg = new MsgUnit();
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            ChartOfAccount ObjToSave = new ChartOfAccount();
            ObjToSave.AccountFather = ObjVM.AccountFather;
            ObjToSave.AccountFatherName = ObjVM.AccountFatherName;
            ObjToSave.AccountKind = ObjVM.AccountKind;
            ObjToSave.AccountLevel = ObjVM.AccountLevel;
            ObjToSave.AccountNumber = ObjVM.AccountNumber;
            ObjToSave.AccountTypeID = ObjVM.AccountTypeID;
            ObjToSave.ArabicName = ObjVM.ArabicName;
            ObjToSave.EnglishName = ObjVM.EnglishName;
            ObjToSave.Note = ObjVM.Note;
            ObjToSave.LevelZero = ObjVM.LevelZero;
            ObjToSave.CurrencyID = ObjVM.CurrencyID;
            ObjToSave.InsDateTime = DateTime.Now;
            ObjToSave.CompanyID = UserInfo.fCompanyId;
            var userId = User.Identity.GetUserId();
            ObjToSave.CompanyYear = UserInfo.CurrYear;
            ObjToSave.AccountFather = "0";
            ObjToSave.AccountNumber = FunctionUnit.RemoveAccountDash(ObjToSave.AccountNumber);
            ObjToSave.IsFirstLevel = true;
            ObjToSave.AccountLevel = 1;
            ObjToSave.AccountNumber = ObjToSave.AccountNumber.PadLeft(ObjToSave.LevelZero.Count(), '0');
            ObjToSave.OrignalAccount = ObjToSave.AccountNumber;
            if (String.IsNullOrEmpty(ObjToSave.EnglishName))
                ObjToSave.EnglishName = ObjToSave.ArabicName;
            ObjToSave.SalesID =  ObjVM.FSalesID;
            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
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
            var AccountInfo = _unitOfWork.ChartOfAccount.GetAccountByID(UserInfo.fCompanyId, ObjToSave.AccountNumber);
            try
            {
                if (AccountInfo != null)
                {

                    Msg.Msg = Resources.Resource.TheAccountNumberIsExist;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }
                _unitOfWork.ChartOfAccount.Add(ObjToSave);
                _unitOfWork.Complete();
                ObjToSave.LevelZero = FunctionUnit.GetLevelChart(CoInfo.AccountChart, ObjToSave.AccountLevel);
                int LevelZeroLength = ObjToSave.LevelZero.Length;
                int AccountNumberLength = _unitOfWork.NativeSql.GetMaxChartNumberFather(ObjToSave.CompanyID).ToString().Length;
                string NewAccountNumber = "";
                if (LevelZeroLength == AccountNumberLength)
                {
                    Msg.LastID = _unitOfWork.NativeSql.GetMaxChartNumberFather(ObjToSave.CompanyID).ToString();
                }
                else
                {
                    LevelZeroLength = LevelZeroLength - AccountNumberLength;
                    for (int i = 1; i <= LevelZeroLength; i++)
                    {
                        NewAccountNumber = NewAccountNumber + "0";
                    }
                    Msg.LastID = NewAccountNumber + _unitOfWork.NativeSql.GetMaxChartNumberFather(ObjToSave.CompanyID).ToString();
                }
                SupplierInformation ObjCust = new SupplierInformation
                {
                    AccountNumber = ObjToSave.AccountNumber,
                              
                    CompanyID = ObjToSave.CompanyID,
                    DebitPeriod = ObjVM.DebitPeriod,
                    Email = ObjVM.Email,
                    Mobile = ObjVM.Mobile,                      
                    PaymnetMethodTypeID = ObjVM.PaymnetMethodTypeID,                
                    TeleFax = ObjVM.TeleFax,
                    Website = ObjVM.Website,
                    InsDateTime = DateTime.Now,
                    InsUserID = UserID,
                    Address=ObjVM.Address,
                    NameOfPersonInCharge= ObjVM.NameOfPersonInCharge,
                    PhoneOfPersonInCharge= ObjVM.PhoneOfPersonInCharge,
                    SupplierCityID= ObjVM.SupplierCityID,
                    SupplierCountryID= ObjVM.SupplierCountryID,
                    SupplierCityBankID = ObjVM.SupplierCityBankID,
                    SupplierCountryBankID = ObjVM.SupplierCountryBankID,
                    SupplierBankID = ObjVM.SupplierBankID,
                    SupplierBranchBankID = ObjVM.SupplierBranchBankID,
                    Telephone = ObjVM.Telephone,
                    SupplierTypeID = ObjVM.SupplierTypeID,
                    BankAccountNumber = ObjVM.BankAccountNumber,
                    BeneficiaryName = ObjVM.BeneficiaryName,
                    IBAN = ObjVM.IBAN,
                    SwiftCode = ObjVM.SwiftCode,
                    BankAdderss = ObjVM.BankAdderss
                };
                _unitOfWork.SupplierInformation.Add(ObjCust);
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
        [Authorize(Roles = "CoOwner,AddmainaccountChartofaccount")]
        public JsonResult SaveChildAccountSupplier(ChartOfAccountSupplierVM ObjVM)
        {
            MsgUnit Msg = new MsgUnit();
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            ChartOfAccount ObjToSave = new ChartOfAccount();
            ObjToSave.AccountFather = ObjVM.AccountFather;
            ObjToSave.AccountTypeID = ObjVM.AccountTypeID;
            ObjToSave.AccountKind = ObjVM.AccountKind;
            ObjToSave.AccountNumber = ObjVM.AccountNumber;
            ObjToSave.ArabicName = ObjVM.ArabicName;
            ObjToSave.EnglishName = ObjVM.EnglishName;
            ObjToSave.Note = ObjVM.Note;
            ObjToSave.LevelZero = ObjVM.LevelZero;
            ObjToSave.AccountLevel = ObjVM.AccountLevel;
            ObjToSave.AccountFather = ObjVM.AccountFather;
            ObjToSave.AccountFatherName = ObjVM.AccountFatherName;
            ObjToSave.CurrencyID = ObjVM.CurrencyID;
            ObjToSave.InsDateTime = DateTime.Now;
            ObjToSave.CompanyID = UserInfo.fCompanyId;
            var userId = User.Identity.GetUserId();
            ObjToSave.CompanyYear = UserInfo.CurrYear;
            ObjToSave.AccountNumber = FunctionUnit.RemoveAccountDash(ObjToSave.AccountNumber);
            ObjToSave.IsFirstLevel = false;
            ObjToSave.SalesID =  ObjVM.FSalesID;

            ObjToSave.AccountNumber = ObjToSave.AccountNumber.PadLeft(ObjToSave.LevelZero.Count(), '0');
            ObjToSave.OrignalAccount = ObjToSave.AccountNumber;
            ObjToSave.AccountNumber = ObjToSave.AccountFather + ObjToSave.AccountNumber;
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
            var AccountInfo = _unitOfWork.ChartOfAccount.GetAccountByID(UserInfo.fCompanyId, ObjToSave.AccountNumber);
            try
            {
                if (AccountInfo != null)
                {

                    Msg.Msg = Resources.Resource.TheAccountNumberIsExist;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }
                _unitOfWork.ChartOfAccount.Add(ObjToSave);
                _unitOfWork.Complete();
                ObjToSave.LevelZero = FunctionUnit.GetLevelChart(CoInfo.AccountChart, ObjToSave.AccountLevel);
                int LevelZeroLength = ObjToSave.LevelZero.Length;
                int AccountNumberLength = _unitOfWork.NativeSql.GetMaxChartNumberChild(CoInfo.CompanyID, ObjToSave.AccountFather).ToString().Length;
                string NewAccountNumber = "";
                if (LevelZeroLength == AccountNumberLength)
                {
                    Msg.LastID = _unitOfWork.NativeSql.GetMaxChartNumberChild(CoInfo.CompanyID, ObjToSave.AccountFather).ToString();
                }
                else
                {
                    LevelZeroLength = LevelZeroLength - AccountNumberLength;
                    for (int i = 1; i <= LevelZeroLength; i++)
                    {
                        NewAccountNumber = NewAccountNumber + "0";
                    }
                    Msg.LastID = NewAccountNumber + _unitOfWork.NativeSql.GetMaxChartNumberChild(CoInfo.CompanyID, ObjToSave.AccountFather).ToString();
                }
                SupplierInformation ObjCust = new SupplierInformation
                {
                    AccountNumber = ObjToSave.AccountNumber,
                    CompanyID = ObjToSave.CompanyID,
                    DebitPeriod = ObjVM.DebitPeriod,
                    Email = ObjVM.Email,
                    Mobile = ObjVM.Mobile,
                    PaymnetMethodTypeID = ObjVM.PaymnetMethodTypeID,
                    TeleFax = ObjVM.TeleFax,
                    Website = ObjVM.Website,
                    InsDateTime = DateTime.Now,
                    InsUserID = UserID,
                    Address = ObjVM.Address,
                    NameOfPersonInCharge = ObjVM.NameOfPersonInCharge,
                    PhoneOfPersonInCharge = ObjVM.PhoneOfPersonInCharge,
                    SupplierCityID = ObjVM.SupplierCityID,
                    SupplierCountryID = ObjVM.SupplierCountryID,
                    SupplierCityBankID = ObjVM.SupplierCityBankID,
                    SupplierCountryBankID = ObjVM.SupplierCountryBankID,
                    SupplierBankID = ObjVM.SupplierBankID,
                    SupplierBranchBankID = ObjVM.SupplierBranchBankID,
                    Telephone = ObjVM.Telephone,
                    SupplierTypeID = ObjVM.SupplierTypeID,
                    BankAccountNumber = ObjVM.BankAccountNumber,
                    BeneficiaryName = ObjVM.BeneficiaryName,
                    IBAN = ObjVM.IBAN,
                    SwiftCode = ObjVM.SwiftCode,
                    BankAdderss = ObjVM.BankAdderss
                };
                _unitOfWork.SupplierInformation.Add(ObjCust);
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
        [Authorize(Roles = "CoOwner,UpdateChartofaccoun")]
        public JsonResult UpdateAccountSupplier(ChartOfAccountSupplierVM ObjVM)
        {
            MsgUnit Msg = new MsgUnit();
            ChartOfAccount ObjToSave = new ChartOfAccount();
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            ObjToSave.AccountTypeID = ObjVM.AccountTypeID;
            ObjToSave.AccountKind = ObjVM.AccountKind;
            ObjToSave.AccountNumber = ObjVM.AccountNumber;
            ObjToSave.ArabicName = ObjVM.ArabicName;
            if (String.IsNullOrEmpty(ObjVM.EnglishName))
                ObjToSave.EnglishName = ObjToSave.ArabicName;
            else
                ObjToSave.EnglishName = ObjVM.EnglishName;
            ObjToSave.Note = ObjVM.Note;
            ObjToSave.StoppedAccount = ObjVM.StoppedAccount;
            ObjToSave.CurrencyID = ObjVM.CurrencyID;
            ObjToSave.InsDateTime = DateTime.Now;
            ObjToSave.CompanyID = UserInfo.fCompanyId;
            var userId = User.Identity.GetUserId();
            ObjToSave.CompanyYear = UserInfo.CurrYear;
            ObjToSave.SalesID =  ObjVM.FSalesID;
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
                _unitOfWork.ChartOfAccount.Update(ObjToSave);
                _unitOfWork.Complete();
                  if (ObjToSave.AccountKind == 2)
                {
                    SupplierInformation ObjCust = new SupplierInformation
                    {
                        AccountNumber = ObjToSave.AccountNumber,
                        CompanyID = ObjToSave.CompanyID,
                        DebitPeriod = ObjVM.DebitPeriod,
                        Email = ObjVM.Email,
                        Mobile = ObjVM.Mobile,
                        PaymnetMethodTypeID = ObjVM.PaymnetMethodTypeID,
                        TeleFax = ObjVM.TeleFax,
                        Website = ObjVM.Website,
                        InsDateTime = DateTime.Now,
                        InsUserID = UserID,
                        Address = ObjVM.Address,
                        NameOfPersonInCharge = ObjVM.NameOfPersonInCharge,
                        PhoneOfPersonInCharge = ObjVM.PhoneOfPersonInCharge,
                        SupplierCityID = ObjVM.SupplierCityID,
                        SupplierCountryID = ObjVM.SupplierCountryID,
                        SupplierCityBankID = ObjVM.SupplierCityBankID,
                        SupplierCountryBankID = ObjVM.SupplierCountryBankID,
                        SupplierBankID = ObjVM.SupplierBankID,
                        SupplierBranchBankID = ObjVM.SupplierBranchBankID,
                        Telephone = ObjVM.Telephone,
                        SupplierTypeID = ObjVM.SupplierTypeID,
                        BankAccountNumber = ObjVM.BankAccountNumber,
                        BeneficiaryName = ObjVM.BeneficiaryName,
                        IBAN = ObjVM.IBAN,
                        SwiftCode = ObjVM.SwiftCode,
                        BankAdderss = ObjVM.BankAdderss
                    };
                    _unitOfWork.SupplierInformation.Add(ObjCust);
                    _unitOfWork.Complete();
                }
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
        [Authorize(Roles = "CoOwner,AddmainaccountChartofaccount")]
        public JsonResult SaveFatherAccountAccreditation(ChartOfAccountAccreditationVM ObjVM)
        {
            MsgUnit Msg = new MsgUnit();
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            ChartOfAccount ObjToSave = new ChartOfAccount();
            ObjToSave.AccountFather = ObjVM.AccountFather;
            ObjToSave.AccountFatherName = ObjVM.AccountFatherName;
            ObjToSave.AccountKind = ObjVM.AccountKind;
            ObjToSave.AccountLevel = ObjVM.AccountLevel;
            ObjToSave.AccountNumber = ObjVM.AccountNumber;
            ObjToSave.AccountTypeID = ObjVM.AccountTypeID;
            ObjToSave.ArabicName = ObjVM.ArabicName;
            ObjToSave.EnglishName = ObjVM.EnglishName;
            ObjToSave.Note = ObjVM.Note;
            ObjToSave.LevelZero = ObjVM.LevelZero;
            ObjToSave.CurrencyID = ObjVM.CurrencyID;
            ObjToSave.InsDateTime = DateTime.Now;
            ObjToSave.CompanyID = UserInfo.fCompanyId;
            var userId = User.Identity.GetUserId();
            ObjToSave.CompanyYear = UserInfo.CurrYear;
            ObjToSave.AccountFather = "0";
            ObjToSave.AccountNumber = FunctionUnit.RemoveAccountDash(ObjToSave.AccountNumber);
            ObjToSave.IsFirstLevel = true;
            ObjToSave.AccountLevel = 1;
            ObjToSave.AccountNumber = ObjToSave.AccountNumber.PadLeft(ObjToSave.LevelZero.Count(), '0');
            ObjToSave.OrignalAccount = ObjToSave.AccountNumber;
            if (String.IsNullOrEmpty(ObjToSave.EnglishName))
                ObjToSave.EnglishName = ObjToSave.ArabicName;
            ObjToSave.SalesID =  ObjVM.FSalesID;
            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
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
            var AccountInfo = _unitOfWork.ChartOfAccount.GetAccountByID(UserInfo.fCompanyId, ObjToSave.AccountNumber);
            try
            {
                if (AccountInfo != null)
                {

                    Msg.Msg = Resources.Resource.TheAccountNumberIsExist;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }
                _unitOfWork.ChartOfAccount.Add(ObjToSave);
                _unitOfWork.Complete();
                ObjToSave.LevelZero = FunctionUnit.GetLevelChart(CoInfo.AccountChart, ObjToSave.AccountLevel);
                int LevelZeroLength = ObjToSave.LevelZero.Length;
                int AccountNumberLength = _unitOfWork.NativeSql.GetMaxChartNumberFather(ObjToSave.CompanyID).ToString().Length;
                string NewAccountNumber = "";
                if (LevelZeroLength == AccountNumberLength)
                {
                    Msg.LastID = _unitOfWork.NativeSql.GetMaxChartNumberFather(ObjToSave.CompanyID).ToString();
                }
                else
                {
                    LevelZeroLength = LevelZeroLength - AccountNumberLength;
                    for (int i = 1; i <= LevelZeroLength; i++)
                    {
                        NewAccountNumber = NewAccountNumber + "0";
                    }
                    Msg.LastID = NewAccountNumber + _unitOfWork.NativeSql.GetMaxChartNumberFather(ObjToSave.CompanyID).ToString();
                }
                AccreditationInformation ObjCust = new AccreditationInformation
                {
                    AccountNumber = ObjToSave.AccountNumber,
                    CompanyID = ObjToSave.CompanyID,
                    AccreditationExpiresPlace= ObjVM.AccreditationExpiresPlace,
                    ValidityOfAccreditation= ObjVM.ValidityOfAccreditation,
                    TypeOfGoods= ObjVM.TypeOfGoods,
                    ShippingPort= ObjVM.ShippingPort,
                    ArrivePort= ObjVM.ArrivePort,
                    Bank= ObjVM.Bank,
                    BankInsurance= ObjVM.BankInsurance,
                    BeneficiaryOfAccreditation= ObjVM.BeneficiaryOfAccreditation,
                    CommissionsPaid= ObjVM.CommissionsPaid,
                    CreditForeignCurrency= ObjVM.CreditForeignCurrency,
                    CreditInlocalCurrency= ObjVM.CreditInlocalCurrency,
                    CurrencyType= ObjVM.CurrencyType,
                    DateOfIssuanceOfCredit= ObjVM.DateOfIssuanceOfCredit,
                    ExchangeRate= ObjVM.ExchangeRate,
                    LastShipmentDate= ObjVM.LastShipmentDate,
                    MultipleShippingMethods= ObjVM.MultipleShippingMethods,
                    PartialCharging= ObjVM.PartialCharging,
                    PaymentTerms= ObjVM.PaymentTerms
                };
                _unitOfWork.AccreditationInformation.Add(ObjCust);
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
        [Authorize(Roles = "CoOwner,AddmainaccountChartofaccount")]
        public JsonResult SaveChildAccountAccreditation(ChartOfAccountAccreditationVM ObjVM)
        {
            MsgUnit Msg = new MsgUnit();
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            ChartOfAccount ObjToSave = new ChartOfAccount();
            ObjToSave.AccountFather = ObjVM.AccountFather;
            ObjToSave.AccountTypeID = ObjVM.AccountTypeID;
            ObjToSave.AccountKind = ObjVM.AccountKind;
            ObjToSave.AccountNumber = ObjVM.AccountNumber;
            ObjToSave.ArabicName = ObjVM.ArabicName;
            ObjToSave.EnglishName = ObjVM.EnglishName;
            ObjToSave.Note = ObjVM.Note;
            ObjToSave.LevelZero = ObjVM.LevelZero;
            ObjToSave.AccountLevel = ObjVM.AccountLevel;
            ObjToSave.AccountFather = ObjVM.AccountFather;
            ObjToSave.AccountFatherName = ObjVM.AccountFatherName;
            ObjToSave.CurrencyID = ObjVM.CurrencyID;
            ObjToSave.InsDateTime = DateTime.Now;
            ObjToSave.CompanyID = UserInfo.fCompanyId;
            var userId = User.Identity.GetUserId();
            ObjToSave.CompanyYear = UserInfo.CurrYear;
            ObjToSave.AccountNumber = FunctionUnit.RemoveAccountDash(ObjToSave.AccountNumber);
            ObjToSave.IsFirstLevel = false;
            ObjToSave.SalesID =  ObjVM.FSalesID;
            ObjToSave.AccountNumber = ObjToSave.AccountNumber.PadLeft(ObjToSave.LevelZero.Count(), '0');
            ObjToSave.OrignalAccount = ObjToSave.AccountNumber;
            ObjToSave.AccountNumber = ObjToSave.AccountFather + ObjToSave.AccountNumber;
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
            var AccountInfo = _unitOfWork.ChartOfAccount.GetAccountByID(UserInfo.fCompanyId, ObjToSave.AccountNumber);
            try
            {
                if (AccountInfo != null)
                {

                    Msg.Msg = Resources.Resource.TheAccountNumberIsExist;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }
                _unitOfWork.ChartOfAccount.Add(ObjToSave);
                _unitOfWork.Complete();
                ObjToSave.LevelZero = FunctionUnit.GetLevelChart(CoInfo.AccountChart, ObjToSave.AccountLevel);
                int LevelZeroLength = ObjToSave.LevelZero.Length;
                int AccountNumberLength = _unitOfWork.NativeSql.GetMaxChartNumberChild(CoInfo.CompanyID, ObjToSave.AccountFather).ToString().Length;
                string NewAccountNumber = "";
                if (LevelZeroLength == AccountNumberLength)
                {
                    Msg.LastID = _unitOfWork.NativeSql.GetMaxChartNumberChild(CoInfo.CompanyID, ObjToSave.AccountFather).ToString();
                }
                else
                {
                    LevelZeroLength = LevelZeroLength - AccountNumberLength;
                    for (int i = 1; i <= LevelZeroLength; i++)
                    {
                        NewAccountNumber = NewAccountNumber + "0";
                    }
                    Msg.LastID = NewAccountNumber + _unitOfWork.NativeSql.GetMaxChartNumberChild(CoInfo.CompanyID, ObjToSave.AccountFather).ToString();
                }
                AccreditationInformation ObjCust = new AccreditationInformation
                {
                    AccountNumber = ObjToSave.AccountNumber,
                    CompanyID = ObjToSave.CompanyID,
                    AccreditationExpiresPlace = ObjVM.AccreditationExpiresPlace,
                    ValidityOfAccreditation = ObjVM.ValidityOfAccreditation,
                    TypeOfGoods = ObjVM.TypeOfGoods,
                    ShippingPort = ObjVM.ShippingPort,
                    ArrivePort = ObjVM.ArrivePort,
                    Bank = ObjVM.Bank,
                    BankInsurance = ObjVM.BankInsurance,
                    BeneficiaryOfAccreditation = ObjVM.BeneficiaryOfAccreditation,
                    CommissionsPaid = ObjVM.CommissionsPaid,
                    CreditForeignCurrency = ObjVM.CreditForeignCurrency,
                    CreditInlocalCurrency = ObjVM.CreditInlocalCurrency,
                    CurrencyType = ObjVM.CurrencyType,
                    DateOfIssuanceOfCredit = ObjVM.DateOfIssuanceOfCredit,
                    ExchangeRate = ObjVM.ExchangeRate,
                    LastShipmentDate = ObjVM.LastShipmentDate,
                    MultipleShippingMethods = ObjVM.MultipleShippingMethods,
                    PartialCharging = ObjVM.PartialCharging,
                    PaymentTerms = ObjVM.PaymentTerms
                };
                _unitOfWork.AccreditationInformation.Add(ObjCust);
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
        [Authorize(Roles = "CoOwner,UpdateChartofaccoun")]
        public JsonResult UpdateAccountAccreditation(ChartOfAccountAccreditationVM ObjVM)
        {
            MsgUnit Msg = new MsgUnit();
            ChartOfAccount ObjToSave = new ChartOfAccount();
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            ObjToSave.AccountTypeID = ObjVM.AccountTypeID;
            ObjToSave.AccountKind = ObjVM.AccountKind;
            ObjToSave.AccountNumber = ObjVM.AccountNumber;
            ObjToSave.ArabicName = ObjVM.ArabicName;
            if (String.IsNullOrEmpty(ObjVM.EnglishName))
                ObjToSave.EnglishName = ObjToSave.ArabicName;
            else
                ObjToSave.EnglishName = ObjVM.EnglishName;
            ObjToSave.Note = ObjVM.Note;
            ObjToSave.StoppedAccount = ObjVM.StoppedAccount;
            ObjToSave.CurrencyID = ObjVM.CurrencyID;
            ObjToSave.InsDateTime = DateTime.Now;
            ObjToSave.CompanyID = UserInfo.fCompanyId;
            var userId = User.Identity.GetUserId();
            ObjToSave.CompanyYear = UserInfo.CurrYear;
            ObjToSave.SalesID =  ObjVM.FSalesID;
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
                _unitOfWork.ChartOfAccount.Update(ObjToSave);
                _unitOfWork.Complete();
                if (ObjToSave.AccountKind == 3)
                {
                    AccreditationInformation ObjCust = new AccreditationInformation
                    {
                        AccountNumber = ObjToSave.AccountNumber,
                        CompanyID = ObjToSave.CompanyID,
                        AccreditationExpiresPlace = ObjVM.AccreditationExpiresPlace,
                        ValidityOfAccreditation = ObjVM.ValidityOfAccreditation,
                        TypeOfGoods = ObjVM.TypeOfGoods,
                        ShippingPort = ObjVM.ShippingPort,
                        ArrivePort = ObjVM.ArrivePort,
                        Bank = ObjVM.Bank,
                        BankInsurance = ObjVM.BankInsurance,
                        BeneficiaryOfAccreditation = ObjVM.BeneficiaryOfAccreditation,
                        CommissionsPaid = ObjVM.CommissionsPaid,
                        CreditForeignCurrency = ObjVM.CreditForeignCurrency,
                        CreditInlocalCurrency = ObjVM.CreditInlocalCurrency,
                        CurrencyType = ObjVM.CurrencyType,
                        DateOfIssuanceOfCredit = ObjVM.DateOfIssuanceOfCredit,
                        ExchangeRate = ObjVM.ExchangeRate,
                        LastShipmentDate = ObjVM.LastShipmentDate,
                        MultipleShippingMethods = ObjVM.MultipleShippingMethods,
                        PartialCharging = ObjVM.PartialCharging,
                        PaymentTerms = ObjVM.PaymentTerms
                    };
                    _unitOfWork.AccreditationInformation.Add(ObjCust);
                    _unitOfWork.Complete();
                }
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
        Acc.Persistence.ApplicationDbContext db = new Acc.Persistence.ApplicationDbContext();
        [ValidateInput(false)]
        public ActionResult TreeListPartial()
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            var model = _unitOfWork.ChartOfAccount.GetAllChartOfAccount(UserInfo.fCompanyId);
            
            return PartialView("_TreeListPartial", model.ToList());
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult TreeListPartialLoad()
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            var model = _unitOfWork.ChartOfAccount.GetAllChartOfAccount(UserInfo.fCompanyId);
            return PartialView("_TreeListPartial", model.ToList());
        }
        [HttpGet]
        public JsonResult GetSaleManID(string id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            if (id != "")
            {
                var SaleManObj = _unitOfWork.ChartOfAccount.GetAccountByID(UserInfo.fCompanyId, id);

                if (SaleManObj == null)
                {
                    SaleManObj.SalesID = 0;
                };

                return Json(SaleManObj.SalesID, JsonRequestBehavior.AllowGet);
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult CheckAccountNumber(string id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);

            var AccountInfo = _unitOfWork.ChartOfAccount.GetAccountByID(UserInfo.fCompanyId, id);
            if (AccountInfo == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(AccountInfo.AccountNumber, JsonRequestBehavior.AllowGet);
            }
            
        }

        [HttpGet]
        public JsonResult GetAccountInfo(string id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);

            var AccountInfo = _unitOfWork.ChartOfAccount.GetAccountByID(UserInfo.fCompanyId, id);
            if (AccountInfo == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(AccountInfo, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public JsonResult CheckAccountInfo(string id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);

            var AccountInfo = _unitOfWork.NativeSql.CheckTransActionAccount(UserInfo.fCompanyId, id);
            if (AccountInfo == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(AccountInfo, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public JsonResult CheckFatherAccountInfo(string id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);

            var AccountInfo = _unitOfWork.NativeSql.CheckFatherTransActionAccount(UserInfo.fCompanyId, id);
            if (AccountInfo == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(AccountInfo, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public JsonResult GetAreaIDByCityID(int id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllArea = _unitOfWork.CustomerArea.GetAllCustomerAreaByCityID(UserInfo.fCompanyId,id);
                if (AllArea == null)
                {
                    return Json(new List<CustomerArea>(), JsonRequestBehavior.AllowGet);
                }

                return Json(AllArea, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<CustomerArea>(), JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public JsonResult GetCityIDByCountryID(int id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllCountry = _unitOfWork.SupplierCity.GetAllSupplierCityByCountryID(UserInfo.fCompanyId, id);
                if (AllCountry == null)
                {
                    return Json(new List<SupplierCity>(), JsonRequestBehavior.AllowGet);
                }

                return Json(AllCountry, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<SupplierCity>(), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult GetBankCityIDByBankCountryID(int id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllCountry = _unitOfWork.SupplierCityBank.GetAllSupplierCityBankByCountryBankID(UserInfo.fCompanyId, id);
                if (AllCountry == null)
                {
                    return Json(new List<SupplierCityBank>(), JsonRequestBehavior.AllowGet);
                }

                return Json(AllCountry, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<SupplierCityBank>(), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult GetBranchBankIDByBankID(int id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllBranchBank = _unitOfWork.SupplierBranchBank.GetAllSupplierBranchBankByBankID(UserInfo.fCompanyId, id);
                if (AllBranchBank == null)
                {
                    return Json(new List<SupplierBranchBank>(), JsonRequestBehavior.AllowGet);
                }

                return Json(AllBranchBank, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<SupplierBranchBank>(), JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ChartDash()
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var CarthDashObj = new ChartDashVM();
            CarthDashObj.WorkWithCostCenter = Company.WorkWithCostCenter;
            return View(CarthDashObj);
        }
    }
}