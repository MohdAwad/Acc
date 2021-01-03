using Acc.Helpers;
using Acc.Models;
using Acc.Persistence;
using Acc.Repositories;
using Acc.ViewModels;
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
    public class AttachController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public AttachController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
            //1212121
        }


        //---Start Item Gallary----//
        [HttpPost]
        public ActionResult UploadNewItemLogo()
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
                          //  string extension = Path.GetExtension(fname);
                            ImgName = fname;

                           // ImgName = fname.Substring(0, fname.IndexOf('.'));

                            // ImgName = fname;
                            // Get the complete folder path and store the file inside it.//ImgName +  
                          //  ImgName = Guid.NewGuid() + extension;

                            // Get the complete folder path and store the file inside it.  
                            fname = Path.Combine(Server.MapPath("~/ItemLogo/"), ImgName);
                            file.SaveAs(fname);

 


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


        [HttpPost]
        public ActionResult UploadUpdateItemLogo(string id)
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
                            //  string extension = Path.GetExtension(fname);
                            ImgName = fname;

                            // ImgName = fname.Substring(0, fname.IndexOf('.'));

                            // ImgName = fname;
                            // Get the complete folder path and store the file inside it.//ImgName +  
                            //  ImgName = Guid.NewGuid() + extension;

                            // Get the complete folder path and store the file inside it.  
                            fname = Path.Combine(Server.MapPath("~/ItemLogo/"), ImgName);
                            file.SaveAs(fname);
                            _unitOfWork.St_ItemCard.UpdateLogo(id, ImgName);
                            _unitOfWork.Complete();




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
        public ActionResult AddItemGallary(string id)
        {

            St_ItemGallary Obj = new St_ItemGallary
            {
                ItemCode = id
            };
            return View(Obj);

          
        }
        public ActionResult DeleteItemAttach(int id)
        {
            try
            {


                if (id != 0)
                {
                    var userId = User.Identity.GetUserId();

                    var UserInfo = _unitOfWork.User.GetUserByID(userId);
                    if (UserInfo == null)
                    {
                        RedirectToAction("", "");
                    }

                    var Obj = _unitOfWork.St_ItemGallary.GetFileById(UserInfo.fCompanyId, id);


                    return PartialView("DeleteItemAttach", Obj);
                }



                return PartialView("DeleteItemAttach", new St_ItemGallary());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return View("Error");
            }
        }

        [HttpPost]
        public JsonResult DeleteItemAttachById(int id)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var Obj = _unitOfWork.St_ItemGallary.GetFileById(UserInfo.fCompanyId, id);
                string FileName = Obj.FileName;

                string path = Path.Combine(new DirectoryInfo(string.Format(@"{0}ItemGallary\{1}\{2}", base.Server.MapPath(@"\"), Obj.ItemCode,Obj.FileName)).ToString());
              


                _unitOfWork.St_ItemGallary.Delete(UserInfo.fCompanyId,id);
                _unitOfWork.Complete();

                if (System.IO.File.Exists(path))
                {
                    // If file found, delete it    
                    System.IO.File.Delete(path);
                   // Console.WriteLine("File deleted.");
                }

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

        public JsonResult GetAllItemsFiles(string id )
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);

               
                string ItemCode = id ;


                var AllTRansFile = _unitOfWork.St_ItemGallary.GetAllFiles(UserInfo.fCompanyId, ItemCode);
                if (AllTRansFile == null)
                {
                    return Json(new List<St_ItemGallary>(), JsonRequestBehavior.AllowGet);
                }

                return Json(AllTRansFile, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<St_ItemGallary>(), JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult SaveItemGalary(string id)
        {
            string ItemCode = id;
            


            bool flag = true;
            string fileName = "";
            string userId = base.User.Identity.GetUserId();
            try
            {

                var UserInfo = _unitOfWork.User.GetMyInfo(userId);


                string path = Path.Combine(new DirectoryInfo(string.Format(@"{0}ItemGallary\{1}", base.Server.MapPath(@"\"), ItemCode)).ToString());
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                foreach (string str4 in base.Request.Files)
                {
                    HttpPostedFileBase base2 = base.Request.Files[str4];
                    fileName = base2.FileName;
                    if ((base2 != null) && (base2.ContentLength > 0))
                    {
                        fileName = Path.GetFileName(base2.FileName);

                        string extension = Path.GetExtension(base2.FileName);
                        string sFileName = base2.FileName.Substring(0, base2.FileName.IndexOf('.'));
                        sFileName = Guid.NewGuid().ToString() + extension;
                        base2.SaveAs(Path.Combine(base.Server.MapPath(String.Format("~/ItemGallary/{0} ", ItemCode )), fileName));




                        St_ItemGallary objSave = new St_ItemGallary
                        {
                            FileName = fileName,
                            ItemCode=ItemCode,
                            CompanyID= UserInfo.fCompanyId



                        };

                        this._unitOfWork.St_ItemGallary.Add(objSave);
                        this._unitOfWork.Complete();

                 

                    }
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                flag = false;
            }
            return (!flag ? base.Json(new { Message = "Error in saving file" }) : base.Json(new { Message = fileName }));
        }


        public FileResult DownloadItemGallaryDocument(int id ,string id2) 
        {


          
           


            string userId = base.User.Identity.GetUserId();
            ApplicationUser userByID = _unitOfWork.User.GetUserByID(userId);
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);

            St_ItemGallary DocumentID = _unitOfWork.St_ItemGallary.GetFileById(UserInfo.fCompanyId, id);




            string path = Server.MapPath("/ItemGallary/"+id2+"/");
            byte[] fileBytes = System.IO.File.ReadAllBytes(path + DocumentID.FileName);
            string sfileName = DocumentID.FileName;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, sfileName);
        }


        ///--End Item Gallary--//

        public FileResult DownloadDocument(int id, string id2, string id3, string id4,int id5 )//nagham task
        { 
        
            
        int Year = id;
        string VoucherNumber = id2;
        string TransactionKindNo = id4;
        string CompanyTransactionKindNo = id3;          
        int sfileid = id5;


        string userId = base.User.Identity.GetUserId();
        ApplicationUser userByID = _unitOfWork.User.GetUserByID(userId);

       TransActionFile DocumentID = _unitOfWork.TransActionFile.GetTransActionFilebyId(sfileid);




        string path = Server.MapPath("/TransActionFiles/" + Year + "/" + VoucherNumber + "/" + TransactionKindNo + "/" + CompanyTransactionKindNo + "/");
       byte[] fileBytes = System.IO.File.ReadAllBytes(path + DocumentID.FileName);
        string sfileName = DocumentID.FileName;
        return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, sfileName);
    }



        public ActionResult SaveTransActionFile(int id, string id2, string id3, string id4)
        {
            int Year = id;
            string VoucherNumber = id2;
            string TransactionKindNo = id4;
            string CompanyTransactionKindNo = id3;



            bool flag = true;
            string fileName = "";
            string userId = base.User.Identity.GetUserId();
            try
            {




                string path = Path.Combine(new DirectoryInfo(string.Format(@"{0}TransActionFiles\{1}\{2}\{3}\{4}", base.Server.MapPath(@"\"), Year, VoucherNumber, TransactionKindNo, CompanyTransactionKindNo)).ToString());
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                foreach (string str4 in base.Request.Files)
                {
                    HttpPostedFileBase base2 = base.Request.Files[str4];
                    fileName = base2.FileName;
                    if ((base2 != null) && (base2.ContentLength > 0))
                    {
                        fileName = Path.GetFileName(base2.FileName);

                        string extension = Path.GetExtension(base2.FileName);
                        string sFileName = base2.FileName.Substring(0, base2.FileName.IndexOf('.'));
                        sFileName = Guid.NewGuid().ToString() + extension;
                        base2.SaveAs(Path.Combine(base.Server.MapPath(String.Format("~/TransActionFiles/{0}/{1}/{2}/{3}", Year, VoucherNumber, TransactionKindNo, CompanyTransactionKindNo)), sFileName));




                        TransActionFile objSave = new TransActionFile
                        {
                            FileName = sFileName,
                            Year = Year,
                            VoucherNumber = VoucherNumber,
                            TransactionKindNo = TransactionKindNo,
                            CompanyTransactionKindNo = CompanyTransactionKindNo



                        };

                        this._unitOfWork.TransActionFile.Add(objSave);
                        this._unitOfWork.Complete();

                        //EmployeeDocument objToSave = new EmployeeDocument
                        //{
                        //    CompanyModelID = id,
                        //    DocName = base2.FileName,
                        //    DocPath = str8,
                        //    EmployeeId = id2,

                        //};
                        //this._unitOfWork.EmployeeDocument.AddModify(objToSave);
                        //this._unitOfWork.Complete();




                    }
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                flag = false;
            }
            return (!flag ? base.Json(new { Message = "Error in saving file" }) : base.Json(new { Message = fileName }));
        }


        public ActionResult SaveTransActionBank(int id,  string id2, string id3, string id4)
        {
            int Year = id;
            string VoucherNumber = id2;
            string TransactionKindNo = id4;//22
            string CompanyTransactionKindNo = id3;//1



            bool flag = true;
            string fileName = "";
            string userId = base.User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);


            var fCompany = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            try
            {
                

                string path = Path.Combine(new DirectoryInfo(string.Format(@"{0}TransActionFiles\{1}\{2}\{3}\{4} ", base.Server.MapPath(@"\"), Year, VoucherNumber, TransactionKindNo, CompanyTransactionKindNo)).ToString());
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                foreach (string str4 in base.Request.Files)
                {
                    HttpPostedFileBase base2 = base.Request.Files[str4];
                    fileName = base2.FileName;
                    if ((base2 != null) && (base2.ContentLength > 0))
                    {
                         
                        fCompany = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);

                        fileName = Path.GetFileName(base2.FileName);

                        string extension = Path.GetExtension(base2.FileName);
                        string sFileName = base2.FileName.Substring(0, base2.FileName.IndexOf('.'));
                        sFileName = Guid.NewGuid().ToString() + extension;
                        base2.SaveAs(Path.Combine(base.Server.MapPath(String.Format("~/TransActionFiles/{0}/{1}/{2}/{3}", Year, VoucherNumber, TransactionKindNo, CompanyTransactionKindNo)), sFileName));




                        TransActionFile objSave = new TransActionFile
                        {
                            FileName = sFileName,
                            CompanyId = UserInfo.fCompanyId,

                            Year = Year,
                            VoucherNumber = VoucherNumber,
                            TransactionKindNo = TransactionKindNo,
                            CompanyTransactionKindNo = CompanyTransactionKindNo


                    };

                        this._unitOfWork.TransActionFile.Add(objSave);
                        this._unitOfWork.Complete();

                        //EmployeeDocument objToSave = new EmployeeDocument
                        //{
                        //    CompanyModelID = id,
                        //    DocName = base2.FileName,
                        //    DocPath = str8,
                        //    EmployeeId = id2,

                        //};
                        //this._unitOfWork.EmployeeDocument.AddModify(objToSave);
                        //this._unitOfWork.Complete();




                    }
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                flag = false;
            }
            return (!flag ? base.Json(new { Message = "Error in saving file" }) : base.Json(new { Message = fileName }));
        }
        public ActionResult SaveTransActionCash(int id, string id2, string id3, string id4)
        {
            int Year = id;
            string VoucherNumber = id2;
            string TransactionKindNo = id4;
            string CompanyTransactionKindNo = id3;



            bool flag = true;
            string fileName = "";
            string userId = base.User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);


            var fCompany = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            try
            {


                string path = Path.Combine(new DirectoryInfo(string.Format(@"{0}TransActionFiles\{1}\{2}\{3}\{4} ", base.Server.MapPath(@"\"), Year, VoucherNumber, TransactionKindNo, CompanyTransactionKindNo)).ToString());
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                foreach (string str4 in base.Request.Files)
                {
                    HttpPostedFileBase base2 = base.Request.Files[str4];
                    fileName = base2.FileName;
                    if ((base2 != null) && (base2.ContentLength > 0))
                    {

                        fCompany = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);

                        fileName = Path.GetFileName(base2.FileName);

                        string extension = Path.GetExtension(base2.FileName);
                        string sFileName = base2.FileName.Substring(0, base2.FileName.IndexOf('.'));
                        sFileName = Guid.NewGuid().ToString() + extension;
                        base2.SaveAs(Path.Combine(base.Server.MapPath(String.Format("~/TransActionFiles/{0}/{1}/{2}/{3}", Year, VoucherNumber, TransactionKindNo, CompanyTransactionKindNo)), sFileName));




                        TransActionFile objSave = new TransActionFile
                        {
                            FileName = sFileName,
                            CompanyId = UserInfo.fCompanyId,

                            Year = Year,
                            VoucherNumber = VoucherNumber,
                            TransactionKindNo = TransactionKindNo,
                            CompanyTransactionKindNo = CompanyTransactionKindNo

                        };

                        this._unitOfWork.TransActionFile.Add(objSave);
                        this._unitOfWork.Complete();

                        




                    }
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                flag = false;
            }
            return (!flag ? base.Json(new { Message = "Error in saving file" }) : base.Json(new { Message = fileName }));
        }
        public ActionResult UploadTransActionFile(int id, string id2, string id3, string id4)
        {
            TransActionFileVM Obj = new TransActionFileVM
            {
                Year = id,
                VoucherNumber = id2,
                TransactionKindNo = id4,
                CompanyTransactionKindNo = id3
            };

            return View(Obj);
        }
        [HttpGet]
        public JsonResult GetAllTRansFiles(int id, string id2, string id3, string id4 )
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var Type = _unitOfWork.TransActionFile.GetType(UserInfo.fCompanyId);
              

                int Year = id;
                int CompanyId = UserInfo.fCompanyId;
                string VoucherNumber = id2;
                string TransactionKindNo = id4;//1
                string CompanyTransactionKindNo = id3;//22
                


                var AllTRansFile = _unitOfWork.TransActionFile.GetAllTransactionFile(Year ,VoucherNumber,TransactionKindNo, CompanyTransactionKindNo);
                if (AllTRansFile == null)
                {
                    return Json(new List<TransActionFile>(), JsonRequestBehavior.AllowGet);
                }

                return Json(AllTRansFile, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<TransActionFile>(), JsonRequestBehavior.AllowGet);
            }

        }
         
        public ActionResult DeleteAttach(int id)
        {
            try
            {


                if (id != 0)
                {
                    var userId = User.Identity.GetUserId();

                    var UserInfo = _unitOfWork.User.GetUserByID(userId);
                    if (UserInfo == null)
                    {
                        RedirectToAction("", "");
                    }

                    var Obj = _unitOfWork.TransActionFile.GetTransActionFilebyId(id);


                    return PartialView("DeleteFile", Obj);
                }



                return PartialView("DeleteFile", new TransActionFile());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return View("Error");
            }
        }
        [HttpPost]
        public JsonResult DeleteAttachById(int id)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);

                var Obj = _unitOfWork.TransActionFile.GetTransActionFilebyId(id);

                string path = Path.Combine(new DirectoryInfo(string.Format(@"{0}TransActionFiles\{1}\{2}\{3}\{4}\{5}", base.Server.MapPath(@"\"), Obj.Year, Obj.VoucherNumber, Obj. TransactionKindNo, Obj.CompanyTransactionKindNo, Obj.FileName)).ToString());
                _unitOfWork.TransActionFile.Delete(id);
                _unitOfWork.Complete();
                if (System.IO.File.Exists(path))
                {
                    // If file found, delete it    
                    System.IO.File.Delete(path);
                    // Console.WriteLine("File deleted.");
                }



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
    }

}