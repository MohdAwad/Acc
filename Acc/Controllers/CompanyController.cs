using Acc.Helpers;
using Acc.Models;
using Acc.Persistence;
using Acc.Repositories;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Acc.Controllers
{
    [Authorize]
    public class CompanyController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        // GET: Company


        [HttpPost]
        public ActionResult UploadFiles()
        {
            try
            {
                string UserID = User.Identity.GetUserId();

                string fname = "";
                string ImgName = "";
                // Checking no of files injected in Request object  
                if (Request.Files.Count > 0)
                {

                    //var PersonalInfo = _context.PersonalInformations.FirstOrDefault(m => m.pi_id == UserID);


                    try
                    {
                        //  Get all files from Request object  
                        HttpFileCollectionBase files = Request.Files;
                        for (int i = 0; i < files.Count; i++)
                        {
                            //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                            //string filename = Path.GetFileName(Request.Files[i].FileName);  

                            HttpPostedFileBase file = files[i];


                            // Checking for Internet Explorer  
                            if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                            {
                                string[] testfiles = file.FileName.Split(new char[] { '\\' });
                                fname = testfiles[testfiles.Length - 1];
                            }
                            else
                            {
                                fname = file.FileName;
                            }
                            //   ImgName = fname.Substring(fname.IndexOf('.'));
                            string extension = Path.GetExtension(fname);
                            // ImgName = fname;

                            ImgName = fname.Substring(0, fname.IndexOf('.'));

                            // ImgName = fname;
                            // Get the complete folder path and store the file inside it.//ImgName +  
                            ImgName = Guid.NewGuid() + extension;

                            // Get the complete folder path and store the file inside it.  
                            fname = Path.Combine(Server.MapPath("~/CompanyLogo/"), ImgName);
                            file.SaveAs(fname);



                            // Byte[] x = WorkWithImage.imgToByteArray(fname);
                            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

                            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                            if (Company != null)
                            {

                                Company.CompanyLogo = ImgName;

                                // Company.

                                _unitOfWork.Company.Update(Company);
                                _unitOfWork.Complete();

                            }



                          




                        }
                        // Returns message that successfully uploaded  
                        return Json(ImgName);
                    }
                    catch (Exception ex)
                    {
                        return Json("Error occurred. Error details: " + ex.Message);
                    }
                }
                else
                {
                    return Json("No files selected.");
                }
            }



            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return View("Error");
            }
        }
        public ActionResult Index()
        {
             var userId = User.Identity.GetUserId();

            var UserInfo = _unitOfWork.User.GetMyInfo(userId);

            if (UserInfo == null)
            {

                return RedirectToAction("Login", "Account");
            }

            var CompanyInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);

            if (CompanyInfo == null)
            {
                CompanyInfo = new Company();
                return View(CompanyInfo);
            }
            else
            {
                return View(CompanyInfo);
            }
        }

        [HttpPost]
        public JsonResult Save(Company ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();


            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);


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

            try
                
            {
                ObjToSave.AccountChartZero = FunctionUnit.ConvertChartToZero(ObjToSave.AccountChart);
                if (ObjToSave.WorkWithCostCenter)
                {
                    ObjToSave.CostChartZero = FunctionUnit.ConvertChartToZero(ObjToSave.CostChart);
                }
                else 
                {
                    ObjToSave.CostChartZero = "";
                };
                _unitOfWork.Company.Add(ObjToSave);
                _unitOfWork.Complete();

                var CompanyID = ObjToSave.CompanyID;

                _unitOfWork.User.UpdateCompanyID(UserID, CompanyID);
                _unitOfWork.Complete();

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

        [Authorize(Roles = "CoOwner,UpdateCompany")]
        public JsonResult Update(Company ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();


            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var CostChartExist = _unitOfWork.NativeSql.CheckCostChartExist(UserInfo.fCompanyId);
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

            try
            {
                ObjToSave.AccountChartZero = FunctionUnit.ConvertChartToZero(ObjToSave.AccountChart);
                if (ObjToSave.WorkWithCostCenter)
                {
                    ObjToSave.CostChartZero = FunctionUnit.ConvertChartToZero(ObjToSave.CostChart);
                }
                else
                {
                    ObjToSave.CostChartZero = "";
                };
                _unitOfWork.Company.Update(ObjToSave);
                _unitOfWork.Complete();
                Msg.Code = 1;
                Msg.Msg = Resources.Resource.UpdatedSuccessfully;
                return Json(Msg, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                Msg.Code = 0;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult CheckCostChartExist()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var CostChartExist = _unitOfWork.NativeSql.CheckCostChartExist(UserInfo.fCompanyId);
            return Json(CostChartExist, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult CheckAccountChartExist()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var AccountChartExist = _unitOfWork.NativeSql.AccountChartExist(UserInfo.fCompanyId);
            return Json(AccountChartExist, JsonRequestBehavior.AllowGet);
        }
    }
}