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
    public class ResettingAccountsController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public ResettingAccountsController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        // GET: ResettingAccounts

        [HttpGet]
        public JsonResult GetAllAccountType()
        {
            try
            {


                var userId = User.Identity.GetUserId();


 


                var R = _unitOfWork.AccountType.GetAllAccountTypeVM();
                int i = 0;
                foreach( var t in R)
                {
                    t.iRow = i;
                    i = i + 1;
                }
                return Json(R, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<AccountType>(), JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            ResettingAccountsVM Obj = new ResettingAccountsVM
            {
                FromYear = UserInfo.CurrYear - 1 ,
                ToYear = UserInfo.CurrYear,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency
            };
            return View(Obj);
        }
        [HttpPost]
        [Authorize(Roles = "CoOwner")]
        public JsonResult GetAccount(ResettingAccountsVM ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                double SumSelected = 0;
                if (ObjToSave == null)
                {
                    
                    return Json(new List<ResttingAccountVM>(), JsonRequestBehavior.AllowGet);
                }
                
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

                string AccountType = "";
                if (ObjToSave.AccountType != null)
                {
                    foreach (var a in ObjToSave.AccountType)
                    {
                        AccountType = AccountType + a.AccountTypeID + ",";

                    }
                    AccountType = AccountType.Remove(AccountType.Length - 1, 1);

                    var d = _unitOfWork.NativeSql.GetAllAcountToResstting(AccountType, 0, UserInfo.CurrYear-1);
                    if (d != null)
                    {
                        d = d.Where(m => m.Total != 0).ToList();
                        SumSelected = d.Sum(m => m.Total);
                    }
                    else
                    {
                        SumSelected = 0;
                    }
                }
               
                 

              

               
           

                var d2= _unitOfWork.NativeSql.GetAllAcountToResstting(AccountType,1,UserInfo.CurrYear-1);
                d2 = d2.Where(m => m.Total != 0).ToList();
                foreach (var t in d2)
                {
                    if (t.Total < 0)
                    {
                        t.Credit =Math.Abs(t.Total);
                    }
                    else
                    {
                        t.Debit = Math.Abs(t.Total);
                    }
                }



                ResttingAccountVM SelectedAccountData = _unitOfWork.NativeSql.GetAllAcountToRessttingByAccount(ObjToSave.AccountNumber, UserInfo.CurrYear);
                if (SelectedAccountData == null)
                {
                    SelectedAccountData = new ResttingAccountVM
                    {
                        AccountNumber = ObjToSave.AccountNumber,
                        ArabicName = ObjToSave.AccountName,
                        
                        Total = 0
                    };
                }



                SelectedAccountData.Total = SelectedAccountData.Total + SumSelected;
                if (SelectedAccountData.Total < 0)
                {
                    SelectedAccountData.Credit = Math.Abs(SelectedAccountData.Total);
                }
                else
                {
                    SelectedAccountData.Debit = Math.Abs(SelectedAccountData.Total);
                }

                var dataList = d2.ToList();
                dataList.Add(SelectedAccountData);

                return Json(dataList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                Msg.Code = 0;
                return Json(new List<ResttingAccountVM>(), JsonRequestBehavior.AllowGet);
            }

        }



        [HttpPost]
        public JsonResult UpdateOpeneinBalance(IEnumerable<ResttingAccountVM> Obj)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                TransActionVM ObjSave = new TransActionVM();

                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                int CurrYear = UserInfo.CurrYear;

                var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetCompanyTransactionKindAll(UserInfo.fCompanyId);
                CompanyTransactionKindObj = CompanyTransactionKindObj.Where(m => m.TransactionKindID == (int)EnumTransKind.OpenBalance);
                var CompanyTransactionKindID = _unitOfWork.CompanyTransactionKind.GetCompanyTransactionKindByTransKindNo(UserInfo.fCompanyId, (int)EnumTransKind.OpenBalance);
        


                ObjSave.Header = new Header()
                {
                    VoucherNumber="1",
                    VHI = 1,
                    VoucherDate =DateTime.Parse(("31/12/"+(CurrYear-1).ToString())),
                    CompanyTransactionKindNo= CompanyTransactionKindID


                };
                ObjSave.Transaction = new List<Transaction>();

                List<Transaction> ListTrans = new List<Transaction>();
                foreach(var t in Obj)
                {
                    if (!String.IsNullOrEmpty(t.AccountNumber))
                    {
                        Transaction n = new Transaction();
                        n.AccountNumber = t.AccountNumber;
                        n.Debit = t.Debit;
                        n.Credit = t.Credit;

                        ListTrans.Add(n);

                    }


                }
                ObjSave.Transaction = ListTrans;
                //VoucherNumber: $("#VoucherNumber").val(),
                //        VoucherDate: $("#VoucherDate").val(),

                //        TotalCredit: $("#TotalCredit").val(),
                //        TotalDebit: $("#TotalDebit").val(),
                //        CompanyTransactionKindNo: $("#CompanyTransactionKindID").val(),
                //        ConversionFactor: $("#CurrencyNewValue").val(),

                //        TransactionKindNo: $("#TransactionKindNo").val()


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
                HeaderTOSave.VoucherNumber = "1";//_unitOfWork.Header.GetMaxVHByKind(UserInfo.fCompanyId, HeaderTOSave.CompanyTransactionKindNo, HeaderTOSave.TransactionKindNo).ToString();
                HeaderTOSave.VHI = int.Parse(HeaderTOSave.VoucherNumber);
                HeaderTOSave.RowCount = iRow;
                //dfkfjkdjfkdj
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
                            if ((Trans.Debit != 0) || (Trans.Credit != 0))
                            {
                                _unitOfWork.Transaction.Add(Trans);
                            }

                         //   _unitOfWork.NativeSql.UpdateOpeiningBalance(Trans.CompanyID, Trans.CompanyYear, Trans.AccountNumber, Trans.Debit, Trans.Credit);

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


                Msg.LastID = "1";//_unitOfWork.Header.GetMaxVHByKind(UserInfo.fCompanyId, HeaderTOSave.CompanyTransactionKindNo, HeaderTOSave.TransactionKindNo).ToString(); ;
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