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
    public class St_InitialInventoryController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public St_InitialInventoryController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var St_ItemWarehouseObj = new St_ItemCardVM
            {
                St_ItemUnit = _unitOfWork.St_ItemUnit.GetAllItemUnit(UserInfo.fCompanyId),
                St_CountryOfOrigin = _unitOfWork.St_CountryOfOrigin.GetAllSt_CountryOfOrigin(UserInfo.fCompanyId),
                St_ManufacturerCompany = _unitOfWork.St_ManufacturerCompany.GetAllSt_ManufacturerCompany(UserInfo.fCompanyId),
                St_Warehouse = _unitOfWork.St_Warehouse.GetAllSt_Warehouse(UserInfo.fCompanyId),
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
            return View(St_ItemWarehouseObj);
        }
        [HttpPost]
        public JsonResult GetAllSt_ItemCardFilterByWarehouseToInitialInventory(St_ItemCardVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllSt_ItemCard = _unitOfWork.NativeSql.GetAllSt_ItemCardFilterByWarehouseToInitialInventory(UserInfo.fCompanyId, Obj.ItemCode, Obj.StockCode);
                if (AllSt_ItemCard == null)
                {
                    return Json(new List<St_ItemCardVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.ItemName))
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.ItemName.Contains(Obj.ItemName)).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.SupplierAccountNumber))
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.SupplierAccountNumber == Obj.SupplierAccountNumber).ToList();
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
                if (Obj.ItemCaseInt != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.StopItem == Convert.ToBoolean(Obj.ItemCaseInt - 1)).ToList();
                }
                if (Obj.ItemNatureNo != 0)
                {
                    AllSt_ItemCard = AllSt_ItemCard.Where(m => m.ItemNatureNo == Obj.ItemNatureNo).ToList();
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
        public ActionResult SaveInitialInventory(string id, string id2)
        {
            try
            {
                if (id != "")
                {
                    var userId = User.Identity.GetUserId();
                    var UserInfo = _unitOfWork.User.GetUserByID(userId);
                    var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                    if (UserInfo == null)
                    {
                        RedirectToAction("", "");
                    }
                    var Obj = _unitOfWork.NativeSql.GetSt_InitialInventoryByItemAndStock(UserInfo.fCompanyId, id, id2);
                    var St_ItemCardVM = new St_ItemCardVM { };
                    St_ItemCardVM.UpdateItemCode = Obj.ItemCode;
                    St_ItemCardVM.StockCode = Obj.StockCode;
                    St_ItemCardVM.ItemName = Obj.ItemName;
                    St_ItemCardVM.StockName = Obj.StockName;
                    St_ItemCardVM.CostRate = Obj.CostRate;
                    St_ItemCardVM.Quantity = Obj.Quantity;
                    return PartialView("SaveInitialInventory", St_ItemCardVM);
                }
                return PartialView("SaveInitialInventory", new St_ItemCardVM());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return View("Error");
            }
        }
        [HttpPost]
        public JsonResult SaveInitialInventory(St_ItemCardVM ObjSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                int year = (DateTime.Now.Year) - 1;
                var CheckIfInitialInventoryExsitInHeader = _unitOfWork.St_Header.CheckIfInitialInventoryExsitInHeader(UserInfo.fCompanyId);
                if (CheckIfInitialInventoryExsitInHeader == null)
                {
                    var ObjSaveSt_Header = new St_Header();
                    ObjSaveSt_Header.CompanyID = UserInfo.fCompanyId;
                    ObjSaveSt_Header.CompanyYear = UserInfo.CurrYear;
                    ObjSaveSt_Header.CompanyTransactionKindNo = 1;
                    ObjSaveSt_Header.TransactionKindNo = 517;
                    ObjSaveSt_Header.StockCode = "*";
                    ObjSaveSt_Header.VoucherNumber = "0";
                    ObjSaveSt_Header.VHI = 0;
                    ObjSaveSt_Header.VoucherDate = new DateTime(year, 12, 31);
                    ObjSaveSt_Header.AccountNumber = "";
                    ObjSaveSt_Header.SaleID = 0;
                    ObjSaveSt_Header.NetTotalLocalBeforDiscount = 0;
                    ObjSaveSt_Header.NetTotalForeignBeforDiscount = 0;
                    ObjSaveSt_Header.NetTotalLineDiscountLocal = 0;
                    ObjSaveSt_Header.NetTotalLineDiscountForeign = 0;
                    ObjSaveSt_Header.NetTotalLocalAfterLineDiscount = 0;
                    ObjSaveSt_Header.NetTotalForeignAfterLineDiscount = 0;
                    ObjSaveSt_Header.NetTotalTaxAfterLineDiscountLocal = 0;
                    ObjSaveSt_Header.NetTotalTaxAfterLineDiscounForeign = 0;
                    ObjSaveSt_Header.NetTotalAfterLineDiscountBeforDiscountAllLocal = 0;
                    ObjSaveSt_Header.NetTotalAfterLineDiscountBeforDiscountAllForeign = 0;
                    ObjSaveSt_Header.NetTotalDiscountLocal = 0;
                    ObjSaveSt_Header.NetTotalDiscountForeign = 0;
                    ObjSaveSt_Header.DiscountPercentage = 0;
                    ObjSaveSt_Header.NetTotalLocalAfterDiscount = 0;
                    ObjSaveSt_Header.NetTotalForeignAfterDiscount = 0;
                    ObjSaveSt_Header.TaxType = 3;
                    ObjSaveSt_Header.NetTotalTaxLocal = 0;
                    ObjSaveSt_Header.NetTotalTaxForeign = 0;
                    ObjSaveSt_Header.NetTotalLocal = 0;
                    ObjSaveSt_Header.NetTotalForeign = 0;
                    if (Resources.Resource.CurLang == "Arb")
                    {
                        ObjSaveSt_Header.Remark = "جرد اولي";
                        ObjSaveSt_Header.Hint = "جرد اولي";
                    }
                    else
                    {
                        ObjSaveSt_Header.Remark = "Initial Inventory";
                        ObjSaveSt_Header.Hint = "Initial Inventory";
                    }
                    ObjSaveSt_Header.Exported = 0;
                    ObjSaveSt_Header.CurrencyID = 1;
                    ObjSaveSt_Header.ConversionFactor = 1;
                    ObjSaveSt_Header.ConversionFactorAfterExpenses = 0;
                    ObjSaveSt_Header.VoucherCase = 0;
                    ObjSaveSt_Header.CashLocal = 0;
                    ObjSaveSt_Header.CreditCardType1 = 0;
                    ObjSaveSt_Header.CreditCardLocal1 = 0;
                    ObjSaveSt_Header.CreditCardType2 = 0;
                    ObjSaveSt_Header.CreditCardLocal2 = 0;
                    ObjSaveSt_Header.ChequeLocal = 0;
                    ObjSaveSt_Header.CreditLocal = 0;
                    ObjSaveSt_Header.CashForeign = 0;
                    ObjSaveSt_Header.CreditCardForeign1 = 0;
                    ObjSaveSt_Header.CreditCardForeign2 = 0;
                    ObjSaveSt_Header.ChequeForeign = 0;
                    ObjSaveSt_Header.CreditForeign = 0;
                    ObjSaveSt_Header.DueDate = DateTime.Now;
                    ObjSaveSt_Header.OfferNumber ="";
                    ObjSaveSt_Header.OrderNumber = "";
                    ObjSaveSt_Header.LocalCost = 0;
                    ObjSaveSt_Header.ForeignCost = 0;
                    ObjSaveSt_Header.DriverID = 0;
                    ObjSaveSt_Header.OriginalVoucherNumber = "0";
                    ObjSaveSt_Header.ShippingValueLocal = 0;
                    ObjSaveSt_Header.ShippingValueForeign = 0;
                    ObjSaveSt_Header.ShippingAccountNumber = "";
                    ObjSaveSt_Header.ShippingCostCenter = "";
                    ObjSaveSt_Header.OtherExpensesValueLocal = 0;
                    ObjSaveSt_Header.OtherExpensesValueForeign = 0;
                    ObjSaveSt_Header.OtherExpensesAccountNumber = "";
                    ObjSaveSt_Header.OtherExpensesCostCenter = "";
                    ObjSaveSt_Header.NetTotalOfGoodsLocal = 0;
                    ObjSaveSt_Header.NetTotalOfGoodsForeign = 0;
                    ObjSaveSt_Header.ExtraExpensesLocal1 = 0;
                    ObjSaveSt_Header.ExtraExpensesLocal2 = 0;
                    ObjSaveSt_Header.ExtraExpensesLocal3 = 0;
                    ObjSaveSt_Header.ExtraExpensesLocal4 = 0;
                    ObjSaveSt_Header.ExtraExpensesLocal5 = 0;
                    ObjSaveSt_Header.ExtraExpensesLocal6 = 0;
                    ObjSaveSt_Header.ExtraExpensesLocal7 = 0;
                    ObjSaveSt_Header.ExtraExpensesLocal8 = 0;
                    ObjSaveSt_Header.ExtraExpensesLocal9 = 0;
                    ObjSaveSt_Header.ExtraExpensesLocal10 = 0;
                    ObjSaveSt_Header.ExtraExpensesLocal11 = 0;
                    ObjSaveSt_Header.ExtraExpensesLocal12 = 0;
                    ObjSaveSt_Header.ExtraExpensesLocal13 = 0;
                    ObjSaveSt_Header.ExtraExpensesLocal14 = 0;
                    ObjSaveSt_Header.ExtraExpensesLocal15 = 0;
                    ObjSaveSt_Header.ExtraExpensesLocal16 = 0;
                    ObjSaveSt_Header.ExtraExpensesLocal17 = 0;
                    ObjSaveSt_Header.ExtraExpensesLocal18 = 0;
                    ObjSaveSt_Header.ExtraExpensesLocal19 = 0;
                    ObjSaveSt_Header.ExtraExpensesLocal20 = 0;
                    ObjSaveSt_Header.ExtraExpensesForeign1 = 0;
                    ObjSaveSt_Header.ExtraExpensesForeign2 = 0;
                    ObjSaveSt_Header.ExtraExpensesForeign3 = 0;
                    ObjSaveSt_Header.ExtraExpensesForeign4 = 0;
                    ObjSaveSt_Header.ExtraExpensesForeign5 = 0;
                    ObjSaveSt_Header.ExtraExpensesForeign6 = 0;
                    ObjSaveSt_Header.ExtraExpensesForeign7 = 0;
                    ObjSaveSt_Header.ExtraExpensesForeign8 = 0;
                    ObjSaveSt_Header.ExtraExpensesForeign9 = 0;
                    ObjSaveSt_Header.ExtraExpensesForeign10 = 0;
                    ObjSaveSt_Header.ExtraExpensesForeign11 = 0;
                    ObjSaveSt_Header.ExtraExpensesForeign12 = 0;
                    ObjSaveSt_Header.ExtraExpensesForeign13 = 0;
                    ObjSaveSt_Header.ExtraExpensesForeign14 = 0;
                    ObjSaveSt_Header.ExtraExpensesForeign15 = 0;
                    ObjSaveSt_Header.ExtraExpensesForeign16 = 0;
                    ObjSaveSt_Header.ExtraExpensesForeign17 = 0;
                    ObjSaveSt_Header.ExtraExpensesForeign18 = 0;
                    ObjSaveSt_Header.ExtraExpensesForeign19 = 0;
                    ObjSaveSt_Header.ExtraExpensesForeign20 = 0;
                    ObjSaveSt_Header.InsDateTime = DateTime.Now;
                    ObjSaveSt_Header.InsUserID = userId;
                    ObjSaveSt_Header.RowCount = _unitOfWork.St_Header.GetMaxRowNumberInitialInventory(UserInfo.fCompanyId);
                    var ObjSaveSt_Transaction = new St_Transaction();
                    ObjSaveSt_Transaction.CompanyID = ObjSaveSt_Header.CompanyID;
                    ObjSaveSt_Transaction.CompanyYear = ObjSaveSt_Header.CompanyYear;
                    ObjSaveSt_Transaction.CompanyTransactionKindNo = ObjSaveSt_Header.CompanyTransactionKindNo;
                    ObjSaveSt_Transaction.TransactionKindNo = ObjSaveSt_Header.TransactionKindNo;
                    ObjSaveSt_Transaction.StockCode = ObjSave.StockCode;
                    ObjSaveSt_Transaction.VoucherNumber = ObjSaveSt_Header.VoucherNumber;
                    ObjSaveSt_Transaction.RowNumber = _unitOfWork.St_Header.GetMaxRowNumberInitialInventory(UserInfo.fCompanyId);
                    ObjSaveSt_Transaction.IsDeleted = 0;
                    ObjSaveSt_Transaction.VHI = ObjSaveSt_Header.VHI;
                    ObjSaveSt_Transaction.VoucherDate = ObjSaveSt_Header.VoucherDate;
                    ObjSaveSt_Transaction.ItemCode = ObjSave.UpdateItemCode;
                    ObjSaveSt_Transaction.SimilarItemCode = ObjSave.UpdateItemCode;
                    ObjSaveSt_Transaction.Quantity = ObjSave.Quantity;
                    ObjSaveSt_Transaction.Bonus =0;
                    ObjSaveSt_Transaction.QuantityInputOutput = ObjSave.Quantity;
                    ObjSaveSt_Transaction.BonusInputOutput = 0;
                    ObjSaveSt_Transaction.TotalLocalBeforDiscount = (ObjSave.Quantity * ObjSave.CostRate);
                    ObjSaveSt_Transaction.TotalForeignBeforDiscount = 0;
                    ObjSaveSt_Transaction.TotalLineDiscountLocal = 0;
                    ObjSaveSt_Transaction.TotalLineDiscountForeign = 0;
                    ObjSaveSt_Transaction.TotalLocalAfterLineDiscount = 0;
                    ObjSaveSt_Transaction.TotalForeignAfterLineDiscount = 0;
                    ObjSaveSt_Transaction.LineDiscountPercentage = 0;
                    ObjSaveSt_Transaction.TotalTaxAfterLineDiscountLocal = 0;
                    ObjSaveSt_Transaction.TotalTaxAfterLineDiscounForeign = 0;
                    ObjSaveSt_Transaction.TotalAfterLineDiscountBeforDiscountAllLocal = 0;
                    ObjSaveSt_Transaction.TotalAfterLineDiscountBeforDiscountAllForeign = 0;
                    ObjSaveSt_Transaction.TotalDiscountLocal = 0;
                    ObjSaveSt_Transaction.TotalDiscountForeign = 0;
                    ObjSaveSt_Transaction.DiscountPercentage = 0;
                    ObjSaveSt_Transaction.TotalLocalAfterDiscount = (ObjSave.Quantity * ObjSave.CostRate);
                    ObjSaveSt_Transaction.TotalForeignAfterDiscount = 0;
                    ObjSaveSt_Transaction.TaxRate = 0;
                    ObjSaveSt_Transaction.TaxType = 0;
                    ObjSaveSt_Transaction.TotalTaxLocal = 0;
                    ObjSaveSt_Transaction.TotalTaxForeign = 0;
                    ObjSaveSt_Transaction.TotalLocal = (ObjSave.Quantity * ObjSave.CostRate);
                    ObjSaveSt_Transaction.TotalForeign = 0;
                    ObjSaveSt_Transaction.TotalCostLocal = (ObjSave.Quantity * ObjSave.CostRate);
                    ObjSaveSt_Transaction.TotalCostForeign = 0;
                    if (ObjSave.Quantity <= 0)
                    {
                        ObjSaveSt_Transaction.PricePieceLocalBeforDiscount = 0;
                    }
                    else
                    {
                        ObjSaveSt_Transaction.PricePieceLocalBeforDiscount = ObjSave.CostRate;
                    }
                    ObjSaveSt_Transaction.PricePieceForeignBeforDiscount = 0;
                    ObjSaveSt_Transaction.PricePieceLineDiscountLocal = 0;
                    ObjSaveSt_Transaction.PricePieceLineDiscountForeign = 0;
                    ObjSaveSt_Transaction.PricePieceLocalAfterLineDiscount = 0;
                    ObjSaveSt_Transaction.PricePieceForeignAfterLineDiscount = 0;
                    ObjSaveSt_Transaction.PricePieceTaxAfterLineDiscountLocal = 0;
                    ObjSaveSt_Transaction.PricePieceTaxAfterLineDiscounForeign = 0;
                    ObjSaveSt_Transaction.PricePieceAfterLineDiscountBeforDiscountAllLocal = 0;
                    ObjSaveSt_Transaction.PricePieceAfterLineDiscountBeforDiscountAllForeign = 0;
                    ObjSaveSt_Transaction.PricePieceDiscountLocal = 0;
                    ObjSaveSt_Transaction.PricePieceDiscountForeign = 0;
                    if (ObjSave.Quantity <= 0)
                    {
                        ObjSaveSt_Transaction.PricePieceLocalAfterDiscount = 0;
                    }
                    else
                    {
                        ObjSaveSt_Transaction.PricePieceLocalAfterDiscount = ObjSave.CostRate;
                    }
                    ObjSaveSt_Transaction.PricePieceForeignAfterDiscount = 0;
                    ObjSaveSt_Transaction.PricePieceTaxLocal = 0;
                    ObjSaveSt_Transaction.PricePieceTaxForeign = 0;
                    if (ObjSave.Quantity <= 0)
                    {
                        ObjSaveSt_Transaction.PricePieceTotalLocal = 0;
                    }
                    else
                    {
                        ObjSaveSt_Transaction.PricePieceTotalLocal = ObjSave.CostRate;
                    }
                    ObjSaveSt_Transaction.PricePieceTotalForeign = 0;
                    if (ObjSave.Quantity <= 0)
                    {
                        ObjSaveSt_Transaction.CostPieceLocal = 0;
                    }
                    else
                    {
                        ObjSaveSt_Transaction.CostPieceLocal = ObjSave.CostRate;
                    }
                    ObjSaveSt_Transaction.CostPieceForeign = 0;
                    if (Resources.Resource.CurLang == "Arb")
                    {
                        ObjSaveSt_Transaction.Remark = "جرد اولي";
                        ObjSaveSt_Transaction.Hint = "جرد اولي";
                    }
                    else
                    {
                        ObjSaveSt_Transaction.Remark = "Initial Inventory";
                        ObjSaveSt_Transaction.Hint = "Initial Inventory";
                    }
                    ObjSaveSt_Transaction.ExpierDate = DateTime.Now;
                    ObjSaveSt_Transaction.BatchNumber = "";
                    ObjSaveSt_Transaction.InsDateTime = DateTime.Now;
                    ObjSaveSt_Transaction.InsUserID = userId;

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
                    _unitOfWork.St_Header.AddHeader(ObjSaveSt_Header);
                    _unitOfWork.St_Header.AddTransaction(ObjSaveSt_Transaction);
                    _unitOfWork.Complete();
                }
                else
                {
                    var CheckIfInitialInventoryExsitInTransaction = _unitOfWork.St_Header.CheckIfInitialInventoryExsitInTransaction(UserInfo.fCompanyId, ObjSave.UpdateItemCode, ObjSave.StockCode);
                    if (CheckIfInitialInventoryExsitInTransaction == null)
                    {
                        var ObjUpdateSt_Header = new St_Header();
                        ObjUpdateSt_Header.CompanyID = UserInfo.fCompanyId;
                        ObjUpdateSt_Header.RowCount = _unitOfWork.St_Header.GetMaxRowNumberInitialInventory(UserInfo.fCompanyId);
                        ObjUpdateSt_Header.DueDate = DateTime.Now;
                        ObjUpdateSt_Header.InsDateTime = DateTime.Now;
                        ObjUpdateSt_Header.InsUserID = userId;
                        var ObjSaveSt_Transaction = new St_Transaction();
                        ObjSaveSt_Transaction.CompanyID = UserInfo.fCompanyId;
                        ObjSaveSt_Transaction.CompanyYear = UserInfo.CurrYear;
                        ObjSaveSt_Transaction.CompanyTransactionKindNo = 1;
                        ObjSaveSt_Transaction.TransactionKindNo = 517;
                        ObjSaveSt_Transaction.StockCode = ObjSave.StockCode;
                        ObjSaveSt_Transaction.RowNumber = _unitOfWork.St_Header.GetMaxRowNumberInitialInventory(UserInfo.fCompanyId);
                        ObjSaveSt_Transaction.IsDeleted = 0;
                        ObjSaveSt_Transaction.VoucherNumber = "0";
                        ObjSaveSt_Transaction.VHI = 0;
                        ObjSaveSt_Transaction.VoucherDate = new DateTime(year, 12, 31);
                        ObjSaveSt_Transaction.ItemCode = ObjSave.UpdateItemCode;
                        ObjSaveSt_Transaction.SimilarItemCode = ObjSave.UpdateItemCode;
                        ObjSaveSt_Transaction.Quantity = ObjSave.Quantity;
                        ObjSaveSt_Transaction.Bonus = 0;
                        ObjSaveSt_Transaction.QuantityInputOutput = ObjSave.Quantity;
                        ObjSaveSt_Transaction.BonusInputOutput = 0;
                        ObjSaveSt_Transaction.TotalLocalBeforDiscount = (ObjSave.Quantity * ObjSave.CostRate);
                        ObjSaveSt_Transaction.TotalForeignBeforDiscount = 0;
                        ObjSaveSt_Transaction.TotalLineDiscountLocal = 0;
                        ObjSaveSt_Transaction.TotalLineDiscountForeign = 0;
                        ObjSaveSt_Transaction.TotalLocalAfterLineDiscount = 0;
                        ObjSaveSt_Transaction.TotalForeignAfterLineDiscount = 0;
                        ObjSaveSt_Transaction.LineDiscountPercentage = 0;
                        ObjSaveSt_Transaction.TotalTaxAfterLineDiscountLocal = 0;
                        ObjSaveSt_Transaction.TotalTaxAfterLineDiscounForeign = 0;
                        ObjSaveSt_Transaction.TotalAfterLineDiscountBeforDiscountAllLocal = 0;
                        ObjSaveSt_Transaction.TotalAfterLineDiscountBeforDiscountAllForeign = 0;
                        ObjSaveSt_Transaction.TotalDiscountLocal = 0;
                        ObjSaveSt_Transaction.TotalDiscountForeign = 0;
                        ObjSaveSt_Transaction.DiscountPercentage = 0;
                        ObjSaveSt_Transaction.TotalLocalAfterDiscount = (ObjSave.Quantity * ObjSave.CostRate);
                        ObjSaveSt_Transaction.TotalForeignAfterDiscount = 0;
                        ObjSaveSt_Transaction.TaxRate = 0;
                        ObjSaveSt_Transaction.TaxType = 0;
                        ObjSaveSt_Transaction.TotalTaxLocal = 0;
                        ObjSaveSt_Transaction.TotalTaxForeign = 0;
                        ObjSaveSt_Transaction.TotalLocal = (ObjSave.Quantity * ObjSave.CostRate);
                        ObjSaveSt_Transaction.TotalForeign = 0;
                        ObjSaveSt_Transaction.TotalCostLocal = (ObjSave.Quantity * ObjSave.CostRate);
                        ObjSaveSt_Transaction.TotalCostForeign = 0;
                        if (ObjSave.Quantity <= 0)
                        {
                            ObjSaveSt_Transaction.PricePieceLocalBeforDiscount = 0;
                        }
                        else
                        {
                            ObjSaveSt_Transaction.PricePieceLocalBeforDiscount = ObjSave.CostRate;
                        }
                        ObjSaveSt_Transaction.PricePieceForeignBeforDiscount = 0;
                        ObjSaveSt_Transaction.PricePieceLineDiscountLocal = 0;
                        ObjSaveSt_Transaction.PricePieceLineDiscountForeign = 0;
                        ObjSaveSt_Transaction.PricePieceLocalAfterLineDiscount = 0;
                        ObjSaveSt_Transaction.PricePieceForeignAfterLineDiscount = 0;
                        ObjSaveSt_Transaction.PricePieceTaxAfterLineDiscountLocal = 0;
                        ObjSaveSt_Transaction.PricePieceTaxAfterLineDiscounForeign = 0;
                        ObjSaveSt_Transaction.PricePieceAfterLineDiscountBeforDiscountAllLocal = 0;
                        ObjSaveSt_Transaction.PricePieceAfterLineDiscountBeforDiscountAllForeign = 0;
                        ObjSaveSt_Transaction.PricePieceDiscountLocal = 0;
                        ObjSaveSt_Transaction.PricePieceDiscountForeign = 0;
                        if (ObjSave.Quantity <= 0)
                        {
                            ObjSaveSt_Transaction.PricePieceLocalAfterDiscount = 0;
                        }
                        else
                        {
                            ObjSaveSt_Transaction.PricePieceLocalAfterDiscount = ObjSave.CostRate;
                        }
                        ObjSaveSt_Transaction.PricePieceForeignAfterDiscount = 0;
                        ObjSaveSt_Transaction.PricePieceTaxLocal = 0;
                        ObjSaveSt_Transaction.PricePieceTaxForeign = 0;
                        if (ObjSave.Quantity <= 0)
                        {
                            ObjSaveSt_Transaction.PricePieceTotalLocal = 0;
                        }
                        else
                        {
                            ObjSaveSt_Transaction.PricePieceTotalLocal = ObjSave.CostRate;
                        }
                        ObjSaveSt_Transaction.PricePieceTotalForeign = 0;
                        if (ObjSave.Quantity <= 0)
                        {
                            ObjSaveSt_Transaction.CostPieceLocal = 0;
                        }
                        else
                        {
                            ObjSaveSt_Transaction.CostPieceLocal = ObjSave.CostRate;
                        }
                        ObjSaveSt_Transaction.CostPieceForeign = 0;
                        if (Resources.Resource.CurLang == "Arb")
                        {
                            ObjSaveSt_Transaction.Remark = "جرد اولي";
                            ObjSaveSt_Transaction.Hint = "جرد اولي";
                        }
                        else
                        {
                            ObjSaveSt_Transaction.Remark = "Initial Inventory";
                            ObjSaveSt_Transaction.Hint = "Initial Inventory";
                        }
                        ObjSaveSt_Transaction.ExpierDate = DateTime.Now;
                        ObjSaveSt_Transaction.BatchNumber = "";
                        ObjSaveSt_Transaction.InsDateTime = DateTime.Now;
                        ObjSaveSt_Transaction.InsUserID = userId;

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
                        _unitOfWork.St_Header.UpdateRowCountInitialInventory(ObjUpdateSt_Header);
                        _unitOfWork.St_Header.AddTransaction(ObjSaveSt_Transaction);
                        _unitOfWork.Complete();
                    }
                    else
                    {
                        var ObjUpdateSt_Header = new St_Header();
                        ObjUpdateSt_Header.CompanyID = UserInfo.fCompanyId;
                        ObjUpdateSt_Header.DueDate = DateTime.Now;
                        ObjUpdateSt_Header.InsDateTime = DateTime.Now;
                        ObjUpdateSt_Header.InsUserID = userId;
                        var ObjUpdateSt_Transaction = new St_Transaction();
                        ObjUpdateSt_Transaction.CompanyID = UserInfo.fCompanyId;
                        ObjUpdateSt_Transaction.StockCode = ObjSave.StockCode;
                        ObjUpdateSt_Transaction.ItemCode = ObjSave.UpdateItemCode;
                        ObjUpdateSt_Transaction.Quantity = ObjSave.Quantity;
                        ObjUpdateSt_Transaction.QuantityInputOutput = ObjSave.Quantity;
                        ObjUpdateSt_Transaction.TotalLocalBeforDiscount = (ObjSave.Quantity * ObjSave.CostRate);
                        ObjUpdateSt_Transaction.TotalLocalAfterDiscount = (ObjSave.Quantity * ObjSave.CostRate);
                        ObjUpdateSt_Transaction.TotalLocal = (ObjSave.Quantity * ObjSave.CostRate);
                        ObjUpdateSt_Transaction.TotalCostLocal = (ObjSave.Quantity * ObjSave.CostRate);
                        if (ObjSave.Quantity <= 0)
                        {
                            ObjUpdateSt_Transaction.PricePieceLocalBeforDiscount = 0;
                            ObjUpdateSt_Transaction.PricePieceLocalAfterDiscount = 0;
                            ObjUpdateSt_Transaction.PricePieceTotalLocal = 0;
                            ObjUpdateSt_Transaction.CostPieceLocal = 0;
                        }
                        else
                        {
                            ObjUpdateSt_Transaction.PricePieceLocalBeforDiscount = ObjSave.CostRate;
                            ObjUpdateSt_Transaction.PricePieceLocalAfterDiscount = ObjSave.CostRate;
                            ObjUpdateSt_Transaction.PricePieceTotalLocal = ObjSave.CostRate;
                            ObjUpdateSt_Transaction.CostPieceLocal = ObjSave.CostRate;
                        }
                        ObjUpdateSt_Transaction.InsDateTime = DateTime.Now;
                        ObjUpdateSt_Transaction.InsUserID = userId;
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
                        
                        _unitOfWork.St_Header.UpdateInitialInventory(ObjUpdateSt_Header);
                        _unitOfWork.St_Header.UpdateInitialInventoryInTransaction(ObjUpdateSt_Transaction);
                        _unitOfWork.Complete();
                    }
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
    }
}