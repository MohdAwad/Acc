using DevExpress.Web.Mvc;
using Acc.Persistence;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Acc.Models;
using Acc.Helpers;
using Acc.ViewModels;

namespace Acc.Controllers
{
    [Authorize]
    public class ServiceBillController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public ServiceBillController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        [Authorize(Roles = "CoOwner,AddServiceBill")]
        public ActionResult NewRetrunMultiPurchase()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            int CurrYear = UserInfo.CurrYear;
            var ObjGet = _unitOfWork.DefinitionOtherAccount.GetDefinitionOtherAccountByID(UserInfo.fCompanyId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            FixAccountInfoVM ObjFix = new FixAccountInfoVM
            {
                AccountNo = "",
                AccountName = ""
            };
            if (ObjGet != null)
            {
                ObjFix.AccountNo = ObjGet.ReturnPurchasesTaxAccountNumber;
                ObjFix.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.ReturnPurchasesTaxAccountNumber);
            }
            var CompanyTransactionKind = _unitOfWork.NativeSql.GetCompanyTransactionKindAll(UserInfo.fCompanyId);
            CompanyTransactionKind = CompanyTransactionKind.Where(m => m.TransactionKindID == 22).ToList();
            HeaderServiceBillVM Obj = new HeaderServiceBillVM
            {
                BillDate = DateTime.Now,
                TaxAccountNumber = ObjFix.AccountNo,
                TaxAccountName = ObjFix.AccountName,
                OrignailTaxAccountNumber = ObjFix.AccountNo,
                OrignailTaxAccountName = ObjFix.AccountName,
                CompanyTransactionKind = CompanyTransactionKind,
                CompanyTransactionKindID = 1,
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                CurrencyID = 1,
                WorkWithCostCenter = Company.WorkWithCostCenter,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency,
                CurrentYear = UserInfo.CurrYear

            };
            return View(Obj);
        }
        [Authorize(Roles = "CoOwner,AddServiceBill")]
        public ActionResult NewRetrunPurchase()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            int CurrYear = UserInfo.CurrYear;
            var ObjGet = _unitOfWork.DefinitionOtherAccount.GetDefinitionOtherAccountByID(UserInfo.fCompanyId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            FixAccountInfoVM ObjFix = new FixAccountInfoVM
            {
                AccountNo = "",
                AccountName = ""
            };
            if (ObjGet != null)
            {
                ObjFix.AccountNo = ObjGet.ReturnPurchasesTaxAccountNumber;
                ObjFix.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.ReturnPurchasesTaxAccountNumber);
            }
            var CompanyTransactionKind = _unitOfWork.NativeSql.GetCompanyTransactionKindAll(UserInfo.fCompanyId);
            CompanyTransactionKind = CompanyTransactionKind.Where(m => m.TransactionKindID == 21).ToList();
            HeaderServiceBillVM Obj = new HeaderServiceBillVM
            {
                BillDate = DateTime.Now,
                TaxAccountNumber = ObjFix.AccountNo,
                TaxAccountName = ObjFix.AccountName,
                OrignailTaxAccountNumber = ObjFix.AccountNo,
                OrignailTaxAccountName = ObjFix.AccountName,
                CompanyTransactionKind = CompanyTransactionKind,
                CompanyTransactionKindID = 1,
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                CurrencyID = 1,
                WorkWithCostCenter = Company.WorkWithCostCenter,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency,
                CurrentYear = UserInfo.CurrYear

            };
            return View(Obj);
        }
        [Authorize(Roles = "CoOwner,AddServiceBill")]
        public ActionResult NewMultiPurchase()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            int CurrYear = UserInfo.CurrYear;
            var ObjGet = _unitOfWork.DefinitionOtherAccount.GetDefinitionOtherAccountByID(UserInfo.fCompanyId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            FixAccountInfoVM ObjFix = new FixAccountInfoVM
            {
                AccountNo = "",
                AccountName = ""
            };
            if (ObjGet != null)
            {
                ObjFix.AccountNo = ObjGet.PurchasesTaxAccountNumber;
                ObjFix.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.PurchasesTaxAccountNumber);
            }
            var CompanyTransactionKind = _unitOfWork.NativeSql.GetCompanyTransactionKindAll(UserInfo.fCompanyId);
            CompanyTransactionKind = CompanyTransactionKind.Where(m => m.TransactionKindID == 13).ToList();
            HeaderServiceBillVM Obj = new HeaderServiceBillVM
            {
                BillDate = DateTime.Now,
                TaxAccountNumber = ObjFix.AccountNo,
                TaxAccountName = ObjFix.AccountName,
                OrignailTaxAccountNumber = ObjFix.AccountNo,
                OrignailTaxAccountName = ObjFix.AccountName,
                CompanyTransactionKind = CompanyTransactionKind,
                CompanyTransactionKindID = 1,
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                CurrencyID = 1,
                WorkWithCostCenter = Company.WorkWithCostCenter,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency,
                CurrentYear = UserInfo.CurrYear

            };
            return View(Obj);
        }
        [Authorize(Roles = "CoOwner,AddServiceBill")]
        public ActionResult NewPurchase()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            int CurrYear = UserInfo.CurrYear;
            var ObjGet = _unitOfWork.DefinitionOtherAccount.GetDefinitionOtherAccountByID(UserInfo.fCompanyId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            FixAccountInfoVM ObjFix = new FixAccountInfoVM
            {
                AccountNo = "",
                AccountName = ""
            };
            if (ObjGet != null)
            {
                ObjFix.AccountNo = ObjGet.PurchasesTaxAccountNumber;
                ObjFix.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.PurchasesTaxAccountNumber);
            }
            var CompanyTransactionKind = _unitOfWork.NativeSql.GetCompanyTransactionKindAll(UserInfo.fCompanyId);
            CompanyTransactionKind = CompanyTransactionKind.Where(m => m.TransactionKindID == 12).ToList();
            HeaderServiceBillVM Obj = new HeaderServiceBillVM
            {
                BillDate = DateTime.Now,
                TaxAccountNumber = ObjFix.AccountNo,
                TaxAccountName = ObjFix.AccountName,
                OrignailTaxAccountNumber = ObjFix.AccountNo,
                OrignailTaxAccountName = ObjFix.AccountName,
                CompanyTransactionKind = CompanyTransactionKind,
                CompanyTransactionKindID = 1,
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                CurrencyID = 1,
                WorkWithCostCenter = Company.WorkWithCostCenter,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency,
                CurrentYear = UserInfo.CurrYear

            };
            return View(Obj);
        }
        [Authorize(Roles = "CoOwner,AddServiceBill")]
        public ActionResult NewReturn()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            int CurrYear = UserInfo.CurrYear;
            var ObjGet = _unitOfWork.DefinitionOtherAccount.GetDefinitionOtherAccountByID(UserInfo.fCompanyId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            FixAccountInfoVM ObjFix = new FixAccountInfoVM
            {
                AccountNo = "",
                AccountName = ""
            };
            if (ObjGet != null)
            {
                ObjFix.AccountNo = ObjGet.ReturnSalesTaxAccountNumber;
                ObjFix.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.ReturnSalesTaxAccountNumber);
            }
            var CompanyTransactionKind = _unitOfWork.NativeSql.GetCompanyTransactionKindAll(UserInfo.fCompanyId);
            CompanyTransactionKind = CompanyTransactionKind.Where(m => m.TransactionKindID == 19).ToList();
            HeaderServiceBillVM Obj = new HeaderServiceBillVM
            {
                BillDate = DateTime.Now,
                TaxAccountNumber = ObjFix.AccountNo,
                TaxAccountName = ObjFix.AccountName,
                OrignailTaxAccountNumber = ObjFix.AccountNo,
                OrignailTaxAccountName = ObjFix.AccountName,
                CompanyTransactionKind = CompanyTransactionKind,
                CompanyTransactionKindID = 1,
                SaleMan = _unitOfWork.Sale.GetAllSale(UserInfo.fCompanyId),
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                CurrencyID = 1,
                WorkWithCostCenter = Company.WorkWithCostCenter,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency,
                CurrentYear = UserInfo.CurrYear

            };
            return View(Obj);
        }
        [Authorize(Roles = "CoOwner,AddServiceBill")]
        public ActionResult NewMultiReturn()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            int CurrYear = UserInfo.CurrYear;
            var ObjGet = _unitOfWork.DefinitionOtherAccount.GetDefinitionOtherAccountByID(UserInfo.fCompanyId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            FixAccountInfoVM ObjFix = new FixAccountInfoVM
            {
                AccountNo = "",
                AccountName = ""
            };
            if (ObjGet != null)
            {
                ObjFix.AccountNo = ObjGet.ReturnSalesTaxAccountNumber;
                ObjFix.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.ReturnSalesTaxAccountNumber);
            }
            var CompanyTransactionKind = _unitOfWork.NativeSql.GetCompanyTransactionKindAll(UserInfo.fCompanyId);
            CompanyTransactionKind = CompanyTransactionKind.Where(m => m.TransactionKindID == 20).ToList();
            HeaderServiceBillVM Obj = new HeaderServiceBillVM
            {
                BillDate = DateTime.Now,
                BillID = 0,
                TaxAccountNumber = ObjFix.AccountNo,
                TaxAccountName = ObjFix.AccountName,
                OrignailTaxAccountNumber = ObjFix.AccountNo,
                OrignailTaxAccountName = ObjFix.AccountName,
                CompanyTransactionKind = CompanyTransactionKind,
                CompanyTransactionKindID = 1,
                SaleMan = _unitOfWork.Sale.GetAllSale(UserInfo.fCompanyId),
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                CurrencyID = 1,
                WorkWithCostCenter = Company.WorkWithCostCenter,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency,
                CurrentYear = UserInfo.CurrYear

            };
            return View(Obj);
        }
        [Authorize(Roles = "CoOwner,AddServiceBill")]
        public ActionResult NewMulti()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            int CurrYear = UserInfo.CurrYear;
            var ObjGet = _unitOfWork.DefinitionOtherAccount.GetDefinitionOtherAccountByID(UserInfo.fCompanyId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            FixAccountInfoVM ObjFix = new FixAccountInfoVM
            {
                AccountNo = "",
                AccountName = ""
            };
            if (ObjGet != null)
            {
                ObjFix.AccountNo = ObjGet.SalesTaxAccountNumber;
                ObjFix.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.SalesTaxAccountNumber);
            }
            var CompanyTransactionKind = _unitOfWork.NativeSql.GetCompanyTransactionKindAll(UserInfo.fCompanyId);
            CompanyTransactionKind = CompanyTransactionKind.Where(m => m.TransactionKindID == 11).ToList();
            HeaderServiceBillVM Obj = new HeaderServiceBillVM
            {
                BillDate = DateTime.Now,
                TaxAccountNumber = ObjFix.AccountNo,
                TaxAccountName = ObjFix.AccountName,
                OrignailTaxAccountNumber = ObjFix.AccountNo,
                OrignailTaxAccountName = ObjFix.AccountName,
                CompanyTransactionKind = CompanyTransactionKind,
                CompanyTransactionKindID = 1,
                SaleMan = _unitOfWork.Sale.GetAllSale(UserInfo.fCompanyId),
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                CurrencyID = 1,
                WorkWithCostCenter = Company.WorkWithCostCenter,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency,
                CurrentYear = UserInfo.CurrYear

            };
            return View(Obj);
        }
        [Authorize(Roles = "CoOwner,AddServiceBill")]
        public ActionResult New()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            int CurrYear = UserInfo.CurrYear;
            var ObjGet = _unitOfWork.DefinitionOtherAccount.GetDefinitionOtherAccountByID(UserInfo.fCompanyId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            FixAccountInfoVM ObjFix = new FixAccountInfoVM
            {
                AccountNo = "",
                AccountName = ""
            };
            if (ObjGet != null)
            {
                ObjFix.AccountNo = ObjGet.SalesTaxAccountNumber;
                ObjFix.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.SalesTaxAccountNumber);
            }
            var CompanyTransactionKind = _unitOfWork.NativeSql.GetCompanyTransactionKindAll(UserInfo.fCompanyId);
            CompanyTransactionKind = CompanyTransactionKind.Where(m => m.TransactionKindID == 10).ToList();
            HeaderServiceBillVM Obj = new HeaderServiceBillVM
            {
                BillDate = DateTime.Now,
                TaxAccountNumber= ObjFix.AccountNo,
                TaxAccountName = ObjFix.AccountName,
                OrignailTaxAccountNumber = ObjFix.AccountNo,
                OrignailTaxAccountName = ObjFix.AccountName,
                CompanyTransactionKind = CompanyTransactionKind,
                CompanyTransactionKindID = 1,
                SaleMan = _unitOfWork.Sale.GetAllSale(UserInfo.fCompanyId),
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                CurrencyID = 1,
                WorkWithCostCenter = Company.WorkWithCostCenter,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency,
                CurrentYear = UserInfo.CurrYear

            };
            return View(Obj);
        }
        [Authorize(Roles = "CoOwner,UpdateServiceBill")]
        public ActionResult Update(int id,int id2,int id3)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            int CurrYear = UserInfo.CurrYear;
            var ObjGet = _unitOfWork.DefinitionOtherAccount.GetDefinitionOtherAccountByID(UserInfo.fCompanyId);
            var HeaderData = _unitOfWork.ServiceBillHeader.GetHeaderDataById(UserInfo.fCompanyId, id, id2, id3, CurrYear);
            var CompanyTransactionKind = _unitOfWork.NativeSql.GetCompanyTransactionKindAll(UserInfo.fCompanyId);
            CompanyTransactionKind = CompanyTransactionKind.Where(m => m.TransactionKindID == id3).ToList();
            var SaleMan = _unitOfWork.Sale.GetAllSale(UserInfo.fCompanyId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            HeaderServiceBillVM Obj = new HeaderServiceBillVM();
            Obj.AccountNumber = HeaderData.AccountNumber;
            Obj.CompanyID = HeaderData.CompanyID;
            Obj.CompanyTransactionKindID = HeaderData.CompanyTransactionKindNo;
            Obj.CompanyYear = HeaderData.CompanyYear;
            Obj.CreditAccountNumber = HeaderData.CreditAccountNumber;
            Obj.CreditCostNumber = HeaderData.CreditCostNumber;
            Obj.DebitAccountNumber = HeaderData.DebitAccountNumber;
            Obj.DebitCostNumber = HeaderData.DebitCostNumber;
            Obj.Discount = HeaderData.Discount;
            Obj.CurrencyNewValue = HeaderData.ConversionFactor;
            Obj.Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId);
            Obj.CurrencyID = HeaderData.FCurrencyID;
            Obj.NetTotal = HeaderData.NetTotal;
            Obj.NoTax = HeaderData.NoTax;
            Obj.Note = HeaderData.Note;
            Obj.TransactionKindNo = HeaderData.TransactionKindNo;
            Obj.Tax = HeaderData.Tax;
            Obj.TaxCostNumber = HeaderData.TaxCostNumber;
            Obj.Total = HeaderData.Total;
            Obj.BillDate = HeaderData.BillDate;
            Obj.BillID = HeaderData.BillID;
            Obj.SaleMan = SaleMan;
            Obj.SaleManNo = HeaderData.SaleManNo;
            Obj.TaxAccountNumber = HeaderData.TaxAccountNumber;
            Obj.DebitAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, Obj.DebitAccountNumber);
            Obj.CreditAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, Obj.CreditAccountNumber);
            Obj.TaxAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, Obj.TaxAccountNumber);
            Obj.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, Obj.AccountNumber);
            Obj.DebitCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, Obj.DebitCostNumber);
            Obj.CreditCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, Obj.CreditCostNumber);
            Obj.TaxCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, Obj.TaxCostNumber);
            Obj.CompanyTransactionKind = CompanyTransactionKind;
            Obj.CompanyTransactionKindID = HeaderData.CompanyTransactionKindNo;
            Obj.WorkWithCostCenter = Company.WorkWithCostCenter;
            Obj.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            Obj.TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency;
            if (id3 == 11)
            {
                FixAccountInfoVM ObjFix = new FixAccountInfoVM
                {
                    AccountNo = "",
                    AccountName = ""
                };
                if (ObjGet != null)
                {
                    ObjFix.AccountNo = ObjGet.SalesTaxAccountNumber;
                    ObjFix.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.SalesTaxAccountNumber);
                }
                Obj.OrignailTaxAccountNumber = ObjFix.AccountNo;
                Obj.OrignailTaxAccountName = ObjFix.AccountName;
                return View("UpdateMulti", Obj);
            }
            else if (id3 == 12)
            {
                FixAccountInfoVM ObjFix = new FixAccountInfoVM
                {
                    AccountNo = "",
                    AccountName = ""
                };
                if (ObjGet != null)
                {
                    ObjFix.AccountNo = ObjGet.PurchasesTaxAccountNumber;
                    ObjFix.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.PurchasesTaxAccountNumber);
                }
                Obj.OrignailTaxAccountNumber = ObjFix.AccountNo;
                Obj.OrignailTaxAccountName = ObjFix.AccountName;
                return View("UpdatePurchase", Obj);
            }
            else if (id3 == 13)
            {
                FixAccountInfoVM ObjFix = new FixAccountInfoVM
                {
                    AccountNo = "",
                    AccountName = ""
                };
                if (ObjGet != null)
                {
                    ObjFix.AccountNo = ObjGet.PurchasesTaxAccountNumber;
                    ObjFix.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.PurchasesTaxAccountNumber);
                }
                Obj.OrignailTaxAccountNumber = ObjFix.AccountNo;
                Obj.OrignailTaxAccountName = ObjFix.AccountName;
                return View("UpdateMultiPurchase", Obj);
            }
            else if (id3 == 19)
            {
                FixAccountInfoVM ObjFix = new FixAccountInfoVM
                {
                    AccountNo = "",
                    AccountName = ""
                };
                if (ObjGet != null)
                {
                    ObjFix.AccountNo = ObjGet.ReturnSalesTaxAccountNumber;
                    ObjFix.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.ReturnSalesTaxAccountNumber);
                }
                Obj.OrignailTaxAccountNumber = ObjFix.AccountNo;
                Obj.OrignailTaxAccountName = ObjFix.AccountName;
                return View("UpdateReturn", Obj);
            }
            else if (id3 == 20)
            {
                FixAccountInfoVM ObjFix = new FixAccountInfoVM
                {
                    AccountNo = "",
                    AccountName = ""
                };
                if (ObjGet != null)
                {
                    ObjFix.AccountNo = ObjGet.ReturnSalesTaxAccountNumber;
                    ObjFix.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.ReturnSalesTaxAccountNumber);
                }
                Obj.OrignailTaxAccountNumber = ObjFix.AccountNo;
                Obj.OrignailTaxAccountName = ObjFix.AccountName;
                return View("UpdateMultiReturn", Obj);
            }
            else if (id3 == 21)
            {
                FixAccountInfoVM ObjFix = new FixAccountInfoVM
                {
                    AccountNo = "",
                    AccountName = ""
                };
                if (ObjGet != null)
                {
                    ObjFix.AccountNo = ObjGet.ReturnPurchasesTaxAccountNumber;
                    ObjFix.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.ReturnPurchasesTaxAccountNumber);
                }
                Obj.OrignailTaxAccountNumber = ObjFix.AccountNo;
                Obj.OrignailTaxAccountName = ObjFix.AccountName;
                return View("UpdateReturnPurchase", Obj);
            }
            else if (id3 == 22)
            {
                FixAccountInfoVM ObjFix = new FixAccountInfoVM
                {
                    AccountNo = "",
                    AccountName = ""
                };
                if (ObjGet != null)
                {
                    ObjFix.AccountNo = ObjGet.ReturnPurchasesTaxAccountNumber;
                    ObjFix.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.ReturnPurchasesTaxAccountNumber);
                }
                Obj.OrignailTaxAccountNumber = ObjFix.AccountNo;
                Obj.OrignailTaxAccountName = ObjFix.AccountName;
                return View("UpdateMultiReturnPurchase", Obj);
            }
            else {
                FixAccountInfoVM ObjFix = new FixAccountInfoVM
                {
                    AccountNo = "",
                    AccountName = ""
                };
                if (ObjGet != null)
                {
                    ObjFix.AccountNo = ObjGet.SalesTaxAccountNumber;
                    ObjFix.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.SalesTaxAccountNumber);
                }
                Obj.OrignailTaxAccountNumber = ObjFix.AccountNo;
                Obj.OrignailTaxAccountName = ObjFix.AccountName;
                return View(Obj);
            }
        }
        [Authorize(Roles = "CoOwner,CopyServiceBill")]
        public ActionResult Copy(int id, int id2, int id3)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            int CurrYear = UserInfo.CurrYear;
            var ObjGet = _unitOfWork.DefinitionOtherAccount.GetDefinitionOtherAccountByID(UserInfo.fCompanyId);
            var HeaderData = _unitOfWork.ServiceBillHeader.GetHeaderDataById(UserInfo.fCompanyId, id, id2, id3, CurrYear);
            var CompanyTransactionKind = _unitOfWork.NativeSql.GetCompanyTransactionKindAll(UserInfo.fCompanyId);
            CompanyTransactionKind = CompanyTransactionKind.Where(m => m.TransactionKindID == id3).ToList();
            var SaleMan = _unitOfWork.Sale.GetAllSale(UserInfo.fCompanyId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            HeaderServiceBillVM Obj = new HeaderServiceBillVM();
            Obj.AccountNumber = HeaderData.AccountNumber;
            Obj.CompanyID = HeaderData.CompanyID;
            Obj.CompanyTransactionKindID = HeaderData.CompanyTransactionKindNo;
            Obj.CompanyYear = HeaderData.CompanyYear;
            Obj.CreditAccountNumber = HeaderData.CreditAccountNumber;
            Obj.CreditCostNumber = HeaderData.CreditCostNumber;
            Obj.DebitAccountNumber = HeaderData.DebitAccountNumber;
            Obj.DebitCostNumber = HeaderData.DebitCostNumber;
            Obj.Discount = HeaderData.Discount;
            Obj.CurrencyNewValue = HeaderData.ConversionFactor;
            Obj.Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId);
            Obj.CurrencyID = HeaderData.FCurrencyID;
            Obj.NetTotal = HeaderData.NetTotal;
            Obj.NoTax = HeaderData.NoTax;
            Obj.Note = HeaderData.Note;
            Obj.TransactionKindNo = HeaderData.TransactionKindNo;
            Obj.Tax = HeaderData.Tax;
            Obj.TaxCostNumber = HeaderData.TaxCostNumber;
            Obj.Total = HeaderData.Total;
            Obj.BillDate = HeaderData.BillDate;
            Obj.BillID = HeaderData.BillID;
            Obj.SaleMan = SaleMan;
            Obj.SaleManNo = HeaderData.SaleManNo;
            Obj.TaxAccountNumber = HeaderData.TaxAccountNumber;
            Obj.DebitAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, Obj.DebitAccountNumber);
            Obj.CreditAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, Obj.CreditAccountNumber);
            Obj.TaxAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, Obj.TaxAccountNumber);
            Obj.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, Obj.AccountNumber);
            Obj.DebitCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, Obj.DebitCostNumber);
            Obj.CreditCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, Obj.CreditCostNumber);
            Obj.TaxCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, Obj.TaxCostNumber);
            Obj.CompanyTransactionKind = CompanyTransactionKind;
            Obj.CompanyTransactionKindID = HeaderData.CompanyTransactionKindNo;
            Obj.WorkWithCostCenter = Company.WorkWithCostCenter;
            Obj.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            Obj.TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency;
            if (id3 == 11)
            {
                FixAccountInfoVM ObjFix = new FixAccountInfoVM
                {
                    AccountNo = "",
                    AccountName = ""
                };
                if (ObjGet != null)
                {
                    ObjFix.AccountNo = ObjGet.SalesTaxAccountNumber;
                    ObjFix.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.SalesTaxAccountNumber);
                }
                Obj.OrignailTaxAccountNumber = ObjFix.AccountNo;
                Obj.OrignailTaxAccountName = ObjFix.AccountName;
                return View("CopyMulti", Obj);
            }
            else if (id3 == 12)
            {
                FixAccountInfoVM ObjFix = new FixAccountInfoVM
                {
                    AccountNo = "",
                    AccountName = ""
                };
                if (ObjGet != null)
                {
                    ObjFix.AccountNo = ObjGet.PurchasesTaxAccountNumber;
                    ObjFix.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.PurchasesTaxAccountNumber);
                }
                Obj.OrignailTaxAccountNumber = ObjFix.AccountNo;
                Obj.OrignailTaxAccountName = ObjFix.AccountName;
                return View("CopyPurchase", Obj);
            }
            else if (id3 == 13)
            {
                FixAccountInfoVM ObjFix = new FixAccountInfoVM
                {
                    AccountNo = "",
                    AccountName = ""
                };
                if (ObjGet != null)
                {
                    ObjFix.AccountNo = ObjGet.PurchasesTaxAccountNumber;
                    ObjFix.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.PurchasesTaxAccountNumber);
                }
                Obj.OrignailTaxAccountNumber = ObjFix.AccountNo;
                Obj.OrignailTaxAccountName = ObjFix.AccountName;
                return View("CopyMultiPurchase", Obj);
            }
            else if (id3 == 19)
            {
                FixAccountInfoVM ObjFix = new FixAccountInfoVM
                {
                    AccountNo = "",
                    AccountName = ""
                };
                if (ObjGet != null)
                {
                    ObjFix.AccountNo = ObjGet.ReturnSalesTaxAccountNumber;
                    ObjFix.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.ReturnSalesTaxAccountNumber);
                }
                Obj.OrignailTaxAccountNumber = ObjFix.AccountNo;
                Obj.OrignailTaxAccountName = ObjFix.AccountName;
                return View("CopyReturn", Obj);
            }
            else if (id3 == 20)
            {
                FixAccountInfoVM ObjFix = new FixAccountInfoVM
                {
                    AccountNo = "",
                    AccountName = ""
                };
                if (ObjGet != null)
                {
                    ObjFix.AccountNo = ObjGet.ReturnSalesTaxAccountNumber;
                    ObjFix.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.ReturnSalesTaxAccountNumber);
                }
                Obj.OrignailTaxAccountNumber = ObjFix.AccountNo;
                Obj.OrignailTaxAccountName = ObjFix.AccountName;
                return View("CopyMultiReturn", Obj);
            }
            else if (id3 == 21)
            {
                FixAccountInfoVM ObjFix = new FixAccountInfoVM
                {
                    AccountNo = "",
                    AccountName = ""
                };
                if (ObjGet != null)
                {
                    ObjFix.AccountNo = ObjGet.ReturnPurchasesTaxAccountNumber;
                    ObjFix.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.ReturnPurchasesTaxAccountNumber);
                }
                Obj.OrignailTaxAccountNumber = ObjFix.AccountNo;
                Obj.OrignailTaxAccountName = ObjFix.AccountName;
                return View("CopyReturnPurchase", Obj);
            }
            else if (id3 == 22)
            {
                FixAccountInfoVM ObjFix = new FixAccountInfoVM
                {
                    AccountNo = "",
                    AccountName = ""
                };
                if (ObjGet != null)
                {
                    ObjFix.AccountNo = ObjGet.ReturnPurchasesTaxAccountNumber;
                    ObjFix.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.ReturnPurchasesTaxAccountNumber);
                }
                Obj.OrignailTaxAccountNumber = ObjFix.AccountNo;
                Obj.OrignailTaxAccountName = ObjFix.AccountName;
                return View("CopyMultiReturnPurchase", Obj);
            }
            else
            {
                FixAccountInfoVM ObjFix = new FixAccountInfoVM
                {
                    AccountNo = "",
                    AccountName = ""
                };
                if (ObjGet != null)
                {
                    ObjFix.AccountNo = ObjGet.SalesTaxAccountNumber;
                    ObjFix.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.SalesTaxAccountNumber);
                }
                Obj.OrignailTaxAccountNumber = ObjFix.AccountNo;
                Obj.OrignailTaxAccountName = ObjFix.AccountName;
                return View(Obj);
            }
        }
        [Authorize(Roles = "CoOwner,DeleteServiceBill")]
        public ActionResult Delete(int id, int id2, int id3)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            int CurrYear = UserInfo.CurrYear;
            var ObjGet = _unitOfWork.DefinitionOtherAccount.GetDefinitionOtherAccountByID(UserInfo.fCompanyId);
            var HeaderData = _unitOfWork.ServiceBillHeader.GetHeaderDataById(UserInfo.fCompanyId, id, id2, id3, CurrYear);
            var CompanyTransactionKind = _unitOfWork.NativeSql.GetCompanyTransactionKindAll(UserInfo.fCompanyId);
            CompanyTransactionKind = CompanyTransactionKind.Where(m => m.TransactionKindID == id3).ToList();
            var SaleMan = _unitOfWork.Sale.GetAllSale(UserInfo.fCompanyId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            HeaderServiceBillVM Obj = new HeaderServiceBillVM();
            Obj.AccountNumber = HeaderData.AccountNumber;
            Obj.CompanyID = HeaderData.CompanyID;
            Obj.CompanyTransactionKindID = HeaderData.CompanyTransactionKindNo;
            Obj.CompanyYear = HeaderData.CompanyYear;
            Obj.CreditAccountNumber = HeaderData.CreditAccountNumber;
            Obj.CreditCostNumber = HeaderData.CreditCostNumber;
            Obj.DebitAccountNumber = HeaderData.DebitAccountNumber;
            Obj.DebitCostNumber = HeaderData.DebitCostNumber;
            Obj.Discount = HeaderData.Discount;
            Obj.CurrencyNewValue = HeaderData.ConversionFactor;
            Obj.Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId);
            Obj.CurrencyID = HeaderData.FCurrencyID;
            Obj.NetTotal = HeaderData.NetTotal;
            Obj.NoTax = HeaderData.NoTax;
            Obj.Note = HeaderData.Note;
            Obj.TransactionKindNo = HeaderData.TransactionKindNo;
            Obj.Tax = HeaderData.Tax;
            Obj.TaxCostNumber = HeaderData.TaxCostNumber;
            Obj.Total = HeaderData.Total;
            Obj.BillDate = HeaderData.BillDate;
            Obj.BillID = HeaderData.BillID;
            Obj.SaleMan = SaleMan;
            Obj.SaleManNo = HeaderData.SaleManNo;
            Obj.TaxAccountNumber = HeaderData.TaxAccountNumber;
            Obj.DebitAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, Obj.DebitAccountNumber);
            Obj.CreditAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, Obj.CreditAccountNumber);
            Obj.TaxAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, Obj.TaxAccountNumber);
            Obj.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, Obj.AccountNumber);
            Obj.DebitCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, Obj.DebitCostNumber);
            Obj.CreditCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, Obj.CreditCostNumber);
            Obj.TaxCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, Obj.TaxCostNumber);
            Obj.CompanyTransactionKind = CompanyTransactionKind;
            Obj.CompanyTransactionKindID = HeaderData.CompanyTransactionKindNo;
            Obj.WorkWithCostCenter = Company.WorkWithCostCenter;
            Obj.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            Obj.TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency;
            if (id3 == 11)
            {
                FixAccountInfoVM ObjFix = new FixAccountInfoVM
                {
                    AccountNo = "",
                    AccountName = ""
                };
                if (ObjGet != null)
                {
                    ObjFix.AccountNo = ObjGet.SalesTaxAccountNumber;
                    ObjFix.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.SalesTaxAccountNumber);
                }
                Obj.OrignailTaxAccountNumber = ObjFix.AccountNo;
                Obj.OrignailTaxAccountName = ObjFix.AccountName;
                return View("DeleteMulti", Obj);
            }
            else if (id3 == 12)
            {
                FixAccountInfoVM ObjFix = new FixAccountInfoVM
                {
                    AccountNo = "",
                    AccountName = ""
                };
                if (ObjGet != null)
                {
                    ObjFix.AccountNo = ObjGet.PurchasesTaxAccountNumber;
                    ObjFix.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.PurchasesTaxAccountNumber);
                }
                Obj.OrignailTaxAccountNumber = ObjFix.AccountNo;
                Obj.OrignailTaxAccountName = ObjFix.AccountName;
                return View("DeletePurchase", Obj);
            }
            else if (id3 == 13)
            {
                FixAccountInfoVM ObjFix = new FixAccountInfoVM
                {
                    AccountNo = "",
                    AccountName = ""
                };
                if (ObjGet != null)
                {
                    ObjFix.AccountNo = ObjGet.PurchasesTaxAccountNumber;
                    ObjFix.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.PurchasesTaxAccountNumber);
                }
                Obj.OrignailTaxAccountNumber = ObjFix.AccountNo;
                Obj.OrignailTaxAccountName = ObjFix.AccountName;
                return View("DeleteMultiPurchase", Obj);
            }
            else if (id3 == 19)
            {
                FixAccountInfoVM ObjFix = new FixAccountInfoVM
                {
                    AccountNo = "",
                    AccountName = ""
                };
                if (ObjGet != null)
                {
                    ObjFix.AccountNo = ObjGet.ReturnSalesTaxAccountNumber;
                    ObjFix.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.ReturnSalesTaxAccountNumber);
                }
                Obj.OrignailTaxAccountNumber = ObjFix.AccountNo;
                Obj.OrignailTaxAccountName = ObjFix.AccountName;
                return View("DeleteReturn", Obj);
            }
            else if (id3 == 20)
            {
                FixAccountInfoVM ObjFix = new FixAccountInfoVM
                {
                    AccountNo = "",
                    AccountName = ""
                };
                if (ObjGet != null)
                {
                    ObjFix.AccountNo = ObjGet.ReturnSalesTaxAccountNumber;
                    ObjFix.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.ReturnSalesTaxAccountNumber);
                }
                Obj.OrignailTaxAccountNumber = ObjFix.AccountNo;
                Obj.OrignailTaxAccountName = ObjFix.AccountName;
                return View("DeleteMultiReturn", Obj);
            }
            else if (id3 == 21)
            {
                FixAccountInfoVM ObjFix = new FixAccountInfoVM
                {
                    AccountNo = "",
                    AccountName = ""
                };
                if (ObjGet != null)
                {
                    ObjFix.AccountNo = ObjGet.ReturnPurchasesTaxAccountNumber;
                    ObjFix.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.ReturnPurchasesTaxAccountNumber);
                }
                Obj.OrignailTaxAccountNumber = ObjFix.AccountNo;
                Obj.OrignailTaxAccountName = ObjFix.AccountName;
                return View("DeletePurchaseReturn", Obj);
            }
            else if (id3 == 22)
            {
                FixAccountInfoVM ObjFix = new FixAccountInfoVM
                {
                    AccountNo = "",
                    AccountName = ""
                };
                if (ObjGet != null)
                {
                    ObjFix.AccountNo = ObjGet.ReturnPurchasesTaxAccountNumber;
                    ObjFix.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.ReturnPurchasesTaxAccountNumber);
                }
                Obj.OrignailTaxAccountNumber = ObjFix.AccountNo;
                Obj.OrignailTaxAccountName = ObjFix.AccountName;
                return View("DeleteMultiPurchaseReturn", Obj);
            }
            else
            {
                FixAccountInfoVM ObjFix = new FixAccountInfoVM
                {
                    AccountNo = "",
                    AccountName = ""
                };
                if (ObjGet != null)
                {
                    ObjFix.AccountNo = ObjGet.SalesTaxAccountNumber;
                    ObjFix.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.SalesTaxAccountNumber);
                }
                Obj.OrignailTaxAccountNumber = ObjFix.AccountNo;
                Obj.OrignailTaxAccountName = ObjFix.AccountName;
                return View(Obj);
            }
        }
        public ActionResult Detail(int id, int id2, int id3 , int id4)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            int CurrYear = UserInfo.CurrYear;
            var ObjGet = _unitOfWork.DefinitionOtherAccount.GetDefinitionOtherAccountByID(UserInfo.fCompanyId);
            var HeaderData = _unitOfWork.ServiceBillHeader.GetHeaderDataById(UserInfo.fCompanyId, id, id2, id3, CurrYear);
            var CompanyTransactionKind = _unitOfWork.NativeSql.GetCompanyTransactionKindAll(UserInfo.fCompanyId);
            CompanyTransactionKind = CompanyTransactionKind.Where(m => m.TransactionKindID == id3).ToList();
            var SaleMan = _unitOfWork.Sale.GetAllSale(UserInfo.fCompanyId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            HeaderServiceBillVM Obj = new HeaderServiceBillVM();
            Obj.AccountNumber = HeaderData.AccountNumber;
            Obj.CompanyID = HeaderData.CompanyID;
            Obj.CompanyTransactionKindID = HeaderData.CompanyTransactionKindNo;
            Obj.CompanyYear = HeaderData.CompanyYear;
            Obj.CreditAccountNumber = HeaderData.CreditAccountNumber;
            Obj.CreditCostNumber = HeaderData.CreditCostNumber;
            Obj.DebitAccountNumber = HeaderData.DebitAccountNumber;
            Obj.DebitCostNumber = HeaderData.DebitCostNumber;
            Obj.Discount = HeaderData.Discount;
            Obj.CurrencyNewValue = HeaderData.ConversionFactor;
            Obj.Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId);
            Obj.CurrencyID = HeaderData.FCurrencyID;
            Obj.NetTotal = HeaderData.NetTotal;
            Obj.NoTax = HeaderData.NoTax;
            Obj.Note = HeaderData.Note;
            Obj.TransactionKindNo = HeaderData.TransactionKindNo;
            Obj.Tax = HeaderData.Tax;
            Obj.TaxCostNumber = HeaderData.TaxCostNumber;
            Obj.Total = HeaderData.Total;
            Obj.BillDate = HeaderData.BillDate;
            Obj.BillID = HeaderData.BillID;
            Obj.SaleMan = SaleMan;
            Obj.SaleManNo = HeaderData.SaleManNo;
            Obj.TaxAccountNumber = HeaderData.TaxAccountNumber;
            Obj.DebitAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, Obj.DebitAccountNumber);
            Obj.CreditAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, Obj.CreditAccountNumber);
            Obj.TaxAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, Obj.TaxAccountNumber);
            Obj.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, Obj.AccountNumber);
            Obj.DebitCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, Obj.DebitCostNumber);
            Obj.CreditCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, Obj.CreditCostNumber);
            Obj.TaxCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, Obj.TaxCostNumber);
            Obj.CompanyTransactionKind = CompanyTransactionKind;
            Obj.CompanyTransactionKindID = HeaderData.CompanyTransactionKindNo;
            Obj.WorkWithCostCenter = Company.WorkWithCostCenter;
            Obj.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            Obj.TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency;
            if (id3 == 11)
            {
                FixAccountInfoVM ObjFix = new FixAccountInfoVM
                {
                    AccountNo = "",
                    AccountName = ""
                };
                if (ObjGet != null)
                {
                    ObjFix.AccountNo = ObjGet.SalesTaxAccountNumber;
                    ObjFix.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.SalesTaxAccountNumber);
                }
                Obj.OrignailTaxAccountNumber = ObjFix.AccountNo;
                Obj.OrignailTaxAccountName = ObjFix.AccountName;
                return View("DetailMulti", Obj);
            }
            else if (id3 == 12)
            {
                FixAccountInfoVM ObjFix = new FixAccountInfoVM
                {
                    AccountNo = "",
                    AccountName = ""
                };
                if (ObjGet != null)
                {
                    ObjFix.AccountNo = ObjGet.PurchasesTaxAccountNumber;
                    ObjFix.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.PurchasesTaxAccountNumber);
                }
                Obj.OrignailTaxAccountNumber = ObjFix.AccountNo;
                Obj.OrignailTaxAccountName = ObjFix.AccountName;
                return View("DetailPurchase", Obj);
            }
            else if (id3 == 13)
            {
                FixAccountInfoVM ObjFix = new FixAccountInfoVM
                {
                    AccountNo = "",
                    AccountName = ""
                };
                if (ObjGet != null)
                {
                    ObjFix.AccountNo = ObjGet.PurchasesTaxAccountNumber;
                    ObjFix.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.PurchasesTaxAccountNumber);
                }
                Obj.OrignailTaxAccountNumber = ObjFix.AccountNo;
                Obj.OrignailTaxAccountName = ObjFix.AccountName;
                return View("DetailMultiPurchase", Obj);
            }
            else if (id3 == 19)
            {
                FixAccountInfoVM ObjFix = new FixAccountInfoVM
                {
                    AccountNo = "",
                    AccountName = ""
                };
                if (ObjGet != null)
                {
                    ObjFix.AccountNo = ObjGet.ReturnSalesTaxAccountNumber;
                    ObjFix.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.ReturnSalesTaxAccountNumber);
                }
                Obj.OrignailTaxAccountNumber = ObjFix.AccountNo;
                Obj.OrignailTaxAccountName = ObjFix.AccountName;
                return View("DetailReturn", Obj);
            }
            else if (id3 == 20)
            {
                FixAccountInfoVM ObjFix = new FixAccountInfoVM
                {
                    AccountNo = "",
                    AccountName = ""
                };
                if (ObjGet != null)
                {
                    ObjFix.AccountNo = ObjGet.ReturnSalesTaxAccountNumber;
                    ObjFix.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.ReturnSalesTaxAccountNumber);
                }
                Obj.OrignailTaxAccountNumber = ObjFix.AccountNo;
                Obj.OrignailTaxAccountName = ObjFix.AccountName;
                return View("DetailMultiReturn", Obj);
            }
            else if (id3 == 21)
            {
                FixAccountInfoVM ObjFix = new FixAccountInfoVM
                {
                    AccountNo = "",
                    AccountName = ""
                };
                if (ObjGet != null)
                {
                    ObjFix.AccountNo = ObjGet.ReturnPurchasesTaxAccountNumber;
                    ObjFix.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.ReturnPurchasesTaxAccountNumber);
                }
                Obj.OrignailTaxAccountNumber = ObjFix.AccountNo;
                Obj.OrignailTaxAccountName = ObjFix.AccountName;
                return View("DetailPurchaseReturn", Obj);
            }
            else if (id3 == 22)
            {
                FixAccountInfoVM ObjFix = new FixAccountInfoVM
                {
                    AccountNo = "",
                    AccountName = ""
                };
                if (ObjGet != null)
                {
                    ObjFix.AccountNo = ObjGet.ReturnPurchasesTaxAccountNumber;
                    ObjFix.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.ReturnPurchasesTaxAccountNumber);
                }
                Obj.OrignailTaxAccountNumber = ObjFix.AccountNo;
                Obj.OrignailTaxAccountName = ObjFix.AccountName;
                return View("DetailMultiPurchaseReturn", Obj);
            }
            else
            {
                FixAccountInfoVM ObjFix = new FixAccountInfoVM
                {
                    AccountNo = "",
                    AccountName = ""
                };
                if (ObjGet != null)
                {
                    ObjFix.AccountNo = ObjGet.SalesTaxAccountNumber;
                    ObjFix.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.SalesTaxAccountNumber);
                }
                Obj.OrignailTaxAccountNumber = ObjFix.AccountNo;
                Obj.OrignailTaxAccountName = ObjFix.AccountName;
                return View(Obj);
            }
        }
        [Authorize(Roles = "CoOwner,ShowServiceBill")]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            int CurrYear = UserInfo.CurrYear;
            string UserID = User.Identity.GetUserId();
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetCompanyTransactionKindAll(UserInfo.fCompanyId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            CompanyTransactionKindObj = CompanyTransactionKindObj.Where(m => m.TransactionKindID == 10 || m.TransactionKindID==11 || m.TransactionKindID == 12|| m.TransactionKindID == 13
            || m.TransactionKindID == 19 || m.TransactionKindID == 20 || m.TransactionKindID == 21 || m.TransactionKindID == 22).ToList();
            int year = CurrYear;
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
                SaleMan = _unitOfWork.Sale.GetAllSale(UserInfo.fCompanyId),
                CompanyYear = CurrYear,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency
            };
            return View(Obj);
        }
        [HttpPost]
        public JsonResult GetHeaders(TransActionFilter Obj)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var u = _unitOfWork.NativeSql.GetServiceBillHeaders(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
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
            if (!String.IsNullOrEmpty(Obj.AccountNumber))
            {
                u = u.Where(m => m.AccountNumber == Obj.AccountNumber).ToList();
            }
            if (Obj.SaleManNo != 0)
            {
                u = u.Where(m => m.SaleManNo == Obj.SaleManNo).ToList();
            }
            int iRow = 0;
            foreach (var iRowCount in u)
            {
                iRowCount.iRowTable = iRow;
                iRow = iRow + 1;
            }
            return Json(u, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveNewServiceBill(TransServiceBillVM ObjSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                int CurrYear = UserInfo.CurrYear;
                var TransActionServiceBill = ObjSave.TransActionServiceBill;
                HeaderServiceBill HeaderServiceBill = new HeaderServiceBill();
                HeaderServiceBill.AccountNumber = ObjSave.HeaderServiceBill.AccountNumber;
                HeaderServiceBill.BillDate = ObjSave.HeaderServiceBill.BillDate;
                HeaderServiceBill.CompanyID = ObjSave.HeaderServiceBill.CompanyID;
                HeaderServiceBill.CompanyYear = ObjSave.HeaderServiceBill.CompanyYear;
                HeaderServiceBill.CreditAccountNumber = ObjSave.HeaderServiceBill.CreditAccountNumber;
                HeaderServiceBill.CreditCostNumber = ObjSave.HeaderServiceBill.CreditCostNumber;
                HeaderServiceBill.DebitAccountNumber = ObjSave.HeaderServiceBill.DebitAccountNumber;
                HeaderServiceBill.DebitCostNumber = ObjSave.HeaderServiceBill.DebitCostNumber;
                HeaderServiceBill.Discount = ObjSave.HeaderServiceBill.Discount;
                HeaderServiceBill.ConversionFactor = ObjSave.HeaderServiceBill.CurrencyNewValue;
                HeaderServiceBill.FCurrencyID = ObjSave.HeaderServiceBill.CurrencyID;
                HeaderServiceBill.NetTotal = ObjSave.HeaderServiceBill.NetTotal;
                HeaderServiceBill.NetTotalForeignAmount = ObjSave.HeaderServiceBill.NetTotalForeignAmount;
                HeaderServiceBill.ForeignAmount = ObjSave.HeaderServiceBill.ForeignAmount;
                HeaderServiceBill.ForeignAmountTax = ObjSave.HeaderServiceBill.ForeignAmountTax;
                HeaderServiceBill.NoTax = ObjSave.HeaderServiceBill.NoTax;
                HeaderServiceBill.Note = ObjSave.HeaderServiceBill.Note;
                HeaderServiceBill.TransactionKindNo = 10;
                HeaderServiceBill.CompanyTransactionKindNo = ObjSave.HeaderServiceBill.CompanyTransactionKindID;
                HeaderServiceBill.Tax = ObjSave.HeaderServiceBill.Tax;
                HeaderServiceBill.TaxAccountNumber = ObjSave.HeaderServiceBill.TaxAccountNumber;
                HeaderServiceBill.TaxCostNumber = ObjSave.HeaderServiceBill.TaxCostNumber;
                HeaderServiceBill.Total = ObjSave.HeaderServiceBill.Total;
                HeaderServiceBill.SaleManNo = ObjSave.HeaderServiceBill.SaleManNo;
                var TransactionKindNo = 10;
                var CompanyTransactionKindID = ObjSave.HeaderServiceBill.CompanyTransactionKindID;
                double LineTotals = 0;
                double Price = 0;
                double Qty = 0;
                double TaxPrec = 0;
                double Total = 0;
                double TotalTax = 0;
                double NetTotal = 0;
                double TotalDebit = 0;
                double TotalCredit = 0;
                double TotalDebitForeign = 0;
                double TotalCreditForeign = 0;
                int iRow = 1;
                var ExchangeRate = HeaderServiceBill.ConversionFactor;
                HeaderServiceBill.CompanyID = UserInfo.fCompanyId;
                HeaderServiceBill.CompanyYear = CurrYear;
                var AllAcc = _unitOfWork.NativeSql.GetTransActionAccount(UserInfo.fCompanyId);
                foreach ( var Trans in TransActionServiceBill)
                {
                    Price = Trans.Price;
                    Qty = Trans.Qty;
                    TaxPrec = Trans.Tax/100;
                    if (HeaderServiceBill.ConversionFactor == 0)
                    {
                        HeaderServiceBill.ConversionFactor = 1;
                    };
                    if (HeaderServiceBill.ConversionFactor == 1)
                    {
                        Trans.ForeignPrice = 0;
                    }
                    else {
                        Trans.ForeignPrice = Trans.Price / ExchangeRate;
                    }
                    
                    Trans.PriceAfterLineDisc = Trans.Price - Trans.LocalDiscount;
                    Trans.ConversionFactor = ExchangeRate;
                    if (HeaderServiceBill.NoTax)
                    {
                        Trans.TaxValue = 0;
                        
                    }
                    else {
                        Trans.TaxValue = Trans.PriceAfterLineDisc * TaxPrec;
                    }
                    Trans.TotalLocal = Trans.PriceAfterLineDisc * Trans.Qty;
                    if(Trans.ForeignPrice > 0)
                    {
                        Trans.ForeignPriceAfterLineDisc = (Trans.Price / ExchangeRate) - Trans.LocalDiscount;
                        Trans.TotalForeign = Trans.ForeignPriceAfterLineDisc * Trans.Qty;
                    }
                    LineTotals = LineTotals + Trans.TotalLocal;
                }
                var TotalDiscPrec = HeaderServiceBill.Discount / LineTotals;
                foreach (var Trans in TransActionServiceBill)
                {
                    TaxPrec = Trans.Tax / 100;
                    Trans.TotalDiscPrec = TotalDiscPrec;
                    Trans.TotalDiscValue = TotalDiscPrec * Trans.TotalLocal;
                    Trans.NetAfterTotalDisc = Trans.TotalLocal - Trans.TotalDiscValue;
                    if (HeaderServiceBill.NoTax)
                    {
                        Trans.NetTotalTax = 0;
                    }
                    else
                    {
                        Trans.NetTotalTax = TaxPrec * Trans.NetAfterTotalDisc;
                    }
                    Total = Total+Trans.NetAfterTotalDisc;
                    TotalTax = TotalTax+Trans.NetTotalTax;
                }
                NetTotal = Total+TotalTax;
                TotalDebit = NetTotal;
                TotalCredit = Total+TotalTax;
                HeaderServiceBill.NetTotal = NetTotal;
                if (HeaderServiceBill.ConversionFactor == 0)
                {
                    HeaderServiceBill.ConversionFactor = 1;
                };
                if (HeaderServiceBill.ConversionFactor == 1)
                {
                    TotalDebitForeign = 0;
                    TotalCreditForeign = 0;
                }
                else {
                    TotalDebitForeign = Total / ExchangeRate;
                    TotalCreditForeign = (HeaderServiceBill.NetTotal) / ExchangeRate;
                }
                
                if (TotalCredit != TotalDebit)
                {
                    Msg.Msg = Resources.Resource.TheAccountingEntryIsUneven;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }
                if (TotalCredit <= 0)
                {
                    Msg.Msg = Resources.Resource.CantBeZero;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }
                var HeaderTOSave = new Header();
                HeaderTOSave.TransactionKindNo = TransactionKindNo;
                HeaderTOSave.CompanyTransactionKindNo = CompanyTransactionKindID;
                HeaderTOSave.InsDateTime = DateTime.Now;
                HeaderTOSave.InsUserID = userId;
                HeaderTOSave.CompanyID = UserInfo.fCompanyId;
                HeaderTOSave.TotalCredit = TotalCredit;
                HeaderTOSave.TotalDebit = TotalDebit;
                HeaderTOSave.TotalDebitForeign = TotalCreditForeign;
                HeaderTOSave.TotalCreditForeign = TotalCreditForeign;
                HeaderTOSave.CompanyYear = CurrYear;
                HeaderTOSave.VoucherNumber = _unitOfWork.Header.GetMaxVHByKind(UserInfo.fCompanyId, HeaderTOSave.CompanyTransactionKindNo, HeaderTOSave.TransactionKindNo , HeaderTOSave.CompanyYear).ToString();
                var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                if (Company.DirectExportTheTransactions)
                {
                    HeaderTOSave.Exported = 1;
                }
                else
                {
                    HeaderTOSave.Exported = 0;
                }
                HeaderTOSave.VHI = int.Parse(HeaderTOSave.VoucherNumber);
                HeaderTOSave.VoucherDate = HeaderServiceBill.BillDate;
                HeaderTOSave.FCurrencyID = HeaderServiceBill.FCurrencyID;
                HeaderTOSave.ConversionFactor = HeaderServiceBill.ConversionFactor;
                if (HeaderServiceBill.NoTax)
                {
                    HeaderTOSave.RowCount = 2;
                }
                else {
                    HeaderTOSave.RowCount = 3;
                }
                HeaderTOSave.SaleID = HeaderServiceBill.SaleManNo;
                _unitOfWork.Header.Add(HeaderTOSave);
                iRow = 0;
                Transaction ObjDebit = new Transaction();
                ObjDebit.AccountNumber = HeaderServiceBill.DebitAccountNumber;
                ObjDebit.CostCenter = HeaderServiceBill.DebitCostNumber;
                ObjDebit.CompanyTransactionKindNo = CompanyTransactionKindID;
                ObjDebit.TransactionKindNo = TransactionKindNo;
                ObjDebit.CompanyYear = HeaderTOSave.CompanyYear;
                ObjDebit.CompanyID = HeaderTOSave.CompanyID;
                ObjDebit.Credit = 0;
                ObjDebit.CreditForeign = 0;
                ObjDebit.Debit = TotalDebit;
                if (HeaderServiceBill.ConversionFactor == 0)
                {
                    HeaderServiceBill.ConversionFactor = 1;
                };
                if (HeaderServiceBill.ConversionFactor == 1)
                {
                    ObjDebit.DebitForeign = 0;
                    ObjDebit.CreditDebitForeign = 0;
                }
                else {
                    ObjDebit.DebitForeign = TotalDebit / ExchangeRate;
                    ObjDebit.CreditDebitForeign = TotalDebit / ExchangeRate;
                }
                ObjDebit.Exported = 0;
                ObjDebit.InsDateTime = DateTime.Now;
                ObjDebit.RowNumber = 1;
                ObjDebit.VHI = HeaderTOSave.VHI;
                ObjDebit.VoucherDate = HeaderTOSave.VoucherDate;
                ObjDebit.VoucherNumber = HeaderTOSave.VoucherNumber;

                Transaction ObjCredit = new Transaction();
                ObjCredit.AccountNumber = HeaderServiceBill.CreditAccountNumber;
                ObjCredit.CostCenter = HeaderServiceBill.CreditCostNumber;
                ObjCredit.CompanyTransactionKindNo = CompanyTransactionKindID;
                ObjCredit.TransactionKindNo = TransactionKindNo;
                ObjCredit.CompanyYear = HeaderTOSave.CompanyYear;
                ObjCredit.CompanyID = HeaderTOSave.CompanyID;
                ObjCredit.Credit = Total;
                if (HeaderServiceBill.ConversionFactor == 0)
                {
                    HeaderServiceBill.ConversionFactor = 1;
                };
                if (HeaderServiceBill.ConversionFactor == 1)
                {
                    ObjCredit.CreditForeign = 0;
                    ObjCredit.CreditDebitForeign = 0;
                }
                else
                {
                    ObjCredit.CreditDebitForeign = Total / ExchangeRate;
                    ObjCredit.CreditForeign = Total / ExchangeRate;
                }
                ObjCredit.Debit = 0;
                ObjCredit.DebitForeign = 0;
                ObjCredit.Exported = 0;
                ObjCredit.InsDateTime = DateTime.Now;
                ObjCredit.RowNumber = 2;
                ObjCredit.VHI = HeaderTOSave.VHI;
                ObjCredit.VoucherDate = HeaderTOSave.VoucherDate;
                ObjCredit.VoucherNumber = HeaderTOSave.VoucherNumber;

                _unitOfWork.Transaction.Add(ObjDebit);
                _unitOfWork.Transaction.Add(ObjCredit);

                if (!HeaderServiceBill.NoTax)
                {
                    Transaction ObjTax = new Transaction();
                    ObjTax.AccountNumber = HeaderServiceBill.TaxAccountNumber;
                    ObjTax.CostCenter = HeaderServiceBill.TaxCostNumber;
                    ObjTax.CompanyTransactionKindNo = CompanyTransactionKindID;
                    ObjTax.TransactionKindNo = TransactionKindNo;
                    ObjTax.CompanyYear = HeaderTOSave.CompanyYear;
                    ObjTax.CompanyID = HeaderTOSave.CompanyID;
                    ObjTax.Credit = TotalTax;
                    if (HeaderServiceBill.ConversionFactor == 0)
                    {
                        HeaderServiceBill.ConversionFactor = 1;
                    };
                    if (HeaderServiceBill.ConversionFactor == 1)
                    {
                        ObjTax.CreditForeign = 0;
                        ObjTax.CreditDebitForeign = 0;
                    }
                    else
                    {
                        ObjTax.CreditDebitForeign = TotalTax / ExchangeRate;
                        ObjTax.CreditForeign = TotalTax / ExchangeRate;
                    }
                    ObjTax.Debit = 0;
                    ObjTax.DebitForeign = 0;
                    ObjTax.Exported = 0;
                    ObjTax.InsDateTime = DateTime.Now;
                    ObjTax.RowNumber = 3;
                    ObjTax.VHI = HeaderTOSave.VHI;
                    ObjTax.VoucherDate = HeaderTOSave.VoucherDate;
                    ObjTax.VoucherNumber = HeaderTOSave.VoucherNumber;
                    _unitOfWork.Transaction.Add(ObjTax);
                }

                iRow = 1;
                foreach (var Trans in TransActionServiceBill)
                {
                    Trans.BillID = HeaderTOSave.VHI;
                    Trans.CompanyID = HeaderTOSave.CompanyID;
                    Trans.CompanyYear = HeaderTOSave.CompanyYear;
                    Trans.iRowID = iRow;
                    Trans.CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo;
                    Trans.TransactionKindNo = HeaderTOSave.TransactionKindNo;
                    Trans.FCurrencyID = HeaderTOSave.FCurrencyID;
                    iRow = iRow + 1;
                    _unitOfWork.TransActionServiceBill.Add(Trans);
                }
                HeaderServiceBill.BillID = HeaderTOSave.VHI;
                HeaderServiceBill.CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo;
                HeaderServiceBill.TransactionKindNo = HeaderTOSave.TransactionKindNo;
                _unitOfWork.ServiceBillHeader.Add(HeaderServiceBill);
                _unitOfWork.Complete();
                Msg.Code = 1;
                Msg.Msg = Resources.Resource.AddedSuccessfully;
                Msg.Year = HeaderTOSave.CompanyYear;
                Msg.FCompanyId = HeaderTOSave.CompanyID;
                Msg.VoucherNumber = HeaderTOSave.VoucherNumber.ToString();
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
        public JsonResult SaveNewReturnServiceBill(TransServiceBillVM ObjSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                int CurrYear = UserInfo.CurrYear;
                var TransActionServiceBill = ObjSave.TransActionServiceBill;
                HeaderServiceBill HeaderServiceBill = new HeaderServiceBill();
                HeaderServiceBill.AccountNumber = ObjSave.HeaderServiceBill.AccountNumber;
                HeaderServiceBill.BillDate = ObjSave.HeaderServiceBill.BillDate;
                HeaderServiceBill.CompanyID = ObjSave.HeaderServiceBill.CompanyID;
                HeaderServiceBill.CompanyYear = ObjSave.HeaderServiceBill.CompanyYear;
                HeaderServiceBill.CreditAccountNumber = ObjSave.HeaderServiceBill.CreditAccountNumber;
                HeaderServiceBill.CreditCostNumber = ObjSave.HeaderServiceBill.CreditCostNumber;
                HeaderServiceBill.DebitAccountNumber = ObjSave.HeaderServiceBill.DebitAccountNumber;
                HeaderServiceBill.DebitCostNumber = ObjSave.HeaderServiceBill.DebitCostNumber;
                HeaderServiceBill.Discount = ObjSave.HeaderServiceBill.Discount;
                HeaderServiceBill.ConversionFactor = ObjSave.HeaderServiceBill.CurrencyNewValue;
                HeaderServiceBill.FCurrencyID = ObjSave.HeaderServiceBill.CurrencyID;
                HeaderServiceBill.NetTotal = ObjSave.HeaderServiceBill.NetTotal;
                HeaderServiceBill.NetTotalForeignAmount = ObjSave.HeaderServiceBill.NetTotalForeignAmount;
                HeaderServiceBill.ForeignAmount = ObjSave.HeaderServiceBill.ForeignAmount;
                HeaderServiceBill.ForeignAmountTax = ObjSave.HeaderServiceBill.ForeignAmountTax;
                HeaderServiceBill.NoTax = ObjSave.HeaderServiceBill.NoTax;
                HeaderServiceBill.Note = ObjSave.HeaderServiceBill.Note;
                HeaderServiceBill.TransactionKindNo = 19;
                HeaderServiceBill.CompanyTransactionKindNo = ObjSave.HeaderServiceBill.CompanyTransactionKindID;
                HeaderServiceBill.Tax = ObjSave.HeaderServiceBill.Tax;
                HeaderServiceBill.TaxAccountNumber = ObjSave.HeaderServiceBill.TaxAccountNumber;
                HeaderServiceBill.TaxCostNumber = ObjSave.HeaderServiceBill.TaxCostNumber;
                HeaderServiceBill.Total = ObjSave.HeaderServiceBill.Total;
                HeaderServiceBill.SaleManNo = ObjSave.HeaderServiceBill.SaleManNo;
                var TransactionKindNo = 19;
                var CompanyTransactionKindID = ObjSave.HeaderServiceBill.CompanyTransactionKindID;
                double LineTotals = 0;
                double Price = 0;
                double Qty = 0;
                double TaxPrec = 0;
                double Total = 0;
                double TotalTax = 0;
                double NetTotal = 0;
                double TotalDebit = 0;
                double TotalCredit = 0;
                double TotalDebitForeign = 0;
                double TotalCreditForeign = 0;
                int iRow = 1;
                var ExchangeRate = HeaderServiceBill.ConversionFactor;
                HeaderServiceBill.CompanyID = UserInfo.fCompanyId;
                HeaderServiceBill.CompanyYear = CurrYear;
                var AllAcc = _unitOfWork.NativeSql.GetTransActionAccount(UserInfo.fCompanyId);
                foreach (var Trans in TransActionServiceBill)
                {
                    Price = Trans.Price;
                    Qty = Trans.Qty;
                    TaxPrec = Trans.Tax / 100;
                    if (HeaderServiceBill.ConversionFactor == 0)
                    {
                        HeaderServiceBill.ConversionFactor = 1;
                    };
                    if (HeaderServiceBill.ConversionFactor == 1)
                    {
                        Trans.ForeignPrice = 0;
                    }
                    else
                    {
                        Trans.ForeignPrice = Trans.Price / ExchangeRate;
                    }

                    Trans.PriceAfterLineDisc = Trans.Price - Trans.LocalDiscount;
                    Trans.ConversionFactor = ExchangeRate;
                    if (HeaderServiceBill.NoTax)
                    {
                        Trans.TaxValue = 0;

                    }
                    else
                    {
                        Trans.TaxValue = Trans.PriceAfterLineDisc * TaxPrec;
                    }
                    Trans.TotalLocal = Trans.PriceAfterLineDisc * Trans.Qty;
                    if (Trans.ForeignPrice > 0)
                    {
                        Trans.ForeignPriceAfterLineDisc = (Trans.Price / ExchangeRate) - Trans.LocalDiscount;
                        Trans.TotalForeign = Trans.ForeignPriceAfterLineDisc * Trans.Qty;
                    }
                    LineTotals = LineTotals + Trans.TotalLocal;
                }
                var TotalDiscPrec = HeaderServiceBill.Discount / LineTotals;
                foreach (var Trans in TransActionServiceBill)
                {
                    TaxPrec = Trans.Tax / 100;
                    Trans.TotalDiscPrec = TotalDiscPrec;
                    Trans.TotalDiscValue = TotalDiscPrec * Trans.TotalLocal;
                    Trans.NetAfterTotalDisc = Trans.TotalLocal - Trans.TotalDiscValue;
                    if (HeaderServiceBill.NoTax)
                    {
                        Trans.NetTotalTax = 0;
                    }
                    else
                    {
                        Trans.NetTotalTax = TaxPrec * Trans.NetAfterTotalDisc;
                    }
                    Total = Total + Trans.NetAfterTotalDisc;
                    TotalTax = TotalTax + Trans.NetTotalTax;
                }
                NetTotal = Total + TotalTax;
                TotalDebit = Total + TotalTax;
                TotalCredit = NetTotal;
                HeaderServiceBill.NetTotal = NetTotal;
                if (HeaderServiceBill.ConversionFactor == 0)
                {
                    HeaderServiceBill.ConversionFactor = 1;
                };
                if (HeaderServiceBill.ConversionFactor == 1)
                {
                    TotalDebitForeign = 0;
                    TotalCreditForeign = 0;
                }
                else
                {
                    TotalDebitForeign = Total / ExchangeRate;
                    TotalCreditForeign = (HeaderServiceBill.NetTotal) / ExchangeRate;
                }

                if (TotalCredit != TotalDebit)
                {
                    Msg.Msg = Resources.Resource.TheAccountingEntryIsUneven;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }
                if (TotalCredit <= 0)
                {
                    Msg.Msg = Resources.Resource.CantBeZero;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }
                var HeaderTOSave = new Header();
                HeaderTOSave.TransactionKindNo = TransactionKindNo;
                HeaderTOSave.CompanyTransactionKindNo = CompanyTransactionKindID;
                HeaderTOSave.InsDateTime = DateTime.Now;
                HeaderTOSave.InsUserID = userId;
                HeaderTOSave.CompanyID = UserInfo.fCompanyId;
                HeaderTOSave.TotalCredit = TotalCredit;
                HeaderTOSave.TotalDebit = TotalDebit;
                HeaderTOSave.TotalDebitForeign = TotalDebitForeign;
                HeaderTOSave.TotalCreditForeign = TotalDebitForeign;
                HeaderTOSave.CompanyYear = CurrYear;
                HeaderTOSave.VoucherNumber = _unitOfWork.Header.GetMaxVHByKind(UserInfo.fCompanyId, HeaderTOSave.CompanyTransactionKindNo, HeaderTOSave.TransactionKindNo, HeaderTOSave.CompanyYear).ToString();
                var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                if (Company.DirectExportTheTransactions)
                {
                    HeaderTOSave.Exported = 1;
                }
                else
                {
                    HeaderTOSave.Exported = 0;
                }
                HeaderTOSave.VHI = int.Parse(HeaderTOSave.VoucherNumber);
                HeaderTOSave.VoucherDate = HeaderServiceBill.BillDate;
                HeaderTOSave.FCurrencyID = HeaderServiceBill.FCurrencyID;
                HeaderTOSave.ConversionFactor = HeaderServiceBill.ConversionFactor;
                if (HeaderServiceBill.NoTax)
                {
                    HeaderTOSave.RowCount = 2;
                }
                else
                {
                    HeaderTOSave.RowCount = 3;
                }
                HeaderTOSave.SaleID = HeaderServiceBill.SaleManNo;
                _unitOfWork.Header.Add(HeaderTOSave);
                iRow = 0;
                Transaction ObjCredit = new Transaction();
                ObjCredit.AccountNumber = HeaderServiceBill.CreditAccountNumber;
                ObjCredit.CostCenter = HeaderServiceBill.CreditCostNumber;
                ObjCredit.CompanyTransactionKindNo = CompanyTransactionKindID;
                ObjCredit.TransactionKindNo = TransactionKindNo;
                ObjCredit.CompanyYear = HeaderTOSave.CompanyYear;
                ObjCredit.CompanyID = HeaderTOSave.CompanyID;
                ObjCredit.Debit = 0;
                ObjCredit.DebitForeign = 0;
                ObjCredit.Credit = TotalCredit;
                if (HeaderServiceBill.ConversionFactor == 0)
                {
                    HeaderServiceBill.ConversionFactor = 1;
                };
                if (HeaderServiceBill.ConversionFactor == 1)
                {
                    ObjCredit.CreditForeign = 0;
                    ObjCredit.CreditDebitForeign = 0;
                }
                else
                {
                    ObjCredit.CreditForeign = TotalCredit / ExchangeRate;
                    ObjCredit.CreditDebitForeign = TotalCredit / ExchangeRate;
                }
                ObjCredit.Exported = 0;
                ObjCredit.InsDateTime = DateTime.Now;
                ObjCredit.RowNumber = 1;
                ObjCredit.VHI = HeaderTOSave.VHI;
                ObjCredit.VoucherDate = HeaderTOSave.VoucherDate;
                ObjCredit.VoucherNumber = HeaderTOSave.VoucherNumber;

                Transaction ObjDebit = new Transaction();
                ObjDebit.AccountNumber = HeaderServiceBill.DebitAccountNumber;
                ObjDebit.CostCenter = HeaderServiceBill.DebitCostNumber;
                ObjDebit.CompanyTransactionKindNo = CompanyTransactionKindID;
                ObjDebit.TransactionKindNo = TransactionKindNo;
                ObjDebit.CompanyYear = HeaderTOSave.CompanyYear;
                ObjDebit.CompanyID = HeaderTOSave.CompanyID;
                ObjDebit.Debit = Total;
                if (HeaderServiceBill.ConversionFactor == 0)
                {
                    HeaderServiceBill.ConversionFactor = 1;
                };
                if (HeaderServiceBill.ConversionFactor == 1)
                {
                    ObjDebit.DebitForeign = 0;
                    ObjDebit.CreditDebitForeign = 0;
                }
                else
                {
                    ObjDebit.CreditDebitForeign = Total / ExchangeRate;
                    ObjDebit.DebitForeign = Total / ExchangeRate;
                }
                ObjDebit.Credit = 0;
                ObjDebit.CreditForeign = 0;
                ObjDebit.Exported = 0;
                ObjDebit.InsDateTime = DateTime.Now;
                ObjDebit.RowNumber = 2;
                ObjDebit.VHI = HeaderTOSave.VHI;
                ObjDebit.VoucherDate = HeaderTOSave.VoucherDate;
                ObjDebit.VoucherNumber = HeaderTOSave.VoucherNumber;

                _unitOfWork.Transaction.Add(ObjDebit);
                _unitOfWork.Transaction.Add(ObjCredit);

                if (!HeaderServiceBill.NoTax)
                {
                    Transaction ObjTax = new Transaction();
                    ObjTax.AccountNumber = HeaderServiceBill.TaxAccountNumber;
                    ObjTax.CostCenter = HeaderServiceBill.TaxCostNumber;
                    ObjTax.CompanyTransactionKindNo = CompanyTransactionKindID;
                    ObjTax.TransactionKindNo = TransactionKindNo;
                    ObjTax.CompanyYear = HeaderTOSave.CompanyYear;
                    ObjTax.CompanyID = HeaderTOSave.CompanyID;
                    ObjTax.Debit = TotalTax;
                    if (HeaderServiceBill.ConversionFactor == 0)
                    {
                        HeaderServiceBill.ConversionFactor = 1;
                    };
                    if (HeaderServiceBill.ConversionFactor == 1)
                    {
                        ObjTax.DebitForeign = 0;
                        ObjTax.CreditDebitForeign = 0;
                    }
                    else
                    {
                        ObjTax.CreditDebitForeign = TotalTax / ExchangeRate;
                        ObjTax.DebitForeign = TotalTax / ExchangeRate;
                    }
                    ObjTax.Credit = 0;
                    ObjTax.CreditForeign = 0;
                    ObjTax.Exported = 0;
                    ObjTax.InsDateTime = DateTime.Now;
                    ObjTax.RowNumber = 3;
                    ObjTax.VHI = HeaderTOSave.VHI;
                    ObjTax.VoucherDate = HeaderTOSave.VoucherDate;
                    ObjTax.VoucherNumber = HeaderTOSave.VoucherNumber;
                    _unitOfWork.Transaction.Add(ObjTax);
                }

                iRow = 1;
                foreach (var Trans in TransActionServiceBill)
                {
                    Trans.BillID = HeaderTOSave.VHI;
                    Trans.CompanyID = HeaderTOSave.CompanyID;
                    Trans.CompanyYear = HeaderTOSave.CompanyYear;
                    Trans.iRowID = iRow;
                    Trans.CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo;
                    Trans.TransactionKindNo = HeaderTOSave.TransactionKindNo;
                    Trans.FCurrencyID = HeaderTOSave.FCurrencyID;
                    iRow = iRow + 1;
                    _unitOfWork.TransActionServiceBill.Add(Trans);
                }
                HeaderServiceBill.BillID = HeaderTOSave.VHI;
                HeaderServiceBill.CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo;
                HeaderServiceBill.TransactionKindNo = HeaderTOSave.TransactionKindNo;
                _unitOfWork.ServiceBillHeader.Add(HeaderServiceBill);
                _unitOfWork.Complete();
                Msg.Code = 1;
                Msg.Msg = Resources.Resource.AddedSuccessfully;
                Msg.Year = HeaderTOSave.CompanyYear;
                Msg.FCompanyId = HeaderTOSave.CompanyID;
                Msg.VoucherNumber = HeaderTOSave.VoucherNumber.ToString();
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
        public JsonResult SaveNewPurchaseServiceBill(TransServiceBillVM ObjSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                int CurrYear = UserInfo.CurrYear;
                var TransActionServiceBill = ObjSave.TransActionServiceBill;
                HeaderServiceBill HeaderServiceBill = new HeaderServiceBill();
                HeaderServiceBill.AccountNumber = ObjSave.HeaderServiceBill.AccountNumber;
                HeaderServiceBill.BillDate = ObjSave.HeaderServiceBill.BillDate;
                HeaderServiceBill.CompanyID = ObjSave.HeaderServiceBill.CompanyID;
                HeaderServiceBill.CompanyYear = ObjSave.HeaderServiceBill.CompanyYear;
                HeaderServiceBill.CreditAccountNumber = ObjSave.HeaderServiceBill.CreditAccountNumber;
                HeaderServiceBill.CreditCostNumber = ObjSave.HeaderServiceBill.CreditCostNumber;
                HeaderServiceBill.DebitAccountNumber = ObjSave.HeaderServiceBill.DebitAccountNumber;
                HeaderServiceBill.DebitCostNumber = ObjSave.HeaderServiceBill.DebitCostNumber;
                HeaderServiceBill.Discount = ObjSave.HeaderServiceBill.Discount;
                HeaderServiceBill.ConversionFactor = ObjSave.HeaderServiceBill.CurrencyNewValue;
                HeaderServiceBill.FCurrencyID = ObjSave.HeaderServiceBill.CurrencyID;
                HeaderServiceBill.NetTotal = ObjSave.HeaderServiceBill.NetTotal;
                HeaderServiceBill.NetTotalForeignAmount = ObjSave.HeaderServiceBill.NetTotalForeignAmount;
                HeaderServiceBill.ForeignAmount = ObjSave.HeaderServiceBill.ForeignAmount;
                HeaderServiceBill.ForeignAmountTax = ObjSave.HeaderServiceBill.ForeignAmountTax;
                HeaderServiceBill.NoTax = ObjSave.HeaderServiceBill.NoTax;
                HeaderServiceBill.Note = ObjSave.HeaderServiceBill.Note;
                HeaderServiceBill.TransactionKindNo = 12;
                HeaderServiceBill.CompanyTransactionKindNo = ObjSave.HeaderServiceBill.CompanyTransactionKindID;
                HeaderServiceBill.Tax = ObjSave.HeaderServiceBill.Tax;
                HeaderServiceBill.TaxAccountNumber = ObjSave.HeaderServiceBill.TaxAccountNumber;
                HeaderServiceBill.TaxCostNumber = ObjSave.HeaderServiceBill.TaxCostNumber;
                HeaderServiceBill.Total = ObjSave.HeaderServiceBill.Total;
                HeaderServiceBill.SaleManNo = 0;
                var TransactionKindNo = 12;
                var CompanyTransactionKindID = ObjSave.HeaderServiceBill.CompanyTransactionKindID;
                double LineTotals = 0;
                double Price = 0;
                double Qty = 0;
                double TaxPrec = 0;
                double Total = 0;
                double TotalTax = 0;
                double NetTotal = 0;
                double TotalDebit = 0;
                double TotalCredit = 0;
                double TotalDebitForeign = 0;
                double TotalCreditForeign = 0;
                int iRow = 1;
                var ExchangeRate = HeaderServiceBill.ConversionFactor;
                HeaderServiceBill.CompanyID = UserInfo.fCompanyId;
                HeaderServiceBill.CompanyYear = CurrYear;
                var AllAcc = _unitOfWork.NativeSql.GetTransActionAccount(UserInfo.fCompanyId);
                foreach (var Trans in TransActionServiceBill)
                {
                    Price = Trans.Price;
                    Qty = Trans.Qty;
                    TaxPrec = Trans.Tax / 100;
                    if (HeaderServiceBill.ConversionFactor == 0)
                    {
                        HeaderServiceBill.ConversionFactor = 1;
                    };
                    if (HeaderServiceBill.ConversionFactor == 1)
                    {
                        Trans.ForeignPrice = 0;
                    }
                    else
                    {
                        Trans.ForeignPrice = Trans.Price / ExchangeRate;
                    }

                    Trans.PriceAfterLineDisc = Trans.Price - Trans.LocalDiscount;
                    Trans.ConversionFactor = ExchangeRate;
                    if (HeaderServiceBill.NoTax)
                    {
                        Trans.TaxValue = 0;

                    }
                    else
                    {
                        Trans.TaxValue = Trans.PriceAfterLineDisc * TaxPrec;
                    }
                    Trans.TotalLocal = Trans.PriceAfterLineDisc * Trans.Qty;
                    if (Trans.ForeignPrice > 0)
                    {
                        Trans.ForeignPriceAfterLineDisc = (Trans.Price / ExchangeRate) - Trans.LocalDiscount;
                        Trans.TotalForeign = Trans.ForeignPriceAfterLineDisc * Trans.Qty;
                    }
                    LineTotals = LineTotals + Trans.TotalLocal;
                }
                var TotalDiscPrec = HeaderServiceBill.Discount / LineTotals;
                foreach (var Trans in TransActionServiceBill)
                {
                    TaxPrec = Trans.Tax / 100;
                    Trans.TotalDiscPrec = TotalDiscPrec;
                    Trans.TotalDiscValue = TotalDiscPrec * Trans.TotalLocal;
                    Trans.NetAfterTotalDisc = Trans.TotalLocal - Trans.TotalDiscValue;
                    if (HeaderServiceBill.NoTax)
                    {
                        Trans.NetTotalTax = 0;
                    }
                    else
                    {
                        Trans.NetTotalTax = TaxPrec * Trans.NetAfterTotalDisc;
                    }
                    Total = Total + Trans.NetAfterTotalDisc;
                    TotalTax = TotalTax + Trans.NetTotalTax;
                }
                NetTotal = Total + TotalTax;
                TotalDebit = Total + TotalTax;
                TotalCredit = NetTotal;
                HeaderServiceBill.NetTotal = NetTotal;
                if (HeaderServiceBill.ConversionFactor == 0)
                {
                    HeaderServiceBill.ConversionFactor = 1;
                };
                if (HeaderServiceBill.ConversionFactor == 1)
                {
                    TotalDebitForeign = 0;
                    TotalCreditForeign = 0;
                }
                else
                {
                    TotalDebitForeign = Total / ExchangeRate;
                    TotalCreditForeign = (HeaderServiceBill.NetTotal) / ExchangeRate;
                }

                if (TotalCredit != TotalDebit)
                {
                    Msg.Msg = Resources.Resource.TheAccountingEntryIsUneven;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }
                if (TotalCredit <= 0)
                {
                    Msg.Msg = Resources.Resource.CantBeZero;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }
                var HeaderTOSave = new Header();
                HeaderTOSave.TransactionKindNo = TransactionKindNo;
                HeaderTOSave.CompanyTransactionKindNo = CompanyTransactionKindID;
                HeaderTOSave.InsDateTime = DateTime.Now;
                HeaderTOSave.InsUserID = userId;
                HeaderTOSave.CompanyID = UserInfo.fCompanyId;
                HeaderTOSave.TotalCredit = TotalCredit;
                HeaderTOSave.TotalDebit = TotalDebit;
                HeaderTOSave.TotalDebitForeign = TotalDebitForeign;
                HeaderTOSave.TotalCreditForeign = TotalDebitForeign;
                HeaderTOSave.CompanyYear = CurrYear;
                HeaderTOSave.VoucherNumber = _unitOfWork.Header.GetMaxVHByKind(UserInfo.fCompanyId, HeaderTOSave.CompanyTransactionKindNo, HeaderTOSave.TransactionKindNo, HeaderTOSave.CompanyYear).ToString();
                var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                if (Company.DirectExportTheTransactions)
                {
                    HeaderTOSave.Exported = 1;
                }
                else
                {
                    HeaderTOSave.Exported = 0;
                }
                HeaderTOSave.VHI = int.Parse(HeaderTOSave.VoucherNumber);
                HeaderTOSave.VoucherDate = HeaderServiceBill.BillDate;
                HeaderTOSave.FCurrencyID = HeaderServiceBill.FCurrencyID;
                HeaderTOSave.ConversionFactor = HeaderServiceBill.ConversionFactor;
                if (HeaderServiceBill.NoTax)
                {
                    HeaderTOSave.RowCount = 2;
                }
                else
                {
                    HeaderTOSave.RowCount = 3;
                }
                HeaderTOSave.SaleID = 0;
                _unitOfWork.Header.Add(HeaderTOSave);
                iRow = 0;
                Transaction ObjCredit = new Transaction();
                ObjCredit.AccountNumber = HeaderServiceBill.CreditAccountNumber;
                ObjCredit.CostCenter = HeaderServiceBill.CreditCostNumber;
                ObjCredit.CompanyTransactionKindNo = CompanyTransactionKindID;
                ObjCredit.TransactionKindNo = TransactionKindNo;
                ObjCredit.CompanyYear = HeaderTOSave.CompanyYear;
                ObjCredit.CompanyID = HeaderTOSave.CompanyID;
                ObjCredit.Debit = 0;
                ObjCredit.DebitForeign = 0;
                ObjCredit.Credit = TotalCredit;
                if (HeaderServiceBill.ConversionFactor == 0)
                {
                    HeaderServiceBill.ConversionFactor = 1;
                };
                if (HeaderServiceBill.ConversionFactor == 1)
                {
                    ObjCredit.CreditForeign = 0;
                    ObjCredit.CreditDebitForeign = 0;
                }
                else
                {
                    ObjCredit.CreditForeign = TotalCredit / ExchangeRate;
                    ObjCredit.CreditDebitForeign = TotalCredit / ExchangeRate;
                }
                ObjCredit.Exported = 0;
                ObjCredit.InsDateTime = DateTime.Now;
                ObjCredit.RowNumber = 1;
                ObjCredit.VHI = HeaderTOSave.VHI;
                ObjCredit.VoucherDate = HeaderTOSave.VoucherDate;
                ObjCredit.VoucherNumber = HeaderTOSave.VoucherNumber;

                Transaction ObjDebit = new Transaction();
                ObjDebit.AccountNumber = HeaderServiceBill.DebitAccountNumber;
                ObjDebit.CostCenter = HeaderServiceBill.DebitCostNumber;
                ObjDebit.CompanyTransactionKindNo = CompanyTransactionKindID;
                ObjDebit.TransactionKindNo = TransactionKindNo;
                ObjDebit.CompanyYear = HeaderTOSave.CompanyYear;
                ObjDebit.CompanyID = HeaderTOSave.CompanyID;
                ObjDebit.Debit = Total;
                if (HeaderServiceBill.ConversionFactor == 0)
                {
                    HeaderServiceBill.ConversionFactor = 1;
                };
                if (HeaderServiceBill.ConversionFactor == 1)
                {
                    ObjDebit.DebitForeign = 0;
                    ObjDebit.CreditDebitForeign = 0;
                }
                else
                {
                    ObjDebit.CreditDebitForeign = Total / ExchangeRate;
                    ObjDebit.DebitForeign = Total / ExchangeRate;
                }
                ObjDebit.Credit = 0;
                ObjDebit.CreditForeign = 0;
                ObjDebit.Exported = 0;
                ObjDebit.InsDateTime = DateTime.Now;
                ObjDebit.RowNumber = 2;
                ObjDebit.VHI = HeaderTOSave.VHI;
                ObjDebit.VoucherDate = HeaderTOSave.VoucherDate;
                ObjDebit.VoucherNumber = HeaderTOSave.VoucherNumber;

                _unitOfWork.Transaction.Add(ObjDebit);
                _unitOfWork.Transaction.Add(ObjCredit);

                if (!HeaderServiceBill.NoTax)
                {
                    Transaction ObjTax = new Transaction();
                    ObjTax.AccountNumber = HeaderServiceBill.TaxAccountNumber;
                    ObjTax.CostCenter = HeaderServiceBill.TaxCostNumber;
                    ObjTax.CompanyTransactionKindNo = CompanyTransactionKindID;
                    ObjTax.TransactionKindNo = TransactionKindNo;
                    ObjTax.CompanyYear = HeaderTOSave.CompanyYear;
                    ObjTax.CompanyID = HeaderTOSave.CompanyID;
                    ObjTax.Debit = TotalTax;
                    if (HeaderServiceBill.ConversionFactor == 0)
                    {
                        HeaderServiceBill.ConversionFactor = 1;
                    };
                    if (HeaderServiceBill.ConversionFactor == 1)
                    {
                        ObjTax.DebitForeign = 0;
                        ObjTax.CreditDebitForeign = 0;
                    }
                    else
                    {
                        ObjTax.CreditDebitForeign = TotalTax / ExchangeRate;
                        ObjTax.DebitForeign = TotalTax / ExchangeRate;
                    }
                    ObjTax.Credit = 0;
                    ObjTax.CreditForeign = 0;
                    ObjTax.Exported = 0;
                    ObjTax.InsDateTime = DateTime.Now;
                    ObjTax.RowNumber = 3;
                    ObjTax.VHI = HeaderTOSave.VHI;
                    ObjTax.VoucherDate = HeaderTOSave.VoucherDate;
                    ObjTax.VoucherNumber = HeaderTOSave.VoucherNumber;
                    _unitOfWork.Transaction.Add(ObjTax);
                }

                iRow = 1;
                foreach (var Trans in TransActionServiceBill)
                {
                    Trans.BillID = HeaderTOSave.VHI;
                    Trans.CompanyID = HeaderTOSave.CompanyID;
                    Trans.CompanyYear = HeaderTOSave.CompanyYear;
                    Trans.iRowID = iRow;
                    Trans.CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo;
                    Trans.TransactionKindNo = HeaderTOSave.TransactionKindNo;
                    Trans.FCurrencyID = HeaderTOSave.FCurrencyID;
                    iRow = iRow + 1;
                    _unitOfWork.TransActionServiceBill.Add(Trans);
                }
                HeaderServiceBill.BillID = HeaderTOSave.VHI;
                HeaderServiceBill.CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo;
                HeaderServiceBill.TransactionKindNo = HeaderTOSave.TransactionKindNo;
                _unitOfWork.ServiceBillHeader.Add(HeaderServiceBill);
                _unitOfWork.Complete();
                Msg.Code = 1;
                Msg.Msg = Resources.Resource.AddedSuccessfully;
                Msg.Year = HeaderTOSave.CompanyYear;
                Msg.FCompanyId = HeaderTOSave.CompanyID;
                Msg.VoucherNumber = HeaderTOSave.VoucherNumber.ToString();
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
        public JsonResult SaveNewReturnPurchaseServiceBill(TransServiceBillVM ObjSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                int CurrYear = UserInfo.CurrYear;
                var TransActionServiceBill = ObjSave.TransActionServiceBill;
                HeaderServiceBill HeaderServiceBill = new HeaderServiceBill();
                HeaderServiceBill.AccountNumber = ObjSave.HeaderServiceBill.AccountNumber;
                HeaderServiceBill.BillDate = ObjSave.HeaderServiceBill.BillDate;
                HeaderServiceBill.CompanyID = ObjSave.HeaderServiceBill.CompanyID;
                HeaderServiceBill.CompanyYear = ObjSave.HeaderServiceBill.CompanyYear;
                HeaderServiceBill.CreditAccountNumber = ObjSave.HeaderServiceBill.CreditAccountNumber;
                HeaderServiceBill.CreditCostNumber = ObjSave.HeaderServiceBill.CreditCostNumber;
                HeaderServiceBill.DebitAccountNumber = ObjSave.HeaderServiceBill.DebitAccountNumber;
                HeaderServiceBill.DebitCostNumber = ObjSave.HeaderServiceBill.DebitCostNumber;
                HeaderServiceBill.Discount = ObjSave.HeaderServiceBill.Discount;
                HeaderServiceBill.ConversionFactor = ObjSave.HeaderServiceBill.CurrencyNewValue;
                HeaderServiceBill.FCurrencyID = ObjSave.HeaderServiceBill.CurrencyID;
                HeaderServiceBill.NetTotal = ObjSave.HeaderServiceBill.NetTotal;
                HeaderServiceBill.NetTotalForeignAmount = ObjSave.HeaderServiceBill.NetTotalForeignAmount;
                HeaderServiceBill.ForeignAmount = ObjSave.HeaderServiceBill.ForeignAmount;
                HeaderServiceBill.ForeignAmountTax = ObjSave.HeaderServiceBill.ForeignAmountTax;
                HeaderServiceBill.NoTax = ObjSave.HeaderServiceBill.NoTax;
                HeaderServiceBill.Note = ObjSave.HeaderServiceBill.Note;
                HeaderServiceBill.TransactionKindNo = 21;
                HeaderServiceBill.CompanyTransactionKindNo = ObjSave.HeaderServiceBill.CompanyTransactionKindID;
                HeaderServiceBill.Tax = ObjSave.HeaderServiceBill.Tax;
                HeaderServiceBill.TaxAccountNumber = ObjSave.HeaderServiceBill.TaxAccountNumber;
                HeaderServiceBill.TaxCostNumber = ObjSave.HeaderServiceBill.TaxCostNumber;
                HeaderServiceBill.Total = ObjSave.HeaderServiceBill.Total;
                HeaderServiceBill.SaleManNo = 0;
                var TransactionKindNo = 21;
                var CompanyTransactionKindID = ObjSave.HeaderServiceBill.CompanyTransactionKindID;
                double LineTotals = 0;
                double Price = 0;
                double Qty = 0;
                double TaxPrec = 0;
                double Total = 0;
                double TotalTax = 0;
                double NetTotal = 0;
                double TotalDebit = 0;
                double TotalCredit = 0;
                double TotalDebitForeign = 0;
                double TotalCreditForeign = 0;
                int iRow = 1;
                var ExchangeRate = HeaderServiceBill.ConversionFactor;
                HeaderServiceBill.CompanyID = UserInfo.fCompanyId;
                HeaderServiceBill.CompanyYear = CurrYear;
                var AllAcc = _unitOfWork.NativeSql.GetTransActionAccount(UserInfo.fCompanyId);
                foreach (var Trans in TransActionServiceBill)
                {
                    Price = Trans.Price;
                    Qty = Trans.Qty;
                    TaxPrec = Trans.Tax / 100;
                    if (HeaderServiceBill.ConversionFactor == 0)
                    {
                        HeaderServiceBill.ConversionFactor = 1;
                    };
                    if (HeaderServiceBill.ConversionFactor == 1)
                    {
                        Trans.ForeignPrice = 0;
                    }
                    else
                    {
                        Trans.ForeignPrice = Trans.Price / ExchangeRate;
                    }

                    Trans.PriceAfterLineDisc = Trans.Price - Trans.LocalDiscount;
                    Trans.ConversionFactor = ExchangeRate;
                    if (HeaderServiceBill.NoTax)
                    {
                        Trans.TaxValue = 0;

                    }
                    else
                    {
                        Trans.TaxValue = Trans.PriceAfterLineDisc * TaxPrec;
                    }
                    Trans.TotalLocal = Trans.PriceAfterLineDisc * Trans.Qty;
                    if (Trans.ForeignPrice > 0)
                    {
                        Trans.ForeignPriceAfterLineDisc = (Trans.Price / ExchangeRate) - Trans.LocalDiscount;
                        Trans.TotalForeign = Trans.ForeignPriceAfterLineDisc * Trans.Qty;
                    }
                    LineTotals = LineTotals + Trans.TotalLocal;
                }
                var TotalDiscPrec = HeaderServiceBill.Discount / LineTotals;
                foreach (var Trans in TransActionServiceBill)
                {
                    TaxPrec = Trans.Tax / 100;
                    Trans.TotalDiscPrec = TotalDiscPrec;
                    Trans.TotalDiscValue = TotalDiscPrec * Trans.TotalLocal;
                    Trans.NetAfterTotalDisc = Trans.TotalLocal - Trans.TotalDiscValue;
                    if (HeaderServiceBill.NoTax)
                    {
                        Trans.NetTotalTax = 0;
                    }
                    else
                    {
                        Trans.NetTotalTax = TaxPrec * Trans.NetAfterTotalDisc;
                    }
                    Total = Total + Trans.NetAfterTotalDisc;
                    TotalTax = TotalTax + Trans.NetTotalTax;
                }
                NetTotal = Total + TotalTax;
                TotalDebit = NetTotal;
                TotalCredit = Total + TotalTax;
                HeaderServiceBill.NetTotal = NetTotal;
                if (HeaderServiceBill.ConversionFactor == 0)
                {
                    HeaderServiceBill.ConversionFactor = 1;
                };
                if (HeaderServiceBill.ConversionFactor == 1)
                {
                    TotalDebitForeign = 0;
                    TotalCreditForeign = 0;
                }
                else
                {
                    TotalDebitForeign = Total / ExchangeRate;
                    TotalCreditForeign = (HeaderServiceBill.NetTotal) / ExchangeRate;
                }

                if (TotalCredit != TotalDebit)
                {
                    Msg.Msg = Resources.Resource.TheAccountingEntryIsUneven;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }
                if (TotalCredit <= 0)
                {
                    Msg.Msg = Resources.Resource.CantBeZero;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }
                var HeaderTOSave = new Header();
                HeaderTOSave.TransactionKindNo = TransactionKindNo;
                HeaderTOSave.CompanyTransactionKindNo = CompanyTransactionKindID;
                HeaderTOSave.InsDateTime = DateTime.Now;
                HeaderTOSave.InsUserID = userId;
                HeaderTOSave.CompanyID = UserInfo.fCompanyId;
                HeaderTOSave.TotalCredit = TotalCredit;
                HeaderTOSave.TotalDebit = TotalDebit;
                HeaderTOSave.TotalDebitForeign = TotalCreditForeign;
                HeaderTOSave.TotalCreditForeign = TotalCreditForeign;
                HeaderTOSave.CompanyYear = CurrYear;
                HeaderTOSave.VoucherNumber = _unitOfWork.Header.GetMaxVHByKind(UserInfo.fCompanyId, HeaderTOSave.CompanyTransactionKindNo, HeaderTOSave.TransactionKindNo, HeaderTOSave.CompanyYear).ToString();
                var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                if (Company.DirectExportTheTransactions)
                {
                    HeaderTOSave.Exported = 1;
                }
                else
                {
                    HeaderTOSave.Exported = 0;
                }
                HeaderTOSave.VHI = int.Parse(HeaderTOSave.VoucherNumber);
                HeaderTOSave.VoucherDate = HeaderServiceBill.BillDate;
                HeaderTOSave.FCurrencyID = HeaderServiceBill.FCurrencyID;
                HeaderTOSave.ConversionFactor = HeaderServiceBill.ConversionFactor;
                if (HeaderServiceBill.NoTax)
                {
                    HeaderTOSave.RowCount = 2;
                }
                else
                {
                    HeaderTOSave.RowCount = 3;
                }
                HeaderTOSave.SaleID = 0;
                _unitOfWork.Header.Add(HeaderTOSave);
                iRow = 0;
                Transaction ObjDebit = new Transaction();
                ObjDebit.AccountNumber = HeaderServiceBill.DebitAccountNumber;
                ObjDebit.CostCenter = HeaderServiceBill.DebitCostNumber;
                ObjDebit.CompanyTransactionKindNo = CompanyTransactionKindID;
                ObjDebit.TransactionKindNo = TransactionKindNo;
                ObjDebit.CompanyYear = HeaderTOSave.CompanyYear;
                ObjDebit.CompanyID = HeaderTOSave.CompanyID;
                ObjDebit.Credit = 0;
                ObjDebit.CreditForeign = 0;
                ObjDebit.Debit = TotalDebit;
                if (HeaderServiceBill.ConversionFactor == 0)
                {
                    HeaderServiceBill.ConversionFactor = 1;
                };
                if (HeaderServiceBill.ConversionFactor == 1)
                {
                    ObjDebit.DebitForeign = 0;
                    ObjDebit.CreditDebitForeign = 0;
                }
                else
                {
                    ObjDebit.DebitForeign = TotalDebit / ExchangeRate;
                    ObjDebit.CreditDebitForeign = TotalDebit / ExchangeRate;
                }
                ObjDebit.Exported = 0;
                ObjDebit.InsDateTime = DateTime.Now;
                ObjDebit.RowNumber = 1;
                ObjDebit.VHI = HeaderTOSave.VHI;
                ObjDebit.VoucherDate = HeaderTOSave.VoucherDate;
                ObjDebit.VoucherNumber = HeaderTOSave.VoucherNumber;

                Transaction ObjCredit = new Transaction();
                ObjCredit.AccountNumber = HeaderServiceBill.CreditAccountNumber;
                ObjCredit.CostCenter = HeaderServiceBill.CreditCostNumber;
                ObjCredit.CompanyTransactionKindNo = CompanyTransactionKindID;
                ObjCredit.TransactionKindNo = TransactionKindNo;
                ObjCredit.CompanyYear = HeaderTOSave.CompanyYear;
                ObjCredit.CompanyID = HeaderTOSave.CompanyID;
                ObjCredit.Credit = Total;
                if (HeaderServiceBill.ConversionFactor == 0)
                {
                    HeaderServiceBill.ConversionFactor = 1;
                };
                if (HeaderServiceBill.ConversionFactor == 1)
                {
                    ObjCredit.CreditForeign = 0;
                    ObjCredit.CreditDebitForeign = 0;
                }
                else
                {
                    ObjCredit.CreditDebitForeign = Total / ExchangeRate;
                    ObjCredit.CreditForeign = Total / ExchangeRate;
                }
                ObjCredit.Debit = 0;
                ObjCredit.DebitForeign = 0;
                ObjCredit.Exported = 0;
                ObjCredit.InsDateTime = DateTime.Now;
                ObjCredit.RowNumber = 2;
                ObjCredit.VHI = HeaderTOSave.VHI;
                ObjCredit.VoucherDate = HeaderTOSave.VoucherDate;
                ObjCredit.VoucherNumber = HeaderTOSave.VoucherNumber;

                _unitOfWork.Transaction.Add(ObjDebit);
                _unitOfWork.Transaction.Add(ObjCredit);

                if (!HeaderServiceBill.NoTax)
                {
                    Transaction ObjTax = new Transaction();
                    ObjTax.AccountNumber = HeaderServiceBill.TaxAccountNumber;
                    ObjTax.CostCenter = HeaderServiceBill.TaxCostNumber;
                    ObjTax.CompanyTransactionKindNo = CompanyTransactionKindID;
                    ObjTax.TransactionKindNo = TransactionKindNo;
                    ObjTax.CompanyYear = HeaderTOSave.CompanyYear;
                    ObjTax.CompanyID = HeaderTOSave.CompanyID;
                    ObjTax.Credit = TotalTax;
                    if (HeaderServiceBill.ConversionFactor == 0)
                    {
                        HeaderServiceBill.ConversionFactor = 1;
                    };
                    if (HeaderServiceBill.ConversionFactor == 1)
                    {
                        ObjTax.CreditForeign = 0;
                        ObjTax.CreditDebitForeign = 0;
                    }
                    else
                    {
                        ObjTax.CreditDebitForeign = TotalTax / ExchangeRate;
                        ObjTax.CreditForeign = TotalTax / ExchangeRate;
                    }
                    ObjTax.Debit = 0;
                    ObjTax.DebitForeign = 0;
                    ObjTax.Exported = 0;
                    ObjTax.InsDateTime = DateTime.Now;
                    ObjTax.RowNumber = 3;
                    ObjTax.VHI = HeaderTOSave.VHI;
                    ObjTax.VoucherDate = HeaderTOSave.VoucherDate;
                    ObjTax.VoucherNumber = HeaderTOSave.VoucherNumber;
                    _unitOfWork.Transaction.Add(ObjTax);
                }

                iRow = 1;
                foreach (var Trans in TransActionServiceBill)
                {
                    Trans.BillID = HeaderTOSave.VHI;
                    Trans.CompanyID = HeaderTOSave.CompanyID;
                    Trans.CompanyYear = HeaderTOSave.CompanyYear;
                    Trans.iRowID = iRow;
                    Trans.CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo;
                    Trans.TransactionKindNo = HeaderTOSave.TransactionKindNo;
                    Trans.FCurrencyID = HeaderTOSave.FCurrencyID;
                    iRow = iRow + 1;
                    _unitOfWork.TransActionServiceBill.Add(Trans);
                }
                HeaderServiceBill.BillID = HeaderTOSave.VHI;
                HeaderServiceBill.CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo;
                HeaderServiceBill.TransactionKindNo = HeaderTOSave.TransactionKindNo;
                _unitOfWork.ServiceBillHeader.Add(HeaderServiceBill);
                _unitOfWork.Complete();
                Msg.Code = 1;
                Msg.Msg = Resources.Resource.AddedSuccessfully;
                Msg.Year = HeaderTOSave.CompanyYear;
                Msg.FCompanyId = HeaderTOSave.CompanyID;
                Msg.VoucherNumber = HeaderTOSave.VoucherNumber.ToString();
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
        public JsonResult SaveNewMultiServiceBill(TransServiceBillVM ObjSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                int CurrYear = UserInfo.CurrYear;
                var TransActionServiceBill = ObjSave.TransActionServiceBill;
                HeaderServiceBill HeaderServiceBill = new HeaderServiceBill();
                HeaderServiceBill.AccountNumber = ObjSave.HeaderServiceBill.AccountNumber;
                HeaderServiceBill.BillDate = ObjSave.HeaderServiceBill.BillDate;
                HeaderServiceBill.CompanyID = ObjSave.HeaderServiceBill.CompanyID;
                HeaderServiceBill.CompanyYear = ObjSave.HeaderServiceBill.CompanyYear;
                HeaderServiceBill.CreditAccountNumber = ObjSave.HeaderServiceBill.CreditAccountNumber;
                HeaderServiceBill.CreditCostNumber = ObjSave.HeaderServiceBill.CreditCostNumber;
                HeaderServiceBill.DebitAccountNumber = ObjSave.HeaderServiceBill.DebitAccountNumber;
                HeaderServiceBill.DebitCostNumber = ObjSave.HeaderServiceBill.DebitCostNumber;
                HeaderServiceBill.Discount = ObjSave.HeaderServiceBill.Discount;
                HeaderServiceBill.ConversionFactor = ObjSave.HeaderServiceBill.CurrencyNewValue;
                HeaderServiceBill.FCurrencyID = ObjSave.HeaderServiceBill.CurrencyID;
                HeaderServiceBill.NetTotal = ObjSave.HeaderServiceBill.NetTotal;
                HeaderServiceBill.NetTotalForeignAmount = ObjSave.HeaderServiceBill.NetTotalForeignAmount;
                HeaderServiceBill.ForeignAmount = ObjSave.HeaderServiceBill.ForeignAmount;
                HeaderServiceBill.ForeignAmountTax = ObjSave.HeaderServiceBill.ForeignAmountTax;
                HeaderServiceBill.NoTax = ObjSave.HeaderServiceBill.NoTax;
                HeaderServiceBill.Note = ObjSave.HeaderServiceBill.Note;
                HeaderServiceBill.TransactionKindNo = 11;
                HeaderServiceBill.CompanyTransactionKindNo = ObjSave.HeaderServiceBill.CompanyTransactionKindID;
                HeaderServiceBill.Tax = ObjSave.HeaderServiceBill.Tax;
                HeaderServiceBill.TaxAccountNumber = ObjSave.HeaderServiceBill.TaxAccountNumber;
                HeaderServiceBill.TaxCostNumber = ObjSave.HeaderServiceBill.TaxCostNumber;
                HeaderServiceBill.Total = ObjSave.HeaderServiceBill.Total;
                HeaderServiceBill.SaleManNo = ObjSave.HeaderServiceBill.SaleManNo;
                var TransactionKindNo = 11;
                var CompanyTransactionKindID = ObjSave.HeaderServiceBill.CompanyTransactionKindID;
                double LineTotals = 0;
                double Price = 0;
                double Qty = 0;
                double TaxPrec = 0;
                double Total = 0;
                double TotalTax = 0;
                double NetTotal = 0;
                double TotalDebit = 0;
                double TotalCredit = 0;
                double TotalDebitForeign = 0;
                double TotalCreditForeign = 0;
                int iRow = 1;
                var ExchangeRate = HeaderServiceBill.ConversionFactor;
                HeaderServiceBill.CompanyID = UserInfo.fCompanyId;
                HeaderServiceBill.CompanyYear = CurrYear;
                var AllAcc = _unitOfWork.NativeSql.GetTransActionAccount(UserInfo.fCompanyId);
                foreach (var Trans in TransActionServiceBill)
                {
                    Price = Trans.Price;
                    Qty = Trans.Qty;
                    TaxPrec = Trans.Tax / 100;
                    if (HeaderServiceBill.ConversionFactor == 0)
                    {
                        HeaderServiceBill.ConversionFactor = 1;
                    };
                    if (HeaderServiceBill.ConversionFactor == 1)
                    {
                        Trans.ForeignPrice = 0;
                    }
                    else
                    {
                        Trans.ForeignPrice = Trans.Price / ExchangeRate;
                    }

                    Trans.PriceAfterLineDisc = Trans.Price - Trans.LocalDiscount;
                    Trans.ConversionFactor = ExchangeRate;
                    if (HeaderServiceBill.NoTax)
                    {
                        Trans.TaxValue = 0;

                    }
                    else
                    {
                        Trans.TaxValue = Trans.PriceAfterLineDisc * TaxPrec;
                    }
                    Trans.TotalLocal = Trans.PriceAfterLineDisc * Trans.Qty;
                    if (Trans.ForeignPrice > 0)
                    {
                        Trans.ForeignPriceAfterLineDisc = (Trans.Price / ExchangeRate) - Trans.LocalDiscount;
                        Trans.TotalForeign = Trans.ForeignPriceAfterLineDisc * Trans.Qty;
                    }
                    LineTotals = LineTotals + Trans.TotalLocal;
                }
                var TotalDiscPrec = HeaderServiceBill.Discount / LineTotals;
                foreach (var Trans in TransActionServiceBill)
                {
                    TaxPrec = Trans.Tax / 100;
                    Trans.TotalDiscPrec = TotalDiscPrec;
                    Trans.TotalDiscValue = TotalDiscPrec * Trans.TotalLocal;
                    Trans.NetAfterTotalDisc = Trans.TotalLocal - Trans.TotalDiscValue;
                    if (HeaderServiceBill.NoTax)
                    {
                        Trans.NetTotalTax = 0;
                    }
                    else
                    {
                        Trans.NetTotalTax = TaxPrec * Trans.NetAfterTotalDisc;
                    }
                    Total = Total + Trans.NetAfterTotalDisc;
                    TotalTax = TotalTax + Trans.NetTotalTax;
                }
                NetTotal = Total + TotalTax;
                TotalDebit = NetTotal;
                TotalCredit = Total + TotalTax;
                HeaderServiceBill.NetTotal = NetTotal;
                if (HeaderServiceBill.ConversionFactor == 0)
                {
                    HeaderServiceBill.ConversionFactor = 1;
                };
                if (HeaderServiceBill.ConversionFactor == 1)
                {
                    TotalDebitForeign = 0;
                    TotalCreditForeign = 0;
                }
                else
                {
                    TotalDebitForeign = Total / ExchangeRate;
                    TotalCreditForeign = (HeaderServiceBill.NetTotal) / ExchangeRate;
                }

                if (TotalCredit != TotalDebit)
                {
                    Msg.Msg = Resources.Resource.TheAccountingEntryIsUneven;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }
                if (TotalCredit <= 0)
                {
                    Msg.Msg = Resources.Resource.CantBeZero;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }
                var HeaderTOSave = new Header();
                HeaderTOSave.TransactionKindNo = TransactionKindNo;
                HeaderTOSave.CompanyTransactionKindNo = CompanyTransactionKindID;
                HeaderTOSave.InsDateTime = DateTime.Now;
                HeaderTOSave.InsUserID = userId;
                HeaderTOSave.CompanyID = UserInfo.fCompanyId;
                HeaderTOSave.TotalCredit = TotalCredit;
                HeaderTOSave.TotalDebit = TotalDebit;
                HeaderTOSave.TotalDebitForeign = TotalCreditForeign;
                HeaderTOSave.TotalCreditForeign = TotalCreditForeign;
                HeaderTOSave.CompanyYear = CurrYear;
                HeaderTOSave.VoucherNumber = _unitOfWork.Header.GetMaxVHByKind(UserInfo.fCompanyId, HeaderTOSave.CompanyTransactionKindNo, HeaderTOSave.TransactionKindNo, HeaderTOSave.CompanyYear).ToString();
                var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                if (Company.DirectExportTheTransactions)
                {
                    HeaderTOSave.Exported = 1;
                }
                else
                {
                    HeaderTOSave.Exported = 0;
                }
                HeaderTOSave.VHI = int.Parse(HeaderTOSave.VoucherNumber);
                HeaderTOSave.VoucherDate = HeaderServiceBill.BillDate;
                HeaderTOSave.FCurrencyID = HeaderServiceBill.FCurrencyID;
                HeaderTOSave.ConversionFactor = HeaderServiceBill.ConversionFactor;
                HeaderTOSave.SaleID = HeaderServiceBill.SaleManNo;
                iRow = 0;
                Transaction ObjDebit = new Transaction();
                ObjDebit.AccountNumber = HeaderServiceBill.DebitAccountNumber;
                ObjDebit.CostCenter = HeaderServiceBill.DebitCostNumber;
                ObjDebit.CompanyTransactionKindNo = CompanyTransactionKindID;
                ObjDebit.TransactionKindNo = TransactionKindNo;
                ObjDebit.CompanyYear = HeaderTOSave.CompanyYear;
                ObjDebit.CompanyID = HeaderTOSave.CompanyID;
                ObjDebit.Credit = 0;
                ObjDebit.CreditForeign = 0;
                ObjDebit.Debit = TotalDebit;
                if (HeaderServiceBill.ConversionFactor == 0)
                {
                    HeaderServiceBill.ConversionFactor = 1;
                };
                if (HeaderServiceBill.ConversionFactor == 1)
                {
                    ObjDebit.DebitForeign = 0;
                    ObjDebit.CreditDebitForeign = 0;
                }
                else
                {
                    ObjDebit.DebitForeign = TotalDebit / ExchangeRate;
                    ObjDebit.CreditDebitForeign = TotalDebit / ExchangeRate;
                }
                ObjDebit.Exported = 0;
                ObjDebit.InsDateTime = DateTime.Now;
                ObjDebit.RowNumber = 1;
                ObjDebit.VHI = HeaderTOSave.VHI;
                ObjDebit.VoucherDate = HeaderTOSave.VoucherDate;
                ObjDebit.VoucherNumber = HeaderTOSave.VoucherNumber;
                _unitOfWork.Transaction.Add(ObjDebit);
                iRow = 1;
                foreach (var Trans in TransActionServiceBill)
                {
                    Transaction ObjCredit = new Transaction();
                    iRow = iRow + 1;
                    ObjCredit.CompanyTransactionKindNo = CompanyTransactionKindID;
                    ObjCredit.TransactionKindNo = TransactionKindNo;
                    ObjCredit.CompanyYear = HeaderTOSave.CompanyYear;
                    ObjCredit.CompanyID = HeaderTOSave.CompanyID;
                    ObjCredit.Credit = Trans.NetAfterTotalDisc;
                    if (HeaderServiceBill.ConversionFactor == 0)
                    {
                        HeaderServiceBill.ConversionFactor = 1;
                    };
                    if (HeaderServiceBill.ConversionFactor == 1)
                    {
                        ObjCredit.CreditForeign = 0;
                        ObjCredit.CreditDebitForeign = 0;
                    }
                    else
                    {
                        ObjCredit.CreditDebitForeign = ObjCredit.Credit / ExchangeRate;
                        ObjCredit.CreditForeign = ObjCredit.Credit / ExchangeRate;
                    }
                    ObjCredit.Debit = 0;
                    ObjCredit.DebitForeign = 0;
                    ObjCredit.Exported = 0;
                    ObjCredit.InsDateTime = DateTime.Now;
                    ObjCredit.RowNumber = iRow;
                    ObjCredit.VHI = HeaderTOSave.VHI;
                    ObjCredit.VoucherDate = HeaderTOSave.VoucherDate;
                    ObjCredit.VoucherNumber = HeaderTOSave.VoucherNumber;
                    _unitOfWork.Transaction.Add(ObjCredit);
                }
                if (!HeaderServiceBill.NoTax)
                {
                    Transaction ObjTax = new Transaction();
                    ObjTax.AccountNumber = HeaderServiceBill.TaxAccountNumber;
                    ObjTax.CostCenter = HeaderServiceBill.TaxCostNumber;
                    ObjTax.CompanyTransactionKindNo = CompanyTransactionKindID;
                    ObjTax.TransactionKindNo = TransactionKindNo;
                    ObjTax.CompanyYear = HeaderTOSave.CompanyYear;
                    ObjTax.CompanyID = HeaderTOSave.CompanyID;
                    ObjTax.Credit = TotalTax;
                    if (HeaderServiceBill.ConversionFactor == 0)
                    {
                        HeaderServiceBill.ConversionFactor = 1;
                    };
                    if (HeaderServiceBill.ConversionFactor == 1)
                    {
                        ObjTax.CreditForeign = 0;
                        ObjTax.CreditDebitForeign = 0;
                    }
                    else
                    {
                        ObjTax.CreditDebitForeign = TotalTax / ExchangeRate;
                        ObjTax.CreditForeign = TotalTax / ExchangeRate;
                    }
                    ObjTax.Debit = 0;
                    ObjTax.DebitForeign = 0;
                    ObjTax.Exported = 0;
                    ObjTax.InsDateTime = DateTime.Now;
                    iRow = iRow + 1;
                    ObjTax.RowNumber = iRow;
                    ObjTax.VHI = HeaderTOSave.VHI;
                    ObjTax.VoucherDate = HeaderTOSave.VoucherDate;
                    ObjTax.VoucherNumber = HeaderTOSave.VoucherNumber;
                    _unitOfWork.Transaction.Add(ObjTax);
                }
                HeaderTOSave.RowCount = iRow;
                _unitOfWork.Header.Add(HeaderTOSave);
                iRow = 1;
                foreach (var Trans in TransActionServiceBill)
                {
                    Trans.BillID = HeaderTOSave.VHI;
                    Trans.CompanyID = HeaderTOSave.CompanyID;
                    Trans.CompanyYear = HeaderTOSave.CompanyYear;
                    Trans.iRowID = iRow;
                    Trans.CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo;
                    Trans.TransactionKindNo = HeaderTOSave.TransactionKindNo;
                    Trans.FCurrencyID = HeaderTOSave.FCurrencyID;
                    iRow = iRow + 1;
                    _unitOfWork.TransActionServiceBill.Add(Trans);
                }
                HeaderServiceBill.BillID = HeaderTOSave.VHI;
                HeaderServiceBill.CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo;
                HeaderServiceBill.TransactionKindNo = HeaderTOSave.TransactionKindNo;
                _unitOfWork.ServiceBillHeader.Add(HeaderServiceBill);
                _unitOfWork.Complete();
                Msg.Code = 1;
                Msg.Msg = Resources.Resource.AddedSuccessfully;
                Msg.Year = HeaderTOSave.CompanyYear;
                Msg.FCompanyId = HeaderTOSave.CompanyID;
                Msg.VoucherNumber = HeaderTOSave.VoucherNumber.ToString();
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
        public JsonResult SaveNewMultiReturnServiceBill(TransServiceBillVM ObjSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                int CurrYear = UserInfo.CurrYear;
                var TransActionServiceBill = ObjSave.TransActionServiceBill;
                HeaderServiceBill HeaderServiceBill = new HeaderServiceBill();
                HeaderServiceBill.AccountNumber = ObjSave.HeaderServiceBill.AccountNumber;
                HeaderServiceBill.BillDate = ObjSave.HeaderServiceBill.BillDate;
                HeaderServiceBill.CompanyID = ObjSave.HeaderServiceBill.CompanyID;
                HeaderServiceBill.CompanyYear = ObjSave.HeaderServiceBill.CompanyYear;
                HeaderServiceBill.CreditAccountNumber = ObjSave.HeaderServiceBill.CreditAccountNumber;
                HeaderServiceBill.CreditCostNumber = ObjSave.HeaderServiceBill.CreditCostNumber;
                HeaderServiceBill.DebitAccountNumber = ObjSave.HeaderServiceBill.DebitAccountNumber;
                HeaderServiceBill.DebitCostNumber = ObjSave.HeaderServiceBill.DebitCostNumber;
                HeaderServiceBill.Discount = ObjSave.HeaderServiceBill.Discount;
                HeaderServiceBill.ConversionFactor = ObjSave.HeaderServiceBill.CurrencyNewValue;
                HeaderServiceBill.FCurrencyID = ObjSave.HeaderServiceBill.CurrencyID;
                HeaderServiceBill.NetTotal = ObjSave.HeaderServiceBill.NetTotal;
                HeaderServiceBill.NetTotalForeignAmount = ObjSave.HeaderServiceBill.NetTotalForeignAmount;
                HeaderServiceBill.ForeignAmount = ObjSave.HeaderServiceBill.ForeignAmount;
                HeaderServiceBill.ForeignAmountTax = ObjSave.HeaderServiceBill.ForeignAmountTax;
                HeaderServiceBill.NoTax = ObjSave.HeaderServiceBill.NoTax;
                HeaderServiceBill.Note = ObjSave.HeaderServiceBill.Note;
                HeaderServiceBill.TransactionKindNo = 20;
                HeaderServiceBill.CompanyTransactionKindNo = ObjSave.HeaderServiceBill.CompanyTransactionKindID;
                HeaderServiceBill.Tax = ObjSave.HeaderServiceBill.Tax;
                HeaderServiceBill.TaxAccountNumber = ObjSave.HeaderServiceBill.TaxAccountNumber;
                HeaderServiceBill.TaxCostNumber = ObjSave.HeaderServiceBill.TaxCostNumber;
                HeaderServiceBill.Total = ObjSave.HeaderServiceBill.Total;
                HeaderServiceBill.SaleManNo = ObjSave.HeaderServiceBill.SaleManNo;
                var TransactionKindNo = 20;
                var CompanyTransactionKindID = ObjSave.HeaderServiceBill.CompanyTransactionKindID;
                double LineTotals = 0;
                double Price = 0;
                double Qty = 0;
                double TaxPrec = 0;
                double Total = 0;
                double TotalTax = 0;
                double NetTotal = 0;
                double TotalDebit = 0;
                double TotalCredit = 0;
                double TotalDebitForeign = 0;
                double TotalCreditForeign = 0;
                int iRow = 1;
                var ExchangeRate = HeaderServiceBill.ConversionFactor;
                HeaderServiceBill.CompanyID = UserInfo.fCompanyId;
                HeaderServiceBill.CompanyYear = CurrYear;
                var AllAcc = _unitOfWork.NativeSql.GetTransActionAccount(UserInfo.fCompanyId);
                foreach (var Trans in TransActionServiceBill)
                {
                    Price = Trans.Price;
                    Qty = Trans.Qty;
                    TaxPrec = Trans.Tax / 100;
                    if (HeaderServiceBill.ConversionFactor == 0)
                    {
                        HeaderServiceBill.ConversionFactor = 1;
                    };
                    if (HeaderServiceBill.ConversionFactor == 1)
                    {
                        Trans.ForeignPrice = 0;
                    }
                    else
                    {
                        Trans.ForeignPrice = Trans.Price / ExchangeRate;
                    }

                    Trans.PriceAfterLineDisc = Trans.Price - Trans.LocalDiscount;
                    Trans.ConversionFactor = ExchangeRate;
                    if (HeaderServiceBill.NoTax)
                    {
                        Trans.TaxValue = 0;

                    }
                    else
                    {
                        Trans.TaxValue = Trans.PriceAfterLineDisc * TaxPrec;
                    }
                    Trans.TotalLocal = Trans.PriceAfterLineDisc * Trans.Qty;
                    if (Trans.ForeignPrice > 0)
                    {
                        Trans.ForeignPriceAfterLineDisc = (Trans.Price / ExchangeRate) - Trans.LocalDiscount;
                        Trans.TotalForeign = Trans.ForeignPriceAfterLineDisc * Trans.Qty;
                    }
                    LineTotals = LineTotals + Trans.TotalLocal;
                }
                var TotalDiscPrec = HeaderServiceBill.Discount / LineTotals;
                foreach (var Trans in TransActionServiceBill)
                {
                    TaxPrec = Trans.Tax / 100;
                    Trans.TotalDiscPrec = TotalDiscPrec;
                    Trans.TotalDiscValue = TotalDiscPrec * Trans.TotalLocal;
                    Trans.NetAfterTotalDisc = Trans.TotalLocal - Trans.TotalDiscValue;
                    if (HeaderServiceBill.NoTax)
                    {
                        Trans.NetTotalTax = 0;
                    }
                    else
                    {
                        Trans.NetTotalTax = TaxPrec * Trans.NetAfterTotalDisc;
                    }
                    Total = Total + Trans.NetAfterTotalDisc;
                    TotalTax = TotalTax + Trans.NetTotalTax;
                }
                NetTotal = Total + TotalTax;
                TotalDebit = Total + TotalTax;
                TotalCredit = NetTotal;
                HeaderServiceBill.NetTotal = NetTotal;
                if (HeaderServiceBill.ConversionFactor == 0)
                {
                    HeaderServiceBill.ConversionFactor = 1;
                };
                if (HeaderServiceBill.ConversionFactor == 1)
                {
                    TotalDebitForeign = 0;
                    TotalCreditForeign = 0;
                }
                else
                {
                    TotalDebitForeign = Total / ExchangeRate;
                    TotalCreditForeign = (HeaderServiceBill.NetTotal) / ExchangeRate;
                }

                if (TotalCredit != TotalDebit)
                {
                    Msg.Msg = Resources.Resource.TheAccountingEntryIsUneven;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }
                if (TotalCredit <= 0)
                {
                    Msg.Msg = Resources.Resource.CantBeZero;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }
                var HeaderTOSave = new Header();
                HeaderTOSave.TransactionKindNo = TransactionKindNo;
                HeaderTOSave.CompanyTransactionKindNo = CompanyTransactionKindID;
                HeaderTOSave.InsDateTime = DateTime.Now;
                HeaderTOSave.InsUserID = userId;
                HeaderTOSave.CompanyID = UserInfo.fCompanyId;
                HeaderTOSave.TotalCredit = TotalCredit;
                HeaderTOSave.TotalDebit = TotalDebit;
                HeaderTOSave.TotalDebitForeign = TotalDebitForeign;
                HeaderTOSave.TotalCreditForeign = TotalDebitForeign;
                HeaderTOSave.CompanyYear = CurrYear;
                HeaderTOSave.VoucherNumber = _unitOfWork.Header.GetMaxVHByKind(UserInfo.fCompanyId, HeaderTOSave.CompanyTransactionKindNo, HeaderTOSave.TransactionKindNo, HeaderTOSave.CompanyYear).ToString();
                var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                if (Company.DirectExportTheTransactions)
                {
                    HeaderTOSave.Exported = 1;
                }
                else
                {
                    HeaderTOSave.Exported = 0;
                }
                HeaderTOSave.VHI = int.Parse(HeaderTOSave.VoucherNumber);
                HeaderTOSave.VoucherDate = HeaderServiceBill.BillDate;
                HeaderTOSave.FCurrencyID = HeaderServiceBill.FCurrencyID;
                HeaderTOSave.ConversionFactor = HeaderServiceBill.ConversionFactor;
                HeaderTOSave.SaleID = HeaderServiceBill.SaleManNo;
                iRow = 0;
                Transaction ObjCredit = new Transaction();
                ObjCredit.AccountNumber = HeaderServiceBill.CreditAccountNumber;
                ObjCredit.CostCenter = HeaderServiceBill.CreditCostNumber;
                ObjCredit.CompanyTransactionKindNo = CompanyTransactionKindID;
                ObjCredit.TransactionKindNo = TransactionKindNo;
                ObjCredit.CompanyYear = HeaderTOSave.CompanyYear;
                ObjCredit.CompanyID = HeaderTOSave.CompanyID;
                ObjCredit.Debit = 0;
                ObjCredit.DebitForeign = 0;
                ObjCredit.Credit = TotalCredit;
                if (HeaderServiceBill.ConversionFactor == 0)
                {
                    HeaderServiceBill.ConversionFactor = 1;
                };
                if (HeaderServiceBill.ConversionFactor == 1)
                {
                    ObjCredit.CreditForeign = 0;
                    ObjCredit.CreditDebitForeign = 0;
                }
                else
                {
                    ObjCredit.CreditForeign = TotalCredit / ExchangeRate;
                    ObjCredit.CreditDebitForeign = TotalCredit / ExchangeRate;
                }
                ObjCredit.Exported = 0;
                ObjCredit.InsDateTime = DateTime.Now;
                ObjCredit.RowNumber = 1;
                ObjCredit.VHI = HeaderTOSave.VHI;
                ObjCredit.VoucherDate = HeaderTOSave.VoucherDate;
                ObjCredit.VoucherNumber = HeaderTOSave.VoucherNumber;
                _unitOfWork.Transaction.Add(ObjCredit);
                iRow = 1;
                foreach (var Trans in TransActionServiceBill)
                {
                    iRow = iRow + 1;
                    Transaction ObjDebit = new Transaction();
                    ObjDebit.CompanyTransactionKindNo = CompanyTransactionKindID;
                    ObjDebit.TransactionKindNo = TransactionKindNo;
                    ObjDebit.CompanyYear = HeaderTOSave.CompanyYear;
                    ObjDebit.CompanyID = HeaderTOSave.CompanyID;
                    ObjDebit.Debit = Trans.NetAfterTotalDisc;
                    if (HeaderServiceBill.ConversionFactor == 0)
                    {
                        HeaderServiceBill.ConversionFactor = 1;
                    };
                    if (HeaderServiceBill.ConversionFactor == 1)
                    {
                        ObjDebit.DebitForeign = 0;
                        ObjDebit.CreditDebitForeign = 0;
                    }
                    else
                    {
                        ObjDebit.CreditDebitForeign = ObjDebit.Debit / ExchangeRate;
                        ObjDebit.DebitForeign = ObjDebit.Debit / ExchangeRate;
                    }
                    ObjDebit.Credit = 0;
                    ObjDebit.CreditForeign = 0;
                    ObjDebit.Exported = 0;
                    ObjDebit.InsDateTime = DateTime.Now;
                    ObjDebit.RowNumber = iRow;
                    ObjDebit.VHI = HeaderTOSave.VHI;
                    ObjDebit.VoucherDate = HeaderTOSave.VoucherDate;
                    ObjDebit.VoucherNumber = HeaderTOSave.VoucherNumber;
                    _unitOfWork.Transaction.Add(ObjDebit);
                }
                if (!HeaderServiceBill.NoTax)
                {
                    Transaction ObjTax = new Transaction();
                    ObjTax.AccountNumber = HeaderServiceBill.TaxAccountNumber;
                    ObjTax.CostCenter = HeaderServiceBill.TaxCostNumber;
                    ObjTax.CompanyTransactionKindNo = CompanyTransactionKindID;
                    ObjTax.TransactionKindNo = TransactionKindNo;
                    ObjTax.CompanyYear = HeaderTOSave.CompanyYear;
                    ObjTax.CompanyID = HeaderTOSave.CompanyID;
                    ObjTax.Debit = TotalTax;
                    if (HeaderServiceBill.ConversionFactor == 0)
                    {
                        HeaderServiceBill.ConversionFactor = 1;
                    };
                    if (HeaderServiceBill.ConversionFactor == 1)
                    {
                        ObjTax.DebitForeign = 0;
                        ObjTax.CreditDebitForeign = 0;
                    }
                    else
                    {
                        ObjTax.CreditDebitForeign = TotalTax / ExchangeRate;
                        ObjTax.DebitForeign = TotalTax / ExchangeRate;
                    }
                    ObjTax.Credit = 0;
                    ObjTax.CreditForeign = 0;
                    ObjTax.Exported = 0;
                    ObjTax.InsDateTime = DateTime.Now;
                    iRow = iRow + 1;
                    ObjTax.RowNumber = iRow;
                    ObjTax.VHI = HeaderTOSave.VHI;
                    ObjTax.VoucherDate = HeaderTOSave.VoucherDate;
                    ObjTax.VoucherNumber = HeaderTOSave.VoucherNumber;
                    _unitOfWork.Transaction.Add(ObjTax);
                }
                HeaderTOSave.RowCount = iRow;
                _unitOfWork.Header.Add(HeaderTOSave);
                iRow = 1;
                foreach (var Trans in TransActionServiceBill)
                {
                    Trans.BillID = HeaderTOSave.VHI;
                    Trans.CompanyID = HeaderTOSave.CompanyID;
                    Trans.CompanyYear = HeaderTOSave.CompanyYear;
                    Trans.iRowID = iRow;
                    Trans.CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo;
                    Trans.TransactionKindNo = HeaderTOSave.TransactionKindNo;
                    Trans.FCurrencyID = HeaderTOSave.FCurrencyID;
                    iRow = iRow + 1;
                    _unitOfWork.TransActionServiceBill.Add(Trans);
                }
                HeaderServiceBill.BillID = HeaderTOSave.VHI;
                HeaderServiceBill.CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo;
                HeaderServiceBill.TransactionKindNo = HeaderTOSave.TransactionKindNo;
                _unitOfWork.ServiceBillHeader.Add(HeaderServiceBill);
                _unitOfWork.Complete();
                Msg.Code = 1;
                Msg.Msg = Resources.Resource.AddedSuccessfully;
                Msg.Year = HeaderTOSave.CompanyYear;
                Msg.FCompanyId = HeaderTOSave.CompanyID;
                Msg.VoucherNumber = HeaderTOSave.VoucherNumber.ToString();
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
        public JsonResult SaveNewMultiPurchaseServiceBill(TransServiceBillVM ObjSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                int CurrYear = UserInfo.CurrYear;
                var TransActionServiceBill = ObjSave.TransActionServiceBill;
                HeaderServiceBill HeaderServiceBill = new HeaderServiceBill();
                HeaderServiceBill.AccountNumber = ObjSave.HeaderServiceBill.AccountNumber;
                HeaderServiceBill.BillDate = ObjSave.HeaderServiceBill.BillDate;
                HeaderServiceBill.CompanyID = ObjSave.HeaderServiceBill.CompanyID;
                HeaderServiceBill.CompanyYear = ObjSave.HeaderServiceBill.CompanyYear;
                HeaderServiceBill.CreditAccountNumber = ObjSave.HeaderServiceBill.CreditAccountNumber;
                HeaderServiceBill.CreditCostNumber = ObjSave.HeaderServiceBill.CreditCostNumber;
                HeaderServiceBill.DebitAccountNumber = ObjSave.HeaderServiceBill.DebitAccountNumber;
                HeaderServiceBill.DebitCostNumber = ObjSave.HeaderServiceBill.DebitCostNumber;
                HeaderServiceBill.Discount = ObjSave.HeaderServiceBill.Discount;
                HeaderServiceBill.ConversionFactor = ObjSave.HeaderServiceBill.CurrencyNewValue;
                HeaderServiceBill.FCurrencyID = ObjSave.HeaderServiceBill.CurrencyID;
                HeaderServiceBill.NetTotal = ObjSave.HeaderServiceBill.NetTotal;
                HeaderServiceBill.NetTotalForeignAmount = ObjSave.HeaderServiceBill.NetTotalForeignAmount;
                HeaderServiceBill.ForeignAmount = ObjSave.HeaderServiceBill.ForeignAmount;
                HeaderServiceBill.ForeignAmountTax = ObjSave.HeaderServiceBill.ForeignAmountTax;
                HeaderServiceBill.NoTax = ObjSave.HeaderServiceBill.NoTax;
                HeaderServiceBill.Note = ObjSave.HeaderServiceBill.Note;
                HeaderServiceBill.TransactionKindNo = 13;
                HeaderServiceBill.CompanyTransactionKindNo = ObjSave.HeaderServiceBill.CompanyTransactionKindID;
                HeaderServiceBill.Tax = ObjSave.HeaderServiceBill.Tax;
                HeaderServiceBill.TaxAccountNumber = ObjSave.HeaderServiceBill.TaxAccountNumber;
                HeaderServiceBill.TaxCostNumber = ObjSave.HeaderServiceBill.TaxCostNumber;
                HeaderServiceBill.Total = ObjSave.HeaderServiceBill.Total;
                HeaderServiceBill.SaleManNo = ObjSave.HeaderServiceBill.SaleManNo;
                var TransactionKindNo = 13;
                var CompanyTransactionKindID = ObjSave.HeaderServiceBill.CompanyTransactionKindID;
                double LineTotals = 0;
                double Price = 0;
                double Qty = 0;
                double TaxPrec = 0;
                double Total = 0;
                double TotalTax = 0;
                double NetTotal = 0;
                double TotalDebit = 0;
                double TotalCredit = 0;
                double TotalDebitForeign = 0;
                double TotalCreditForeign = 0;
                int iRow = 1;
                var ExchangeRate = HeaderServiceBill.ConversionFactor;
                HeaderServiceBill.CompanyID = UserInfo.fCompanyId;
                HeaderServiceBill.CompanyYear = CurrYear;
                var AllAcc = _unitOfWork.NativeSql.GetTransActionAccount(UserInfo.fCompanyId);
                foreach (var Trans in TransActionServiceBill)
                {
                    Price = Trans.Price;
                    Qty = Trans.Qty;
                    TaxPrec = Trans.Tax / 100;
                    if (HeaderServiceBill.ConversionFactor == 0)
                    {
                        HeaderServiceBill.ConversionFactor = 1;
                    };
                    if (HeaderServiceBill.ConversionFactor == 1)
                    {
                        Trans.ForeignPrice = 0;
                    }
                    else
                    {
                        Trans.ForeignPrice = Trans.Price / ExchangeRate;
                    }

                    Trans.PriceAfterLineDisc = Trans.Price - Trans.LocalDiscount;
                    Trans.ConversionFactor = ExchangeRate;
                    if (HeaderServiceBill.NoTax)
                    {
                        Trans.TaxValue = 0;

                    }
                    else
                    {
                        Trans.TaxValue = Trans.PriceAfterLineDisc * TaxPrec;
                    }
                    Trans.TotalLocal = Trans.PriceAfterLineDisc * Trans.Qty;
                    if (Trans.ForeignPrice > 0)
                    {
                        Trans.ForeignPriceAfterLineDisc = (Trans.Price / ExchangeRate) - Trans.LocalDiscount;
                        Trans.TotalForeign = Trans.ForeignPriceAfterLineDisc * Trans.Qty;
                    }
                    LineTotals = LineTotals + Trans.TotalLocal;
                }
                var TotalDiscPrec = HeaderServiceBill.Discount / LineTotals;
                foreach (var Trans in TransActionServiceBill)
                {
                    TaxPrec = Trans.Tax / 100;
                    Trans.TotalDiscPrec = TotalDiscPrec;
                    Trans.TotalDiscValue = TotalDiscPrec * Trans.TotalLocal;
                    Trans.NetAfterTotalDisc = Trans.TotalLocal - Trans.TotalDiscValue;
                    if (HeaderServiceBill.NoTax)
                    {
                        Trans.NetTotalTax = 0;
                    }
                    else
                    {
                        Trans.NetTotalTax = TaxPrec * Trans.NetAfterTotalDisc;
                    }
                    Total = Total + Trans.NetAfterTotalDisc;
                    TotalTax = TotalTax + Trans.NetTotalTax;
                }
                NetTotal = Total + TotalTax;
                TotalDebit = Total + TotalTax;
                TotalCredit = NetTotal;
                HeaderServiceBill.NetTotal = NetTotal;
                if (HeaderServiceBill.ConversionFactor == 0)
                {
                    HeaderServiceBill.ConversionFactor = 1;
                };
                if (HeaderServiceBill.ConversionFactor == 1)
                {
                    TotalDebitForeign = 0;
                    TotalCreditForeign = 0;
                }
                else
                {
                    TotalDebitForeign = Total / ExchangeRate;
                    TotalCreditForeign = (HeaderServiceBill.NetTotal) / ExchangeRate;
                }

                if (TotalCredit != TotalDebit)
                {
                    Msg.Msg = Resources.Resource.TheAccountingEntryIsUneven;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }
                if (TotalCredit <= 0)
                {
                    Msg.Msg = Resources.Resource.CantBeZero;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }
                var HeaderTOSave = new Header();
                HeaderTOSave.TransactionKindNo = TransactionKindNo;
                HeaderTOSave.CompanyTransactionKindNo = CompanyTransactionKindID;
                HeaderTOSave.InsDateTime = DateTime.Now;
                HeaderTOSave.InsUserID = userId;
                HeaderTOSave.CompanyID = UserInfo.fCompanyId;
                HeaderTOSave.TotalCredit = TotalCredit;
                HeaderTOSave.TotalDebit = TotalDebit;
                HeaderTOSave.TotalDebitForeign = TotalDebitForeign;
                HeaderTOSave.TotalCreditForeign = TotalDebitForeign;
                HeaderTOSave.CompanyYear = CurrYear;
                HeaderTOSave.VoucherNumber = _unitOfWork.Header.GetMaxVHByKind(UserInfo.fCompanyId, HeaderTOSave.CompanyTransactionKindNo, HeaderTOSave.TransactionKindNo, HeaderTOSave.CompanyYear).ToString();
                var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                if (Company.DirectExportTheTransactions)
                {
                    HeaderTOSave.Exported = 1;
                }
                else
                {
                    HeaderTOSave.Exported = 0;
                }
                HeaderTOSave.VHI = int.Parse(HeaderTOSave.VoucherNumber);
                HeaderTOSave.VoucherDate = HeaderServiceBill.BillDate;
                HeaderTOSave.FCurrencyID = HeaderServiceBill.FCurrencyID;
                HeaderTOSave.ConversionFactor = HeaderServiceBill.ConversionFactor;
                HeaderTOSave.SaleID = HeaderServiceBill.SaleManNo;
                iRow = 0;
                Transaction ObjCredit = new Transaction();
                ObjCredit.AccountNumber = HeaderServiceBill.CreditAccountNumber;
                ObjCredit.CostCenter = HeaderServiceBill.CreditCostNumber;
                ObjCredit.CompanyTransactionKindNo = CompanyTransactionKindID;
                ObjCredit.TransactionKindNo = TransactionKindNo;
                ObjCredit.CompanyYear = HeaderTOSave.CompanyYear;
                ObjCredit.CompanyID = HeaderTOSave.CompanyID;
                ObjCredit.Debit = 0;
                ObjCredit.DebitForeign = 0;
                ObjCredit.Credit = TotalCredit;
                if (HeaderServiceBill.ConversionFactor == 0)
                {
                    HeaderServiceBill.ConversionFactor = 1;
                };
                if (HeaderServiceBill.ConversionFactor == 1)
                {
                    ObjCredit.CreditForeign = 0;
                    ObjCredit.CreditDebitForeign = 0;
                }
                else
                {
                    ObjCredit.CreditForeign = TotalCredit / ExchangeRate;
                    ObjCredit.CreditDebitForeign = TotalCredit / ExchangeRate;
                }
                ObjCredit.Exported = 0;
                ObjCredit.InsDateTime = DateTime.Now;
                ObjCredit.RowNumber = 1;
                ObjCredit.VHI = HeaderTOSave.VHI;
                ObjCredit.VoucherDate = HeaderTOSave.VoucherDate;
                ObjCredit.VoucherNumber = HeaderTOSave.VoucherNumber;
                _unitOfWork.Transaction.Add(ObjCredit);
                iRow = 1;
                foreach (var Trans in TransActionServiceBill)
                {
                    iRow = iRow + 1;
                    Transaction ObjDebit = new Transaction();
                    ObjDebit.CompanyTransactionKindNo = CompanyTransactionKindID;
                    ObjDebit.TransactionKindNo = TransactionKindNo;
                    ObjDebit.CompanyYear = HeaderTOSave.CompanyYear;
                    ObjDebit.CompanyID = HeaderTOSave.CompanyID;
                    ObjDebit.Debit = Trans.NetAfterTotalDisc;
                    if (HeaderServiceBill.ConversionFactor == 0)
                    {
                        HeaderServiceBill.ConversionFactor = 1;
                    };
                    if (HeaderServiceBill.ConversionFactor == 1)
                    {
                        ObjDebit.DebitForeign = 0;
                        ObjDebit.CreditDebitForeign = 0;
                    }
                    else
                    {
                        ObjDebit.CreditDebitForeign = ObjDebit.Debit / ExchangeRate;
                        ObjDebit.DebitForeign = ObjDebit.Debit / ExchangeRate;
                    }
                    ObjDebit.Credit = 0;
                    ObjDebit.CreditForeign = 0;
                    ObjDebit.Exported = 0;
                    ObjDebit.InsDateTime = DateTime.Now;
                    ObjDebit.RowNumber = iRow;
                    ObjDebit.VHI = HeaderTOSave.VHI;
                    ObjDebit.VoucherDate = HeaderTOSave.VoucherDate;
                    ObjDebit.VoucherNumber = HeaderTOSave.VoucherNumber;
                    _unitOfWork.Transaction.Add(ObjDebit);
                }
                if (!HeaderServiceBill.NoTax)
                {
                    Transaction ObjTax = new Transaction();
                    ObjTax.AccountNumber = HeaderServiceBill.TaxAccountNumber;
                    ObjTax.CostCenter = HeaderServiceBill.TaxCostNumber;
                    ObjTax.CompanyTransactionKindNo = CompanyTransactionKindID;
                    ObjTax.TransactionKindNo = TransactionKindNo;
                    ObjTax.CompanyYear = HeaderTOSave.CompanyYear;
                    ObjTax.CompanyID = HeaderTOSave.CompanyID;
                    ObjTax.Debit = TotalTax;
                    if (HeaderServiceBill.ConversionFactor == 0)
                    {
                        HeaderServiceBill.ConversionFactor = 1;
                    };
                    if (HeaderServiceBill.ConversionFactor == 1)
                    {
                        ObjTax.DebitForeign = 0;
                        ObjTax.CreditDebitForeign = 0;
                    }
                    else
                    {
                        ObjTax.CreditDebitForeign = TotalTax / ExchangeRate;
                        ObjTax.DebitForeign = TotalTax / ExchangeRate;
                    }
                    ObjTax.Credit = 0;
                    ObjTax.CreditForeign = 0;
                    ObjTax.Exported = 0;
                    ObjTax.InsDateTime = DateTime.Now;
                    iRow = iRow + 1;
                    ObjTax.RowNumber = iRow;
                    ObjTax.VHI = HeaderTOSave.VHI;
                    ObjTax.VoucherDate = HeaderTOSave.VoucherDate;
                    ObjTax.VoucherNumber = HeaderTOSave.VoucherNumber;
                    _unitOfWork.Transaction.Add(ObjTax);
                }
                HeaderTOSave.RowCount = iRow;
                _unitOfWork.Header.Add(HeaderTOSave);
                iRow = 1;
                foreach (var Trans in TransActionServiceBill)
                {
                    Trans.BillID = HeaderTOSave.VHI;
                    Trans.CompanyID = HeaderTOSave.CompanyID;
                    Trans.CompanyYear = HeaderTOSave.CompanyYear;
                    Trans.iRowID = iRow;
                    Trans.CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo;
                    Trans.TransactionKindNo = HeaderTOSave.TransactionKindNo;
                    Trans.FCurrencyID = HeaderTOSave.FCurrencyID;
                    iRow = iRow + 1;
                    _unitOfWork.TransActionServiceBill.Add(Trans);
                }
                HeaderServiceBill.BillID = HeaderTOSave.VHI;
                HeaderServiceBill.CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo;
                HeaderServiceBill.TransactionKindNo = HeaderTOSave.TransactionKindNo;
                _unitOfWork.ServiceBillHeader.Add(HeaderServiceBill);
                _unitOfWork.Complete();
                Msg.Code = 1;
                Msg.Msg = Resources.Resource.AddedSuccessfully;
                Msg.Year = HeaderTOSave.CompanyYear;
                Msg.FCompanyId = HeaderTOSave.CompanyID;
                Msg.VoucherNumber = HeaderTOSave.VoucherNumber.ToString();
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
        public JsonResult SaveNewReturnMultiPurchaseServiceBill(TransServiceBillVM ObjSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                int CurrYear = UserInfo.CurrYear;
                var TransActionServiceBill = ObjSave.TransActionServiceBill;
                HeaderServiceBill HeaderServiceBill = new HeaderServiceBill();
                HeaderServiceBill.AccountNumber = ObjSave.HeaderServiceBill.AccountNumber;
                HeaderServiceBill.BillDate = ObjSave.HeaderServiceBill.BillDate;
                HeaderServiceBill.CompanyID = ObjSave.HeaderServiceBill.CompanyID;
                HeaderServiceBill.CompanyYear = ObjSave.HeaderServiceBill.CompanyYear;
                HeaderServiceBill.CreditAccountNumber = ObjSave.HeaderServiceBill.CreditAccountNumber;
                HeaderServiceBill.CreditCostNumber = ObjSave.HeaderServiceBill.CreditCostNumber;
                HeaderServiceBill.DebitAccountNumber = ObjSave.HeaderServiceBill.DebitAccountNumber;
                HeaderServiceBill.DebitCostNumber = ObjSave.HeaderServiceBill.DebitCostNumber;
                HeaderServiceBill.Discount = ObjSave.HeaderServiceBill.Discount;
                HeaderServiceBill.ConversionFactor = ObjSave.HeaderServiceBill.CurrencyNewValue;
                HeaderServiceBill.FCurrencyID = ObjSave.HeaderServiceBill.CurrencyID;
                HeaderServiceBill.NetTotal = ObjSave.HeaderServiceBill.NetTotal;
                HeaderServiceBill.NetTotalForeignAmount = ObjSave.HeaderServiceBill.NetTotalForeignAmount;
                HeaderServiceBill.ForeignAmount = ObjSave.HeaderServiceBill.ForeignAmount;
                HeaderServiceBill.ForeignAmountTax = ObjSave.HeaderServiceBill.ForeignAmountTax;
                HeaderServiceBill.NoTax = ObjSave.HeaderServiceBill.NoTax;
                HeaderServiceBill.Note = ObjSave.HeaderServiceBill.Note;
                HeaderServiceBill.TransactionKindNo = 22;
                HeaderServiceBill.CompanyTransactionKindNo = ObjSave.HeaderServiceBill.CompanyTransactionKindID;
                HeaderServiceBill.Tax = ObjSave.HeaderServiceBill.Tax;
                HeaderServiceBill.TaxAccountNumber = ObjSave.HeaderServiceBill.TaxAccountNumber;
                HeaderServiceBill.TaxCostNumber = ObjSave.HeaderServiceBill.TaxCostNumber;
                HeaderServiceBill.Total = ObjSave.HeaderServiceBill.Total;
                HeaderServiceBill.SaleManNo = ObjSave.HeaderServiceBill.SaleManNo;
                var TransactionKindNo = 22;
                var CompanyTransactionKindID = ObjSave.HeaderServiceBill.CompanyTransactionKindID;
                double LineTotals = 0;
                double Price = 0;
                double Qty = 0;
                double TaxPrec = 0;
                double Total = 0;
                double TotalTax = 0;
                double NetTotal = 0;
                double TotalDebit = 0;
                double TotalCredit = 0;
                double TotalDebitForeign = 0;
                double TotalCreditForeign = 0;
                int iRow = 1;
                var ExchangeRate = HeaderServiceBill.ConversionFactor;
                HeaderServiceBill.CompanyID = UserInfo.fCompanyId;
                HeaderServiceBill.CompanyYear = CurrYear;
                var AllAcc = _unitOfWork.NativeSql.GetTransActionAccount(UserInfo.fCompanyId);
                foreach (var Trans in TransActionServiceBill)
                {
                    Price = Trans.Price;
                    Qty = Trans.Qty;
                    TaxPrec = Trans.Tax / 100;
                    if (HeaderServiceBill.ConversionFactor == 0)
                    {
                        HeaderServiceBill.ConversionFactor = 1;
                    };
                    if (HeaderServiceBill.ConversionFactor == 1)
                    {
                        Trans.ForeignPrice = 0;
                    }
                    else
                    {
                        Trans.ForeignPrice = Trans.Price / ExchangeRate;
                    }

                    Trans.PriceAfterLineDisc = Trans.Price - Trans.LocalDiscount;
                    Trans.ConversionFactor = ExchangeRate;
                    if (HeaderServiceBill.NoTax)
                    {
                        Trans.TaxValue = 0;

                    }
                    else
                    {
                        Trans.TaxValue = Trans.PriceAfterLineDisc * TaxPrec;
                    }
                    Trans.TotalLocal = Trans.PriceAfterLineDisc * Trans.Qty;
                    if (Trans.ForeignPrice > 0)
                    {
                        Trans.ForeignPriceAfterLineDisc = (Trans.Price / ExchangeRate) - Trans.LocalDiscount;
                        Trans.TotalForeign = Trans.ForeignPriceAfterLineDisc * Trans.Qty;
                    }
                    LineTotals = LineTotals + Trans.TotalLocal;
                }
                var TotalDiscPrec = HeaderServiceBill.Discount / LineTotals;
                foreach (var Trans in TransActionServiceBill)
                {
                    TaxPrec = Trans.Tax / 100;
                    Trans.TotalDiscPrec = TotalDiscPrec;
                    Trans.TotalDiscValue = TotalDiscPrec * Trans.TotalLocal;
                    Trans.NetAfterTotalDisc = Trans.TotalLocal - Trans.TotalDiscValue;
                    if (HeaderServiceBill.NoTax)
                    {
                        Trans.NetTotalTax = 0;
                    }
                    else
                    {
                        Trans.NetTotalTax = TaxPrec * Trans.NetAfterTotalDisc;
                    }
                    Total = Total + Trans.NetAfterTotalDisc;
                    TotalTax = TotalTax + Trans.NetTotalTax;
                }
                NetTotal = Total + TotalTax;
                TotalDebit = NetTotal;
                TotalCredit = Total + TotalTax;
                HeaderServiceBill.NetTotal = NetTotal;
                if (HeaderServiceBill.ConversionFactor == 0)
                {
                    HeaderServiceBill.ConversionFactor = 1;
                };
                if (HeaderServiceBill.ConversionFactor == 1)
                {
                    TotalDebitForeign = 0;
                    TotalCreditForeign = 0;
                }
                else
                {
                    TotalDebitForeign = Total / ExchangeRate;
                    TotalCreditForeign = (HeaderServiceBill.NetTotal) / ExchangeRate;
                }

                if (TotalCredit != TotalDebit)
                {
                    Msg.Msg = Resources.Resource.TheAccountingEntryIsUneven;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }
                if (TotalCredit <= 0)
                {
                    Msg.Msg = Resources.Resource.CantBeZero;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }
                var HeaderTOSave = new Header();
                HeaderTOSave.TransactionKindNo = TransactionKindNo;
                HeaderTOSave.CompanyTransactionKindNo = CompanyTransactionKindID;
                HeaderTOSave.InsDateTime = DateTime.Now;
                HeaderTOSave.InsUserID = userId;
                HeaderTOSave.CompanyID = UserInfo.fCompanyId;
                HeaderTOSave.TotalCredit = TotalCredit;
                HeaderTOSave.TotalDebit = TotalDebit;
                HeaderTOSave.TotalDebitForeign = TotalCreditForeign;
                HeaderTOSave.TotalCreditForeign = TotalCreditForeign;
                HeaderTOSave.CompanyYear = CurrYear;
                HeaderTOSave.VoucherNumber = _unitOfWork.Header.GetMaxVHByKind(UserInfo.fCompanyId, HeaderTOSave.CompanyTransactionKindNo, HeaderTOSave.TransactionKindNo, HeaderTOSave.CompanyYear).ToString();
                var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                if (Company.DirectExportTheTransactions)
                {
                    HeaderTOSave.Exported = 1;
                }
                else
                {
                    HeaderTOSave.Exported = 0;
                }
                HeaderTOSave.VHI = int.Parse(HeaderTOSave.VoucherNumber);
                HeaderTOSave.VoucherDate = HeaderServiceBill.BillDate;
                HeaderTOSave.FCurrencyID = HeaderServiceBill.FCurrencyID;
                HeaderTOSave.ConversionFactor = HeaderServiceBill.ConversionFactor;
                HeaderTOSave.SaleID = HeaderServiceBill.SaleManNo;
                iRow = 0;
                Transaction ObjDebit = new Transaction();
                ObjDebit.AccountNumber = HeaderServiceBill.DebitAccountNumber;
                ObjDebit.CostCenter = HeaderServiceBill.DebitCostNumber;
                ObjDebit.CompanyTransactionKindNo = CompanyTransactionKindID;
                ObjDebit.TransactionKindNo = TransactionKindNo;
                ObjDebit.CompanyYear = HeaderTOSave.CompanyYear;
                ObjDebit.CompanyID = HeaderTOSave.CompanyID;
                ObjDebit.Credit = 0;
                ObjDebit.CreditForeign = 0;
                ObjDebit.Debit = TotalDebit;
                if (HeaderServiceBill.ConversionFactor == 0)
                {
                    HeaderServiceBill.ConversionFactor = 1;
                };
                if (HeaderServiceBill.ConversionFactor == 1)
                {
                    ObjDebit.DebitForeign = 0;
                    ObjDebit.CreditDebitForeign = 0;
                }
                else
                {
                    ObjDebit.DebitForeign = TotalDebit / ExchangeRate;
                    ObjDebit.CreditDebitForeign = TotalDebit / ExchangeRate;
                }
                ObjDebit.Exported = 0;
                ObjDebit.InsDateTime = DateTime.Now;
                ObjDebit.RowNumber = 1;
                ObjDebit.VHI = HeaderTOSave.VHI;
                ObjDebit.VoucherDate = HeaderTOSave.VoucherDate;
                ObjDebit.VoucherNumber = HeaderTOSave.VoucherNumber;
                _unitOfWork.Transaction.Add(ObjDebit);
                iRow = 1;
                foreach (var Trans in TransActionServiceBill)
                {
                    Transaction ObjCredit = new Transaction();
                    iRow = iRow + 1;
                    ObjCredit.CompanyTransactionKindNo = CompanyTransactionKindID;
                    ObjCredit.TransactionKindNo = TransactionKindNo;
                    ObjCredit.CompanyYear = HeaderTOSave.CompanyYear;
                    ObjCredit.CompanyID = HeaderTOSave.CompanyID;
                    ObjCredit.Credit = Trans.NetAfterTotalDisc;
                    if (HeaderServiceBill.ConversionFactor == 0)
                    {
                        HeaderServiceBill.ConversionFactor = 1;
                    };
                    if (HeaderServiceBill.ConversionFactor == 1)
                    {
                        ObjCredit.CreditForeign = 0;
                        ObjCredit.CreditDebitForeign = 0;
                    }
                    else
                    {
                        ObjCredit.CreditDebitForeign = ObjCredit.Credit / ExchangeRate;
                        ObjCredit.CreditForeign = ObjCredit.Credit / ExchangeRate;
                    }
                    ObjCredit.Debit = 0;
                    ObjCredit.DebitForeign = 0;
                    ObjCredit.Exported = 0;
                    ObjCredit.InsDateTime = DateTime.Now;
                    ObjCredit.RowNumber = iRow;
                    ObjCredit.VHI = HeaderTOSave.VHI;
                    ObjCredit.VoucherDate = HeaderTOSave.VoucherDate;
                    ObjCredit.VoucherNumber = HeaderTOSave.VoucherNumber;
                    _unitOfWork.Transaction.Add(ObjCredit);
                }
                if (!HeaderServiceBill.NoTax)
                {
                    Transaction ObjTax = new Transaction();
                    ObjTax.AccountNumber = HeaderServiceBill.TaxAccountNumber;
                    ObjTax.CostCenter = HeaderServiceBill.TaxCostNumber;
                    ObjTax.CompanyTransactionKindNo = CompanyTransactionKindID;
                    ObjTax.TransactionKindNo = TransactionKindNo;
                    ObjTax.CompanyYear = HeaderTOSave.CompanyYear;
                    ObjTax.CompanyID = HeaderTOSave.CompanyID;
                    ObjTax.Credit = TotalTax;
                    if (HeaderServiceBill.ConversionFactor == 0)
                    {
                        HeaderServiceBill.ConversionFactor = 1;
                    };
                    if (HeaderServiceBill.ConversionFactor == 1)
                    {
                        ObjTax.CreditForeign = 0;
                        ObjTax.CreditDebitForeign = 0;
                    }
                    else
                    {
                        ObjTax.CreditDebitForeign = TotalTax / ExchangeRate;
                        ObjTax.CreditForeign = TotalTax / ExchangeRate;
                    }
                    ObjTax.Debit = 0;
                    ObjTax.DebitForeign = 0;
                    ObjTax.Exported = 0;
                    ObjTax.InsDateTime = DateTime.Now;
                    iRow = iRow + 1;
                    ObjTax.RowNumber = iRow;
                    ObjTax.VHI = HeaderTOSave.VHI;
                    ObjTax.VoucherDate = HeaderTOSave.VoucherDate;
                    ObjTax.VoucherNumber = HeaderTOSave.VoucherNumber;
                    _unitOfWork.Transaction.Add(ObjTax);
                }
                HeaderTOSave.RowCount = iRow;
                _unitOfWork.Header.Add(HeaderTOSave);
                iRow = 1;
                foreach (var Trans in TransActionServiceBill)
                {
                    Trans.BillID = HeaderTOSave.VHI;
                    Trans.CompanyID = HeaderTOSave.CompanyID;
                    Trans.CompanyYear = HeaderTOSave.CompanyYear;
                    Trans.iRowID = iRow;
                    Trans.CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo;
                    Trans.TransactionKindNo = HeaderTOSave.TransactionKindNo;
                    Trans.FCurrencyID = HeaderTOSave.FCurrencyID;
                    iRow = iRow + 1;
                    _unitOfWork.TransActionServiceBill.Add(Trans);
                }
                HeaderServiceBill.BillID = HeaderTOSave.VHI;
                HeaderServiceBill.CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo;
                HeaderServiceBill.TransactionKindNo = HeaderTOSave.TransactionKindNo;
                _unitOfWork.ServiceBillHeader.Add(HeaderServiceBill);
                _unitOfWork.Complete();
                Msg.Code = 1;
                Msg.Msg = Resources.Resource.AddedSuccessfully;
                Msg.Year = HeaderTOSave.CompanyYear;
                Msg.FCompanyId = HeaderTOSave.CompanyID;
                Msg.VoucherNumber = HeaderTOSave.VoucherNumber.ToString();
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
        [Authorize(Roles = "CoOwner,AttachServiceBill")]
        public ActionResult ShowAttach(int id, string id2, string id3, string id4)
        {

            TransActionFileVM Obj = new TransActionFileVM
            {
                Year = id,
                VoucherNumber = id2,
                TransactionKindNo = id4,
                CompanyTransactionKindNo = id3
            };

            return View("ShowAttach", Obj);

        }
        [HttpPost]
        public JsonResult UpdateServiceBill(TransServiceBillVM ObjSave)
        {
            MsgUnit Msg = new MsgUnit();
            var HeaderTOSave = new Header();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                int CurrYear = ObjSave.HeaderServiceBill.CompanyYear;
                var TransActionServiceBill = ObjSave.TransActionServiceBill;
                HeaderServiceBill HeaderServiceBill = new HeaderServiceBill();
                HeaderServiceBill.AccountNumber = ObjSave.HeaderServiceBill.AccountNumber;
                HeaderServiceBill.BillDate = ObjSave.HeaderServiceBill.BillDate;
                HeaderServiceBill.CompanyID = ObjSave.HeaderServiceBill.CompanyID;
                HeaderServiceBill.CompanyYear = ObjSave.HeaderServiceBill.CompanyYear;
                HeaderServiceBill.CreditAccountNumber = ObjSave.HeaderServiceBill.CreditAccountNumber;
                HeaderServiceBill.CreditCostNumber = ObjSave.HeaderServiceBill.CreditCostNumber;
                HeaderServiceBill.DebitAccountNumber = ObjSave.HeaderServiceBill.DebitAccountNumber;
                HeaderServiceBill.DebitCostNumber = ObjSave.HeaderServiceBill.DebitCostNumber;
                HeaderServiceBill.Discount = ObjSave.HeaderServiceBill.Discount;
                HeaderServiceBill.ConversionFactor = ObjSave.HeaderServiceBill.CurrencyNewValue;
                HeaderServiceBill.FCurrencyID = ObjSave.HeaderServiceBill.CurrencyID;
                HeaderServiceBill.NetTotal = ObjSave.HeaderServiceBill.NetTotal;
                HeaderServiceBill.NetTotalForeignAmount = ObjSave.HeaderServiceBill.NetTotalForeignAmount;
                HeaderServiceBill.ForeignAmount = ObjSave.HeaderServiceBill.ForeignAmount;
                HeaderServiceBill.ForeignAmountTax = ObjSave.HeaderServiceBill.ForeignAmountTax;
                HeaderServiceBill.NoTax = ObjSave.HeaderServiceBill.NoTax;
                HeaderServiceBill.Note = ObjSave.HeaderServiceBill.Note;
                HeaderServiceBill.TransactionKindNo = 10;
                HeaderServiceBill.CompanyTransactionKindNo = ObjSave.HeaderServiceBill.CompanyTransactionKindID;
                HeaderServiceBill.Tax = ObjSave.HeaderServiceBill.Tax;
                HeaderServiceBill.TaxAccountNumber = ObjSave.HeaderServiceBill.TaxAccountNumber;
                HeaderServiceBill.TaxCostNumber = ObjSave.HeaderServiceBill.TaxCostNumber;
                HeaderServiceBill.Total = ObjSave.HeaderServiceBill.Total;
                HeaderServiceBill.SaleManNo = ObjSave.HeaderServiceBill.SaleManNo;
                var TransactionKindNo = 10;
                var CompanyTransactionKindID = ObjSave.HeaderServiceBill.CompanyTransactionKindID;
                double LineTotals = 0;
                double Price = 0;
                double Qty = 0;
                double TaxPrec = 0;
                double Total = 0;
                double TotalTax = 0;
                double NetTotal = 0;
                double TotalDebit = 0;
                double TotalCredit = 0;
                double TotalDebitForeign = 0;
                double TotalCreditForeign = 0;
                int iRow = 1;
                var ExchangeRate = HeaderServiceBill.ConversionFactor;
                HeaderServiceBill.CompanyID = UserInfo.fCompanyId;
                HeaderServiceBill.CompanyYear = CurrYear;
                var AllAcc = _unitOfWork.NativeSql.GetTransActionAccount(UserInfo.fCompanyId);
                foreach (var Trans in TransActionServiceBill)
                {
                    Price = Trans.Price;
                    Qty = Trans.Qty;
                    TaxPrec = Trans.Tax / 100;
                    if (HeaderServiceBill.ConversionFactor == 0)
                    {
                        HeaderServiceBill.ConversionFactor = 1;
                    };
                    if (HeaderServiceBill.ConversionFactor == 1)
                    {
                        Trans.ForeignPrice = 0;
                    }
                    else
                    {
                        Trans.ForeignPrice = Trans.Price / ExchangeRate;
                    }

                    Trans.PriceAfterLineDisc = Trans.Price - Trans.LocalDiscount;
                    Trans.ConversionFactor = ExchangeRate;
                    if (HeaderServiceBill.NoTax)
                    {
                        Trans.TaxValue = 0;

                    }
                    else
                    {
                        Trans.TaxValue = Trans.PriceAfterLineDisc * TaxPrec;
                    }
                    Trans.TotalLocal = Trans.PriceAfterLineDisc * Trans.Qty;
                    if (Trans.ForeignPrice > 0)
                    {
                        Trans.ForeignPriceAfterLineDisc = (Trans.Price / ExchangeRate) - Trans.LocalDiscount;
                        Trans.TotalForeign = Trans.ForeignPriceAfterLineDisc * Trans.Qty;
                    }
                    LineTotals = LineTotals + Trans.TotalLocal;
                }
                var TotalDiscPrec = HeaderServiceBill.Discount / LineTotals;
                foreach (var Trans in TransActionServiceBill)
                {
                    TaxPrec = Trans.Tax / 100;
                    Trans.TotalDiscPrec = TotalDiscPrec;
                    Trans.TotalDiscValue = TotalDiscPrec * Trans.TotalLocal;
                    Trans.NetAfterTotalDisc = Trans.TotalLocal - Trans.TotalDiscValue;
                    if (HeaderServiceBill.NoTax)
                    {
                        Trans.NetTotalTax = 0;
                    }
                    else
                    {
                        Trans.NetTotalTax = TaxPrec * Trans.NetAfterTotalDisc;
                    }
                    Total = Total + Trans.NetAfterTotalDisc;
                    TotalTax = TotalTax + Trans.NetTotalTax;
                }
                NetTotal = Total + TotalTax;
                TotalDebit = NetTotal;
                TotalCredit = Total + TotalTax;
                HeaderServiceBill.NetTotal = NetTotal;
                if (HeaderServiceBill.ConversionFactor == 0)
                {
                    HeaderServiceBill.ConversionFactor = 1;
                };
                if (HeaderServiceBill.ConversionFactor == 1)
                {
                    TotalDebitForeign = 0;
                    TotalCreditForeign = 0;
                }
                else
                {
                    TotalDebitForeign = Total / ExchangeRate;
                    TotalCreditForeign = (HeaderServiceBill.NetTotal) / ExchangeRate;
                }

                if (TotalCredit != TotalDebit)
                {
                    Msg.Msg = Resources.Resource.TheAccountingEntryIsUneven;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }
                if (TotalCredit <= 0)
                {
                    Msg.Msg = Resources.Resource.CantBeZero;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }
                
                
                HeaderTOSave.TransactionKindNo = TransactionKindNo;
                HeaderTOSave.CompanyTransactionKindNo = CompanyTransactionKindID;
                HeaderTOSave.InsDateTime = DateTime.Now;
                HeaderTOSave.InsUserID = userId;
                HeaderTOSave.CompanyID = UserInfo.fCompanyId;
                HeaderTOSave.TotalCredit = TotalCredit;
                HeaderTOSave.TotalDebit = TotalDebit;
                HeaderTOSave.TotalDebitForeign = TotalCreditForeign;
                HeaderTOSave.TotalCreditForeign = TotalCreditForeign;
                HeaderTOSave.CompanyYear = CurrYear;
                HeaderTOSave.VoucherNumber = ObjSave.HeaderServiceBill.BillID.ToString();
                HeaderTOSave.VHI = int.Parse(HeaderTOSave.VoucherNumber);
                HeaderTOSave.VoucherDate = HeaderServiceBill.BillDate;
                HeaderTOSave.FCurrencyID = HeaderServiceBill.FCurrencyID;
                HeaderTOSave.ConversionFactor = HeaderServiceBill.ConversionFactor;
                if (HeaderServiceBill.NoTax)
                {
                    HeaderTOSave.RowCount = 2;
                }
                else
                {
                    HeaderTOSave.RowCount = 3;
                }
                HeaderTOSave.SaleID = HeaderServiceBill.SaleManNo;
                _unitOfWork.ServiceBillHeader.Delete(HeaderTOSave.CompanyID, HeaderTOSave.VHI, HeaderServiceBill.CompanyTransactionKindNo, HeaderServiceBill.TransactionKindNo, HeaderTOSave.CompanyYear);
                _unitOfWork.TransActionServiceBill.Delete(HeaderTOSave.CompanyID, HeaderTOSave.VHI, HeaderServiceBill.CompanyTransactionKindNo, HeaderServiceBill.TransactionKindNo, HeaderTOSave.CompanyYear);
                _unitOfWork.Transaction.MarkDeleteTrans(HeaderTOSave.CompanyID, HeaderTOSave.VoucherNumber, HeaderTOSave.CompanyTransactionKindNo, HeaderTOSave.TransactionKindNo, HeaderTOSave.CompanyYear);
                _unitOfWork.Complete();
                _unitOfWork.Header.Update(HeaderTOSave);
                
                iRow = 0;
                Transaction ObjDebit = new Transaction();
                ObjDebit.AccountNumber = HeaderServiceBill.DebitAccountNumber;
                ObjDebit.CostCenter = HeaderServiceBill.DebitCostNumber;
                ObjDebit.CompanyTransactionKindNo = CompanyTransactionKindID;
                ObjDebit.TransactionKindNo = TransactionKindNo;
                ObjDebit.CompanyYear = HeaderTOSave.CompanyYear;
                ObjDebit.CompanyID = HeaderTOSave.CompanyID;
                ObjDebit.Credit = 0;
                ObjDebit.CreditForeign = 0;
                ObjDebit.Debit = TotalDebit;
                if (HeaderServiceBill.ConversionFactor == 0)
                {
                    HeaderServiceBill.ConversionFactor = 1;
                };
                if (HeaderServiceBill.ConversionFactor == 1)
                {
                    ObjDebit.DebitForeign = 0;
                    ObjDebit.CreditDebitForeign = 0;
                }
                else
                {
                    ObjDebit.DebitForeign = TotalDebit / ExchangeRate;
                    ObjDebit.CreditDebitForeign = TotalDebit / ExchangeRate;
                }
                ObjDebit.Exported = 0;
                ObjDebit.InsDateTime = DateTime.Now;
                ObjDebit.RowNumber = 1;
                ObjDebit.VHI = HeaderTOSave.VHI;
                ObjDebit.VoucherDate = HeaderTOSave.VoucherDate;
                ObjDebit.VoucherNumber = HeaderTOSave.VoucherNumber;

                Transaction ObjCredit = new Transaction();
                ObjCredit.AccountNumber = HeaderServiceBill.CreditAccountNumber;
                ObjCredit.CostCenter = HeaderServiceBill.CreditCostNumber;
                ObjCredit.CompanyTransactionKindNo = CompanyTransactionKindID;
                ObjCredit.TransactionKindNo = TransactionKindNo;
                ObjCredit.CompanyYear = HeaderTOSave.CompanyYear;
                ObjCredit.CompanyID = HeaderTOSave.CompanyID;
                ObjCredit.Credit = Total;
                if (HeaderServiceBill.ConversionFactor == 0)
                {
                    HeaderServiceBill.ConversionFactor = 1;
                };
                if (HeaderServiceBill.ConversionFactor == 1)
                {
                    ObjCredit.CreditForeign = 0;
                    ObjCredit.CreditDebitForeign = 0;
                }
                else
                {
                    ObjCredit.CreditDebitForeign = Total / ExchangeRate;
                    ObjCredit.CreditForeign = Total / ExchangeRate;
                }
                ObjCredit.Debit = 0;
                ObjCredit.DebitForeign = 0;
                ObjCredit.Exported = 0;
                ObjCredit.InsDateTime = DateTime.Now;
                ObjCredit.RowNumber = 2;
                ObjCredit.VHI = HeaderTOSave.VHI;
                ObjCredit.VoucherDate = HeaderTOSave.VoucherDate;
                ObjCredit.VoucherNumber = HeaderTOSave.VoucherNumber;

                _unitOfWork.Transaction.Add(ObjDebit);
                _unitOfWork.Transaction.Add(ObjCredit);

                if (!HeaderServiceBill.NoTax)
                {
                    Transaction ObjTax = new Transaction();
                    ObjTax.AccountNumber = HeaderServiceBill.TaxAccountNumber;
                    ObjTax.CostCenter = HeaderServiceBill.TaxCostNumber;
                    ObjTax.CompanyTransactionKindNo = CompanyTransactionKindID;
                    ObjTax.TransactionKindNo = TransactionKindNo;
                    ObjTax.CompanyYear = HeaderTOSave.CompanyYear;
                    ObjTax.CompanyID = HeaderTOSave.CompanyID;
                    ObjTax.Credit = TotalTax;
                    if (HeaderServiceBill.ConversionFactor == 0)
                    {
                        HeaderServiceBill.ConversionFactor = 1;
                    };
                    if (HeaderServiceBill.ConversionFactor == 1)
                    {
                        ObjTax.CreditForeign = 0;
                        ObjTax.CreditDebitForeign = 0;
                    }
                    else
                    {
                        ObjTax.CreditDebitForeign = TotalTax / ExchangeRate;
                        ObjTax.CreditForeign = TotalTax / ExchangeRate;
                    }
                    ObjTax.Debit = 0;
                    ObjTax.DebitForeign = 0;
                    ObjTax.Exported = 0;
                    ObjTax.InsDateTime = DateTime.Now;
                    ObjTax.RowNumber = 3;
                    ObjTax.VHI = HeaderTOSave.VHI;
                    ObjTax.VoucherDate = HeaderTOSave.VoucherDate;
                    ObjTax.VoucherNumber = HeaderTOSave.VoucherNumber;
                    _unitOfWork.Transaction.Add(ObjTax);
                }

                iRow = 1;
                foreach (var Trans in TransActionServiceBill)
                {
                    Trans.BillID = HeaderTOSave.VHI;
                    Trans.CompanyID = HeaderTOSave.CompanyID;
                    Trans.CompanyYear = HeaderTOSave.CompanyYear;
                    Trans.iRowID = iRow;
                    Trans.CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo;
                    Trans.TransactionKindNo = HeaderTOSave.TransactionKindNo;
                    Trans.FCurrencyID = HeaderTOSave.FCurrencyID;
                    iRow = iRow + 1;
                    _unitOfWork.TransActionServiceBill.Add(Trans);
                }
                HeaderServiceBill.BillID = HeaderTOSave.VHI;
                HeaderServiceBill.CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo;
                HeaderServiceBill.TransactionKindNo = HeaderTOSave.TransactionKindNo;
                _unitOfWork.ServiceBillHeader.Add(HeaderServiceBill);
                _unitOfWork.Transaction.DeleteMarkeTrans(HeaderTOSave.CompanyID, HeaderTOSave.VoucherNumber,HeaderTOSave.CompanyTransactionKindNo, HeaderTOSave.TransactionKindNo, HeaderTOSave.CompanyYear);

                _unitOfWork.Complete();
                Msg.Code = 1;
                Msg.Msg = Resources.Resource.AddedSuccessfully;
                Msg.Year = HeaderTOSave.CompanyYear;
                Msg.FCompanyId = HeaderTOSave.CompanyID;
                Msg.VoucherNumber = HeaderTOSave.VoucherNumber.ToString();
                Msg.TransactionKindNo = HeaderTOSave.TransactionKindNo.ToString();
                Msg.CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo.ToString();
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _unitOfWork.Transaction.UnMarkDeleteTrans(HeaderTOSave.CompanyID, HeaderTOSave.VoucherNumber, HeaderTOSave.CompanyTransactionKindNo, HeaderTOSave.TransactionKindNo, HeaderTOSave.CompanyYear);
                _unitOfWork.Complete();
                   Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                Msg.Code = 0;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult UpdateReturnServiceBill(TransServiceBillVM ObjSave)
        {
            MsgUnit Msg = new MsgUnit();
            var HeaderTOSave = new Header();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                int CurrYear = ObjSave.HeaderServiceBill.CompanyYear;
                var TransActionServiceBill = ObjSave.TransActionServiceBill;
                HeaderServiceBill HeaderServiceBill = new HeaderServiceBill();
                HeaderServiceBill.AccountNumber = ObjSave.HeaderServiceBill.AccountNumber;
                HeaderServiceBill.BillDate = ObjSave.HeaderServiceBill.BillDate;
                HeaderServiceBill.CompanyID = ObjSave.HeaderServiceBill.CompanyID;
                HeaderServiceBill.CompanyYear = ObjSave.HeaderServiceBill.CompanyYear;
                HeaderServiceBill.CreditAccountNumber = ObjSave.HeaderServiceBill.CreditAccountNumber;
                HeaderServiceBill.CreditCostNumber = ObjSave.HeaderServiceBill.CreditCostNumber;
                HeaderServiceBill.DebitAccountNumber = ObjSave.HeaderServiceBill.DebitAccountNumber;
                HeaderServiceBill.DebitCostNumber = ObjSave.HeaderServiceBill.DebitCostNumber;
                HeaderServiceBill.Discount = ObjSave.HeaderServiceBill.Discount;
                HeaderServiceBill.ConversionFactor = ObjSave.HeaderServiceBill.CurrencyNewValue;
                HeaderServiceBill.FCurrencyID = ObjSave.HeaderServiceBill.CurrencyID;
                HeaderServiceBill.NetTotal = ObjSave.HeaderServiceBill.NetTotal;
                HeaderServiceBill.NetTotalForeignAmount = ObjSave.HeaderServiceBill.NetTotalForeignAmount;
                HeaderServiceBill.ForeignAmount = ObjSave.HeaderServiceBill.ForeignAmount;
                HeaderServiceBill.ForeignAmountTax = ObjSave.HeaderServiceBill.ForeignAmountTax;
                HeaderServiceBill.NoTax = ObjSave.HeaderServiceBill.NoTax;
                HeaderServiceBill.Note = ObjSave.HeaderServiceBill.Note;
                HeaderServiceBill.TransactionKindNo = 19;
                HeaderServiceBill.CompanyTransactionKindNo = ObjSave.HeaderServiceBill.CompanyTransactionKindID;
                HeaderServiceBill.Tax = ObjSave.HeaderServiceBill.Tax;
                HeaderServiceBill.TaxAccountNumber = ObjSave.HeaderServiceBill.TaxAccountNumber;
                HeaderServiceBill.TaxCostNumber = ObjSave.HeaderServiceBill.TaxCostNumber;
                HeaderServiceBill.Total = ObjSave.HeaderServiceBill.Total;
                HeaderServiceBill.SaleManNo = ObjSave.HeaderServiceBill.SaleManNo;
                var TransactionKindNo = 19;
                var CompanyTransactionKindID = ObjSave.HeaderServiceBill.CompanyTransactionKindID;
                double LineTotals = 0;
                double Price = 0;
                double Qty = 0;
                double TaxPrec = 0;
                double Total = 0;
                double TotalTax = 0;
                double NetTotal = 0;
                double TotalDebit = 0;
                double TotalCredit = 0;
                double TotalDebitForeign = 0;
                double TotalCreditForeign = 0;
                int iRow = 1;
                var ExchangeRate = HeaderServiceBill.ConversionFactor;
                HeaderServiceBill.CompanyID = UserInfo.fCompanyId;
                HeaderServiceBill.CompanyYear = CurrYear;
                var AllAcc = _unitOfWork.NativeSql.GetTransActionAccount(UserInfo.fCompanyId);
                foreach (var Trans in TransActionServiceBill)
                {
                    Price = Trans.Price;
                    Qty = Trans.Qty;
                    TaxPrec = Trans.Tax / 100;
                    if (HeaderServiceBill.ConversionFactor == 0)
                    {
                        HeaderServiceBill.ConversionFactor = 1;
                    };
                    if (HeaderServiceBill.ConversionFactor == 1)
                    {
                        Trans.ForeignPrice = 0;
                    }
                    else
                    {
                        Trans.ForeignPrice = Trans.Price / ExchangeRate;
                    }

                    Trans.PriceAfterLineDisc = Trans.Price - Trans.LocalDiscount;
                    Trans.ConversionFactor = ExchangeRate;
                    if (HeaderServiceBill.NoTax)
                    {
                        Trans.TaxValue = 0;

                    }
                    else
                    {
                        Trans.TaxValue = Trans.PriceAfterLineDisc * TaxPrec;
                    }
                    Trans.TotalLocal = Trans.PriceAfterLineDisc * Trans.Qty;
                    if (Trans.ForeignPrice > 0)
                    {
                        Trans.ForeignPriceAfterLineDisc = (Trans.Price / ExchangeRate) - Trans.LocalDiscount;
                        Trans.TotalForeign = Trans.ForeignPriceAfterLineDisc * Trans.Qty;
                    }
                    LineTotals = LineTotals + Trans.TotalLocal;
                }
                var TotalDiscPrec = HeaderServiceBill.Discount / LineTotals;
                foreach (var Trans in TransActionServiceBill)
                {
                    TaxPrec = Trans.Tax / 100;
                    Trans.TotalDiscPrec = TotalDiscPrec;
                    Trans.TotalDiscValue = TotalDiscPrec * Trans.TotalLocal;
                    Trans.NetAfterTotalDisc = Trans.TotalLocal - Trans.TotalDiscValue;
                    if (HeaderServiceBill.NoTax)
                    {
                        Trans.NetTotalTax = 0;
                    }
                    else
                    {
                        Trans.NetTotalTax = TaxPrec * Trans.NetAfterTotalDisc;
                    }
                    Total = Total + Trans.NetAfterTotalDisc;
                    TotalTax = TotalTax + Trans.NetTotalTax;
                }
                NetTotal = Total + TotalTax;
                TotalDebit = Total + TotalTax;
                TotalCredit = NetTotal;
                HeaderServiceBill.NetTotal = NetTotal;
                if (HeaderServiceBill.ConversionFactor == 0)
                {
                    HeaderServiceBill.ConversionFactor = 1;
                };
                if (HeaderServiceBill.ConversionFactor == 1)
                {
                    TotalDebitForeign = 0;
                    TotalCreditForeign = 0;
                }
                else
                {
                    TotalDebitForeign = Total / ExchangeRate;
                    TotalCreditForeign = (HeaderServiceBill.NetTotal) / ExchangeRate;
                }

                if (TotalCredit != TotalDebit)
                {
                    Msg.Msg = Resources.Resource.TheAccountingEntryIsUneven;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }
                if (TotalCredit <= 0)
                {
                    Msg.Msg = Resources.Resource.CantBeZero;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }
                
                HeaderTOSave.TransactionKindNo = TransactionKindNo;
                HeaderTOSave.CompanyTransactionKindNo = CompanyTransactionKindID;
                HeaderTOSave.InsDateTime = DateTime.Now;
                HeaderTOSave.InsUserID = userId;
                HeaderTOSave.CompanyID = UserInfo.fCompanyId;
                HeaderTOSave.TotalCredit = TotalCredit;
                HeaderTOSave.TotalDebit = TotalDebit;
                HeaderTOSave.TotalDebitForeign = TotalDebitForeign;
                HeaderTOSave.TotalCreditForeign = TotalDebitForeign;
                HeaderTOSave.CompanyYear = CurrYear;
                HeaderTOSave.VoucherNumber = ObjSave.HeaderServiceBill.BillID.ToString();
                HeaderTOSave.VHI = int.Parse(HeaderTOSave.VoucherNumber);
                HeaderTOSave.VoucherDate = HeaderServiceBill.BillDate;
                HeaderTOSave.FCurrencyID = HeaderServiceBill.FCurrencyID;
                HeaderTOSave.ConversionFactor = HeaderServiceBill.ConversionFactor;
                if (HeaderServiceBill.NoTax)
                {
                    HeaderTOSave.RowCount = 2;
                }
                else
                {
                    HeaderTOSave.RowCount = 3;
                }
                HeaderTOSave.SaleID = HeaderServiceBill.SaleManNo;
                _unitOfWork.ServiceBillHeader.Delete(HeaderTOSave.CompanyID, HeaderTOSave.VHI, HeaderServiceBill.CompanyTransactionKindNo, HeaderServiceBill.TransactionKindNo, HeaderTOSave.CompanyYear);
                _unitOfWork.TransActionServiceBill.Delete(HeaderTOSave.CompanyID, HeaderTOSave.VHI, HeaderServiceBill.CompanyTransactionKindNo, HeaderServiceBill.TransactionKindNo, HeaderTOSave.CompanyYear);
                _unitOfWork.Transaction.MarkDeleteTrans(HeaderTOSave.CompanyID, HeaderTOSave.VoucherNumber, HeaderTOSave.CompanyTransactionKindNo, HeaderTOSave.TransactionKindNo, HeaderTOSave.CompanyYear);
                _unitOfWork.Complete();
                _unitOfWork.Header.Update(HeaderTOSave);
                iRow = 0;
                Transaction ObjCredit = new Transaction();
                ObjCredit.AccountNumber = HeaderServiceBill.CreditAccountNumber;
                ObjCredit.CostCenter = HeaderServiceBill.CreditCostNumber;
                ObjCredit.CompanyTransactionKindNo = CompanyTransactionKindID;
                ObjCredit.TransactionKindNo = TransactionKindNo;
                ObjCredit.CompanyYear = HeaderTOSave.CompanyYear;
                ObjCredit.CompanyID = HeaderTOSave.CompanyID;
                ObjCredit.Debit = 0;
                ObjCredit.DebitForeign = 0;
                ObjCredit.Credit = TotalCredit;
                if (HeaderServiceBill.ConversionFactor == 0)
                {
                    HeaderServiceBill.ConversionFactor = 1;
                };
                if (HeaderServiceBill.ConversionFactor == 1)
                {
                    ObjCredit.CreditForeign = 0;
                    ObjCredit.CreditDebitForeign = 0;
                }
                else
                {
                    ObjCredit.CreditForeign = TotalCredit / ExchangeRate;
                    ObjCredit.CreditDebitForeign = TotalCredit / ExchangeRate;
                }
                ObjCredit.Exported = 0;
                ObjCredit.InsDateTime = DateTime.Now;
                ObjCredit.RowNumber = 1;
                ObjCredit.VHI = HeaderTOSave.VHI;
                ObjCredit.VoucherDate = HeaderTOSave.VoucherDate;
                ObjCredit.VoucherNumber = HeaderTOSave.VoucherNumber;

                Transaction ObjDebit = new Transaction();
                ObjDebit.AccountNumber = HeaderServiceBill.DebitAccountNumber;
                ObjDebit.CostCenter = HeaderServiceBill.DebitCostNumber;
                ObjDebit.CompanyTransactionKindNo = CompanyTransactionKindID;
                ObjDebit.TransactionKindNo = TransactionKindNo;
                ObjDebit.CompanyYear = HeaderTOSave.CompanyYear;
                ObjDebit.CompanyID = HeaderTOSave.CompanyID;
                ObjDebit.Debit = Total;
                if (HeaderServiceBill.ConversionFactor == 0)
                {
                    HeaderServiceBill.ConversionFactor = 1;
                };
                if (HeaderServiceBill.ConversionFactor == 1)
                {
                    ObjDebit.DebitForeign = 0;
                    ObjDebit.CreditDebitForeign = 0;
                }
                else
                {
                    ObjDebit.CreditDebitForeign = Total / ExchangeRate;
                    ObjDebit.DebitForeign = Total / ExchangeRate;
                }
                ObjDebit.Credit = 0;
                ObjDebit.CreditForeign = 0;
                ObjDebit.Exported = 0;
                ObjDebit.InsDateTime = DateTime.Now;
                ObjDebit.RowNumber = 2;
                ObjDebit.VHI = HeaderTOSave.VHI;
                ObjDebit.VoucherDate = HeaderTOSave.VoucherDate;
                ObjDebit.VoucherNumber = HeaderTOSave.VoucherNumber;

                _unitOfWork.Transaction.Add(ObjDebit);
                _unitOfWork.Transaction.Add(ObjCredit);

                if (!HeaderServiceBill.NoTax)
                {
                    Transaction ObjTax = new Transaction();
                    ObjTax.AccountNumber = HeaderServiceBill.TaxAccountNumber;
                    ObjTax.CostCenter = HeaderServiceBill.TaxCostNumber;
                    ObjTax.CompanyTransactionKindNo = CompanyTransactionKindID;
                    ObjTax.TransactionKindNo = TransactionKindNo;
                    ObjTax.CompanyYear = HeaderTOSave.CompanyYear;
                    ObjTax.CompanyID = HeaderTOSave.CompanyID;
                    ObjTax.Debit = TotalTax;
                    if (HeaderServiceBill.ConversionFactor == 0)
                    {
                        HeaderServiceBill.ConversionFactor = 1;
                    };
                    if (HeaderServiceBill.ConversionFactor == 1)
                    {
                        ObjTax.DebitForeign = 0;
                        ObjTax.CreditDebitForeign = 0;
                    }
                    else
                    {
                        ObjTax.CreditDebitForeign = TotalTax / ExchangeRate;
                        ObjTax.DebitForeign = TotalTax / ExchangeRate;
                    }
                    ObjTax.Credit = 0;
                    ObjTax.CreditForeign = 0;
                    ObjTax.Exported = 0;
                    ObjTax.InsDateTime = DateTime.Now;
                    ObjTax.RowNumber = 3;
                    ObjTax.VHI = HeaderTOSave.VHI;
                    ObjTax.VoucherDate = HeaderTOSave.VoucherDate;
                    ObjTax.VoucherNumber = HeaderTOSave.VoucherNumber;
                    _unitOfWork.Transaction.Add(ObjTax);
                }

                iRow = 1;
                foreach (var Trans in TransActionServiceBill)
                {
                    Trans.BillID = HeaderTOSave.VHI;
                    Trans.CompanyID = HeaderTOSave.CompanyID;
                    Trans.CompanyYear = HeaderTOSave.CompanyYear;
                    Trans.iRowID = iRow;
                    Trans.CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo;
                    Trans.TransactionKindNo = HeaderTOSave.TransactionKindNo;
                    Trans.FCurrencyID = HeaderTOSave.FCurrencyID;
                    iRow = iRow + 1;
                    _unitOfWork.TransActionServiceBill.Add(Trans);
                }
                HeaderServiceBill.BillID = HeaderTOSave.VHI;
                HeaderServiceBill.CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo;
                HeaderServiceBill.TransactionKindNo = HeaderTOSave.TransactionKindNo;
                _unitOfWork.ServiceBillHeader.Add(HeaderServiceBill);
                _unitOfWork.Transaction.DeleteMarkeTrans(HeaderTOSave.CompanyID, HeaderTOSave.VoucherNumber, HeaderTOSave.CompanyTransactionKindNo, HeaderTOSave.TransactionKindNo, HeaderTOSave.CompanyYear);
                _unitOfWork.Complete();
                Msg.Code = 1;
                Msg.Msg = Resources.Resource.AddedSuccessfully;
                Msg.Year = HeaderTOSave.CompanyYear;
                Msg.FCompanyId = HeaderTOSave.CompanyID;
                Msg.VoucherNumber = HeaderTOSave.VoucherNumber.ToString();
                Msg.TransactionKindNo = HeaderTOSave.TransactionKindNo.ToString();
                Msg.CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo.ToString();
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _unitOfWork.Transaction.UnMarkDeleteTrans(HeaderTOSave.CompanyID, HeaderTOSave.VoucherNumber, HeaderTOSave.CompanyTransactionKindNo, HeaderTOSave.TransactionKindNo, HeaderTOSave.CompanyYear);
                _unitOfWork.Complete();
                Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                Msg.Code = 0;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult UpdatePurchaseServiceBill(TransServiceBillVM ObjSave)
        {
            MsgUnit Msg = new MsgUnit();
            var HeaderTOSave = new Header();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                int CurrYear = ObjSave.HeaderServiceBill.CompanyYear;
                var TransActionServiceBill = ObjSave.TransActionServiceBill;
                HeaderServiceBill HeaderServiceBill = new HeaderServiceBill();
                HeaderServiceBill.AccountNumber = ObjSave.HeaderServiceBill.AccountNumber;
                HeaderServiceBill.BillDate = ObjSave.HeaderServiceBill.BillDate;
                HeaderServiceBill.CompanyID = ObjSave.HeaderServiceBill.CompanyID;
                HeaderServiceBill.CompanyYear = ObjSave.HeaderServiceBill.CompanyYear;
                HeaderServiceBill.CreditAccountNumber = ObjSave.HeaderServiceBill.CreditAccountNumber;
                HeaderServiceBill.CreditCostNumber = ObjSave.HeaderServiceBill.CreditCostNumber;
                HeaderServiceBill.DebitAccountNumber = ObjSave.HeaderServiceBill.DebitAccountNumber;
                HeaderServiceBill.DebitCostNumber = ObjSave.HeaderServiceBill.DebitCostNumber;
                HeaderServiceBill.Discount = ObjSave.HeaderServiceBill.Discount;
                HeaderServiceBill.ConversionFactor = ObjSave.HeaderServiceBill.CurrencyNewValue;
                HeaderServiceBill.FCurrencyID = ObjSave.HeaderServiceBill.CurrencyID;
                HeaderServiceBill.NetTotal = ObjSave.HeaderServiceBill.NetTotal;
                HeaderServiceBill.NetTotalForeignAmount = ObjSave.HeaderServiceBill.NetTotalForeignAmount;
                HeaderServiceBill.ForeignAmount = ObjSave.HeaderServiceBill.ForeignAmount;
                HeaderServiceBill.ForeignAmountTax = ObjSave.HeaderServiceBill.ForeignAmountTax;
                HeaderServiceBill.NoTax = ObjSave.HeaderServiceBill.NoTax;
                HeaderServiceBill.Note = ObjSave.HeaderServiceBill.Note;
                HeaderServiceBill.TransactionKindNo = 12;
                HeaderServiceBill.CompanyTransactionKindNo = ObjSave.HeaderServiceBill.CompanyTransactionKindID;
                HeaderServiceBill.Tax = ObjSave.HeaderServiceBill.Tax;
                HeaderServiceBill.TaxAccountNumber = ObjSave.HeaderServiceBill.TaxAccountNumber;
                HeaderServiceBill.TaxCostNumber = ObjSave.HeaderServiceBill.TaxCostNumber;
                HeaderServiceBill.Total = ObjSave.HeaderServiceBill.Total;
                HeaderServiceBill.SaleManNo = 0;
                var TransactionKindNo = 12;
                var CompanyTransactionKindID = ObjSave.HeaderServiceBill.CompanyTransactionKindID;
                double LineTotals = 0;
                double Price = 0;
                double Qty = 0;
                double TaxPrec = 0;
                double Total = 0;
                double TotalTax = 0;
                double NetTotal = 0;
                double TotalDebit = 0;
                double TotalCredit = 0;
                double TotalDebitForeign = 0;
                double TotalCreditForeign = 0;
                int iRow = 1;
                var ExchangeRate = HeaderServiceBill.ConversionFactor;
                HeaderServiceBill.CompanyID = UserInfo.fCompanyId;
                HeaderServiceBill.CompanyYear = CurrYear;
                var AllAcc = _unitOfWork.NativeSql.GetTransActionAccount(UserInfo.fCompanyId);
                foreach (var Trans in TransActionServiceBill)
                {
                    Price = Trans.Price;
                    Qty = Trans.Qty;
                    TaxPrec = Trans.Tax / 100;
                    if (HeaderServiceBill.ConversionFactor == 0)
                    {
                        HeaderServiceBill.ConversionFactor = 1;
                    };
                    if (HeaderServiceBill.ConversionFactor == 1)
                    {
                        Trans.ForeignPrice = 0;
                    }
                    else
                    {
                        Trans.ForeignPrice = Trans.Price / ExchangeRate;
                    }

                    Trans.PriceAfterLineDisc = Trans.Price - Trans.LocalDiscount;
                    Trans.ConversionFactor = ExchangeRate;
                    if (HeaderServiceBill.NoTax)
                    {
                        Trans.TaxValue = 0;

                    }
                    else
                    {
                        Trans.TaxValue = Trans.PriceAfterLineDisc * TaxPrec;
                    }
                    Trans.TotalLocal = Trans.PriceAfterLineDisc * Trans.Qty;
                    if (Trans.ForeignPrice > 0)
                    {
                        Trans.ForeignPriceAfterLineDisc = (Trans.Price / ExchangeRate) - Trans.LocalDiscount;
                        Trans.TotalForeign = Trans.ForeignPriceAfterLineDisc * Trans.Qty;
                    }
                    LineTotals = LineTotals + Trans.TotalLocal;
                }
                var TotalDiscPrec = HeaderServiceBill.Discount / LineTotals;
                foreach (var Trans in TransActionServiceBill)
                {
                    TaxPrec = Trans.Tax / 100;
                    Trans.TotalDiscPrec = TotalDiscPrec;
                    Trans.TotalDiscValue = TotalDiscPrec * Trans.TotalLocal;
                    Trans.NetAfterTotalDisc = Trans.TotalLocal - Trans.TotalDiscValue;
                    if (HeaderServiceBill.NoTax)
                    {
                        Trans.NetTotalTax = 0;
                    }
                    else
                    {
                        Trans.NetTotalTax = TaxPrec * Trans.NetAfterTotalDisc;
                    }
                    Total = Total + Trans.NetAfterTotalDisc;
                    TotalTax = TotalTax + Trans.NetTotalTax;
                }
                NetTotal = Total + TotalTax;
                TotalDebit = Total + TotalTax;
                TotalCredit = NetTotal;
                HeaderServiceBill.NetTotal = NetTotal;
                if (HeaderServiceBill.ConversionFactor == 0)
                {
                    HeaderServiceBill.ConversionFactor = 1;
                };
                if (HeaderServiceBill.ConversionFactor == 1)
                {
                    TotalDebitForeign = 0;
                    TotalCreditForeign = 0;
                }
                else
                {
                    TotalDebitForeign = Total / ExchangeRate;
                    TotalCreditForeign = (HeaderServiceBill.NetTotal) / ExchangeRate;
                }

                if (TotalCredit != TotalDebit)
                {
                    Msg.Msg = Resources.Resource.TheAccountingEntryIsUneven;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }
                if (TotalCredit <= 0)
                {
                    Msg.Msg = Resources.Resource.CantBeZero;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }

                HeaderTOSave.TransactionKindNo = TransactionKindNo;
                HeaderTOSave.CompanyTransactionKindNo = CompanyTransactionKindID;
                HeaderTOSave.InsDateTime = DateTime.Now;
                HeaderTOSave.InsUserID = userId;
                HeaderTOSave.CompanyID = UserInfo.fCompanyId;
                HeaderTOSave.TotalCredit = TotalCredit;
                HeaderTOSave.TotalDebit = TotalDebit;
                HeaderTOSave.TotalDebitForeign = TotalDebitForeign;
                HeaderTOSave.TotalCreditForeign = TotalDebitForeign;
                HeaderTOSave.CompanyYear = CurrYear;
                HeaderTOSave.VoucherNumber = ObjSave.HeaderServiceBill.BillID.ToString();
                HeaderTOSave.VHI = int.Parse(HeaderTOSave.VoucherNumber);
                HeaderTOSave.VoucherDate = HeaderServiceBill.BillDate;
                HeaderTOSave.FCurrencyID = HeaderServiceBill.FCurrencyID;
                HeaderTOSave.ConversionFactor = HeaderServiceBill.ConversionFactor;
                if (HeaderServiceBill.NoTax)
                {
                    HeaderTOSave.RowCount = 2;
                }
                else
                {
                    HeaderTOSave.RowCount = 3;
                }
                HeaderTOSave.SaleID = 0;
                _unitOfWork.ServiceBillHeader.Delete(HeaderTOSave.CompanyID, HeaderTOSave.VHI, HeaderServiceBill.CompanyTransactionKindNo, HeaderServiceBill.TransactionKindNo, HeaderTOSave.CompanyYear);
                _unitOfWork.TransActionServiceBill.Delete(HeaderTOSave.CompanyID, HeaderTOSave.VHI, HeaderServiceBill.CompanyTransactionKindNo, HeaderServiceBill.TransactionKindNo, HeaderTOSave.CompanyYear);
                _unitOfWork.Transaction.MarkDeleteTrans(HeaderTOSave.CompanyID, HeaderTOSave.VoucherNumber, HeaderTOSave.CompanyTransactionKindNo, HeaderTOSave.TransactionKindNo, HeaderTOSave.CompanyYear);
                _unitOfWork.Complete();
                _unitOfWork.Header.Update(HeaderTOSave);
                iRow = 0;
                Transaction ObjCredit = new Transaction();
                ObjCredit.AccountNumber = HeaderServiceBill.CreditAccountNumber;
                ObjCredit.CostCenter = HeaderServiceBill.CreditCostNumber;
                ObjCredit.CompanyTransactionKindNo = CompanyTransactionKindID;
                ObjCredit.TransactionKindNo = TransactionKindNo;
                ObjCredit.CompanyYear = HeaderTOSave.CompanyYear;
                ObjCredit.CompanyID = HeaderTOSave.CompanyID;
                ObjCredit.Debit = 0;
                ObjCredit.DebitForeign = 0;
                ObjCredit.Credit = TotalCredit;
                if (HeaderServiceBill.ConversionFactor == 0)
                {
                    HeaderServiceBill.ConversionFactor = 1;
                };
                if (HeaderServiceBill.ConversionFactor == 1)
                {
                    ObjCredit.CreditForeign = 0;
                    ObjCredit.CreditDebitForeign = 0;
                }
                else
                {
                    ObjCredit.CreditForeign = TotalCredit / ExchangeRate;
                    ObjCredit.CreditDebitForeign = TotalCredit / ExchangeRate;
                }
                ObjCredit.Exported = 0;
                ObjCredit.InsDateTime = DateTime.Now;
                ObjCredit.RowNumber = 1;
                ObjCredit.VHI = HeaderTOSave.VHI;
                ObjCredit.VoucherDate = HeaderTOSave.VoucherDate;
                ObjCredit.VoucherNumber = HeaderTOSave.VoucherNumber;

                Transaction ObjDebit = new Transaction();
                ObjDebit.AccountNumber = HeaderServiceBill.DebitAccountNumber;
                ObjDebit.CostCenter = HeaderServiceBill.DebitCostNumber;
                ObjDebit.CompanyTransactionKindNo = CompanyTransactionKindID;
                ObjDebit.TransactionKindNo = TransactionKindNo;
                ObjDebit.CompanyYear = HeaderTOSave.CompanyYear;
                ObjDebit.CompanyID = HeaderTOSave.CompanyID;
                ObjDebit.Debit = Total;
                if (HeaderServiceBill.ConversionFactor == 0)
                {
                    HeaderServiceBill.ConversionFactor = 1;
                };
                if (HeaderServiceBill.ConversionFactor == 1)
                {
                    ObjDebit.DebitForeign = 0;
                    ObjDebit.CreditDebitForeign = 0;
                }
                else
                {
                    ObjDebit.CreditDebitForeign = Total / ExchangeRate;
                    ObjDebit.DebitForeign = Total / ExchangeRate;
                }
                ObjDebit.Credit = 0;
                ObjDebit.CreditForeign = 0;
                ObjDebit.Exported = 0;
                ObjDebit.InsDateTime = DateTime.Now;
                ObjDebit.RowNumber = 2;
                ObjDebit.VHI = HeaderTOSave.VHI;
                ObjDebit.VoucherDate = HeaderTOSave.VoucherDate;
                ObjDebit.VoucherNumber = HeaderTOSave.VoucherNumber;

                _unitOfWork.Transaction.Add(ObjDebit);
                _unitOfWork.Transaction.Add(ObjCredit);

                if (!HeaderServiceBill.NoTax)
                {
                    Transaction ObjTax = new Transaction();
                    ObjTax.AccountNumber = HeaderServiceBill.TaxAccountNumber;
                    ObjTax.CostCenter = HeaderServiceBill.TaxCostNumber;
                    ObjTax.CompanyTransactionKindNo = CompanyTransactionKindID;
                    ObjTax.TransactionKindNo = TransactionKindNo;
                    ObjTax.CompanyYear = HeaderTOSave.CompanyYear;
                    ObjTax.CompanyID = HeaderTOSave.CompanyID;
                    ObjTax.Debit = TotalTax;
                    if (HeaderServiceBill.ConversionFactor == 0)
                    {
                        HeaderServiceBill.ConversionFactor = 1;
                    };
                    if (HeaderServiceBill.ConversionFactor == 1)
                    {
                        ObjTax.DebitForeign = 0;
                        ObjTax.CreditDebitForeign = 0;
                    }
                    else
                    {
                        ObjTax.CreditDebitForeign = TotalTax / ExchangeRate;
                        ObjTax.DebitForeign = TotalTax / ExchangeRate;
                    }
                    ObjTax.Credit = 0;
                    ObjTax.CreditForeign = 0;
                    ObjTax.Exported = 0;
                    ObjTax.InsDateTime = DateTime.Now;
                    ObjTax.RowNumber = 3;
                    ObjTax.VHI = HeaderTOSave.VHI;
                    ObjTax.VoucherDate = HeaderTOSave.VoucherDate;
                    ObjTax.VoucherNumber = HeaderTOSave.VoucherNumber;
                    _unitOfWork.Transaction.Add(ObjTax);
                }

                iRow = 1;
                foreach (var Trans in TransActionServiceBill)
                {
                    Trans.BillID = HeaderTOSave.VHI;
                    Trans.CompanyID = HeaderTOSave.CompanyID;
                    Trans.CompanyYear = HeaderTOSave.CompanyYear;
                    Trans.iRowID = iRow;
                    Trans.CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo;
                    Trans.TransactionKindNo = HeaderTOSave.TransactionKindNo;
                    Trans.FCurrencyID = HeaderTOSave.FCurrencyID;
                    iRow = iRow + 1;
                    _unitOfWork.TransActionServiceBill.Add(Trans);
                }
                HeaderServiceBill.BillID = HeaderTOSave.VHI;
                HeaderServiceBill.CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo;
                HeaderServiceBill.TransactionKindNo = HeaderTOSave.TransactionKindNo;
                _unitOfWork.ServiceBillHeader.Add(HeaderServiceBill);
                _unitOfWork.Transaction.DeleteMarkeTrans(HeaderTOSave.CompanyID, HeaderTOSave.VoucherNumber, HeaderTOSave.CompanyTransactionKindNo, HeaderTOSave.TransactionKindNo, HeaderTOSave.CompanyYear);
                _unitOfWork.Complete();
                Msg.Code = 1;
                Msg.Msg = Resources.Resource.AddedSuccessfully;
                Msg.Year = HeaderTOSave.CompanyYear;
                Msg.FCompanyId = HeaderTOSave.CompanyID;
                Msg.VoucherNumber = HeaderTOSave.VoucherNumber.ToString();
                Msg.TransactionKindNo = HeaderTOSave.TransactionKindNo.ToString();
                Msg.CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo.ToString();
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _unitOfWork.Transaction.UnMarkDeleteTrans(HeaderTOSave.CompanyID, HeaderTOSave.VoucherNumber, HeaderTOSave.CompanyTransactionKindNo, HeaderTOSave.TransactionKindNo, HeaderTOSave.CompanyYear);
                _unitOfWork.Complete();
                Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                Msg.Code = 0;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult UpdateReturnPurchaseServiceBill(TransServiceBillVM ObjSave)
        {
            MsgUnit Msg = new MsgUnit();
            var HeaderTOSave = new Header();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                int CurrYear = ObjSave.HeaderServiceBill.CompanyYear;
                var TransActionServiceBill = ObjSave.TransActionServiceBill;
                HeaderServiceBill HeaderServiceBill = new HeaderServiceBill();
                HeaderServiceBill.AccountNumber = ObjSave.HeaderServiceBill.AccountNumber;
                HeaderServiceBill.BillDate = ObjSave.HeaderServiceBill.BillDate;
                HeaderServiceBill.CompanyID = ObjSave.HeaderServiceBill.CompanyID;
                HeaderServiceBill.CompanyYear = ObjSave.HeaderServiceBill.CompanyYear;
                HeaderServiceBill.CreditAccountNumber = ObjSave.HeaderServiceBill.CreditAccountNumber;
                HeaderServiceBill.CreditCostNumber = ObjSave.HeaderServiceBill.CreditCostNumber;
                HeaderServiceBill.DebitAccountNumber = ObjSave.HeaderServiceBill.DebitAccountNumber;
                HeaderServiceBill.DebitCostNumber = ObjSave.HeaderServiceBill.DebitCostNumber;
                HeaderServiceBill.Discount = ObjSave.HeaderServiceBill.Discount;
                HeaderServiceBill.ConversionFactor = ObjSave.HeaderServiceBill.CurrencyNewValue;
                HeaderServiceBill.FCurrencyID = ObjSave.HeaderServiceBill.CurrencyID;
                HeaderServiceBill.NetTotal = ObjSave.HeaderServiceBill.NetTotal;
                HeaderServiceBill.NetTotalForeignAmount = ObjSave.HeaderServiceBill.NetTotalForeignAmount;
                HeaderServiceBill.ForeignAmount = ObjSave.HeaderServiceBill.ForeignAmount;
                HeaderServiceBill.ForeignAmountTax = ObjSave.HeaderServiceBill.ForeignAmountTax;
                HeaderServiceBill.NoTax = ObjSave.HeaderServiceBill.NoTax;
                HeaderServiceBill.Note = ObjSave.HeaderServiceBill.Note;
                HeaderServiceBill.TransactionKindNo = 21;
                HeaderServiceBill.CompanyTransactionKindNo = ObjSave.HeaderServiceBill.CompanyTransactionKindID;
                HeaderServiceBill.Tax = ObjSave.HeaderServiceBill.Tax;
                HeaderServiceBill.TaxAccountNumber = ObjSave.HeaderServiceBill.TaxAccountNumber;
                HeaderServiceBill.TaxCostNumber = ObjSave.HeaderServiceBill.TaxCostNumber;
                HeaderServiceBill.Total = ObjSave.HeaderServiceBill.Total;
                HeaderServiceBill.SaleManNo = 0;
                var TransactionKindNo = 21;
                var CompanyTransactionKindID = ObjSave.HeaderServiceBill.CompanyTransactionKindID;
                double LineTotals = 0;
                double Price = 0;
                double Qty = 0;
                double TaxPrec = 0;
                double Total = 0;
                double TotalTax = 0;
                double NetTotal = 0;
                double TotalDebit = 0;
                double TotalCredit = 0;
                double TotalDebitForeign = 0;
                double TotalCreditForeign = 0;
                int iRow = 1;
                var ExchangeRate = HeaderServiceBill.ConversionFactor;
                HeaderServiceBill.CompanyID = UserInfo.fCompanyId;
                HeaderServiceBill.CompanyYear = CurrYear;
                var AllAcc = _unitOfWork.NativeSql.GetTransActionAccount(UserInfo.fCompanyId);
                foreach (var Trans in TransActionServiceBill)
                {
                    Price = Trans.Price;
                    Qty = Trans.Qty;
                    TaxPrec = Trans.Tax / 100;
                    if (HeaderServiceBill.ConversionFactor == 0)
                    {
                        HeaderServiceBill.ConversionFactor = 1;
                    };
                    if (HeaderServiceBill.ConversionFactor == 1)
                    {
                        Trans.ForeignPrice = 0;
                    }
                    else
                    {
                        Trans.ForeignPrice = Trans.Price / ExchangeRate;
                    }

                    Trans.PriceAfterLineDisc = Trans.Price - Trans.LocalDiscount;
                    Trans.ConversionFactor = ExchangeRate;
                    if (HeaderServiceBill.NoTax)
                    {
                        Trans.TaxValue = 0;

                    }
                    else
                    {
                        Trans.TaxValue = Trans.PriceAfterLineDisc * TaxPrec;
                    }
                    Trans.TotalLocal = Trans.PriceAfterLineDisc * Trans.Qty;
                    if (Trans.ForeignPrice > 0)
                    {
                        Trans.ForeignPriceAfterLineDisc = (Trans.Price / ExchangeRate) - Trans.LocalDiscount;
                        Trans.TotalForeign = Trans.ForeignPriceAfterLineDisc * Trans.Qty;
                    }
                    LineTotals = LineTotals + Trans.TotalLocal;
                }
                var TotalDiscPrec = HeaderServiceBill.Discount / LineTotals;
                foreach (var Trans in TransActionServiceBill)
                {
                    TaxPrec = Trans.Tax / 100;
                    Trans.TotalDiscPrec = TotalDiscPrec;
                    Trans.TotalDiscValue = TotalDiscPrec * Trans.TotalLocal;
                    Trans.NetAfterTotalDisc = Trans.TotalLocal - Trans.TotalDiscValue;
                    if (HeaderServiceBill.NoTax)
                    {
                        Trans.NetTotalTax = 0;
                    }
                    else
                    {
                        Trans.NetTotalTax = TaxPrec * Trans.NetAfterTotalDisc;
                    }
                    Total = Total + Trans.NetAfterTotalDisc;
                    TotalTax = TotalTax + Trans.NetTotalTax;
                }
                NetTotal = Total + TotalTax;
                TotalDebit = NetTotal;
                TotalCredit = Total + TotalTax;
                HeaderServiceBill.NetTotal = NetTotal;
                if (HeaderServiceBill.ConversionFactor == 0)
                {
                    HeaderServiceBill.ConversionFactor = 1;
                };
                if (HeaderServiceBill.ConversionFactor == 1)
                {
                    TotalDebitForeign = 0;
                    TotalCreditForeign = 0;
                }
                else
                {
                    TotalDebitForeign = Total / ExchangeRate;
                    TotalCreditForeign = (HeaderServiceBill.NetTotal) / ExchangeRate;
                }

                if (TotalCredit != TotalDebit)
                {
                    Msg.Msg = Resources.Resource.TheAccountingEntryIsUneven;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }
                if (TotalCredit <= 0)
                {
                    Msg.Msg = Resources.Resource.CantBeZero;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }


                HeaderTOSave.TransactionKindNo = TransactionKindNo;
                HeaderTOSave.CompanyTransactionKindNo = CompanyTransactionKindID;
                HeaderTOSave.InsDateTime = DateTime.Now;
                HeaderTOSave.InsUserID = userId;
                HeaderTOSave.CompanyID = UserInfo.fCompanyId;
                HeaderTOSave.TotalCredit = TotalCredit;
                HeaderTOSave.TotalDebit = TotalDebit;
                HeaderTOSave.TotalDebitForeign = TotalCreditForeign;
                HeaderTOSave.TotalCreditForeign = TotalCreditForeign;
                HeaderTOSave.CompanyYear = CurrYear;
                HeaderTOSave.VoucherNumber = ObjSave.HeaderServiceBill.BillID.ToString();
                HeaderTOSave.VHI = int.Parse(HeaderTOSave.VoucherNumber);
                HeaderTOSave.VoucherDate = HeaderServiceBill.BillDate;
                HeaderTOSave.FCurrencyID = HeaderServiceBill.FCurrencyID;
                HeaderTOSave.ConversionFactor = HeaderServiceBill.ConversionFactor;
                if (HeaderServiceBill.NoTax)
                {
                    HeaderTOSave.RowCount = 2;
                }
                else
                {
                    HeaderTOSave.RowCount = 3;
                }
                HeaderTOSave.SaleID = 0;
                _unitOfWork.ServiceBillHeader.Delete(HeaderTOSave.CompanyID, HeaderTOSave.VHI, HeaderServiceBill.CompanyTransactionKindNo, HeaderServiceBill.TransactionKindNo, HeaderTOSave.CompanyYear);
                _unitOfWork.TransActionServiceBill.Delete(HeaderTOSave.CompanyID, HeaderTOSave.VHI, HeaderServiceBill.CompanyTransactionKindNo, HeaderServiceBill.TransactionKindNo, HeaderTOSave.CompanyYear);
                _unitOfWork.Transaction.MarkDeleteTrans(HeaderTOSave.CompanyID, HeaderTOSave.VoucherNumber, HeaderTOSave.CompanyTransactionKindNo, HeaderTOSave.TransactionKindNo, HeaderTOSave.CompanyYear);
                _unitOfWork.Complete();
                _unitOfWork.Header.Update(HeaderTOSave);


                iRow = 0;
                Transaction ObjDebit = new Transaction();
                ObjDebit.AccountNumber = HeaderServiceBill.DebitAccountNumber;
                ObjDebit.CostCenter = HeaderServiceBill.DebitCostNumber;
                ObjDebit.CompanyTransactionKindNo = CompanyTransactionKindID;
                ObjDebit.TransactionKindNo = TransactionKindNo;
                ObjDebit.CompanyYear = HeaderTOSave.CompanyYear;
                ObjDebit.CompanyID = HeaderTOSave.CompanyID;
                ObjDebit.Credit = 0;
                ObjDebit.CreditForeign = 0;
                ObjDebit.Debit = TotalDebit;
                if (HeaderServiceBill.ConversionFactor == 0)
                {
                    HeaderServiceBill.ConversionFactor = 1;
                };
                if (HeaderServiceBill.ConversionFactor == 1)
                {
                    ObjDebit.DebitForeign = 0;
                    ObjDebit.CreditDebitForeign = 0;
                }
                else
                {
                    ObjDebit.DebitForeign = TotalDebit / ExchangeRate;
                    ObjDebit.CreditDebitForeign = TotalDebit / ExchangeRate;
                }
                ObjDebit.Exported = 0;
                ObjDebit.InsDateTime = DateTime.Now;
                ObjDebit.RowNumber = 1;
                ObjDebit.VHI = HeaderTOSave.VHI;
                ObjDebit.VoucherDate = HeaderTOSave.VoucherDate;
                ObjDebit.VoucherNumber = HeaderTOSave.VoucherNumber;

                Transaction ObjCredit = new Transaction();
                ObjCredit.AccountNumber = HeaderServiceBill.CreditAccountNumber;
                ObjCredit.CostCenter = HeaderServiceBill.CreditCostNumber;
                ObjCredit.CompanyTransactionKindNo = CompanyTransactionKindID;
                ObjCredit.TransactionKindNo = TransactionKindNo;
                ObjCredit.CompanyYear = HeaderTOSave.CompanyYear;
                ObjCredit.CompanyID = HeaderTOSave.CompanyID;
                ObjCredit.Credit = Total;
                if (HeaderServiceBill.ConversionFactor == 0)
                {
                    HeaderServiceBill.ConversionFactor = 1;
                };
                if (HeaderServiceBill.ConversionFactor == 1)
                {
                    ObjCredit.CreditForeign = 0;
                    ObjCredit.CreditDebitForeign = 0;
                }
                else
                {
                    ObjCredit.CreditDebitForeign = Total / ExchangeRate;
                    ObjCredit.CreditForeign = Total / ExchangeRate;
                }
                ObjCredit.Debit = 0;
                ObjCredit.DebitForeign = 0;
                ObjCredit.Exported = 0;
                ObjCredit.InsDateTime = DateTime.Now;
                ObjCredit.RowNumber = 2;
                ObjCredit.VHI = HeaderTOSave.VHI;
                ObjCredit.VoucherDate = HeaderTOSave.VoucherDate;
                ObjCredit.VoucherNumber = HeaderTOSave.VoucherNumber;

                _unitOfWork.Transaction.Add(ObjDebit);
                _unitOfWork.Transaction.Add(ObjCredit);

                if (!HeaderServiceBill.NoTax)
                {
                    Transaction ObjTax = new Transaction();
                    ObjTax.AccountNumber = HeaderServiceBill.TaxAccountNumber;
                    ObjTax.CostCenter = HeaderServiceBill.TaxCostNumber;
                    ObjTax.CompanyTransactionKindNo = CompanyTransactionKindID;
                    ObjTax.TransactionKindNo = TransactionKindNo;
                    ObjTax.CompanyYear = HeaderTOSave.CompanyYear;
                    ObjTax.CompanyID = HeaderTOSave.CompanyID;
                    ObjTax.Credit = TotalTax;
                    if (HeaderServiceBill.ConversionFactor == 0)
                    {
                        HeaderServiceBill.ConversionFactor = 1;
                    };
                    if (HeaderServiceBill.ConversionFactor == 1)
                    {
                        ObjTax.CreditForeign = 0;
                        ObjTax.CreditDebitForeign = 0;
                    }
                    else
                    {
                        ObjTax.CreditDebitForeign = TotalTax / ExchangeRate;
                        ObjTax.CreditForeign = TotalTax / ExchangeRate;
                    }
                    ObjTax.Debit = 0;
                    ObjTax.DebitForeign = 0;
                    ObjTax.Exported = 0;
                    ObjTax.InsDateTime = DateTime.Now;
                    ObjTax.RowNumber = 3;
                    ObjTax.VHI = HeaderTOSave.VHI;
                    ObjTax.VoucherDate = HeaderTOSave.VoucherDate;
                    ObjTax.VoucherNumber = HeaderTOSave.VoucherNumber;
                    _unitOfWork.Transaction.Add(ObjTax);
                }

                iRow = 1;
                foreach (var Trans in TransActionServiceBill)
                {
                    Trans.BillID = HeaderTOSave.VHI;
                    Trans.CompanyID = HeaderTOSave.CompanyID;
                    Trans.CompanyYear = HeaderTOSave.CompanyYear;
                    Trans.iRowID = iRow;
                    Trans.CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo;
                    Trans.TransactionKindNo = HeaderTOSave.TransactionKindNo;
                    Trans.FCurrencyID = HeaderTOSave.FCurrencyID;
                    iRow = iRow + 1;
                    _unitOfWork.TransActionServiceBill.Add(Trans);
                }
                HeaderServiceBill.BillID = HeaderTOSave.VHI;
                HeaderServiceBill.CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo;
                HeaderServiceBill.TransactionKindNo = HeaderTOSave.TransactionKindNo;
                _unitOfWork.ServiceBillHeader.Add(HeaderServiceBill);
                _unitOfWork.Transaction.DeleteMarkeTrans(HeaderTOSave.CompanyID, HeaderTOSave.VoucherNumber, HeaderTOSave.CompanyTransactionKindNo, HeaderTOSave.TransactionKindNo, HeaderTOSave.CompanyYear);

                _unitOfWork.Complete();
                Msg.Code = 1;
                Msg.Msg = Resources.Resource.AddedSuccessfully;
                Msg.Year = HeaderTOSave.CompanyYear;
                Msg.FCompanyId = HeaderTOSave.CompanyID;
                Msg.VoucherNumber = HeaderTOSave.VoucherNumber.ToString();
                Msg.TransactionKindNo = HeaderTOSave.TransactionKindNo.ToString();
                Msg.CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo.ToString();
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _unitOfWork.Transaction.UnMarkDeleteTrans(HeaderTOSave.CompanyID, HeaderTOSave.VoucherNumber, HeaderTOSave.CompanyTransactionKindNo, HeaderTOSave.TransactionKindNo, HeaderTOSave.CompanyYear);
                _unitOfWork.Complete();
                Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                Msg.Code = 0;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult UpdateMultiServiceBill(TransServiceBillVM ObjSave)
        {
            MsgUnit Msg = new MsgUnit();
            var HeaderTOSave = new Header();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                int CurrYear = ObjSave.HeaderServiceBill.CompanyYear;
                var TransActionServiceBill = ObjSave.TransActionServiceBill;
                HeaderServiceBill HeaderServiceBill = new HeaderServiceBill();
                HeaderServiceBill.AccountNumber = ObjSave.HeaderServiceBill.AccountNumber;
                HeaderServiceBill.BillDate = ObjSave.HeaderServiceBill.BillDate;
                HeaderServiceBill.CompanyID = ObjSave.HeaderServiceBill.CompanyID;
                HeaderServiceBill.CompanyYear = ObjSave.HeaderServiceBill.CompanyYear;
                HeaderServiceBill.CreditAccountNumber = ObjSave.HeaderServiceBill.CreditAccountNumber;
                HeaderServiceBill.CreditCostNumber = ObjSave.HeaderServiceBill.CreditCostNumber;
                HeaderServiceBill.DebitAccountNumber = ObjSave.HeaderServiceBill.DebitAccountNumber;
                HeaderServiceBill.DebitCostNumber = ObjSave.HeaderServiceBill.DebitCostNumber;
                HeaderServiceBill.Discount = ObjSave.HeaderServiceBill.Discount;
                HeaderServiceBill.ConversionFactor = ObjSave.HeaderServiceBill.CurrencyNewValue;
                HeaderServiceBill.FCurrencyID = ObjSave.HeaderServiceBill.CurrencyID;
                HeaderServiceBill.NetTotal = ObjSave.HeaderServiceBill.NetTotal;
                HeaderServiceBill.NetTotalForeignAmount = ObjSave.HeaderServiceBill.NetTotalForeignAmount;
                HeaderServiceBill.ForeignAmount = ObjSave.HeaderServiceBill.ForeignAmount;
                HeaderServiceBill.ForeignAmountTax = ObjSave.HeaderServiceBill.ForeignAmountTax;
                HeaderServiceBill.NoTax = ObjSave.HeaderServiceBill.NoTax;
                HeaderServiceBill.Note = ObjSave.HeaderServiceBill.Note;
                HeaderServiceBill.TransactionKindNo = 11;
                HeaderServiceBill.CompanyTransactionKindNo = ObjSave.HeaderServiceBill.CompanyTransactionKindID;
                HeaderServiceBill.Tax = ObjSave.HeaderServiceBill.Tax;
                HeaderServiceBill.TaxAccountNumber = ObjSave.HeaderServiceBill.TaxAccountNumber;
                HeaderServiceBill.TaxCostNumber = ObjSave.HeaderServiceBill.TaxCostNumber;
                HeaderServiceBill.Total = ObjSave.HeaderServiceBill.Total;
                HeaderServiceBill.SaleManNo = ObjSave.HeaderServiceBill.SaleManNo;
                var TransactionKindNo = 11;
                var CompanyTransactionKindID = ObjSave.HeaderServiceBill.CompanyTransactionKindID;
                double LineTotals = 0;
                double Price = 0;
                double Qty = 0;
                double TaxPrec = 0;
                double Total = 0;
                double TotalTax = 0;
                double NetTotal = 0;
                double TotalDebit = 0;
                double TotalCredit = 0;
                double TotalDebitForeign = 0;
                double TotalCreditForeign = 0;
                int iRow = 1;
                var ExchangeRate = HeaderServiceBill.ConversionFactor;
                HeaderServiceBill.CompanyID = UserInfo.fCompanyId;
                HeaderServiceBill.CompanyYear = CurrYear;
                var AllAcc = _unitOfWork.NativeSql.GetTransActionAccount(UserInfo.fCompanyId);
                foreach (var Trans in TransActionServiceBill)
                {
                    Price = Trans.Price;
                    Qty = Trans.Qty;
                    TaxPrec = Trans.Tax / 100;
                    if (HeaderServiceBill.ConversionFactor == 0)
                    {
                        HeaderServiceBill.ConversionFactor = 1;
                    };
                    if (HeaderServiceBill.ConversionFactor == 1)
                    {
                        Trans.ForeignPrice = 0;
                    }
                    else
                    {
                        Trans.ForeignPrice = Trans.Price / ExchangeRate;
                    }

                    Trans.PriceAfterLineDisc = Trans.Price - Trans.LocalDiscount;
                    Trans.ConversionFactor = ExchangeRate;
                    if (HeaderServiceBill.NoTax)
                    {
                        Trans.TaxValue = 0;

                    }
                    else
                    {
                        Trans.TaxValue = Trans.PriceAfterLineDisc * TaxPrec;
                    }
                    Trans.TotalLocal = Trans.PriceAfterLineDisc * Trans.Qty;
                    if (Trans.ForeignPrice > 0)
                    {
                        Trans.ForeignPriceAfterLineDisc = (Trans.Price / ExchangeRate) - Trans.LocalDiscount;
                        Trans.TotalForeign = Trans.ForeignPriceAfterLineDisc * Trans.Qty;
                    }
                    LineTotals = LineTotals + Trans.TotalLocal;
                }
                var TotalDiscPrec = HeaderServiceBill.Discount / LineTotals;
                foreach (var Trans in TransActionServiceBill)
                {
                    TaxPrec = Trans.Tax / 100;
                    Trans.TotalDiscPrec = TotalDiscPrec;
                    Trans.TotalDiscValue = TotalDiscPrec * Trans.TotalLocal;
                    Trans.NetAfterTotalDisc = Trans.TotalLocal - Trans.TotalDiscValue;
                    if (HeaderServiceBill.NoTax)
                    {
                        Trans.NetTotalTax = 0;
                    }
                    else
                    {
                        Trans.NetTotalTax = TaxPrec * Trans.NetAfterTotalDisc;
                    }
                    Total = Total + Trans.NetAfterTotalDisc;
                    TotalTax = TotalTax + Trans.NetTotalTax;
                }
                NetTotal = Total + TotalTax;
                TotalDebit = NetTotal;
                TotalCredit = Total + TotalTax;
                HeaderServiceBill.NetTotal = NetTotal;
                if (HeaderServiceBill.ConversionFactor == 0)
                {
                    HeaderServiceBill.ConversionFactor = 1;
                };
                if (HeaderServiceBill.ConversionFactor == 1)
                {
                    TotalDebitForeign = 0;
                    TotalCreditForeign = 0;
                }
                else
                {
                    TotalDebitForeign = Total / ExchangeRate;
                    TotalCreditForeign = (HeaderServiceBill.NetTotal) / ExchangeRate;
                }

                if (TotalCredit != TotalDebit)
                {
                    Msg.Msg = Resources.Resource.TheAccountingEntryIsUneven;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }
                if (TotalCredit <= 0)
                {
                    Msg.Msg = Resources.Resource.CantBeZero;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }
                
                HeaderTOSave.TransactionKindNo = TransactionKindNo;
                HeaderTOSave.CompanyTransactionKindNo = CompanyTransactionKindID;
                HeaderTOSave.InsDateTime = DateTime.Now;
                HeaderTOSave.InsUserID = userId;
                HeaderTOSave.CompanyID = UserInfo.fCompanyId;
                HeaderTOSave.TotalCredit = TotalCredit;
                HeaderTOSave.TotalDebit = TotalDebit;
                HeaderTOSave.TotalDebitForeign = TotalCreditForeign;
                HeaderTOSave.TotalCreditForeign = TotalCreditForeign;
                HeaderTOSave.CompanyYear = CurrYear;
                HeaderTOSave.VoucherNumber = ObjSave.HeaderServiceBill.BillID.ToString();
                HeaderTOSave.VHI = int.Parse(HeaderTOSave.VoucherNumber);
                HeaderTOSave.VoucherDate = HeaderServiceBill.BillDate;
                HeaderTOSave.FCurrencyID = HeaderServiceBill.FCurrencyID;
                HeaderTOSave.ConversionFactor = HeaderServiceBill.ConversionFactor;
                HeaderTOSave.SaleID = HeaderServiceBill.SaleManNo;

                _unitOfWork.ServiceBillHeader.Delete(HeaderTOSave.CompanyID, HeaderTOSave.VHI, HeaderServiceBill.CompanyTransactionKindNo, HeaderServiceBill.TransactionKindNo, HeaderTOSave.CompanyYear);
                _unitOfWork.TransActionServiceBill.Delete(HeaderTOSave.CompanyID, HeaderTOSave.VHI, HeaderServiceBill.CompanyTransactionKindNo, HeaderServiceBill.TransactionKindNo, HeaderTOSave.CompanyYear);
                _unitOfWork.Transaction.MarkDeleteTrans(HeaderTOSave.CompanyID, HeaderTOSave.VoucherNumber, HeaderTOSave.CompanyTransactionKindNo, HeaderTOSave.TransactionKindNo, HeaderTOSave.CompanyYear);
                _unitOfWork.Complete();

                iRow = 0;
                Transaction ObjDebit = new Transaction();
                ObjDebit.AccountNumber = HeaderServiceBill.DebitAccountNumber;
                ObjDebit.CostCenter = HeaderServiceBill.DebitCostNumber;
                ObjDebit.CompanyTransactionKindNo = CompanyTransactionKindID;
                ObjDebit.TransactionKindNo = TransactionKindNo;
                ObjDebit.CompanyYear = HeaderTOSave.CompanyYear;
                ObjDebit.CompanyID = HeaderTOSave.CompanyID;
                ObjDebit.Credit = 0;
                ObjDebit.CreditForeign = 0;
                ObjDebit.Debit = TotalDebit;
                if (HeaderServiceBill.ConversionFactor == 0)
                {
                    HeaderServiceBill.ConversionFactor = 1;
                };
                if (HeaderServiceBill.ConversionFactor == 1)
                {
                    ObjDebit.DebitForeign = 0;
                    ObjDebit.CreditDebitForeign = 0;
                }
                else
                {
                    ObjDebit.DebitForeign = TotalDebit / ExchangeRate;
                    ObjDebit.CreditDebitForeign = TotalDebit / ExchangeRate;
                }
                ObjDebit.Exported = 0;
                ObjDebit.InsDateTime = DateTime.Now;
                ObjDebit.RowNumber = 1;
                ObjDebit.VHI = HeaderTOSave.VHI;
                ObjDebit.VoucherDate = HeaderTOSave.VoucherDate;
                ObjDebit.VoucherNumber = HeaderTOSave.VoucherNumber;
                _unitOfWork.Transaction.Add(ObjDebit);
                iRow = 1;
                foreach (var Trans in TransActionServiceBill)
                {
                    Transaction ObjCredit = new Transaction();
                    iRow = iRow + 1;
                    ObjCredit.CompanyTransactionKindNo = CompanyTransactionKindID;
                    ObjCredit.TransactionKindNo = TransactionKindNo;
                    ObjCredit.CompanyYear = HeaderTOSave.CompanyYear;
                    ObjCredit.CompanyID = HeaderTOSave.CompanyID;
                    ObjCredit.Credit = Trans.NetAfterTotalDisc;
                    if (HeaderServiceBill.ConversionFactor == 0)
                    {
                        HeaderServiceBill.ConversionFactor = 1;
                    };
                    if (HeaderServiceBill.ConversionFactor == 1)
                    {
                        ObjCredit.CreditForeign = 0;
                        ObjCredit.CreditDebitForeign = 0;
                    }
                    else
                    {
                        ObjCredit.CreditDebitForeign = ObjCredit.Credit / ExchangeRate;
                        ObjCredit.CreditForeign = ObjCredit.Credit / ExchangeRate;
                    }
                    ObjCredit.Debit = 0;
                    ObjCredit.DebitForeign = 0;
                    ObjCredit.Exported = 0;
                    ObjCredit.InsDateTime = DateTime.Now;
                    ObjCredit.RowNumber = iRow;
                    ObjCredit.VHI = HeaderTOSave.VHI;
                    ObjCredit.VoucherDate = HeaderTOSave.VoucherDate;
                    ObjCredit.VoucherNumber = HeaderTOSave.VoucherNumber;
                    _unitOfWork.Transaction.Add(ObjCredit);
                }
                if (!HeaderServiceBill.NoTax)
                {
                    Transaction ObjTax = new Transaction();
                    ObjTax.AccountNumber = HeaderServiceBill.TaxAccountNumber;
                    ObjTax.CostCenter = HeaderServiceBill.TaxCostNumber;
                    ObjTax.CompanyTransactionKindNo = CompanyTransactionKindID;
                    ObjTax.TransactionKindNo = TransactionKindNo;
                    ObjTax.CompanyYear = HeaderTOSave.CompanyYear;
                    ObjTax.CompanyID = HeaderTOSave.CompanyID;
                    ObjTax.Credit = TotalTax;
                    if (HeaderServiceBill.ConversionFactor == 0)
                    {
                        HeaderServiceBill.ConversionFactor = 1;
                    };
                    if (HeaderServiceBill.ConversionFactor == 1)
                    {
                        ObjTax.CreditForeign = 0;
                        ObjTax.CreditDebitForeign = 0;
                    }
                    else
                    {
                        ObjTax.CreditDebitForeign = TotalTax / ExchangeRate;
                        ObjTax.CreditForeign = TotalTax / ExchangeRate;
                    }
                    ObjTax.Debit = 0;
                    ObjTax.DebitForeign = 0;
                    ObjTax.Exported = 0;
                    ObjTax.InsDateTime = DateTime.Now;
                    iRow = iRow + 1;
                    ObjTax.RowNumber = iRow;
                    ObjTax.VHI = HeaderTOSave.VHI;
                    ObjTax.VoucherDate = HeaderTOSave.VoucherDate;
                    ObjTax.VoucherNumber = HeaderTOSave.VoucherNumber;
                    _unitOfWork.Transaction.Add(ObjTax);
                }
                HeaderTOSave.RowCount = iRow;
                _unitOfWork.Header.Update(HeaderTOSave);
                iRow = 1;
                foreach (var Trans in TransActionServiceBill)
                {
                    Trans.BillID = HeaderTOSave.VHI;
                    Trans.CompanyID = HeaderTOSave.CompanyID;
                    Trans.CompanyYear = HeaderTOSave.CompanyYear;
                    Trans.iRowID = iRow;
                    Trans.CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo;
                    Trans.TransactionKindNo = HeaderTOSave.TransactionKindNo;
                    Trans.FCurrencyID = HeaderTOSave.FCurrencyID;
                    iRow = iRow + 1;
                    _unitOfWork.TransActionServiceBill.Add(Trans);
                }
                HeaderServiceBill.BillID = HeaderTOSave.VHI;
                HeaderServiceBill.CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo;
                HeaderServiceBill.TransactionKindNo = HeaderTOSave.TransactionKindNo;
                _unitOfWork.ServiceBillHeader.Add(HeaderServiceBill);
                _unitOfWork.Transaction.DeleteMarkeTrans(HeaderTOSave.CompanyID, HeaderTOSave.VoucherNumber, HeaderTOSave.CompanyTransactionKindNo, HeaderTOSave.TransactionKindNo, HeaderTOSave.CompanyYear);
                _unitOfWork.Complete();
                Msg.Code = 1;
                Msg.Msg = Resources.Resource.AddedSuccessfully;
                Msg.Year = HeaderTOSave.CompanyYear;
                Msg.FCompanyId = HeaderTOSave.CompanyID;
                Msg.VoucherNumber = HeaderTOSave.VoucherNumber.ToString();
                Msg.TransactionKindNo = HeaderTOSave.TransactionKindNo.ToString();
                Msg.CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo.ToString();
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _unitOfWork.Transaction.UnMarkDeleteTrans(HeaderTOSave.CompanyID, HeaderTOSave.VoucherNumber, HeaderTOSave.CompanyTransactionKindNo, HeaderTOSave.TransactionKindNo, HeaderTOSave.CompanyYear);
                _unitOfWork.Complete();
                Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                Msg.Code = 0;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult UpdateMultiReturnServiceBill(TransServiceBillVM ObjSave)
        {
            MsgUnit Msg = new MsgUnit();
            var HeaderTOSave = new Header();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                int CurrYear = ObjSave.HeaderServiceBill.CompanyYear;
                var TransActionServiceBill = ObjSave.TransActionServiceBill;
                HeaderServiceBill HeaderServiceBill = new HeaderServiceBill();
                HeaderServiceBill.AccountNumber = ObjSave.HeaderServiceBill.AccountNumber;
                HeaderServiceBill.BillDate = ObjSave.HeaderServiceBill.BillDate;
                HeaderServiceBill.CompanyID = ObjSave.HeaderServiceBill.CompanyID;
                HeaderServiceBill.CompanyYear = ObjSave.HeaderServiceBill.CompanyYear;
                HeaderServiceBill.DebitAccountNumber = ObjSave.HeaderServiceBill.DebitAccountNumber;
                HeaderServiceBill.DebitCostNumber = ObjSave.HeaderServiceBill.DebitCostNumber;
                HeaderServiceBill.CreditAccountNumber = ObjSave.HeaderServiceBill.CreditAccountNumber;
                HeaderServiceBill.CreditCostNumber = ObjSave.HeaderServiceBill.CreditCostNumber;
                HeaderServiceBill.Discount = ObjSave.HeaderServiceBill.Discount;
                HeaderServiceBill.ConversionFactor = ObjSave.HeaderServiceBill.CurrencyNewValue;
                HeaderServiceBill.FCurrencyID = ObjSave.HeaderServiceBill.CurrencyID;
                HeaderServiceBill.NetTotal = ObjSave.HeaderServiceBill.NetTotal;
                HeaderServiceBill.NetTotalForeignAmount = ObjSave.HeaderServiceBill.NetTotalForeignAmount;
                HeaderServiceBill.ForeignAmount = ObjSave.HeaderServiceBill.ForeignAmount;
                HeaderServiceBill.ForeignAmountTax = ObjSave.HeaderServiceBill.ForeignAmountTax;
                HeaderServiceBill.NoTax = ObjSave.HeaderServiceBill.NoTax;
                HeaderServiceBill.Note = ObjSave.HeaderServiceBill.Note;
                HeaderServiceBill.TransactionKindNo = 20;
                HeaderServiceBill.CompanyTransactionKindNo = ObjSave.HeaderServiceBill.CompanyTransactionKindID;
                HeaderServiceBill.Tax = ObjSave.HeaderServiceBill.Tax;
                HeaderServiceBill.TaxAccountNumber = ObjSave.HeaderServiceBill.TaxAccountNumber;
                HeaderServiceBill.TaxCostNumber = ObjSave.HeaderServiceBill.TaxCostNumber;
                HeaderServiceBill.Total = ObjSave.HeaderServiceBill.Total;
                HeaderServiceBill.SaleManNo = ObjSave.HeaderServiceBill.SaleManNo;
                var TransactionKindNo = 20;
                var CompanyTransactionKindID = ObjSave.HeaderServiceBill.CompanyTransactionKindID;
                double LineTotals = 0;
                double Price = 0;
                double Qty = 0;
                double TaxPrec = 0;
                double Total = 0;
                double TotalTax = 0;
                double NetTotal = 0;
                double TotalCredit = 0;
                double TotalDebit = 0;
                double TotalCreditForeign = 0;
                double TotalDebitForeign = 0;
                int iRow = 1;
                var ExchangeRate = HeaderServiceBill.ConversionFactor;
                HeaderServiceBill.CompanyID = UserInfo.fCompanyId;
                HeaderServiceBill.CompanyYear = CurrYear;
                var AllAcc = _unitOfWork.NativeSql.GetTransActionAccount(UserInfo.fCompanyId);
                foreach (var Trans in TransActionServiceBill)
                {
                    Price = Trans.Price;
                    Qty = Trans.Qty;
                    TaxPrec = Trans.Tax / 100;
                    if (HeaderServiceBill.ConversionFactor == 0)
                    {
                        HeaderServiceBill.ConversionFactor = 1;
                    };
                    if (HeaderServiceBill.ConversionFactor == 1)
                    {
                        Trans.ForeignPrice = 0;
                    }
                    else
                    {
                        Trans.ForeignPrice = Trans.Price / ExchangeRate;
                    }

                    Trans.PriceAfterLineDisc = Trans.Price - Trans.LocalDiscount;
                    Trans.ConversionFactor = ExchangeRate;
                    if (HeaderServiceBill.NoTax)
                    {
                        Trans.TaxValue = 0;

                    }
                    else
                    {
                        Trans.TaxValue = Trans.PriceAfterLineDisc * TaxPrec;
                    }
                    Trans.TotalLocal = Trans.PriceAfterLineDisc * Trans.Qty;
                    if (Trans.ForeignPrice > 0)
                    {
                        Trans.ForeignPriceAfterLineDisc = (Trans.Price / ExchangeRate) - Trans.LocalDiscount;
                        Trans.TotalForeign = Trans.ForeignPriceAfterLineDisc * Trans.Qty;
                    }
                    LineTotals = LineTotals + Trans.TotalLocal;
                }
                var TotalDiscPrec = HeaderServiceBill.Discount / LineTotals;
                foreach (var Trans in TransActionServiceBill)
                {
                    TaxPrec = Trans.Tax / 100;
                    Trans.TotalDiscPrec = TotalDiscPrec;
                    Trans.TotalDiscValue = TotalDiscPrec * Trans.TotalLocal;
                    Trans.NetAfterTotalDisc = Trans.TotalLocal - Trans.TotalDiscValue;
                    if (HeaderServiceBill.NoTax)
                    {
                        Trans.NetTotalTax = 0;
                    }
                    else
                    {
                        Trans.NetTotalTax = TaxPrec * Trans.NetAfterTotalDisc;
                    }
                    Total = Total + Trans.NetAfterTotalDisc;
                    TotalTax = TotalTax + Trans.NetTotalTax;
                }
                NetTotal = Total + TotalTax;
                TotalCredit = NetTotal;
                TotalDebit = Total + TotalTax;
                HeaderServiceBill.NetTotal = NetTotal;
                if (HeaderServiceBill.ConversionFactor == 0)
                {
                    HeaderServiceBill.ConversionFactor = 1;
                };
                if (HeaderServiceBill.ConversionFactor == 1)
                {
                    TotalCreditForeign = 0;
                    TotalDebitForeign = 0;
                }
                else
                {
                    TotalCreditForeign = Total / ExchangeRate;
                    TotalDebitForeign = (HeaderServiceBill.NetTotal) / ExchangeRate;
                }

                if (TotalDebit != TotalCredit)
                {
                    Msg.Msg = Resources.Resource.TheAccountingEntryIsUneven;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }
                if (TotalDebit <= 0)
                {
                    Msg.Msg = Resources.Resource.CantBeZero;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }

                HeaderTOSave.TransactionKindNo = TransactionKindNo;
                HeaderTOSave.CompanyTransactionKindNo = CompanyTransactionKindID;
                HeaderTOSave.InsDateTime = DateTime.Now;
                HeaderTOSave.InsUserID = userId;
                HeaderTOSave.CompanyID = UserInfo.fCompanyId;
                HeaderTOSave.TotalDebit = TotalDebit;
                HeaderTOSave.TotalCredit = TotalCredit;
                HeaderTOSave.TotalCreditForeign = TotalDebitForeign;
                HeaderTOSave.TotalDebitForeign = TotalDebitForeign;
                HeaderTOSave.CompanyYear = CurrYear;
                HeaderTOSave.VoucherNumber = ObjSave.HeaderServiceBill.BillID.ToString();
                HeaderTOSave.VHI = int.Parse(HeaderTOSave.VoucherNumber);
                HeaderTOSave.VoucherDate = HeaderServiceBill.BillDate;
                HeaderTOSave.FCurrencyID = HeaderServiceBill.FCurrencyID;
                HeaderTOSave.ConversionFactor = HeaderServiceBill.ConversionFactor;
                HeaderTOSave.SaleID = HeaderServiceBill.SaleManNo;

                _unitOfWork.ServiceBillHeader.Delete(HeaderTOSave.CompanyID, HeaderTOSave.VHI, HeaderServiceBill.CompanyTransactionKindNo, HeaderServiceBill.TransactionKindNo, HeaderTOSave.CompanyYear);
                _unitOfWork.TransActionServiceBill.Delete(HeaderTOSave.CompanyID, HeaderTOSave.VHI, HeaderServiceBill.CompanyTransactionKindNo, HeaderServiceBill.TransactionKindNo, HeaderTOSave.CompanyYear);
                _unitOfWork.Transaction.MarkDeleteTrans(HeaderTOSave.CompanyID, HeaderTOSave.VoucherNumber, HeaderTOSave.CompanyTransactionKindNo, HeaderTOSave.TransactionKindNo, HeaderTOSave.CompanyYear);
                _unitOfWork.Complete();

                iRow = 0;
                Transaction ObjCredit = new Transaction();
                ObjCredit.AccountNumber = HeaderServiceBill.CreditAccountNumber;
                ObjCredit.CostCenter = HeaderServiceBill.CreditCostNumber;
                ObjCredit.CompanyTransactionKindNo = CompanyTransactionKindID;
                ObjCredit.TransactionKindNo = TransactionKindNo;
                ObjCredit.CompanyYear = HeaderTOSave.CompanyYear;
                ObjCredit.CompanyID = HeaderTOSave.CompanyID;
                ObjCredit.Debit = 0;
                ObjCredit.DebitForeign = 0;
                ObjCredit.Credit = TotalCredit;
                if (HeaderServiceBill.ConversionFactor == 0)
                {
                    HeaderServiceBill.ConversionFactor = 1;
                };
                if (HeaderServiceBill.ConversionFactor == 1)
                {
                    ObjCredit.CreditForeign = 0;
                    ObjCredit.CreditDebitForeign = 0;
                }
                else
                {
                    ObjCredit.CreditForeign = TotalCredit / ExchangeRate;
                    ObjCredit.CreditDebitForeign = TotalCredit / ExchangeRate;
                }
                ObjCredit.Exported = 0;
                ObjCredit.InsDateTime = DateTime.Now;
                ObjCredit.RowNumber = 1;
                ObjCredit.VHI = HeaderTOSave.VHI;
                ObjCredit.VoucherDate = HeaderTOSave.VoucherDate;
                ObjCredit.VoucherNumber = HeaderTOSave.VoucherNumber;
                _unitOfWork.Transaction.Add(ObjCredit);
                iRow = 1;
                foreach (var Trans in TransActionServiceBill)
                {
                    Transaction ObjDebit = new Transaction();
                    iRow = iRow + 1;
                    ObjDebit.CompanyTransactionKindNo = CompanyTransactionKindID;
                    ObjDebit.TransactionKindNo = TransactionKindNo;
                    ObjDebit.CompanyYear = HeaderTOSave.CompanyYear;
                    ObjDebit.CompanyID = HeaderTOSave.CompanyID;
                    ObjDebit.Debit = Trans.NetAfterTotalDisc;
                    if (HeaderServiceBill.ConversionFactor == 0)
                    {
                        HeaderServiceBill.ConversionFactor = 1;
                    };
                    if (HeaderServiceBill.ConversionFactor == 1)
                    {
                        ObjDebit.DebitForeign = 0;
                        ObjDebit.CreditDebitForeign = 0;
                    }
                    else
                    {
                        ObjDebit.CreditDebitForeign = ObjDebit.Debit / ExchangeRate;
                        ObjDebit.DebitForeign = ObjDebit.Debit / ExchangeRate;
                    }
                    ObjDebit.Credit = 0;
                    ObjDebit.CreditForeign = 0;
                    ObjDebit.Exported = 0;
                    ObjDebit.InsDateTime = DateTime.Now;
                    ObjDebit.RowNumber = iRow;
                    ObjDebit.VHI = HeaderTOSave.VHI;
                    ObjDebit.VoucherDate = HeaderTOSave.VoucherDate;
                    ObjDebit.VoucherNumber = HeaderTOSave.VoucherNumber;
                    _unitOfWork.Transaction.Add(ObjDebit);
                }
                if (!HeaderServiceBill.NoTax)
                {
                    Transaction ObjTax = new Transaction();
                    ObjTax.AccountNumber = HeaderServiceBill.TaxAccountNumber;
                    ObjTax.CostCenter = HeaderServiceBill.TaxCostNumber;
                    ObjTax.CompanyTransactionKindNo = CompanyTransactionKindID;
                    ObjTax.TransactionKindNo = TransactionKindNo;
                    ObjTax.CompanyYear = HeaderTOSave.CompanyYear;
                    ObjTax.CompanyID = HeaderTOSave.CompanyID;
                    ObjTax.Debit = TotalTax;
                    if (HeaderServiceBill.ConversionFactor == 0)
                    {
                        HeaderServiceBill.ConversionFactor = 1;
                    };
                    if (HeaderServiceBill.ConversionFactor == 1)
                    {
                        ObjTax.DebitForeign = 0;
                        ObjTax.CreditDebitForeign = 0;
                    }
                    else
                    {
                        ObjTax.CreditDebitForeign = TotalTax / ExchangeRate;
                        ObjTax.DebitForeign = TotalTax / ExchangeRate;
                    }
                    ObjTax.Credit = 0;
                    ObjTax.CreditForeign = 0;
                    ObjTax.Exported = 0;
                    ObjTax.InsDateTime = DateTime.Now;
                    iRow = iRow + 1;
                    ObjTax.RowNumber = iRow;
                    ObjTax.VHI = HeaderTOSave.VHI;
                    ObjTax.VoucherDate = HeaderTOSave.VoucherDate;
                    ObjTax.VoucherNumber = HeaderTOSave.VoucherNumber;
                    _unitOfWork.Transaction.Add(ObjTax);
                }
                HeaderTOSave.RowCount = iRow;
                _unitOfWork.Header.Update(HeaderTOSave);
                iRow = 1;
                foreach (var Trans in TransActionServiceBill)
                {
                    Trans.BillID = HeaderTOSave.VHI;
                    Trans.CompanyID = HeaderTOSave.CompanyID;
                    Trans.CompanyYear = HeaderTOSave.CompanyYear;
                    Trans.iRowID = iRow;
                    Trans.CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo;
                    Trans.TransactionKindNo = HeaderTOSave.TransactionKindNo;
                    Trans.FCurrencyID = HeaderTOSave.FCurrencyID;
                    iRow = iRow + 1;
                    _unitOfWork.TransActionServiceBill.Add(Trans);
                }
                HeaderServiceBill.BillID = HeaderTOSave.VHI;
                HeaderServiceBill.CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo;
                HeaderServiceBill.TransactionKindNo = HeaderTOSave.TransactionKindNo;
                _unitOfWork.ServiceBillHeader.Add(HeaderServiceBill);
                _unitOfWork.Transaction.DeleteMarkeTrans(HeaderTOSave.CompanyID, HeaderTOSave.VoucherNumber, HeaderTOSave.CompanyTransactionKindNo, HeaderTOSave.TransactionKindNo, HeaderTOSave.CompanyYear);
                _unitOfWork.Complete();
                Msg.Code = 1;
                Msg.Msg = Resources.Resource.AddedSuccessfully;
                Msg.Year = HeaderTOSave.CompanyYear;
                Msg.FCompanyId = HeaderTOSave.CompanyID;
                Msg.VoucherNumber = HeaderTOSave.VoucherNumber.ToString();
                Msg.TransactionKindNo = HeaderTOSave.TransactionKindNo.ToString();
                Msg.CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo.ToString();
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _unitOfWork.Transaction.UnMarkDeleteTrans(HeaderTOSave.CompanyID, HeaderTOSave.VoucherNumber, HeaderTOSave.CompanyTransactionKindNo, HeaderTOSave.TransactionKindNo, HeaderTOSave.CompanyYear);
                _unitOfWork.Complete();
                Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                Msg.Code = 0;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult UpdateMultiPurchaseServiceBill(TransServiceBillVM ObjSave)
        {
            MsgUnit Msg = new MsgUnit();
            var HeaderTOSave = new Header();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                int CurrYear = ObjSave.HeaderServiceBill.CompanyYear;
                var TransActionServiceBill = ObjSave.TransActionServiceBill;
                HeaderServiceBill HeaderServiceBill = new HeaderServiceBill();
                HeaderServiceBill.AccountNumber = ObjSave.HeaderServiceBill.AccountNumber;
                HeaderServiceBill.BillDate = ObjSave.HeaderServiceBill.BillDate;
                HeaderServiceBill.CompanyID = ObjSave.HeaderServiceBill.CompanyID;
                HeaderServiceBill.CompanyYear = ObjSave.HeaderServiceBill.CompanyYear;
                HeaderServiceBill.DebitAccountNumber = ObjSave.HeaderServiceBill.DebitAccountNumber;
                HeaderServiceBill.DebitCostNumber = ObjSave.HeaderServiceBill.DebitCostNumber;
                HeaderServiceBill.CreditAccountNumber = ObjSave.HeaderServiceBill.CreditAccountNumber;
                HeaderServiceBill.CreditCostNumber = ObjSave.HeaderServiceBill.CreditCostNumber;
                HeaderServiceBill.Discount = ObjSave.HeaderServiceBill.Discount;
                HeaderServiceBill.ConversionFactor = ObjSave.HeaderServiceBill.CurrencyNewValue;
                HeaderServiceBill.FCurrencyID = ObjSave.HeaderServiceBill.CurrencyID;
                HeaderServiceBill.NetTotal = ObjSave.HeaderServiceBill.NetTotal;
                HeaderServiceBill.NetTotalForeignAmount = ObjSave.HeaderServiceBill.NetTotalForeignAmount;
                HeaderServiceBill.ForeignAmount = ObjSave.HeaderServiceBill.ForeignAmount;
                HeaderServiceBill.ForeignAmountTax = ObjSave.HeaderServiceBill.ForeignAmountTax;
                HeaderServiceBill.NoTax = ObjSave.HeaderServiceBill.NoTax;
                HeaderServiceBill.Note = ObjSave.HeaderServiceBill.Note;
                HeaderServiceBill.TransactionKindNo = 13;
                HeaderServiceBill.CompanyTransactionKindNo = ObjSave.HeaderServiceBill.CompanyTransactionKindID;
                HeaderServiceBill.Tax = ObjSave.HeaderServiceBill.Tax;
                HeaderServiceBill.TaxAccountNumber = ObjSave.HeaderServiceBill.TaxAccountNumber;
                HeaderServiceBill.TaxCostNumber = ObjSave.HeaderServiceBill.TaxCostNumber;
                HeaderServiceBill.Total = ObjSave.HeaderServiceBill.Total;
                HeaderServiceBill.SaleManNo = ObjSave.HeaderServiceBill.SaleManNo;
                var TransactionKindNo = 13;
                var CompanyTransactionKindID = ObjSave.HeaderServiceBill.CompanyTransactionKindID;
                double LineTotals = 0;
                double Price = 0;
                double Qty = 0;
                double TaxPrec = 0;
                double Total = 0;
                double TotalTax = 0;
                double NetTotal = 0;
                double TotalCredit = 0;
                double TotalDebit = 0;
                double TotalCreditForeign = 0;
                double TotalDebitForeign = 0;
                int iRow = 1;
                var ExchangeRate = HeaderServiceBill.ConversionFactor;
                HeaderServiceBill.CompanyID = UserInfo.fCompanyId;
                HeaderServiceBill.CompanyYear = CurrYear;
                var AllAcc = _unitOfWork.NativeSql.GetTransActionAccount(UserInfo.fCompanyId);
                foreach (var Trans in TransActionServiceBill)
                {
                    Price = Trans.Price;
                    Qty = Trans.Qty;
                    TaxPrec = Trans.Tax / 100;
                    if (HeaderServiceBill.ConversionFactor == 0)
                    {
                        HeaderServiceBill.ConversionFactor = 1;
                    };
                    if (HeaderServiceBill.ConversionFactor == 1)
                    {
                        Trans.ForeignPrice = 0;
                    }
                    else
                    {
                        Trans.ForeignPrice = Trans.Price / ExchangeRate;
                    }

                    Trans.PriceAfterLineDisc = Trans.Price - Trans.LocalDiscount;
                    Trans.ConversionFactor = ExchangeRate;
                    if (HeaderServiceBill.NoTax)
                    {
                        Trans.TaxValue = 0;

                    }
                    else
                    {
                        Trans.TaxValue = Trans.PriceAfterLineDisc * TaxPrec;
                    }
                    Trans.TotalLocal = Trans.PriceAfterLineDisc * Trans.Qty;
                    if (Trans.ForeignPrice > 0)
                    {
                        Trans.ForeignPriceAfterLineDisc = (Trans.Price / ExchangeRate) - Trans.LocalDiscount;
                        Trans.TotalForeign = Trans.ForeignPriceAfterLineDisc * Trans.Qty;
                    }
                    LineTotals = LineTotals + Trans.TotalLocal;
                }
                var TotalDiscPrec = HeaderServiceBill.Discount / LineTotals;
                foreach (var Trans in TransActionServiceBill)
                {
                    TaxPrec = Trans.Tax / 100;
                    Trans.TotalDiscPrec = TotalDiscPrec;
                    Trans.TotalDiscValue = TotalDiscPrec * Trans.TotalLocal;
                    Trans.NetAfterTotalDisc = Trans.TotalLocal - Trans.TotalDiscValue;
                    if (HeaderServiceBill.NoTax)
                    {
                        Trans.NetTotalTax = 0;
                    }
                    else
                    {
                        Trans.NetTotalTax = TaxPrec * Trans.NetAfterTotalDisc;
                    }
                    Total = Total + Trans.NetAfterTotalDisc;
                    TotalTax = TotalTax + Trans.NetTotalTax;
                }
                NetTotal = Total + TotalTax;
                TotalCredit = NetTotal;
                TotalDebit = Total + TotalTax;
                HeaderServiceBill.NetTotal = NetTotal;
                if (HeaderServiceBill.ConversionFactor == 0)
                {
                    HeaderServiceBill.ConversionFactor = 1;
                };
                if (HeaderServiceBill.ConversionFactor == 1)
                {
                    TotalCreditForeign = 0;
                    TotalDebitForeign = 0;
                }
                else
                {
                    TotalCreditForeign = Total / ExchangeRate;
                    TotalDebitForeign = (HeaderServiceBill.NetTotal) / ExchangeRate;
                }

                if (TotalDebit != TotalCredit)
                {
                    Msg.Msg = Resources.Resource.TheAccountingEntryIsUneven;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }
                if (TotalDebit <= 0)
                {
                    Msg.Msg = Resources.Resource.CantBeZero;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }

                HeaderTOSave.TransactionKindNo = TransactionKindNo;
                HeaderTOSave.CompanyTransactionKindNo = CompanyTransactionKindID;
                HeaderTOSave.InsDateTime = DateTime.Now;
                HeaderTOSave.InsUserID = userId;
                HeaderTOSave.CompanyID = UserInfo.fCompanyId;
                HeaderTOSave.TotalDebit = TotalDebit;
                HeaderTOSave.TotalCredit = TotalCredit;
                HeaderTOSave.TotalCreditForeign = TotalDebitForeign;
                HeaderTOSave.TotalDebitForeign = TotalDebitForeign;
                HeaderTOSave.CompanyYear = CurrYear;
                HeaderTOSave.VoucherNumber = ObjSave.HeaderServiceBill.BillID.ToString();
                HeaderTOSave.VHI = int.Parse(HeaderTOSave.VoucherNumber);
                HeaderTOSave.VoucherDate = HeaderServiceBill.BillDate;
                HeaderTOSave.FCurrencyID = HeaderServiceBill.FCurrencyID;
                HeaderTOSave.ConversionFactor = HeaderServiceBill.ConversionFactor;
                HeaderTOSave.SaleID = HeaderServiceBill.SaleManNo;

                _unitOfWork.ServiceBillHeader.Delete(HeaderTOSave.CompanyID, HeaderTOSave.VHI, HeaderServiceBill.CompanyTransactionKindNo, HeaderServiceBill.TransactionKindNo, HeaderTOSave.CompanyYear);
                _unitOfWork.TransActionServiceBill.Delete(HeaderTOSave.CompanyID, HeaderTOSave.VHI, HeaderServiceBill.CompanyTransactionKindNo, HeaderServiceBill.TransactionKindNo, HeaderTOSave.CompanyYear);
                _unitOfWork.Transaction.MarkDeleteTrans(HeaderTOSave.CompanyID, HeaderTOSave.VoucherNumber, HeaderTOSave.CompanyTransactionKindNo, HeaderTOSave.TransactionKindNo, HeaderTOSave.CompanyYear);
                _unitOfWork.Complete();

                iRow = 0;
                Transaction ObjCredit = new Transaction();
                ObjCredit.AccountNumber = HeaderServiceBill.CreditAccountNumber;
                ObjCredit.CostCenter = HeaderServiceBill.CreditCostNumber;
                ObjCredit.CompanyTransactionKindNo = CompanyTransactionKindID;
                ObjCredit.TransactionKindNo = TransactionKindNo;
                ObjCredit.CompanyYear = HeaderTOSave.CompanyYear;
                ObjCredit.CompanyID = HeaderTOSave.CompanyID;
                ObjCredit.Debit = 0;
                ObjCredit.DebitForeign = 0;
                ObjCredit.Credit = TotalCredit;
                if (HeaderServiceBill.ConversionFactor == 0)
                {
                    HeaderServiceBill.ConversionFactor = 1;
                };
                if (HeaderServiceBill.ConversionFactor == 1)
                {
                    ObjCredit.CreditForeign = 0;
                    ObjCredit.CreditDebitForeign = 0;
                }
                else
                {
                    ObjCredit.CreditForeign = TotalCredit / ExchangeRate;
                    ObjCredit.CreditDebitForeign = TotalCredit / ExchangeRate;
                }
                ObjCredit.Exported = 0;
                ObjCredit.InsDateTime = DateTime.Now;
                ObjCredit.RowNumber = 1;
                ObjCredit.VHI = HeaderTOSave.VHI;
                ObjCredit.VoucherDate = HeaderTOSave.VoucherDate;
                ObjCredit.VoucherNumber = HeaderTOSave.VoucherNumber;
                _unitOfWork.Transaction.Add(ObjCredit);
                iRow = 1;
                foreach (var Trans in TransActionServiceBill)
                {
                    Transaction ObjDebit = new Transaction();
                    iRow = iRow + 1;
                    ObjDebit.CompanyTransactionKindNo = CompanyTransactionKindID;
                    ObjDebit.TransactionKindNo = TransactionKindNo;
                    ObjDebit.CompanyYear = HeaderTOSave.CompanyYear;
                    ObjDebit.CompanyID = HeaderTOSave.CompanyID;
                    ObjDebit.Debit = Trans.NetAfterTotalDisc;
                    if (HeaderServiceBill.ConversionFactor == 0)
                    {
                        HeaderServiceBill.ConversionFactor = 1;
                    };
                    if (HeaderServiceBill.ConversionFactor == 1)
                    {
                        ObjDebit.DebitForeign = 0;
                        ObjDebit.CreditDebitForeign = 0;
                    }
                    else
                    {
                        ObjDebit.CreditDebitForeign = ObjDebit.Debit / ExchangeRate;
                        ObjDebit.DebitForeign = ObjDebit.Debit / ExchangeRate;
                    }
                    ObjDebit.Credit = 0;
                    ObjDebit.CreditForeign = 0;
                    ObjDebit.Exported = 0;
                    ObjDebit.InsDateTime = DateTime.Now;
                    ObjDebit.RowNumber = iRow;
                    ObjDebit.VHI = HeaderTOSave.VHI;
                    ObjDebit.VoucherDate = HeaderTOSave.VoucherDate;
                    ObjDebit.VoucherNumber = HeaderTOSave.VoucherNumber;
                    _unitOfWork.Transaction.Add(ObjDebit);
                }
                if (!HeaderServiceBill.NoTax)
                {
                    Transaction ObjTax = new Transaction();
                    ObjTax.AccountNumber = HeaderServiceBill.TaxAccountNumber;
                    ObjTax.CostCenter = HeaderServiceBill.TaxCostNumber;
                    ObjTax.CompanyTransactionKindNo = CompanyTransactionKindID;
                    ObjTax.TransactionKindNo = TransactionKindNo;
                    ObjTax.CompanyYear = HeaderTOSave.CompanyYear;
                    ObjTax.CompanyID = HeaderTOSave.CompanyID;
                    ObjTax.Debit = TotalTax;
                    if (HeaderServiceBill.ConversionFactor == 0)
                    {
                        HeaderServiceBill.ConversionFactor = 1;
                    };
                    if (HeaderServiceBill.ConversionFactor == 1)
                    {
                        ObjTax.DebitForeign = 0;
                        ObjTax.CreditDebitForeign = 0;
                    }
                    else
                    {
                        ObjTax.CreditDebitForeign = TotalTax / ExchangeRate;
                        ObjTax.DebitForeign = TotalTax / ExchangeRate;
                    }
                    ObjTax.Credit = 0;
                    ObjTax.CreditForeign = 0;
                    ObjTax.Exported = 0;
                    ObjTax.InsDateTime = DateTime.Now;
                    iRow = iRow + 1;
                    ObjTax.RowNumber = iRow;
                    ObjTax.VHI = HeaderTOSave.VHI;
                    ObjTax.VoucherDate = HeaderTOSave.VoucherDate;
                    ObjTax.VoucherNumber = HeaderTOSave.VoucherNumber;
                    _unitOfWork.Transaction.Add(ObjTax);
                }
                HeaderTOSave.RowCount = iRow;
                _unitOfWork.Header.Update(HeaderTOSave);
                iRow = 1;
                foreach (var Trans in TransActionServiceBill)
                {
                    Trans.BillID = HeaderTOSave.VHI;
                    Trans.CompanyID = HeaderTOSave.CompanyID;
                    Trans.CompanyYear = HeaderTOSave.CompanyYear;
                    Trans.iRowID = iRow;
                    Trans.CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo;
                    Trans.TransactionKindNo = HeaderTOSave.TransactionKindNo;
                    Trans.FCurrencyID = HeaderTOSave.FCurrencyID;
                    iRow = iRow + 1;
                    _unitOfWork.TransActionServiceBill.Add(Trans);
                }
                HeaderServiceBill.BillID = HeaderTOSave.VHI;
                HeaderServiceBill.CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo;
                HeaderServiceBill.TransactionKindNo = HeaderTOSave.TransactionKindNo;
                _unitOfWork.ServiceBillHeader.Add(HeaderServiceBill);
                _unitOfWork.Transaction.DeleteMarkeTrans(HeaderTOSave.CompanyID, HeaderTOSave.VoucherNumber, HeaderTOSave.CompanyTransactionKindNo, HeaderTOSave.TransactionKindNo, HeaderTOSave.CompanyYear);
                _unitOfWork.Complete();
                Msg.Code = 1;
                Msg.Msg = Resources.Resource.AddedSuccessfully;
                Msg.Year = HeaderTOSave.CompanyYear;
                Msg.FCompanyId = HeaderTOSave.CompanyID;
                Msg.VoucherNumber = HeaderTOSave.VoucherNumber.ToString();
                Msg.TransactionKindNo = HeaderTOSave.TransactionKindNo.ToString();
                Msg.CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo.ToString();
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _unitOfWork.Transaction.UnMarkDeleteTrans(HeaderTOSave.CompanyID, HeaderTOSave.VoucherNumber, HeaderTOSave.CompanyTransactionKindNo, HeaderTOSave.TransactionKindNo, HeaderTOSave.CompanyYear);
                _unitOfWork.Complete();
                Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                Msg.Code = 0;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult UpdateMultiReturnPurchaseServiceBill(TransServiceBillVM ObjSave)
        {
            MsgUnit Msg = new MsgUnit();
            var HeaderTOSave = new Header();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                int CurrYear = ObjSave.HeaderServiceBill.CompanyYear;
                var TransActionServiceBill = ObjSave.TransActionServiceBill;
                HeaderServiceBill HeaderServiceBill = new HeaderServiceBill();
                HeaderServiceBill.AccountNumber = ObjSave.HeaderServiceBill.AccountNumber;
                HeaderServiceBill.BillDate = ObjSave.HeaderServiceBill.BillDate;
                HeaderServiceBill.CompanyID = ObjSave.HeaderServiceBill.CompanyID;
                HeaderServiceBill.CompanyYear = ObjSave.HeaderServiceBill.CompanyYear;
                HeaderServiceBill.CreditAccountNumber = ObjSave.HeaderServiceBill.CreditAccountNumber;
                HeaderServiceBill.CreditCostNumber = ObjSave.HeaderServiceBill.CreditCostNumber;
                HeaderServiceBill.DebitAccountNumber = ObjSave.HeaderServiceBill.DebitAccountNumber;
                HeaderServiceBill.DebitCostNumber = ObjSave.HeaderServiceBill.DebitCostNumber;
                HeaderServiceBill.Discount = ObjSave.HeaderServiceBill.Discount;
                HeaderServiceBill.ConversionFactor = ObjSave.HeaderServiceBill.CurrencyNewValue;
                HeaderServiceBill.FCurrencyID = ObjSave.HeaderServiceBill.CurrencyID;
                HeaderServiceBill.NetTotal = ObjSave.HeaderServiceBill.NetTotal;
                HeaderServiceBill.NetTotalForeignAmount = ObjSave.HeaderServiceBill.NetTotalForeignAmount;
                HeaderServiceBill.ForeignAmount = ObjSave.HeaderServiceBill.ForeignAmount;
                HeaderServiceBill.ForeignAmountTax = ObjSave.HeaderServiceBill.ForeignAmountTax;
                HeaderServiceBill.NoTax = ObjSave.HeaderServiceBill.NoTax;
                HeaderServiceBill.Note = ObjSave.HeaderServiceBill.Note;
                HeaderServiceBill.TransactionKindNo = 22;
                HeaderServiceBill.CompanyTransactionKindNo = ObjSave.HeaderServiceBill.CompanyTransactionKindID;
                HeaderServiceBill.Tax = ObjSave.HeaderServiceBill.Tax;
                HeaderServiceBill.TaxAccountNumber = ObjSave.HeaderServiceBill.TaxAccountNumber;
                HeaderServiceBill.TaxCostNumber = ObjSave.HeaderServiceBill.TaxCostNumber;
                HeaderServiceBill.Total = ObjSave.HeaderServiceBill.Total;
                HeaderServiceBill.SaleManNo = ObjSave.HeaderServiceBill.SaleManNo;
                var TransactionKindNo = 22;
                var CompanyTransactionKindID = ObjSave.HeaderServiceBill.CompanyTransactionKindID;
                double LineTotals = 0;
                double Price = 0;
                double Qty = 0;
                double TaxPrec = 0;
                double Total = 0;
                double TotalTax = 0;
                double NetTotal = 0;
                double TotalDebit = 0;
                double TotalCredit = 0;
                double TotalDebitForeign = 0;
                double TotalCreditForeign = 0;
                int iRow = 1;
                var ExchangeRate = HeaderServiceBill.ConversionFactor;
                HeaderServiceBill.CompanyID = UserInfo.fCompanyId;
                HeaderServiceBill.CompanyYear = CurrYear;
                var AllAcc = _unitOfWork.NativeSql.GetTransActionAccount(UserInfo.fCompanyId);
                foreach (var Trans in TransActionServiceBill)
                {
                    Price = Trans.Price;
                    Qty = Trans.Qty;
                    TaxPrec = Trans.Tax / 100;
                    if (HeaderServiceBill.ConversionFactor == 0)
                    {
                        HeaderServiceBill.ConversionFactor = 1;
                    };
                    if (HeaderServiceBill.ConversionFactor == 1)
                    {
                        Trans.ForeignPrice = 0;
                    }
                    else
                    {
                        Trans.ForeignPrice = Trans.Price / ExchangeRate;
                    }

                    Trans.PriceAfterLineDisc = Trans.Price - Trans.LocalDiscount;
                    Trans.ConversionFactor = ExchangeRate;
                    if (HeaderServiceBill.NoTax)
                    {
                        Trans.TaxValue = 0;

                    }
                    else
                    {
                        Trans.TaxValue = Trans.PriceAfterLineDisc * TaxPrec;
                    }
                    Trans.TotalLocal = Trans.PriceAfterLineDisc * Trans.Qty;
                    if (Trans.ForeignPrice > 0)
                    {
                        Trans.ForeignPriceAfterLineDisc = (Trans.Price / ExchangeRate) - Trans.LocalDiscount;
                        Trans.TotalForeign = Trans.ForeignPriceAfterLineDisc * Trans.Qty;
                    }
                    LineTotals = LineTotals + Trans.TotalLocal;
                }
                var TotalDiscPrec = HeaderServiceBill.Discount / LineTotals;
                foreach (var Trans in TransActionServiceBill)
                {
                    TaxPrec = Trans.Tax / 100;
                    Trans.TotalDiscPrec = TotalDiscPrec;
                    Trans.TotalDiscValue = TotalDiscPrec * Trans.TotalLocal;
                    Trans.NetAfterTotalDisc = Trans.TotalLocal - Trans.TotalDiscValue;
                    if (HeaderServiceBill.NoTax)
                    {
                        Trans.NetTotalTax = 0;
                    }
                    else
                    {
                        Trans.NetTotalTax = TaxPrec * Trans.NetAfterTotalDisc;
                    }
                    Total = Total + Trans.NetAfterTotalDisc;
                    TotalTax = TotalTax + Trans.NetTotalTax;
                }
                NetTotal = Total + TotalTax;
                TotalDebit = NetTotal;
                TotalCredit = Total + TotalTax;
                HeaderServiceBill.NetTotal = NetTotal;
                if (HeaderServiceBill.ConversionFactor == 0)
                {
                    HeaderServiceBill.ConversionFactor = 1;
                };
                if (HeaderServiceBill.ConversionFactor == 1)
                {
                    TotalDebitForeign = 0;
                    TotalCreditForeign = 0;
                }
                else
                {
                    TotalDebitForeign = Total / ExchangeRate;
                    TotalCreditForeign = (HeaderServiceBill.NetTotal) / ExchangeRate;
                }

                if (TotalCredit != TotalDebit)
                {
                    Msg.Msg = Resources.Resource.TheAccountingEntryIsUneven;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }
                if (TotalCredit <= 0)
                {
                    Msg.Msg = Resources.Resource.CantBeZero;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }

                HeaderTOSave.TransactionKindNo = TransactionKindNo;
                HeaderTOSave.CompanyTransactionKindNo = CompanyTransactionKindID;
                HeaderTOSave.InsDateTime = DateTime.Now;
                HeaderTOSave.InsUserID = userId;
                HeaderTOSave.CompanyID = UserInfo.fCompanyId;
                HeaderTOSave.TotalCredit = TotalCredit;
                HeaderTOSave.TotalDebit = TotalDebit;
                HeaderTOSave.TotalDebitForeign = TotalCreditForeign;
                HeaderTOSave.TotalCreditForeign = TotalCreditForeign;
                HeaderTOSave.CompanyYear = CurrYear;
                HeaderTOSave.VoucherNumber = ObjSave.HeaderServiceBill.BillID.ToString();
                HeaderTOSave.VHI = int.Parse(HeaderTOSave.VoucherNumber);
                HeaderTOSave.VoucherDate = HeaderServiceBill.BillDate;
                HeaderTOSave.FCurrencyID = HeaderServiceBill.FCurrencyID;
                HeaderTOSave.ConversionFactor = HeaderServiceBill.ConversionFactor;
                HeaderTOSave.SaleID = HeaderServiceBill.SaleManNo;

                _unitOfWork.ServiceBillHeader.Delete(HeaderTOSave.CompanyID, HeaderTOSave.VHI, HeaderServiceBill.CompanyTransactionKindNo, HeaderServiceBill.TransactionKindNo, HeaderTOSave.CompanyYear);
                _unitOfWork.TransActionServiceBill.Delete(HeaderTOSave.CompanyID, HeaderTOSave.VHI, HeaderServiceBill.CompanyTransactionKindNo, HeaderServiceBill.TransactionKindNo, HeaderTOSave.CompanyYear);
                _unitOfWork.Transaction.MarkDeleteTrans(HeaderTOSave.CompanyID, HeaderTOSave.VoucherNumber, HeaderTOSave.CompanyTransactionKindNo, HeaderTOSave.TransactionKindNo, HeaderTOSave.CompanyYear);
                _unitOfWork.Complete();

                iRow = 0;
                Transaction ObjDebit = new Transaction();
                ObjDebit.AccountNumber = HeaderServiceBill.DebitAccountNumber;
                ObjDebit.CostCenter = HeaderServiceBill.DebitCostNumber;
                ObjDebit.CompanyTransactionKindNo = CompanyTransactionKindID;
                ObjDebit.TransactionKindNo = TransactionKindNo;
                ObjDebit.CompanyYear = HeaderTOSave.CompanyYear;
                ObjDebit.CompanyID = HeaderTOSave.CompanyID;
                ObjDebit.Credit = 0;
                ObjDebit.CreditForeign = 0;
                ObjDebit.Debit = TotalDebit;
                if (HeaderServiceBill.ConversionFactor == 0)
                {
                    HeaderServiceBill.ConversionFactor = 1;
                };
                if (HeaderServiceBill.ConversionFactor == 1)
                {
                    ObjDebit.DebitForeign = 0;
                    ObjDebit.CreditDebitForeign = 0;
                }
                else
                {
                    ObjDebit.DebitForeign = TotalDebit / ExchangeRate;
                    ObjDebit.CreditDebitForeign = TotalDebit / ExchangeRate;
                }
                ObjDebit.Exported = 0;
                ObjDebit.InsDateTime = DateTime.Now;
                ObjDebit.RowNumber = 1;
                ObjDebit.VHI = HeaderTOSave.VHI;
                ObjDebit.VoucherDate = HeaderTOSave.VoucherDate;
                ObjDebit.VoucherNumber = HeaderTOSave.VoucherNumber;
                _unitOfWork.Transaction.Add(ObjDebit);
                iRow = 1;
                foreach (var Trans in TransActionServiceBill)
                {
                    Transaction ObjCredit = new Transaction();
                    iRow = iRow + 1;
                    ObjCredit.CompanyTransactionKindNo = CompanyTransactionKindID;
                    ObjCredit.TransactionKindNo = TransactionKindNo;
                    ObjCredit.CompanyYear = HeaderTOSave.CompanyYear;
                    ObjCredit.CompanyID = HeaderTOSave.CompanyID;
                    ObjCredit.Credit = Trans.NetAfterTotalDisc;
                    if (HeaderServiceBill.ConversionFactor == 0)
                    {
                        HeaderServiceBill.ConversionFactor = 1;
                    };
                    if (HeaderServiceBill.ConversionFactor == 1)
                    {
                        ObjCredit.CreditForeign = 0;
                        ObjCredit.CreditDebitForeign = 0;
                    }
                    else
                    {
                        ObjCredit.CreditDebitForeign = ObjCredit.Credit / ExchangeRate;
                        ObjCredit.CreditForeign = ObjCredit.Credit / ExchangeRate;
                    }
                    ObjCredit.Debit = 0;
                    ObjCredit.DebitForeign = 0;
                    ObjCredit.Exported = 0;
                    ObjCredit.InsDateTime = DateTime.Now;
                    ObjCredit.RowNumber = iRow;
                    ObjCredit.VHI = HeaderTOSave.VHI;
                    ObjCredit.VoucherDate = HeaderTOSave.VoucherDate;
                    ObjCredit.VoucherNumber = HeaderTOSave.VoucherNumber;
                    _unitOfWork.Transaction.Add(ObjCredit);
                }
                if (!HeaderServiceBill.NoTax)
                {
                    Transaction ObjTax = new Transaction();
                    ObjTax.AccountNumber = HeaderServiceBill.TaxAccountNumber;
                    ObjTax.CostCenter = HeaderServiceBill.TaxCostNumber;
                    ObjTax.CompanyTransactionKindNo = CompanyTransactionKindID;
                    ObjTax.TransactionKindNo = TransactionKindNo;
                    ObjTax.CompanyYear = HeaderTOSave.CompanyYear;
                    ObjTax.CompanyID = HeaderTOSave.CompanyID;
                    ObjTax.Credit = TotalTax;
                    if (HeaderServiceBill.ConversionFactor == 0)
                    {
                        HeaderServiceBill.ConversionFactor = 1;
                    };
                    if (HeaderServiceBill.ConversionFactor == 1)
                    {
                        ObjTax.CreditForeign = 0;
                        ObjTax.CreditDebitForeign = 0;
                    }
                    else
                    {
                        ObjTax.CreditDebitForeign = TotalTax / ExchangeRate;
                        ObjTax.CreditForeign = TotalTax / ExchangeRate;
                    }
                    ObjTax.Debit = 0;
                    ObjTax.DebitForeign = 0;
                    ObjTax.Exported = 0;
                    ObjTax.InsDateTime = DateTime.Now;
                    iRow = iRow + 1;
                    ObjTax.RowNumber = iRow;
                    ObjTax.VHI = HeaderTOSave.VHI;
                    ObjTax.VoucherDate = HeaderTOSave.VoucherDate;
                    ObjTax.VoucherNumber = HeaderTOSave.VoucherNumber;
                    _unitOfWork.Transaction.Add(ObjTax);
                }
                HeaderTOSave.RowCount = iRow;
                _unitOfWork.Header.Update(HeaderTOSave);
                iRow = 1;
                foreach (var Trans in TransActionServiceBill)
                {
                    Trans.BillID = HeaderTOSave.VHI;
                    Trans.CompanyID = HeaderTOSave.CompanyID;
                    Trans.CompanyYear = HeaderTOSave.CompanyYear;
                    Trans.iRowID = iRow;
                    Trans.CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo;
                    Trans.TransactionKindNo = HeaderTOSave.TransactionKindNo;
                    Trans.FCurrencyID = HeaderTOSave.FCurrencyID;
                    iRow = iRow + 1;
                    _unitOfWork.TransActionServiceBill.Add(Trans);
                }
                HeaderServiceBill.BillID = HeaderTOSave.VHI;
                HeaderServiceBill.CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo;
                HeaderServiceBill.TransactionKindNo = HeaderTOSave.TransactionKindNo;
                _unitOfWork.ServiceBillHeader.Add(HeaderServiceBill);
                _unitOfWork.Transaction.DeleteMarkeTrans(HeaderTOSave.CompanyID, HeaderTOSave.VoucherNumber, HeaderTOSave.CompanyTransactionKindNo, HeaderTOSave.TransactionKindNo, HeaderTOSave.CompanyYear);
                _unitOfWork.Complete();
                Msg.Code = 1;
                Msg.Msg = Resources.Resource.AddedSuccessfully;
                Msg.Year = HeaderTOSave.CompanyYear;
                Msg.FCompanyId = HeaderTOSave.CompanyID;
                Msg.VoucherNumber = HeaderTOSave.VoucherNumber.ToString();
                Msg.TransactionKindNo = HeaderTOSave.TransactionKindNo.ToString();
                Msg.CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo.ToString();
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _unitOfWork.Transaction.UnMarkDeleteTrans(HeaderTOSave.CompanyID, HeaderTOSave.VoucherNumber, HeaderTOSave.CompanyTransactionKindNo, HeaderTOSave.TransactionKindNo, HeaderTOSave.CompanyYear);
                _unitOfWork.Complete();
                Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                Msg.Code = 0;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult DeleteServiceBill(HeaderServiceBillVM ObjSave)
        {
            MsgUnit Msg = new MsgUnit();
            var HeaderTOSave = new Header();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                int CurrYear = ObjSave.CompanyYear;
                _unitOfWork.ServiceBillHeader.Delete(UserInfo.fCompanyId, ObjSave.BillID, ObjSave.CompanyTransactionKindID, ObjSave.TransactionKindNo, CurrYear);
                _unitOfWork.TransActionServiceBill.Delete(UserInfo.fCompanyId, ObjSave.BillID, ObjSave.CompanyTransactionKindID, ObjSave.TransactionKindNo, CurrYear);
                HeaderTOSave.CompanyID = UserInfo.fCompanyId;
                HeaderTOSave.CompanyTransactionKindNo = ObjSave.CompanyTransactionKindID;
                HeaderTOSave.TransactionKindNo = ObjSave.TransactionKindNo;
                HeaderTOSave.VoucherNumber = ObjSave.BillID.ToString();
                HeaderTOSave.CompanyYear = CurrYear;
                TransActionDeleteVM ObjDelete = new TransActionDeleteVM
                {
                    CompanyID = HeaderTOSave.CompanyID,
                    CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo,
                    TransactionKindNo = HeaderTOSave.TransactionKindNo,
                    VoucherNumber = HeaderTOSave.VoucherNumber,
                    CompanyYear = HeaderTOSave.CompanyYear
                };
                _unitOfWork.Header.Delete(ObjDelete);
                _unitOfWork.NativeSql.DeleteTransActionTrans(HeaderTOSave.CompanyID, HeaderTOSave.VoucherNumber,
                 HeaderTOSave.CompanyTransactionKindNo, HeaderTOSave.TransactionKindNo, HeaderTOSave.CompanyYear);
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
        [ValidateInput(false)]
        public ActionResult ServiceGridViewPartial(int?id,int? id2,int?id3 , int id4)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            IEnumerable<TransActionServiceBillVM> Data = new List<TransActionServiceBillVM>();
            if (id > 0)
            {
                 Data = _unitOfWork.NativeSql.GetTransActionServiceBillsData(UserInfo.fCompanyId,int.Parse(id.ToString()), int.Parse(id2.ToString()), int.Parse(id3.ToString()),id4);

            }
            else
            {

            }
           
            return PartialView("_ServiceGridViewPartial", Data);
        }
        [ValidateInput(false)]
        public ActionResult MultiServiceGridViewPartial(int? id, int? id2, int? id3 , int id4)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            ViewBag.WorkWithCostCenter = Company.WorkWithCostCenter;
            IEnumerable<TransActionServiceBillVM> Data = new List<TransActionServiceBillVM>();
            if (id > 0)
            {
                Data = _unitOfWork.NativeSql.GetTransActionServiceBillsData(UserInfo.fCompanyId, int.Parse(id.ToString()), int.Parse(id2.ToString()), int.Parse(id3.ToString()),id4);

            }
            else
            {

            }
            return PartialView("_MultiServiceGridViewPartial", Data);
        }
        [HttpGet]
        public ActionResult GetServiceDetails(int id, int id2, int id3, int id4)
        {
            var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllServiceDetails = _unitOfWork.NativeSql.GetTransActionServiceBillsData(UserInfo.fCompanyId, id, id2, id3, id4);

            if (AllServiceDetails == null)
                {
                    return Json(new TransActionServiceBillVM(), JsonRequestBehavior.AllowGet);
                }
                return Json(AllServiceDetails, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "CoOwner,ExportServiceBill")]
        public ActionResult Export()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetCompanyTransactionKindAll(UserInfo.fCompanyId);
            CompanyTransactionKindObj = CompanyTransactionKindObj.Where(m => m.TransactionKindID == 10 || m.TransactionKindID == 11 || m.TransactionKindID == 12 || m.TransactionKindID == 13
            || m.TransactionKindID == 19 || m.TransactionKindID == 20 || m.TransactionKindID == 21 || m.TransactionKindID == 22).ToList();
            int year = DateTime.Now.Year;
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
            var ServiceBillFilter = new ServiceBillFilterVM
            {
                FromDate = FromDate,
                ToDate = ToDate,
                CompanyTransactionKind = CompanyTransactionKindObj,
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency
        };
            return View(ServiceBillFilter);
        }
        [HttpPost]
        public JsonResult GetAllServiceBillUnExport(ReceiptCashFilterVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllServiceBill = _unitOfWork.NativeSql.GetAllServiceBillUnExport(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                if (AllServiceBill == null)
                {
                    return Json(new List<ServiceBillFilterVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.VoucherNumber))
                {
                    AllServiceBill = AllServiceBill.Where(m => m.VoucherNumber == Obj.VoucherNumber).ToList();
                }
                if (Obj.CurrencyID != 0)
                {
                    AllServiceBill = AllServiceBill.Where(m => m.CurrencyID == Obj.CurrencyID).ToList();
                }
                if (Obj.CompanyTransactionKindNo != 0)
                {
                    AllServiceBill = AllServiceBill.Where(m => m.CompanyTransactionKindNo == Obj.CompanyTransactionKindNo).ToList();
                }
                int iRow = 0;
                foreach (var iRowCount in AllServiceBill)
                {
                    iRowCount.iRowTable = iRow;
                    iRow = iRow + 1;
                }
                return Json(AllServiceBill, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<ServiceBillFilterVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [Authorize(Roles = "CoOwner,UnExportServiceBill")]
        public ActionResult UnExport()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetCompanyTransactionKindAll(UserInfo.fCompanyId);
            CompanyTransactionKindObj = CompanyTransactionKindObj.Where(m => m.TransactionKindID == 10 || m.TransactionKindID == 11 || m.TransactionKindID == 12 || m.TransactionKindID == 13
            || m.TransactionKindID == 19 || m.TransactionKindID == 20 || m.TransactionKindID == 21 || m.TransactionKindID == 22).ToList();
            int year = DateTime.Now.Year;
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
            var ServiceBillFilter = new ServiceBillFilterVM
            {
                FromDate = FromDate,
                ToDate = ToDate,
                CompanyTransactionKind = CompanyTransactionKindObj,
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency
            };
            return View(ServiceBillFilter);
        }
        [HttpPost]
        public JsonResult GetAllServiceBillExport(ReceiptCashFilterVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllServiceBill = _unitOfWork.NativeSql.GetAllServiceBillExport(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                if (AllServiceBill == null)
                {
                    return Json(new List<ServiceBillFilterVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.VoucherNumber))
                {
                    AllServiceBill = AllServiceBill.Where(m => m.VoucherNumber == Obj.VoucherNumber).ToList();
                }
                if (Obj.CurrencyID != 0)
                {
                    AllServiceBill = AllServiceBill.Where(m => m.CurrencyID == Obj.CurrencyID).ToList();
                }
                if (Obj.CompanyTransactionKindNo != 0)
                {
                    AllServiceBill = AllServiceBill.Where(m => m.CompanyTransactionKindNo == Obj.CompanyTransactionKindNo).ToList();
                }
                int iRow = 0;
                foreach (var iRowCount in AllServiceBill)
                {
                    iRowCount.iRowTable = iRow;
                    iRow = iRow + 1;
                }
                return Json(AllServiceBill, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<ServiceBillFilterVM>(), JsonRequestBehavior.AllowGet);
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