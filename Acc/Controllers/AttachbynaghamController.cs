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
    public class AttachbynaghamController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public AttachbynaghamController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
            //1212121
        }

        public ActionResult SaveTransActionFile(int id, string id2, string id3, string id4)
        {
            int Year = id;
            string VoucherNumber = id2;
            string CompanyTransactionKindNo = id4;
            string TransactionKindNo = id3;



            bool flag = true;
            string fileName = "";
            string userId = base.User.Identity.GetUserId();
            try
            {




                string path = Path.Combine(new DirectoryInfo(string.Format(@"{0}TransActionFiles\{1}\{2}\{3}\{4}", base.Server.MapPath(@"\"), Year, TransactionKindNo, CompanyTransactionKindNo, VoucherNumber)).ToString());
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
                        base2.SaveAs(Path.Combine(base.Server.MapPath(String.Format("~/TransActionFiles/{0}/{1}/{2}/{3}", Year, TransactionKindNo, CompanyTransactionKindNo, VoucherNumber)), sFileName));




                        TransActionFile objSave = new TransActionFile
                        {
                            FileName = sFileName,
                            Year = Year,
                            CompanyTransactionKindNo = CompanyTransactionKindNo,
                            TransactionKindNo = TransactionKindNo,
                            VoucherNumber = VoucherNumber


                        };

                        _unitOfWork.TransActionFile.Add(objSave);
                        _unitOfWork.Complete();





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
        // GET: Attach
        public ActionResult UploadTransActionFile(int id, string id2, string id3, string id4)
        {
            TransActionFileVM Obj = new TransActionFileVM
            {
                Year = id,
                VoucherNumber = id2,
                CompanyTransactionKindNo = id4,
                TransactionKindNo = id3
            };
            return View(Obj);
        }


        [HttpGet]
        public JsonResult GetAllTRansFiles(int id, string id2, string id3, string id4)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);

                int Year = id;
                string VoucherNumber = id2;
                string CompanyTransactionKindNo = id4;
                string TransactionKindNo = id3;


                var AllTRansFile = _unitOfWork.TransActionFile.GetAllTransactionFile(Year,VoucherNumber,TransactionKindNo, CompanyTransactionKindNo);
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

        //public ActionResult DeleteAttach(int id)
        //{
        //    try
        //    {


        //        if (id != 0)
        //        {
        //            var userId = User.Identity.GetUserId();

        //            var UserInfo = _unitOfWork.User.GetUserByID(userId);
        //            if (UserInfo == null)
        //            {
        //                RedirectToAction("", "");
        //            }

        //            var Obj = _unitOfWork.TransActionFile.GetTransActionFilebyId(id);


        //            return PartialView("DeleteFilebynagham", Obj);
        //        }



        //        return PartialView("DeleteFilebynagham", new TransActionFile());
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.Error = ex.Message.ToString();
        //        return View("Error");
        //    }
        //}


        [HttpPost]
        public JsonResult DeleteAttachById(int id)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);






                _unitOfWork.TransActionFile.Delete(id);
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




    }

}