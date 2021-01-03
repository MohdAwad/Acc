using Acc.Models;
using Acc.Persistence;
using Acc.Repositories;
using Acc.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Acc.Controllers
{
    [Authorize]
    public class AccDashBoardController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public AccDashBoardController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        // GET: DashBoard

        [HttpPost]
        public double GetTrialExpenseAnlysis(AccountLevelRepVM Obj)
        {
            try
            {

                var userId = User.Identity.GetUserId();
                string UserID = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
                var AccountData = _unitOfWork.NativeSql.GetChartOfAccountByLevel(UserInfo.fCompanyId);
                AccountData = AccountData.Where(m => m.AccountTypeID == 14 || m.AccountTypeID == 15 || m.AccountTypeID == 16).ToList();
                if (Obj.AccountTypeID != 0)
                {
                    AccountData = AccountData.Where(m => m.AccountTypeID == Obj.AccountTypeID).ToList();
                }

                int CurrYear = UserInfo.CurrYear;

                var Date = "01/01/" + CurrYear;
                var OpenDate = "31/12/" + (CurrYear - 1).ToString();

                string TempCostID = "0";
                int TempCostType = 0;

                if (Obj.ByCostCenter)
                {
                    TempCostID = Obj.CostCenterNumber;
                    if (Obj.Partofthenumber)
                    {
                        TempCostType = Obj.CostSearchType;
                    }


                }

                IEnumerable<TrialBalanceVM> TotData = new List<TrialBalanceVM>();
                if (Obj.Partofthenumber)
                {
                    if (Obj.CostSearchType == 1)
                    {
                        Obj.CostCenterNumber = Obj.CostCenterNumber + "%";
                    }
                    else if (Obj.CostSearchType == 2)
                    {
                        Obj.CostCenterNumber = "%" + Obj.CostCenterNumber;
                    }
                    else if (Obj.CostSearchType == 3)
                    {
                        Obj.CostCenterNumber = "%" + Obj.CostCenterNumber + "%";

                    }
                }

                var TranData = _unitOfWork.NativeSql.GetTransactionForTrial(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate, Obj.ByCostCenter, Obj.CostCenterNumber, Obj.Partofthenumber, Obj.CostSearchType);
                if (DateTime.Parse(Date) != Obj.FromDate)
                    TotData = _unitOfWork.NativeSql.GetTotCreditDebitForTrial(UserInfo.fCompanyId, DateTime.Parse(Date), Obj.FromDate, Obj.ByCostCenter, Obj.CostCenterNumber, Obj.Partofthenumber, Obj.CostSearchType);
              
                
                
                //----------------Get Open-------------//
                IEnumerable<TrialBalanceVM> TotDataOpen = new List<TrialBalanceVM>();
                TotDataOpen = _unitOfWork.NativeSql.GetTotCreditDebitForTrialOpen(UserInfo.fCompanyId, DateTime.Parse(OpenDate), DateTime.Parse(OpenDate));
                foreach (var D in TotDataOpen)
                {
                    TotData = TotData.Append(D);

                }
                //------------------------------------//



                double TotalDebit = 0;
                double TOTCredit = 0;
                double NetCredit = 0;
                double NetDebit = 0;
                double CreditBalance = 0;
                double DebitBalance = 0;
                double CreditTransAction = 0;
                double DebitTransAction = 0;

                List<TrialBalanceVM> TrialBalanceVMList = new List<TrialBalanceVM>();

                IEnumerable<ChartOfAccount> Accounts = new List<ChartOfAccount>();

                IEnumerable<ChartOfAccount> LessMainAccount = new List<ChartOfAccount>();
                if (!String.IsNullOrEmpty(Obj.AccNo))
                {

                    var MainAccount = AccountData.FirstOrDefault(m => m.AccountNumber == Obj.AccNo);
                    var MainChild = AccountData.Where(m => m.AccountFather == Obj.AccNo).OrderBy(m => m.AccountNumber).OrderBy(m => m.AccountLevel).ToList();

                    foreach (var Acc in MainChild)
                    {
                        TotalDebit = 0;
                        TOTCredit = 0;
                        NetCredit = 0;
                        NetDebit = 0;
                        CreditBalance = 0;
                        DebitBalance = 0;
                        CreditTransAction = 0;
                        DebitTransAction = 0;


                        if (AccountData.FirstOrDefault(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel) != null)//Check if Account Had Branches
                        {
                            var AllAccount = AccountData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel)
                                            .OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();//Get All Account Branches
                            int FatherLevel = Acc.AccountLevel;
                            foreach (var CurrAccLevel in AllAccount)
                            {
                                if (CurrAccLevel.AccountLevel == FatherLevel)
                                {
                                    break;
                                }
                                else
                                {

                                    var TransActionData = TranData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                    var BalanceData = TotData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                    foreach (var D in TransActionData)
                                    {
                                        CreditTransAction += D.CreditTransAction;
                                        DebitTransAction += D.DebitTransAction;
                                    }
                                    foreach (var Balance in BalanceData)
                                    {
                                        CreditBalance += Balance.CreditBalance;
                                        DebitBalance += Balance.DebitBalance;
                                    }



                                    TotalDebit = DebitTransAction + DebitBalance;
                                    TOTCredit = CreditTransAction + CreditBalance;




                                }
                            }




                        }
                        else
                        {
                            var TransActionData = TranData.Where(m => m.AccountNumber == Acc.AccountNumber);
                            var BalanceData = TotData.Where(m => m.AccountNumber == Acc.AccountNumber);
                            foreach (var D in TransActionData)
                            {
                                CreditTransAction += D.CreditTransAction;
                                DebitTransAction += D.DebitTransAction;
                            }
                            foreach (var Balance in BalanceData)
                            {
                                CreditBalance += Balance.CreditBalance;
                                DebitBalance += Balance.DebitBalance;
                            }



                            TotalDebit = DebitTransAction + DebitBalance;
                            TOTCredit = CreditTransAction + CreditBalance;
                        }
                        if (TotalDebit > TOTCredit)
                        {
                            NetDebit = TotalDebit - TOTCredit;
                        }
                        if (TotalDebit < TOTCredit)
                        {
                            NetCredit = TOTCredit - TotalDebit;
                        }

                        TrialBalanceVM trialBalance = new TrialBalanceVM();
                        trialBalance.AccountNumber = Acc.AccountNumber;
                        trialBalance.Name = Acc.ArabicName;

                        trialBalance.CreditBalance = CreditBalance;
                        trialBalance.CreditTransAction = CreditTransAction;
                        trialBalance.DebitBalance = DebitBalance;
                        trialBalance.DebitTransAction = DebitTransAction;
                        trialBalance.NetCredit = NetCredit;
                        trialBalance.NetDebit = NetDebit;
                        trialBalance.TempCostID = TempCostID;
                        trialBalance.TempCostType = TempCostType;

                        trialBalance.nCreditBalance = CreditBalance;
                        trialBalance.nCreditTransAction = CreditTransAction;
                        trialBalance.nDebitBalance = DebitBalance;
                        trialBalance.nDebitTransAction = DebitTransAction;
                        trialBalance.nNetCredit = NetCredit;
                        trialBalance.nNetDebit = NetDebit;


                        trialBalance.nCreditBalance = CreditBalance;
                        trialBalance.nCreditTransAction = CreditTransAction;
                        trialBalance.nDebitBalance = DebitBalance;
                        trialBalance.nDebitTransAction = DebitTransAction;
                        trialBalance.nNetCredit = NetCredit;
                        trialBalance.nNetDebit = NetDebit;
                     

                        if (!Obj.ShowZeroBalance)
                        {
                            if ((trialBalance.NetCredit - trialBalance.NetDebit) != 0)
                            {
                                TrialBalanceVMList.Add(trialBalance);
                            }
                        }
                        else
                        {
                            if ((trialBalance.NetCredit != 0) || (trialBalance.NetDebit != 0) || (TotalDebit != 0) || (NetCredit != 0))
                            {
                                TrialBalanceVMList.Add(trialBalance);
                            }


                        }
                    }


                }
                else

                if ((Obj.AccountLevelDropVMID > 0) && (!Obj.Detail))
                {


                    Accounts = AccountData.Where(m => m.AccountLevel == Obj.AccountLevelDropVMID).OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();

                    LessMainAccount = AccountData.Where(m => m.AccountLevel < Obj.AccountLevelDropVMID).OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();

                    IEnumerable<string> FaherAccount = LessMainAccount.Where(m => m.AccountFather != "0").Select(m => m.AccountFather);


                    foreach (var A in LessMainAccount)
                    {
                        if (AccountData.FirstOrDefault(m => m.AccountFather == A.AccountNumber) != null)
                        {
                            //do nothing
                        }
                        else

                        {
                            Accounts = Accounts.Append(A);
                        }

                    }
                    Accounts = Accounts.OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();
                    // var MainChild = AccountData.Where(m => m.AccountFather == MainAcc.AccountNumber).OrderBy(m => m.AccountNumber).OrderBy(m => m.AccountLevel).ToList();
                    foreach (var Acc in Accounts)
                    {
                        TotalDebit = 0;
                        TOTCredit = 0;
                        NetCredit = 0;
                        NetDebit = 0;
                        CreditBalance = 0;
                        DebitBalance = 0;
                        CreditTransAction = 0;
                        DebitTransAction = 0;


                        if (AccountData.FirstOrDefault(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel) != null)
                        {
                            var AllAccount = AccountData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel)
                                            .OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();
                            int FatherLevel = Acc.AccountLevel;

                            foreach (var CurrAccLevel in AllAccount)
                            {
                                if (CurrAccLevel.AccountLevel == FatherLevel)
                                {
                                    break;
                                }
                                else
                                {
                                    var TransActionData = TranData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                    var BalanceData = TotData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                    foreach (var D in TransActionData)
                                    {
                                        CreditTransAction += D.CreditTransAction;
                                        DebitTransAction += D.DebitTransAction;
                                    }
                                    foreach (var Balance in BalanceData)
                                    {
                                        CreditBalance += Balance.CreditBalance;
                                        DebitBalance += Balance.DebitBalance;
                                    }
                                    TotalDebit = DebitTransAction + DebitBalance;
                                    TOTCredit = CreditTransAction + CreditBalance;
                                }
                            }
                        }
                        else
                        {
                            var TransActionData = TranData.Where(m => m.AccountNumber == Acc.AccountNumber);
                            var BalanceData = TotData.Where(m => m.AccountNumber == Acc.AccountNumber);
                            foreach (var D in TransActionData)
                            {
                                CreditTransAction += D.CreditTransAction;
                                DebitTransAction += D.DebitTransAction;
                            }
                            foreach (var Balance in BalanceData)
                            {
                                CreditBalance += Balance.CreditBalance;
                                DebitBalance += Balance.DebitBalance;
                            }

                            TotalDebit = DebitTransAction + DebitBalance;
                            TOTCredit = CreditTransAction + CreditBalance;
                        }
                        if (TotalDebit > TOTCredit)
                        {
                            NetDebit = TotalDebit - TOTCredit;
                        }
                        if (TotalDebit < TOTCredit)
                        {
                            NetCredit = TOTCredit - TotalDebit;
                        }

                        TrialBalanceVM trialBalance = new TrialBalanceVM();
                        trialBalance.AccountNumber = Acc.AccountNumber;
                        trialBalance.Name = Acc.ArabicName;
                        trialBalance.Level = Acc.AccountLevel;
                        trialBalance.CreditBalance = CreditBalance;
                        trialBalance.CreditTransAction = CreditTransAction;
                        trialBalance.DebitBalance = DebitBalance;
                        trialBalance.DebitTransAction = DebitTransAction;
                        trialBalance.NetCredit = NetCredit;
                        trialBalance.NetDebit = NetDebit;

                        trialBalance.nCreditBalance = CreditBalance;
                        trialBalance.nCreditTransAction = CreditTransAction;
                        trialBalance.nDebitBalance = DebitBalance;
                        trialBalance.nDebitTransAction = DebitTransAction;
                        trialBalance.nNetCredit = NetCredit;
                        trialBalance.nNetDebit = NetDebit;
                        trialBalance.TempCostID = TempCostID;
                        trialBalance.TempCostType = TempCostType;
                        if (AccountData.FirstOrDefault(m => m.AccountFather == Acc.AccountNumber) != null)
                        {
                            trialBalance.IsMainAccount = 1;

                        }
                        else
                        {
                            trialBalance.IsMainAccount = 0;


                        }

                        if (!Obj.ShowZeroBalance)
                        {
                            if ((trialBalance.NetCredit - trialBalance.NetDebit) != 0)
                            {
                                TrialBalanceVMList.Add(trialBalance);
                            }
                        }
                        else
                        {
                            if ((trialBalance.NetCredit != 0) || (trialBalance.NetDebit != 0) || (TotalDebit != 0) || (NetCredit != 0))
                            {
                                TrialBalanceVMList.Add(trialBalance);
                            }


                        }
                    }

                }
                else if ((Obj.AccountLevelDropVMID > 0) && (Obj.Detail))
                {


                    Accounts = AccountData.Where(m => m.AccountLevel <= Obj.AccountLevelDropVMID).ToList().OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();

                    foreach (var Acc in Accounts)//Each Main Account
                    {
                        TotalDebit = 0;
                        TOTCredit = 0;
                        NetCredit = 0;
                        NetDebit = 0;
                        CreditBalance = 0;
                        DebitBalance = 0;
                        CreditTransAction = 0;
                        DebitTransAction = 0;

                        var MainAccount = AccountData.FirstOrDefault(m => m.AccountNumber == Acc.AccountNumber);
                        var MainChild = AccountData.Where(m => m.AccountFather == Acc.AccountNumber).OrderBy(m => m.AccountNumber).OrderBy(m => m.AccountLevel).ToList();

                        var TransActionData = TranData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber));
                        var BalanceData = TotData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber));
                        foreach (var D in TransActionData)
                        {
                            CreditTransAction += D.CreditTransAction;
                            DebitTransAction += D.DebitTransAction;
                        }
                        foreach (var Balance in BalanceData)
                        {
                            CreditBalance += Balance.CreditBalance;
                            DebitBalance += Balance.DebitBalance;
                        }



                        TotalDebit = DebitTransAction + DebitBalance;
                        TOTCredit = CreditTransAction + CreditBalance;

                        if (TotalDebit > TOTCredit)
                        {
                            NetDebit = TotalDebit - TOTCredit;
                        }
                        if (TotalDebit < TOTCredit)
                        {
                            NetCredit = TOTCredit - TotalDebit;
                        }

                        TrialBalanceVM trialBalance = new TrialBalanceVM();
                        trialBalance.AccountNumber = Acc.AccountNumber;
                        trialBalance.Name = Acc.ArabicName;

                        trialBalance.CreditBalance = CreditBalance;
                        trialBalance.CreditTransAction = CreditTransAction;
                        trialBalance.DebitBalance = DebitBalance;
                        trialBalance.DebitTransAction = DebitTransAction;
                        trialBalance.NetCredit = NetCredit;
                        trialBalance.NetDebit = NetDebit;
                        trialBalance.TempCostID = TempCostID;
                        trialBalance.TempCostType = TempCostType;
                        if ((AccountData.FirstOrDefault(m => m.AccountFather == Acc.AccountNumber) != null) && (Acc.AccountLevel < Obj.AccountLevelDropVMID))
                        {
                            trialBalance.IsMainAccount = 1;
                            trialBalance.MainAccount = "{*}";


                            trialBalance.nCreditBalance = 0;
                            trialBalance.nCreditTransAction = 0;
                            trialBalance.nDebitBalance = 0;
                            trialBalance.nDebitTransAction = 0;
                            trialBalance.nNetCredit = 0;
                            trialBalance.nNetDebit = 0;

                        }
                        else
                        {
                            trialBalance.IsMainAccount = 0;
                            trialBalance.MainAccount = "";
                            trialBalance.nCreditBalance = CreditBalance;
                            trialBalance.nCreditTransAction = CreditTransAction;
                            trialBalance.nDebitBalance = DebitBalance;
                            trialBalance.nDebitTransAction = DebitTransAction;
                            trialBalance.nNetCredit = NetCredit;
                            trialBalance.nNetDebit = NetDebit;


                        }
                        if (!Obj.ShowZeroBalance)
                        {
                            if ((trialBalance.NetCredit - trialBalance.NetDebit) != 0)
                            {
                                TrialBalanceVMList.Add(trialBalance);
                            }
                        }
                        else
                        {
                            if ((trialBalance.NetCredit != 0) || (trialBalance.NetDebit != 0) || (TotalDebit != 0) || (NetCredit != 0))
                            {
                                TrialBalanceVMList.Add(trialBalance);
                            }


                        }




                    }






                }


                var NetToalDebit = TrialBalanceVMList.Sum(m => m.NetDebit);
                var NetToalCredit = TrialBalanceVMList.Sum(m => m.NetCredit);
                var NetTotal = NetToalCredit - NetToalDebit;

                return NetTotal;


            }
            catch (Exception ex)
            {
                string err = ex.Message;

                List<TrialBalanceVM> TrialBalanceVMList = new List<TrialBalanceVM>();

                return 0;

            }


        }

        public IEnumerable<ServiceReportVM> GetServiceReport(ServiceReportVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllServiceReportVM = _unitOfWork.NativeSql.GetServiceReport(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                List<ServiceReportVM> AllData = new List<ServiceReportVM>();
                List<ServiceReportVM> Ch1 = new List<ServiceReportVM>();
                List<ServiceReportVM> Ch2 = new List<ServiceReportVM>();
                List<ServiceReportVM> Ch3 = new List<ServiceReportVM>();
                List<ServiceReportVM> Ch4 = new List<ServiceReportVM>();
                List<ServiceReportVM> Ch5 = new List<ServiceReportVM>();
                List<ServiceReportVM> Ch6 = new List<ServiceReportVM>();
                List<ServiceReportVM> Ch7 = new List<ServiceReportVM>();
                List<ServiceReportVM> Ch8 = new List<ServiceReportVM>();

                if (AllServiceReportVM == null)
                {
                    return  (new List<ServiceReportVM>() );
                }
                if (Obj.SaleID != 0)
                {
                    AllServiceReportVM = AllServiceReportVM.Where(m => m.SaleID == Obj.SaleID).ToList();
                }
                if (Obj.BillID != 0)
                {
                    AllServiceReportVM = AllServiceReportVM.Where(m => m.BillID == Obj.BillID).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.AccountNumber))
                {
                    AllServiceReportVM = AllServiceReportVM.Where(m => m.AccountNumber == Obj.AccountNumber).ToList();
                }
                if (Obj.SaleService)
                {
                    Ch1 = AllServiceReportVM.Where(m => m.TransactionKindNo == 10).ToList();
                }
                if (Obj.SaleMultiService)
                {
                    Ch2 = AllServiceReportVM.Where(m => m.TransactionKindNo == 11).ToList();
                }
                if (Obj.ReturnService)
                {
                    Ch3 = AllServiceReportVM.Where(m => m.TransactionKindNo == 19).ToList();
                }
                if (Obj.ReturnMultiService)
                {
                    Ch4 = AllServiceReportVM.Where(m => m.TransactionKindNo == 20).ToList();
                }
                if (Obj.PurchaseService)
                {
                    Ch5 = AllServiceReportVM.Where(m => m.TransactionKindNo == 12).ToList();
                }
                if (Obj.PurchaseMultiService)
                {
                    Ch6 = AllServiceReportVM.Where(m => m.TransactionKindNo == 13).ToList();
                }
                if (Obj.ReturnPurchaseService)
                {
                    Ch7 = AllServiceReportVM.Where(m => m.TransactionKindNo == 21).ToList();
                }
                if (Obj.ReturnPurchaseMultiService)
                {
                    Ch8 = AllServiceReportVM.Where(m => m.TransactionKindNo == 22).ToList();
                }
                if (Ch1.Count > 0)
                {
                    foreach (var c in Ch1)
                    {
                        AllData.Add(c);
                    }
                }
                if (Ch2.Count > 0)
                {
                    foreach (var c in Ch2)
                    {
                        AllData.Add(c);
                    }
                }
                if (Ch3.Count > 0)
                {
                    foreach (var c in Ch3)
                    {
                        AllData.Add(c);
                    }
                }
                if (Ch4.Count > 0)
                {
                    foreach (var c in Ch4)
                    {
                        AllData.Add(c);
                    }
                }
                if (Ch5.Count > 0)
                {
                    foreach (var c in Ch5)
                    {
                        AllData.Add(c);
                    }
                }
                if (Ch6.Count > 0)
                {
                    foreach (var c in Ch6)
                    {
                        AllData.Add(c);
                    }
                }
                if (Ch7.Count > 0)
                {
                    foreach (var c in Ch7)
                    {
                        AllData.Add(c);
                    }
                }
                if (Ch8.Count > 0)
                {
                    foreach (var c in Ch8)
                    {
                        AllData.Add(c);
                    }
                }
                if (Obj.SaleService == false && Obj.SaleMultiService == false && Obj.ReturnService == false && Obj.ReturnMultiService == false &&
                    Obj.PurchaseService == false && Obj.PurchaseMultiService == false && Obj.ReturnPurchaseService == false && Obj.ReturnPurchaseMultiService == false)
                {
                    return  (AllServiceReportVM );
                }
                else
                {
                    return  (AllData );
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return  (new List<ServiceReportVM>());
            }

        }


        [HttpGet]
        public JsonResult GetDash4()
        {
            try
            {
                var userId = User.Identity.GetUserId();

                var UserInfo = _unitOfWork.User.GetMyInfo(userId);

                int CurrYear = UserInfo.CurrYear;

                var FromDate = DateTime.Parse("01/01/" + CurrYear);
               var ToDate = DateTime.Now.Date;

                

                var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);





                var ExpensData = _unitOfWork.NativeSql.GetTotlExpense(UserInfo.fCompanyId, FromDate, ToDate);





                DataTable dt = new DataTable();
                List<object> iData = new List<object>();
               
                dt.Columns.Add("All", System.Type.GetType("System.Double"));
                dt.Columns.Add("ADMINISTRATIONEXPENSES", System.Type.GetType("System.Double"));
                dt.Columns.Add("SALESEXPENSES", System.Type.GetType("System.Double"));
                dt.Columns.Add("FINANCIALEXPENSES", System.Type.GetType("System.Double"));

                double d = 0;
                DataRow dr = dt.NewRow();


                dr["All"] = Math.Round(ExpensData.Where(m => m.MonthNo == 1).Sum(m => m.NET), 3);
                dr["ADMINISTRATIONEXPENSES"] = Math.Round(ExpensData.Where(m => m.MonthNo == 1 && m.AccountTypeID==15).Sum(m => m.NET), 3);
                dr["SALESEXPENSES"] = Math.Round(ExpensData.Where(m => m.MonthNo == 1 && m.AccountTypeID == 14).Sum(m => m.NET ), 3);
                dr["FINANCIALEXPENSES"] = Math.Round(ExpensData.Where(m => m.MonthNo == 1 && m.AccountTypeID == 16).Sum(m => m.NET ), 3);

                dt.Rows.Add(dr);

                for (int x = 2; x <= 12; x++)
                {
                    dr = dt.NewRow();



                    dr["All"] = Math.Round(ExpensData.Where(m => m.MonthNo == x).Sum(m => m.NET), 3);
                    dr["ADMINISTRATIONEXPENSES"] = Math.Round(ExpensData.Where(m => m.MonthNo == x && m.AccountTypeID == 15).Sum(m => m.NET), 3);
                    dr["SALESEXPENSES"] = Math.Round(ExpensData.Where(m => m.MonthNo == x && m.AccountTypeID == 14).Sum(m => m.NET), 3);
                    dr["FINANCIALEXPENSES"] = Math.Round(ExpensData.Where(m => m.MonthNo == x && m.AccountTypeID == 16).Sum(m => m.NET), 3);
                    dt.Rows.Add(dr);
                }






                foreach (DataColumn dc in dt.Columns)
                {
                    List<object> x = new List<object>();
                    x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                    iData.Add(x);
                }



                return Json(iData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                List<object> iData = new List<object>();
                //Creating sample data  
                DataTable dt = new DataTable();
           
                dt.Columns.Add("Value", System.Type.GetType("System.Double"));
                DataRow dr = dt.NewRow();
               
                dr["Value"] = 0;
                dt.Rows.Add(dr);

                foreach (DataColumn dc in dt.Columns)
                {
                    List<object> x = new List<object>();
                    x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                    iData.Add(x);
                }
                return Json(iData, JsonRequestBehavior.AllowGet);
            }
            //    List<object> iData = new List<object>();

        }

        [HttpGet]
        public JsonResult GetDash3()
        {
            try
            {
                var userId = User.Identity.GetUserId();

                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                int CurrYear = UserInfo.CurrYear;
                var FromDate = "01/01/" + CurrYear;

                ServiceReportVM ObjSale = new ServiceReportVM();
                ObjSale.FromDate = DateTime.Parse("01/01/" + CurrYear);
                ObjSale.ToDate = DateTime.Now.Date;

                ObjSale.SaleService = true;
                ObjSale.SaleMultiService = true;
                ObjSale.ReturnService = true;
                ObjSale.ReturnMultiService = true;
                double SaleValue = 0;
                var SaleData = GetServiceReport(ObjSale);
                if(SaleData!=null)
                  SaleValue = SaleData.Sum(m => m.NetTotal);


                ServiceReportVM ObjPur = new ServiceReportVM();
                ObjPur.FromDate = DateTime.Parse("01/01/" + CurrYear);
                ObjPur.ToDate = DateTime.Now.Date;

                ObjPur.PurchaseService = true;
                ObjPur.PurchaseMultiService = true;
                ObjPur.ReturnPurchaseService = true;
                ObjPur.ReturnPurchaseMultiService = true;
                double PurValue = 0;
                var ObjPurData = GetServiceReport(ObjPur);
                if (ObjPurData != null)
                    PurValue = ObjPurData.Sum(m => m.NetTotal);


                //AllVacation.Where(m => m.EmpId == CurrEmpSlip.EmployeeId && m.FVactionType == (int)VacTypeEnum.UnPaid).Sum(x => (double?)x.TotDays * x.EfectedValue) ?? 0;

                //"January", "February", "March", "April", "May", "June", "July","August","September","October","November","December"

           

                DataTable dt = new DataTable();
                List<object> iData = new List<object>();
                dt.Columns.Add("Sale", System.Type.GetType("System.Double"));
                dt.Columns.Add("Pur", System.Type.GetType("System.Double"));

                double d = 0;
                DataRow dr = dt.NewRow();        
                dr["Sale"] = Math.Round(SaleData.Where(m=>m.MonthNo==1).Sum(m=>m.NetTotal),3);
                dr["Pur"] =Math.Abs( Math.Round(ObjPurData.Where(m => m.MonthNo == 1).Sum(m => m.NetTotal), 3));
                dt.Rows.Add(dr);

                  dr = dt.NewRow();
                dr["Sale"] = Math.Round(SaleData.Where(m => m.MonthNo == 2).Sum(m => m.NetTotal), 3);
                dr["Pur"] =Math.Abs( Math.Round(ObjPurData.Where(m => m.MonthNo == 2).Sum(m => m.NetTotal), 3) );
                dt.Rows.Add(dr);

                  dr = dt.NewRow();
                dr["Sale"] = Math.Round(SaleData.Where(m => m.MonthNo == 3).Sum(m => m.NetTotal), 3);
                dr["Pur"] = Math.Abs(Math.Round(ObjPurData.Where(m => m.MonthNo == 3).Sum(m => m.NetTotal), 3) );
                dt.Rows.Add(dr);

                  dr = dt.NewRow();
                dr["Sale"] = Math.Round(SaleData.Where(m => m.MonthNo == 4).Sum(m => m.NetTotal), 3);
                dr["Pur"] = Math.Abs(Math.Round(ObjPurData.Where(m => m.MonthNo == 4).Sum(m => m.NetTotal), 3) );
                dt.Rows.Add(dr);

                  dr = dt.NewRow();
                dr["Sale"] = Math.Round(SaleData.Where(m => m.MonthNo == 5).Sum(m => m.NetTotal), 3);
                dr["Pur"] = Math.Abs(Math.Round(ObjPurData.Where(m => m.MonthNo == 5).Sum(m => m.NetTotal), 3) );
                dt.Rows.Add(dr);

                  dr = dt.NewRow();
                dr["Sale"] = Math.Round(SaleData.Where(m => m.MonthNo == 6).Sum(m => m.NetTotal), 3);
                dr["Pur"] = Math.Abs(Math.Round(ObjPurData.Where(m => m.MonthNo == 6).Sum(m => m.NetTotal), 3) );
                dt.Rows.Add(dr);

                  dr = dt.NewRow();
                dr["Sale"] = Math.Round(SaleData.Where(m => m.MonthNo == 7).Sum(m => m.NetTotal), 3);
                dr["Pur"] = Math.Abs(Math.Round(ObjPurData.Where(m => m.MonthNo == 7).Sum(m => m.NetTotal), 3) );
                dt.Rows.Add(dr);

                  dr = dt.NewRow();
                dr["Sale"] = Math.Round(SaleData.Where(m => m.MonthNo ==8).Sum(m => m.NetTotal), 3);
                dr["Pur"] = Math.Abs(Math.Round(ObjPurData.Where(m => m.MonthNo == 8).Sum(m => m.NetTotal), 3) );
                dt.Rows.Add(dr);

                   dr = dt.NewRow();
                dr["Sale"] = Math.Round(SaleData.Where(m => m.MonthNo == 9).Sum(m => m.NetTotal), 3);
                dr["Pur"] = Math.Abs(Math.Round(ObjPurData.Where(m => m.MonthNo ==9).Sum(m => m.NetTotal), 3));
                dt.Rows.Add(dr);

                  dr = dt.NewRow();
                dr["Sale"] = Math.Round(SaleData.Where(m => m.MonthNo == 10).Sum(m => m.NetTotal), 3);
                dr["Pur"] = Math.Abs(Math.Round(ObjPurData.Where(m => m.MonthNo == 10).Sum(m => m.NetTotal), 3) );
                dt.Rows.Add(dr);

                  dr = dt.NewRow();
                dr["Sale"] = Math.Round(SaleData.Where(m => m.MonthNo == 11).Sum(m => m.NetTotal), 3);
                dr["Pur"] = Math.Abs(Math.Round(ObjPurData.Where(m => m.MonthNo == 11).Sum(m => m.NetTotal), 3) );
                dt.Rows.Add(dr);

                 dr = dt.NewRow();
                dr["Sale"] = Math.Round(SaleData.Where(m => m.MonthNo == 12).Sum(m => m.NetTotal), 3);
                dr["Pur"] = Math.Abs(Math.Round(ObjPurData.Where(m => m.MonthNo == 12).Sum(m => m.NetTotal), 3) );
                dt.Rows.Add(dr);
         






                foreach (DataColumn dc in dt.Columns)
                {
                    List<object> x = new List<object>();
                    x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                    iData.Add(x);
                }



                return Json(iData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                List<object> iData = new List<object>();
                //Creating sample data  
                DataTable dt = new DataTable();
                dt.Columns.Add("Sale", System.Type.GetType("System.Double"));
                dt.Columns.Add("Pur", System.Type.GetType("System.Double"));
                DataRow dr = dt.NewRow();
                dr["Sale"] = 0;
                dr["Pur"] = 0;
                dt.Rows.Add(dr);

                foreach (DataColumn dc in dt.Columns)
                {
                    List<object> x = new List<object>();
                    x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                    iData.Add(x);
                }
                return Json(iData, JsonRequestBehavior.AllowGet);
            }
            //    List<object> iData = new List<object>();

        }


        [HttpGet]
        public JsonResult GetDash1()
        {
            try
            {
                AccountLevelRepVM Obj = new AccountLevelRepVM();
            
                var userId = User.Identity.GetUserId();
   
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
 
                string UserID = User.Identity.GetUserId();
              
                var AccountData = _unitOfWork.NativeSql.GetChartOfAccountByLevel(UserInfo.fCompanyId);
                int CurrYear = UserInfo.CurrYear;

                var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                String Chart = CoInfo.AccountChart;


                Chart = Chart.Trim(new Char[] { '-', '-', ' ' });
                int i = 1;
                string[] numbers = Regex.Split(Chart, @"\D+");
                foreach (string value in numbers)
                {
                    if (!string.IsNullOrEmpty(value))
                    {

                       
                        
                        i = i + 1;
                    }
                }


                Obj.AccountLevelDropVMID =i;

                var Date = "01/01/" + CurrYear;
                var OpenDate = "31/12/" + (CurrYear - 1).ToString();
                Obj.FromDate =DateTime.Parse( Date);
                Obj.ToDate = DateTime.Parse("31/12/" + CurrYear);

                string TempCostID = "0";
                int TempCostType = 0;
             

                if (Obj.ByCostCenter)
                {
                    TempCostID = Obj.CostCenterNumber;
                    if (Obj.Partofthenumber)
                    {
                        TempCostType = Obj.CostSearchType;
                    }


                }

                IEnumerable<TrialBalanceVM> TotData = new List<TrialBalanceVM>();
                if (Obj.Partofthenumber)
                {
                    if (Obj.CostSearchType == 1)
                    {
                        Obj.CostCenterNumber = Obj.CostCenterNumber + "%";
                    }
                    else if (Obj.CostSearchType == 2)
                    {
                        Obj.CostCenterNumber = "%" + Obj.CostCenterNumber;
                    }
                    else if (Obj.CostSearchType == 3)
                    {
                        Obj.CostCenterNumber = "%" + Obj.CostCenterNumber + "%";

                    }
                }

                var TranData = _unitOfWork.NativeSql.GetTransactionForTrial(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate, Obj.ByCostCenter, Obj.CostCenterNumber, Obj.Partofthenumber, Obj.CostSearchType);
                if (DateTime.Parse(Date) != Obj.FromDate)
                    TotData = _unitOfWork.NativeSql.GetTotCreditDebitForTrial(UserInfo.fCompanyId, DateTime.Parse(Date), Obj.FromDate, Obj.ByCostCenter, Obj.CostCenterNumber, Obj.Partofthenumber, Obj.CostSearchType);

                //----------------Get Open-------------//
                IEnumerable<TrialBalanceVM> TotDataOpen = new List<TrialBalanceVM>();
                TotDataOpen = _unitOfWork.NativeSql.GetTotCreditDebitForTrialOpen(UserInfo.fCompanyId, DateTime.Parse(OpenDate), DateTime.Parse(OpenDate));
                foreach (var D in TotDataOpen)
                {
                    TotData = TotData.Append(D);

                }
                //------------------------------------//



                double TotalDebit = 0;
                double TOTCredit = 0;
                double NetCredit = 0;
                double NetDebit = 0;
                double CreditBalance = 0;
                double DebitBalance = 0;
                double CreditTransAction = 0;
                double DebitTransAction = 0;

                List<TrialBalanceVM> TrialBalanceVMList = new List<TrialBalanceVM>();

                IEnumerable<ChartOfAccount> Accounts = new List<ChartOfAccount>();

                IEnumerable<ChartOfAccount> LessMainAccount = new List<ChartOfAccount>();

                //----//
                var OtherAccount = _unitOfWork.DefinitionOtherAccount.GetDefinitionOtherAccountByID(UserInfo.fCompanyId);
                string FundAccount = OtherAccount.CashFundsAccountNumber;
                Obj.AccNo = FundAccount;
                //----//

                //-------------------//
                if (!String.IsNullOrEmpty(Obj.AccNo))
                {

                    var MainAccount = AccountData.FirstOrDefault(m => m.AccountNumber == Obj.AccNo);
                    var MainChild = AccountData.Where(m => m.AccountFather == Obj.AccNo).OrderBy(m => m.AccountNumber).OrderBy(m => m.AccountLevel).ToList();

                    foreach (var Acc in MainChild)
                    {
                        TotalDebit = 0;
                        TOTCredit = 0;
                        NetCredit = 0;
                        NetDebit = 0;
                        CreditBalance = 0;
                        DebitBalance = 0;
                        CreditTransAction = 0;
                        DebitTransAction = 0;


                        if (AccountData.FirstOrDefault(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel) != null)//Check if Account Had Branches
                        {
                            var AllAccount = AccountData.Where(m => m.AccountNumber.StartsWith(Acc.AccountNumber) && m.AccountLevel > Acc.AccountLevel)
                                            .OrderBy(m => m.AccountNumber).ThenBy(m => m.AccountLevel).ToList();//Get All Account Branches
                            int FatherLevel = Acc.AccountLevel;
                            foreach (var CurrAccLevel in AllAccount)
                            {
                                if (CurrAccLevel.AccountLevel == FatherLevel)
                                {
                                    break;
                                }
                                else
                                {

                                    var TransActionData = TranData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                    var BalanceData = TotData.Where(m => m.AccountNumber == CurrAccLevel.AccountNumber);
                                    foreach (var D in TransActionData)
                                    {
                                        CreditTransAction += D.CreditTransAction;
                                        DebitTransAction += D.DebitTransAction;
                                    }
                                    foreach (var Balance in BalanceData)
                                    {
                                        CreditBalance += Balance.CreditBalance;
                                        DebitBalance += Balance.DebitBalance;
                                    }



                                    TotalDebit = DebitTransAction + DebitBalance;
                                    TOTCredit = CreditTransAction + CreditBalance;




                                }
                            }




                        }
                        else
                        {
                            var TransActionData = TranData.Where(m => m.AccountNumber == Acc.AccountNumber);
                            var BalanceData = TotData.Where(m => m.AccountNumber == Acc.AccountNumber);
                            foreach (var D in TransActionData)
                            {
                                CreditTransAction += D.CreditTransAction;
                                DebitTransAction += D.DebitTransAction;
                            }
                            foreach (var Balance in BalanceData)
                            {
                                CreditBalance += Balance.CreditBalance;
                                DebitBalance += Balance.DebitBalance;
                            }
                            TotalDebit = DebitTransAction + DebitBalance;
                            TOTCredit = CreditTransAction + CreditBalance;
                        }
                        if (TotalDebit > TOTCredit)
                        {
                            NetDebit = TotalDebit - TOTCredit;
                        }
                        if (TotalDebit < TOTCredit)
                        {
                            NetCredit = TOTCredit - TotalDebit;
                        }

                        TrialBalanceVM trialBalance = new TrialBalanceVM();
                        trialBalance.AccountNumber = Acc.AccountNumber;
                        trialBalance.Name = Acc.ArabicName;

                        trialBalance.CreditBalance = CreditBalance;
                        trialBalance.CreditTransAction = CreditTransAction;
                        trialBalance.DebitBalance = DebitBalance;
                        trialBalance.DebitTransAction = DebitTransAction;
                        trialBalance.NetCredit = NetCredit;
                        trialBalance.NetDebit = NetDebit;
                        trialBalance.TempCostID = TempCostID;
                        trialBalance.TempCostType = TempCostType;



                        trialBalance.nCreditBalance = CreditBalance;
                        trialBalance.nCreditTransAction = CreditTransAction;
                        trialBalance.nDebitBalance = DebitBalance;
                        trialBalance.nDebitTransAction = DebitTransAction;
                        trialBalance.nNetCredit = NetCredit;
                        trialBalance.nNetDebit = NetDebit;


                        if (!Obj.ShowZeroBalance)
                        {
                            if ((trialBalance.NetCredit - trialBalance.NetDebit) != 0)
                            {
                                TrialBalanceVMList.Add(trialBalance);
                            }
                        }
                        else
                        {
                            if ((trialBalance.NetCredit != 0) || (trialBalance.NetDebit != 0) || (TotalDebit != 0) || (NetCredit != 0))
                            {
                                TrialBalanceVMList.Add(trialBalance);
                            }


                        }
                    }


                }

                //---------------------//

                var NetDebitFund = TrialBalanceVMList.Sum(m => m.nNetDebit);
                var NetCreditFund = TrialBalanceVMList.Sum(m => m.nNetCredit);
                var FundBalance =   NetDebitFund- NetCreditFund;

                double BankBalance =_unitOfWork.NativeSql.GetTotalBankFund(UserInfo.fCompanyId, CurrYear);

              //  var ChequeUnderCollectionCount = _unitOfWork.NativeSql.GetCountAllUbderCollectionCheque(UserInfo.fCompanyId);
                var ChequeUnderCollection =double.Parse( _unitOfWork.NativeSql.GetSumAllUbderCollectionCheque(UserInfo.fCompanyId));

              //  var PostdatedChequeCount = _unitOfWork.NativeSql.GetCountAllPostdatedCheque(UserInfo.fCompanyId);
                var PostdatedCheque = _unitOfWork.NativeSql.GetSumAllPostdatedCheque(UserInfo.fCompanyId);

               var PaperAmount = _unitOfWork.NativeSql.GetSumAllChequeInFund(UserInfo.fCompanyId);

                double Total = ChequeUnderCollection +double.Parse(PostdatedCheque) + FundBalance;
                DataTable dt = new DataTable();
                List<object> iData = new List<object>();
                dt.Columns.Add("Name", System.Type.GetType("System.String"));
                dt.Columns.Add("Count", System.Type.GetType("System.Double"));
                dt.Columns.Add("Price", System.Type.GetType("System.Double"));


                DataRow dr = dt.NewRow();
                dr["Name"] = string.Format("{0}", Resources.Resource.UnpaidChequesReceived);
                dr["Count"] = 0;
                dr["Price"] = Math.Round( (double.Parse(ChequeUnderCollection.ToString()) +double.Parse(PaperAmount)),3);
                dt.Rows.Add(dr);


                 

                dr = dt.NewRow();
                dr["Name"] = string.Format("{0}", Resources.Resource.UnpaidChequesPayment);
                dr["Count"] = 0;
                dr["Price"] = Math.Round(double.Parse(PostdatedCheque),3 );
                dt.Rows.Add(dr);


                dr = dt.NewRow();
                dr["Name"] = string.Format("{0}", Resources.Resource.FundsBalances);
                dr["Count"] = 0;
                dr["Price"] = Math.Round( FundBalance,3);
                dt.Rows.Add(dr);


                dr = dt.NewRow();
                dr["Name"] = string.Format("{0}", Resources.Resource.BanksBalances);
                dr["Count"] = 0;
                dr["Price"] =Math.Round (   BankBalance,3 );
                dt.Rows.Add(dr);

                foreach (DataColumn dc in dt.Columns)
                {
                    List<object> x = new List<object>();
                    x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                    iData.Add(x);
                }



                return Json(iData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                List<object> iData = new List<object>();
                //Creating sample data  
                DataTable dt = new DataTable();
                dt.Columns.Add("Name", System.Type.GetType("System.String"));
                dt.Columns.Add("Count", System.Type.GetType("System.Double"));
                dt.Columns.Add("Price", System.Type.GetType("System.Double"));
                DataRow dr = dt.NewRow();
                dr["Name"] = "No data";
                dr["Count"] = 0;
                dr["Price"] = 0;
                dt.Rows.Add(dr);

                foreach (DataColumn dc in dt.Columns)
                {
                    List<object> x = new List<object>();
                    x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                    iData.Add(x);
                }
                return Json(iData, JsonRequestBehavior.AllowGet);
            }
            //    List<object> iData = new List<object>();

        }



        [HttpGet]
        public JsonResult GetDash2()
        {
            try
            {
                var userId = User.Identity.GetUserId();

                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                int CurrYear = UserInfo.CurrYear;
                var FromDate = "01/01/" + CurrYear;
                ClientSuplierFilterVM Obj = new ClientSuplierFilterVM();
                Obj.ToDate = DateTime.Now;
            
                Obj.Sales = false;
                Obj.ToDate = DateTime.Now.Date;

                Obj.ClientSup = 1;
                var AccountData = _unitOfWork.NativeSql.GetAccountsInfoByKind(UserInfo.fCompanyId, Obj.ClientSup);
                var TransData = _unitOfWork.NativeSql.TransactionByAccKind(UserInfo.fCompanyId, DateTime.Parse(FromDate), Obj.ToDate, Obj.ClientSup);
                List<CustSuppRepVM> ObjList = new List<CustSuppRepVM>();
                foreach (var Acc in AccountData)
                {
                    CustSuppRepVM ObjC = new CustSuppRepVM();
                    ObjC.AccountNumber = Acc.AccountNumber;
                    ObjC.AccountName = Acc.AccountName;
                    ObjC.AccountTypeID = Acc.AccountTypeID;
                    ObjC.AccountKind = Acc.AccountKind;
                    ObjC.SalesID = Acc.SalesID;
                    ObjC.Email = Acc.Email;
                    ObjC.Telephone = Acc.Telephone;
                    ObjC.Mobile = Acc.Mobile;
                    ObjC.TeleFax = Acc.TeleFax;
                    if (Obj.ClientSup == 1)
                    {
                        ObjC.SaleName = Acc.SaleName;
                        ObjC.CustomerArea = Acc.CustomerArea;
                        ObjC.CustomerCity = Acc.CustomerCity;
                        ObjC.StreetName = Acc.StreetName;
                        ObjC.SupplierCity = "";
                        ObjC.SupplierCountry = "";
                        ObjC.NameOfPersonInCharge = "";
                    }
                    else
                    {
                        ObjC.SaleName = "";
                        ObjC.CustomerArea = "";
                        ObjC.CustomerCity = "";
                        ObjC.StreetName = "";
                        ObjC.SupplierCity = Acc.SupplierCity;
                        ObjC.SupplierCountry = Acc.SupplierCountry;
                        ObjC.NameOfPersonInCharge = Acc.NameOfPersonInCharge;
                    }
                    if ((TransData.FirstOrDefault(m => m.AccountNumber == Acc.AccountNumber) != null))
                    {
                        var D = TransData.FirstOrDefault(m => m.AccountNumber == Acc.AccountNumber);
                        ObjC.Balance = (Acc.OpeningBalanceDebit + D.Debit) - (Acc.OpeningBalanceCredit + D.Credit);
                    }
                    ObjList.Add(ObjC);
                }
                var ClientBalance = ObjList.Sum(m => m.Balance);

//----//

                Obj.ClientSup =2;
                  AccountData = _unitOfWork.NativeSql.GetAccountsInfoByKind(UserInfo.fCompanyId, Obj.ClientSup);
                  TransData = _unitOfWork.NativeSql.TransactionByAccKind(UserInfo.fCompanyId, DateTime.Parse(FromDate), Obj.ToDate, Obj.ClientSup);
               ObjList = new List<CustSuppRepVM>();
                foreach (var Acc in AccountData)
                {
                    CustSuppRepVM ObjC = new CustSuppRepVM();
                    ObjC.AccountNumber = Acc.AccountNumber;
                    ObjC.AccountName = Acc.AccountName;
                    ObjC.AccountTypeID = Acc.AccountTypeID;
                    ObjC.AccountKind = Acc.AccountKind;
                    ObjC.SalesID = Acc.SalesID;
                    ObjC.Email = Acc.Email;
                    ObjC.Telephone = Acc.Telephone;
                    ObjC.Mobile = Acc.Mobile;
                    ObjC.TeleFax = Acc.TeleFax;
                    if (Obj.ClientSup == 1)
                    {
                        ObjC.SaleName = Acc.SaleName;
                        ObjC.CustomerArea = Acc.CustomerArea;
                        ObjC.CustomerCity = Acc.CustomerCity;
                        ObjC.StreetName = Acc.StreetName;
                        ObjC.SupplierCity = "";
                        ObjC.SupplierCountry = "";
                        ObjC.NameOfPersonInCharge = "";
                    }
                    else
                    {
                        ObjC.SaleName = "";
                        ObjC.CustomerArea = "";
                        ObjC.CustomerCity = "";
                        ObjC.StreetName = "";
                        ObjC.SupplierCity = Acc.SupplierCity;
                        ObjC.SupplierCountry = Acc.SupplierCountry;
                        ObjC.NameOfPersonInCharge = Acc.NameOfPersonInCharge;
                    }
                    if ((TransData.FirstOrDefault(m => m.AccountNumber == Acc.AccountNumber) != null))
                    {
                        var D = TransData.FirstOrDefault(m => m.AccountNumber == Acc.AccountNumber);
                        ObjC.Balance = (Acc.OpeningBalanceDebit + D.Debit) - (Acc.OpeningBalanceCredit + D.Credit);
                    }
                    ObjList.Add(ObjC);
                }

                var SupBalance = ObjList.Sum(m => m.Balance);

//----//



                DataTable dt = new DataTable();
                List<object> iData = new List<object>();
                dt.Columns.Add("Name", System.Type.GetType("System.String"));
                dt.Columns.Add("Count", System.Type.GetType("System.Double"));
                dt.Columns.Add("Price", System.Type.GetType("System.Double"));


                DataRow dr = dt.NewRow();
                dr["Name"] = string.Format("{0}", Resources.Resource.ClientsBalances);
                dr["Count"] = 0;
                dr["Price"] = Math.Round(ClientBalance,3);
                dt.Rows.Add(dr);




                dr = dt.NewRow();
                dr["Name"] = string.Format("{0}", Resources.Resource.SuppliersBalances);
                dr["Count"] = 0;
                dr["Price"] = Math.Round(SupBalance,3);
                dt.Rows.Add(dr);


             

                foreach (DataColumn dc in dt.Columns)
                {
                    List<object> x = new List<object>();
                    x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                    iData.Add(x);
                }



                return Json(iData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                List<object> iData = new List<object>();
                //Creating sample data  
                DataTable dt = new DataTable();
                dt.Columns.Add("Name", System.Type.GetType("System.String"));
                dt.Columns.Add("Count", System.Type.GetType("System.Double"));
                dt.Columns.Add("Price", System.Type.GetType("System.Double"));
                DataRow dr = dt.NewRow();
                dr["Name"] = "No data";
                dr["Count"] = 0;
                dr["Price"] = 0;
                dt.Rows.Add(dr);

                foreach (DataColumn dc in dt.Columns)
                {
                    List<object> x = new List<object>();
                    x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                    iData.Add(x);
                }
                return Json(iData, JsonRequestBehavior.AllowGet);
            }
            //    List<object> iData = new List<object>();

        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public    JsonResult GetCheckChart()
        {
            try
            {
                
                var userId = User.Identity.GetUserId();
                //  var CoData = _unitOfWork.UserAccount.GetUserByID(userId);

               
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);

                var ChequeUnderCollectionCount = _unitOfWork.NativeSql.GetCountAllUbderCollectionCheque(UserInfo.fCompanyId);
                var ChequeUnderCollection = _unitOfWork.NativeSql.GetCountAllUbderCollectionCheque(UserInfo.fCompanyId);

                var PostdatedChequeCount = _unitOfWork.NativeSql.GetCountAllPostdatedCheque(UserInfo.fCompanyId);
                var PostdatedCheque = _unitOfWork.NativeSql.GetSumAllPostdatedCheque(UserInfo.fCompanyId);

 
                var PaperCount = _unitOfWork.NativeSql.GetCountAllChequeInFund(UserInfo.fCompanyId);
                var PaperAmount = _unitOfWork.NativeSql.GetSumAllChequeInFund(UserInfo.fCompanyId);


               



                var UnderCollectionPaperCount = _unitOfWork.NativeSql.GetCountAllUbderCollectionCheque(UserInfo.fCompanyId);
                var UnderCollectionPaperAmount = _unitOfWork.NativeSql.GetSumAllUbderCollectionCheque(UserInfo.fCompanyId);

                var EndorsementPaperCount = _unitOfWork.NativeSql.GetCountAllEndorsementPaperCount(UserInfo.fCompanyId);
                var EndorsementPaperAmount = _unitOfWork.NativeSql.GetSumAllEndorsementPaperCount(UserInfo.fCompanyId);
                var PaymentUnderCollectionPaperCount = _unitOfWork.NativeSql.GetCountAllPaymentUbderCollectionCheque(UserInfo.fCompanyId);
                var PaymentUnderCollectionPaperAmount = _unitOfWork.NativeSql.GetSumAllPaymentUbderCollectionCheque(UserInfo.fCompanyId);
                var ClearingDepositPaperCount = _unitOfWork.NativeSql.GetCountAllClearingDepositCheque(UserInfo.fCompanyId);
                var ClearingDepositPaperAmount = _unitOfWork.NativeSql.GetSumAllClearingDepositCheque(UserInfo.fCompanyId);
                var DrawingUnderCollectionPaperCount = _unitOfWork.NativeSql.GetCountAllDrawingUnderCollectionCheque(UserInfo.fCompanyId);
                var DrawingUnderCollectionPaperAmount = _unitOfWork.NativeSql.GetSumAllDrawingUnderCollectionCheque(UserInfo.fCompanyId);
                var ReturnedChequePaperCount = _unitOfWork.NativeSql.GetCountAllReturnedCheque(UserInfo.fCompanyId);
                var ReturnedChequePaperAmount = _unitOfWork.NativeSql.GetSumAllReturnedCheque(UserInfo.fCompanyId);
                var CourtFundPaperCount = _unitOfWork.NativeSql.GetCountAllCourtFundCheque(UserInfo.fCompanyId);
                var CourtFundPaperAmount = _unitOfWork.NativeSql.GetSumAllCourtFundCheque(UserInfo.fCompanyId);


                DataTable dt = new DataTable();
                List<object> iData = new List<object>();
                dt.Columns.Add("Name", System.Type.GetType("System.String"));
                dt.Columns.Add("Count", System.Type.GetType("System.Double"));
                dt.Columns.Add("Price", System.Type.GetType("System.Double"));


                DataRow dr = dt.NewRow();
                dr["Name"] =string.Format("{0} ({1})", Resources.Resource.ReturnAChequeFromTheChequeBox, PaperCount.ToString());
                dr["Count"] = PaperCount;
                dr["Price"] = PaperAmount;
                dt.Rows.Add(dr);


                dr = dt.NewRow();
                dr["Name"] = string.Format("{0} ({1})", Resources.Resource.ClearingDeposit, PaperCount.ToString());
               
                dr["Count"] = PaperCount;
                dr["Price"] = PaperAmount;
                dt.Rows.Add(dr);


                dr = dt.NewRow();
                dr["Name"] = string.Format("{0} ({1})", Resources.Resource.ChequeEndorsement, PaperCount.ToString());
          
                dr["Count"] = PaperCount;
                dr["Price"] = PaperAmount;
                dt.Rows.Add(dr);




                dr = dt.NewRow();
                dr["Name"] = string.Format("{0} ({1})", Resources.Resource.FundChequesDrawnFromUnderCollection, DrawingUnderCollectionPaperCount.ToString());
             
                dr["Count"] = DrawingUnderCollectionPaperCount;
                dr["Price"] = DrawingUnderCollectionPaperAmount;
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                dr["Name"] = string.Format("{0} ({1})", Resources.Resource.DepositChequesUnderCollection, PaperCount.ToString());
           
                dr["Count"] = PaperCount;
                dr["Price"] = PaperAmount;
                dt.Rows.Add(dr);


                dr = dt.NewRow();
                dr["Name"] = string.Format("{0} ({1})", Resources.Resource.ReturnChequeClearingDeposit, ClearingDepositPaperCount.ToString());
               
                dr["Count"] = ClearingDepositPaperCount;
                dr["Price"] = ClearingDepositPaperAmount;
                dt.Rows.Add(dr);


                dr = dt.NewRow();
                dr["Name"] = string.Format("{0} ({1})", Resources.Resource.PaymentChequeEndorsement, EndorsementPaperCount.ToString());
              
                dr["Count"] = EndorsementPaperCount;
                dr["Price"] = EndorsementPaperAmount;
                dt.Rows.Add(dr);


                dr = dt.NewRow();
                dr["Name"] = string.Format("{0} ({1})", Resources.Resource.ReturnedChequeFund, ReturnedChequePaperCount.ToString());
              
                dr["Count"] = ReturnedChequePaperCount;
                dr["Price"] = ReturnedChequePaperAmount;
                dt.Rows.Add(dr);



                dr = dt.NewRow();
                dr["Name"] = string.Format("{0} ({1})", Resources.Resource.DrawingChequeUnderCollection, UnderCollectionPaperCount.ToString());
           
                dr["Count"] = UnderCollectionPaperCount;
                dr["Price"] = UnderCollectionPaperAmount;
                dt.Rows.Add(dr);



                dr = dt.NewRow();
                dr["Name"] = string.Format("{0} ({1})", Resources.Resource.ReturnChequeEndorsement, EndorsementPaperCount.ToString());
              
                dr["Count"] = EndorsementPaperCount;
                dr["Price"] = EndorsementPaperAmount;
                dt.Rows.Add(dr);




                dr = dt.NewRow();
                dr["Name"] = string.Format("{0} ({1})", Resources.Resource.CourtFund, CourtFundPaperCount.ToString());
                 
                dr["Count"] = CourtFundPaperCount;
                dr["Price"] = CourtFundPaperAmount;
                dt.Rows.Add(dr);



                dr = dt.NewRow();
                dr["Name"] = string.Format("{0} ({1})", Resources.Resource.PaymentChequeUnderCollection, UnderCollectionPaperCount.ToString());
              
                dr["Count"] = UnderCollectionPaperCount;
                dr["Price"] = UnderCollectionPaperAmount;
                dt.Rows.Add(dr);


                dr = dt.NewRow();
                dr["Name"] = string.Format("{0} ({1})", Resources.Resource.ReturnChequeUnderCollection, PaymentUnderCollectionPaperCount.ToString());
          
                dr["Count"] = PaymentUnderCollectionPaperCount;
                dr["Price"] = PaymentUnderCollectionPaperAmount;
                dt.Rows.Add(dr);
               
             

                foreach (DataColumn dc in dt.Columns)
                {
                    List<object> x = new List<object>();
                    x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                    iData.Add(x);
                }



                return Json(iData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                List<object> iData = new List<object>();
                //Creating sample data  
                DataTable dt = new DataTable();
                dt.Columns.Add("Name", System.Type.GetType("System.String"));
                dt.Columns.Add("Count", System.Type.GetType("System.Double"));
                dt.Columns.Add("Price", System.Type.GetType("System.Double"));
                DataRow dr = dt.NewRow();
                dr["Name"] ="No data";
                dr["Count"] = 0;
                dr["Price"] = 0;
                dt.Rows.Add(dr);

                foreach (DataColumn dc in dt.Columns)
                {
                    List<object> x = new List<object>();
                    x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                    iData.Add(x);
                }
                return Json(iData, JsonRequestBehavior.AllowGet);
            }
            //    List<object> iData = new List<object>();

        }


        [HttpGet]
        public JsonResult GetPaymentChequeChart()
        {
            try
            {

                var userId = User.Identity.GetUserId();
                //  var CoData = _unitOfWork.UserAccount.GetUserByID(userId);


                var UserInfo = _unitOfWork.User.GetMyInfo(userId);

               
                var PostdatedChequeCount = _unitOfWork.NativeSql.GetCountAllPostdatedCheque(UserInfo.fCompanyId);
                var PostdatedChequeAmount = _unitOfWork.NativeSql.GetSumAllPostdatedCheque(UserInfo.fCompanyId);
                var ReturnPostdatedChequeCount = _unitOfWork.NativeSql.GetCountAllReturnPostdatedCheque(UserInfo.fCompanyId);
                var ReturnPostdatedChequeAmount = _unitOfWork.NativeSql.GetSumAlllReturnPostdatedCheque(UserInfo.fCompanyId);
                var ReturnPaidPostdatedChequeCount = _unitOfWork.NativeSql.GetCountAllReturnPaidPostdatedCheque(UserInfo.fCompanyId);
                var ReturnPaidPostdatedChequeAmount = _unitOfWork.NativeSql.GetSumAlllReturnPaidPostdatedCheque(UserInfo.fCompanyId);


                DataTable dt = new DataTable();
                List<object> iData = new List<object>();
                dt.Columns.Add("Name", System.Type.GetType("System.String"));
                dt.Columns.Add("Count", System.Type.GetType("System.Double"));
                dt.Columns.Add("Price", System.Type.GetType("System.Double"));


                DataRow dr = dt.NewRow();
                dr["Name"] = string.Format("{0} ({1})", Resources.Resource.PostdatedCheque, PostdatedChequeCount.ToString());
                dr["Count"] = PostdatedChequeCount;
                dr["Price"] = PostdatedChequeAmount;
                dt.Rows.Add(dr);


                dr = dt.NewRow();
                dr["Name"] = string.Format("{0} ({1})", Resources.Resource.ReturnPostdatedCheques, ReturnPostdatedChequeCount.ToString());

                dr["Count"] = ReturnPostdatedChequeCount;
                dr["Price"] = ReturnPostdatedChequeAmount;
                dt.Rows.Add(dr);


                dr = dt.NewRow();
                dr["Name"] = string.Format("{0} ({1})", Resources.Resource.ReturnPaidPostdatedCheques, ReturnPaidPostdatedChequeCount.ToString());

                dr["Count"] = ReturnPaidPostdatedChequeCount;
                dr["Price"] = ReturnPaidPostdatedChequeAmount;
                dt.Rows.Add(dr);



  

 


                foreach (DataColumn dc in dt.Columns)
                {
                    List<object> x = new List<object>();
                    x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                    iData.Add(x);
                }



                return Json(iData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                List<object> iData = new List<object>();
                //Creating sample data  
                DataTable dt = new DataTable();
                dt.Columns.Add("Name", System.Type.GetType("System.String"));
                dt.Columns.Add("Count", System.Type.GetType("System.Double"));
                dt.Columns.Add("Price", System.Type.GetType("System.Double"));
                DataRow dr = dt.NewRow();
                dr["Name"] = "No data";
                dr["Count"] = 0;
                dr["Price"] = 0;
                dt.Rows.Add(dr);

                foreach (DataColumn dc in dt.Columns)
                {
                    List<object> x = new List<object>();
                    x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                    iData.Add(x);
                }
                return Json(iData, JsonRequestBehavior.AllowGet);
            }
            //    List<object> iData = new List<object>();

        }

        public ActionResult ReceivedCheque()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var PaperCount = _unitOfWork.NativeSql.GetCountAllChequeInFund(UserInfo.fCompanyId);
            var PaperAmount = _unitOfWork.NativeSql.GetSumAllChequeInFund(UserInfo.fCompanyId);
            var UnderCollectionPaperCount = _unitOfWork.NativeSql.GetCountAllUbderCollectionCheque(UserInfo.fCompanyId);
            var UnderCollectionPaperAmount = _unitOfWork.NativeSql.GetSumAllUbderCollectionCheque(UserInfo.fCompanyId);
            var EndorsementPaperCount = _unitOfWork.NativeSql.GetCountAllEndorsementPaperCount(UserInfo.fCompanyId);
            var EndorsementPaperAmount = _unitOfWork.NativeSql.GetSumAllEndorsementPaperCount(UserInfo.fCompanyId);
            var PaymentUnderCollectionPaperCount = _unitOfWork.NativeSql.GetCountAllPaymentUbderCollectionCheque(UserInfo.fCompanyId);
            var PaymentUnderCollectionPaperAmount = _unitOfWork.NativeSql.GetSumAllPaymentUbderCollectionCheque(UserInfo.fCompanyId);
            var ClearingDepositPaperCount = _unitOfWork.NativeSql.GetCountAllClearingDepositCheque(UserInfo.fCompanyId);
            var ClearingDepositPaperAmount = _unitOfWork.NativeSql.GetSumAllClearingDepositCheque(UserInfo.fCompanyId);
            var DrawingUnderCollectionPaperCount = _unitOfWork.NativeSql.GetCountAllDrawingUnderCollectionCheque(UserInfo.fCompanyId);
            var DrawingUnderCollectionPaperAmount = _unitOfWork.NativeSql.GetSumAllDrawingUnderCollectionCheque(UserInfo.fCompanyId);
            var ReturnedChequePaperCount = _unitOfWork.NativeSql.GetCountAllReturnedCheque(UserInfo.fCompanyId);
            var ReturnedChequePaperAmount = _unitOfWork.NativeSql.GetSumAllReturnedCheque(UserInfo.fCompanyId);
            var CourtFundPaperCount = _unitOfWork.NativeSql.GetCountAllCourtFundCheque(UserInfo.fCompanyId);
            var CourtFundPaperAmount = _unitOfWork.NativeSql.GetSumAllCourtFundCheque(UserInfo.fCompanyId);

            var PaperFilter = new PaperFilterVM
            {
                PaperCount = PaperCount,
                UnderCollectionPaperCount = UnderCollectionPaperCount,
                EndorsementPaperCount = EndorsementPaperCount,
                PaymentUnderCollectionPaperCount = PaymentUnderCollectionPaperCount,
                ClearingDepositPaperCount = ClearingDepositPaperCount,
                DrawingUnderCollectionPaperCount = DrawingUnderCollectionPaperCount,
                ReturnedChequePaperCount = ReturnedChequePaperCount,
                CourtFundPaperCount = CourtFundPaperCount,
                PaperAmount = PaperAmount,
                UnderCollectionPaperAmount = UnderCollectionPaperAmount,
                EndorsementPaperAmount = EndorsementPaperAmount,
                PaymentUnderCollectionPaperAmount = PaymentUnderCollectionPaperAmount,
                ClearingDepositPaperAmount = ClearingDepositPaperAmount,
                DrawingUnderCollectionPaperAmount = DrawingUnderCollectionPaperAmount,
                ReturnedChequePaperAmount = ReturnedChequePaperAmount,
                CourtFundPaperAmount = CourtFundPaperAmount
            };
            return View(PaperFilter);
        }
    }
}