using Acc.Persistence;
using Acc.Repositories;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Acc.ViewModels;
using Acc.Helpers;
using Acc.Models;
using System.Data;
using DevExpress.DashboardCommon.Native;

namespace Acc.Controllers
{
    [Authorize]
    public class St_ReportController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public St_ReportController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ItemTransactions()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            int year = DateTime.Now.Year;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var St_Warehouse = _unitOfWork.St_Warehouse.GetAllSt_Warehouse(UserInfo.fCompanyId);
            var St_ItemTransactionReportVM = new St_ItemTransactionReportVM
            {
                St_Warehouse = St_Warehouse,
                FromDate = new DateTime(year, 1, 1),
                ToDate = new DateTime(year, 12, 31),
                ShowItemCost = true,
                ApprovingPreviousBalance = true,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency
            };
            return View(St_ItemTransactionReportVM);
        }
        [HttpPost]
        public JsonResult GetItemTransactions(St_ItemTransactionReportVM Obj)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            if (!String.IsNullOrEmpty(Obj.ItemCode))
            {
                if (!Obj.ApprovingPreviousBalance)
                {
                    if (!String.IsNullOrEmpty(Obj.StockCode))
                    {
                        var GetAverageCost = _unitOfWork.NativeSql.GetAverageCostByStockCode(UserInfo.fCompanyId, Obj.ItemCode, Obj.StockCode, Obj.ToDate);
                        DataTable dt = FunctionUnit.LINQResultToDataTable(GetAverageCost);
                        string ItemCode;
                        string VoucherNumber;
                        DateTime VoucherDate;
                        int TransactionKindNo;
                        string StockCode;
                        double PricePieceLocalAfterDiscount;
                        double TotalLocalAfterDiscount;
                        double TotalCostLocal;
                        double QuantityInputOutput;
                        string TransactionKindName;
                        string StockName;
                        string Remark;
                        double QuantityIn;
                        double QuantityOut;
                        string FromStockName;
                        string ToStockName;
                        string AccountName;
                        double CostPieceLocal;
                        DateTime InsDateTime = DateTime.Now;
                        String InsUserID = userId;
                        double Last_SumTotalCostLocal = 0;
                        double Last_SumQuantityInputOutput = 0;
                        double Last_AverageCost = 0;
                        double AverageCost = 0;
                        double SumTotalCostLocal = 0;
                        double SumQuantityInputOutput = 0;
                        double OldQuantityDifference = 0;
                        double OldTotalDifference = 0;
                        _unitOfWork.NativeSql.DeleteSt_TempCalculateAverageCost(InsUserID);
                        _unitOfWork.Complete();

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            ItemCode = (dt.Rows[i]["ItemCode"].ToString());
                            VoucherNumber = (dt.Rows[i]["VoucherNumber"].ToString());
                            VoucherDate = DateTime.Parse(dt.Rows[i]["VoucherDate"].ToString());
                            TransactionKindNo = int.Parse(dt.Rows[i]["TransactionKindNo"].ToString());
                            StockCode = (dt.Rows[i]["StockCode"].ToString());
                            TotalCostLocal = double.Parse(dt.Rows[i]["TotalCostLocal"].ToString());
                            QuantityInputOutput = double.Parse(dt.Rows[i]["QuantityInputOutput"].ToString());
                            PricePieceLocalAfterDiscount = double.Parse(dt.Rows[i]["PricePieceLocalAfterDiscount"].ToString());
                            TotalLocalAfterDiscount = double.Parse(dt.Rows[i]["TotalLocalAfterDiscount"].ToString());
                            QuantityIn = double.Parse(dt.Rows[i]["QuantityIn"].ToString());
                            QuantityOut = double.Parse(dt.Rows[i]["QuantityOut"].ToString());
                            TransactionKindName = (dt.Rows[i]["TransactionKindName"].ToString());
                            StockName = (dt.Rows[i]["StockName"].ToString());
                            Remark = (dt.Rows[i]["Remark"].ToString());
                            FromStockName = (dt.Rows[i]["FromStockName"].ToString());
                            ToStockName = (dt.Rows[i]["ToStockName"].ToString());
                            AccountName = (dt.Rows[i]["AccountName"].ToString());
                            CostPieceLocal = double.Parse(dt.Rows[i]["CostPieceLocal"].ToString());
                            if (i == 0)
                            {
                                Last_SumTotalCostLocal = TotalCostLocal;
                                Last_SumQuantityInputOutput = QuantityInputOutput;
                                if (QuantityInputOutput > 0)
                                {
                                    Last_AverageCost = (TotalCostLocal / QuantityInputOutput);
                                }
                                else if (QuantityInputOutput <= 0)
                                {
                                    Last_AverageCost = 0;
                                }
                                AverageCost = Last_AverageCost;
                                SumTotalCostLocal = Last_SumTotalCostLocal;
                                SumQuantityInputOutput = Last_SumQuantityInputOutput;
                            }
                            else
                            {
                                if (QuantityInputOutput > 0)
                                {
                                    if (Last_SumQuantityInputOutput > 0)
                                    {
                                        Last_SumQuantityInputOutput = Last_SumQuantityInputOutput + QuantityInputOutput;
                                        Last_SumTotalCostLocal = Last_SumTotalCostLocal + TotalCostLocal;
                                        if (Last_SumTotalCostLocal == 0)
                                        {
                                            Last_AverageCost = (TotalCostLocal / Last_SumQuantityInputOutput);
                                        }
                                        else
                                        {
                                            Last_AverageCost = (Last_SumTotalCostLocal / Last_SumQuantityInputOutput);
                                        }
                                        if (Last_AverageCost < 0)
                                        {
                                            Last_AverageCost = Last_AverageCost * -1;
                                        }
                                        AverageCost = Last_AverageCost;
                                        SumTotalCostLocal = Last_SumTotalCostLocal;
                                        SumQuantityInputOutput = Last_SumQuantityInputOutput;
                                    }
                                    else if (Last_SumQuantityInputOutput < 0)
                                    {
                                        OldQuantityDifference = Last_SumQuantityInputOutput + QuantityInputOutput;
                                        if (OldQuantityDifference > 0)
                                        {
                                            OldTotalDifference = Math.Abs(Last_SumQuantityInputOutput) * (CostPieceLocal - Last_AverageCost);
                                            Last_SumQuantityInputOutput = Last_SumQuantityInputOutput + QuantityInputOutput;
                                            Last_SumTotalCostLocal = Last_SumTotalCostLocal + TotalCostLocal;
                                            Last_AverageCost = (OldTotalDifference / Math.Abs(Last_SumQuantityInputOutput)) + CostPieceLocal;
                                            if (Last_AverageCost < 0)
                                            {
                                                Last_AverageCost = Last_AverageCost * -1;
                                            }
                                            AverageCost = Last_AverageCost;
                                            SumTotalCostLocal = Last_SumTotalCostLocal;
                                            SumQuantityInputOutput = Last_SumQuantityInputOutput;
                                        }
                                        else if (OldQuantityDifference < 0)
                                        {
                                            OldTotalDifference = QuantityInputOutput * (CostPieceLocal - Last_AverageCost);
                                            Last_SumQuantityInputOutput = Last_SumQuantityInputOutput + QuantityInputOutput;
                                            Last_SumTotalCostLocal = Last_SumTotalCostLocal + TotalCostLocal;
                                            Last_AverageCost = (OldTotalDifference / Math.Abs(Last_SumQuantityInputOutput)) + CostPieceLocal;
                                            if (Last_AverageCost < 0)
                                            {
                                                Last_AverageCost = Last_AverageCost * -1;
                                            }
                                            AverageCost = Last_AverageCost;
                                            SumTotalCostLocal = Last_SumTotalCostLocal;
                                            SumQuantityInputOutput = Last_SumQuantityInputOutput;
                                        }
                                        else if (OldQuantityDifference == 0)
                                        {
                                            Last_SumQuantityInputOutput = Last_SumQuantityInputOutput + QuantityInputOutput;
                                            Last_SumTotalCostLocal = Last_SumTotalCostLocal + TotalCostLocal;
                                            Last_AverageCost = CostPieceLocal;
                                            if (Last_AverageCost < 0)
                                            {
                                                Last_AverageCost = Last_AverageCost * -1;
                                            }
                                            AverageCost = Last_AverageCost;
                                            SumTotalCostLocal = Last_SumTotalCostLocal;
                                            SumQuantityInputOutput = Last_SumQuantityInputOutput;
                                        }
                                    }
                                    else if (Last_SumQuantityInputOutput == 0)
                                    {
                                        Last_SumQuantityInputOutput = Last_SumQuantityInputOutput + QuantityInputOutput;
                                        Last_SumTotalCostLocal = Last_SumTotalCostLocal + TotalCostLocal;
                                        Last_AverageCost = CostPieceLocal;
                                        if (Last_AverageCost < 0)
                                        {
                                            Last_AverageCost = Last_AverageCost * -1;
                                        }
                                        AverageCost = Last_AverageCost;
                                        SumTotalCostLocal = Last_SumTotalCostLocal;
                                        SumQuantityInputOutput = Last_SumQuantityInputOutput;
                                    }
                                }
                                else if (QuantityInputOutput < 0)
                                {
                                    TotalCostLocal = Last_AverageCost * QuantityInputOutput;
                                    Last_SumTotalCostLocal = Last_SumTotalCostLocal + TotalCostLocal;
                                    Last_SumQuantityInputOutput = Last_SumQuantityInputOutput + QuantityInputOutput;
                                    if (Last_AverageCost < 0)
                                    {
                                        Last_AverageCost = Last_AverageCost * -1;
                                    }
                                    AverageCost = Last_AverageCost;
                                    SumTotalCostLocal = Last_SumTotalCostLocal;
                                    SumQuantityInputOutput = Last_SumQuantityInputOutput;
                                    CostPieceLocal = Last_AverageCost;
                                }
                            }
                            _unitOfWork.NativeSql.SaveSt_TempCalculateAverageCost(ItemCode, VoucherDate, VoucherNumber, TransactionKindNo, StockCode, TotalCostLocal,
                                    QuantityInputOutput, SumTotalCostLocal, SumQuantityInputOutput, CostPieceLocal, AverageCost, InsUserID, InsDateTime, PricePieceLocalAfterDiscount,
                                    TotalLocalAfterDiscount, QuantityIn, QuantityOut, TransactionKindName, StockName, Remark, FromStockName, ToStockName, AccountName);

                            _unitOfWork.Complete();
                        }
                    }
                    else
                    {
                        var GetAverageCost = _unitOfWork.NativeSql.GetAverageCost(UserInfo.fCompanyId, Obj.ItemCode, Obj.ToDate);
                        DataTable dt = FunctionUnit.LINQResultToDataTable(GetAverageCost);
                        string ItemCode;
                        string VoucherNumber;
                        DateTime VoucherDate;
                        int TransactionKindNo;
                        string StockCode;
                        double PricePieceLocalAfterDiscount;
                        double TotalLocalAfterDiscount;
                        double TotalCostLocal;
                        double QuantityInputOutput;
                        string TransactionKindName;
                        string StockName;
                        string Remark;
                        double QuantityIn;
                        double QuantityOut;
                        string FromStockName;
                        string ToStockName;
                        string AccountName;
                        double CostPieceLocal;
                        DateTime InsDateTime = DateTime.Now;
                        String InsUserID = userId;
                        double Last_SumTotalCostLocal = 0;
                        double Last_SumQuantityInputOutput = 0;
                        double Last_AverageCost = 0;
                        double AverageCost = 0;
                        double SumTotalCostLocal = 0;
                        double SumQuantityInputOutput = 0;
                        double OldQuantityDifference = 0;
                        double OldTotalDifference = 0;
                        _unitOfWork.NativeSql.DeleteSt_TempCalculateAverageCost(InsUserID);
                        _unitOfWork.Complete();

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            ItemCode = (dt.Rows[i]["ItemCode"].ToString());
                            VoucherNumber = (dt.Rows[i]["VoucherNumber"].ToString());
                            VoucherDate = DateTime.Parse(dt.Rows[i]["VoucherDate"].ToString());
                            TransactionKindNo = int.Parse(dt.Rows[i]["TransactionKindNo"].ToString());
                            StockCode = (dt.Rows[i]["StockCode"].ToString());
                            TotalCostLocal = double.Parse(dt.Rows[i]["TotalCostLocal"].ToString());
                            QuantityInputOutput = double.Parse(dt.Rows[i]["QuantityInputOutput"].ToString());
                            PricePieceLocalAfterDiscount = double.Parse(dt.Rows[i]["PricePieceLocalAfterDiscount"].ToString());
                            TotalLocalAfterDiscount = double.Parse(dt.Rows[i]["TotalLocalAfterDiscount"].ToString());
                            QuantityIn = double.Parse(dt.Rows[i]["QuantityIn"].ToString());
                            QuantityOut = double.Parse(dt.Rows[i]["QuantityOut"].ToString());
                            TransactionKindName = (dt.Rows[i]["TransactionKindName"].ToString());
                            StockName = (dt.Rows[i]["StockName"].ToString());
                            Remark = (dt.Rows[i]["Remark"].ToString());
                            FromStockName = (dt.Rows[i]["FromStockName"].ToString());
                            ToStockName = (dt.Rows[i]["ToStockName"].ToString());
                            AccountName = (dt.Rows[i]["AccountName"].ToString());
                            CostPieceLocal = double.Parse(dt.Rows[i]["CostPieceLocal"].ToString());
                            if (i == 0)
                            {
                                Last_SumTotalCostLocal = TotalCostLocal;
                                Last_SumQuantityInputOutput = QuantityInputOutput;
                                if (QuantityInputOutput > 0)
                                {
                                    Last_AverageCost = (TotalCostLocal / QuantityInputOutput);
                                }
                                else if (QuantityInputOutput <= 0)
                                {
                                    Last_AverageCost = 0;
                                }
                                AverageCost = Last_AverageCost;
                                SumTotalCostLocal = Last_SumTotalCostLocal;
                                SumQuantityInputOutput = Last_SumQuantityInputOutput;
                            }
                            else
                            {
                                if (QuantityInputOutput > 0)
                                {
                                    if (Last_SumQuantityInputOutput > 0)
                                    {
                                        Last_SumQuantityInputOutput = Last_SumQuantityInputOutput + QuantityInputOutput;
                                        Last_SumTotalCostLocal = Last_SumTotalCostLocal + TotalCostLocal;
                                        if (Last_SumTotalCostLocal == 0)
                                        {
                                            Last_AverageCost = (TotalCostLocal / Last_SumQuantityInputOutput);
                                        }
                                        else
                                        {
                                            Last_AverageCost = (Last_SumTotalCostLocal / Last_SumQuantityInputOutput);
                                        }
                                        if (Last_AverageCost < 0)
                                        {
                                            Last_AverageCost = Last_AverageCost * -1;
                                        }
                                        AverageCost = Last_AverageCost;
                                        SumTotalCostLocal = Last_SumTotalCostLocal;
                                        SumQuantityInputOutput = Last_SumQuantityInputOutput;
                                    }
                                    else if (Last_SumQuantityInputOutput < 0)
                                    {
                                        OldQuantityDifference = Last_SumQuantityInputOutput + QuantityInputOutput;
                                        if (OldQuantityDifference > 0)
                                        {
                                            OldTotalDifference = Math.Abs(Last_SumQuantityInputOutput) * (CostPieceLocal - Last_AverageCost);
                                            Last_SumQuantityInputOutput = Last_SumQuantityInputOutput + QuantityInputOutput;
                                            Last_SumTotalCostLocal = Last_SumTotalCostLocal + TotalCostLocal;
                                            Last_AverageCost = (OldTotalDifference / Math.Abs(Last_SumQuantityInputOutput)) + CostPieceLocal;
                                            if (Last_AverageCost < 0)
                                            {
                                                Last_AverageCost = Last_AverageCost * -1;
                                            }
                                            AverageCost = Last_AverageCost;
                                            SumTotalCostLocal = Last_SumTotalCostLocal;
                                            SumQuantityInputOutput = Last_SumQuantityInputOutput;
                                        }
                                        else if (OldQuantityDifference < 0)
                                        {
                                            OldTotalDifference = QuantityInputOutput * (CostPieceLocal - Last_AverageCost);
                                            Last_SumQuantityInputOutput = Last_SumQuantityInputOutput + QuantityInputOutput;
                                            Last_SumTotalCostLocal = Last_SumTotalCostLocal + TotalCostLocal;
                                            Last_AverageCost = (OldTotalDifference / Math.Abs(Last_SumQuantityInputOutput)) + CostPieceLocal;
                                            if (Last_AverageCost < 0)
                                            {
                                                Last_AverageCost = Last_AverageCost * -1;
                                            }
                                            AverageCost = Last_AverageCost;
                                            SumTotalCostLocal = Last_SumTotalCostLocal;
                                            SumQuantityInputOutput = Last_SumQuantityInputOutput;
                                        }
                                        else if (OldQuantityDifference == 0)
                                        {
                                            Last_SumQuantityInputOutput = Last_SumQuantityInputOutput + QuantityInputOutput;
                                            Last_SumTotalCostLocal = Last_SumTotalCostLocal + TotalCostLocal;
                                            Last_AverageCost = CostPieceLocal;
                                            if (Last_AverageCost < 0)
                                            {
                                                Last_AverageCost = Last_AverageCost * -1;
                                            }
                                            AverageCost = Last_AverageCost;
                                            SumTotalCostLocal = Last_SumTotalCostLocal;
                                            SumQuantityInputOutput = Last_SumQuantityInputOutput;
                                        }
                                    }
                                    else if (Last_SumQuantityInputOutput == 0)
                                    {
                                        Last_SumQuantityInputOutput = Last_SumQuantityInputOutput + QuantityInputOutput;
                                        Last_SumTotalCostLocal = Last_SumTotalCostLocal + TotalCostLocal;
                                        Last_AverageCost = CostPieceLocal;
                                        if (Last_AverageCost < 0)
                                        {
                                            Last_AverageCost = Last_AverageCost * -1;
                                        }
                                        AverageCost = Last_AverageCost;
                                        SumTotalCostLocal = Last_SumTotalCostLocal;
                                        SumQuantityInputOutput = Last_SumQuantityInputOutput;
                                    }
                                }
                                else if (QuantityInputOutput < 0)
                                {
                                    TotalCostLocal = Last_AverageCost * QuantityInputOutput;
                                    Last_SumTotalCostLocal = Last_SumTotalCostLocal + TotalCostLocal;
                                    Last_SumQuantityInputOutput = Last_SumQuantityInputOutput + QuantityInputOutput;
                                    if (Last_AverageCost < 0)
                                    {
                                        Last_AverageCost = Last_AverageCost * -1;
                                    }
                                    AverageCost = Last_AverageCost;
                                    SumTotalCostLocal = Last_SumTotalCostLocal;
                                    SumQuantityInputOutput = Last_SumQuantityInputOutput;
                                    CostPieceLocal = Last_AverageCost;
                                }
                            }
                            _unitOfWork.NativeSql.SaveSt_TempCalculateAverageCost(ItemCode, VoucherDate, VoucherNumber, TransactionKindNo, StockCode, TotalCostLocal,
                                    QuantityInputOutput, SumTotalCostLocal, SumQuantityInputOutput, CostPieceLocal, AverageCost, InsUserID, InsDateTime, PricePieceLocalAfterDiscount,
                                    TotalLocalAfterDiscount, QuantityIn, QuantityOut, TransactionKindName, StockName, Remark, FromStockName, ToStockName, AccountName);

                            _unitOfWork.Complete();
                        }
                    }
                    try
                    {
                        St_ItemTransactionReportWithTotalVM LastObj = new St_ItemTransactionReportWithTotalVM();
                        int year = (DateTime.Now.Year) - 1;
                        DateTime OpeningDate = new DateTime(year, 12, 31);
                        if (!String.IsNullOrEmpty(Obj.StockCode))
                        {
                            FunctionUnit ObjFunction = new FunctionUnit();
                            var GetLastAverageCost = ObjFunction.GetAvgCostByStock(UserInfo.fCompanyId, Obj.ItemCode, Obj.StockCode, Obj.ToDate);
                            LastObj.LastAverageCost = GetLastAverageCost.AverageCost;
                            LastObj.EndOfPeriodBalance = GetLastAverageCost.SumQuantityInputOutput;
                            var GetStartAverageCost = ObjFunction.GetAvgCostByStock(UserInfo.fCompanyId, Obj.ItemCode, Obj.StockCode, Obj.FromDate);
                            LastObj.StartOfPeriodBalance = GetStartAverageCost.SumQuantityInputOutput;
                            var GetOpeningAverageCost = ObjFunction.GetAvgCostByStock(UserInfo.fCompanyId, Obj.ItemCode, Obj.StockCode, OpeningDate);
                            LastObj.OpeningBalance = GetOpeningAverageCost.SumQuantityInputOutput;


                        }
                        else
                        {
                            FunctionUnit ObjFunction = new FunctionUnit();
                            var GetLastAverageCost = ObjFunction.GetAvgCost(UserInfo.fCompanyId, Obj.ItemCode, Obj.ToDate);
                            LastObj.LastAverageCost = GetLastAverageCost.AverageCost;
                            LastObj.EndOfPeriodBalance = GetLastAverageCost.SumQuantityInputOutput;
                            var GetStartAverageCost = ObjFunction.GetAvgCost(UserInfo.fCompanyId, Obj.ItemCode, Obj.FromDate);
                            LastObj.StartOfPeriodBalance = GetStartAverageCost.SumQuantityInputOutput;
                            var GetOpeningAverageCost = ObjFunction.GetAvgCost(UserInfo.fCompanyId, Obj.ItemCode, OpeningDate);
                            LastObj.OpeningBalance = GetOpeningAverageCost.SumQuantityInputOutput;

                        }
                        var ItemTransactions = _unitOfWork.NativeSql.GetTempAverageCost(userId, Obj.FromDate, Obj.ToDate);
                        LastObj.St_ItemTransactionReportVM = ItemTransactions;
                        if (ItemTransactions == null)
                        {
                            St_ItemTransactionReportWithTotalVM EmptyObj = new St_ItemTransactionReportWithTotalVM();
                            EmptyObj.St_ItemTransactionReportVM = new List<St_ItemTransactionReportVM>();
                            EmptyObj.LastAverageCost = 0;
                            EmptyObj.EndOfPeriodBalance = 0;
                            EmptyObj.StartOfPeriodBalance = 0;
                            EmptyObj.OpeningBalance = 0;
                            return Json(EmptyObj, JsonRequestBehavior.AllowGet);
                        }
                        return Json(LastObj, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception ex)
                    {
                        St_ItemTransactionReportWithTotalVM EmptyObj = new St_ItemTransactionReportWithTotalVM();
                        EmptyObj.St_ItemTransactionReportVM = new List<St_ItemTransactionReportVM>();
                        EmptyObj.LastAverageCost = 0;
                        EmptyObj.EndOfPeriodBalance = 0;
                        EmptyObj.StartOfPeriodBalance = 0;
                        EmptyObj.OpeningBalance = 0;
                        ViewBag.Error = ex.Message.ToString();
                        return Json(EmptyObj, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(Obj.StockCode))
                    {

                        var GetAverageCost = _unitOfWork.NativeSql.GetAverageCostByStockCode(UserInfo.fCompanyId, Obj.ItemCode, Obj.StockCode,Obj.ToDate);
                        DataTable dt = FunctionUnit.LINQResultToDataTable(GetAverageCost);
                        string ItemCode;
                        string VoucherNumber;
                        DateTime VoucherDate;
                        int TransactionKindNo;
                        string StockCode;
                        double PricePieceLocalAfterDiscount;
                        double TotalLocalAfterDiscount;
                        double TotalCostLocal;
                        double QuantityInputOutput;
                        string TransactionKindName;
                        string StockName;
                        string Remark;
                        double QuantityIn;
                        double QuantityOut;
                        string FromStockName;
                        string ToStockName;
                        string AccountName;
                        double CostPieceLocal;
                        DateTime InsDateTime = DateTime.Now;
                        String InsUserID = userId;
                        double Last_SumTotalCostLocal = 0;
                        double Last_SumQuantityInputOutput = 0;
                        double Last_AverageCost = 0;
                        double AverageCost = 0;
                        double SumTotalCostLocal = 0;
                        double SumQuantityInputOutput = 0;
                        double OldQuantityDifference = 0;
                        double OldTotalDifference = 0;
                        double PreviousSumTotalCostLocal = 0;
                        double PreviousSumQuantityInputOutput = 0;
                        _unitOfWork.NativeSql.DeleteSt_TempCalculateAverageCost(InsUserID);
                        _unitOfWork.Complete();

                        DateTime PreviousBalanceDate = Obj.FromDate.AddDays(-1);
                        FunctionUnit ObjFunction = new FunctionUnit();
                        var GetPreviousAverageCost = ObjFunction.GetAvgCostByStock(UserInfo.fCompanyId, Obj.ItemCode, Obj.StockCode, PreviousBalanceDate);
                        PreviousSumTotalCostLocal = GetPreviousAverageCost.SumTotalCostLocal;
                        PreviousSumQuantityInputOutput = GetPreviousAverageCost.SumQuantityInputOutput;

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            ItemCode = (dt.Rows[i]["ItemCode"].ToString());
                            VoucherNumber = (dt.Rows[i]["VoucherNumber"].ToString());
                            VoucherDate = DateTime.Parse(dt.Rows[i]["VoucherDate"].ToString());
                            TransactionKindNo = int.Parse(dt.Rows[i]["TransactionKindNo"].ToString());
                            StockCode = (dt.Rows[i]["StockCode"].ToString());
                            TotalCostLocal = double.Parse(dt.Rows[i]["TotalCostLocal"].ToString());
                            QuantityInputOutput = double.Parse(dt.Rows[i]["QuantityInputOutput"].ToString());
                            PricePieceLocalAfterDiscount = double.Parse(dt.Rows[i]["PricePieceLocalAfterDiscount"].ToString());
                            TotalLocalAfterDiscount = double.Parse(dt.Rows[i]["TotalLocalAfterDiscount"].ToString());
                            QuantityIn = double.Parse(dt.Rows[i]["QuantityIn"].ToString());
                            QuantityOut = double.Parse(dt.Rows[i]["QuantityOut"].ToString());
                            TransactionKindName = (dt.Rows[i]["TransactionKindName"].ToString());
                            StockName = (dt.Rows[i]["StockName"].ToString());
                            Remark = (dt.Rows[i]["Remark"].ToString());
                            FromStockName = (dt.Rows[i]["FromStockName"].ToString());
                            ToStockName = (dt.Rows[i]["ToStockName"].ToString());
                            AccountName = (dt.Rows[i]["AccountName"].ToString());
                            CostPieceLocal = double.Parse(dt.Rows[i]["CostPieceLocal"].ToString());
                            if (i == 0)
                            {
                                Last_SumTotalCostLocal = TotalCostLocal + PreviousSumTotalCostLocal;
                                Last_SumQuantityInputOutput = QuantityInputOutput + PreviousSumQuantityInputOutput;
                                if (QuantityInputOutput > 0)
                                {
                                    Last_AverageCost = (TotalCostLocal / QuantityInputOutput);
                                }
                                else if (QuantityInputOutput <= 0)
                                {
                                    Last_AverageCost = 0;
                                }
                                AverageCost = Last_AverageCost;
                                SumTotalCostLocal = Last_SumTotalCostLocal  ;
                                SumQuantityInputOutput = Last_SumQuantityInputOutput  ;
                            }
                            else
                            {
                                if (QuantityInputOutput > 0)
                                {
                                    if (Last_SumQuantityInputOutput > 0)
                                    {
                                        Last_SumQuantityInputOutput = Last_SumQuantityInputOutput + QuantityInputOutput;
                                        Last_SumTotalCostLocal = Last_SumTotalCostLocal + TotalCostLocal;
                                        if (Last_SumTotalCostLocal == 0)
                                        {
                                            Last_AverageCost = (TotalCostLocal / Last_SumQuantityInputOutput);
                                        }
                                        else
                                        {
                                            Last_AverageCost = (Last_SumTotalCostLocal / Last_SumQuantityInputOutput);
                                        }
                                        if (Last_AverageCost < 0)
                                        {
                                            Last_AverageCost = Last_AverageCost * -1;
                                        }
                                        AverageCost = Last_AverageCost;
                                        SumTotalCostLocal = Last_SumTotalCostLocal;
                                        SumQuantityInputOutput = Last_SumQuantityInputOutput;
                                    }
                                    else if (Last_SumQuantityInputOutput < 0)
                                    {
                                        OldQuantityDifference = Last_SumQuantityInputOutput + QuantityInputOutput;
                                        if (OldQuantityDifference > 0)
                                        {
                                            OldTotalDifference = Math.Abs(Last_SumQuantityInputOutput) * (CostPieceLocal - Last_AverageCost);
                                            Last_SumQuantityInputOutput = Last_SumQuantityInputOutput + QuantityInputOutput;
                                            Last_SumTotalCostLocal = Last_SumTotalCostLocal + TotalCostLocal;
                                            Last_AverageCost = (OldTotalDifference / Math.Abs(Last_SumQuantityInputOutput)) + CostPieceLocal;
                                            if (Last_AverageCost < 0)
                                            {
                                                Last_AverageCost = Last_AverageCost * -1;
                                            }
                                            AverageCost = Last_AverageCost;
                                            SumTotalCostLocal = Last_SumTotalCostLocal;
                                            SumQuantityInputOutput = Last_SumQuantityInputOutput;
                                        }
                                        else if (OldQuantityDifference < 0)
                                        {
                                            OldTotalDifference = QuantityInputOutput * (CostPieceLocal - Last_AverageCost);
                                            Last_SumQuantityInputOutput = Last_SumQuantityInputOutput + QuantityInputOutput;
                                            Last_SumTotalCostLocal = Last_SumTotalCostLocal + TotalCostLocal;
                                            Last_AverageCost = (OldTotalDifference / Math.Abs(Last_SumQuantityInputOutput)) + CostPieceLocal;
                                            if (Last_AverageCost < 0)
                                            {
                                                Last_AverageCost = Last_AverageCost * -1;
                                            }
                                            AverageCost = Last_AverageCost;
                                            SumTotalCostLocal = Last_SumTotalCostLocal;
                                            SumQuantityInputOutput = Last_SumQuantityInputOutput;
                                        }
                                        else if (OldQuantityDifference == 0)
                                        {
                                            Last_SumQuantityInputOutput = Last_SumQuantityInputOutput + QuantityInputOutput;
                                            Last_SumTotalCostLocal = Last_SumTotalCostLocal + TotalCostLocal;
                                            Last_AverageCost = CostPieceLocal;
                                            if (Last_AverageCost < 0)
                                            {
                                                Last_AverageCost = Last_AverageCost * -1;
                                            }
                                            AverageCost = Last_AverageCost;
                                            SumTotalCostLocal = Last_SumTotalCostLocal;
                                            SumQuantityInputOutput = Last_SumQuantityInputOutput;
                                        }
                                    }
                                    else if (Last_SumQuantityInputOutput == 0)
                                    {
                                        Last_SumQuantityInputOutput = Last_SumQuantityInputOutput + QuantityInputOutput;
                                        Last_SumTotalCostLocal = Last_SumTotalCostLocal + TotalCostLocal;
                                        Last_AverageCost = CostPieceLocal;
                                        if (Last_AverageCost < 0)
                                        {
                                            Last_AverageCost = Last_AverageCost * -1;
                                        }
                                        AverageCost = Last_AverageCost;
                                        SumTotalCostLocal = Last_SumTotalCostLocal;
                                        SumQuantityInputOutput = Last_SumQuantityInputOutput;
                                    }
                                }
                                else if (QuantityInputOutput < 0)
                                {
                                    TotalCostLocal = Last_AverageCost * QuantityInputOutput;
                                    Last_SumTotalCostLocal = Last_SumTotalCostLocal + TotalCostLocal;
                                    Last_SumQuantityInputOutput = Last_SumQuantityInputOutput + QuantityInputOutput;
                                    if (Last_AverageCost < 0)
                                    {
                                        Last_AverageCost = Last_AverageCost * -1;
                                    }
                                    AverageCost = Last_AverageCost;
                                    SumTotalCostLocal = Last_SumTotalCostLocal;
                                    SumQuantityInputOutput = Last_SumQuantityInputOutput;
                                    CostPieceLocal = Last_AverageCost;
                                }
                            }
                            _unitOfWork.NativeSql.SaveSt_TempCalculateAverageCost(ItemCode, VoucherDate, VoucherNumber, TransactionKindNo, StockCode, TotalCostLocal,
                                    QuantityInputOutput, SumTotalCostLocal, SumQuantityInputOutput, CostPieceLocal, AverageCost, InsUserID, InsDateTime, PricePieceLocalAfterDiscount,
                                    TotalLocalAfterDiscount, QuantityIn, QuantityOut, TransactionKindName, StockName, Remark, FromStockName, ToStockName, AccountName);

                            _unitOfWork.Complete();
                        }
                    }
                    else
                    {
                        var GetAverageCost = _unitOfWork.NativeSql.GetAverageCost(UserInfo.fCompanyId, Obj.ItemCode,Obj.ToDate);
                        DataTable dt = FunctionUnit.LINQResultToDataTable(GetAverageCost);
                        string ItemCode;
                        string VoucherNumber;
                        DateTime VoucherDate;
                        int TransactionKindNo;
                        string StockCode;
                        double PricePieceLocalAfterDiscount;
                        double TotalLocalAfterDiscount;
                        double TotalCostLocal;
                        double QuantityInputOutput;
                        string TransactionKindName;
                        string StockName;
                        string Remark;
                        double QuantityIn;
                        double QuantityOut;
                        string FromStockName;
                        string ToStockName;
                        string AccountName;
                        double CostPieceLocal;
                        DateTime InsDateTime = DateTime.Now;
                        String InsUserID = userId;
                        double Last_SumTotalCostLocal = 0;
                        double Last_SumQuantityInputOutput = 0;
                        double Last_AverageCost = 0;
                        double AverageCost = 0;
                        double SumTotalCostLocal = 0;
                        double SumQuantityInputOutput = 0;
                        double OldQuantityDifference = 0;
                        double OldTotalDifference = 0;
                        double PreviousSumTotalCostLocal = 0;
                        double PreviousSumQuantityInputOutput = 0;
                        _unitOfWork.NativeSql.DeleteSt_TempCalculateAverageCost(InsUserID);
                        _unitOfWork.Complete();

                        DateTime PreviousBalanceDate = Obj.FromDate.AddDays(-1);
                        FunctionUnit ObjFunction = new FunctionUnit();
                        var GetPreviousAverageCost = ObjFunction.GetAvgCost(UserInfo.fCompanyId, Obj.ItemCode, PreviousBalanceDate);
                        PreviousSumTotalCostLocal = GetPreviousAverageCost.SumTotalCostLocal;
                        PreviousSumQuantityInputOutput = GetPreviousAverageCost.SumQuantityInputOutput;

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            ItemCode = (dt.Rows[i]["ItemCode"].ToString());
                            VoucherNumber = (dt.Rows[i]["VoucherNumber"].ToString());
                            VoucherDate = DateTime.Parse(dt.Rows[i]["VoucherDate"].ToString());
                            TransactionKindNo = int.Parse(dt.Rows[i]["TransactionKindNo"].ToString());
                            StockCode = (dt.Rows[i]["StockCode"].ToString());
                            TotalCostLocal = double.Parse(dt.Rows[i]["TotalCostLocal"].ToString());
                            QuantityInputOutput = double.Parse(dt.Rows[i]["QuantityInputOutput"].ToString());
                            PricePieceLocalAfterDiscount = double.Parse(dt.Rows[i]["PricePieceLocalAfterDiscount"].ToString());
                            TotalLocalAfterDiscount = double.Parse(dt.Rows[i]["TotalLocalAfterDiscount"].ToString());
                            QuantityIn = double.Parse(dt.Rows[i]["QuantityIn"].ToString());
                            QuantityOut = double.Parse(dt.Rows[i]["QuantityOut"].ToString());
                            TransactionKindName = (dt.Rows[i]["TransactionKindName"].ToString());
                            StockName = (dt.Rows[i]["StockName"].ToString());
                            Remark = (dt.Rows[i]["Remark"].ToString());
                            FromStockName = (dt.Rows[i]["FromStockName"].ToString());
                            ToStockName = (dt.Rows[i]["ToStockName"].ToString());
                            AccountName = (dt.Rows[i]["AccountName"].ToString());
                            CostPieceLocal = double.Parse(dt.Rows[i]["CostPieceLocal"].ToString());
                            if (i == 0)
                            {
                                Last_SumTotalCostLocal = TotalCostLocal + PreviousSumTotalCostLocal;
                                Last_SumQuantityInputOutput = QuantityInputOutput + PreviousSumQuantityInputOutput;
                                if (QuantityInputOutput > 0)
                                {
                                    Last_AverageCost = (TotalCostLocal / QuantityInputOutput);
                                }
                                else if (QuantityInputOutput <= 0)
                                {
                                    Last_AverageCost = 0;
                                }
                                AverageCost = Last_AverageCost;
                                SumTotalCostLocal = Last_SumTotalCostLocal;
                                SumQuantityInputOutput = Last_SumQuantityInputOutput;
                            }
                            else
                            {
                                if (QuantityInputOutput > 0)
                                {
                                    if (Last_SumQuantityInputOutput > 0)
                                    {
                                        Last_SumQuantityInputOutput = Last_SumQuantityInputOutput + QuantityInputOutput;
                                        Last_SumTotalCostLocal = Last_SumTotalCostLocal + TotalCostLocal;
                                        if (Last_SumTotalCostLocal == 0)
                                        {
                                            Last_AverageCost = (TotalCostLocal / Last_SumQuantityInputOutput);
                                        }
                                        else
                                        {
                                            Last_AverageCost = (Last_SumTotalCostLocal / Last_SumQuantityInputOutput);
                                        }
                                        if (Last_AverageCost < 0)
                                        {
                                            Last_AverageCost = Last_AverageCost * -1;
                                        }
                                        AverageCost = Last_AverageCost;
                                        SumTotalCostLocal = Last_SumTotalCostLocal;
                                        SumQuantityInputOutput = Last_SumQuantityInputOutput;
                                    }
                                    else if (Last_SumQuantityInputOutput < 0)
                                    {
                                        OldQuantityDifference = Last_SumQuantityInputOutput + QuantityInputOutput;
                                        if (OldQuantityDifference > 0)
                                        {
                                            OldTotalDifference = Math.Abs(Last_SumQuantityInputOutput) * (CostPieceLocal - Last_AverageCost);
                                            Last_SumQuantityInputOutput = Last_SumQuantityInputOutput + QuantityInputOutput;
                                            Last_SumTotalCostLocal = Last_SumTotalCostLocal + TotalCostLocal;
                                            Last_AverageCost = (OldTotalDifference / Math.Abs(Last_SumQuantityInputOutput)) + CostPieceLocal;
                                            if (Last_AverageCost < 0)
                                            {
                                                Last_AverageCost = Last_AverageCost * -1;
                                            }
                                            AverageCost = Last_AverageCost;
                                            SumTotalCostLocal = Last_SumTotalCostLocal;
                                            SumQuantityInputOutput = Last_SumQuantityInputOutput;
                                        }
                                        else if (OldQuantityDifference < 0)
                                        {
                                            OldTotalDifference = QuantityInputOutput * (CostPieceLocal - Last_AverageCost);
                                            Last_SumQuantityInputOutput = Last_SumQuantityInputOutput + QuantityInputOutput;
                                            Last_SumTotalCostLocal = Last_SumTotalCostLocal + TotalCostLocal;
                                            Last_AverageCost = (OldTotalDifference / Math.Abs(Last_SumQuantityInputOutput)) + CostPieceLocal;
                                            if (Last_AverageCost < 0)
                                            {
                                                Last_AverageCost = Last_AverageCost * -1;
                                            }
                                            AverageCost = Last_AverageCost;
                                            SumTotalCostLocal = Last_SumTotalCostLocal;
                                            SumQuantityInputOutput = Last_SumQuantityInputOutput;
                                        }
                                        else if (OldQuantityDifference == 0)
                                        {
                                            Last_SumQuantityInputOutput = Last_SumQuantityInputOutput + QuantityInputOutput;
                                            Last_SumTotalCostLocal = Last_SumTotalCostLocal + TotalCostLocal;
                                            Last_AverageCost = CostPieceLocal;
                                            if (Last_AverageCost < 0)
                                            {
                                                Last_AverageCost = Last_AverageCost * -1;
                                            }
                                            AverageCost = Last_AverageCost;
                                            SumTotalCostLocal = Last_SumTotalCostLocal;
                                            SumQuantityInputOutput = Last_SumQuantityInputOutput;
                                        }
                                    }
                                    else if (Last_SumQuantityInputOutput == 0)
                                    {
                                        Last_SumQuantityInputOutput = Last_SumQuantityInputOutput + QuantityInputOutput;
                                        Last_SumTotalCostLocal = Last_SumTotalCostLocal + TotalCostLocal;
                                        Last_AverageCost = CostPieceLocal;
                                        if (Last_AverageCost < 0)
                                        {
                                            Last_AverageCost = Last_AverageCost * -1;
                                        }
                                        AverageCost = Last_AverageCost;
                                        SumTotalCostLocal = Last_SumTotalCostLocal;
                                        SumQuantityInputOutput = Last_SumQuantityInputOutput;
                                    }
                                }
                                else if (QuantityInputOutput < 0)
                                {
                                    TotalCostLocal = Last_AverageCost * QuantityInputOutput;
                                    Last_SumTotalCostLocal = Last_SumTotalCostLocal + TotalCostLocal;
                                    Last_SumQuantityInputOutput = Last_SumQuantityInputOutput + QuantityInputOutput;
                                    if (Last_AverageCost < 0)
                                    {
                                        Last_AverageCost = Last_AverageCost * -1;
                                    }
                                    AverageCost = Last_AverageCost;
                                    SumTotalCostLocal = Last_SumTotalCostLocal;
                                    SumQuantityInputOutput = Last_SumQuantityInputOutput;
                                    CostPieceLocal = Last_AverageCost;
                                }
                            }
                            _unitOfWork.NativeSql.SaveSt_TempCalculateAverageCost(ItemCode, VoucherDate, VoucherNumber, TransactionKindNo, StockCode, TotalCostLocal,
                                    QuantityInputOutput, SumTotalCostLocal, SumQuantityInputOutput, CostPieceLocal, AverageCost, InsUserID, InsDateTime, PricePieceLocalAfterDiscount,
                                    TotalLocalAfterDiscount, QuantityIn, QuantityOut, TransactionKindName, StockName, Remark, FromStockName, ToStockName, AccountName);

                            _unitOfWork.Complete();
                        }
                    }
                    try
                    {
                        St_ItemTransactionReportWithTotalVM LastObj = new St_ItemTransactionReportWithTotalVM();
                        int year = (DateTime.Now.Year) - 1;
                        DateTime OpeningDate = new DateTime(year, 12, 31);
                        if (!String.IsNullOrEmpty(Obj.StockCode))
                        {
                            FunctionUnit ObjFunction = new FunctionUnit();
                            var GetLastAverageCost = ObjFunction.GetAvgCostByStock(UserInfo.fCompanyId, Obj.ItemCode, Obj.StockCode, Obj.ToDate);
                            LastObj.LastAverageCost = GetLastAverageCost.AverageCost;
                            LastObj.EndOfPeriodBalance = GetLastAverageCost.SumQuantityInputOutput;
                            var GetStartAverageCost = ObjFunction.GetAvgCostByStock(UserInfo.fCompanyId, Obj.ItemCode, Obj.StockCode, Obj.FromDate);
                            LastObj.StartOfPeriodBalance = GetStartAverageCost.SumQuantityInputOutput;
                            var GetOpeningAverageCost = ObjFunction.GetAvgCostByStock(UserInfo.fCompanyId, Obj.ItemCode, Obj.StockCode, OpeningDate);
                            LastObj.OpeningBalance = GetOpeningAverageCost.SumQuantityInputOutput;


                        }
                        else
                        {
                            FunctionUnit ObjFunction = new FunctionUnit();
                            var GetLastAverageCost = ObjFunction.GetAvgCost(UserInfo.fCompanyId, Obj.ItemCode, Obj.ToDate);
                            LastObj.LastAverageCost = GetLastAverageCost.AverageCost;
                            LastObj.EndOfPeriodBalance = GetLastAverageCost.SumQuantityInputOutput;
                            var GetStartAverageCost = ObjFunction.GetAvgCost(UserInfo.fCompanyId, Obj.ItemCode, Obj.FromDate);
                            LastObj.StartOfPeriodBalance = GetStartAverageCost.SumQuantityInputOutput;
                            var GetOpeningAverageCost = ObjFunction.GetAvgCost(UserInfo.fCompanyId, Obj.ItemCode, OpeningDate);
                            LastObj.OpeningBalance = GetOpeningAverageCost.SumQuantityInputOutput;

                        }
                        var ItemTransactions = _unitOfWork.NativeSql.GetTempAverageCost(userId, Obj.FromDate, Obj.ToDate);
                        LastObj.St_ItemTransactionReportVM = ItemTransactions;
                        if (ItemTransactions == null)
                        {
                            St_ItemTransactionReportWithTotalVM EmptyObj = new St_ItemTransactionReportWithTotalVM();
                            EmptyObj.St_ItemTransactionReportVM = new List<St_ItemTransactionReportVM>();
                            EmptyObj.LastAverageCost = 0;
                            EmptyObj.EndOfPeriodBalance = 0;
                            EmptyObj.StartOfPeriodBalance = 0;
                            EmptyObj.OpeningBalance = 0;
                            return Json(EmptyObj, JsonRequestBehavior.AllowGet);
                        }
                        return Json(LastObj, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception ex)
                    {
                        St_ItemTransactionReportWithTotalVM EmptyObj = new St_ItemTransactionReportWithTotalVM();
                        EmptyObj.St_ItemTransactionReportVM = new List<St_ItemTransactionReportVM>();
                        EmptyObj.LastAverageCost = 0;
                        EmptyObj.EndOfPeriodBalance = 0;
                        EmptyObj.StartOfPeriodBalance = 0;
                        EmptyObj.OpeningBalance = 0;
                        ViewBag.Error = ex.Message.ToString();
                        return Json(EmptyObj, JsonRequestBehavior.AllowGet);
                    }
                }
                
            }
            else
            {
                St_ItemTransactionReportWithTotalVM EmptyObj = new St_ItemTransactionReportWithTotalVM();
                EmptyObj.St_ItemTransactionReportVM = new List<St_ItemTransactionReportVM>();
                EmptyObj.LastAverageCost = 0;
                EmptyObj.EndOfPeriodBalance = 0;
                EmptyObj.StartOfPeriodBalance = 0;
                EmptyObj.OpeningBalance = 0;
                return Json(EmptyObj, JsonRequestBehavior.AllowGet);
            }

        }
    }
}