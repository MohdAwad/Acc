using Acc.Helpers;
using Acc.Models;
using Acc.Persistence;
using Acc.Repositories;
using Acc.ViewModels;
using DevExpress.DataAccess.Native.Web;
using DevExpress.Utils.OAuth.Provider;
using Microsoft.AspNet.Identity;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Acc.Controllers
{
    [Authorize]

    public class ExcelReportController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public ExcelReportController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        // GET: ExcelReport
        public ActionResult Index()
        {
            return View();
        }



        [HttpGet]
        public void Download(string fileGuid, string fileName)
        {
            if (TempData[fileGuid] != null)
            {
                FileInfo Template = new FileInfo(Path.Combine(Server.MapPath("~/ExcelTemplate/"), "Test.xlsx"));
                string handle = Guid.NewGuid().ToString();

                //EPPlusTest = Namespace/Project
                //templates = folder
                //VendorTemplate.xlsx = file
                //xlsx
                //ExcelPackage has a constructor that only requires a stream.
                ExcelPackage Ep = new ExcelPackage(Template);


                ExcelWorksheet Sheet = Ep.Workbook.Worksheets["Sheet1"];

                DataTable table1 = new DataTable("EmployeeInfoDataView");
                table1 = (DataTable)TempData[fileGuid];


                int row = 2;


                for (int i = 0; i <= table1.Rows.Count - 1; i++)
                {
                    Sheet.Cells[string.Format("A{0}", row)].Value = table1.Rows[i]["EmployeeId"];
                    Sheet.Cells[string.Format("B{0}", row)].Value = table1.Rows[i]["EmployeeFullName"];
                    Sheet.Cells[string.Format("C{0}", row)].Value = table1.Rows[i][""];
                    Sheet.Cells[string.Format("D{0}", row)].Value = table1.Rows[i][""];
                    Sheet.Cells[string.Format("E{0}", row)].Value = table1.Rows[i][""];
                    Sheet.Cells[string.Format("F{0}", row)].Value = table1.Rows[i][""];
                    Sheet.Cells[string.Format("G{0}", row)].Value = table1.Rows[i][""];
                    Sheet.Cells[string.Format("H{0}", row)].Value = table1.Rows[i][""];
                    Sheet.Cells[string.Format("I{0}", row)].Value = table1.Rows[i][""];
                    Sheet.Cells[string.Format("J{0}", row)].Value = table1.Rows[i][""];
                    Sheet.Cells[string.Format("K{0}", row)].Value = table1.Rows[i][""];
                    Sheet.Cells[string.Format("K{0}", row)].Value = table1.Rows[i][""];
                    Sheet.Cells[string.Format("L{0}", row)].Value = table1.Rows[i][""];
                    Sheet.Cells[string.Format("M{0}", row)].Value = table1.Rows[i][""];
                    Sheet.Cells[string.Format("N{0}", row)].Value = table1.Rows[i][""];


                    row++;
                }



                Sheet.Cells["A1"].LoadFromDataTable(table1, true);

                Sheet.Cells["A:AZ"].AutoFitColumns();

                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment: filename=" + "Test.xlsx");
                Response.BinaryWrite(Ep.GetAsByteArray());
                Response.End();

                //  byte[] data = TempData[fileGuid] as byte[];
                // return File(data, "application/vnd.ms-excel", fileName);
            }
            else
            {
                // Problem - Log the error, generate a blank file,
                //           redirect to another controller action - whatever fits with your application
                //  return new EmptyResult();
            }
        }


        public ActionResult CreateAssetTypeExcel(IEnumerable<AssetType> Obj)
        {
            //https://riptutorial.com/epplus/example/26423/fill-with-a-datatable-from-an-sql-query-or-stored-procedure


            string handle = Guid.NewGuid().ToString();
            DataTable table1 = new DataTable("AssetType");
            table1 = FunctionUnit.LINQResultToDataTable(Obj);





            TempData[handle] = table1;


            return new JsonResult()
            {
                Data = new { FileGuid = handle }
            };

        }


        [HttpGet]
        public void DownloadAssetTypeExcel(string fileGuid)
        {
            if (TempData[fileGuid] != null)
            {
                FileInfo Template = new FileInfo(Path.Combine(Server.MapPath("~/ExcelTemplate/"), "AssetType.xlsx"));
                string handle = Guid.NewGuid().ToString();

                ExcelPackage Ep = new ExcelPackage(Template);


                ExcelWorksheet Sheet = Ep.Workbook.Worksheets["Sheet1"];

                DataTable table1 = new DataTable("AssetType");
                table1 = (DataTable)TempData[fileGuid];


                int row = 6;
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetUserByID(userId);
                var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);


                Sheet.Cells[string.Format("A{0}", 1)].Value = CoInfo.ArabicName;
                Sheet.Cells[string.Format("A{0}", 2)].Value = @Resources.Resource.AssetType;

                for (int i = 0; i <= table1.Rows.Count - 1; i++)
                {

                    //string sDate = table1.Rows[i]["sDate"].ToString();
                    Sheet.Cells[string.Format("A{0}", row)].Value = table1.Rows[i]["AssetTypeID"];
                    Sheet.Cells[string.Format("B{0}", row)].Value = table1.Rows[i]["Name"];
                    Sheet.Cells[string.Format("C{0}", row)].Value = table1.Rows[i]["ConsRatio"];



                    row++;
                }


                // Sheet.Cells["A1"].LoadFromDataTable(table1, true);

                Sheet.Cells["A:AZ"].AutoFitColumns();

                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment: filename=" + "AttendanceReport.xlsx");
                Response.BinaryWrite(Ep.GetAsByteArray());
                Response.End();

                //  byte[] data = TempData[fileGuid] as byte[];
                // return File(data, "application/vnd.ms-excel", fileName);
            }
            else
            {
                // Problem - Log the error, generate a blank file,
                //           redirect to another controller action - whatever fits with your application
                //  return new EmptyResult();
            }
        }





        public ActionResult CreateCurrencyExcel(IEnumerable<Currency> Obj)
        {
            //https://riptutorial.com/epplus/example/26423/fill-with-a-datatable-from-an-sql-query-or-stored-procedure


            string handle = Guid.NewGuid().ToString();
            DataTable table1 = new DataTable("Currency");
            table1 = FunctionUnit.LINQResultToDataTable(Obj);





            TempData[handle] = table1;


            return new JsonResult()
            {
                Data = new { FileGuid = handle }
            };

        }


        [HttpGet]
        public void DownloadCurrencyExcel(string fileGuid)
        {
            if (TempData[fileGuid] != null)
            {
                FileInfo Template = new FileInfo(Path.Combine(Server.MapPath("~/ExcelTemplate/"), "Currency.xlsx"));
                string handle = Guid.NewGuid().ToString();

                ExcelPackage Ep = new ExcelPackage(Template);


                ExcelWorksheet Sheet = Ep.Workbook.Worksheets["Sheet1"];

                DataTable table1 = new DataTable("Currency");
                table1 = (DataTable)TempData[fileGuid];


                int row = 6;
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetUserByID(userId);
                var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);


                Sheet.Cells[string.Format("A{0}", 1)].Value = CoInfo.ArabicName;
                Sheet.Cells[string.Format("A{0}", 2)].Value = @Resources.Resource.Currency;

                if (@Resources.Resource.CurLang == "Arb")
                {

                    for (int i = 0; i <= table1.Rows.Count - 1; i++)
                    {

                        //string sDate = table1.Rows[i]["sDate"].ToString();
                        Sheet.Cells[string.Format("A{0}", row)].Value = table1.Rows[i]["CurrencyID"];

                        Sheet.Cells[string.Format("B{0}", row)].Value = table1.Rows[i]["ArabicName"];





                        row++;
                    }
                }
                else
                {
                    for (int i = 0; i <= table1.Rows.Count - 1; i++)
                    {

                        //string sDate = table1.Rows[i]["sDate"].ToString();
                        Sheet.Cells[string.Format("A{0}", row)].Value = table1.Rows[i]["CurrencyID"];

                        Sheet.Cells[string.Format("B{0}", row)].Value = table1.Rows[i]["EnglishName"];





                        row++;
                    }
                }


                // Sheet.Cells["A1"].LoadFromDataTable(table1, true);

                Sheet.Cells["A:AZ"].AutoFitColumns();

                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment: filename=" + "AttendanceReport.xlsx");
                Response.BinaryWrite(Ep.GetAsByteArray());
                Response.End();

                //  byte[] data = TempData[fileGuid] as byte[];
                // return File(data, "application/vnd.ms-excel", fileName);
            }
            else
            {
                // Problem - Log the error, generate a blank file,
                //           redirect to another controller action - whatever fits with your application
                //  return new EmptyResult();
            }
        }
        public ActionResult CreateCurrencyValueExcel(IEnumerable<CurrencyValueVM> Obj)
        {
            //https://riptutorial.com/epplus/example/26423/fill-with-a-datatable-from-an-sql-query-or-stored-procedure


            string handle = Guid.NewGuid().ToString();
            DataTable table1 = new DataTable("CurrencyValueVM");
            table1 = FunctionUnit.LINQResultToDataTable(Obj);





            TempData[handle] = table1;


            return new JsonResult()
            {
                Data = new { FileGuid = handle }
            };

        }


        [HttpGet]
        public void DownloadCurrencyValueExcel(string fileGuid)
        {
            if (TempData[fileGuid] != null)
            {
                FileInfo Template = new FileInfo(Path.Combine(Server.MapPath("~/ExcelTemplate/"), "CurrencyValue.xlsx"));
                string handle = Guid.NewGuid().ToString();

                ExcelPackage Ep = new ExcelPackage(Template);


                ExcelWorksheet Sheet = Ep.Workbook.Worksheets["Sheet1"];

                DataTable table1 = new DataTable("CurrencyValueVM");
                table1 = (DataTable)TempData[fileGuid];


                int row = 6;
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetUserByID(userId);
                var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);


                Sheet.Cells[string.Format("A{0}", 1)].Value = CoInfo.ArabicName;
                Sheet.Cells[string.Format("A{0}", 2)].Value = @Resources.Resource.CurrencyValue;

                if (@Resources.Resource.CurLang == "Arb")
                {
                    for (int i = 0; i <= table1.Rows.Count - 1; i++)
                    {

                        //string sDate = table1.Rows[i]["sDate"].ToString();
                        Sheet.Cells[string.Format("A{0}", row)].Value = table1.Rows[i]["CurrencyID"];
                        Sheet.Cells[string.Format("B{0}", row)].Value = table1.Rows[i]["ArabicName"];
                        Sheet.Cells[string.Format("C{0}", row)].Value = table1.Rows[i]["SConversionFactor"];
                        Sheet.Cells[string.Format("D{0}", row)].Value = table1.Rows[i]["sInsDateTime"];

                        //GetAllCurrencyValue

                        row++;
                    }

                }

                else
                {
                    for (int i = 0; i <= table1.Rows.Count - 1; i++)
                    {

                        //string sDate = table1.Rows[i]["sDate"].ToString();
                        Sheet.Cells[string.Format("A{0}", row)].Value = table1.Rows[i]["CurrencyID"];
                        Sheet.Cells[string.Format("B{0}", row)].Value = table1.Rows[i]["EnglishName"];
                        Sheet.Cells[string.Format("C{0}", row)].Value = table1.Rows[i]["SConversionFactor"];
                        Sheet.Cells[string.Format("D{0}", row)].Value = table1.Rows[i]["sInsDateTime"];



                        row++;
                    }

                }
                // Sheet.Cells["A1"].LoadFromDataTable(table1, true);

                Sheet.Cells["A:AZ"].AutoFitColumns();

                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment: filename=" + "AttendanceReport.xlsx");
                Response.BinaryWrite(Ep.GetAsByteArray());
                Response.End();

                //  byte[] data = TempData[fileGuid] as byte[];
                // return File(data, "application/vnd.ms-excel", fileName);
            }
            else
            {
                // Problem - Log the error, generate a blank file,
                //           redirect to another controller action - whatever fits with your application
                //  return new EmptyResult();
            }
        }


        public ActionResult CreateSaleExcel(IEnumerable<Sale> Obj)
        {
            //https://riptutorial.com/epplus/example/26423/fill-with-a-datatable-from-an-sql-query-or-stored-procedure


            string handle = Guid.NewGuid().ToString();
            DataTable table1 = new DataTable("Sale");
            table1 = FunctionUnit.LINQResultToDataTable(Obj);





            TempData[handle] = table1;


            return new JsonResult()
            {
                Data = new { FileGuid = handle }
            };

        }


        [HttpGet]
        public void DownloadSaleExcel(string fileGuid)
        {
            if (TempData[fileGuid] != null)
            {
                FileInfo Template = new FileInfo(Path.Combine(Server.MapPath("~/ExcelTemplate/"), "Sale.xlsx"));
                string handle = Guid.NewGuid().ToString();

                ExcelPackage Ep = new ExcelPackage(Template);


                ExcelWorksheet Sheet = Ep.Workbook.Worksheets["Sheet1"];

                DataTable table1 = new DataTable("Sale");
                table1 = (DataTable)TempData[fileGuid];


                int row = 6;
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetUserByID(userId);
                var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);


                Sheet.Cells[string.Format("A{0}", 1)].Value = CoInfo.ArabicName;
                Sheet.Cells[string.Format("A{0}", 2)].Value = @Resources.Resource.Sales;

                for (int i = 0; i <= table1.Rows.Count - 1; i++)
                {

                    //string sDate = table1.Rows[i]["sDate"].ToString();
                    Sheet.Cells[string.Format("A{0}", row)].Value = table1.Rows[i]["SalesID"];
                    Sheet.Cells[string.Format("B{0}", row)].Value = table1.Rows[i]["SalesName"];



                    row++;
                }



                // Sheet.Cells["A1"].LoadFromDataTable(table1, true);

                Sheet.Cells["A:AZ"].AutoFitColumns();

                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment: filename=" + "AttendanceReport.xlsx");
                Response.BinaryWrite(Ep.GetAsByteArray());
                Response.End();

                //  byte[] data = TempData[fileGuid] as byte[];
                // return File(data, "application/vnd.ms-excel", fileName);
            }
            else
            {
                // Problem - Log the error, generate a blank file,
                //           redirect to another controller action - whatever fits with your application
                //  return new EmptyResult();
            }
        }


        public ActionResult CreateCompanyTransactionKindExcel(IEnumerable<CompanyTransationKindVM> Obj)
        {
            //https://riptutorial.com/epplus/example/26423/fill-with-a-datatable-from-an-sql-query-or-stored-procedure


            string handle = Guid.NewGuid().ToString();
            DataTable table1 = new DataTable("CompanyTransationKindVM");
            table1 = FunctionUnit.LINQResultToDataTable(Obj);





            TempData[handle] = table1;


            return new JsonResult()
            {
                Data = new { FileGuid = handle }
            };

        }


        [HttpGet]
        public void DownloadCompanyTransactionKindExcel(string fileGuid)
        {
            if (TempData[fileGuid] != null)
            {
                FileInfo Template = new FileInfo(Path.Combine(Server.MapPath("~/ExcelTemplate/"), "CompanyTransactionKind.xlsx"));
                string handle = Guid.NewGuid().ToString();

                ExcelPackage Ep = new ExcelPackage(Template);


                ExcelWorksheet Sheet = Ep.Workbook.Worksheets["Sheet1"];

                DataTable table1 = new DataTable("CompanyTransationKindVM");
                table1 = (DataTable)TempData[fileGuid];


                int row = 6;
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetUserByID(userId);
                var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);


                Sheet.Cells[string.Format("A{0}", 1)].Value = CoInfo.ArabicName;
                Sheet.Cells[string.Format("A{0}", 2)].Value = @Resources.Resource.CompnyTransactionKind;

                for (int i = 0; i <= table1.Rows.Count - 1; i++)
                {

                    Sheet.Cells[string.Format("A{0}", row)].Value = table1.Rows[i]["CompanyTransactionKindID"];
                    Sheet.Cells[string.Format("B{0}", row)].Value = table1.Rows[i]["CompanyTransactionKindName"];
                    Sheet.Cells[string.Format("C{0}", row)].Value = table1.Rows[i]["TransactionKindName"];
                    Sheet.Cells[string.Format("D{0}", row)].Value = table1.Rows[i]["CaseSerial"];



                    row++;
                }



                // Sheet.Cells["A1"].LoadFromDataTable(table1, true);

                Sheet.Cells["A:AZ"].AutoFitColumns();

                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment: filename=" + "AttendanceReport.xlsx");
                Response.BinaryWrite(Ep.GetAsByteArray());
                Response.End();

                //  byte[] data = TempData[fileGuid] as byte[];
                // return File(data, "application/vnd.ms-excel", fileName);
            }
            else
            {
                // Problem - Log the error, generate a blank file,
                //           redirect to another controller action - whatever fits with your application
                //  return new EmptyResult();
            }
        }



        public ActionResult CreateServiceGroupExcel(IEnumerable<ServiceGroup> Obj)
        {
            //https://riptutorial.com/epplus/example/26423/fill-with-a-datatable-from-an-sql-query-or-stored-procedure


            string handle = Guid.NewGuid().ToString();
            DataTable table1 = new DataTable("ServiceGroup");
            table1 = FunctionUnit.LINQResultToDataTable(Obj);





            TempData[handle] = table1;


            return new JsonResult()
            {
                Data = new { FileGuid = handle }
            };

        }


        [HttpGet]
        public void DownloadServiceGroupExcel(string fileGuid)
        {
            if (TempData[fileGuid] != null)
            {
                FileInfo Template = new FileInfo(Path.Combine(Server.MapPath("~/ExcelTemplate/"), "ServiceGroup.xlsx"));
                string handle = Guid.NewGuid().ToString();

                ExcelPackage Ep = new ExcelPackage(Template);


                ExcelWorksheet Sheet = Ep.Workbook.Worksheets["Sheet1"];

                DataTable table1 = new DataTable("ServiceGroup");
                table1 = (DataTable)TempData[fileGuid];


                int row = 6;
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetUserByID(userId);
                var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);


                Sheet.Cells[string.Format("A{0}", 1)].Value = CoInfo.ArabicName;
                Sheet.Cells[string.Format("A{0}", 2)].Value = @Resources.Resource.ServiceGroup;

                if (@Resources.Resource.CurLang == "Arb")
                {

                    for (int i = 0; i <= table1.Rows.Count - 1; i++)
                    {

                        //string sDate = table1.Rows[i]["sDate"].ToString();
                        Sheet.Cells[string.Format("A{0}", row)].Value = table1.Rows[i]["ServiceGroupID"];

                        Sheet.Cells[string.Format("B{0}", row)].Value = table1.Rows[i]["ArabicName"];





                        row++;
                    }
                }
                else
                {
                    for (int i = 0; i <= table1.Rows.Count - 1; i++)
                    {

                        //string sDate = table1.Rows[i]["sDate"].ToString();
                        Sheet.Cells[string.Format("A{0}", row)].Value = table1.Rows[i]["ServiceGroupID"];

                        Sheet.Cells[string.Format("B{0}", row)].Value = table1.Rows[i]["EnglishName"];





                        row++;
                    }
                }


                // Sheet.Cells["A1"].LoadFromDataTable(table1, true);

                Sheet.Cells["A:AZ"].AutoFitColumns();

                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment: filename=" + "AttendanceReport.xlsx");
                Response.BinaryWrite(Ep.GetAsByteArray());
                Response.End();

                //  byte[] data = TempData[fileGuid] as byte[];
                // return File(data, "application/vnd.ms-excel", fileName);
            }
            else
            {
                // Problem - Log the error, generate a blank file,
                //           redirect to another controller action - whatever fits with your application
                //  return new EmptyResult();
            }
        }

        public ActionResult CreateServiceExcel(IEnumerable<ServiceVM> Obj)
        {
            //https://riptutorial.com/epplus/example/26423/fill-with-a-datatable-from-an-sql-query-or-stored-procedure


            string handle = Guid.NewGuid().ToString();
            DataTable table1 = new DataTable("ServiceVM");
            table1 = FunctionUnit.LINQResultToDataTable(Obj);





            TempData[handle] = table1;


            return new JsonResult()
            {
                Data = new { FileGuid = handle }
            };

        }


        [HttpGet]
        public void DownloadServiceExcel(string fileGuid)
        {
            if (TempData[fileGuid] != null)
            {
                FileInfo Template = new FileInfo(Path.Combine(Server.MapPath("~/ExcelTemplate/"), "Service.xlsx"));
                string handle = Guid.NewGuid().ToString();

                ExcelPackage Ep = new ExcelPackage(Template);


                ExcelWorksheet Sheet = Ep.Workbook.Worksheets["Sheet1"];

                DataTable table1 = new DataTable("ServiceVM");
                table1 = (DataTable)TempData[fileGuid];


                int row = 6;
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetUserByID(userId);
                var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);


                Sheet.Cells[string.Format("A{0}", 1)].Value = CoInfo.ArabicName;
                Sheet.Cells[string.Format("A{0}", 2)].Value = @Resources.Resource.Service;




                for (int i = 0; i <= table1.Rows.Count - 1; i++)
                {

                    //string sDate = table1.Rows[i]["sDate"].ToString();
                    Sheet.Cells[string.Format("A{0}", row)].Value = table1.Rows[i]["ServiceID"];

                    Sheet.Cells[string.Format("B{0}", row)].Value = table1.Rows[i]["ServiceName"];
                    Sheet.Cells[string.Format("C{0}", row)].Value = table1.Rows[i]["ServiceGroupName"];
                    Sheet.Cells[string.Format("D{0}", row)].Value = table1.Rows[i]["SSalePrice"];
                    Sheet.Cells[string.Format("E{0}", row)].Value = table1.Rows[i]["SCostPrice"];
                    Sheet.Cells[string.Format("F{0}", row)].Value = table1.Rows[i]["STaxPercentage"];





                    row++;
                }



                // Sheet.Cells["A1"].LoadFromDataTable(table1, true);

                Sheet.Cells["A:AZ"].AutoFitColumns();

                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment: filename=" + "AttendanceReport.xlsx");
                Response.BinaryWrite(Ep.GetAsByteArray());
                Response.End();

                //  byte[] data = TempData[fileGuid] as byte[];
                // return File(data, "application/vnd.ms-excel", fileName);
            }
            else
            {
                // Problem - Log the error, generate a blank file,
                //           redirect to another controller action - whatever fits with your application
                //  return new EmptyResult();
            }
        }


        public ActionResult CreateDrawnBankExcel(IEnumerable<DrawnBank> Obj)
        {
            //https://riptutorial.com/epplus/example/26423/fill-with-a-datatable-from-an-sql-query-or-stored-procedure


            string handle = Guid.NewGuid().ToString();
            DataTable table1 = new DataTable("DrawnBank");
            table1 = FunctionUnit.LINQResultToDataTable(Obj);





            TempData[handle] = table1;


            return new JsonResult()
            {
                Data = new { FileGuid = handle }
            };

        }


        [HttpGet]
        public void DownloadDrawnBankExcel(string fileGuid)
        {
            if (TempData[fileGuid] != null)
            {
                FileInfo Template = new FileInfo(Path.Combine(Server.MapPath("~/ExcelTemplate/"), "DrawnBank.xlsx"));
                string handle = Guid.NewGuid().ToString();

                ExcelPackage Ep = new ExcelPackage(Template);


                ExcelWorksheet Sheet = Ep.Workbook.Worksheets["Sheet1"];

                DataTable table1 = new DataTable("DrawnBank");
                table1 = (DataTable)TempData[fileGuid];


                int row = 6;
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetUserByID(userId);
                var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);


                Sheet.Cells[string.Format("A{0}", 1)].Value = CoInfo.ArabicName;
                Sheet.Cells[string.Format("A{0}", 2)].Value = @Resources.Resource.DrawnBankName;


                if (@Resources.Resource.CurLang == "Arb")
                {
                    for (int i = 0; i <= table1.Rows.Count - 1; i++)
                    {

                        //string sDate = table1.Rows[i]["sDate"].ToString();
                        Sheet.Cells[string.Format("A{0}", row)].Value = table1.Rows[i]["BankID"];

                        Sheet.Cells[string.Format("B{0}", row)].Value = table1.Rows[i]["ArabicName"];
                        Sheet.Cells[string.Format("C{0}", row)].Value = table1.Rows[i]["Active"];






                        row++;
                    }


                }

                else
                {

                    for (int i = 0; i <= table1.Rows.Count - 1; i++)
                    {

                        //string sDate = table1.Rows[i]["sDate"].ToString();
                        Sheet.Cells[string.Format("A{0}", row)].Value = table1.Rows[i]["BankID"];

                        Sheet.Cells[string.Format("B{0}", row)].Value = table1.Rows[i]["EnglishName"];
                        Sheet.Cells[string.Format("C{0}", row)].Value = table1.Rows[i]["Active"];





                        row++;
                    }

                }

                // Sheet.Cells["A1"].LoadFromDataTable(table1, true);

                Sheet.Cells["A:AZ"].AutoFitColumns();

                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment: filename=" + "AttendanceReport.xlsx");
                Response.BinaryWrite(Ep.GetAsByteArray());
                Response.End();

                //  byte[] data = TempData[fileGuid] as byte[];
                // return File(data, "application/vnd.ms-excel", fileName);
            }
            else
            {
                // Problem - Log the error, generate a blank file,
                //           redirect to another controller action - whatever fits with your application
                //  return new EmptyResult();
            }
        }


        public ActionResult CreateDefinitionBankExcel(IEnumerable<DefinitionBanksFilterVM> Obj)
        {
            //https://riptutorial.com/epplus/example/26423/fill-with-a-datatable-from-an-sql-query-or-stored-procedure


            string handle = Guid.NewGuid().ToString();
            DataTable table1 = new DataTable("DefinitionBanksFilterVM");
            table1 = FunctionUnit.LINQResultToDataTable(Obj);





            TempData[handle] = table1;


            return new JsonResult()
            {
                Data = new { FileGuid = handle }
            };

        }


        [HttpGet]
        public void DownloadDefinitionBankExcel(string fileGuid)
        {
            if (TempData[fileGuid] != null)
            {
                FileInfo Template = new FileInfo(Path.Combine(Server.MapPath("~/ExcelTemplate/"), "DefinitionBank.xlsx"));
                string handle = Guid.NewGuid().ToString();

                ExcelPackage Ep = new ExcelPackage(Template);


                ExcelWorksheet Sheet = Ep.Workbook.Worksheets["Sheet1"];

                DataTable table1 = new DataTable("DefinitionBanksFilterVM");
                table1 = (DataTable)TempData[fileGuid];


                int row = 6;
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetUserByID(userId);
                var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);


                Sheet.Cells[string.Format("A{0}", 1)].Value = CoInfo.ArabicName;
                Sheet.Cells[string.Format("A{0}", 2)].Value = @Resources.Resource.DefinitionBanks;



                for (int i = 0; i <= table1.Rows.Count - 1; i++)
                {

                    //string sDate = table1.Rows[i]["sDate"].ToString();
                    Sheet.Cells[string.Format("A{0}", row)].Value = table1.Rows[i]["BankID"];

                    Sheet.Cells[string.Format("B{0}", row)].Value = table1.Rows[i]["BankAccountName "];
                    Sheet.Cells[string.Format("C{0}", row)].Value = table1.Rows[i]["ChequeUnderCollectionAccountName"];
                    Sheet.Cells[string.Format("D{0}", row)].Value = table1.Rows[i]["ChequeUnderCollectionAccountName"];
                    Sheet.Cells[string.Format("E{0}", row)].Value = table1.Rows[i]["PostdatedChequeAccountName"];
                    Sheet.Cells[string.Format("F{0}", row)].Value = table1.Rows[i]["BillsOfExchangeAccountName"];
                    Sheet.Cells[string.Format("G{0}", row)].Value = table1.Rows[i]["UserName"];






                    row++;
                }





                // Sheet.Cells["A1"].LoadFromDataTable(table1, true);

                Sheet.Cells["A:AZ"].AutoFitColumns();

                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment: filename=" + "AttendanceReport.xlsx");
                Response.BinaryWrite(Ep.GetAsByteArray());
                Response.End();

                //  byte[] data = TempData[fileGuid] as byte[];
                // return File(data, "application/vnd.ms-excel", fileName);
            }
            else
            {
                // Problem - Log the error, generate a blank file,
                //           redirect to another controller action - whatever fits with your application
                //  return new EmptyResult();
            }
        }



        public ActionResult CreateAssetAdministrationExcel(IEnumerable<AssetAdministration> Obj)
        {
            //https://riptutorial.com/epplus/example/26423/fill-with-a-datatable-from-an-sql-query-or-stored-procedure


            string handle = Guid.NewGuid().ToString();
            DataTable table1 = new DataTable("AssetAdministration");
            table1 = FunctionUnit.LINQResultToDataTable(Obj);





            TempData[handle] = table1;


            return new JsonResult()
            {
                Data = new { FileGuid = handle }
            };

        }


        [HttpGet]
        public void DownloadAssetAdministrationExcel(string fileGuid)
        {
            if (TempData[fileGuid] != null)
            {
                FileInfo Template = new FileInfo(Path.Combine(Server.MapPath("~/ExcelTemplate/"), "AssetAdministration.xlsx"));
                string handle = Guid.NewGuid().ToString();

                ExcelPackage Ep = new ExcelPackage(Template);


                ExcelWorksheet Sheet = Ep.Workbook.Worksheets["Sheet1"];

                DataTable table1 = new DataTable("AssetAdministration");
                table1 = (DataTable)TempData[fileGuid];


                int row = 6;
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetUserByID(userId);
                var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);


                Sheet.Cells[string.Format("A{0}", 1)].Value = CoInfo.ArabicName;
                Sheet.Cells[string.Format("A{0}", 2)].Value = @Resources.Resource.AssetAdministration;



                for (int i = 0; i <= table1.Rows.Count - 1; i++)
                {

                    //string sDate = table1.Rows[i]["sDate"].ToString();
                    Sheet.Cells[string.Format("A{0}", row)].Value = table1.Rows[i]["AdministrationID"];

                    Sheet.Cells[string.Format("B{0}", row)].Value = table1.Rows[i]["AdministrationName"];






                    row++;
                }





                // Sheet.Cells["A1"].LoadFromDataTable(table1, true);

                Sheet.Cells["A:AZ"].AutoFitColumns();

                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment: filename=" + "AttendanceReport.xlsx");
                Response.BinaryWrite(Ep.GetAsByteArray());
                Response.End();

                //  byte[] data = TempData[fileGuid] as byte[];
                // return File(data, "application/vnd.ms-excel", fileName);
            }
            else
            {
                // Problem - Log the error, generate a blank file,
                //           redirect to another controller action - whatever fits with your application
                //  return new EmptyResult();
            }
        }
        public ActionResult CreateAssetCircleExcel(IEnumerable<AssetCircle> Obj)
        {
            //https://riptutorial.com/epplus/example/26423/fill-with-a-datatable-from-an-sql-query-or-stored-procedure


            string handle = Guid.NewGuid().ToString();
            DataTable table1 = new DataTable("AssetCircle");
            table1 = FunctionUnit.LINQResultToDataTable(Obj);





            TempData[handle] = table1;


            return new JsonResult()
            {
                Data = new { FileGuid = handle }
            };

        }


        [HttpGet]
        public void DownloadAssetCircleExcel(string fileGuid)
        {
            if (TempData[fileGuid] != null)
            {
                FileInfo Template = new FileInfo(Path.Combine(Server.MapPath("~/ExcelTemplate/"), "AssetCircle.xlsx"));
                string handle = Guid.NewGuid().ToString();

                ExcelPackage Ep = new ExcelPackage(Template);


                ExcelWorksheet Sheet = Ep.Workbook.Worksheets["Circle"];

                DataTable table1 = new DataTable("AssetCircle");
                table1 = (DataTable)TempData[fileGuid];


                int row = 6;
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetUserByID(userId);
                var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);


                Sheet.Cells[string.Format("A{0}", 1)].Value = CoInfo.ArabicName;
                Sheet.Cells[string.Format("A{0}", 2)].Value = @Resources.Resource.AssetCircle;



                for (int i = 0; i <= table1.Rows.Count - 1; i++)
                {

                    //string sDate = table1.Rows[i]["sDate"].ToString();
                    Sheet.Cells[string.Format("A{0}", row)].Value = table1.Rows[i]["CircleID"];

                    Sheet.Cells[string.Format("B{0}", row)].Value = table1.Rows[i]["CircleName"];






                    row++;
                }





                // Sheet.Cells["A1"].LoadFromDataTable(table1, true);

                Sheet.Cells["A:AZ"].AutoFitColumns();

                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment: filename=" + "AttendanceReport.xlsx");
                Response.BinaryWrite(Ep.GetAsByteArray());
                Response.End();

                //  byte[] data = TempData[fileGuid] as byte[];
                // return File(data, "application/vnd.ms-excel", fileName);
            }
            else
            {
                // Problem - Log the error, generate a blank file,
                //           redirect to another controller action - whatever fits with your application
                //  return new EmptyResult();
            }
        }




        public ActionResult CreateAssetSectionExcel(IEnumerable<AssetSection> Obj)
        {
            //https://riptutorial.com/epplus/example/26423/fill-with-a-datatable-from-an-sql-query-or-stored-procedure


            string handle = Guid.NewGuid().ToString();
            DataTable table1 = new DataTable("AssetSection");
            table1 = FunctionUnit.LINQResultToDataTable(Obj);





            TempData[handle] = table1;


            return new JsonResult()
            {
                Data = new { FileGuid = handle }
            };

        }


        [HttpGet]
        public void DownloadAssetSectionExcel(string fileGuid)
        {
            if (TempData[fileGuid] != null)
            {
                FileInfo Template = new FileInfo(Path.Combine(Server.MapPath("~/ExcelTemplate/"), "AssetSection.xlsx"));
                string handle = Guid.NewGuid().ToString();

                ExcelPackage Ep = new ExcelPackage(Template);


                ExcelWorksheet Sheet = Ep.Workbook.Worksheets["Sheet1"];

                DataTable table1 = new DataTable("AssetSection");
                table1 = (DataTable)TempData[fileGuid];


                int row = 6;
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetUserByID(userId);
                var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);


                Sheet.Cells[string.Format("A{0}", 1)].Value = CoInfo.ArabicName;
                Sheet.Cells[string.Format("A{0}", 2)].Value = @Resources.Resource.AssetSection;



                for (int i = 0; i <= table1.Rows.Count - 1; i++)
                {

                    //string sDate = table1.Rows[i]["sDate"].ToString();
                    Sheet.Cells[string.Format("A{0}", row)].Value = table1.Rows[i]["SectionID"];

                    Sheet.Cells[string.Format("B{0}", row)].Value = table1.Rows[i]["SectionName"];






                    row++;
                }





                // Sheet.Cells["A1"].LoadFromDataTable(table1, true);

                Sheet.Cells["A:AZ"].AutoFitColumns();

                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment: filename=" + "AttendanceReport.xlsx");
                Response.BinaryWrite(Ep.GetAsByteArray());
                Response.End();

                //  byte[] data = TempData[fileGuid] as byte[];
                // return File(data, "application/vnd.ms-excel", fileName);
            }
            else
            {
                // Problem - Log the error, generate a blank file,
                //           redirect to another controller action - whatever fits with your application
                //  return new EmptyResult();
            }
        }

        public ActionResult CreateAssetSiteExcel(IEnumerable<AssetSite> Obj)
        {
            //https://riptutorial.com/epplus/example/26423/fill-with-a-datatable-from-an-sql-query-or-stored-procedure


            string handle = Guid.NewGuid().ToString();
            DataTable table1 = new DataTable("AssetSite");
            table1 = FunctionUnit.LINQResultToDataTable(Obj);





            TempData[handle] = table1;


            return new JsonResult()
            {
                Data = new { FileGuid = handle }
            };

        }


        [HttpGet]
        public void DownloadAssetSiteExcel(string fileGuid)
        {
            if (TempData[fileGuid] != null)
            {
                FileInfo Template = new FileInfo(Path.Combine(Server.MapPath("~/ExcelTemplate/"), "AssetSite.xlsx"));
                string handle = Guid.NewGuid().ToString();

                ExcelPackage Ep = new ExcelPackage(Template);


                ExcelWorksheet Sheet = Ep.Workbook.Worksheets["Sheet1"];

                DataTable table1 = new DataTable("AssetSite");
                table1 = (DataTable)TempData[fileGuid];


                int row = 6;
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetUserByID(userId);
                var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);


                Sheet.Cells[string.Format("A{0}", 1)].Value = CoInfo.ArabicName;
                Sheet.Cells[string.Format("A{0}", 2)].Value = @Resources.Resource.AssetSite;



                for (int i = 0; i <= table1.Rows.Count - 1; i++)
                {

                    //string sDate = table1.Rows[i]["sDate"].ToString();
                    Sheet.Cells[string.Format("A{0}", row)].Value = table1.Rows[i]["SiteID"];

                    Sheet.Cells[string.Format("B{0}", row)].Value = table1.Rows[i]["SiteName"];






                    row++;
                }





                // Sheet.Cells["A1"].LoadFromDataTable(table1, true);

                Sheet.Cells["A:AZ"].AutoFitColumns();

                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment: filename=" + "AttendanceReport.xlsx");
                Response.BinaryWrite(Ep.GetAsByteArray());
                Response.End();

                //  byte[] data = TempData[fileGuid] as byte[];
                // return File(data, "application/vnd.ms-excel", fileName);
            }
            else
            {
                // Problem - Log the error, generate a blank file,
                //           redirect to another controller action - whatever fits with your application
                //  return new EmptyResult();
            }
        }


        public ActionResult CreateAssetTrusteeExcel(IEnumerable<AssetTrustee> Obj)
        {
            //https://riptutorial.com/epplus/example/26423/fill-with-a-datatable-from-an-sql-query-or-stored-procedure


            string handle = Guid.NewGuid().ToString();
            DataTable table1 = new DataTable("AssetTrustee");
            table1 = FunctionUnit.LINQResultToDataTable(Obj);





            TempData[handle] = table1;


            return new JsonResult()
            {
                Data = new { FileGuid = handle }
            };

        }


        [HttpGet]
        public void DownloadAssetTrusteeExcel(string fileGuid)
        {
            if (TempData[fileGuid] != null)
            {
                FileInfo Template = new FileInfo(Path.Combine(Server.MapPath("~/ExcelTemplate/"), "AssetTrustee.xlsx"));
                string handle = Guid.NewGuid().ToString();

                ExcelPackage Ep = new ExcelPackage(Template);


                ExcelWorksheet Sheet = Ep.Workbook.Worksheets["Sheet1"];

                DataTable table1 = new DataTable("AssetTrustee");
                table1 = (DataTable)TempData[fileGuid];


                int row = 6;
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetUserByID(userId);
                var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);


                Sheet.Cells[string.Format("A{0}", 1)].Value = CoInfo.ArabicName;
                Sheet.Cells[string.Format("A{0}", 2)].Value = @Resources.Resource.AssetTrustee;



                for (int i = 0; i <= table1.Rows.Count - 1; i++)
                {

                    //string sDate = table1.Rows[i]["sDate"].ToString();
                    Sheet.Cells[string.Format("A{0}", row)].Value = table1.Rows[i]["TrusteeID"];

                    Sheet.Cells[string.Format("B{0}", row)].Value = table1.Rows[i]["TrusteeName"];






                    row++;
                }





                // Sheet.Cells["A1"].LoadFromDataTable(table1, true);

                Sheet.Cells["A:AZ"].AutoFitColumns();

                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment: filename=" + "AttendanceReport.xlsx");
                Response.BinaryWrite(Ep.GetAsByteArray());
                Response.End();

                //  byte[] data = TempData[fileGuid] as byte[];
                // return File(data, "application/vnd.ms-excel", fileName);
            }
            else
            {
                // Problem - Log the error, generate a blank file,
                //           redirect to another controller action - whatever fits with your application
                //  return new EmptyResult();
            }
        }
        public ActionResult CreateSupplierBankExcel(IEnumerable<SupplierBank> Obj)
        {
            //https://riptutorial.com/epplus/example/26423/fill-with-a-datatable-from-an-sql-query-or-stored-procedure


            string handle = Guid.NewGuid().ToString();
            DataTable table1 = new DataTable("SupplierBank");
            table1 = FunctionUnit.LINQResultToDataTable(Obj);





            TempData[handle] = table1;


            return new JsonResult()
            {
                Data = new { FileGuid = handle }
            };

        }


        [HttpGet]
        public void DownloadSupplierBankExcel(string fileGuid)
        {
            if (TempData[fileGuid] != null)
            {
                FileInfo Template = new FileInfo(Path.Combine(Server.MapPath("~/ExcelTemplate/"), "SupplierBank.xlsx"));
                string handle = Guid.NewGuid().ToString();

                ExcelPackage Ep = new ExcelPackage(Template);


                ExcelWorksheet Sheet = Ep.Workbook.Worksheets["Sheet1"];

                DataTable table1 = new DataTable("SupplierBank");
                table1 = (DataTable)TempData[fileGuid];


                int row = 6;
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetUserByID(userId);
                var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);


                Sheet.Cells[string.Format("A{0}", 1)].Value = CoInfo.ArabicName;
                Sheet.Cells[string.Format("A{0}", 2)].Value = @Resources.Resource.SupplierBank;


                if (@Resources.Resource.CurLang == "Arb")
                {
                    for (int i = 0; i <= table1.Rows.Count - 1; i++)
                    {

                        //string sDate = table1.Rows[i]["sDate"].ToString();
                        Sheet.Cells[string.Format("A{0}", row)].Value = table1.Rows[i]["SupplierBankID"];

                        Sheet.Cells[string.Format("B{0}", row)].Value = table1.Rows[i]["ArabicName"];






                        row++;
                    }

                }
                else
                {
                    for (int i = 0; i <= table1.Rows.Count - 1; i++)
                    {

                        //string sDate = table1.Rows[i]["sDate"].ToString();
                        Sheet.Cells[string.Format("A{0}", row)].Value = table1.Rows[i]["SupplierBankID"];

                        Sheet.Cells[string.Format("B{0}", row)].Value = table1.Rows[i]["EnglishName"];






                        row++;
                    }

                }





                // Sheet.Cells["A1"].LoadFromDataTable(table1, true);

                Sheet.Cells["A:AZ"].AutoFitColumns();

                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment: filename=" + "AttendanceReport.xlsx");
                Response.BinaryWrite(Ep.GetAsByteArray());
                Response.End();

                //  byte[] data = TempData[fileGuid] as byte[];
                // return File(data, "application/vnd.ms-excel", fileName);
            }
            else
            {
                // Problem - Log the error, generate a blank file,
                //           redirect to another controller action - whatever fits with your application
                //  return new EmptyResult();
            }
        }


        public ActionResult CreateSupplierCountryExcel(IEnumerable<SupplierCountry> Obj)
        {
            //https://riptutorial.com/epplus/example/26423/fill-with-a-datatable-from-an-sql-query-or-stored-procedure


            string handle = Guid.NewGuid().ToString();
            DataTable table1 = new DataTable("SupplierCountry");
            table1 = FunctionUnit.LINQResultToDataTable(Obj);





            TempData[handle] = table1;


            return new JsonResult()
            {
                Data = new { FileGuid = handle }
            };

        }


        [HttpGet]
        public void DownloadSupplierCountryExcel(string fileGuid)
        {
            if (TempData[fileGuid] != null)
            {
                FileInfo Template = new FileInfo(Path.Combine(Server.MapPath("~/ExcelTemplate/"), "SupplierCountry.xlsx"));
                string handle = Guid.NewGuid().ToString();

                ExcelPackage Ep = new ExcelPackage(Template);


                ExcelWorksheet Sheet = Ep.Workbook.Worksheets["Sheet1"];

                DataTable table1 = new DataTable("SupplierCountry");
                table1 = (DataTable)TempData[fileGuid];


                int row = 6;
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetUserByID(userId);
                var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);


                Sheet.Cells[string.Format("A{0}", 1)].Value = CoInfo.ArabicName;
                Sheet.Cells[string.Format("A{0}", 2)].Value = @Resources.Resource.SupplierBank;


                if (@Resources.Resource.CurLang == "Arb")
                {
                    for (int i = 0; i <= table1.Rows.Count - 1; i++)
                    {

                        //string sDate = table1.Rows[i]["sDate"].ToString();
                        Sheet.Cells[string.Format("A{0}", row)].Value = table1.Rows[i]["SupplierCountryID"];

                        Sheet.Cells[string.Format("B{0}", row)].Value = table1.Rows[i]["ArabicName"];






                        row++;
                    }

                }
                else
                {
                    for (int i = 0; i <= table1.Rows.Count - 1; i++)
                    {

                        //string sDate = table1.Rows[i]["sDate"].ToString();
                        Sheet.Cells[string.Format("A{0}", row)].Value = table1.Rows[i]["SupplierCountryID"];

                        Sheet.Cells[string.Format("B{0}", row)].Value = table1.Rows[i]["EnglishName"];






                        row++;
                    }

                }





                // Sheet.Cells["A1"].LoadFromDataTable(table1, true);

                Sheet.Cells["A:AZ"].AutoFitColumns();

                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment: filename=" + "AttendanceReport.xlsx");
                Response.BinaryWrite(Ep.GetAsByteArray());
                Response.End();

                //  byte[] data = TempData[fileGuid] as byte[];
                // return File(data, "application/vnd.ms-excel", fileName);
            }
            else
            {
                // Problem - Log the error, generate a blank file,
                //           redirect to another controller action - whatever fits with your application
                //  return new EmptyResult();
            }
        }




        public ActionResult CreateSupplierBranchBankExcel(IEnumerable<SupplierBranchBankVM> Obj)
        {
            //https://riptutorial.com/epplus/example/26423/fill-with-a-datatable-from-an-sql-query-or-stored-procedure


            string handle = Guid.NewGuid().ToString();
            DataTable table1 = new DataTable("SupplierBranchBankVM");
            table1 = FunctionUnit.LINQResultToDataTable(Obj);





            TempData[handle] = table1;


            return new JsonResult()
            {
                Data = new { FileGuid = handle }
            };

        }


        [HttpGet]
        public void DownloadSupplierBranchBankExcel(string fileGuid)
        {
            if (TempData[fileGuid] != null)
            {
                FileInfo Template = new FileInfo(Path.Combine(Server.MapPath("~/ExcelTemplate/"), "SupplierBranchBank.xlsx"));
                string handle = Guid.NewGuid().ToString();

                ExcelPackage Ep = new ExcelPackage(Template);


                ExcelWorksheet Sheet = Ep.Workbook.Worksheets["Sheet1"];

                DataTable table1 = new DataTable("SupplierBranchBankVM");
                table1 = (DataTable)TempData[fileGuid];


                int row = 6;
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetUserByID(userId);
                var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);


                Sheet.Cells[string.Format("A{0}", 1)].Value = CoInfo.ArabicName;
                Sheet.Cells[string.Format("A{0}", 2)].Value = @Resources.Resource.SupplierBranchBank;



                for (int i = 0; i <= table1.Rows.Count - 1; i++)
                {

                    //string sDate = table1.Rows[i]["sDate"].ToString();
                    Sheet.Cells[string.Format("A{0}", row)].Value = table1.Rows[i]["SupplierBranchBankID"];

                    Sheet.Cells[string.Format("B{0}", row)].Value = table1.Rows[i]["SupplierBranchBankName"];
                    Sheet.Cells[string.Format("C{0}", row)].Value = table1.Rows[i]["SupplierBankName"];






                    row++;
                }







                // Sheet.Cells["A1"].LoadFromDataTable(table1, true);

                Sheet.Cells["A:AZ"].AutoFitColumns();

                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment: filename=" + "AttendanceReport.xlsx");
                Response.BinaryWrite(Ep.GetAsByteArray());
                Response.End();

                //  byte[] data = TempData[fileGuid] as byte[];
                // return File(data, "application/vnd.ms-excel", fileName);
            }
            else
            {
                // Problem - Log the error, generate a blank file,
                //           redirect to another controller action - whatever fits with your application
                //  return new EmptyResult();
            }
        }


        public ActionResult CreateSupplierCountryBankExcel(IEnumerable<SupplierCountryBank> Obj)
        {
            //https://riptutorial.com/epplus/example/26423/fill-with-a-datatable-from-an-sql-query-or-stored-procedure


            string handle = Guid.NewGuid().ToString();
            DataTable table1 = new DataTable("SupplierCountryBank");
            table1 = FunctionUnit.LINQResultToDataTable(Obj);





            TempData[handle] = table1;


            return new JsonResult()
            {
                Data = new { FileGuid = handle }
            };

        }


        [HttpGet]
        public void DownloadSupplierCountryBankExcel(string fileGuid)
        {
            if (TempData[fileGuid] != null)
            {
                FileInfo Template = new FileInfo(Path.Combine(Server.MapPath("~/ExcelTemplate/"), "SupplierCountryBank.xlsx"));
                string handle = Guid.NewGuid().ToString();

                ExcelPackage Ep = new ExcelPackage(Template);


                ExcelWorksheet Sheet = Ep.Workbook.Worksheets["Sheet1"];

                DataTable table1 = new DataTable("SupplierCountryBank");
                table1 = (DataTable)TempData[fileGuid];


                int row = 6;
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetUserByID(userId);
                var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);


                Sheet.Cells[string.Format("A{0}", 1)].Value = CoInfo.ArabicName;
                Sheet.Cells[string.Format("A{0}", 2)].Value = @Resources.Resource.SupplierCountryBank;

                if (@Resources.Resource.CurLang == "Arb")
                {
                    for (int i = 0; i <= table1.Rows.Count - 1; i++)
                    {

                        //string sDate = table1.Rows[i]["sDate"].ToString();
                        Sheet.Cells[string.Format("A{0}", row)].Value = table1.Rows[i]["SupplierCountryBankID"];

                        Sheet.Cells[string.Format("B{0}", row)].Value = table1.Rows[i]["ArabicName"];






                        row++;
                    }


                }
                else
                {

                    for (int i = 0; i <= table1.Rows.Count - 1; i++)
                    {

                        //string sDate = table1.Rows[i]["sDate"].ToString();
                        Sheet.Cells[string.Format("A{0}", row)].Value = table1.Rows[i]["SupplierCountryBankID"];

                        Sheet.Cells[string.Format("B{0}", row)].Value = table1.Rows[i]["EnglishName"];






                        row++;
                    }

                }







                // Sheet.Cells["A1"].LoadFromDataTable(table1, true);

                Sheet.Cells["A:AZ"].AutoFitColumns();

                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment: filename=" + "AttendanceReport.xlsx");
                Response.BinaryWrite(Ep.GetAsByteArray());
                Response.End();

                //  byte[] data = TempData[fileGuid] as byte[];
                // return File(data, "application/vnd.ms-excel", fileName);
            }
            else
            {
                // Problem - Log the error, generate a blank file,
                //           redirect to another controller action - whatever fits with your application
                //  return new EmptyResult();
            }
        }





        public ActionResult CreateSupplierCityExcel(IEnumerable<SupplierCityVM> Obj)
        {
            //https://riptutorial.com/epplus/example/26423/fill-with-a-datatable-from-an-sql-query-or-stored-procedure


            string handle = Guid.NewGuid().ToString();
            DataTable table1 = new DataTable("SupplierCityVM");
            table1 = FunctionUnit.LINQResultToDataTable(Obj);





            TempData[handle] = table1;


            return new JsonResult()
            {
                Data = new { FileGuid = handle }
            };

        }


        [HttpGet]
        public void DownloadSupplierCityExcel(string fileGuid)
        {
            if (TempData[fileGuid] != null)
            {
                FileInfo Template = new FileInfo(Path.Combine(Server.MapPath("~/ExcelTemplate/"), "SupplierCity.xlsx"));
                string handle = Guid.NewGuid().ToString();

                ExcelPackage Ep = new ExcelPackage(Template);


                ExcelWorksheet Sheet = Ep.Workbook.Worksheets["Sheet1"];

                DataTable table1 = new DataTable("SupplierCityVM");
                table1 = (DataTable)TempData[fileGuid];


                int row = 6;
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetUserByID(userId);
                var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);


                Sheet.Cells[string.Format("A{0}", 1)].Value = CoInfo.ArabicName;
                Sheet.Cells[string.Format("A{0}", 2)].Value = @Resources.Resource.SupplierCity;


                for (int i = 0; i <= table1.Rows.Count - 1; i++)
                {

                    //string sDate = table1.Rows[i]["sDate"].ToString();
                    Sheet.Cells[string.Format("A{0}", row)].Value = table1.Rows[i]["SupplierCityID"];
                    Sheet.Cells[string.Format("B{0}", row)].Value = table1.Rows[i]["SupplierCityName"];
                    Sheet.Cells[string.Format("C{0}", row)].Value = table1.Rows[i]["SupplierCountryName"];






                    row++;
                }











                // Sheet.Cells["A1"].LoadFromDataTable(table1, true);

                Sheet.Cells["A:AZ"].AutoFitColumns();

                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment: filename=" + "AttendanceReport.xlsx");
                Response.BinaryWrite(Ep.GetAsByteArray());
                Response.End();

                //  byte[] data = TempData[fileGuid] as byte[];
                // return File(data, "application/vnd.ms-excel", fileName);
            }
            else
            {
                // Problem - Log the error, generate a blank file,
                //           redirect to another controller action - whatever fits with your application
                //  return new EmptyResult();
            }
        }





        public ActionResult CreateSupplierCityBankExcel(IEnumerable<SupplierCityBankVM> Obj)
        {
            //https://riptutorial.com/epplus/example/26423/fill-with-a-datatable-from-an-sql-query-or-stored-procedure


            string handle = Guid.NewGuid().ToString();
            DataTable table1 = new DataTable("SupplierCityBankVM");
            table1 = FunctionUnit.LINQResultToDataTable(Obj);





            TempData[handle] = table1;


            return new JsonResult()
            {
                Data = new { FileGuid = handle }
            };

        }


        [HttpGet]
        public void DownloadSupplierCityBankExcel(string fileGuid)
        {
            if (TempData[fileGuid] != null)
            {
                FileInfo Template = new FileInfo(Path.Combine(Server.MapPath("~/ExcelTemplate/"), "SupplierCityBank.xlsx"));
                string handle = Guid.NewGuid().ToString();

                ExcelPackage Ep = new ExcelPackage(Template);


                ExcelWorksheet Sheet = Ep.Workbook.Worksheets["Sheet1"];

                DataTable table1 = new DataTable("SupplierCityBankVM");
                table1 = (DataTable)TempData[fileGuid];


                int row = 6;
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetUserByID(userId);
                var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);


                Sheet.Cells[string.Format("A{0}", 1)].Value = CoInfo.ArabicName;
                Sheet.Cells[string.Format("A{0}", 2)].Value = @Resources.Resource.SupplierCityBank;


                for (int i = 0; i <= table1.Rows.Count - 1; i++)
                {

                    //string sDate = table1.Rows[i]["sDate"].ToString();
                    Sheet.Cells[string.Format("A{0}", row)].Value = table1.Rows[i]["SupplierCityBankID"];
                    Sheet.Cells[string.Format("B{0}", row)].Value = table1.Rows[i]["SupplierCityBankName"];
                    Sheet.Cells[string.Format("C{0}", row)].Value = table1.Rows[i]["SupplierCountryBankName"];






                    row++;
                }











                // Sheet.Cells["A1"].LoadFromDataTable(table1, true);

                Sheet.Cells["A:AZ"].AutoFitColumns();

                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment: filename=" + "AttendanceReport.xlsx");
                Response.BinaryWrite(Ep.GetAsByteArray());
                Response.End();

                //  byte[] data = TempData[fileGuid] as byte[];
                // return File(data, "application/vnd.ms-excel", fileName);
            }
            else
            {
                // Problem - Log the error, generate a blank file,
                //           redirect to another controller action - whatever fits with your application
                //  return new EmptyResult();
            }
        }





        public ActionResult CreateCustomerCityExcel(IEnumerable<CustomerCity> Obj)
        {
            //https://riptutorial.com/epplus/example/26423/fill-with-a-datatable-from-an-sql-query-or-stored-procedure


            string handle = Guid.NewGuid().ToString();
            DataTable table1 = new DataTable("CustomerCity");
            table1 = FunctionUnit.LINQResultToDataTable(Obj);





            TempData[handle] = table1;


            return new JsonResult()
            {
                Data = new { FileGuid = handle }
            };

        }


        [HttpGet]
        public void DownloadCustomerCityExcel(string fileGuid)
        {
            if (TempData[fileGuid] != null)
            {
                FileInfo Template = new FileInfo(Path.Combine(Server.MapPath("~/ExcelTemplate/"), "CustomerCity.xlsx"));
                string handle = Guid.NewGuid().ToString();

                ExcelPackage Ep = new ExcelPackage(Template);


                ExcelWorksheet Sheet = Ep.Workbook.Worksheets["Sheet1"];

                DataTable table1 = new DataTable("CustomerCity");
                table1 = (DataTable)TempData[fileGuid];


                int row = 6;
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetUserByID(userId);
                var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);


                Sheet.Cells[string.Format("A{0}", 1)].Value = CoInfo.ArabicName;
                Sheet.Cells[string.Format("A{0}", 2)].Value = @Resources.Resource.CustomerCity;


                if (@Resources.Resource.CurLang == "Arb")

                {
                    for (int i = 0; i <= table1.Rows.Count - 1; i++)
                    {

                        //string sDate = table1.Rows[i]["sDate"].ToString();
                        Sheet.Cells[string.Format("A{0}", row)].Value = table1.Rows[i]["CustomerCityID"];
                        Sheet.Cells[string.Format("B{0}", row)].Value = table1.Rows[i]["ArabicName"];






                        row++;
                    }


                }
                else
                {
                    for (int i = 0; i <= table1.Rows.Count - 1; i++)
                    {

                        //string sDate = table1.Rows[i]["sDate"].ToString();
                        Sheet.Cells[string.Format("A{0}", row)].Value = table1.Rows[i]["CustomerCityID"];
                        Sheet.Cells[string.Format("B{0}", row)].Value = table1.Rows[i]["EnglishName"];






                        row++;
                    }

                }



                // Sheet.Cells["A1"].LoadFromDataTable(table1, true);

                Sheet.Cells["A:AZ"].AutoFitColumns();

                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment: filename=" + "AttendanceReport.xlsx");
                Response.BinaryWrite(Ep.GetAsByteArray());
                Response.End();

                //  byte[] data = TempData[fileGuid] as byte[];
                // return File(data, "application/vnd.ms-excel", fileName);
            }
            else
            {
                // Problem - Log the error, generate a blank file,
                //           redirect to another controller action - whatever fits with your application
                //  return new EmptyResult();
            }
        }





        public ActionResult CreateCustomerAreaExcel(IEnumerable<CustomerAreaVM> Obj)
        {
            //https://riptutorial.com/epplus/example/26423/fill-with-a-datatable-from-an-sql-query-or-stored-procedure


            string handle = Guid.NewGuid().ToString();
            DataTable table1 = new DataTable("CustomerAreaVM");
            table1 = FunctionUnit.LINQResultToDataTable(Obj);





            TempData[handle] = table1;


            return new JsonResult()
            {
                Data = new { FileGuid = handle }
            };

        }


        [HttpGet]
        public void DownloadCustomerAreaExcel(string fileGuid)
        {
            if (TempData[fileGuid] != null)
            {
                FileInfo Template = new FileInfo(Path.Combine(Server.MapPath("~/ExcelTemplate/"), "CustomerArea.xlsx"));
                string handle = Guid.NewGuid().ToString();

                ExcelPackage Ep = new ExcelPackage(Template);


                ExcelWorksheet Sheet = Ep.Workbook.Worksheets["Sheet1"];

                DataTable table1 = new DataTable("CustomerAreaVM");
                table1 = (DataTable)TempData[fileGuid];


                int row = 6;
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetUserByID(userId);
                var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);


                Sheet.Cells[string.Format("A{0}", 1)].Value = CoInfo.ArabicName;
                Sheet.Cells[string.Format("A{0}", 2)].Value = @Resources.Resource.CustomerArea;



                for (int i = 0; i <= table1.Rows.Count - 1; i++)
                {

                    //string sDate = table1.Rows[i]["sDate"].ToString();
                    Sheet.Cells[string.Format("A{0}", row)].Value = table1.Rows[i]["CustomerAreaID"];
                    Sheet.Cells[string.Format("B{0}", row)].Value = table1.Rows[i]["CustomerAreaName"];
                    Sheet.Cells[string.Format("C{0}", row)].Value = table1.Rows[i]["CustomerCityName"];






                    row++;
                }





                // Sheet.Cells["A1"].LoadFromDataTable(table1, true);

                Sheet.Cells["A:AZ"].AutoFitColumns();

                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment: filename=" + "AttendanceReport.xlsx");
                Response.BinaryWrite(Ep.GetAsByteArray());
                Response.End();

                //  byte[] data = TempData[fileGuid] as byte[];
                // return File(data, "application/vnd.ms-excel", fileName);
            }
            else
            {
                // Problem - Log the error, generate a blank file,
                //           redirect to another controller action - whatever fits with your application
                //  return new EmptyResult();
            }
        }





        public ActionResult CreateTrialBalanceExcel(IEnumerable<TrialBalanceVM> Obj)
        {
            //https://riptutorial.com/epplus/example/26423/fill-with-a-datatable-from-an-sql-query-or-stored-procedure


            string handle = Guid.NewGuid().ToString();
            DataTable table1 = new DataTable("TrialBalanceVM");
            table1 = FunctionUnit.LINQResultToDataTable(Obj);





            TempData[handle] = table1;


            return new JsonResult()
            {
                Data = new { FileGuid = handle }
            };

        }


        [HttpGet]
        public void DownloadTrialBalanceExcel(string fileGuid)
        {
            if (TempData[fileGuid] != null)
            {
                FileInfo Template = new FileInfo(Path.Combine(Server.MapPath("~/ExcelTemplate/"), "TrialBalance.xlsx"));
                string handle = Guid.NewGuid().ToString();

                ExcelPackage Ep = new ExcelPackage(Template);

                ExcelWorksheet Sheet = Ep.Workbook.Worksheets["Sheet1"];

                DataTable table1 = new DataTable("TrialBalanceVM");
                table1 = (DataTable)TempData[fileGuid];


                int row = 6;
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetUserByID(userId);
                var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);


                Sheet.Cells[string.Format("A{0}", 1)].Value = CoInfo.ArabicName;
                Sheet.Cells[string.Format("A{0}", 2)].Value = @Resources.Resource.TrialBalance;



                for (int i = 0; i <= table1.Rows.Count - 1; i++)
                {

                    //string sDate = table1.Rows[i]["sDate"].ToString();
                    Sheet.Cells[string.Format("A{0}", row)].Value = table1.Rows[i]["MainAccount"];
                    Sheet.Cells[string.Format("B{0}", row)].Value = table1.Rows[i]["AccountNumber"];
                    Sheet.Cells[string.Format("C{0}", row)].Value = table1.Rows[i]["Name"];
                    Sheet.Cells[string.Format("D{0}", row)].Value = table1.Rows[i]["DebitBalance"];
                    Sheet.Cells[string.Format("E{0}", row)].Value = table1.Rows[i]["CreditBalance"];
                    Sheet.Cells[string.Format("F{0}", row)].Value = table1.Rows[i]["DebitTransAction"];
                    Sheet.Cells[string.Format("G{0}", row)].Value = table1.Rows[i]["CreditTransAction"];
                    Sheet.Cells[string.Format("H{0}", row)].Value = table1.Rows[i]["NetDebit"];
                    Sheet.Cells[string.Format("I{0}", row)].Value = table1.Rows[i]["NetCredit"];






                    row++;
                }





                // Sheet.Cells["A1"].LoadFromDataTable(table1, true);

                Sheet.Cells["A:AZ"].AutoFitColumns();

                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment: filename=" + "AttendanceReport.xlsx");
                Response.BinaryWrite(Ep.GetAsByteArray());
                Response.End();

                //  byte[] data = TempData[fileGuid] as byte[];
                // return File(data, "application/vnd.ms-excel", fileName);
            }
            else
            {
                // Problem - Log the error, generate a blank file,
                //           redirect to another controller action - whatever fits with your application
                //  return new EmptyResult();
            }
        }

    }

}

