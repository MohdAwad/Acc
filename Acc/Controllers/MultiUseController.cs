﻿using Acc.Models;
using Acc.Persistence;
using Acc.Repositories;
using Acc.ViewModels;
using DevExpress.DataProcessing;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Acc.Controllers
{
    [Authorize]
    public class MultiUseController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public MultiUseController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SearchEmployeeHR(string ID)
        {
            ViewBag.ID = ID;
            return PartialView();
        }
        public JsonResult GetAllActiveEmp(int id)
        {
            try
            {
                string connetionString;
                System.Data.SqlClient.SqlConnection cnn;
                DataTable dt = new DataTable();

                connetionString = @"Data Source=174.138.183.2;Initial Catalog=Brand_Center_HR;User Id=Brand_Center_HR;Password=Matrix__90;";
                string Sql = "select EmployeeId,EmployeeFullName,EmployeeEmail from Employees  ";
                cnn = new SqlConnection(connetionString);

                cnn.Open();
                SqlCommand cmd = new SqlCommand(Sql, cnn);
              
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                cnn.Close();

                List<EmpVM> Empist = new List<EmpVM>();
                Empist = (from DataRow dr in dt.Rows
                               select new EmpVM()
                               {
                                   EmployeeId = dr["EmployeeId"].ToString(),
                                   EmployeeFullName = dr["EmployeeFullName"].ToString() 
                                  
                                   
                               }).ToList();
               
                return Json(Empist, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string err = ex.Message;



                return Json(new List<EmpVM>(), JsonRequestBehavior.AllowGet);

            }


        }
        public JsonResult GetAcountFather(int id)
        {
            try
            {

                string UserID = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(UserID);


                var model = _unitOfWork.NativeSql.GetFatherAccount(UserInfo.fCompanyId);



                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string err = ex.Message;



                return Json(new List<MutilUseSearchVM>(), JsonRequestBehavior.AllowGet);

            }


        }
        public ActionResult SearchFatherAccount(string ID)
        {
            ViewBag.ID = ID;
            return PartialView();
        }
        public JsonResult GetSalesGainsAcountAcc(int id)
        {
            try
            {

                string UserID = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(UserID);


                var model = _unitOfWork.NativeSql.GetAllTransActionAccountSalesGain(UserInfo.fCompanyId);



                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string err = ex.Message;



                return Json(new List<MutilUseSearchVM>(), JsonRequestBehavior.AllowGet);

            }


        }
        public JsonResult GetPurchaseAcountAcc(int id)
        {
            try
            {

                string UserID = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(UserID);


                var model = _unitOfWork.NativeSql.GetAllTransActionAccountPurchase(UserInfo.fCompanyId);



                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string err = ex.Message;



                return Json(new List<MutilUseSearchVM>(), JsonRequestBehavior.AllowGet);

            }


        }
        public JsonResult GetAcountAcc(int id)
        {
            try
            {

                string UserID = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(UserID);


                var model = _unitOfWork.NativeSql.GetTransActionAccount(UserInfo.fCompanyId);



                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string err = ex.Message;



                return Json(new List<MutilUseSearchVM>(), JsonRequestBehavior.AllowGet);

            }


        }
        public JsonResult GetAllAcountAcc(int id)
        {
            try
            {

                string UserID = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(UserID);


                var model = _unitOfWork.NativeSql.GetAllTransActionAccount(UserInfo.fCompanyId);



                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string err = ex.Message;



                return Json(new List<MutilUseSearchVM>(), JsonRequestBehavior.AllowGet);

            }


        }
        public JsonResult GetCustomerAccount(int id)
        {
            try
            {
                string UserID = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
                var model = _unitOfWork.NativeSql.GetCustomerAccount(UserInfo.fCompanyId);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return Json(new List<MutilUseSearchVM>(), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetCustomerFatherAccount(int id)
        {
            try
            {
                string UserID = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
                var model = _unitOfWork.NativeSql.GetCustomerFatherAccount(UserInfo.fCompanyId);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return Json(new List<MutilUseSearchVM>(), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetClientAccount(int id)
        {
            try
            {

                string UserID = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(UserID);


                var model = _unitOfWork.NativeSql.GetClientAccount(UserInfo.fCompanyId);



                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string err = ex.Message;



                return Json(new List<MutilUseSearchVM>(), JsonRequestBehavior.AllowGet);

            }


        }
        public JsonResult GetClientFatherAccount(int id)
        {
            try
            {

                string UserID = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(UserID);


                var model = _unitOfWork.NativeSql.GetClientFatherAccount(UserInfo.fCompanyId);



                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string err = ex.Message;



                return Json(new List<MutilUseSearchVM>(), JsonRequestBehavior.AllowGet);

            }


        }
        public JsonResult GetCustomerClientAccount(int id)
        {
            try
            {

                string UserID = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(UserID);


                var model = _unitOfWork.NativeSql.GetCustomerAndClientAccount(UserInfo.fCompanyId);



                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string err = ex.Message;



                return Json(new List<MutilUseSearchVM>(), JsonRequestBehavior.AllowGet);

            }


        }
        public JsonResult GetAllServices(int id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllService = _unitOfWork.NativeSql.GetAllService(UserInfo.fCompanyId);
                if (AllService == null)
                {
                    return Json(new List<ServiceVM>(), JsonRequestBehavior.AllowGet);
                }

                return Json(AllService, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<ServiceVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult GetAllAsset(int id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllService = _unitOfWork.Asset.GetAllAssetNative(UserInfo.fCompanyId,0);
                if (AllService == null)
                {
                    return Json(new List<AssetVM>(), JsonRequestBehavior.AllowGet);
                }

                return Json(AllService, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<AssetVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult SearchAsset(string ID)
        {
            ViewBag.ID = ID;
            return PartialView();
        }
        public ActionResult SearchSalesGainsAcc(string ID)
        {
            ViewBag.ID = ID;
            return PartialView();
        }
        public ActionResult SearchPurchasesAcc(string ID)
        {
            ViewBag.ID = ID;
            return PartialView();
        }
        public ActionResult SearchAcc(string ID)
        {
            ViewBag.ID = ID;
            return PartialView();
        }
        public ActionResult SearchService(string ID)
        {
            ViewBag.ID = ID;
            return PartialView();
        }
        public ActionResult SearchAllAcc(string ID)
        {
            ViewBag.ID = ID;
            return PartialView();
        }
        public ActionResult SearchCustomerAccount(string ID)
        {
            ViewBag.ID = ID;
            return PartialView();
        }
        public ActionResult SearchCustomerFatherAccount(string ID)
        {
            ViewBag.ID = ID;
            return PartialView();
        }
        public ActionResult SearchClientAccount(string ID)
        {
            ViewBag.ID = ID;
            return PartialView();
        }
        public ActionResult SearchClientFatherAccount(string ID)
        {
            ViewBag.ID = ID;
            return PartialView();
        }
        public ActionResult SearchCustomerAndClientAccount(string ID)
        {
            ViewBag.ID = ID;
            return PartialView();
        }
        public ActionResult AccLookup(string ID)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            ViewBag["ID"] = ID;
            //  var model = _unitOfWork.ChartOfAccount.GetAllChartOfAccount(UserInfo.fCompanyId); ;
            var model = _unitOfWork.NativeSql.GetTransActionAccount(UserInfo.fCompanyId);
            return PartialView(model);
        }
        public JsonResult GetCostCenter(int id)
        {
            try
            {
                string UserID = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
                var model = _unitOfWork.NativeSql.GetTransActionCostCenter(UserInfo.fCompanyId);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return Json(new List<MutilUseSearchVM>(), JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult SearchCostCenter(string ID)
        {
            ViewBag.ID = ID;
            return PartialView();
        }
        public JsonResult GetDefinitionBank(int id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                string UserID = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
                var model = _unitOfWork.NativeSql.GetAllBankAccountNumber(UserInfo.fCompanyId);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return Json(new List<DefinitionBank>(), JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult SearchBankAccountNumber(string ID)
        {
            ViewBag.ID = ID;
            return PartialView();
        }
        public JsonResult GetDrawnBank(int id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                string UserID = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
                var model = _unitOfWork.NativeSql.GetAllDrawnBank(UserInfo.fCompanyId);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return Json(new List<DrawnBank>(), JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult SearchDrawnBank(string ID)
        {
            ViewBag.ID = ID;
            return PartialView();
        }
        public JsonResult GetDefinitionFund(int id)
        {
            try
            {
                string UserID = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
                var DefinitionObj = _unitOfWork.DefinitionOtherAccount.GetDefinitionOtherAccountByID(UserInfo.fCompanyId);
                var CashFund = DefinitionObj.CashFundsAccountNumber + "%";
                var model = _unitOfWork.NativeSql.GetAllDefinitionFund(UserInfo.fCompanyId, CashFund);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return Json(new List<MutilUseSearchVM>(), JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult SearchFundAccountNumber(string ID)
        {
            ViewBag.ID = ID;
            return PartialView();
        }
        public JsonResult GetDefinitionExpense(int id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                string UserID = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
                var DefinitionObj = _unitOfWork.DefinitionOtherAccount.GetDefinitionOtherAccountByID(UserInfo.fCompanyId);
                var Expense = DefinitionObj.ExpensesAccountNumber + "%";
                var model = _unitOfWork.NativeSql.GetAllDefinitionExpense(UserInfo.fCompanyId, Expense);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return Json(new List<MutilUseSearchVM>(), JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult SearchExpenseAccountNumber(string ID)
        {
            ViewBag.ID = ID;
            return PartialView();
        }
        public JsonResult GetDefinitionPaidExpense(int id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                string UserID = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
                var DefinitionObj = _unitOfWork.DefinitionOtherAccount.GetDefinitionOtherAccountByID(UserInfo.fCompanyId);
                var PaidExpense = DefinitionObj.PaidExpensesAccountNumber + "%";
                var model = _unitOfWork.NativeSql.GetAllDefinitionPaidExpense(UserInfo.fCompanyId, PaidExpense);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return Json(new List<MutilUseSearchVM>(), JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult SearchPaidExpenseAccountNumber(string ID)
        {
            ViewBag.ID = ID;
            return PartialView();
        }
        public JsonResult GetDefinitionRevenue(int id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                string UserID = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
                var DefinitionObj = _unitOfWork.DefinitionOtherAccount.GetDefinitionOtherAccountByID(UserInfo.fCompanyId);
                var Revenues = DefinitionObj.RevenuesAccountNumber + "%";
                var model =  _unitOfWork.NativeSql.GetAllDefinitionRevenue(UserInfo.fCompanyId, Revenues);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return Json(new List<MutilUseSearchVM>(), JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult SearchRevenueAccountNumber(string ID)
        {
            ViewBag.ID = ID;
            return PartialView();
        }
        public JsonResult GetDefinitionRevenueReceived(int id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                string UserID = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
                var DefinitionObj = _unitOfWork.DefinitionOtherAccount.GetDefinitionOtherAccountByID(UserInfo.fCompanyId);
                var RevenuesReceived = DefinitionObj.RevenuesReceivedAccountNumber + "%";
                var model =  _unitOfWork.NativeSql.GetAllDefinitionRevenueReceived(UserInfo.fCompanyId, RevenuesReceived);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return Json(new List<MutilUseSearchVM>(), JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult SearchRevenueReceivedAccountNumber(string ID)
        {
            ViewBag.ID = ID;
            return PartialView();
        }
        public JsonResult GetDefinitionTax(int id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                string UserID = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
                var model = _unitOfWork.NativeSql.GetAllDefinitionTax(UserInfo.fCompanyId);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                var model = "";
                return Json(new List<MutilUseSearchVM>(), JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult SearcTaxAccountNumber(string ID)
        {
            ViewBag.ID = ID;
            return PartialView();
        }
        public JsonResult GetDefinitionChequeFund(int id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                string UserID = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
                var DefinitionObj = _unitOfWork.DefinitionOtherAccount.GetDefinitionOtherAccountByID(UserInfo.fCompanyId);
                var ChequeFund = DefinitionObj.ChequeFundAccountNumber + "%";
                var model = _unitOfWork.NativeSql.GetAllDefinitionChequeFund(UserInfo.fCompanyId, ChequeFund);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return Json(new List<MutilUseSearchVM>(), JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult SearchChequeFundAccountNumber(string ID)
        {
            ViewBag.ID = ID;
            return PartialView();
        }
        public JsonResult GetFixAccountInfo(string id)
        {
            try
            {
                string UserID = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
                var ObjGet = _unitOfWork.DefinitionOtherAccount.GetDefinitionOtherAccountByID(UserInfo.fCompanyId);
                FixAccountInfoVM Obj = new FixAccountInfoVM
                {
                    AccountNo = "",
                    AccountName = ""
                };
                if (ObjGet != null)
                {
                    if (id == "SalesTaxAccountNumber")
                    {
                        Obj.AccountNo = ObjGet.SalesTaxAccountNumber;
                        Obj.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.SalesTaxAccountNumber);
                    }
                }
                return Json(Obj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return Json(new List<MutilUseSearchVM>(), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetAllDefinitionAsset(int id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllDefinitionAsset = _unitOfWork.NativeSql.GetAllDefinitionAsset(UserInfo.fCompanyId);
                if (AllDefinitionAsset == null)
                {
                    return Json(new List<DefinitionAssetSiteVM>(), JsonRequestBehavior.AllowGet);
                }
                return Json(AllDefinitionAsset, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<DefinitionAssetSiteVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult SearchDefinitionAsset(string ID)
        {
            ViewBag.ID = ID;
            return PartialView();
        }
        public JsonResult GetAllSt_ItemGroupH(int id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllSt_ItemGroup = _unitOfWork.NativeSql.GetAllSt_ItemGroupH(UserInfo.fCompanyId);
                if (AllSt_ItemGroup == null)
                {
                    return Json(new List<St_WarehouseSearchHVM>(), JsonRequestBehavior.AllowGet);
                }
                return Json(AllSt_ItemGroup, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<St_WarehouseSearchHVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult SearchSt_ItemGroupH(string ID)
        {
            ViewBag.ID = ID;
            return PartialView();
        }
        public JsonResult GetAllSt_ItemCardH(St_WarehouseSearchHVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllSt_ItemCardH = _unitOfWork.NativeSql.GetAllSt_ItemCardH(UserInfo.fCompanyId);
                if (AllSt_ItemCardH == null)
                {
                    return Json(new List<St_WarehouseSearchHVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.SearchItemCode))
                {
                    AllSt_ItemCardH = AllSt_ItemCardH.Where(m => m.SearchItemCode.Contains(Obj.SearchItemCode)).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.SearchItemName))
                {
                    AllSt_ItemCardH = AllSt_ItemCardH.Where(m => m.SearchItemName.Contains(Obj.SearchItemName)).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.SearchGroupCode))
                {
                    AllSt_ItemCardH = AllSt_ItemCardH.Where(m => m.SearchGroupCode == Obj.SearchGroupCode).ToList();
                }
                return Json(AllSt_ItemCardH, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<St_WarehouseSearchHVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult SearchSt_ItemCardH(string ID)
        {
            ViewBag.ID = ID;
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var St_ItemCardHObj = new St_WarehouseSearchHVM
            {
                St_ItemGroupH = _unitOfWork.St_ItemGroupH.GetAllSt_ItemGroupH(UserInfo.fCompanyId),
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency
            };
            return PartialView(St_ItemCardHObj);
        }
        public JsonResult GetSt_FactoryH(int id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                string UserID = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
                var model = _unitOfWork.NativeSql.GetAllSt_FactoryH(UserInfo.fCompanyId);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return Json(new List<St_WarehouseSearchHVM>(), JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult SearchSt_FactoryH(string ID)
        {
            ViewBag.ID = ID;
            return PartialView();
        }
        public JsonResult GetAllSt_ItemCard(St_WarehouseSearchVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllSt_ItemCard = _unitOfWork.NativeSql.GetAllSt_ItemCard(UserInfo.fCompanyId);
                if (AllSt_ItemCard == null)
                {
                    return Json(new List<St_ItemCardVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.ItemName))
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.ItemName.Contains(Obj.ItemName)).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.SearchSupplierAccountNumber))
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.SearchSupplierAccountNumber == Obj.SearchSupplierAccountNumber).ToList();
                }
                if (Obj.ItemUnitNo != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.ItemUnitNo == Obj.ItemUnitNo).ToList();
                }
                if (Obj.CountryOfOriginNo != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.CountryOfOriginNo == Obj.CountryOfOriginNo).ToList();
                }
                if (Obj.ManufacturerCompanyNo != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.ManufacturerCompanyNo == Obj.ManufacturerCompanyNo).ToList();
                }
                if (Obj.ItemNatureNo != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.ItemNatureNo == Obj.ItemNatureNo).ToList();
                }
                if (Obj.ItemCaseInt != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.StopItem == Convert.ToBoolean(Obj.ItemCaseInt - 1)).ToList();
                }
                if (Obj.Categorie_1 != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.Categorie_1 == Obj.Categorie_1).ToList();
                }
                if (Obj.Categorie_2 != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.Categorie_2 == Obj.Categorie_2).ToList();
                }
                if (Obj.Categorie_3 != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.Categorie_3 == Obj.Categorie_3).ToList();
                }
                if (Obj.Categorie_4 != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.Categorie_4 == Obj.Categorie_4).ToList();
                }
                if (Obj.Categorie_5 != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.Categorie_5 == Obj.Categorie_5).ToList();
                }
                if (Obj.Categorie_6 != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.Categorie_6 == Obj.Categorie_6).ToList();
                }
                if (Obj.Categorie_7 != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.Categorie_7 == Obj.Categorie_7).ToList();
                }
                if (Obj.Categorie_8 != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.Categorie_8 == Obj.Categorie_8).ToList();
                }
                if (Obj.Categorie_9 != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.Categorie_9 == Obj.Categorie_9).ToList();
                }
                if (Obj.Categorie_10 != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.Categorie_10 == Obj.Categorie_10).ToList();
                }
                if (Obj.Categorie_11 != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.Categorie_11 == Obj.Categorie_11).ToList();
                }
                if (Obj.Categorie_12 != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.Categorie_12 == Obj.Categorie_12).ToList();
                }
                if (Obj.Categorie_13 != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.Categorie_13 == Obj.Categorie_13).ToList();
                }
                if (Obj.Categorie_14 != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.Categorie_14 == Obj.Categorie_14).ToList();
                }
                if (Obj.Categorie_15 != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.Categorie_15 == Obj.Categorie_15).ToList();
                }
                return Json(AllSt_ItemCard, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<St_ItemCardVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult GetSt_ItemCard(int id)
        {
            try
            {

                string UserID = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(UserID);


                var model = _unitOfWork.NativeSql.GetSt_ItemCard(UserInfo.fCompanyId);



                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string err = ex.Message;



                return Json(new List<MutilUseSearchVM>(), JsonRequestBehavior.AllowGet);

            }


        }
        public ActionResult SearchSt_ItemCard(string ID)
        {
            ViewBag.ID = ID;
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var St_ItemCardObj = new St_WarehouseSearchVM
            {
                St_ItemUnit = _unitOfWork.St_ItemUnit.GetAllItemUnit(UserInfo.fCompanyId),
                St_CountryOfOrigin = _unitOfWork.St_CountryOfOrigin.GetAllSt_CountryOfOrigin(UserInfo.fCompanyId),
                St_ManufacturerCompany = _unitOfWork.St_ManufacturerCompany.GetAllSt_ManufacturerCompany(UserInfo.fCompanyId),
                St_DescriptionDetail1 = _unitOfWork.St_DescriptionDetail.GetSt_DescriptionDetailBySt_Description(UserInfo.fCompanyId, 1),
                St_DescriptionDetail2 = _unitOfWork.St_DescriptionDetail.GetSt_DescriptionDetailBySt_Description(UserInfo.fCompanyId, 2),
                St_DescriptionDetail3 = _unitOfWork.St_DescriptionDetail.GetSt_DescriptionDetailBySt_Description(UserInfo.fCompanyId, 3),
                St_DescriptionDetail4 = _unitOfWork.St_DescriptionDetail.GetSt_DescriptionDetailBySt_Description(UserInfo.fCompanyId, 4),
                St_DescriptionDetail5 = _unitOfWork.St_DescriptionDetail.GetSt_DescriptionDetailBySt_Description(UserInfo.fCompanyId, 5),
                St_DescriptionDetail6 = _unitOfWork.St_DescriptionDetail.GetSt_DescriptionDetailBySt_Description(UserInfo.fCompanyId, 6),
                St_DescriptionDetail7 = _unitOfWork.St_DescriptionDetail.GetSt_DescriptionDetailBySt_Description(UserInfo.fCompanyId, 7),
                St_DescriptionDetail8 = _unitOfWork.St_DescriptionDetail.GetSt_DescriptionDetailBySt_Description(UserInfo.fCompanyId, 8),
                St_DescriptionDetail9 = _unitOfWork.St_DescriptionDetail.GetSt_DescriptionDetailBySt_Description(UserInfo.fCompanyId, 9),
                St_DescriptionDetail10 = _unitOfWork.St_DescriptionDetail.GetSt_DescriptionDetailBySt_Description(UserInfo.fCompanyId, 10),
                St_DescriptionDetail11 = _unitOfWork.St_DescriptionDetail.GetSt_DescriptionDetailBySt_Description(UserInfo.fCompanyId, 11),
                St_DescriptionDetail12 = _unitOfWork.St_DescriptionDetail.GetSt_DescriptionDetailBySt_Description(UserInfo.fCompanyId, 12),
                St_DescriptionDetail13 = _unitOfWork.St_DescriptionDetail.GetSt_DescriptionDetailBySt_Description(UserInfo.fCompanyId, 13),
                St_DescriptionDetail14 = _unitOfWork.St_DescriptionDetail.GetSt_DescriptionDetailBySt_Description(UserInfo.fCompanyId, 14),
                St_DescriptionDetail15 = _unitOfWork.St_DescriptionDetail.GetSt_DescriptionDetailBySt_Description(UserInfo.fCompanyId, 15),
                Categorie_1Name = _unitOfWork.St_Description.GetSt_DescriptionNameByID(UserInfo.fCompanyId, 1),
                Categorie_2Name = _unitOfWork.St_Description.GetSt_DescriptionNameByID(UserInfo.fCompanyId, 2),
                Categorie_3Name = _unitOfWork.St_Description.GetSt_DescriptionNameByID(UserInfo.fCompanyId, 3),
                Categorie_4Name = _unitOfWork.St_Description.GetSt_DescriptionNameByID(UserInfo.fCompanyId, 4),
                Categorie_5Name = _unitOfWork.St_Description.GetSt_DescriptionNameByID(UserInfo.fCompanyId, 5),
                Categorie_6Name = _unitOfWork.St_Description.GetSt_DescriptionNameByID(UserInfo.fCompanyId, 6),
                Categorie_7Name = _unitOfWork.St_Description.GetSt_DescriptionNameByID(UserInfo.fCompanyId, 7),
                Categorie_8Name = _unitOfWork.St_Description.GetSt_DescriptionNameByID(UserInfo.fCompanyId, 8),
                Categorie_9Name = _unitOfWork.St_Description.GetSt_DescriptionNameByID(UserInfo.fCompanyId, 9),
                Categorie_10Name = _unitOfWork.St_Description.GetSt_DescriptionNameByID(UserInfo.fCompanyId, 10),
                Categorie_11Name = _unitOfWork.St_Description.GetSt_DescriptionNameByID(UserInfo.fCompanyId, 11),
                Categorie_12Name = _unitOfWork.St_Description.GetSt_DescriptionNameByID(UserInfo.fCompanyId, 12),
                Categorie_13Name = _unitOfWork.St_Description.GetSt_DescriptionNameByID(UserInfo.fCompanyId, 13),
                Categorie_14Name = _unitOfWork.St_Description.GetSt_DescriptionNameByID(UserInfo.fCompanyId, 14),
                Categorie_15Name = _unitOfWork.St_Description.GetSt_DescriptionNameByID(UserInfo.fCompanyId, 15),
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency
            };
            return PartialView(St_ItemCardObj);
        }
        public JsonResult GetSt_ItemUnit(int id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                string UserID = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
                var model = _unitOfWork.NativeSql.GetAllSt_ItemUnit(UserInfo.fCompanyId);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return Json(new List<St_WarehouseSearchVM>(), JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult SearchSt_ItemUnit(string ID)
        {
            ViewBag.ID = ID;
            return PartialView();
        }
        [HttpGet]
        public JsonResult CheckAcountAcc(string id)
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
        public JsonResult CheckCostCenter(string id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);

            var CostCenterInfo = _unitOfWork.NativeSql.CheckTransActionCostCenter(UserInfo.fCompanyId, id);
            if (CostCenterInfo == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(CostCenterInfo, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult CheckAllAcountAcc(string id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);

            var AccountInfo = _unitOfWork.NativeSql.CheckAllAccountNumber(UserInfo.fCompanyId, id);
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
        public JsonResult CheckFatherAcountAcc(string id)
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
        public JsonResult CheckDefinitionRevenueReceived(string id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var DefinitionObj = _unitOfWork.DefinitionOtherAccount.GetDefinitionOtherAccountByID(UserInfo.fCompanyId);
            var RevenuesReceived = DefinitionObj.RevenuesReceivedAccountNumber + "%";
            var model = _unitOfWork.NativeSql.CheckAllDefinitionRevenueReceived(UserInfo.fCompanyId, RevenuesReceived, id);
            if (model == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }  
        [HttpGet]
        public JsonResult CheckDefinitionRevenue(string id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var DefinitionObj = _unitOfWork.DefinitionOtherAccount.GetDefinitionOtherAccountByID(UserInfo.fCompanyId);
            var Revenues = DefinitionObj.RevenuesAccountNumber + "%";
            var model = _unitOfWork.NativeSql.CheckAllDefinitionRevenue(UserInfo.fCompanyId, Revenues , id);
            if (model == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }    
        [HttpGet]
        public JsonResult CheckDefinitionExpense(string id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var DefinitionObj = _unitOfWork.DefinitionOtherAccount.GetDefinitionOtherAccountByID(UserInfo.fCompanyId);
            var Expense = DefinitionObj.ExpensesAccountNumber + "%";
            var model = _unitOfWork.NativeSql.CheckAllDefinitionExpense(UserInfo.fCompanyId, Expense , id);
            if (model == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        } 
        [HttpGet]
        public JsonResult CheckDefinitionPaidExpense(string id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var DefinitionObj = _unitOfWork.DefinitionOtherAccount.GetDefinitionOtherAccountByID(UserInfo.fCompanyId);
            var PaidExpense = DefinitionObj.PaidExpensesAccountNumber + "%";
            var model = _unitOfWork.NativeSql.CheckAllDefinitionPaidExpense(UserInfo.fCompanyId, PaidExpense , id);
            if (model == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }
         [HttpGet]
        public JsonResult CheckDefinitionBank(string id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var model = _unitOfWork.NativeSql.CheckAllBankAccountNumber(UserInfo.fCompanyId , id);
            if (model == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult CheckClientAccount(string id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var model = _unitOfWork.NativeSql.CheckClientAccount(UserInfo.fCompanyId, id);
            if (model == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }
        
        [HttpGet]
        public JsonResult CheckCustomerAccount(string id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var model = _unitOfWork.NativeSql.CheckCustomerAccount(UserInfo.fCompanyId , id);
            if (model == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public JsonResult CheckDrawnBank(string id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var model = _unitOfWork.NativeSql.CheckAllDrawnBank(UserInfo.fCompanyId, id);
            if (model == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public JsonResult CheckDefinitionChequeFund(string id)
        {
            var userId = User.Identity.GetUserId();
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var DefinitionObj = _unitOfWork.DefinitionOtherAccount.GetDefinitionOtherAccountByID(UserInfo.fCompanyId);
            var ChequeFund = DefinitionObj.ChequeFundAccountNumber + "%";
            var model = _unitOfWork.NativeSql.CheckAllDefinitionChequeFund(UserInfo.fCompanyId, ChequeFund , id);
            if (model == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult CheckDefinitionFund(string id)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var DefinitionObj = _unitOfWork.DefinitionOtherAccount.GetDefinitionOtherAccountByID(UserInfo.fCompanyId);
            var CashFund = DefinitionObj.CashFundsAccountNumber + "%";
            var model = _unitOfWork.NativeSql.CheckAllDefinitionFund(UserInfo.fCompanyId, CashFund, id);
            if (model == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }



        [HttpGet]
        public JsonResult CheckCustomerClientAccount(string id)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            var model = _unitOfWork.NativeSql.CheckCustomerAndClientAccount(UserInfo.fCompanyId, id);
            if (model == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult CheckDefinitionTax(string id)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            var model = _unitOfWork.NativeSql.CheckAllDefinitionTax(UserInfo.fCompanyId , id);
            if (model == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }   
        [HttpGet]
        public JsonResult CheckAllServices(string id)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            var model = _unitOfWork.NativeSql.CheckAllService(UserInfo.fCompanyId , id);
            if (model == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }  
        
        
        [HttpGet]
        public JsonResult CheckSalesGainsAcountAcc(string id)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            var model = _unitOfWork.NativeSql.CheckAllTransActionAccountSalesGain(UserInfo.fCompanyId , id);
            if (model == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }



        [HttpGet]
        public JsonResult CheckPurchaseAcountAcc(string id)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            var model = _unitOfWork.NativeSql.CheckAllTransActionAccountPurchase(UserInfo.fCompanyId , id);
            if (model == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }   
        
        [HttpGet]
        public JsonResult CheckAllAsset(string id)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var model = _unitOfWork.Asset.CheckAllAssetNative(UserInfo.fCompanyId, 0 , id);
            if (model == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult CheckAllDefinitionAsset(string id)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var model = _unitOfWork.NativeSql.CheckAllDefinitionAsset(UserInfo.fCompanyId , id);
            if (model == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult CheckSt_ItemCard(string id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);

            var ItemInfo = _unitOfWork.NativeSql.CheckSt_ItemCard(UserInfo.fCompanyId, id);
            if (ItemInfo == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(ItemInfo, JsonRequestBehavior.AllowGet);
            }
        }


    }
}