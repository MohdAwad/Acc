using Acc.Helpers;
using Acc.Models;
using Acc.Persistence;
using Acc.Repositories;
using Acc.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Acc.Controllers
{
    [Authorize]
    public class UsersController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsersController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        public ActionResult Modify(string id)
        {
            var userId = User.Identity.GetUserId();
            var CoInfo = _unitOfWork.User.GetUserByID(userId);


            var UInfo = _unitOfWork.User.GetUserByIDAndCo(CoInfo.fCompanyId, id);
            if (UInfo == null)
            {
                return RedirectToAction("Index");
            }

            UserViewModel Obj = new UserViewModel
            {
                Email = UInfo.Email,
                UserId = UInfo.UserId,

                AccountStatus = UInfo.AccountStatus,


                UserType = UInfo.UserType,

                Password = UInfo.PasswordHash,
                ConfirmPassword = UInfo.PasswordHash



            };
            return PartialView("Modify", Obj);

        }
        public ActionResult ModifyPermission(string id)
        {
            var userId = User.Identity.GetUserId();
            var CoInfo = _unitOfWork.User.GetUserByID(userId);


            var UInfo = _unitOfWork.User.GetUserByIDAndCo(CoInfo.fCompanyId, id);
            if (UInfo == null)
            {
                return RedirectToAction("Index");
            }

            //UserAccountVM Obj = new UserAccountVM
            //{
            //    Email = UInfo.Email,
            //    UserId = UInfo.UserId,
            //    Id = UInfo.Id,

            //    AccountStatus = UInfo.AccountStatus,


            //    UserType = UInfo.UserType,

            //    Password = UInfo.PasswordHash,
            //    ConfirmPassword = UInfo.PasswordHash,
            //    ApplicationUser=UInfo



            //};
            return View("ModifyPermission", UInfo);

        }

        [HttpGet]
        public JsonResult GetUser()
        {
            var userId = User.Identity.GetUserId();

            var CoData = _unitOfWork.User.GetUserByID(userId);
            if (CoData == null)
            {
                return Json(new List<ApplicationUser>(), JsonRequestBehavior.AllowGet);

            }
            var u = _unitOfWork.User.GetAllUsers(CoData.fCompanyId);
            u = u.Where(m => m.UserType == 0);
            return Json(u, JsonRequestBehavior.AllowGet);


        }
        [Authorize(Roles = "CoOwner,ShowUser")]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var CoInfo = _unitOfWork.User.GetUserByID(userId);




            if (CoInfo == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var Obj = new UserViewModel();
            // var Job = _unitOfWork.User.GetAllUsers(CoInfo.fCompanyId);
            // if (Job == null)
            //   Job = new List<ApplicationUser>();
            // var DepatrView = Job.Select(Mapper.Map<ApplicationUser, UserViewModel>);


            return View(Obj);




        }
        public ActionResult Merchant()
        {
            var userId = User.Identity.GetUserId();
            var CoInfo = _unitOfWork.User.GetUserByID(userId);




            if (CoInfo == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var Obj = new UserViewModel();
            // var Job = _unitOfWork.User.GetAllUsers(CoInfo.fCompanyId);
            // if (Job == null)
            //   Job = new List<ApplicationUser>();
            // var DepatrView = Job.Select(Mapper.Map<ApplicationUser, UserViewModel>);


            return View(Obj);




        }

        [Authorize(Roles = "CoOwner,AddUser")]
        public ActionResult Add()
        {
            var userId = User.Identity.GetUserId();
            var CoInfo = _unitOfWork.User.GetUserByID(userId);
            if (CoInfo == null)
            {
                RedirectToAction("Login", "Account");
            }


            var JOb = new UserViewModel();


            //  JOb.JobId = _unitOfWork..GetMaxSerial(CoInfo.CompanyProfileId);

            return View("Add", JOb);
        }

        [Authorize(Roles = "CoOwner,UpdateUser")]
        public ActionResult ChangeUserPassword(string id)
        {
            if (id != "")
            {
                var userId = User.Identity.GetUserId();
                var CoInfo = _unitOfWork.User.GetUserByID(userId);
                if (CoInfo == null)
                {
                    RedirectToAction("Login", "Account");
                }

                var JobInfo = _unitOfWork.User.GetUserByIDAndCo(CoInfo.fCompanyId, id);
                if (JobInfo == null)
                    return RedirectToAction("Index");

                var JobtV = new UserViewModel();
                JobtV.fCompanyId = JobInfo.fCompanyId;
                JobtV.Password = "";

                JobtV.Email = JobInfo.Email;
                JobtV.Id = JobInfo.Id;





                return PartialView("ChangeUserPassword", JobtV);
            }

            var Depart = new UserViewModel();

            return PartialView("ChangeUserPassword", Depart);
        }

  

        //public ActionResult Delete(int id)
        //{
        //    if (id != 0)
        //    {
        //        var userId = User.Identity.GetUserId();
        //        var CoInfo = _unitOfWork.User.GetUserByID(userId);
        //        if (CoInfo.fCompanyId == 0)
        //        {
        //            RedirectToAction("Login", "Account");
        //        }

        //        var DepartInfo = _unitOfWork.Job.GetJobByID(CoInfo.fCompanyId, id);
        //        var DepartV = new JobViewModel();
        //        DepartV.CompanyProfileId = DepartInfo.CompanyProfileId;
        //        DepartV.JobArName = DepartInfo.JobArName;
        //        DepartV.JobEnName = DepartInfo.JobEnName;
        //        DepartV.JobId = DepartInfo.JobId;
        //        DepartV.AddBYUserID = DepartInfo.AddBYUserID;


        //        return PartialView("Delete", DepartV);
        //    }

        //    var Depart = new JobViewModel();

        //    return PartialView("Delete", Depart);
        //}


        [HttpPost]
        [Authorize(Roles = "CoOwner,UpdateUser")]
        public async Task<ActionResult> UpdatePermission(ApplicationUser ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            var userId = User.Identity.GetUserId();
            var CoInfo = _unitOfWork.User.GetUserByID(userId);
            if (CoInfo.fCompanyId == 0)
            {
                RedirectToAction("Login", "Account");
            }

            ObjToSave.fCompanyId = CoInfo.fCompanyId;
         
            //  ObjToSave.AddBYUserID = userId;
            if (!ModelState.IsValid)
            {
                string Err = " ";
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (ModelError error in errors)
                {
                    Err = Err + error.ErrorMessage + "  ";
                }

                Msg.Msg = Resources.Resource.SomthingWentWrong + " " + Err;
                Msg.Code = 0;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }



            try
            {

                //    await _unitOfWork.User.RemoveUserPermission(ObjToSave);
                //_unitOfWork.Complete();
                _unitOfWork.User.UpdateUserPers(ObjToSave);
                await _unitOfWork.User.UpdateUserPermission(ObjToSave);
                _unitOfWork.Complete();

                Msg.Msg = Resources.Resource.AddedSuccessfully;
                Msg.Code = 1;

                return Json(Msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                Msg.Msg = Resources.Resource.SomthingWentWrong + " " + ex.Message.ToString();
                Msg.Code = 0;
                return Json(Msg, JsonRequestBehavior.AllowGet);
                //  return RedirectToAction("Index");

            }




            // return Json(Msg, JsonRequestBehavior.AllowGet);


        }


        [HttpPost]
        public ActionResult Savenew(UserViewModel ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            var userId = User.Identity.GetUserId();
            var CoInfo = _unitOfWork.User.GetUserByID(userId);
            if (CoInfo.fCompanyId == 0)
            {
                RedirectToAction("Login", "Account");
            }

            ObjToSave.fCompanyId = CoInfo.fCompanyId;
            //  ObjToSave.AddBYUserID = userId;
            if (!ModelState.IsValid)
            {
                string Err = " ";
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (ModelError error in errors)
                {
                    Err = Err + error.ErrorMessage + "  ";
                }

                Msg.Msg = Resources.Resource.SomthingWentWrong + " " + Err;
                Msg.Code = 0;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }



            try
            {


                Msg.Msg = Resources.Resource.AddedSuccessfully;
                Msg.Code = 1;
                return RedirectToAction("Index");
                // return Json(Msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                Msg.Msg = Resources.Resource.SomthingWentWrong + " " + ex.Message.ToString();
                Msg.Code = 0;
                //return Json(Msg, JsonRequestBehavior.AllowGet);
                return RedirectToAction("Index");

            }




            // return Json(Msg, JsonRequestBehavior.AllowGet);


        }

        [HttpPost]
        [Authorize(Roles = "CoOwner,UpdateUser")]
        public JsonResult ChangeUserPassword(UserViewModel ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            var userId = User.Identity.GetUserId();
            var CoInfo = _unitOfWork.User.GetUserByID(userId);
            if (CoInfo.fCompanyId == 0)
            {
                RedirectToAction("Login", "Account");
            }

            ObjToSave.fCompanyId = CoInfo.fCompanyId;
            //  ObjToSave.AddBYUserID = userId;
            if (!ModelState.IsValid)
            {
                string Err = " ";
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (ModelError error in errors)
                {
                    Err = Err + error.ErrorMessage + "  ";
                }

                Msg.Msg = Resources.Resource.SomthingWentWrong + " " + Err;
                Msg.Code = 0;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }

            try
            {
                ApplicationUser UserToSave = new ApplicationUser();
                var hasher = new PasswordHasher();
                string hashedPwd = hasher.HashPassword(ObjToSave.Password);


                UserToSave.PasswordHash = hashedPwd;
                UserToSave.Email = ObjToSave.Email;
                UserToSave.Id = ObjToSave.Id;

                _unitOfWork.User.ChangePass(UserToSave);
                _unitOfWork.Complete();
                Msg.Msg = Resources.Resource.UpdatedSuccessfully;
                Msg.Code = 1;
                // return RedirectToAction("Index", "Users");
                return Json(Msg, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                Msg.Msg = Resources.Resource.SomthingWentWrong + " " + ex.Message.ToString();
                Msg.Code = 0;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }




        }


     





         


        [Authorize(Roles = "CoOwner,UpdateUser")]
        public ActionResult ReSetUserPass(string id)
        {
            var userId = User.Identity.GetUserId();
            var CoInfo = _unitOfWork.User.GetUserByID(userId);
            if (CoInfo.fCompanyId == 0)
            {
                RedirectToAction("Login", "Account");
            }


            var UInfo = _unitOfWork.User.GetUserByIDAndCo(CoInfo.fCompanyId, id);
            if (UInfo == null)
            {
                return RedirectToAction("Index");
            }

            ResetPasswordViewModel Obj = new ResetPasswordViewModel
            {
                Email = UInfo.Email
            };
            return PartialView("ResetPassword", Obj);

        }


        [Authorize(Roles = "CoOwner,UpdateUser")]
        public ActionResult DeActivate(string id)
        {
            if (id != "")
            {
                var userId = User.Identity.GetUserId();
                var CoInfo = _unitOfWork.User.GetUserByID(userId);
                if (CoInfo == null)
                {
                    RedirectToAction("Login", "Account");
                }

                var JobInfo = _unitOfWork.User.GetUserByIDAndCo(CoInfo.fCompanyId, id);
                if (JobInfo == null)
                    return RedirectToAction("Index");

                var JobtV = new UserViewModel();
                JobtV.fCompanyId = JobInfo.fCompanyId;
                JobtV.Password = "";

                JobtV.Email = JobInfo.Email;
                JobtV.Id = JobInfo.Id;





                return PartialView("DeActivate", JobtV);
            }

            var Depart = new UserViewModel();

            return PartialView("DeActivate", Depart);
        }

        public ActionResult Activate(string id)
        {
            var userId = User.Identity.GetUserId();
            var CoInfo = _unitOfWork.User.GetUserByID(userId);
            if (CoInfo.fCompanyId == 0)
            {
                RedirectToAction("Login", "Account");
            }


            var UInfo = _unitOfWork.User.GetUserByIDAndCo(CoInfo.fCompanyId, id);
            if (UInfo == null)
            {
                return RedirectToAction("Index");
            }

            UserViewModel Obj = new UserViewModel
            {
                Email = UInfo.Email,
                Id = UInfo.Id,
                AccountStatus = UInfo.AccountStatus

            };
            return PartialView("Activate", Obj);

        }
        [HttpPost]
        [Authorize(Roles = "CoOwner,UpdateUser")]
        public ActionResult DeActivate(UserViewModel ObjToSave)
        {
            var userId = User.Identity.GetUserId();
            var CoInfo = _unitOfWork.User.GetUserByID(userId);
            if (CoInfo.fCompanyId == 0)
            {
                RedirectToAction("Login", "Account");
            }

            ObjToSave.fCompanyId = CoInfo.fCompanyId;
            ObjToSave.AccountStatus = 0;
            //  ObjToSave.AddBYUserID = userId;
            //if (!ModelState.IsValid)
            //{
            //    var errors = ModelState.Values.SelectMany(v => v.Errors);
            //    return View("AddModify", ObjToSave);
            //}
            ApplicationUser UserToSave = new ApplicationUser();




            UserToSave.fCompanyId = ObjToSave.fCompanyId;
            UserToSave.Id = ObjToSave.Id;
            UserToSave.AccountStatus = 1;

            _unitOfWork.User.DeActivate(UserToSave);
            _unitOfWork.Complete();
            return RedirectToAction("Index");


        }
        [HttpPost]
        [Authorize(Roles = "CoOwner,UpdateUser")]
        public ActionResult Activate(UserViewModel ObjToSave)
        {
            var userId = User.Identity.GetUserId();
            var CoInfo = _unitOfWork.User.GetUserByID(userId);
            if (CoInfo.fCompanyId == 0)
            {
                RedirectToAction("Login", "Account");
            }

            ObjToSave.fCompanyId = CoInfo.fCompanyId;
            ObjToSave.AccountStatus = 1;
            //  ObjToSave.AddBYUserID = userId;
            //if (!ModelState.IsValid)
            //{
            //    var errors = ModelState.Values.SelectMany(v => v.Errors);
            //    return View("AddModify", ObjToSave);
            //}
            ApplicationUser UserToSave = new ApplicationUser();




            UserToSave.fCompanyId = ObjToSave.fCompanyId;
            UserToSave.Id = ObjToSave.Id;
            UserToSave.AccountStatus = 0;

            _unitOfWork.User.DeActivate(UserToSave);
            _unitOfWork.Complete();
            return RedirectToAction("Index");


        }
        //DeActivate
    }
}