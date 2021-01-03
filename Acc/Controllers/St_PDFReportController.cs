using Acc.DevReport;
using Acc.Helpers;
using Acc.Models;
using Acc.Persistence;
using Acc.Repositories;
using Acc.ViewModels;
using DevExpress.DataProcessing;
using DevExpress.Utils.DPI;
using DevExpress.Utils.StructuredStorage.Internal.Reader;
using DevExpress.Web;
using DevExpress.XtraReports.UI;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Acc.Controllers
{
    [Authorize]

    public class St_PDFReportController : BaseController
    {

        private readonly IUnitOfWork _unitOfWork;

        public St_PDFReportController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult PurchaseOrderLocalDetailPdf(string id, string id2, string id3, string id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var HeaderSt_PurchaseOrderVM = _unitOfWork.St_PurchaseOrder.GetSt_PurchaseOrderHeaderDataById(id, UserInfo.fCompanyId, int.Parse(id2), int.Parse(id3), int.Parse(id4));
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var AllItems = _unitOfWork.NativeSql.GetSt_PurchaseOrderTransactionLocal(id, int.Parse(id2), UserInfo.fCompanyId, int.Parse(id3), int.Parse(id4));
            var St_PurchaseOrderVM = new St_PurchaseOrderVM { };
            St_PurchaseOrderVM.CompanyTransactionKindNo = HeaderSt_PurchaseOrderVM.CompanyTransactionKindNo;
            St_PurchaseOrderVM.TransactionKindNo = HeaderSt_PurchaseOrderVM.TransactionKindNo;
            St_PurchaseOrderVM.CompanyYear = HeaderSt_PurchaseOrderVM.CompanyYear;
            St_PurchaseOrderVM.TaxType = HeaderSt_PurchaseOrderVM.TaxType;
            St_PurchaseOrderVM.VoucherCase = HeaderSt_PurchaseOrderVM.VoucherCase;
            St_PurchaseOrderVM.VoucherDate = HeaderSt_PurchaseOrderVM.VoucherDate;
            St_PurchaseOrderVM.CurrencyID = HeaderSt_PurchaseOrderVM.CurrencyID;
            St_PurchaseOrderVM.ConversionFactor = HeaderSt_PurchaseOrderVM.ConversionFactor;
            St_PurchaseOrderVM.SupplierAccountNumber = HeaderSt_PurchaseOrderVM.SupplierAccountNumber;
            St_PurchaseOrderVM.TotalQuantity = HeaderSt_PurchaseOrderVM.TotalQuantity;
            St_PurchaseOrderVM.NetTotal = HeaderSt_PurchaseOrderVM.NetTotal;
            St_PurchaseOrderVM.SupplierAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, HeaderSt_PurchaseOrderVM.SupplierAccountNumber);
            St_PurchaseOrderVM.Remark = HeaderSt_PurchaseOrderVM.Remark;
            St_PurchaseOrderVM.Hint = HeaderSt_PurchaseOrderVM.Hint;
            St_PurchaseOrderVM.VoucherNumber = HeaderSt_PurchaseOrderVM.VoucherNumber;
            St_PurchaseOrderVM.VHI = HeaderSt_PurchaseOrderVM.VHI;
            St_PurchaseOrderVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;

            St_PurchaseOrderLocalDetail report = new St_PurchaseOrderLocalDetail();

            // cokin 
            string reportFilePath = Server.MapPath("/St_ReportLayout/St_PurchaseOrderLocalDetail.repx");
            report.LoadLayoutFromXml(reportFilePath);



            DataSet ds = new DataSet();
            DataTable table1 = new DataTable("St_PurchaseOrderVM");
            table1 = FunctionUnit.LINQResultToDataTable(AllItems);

            ds.Tables.Add(table1);

            report.DataSource = ds;
            report.DataMember = "St_PurchaseOrderVM";


            report.LbCoName.Text = CoInfo.ArabicName;
            report.LbVhDate.Text = St_PurchaseOrderVM.VoucherDate.ToString("dd/MM/yyyy");
            report.LbVhNo.Text = St_PurchaseOrderVM.VoucherNumber.ToString();

            if (St_PurchaseOrderVM.VoucherCase == 1)
            {
                report.VoucherCase.Text = Resources.Resource.Cash;
            }
            else
            {
                report.VoucherCase.Text = Resources.Resource.VoucherCredit;

            }


            if (St_PurchaseOrderVM.TaxType == 1)
            {
                report.TaxType.Text = Resources.Resource.Taxable;


            }
            else if (St_PurchaseOrderVM.TaxType == 2)
            {
                report.TaxType.Text = Resources.Resource.TaxableByZero;

            }
            else
            {
                report.TaxType.Text = Resources.Resource.TaxExempt;

            }

            report.SupplierNo.Text = St_PurchaseOrderVM.SupplierAccountNumber;
            report.SupplierName.Text = St_PurchaseOrderVM.SupplierAccountName;

            report.Remark.Text = St_PurchaseOrderVM.Remark;
            report.Hint.Text = St_PurchaseOrderVM.Hint;


            report.Qty.Text = AllItems.Sum(m => m.Quantity).ToString();
            St_PurchaseOrderVM.NetTotal = AllItems.Sum(m => (m.Total));

            if (CoInfo.TheDecimalPointForTheLocalCurrency == 2)
            {
                report.Net.Text = "" + String.Format("{0:n2}", St_PurchaseOrderVM.NetTotal) + "";

            }
            else
            {
                report.Net.Text = "" + String.Format("{0:n3}", St_PurchaseOrderVM.NetTotal) + "";
            }
            report.LbPrintTime.Text = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, FunctionUnit.Jordan_Time_Zone).ToString();

            report.CreateDocument();



            return View("ReportPDF", report);
        }


        public ActionResult GetAllSt_PurchaseOrderPDF(string id, string id2, string id3, string id4, int id5, int id6, int id7)
        {
            try
            {
                St_PurchaseOrderVM Obj = new St_PurchaseOrderVM();
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);

                DateTime fromDate = DateTime.Parse(id.Substring(0, 2) + "/" + id.Substring(2, 2) + "/" + id.Substring(4, 4));
                DateTime toDate = DateTime.Parse(id2.Substring(0, 2) + "/" + id2.Substring(2, 2) + "/" + id2.Substring(4, 4));
                Obj.FromDate = fromDate;
                Obj.ToDate = toDate;
                Obj.VoucherNumber = id3;
                Obj.SupplierAccountNumber = id4;
                Obj.LinkWithVoucher = id5;
                Obj.TaxType = id6;
                Obj.VoucherCase = id7;


                if (Obj.VoucherNumber == "0")
                {
                    Obj.VoucherNumber = "";
                }
                if (Obj.SupplierAccountNumber == "0")
                {
                    Obj.SupplierAccountNumber = "";
                }
                var AllGetAllSt_PurchaseOrder = _unitOfWork.NativeSql.GetAllGetAllSt_PurchaseOrder(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                if (AllGetAllSt_PurchaseOrder == null)
                {
                    return Json(new List<St_PurchaseOrderVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.VoucherNumber))
                {
                    AllGetAllSt_PurchaseOrder = AllGetAllSt_PurchaseOrder.Where(m => m.VoucherNumber == Obj.VoucherNumber).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.SupplierAccountNumber))
                {
                    AllGetAllSt_PurchaseOrder = AllGetAllSt_PurchaseOrder.Where(m => m.SupplierAccountNumber == Obj.SupplierAccountNumber).ToList();
                }
                if (Obj.TaxType != 0)
                {
                    AllGetAllSt_PurchaseOrder = AllGetAllSt_PurchaseOrder.Where(m => m.TaxType == Obj.TaxType).ToList();
                }
                if (Obj.VoucherCase != 0)
                {
                    AllGetAllSt_PurchaseOrder = AllGetAllSt_PurchaseOrder.Where(m => m.VoucherCase == Obj.VoucherCase).ToList();
                }
                if (Obj.LinkWithVoucher != 0)
                {
                    AllGetAllSt_PurchaseOrder = AllGetAllSt_PurchaseOrder.Where(m => m.LinkWithVoucher == Obj.LinkWithVoucher).ToList();
                }

                DataSet ds = new DataSet();
                DataTable table1 = new DataTable("St_PurchaseOrderVM");
                table1 = FunctionUnit.LINQResultToDataTable(AllGetAllSt_PurchaseOrder);

                ds.Tables.Add(table1);

                var report = new St_PurchaseOrderReport();

                string reportFilePath = Server.MapPath("/St_ReportLayout/St_PurchaseOrderReport.repx");
                report.LoadLayoutFromXml(reportFilePath);

                report.LbCoName.Text = CoInfo.ArabicName;


                report.DataSource = ds;
                report.DataMember = "St_PurchaseOrderVM";

                report.LbFromDate.Text = Obj.FromDate.ToString("dd/MM/yyyy");
                report.LbTODate.Text = Obj.ToDate.ToString("dd/MM/yyyy");
                report.LbVoucherNo.Text = Obj.VoucherNumber;

                if (Obj.VoucherCase != 0)
                {
                    if (Obj.VoucherCase == 1)
                    {
                        report.VoucherCase.Text = Resources.Resource.Cash;
                    }
                    else
                    {
                        report.VoucherCase.Text = Resources.Resource.VoucherCredit;

                    }

                }



                if (Obj.TaxType != 0)
                {
                    if (Obj.TaxType == 1)
                    {
                        report.TaxType.Text = Resources.Resource.Taxable;


                    }
                    else if (Obj.TaxType == 2)
                    {
                        report.TaxType.Text = Resources.Resource.TaxableByZero;

                    }
                    else
                    {
                        report.TaxType.Text = Resources.Resource.TaxExempt;

                    }

                }



                if (Obj.LinkWithVoucher != -1)
                {
                    if (Obj.LinkWithVoucher == 1)
                    {
                        report.LinkCase.Text = Resources.Resource.Related;
                    }
                    else
                    {
                        report.LinkCase.Text = Resources.Resource.NotRelated;

                    }

                }



                //report.LevelName.Text = String.Format("{0} {1}", Resources.Resource.Level, Obj.AccountLevelDropVMID.ToString());




                report.ToAccountNumber.Text = Obj.SupplierAccountNumber;


                if (!String.IsNullOrEmpty(Obj.SupplierAccountNumber))
                {
                    var AccName = _unitOfWork.ChartOfAccount.GetAccountByID(UserInfo.fCompanyId, Obj.SupplierAccountNumber);
                    if (Resources.Resource.CurLang == "Arb")
                    {
                        report.AccountName.Text = AccName.ArabicName;
                    }
                    else
                    {
                        report.AccountName.Text = AccName.EnglishName;
                    }

                }


                report.CreateDocument();




                return View("ReportPDF", report);
            }
            catch (Exception ex)
            {
                string err = ex.Message;

                List<St_PurchaseOrderVM> VMList = new List<St_PurchaseOrderVM>();

                St_PurchaseOrderReport report = new St_PurchaseOrderReport();

                string reportFilePath = Server.MapPath("/St_ReportLayout/St_PurchaseOrderReport.repx");
                report.LoadLayoutFromXml(reportFilePath);

                DataSet ds = new DataSet();
                DataTable table1 = new DataTable("St_PurchaseOrderVM");
                table1 = FunctionUnit.LINQResultToDataTable(VMList);

                ds.Tables.Add(table1);




                report.DataSource = ds;
                report.DataMember = "St_PurchaseOrderVM";
                // report.LbPrintTime.Text = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, FunctionUnit.Jordan_Time_Zone).ToString();

                report.CreateDocument();

                return View("ReportPDF", report);

            }

        }


        public ActionResult GetAllSt_PurchaseVoucherPDF(string id, string id2, string id3, string id4, string id5, int id6, int id7)
        {
            try
            {
                St_HeaderVM Obj = new St_HeaderVM();
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);

                DateTime fromDate = DateTime.Parse(id.Substring(0, 2) + "/" + id.Substring(2, 2) + "/" + id.Substring(4, 4));
                DateTime toDate = DateTime.Parse(id2.Substring(0, 2) + "/" + id2.Substring(2, 2) + "/" + id2.Substring(4, 4));
                Obj.FromDate = fromDate;
                Obj.ToDate = toDate;
                Obj.VoucherNumber = id3;
                Obj.AccountNumber = id4;
                Obj.StockCode = id5;
                Obj.TaxType = id6;
                Obj.VoucherCase = id7;


                if (Obj.VoucherNumber == "0")
                {
                    Obj.VoucherNumber = "";
                }
                if (Obj.AccountNumber == "0")
                {
                    Obj.AccountNumber = "";
                }

                if (Obj.StockCode == "0")
                {
                    Obj.StockCode = "";
                }
                var AllSt_PurchaseVoucher = _unitOfWork.NativeSql.GetAllSt_PurchaseVoucher(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                if (AllSt_PurchaseVoucher == null)
                {
                    return Json(new List<St_HeaderVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.VoucherNumber))
                {
                    AllSt_PurchaseVoucher = AllSt_PurchaseVoucher.Where(m => m.VoucherNumber == Obj.VoucherNumber).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.AccountNumber))
                {
                    AllSt_PurchaseVoucher = AllSt_PurchaseVoucher.Where(m => m.AccountNumber == Obj.AccountNumber).ToList();
                }
                if (Obj.TaxType != 0)
                {
                    AllSt_PurchaseVoucher = AllSt_PurchaseVoucher.Where(m => m.TaxType == Obj.TaxType).ToList();
                }
                if (Obj.VoucherCase != 0)
                {
                    AllSt_PurchaseVoucher = AllSt_PurchaseVoucher.Where(m => m.VoucherCase == Obj.VoucherCase).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.StockCode))
                {
                    AllSt_PurchaseVoucher = AllSt_PurchaseVoucher.Where(m => m.StockCode == Obj.StockCode).ToList();
                }
                DataSet ds = new DataSet();
                DataTable table1 = new DataTable("St_HeaderVM");
                table1 = FunctionUnit.LINQResultToDataTable(AllSt_PurchaseVoucher);

                ds.Tables.Add(table1);

                var report = new St_PurchaseVoucherReport();

                string reportFilePath = Server.MapPath("/St_ReportLayout/St_PurchaseVoucherReport.repx");
                report.LoadLayoutFromXml(reportFilePath);

                report.LbCoName.Text = CoInfo.ArabicName;


                report.DataSource = ds;
                report.DataMember = "St_HeaderVM";

                report.LbFromDate.Text = Obj.FromDate.ToString("dd/MM/yyyy");
                report.LbTODate.Text = Obj.ToDate.ToString("dd/MM/yyyy");
                report.LbVoucherNo.Text = Obj.VoucherNumber;

                if (Obj.VoucherCase != 0)
                {
                    if (Obj.VoucherCase == 1)
                    {
                        report.VoucherCase.Text = Resources.Resource.Cash;
                    }
                    else
                    {
                        report.VoucherCase.Text = Resources.Resource.VoucherCredit;

                    }

                }

                if (Obj.TaxType != 0)
                {
                    if (Obj.TaxType == 1)
                    {
                        report.TaxType.Text = Resources.Resource.Taxable;


                    }
                    else if (Obj.TaxType == 2)
                    {
                        report.TaxType.Text = Resources.Resource.TaxableByZero;

                    }
                    else
                    {
                        report.TaxType.Text = Resources.Resource.TaxExempt;

                    }

                }

                report.StockCase.Text = Obj.StockCode;

                if (!String.IsNullOrEmpty(Obj.StockCode))
                {
                    var AccName = _unitOfWork.St_Warehouse.GetSt_WarehouseByID(UserInfo.fCompanyId, Obj.StockCode);
                    if (Resources.Resource.CurLang == "Arb")
                    {
                        report.StockCase.Text = AccName.ArabicName;
                    }
                    else
                    {
                        report.StockCase.Text = AccName.EnglishName;
                    }

                }



                report.ToAccountNumber.Text = Obj.AccountNumber;


                if (!String.IsNullOrEmpty(Obj.AccountNumber))
                {
                    var AccName = _unitOfWork.ChartOfAccount.GetAccountByID(UserInfo.fCompanyId, Obj.AccountNumber);
                    if (Resources.Resource.CurLang == "Arb")
                    {
                        report.AccountName.Text = AccName.ArabicName;
                    }
                    else
                    {
                        report.AccountName.Text = AccName.EnglishName;
                    }

                }


                report.CreateDocument();


                return View("ReportPDF", report);
            }
            catch (Exception ex)
            {
                string err = ex.Message;

                List<St_HeaderVM> VMList = new List<St_HeaderVM>();

                St_PurchaseVoucherReport report = new St_PurchaseVoucherReport();

                string reportFilePath = Server.MapPath("/St_ReportLayout/St_PurchaseVoucherReport.repx");
                report.LoadLayoutFromXml(reportFilePath);

                DataSet ds = new DataSet();
                DataTable table1 = new DataTable("St_HeaderVM");
                table1 = FunctionUnit.LINQResultToDataTable(VMList);

                ds.Tables.Add(table1);




                report.DataSource = ds;
                report.DataMember = "St_HeaderVM";
                // report.LbPrintTime.Text = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, FunctionUnit.Jordan_Time_Zone).ToString();

                report.CreateDocument();

                return View("ReportPDF", report);

            }

        }

        public ActionResult St_PurchaseVoucherDetailPDF(string id, string id2, string id3, string id4, string id5)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var St_Warehouse = _unitOfWork.St_Warehouse.GetAllSt_Warehouse(UserInfo.fCompanyId);
            var St_HeaderObj = _unitOfWork.St_Header.GetSt_HeaderDataById(id, UserInfo.fCompanyId, int.Parse(id2), int.Parse(id3), int.Parse(id4), id5);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var HeaderObj = _unitOfWork.Header.GetHeaderDataById(id, UserInfo.fCompanyId, int.Parse(id2), int.Parse(id3), int.Parse(id4));
            var AllItems = _unitOfWork.NativeSql.GetSt_Transacation(id, int.Parse(id2), UserInfo.fCompanyId, int.Parse(id3), int.Parse(id4), id5);
            var TransactionCreditObj = _unitOfWork.NativeSql.GetTransactionsDetailCredit(id, UserInfo.fCompanyId, int.Parse(id2), 1, int.Parse(id4));
            var TransactionDebitObj = _unitOfWork.NativeSql.GetTransactionsDetailDebit(id, UserInfo.fCompanyId, int.Parse(id2), 2, int.Parse(id4));
            var TransactionTaxObj = _unitOfWork.NativeSql.GetTransactionsDetailTax(id, UserInfo.fCompanyId, int.Parse(id2), 3, int.Parse(id4));
            var ObjGet = _unitOfWork.St_OtherAccount.GetSt_OtherAccountByID(UserInfo.fCompanyId);
            FixAccountInfoVM ObjFix = new FixAccountInfoVM
            {
                AccountNo = "",
                AccountName = ""
            };
            if (ObjGet != null)
            {
                ObjFix.AccountNo = ObjGet.PurchaseTaxAccountNumber;
                ObjFix.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.PurchaseTaxAccountNumber);
            }
            var St_HeaderVM = new St_HeaderVM { };
            St_HeaderVM.St_Warehouse = St_Warehouse;
            St_HeaderVM.StockCode = St_HeaderObj.StockCode;
            St_HeaderVM.CompanyTransactionKindNo = St_HeaderObj.CompanyTransactionKindNo;
            St_HeaderVM.TransactionKindNo = St_HeaderObj.TransactionKindNo;
            St_HeaderVM.CompanyYear = St_HeaderObj.CompanyYear;
            St_HeaderVM.TaxType = St_HeaderObj.TaxType;
            St_HeaderVM.VoucherCase = St_HeaderObj.VoucherCase;
            St_HeaderVM.VoucherDate = St_HeaderObj.VoucherDate;
            St_HeaderVM.DueDate = St_HeaderObj.DueDate;
            St_HeaderVM.CurrencyID = St_HeaderObj.CurrencyID;
            St_HeaderVM.ConversionFactor = St_HeaderObj.ConversionFactor;
            St_HeaderVM.AccountNumber = St_HeaderObj.AccountNumber;
            St_HeaderVM.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, St_HeaderObj.AccountNumber);
            St_HeaderVM.OriginalVoucherNumber = St_HeaderObj.OriginalVoucherNumber;
            St_HeaderVM.OrderNumber = St_HeaderObj.OrderNumber;
            St_HeaderVM.NetTotalLocalBeforDiscount = St_HeaderObj.NetTotalLocalBeforDiscount;
            St_HeaderVM.NetTotalLineDiscountLocal = St_HeaderObj.NetTotalLineDiscountLocal;
            St_HeaderVM.NetTotalLocalAfterLineDiscount = St_HeaderObj.NetTotalLocalAfterLineDiscount;
            St_HeaderVM.NetTotalTaxAfterLineDiscountLocal = St_HeaderObj.NetTotalTaxAfterLineDiscountLocal;
            St_HeaderVM.NetTotalAfterLineDiscountBeforDiscountAllLocal = St_HeaderObj.NetTotalAfterLineDiscountBeforDiscountAllLocal;
            St_HeaderVM.NetTotalDiscountLocal = St_HeaderObj.NetTotalDiscountLocal;
            St_HeaderVM.DiscountPercentage = St_HeaderObj.DiscountPercentage;
            St_HeaderVM.NetTotalLocalAfterDiscount = St_HeaderObj.NetTotalLocalAfterDiscount;
            St_HeaderVM.NetTotalTaxLocal = St_HeaderObj.NetTotalTaxLocal;
            St_HeaderVM.NetTotalLocal = St_HeaderObj.NetTotalLocal;
            St_HeaderVM.Remark = St_HeaderObj.Remark;
            St_HeaderVM.Hint = St_HeaderObj.Hint;
            St_HeaderVM.VoucherNumber = St_HeaderObj.VoucherNumber;
            St_HeaderVM.VHI = St_HeaderObj.VHI;
            St_HeaderVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            St_HeaderVM.WorkWithCostCenter = Company.WorkWithCostCenter;
            St_HeaderVM.DebitAccountNumber = TransactionDebitObj.AccountNumber;
            St_HeaderVM.CreditAccountNumber = TransactionCreditObj.AccountNumber;
            St_HeaderVM.DebitCostNumber = TransactionDebitObj.CostCenter;
            St_HeaderVM.CreditCostNumber = TransactionCreditObj.CostCenter;
            St_HeaderVM.DebitAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionDebitObj.AccountNumber);
            St_HeaderVM.CreditAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionCreditObj.AccountNumber);
            St_HeaderVM.DebitCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionDebitObj.CostCenter);
            St_HeaderVM.CreditCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionCreditObj.CostCenter);
            St_HeaderVM.OrignailTaxAccountNumber = ObjFix.AccountNo;
            St_HeaderVM.OrignailTaxAccountName = ObjFix.AccountName;
            if (HeaderObj.RowCount > 2)
            {
                St_HeaderVM.TaxAccountNumber = TransactionTaxObj.AccountNumber;
                St_HeaderVM.TaxCostNumber = TransactionTaxObj.CostCenter;
                St_HeaderVM.TaxCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionTaxObj.CostCenter);
                St_HeaderVM.TaxAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionTaxObj.AccountNumber);
            }
            St_PurchaseVoucherDetail report = new St_PurchaseVoucherDetail();


            string reportFilePath = Server.MapPath("/St_ReportLayout/St_PurchaseVoucherDetail.repx");
            report.LoadLayoutFromXml(reportFilePath);



            DataSet ds = new DataSet();
            DataTable table1 = new DataTable("St_HeaderVM");
            table1 = FunctionUnit.LINQResultToDataTable(AllItems);

            ds.Tables.Add(table1);

            report.DataSource = ds;
            report.DataMember = "St_HeaderVM";




            St_HeaderVM.sNetTotalLocalAfterLineDiscount = AllItems.Sum(m => (m.TotalLocalAfterLineDiscount)).ToString();
            St_HeaderVM.sNetTotalDiscountLocal = AllItems.Sum(m => (m.TotalDiscountLocal)).ToString();
            St_HeaderVM.sDiscountPercentage = AllItems.Sum(m => (m.DiscountPercentage)).ToString();
            St_HeaderVM.sNetTotalLocalAfterDiscount = AllItems.Sum(m => (m.TotalLocalAfterDiscount)).ToString();
            St_HeaderVM.sNetTotalTaxLocal = AllItems.Sum(m => (m.TotalTaxLocal)).ToString();
            St_HeaderVM.sNetTotalLocal = AllItems.Sum(m => (m.TotalLocal)).ToString();


            report.Remark.Text = St_HeaderVM.Remark;
            report.Hint.Text = St_HeaderVM.Hint;



            if (CoInfo.TheDecimalPointForTheLocalCurrency == 2)
            {
                St_HeaderVM.sNetTotalLocalAfterLineDiscount = "" + String.Format("{0:n2}", St_HeaderVM.sNetTotalLocalAfterLineDiscount) + "";
                St_HeaderVM.sNetTotalDiscountLocal = "" + String.Format("{0:n2}", St_HeaderVM.sNetTotalDiscountLocal) + "";
                St_HeaderVM.sDiscountPercentage = "" + String.Format("{0:n2}", St_HeaderVM.sDiscountPercentage) + "";
                St_HeaderVM.sNetTotalLocalAfterDiscount = "" + String.Format("{0:n2}", St_HeaderVM.sNetTotalLocalAfterDiscount) + "";
                St_HeaderVM.sNetTotalTaxLocal = "" + String.Format("{0:n2}", St_HeaderVM.sNetTotalTaxLocal) + "";
                St_HeaderVM.sNetTotalLocal = "" + String.Format("{0:n2}", St_HeaderVM.sNetTotalLocal) + "";

            }
            else
            {
                St_HeaderVM.sNetTotalLocalAfterLineDiscount = "" + String.Format("{0:n3}", St_HeaderVM.sNetTotalLocalAfterLineDiscount) + "";
                St_HeaderVM.sNetTotalDiscountLocal = "" + String.Format("{0:n3}", St_HeaderVM.sNetTotalDiscountLocal) + "";
                St_HeaderVM.sDiscountPercentage = "" + String.Format("{0:n3}", St_HeaderVM.sDiscountPercentage) + "";
                St_HeaderVM.sNetTotalLocalAfterDiscount = "" + String.Format("{0:n3}", St_HeaderVM.sNetTotalLocalAfterDiscount) + "";
                St_HeaderVM.sNetTotalTaxLocal = "" + String.Format("{0:n3}", St_HeaderVM.sNetTotalTaxLocal) + "";
                St_HeaderVM.sNetTotalLocal = "" + String.Format("{0:n3}", St_HeaderVM.sNetTotalLocal) + "";
            }



            report.Sum3.Text = St_HeaderVM.sNetTotalLocalAfterLineDiscount;
            report.Sum2.Text = St_HeaderVM.sNetTotalDiscountLocal.ToString();
            report.Sum.Text = St_HeaderVM.sDiscountPercentage;
            report.Sum4.Text = St_HeaderVM.sNetTotalLocalAfterDiscount;
            report.Sum5.Text = St_HeaderVM.sNetTotalTaxLocal;
            report.Sum6.Text = St_HeaderVM.sNetTotalLocal;


            report.LbCoName.Text = CoInfo.ArabicName;
            report.LbVhDate.Text = St_HeaderVM.VoucherDate.ToString("dd/MM/yyyy");
            report.LbVhNo.Text = St_HeaderVM.VoucherNumber.ToString();

            report.StockNo.Text = St_HeaderVM.StockCode;

            if (!String.IsNullOrEmpty(St_HeaderVM.StockCode))
            {
                var AccName = _unitOfWork.St_Warehouse.GetSt_WarehouseByID(UserInfo.fCompanyId, St_HeaderVM.StockCode);
                if (Resources.Resource.CurLang == "Arb")
                {
                    report.StockNo.Text = AccName.ArabicName;
                }
                else
                {
                    report.StockNo.Text = AccName.EnglishName;
                }

            }

            report.OrderNo.Text = St_HeaderVM.OrderNumber;

            if (St_HeaderVM.VoucherCase != 0)
            {
                if (St_HeaderVM.VoucherCase == 1)
                {
                    report.VoucherCase.Text = Resources.Resource.Cash;
                }
                else
                {
                    report.VoucherCase.Text = Resources.Resource.VoucherCredit;

                }
            }


            report.OriginalVo.Text = St_HeaderVM.OriginalVoucherNumber;

            report.DueDate.Text = St_HeaderVM.DueDate.ToString("dd/MM/yyyy");

            report.CreditNo.Text = St_HeaderVM.CreditAccountNumber;
            report.CreditName.Text = St_HeaderVM.CreditAccountName;

            report.DebitNo.Text = St_HeaderVM.DebitAccountNumber;
            report.DebitName.Text = St_HeaderVM.DebitAccountName;

            report.CreditCostNo.Text = St_HeaderVM.CreditCostNumber;
            report.CreditCostName.Text = St_HeaderVM.CreditCostName;

            report.DebitCostNo.Text = St_HeaderVM.DebitCostNumber;
            report.DebitCostName.Text = St_HeaderVM.DebitCostName;

            report.TaxNo.Text = St_HeaderVM.TaxAccountNumber;
            report.TaxName.Text = St_HeaderVM.TaxAccountName;

            report.TaxCostNo.Text = St_HeaderVM.TaxCostNumber;
            report.TaxCostName.Text = St_HeaderVM.TaxCostName;


            if (St_HeaderVM.TaxType == 1)
            {
                report.TaxType.Text = Resources.Resource.Taxable;


            }
            else if (St_HeaderVM.TaxType == 2)
            {
                report.TaxType.Text = Resources.Resource.TaxableByZero;

            }
            else
            {
                report.TaxType.Text = Resources.Resource.TaxExempt;

            }



            report.SupplierNo.Text = St_HeaderVM.AccountNumber;
            report.SupplierName.Text = St_HeaderVM.AccountName;




            report.CreateDocument();



            return View("ReportPDF", report);
        }



        public ActionResult St_ReturnPurchaseVoucherDetailPDF(string id, string id2, string id3, string id4, string id5)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var AllItems = _unitOfWork.NativeSql.GetSt_Transacation(id, int.Parse(id2), UserInfo.fCompanyId, int.Parse(id3), int.Parse(id4), id5);
            var St_Warehouse = _unitOfWork.St_Warehouse.GetAllSt_Warehouse(UserInfo.fCompanyId);
            var St_HeaderObj = _unitOfWork.St_Header.GetSt_HeaderDataById(id, UserInfo.fCompanyId, int.Parse(id2), int.Parse(id3), int.Parse(id4), id5);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var HeaderObj = _unitOfWork.Header.GetHeaderDataById(id, UserInfo.fCompanyId, int.Parse(id2), int.Parse(id3), int.Parse(id4));
            var TransactionDebitObj = _unitOfWork.NativeSql.GetTransactionsDetailDebit(id, UserInfo.fCompanyId, int.Parse(id2), 1, int.Parse(id4));
            var TransactionCreditObj = _unitOfWork.NativeSql.GetTransactionsDetailCredit(id, UserInfo.fCompanyId, int.Parse(id2), 2, int.Parse(id4));
            var TransactionTaxObj = _unitOfWork.NativeSql.GetTransactionsDetailTax(id, UserInfo.fCompanyId, int.Parse(id2), 3, int.Parse(id4));
            var ObjGet = _unitOfWork.St_OtherAccount.GetSt_OtherAccountByID(UserInfo.fCompanyId);
            FixAccountInfoVM ObjFix = new FixAccountInfoVM
            {
                AccountNo = "",
                AccountName = ""
            };
            if (ObjGet != null)
            {
                ObjFix.AccountNo = ObjGet.PurchaseTaxReturnAccountNumber;
                ObjFix.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.PurchaseTaxReturnAccountNumber);
            }
            var St_HeaderVM = new St_HeaderVM { };
            St_HeaderVM.St_Warehouse = St_Warehouse;
            St_HeaderVM.StockCode = St_HeaderObj.StockCode;
            St_HeaderVM.CompanyTransactionKindNo = St_HeaderObj.CompanyTransactionKindNo;
            St_HeaderVM.TransactionKindNo = St_HeaderObj.TransactionKindNo;
            St_HeaderVM.CompanyYear = St_HeaderObj.CompanyYear;
            St_HeaderVM.TaxType = St_HeaderObj.TaxType;
            St_HeaderVM.VoucherCase = St_HeaderObj.VoucherCase;
            St_HeaderVM.VoucherDate = St_HeaderObj.VoucherDate;
            St_HeaderVM.CurrencyID = St_HeaderObj.CurrencyID;
            St_HeaderVM.ConversionFactor = St_HeaderObj.ConversionFactor;
            St_HeaderVM.AccountNumber = St_HeaderObj.AccountNumber;
            St_HeaderVM.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, St_HeaderObj.AccountNumber);
            St_HeaderVM.NetTotalLocalBeforDiscount = St_HeaderObj.NetTotalLocalBeforDiscount;
            St_HeaderVM.NetTotalLineDiscountLocal = St_HeaderObj.NetTotalLineDiscountLocal;
            St_HeaderVM.NetTotalLocalAfterLineDiscount = St_HeaderObj.NetTotalLocalAfterLineDiscount;
            St_HeaderVM.NetTotalTaxAfterLineDiscountLocal = St_HeaderObj.NetTotalTaxAfterLineDiscountLocal;
            St_HeaderVM.NetTotalAfterLineDiscountBeforDiscountAllLocal = St_HeaderObj.NetTotalAfterLineDiscountBeforDiscountAllLocal;
            St_HeaderVM.NetTotalDiscountLocal = St_HeaderObj.NetTotalDiscountLocal;
            St_HeaderVM.DiscountPercentage = St_HeaderObj.DiscountPercentage;
            St_HeaderVM.NetTotalLocalAfterDiscount = St_HeaderObj.NetTotalLocalAfterDiscount;
            St_HeaderVM.NetTotalTaxLocal = St_HeaderObj.NetTotalTaxLocal;
            St_HeaderVM.NetTotalLocal = St_HeaderObj.NetTotalLocal;
            St_HeaderVM.Remark = St_HeaderObj.Remark;
            St_HeaderVM.Hint = St_HeaderObj.Hint;
            St_HeaderVM.OriginalVoucherNumber = St_HeaderObj.OriginalVoucherNumber;
            St_HeaderVM.VoucherNumber = St_HeaderObj.VoucherNumber;
            St_HeaderVM.VHI = St_HeaderObj.VHI;
            St_HeaderVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            St_HeaderVM.WorkWithCostCenter = Company.WorkWithCostCenter;
            St_HeaderVM.DebitAccountNumber = TransactionDebitObj.AccountNumber;
            St_HeaderVM.CreditAccountNumber = TransactionCreditObj.AccountNumber;
            St_HeaderVM.DebitCostNumber = TransactionDebitObj.CostCenter;
            St_HeaderVM.CreditCostNumber = TransactionCreditObj.CostCenter;
            St_HeaderVM.DebitAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionDebitObj.AccountNumber);
            St_HeaderVM.CreditAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionCreditObj.AccountNumber);
            St_HeaderVM.DebitCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionDebitObj.CostCenter);
            St_HeaderVM.CreditCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionCreditObj.CostCenter);
            St_HeaderVM.OrignailTaxAccountNumber = ObjFix.AccountNo;
            St_HeaderVM.OrignailTaxAccountName = ObjFix.AccountName;
            if (HeaderObj.RowCount > 2)
            {
                St_HeaderVM.TaxAccountNumber = TransactionTaxObj.AccountNumber;
                St_HeaderVM.TaxCostNumber = TransactionTaxObj.CostCenter;
                St_HeaderVM.TaxCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionTaxObj.CostCenter);
                St_HeaderVM.TaxAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionTaxObj.AccountNumber);
            }


            St_ReturnPurchaseVoucherDetail report = new St_ReturnPurchaseVoucherDetail();


            string reportFilePath = Server.MapPath("/St_ReportLayout/St_ReturnPurchaseVoucherDetail.repx");
            report.LoadLayoutFromXml(reportFilePath);

            DataSet ds = new DataSet();
            DataTable table1 = new DataTable("St_HeaderVM");
            table1 = FunctionUnit.LINQResultToDataTable(AllItems);

            ds.Tables.Add(table1);

            report.DataSource = ds;
            report.DataMember = "St_HeaderVM";




            St_HeaderVM.sNetTotalLocalAfterLineDiscount = AllItems.Sum(m => (m.TotalLocalAfterLineDiscount)).ToString();
            St_HeaderVM.sNetTotalDiscountLocal = AllItems.Sum(m => (m.TotalDiscountLocal)).ToString();
            St_HeaderVM.sDiscountPercentage = AllItems.Sum(m => (m.DiscountPercentage)).ToString();
            St_HeaderVM.sNetTotalLocalAfterDiscount = AllItems.Sum(m => (m.TotalLocalAfterDiscount)).ToString();
            St_HeaderVM.sNetTotalTaxLocal = AllItems.Sum(m => (m.TotalTaxLocal)).ToString();
            St_HeaderVM.sNetTotalLocal = AllItems.Sum(m => (m.TotalLocal)).ToString();


            report.Remark.Text = St_HeaderVM.Remark;
            report.Hint.Text = St_HeaderVM.Hint;



            if (CoInfo.TheDecimalPointForTheLocalCurrency == 2)
            {
                St_HeaderVM.sNetTotalLocalAfterLineDiscount = "" + String.Format("{0:n2}", St_HeaderVM.sNetTotalLocalAfterLineDiscount) + "";
                St_HeaderVM.sNetTotalDiscountLocal = "" + String.Format("{0:n2}", St_HeaderVM.sNetTotalDiscountLocal) + "";
                St_HeaderVM.sDiscountPercentage = "" + String.Format("{0:n2}", St_HeaderVM.sDiscountPercentage) + "";
                St_HeaderVM.sNetTotalLocalAfterDiscount = "" + String.Format("{0:n2}", St_HeaderVM.sNetTotalLocalAfterDiscount) + "";
                St_HeaderVM.sNetTotalTaxLocal = "" + String.Format("{0:n2}", St_HeaderVM.sNetTotalTaxLocal) + "";
                St_HeaderVM.sNetTotalLocal = "" + String.Format("{0:n2}", St_HeaderVM.sNetTotalLocal) + "";

            }
            else
            {
                St_HeaderVM.sNetTotalLocalAfterLineDiscount = "" + String.Format("{0:n3}", St_HeaderVM.sNetTotalLocalAfterLineDiscount) + "";
                St_HeaderVM.sNetTotalDiscountLocal = "" + String.Format("{0:n3}", St_HeaderVM.sNetTotalDiscountLocal) + "";
                St_HeaderVM.sDiscountPercentage = "" + String.Format("{0:n3}", St_HeaderVM.sDiscountPercentage) + "";
                St_HeaderVM.sNetTotalLocalAfterDiscount = "" + String.Format("{0:n3}", St_HeaderVM.sNetTotalLocalAfterDiscount) + "";
                St_HeaderVM.sNetTotalTaxLocal = "" + String.Format("{0:n3}", St_HeaderVM.sNetTotalTaxLocal) + "";
                St_HeaderVM.sNetTotalLocal = "" + String.Format("{0:n3}", St_HeaderVM.sNetTotalLocal) + "";
            }



            report.Sum3.Text = St_HeaderVM.sNetTotalLocalAfterLineDiscount;
            report.Sum2.Text = St_HeaderVM.sNetTotalDiscountLocal.ToString();
            report.Sum.Text = St_HeaderVM.sDiscountPercentage;
            report.Sum4.Text = St_HeaderVM.sNetTotalLocalAfterDiscount;
            report.Sum5.Text = St_HeaderVM.sNetTotalTaxLocal;
            report.Sum6.Text = St_HeaderVM.sNetTotalLocal;


            report.LbCoName.Text = CoInfo.ArabicName;
            report.LbVhDate.Text = St_HeaderVM.VoucherDate.ToString("dd/MM/yyyy");
            report.LbVhNo.Text = St_HeaderVM.VoucherNumber.ToString();

            report.StockNo.Text = St_HeaderVM.StockCode;

            if (!String.IsNullOrEmpty(St_HeaderVM.StockCode))
            {
                var AccName = _unitOfWork.St_Warehouse.GetSt_WarehouseByID(UserInfo.fCompanyId, St_HeaderVM.StockCode);
                if (Resources.Resource.CurLang == "Arb")
                {
                    report.StockNo.Text = AccName.ArabicName;
                }
                else
                {
                    report.StockNo.Text = AccName.EnglishName;
                }

            }


            if (St_HeaderVM.VoucherCase != 0)
            {
                if (St_HeaderVM.VoucherCase == 1)
                {
                    report.VoucherCase.Text = Resources.Resource.Cash;
                }
                else
                {
                    report.VoucherCase.Text = Resources.Resource.VoucherCredit;

                }
            }


            report.OriginalVo.Text = St_HeaderVM.OriginalVoucherNumber;


            report.CreditNo.Text = St_HeaderVM.CreditAccountNumber;
            report.CreditName.Text = St_HeaderVM.CreditAccountName;

            report.DebitNo.Text = St_HeaderVM.DebitAccountNumber;
            report.DebitName.Text = St_HeaderVM.DebitAccountName;

            report.CreditCostNo.Text = St_HeaderVM.CreditCostNumber;
            report.CreditCostName.Text = St_HeaderVM.CreditCostName;

            report.DebitCostNo.Text = St_HeaderVM.DebitCostNumber;
            report.DebitCostName.Text = St_HeaderVM.DebitCostName;

            report.TaxNo.Text = St_HeaderVM.TaxAccountNumber;
            report.TaxName.Text = St_HeaderVM.TaxAccountName;

            report.TaxCostNo.Text = St_HeaderVM.TaxCostNumber;
            report.TaxCostName.Text = St_HeaderVM.TaxCostName;


            if (St_HeaderVM.TaxType == 1)
            {
                report.TaxType.Text = Resources.Resource.Taxable;


            }
            else if (St_HeaderVM.TaxType == 2)
            {
                report.TaxType.Text = Resources.Resource.TaxableByZero;

            }
            else
            {
                report.TaxType.Text = Resources.Resource.TaxExempt;

            }



            report.SupplierNo.Text = St_HeaderVM.AccountNumber;
            report.SupplierName.Text = St_HeaderVM.AccountName;




            report.CreateDocument();



            return View("ReportPDF", report);
        }


        public ActionResult GetAllSt_ReturnPurchaseVoucherPDF(string id, string id2, string id3, string id4, string id5, int id6, int id7)
        {
            try
            {

                St_HeaderVM Obj = new St_HeaderVM();
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);

                DateTime fromDate = DateTime.Parse(id.Substring(0, 2) + "/" + id.Substring(2, 2) + "/" + id.Substring(4, 4));
                DateTime toDate = DateTime.Parse(id2.Substring(0, 2) + "/" + id2.Substring(2, 2) + "/" + id2.Substring(4, 4));
                Obj.FromDate = fromDate;
                Obj.ToDate = toDate;
                Obj.VoucherNumber = id3;
                Obj.AccountNumber = id4;
                Obj.StockCode = id5;
                Obj.TaxType = id6;
                Obj.VoucherCase = id7;

                if (Obj.VoucherNumber == "0")
                {
                    Obj.VoucherNumber = "";
                }
                if (Obj.AccountNumber == "0")
                {
                    Obj.AccountNumber = "";
                }

                if (Obj.StockCode == "0")
                {
                    Obj.StockCode = "";
                }
                var AllSt_ReturnPurchaseVoucher = _unitOfWork.NativeSql.GetAllSt_ReturnPurchaseVoucher(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                if (AllSt_ReturnPurchaseVoucher == null)
                {
                    return Json(new List<St_HeaderVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.VoucherNumber))
                {
                    AllSt_ReturnPurchaseVoucher = AllSt_ReturnPurchaseVoucher.Where(m => m.VoucherNumber == Obj.VoucherNumber).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.AccountNumber))
                {
                    AllSt_ReturnPurchaseVoucher = AllSt_ReturnPurchaseVoucher.Where(m => m.AccountNumber == Obj.AccountNumber).ToList();
                }
                if (Obj.TaxType != 0)
                {
                    AllSt_ReturnPurchaseVoucher = AllSt_ReturnPurchaseVoucher.Where(m => m.TaxType == Obj.TaxType).ToList();
                }
                if (Obj.VoucherCase != 0)
                {
                    AllSt_ReturnPurchaseVoucher = AllSt_ReturnPurchaseVoucher.Where(m => m.VoucherCase == Obj.VoucherCase).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.StockCode))
                {
                    AllSt_ReturnPurchaseVoucher = AllSt_ReturnPurchaseVoucher.Where(m => m.StockCode == Obj.StockCode).ToList();
                }
                DataSet ds = new DataSet();
                DataTable table1 = new DataTable("St_HeaderVM");
                table1 = FunctionUnit.LINQResultToDataTable(AllSt_ReturnPurchaseVoucher);

                ds.Tables.Add(table1);

                var report = new St_ReturnPurchaseVoucherReport();

                string reportFilePath = Server.MapPath("/St_ReportLayout/St_ReturnPurchaseVoucherReport.repx");
                report.LoadLayoutFromXml(reportFilePath);

                report.LbCoName.Text = CoInfo.ArabicName;


                report.DataSource = ds;
                report.DataMember = "St_HeaderVM";

                report.LbFromDate.Text = Obj.FromDate.ToString("dd/MM/yyyy");
                report.LbTODate.Text = Obj.ToDate.ToString("dd/MM/yyyy");
                report.LbVoucherNo.Text = Obj.VoucherNumber;

                if (Obj.VoucherCase != 0)
                {
                    if (Obj.VoucherCase == 1)
                    {
                        report.VoucherCase.Text = Resources.Resource.Cash;
                    }
                    else
                    {
                        report.VoucherCase.Text = Resources.Resource.VoucherCredit;

                    }

                }


                report.StockCase.Text = Obj.StockCode;

                if (!String.IsNullOrEmpty(Obj.StockCode))
                {
                    var AccName = _unitOfWork.St_Warehouse.GetSt_WarehouseByID(UserInfo.fCompanyId, Obj.StockCode);
                    if (Resources.Resource.CurLang == "Arb")
                    {
                        report.StockCase.Text = AccName.ArabicName;
                    }
                    else
                    {
                        report.StockCase.Text = AccName.EnglishName;
                    }

                }




                if (Obj.TaxType != 0)
                {
                    if (Obj.TaxType == 1)
                    {
                        report.TaxType.Text = Resources.Resource.Taxable;


                    }
                    else if (Obj.TaxType == 2)
                    {
                        report.TaxType.Text = Resources.Resource.TaxableByZero;

                    }
                    else
                    {
                        report.TaxType.Text = Resources.Resource.TaxExempt;

                    }

                }




                report.ToAccountNumber.Text = Obj.AccountNumber;


                if (!String.IsNullOrEmpty(Obj.AccountNumber))
                {
                    var AccName = _unitOfWork.ChartOfAccount.GetAccountByID(UserInfo.fCompanyId, Obj.AccountNumber);
                    if (Resources.Resource.CurLang == "Arb")
                    {
                        report.AccountName.Text = AccName.ArabicName;
                    }
                    else
                    {
                        report.AccountName.Text = AccName.EnglishName;
                    }

                }


                report.CreateDocument();




                return View("ReportPDF", report);
            }
            catch (Exception ex)
            {
                string err = ex.Message;

                List<St_HeaderVM> VMList = new List<St_HeaderVM>();

                St_ReturnPurchaseVoucherReport report = new St_ReturnPurchaseVoucherReport();

                string reportFilePath = Server.MapPath("/St_ReportLayout/St_ReturnPurchaseVoucherReport.repx");
                report.LoadLayoutFromXml(reportFilePath);

                DataSet ds = new DataSet();
                DataTable table1 = new DataTable("St_HeaderVM");
                table1 = FunctionUnit.LINQResultToDataTable(VMList);

                ds.Tables.Add(table1);




                report.DataSource = ds;
                report.DataMember = "St_HeaderVM";
                // report.LbPrintTime.Text = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, FunctionUnit.Jordan_Time_Zone).ToString();

                report.CreateDocument();

                return View("ReportPDF", report);

            }

        }



        public ActionResult GetAllSt_InVoucherPDF(string id, string id2, string id3, string id4)
        {
            try
            {
                St_HeaderVM Obj = new St_HeaderVM();
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);

                DateTime fromDate = DateTime.Parse(id.Substring(0, 2) + "/" + id.Substring(2, 2) + "/" + id.Substring(4, 4));
                DateTime toDate = DateTime.Parse(id2.Substring(0, 2) + "/" + id2.Substring(2, 2) + "/" + id2.Substring(4, 4));
                Obj.FromDate = fromDate;
                Obj.ToDate = toDate;
                Obj.VoucherNumber = id3;
                Obj.StockCode = id4;
              

                if (Obj.VoucherNumber == "0")
                {
                    Obj.VoucherNumber = "";
                }
            
                if (Obj.StockCode == "0")
                {
                    Obj.StockCode = "";
                }
                var AllSt_InVoucher = _unitOfWork.NativeSql.GetAllSt_InVoucher(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                if (AllSt_InVoucher == null)
                {
                    return Json(new List<St_HeaderVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.VoucherNumber))
                {
                    AllSt_InVoucher = AllSt_InVoucher.Where(m => m.VoucherNumber == Obj.VoucherNumber).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.StockCode))
                {
                    AllSt_InVoucher = AllSt_InVoucher.Where(m => m.StockCode == Obj.StockCode).ToList();
                }
                DataSet ds = new DataSet();
                DataTable table1 = new DataTable("St_HeaderVM");
                table1 = FunctionUnit.LINQResultToDataTable(AllSt_InVoucher);

                ds.Tables.Add(table1);

                var report = new St_InVoucherReport();

                string reportFilePath = Server.MapPath("/St_ReportLayout/St_InVoucherReport.repx");
                report.LoadLayoutFromXml(reportFilePath);

                report.LbCoName.Text = CoInfo.ArabicName;


                report.DataSource = ds;
                report.DataMember = "St_HeaderVM";

                report.LbFromDate.Text = Obj.FromDate.ToString("dd/MM/yyyy");
                report.LbTODate.Text = Obj.ToDate.ToString("dd/MM/yyyy");
                report.LbVoucherNo.Text = Obj.VoucherNumber;

           


                report.StockCase.Text = Obj.StockCode;

                if (!String.IsNullOrEmpty(Obj.StockCode))
                {
                    var AccName = _unitOfWork.St_Warehouse.GetSt_WarehouseByID(UserInfo.fCompanyId, Obj.StockCode);
                    if (Resources.Resource.CurLang == "Arb")
                    {
                        report.StockCase.Text = AccName.ArabicName;
                    }
                    else
                    {
                        report.StockCase.Text = AccName.EnglishName;
                    }

                }


                report.CreateDocument();




                return View("ReportPDF", report);
            }
            catch (Exception ex)
            {
                string err = ex.Message;

                List<St_HeaderVM> VMList = new List<St_HeaderVM>();

                St_InVoucherReport report = new St_InVoucherReport();

                string reportFilePath = Server.MapPath("/St_ReportLayout/St_InVoucherReport.repx");
                report.LoadLayoutFromXml(reportFilePath);

                DataSet ds = new DataSet();
                DataTable table1 = new DataTable("St_HeaderVM");
                table1 = FunctionUnit.LINQResultToDataTable(VMList);

                ds.Tables.Add(table1);




                report.DataSource = ds;
                report.DataMember = "St_HeaderVM";
                // report.LbPrintTime.Text = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, FunctionUnit.Jordan_Time_Zone).ToString();

                report.CreateDocument();

                return View("ReportPDF", report);
            }

        }


        public ActionResult St_InVoucherDetailPDF(string id, string id2, string id3, string id4, string id5)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var St_Warehouse = _unitOfWork.St_Warehouse.GetAllSt_Warehouse(UserInfo.fCompanyId);
            var AllItems = _unitOfWork.NativeSql.GetSt_Transacation(id, int.Parse(id2), UserInfo.fCompanyId, int.Parse(id3), int.Parse(id4), id5);
            var St_HeaderObj = _unitOfWork.St_Header.GetSt_HeaderDataById(id, UserInfo.fCompanyId, int.Parse(id2), int.Parse(id3), int.Parse(id4), id5);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var HeaderObj = _unitOfWork.Header.GetHeaderDataById(id, UserInfo.fCompanyId, int.Parse(id2), int.Parse(id3), int.Parse(id4));
            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);

            var St_HeaderVM = new St_HeaderVM { };
            St_HeaderVM.St_Warehouse = St_Warehouse;
            St_HeaderVM.StockCode = St_HeaderObj.StockCode;
            St_HeaderVM.CompanyTransactionKindNo = St_HeaderObj.CompanyTransactionKindNo;
            St_HeaderVM.TransactionKindNo = St_HeaderObj.TransactionKindNo;
            St_HeaderVM.CompanyYear = St_HeaderObj.CompanyYear;
            St_HeaderVM.TaxType = St_HeaderObj.TaxType;
            St_HeaderVM.VoucherCase = St_HeaderObj.VoucherCase;
            St_HeaderVM.VoucherDate = St_HeaderObj.VoucherDate;
            St_HeaderVM.DueDate = St_HeaderObj.VoucherDate;
            St_HeaderVM.CurrencyID = St_HeaderObj.CurrencyID;
            St_HeaderVM.ConversionFactor = St_HeaderObj.ConversionFactor;
            St_HeaderVM.NetTotalLocalBeforDiscount = St_HeaderObj.NetTotalLocalBeforDiscount;
            St_HeaderVM.NetTotalLineDiscountLocal = St_HeaderObj.NetTotalLineDiscountLocal;
            St_HeaderVM.NetTotalLocalAfterLineDiscount = St_HeaderObj.NetTotalLocalAfterLineDiscount;
            St_HeaderVM.NetTotalTaxAfterLineDiscountLocal = St_HeaderObj.NetTotalTaxAfterLineDiscountLocal;
            St_HeaderVM.NetTotalAfterLineDiscountBeforDiscountAllLocal = St_HeaderObj.NetTotalAfterLineDiscountBeforDiscountAllLocal;
            St_HeaderVM.NetTotalDiscountLocal = St_HeaderObj.NetTotalDiscountLocal;
            St_HeaderVM.DiscountPercentage = St_HeaderObj.DiscountPercentage;
            St_HeaderVM.NetTotalLocalAfterDiscount = St_HeaderObj.NetTotalLocalAfterDiscount;
            St_HeaderVM.NetTotalTaxLocal = St_HeaderObj.NetTotalTaxLocal;
            St_HeaderVM.NetTotalLocal = St_HeaderObj.NetTotalLocal;
            St_HeaderVM.Remark = St_HeaderObj.Remark;
            St_HeaderVM.Hint = St_HeaderObj.Hint;
            St_HeaderVM.VoucherNumber = St_HeaderObj.VoucherNumber;
            St_HeaderVM.VHI = St_HeaderObj.VHI;
            St_HeaderVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;

            St_InVoucherDetail report = new St_InVoucherDetail();

            // cokin 
            string reportFilePath = Server.MapPath("/St_ReportLayout/St_InVoucherDetail.repx");
            report.LoadLayoutFromXml(reportFilePath);



            DataSet ds = new DataSet();
            DataTable table1 = new DataTable("St_HeaderVM");
            table1 = FunctionUnit.LINQResultToDataTable(AllItems);

            ds.Tables.Add(table1);

            report.DataSource = ds;
            report.DataMember = "St_HeaderVM";


            report.LbCoName.Text = CoInfo.ArabicName;
            report.LbVhDate.Text = St_HeaderVM.VoucherDate.ToString("dd/MM/yyyy");
            report.LbVhNo.Text = St_HeaderVM.VoucherNumber.ToString();

            St_HeaderVM.sNetTotalLocal = AllItems.Sum(m => (m.TotalLocal)).ToString();

            report.StockNo.Text = St_HeaderVM.StockCode;

            if (!String.IsNullOrEmpty(St_HeaderVM.StockCode))
            {
                var AccName = _unitOfWork.St_Warehouse.GetSt_WarehouseByID(UserInfo.fCompanyId, St_HeaderVM.StockCode);
                if (Resources.Resource.CurLang == "Arb")
                {
                    report.StockNo.Text = AccName.ArabicName;
                }
                else
                {
                    report.StockNo.Text = AccName.EnglishName;
                }

            }
            report.Remark.Text = St_HeaderVM.Remark;
            report.Hint.Text = St_HeaderVM.Hint;


            if (CoInfo.TheDecimalPointForTheLocalCurrency == 2)
            {
                St_HeaderVM.sNetTotalLocal = "" + String.Format("{0:n2}", St_HeaderVM.sNetTotalLocal) + "";

            }
            else
            {
                St_HeaderVM.sNetTotalLocal = "" + String.Format("{0:n3}", St_HeaderVM.sNetTotalLocal) + "";
            }

            report.Sum6.Text = St_HeaderVM.sNetTotalLocal;


            report.CreateDocument();



            return View("ReportPDF", report);
        }


        public ActionResult GetAllSt_OfferPricePDF(string id, string id2, string id3, string id4, int id5, int id6, int id7)
        {
            try
            {
                St_OfferPriceVM Obj = new St_OfferPriceVM();
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);

                DateTime fromDate = DateTime.Parse(id.Substring(0, 2) + "/" + id.Substring(2, 2) + "/" + id.Substring(4, 4));
                DateTime toDate = DateTime.Parse(id2.Substring(0, 2) + "/" + id2.Substring(2, 2) + "/" + id2.Substring(4, 4));
                Obj.FromDate = fromDate;
                Obj.ToDate = toDate;
                Obj.VoucherNumber = id3;
                Obj.CustomerAccountNumber = id4;
                Obj.LinkWithVoucher = id5;
                Obj.TaxType = id6;
                Obj.VoucherCase = id7;


                if (Obj.VoucherNumber == "0")
                {
                    Obj.VoucherNumber = "";
                }
                if (Obj.CustomerAccountNumber == "0")
                {
                    Obj.CustomerAccountNumber = "";
                }
                var GetAllSt_OfferPrice = _unitOfWork.NativeSql.GetAllSt_OfferPrice(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                if (GetAllSt_OfferPrice == null)
                {
                    return Json(new List<St_OfferPriceVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.VoucherNumber))
                {
                    GetAllSt_OfferPrice = GetAllSt_OfferPrice.Where(m => m.VoucherNumber == Obj.VoucherNumber).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.CustomerAccountNumber))
                {
                    GetAllSt_OfferPrice = GetAllSt_OfferPrice.Where(m => m.CustomerAccountNumber == Obj.CustomerAccountNumber).ToList();
                }
                if (Obj.TaxType != 0)
                {
                    GetAllSt_OfferPrice = GetAllSt_OfferPrice.Where(m => m.TaxType == Obj.TaxType).ToList();
                }
                if (Obj.VoucherCase != 0)
                {
                    GetAllSt_OfferPrice = GetAllSt_OfferPrice.Where(m => m.VoucherCase == Obj.VoucherCase).ToList();
                }
                if (Obj.LinkWithVoucher != 0)
                {
                    GetAllSt_OfferPrice = GetAllSt_OfferPrice.Where(m => m.LinkWithVoucher == Obj.LinkWithVoucher).ToList();
                }
                DataSet ds = new DataSet();
                DataTable table1 = new DataTable("St_OfferPriceVM");
                table1 = FunctionUnit.LINQResultToDataTable(GetAllSt_OfferPrice);

                ds.Tables.Add(table1);

                var report = new St_OfferPriceReport();

                string reportFilePath = Server.MapPath("/St_ReportLayout/St_OfferPriceReport.repx");
                report.LoadLayoutFromXml(reportFilePath);

                report.LbCoName.Text = CoInfo.ArabicName;


                report.DataSource = ds;
                report.DataMember = "St_OfferPriceVM";

                report.LbFromDate.Text = Obj.FromDate.ToString("dd/MM/yyyy");
                report.LbTODate.Text = Obj.ToDate.ToString("dd/MM/yyyy");
                report.LbVoucherNo.Text = Obj.VoucherNumber;

                if (Obj.VoucherCase != 0)
                {
                    if (Obj.VoucherCase == 1)
                    {
                        report.VoucherCase.Text = Resources.Resource.Cash;
                    }
                    else
                    {
                        report.VoucherCase.Text = Resources.Resource.VoucherCredit;

                    }

                }



                if (Obj.TaxType != 0)
                {
                    if (Obj.TaxType == 1)
                    {
                        report.TaxType.Text = Resources.Resource.Taxable;


                    }
                    else if (Obj.TaxType == 2)
                    {
                        report.TaxType.Text = Resources.Resource.TaxableByZero;

                    }
                    else
                    {
                        report.TaxType.Text = Resources.Resource.TaxExempt;

                    }

                }



                if (Obj.LinkWithVoucher != -1)
                {
                    if (Obj.LinkWithVoucher == 1)
                    {
                        report.LinkCase.Text = Resources.Resource.Related;
                    }
                    else
                    {
                        report.LinkCase.Text = Resources.Resource.NotRelated;

                    }

                }



                report.ToAccountNumber.Text = Obj.CustomerAccountNumber;


                if (!String.IsNullOrEmpty(Obj.CustomerAccountNumber))
                {
                    var AccName = _unitOfWork.ChartOfAccount.GetAccountByID(UserInfo.fCompanyId, Obj.CustomerAccountNumber);
                    if (Resources.Resource.CurLang == "Arb")
                    {
                        report.AccountName.Text = AccName.ArabicName;
                    }
                    else
                    {
                        report.AccountName.Text = AccName.EnglishName;
                    }

                }


                report.CreateDocument();




                return View("ReportPDF", report);
            }
            catch (Exception ex)
            {
                string err = ex.Message;

                List<St_OfferPriceVM> VMList = new List<St_OfferPriceVM>();

                St_OfferPriceReport report = new St_OfferPriceReport();

                string reportFilePath = Server.MapPath("/St_ReportLayout/St_OfferPriceReport.repx");
                report.LoadLayoutFromXml(reportFilePath);

                DataSet ds = new DataSet();
                DataTable table1 = new DataTable("St_OfferPriceVM");
                table1 = FunctionUnit.LINQResultToDataTable(VMList);

                ds.Tables.Add(table1);




                report.DataSource = ds;
                report.DataMember = "St_OfferPriceVM";
                // report.LbPrintTime.Text = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, FunctionUnit.Jordan_Time_Zone).ToString();

                report.CreateDocument();

                return View("ReportPDF", report);
            }

        }


        public ActionResult St_OfferPriceDetailPDf(string id, string id2, string id3, string id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            var HeaderObj = _unitOfWork.St_OfferPrice.GetSt_OfferPriceHeaderDataById(id, UserInfo.fCompanyId, int.Parse(id2), int.Parse(id3), int.Parse(id4));
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var St_OfferPriceVM = new St_OfferPriceVM { };
            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var AllItems = _unitOfWork.NativeSql.GetSt_OfferPriceTransaction(id, int.Parse(id2), UserInfo.fCompanyId, int.Parse(id3), int.Parse(id4));
            St_OfferPriceVM.CompanyTransactionKindNo = HeaderObj.CompanyTransactionKindNo;
            St_OfferPriceVM.TransactionKindNo = HeaderObj.TransactionKindNo;
            St_OfferPriceVM.CompanyYear = HeaderObj.CompanyYear;
            St_OfferPriceVM.TaxType = HeaderObj.TaxType;
            St_OfferPriceVM.VoucherCase = HeaderObj.VoucherCase;
            St_OfferPriceVM.VoucherDate = HeaderObj.VoucherDate;
            St_OfferPriceVM.CurrencyID = HeaderObj.CurrencyID;
            St_OfferPriceVM.ConversionFactor = HeaderObj.ConversionFactor;
            St_OfferPriceVM.CustomerAccountNumber = HeaderObj.CustomerAccountNumber;
            St_OfferPriceVM.TotalQuantity = HeaderObj.TotalQuantity;
            St_OfferPriceVM.NetTotal = HeaderObj.NetTotal;
            St_OfferPriceVM.CustomerAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, HeaderObj.CustomerAccountNumber);
            St_OfferPriceVM.Remark = HeaderObj.Remark;
            St_OfferPriceVM.Hint = HeaderObj.Hint;
            St_OfferPriceVM.VoucherNumber = HeaderObj.VoucherNumber;
            St_OfferPriceVM.VHI = HeaderObj.VHI;
            St_OfferPriceVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;

            St_OfferPriceDetail report = new St_OfferPriceDetail();

            // cokin 
            string reportFilePath = Server.MapPath("/St_ReportLayout/St_OfferPriceDetail.repx");
            report.LoadLayoutFromXml(reportFilePath);



            DataSet ds = new DataSet();
            DataTable table1 = new DataTable("St_OfferPriceVM");
            table1 = FunctionUnit.LINQResultToDataTable(AllItems);

            ds.Tables.Add(table1);

            report.DataSource = ds;
            report.DataMember = "St_OfferPriceVM";


            report.LbCoName.Text = CoInfo.ArabicName;
            report.LbVhDate.Text = St_OfferPriceVM.VoucherDate.ToString("dd/MM/yyyy");
            report.LbVhNo.Text = St_OfferPriceVM.VoucherNumber.ToString();

            if (St_OfferPriceVM.VoucherCase == 1)
            {
                report.VoucherCase.Text = Resources.Resource.Cash;
            }
            else
            {
                report.VoucherCase.Text = Resources.Resource.VoucherCredit;

            }


            if (St_OfferPriceVM.TaxType == 1)
            {
                report.TaxType.Text = Resources.Resource.Taxable;


            }
            else if (St_OfferPriceVM.TaxType == 2)
            {
                report.TaxType.Text = Resources.Resource.TaxableByZero;

            }
            else
            {
                report.TaxType.Text = Resources.Resource.TaxExempt;

            }

            report.AccNo.Text = St_OfferPriceVM.CustomerAccountNumber;
            report.AccName.Text = St_OfferPriceVM.CustomerAccountName;



            report.Remark.Text = St_OfferPriceVM.Remark;
            report.Hint.Text = St_OfferPriceVM.Hint;

            report.Qty.Text = AllItems.Sum(m => m.Quantity).ToString();
            St_OfferPriceVM.NetTotal = AllItems.Sum(m => (m.Total));

            if (CoInfo.TheDecimalPointForTheLocalCurrency == 2)
            {
                report.Net.Text = "" + String.Format("{0:n2}", St_OfferPriceVM.NetTotal) + "";

            }
            else
            {
                report.Net.Text = "" + String.Format("{0:n3}", St_OfferPriceVM.NetTotal) + "";
            }
            report.LbPrintTime.Text = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, FunctionUnit.Jordan_Time_Zone).ToString();

            report.CreateDocument();



            return View("ReportPDF", report);
        }

        public ActionResult GetAllSt_SaleOrderPDF(string id, string id2, string id3, string id4, int id5, int id6, int id7)
        {
            try
            {
                St_SaleOrderVM Obj = new St_SaleOrderVM();
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);

                DateTime fromDate = DateTime.Parse(id.Substring(0, 2) + "/" + id.Substring(2, 2) + "/" + id.Substring(4, 4));
                DateTime toDate = DateTime.Parse(id2.Substring(0, 2) + "/" + id2.Substring(2, 2) + "/" + id2.Substring(4, 4));
                Obj.FromDate = fromDate;
                Obj.ToDate = toDate;
                Obj.VoucherNumber = id3;
                Obj.CustomerAccountNumber = id4;
                Obj.LinkWithVoucher = id5;
                Obj.TaxType = id6;
                Obj.VoucherCase = id7;


                if (Obj.VoucherNumber == "0")
                {
                    Obj.VoucherNumber = "";
                }
                if (Obj.CustomerAccountNumber == "0")
                {
                    Obj.CustomerAccountNumber = "";
                }
                var GetAllSt_SaleOrder = _unitOfWork.NativeSql.GetAllSt_SaleOrder(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                if (GetAllSt_SaleOrder == null)
                {
                    return Json(new List<St_SaleOrderVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.VoucherNumber))
                {
                    GetAllSt_SaleOrder = GetAllSt_SaleOrder.Where(m => m.VoucherNumber == Obj.VoucherNumber).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.CustomerAccountNumber))
                {
                    GetAllSt_SaleOrder = GetAllSt_SaleOrder.Where(m => m.CustomerAccountNumber == Obj.CustomerAccountNumber).ToList();
                }
                if (Obj.TaxType != 0)
                {
                    GetAllSt_SaleOrder = GetAllSt_SaleOrder.Where(m => m.TaxType == Obj.TaxType).ToList();
                }
                if (Obj.VoucherCase != 0)
                {
                    GetAllSt_SaleOrder = GetAllSt_SaleOrder.Where(m => m.VoucherCase == Obj.VoucherCase).ToList();
                }
                if (Obj.LinkWithVoucher != 0)
                {
                    GetAllSt_SaleOrder = GetAllSt_SaleOrder.Where(m => m.LinkWithVoucher == Obj.LinkWithVoucher).ToList();
                }
                DataSet ds = new DataSet();
                DataTable table1 = new DataTable("St_SaleOrderVM");
                table1 = FunctionUnit.LINQResultToDataTable(GetAllSt_SaleOrder);

                ds.Tables.Add(table1);

                var report = new St_SaleOrderReport();

                string reportFilePath = Server.MapPath("/St_ReportLayout/St_SaleOrderReport.repx");
                report.LoadLayoutFromXml(reportFilePath);

                report.LbCoName.Text = CoInfo.ArabicName;


                report.DataSource = ds;
                report.DataMember = "St_SaleOrderVM";

                report.LbFromDate.Text = Obj.FromDate.ToString("dd/MM/yyyy");
                report.LbTODate.Text = Obj.ToDate.ToString("dd/MM/yyyy");
                report.LbVoucherNo.Text = Obj.VoucherNumber;

                if (Obj.VoucherCase != 0)
                {
                    if (Obj.VoucherCase == 1)
                    {
                        report.VoucherCase.Text = Resources.Resource.Cash;
                    }
                    else
                    {
                        report.VoucherCase.Text = Resources.Resource.VoucherCredit;

                    }

                }



                if (Obj.TaxType != 0)
                {
                    if (Obj.TaxType == 1)
                    {
                        report.TaxType.Text = Resources.Resource.Taxable;


                    }
                    else if (Obj.TaxType == 2)
                    {
                        report.TaxType.Text = Resources.Resource.TaxableByZero;

                    }
                    else
                    {
                        report.TaxType.Text = Resources.Resource.TaxExempt;

                    }

                }



                if (Obj.LinkWithVoucher != -1)
                {
                    if (Obj.LinkWithVoucher == 1)
                    {
                        report.LinkCase.Text = Resources.Resource.Related;
                    }
                    else
                    {
                        report.LinkCase.Text = Resources.Resource.NotRelated;

                    }

                }



                report.ToAccountNumber.Text = Obj.CustomerAccountNumber;


                if (!String.IsNullOrEmpty(Obj.CustomerAccountNumber))
                {
                    var AccName = _unitOfWork.ChartOfAccount.GetAccountByID(UserInfo.fCompanyId, Obj.CustomerAccountNumber);
                    if (Resources.Resource.CurLang == "Arb")
                    {
                        report.AccountName.Text = AccName.ArabicName;
                    }
                    else
                    {
                        report.AccountName.Text = AccName.EnglishName;
                    }

                }


                report.CreateDocument();




                return View("ReportPDF", report);
            }
            catch (Exception ex)
            {
                string err = ex.Message;

                List<St_SaleOrderVM> VMList = new List<St_SaleOrderVM>();

                St_SaleOrderReport report = new St_SaleOrderReport();

                string reportFilePath = Server.MapPath("/St_ReportLayout/St_SaleOrderReport.repx");
                report.LoadLayoutFromXml(reportFilePath);

                DataSet ds = new DataSet();
                DataTable table1 = new DataTable("St_SaleOrderVM");
                table1 = FunctionUnit.LINQResultToDataTable(VMList);

                ds.Tables.Add(table1);




                report.DataSource = ds;
                report.DataMember = "St_SaleOrderVM";
                // report.LbPrintTime.Text = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, FunctionUnit.Jordan_Time_Zone).ToString();

                report.CreateDocument();

                return View("ReportPDF", report);
            }

        }

        public ActionResult St_SaleOrderDetailPDF(string id, string id2, string id3, string id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            var HeaderObj = _unitOfWork.St_SalesOrder.GetSt_SalesOrderHeaderDataById(id, UserInfo.fCompanyId, int.Parse(id2), int.Parse(id3), int.Parse(id4));
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var St_SaleOrderVM = new St_SaleOrderVM { };
            var AllItems = _unitOfWork.NativeSql.GetSt_SaleOrderTransaction(id, int.Parse(id2), UserInfo.fCompanyId, int.Parse(id3), int.Parse(id4));
            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            St_SaleOrderVM.CompanyTransactionKindNo = HeaderObj.CompanyTransactionKindNo;
            St_SaleOrderVM.TransactionKindNo = HeaderObj.TransactionKindNo;
            St_SaleOrderVM.CompanyYear = HeaderObj.CompanyYear;
            St_SaleOrderVM.TaxType = HeaderObj.TaxType;
            St_SaleOrderVM.VoucherCase = HeaderObj.VoucherCase;
            St_SaleOrderVM.VoucherDate = HeaderObj.VoucherDate;
            St_SaleOrderVM.CurrencyID = HeaderObj.CurrencyID;
            St_SaleOrderVM.ConversionFactor = HeaderObj.ConversionFactor;
            St_SaleOrderVM.CustomerAccountNumber = HeaderObj.CustomerAccountNumber;
            St_SaleOrderVM.TotalQuantity = HeaderObj.TotalQuantity;
            St_SaleOrderVM.NetTotal = HeaderObj.NetTotal;
            St_SaleOrderVM.CustomerAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, HeaderObj.CustomerAccountNumber);
            St_SaleOrderVM.Remark = HeaderObj.Remark;
            St_SaleOrderVM.Hint = HeaderObj.Hint;
            St_SaleOrderVM.VoucherNumber = HeaderObj.VoucherNumber;
            St_SaleOrderVM.VHI = HeaderObj.VHI;
            St_SaleOrderVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;

            St_SaleOrderDetail report = new St_SaleOrderDetail();

            // cokin 
            string reportFilePath = Server.MapPath("/St_ReportLayout/St_SaleOrderDetail.repx");
            report.LoadLayoutFromXml(reportFilePath);



            DataSet ds = new DataSet();
            DataTable table1 = new DataTable("St_SaleOrderVM");
            table1 = FunctionUnit.LINQResultToDataTable(AllItems);

            ds.Tables.Add(table1);

            report.DataSource = ds;
            report.DataMember = "St_SaleOrderVM";


            report.LbCoName.Text = CoInfo.ArabicName;
            report.LbVhDate.Text = St_SaleOrderVM.VoucherDate.ToString("dd/MM/yyyy");
            report.LbVhNo.Text = St_SaleOrderVM.VoucherNumber.ToString();

            if (St_SaleOrderVM.VoucherCase == 1)
            {
                report.VoucherCase.Text = Resources.Resource.Cash;
            }
            else
            {
                report.VoucherCase.Text = Resources.Resource.VoucherCredit;

            }


            if (St_SaleOrderVM.TaxType == 1)
            {
                report.TaxType.Text = Resources.Resource.Taxable;


            }
            else if (St_SaleOrderVM.TaxType == 2)
            {
                report.TaxType.Text = Resources.Resource.TaxableByZero;

            }
            else
            {
                report.TaxType.Text = Resources.Resource.TaxExempt;

            }

            report.AccNo.Text = St_SaleOrderVM.CustomerAccountNumber;
            report.AccName.Text = St_SaleOrderVM.CustomerAccountName;



            report.Remark.Text = St_SaleOrderVM.Remark;
            report.Hint.Text = St_SaleOrderVM.Hint;

            report.Qty.Text = AllItems.Sum(m => m.Quantity).ToString();
            St_SaleOrderVM.NetTotal = AllItems.Sum(m => (m.Total));

            if (CoInfo.TheDecimalPointForTheLocalCurrency == 2)
            {
                report.Net.Text = "" + String.Format("{0:n2}", St_SaleOrderVM.NetTotal) + "";

            }
            else
            {
                report.Net.Text = "" + String.Format("{0:n3}", St_SaleOrderVM.NetTotal) + "";
            }
            report.LbPrintTime.Text = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, FunctionUnit.Jordan_Time_Zone).ToString();

            report.CreateDocument();



            return View("ReportPDF", report);
        }


        public ActionResult GetAllSt_SaleVoucherPDF(string id, string id2, string id3, string id4, string id5, int id6, int id7 , int id8)
        {
            try
            {
                St_HeaderVM Obj = new St_HeaderVM();
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);

                DateTime fromDate = DateTime.Parse(id.Substring(0, 2) + "/" + id.Substring(2, 2) + "/" + id.Substring(4, 4));
                DateTime toDate = DateTime.Parse(id2.Substring(0, 2) + "/" + id2.Substring(2, 2) + "/" + id2.Substring(4, 4));
                Obj.FromDate = fromDate;
                Obj.ToDate = toDate;
                Obj.VoucherNumber = id3;
                Obj.AccountNumber = id4;
                Obj.StockCode = id5;
                Obj.TaxType = id6;
                Obj.VoucherCase = id7;
                Obj.SaleID = id8;

                if (Obj.VoucherNumber == "0")
                {
                    Obj.VoucherNumber = "";
                }
                if (Obj.AccountNumber == "0")
                {
                    Obj.AccountNumber = "";
                }

                if (Obj.StockCode == "0")
                {
                    Obj.StockCode = "";
                }
                var AllSt_SaleVoucher = _unitOfWork.NativeSql.GetAllSt_SaleVoucher(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                if (AllSt_SaleVoucher == null)
                {
                    return Json(new List<St_HeaderVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.VoucherNumber))
                {
                    AllSt_SaleVoucher = AllSt_SaleVoucher.Where(m => m.VoucherNumber == Obj.VoucherNumber).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.AccountNumber))
                {
                    AllSt_SaleVoucher = AllSt_SaleVoucher.Where(m => m.AccountNumber == Obj.AccountNumber).ToList();
                }
                if (Obj.TaxType != 0)
                {
                    AllSt_SaleVoucher = AllSt_SaleVoucher.Where(m => m.TaxType == Obj.TaxType).ToList();
                }
                if (Obj.VoucherCase != 0)
                {
                    AllSt_SaleVoucher = AllSt_SaleVoucher.Where(m => m.VoucherCase == Obj.VoucherCase).ToList();
                }
                if (Obj.SaleID != 0)
                {
                    AllSt_SaleVoucher = AllSt_SaleVoucher.Where(m => m.SaleID == Obj.SaleID).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.StockCode))
                {
                    AllSt_SaleVoucher = AllSt_SaleVoucher.Where(m => m.StockCode == Obj.StockCode).ToList();
                }
                DataSet ds = new DataSet();
                DataTable table1 = new DataTable("St_HeaderVM");
                table1 = FunctionUnit.LINQResultToDataTable(AllSt_SaleVoucher);

                ds.Tables.Add(table1);

                var report = new St_SaleVoucherReport();

                string reportFilePath = Server.MapPath("/St_ReportLayout/St_SaleVoucherReport.repx");
                report.LoadLayoutFromXml(reportFilePath);

                report.LbCoName.Text = CoInfo.ArabicName;


                report.DataSource = ds;
                report.DataMember = "St_HeaderVM";

                report.LbFromDate.Text = Obj.FromDate.ToString("dd/MM/yyyy");
                report.LbTODate.Text = Obj.ToDate.ToString("dd/MM/yyyy");
                report.LbVoucherNo.Text = Obj.VoucherNumber;

                if (Obj.VoucherCase != 0)
                {
                    if (Obj.VoucherCase == 1)
                    {
                        report.VoucherCase.Text = Resources.Resource.Cash;
                    }
                    else
                    {
                        report.VoucherCase.Text = Resources.Resource.VoucherCredit;

                    }

                }

                if (Obj.TaxType != 0)
                {
                    if (Obj.TaxType == 1)
                    {
                        report.TaxType.Text = Resources.Resource.Taxable;


                    }
                    else if (Obj.TaxType == 2)
                    {
                        report.TaxType.Text = Resources.Resource.TaxableByZero;

                    }
                    else
                    {
                        report.TaxType.Text = Resources.Resource.TaxExempt;

                    }

                }

                report.StockCase.Text = Obj.StockCode;

                if (!String.IsNullOrEmpty(Obj.StockCode))
                {
                    var AccName = _unitOfWork.St_Warehouse.GetSt_WarehouseByID(UserInfo.fCompanyId, Obj.StockCode);
                    if (Resources.Resource.CurLang == "Arb")
                    {
                        report.StockCase.Text = AccName.ArabicName;
                    }
                    else
                    {
                        report.StockCase.Text = AccName.EnglishName;
                    }

                }



                report.ToAccountNumber.Text = Obj.AccountNumber;


                if (!String.IsNullOrEmpty(Obj.AccountNumber))
                {
                    var AccName = _unitOfWork.ChartOfAccount.GetAccountByID(UserInfo.fCompanyId, Obj.AccountNumber);
                    if (Resources.Resource.CurLang == "Arb")
                    {
                        report.AccountName.Text = AccName.ArabicName;
                    }
                    else
                    {
                        report.AccountName.Text = AccName.EnglishName;
                    }

                }

                if (Obj.SaleID != 0)
                {
                    report.SaleMan.Text = String.Format("{0} {1}", Resources.Resource.SaleManName, Obj.SaleID.ToString());

                }
                else
                {
                    report.SaleMan.Text = " ";

                }



                report.CreateDocument();


                return View("ReportPDF", report);
            }
            catch (Exception ex)
            {
                string err = ex.Message;

                List<St_HeaderVM> VMList = new List<St_HeaderVM>();

                St_SaleVoucherReport report = new St_SaleVoucherReport();

                string reportFilePath = Server.MapPath("/St_ReportLayout/St_SaleVoucherReport.repx");
                report.LoadLayoutFromXml(reportFilePath);

                DataSet ds = new DataSet();
                DataTable table1 = new DataTable("St_HeaderVM");
                table1 = FunctionUnit.LINQResultToDataTable(VMList);

                ds.Tables.Add(table1);




                report.DataSource = ds;
                report.DataMember = "St_HeaderVM";
                // report.LbPrintTime.Text = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, FunctionUnit.Jordan_Time_Zone).ToString();

                report.CreateDocument();

                return View("ReportPDF", report);

            }

        }


        public ActionResult St_SaleVoucherDetailPDF(string id, string id2, string id3, string id4, string id5)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var St_Warehouse = _unitOfWork.St_Warehouse.GetAllSt_Warehouse(UserInfo.fCompanyId);
            var Sales = _unitOfWork.Sale.GetAllSale(UserInfo.fCompanyId);
            var AllItems = _unitOfWork.NativeSql.GetSt_Transacation(id, int.Parse(id2), UserInfo.fCompanyId, int.Parse(id3), int.Parse(id4), id5);
            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId);
            var St_HeaderObj = _unitOfWork.St_Header.GetSt_HeaderDataById(id, UserInfo.fCompanyId, int.Parse(id2), int.Parse(id3), int.Parse(id4), id5);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var HeaderObj = _unitOfWork.Header.GetHeaderDataById(id, UserInfo.fCompanyId, int.Parse(id2), int.Parse(id3), int.Parse(id4));
            var TransactionDebitObj = _unitOfWork.NativeSql.GetTransactionsDetailDebit(id, UserInfo.fCompanyId, int.Parse(id2), 1, int.Parse(id4));
            var TransactionCreditObj = _unitOfWork.NativeSql.GetTransactionsDetailCredit(id, UserInfo.fCompanyId, int.Parse(id2), 2, int.Parse(id4));
            var TransactionTaxObj = _unitOfWork.NativeSql.GetTransactionsDetailTax(id, UserInfo.fCompanyId, int.Parse(id2), 3, int.Parse(id4));
            var ObjGet = _unitOfWork.St_OtherAccount.GetSt_OtherAccountByID(UserInfo.fCompanyId);
            FixAccountInfoVM ObjFix = new FixAccountInfoVM
            {
                AccountNo = "",
                AccountName = ""
            };
            if (ObjGet != null)
            {
                ObjFix.AccountNo = ObjGet.SalesTaxAccountNumber;
                ObjFix.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.PurchaseTaxReturnAccountNumber);
            }
            var St_HeaderVM = new St_HeaderVM { };
            St_HeaderVM.St_Warehouse = St_Warehouse;
            St_HeaderVM.StockCode = St_HeaderObj.StockCode;
            St_HeaderVM.SaleMan = Sales;
            St_HeaderVM.SaleID = St_HeaderObj.SaleID;
            St_HeaderVM.Currency = Currency;
            St_HeaderVM.CurrencyID = St_HeaderObj.CurrencyID;
            St_HeaderVM.ConversionFactor = St_HeaderObj.ConversionFactor;
            St_HeaderVM.CompanyTransactionKindNo = St_HeaderObj.CompanyTransactionKindNo;
            St_HeaderVM.TransactionKindNo = St_HeaderObj.TransactionKindNo;
            St_HeaderVM.CompanyYear = St_HeaderObj.CompanyYear;
            St_HeaderVM.TaxType = St_HeaderObj.TaxType;
            St_HeaderVM.VoucherCase = St_HeaderObj.VoucherCase;
            St_HeaderVM.VoucherDate = St_HeaderObj.VoucherDate;
            St_HeaderVM.DueDate = St_HeaderObj.DueDate;
            St_HeaderVM.OriginalVoucherNumber = St_HeaderObj.OriginalVoucherNumber;
            St_HeaderVM.AccountNumber = St_HeaderObj.AccountNumber;
            St_HeaderVM.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, St_HeaderObj.AccountNumber);
            St_HeaderVM.NetTotalLocalBeforDiscount = St_HeaderObj.NetTotalLocalBeforDiscount;
            St_HeaderVM.NetTotalLineDiscountLocal = St_HeaderObj.NetTotalLineDiscountLocal;
            St_HeaderVM.NetTotalLocalAfterLineDiscount = St_HeaderObj.NetTotalLocalAfterLineDiscount;
            St_HeaderVM.NetTotalTaxAfterLineDiscountLocal = St_HeaderObj.NetTotalTaxAfterLineDiscountLocal;
            St_HeaderVM.NetTotalAfterLineDiscountBeforDiscountAllLocal = St_HeaderObj.NetTotalAfterLineDiscountBeforDiscountAllLocal;
            St_HeaderVM.NetTotalDiscountLocal = St_HeaderObj.NetTotalDiscountLocal;
            St_HeaderVM.DiscountPercentage = St_HeaderObj.DiscountPercentage;
            St_HeaderVM.NetTotalLocalAfterDiscount = St_HeaderObj.NetTotalLocalAfterDiscount;
            St_HeaderVM.NetTotalTaxLocal = St_HeaderObj.NetTotalTaxLocal;
            St_HeaderVM.NetTotalLocal = St_HeaderObj.NetTotalLocal;
            if (St_HeaderVM.CurrencyID == 1)
            {
                St_HeaderVM.NetTotalForeignBeforDiscount = 0;
                St_HeaderVM.NetTotalLineDiscountForeign = 0;
                St_HeaderVM.NetTotalForeignAfterLineDiscount = 0;
                St_HeaderVM.NetTotalTaxAfterLineDiscounForeign = 0;
                St_HeaderVM.NetTotalAfterLineDiscountBeforDiscountAllForeign = 0;
                St_HeaderVM.NetTotalDiscountForeign = 0;
                St_HeaderVM.NetTotalForeignAfterDiscount = 0;
                St_HeaderVM.NetTotalTaxForeign = 0;
                St_HeaderVM.NetTotalForeign = 0;
            }
            else
            {
                St_HeaderVM.NetTotalForeignBeforDiscount = St_HeaderVM.NetTotalLocalBeforDiscount / St_HeaderVM.ConversionFactor;
                St_HeaderVM.NetTotalLineDiscountForeign = St_HeaderVM.NetTotalLineDiscountLocal / St_HeaderVM.ConversionFactor;
                St_HeaderVM.NetTotalForeignAfterLineDiscount = St_HeaderVM.NetTotalLocalAfterLineDiscount / St_HeaderVM.ConversionFactor;
                St_HeaderVM.NetTotalTaxAfterLineDiscounForeign = St_HeaderVM.NetTotalTaxAfterLineDiscountLocal / St_HeaderVM.ConversionFactor;
                St_HeaderVM.NetTotalAfterLineDiscountBeforDiscountAllForeign = St_HeaderVM.NetTotalAfterLineDiscountBeforDiscountAllLocal / St_HeaderVM.ConversionFactor;
                St_HeaderVM.NetTotalDiscountForeign = St_HeaderVM.NetTotalDiscountLocal / St_HeaderVM.ConversionFactor;
                St_HeaderVM.NetTotalForeignAfterDiscount = St_HeaderVM.NetTotalLocalAfterDiscount / St_HeaderVM.ConversionFactor;
                St_HeaderVM.NetTotalTaxForeign = St_HeaderVM.NetTotalTaxLocal / St_HeaderVM.ConversionFactor;
                St_HeaderVM.NetTotalForeign = St_HeaderVM.NetTotalLocal / St_HeaderVM.ConversionFactor;
            }
            St_HeaderVM.Remark = St_HeaderObj.Remark;
            St_HeaderVM.Hint = St_HeaderObj.Hint;
            St_HeaderVM.OfferNumber = St_HeaderObj.OfferNumber;
            St_HeaderVM.OrderNumber = St_HeaderObj.OrderNumber;
            St_HeaderVM.VoucherNumber = St_HeaderObj.VoucherNumber;
            St_HeaderVM.VHI = St_HeaderObj.VHI;
            St_HeaderVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            St_HeaderVM.TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency;
            St_HeaderVM.WorkWithCostCenter = Company.WorkWithCostCenter;
            St_HeaderVM.DebitAccountNumber = TransactionDebitObj.AccountNumber;
            St_HeaderVM.CreditAccountNumber = TransactionCreditObj.AccountNumber;
            St_HeaderVM.DebitCostNumber = TransactionDebitObj.CostCenter;
            St_HeaderVM.CreditCostNumber = TransactionCreditObj.CostCenter;
            St_HeaderVM.DebitAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionDebitObj.AccountNumber);
            St_HeaderVM.CreditAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionCreditObj.AccountNumber);
            St_HeaderVM.DebitCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionDebitObj.CostCenter);
            St_HeaderVM.CreditCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionCreditObj.CostCenter);
            St_HeaderVM.OrignailTaxAccountNumber = ObjFix.AccountNo;
            St_HeaderVM.OrignailTaxAccountName = ObjFix.AccountName;
            if (HeaderObj.RowCount > 2)
            {
                St_HeaderVM.TaxAccountNumber = TransactionTaxObj.AccountNumber;
                St_HeaderVM.TaxCostNumber = TransactionTaxObj.CostCenter;
                St_HeaderVM.TaxCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionTaxObj.CostCenter);
                St_HeaderVM.TaxAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionTaxObj.AccountNumber);
            }
            St_SaleVoucherDetail report = new St_SaleVoucherDetail();


            string reportFilePath = Server.MapPath("/St_ReportLayout/St_SaleVoucherDetail.repx");
            report.LoadLayoutFromXml(reportFilePath);



            DataSet ds = new DataSet();
            DataTable table1 = new DataTable("St_HeaderVM");
            table1 = FunctionUnit.LINQResultToDataTable(AllItems);

            ds.Tables.Add(table1);

            report.DataSource = ds;
            report.DataMember = "St_HeaderVM";




            St_HeaderVM.sNetTotalLocalAfterLineDiscount = AllItems.Sum(m => (m.TotalLocalAfterLineDiscount)).ToString();
            St_HeaderVM.sNetTotalDiscountLocal = AllItems.Sum(m => (m.TotalDiscountLocal)).ToString();
            St_HeaderVM.sDiscountPercentage = AllItems.Sum(m => (m.DiscountPercentage)).ToString();
            St_HeaderVM.sNetTotalLocalAfterDiscount = AllItems.Sum(m => (m.TotalLocalAfterDiscount)).ToString();
            St_HeaderVM.sNetTotalTaxLocal = AllItems.Sum(m => (m.TotalTaxLocal)).ToString();
            St_HeaderVM.sNetTotalLocal = AllItems.Sum(m => (m.TotalLocal)).ToString();
            St_HeaderVM.sNetTotalForeignAfterLineDiscount = AllItems.Sum(m => (m.TotalForeignAfterLineDiscount)).ToString();
            St_HeaderVM.sNetTotalDiscountForeign = AllItems.Sum(m => (m.TotalDiscountForeign)).ToString();
            St_HeaderVM.sDiscountPercentageForeign = AllItems.Sum(m => (m.NetTotalDiscountForeign)).ToString();
            St_HeaderVM.sNetTotalForeignAfterDiscount = AllItems.Sum(m => (m.TotalForeignAfterDiscount)).ToString();
            St_HeaderVM.sNetTotalTaxForeign = AllItems.Sum(m => (m.TotalTaxForeign)).ToString();
            St_HeaderVM.sNetTotalForeign = AllItems.Sum(m => (m.TotalForeign)).ToString();


            report.Remark.Text = St_HeaderVM.Remark;
            report.Hint.Text = St_HeaderVM.Hint;



            if (CoInfo.TheDecimalPointForTheLocalCurrency == 2)
            {
                St_HeaderVM.sNetTotalLocalAfterLineDiscount = "" + String.Format("{0:n2}", St_HeaderVM.sNetTotalLocalAfterLineDiscount) + "";
                St_HeaderVM.sNetTotalDiscountLocal = "" + String.Format("{0:n2}", St_HeaderVM.sNetTotalDiscountLocal) + "";
                St_HeaderVM.sDiscountPercentage = "" + String.Format("{0:n2}", St_HeaderVM.sDiscountPercentage) + "";
                St_HeaderVM.sNetTotalLocalAfterDiscount = "" + String.Format("{0:n2}", St_HeaderVM.sNetTotalLocalAfterDiscount) + "";
                St_HeaderVM.sNetTotalTaxLocal = "" + String.Format("{0:n2}", St_HeaderVM.sNetTotalTaxLocal) + "";
                St_HeaderVM.sNetTotalLocal = "" + String.Format("{0:n2}", St_HeaderVM.sNetTotalLocal) + "";
                St_HeaderVM.sNetTotalForeignAfterLineDiscount = "" + String.Format("{0:n2}", St_HeaderVM.sNetTotalForeignAfterLineDiscount) + "";
                St_HeaderVM.sNetTotalDiscountForeign = "" + String.Format("{0:n2}", St_HeaderVM.sNetTotalDiscountForeign) + "";
                St_HeaderVM.sDiscountPercentageForeign = "" + String.Format("{0:n2}", St_HeaderVM.sDiscountPercentageForeign) + "";
                St_HeaderVM.sNetTotalForeignAfterDiscount = "" + String.Format("{0:n2}", St_HeaderVM.sNetTotalForeignAfterDiscount) + "";
                St_HeaderVM.sNetTotalTaxForeign = "" + String.Format("{0:n2}", St_HeaderVM.sNetTotalTaxForeign) + "";
                St_HeaderVM.sNetTotalForeign = "" + String.Format("{0:n2}", St_HeaderVM.sNetTotalForeign) + "";

            }
            else
            {
                St_HeaderVM.sNetTotalLocalAfterLineDiscount = "" + String.Format("{0:n3}", St_HeaderVM.sNetTotalLocalAfterLineDiscount) + "";
                St_HeaderVM.sNetTotalDiscountLocal = "" + String.Format("{0:n3}", St_HeaderVM.sNetTotalDiscountLocal) + "";
                St_HeaderVM.sDiscountPercentage = "" + String.Format("{0:n3}", St_HeaderVM.sDiscountPercentage) + "";
                St_HeaderVM.sNetTotalLocalAfterDiscount = "" + String.Format("{0:n3}", St_HeaderVM.sNetTotalLocalAfterDiscount) + "";
                St_HeaderVM.sNetTotalTaxLocal = "" + String.Format("{0:n3}", St_HeaderVM.sNetTotalTaxLocal) + "";
                St_HeaderVM.sNetTotalLocal = "" + String.Format("{0:n3}", St_HeaderVM.sNetTotalLocal) + "";
                St_HeaderVM.sNetTotalForeignAfterLineDiscount = "" + String.Format("{0:n3}", St_HeaderVM.sNetTotalForeignAfterLineDiscount) + "";
                St_HeaderVM.sNetTotalDiscountForeign = "" + String.Format("{0:n3}", St_HeaderVM.sNetTotalDiscountForeign) + "";
                St_HeaderVM.sDiscountPercentageForeign = "" + String.Format("{0:n3}", St_HeaderVM.sDiscountPercentageForeign) + "";
                St_HeaderVM.sNetTotalForeignAfterDiscount = "" + String.Format("{0:n3}", St_HeaderVM.sNetTotalForeignAfterDiscount) + "";
                St_HeaderVM.sNetTotalTaxForeign = "" + String.Format("{0:n3}", St_HeaderVM.sNetTotalTaxForeign) + "";
                St_HeaderVM.sNetTotalForeign = "" + String.Format("{0:n3}", St_HeaderVM.sNetTotalForeign) + "";
            }



            report.Sum3.Text = St_HeaderVM.sNetTotalLocalAfterLineDiscount;
            report.Sum2.Text = St_HeaderVM.sNetTotalDiscountLocal.ToString();
            report.Sum.Text = St_HeaderVM.sDiscountPercentage;
            report.Sum4.Text = St_HeaderVM.sNetTotalLocalAfterDiscount;
            report.Sum5.Text = St_HeaderVM.sNetTotalTaxLocal;
            report.Sum6.Text = St_HeaderVM.sNetTotalLocal;


            report.Sum7.Text = St_HeaderVM.sNetTotalForeignAfterLineDiscount;
            report.Sum8.Text = St_HeaderVM.sNetTotalDiscountForeign;
            report.Sum10.Text = St_HeaderVM.sDiscountPercentageForeign;
            report.Sum11.Text = St_HeaderVM.sNetTotalForeignAfterDiscount;
            report.Sum12.Text = St_HeaderVM.sNetTotalTaxForeign;
            report.Sum13.Text = St_HeaderVM.sNetTotalForeign;


            report.LbCoName.Text = CoInfo.ArabicName;
            report.LbVhDate.Text = St_HeaderVM.VoucherDate.ToString("dd/MM/yyyy");
            report.LbVhNo.Text = St_HeaderVM.VoucherNumber.ToString();

            report.StockNo.Text = St_HeaderVM.StockCode;

            if (!String.IsNullOrEmpty(St_HeaderVM.StockCode))
            {
                var AccName = _unitOfWork.St_Warehouse.GetSt_WarehouseByID(UserInfo.fCompanyId, St_HeaderVM.StockCode);
                if (Resources.Resource.CurLang == "Arb")
                {
                    report.StockNo.Text = AccName.ArabicName;
                }
                else
                {
                    report.StockNo.Text = AccName.EnglishName;
                }

            }

            report.OrderNo.Text = St_HeaderVM.OrderNumber;

            if (St_HeaderVM.VoucherCase != 0)
            {
                if (St_HeaderVM.VoucherCase == 1)
                {
                    report.VoucherCase.Text = Resources.Resource.Cash;
                }
                else
                {
                    report.VoucherCase.Text = Resources.Resource.VoucherCredit;

                }
            }

            if (St_HeaderVM.SaleID != 0)
            {
                report.SaleMan.Text = String.Format("{0} {1}", Resources.Resource.SaleManName, St_HeaderVM.SaleID.ToString());

            }
            else
            {
                report.SaleMan.Text = " ";

            }


            report.OfferNo.Text = St_HeaderVM.OfferNumber;

            if (St_HeaderVM.CurrencyID != 0)
            {
                var CurrencyObj = _unitOfWork.Currency.GetCurrencyByID(UserInfo.fCompanyId, St_HeaderVM.CurrencyID);
                if (Resources.Resource.CurLang == "Arb")
                {
                    report.Currency.Text = CurrencyObj.ArabicName;

                }

                else
                {
                    report.Currency.Text = CurrencyObj.EnglishName;
                }

            }


            report.CurrencyValue.Text = St_HeaderVM.ConversionFactor.ToString();
            report.OriginalVo.Text = St_HeaderVM.OriginalVoucherNumber;

            report.DueDate.Text = St_HeaderVM.DueDate.ToString("dd/MM/yyyy");

            report.CreditNo.Text = St_HeaderVM.CreditAccountNumber;
            report.CreditName.Text = St_HeaderVM.CreditAccountName;

            report.DebitNo.Text = St_HeaderVM.DebitAccountNumber;
            report.DebitName.Text = St_HeaderVM.DebitAccountName;

            report.CreditCostNo.Text = St_HeaderVM.CreditCostNumber;
            report.CreditCostName.Text = St_HeaderVM.CreditCostName;

            report.DebitCostNo.Text = St_HeaderVM.DebitCostNumber;
            report.DebitCostName.Text = St_HeaderVM.DebitCostName;

            report.TaxNo.Text = St_HeaderVM.TaxAccountNumber;
            report.TaxName.Text = St_HeaderVM.TaxAccountName;

            report.TaxCostNo.Text = St_HeaderVM.TaxCostNumber;
            report.TaxCostName.Text = St_HeaderVM.TaxCostName;


            if (St_HeaderVM.TaxType == 1)
            {
                report.TaxType.Text = Resources.Resource.Taxable;


            }
            else if (St_HeaderVM.TaxType == 2)
            {
                report.TaxType.Text = Resources.Resource.TaxableByZero;

            }
            else
            {
                report.TaxType.Text = Resources.Resource.TaxExempt;

            }



            report.SupplierNo.Text = St_HeaderVM.AccountNumber;
            report.SupplierName.Text = St_HeaderVM.AccountName;




            report.CreateDocument();



            return View("ReportPDF", report);
        }


        public ActionResult GetAllSt_ReturnSaleVoucherPDF(string id, string id2, string id3, string id4, string id5, int id6, int id7, int id8)
        {
            try
            {
                St_HeaderVM Obj = new St_HeaderVM();
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);

                DateTime fromDate = DateTime.Parse(id.Substring(0, 2) + "/" + id.Substring(2, 2) + "/" + id.Substring(4, 4));
                DateTime toDate = DateTime.Parse(id2.Substring(0, 2) + "/" + id2.Substring(2, 2) + "/" + id2.Substring(4, 4));
                Obj.FromDate = fromDate;
                Obj.ToDate = toDate;
                Obj.VoucherNumber = id3;
                Obj.AccountNumber = id4;
                Obj.StockCode = id5;
                Obj.TaxType = id6;
                Obj.VoucherCase = id7;
                Obj.SaleID = id8;

                if (Obj.VoucherNumber == "0")
                {
                    Obj.VoucherNumber = "";
                }
                if (Obj.AccountNumber == "0")
                {
                    Obj.AccountNumber = "";
                }

                if (Obj.StockCode == "0")
                {
                    Obj.StockCode = "";
                }
                var AllSt_ReturnSaleVoucher = _unitOfWork.NativeSql.GetAllSt_ReturnSaleVoucher(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                if (AllSt_ReturnSaleVoucher == null)
                {
                    return Json(new List<St_HeaderVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.VoucherNumber))
                {
                    AllSt_ReturnSaleVoucher = AllSt_ReturnSaleVoucher.Where(m => m.VoucherNumber == Obj.VoucherNumber).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.AccountNumber))
                {
                    AllSt_ReturnSaleVoucher = AllSt_ReturnSaleVoucher.Where(m => m.AccountNumber == Obj.AccountNumber).ToList();
                }
                if (Obj.TaxType != 0)
                {
                    AllSt_ReturnSaleVoucher = AllSt_ReturnSaleVoucher.Where(m => m.TaxType == Obj.TaxType).ToList();
                }
                if (Obj.VoucherCase != 0)
                {
                    AllSt_ReturnSaleVoucher = AllSt_ReturnSaleVoucher.Where(m => m.VoucherCase == Obj.VoucherCase).ToList();
                }
                if (Obj.SaleID != 0)
                {
                    AllSt_ReturnSaleVoucher = AllSt_ReturnSaleVoucher.Where(m => m.SaleID == Obj.SaleID).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.StockCode))
                {
                    AllSt_ReturnSaleVoucher = AllSt_ReturnSaleVoucher.Where(m => m.StockCode == Obj.StockCode).ToList();
                }
                DataSet ds = new DataSet();
                DataTable table1 = new DataTable("St_HeaderVM");
                table1 = FunctionUnit.LINQResultToDataTable(AllSt_ReturnSaleVoucher);

                ds.Tables.Add(table1);

                var report = new St_ReturnSaleVoucherReport();

                string reportFilePath = Server.MapPath("/St_ReportLayout/St_ReturnSaleVoucherReport.repx");
                report.LoadLayoutFromXml(reportFilePath);

                report.LbCoName.Text = CoInfo.ArabicName;


                report.DataSource = ds;
                report.DataMember = "St_HeaderVM";

                report.LbFromDate.Text = Obj.FromDate.ToString("dd/MM/yyyy");
                report.LbTODate.Text = Obj.ToDate.ToString("dd/MM/yyyy");
                report.LbVoucherNo.Text = Obj.VoucherNumber;

                if (Obj.VoucherCase != 0)
                {
                    if (Obj.VoucherCase == 1)
                    {
                        report.VoucherCase.Text = Resources.Resource.Cash;
                    }
                    else
                    {
                        report.VoucherCase.Text = Resources.Resource.VoucherCredit;

                    }

                }

                if (Obj.TaxType != 0)
                {
                    if (Obj.TaxType == 1)
                    {
                        report.TaxType.Text = Resources.Resource.Taxable;


                    }
                    else if (Obj.TaxType == 2)
                    {
                        report.TaxType.Text = Resources.Resource.TaxableByZero;

                    }
                    else
                    {
                        report.TaxType.Text = Resources.Resource.TaxExempt;

                    }

                }

                report.StockCase.Text = Obj.StockCode;

                if (!String.IsNullOrEmpty(Obj.StockCode))
                {
                    var AccName = _unitOfWork.St_Warehouse.GetSt_WarehouseByID(UserInfo.fCompanyId, Obj.StockCode);
                    if (Resources.Resource.CurLang == "Arb")
                    {
                        report.StockCase.Text = AccName.ArabicName;
                    }
                    else
                    {
                        report.StockCase.Text = AccName.EnglishName;
                    }

                }



                report.ToAccountNumber.Text = Obj.AccountNumber;


                if (!String.IsNullOrEmpty(Obj.AccountNumber))
                {
                    var AccName = _unitOfWork.ChartOfAccount.GetAccountByID(UserInfo.fCompanyId, Obj.AccountNumber);
                    if (Resources.Resource.CurLang == "Arb")
                    {
                        report.AccountName.Text = AccName.ArabicName;
                    }
                    else
                    {
                        report.AccountName.Text = AccName.EnglishName;
                    }

                }

                if (Obj.SaleID != 0)
                {
                    report.SaleMan.Text = String.Format("{0} {1}", Resources.Resource.SaleManName, Obj.SaleID.ToString());

                }
                else
                {
                    report.SaleMan.Text = " ";

                }



                report.CreateDocument();


                return View("ReportPDF", report);
            }
            catch (Exception ex)
            {
                string err = ex.Message;

                List<St_HeaderVM> VMList = new List<St_HeaderVM>();

                St_ReturnSaleVoucherReport report = new St_ReturnSaleVoucherReport();

                string reportFilePath = Server.MapPath("/St_ReportLayout/St_ReturnSaleVoucherReport.repx");
                report.LoadLayoutFromXml(reportFilePath);

                DataSet ds = new DataSet();
                DataTable table1 = new DataTable("St_HeaderVM");
                table1 = FunctionUnit.LINQResultToDataTable(VMList);

                ds.Tables.Add(table1);




                report.DataSource = ds;
                report.DataMember = "St_HeaderVM";
                // report.LbPrintTime.Text = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, FunctionUnit.Jordan_Time_Zone).ToString();

                report.CreateDocument();

                return View("ReportPDF", report);

            }

        }


        public ActionResult St_ReturnSaleVoucherDetailPDF(string id, string id2, string id3, string id4, string id5)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var AllItems = _unitOfWork.NativeSql.GetSt_Transacation(id, int.Parse(id2), UserInfo.fCompanyId, int.Parse(id3), int.Parse(id4), id5);
            var St_Warehouse = _unitOfWork.St_Warehouse.GetAllSt_Warehouse(UserInfo.fCompanyId);
            var Sales = _unitOfWork.Sale.GetAllSale(UserInfo.fCompanyId);
            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId);
            var St_HeaderObj = _unitOfWork.St_Header.GetSt_HeaderDataById(id, UserInfo.fCompanyId, int.Parse(id2), int.Parse(id3), int.Parse(id4), id5);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var HeaderObj = _unitOfWork.Header.GetHeaderDataById(id, UserInfo.fCompanyId, int.Parse(id2), int.Parse(id3), int.Parse(id4));
            var TransactionDebitObj = _unitOfWork.NativeSql.GetTransactionsDetailDebit(id, UserInfo.fCompanyId, int.Parse(id2), 1, int.Parse(id4));
            var TransactionCreditObj = _unitOfWork.NativeSql.GetTransactionsDetailCredit(id, UserInfo.fCompanyId, int.Parse(id2), 2, int.Parse(id4));
            var TransactionTaxObj = _unitOfWork.NativeSql.GetTransactionsDetailTax(id, UserInfo.fCompanyId, int.Parse(id2), 3, int.Parse(id4));
            var ObjGet = _unitOfWork.St_OtherAccount.GetSt_OtherAccountByID(UserInfo.fCompanyId);
            FixAccountInfoVM ObjFix = new FixAccountInfoVM
            {
                AccountNo = "",
                AccountName = ""
            };
            if (ObjGet != null)
            {
                ObjFix.AccountNo = ObjGet.SalesTaxAccountNumber;
                ObjFix.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.PurchaseTaxReturnAccountNumber);
            }
            var St_HeaderVM = new St_HeaderVM { };
            St_HeaderVM.St_Warehouse = St_Warehouse;
            St_HeaderVM.StockCode = St_HeaderObj.StockCode;
            St_HeaderVM.SaleMan = Sales;
            St_HeaderVM.SaleID = St_HeaderObj.SaleID;
            St_HeaderVM.Currency = Currency;
            St_HeaderVM.CurrencyID = St_HeaderObj.CurrencyID;
            St_HeaderVM.ConversionFactor = St_HeaderObj.ConversionFactor;
            St_HeaderVM.CompanyTransactionKindNo = St_HeaderObj.CompanyTransactionKindNo;
            St_HeaderVM.TransactionKindNo = St_HeaderObj.TransactionKindNo;
            St_HeaderVM.CompanyYear = St_HeaderObj.CompanyYear;
            St_HeaderVM.TaxType = St_HeaderObj.TaxType;
            St_HeaderVM.VoucherCase = St_HeaderObj.VoucherCase;
            St_HeaderVM.VoucherDate = St_HeaderObj.VoucherDate;
            St_HeaderVM.AccountNumber = St_HeaderObj.AccountNumber;
            St_HeaderVM.AccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, St_HeaderObj.AccountNumber);
            St_HeaderVM.NetTotalLocalBeforDiscount = St_HeaderObj.NetTotalLocalBeforDiscount;
            St_HeaderVM.NetTotalLineDiscountLocal = St_HeaderObj.NetTotalLineDiscountLocal;
            St_HeaderVM.NetTotalLocalAfterLineDiscount = St_HeaderObj.NetTotalLocalAfterLineDiscount;
            St_HeaderVM.NetTotalTaxAfterLineDiscountLocal = St_HeaderObj.NetTotalTaxAfterLineDiscountLocal;
            St_HeaderVM.NetTotalAfterLineDiscountBeforDiscountAllLocal = St_HeaderObj.NetTotalAfterLineDiscountBeforDiscountAllLocal;
            St_HeaderVM.NetTotalDiscountLocal = St_HeaderObj.NetTotalDiscountLocal;
            St_HeaderVM.DiscountPercentage = St_HeaderObj.DiscountPercentage;
            St_HeaderVM.NetTotalLocalAfterDiscount = St_HeaderObj.NetTotalLocalAfterDiscount;
            St_HeaderVM.NetTotalTaxLocal = St_HeaderObj.NetTotalTaxLocal;
            St_HeaderVM.NetTotalLocal = St_HeaderObj.NetTotalLocal;
            if (St_HeaderVM.CurrencyID == 1)
            {
                St_HeaderVM.NetTotalForeignBeforDiscount = 0;
                St_HeaderVM.NetTotalLineDiscountForeign = 0;
                St_HeaderVM.NetTotalForeignAfterLineDiscount = 0;
                St_HeaderVM.NetTotalTaxAfterLineDiscounForeign = 0;
                St_HeaderVM.NetTotalAfterLineDiscountBeforDiscountAllForeign = 0;
                St_HeaderVM.NetTotalDiscountForeign = 0;
                St_HeaderVM.NetTotalForeignAfterDiscount = 0;
                St_HeaderVM.NetTotalTaxForeign = 0;
                St_HeaderVM.NetTotalForeign = 0;
            }
            else
            {
                St_HeaderVM.NetTotalForeignBeforDiscount = St_HeaderVM.NetTotalLocalBeforDiscount / St_HeaderVM.ConversionFactor;
                St_HeaderVM.NetTotalLineDiscountForeign = St_HeaderVM.NetTotalLineDiscountLocal / St_HeaderVM.ConversionFactor;
                St_HeaderVM.NetTotalForeignAfterLineDiscount = St_HeaderVM.NetTotalLocalAfterLineDiscount / St_HeaderVM.ConversionFactor;
                St_HeaderVM.NetTotalTaxAfterLineDiscounForeign = St_HeaderVM.NetTotalTaxAfterLineDiscountLocal / St_HeaderVM.ConversionFactor;
                St_HeaderVM.NetTotalAfterLineDiscountBeforDiscountAllForeign = St_HeaderVM.NetTotalAfterLineDiscountBeforDiscountAllLocal / St_HeaderVM.ConversionFactor;
                St_HeaderVM.NetTotalDiscountForeign = St_HeaderVM.NetTotalDiscountLocal / St_HeaderVM.ConversionFactor;
                St_HeaderVM.NetTotalForeignAfterDiscount = St_HeaderVM.NetTotalLocalAfterDiscount / St_HeaderVM.ConversionFactor;
                St_HeaderVM.NetTotalTaxForeign = St_HeaderVM.NetTotalTaxLocal / St_HeaderVM.ConversionFactor;
                St_HeaderVM.NetTotalForeign = St_HeaderVM.NetTotalLocal / St_HeaderVM.ConversionFactor;
            }
            St_HeaderVM.Remark = St_HeaderObj.Remark;
            St_HeaderVM.Hint = St_HeaderObj.Hint;
            St_HeaderVM.OriginalVoucherNumber = St_HeaderVM.OriginalVoucherNumber;
            St_HeaderVM.VoucherNumber = St_HeaderObj.VoucherNumber;
            St_HeaderVM.VHI = St_HeaderObj.VHI;
            St_HeaderVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            St_HeaderVM.TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency;
            St_HeaderVM.WorkWithCostCenter = Company.WorkWithCostCenter;
            St_HeaderVM.DebitAccountNumber = TransactionDebitObj.AccountNumber;
            St_HeaderVM.CreditAccountNumber = TransactionCreditObj.AccountNumber;
            St_HeaderVM.DebitCostNumber = TransactionDebitObj.CostCenter;
            St_HeaderVM.CreditCostNumber = TransactionCreditObj.CostCenter;
            St_HeaderVM.DebitAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionDebitObj.AccountNumber);
            St_HeaderVM.CreditAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionCreditObj.AccountNumber);
            St_HeaderVM.DebitCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionDebitObj.CostCenter);
            St_HeaderVM.CreditCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionCreditObj.CostCenter);
            St_HeaderVM.OrignailTaxAccountNumber = ObjFix.AccountNo;
            St_HeaderVM.OrignailTaxAccountName = ObjFix.AccountName;
            if (HeaderObj.RowCount > 2)
            {
                St_HeaderVM.TaxAccountNumber = TransactionTaxObj.AccountNumber;
                St_HeaderVM.TaxCostNumber = TransactionTaxObj.CostCenter;
                St_HeaderVM.TaxCostName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionTaxObj.CostCenter);
                St_HeaderVM.TaxAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionTaxObj.AccountNumber);
            }

            St_ReturnSaleVoucherDetail report = new St_ReturnSaleVoucherDetail();


            string reportFilePath = Server.MapPath("/St_ReportLayout/St_ReturnSaleVoucherDetail.repx");
            report.LoadLayoutFromXml(reportFilePath);



            DataSet ds = new DataSet();
            DataTable table1 = new DataTable("St_HeaderVM");
            table1 = FunctionUnit.LINQResultToDataTable(AllItems);

            ds.Tables.Add(table1);

            report.DataSource = ds;
            report.DataMember = "St_HeaderVM";




            St_HeaderVM.sNetTotalLocalAfterLineDiscount = AllItems.Sum(m => (m.TotalLocalAfterLineDiscount)).ToString();
            St_HeaderVM.sNetTotalDiscountLocal = AllItems.Sum(m => (m.TotalDiscountLocal)).ToString();
            St_HeaderVM.sDiscountPercentage = AllItems.Sum(m => (m.DiscountPercentage)).ToString();
            St_HeaderVM.sNetTotalLocalAfterDiscount = AllItems.Sum(m => (m.TotalLocalAfterDiscount)).ToString();
            St_HeaderVM.sNetTotalTaxLocal = AllItems.Sum(m => (m.TotalTaxLocal)).ToString();
            St_HeaderVM.sNetTotalLocal = AllItems.Sum(m => (m.TotalLocal)).ToString();
            St_HeaderVM.sNetTotalForeignAfterLineDiscount = AllItems.Sum(m => (m.TotalForeignAfterLineDiscount)).ToString();
            St_HeaderVM.sNetTotalDiscountForeign = AllItems.Sum(m => (m.TotalDiscountForeign)).ToString();
            St_HeaderVM.sDiscountPercentageForeign = AllItems.Sum(m => (m.NetTotalDiscountForeign)).ToString();
            St_HeaderVM.sNetTotalForeignAfterDiscount = AllItems.Sum(m => (m.TotalForeignAfterDiscount)).ToString();
            St_HeaderVM.sNetTotalTaxForeign = AllItems.Sum(m => (m.TotalTaxForeign)).ToString();
            St_HeaderVM.sNetTotalForeign = AllItems.Sum(m => (m.TotalForeign)).ToString();


            report.Remark.Text = St_HeaderVM.Remark;
            report.Hint.Text = St_HeaderVM.Hint;



            if (CoInfo.TheDecimalPointForTheLocalCurrency == 2)
            {
                St_HeaderVM.sNetTotalLocalAfterLineDiscount = "" + String.Format("{0:n2}", St_HeaderVM.sNetTotalLocalAfterLineDiscount) + "";
                St_HeaderVM.sNetTotalDiscountLocal = "" + String.Format("{0:n2}", St_HeaderVM.sNetTotalDiscountLocal) + "";
                St_HeaderVM.sDiscountPercentage = "" + String.Format("{0:n2}", St_HeaderVM.sDiscountPercentage) + "";
                St_HeaderVM.sNetTotalLocalAfterDiscount = "" + String.Format("{0:n2}", St_HeaderVM.sNetTotalLocalAfterDiscount) + "";
                St_HeaderVM.sNetTotalTaxLocal = "" + String.Format("{0:n2}", St_HeaderVM.sNetTotalTaxLocal) + "";
                St_HeaderVM.sNetTotalLocal = "" + String.Format("{0:n2}", St_HeaderVM.sNetTotalLocal) + "";
                St_HeaderVM.sNetTotalForeignAfterLineDiscount = "" + String.Format("{0:n2}", St_HeaderVM.sNetTotalForeignAfterLineDiscount) + "";
                St_HeaderVM.sNetTotalDiscountForeign = "" + String.Format("{0:n2}", St_HeaderVM.sNetTotalDiscountForeign) + "";
                St_HeaderVM.sDiscountPercentageForeign = "" + String.Format("{0:n2}", St_HeaderVM.sDiscountPercentageForeign) + "";
                St_HeaderVM.sNetTotalForeignAfterDiscount = "" + String.Format("{0:n2}", St_HeaderVM.sNetTotalForeignAfterDiscount) + "";
                St_HeaderVM.sNetTotalTaxForeign = "" + String.Format("{0:n2}", St_HeaderVM.sNetTotalTaxForeign) + "";
                St_HeaderVM.sNetTotalForeign = "" + String.Format("{0:n2}", St_HeaderVM.sNetTotalForeign) + "";

            }
            else
            {
                St_HeaderVM.sNetTotalLocalAfterLineDiscount = "" + String.Format("{0:n3}", St_HeaderVM.sNetTotalLocalAfterLineDiscount) + "";
                St_HeaderVM.sNetTotalDiscountLocal = "" + String.Format("{0:n3}", St_HeaderVM.sNetTotalDiscountLocal) + "";
                St_HeaderVM.sDiscountPercentage = "" + String.Format("{0:n3}", St_HeaderVM.sDiscountPercentage) + "";
                St_HeaderVM.sNetTotalLocalAfterDiscount = "" + String.Format("{0:n3}", St_HeaderVM.sNetTotalLocalAfterDiscount) + "";
                St_HeaderVM.sNetTotalTaxLocal = "" + String.Format("{0:n3}", St_HeaderVM.sNetTotalTaxLocal) + "";
                St_HeaderVM.sNetTotalLocal = "" + String.Format("{0:n3}", St_HeaderVM.sNetTotalLocal) + "";
                St_HeaderVM.sNetTotalForeignAfterLineDiscount = "" + String.Format("{0:n3}", St_HeaderVM.sNetTotalForeignAfterLineDiscount) + "";
                St_HeaderVM.sNetTotalDiscountForeign = "" + String.Format("{0:n3}", St_HeaderVM.sNetTotalDiscountForeign) + "";
                St_HeaderVM.sDiscountPercentageForeign = "" + String.Format("{0:n3}", St_HeaderVM.sDiscountPercentageForeign) + "";
                St_HeaderVM.sNetTotalForeignAfterDiscount = "" + String.Format("{0:n3}", St_HeaderVM.sNetTotalForeignAfterDiscount) + "";
                St_HeaderVM.sNetTotalTaxForeign = "" + String.Format("{0:n3}", St_HeaderVM.sNetTotalTaxForeign) + "";
                St_HeaderVM.sNetTotalForeign = "" + String.Format("{0:n3}", St_HeaderVM.sNetTotalForeign) + "";
            }



            report.Sum3.Text = St_HeaderVM.sNetTotalLocalAfterLineDiscount;
            report.Sum2.Text = St_HeaderVM.sNetTotalDiscountLocal.ToString();
            report.Sum.Text = St_HeaderVM.sDiscountPercentage;
            report.Sum4.Text = St_HeaderVM.sNetTotalLocalAfterDiscount;
            report.Sum5.Text = St_HeaderVM.sNetTotalTaxLocal;
            report.Sum6.Text = St_HeaderVM.sNetTotalLocal;


            report.Sum7.Text = St_HeaderVM.sNetTotalForeignAfterLineDiscount;
            report.Sum8.Text = St_HeaderVM.sNetTotalDiscountForeign;
            report.Sum10.Text = St_HeaderVM.sDiscountPercentageForeign;
            report.Sum11.Text = St_HeaderVM.sNetTotalForeignAfterDiscount;
            report.Sum12.Text = St_HeaderVM.sNetTotalTaxForeign;
            report.Sum13.Text = St_HeaderVM.sNetTotalForeign;


            report.LbCoName.Text = CoInfo.ArabicName;
            report.LbVhDate.Text = St_HeaderVM.VoucherDate.ToString("dd/MM/yyyy");
            report.LbVhNo.Text = St_HeaderVM.VoucherNumber.ToString();

            report.StockNo.Text = St_HeaderVM.StockCode;

            if (!String.IsNullOrEmpty(St_HeaderVM.StockCode))
            {
                var AccName = _unitOfWork.St_Warehouse.GetSt_WarehouseByID(UserInfo.fCompanyId, St_HeaderVM.StockCode);
                if (Resources.Resource.CurLang == "Arb")
                {
                    report.StockNo.Text = AccName.ArabicName;
                }
                else
                {
                    report.StockNo.Text = AccName.EnglishName;
                }

            }


            if (St_HeaderVM.VoucherCase != 0)
            {
                if (St_HeaderVM.VoucherCase == 1)
                {
                    report.VoucherCase.Text = Resources.Resource.Cash;
                }
                else
                {
                    report.VoucherCase.Text = Resources.Resource.VoucherCredit;

                }
            }

            if (St_HeaderVM.SaleID != 0)
            {
                report.SaleMan.Text = String.Format("{0} {1}", Resources.Resource.SaleManName, St_HeaderVM.SaleID.ToString());

            }
            else
            {
                report.SaleMan.Text = " ";

            }

            if (St_HeaderVM.CurrencyID != 0)
            {
                var CurrencyObj = _unitOfWork.Currency.GetCurrencyByID(UserInfo.fCompanyId, St_HeaderVM.CurrencyID);
                if (Resources.Resource.CurLang == "Arb")
                {
                    report.Currency.Text = CurrencyObj.ArabicName;

                }

                else
                {
                    report.Currency.Text = CurrencyObj.EnglishName;
                }

            }


            report.CurrencyValue.Text = St_HeaderVM.ConversionFactor.ToString();
            report.OriginalVo.Text = St_HeaderVM.OriginalVoucherNumber;


            report.CreditNo.Text = St_HeaderVM.CreditAccountNumber;
            report.CreditName.Text = St_HeaderVM.CreditAccountName;

            report.DebitNo.Text = St_HeaderVM.DebitAccountNumber;
            report.DebitName.Text = St_HeaderVM.DebitAccountName;

            report.CreditCostNo.Text = St_HeaderVM.CreditCostNumber;
            report.CreditCostName.Text = St_HeaderVM.CreditCostName;

            report.DebitCostNo.Text = St_HeaderVM.DebitCostNumber;
            report.DebitCostName.Text = St_HeaderVM.DebitCostName;

            report.TaxNo.Text = St_HeaderVM.TaxAccountNumber;
            report.TaxName.Text = St_HeaderVM.TaxAccountName;

            report.TaxCostNo.Text = St_HeaderVM.TaxCostNumber;
            report.TaxCostName.Text = St_HeaderVM.TaxCostName;


            if (St_HeaderVM.TaxType == 1)
            {
                report.TaxType.Text = Resources.Resource.Taxable;


            }
            else if (St_HeaderVM.TaxType == 2)
            {
                report.TaxType.Text = Resources.Resource.TaxableByZero;

            }
            else
            {
                report.TaxType.Text = Resources.Resource.TaxExempt;

            }



            report.SupplierNo.Text = St_HeaderVM.AccountNumber;
            report.SupplierName.Text = St_HeaderVM.AccountName;




            report.CreateDocument();



            return View("ReportPDF", report);
        }

        public ActionResult GetAllSt_OutVoucherPDF(string id, string id2, string id3, string id4)
        {
            try
            {
                St_HeaderVM Obj = new St_HeaderVM();
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);

                DateTime fromDate = DateTime.Parse(id.Substring(0, 2) + "/" + id.Substring(2, 2) + "/" + id.Substring(4, 4));
                DateTime toDate = DateTime.Parse(id2.Substring(0, 2) + "/" + id2.Substring(2, 2) + "/" + id2.Substring(4, 4));
                Obj.FromDate = fromDate;
                Obj.ToDate = toDate;
                Obj.VoucherNumber = id3;
                Obj.StockCode = id4;


                if (Obj.VoucherNumber == "0")
                {
                    Obj.VoucherNumber = "";
                }

                if (Obj.StockCode == "0")
                {
                    Obj.StockCode = "";
                } 
                var AllSt_OutVoucher = _unitOfWork.NativeSql.GetAllSt_OutVoucher(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                if (AllSt_OutVoucher == null)
                {
                    return Json(new List<St_HeaderVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.VoucherNumber))
                {
                    AllSt_OutVoucher = AllSt_OutVoucher.Where(m => m.VoucherNumber == Obj.VoucherNumber).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.StockCode))
                {
                    AllSt_OutVoucher = AllSt_OutVoucher.Where(m => m.StockCode == Obj.StockCode).ToList();
                }
                DataSet ds = new DataSet();
                DataTable table1 = new DataTable("St_HeaderVM");
                table1 = FunctionUnit.LINQResultToDataTable(AllSt_OutVoucher);

                ds.Tables.Add(table1);

                var report = new St_OutVoucherReport();

                string reportFilePath = Server.MapPath("/St_ReportLayout/St_OutVoucherReport.repx");
                report.LoadLayoutFromXml(reportFilePath);

                report.LbCoName.Text = CoInfo.ArabicName;


                report.DataSource = ds;
                report.DataMember = "St_HeaderVM";

                report.LbFromDate.Text = Obj.FromDate.ToString("dd/MM/yyyy");
                report.LbTODate.Text = Obj.ToDate.ToString("dd/MM/yyyy");
                report.LbVoucherNo.Text = Obj.VoucherNumber;




                report.StockCase.Text = Obj.StockCode;

                if (!String.IsNullOrEmpty(Obj.StockCode))
                {
                    var AccName = _unitOfWork.St_Warehouse.GetSt_WarehouseByID(UserInfo.fCompanyId, Obj.StockCode);
                    if (Resources.Resource.CurLang == "Arb")
                    {
                        report.StockCase.Text = AccName.ArabicName;
                    }
                    else
                    {
                        report.StockCase.Text = AccName.EnglishName;
                    }

                }


                report.CreateDocument();




                return View("ReportPDF", report);
            }
            catch (Exception ex)
            {
                string err = ex.Message;

                List<St_HeaderVM> VMList = new List<St_HeaderVM>();

                St_OutVoucherReport report = new St_OutVoucherReport();

                string reportFilePath = Server.MapPath("/St_ReportLayout/St_OutVoucherReport.repx");
                report.LoadLayoutFromXml(reportFilePath);

                DataSet ds = new DataSet();
                DataTable table1 = new DataTable("St_HeaderVM");
                table1 = FunctionUnit.LINQResultToDataTable(VMList);

                ds.Tables.Add(table1);




                report.DataSource = ds;
                report.DataMember = "St_HeaderVM";
                // report.LbPrintTime.Text = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, FunctionUnit.Jordan_Time_Zone).ToString();

                report.CreateDocument();

                return View("ReportPDF", report);
            }

        }


        public ActionResult St_OutVoucherDetailPDF(string id, string id2, string id3, string id4, string id5)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var St_Warehouse = _unitOfWork.St_Warehouse.GetAllSt_Warehouse(UserInfo.fCompanyId);
            var AllItems = _unitOfWork.NativeSql.GetSt_Transacation(id, int.Parse(id2), UserInfo.fCompanyId, int.Parse(id3), int.Parse(id4), id5);
            var St_HeaderObj = _unitOfWork.St_Header.GetSt_HeaderDataById(id, UserInfo.fCompanyId, int.Parse(id2), int.Parse(id3), int.Parse(id4), id5);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var HeaderObj = _unitOfWork.Header.GetHeaderDataById(id, UserInfo.fCompanyId, int.Parse(id2), int.Parse(id3), int.Parse(id4));
            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);

            var St_HeaderVM = new St_HeaderVM { };
            St_HeaderVM.St_Warehouse = St_Warehouse;
            St_HeaderVM.StockCode = St_HeaderObj.StockCode;
            St_HeaderVM.CompanyTransactionKindNo = St_HeaderObj.CompanyTransactionKindNo;
            St_HeaderVM.TransactionKindNo = St_HeaderObj.TransactionKindNo;
            St_HeaderVM.CompanyYear = St_HeaderObj.CompanyYear;
            St_HeaderVM.TaxType = St_HeaderObj.TaxType;
            St_HeaderVM.VoucherCase = St_HeaderObj.VoucherCase;
            St_HeaderVM.VoucherDate = St_HeaderObj.VoucherDate;
            St_HeaderVM.DueDate = St_HeaderObj.VoucherDate;
            St_HeaderVM.CurrencyID = St_HeaderObj.CurrencyID;
            St_HeaderVM.ConversionFactor = St_HeaderObj.ConversionFactor;
            St_HeaderVM.NetTotalLocalBeforDiscount = St_HeaderObj.NetTotalLocalBeforDiscount;
            St_HeaderVM.NetTotalLineDiscountLocal = St_HeaderObj.NetTotalLineDiscountLocal;
            St_HeaderVM.NetTotalLocalAfterLineDiscount = St_HeaderObj.NetTotalLocalAfterLineDiscount;
            St_HeaderVM.NetTotalTaxAfterLineDiscountLocal = St_HeaderObj.NetTotalTaxAfterLineDiscountLocal;
            St_HeaderVM.NetTotalAfterLineDiscountBeforDiscountAllLocal = St_HeaderObj.NetTotalAfterLineDiscountBeforDiscountAllLocal;
            St_HeaderVM.NetTotalDiscountLocal = St_HeaderObj.NetTotalDiscountLocal;
            St_HeaderVM.DiscountPercentage = St_HeaderObj.DiscountPercentage;
            St_HeaderVM.NetTotalLocalAfterDiscount = St_HeaderObj.NetTotalLocalAfterDiscount;
            St_HeaderVM.NetTotalTaxLocal = St_HeaderObj.NetTotalTaxLocal;
            St_HeaderVM.NetTotalLocal = St_HeaderObj.NetTotalLocal;
            St_HeaderVM.Remark = St_HeaderObj.Remark;
            St_HeaderVM.Hint = St_HeaderObj.Hint;
            St_HeaderVM.VoucherNumber = St_HeaderObj.VoucherNumber;
            St_HeaderVM.VHI = St_HeaderObj.VHI;
            St_HeaderVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;

            St_OutVoucherDetail report = new St_OutVoucherDetail();

            // cokin 
            string reportFilePath = Server.MapPath("/St_ReportLayout/St_OutVoucherDetail.repx");
            report.LoadLayoutFromXml(reportFilePath);



            DataSet ds = new DataSet();
            DataTable table1 = new DataTable("St_HeaderVM");
            table1 = FunctionUnit.LINQResultToDataTable(AllItems);

            ds.Tables.Add(table1);

            report.DataSource = ds;
            report.DataMember = "St_HeaderVM";


            report.LbCoName.Text = CoInfo.ArabicName;
            report.LbVhDate.Text = St_HeaderVM.VoucherDate.ToString("dd/MM/yyyy");
            report.LbVhNo.Text = St_HeaderVM.VoucherNumber.ToString();

            St_HeaderVM.sNetTotalLocal = AllItems.Sum(m => (m.TotalLocal)).ToString();

            report.StockNo.Text = St_HeaderVM.StockCode;

            if (!String.IsNullOrEmpty(St_HeaderVM.StockCode))
            {
                var AccName = _unitOfWork.St_Warehouse.GetSt_WarehouseByID(UserInfo.fCompanyId, St_HeaderVM.StockCode);
                if (Resources.Resource.CurLang == "Arb")
                {
                    report.StockNo.Text = AccName.ArabicName;
                }
                else
                {
                    report.StockNo.Text = AccName.EnglishName;
                }

            }
            report.Remark.Text = St_HeaderVM.Remark;
            report.Hint.Text = St_HeaderVM.Hint;


            if (CoInfo.TheDecimalPointForTheLocalCurrency == 2)
            {
                St_HeaderVM.sNetTotalLocal = "" + String.Format("{0:n2}", St_HeaderVM.sNetTotalLocal) + "";

            }
            else
            {
                St_HeaderVM.sNetTotalLocal = "" + String.Format("{0:n3}", St_HeaderVM.sNetTotalLocal) + "";
            }

            report.Sum6.Text = St_HeaderVM.sNetTotalLocal;


            report.CreateDocument();



            return View("ReportPDF", report);
        }


        public ActionResult GetAllSt_SpoiledVoucherPDF(string id, string id2, string id3, string id4)
        {
            try
            {
                St_HeaderVM Obj = new St_HeaderVM();
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);

                DateTime fromDate = DateTime.Parse(id.Substring(0, 2) + "/" + id.Substring(2, 2) + "/" + id.Substring(4, 4));
                DateTime toDate = DateTime.Parse(id2.Substring(0, 2) + "/" + id2.Substring(2, 2) + "/" + id2.Substring(4, 4));
                Obj.FromDate = fromDate;
                Obj.ToDate = toDate;
                Obj.VoucherNumber = id3;
                Obj.StockCode = id4;


                if (Obj.VoucherNumber == "0")
                {
                    Obj.VoucherNumber = "";
                }

                if (Obj.StockCode == "0")
                {
                    Obj.StockCode = "";
                }
                var AllSt_SpoiledVoucher = _unitOfWork.NativeSql.GetAllSt_SpoiledVoucher(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                if (AllSt_SpoiledVoucher == null)
                {
                    return Json(new List<St_HeaderVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.VoucherNumber))
                {
                    AllSt_SpoiledVoucher = AllSt_SpoiledVoucher.Where(m => m.VoucherNumber == Obj.VoucherNumber).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.StockCode))
                {
                    AllSt_SpoiledVoucher = AllSt_SpoiledVoucher.Where(m => m.StockCode == Obj.StockCode).ToList();
                }
                DataSet ds = new DataSet();
                DataTable table1 = new DataTable("St_HeaderVM");
                table1 = FunctionUnit.LINQResultToDataTable(AllSt_SpoiledVoucher);

                ds.Tables.Add(table1);

                var report = new St_SpoiledVoucherReport();

                string reportFilePath = Server.MapPath("/St_ReportLayout/St_SpoiledVoucherReport.repx");
                report.LoadLayoutFromXml(reportFilePath);

                report.LbCoName.Text = CoInfo.ArabicName;


                report.DataSource = ds;
                report.DataMember = "St_HeaderVM";

                report.LbFromDate.Text = Obj.FromDate.ToString("dd/MM/yyyy");
                report.LbTODate.Text = Obj.ToDate.ToString("dd/MM/yyyy");
                report.LbVoucherNo.Text = Obj.VoucherNumber;




                report.StockCase.Text = Obj.StockCode;

                if (!String.IsNullOrEmpty(Obj.StockCode))
                {
                    var AccName = _unitOfWork.St_Warehouse.GetSt_WarehouseByID(UserInfo.fCompanyId, Obj.StockCode);
                    if (Resources.Resource.CurLang == "Arb")
                    {
                        report.StockCase.Text = AccName.ArabicName;
                    }
                    else
                    {
                        report.StockCase.Text = AccName.EnglishName;
                    }

                }


                report.CreateDocument();




                return View("ReportPDF", report);
            }
            catch (Exception ex)
            {
                string err = ex.Message;

                List<St_HeaderVM> VMList = new List<St_HeaderVM>();

                St_SpoiledVoucherReport report = new St_SpoiledVoucherReport();

                string reportFilePath = Server.MapPath("/St_ReportLayout/St_SpoiledVoucherReport.repx");
                report.LoadLayoutFromXml(reportFilePath);

                DataSet ds = new DataSet();
                DataTable table1 = new DataTable("St_HeaderVM");
                table1 = FunctionUnit.LINQResultToDataTable(VMList);

                ds.Tables.Add(table1);




                report.DataSource = ds;
                report.DataMember = "St_HeaderVM";
                // report.LbPrintTime.Text = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, FunctionUnit.Jordan_Time_Zone).ToString();

                report.CreateDocument();

                return View("ReportPDF", report);
            }

        }

        public ActionResult St_SpoiledVoucherDetailPDF(string id, string id2, string id3, string id4, string id5)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var St_Warehouse = _unitOfWork.St_Warehouse.GetAllSt_Warehouse(UserInfo.fCompanyId);
            var St_HeaderObj = _unitOfWork.St_Header.GetSt_HeaderDataById(id, UserInfo.fCompanyId, int.Parse(id2), int.Parse(id3), int.Parse(id4), id5);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var AllItems = _unitOfWork.NativeSql.GetSt_Transacation(id, int.Parse(id2), UserInfo.fCompanyId, int.Parse(id3), int.Parse(id4), id5);
            var St_HeaderVM = new St_HeaderVM { };
            St_HeaderVM.St_Warehouse = St_Warehouse;
            St_HeaderVM.StockCode = St_HeaderObj.StockCode;
            St_HeaderVM.CompanyTransactionKindNo = St_HeaderObj.CompanyTransactionKindNo;
            St_HeaderVM.TransactionKindNo = St_HeaderObj.TransactionKindNo;
            St_HeaderVM.CompanyYear = St_HeaderObj.CompanyYear;
            St_HeaderVM.TaxType = St_HeaderObj.TaxType;
            St_HeaderVM.VoucherCase = St_HeaderObj.VoucherCase;
            St_HeaderVM.VoucherDate = St_HeaderObj.VoucherDate;
            St_HeaderVM.DueDate = St_HeaderObj.VoucherDate;
            St_HeaderVM.CurrencyID = St_HeaderObj.CurrencyID;
            St_HeaderVM.ConversionFactor = St_HeaderObj.ConversionFactor;
            St_HeaderVM.NetTotalLocalBeforDiscount = St_HeaderObj.NetTotalLocalBeforDiscount;
            St_HeaderVM.NetTotalLineDiscountLocal = St_HeaderObj.NetTotalLineDiscountLocal;
            St_HeaderVM.NetTotalLocalAfterLineDiscount = St_HeaderObj.NetTotalLocalAfterLineDiscount;
            St_HeaderVM.NetTotalTaxAfterLineDiscountLocal = St_HeaderObj.NetTotalTaxAfterLineDiscountLocal;
            St_HeaderVM.NetTotalAfterLineDiscountBeforDiscountAllLocal = St_HeaderObj.NetTotalAfterLineDiscountBeforDiscountAllLocal;
            St_HeaderVM.NetTotalDiscountLocal = St_HeaderObj.NetTotalDiscountLocal;
            St_HeaderVM.DiscountPercentage = St_HeaderObj.DiscountPercentage;
            St_HeaderVM.NetTotalLocalAfterDiscount = St_HeaderObj.NetTotalLocalAfterDiscount;
            St_HeaderVM.NetTotalTaxLocal = St_HeaderObj.NetTotalTaxLocal;
            St_HeaderVM.NetTotalLocal = St_HeaderObj.NetTotalLocal;
            St_HeaderVM.Remark = St_HeaderObj.Remark;
            St_HeaderVM.Hint = St_HeaderObj.Hint;
            St_HeaderVM.VoucherNumber = St_HeaderObj.VoucherNumber;
            St_HeaderVM.VHI = St_HeaderObj.VHI;
            St_HeaderVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;

            St_SpoiledVoucherDetail report = new St_SpoiledVoucherDetail();

            // cokin 
            string reportFilePath = Server.MapPath("/St_ReportLayout/St_SpoiledVoucherDetail.repx");
            report.LoadLayoutFromXml(reportFilePath);



            DataSet ds = new DataSet();
            DataTable table1 = new DataTable("St_HeaderVM");
            table1 = FunctionUnit.LINQResultToDataTable(AllItems);

            ds.Tables.Add(table1);

            report.DataSource = ds;
            report.DataMember = "St_HeaderVM";


            report.LbCoName.Text = CoInfo.ArabicName;
            report.LbVhDate.Text = St_HeaderVM.VoucherDate.ToString("dd/MM/yyyy");
            report.LbVhNo.Text = St_HeaderVM.VoucherNumber.ToString();

            St_HeaderVM.sNetTotalLocal = AllItems.Sum(m => (m.TotalLocal)).ToString();

            report.StockNo.Text = St_HeaderVM.StockCode;

            if (!String.IsNullOrEmpty(St_HeaderVM.StockCode))
            {
                var AccName = _unitOfWork.St_Warehouse.GetSt_WarehouseByID(UserInfo.fCompanyId, St_HeaderVM.StockCode);
                if (Resources.Resource.CurLang == "Arb")
                {
                    report.StockNo.Text = AccName.ArabicName;
                }
                else
                {
                    report.StockNo.Text = AccName.EnglishName;
                }

            }
            report.Remark.Text = St_HeaderVM.Remark;
            report.Hint.Text = St_HeaderVM.Hint;


            if (CoInfo.TheDecimalPointForTheLocalCurrency == 2)
            {
                St_HeaderVM.sNetTotalLocal = "" + String.Format("{0:n2}", St_HeaderVM.sNetTotalLocal) + "";

            }
            else
            {
                St_HeaderVM.sNetTotalLocal = "" + String.Format("{0:n3}", St_HeaderVM.sNetTotalLocal) + "";
            }

            report.Sum6.Text = St_HeaderVM.sNetTotalLocal;


            report.CreateDocument();



            return View("ReportPDF", report);
        }

        public ActionResult GetAllSt_TransferVoucherPDF(string id, string id2, string id3, string id4)
        {
            try
            {
                St_HeaderVM Obj = new St_HeaderVM();
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);

                DateTime fromDate = DateTime.Parse(id.Substring(0, 2) + "/" + id.Substring(2, 2) + "/" + id.Substring(4, 4));
                DateTime toDate = DateTime.Parse(id2.Substring(0, 2) + "/" + id2.Substring(2, 2) + "/" + id2.Substring(4, 4));
                Obj.FromDate = fromDate;
                Obj.ToDate = toDate;
                Obj.VoucherNumber = id3;
                Obj.StockCode = id4;


                if (Obj.VoucherNumber == "0")
                {
                    Obj.VoucherNumber = "";
                }

                if (Obj.StockCode == "0")
                {
                    Obj.StockCode = "";
                }
                var AllSt_TransferVoucher = _unitOfWork.NativeSql.GetAllSt_TransferVoucher(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                if (AllSt_TransferVoucher == null)
                {
                    return Json(new List<St_HeaderVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.VoucherNumber))
                {
                    AllSt_TransferVoucher = AllSt_TransferVoucher.Where(m => m.VoucherNumber == Obj.VoucherNumber).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.StockCode))
                {
                    AllSt_TransferVoucher = AllSt_TransferVoucher.Where(m => m.StockCode == Obj.StockCode).ToList();
                }
                if (Obj.TransferType > 0)
                {
                    AllSt_TransferVoucher = AllSt_TransferVoucher.Where(m => m.TransferType == Obj.TransferType).ToList();
                }
                DataSet ds = new DataSet();
                DataTable table1 = new DataTable("St_HeaderVM");
                table1 = FunctionUnit.LINQResultToDataTable(AllSt_TransferVoucher);

                ds.Tables.Add(table1);

                var report = new St_TransferVoucherReport();

                string reportFilePath = Server.MapPath("/St_ReportLayout/St_TransferVoucherReport.repx");
                report.LoadLayoutFromXml(reportFilePath);

                report.LbCoName.Text = CoInfo.ArabicName;


                report.DataSource = ds;
                report.DataMember = "St_HeaderVM";

                report.LbFromDate.Text = Obj.FromDate.ToString("dd/MM/yyyy");
                report.LbTODate.Text = Obj.ToDate.ToString("dd/MM/yyyy");
                report.LbVoucherNo.Text = Obj.VoucherNumber;


                report.transfer.Text = Obj.TransferType.ToString();

                if (Obj.TransferType != 0)
                {
                    if (Obj.TransferType == 1)
                    {
                        report.transfer.Text = Resources.Resource.InVoucher;
                    }
                    else
                    {
                        report.transfer.Text = Resources.Resource.OutVoucher;

                    }

                }
                else
                {
                    report.transfer.Text = "  ";
                }

                report.StockCase.Text = Obj.StockCode;

                if (!String.IsNullOrEmpty(Obj.StockCode))
                {
                    var AccName = _unitOfWork.St_Warehouse.GetSt_WarehouseByID(UserInfo.fCompanyId, Obj.StockCode);
                    if (Resources.Resource.CurLang == "Arb")
                    {
                        report.StockCase.Text = AccName.ArabicName;
                    }
                    else
                    {
                        report.StockCase.Text = AccName.EnglishName;
                    }

                }


                report.CreateDocument();




                return View("ReportPDF", report);
            }
            catch (Exception ex)
            {
                string err = ex.Message;

                List<St_HeaderVM> VMList = new List<St_HeaderVM>();

                St_SpoiledVoucherReport report = new St_SpoiledVoucherReport();

                string reportFilePath = Server.MapPath("/St_ReportLayout/St_SpoiledVoucherReport.repx");
                report.LoadLayoutFromXml(reportFilePath);

                DataSet ds = new DataSet();
                DataTable table1 = new DataTable("St_HeaderVM");
                table1 = FunctionUnit.LINQResultToDataTable(VMList);

                ds.Tables.Add(table1);




                report.DataSource = ds;
                report.DataMember = "St_HeaderVM";
                // report.LbPrintTime.Text = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, FunctionUnit.Jordan_Time_Zone).ToString();

                report.CreateDocument();

                return View("ReportPDF", report);
            }

        }

    }

    }



