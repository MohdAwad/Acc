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
    public class TransActionController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransActionController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        [HttpGet]
        [Authorize(Roles = "CoOwner,DeleteTransActionTrans")]
        public ActionResult Delete(string id, int id2,int id3,int id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            var HeadrObj = _unitOfWork.Header.GetHeaderDataById(id, UserInfo.fCompanyId, id2,id3, id4);

            var TransObj = new List<Transaction>();

            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetCompanyTransactionKind(UserInfo.fCompanyId);
            var CompanyTransactionKindID = HeadrObj.CompanyTransactionKindNo;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            BankGuarantee ObjBankGuarantee = new BankGuarantee
            {
                CompanyID = HeadrObj.CompanyID,
                CompanyYear = HeadrObj.CompanyYear,
                VoucherNumber = HeadrObj.VoucherNumber

            };

            var BankGuaranteeData = _unitOfWork.BankGuarantee.GetBankGuarantee(ObjBankGuarantee);
            var TransVM = new TransActionVM
            {
                Header = HeadrObj,
                Transaction = TransObj,
                CompanyTransactionKind = CompanyTransactionKindObj,
                CompanyTransactionKindID = CompanyTransactionKindID,
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                CurrencyID = HeadrObj.FCurrencyID,
                BankGuarantee = BankGuaranteeData,
                WorkWithCostCenter = Company.WorkWithCostCenter,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency
            };

            return PartialView(TransVM);
        }
        [HttpGet]
        public ActionResult Detail(string id,int id2,int id3, int id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            var HeadrObj = _unitOfWork.Header.GetHeaderDataById(id, UserInfo.fCompanyId, id2,id3,id4);

            var TransObj = new List<Transaction>();

            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetCompanyTransactionKindAll(UserInfo.fCompanyId);
            var CompanyTransactionKindID = HeadrObj.CompanyTransactionKindNo;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            ViewBag.WorkWithCostCenter = Company.WorkWithCostCenter;
            if (Company.WorkWithCostCenter)
            {
                ViewBag.WorkWithCostCenter = "true";
            }else
            {
                ViewBag.WorkWithCostCenter = "false";
            }

            BankGuarantee ObjBankGuarantee = new BankGuarantee
            {
                CompanyID = HeadrObj.CompanyID,
                CompanyYear = HeadrObj.CompanyYear,
                VoucherNumber = HeadrObj.VoucherNumber

            };

            var BankGuaranteeData = _unitOfWork.BankGuarantee.GetBankGuarantee(ObjBankGuarantee);
            var TransVM = new TransActionVM
            {
                Header = HeadrObj,
                Transaction = TransObj,
                CompanyTransactionKind = CompanyTransactionKindObj,
                CompanyTransactionKindID = CompanyTransactionKindID,
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                CurrencyID = HeadrObj.FCurrencyID,
                BankGuarantee=BankGuaranteeData,
                WorkWithCostCenter = Company.WorkWithCostCenter,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency
        };

            return PartialView(TransVM);
        }
        [HttpGet]
        public JsonResult GetMaxVHIForAcc(int id,string id2,int id3)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            if (id != 0)
            {
               var ObjComapnyTransactionKind = _unitOfWork.CompanyTransactionKind.GetCompanyTransactionKindByID(UserInfo.fCompanyId, id);
                if (ObjComapnyTransactionKind.AutoSerial)
                {
                    var TransactionKindNo = _unitOfWork.CompanyTransactionKind.GetTransKindForCompanyTransactionKind(UserInfo.fCompanyId, id);
                    var VHI = _unitOfWork.Header.GetMaxVHByKind(UserInfo.fCompanyId, id, TransactionKindNo, id3).ToString();
                    return Json(VHI, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string SerialNumber = "";
                    int Serial = ObjComapnyTransactionKind.Serial;
                    string Symbol = ObjComapnyTransactionKind.Symbol;
                    DateTime dDate = DateTime.Now;
                    string sMonth = "";
                    string YearNo = dDate.ToString("yy");
                    if (id2 == "")
                    {
                        sMonth = dDate.ToString("MM");
                    }
                    else {
                        sMonth = id2;
                    }
                    int MonthNo = Int16.Parse(sMonth);
                    int LengthLastSerial = _unitOfWork.CompanyTransactionKindMonthlySerial.GetMaxSerial(UserInfo.fCompanyId, id, MonthNo).ToString().Length;
                    Serial = Serial - LengthLastSerial;
                    for (int i = 0; i <= Serial; i++)
                    {
                        if (i < Serial)
                        {
                            SerialNumber = SerialNumber + "0";
                        }
                        else if (i == Serial)
                        {
                            SerialNumber = SerialNumber + _unitOfWork.CompanyTransactionKindMonthlySerial.GetMaxSerial(UserInfo.fCompanyId, id, MonthNo).ToString();
                        }
                    }
                    var VHI = Symbol + YearNo + sMonth + SerialNumber;
                    return Json(VHI, JsonRequestBehavior.AllowGet);
                }
               
                
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        public ActionResult TransDash()
        {
             
            return View( );
        }
        [Authorize(Roles = "CoOwner,AddTransActionTrans")]
        public ActionResult Create()
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            int CurrYear = UserInfo.CurrYear;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var HeadrObj = new Header();
            HeadrObj.VoucherDate = DateTime.Now;
            var TransObj = new List<Transaction>();
             
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetCompanyTransactionKind(UserInfo.fCompanyId);
            
            var ObjComapnyTransactionKind = _unitOfWork.CompanyTransactionKind.GetCompanyTransactionKindByID(UserInfo.fCompanyId, 1);
            if (ObjComapnyTransactionKind.AutoSerial)
            {
                var TransactionKindNo = _unitOfWork.CompanyTransactionKind.GetTransKindForCompanyTransactionKind(UserInfo.fCompanyId, 1);
                HeadrObj.VoucherNumber = _unitOfWork.Header.GetMaxVHByKind(UserInfo.fCompanyId, TransactionKindNo, 1, CurrYear).ToString();
                HeadrObj.VHI = int.Parse(HeadrObj.VoucherNumber);
            }
            else
            {
                string SerialNumber = "";
                int Serial = ObjComapnyTransactionKind.Serial;
                string Symbol = ObjComapnyTransactionKind.Symbol;
                DateTime dDate = DateTime.Now;
                string sMonth = dDate.ToString("MM");
                string YearNo = dDate.ToString("yy");
                int MonthNo = Int16.Parse(sMonth);
                int LengthLastSerial = _unitOfWork.CompanyTransactionKindMonthlySerial.GetMaxSerial(UserInfo.fCompanyId, 1, MonthNo).ToString().Length;
                Serial = Serial - LengthLastSerial;
                for (int i = 0; i <= Serial; i++)
                {
                    if (i < Serial)
                    {
                        SerialNumber = SerialNumber + "0";
                    }
                    else if (i == Serial)
                    {
                        SerialNumber = SerialNumber + _unitOfWork.CompanyTransactionKindMonthlySerial.GetMaxSerial(UserInfo.fCompanyId, 1, MonthNo).ToString();
                    }
                }
                HeadrObj.VoucherNumber = Symbol + YearNo + sMonth + SerialNumber;
                HeadrObj.VHI = _unitOfWork.CompanyTransactionKindMonthlySerial.GetMaxSerial(UserInfo.fCompanyId, 1, MonthNo);
            }
            var TransVM = new TransActionVM
            {
                Header = HeadrObj,
                Transaction = TransObj,
                CompanyTransactionKind = CompanyTransactionKindObj,
                CompanyTransactionKindID = 1,
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                CurrencyID = 1,
                CurrencyNewValue = 1,
                WorkWithCostCenter = Company.WorkWithCostCenter,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency,
                CurrentYear = UserInfo.CurrYear,
                BankGuarantee = new BankGuarantee
                {
                    DueDate = DateTime.Now,
                    VoucherDate = DateTime.Now
                }
            };
            return View(TransVM);
        }
        [Authorize(Roles = "CoOwner,ShowTransActionTrans")]
        public ActionResult Index()
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetCompanyTransactionKind(UserInfo.fCompanyId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            DateTime Today = DateTime.Now;
            DateTime FromDate = DateTime.Now;
            DateTime ToDate = DateTime.Now;
            int CurrentYear = Today.Year;
            if (CurrentYear == UserInfo.CurrYear)
            {
                FromDate = DateTime.Now;
                ToDate = DateTime.Now;
            }
            else if (CurrentYear < UserInfo.CurrYear || CurrentYear > UserInfo.CurrYear)
            {
                FromDate = new DateTime(UserInfo.CurrYear, 1, 1);
                ToDate = new DateTime(UserInfo.CurrYear, 1, 1);
            }
            TransActionFilter Obj = new TransActionFilter
            {
                CompanyTransactionKind = CompanyTransactionKindObj,
                FromDate = FromDate,
                ToDate = ToDate,
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency,
                CompanyYear = UserInfo.CurrYear
            };
            return View(Obj);
        }
        [HttpPost]
        public JsonResult GetHeaders(TransActionFilter Obj)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var u = _unitOfWork.NativeSql.GetHeadersForReport(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
            if (u == null)
            {
                return Json(new List<TransActionFilter>(), JsonRequestBehavior.AllowGet);
            }
            if (!String.IsNullOrEmpty(Obj.VoucherNumber))
            {
                u = u.Where(m => m.VoucherNumber == Obj.VoucherNumber).ToList();
            }
            if (Obj.CurrencyID != 0)
            {
                u = u.Where(m => m.CurrencyID == Obj.CurrencyID).ToList();
            }
            if (Obj.CompanyTransactionKindNo != 0)
            {
                u = u.Where(m => m.CompanyTransactionKindNo == Obj.CompanyTransactionKindNo).ToList();
            }
            
            int iRow = 0;
            foreach (var iRowCount in u)
            {
                iRowCount.iRowTable = iRow;
                iRow = iRow + 1;
            }
            return Json(u, JsonRequestBehavior.AllowGet);
        }
        [ValidateInput(false)]
        public ActionResult GridViewTransActionPartial(string id, int? id2,int? id3, int? id4)
        {
            
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            ViewBag.WorkWithCostCenter = Company.WorkWithCostCenter;

            if (!String.IsNullOrEmpty(id))
            {
                int x = int.Parse(id2.ToString());
                int TrKind = int.Parse(id3.ToString());
                int Year = int.Parse(id4.ToString());
                var TransObj = _unitOfWork.NativeSql.GetTransactionsDetail(id, x, UserInfo.fCompanyId, TrKind, Year);
              
                return PartialView("GridViewTransActionPartial", TransObj);
            }
            else
            {
                var TransObj = new List<TransActionModelVM>();
                return PartialView("GridViewTransActionPartial", TransObj);
            }

            
        }
        public ActionResult AccLookup(int? key)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            ViewData["ID"] = key;
          //  var model = _unitOfWork.ChartOfAccount.GetAllChartOfAccount(UserInfo.fCompanyId); ;
            var model = _unitOfWork.NativeSql.GetTransActionAccount(UserInfo.fCompanyId);  
            return PartialView(model);
        }
        [HttpPost]
        public JsonResult SaveNewTransAction(TransActionVM ObjSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                int CurrYear = UserInfo.CurrYear;
                var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                var TranAction = ObjSave.Transaction;
                double TotalDebit = 0;
                double TotalCredit = 0;
                double TotalDebitForeign = 0;
                double TotalCreditForeign = 0;
                int iRow = 0;

                var AllAcc = _unitOfWork.NativeSql.GetTransActionAccount(UserInfo.fCompanyId);


                foreach (var Trans in TranAction)
                {
                    if (!String.IsNullOrWhiteSpace(Trans.AccountNumber))
                    {
                        iRow = iRow + 1;
                        try
                        {
                            TotalDebit = TotalDebit+Trans.Debit;
                            TotalCredit = TotalCredit+Trans.Credit;

                            if(Trans.Debit!=0)
                            {
                                TotalDebitForeign = TotalDebitForeign + Trans.CreditDebitForeign;
                            }
                            else
                            {
                                TotalCreditForeign = TotalCreditForeign + Trans.CreditDebitForeign;
                            }

                            if (AllAcc.FirstOrDefault(m => m.AccountNumber == Trans.AccountNumber) == null)
                            {
                                Msg.Msg = Resources.Resource.ThisAccountNoDoseNotExist + " : " + Trans.AccountNumber;
                                Msg.Code = 0;
                                return Json(Msg, JsonRequestBehavior.AllowGet);
                            }

                        }
                        catch(Exception ex)
                        {
                            Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                            Msg.Code = 0;
                            return Json(Msg, JsonRequestBehavior.AllowGet);

                        }
                    }

                }
                TotalCredit = Math.Round(TotalCredit, 3);
                TotalDebit = Math.Round(TotalDebit, 3);

                if (TotalCredit != TotalDebit)
                {
                    Msg.Msg = Resources.Resource.TheAccountingEntryIsUneven  ;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);

                }

                if (TotalCredit <= 0)
                {
                    Msg.Msg = Resources.Resource.CantBeZero;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }
                var HeaderTOSave = ObjSave.Header;
                HeaderTOSave.InsDateTime = DateTime.Now;
                HeaderTOSave.InsUserID = userId;
                HeaderTOSave.TransactionKindNo = _unitOfWork.CompanyTransactionKind.GetTransKindForCompanyTransactionKind(UserInfo.fCompanyId, HeaderTOSave.CompanyTransactionKindNo);
                HeaderTOSave.CompanyID = UserInfo.fCompanyId;
                HeaderTOSave.TotalCredit = TotalCredit;
                HeaderTOSave.TotalDebit = TotalDebit;
                HeaderTOSave.TotalDebitForeign = TotalDebitForeign;
                HeaderTOSave.TotalCreditForeign = TotalCreditForeign;
                HeaderTOSave.CompanyYear = CurrYear;
                
                var ObjComapnyTransactionKind = _unitOfWork.CompanyTransactionKind.GetCompanyTransactionKindByID(UserInfo.fCompanyId, HeaderTOSave.CompanyTransactionKindNo);
                if (ObjComapnyTransactionKind.AutoSerial)
                {
                    HeaderTOSave.VoucherNumber = _unitOfWork.Header.GetMaxVHByKind(UserInfo.fCompanyId, HeaderTOSave.CompanyTransactionKindNo, HeaderTOSave.TransactionKindNo, HeaderTOSave.CompanyYear).ToString();
                    HeaderTOSave.VHI = int.Parse(HeaderTOSave.VoucherNumber);
                }
                else
                {
                    string SerialNumber = "";
                    int Serial = ObjComapnyTransactionKind.Serial;
                    string Symbol = ObjComapnyTransactionKind.Symbol;
                    DateTime dDate = HeaderTOSave.VoucherDate;
                    string YearNo = dDate.ToString("yy");
                    string sMonth = dDate.ToString("MM");
                    int MonthNo = Int16.Parse(sMonth);
                    int LengthLastSerial = _unitOfWork.CompanyTransactionKindMonthlySerial.GetMaxSerial(UserInfo.fCompanyId, HeaderTOSave.CompanyTransactionKindNo, MonthNo).ToString().Length;
                    Serial = Serial - LengthLastSerial;
                    for (int i = 0; i <= Serial; i++)
                    {
                        if (i < Serial)
                        {
                            SerialNumber = SerialNumber + "0";
                        }
                        else if (i == Serial)
                        {
                            SerialNumber = SerialNumber + _unitOfWork.CompanyTransactionKindMonthlySerial.GetMaxSerial(UserInfo.fCompanyId, HeaderTOSave.CompanyTransactionKindNo, MonthNo).ToString();
                        }
                    }
                    HeaderTOSave.VoucherNumber = Symbol + YearNo + sMonth + SerialNumber;
                    HeaderTOSave.VHI = _unitOfWork.CompanyTransactionKindMonthlySerial.GetMaxSerial(UserInfo.fCompanyId, HeaderTOSave.CompanyTransactionKindNo, MonthNo);
                    _unitOfWork.CompanyTransactionKindMonthlySerial.Update(UserInfo.fCompanyId, HeaderTOSave.CompanyTransactionKindNo, MonthNo);
                }
                if (Company.DirectExportTheTransactions)
                {
                    HeaderTOSave.Exported = 1;
                }
                else
                {
                    HeaderTOSave.Exported = 0;
                }

                HeaderTOSave.RowCount = iRow;
                _unitOfWork.Header.Add(HeaderTOSave);
                _unitOfWork.Complete();
                  iRow = 0;
                foreach (var Trans in TranAction)
                {
                    if (!String.IsNullOrWhiteSpace(Trans.AccountNumber))
                    {
                        try
                        {
                            iRow = iRow + 1;
                            Trans.CompanyID = UserInfo.fCompanyId;
                            Trans.CompanyYear = HeaderTOSave.CompanyYear;
                            Trans.CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo;
                            Trans.TransactionKindNo = HeaderTOSave.TransactionKindNo;
                            Trans.VoucherDate = HeaderTOSave.VoucherDate;
                            Trans.VoucherNumber = HeaderTOSave.VoucherNumber;
                            Trans.InsUserID = HeaderTOSave.InsUserID;
                            Trans.VHI = HeaderTOSave.VHI;
                            Trans.InsDateTime = DateTime.Now;
                            Trans.RowNumber = iRow;

                            if (Trans.Debit > 0)
                            {
                                Trans.DebitForeign = Trans.CreditDebitForeign;
                            }
                            else
                            {
                                Trans.CreditForeign = Trans.CreditDebitForeign;
                            }
                            _unitOfWork.Transaction.Add(Trans);
                            _unitOfWork.Complete();
                            


                        }
                        catch (Exception ex)
                        {
                            Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                            Msg.Code = 0;
                            return Json(Msg, JsonRequestBehavior.AllowGet);

                        }


                    }




                }


                if (HeaderTOSave.TransactionKindNo == 14)
                {
                    BankGuarantee ObjBank = new BankGuarantee
                    {
                        CompanyID=HeaderTOSave.CompanyID,
                        CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo,
                        CompanyYear = HeaderTOSave.CompanyYear,
                        InsDateTime = HeaderTOSave.InsDateTime,
                        InsUserID = HeaderTOSave.InsUserID,
                      
                        TransactionKindNo = HeaderTOSave.TransactionKindNo,
                        VoucherDate = HeaderTOSave.VoucherDate,
                        VoucherNumber = HeaderTOSave.VoucherNumber,
                        IssuedBy = ObjSave.BankGuarantee.IssuedBy,
                        DepositValue = ObjSave.BankGuarantee.DepositValue,
                        DueDate = ObjSave.BankGuarantee.DueDate,
                        ExpensesAmount = ObjSave.BankGuarantee.ExpensesAmount,
                        OrderNo = ObjSave.BankGuarantee.OrderNo,
                        TheBeneficiary = ObjSave.BankGuarantee.TheBeneficiary,
                        WarrantyAmount = ObjSave.BankGuarantee.WarrantyAmount,
                        WarrantyNumber = ObjSave.BankGuarantee.WarrantyNumber
                    };
                    _unitOfWork.BankGuarantee.Add(ObjBank);
                    _unitOfWork.Complete();

                }
                var OldCurrency = _unitOfWork.NativeSql.GetCurrencyValueByID(UserInfo.fCompanyId, ObjSave.CurrencyID);
                if (OldCurrency.ConversionFactor != ObjSave.CurrencyNewValue)
                {
                    var NewCurr = new CurrencyValue
                    {
                        CurrencyID=ObjSave.CurrencyID,
                        CompanyID = UserInfo.fCompanyId,
                        ConversionFactor = ObjSave.CurrencyNewValue,
                        InsDateTime = DateTime.Now,
                        InsUserID = userId
                    };
                    _unitOfWork.CurrencyValue.Add(NewCurr);
                    _unitOfWork.Complete();
                }

                if (ObjComapnyTransactionKind.AutoSerial)
                {
                    Msg.LastID = _unitOfWork.Header.GetMaxVHByKind(UserInfo.fCompanyId, HeaderTOSave.CompanyTransactionKindNo, HeaderTOSave.TransactionKindNo, HeaderTOSave.CompanyYear).ToString();
                 }
                else
                {
                    string SerialNumber = "";
                    int Serial = ObjComapnyTransactionKind.Serial;
                    string Symbol = ObjComapnyTransactionKind.Symbol;
                    DateTime dDate = HeaderTOSave.VoucherDate;
                    string YearNo = dDate.ToString("yy");
                    string sMonth = dDate.ToString("MM");
                    int MonthNo = Int16.Parse(sMonth);
                    int LengthLastSerial = _unitOfWork.CompanyTransactionKindMonthlySerial.GetMaxSerial(UserInfo.fCompanyId, HeaderTOSave.CompanyTransactionKindNo, MonthNo).ToString().Length;
                    Serial = Serial - LengthLastSerial;
                    for (int i = 0; i <= Serial; i++)
                    {
                        if (i < Serial)
                        {
                            SerialNumber = SerialNumber + "0";
                        }
                        else if (i == Serial)
                        {
                            SerialNumber = SerialNumber + _unitOfWork.CompanyTransactionKindMonthlySerial.GetMaxSerial(UserInfo.fCompanyId, HeaderTOSave.CompanyTransactionKindNo, MonthNo).ToString();
                        }
                    }
                    Msg.LastID = Symbol + YearNo + sMonth + SerialNumber;
                }

                Msg.Code = 1;
                Msg.Msg = Resources.Resource.AddedSuccessfully;

                //nagam 


                Msg.Year = HeaderTOSave.CompanyYear;
                Msg.VoucherNumber = HeaderTOSave.VoucherNumber;
                Msg.TransactionKindNo = HeaderTOSave.TransactionKindNo.ToString();
                Msg.CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo.ToString();

                



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
        public JsonResult UpdateTransActionTrans(TransActionVM ObjSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var TranAction = ObjSave.Transaction;
                double TotalDebit = 0;
                double TotalCredit = 0;
                double TotalDebitForeign = 0;
                double TotalCreditForeign = 0;
                int iRow = 0;
                var AllAcc = _unitOfWork.NativeSql.GetTransActionAccount(UserInfo.fCompanyId);
                foreach (var Trans in TranAction)
                {
                    if (!String.IsNullOrWhiteSpace(Trans.AccountNumber))
                    {
                        iRow = iRow + 1;
                        try
                        {
                            TotalDebit = TotalDebit + Trans.Debit;
                            TotalCredit = TotalCredit + Trans.Credit;

                            if (Trans.Debit != 0)
                            {
                                TotalDebitForeign = TotalDebitForeign + Trans.CreditDebitForeign;
                            }
                            else
                            {
                                TotalCreditForeign = TotalCreditForeign + Trans.CreditDebitForeign;
                            }
                            if (AllAcc.FirstOrDefault(m => m.AccountNumber == Trans.AccountNumber) == null)
                            {
                                Msg.Msg = Resources.Resource.ThisAccountNoDoseNotExist + " : " + Trans.AccountNumber;
                                Msg.Code = 0;
                                return Json(Msg, JsonRequestBehavior.AllowGet);
                            }


                        }
                        catch (Exception ex)
                        {
                            Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                            Msg.Code = 0;
                            return Json(Msg, JsonRequestBehavior.AllowGet);

                        }


                    }




                }
                TotalCredit = Math.Round(TotalCredit, 3);
                TotalDebit = Math.Round(TotalDebit, 3);
                if (TotalCredit != TotalDebit)
                {
                    Msg.Msg = Resources.Resource.TheAccountingEntryIsUneven;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);

                }

                //---------------Save Header-------//
                var HeaderTOSave = ObjSave.Header;
                HeaderTOSave.InsDateTime = DateTime.Now;
                HeaderTOSave.InsUserID = userId;
                HeaderTOSave.CompanyID = UserInfo.fCompanyId;
                HeaderTOSave.TotalCredit = TotalCredit;
                HeaderTOSave.TotalDebit = TotalDebit;
                HeaderTOSave.TotalDebitForeign = TotalDebitForeign;
                HeaderTOSave.TotalCreditForeign = TotalCreditForeign;

                HeaderTOSave.CompanyYear = ObjSave.Header.CompanyYear ;
                HeaderTOSave.RowCount = iRow;
                _unitOfWork.Header.Update(HeaderTOSave);
                _unitOfWork.Complete();
                iRow = 0;
                _unitOfWork.NativeSql.DeleteTransActionTrans(HeaderTOSave.CompanyID, HeaderTOSave.VoucherNumber,
                    HeaderTOSave.CompanyTransactionKindNo, HeaderTOSave.TransactionKindNo, HeaderTOSave.CompanyYear);
                _unitOfWork.Complete();

                foreach (var Trans in TranAction)
                {
                    if (!String.IsNullOrWhiteSpace(Trans.AccountNumber))
                    {
                        try
                        {
                            iRow = iRow + 1;
                            Trans.CompanyID = UserInfo.fCompanyId;
                            Trans.CompanyYear = HeaderTOSave.CompanyYear;
                            Trans.CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo;
                            Trans.TransactionKindNo = HeaderTOSave.TransactionKindNo;
                            Trans.VoucherDate = HeaderTOSave.VoucherDate;
                            Trans.VoucherNumber = HeaderTOSave.VoucherNumber;
                            Trans.InsUserID = HeaderTOSave.InsUserID;
                            Trans.VHI = HeaderTOSave.VHI;
                            Trans.InsDateTime = DateTime.Now;
                            Trans.RowNumber = iRow;

                            if (Trans.Debit > 0)
                            {
                                Trans.DebitForeign = Trans.CreditDebitForeign;
                            }
                            else
                            {
                                Trans.CreditForeign = Trans.CreditDebitForeign;
                            }
                            _unitOfWork.Transaction.Add(Trans);
                            _unitOfWork.Complete();



                            if (HeaderTOSave.TransactionKindNo == 14)
                            {
                                BankGuarantee ObjBank = new BankGuarantee
                                {
                                    CompanyID = HeaderTOSave.CompanyID,
                                    CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo,
                                    CompanyYear = HeaderTOSave.CompanyYear,
                                    InsDateTime = HeaderTOSave.InsDateTime,
                                    InsUserID = HeaderTOSave.InsUserID,

                                    TransactionKindNo = HeaderTOSave.TransactionKindNo,
                                    VoucherDate = HeaderTOSave.VoucherDate,
                                    VoucherNumber = HeaderTOSave.VoucherNumber,
                                    IssuedBy = ObjSave.BankGuarantee.IssuedBy,
                                    DepositValue = ObjSave.BankGuarantee.DepositValue,
                                    DueDate = ObjSave.BankGuarantee.DueDate,
                                    ExpensesAmount = ObjSave.BankGuarantee.ExpensesAmount,
                                    OrderNo = ObjSave.BankGuarantee.OrderNo,
                                    TheBeneficiary = ObjSave.BankGuarantee.TheBeneficiary,
                                    WarrantyAmount = ObjSave.BankGuarantee.WarrantyAmount,
                                    WarrantyNumber = ObjSave.BankGuarantee.WarrantyNumber
                                };
                                _unitOfWork.BankGuarantee.Update(ObjBank);
                                _unitOfWork.Complete();

                            }



                        }
                        catch (Exception ex)
                        {
                            Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                            Msg.Code = 0;
                            return Json(Msg, JsonRequestBehavior.AllowGet);

                        }


                    }




                }


                var OldCurrency = _unitOfWork.NativeSql.GetCurrencyValueByID(UserInfo.fCompanyId, ObjSave.CurrencyID);
                if (OldCurrency.ConversionFactor != ObjSave.CurrencyNewValue)
                {
                    var NewCurr = new CurrencyValue
                    {
                        CurrencyID = ObjSave.CurrencyID,
                        CompanyID = UserInfo.fCompanyId,
                        ConversionFactor = ObjSave.CurrencyNewValue,
                        InsDateTime = DateTime.Now,
                        InsUserID = userId
                    };
                    _unitOfWork.CurrencyValue.Add(NewCurr);
                    _unitOfWork.Complete();
                }

                // end Change Currency--///






                Msg.LastID = "1";
                Msg.Code = 1;

                Msg.Year = HeaderTOSave.CompanyYear;
                Msg.VoucherNumber = HeaderTOSave.VoucherNumber;
                Msg.TransactionKindNo = HeaderTOSave.TransactionKindNo.ToString();

                Msg.CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo.ToString();


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
        public JsonResult UpdateOpeneinBalance(TransActionVM ObjSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                int CurrYear = UserInfo.CurrYear;


                var Y = _unitOfWork.NativeSql.GetAllYear();
                int Min = Y.Min(m => m.Year);
                if (Min != UserInfo.CurrYear)
                {
                    Msg.Msg = Resources.Resource.CantUpdateOpeningBalance  ;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }
                
               


                var TranAction = ObjSave.Transaction;
                double TotalDebit = 0;
                double TotalCredit = 0;
                double TotalDebitForeign = 0;
                double TotalCreditForeign = 0;
                int iRow = 0;
                ObjSave.Header.CompanyYear = CurrYear;
                ObjSave.Header.TransactionKindNo = _unitOfWork.CompanyTransactionKind.GetTransKindForCompanyTransactionKind(UserInfo.fCompanyId, ObjSave.Header.CompanyTransactionKindNo);
               var AllAcc = _unitOfWork.NativeSql.GetTransActionAccount(UserInfo.fCompanyId);
                TransActionDeleteVM ObjDelete = new TransActionDeleteVM
                {
                    CompanyID = UserInfo.fCompanyId,
                    CompanyTransactionKindNo = ObjSave.Header.CompanyTransactionKindNo,
                    CompanyYear = ObjSave.Header.CompanyYear,
                    TransactionKindNo = ObjSave.Header.TransactionKindNo,
                    VoucherNumber = ObjSave.Header.VoucherNumber
                };
                _unitOfWork.NativeSql.DeleteTransActionTrans(UserInfo.fCompanyId, ObjSave.Header.VoucherNumber,
                ObjSave.Header.CompanyTransactionKindNo, ObjSave.Header.TransactionKindNo, ObjSave.Header.CompanyYear);
                  _unitOfWork.Header.Delete(ObjDelete);
                _unitOfWork.Complete();


                foreach (var Trans in TranAction)
                {
                    if (!String.IsNullOrWhiteSpace(Trans.AccountNumber))
                    {
                        iRow = iRow + 1;
                        try
                        {
                            TotalDebit = TotalDebit + Trans.Debit;
                            TotalCredit = TotalCredit + Trans.Credit;

                            if (Trans.Debit != 0)
                            {
                                TotalDebitForeign = TotalDebitForeign + Trans.CreditDebitForeign;
                            }
                            else
                            {
                                TotalCreditForeign = TotalCreditForeign + Trans.CreditDebitForeign;
                            }

                            if (AllAcc.FirstOrDefault(m => m.AccountNumber == Trans.AccountNumber) == null)
                            {
                                Msg.Msg = Resources.Resource.ThisAccountNoDoseNotExist + " : " + Trans.AccountNumber;
                                Msg.Code = 0;
                                return Json(Msg, JsonRequestBehavior.AllowGet);
                            }
                        }
                        catch (Exception ex)
                        {
                            Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                            Msg.Code = 0;
                            return Json(Msg, JsonRequestBehavior.AllowGet);

                        }


                    }




                }
       

                //---------------Save Header-------//
                var HeaderTOSave = ObjSave.Header;
                HeaderTOSave.InsDateTime = DateTime.Now;
                HeaderTOSave.InsUserID = userId;
                HeaderTOSave.TransactionKindNo = _unitOfWork.CompanyTransactionKind.GetTransKindForCompanyTransactionKind(UserInfo.fCompanyId, HeaderTOSave.CompanyTransactionKindNo);
                HeaderTOSave.CompanyID = UserInfo.fCompanyId;
                HeaderTOSave.TotalCredit = TotalCredit;
                HeaderTOSave.TotalDebit = TotalDebit;
                HeaderTOSave.TotalDebitForeign = TotalDebitForeign;
                HeaderTOSave.TotalCreditForeign = TotalCreditForeign;

                HeaderTOSave.CompanyYear = CurrYear;
                HeaderTOSave.VoucherNumber = "1";
                HeaderTOSave.VHI = int.Parse(HeaderTOSave.VoucherNumber);
                HeaderTOSave.RowCount = iRow;
                _unitOfWork.Header.Add(HeaderTOSave);
                _unitOfWork.Complete();
                iRow = 0;
                foreach (var Trans in TranAction)
                {
                    if (!String.IsNullOrWhiteSpace(Trans.AccountNumber))
                    {
                        try
                        {
                            iRow = iRow + 1;
                            Trans.CompanyID = UserInfo.fCompanyId;
                            Trans.CompanyYear = HeaderTOSave.CompanyYear;
                            Trans.CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo;
                            Trans.TransactionKindNo = HeaderTOSave.TransactionKindNo;
                            Trans.VoucherDate = HeaderTOSave.VoucherDate;
                            Trans.VoucherNumber = HeaderTOSave.VoucherNumber;
                            Trans.InsUserID = HeaderTOSave.InsUserID;
                            Trans.VHI = HeaderTOSave.VHI;
                            Trans.InsDateTime = DateTime.Now;
                            Trans.RowNumber = iRow;

                            if (Trans.Debit > 0)
                            {
                                Trans.DebitForeign = Trans.CreditDebitForeign;
                            }
                            else
                            {
                                Trans.CreditForeign = Trans.CreditDebitForeign;
                            }
                            if ((Trans.Debit !=0) || (Trans.Credit != 0))
                            {
                                _unitOfWork.Transaction.Add(Trans);
                            }
                           
                            _unitOfWork.NativeSql.UpdateOpeiningBalance(Trans.CompanyID, Trans.CompanyYear, Trans.AccountNumber, Trans.Debit, Trans.Credit);

                            _unitOfWork.Complete();



                        }
                        catch (Exception ex)
                        {
                            Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                            Msg.Code = 0;
                            return Json(Msg, JsonRequestBehavior.AllowGet);

                        }


                    }




                }
                Msg.LastID = "1";
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
        public JsonResult DeleteTransActionTrans(TransActionDeleteVM ObjSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);

                ObjSave.CompanyID = UserInfo.fCompanyId;
                if (ObjSave.TransactionKindNo == (int)EnumTransKind.DepositInBank)
                {
                    
                    _unitOfWork.NativeSql.DeleteTransActionTrans(UserInfo.fCompanyId, ObjSave.VoucherNumber,
                     ObjSave.CompanyTransactionKindNo, ObjSave.TransactionKindNo, ObjSave.CompanyYear);
                    _unitOfWork.Header.Delete(ObjSave);
                 
                }
                else if (ObjSave.TransactionKindNo == (int)EnumTransKind.ReceiptBank)
                {

                    _unitOfWork.NativeSql.DeleteTransActionTrans(UserInfo.fCompanyId, ObjSave.VoucherNumber,
                     ObjSave.CompanyTransactionKindNo, ObjSave.TransactionKindNo, ObjSave.CompanyYear);
                    _unitOfWork.NativeSql.DeletePaper(UserInfo.fCompanyId, ObjSave.VoucherNumber,
                     ObjSave.CompanyTransactionKindNo, ObjSave.TransactionKindNo, ObjSave.CompanyYear);
                    _unitOfWork.Header.Delete(ObjSave);

                }
                else if (ObjSave.TransactionKindNo == (int)EnumTransKind.PaymentBank)
                {

                    _unitOfWork.NativeSql.DeleteTransActionTrans(UserInfo.fCompanyId, ObjSave.VoucherNumber,
                     ObjSave.CompanyTransactionKindNo, ObjSave.TransactionKindNo, ObjSave.CompanyYear);
                    _unitOfWork.NativeSql.DeletePaper(UserInfo.fCompanyId, ObjSave.VoucherNumber,
                     ObjSave.CompanyTransactionKindNo, ObjSave.TransactionKindNo, ObjSave.CompanyYear);
                    _unitOfWork.Header.Delete(ObjSave);

                }
                else

                {
                    _unitOfWork.BankGuarantee.Delete(ObjSave);
                    _unitOfWork.NativeSql.DeleteTransActionTrans(UserInfo.fCompanyId, ObjSave.VoucherNumber,
                      ObjSave.CompanyTransactionKindNo, ObjSave.TransactionKindNo, ObjSave.CompanyYear);
                    _unitOfWork.Header.Delete(ObjSave);
                }




              
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
        [Authorize(Roles = "CoOwner,UpdateTransActionTrans")]
        public ActionResult Update(string  id, int id2,int id3,int id4)
        {

            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var HeadrObj = _unitOfWork.Header.GetHeaderDataById(id, UserInfo.fCompanyId, id2,id3,id4);

            var TransObj = new List<Transaction>();

            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetCompanyTransactionKind(UserInfo.fCompanyId);
            var CompanyTransactionKindID = HeadrObj.CompanyTransactionKindNo;
            BankGuarantee ObjBankGuarantee = new BankGuarantee
            {
                CompanyID = HeadrObj.CompanyID,
                CompanyYear = HeadrObj.CompanyYear,
                VoucherNumber = HeadrObj.VoucherNumber

            };

            var BankGuaranteeData = _unitOfWork.BankGuarantee.GetBankGuarantee(ObjBankGuarantee);
            if (BankGuaranteeData == null)
            {
                BankGuaranteeData = new BankGuarantee();
                BankGuaranteeData.DueDate = HeadrObj.VoucherDate;
            }
            var TransVM = new TransActionVM
            {
                Header = HeadrObj,
                Transaction = TransObj,
                CompanyTransactionKind = CompanyTransactionKindObj,
                CompanyTransactionKindID = CompanyTransactionKindID,
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                CurrencyID = HeadrObj.FCurrencyID,
                BankGuarantee = BankGuaranteeData,
                NetTotal = HeadrObj.TotalDebit - HeadrObj.TotalCredit,
                sNetTotal = (HeadrObj.TotalDebit - HeadrObj.TotalCredit).ToString(),
                TotalCredit = HeadrObj.TotalCredit.ToString(),
                TotalDebit = HeadrObj.TotalDebit.ToString(),
                WorkWithCostCenter = Company.WorkWithCostCenter,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency
            };

            return View(TransVM);
        }
        [Authorize(Roles = "CoOwner,CopyTransActionTrans")]
        public ActionResult Copy(string id, int id2, int id3,int id4)
        {

            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var HeadrObj = _unitOfWork.Header.GetHeaderDataById(id, UserInfo.fCompanyId, id2, id3,id4);

            var TransObj = new List<Transaction>();

            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetCompanyTransactionKind(UserInfo.fCompanyId);
            var CompanyTransactionKindID = HeadrObj.CompanyTransactionKindNo;
            BankGuarantee ObjBankGuarantee = new BankGuarantee
            {
                CompanyID = HeadrObj.CompanyID,
                CompanyYear = HeadrObj.CompanyYear,
                VoucherNumber = HeadrObj.VoucherNumber

            };

            var BankGuaranteeData = _unitOfWork.BankGuarantee.GetBankGuarantee(ObjBankGuarantee);
            if (BankGuaranteeData == null)
            {
                BankGuaranteeData = new BankGuarantee();
                BankGuaranteeData.DueDate = HeadrObj.VoucherDate;
            }
            var TransVM = new TransActionVM
            {
                Header = HeadrObj,
                Transaction = TransObj,
                CompanyTransactionKind = CompanyTransactionKindObj,
                CompanyTransactionKindID = CompanyTransactionKindID,
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                CurrencyID = HeadrObj.FCurrencyID,
                BankGuarantee = BankGuaranteeData,
                NetTotal = HeadrObj.TotalDebit - HeadrObj.TotalCredit,
                sNetTotal = (HeadrObj.TotalDebit - HeadrObj.TotalCredit).ToString(),
                TotalCredit = HeadrObj.TotalCredit.ToString(),
                TotalDebit = HeadrObj.TotalDebit.ToString(),
                WorkWithCostCenter = Company.WorkWithCostCenter,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency
            };

            return View(TransVM);
        }
        [Authorize(Roles = "CoOwner,AttachTransActionTrans")]
        public ActionResult ShowAttach(int id, string id2, string id3, string id4)
        {

            TransActionFileVM Obj = new TransActionFileVM
            {
                Year = id,
                VoucherNumber = id2,
                TransactionKindNo = id4,
                CompanyTransactionKindNo = id3
            };

            // select* from TransActionFiles where Year = id and VoucherNumber = id2 and CompanyTransactionKindNo = id3 and TransactionKindNo = id4
            return View("ShowAttach", Obj);

        }
        [HttpGet]
        public JsonResult GetTransActionsDetail(string id, int id2, int id3,int id4)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var u = _unitOfWork.NativeSql.GetTransactionsDetail(id, id2, UserInfo.fCompanyId, id3,id4);

 
            if (u == null)
            {
                return Json(new TransActionVM(), JsonRequestBehavior.AllowGet);
            }
            
            return Json(u, JsonRequestBehavior.AllowGet);



        }
        [HttpGet]
        public JsonResult GetTransactionKindNo(int id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var TransactionKindNo = _unitOfWork.CompanyTransactionKind.GetTransKindForCompanyTransactionKind(UserInfo.fCompanyId, id);
 
           return Json(TransactionKindNo, JsonRequestBehavior.AllowGet);
       
        }
        [Authorize(Roles = "CoOwner,ExportTransActionTrans")]
        public ActionResult ExportTransaction()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetCompanyTransactionKind(UserInfo.fCompanyId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            DateTime Today = DateTime.Now;
            DateTime FromDate = DateTime.Now;
            DateTime ToDate = DateTime.Now;
            int CurrentYear = Today.Year;
            if (CurrentYear == UserInfo.CurrYear)
            {
                FromDate = DateTime.Now;
                ToDate = DateTime.Now;
            }
            else if (CurrentYear < UserInfo.CurrYear || CurrentYear > UserInfo.CurrYear)
            {
                FromDate = new DateTime(UserInfo.CurrYear, 1, 1);
                ToDate = new DateTime(UserInfo.CurrYear, 1, 1);
            }
            var TransActionVM = new TransActionVM
            {
                FromDate = FromDate,
                ToDate = ToDate,
                CompanyTransactionKind = CompanyTransactionKindObj,
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency,
                CompanyYear = UserInfo.CurrYear
            };
            return View(TransActionVM);
        }
        [HttpPost]
        public JsonResult GetAllTransactionUnExport(TransActionVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllTransaction = _unitOfWork.NativeSql.GetAllTransactionFromHeaderUnExport(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                if (AllTransaction == null)
                {
                    return Json(new List<TransActionVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.VoucherNumber))
                {
                    AllTransaction = AllTransaction.Where(m => m.VoucherNumber == Obj.VoucherNumber).ToList();
                }
                if (Obj.CurrencyID != 0)
                {
                    AllTransaction = AllTransaction.Where(m => m.CurrencyID == Obj.CurrencyID).ToList();
                }
                if (Obj.CompanyTransactionKindNo != 0)
                {
                    AllTransaction = AllTransaction.Where(m => m.CompanyTransactionKindNo == Obj.CompanyTransactionKindNo).ToList();
                }
                int iRow = 0;
                foreach (var iRowCount in AllTransaction)
                {
                    iRowCount.iRowTable = iRow;
                    iRow = iRow + 1;
                }
                return Json(AllTransaction, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<TransActionVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [Authorize(Roles = "CoOwner,UnExportTransActionTrans")]
        public ActionResult UnExportTransaction()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetCompanyTransactionKind(UserInfo.fCompanyId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            DateTime Today = DateTime.Now;
            DateTime FromDate = DateTime.Now;
            DateTime ToDate = DateTime.Now;
            int CurrentYear = Today.Year;
            if (CurrentYear == UserInfo.CurrYear)
            {
                FromDate = DateTime.Now;
                ToDate = DateTime.Now;
            }
            else if (CurrentYear < UserInfo.CurrYear || CurrentYear > UserInfo.CurrYear)
            {
                FromDate = new DateTime(UserInfo.CurrYear, 1, 1);
                ToDate = new DateTime(UserInfo.CurrYear, 1, 1);
            }
            var TransActionVM = new TransActionVM
            {
                FromDate = FromDate,
                ToDate = ToDate,
                CompanyTransactionKind = CompanyTransactionKindObj,
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency,
                CompanyYear = UserInfo.CurrYear
            };
            return View(TransActionVM);
        }
        [HttpPost]
        public JsonResult GetAllTransactionExport(TransActionVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllTransaction = _unitOfWork.NativeSql.GetAllTransactionFromHeaderExport(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                if (AllTransaction == null)
                {
                    return Json(new List<TransActionVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.VoucherNumber))
                {
                    AllTransaction = AllTransaction.Where(m => m.VoucherNumber == Obj.VoucherNumber).ToList();
                }
                if (Obj.CurrencyID != 0)
                {
                    AllTransaction = AllTransaction.Where(m => m.CurrencyID == Obj.CurrencyID).ToList();
                }
                if (Obj.CompanyTransactionKindNo != 0)
                {
                    AllTransaction = AllTransaction.Where(m => m.CompanyTransactionKindNo == Obj.CompanyTransactionKindNo).ToList();
                }
                int iRow = 0;
                foreach (var iRowCount in AllTransaction)
                {
                    iRowCount.iRowTable = iRow;
                    iRow = iRow + 1;
                }
                return Json(AllTransaction, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<TransActionVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public JsonResult UpdateToExport(ExportAndUnExportVM ObjToUpdate)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                foreach (var UpdateHeader in ObjToUpdate.Header)
                {
                    UpdateHeader.Exported = 1;
                    UpdateHeader.CompanyID = UserInfo.fCompanyId;
                    _unitOfWork.Header.UpdateToExportAndUnExport(UpdateHeader);
                    _unitOfWork.Complete();
                }
                Msg.Code = 1;
                Msg.Msg = Resources.Resource.ExportSuccessfully;
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
        public JsonResult UpdateToUnExport(ExportAndUnExportVM ObjToUpdate)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                foreach (var UpdateHeader in ObjToUpdate.Header)
                {
                    UpdateHeader.Exported = 0;
                    UpdateHeader.CompanyID = UserInfo.fCompanyId;
                    _unitOfWork.Header.UpdateToExportAndUnExport(UpdateHeader);
                    _unitOfWork.Complete();
                }
                Msg.Code = 1;
                Msg.Msg = Resources.Resource.UnExportSuccessfully;
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