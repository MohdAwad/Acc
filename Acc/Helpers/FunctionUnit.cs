using Acc.Persistence;
using Acc.Repositories;
using Acc.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;

namespace Acc.Helpers
{
    public class FunctionUnit
    {
        private readonly IUnitOfWork _unitOfWork;
        public FunctionUnit()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        public static TimeZoneInfo Jordan_Time_Zone = TimeZoneInfo.FindSystemTimeZoneById("Jordan Standard Time");

        public  St_AvaregCostVM GetAvgCostByStock( int CompanyID,string sItemCode,string sStockCode,DateTime ToDate)
        {
            St_AvaregCostVM ObjAvg= new St_AvaregCostVM();
              //var userId = User.Identity.GetUserId();
              //var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var GetAverageCostByStockCode = _unitOfWork.NativeSql.GetAverageCostByStockCode(CompanyID, sItemCode, sStockCode, ToDate);
            DataTable dt = FunctionUnit.LINQResultToDataTable(GetAverageCostByStockCode);

            string ItemCode;
            string VoucherNumber;
            DateTime VoucherDate;
            int TransactionKindNo;
            string StockCode;
            double TotalCostLocal;
            double QuantityInputOutput;
            double CostPieceLocal;
            //DateTime InsDateTime = DateTime.Now;
            //String InsUserID = userId;
            double Last_SumTotalCostLocal = 0;
            double Last_SumQuantityInputOutput = 0;
            double Last_AverageCost = 0;
            double AverageCost = 0;
            double SumTotalCostLocal = 0;
            double SumQuantityInputOutput = 0;
            double OldQuantityDifference = 0;
            double OldTotalDifference = 0;
            //_unitOfWork.NativeSql.DeleteSt_TempCalculateAverageCost(InsUserID);
            //_unitOfWork.Complete();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ItemCode = (dt.Rows[i]["ItemCode"].ToString());
                VoucherNumber = (dt.Rows[i]["VoucherNumber"].ToString());
                VoucherDate = DateTime.Parse(dt.Rows[i]["VoucherDate"].ToString());
                TransactionKindNo = int.Parse(dt.Rows[i]["TransactionKindNo"].ToString());
                StockCode = (dt.Rows[i]["StockCode"].ToString());
                TotalCostLocal = double.Parse(dt.Rows[i]["TotalCostLocal"].ToString());
                QuantityInputOutput = double.Parse(dt.Rows[i]["QuantityInputOutput"].ToString());
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
                if (i == dt.Rows.Count - 1)
                {
                    ObjAvg.ItemCode = ItemCode;
                    ObjAvg.VoucherDate = VoucherDate;
                    ObjAvg.VoucherNumber = VoucherNumber;
                    ObjAvg.TransactionKindNo = TransactionKindNo;
                    ObjAvg.StockCode = StockCode;
                    ObjAvg.TotalCostLocal = TotalCostLocal;
                    ObjAvg.QuantityInputOutput = QuantityInputOutput;
                    ObjAvg.SumTotalCostLocal = SumTotalCostLocal;
                    ObjAvg.SumQuantityInputOutput = SumQuantityInputOutput;
                    ObjAvg.CostPieceLocal = CostPieceLocal;
                    ObjAvg.AverageCost = AverageCost;

                }
                //_unitOfWork.NativeSql.SaveSt_TempCalculateAverageCost(ItemCode, VoucherDate, VoucherNumber, TransactionKindNo, StockCode, TotalCostLocal,
                //        QuantityInputOutput, SumTotalCostLocal, SumQuantityInputOutput, CostPieceLocal, AverageCost, InsUserID, InsDateTime);
                //_unitOfWork.Complete();
            }
            return ObjAvg;


        }
        public St_AvaregCostVM GetAvgCost(int CompanyID, string sItemCode, DateTime ToDate)
        {
            St_AvaregCostVM ObjAvg = new St_AvaregCostVM();
            //var userId = User.Identity.GetUserId();
            //var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var GetAverageCostByStockCode = _unitOfWork.NativeSql.GetAverageCost(CompanyID, sItemCode, ToDate);
            DataTable dt = FunctionUnit.LINQResultToDataTable(GetAverageCostByStockCode);

            string ItemCode;
            string VoucherNumber;
            DateTime VoucherDate;
            int TransactionKindNo;
            string StockCode;
            double TotalCostLocal;
            double QuantityInputOutput;
            double CostPieceLocal;
            //DateTime InsDateTime = DateTime.Now;
            //String InsUserID = userId;
            double Last_SumTotalCostLocal = 0;
            double Last_SumQuantityInputOutput = 0;
            double Last_AverageCost = 0;
            double AverageCost = 0;
            double SumTotalCostLocal = 0;
            double SumQuantityInputOutput = 0;
            double OldQuantityDifference = 0;
            double OldTotalDifference = 0;
            //_unitOfWork.NativeSql.DeleteSt_TempCalculateAverageCost(InsUserID);
            //_unitOfWork.Complete();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ItemCode = (dt.Rows[i]["ItemCode"].ToString());
                VoucherNumber = (dt.Rows[i]["VoucherNumber"].ToString());
                VoucherDate = DateTime.Parse(dt.Rows[i]["VoucherDate"].ToString());
                TransactionKindNo = int.Parse(dt.Rows[i]["TransactionKindNo"].ToString());
                StockCode = (dt.Rows[i]["StockCode"].ToString());
                TotalCostLocal = double.Parse(dt.Rows[i]["TotalCostLocal"].ToString());
                QuantityInputOutput = double.Parse(dt.Rows[i]["QuantityInputOutput"].ToString());
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
                if (i == dt.Rows.Count - 1)
                {
                    ObjAvg.ItemCode = ItemCode;
                    ObjAvg.VoucherDate = VoucherDate;
                    ObjAvg.VoucherNumber = VoucherNumber;
                    ObjAvg.TransactionKindNo = TransactionKindNo;
                    ObjAvg.StockCode = StockCode;
                    ObjAvg.TotalCostLocal = TotalCostLocal;
                    ObjAvg.QuantityInputOutput = QuantityInputOutput;
                    ObjAvg.SumTotalCostLocal = SumTotalCostLocal;
                    ObjAvg.SumQuantityInputOutput = SumQuantityInputOutput;
                    ObjAvg.CostPieceLocal = CostPieceLocal;
                    ObjAvg.AverageCost = AverageCost;

                }
                //_unitOfWork.NativeSql.SaveSt_TempCalculateAverageCost(ItemCode, VoucherDate, VoucherNumber, TransactionKindNo, StockCode, TotalCostLocal,
                //        QuantityInputOutput, SumTotalCostLocal, SumQuantityInputOutput, CostPieceLocal, AverageCost, InsUserID, InsDateTime);
                //_unitOfWork.Complete();
            }
            return ObjAvg;


        }
        public St_AvaregCostHVM GetAvgCostH(int CompanyID, string sItemCode, DateTime ToDate)
        {
            St_AvaregCostHVM ObjAvg = new St_AvaregCostHVM();
            //var userId = User.Identity.GetUserId();
            //var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var GetAverageCostByStockCode = _unitOfWork.NativeSql.GetAverageCostH(CompanyID, sItemCode, ToDate);
            DataTable dt = FunctionUnit.LINQResultToDataTable(GetAverageCostByStockCode);

            string ItemCode;
            string VoucherNumber;
            DateTime VoucherDate;
            int TransactionKindNo;
            string StockCode;
            string BranchCode;
            double TotalCostLocal;
            double QuantityInputOutput;
            double CostPieceLocal;
            //DateTime InsDateTime = DateTime.Now;
            //String InsUserID = userId;
            double Last_SumTotalCostLocal = 0;
            double Last_SumQuantityInputOutput = 0;
            double Last_AverageCost = 0;
            double AverageCost = 0;
            double SumTotalCostLocal = 0;
            double SumQuantityInputOutput = 0;
            double OldQuantityDifference = 0;
            double OldTotalDifference = 0;
            //_unitOfWork.NativeSql.DeleteSt_TempCalculateAverageCost(InsUserID);
            //_unitOfWork.Complete();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ItemCode = (dt.Rows[i]["ItemCode"].ToString());
                VoucherNumber = (dt.Rows[i]["VoucherNumber"].ToString());
                VoucherDate = DateTime.Parse(dt.Rows[i]["VoucherDate"].ToString());
                TransactionKindNo = int.Parse(dt.Rows[i]["TransactionKindNo"].ToString());
                BranchCode = (dt.Rows[i]["BranchCode"].ToString());
                StockCode = (dt.Rows[i]["StockCode"].ToString());
                TotalCostLocal = double.Parse(dt.Rows[i]["TotalCostLocal"].ToString());
                QuantityInputOutput = double.Parse(dt.Rows[i]["QuantityInputOutput"].ToString());
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
                if (i == dt.Rows.Count - 1)
                {
                    ObjAvg.ItemCode = ItemCode;

                }
                //_unitOfWork.NativeSql.SaveSt_TempCalculateAverageCost(ItemCode, VoucherDate, VoucherNumber, TransactionKindNo, StockCode,BranchCode, TotalCostLocal,
                //        QuantityInputOutput, SumTotalCostLocal, SumQuantityInputOutput, CostPieceLocal, AverageCost, InsUserID, InsDateTime);
                //_unitOfWork.Complete();
            }
            return ObjAvg;


        }
        public static DataTable LINQResultToDataTable<T>(IEnumerable<T> Linqlist)
        {
            DataTable dt = new DataTable();


            PropertyInfo[] columns = null;

            if (Linqlist == null) return dt;

            foreach (T Record in Linqlist)
            {

                if (columns == null)
                {
                    columns = ((Type)Record.GetType()).GetProperties();
                    foreach (PropertyInfo GetProperty in columns)
                    {
                        Type colType = GetProperty.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                        == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dt.Columns.Add(new DataColumn(GetProperty.Name, colType));
                    }
                }

                DataRow dr = dt.NewRow();

                foreach (PropertyInfo pinfo in columns)
                {
                    dr[pinfo.Name] = pinfo.GetValue(Record, null) == null ? DBNull.Value : pinfo.GetValue
                    (Record, null);
                }

                dt.Rows.Add(dr);
            }
            return dt;
        }
        public static string GetLevelChart(string Chart, int Level)
        {
            string ChartZero = "";
            string[] words = Chart.Split('-');
            int CurrLevel = 1;
            foreach (string word in words)
            {
                string Zero = "";
                if (Level == CurrLevel)
                {
                    for (int i = 1; i <= int.Parse(word); i++)
                    {
                        Zero = Zero + "9";

                    }
                    return Zero;
                }
                else
                {
                    CurrLevel++;
                }






            }

            return ChartZero;

        }
        public static string ConvertChartToZero(string Chart)
        {
            string ChartZero = "";
            string[] words = Chart.Split('-');
            foreach (string word in words)
            {
                string Zero = "";
                for (int i = 1; i <= int.Parse(word); i++)
                {
                    Zero = Zero + "9";

                }
                if (String.IsNullOrEmpty(ChartZero))
                {
                    ChartZero = ChartZero + Zero;

                }
                else
                {
                    ChartZero = ChartZero + "-" + Zero;

                }


            }

            return ChartZero;

        }
        public static string RemoveAccountDash(string Chart)
        {
            if (String.IsNullOrEmpty(Chart))
                return "";
            string ChartZero = "";
            string[] words = Chart.Split('-');
            foreach (string word in words)
            {

                ChartZero = ChartZero + word;




            }

            return ChartZero;

        }
    }
}