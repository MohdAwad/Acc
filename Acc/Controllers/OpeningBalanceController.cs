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
    public class OpeningBalanceController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public OpeningBalanceController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        // GET: OpeningBalance


        [Authorize(Roles = "CoOwner,ShowOpeningBalance")]
        public ActionResult Update()
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var HeadrObj = new Header();
            int CurrYear = UserInfo.CurrYear;
            HeadrObj.VoucherDate =DateTime.Parse("31/12/"+(CurrYear-1).ToString());
            var TransObj = new List<Transaction>();

            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetCompanyTransactionKindAll(UserInfo.fCompanyId);
            CompanyTransactionKindObj = CompanyTransactionKindObj.Where(m => m.TransactionKindID == (int)EnumTransKind.OpenBalance);
            var CompanyTransactionKindID =  _unitOfWork.CompanyTransactionKind.GetCompanyTransactionKindByTransKindNo(UserInfo.fCompanyId, (int)EnumTransKind.OpenBalance);
            HeadrObj.VoucherNumber = "1";// _unitOfWork.Header.GetMaxVHByKind(UserInfo.fCompanyId, CompanyTransactionKindID,(int)EnumTransKind.OpenBalance).ToString();
            HeadrObj.VHI = int.Parse(HeadrObj.VoucherNumber);


            DateTime dt = DateTime.Parse("31/12/" + (UserInfo.CurrYear - 1).ToString());
            var Data = _unitOfWork.NativeSql.GetOpenBalance(UserInfo.fCompanyId, dt, dt);


           // var Data = _unitOfWork.NativeSql.GetTransActionAccountForBalance(UserInfo.fCompanyId);
            var OpeningBalanceCredit = Data.Sum(m => m.CreditBalance);
            var OpeningBalanceDebit = Data.Sum(m => m.DebitBalance);
            HeadrObj.TotalCredit = OpeningBalanceCredit;
            HeadrObj.TotalDebit = OpeningBalanceDebit;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var Obj = new OpeningBalanceVM
            {
                Header = HeadrObj,
                Transaction = TransObj,
                CompanyTransactionKind = CompanyTransactionKindObj,
                CompanyTransactionKindID = CompanyTransactionKindID,
                CurrencyNewValue = 0,
                Credit = OpeningBalanceCredit,
                Debit = OpeningBalanceDebit,
                NetTOT = OpeningBalanceDebit - OpeningBalanceCredit,
                TotalCredit = OpeningBalanceCredit.ToString(),
                TotalDebit = OpeningBalanceDebit.ToString(),
                sNetTotal = (OpeningBalanceDebit - OpeningBalanceCredit).ToString(),
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency
            };

         
            return View(Obj);
        }
        [ValidateInput(false)]
        public ActionResult GridViewTransActionPartial(string id, string id2)
        {

            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            //    var Data = _unitOfWork.NativeSql.GetTransActionAccountForBalance(UserInfo.fCompanyId);

            DateTime dt = new DateTime();
            dt = DateTime.Parse("31/12/" + (UserInfo.CurrYear - 1).ToString());


            var Y = _unitOfWork.NativeSql.GetAllYear();
            int Min = Y.Min(m => m.Year);
            if (Min != UserInfo.CurrYear)
            {
                var Data = _unitOfWork.NativeSql.GetOpenBalance(UserInfo.fCompanyId, dt, dt);
                Data = Data.OrderBy(m => m.AccountNumber).ToList();
                var TransObj = new List<OpeningBalanceVM>();
                if (Data.Count() > 0)
                {
                    foreach (var Acc in Data)
                    {
                        var Obj = new OpeningBalanceVM();
                        Obj.AccountNumber = Acc.AccountNumber;
                        Obj.AccountName = Acc.AccountName;
                        Obj.Credit = Acc.CreditBalance;
                        Obj.Debit = Acc.DebitBalance;
                        if (Acc.DebitBalance == 0)
                            Obj.sDebit = "";
                        else
                            Obj.sDebit = Acc.DebitBalance.ToString();
                        TransObj.Add(Obj);
                    }



                    return PartialView("GridViewTransActionPartial", TransObj);
                }
                else
                {

                    return PartialView("GridViewTransActionPartial", TransObj);
                }
            }
            else 
            {
                var Data = _unitOfWork.NativeSql.GetTransActionAccountForBalance(UserInfo.fCompanyId);
                Data = Data.OrderBy(m => m.AccountNumber).ToList();
                var TransObj = new List<OpeningBalanceVM>();
                if (Data.Count() > 0)
                {
                    foreach (var Acc in Data)
                    {
                        var Obj = new OpeningBalanceVM();
                        Obj.AccountNumber = Acc.AccountNumber;
                        Obj.AccountName = Acc.ArabicName;
                        Obj.Credit = Acc.OpeningBalanceCredit;
                        Obj.Debit = Acc.OpeningBalanceDebit;
                        if (Acc.OpeningBalanceDebit == 0)
                            Obj.sDebit = "";
                        else
                            Obj.sDebit = Acc.OpeningBalanceDebit.ToString();
                        TransObj.Add(Obj);
                    }



                    return PartialView("GridViewTransActionPartial", TransObj);
                }
                else
                {

                    return PartialView("GridViewTransActionPartial", TransObj);
                }
            }
         


        }
    }
}