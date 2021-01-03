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
    public class DefinitionBankController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public DefinitionBankController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        // GET: DefinitionBank


        [Authorize(Roles = "CoOwner,ShowBank")]

        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var DefinitionBanksFilter = new DefinitionBanksFilterVM
            {
            };
            return View(DefinitionBanksFilter);
        }
        [HttpPost]
        public JsonResult GetAllDefinitionBank(DefinitionBanksFilterVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllDefinitionBank = _unitOfWork.NativeSql.GetAllDefinitionBank(UserInfo.fCompanyId);
                if (AllDefinitionBank == null)
                {
                    return Json(new List<DefinitionBanksFilterVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.AccountNumber))
                {
                    AllDefinitionBank = AllDefinitionBank.Where(m => m.BankAccountNumber == Obj.AccountNumber ||
                                                                m.ChequeUnderCollectionAccountNumber == Obj.AccountNumber ||
                                                                m.PostdatedChequeAccountNumber == Obj.AccountNumber ||
                                                                m.BillsOfExchangeAccountNumber == Obj.AccountNumber).ToList();
                }
                return Json(AllDefinitionBank, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<DefinitionBanksFilterVM>(), JsonRequestBehavior.AllowGet);
            }

        }

        [Authorize(Roles = "CoOwner,AddBank")]
        public ActionResult AddNew()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            DefinitionBankVM Obj = new DefinitionBankVM
            {
                DefinitionBank = new DefinitionBank
                {
                    BankID = _unitOfWork.DefinitionBank.GetMaxSerial(UserInfo.fCompanyId)
                }
            };
            return View(Obj);
        }
        [HttpPost]
        public JsonResult SaveDefinitionBank(DefinitionBank ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                ObjToSave.BankID = _unitOfWork.DefinitionBank.GetMaxSerial(UserInfo.fCompanyId);
                ObjToSave.InsDateTime = DateTime.Now;
                ObjToSave.InsUserID = userId;
                ObjToSave.CompanyID = UserInfo.fCompanyId;

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
                _unitOfWork.DefinitionBank.Add(ObjToSave);
                _unitOfWork.Complete();
                Msg.LastID = _unitOfWork.DefinitionBank.GetMaxSerial(UserInfo.fCompanyId).ToString();
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

        [Authorize(Roles = "CoOwner,UpdateBank")]
        public ActionResult UpdateDefinitionBank(int id)
        {
            try
            {


                if (id != 0)
                {
                    var userId = User.Identity.GetUserId();
                    var UserInfo = _unitOfWork.User.GetUserByID(userId);
                    var CompanyInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                    if (UserInfo == null)
                    {
                        RedirectToAction("", "");
                    }

                    var ObjUpdate = _unitOfWork.DefinitionBank.GetDefinitionBankByID(UserInfo.fCompanyId, id);
                    DefinitionBankVM Obj = new DefinitionBankVM
                    {
                        DefinitionBank = new DefinitionBank
                        {
                           BankID = ObjUpdate.BankID,
                           BankAccountNumber = ObjUpdate.BankAccountNumber,
                           ChequeUnderCollectionAccountNumber = ObjUpdate.ChequeUnderCollectionAccountNumber,
                           PostdatedChequeAccountNumber = ObjUpdate.PostdatedChequeAccountNumber,
                           BillsOfExchangeAccountNumber = ObjUpdate.BillsOfExchangeAccountNumber
                        },
                        BankAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjUpdate.BankAccountNumber),
                        ChequeUnderCollectionAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjUpdate.ChequeUnderCollectionAccountNumber),
                        PostdatedChequeAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjUpdate.PostdatedChequeAccountNumber),
                        BillsOfExchangeAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjUpdate.BillsOfExchangeAccountNumber)
                    };

                    return View("UpdateDefinitionBank", Obj);
                }
                return View("UpdateDefinitionBank", new DefinitionBank());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return View("Error");
            }
        }

        [Authorize(Roles = "CoOwner,DeleteBank")]
        public ActionResult DeleteDefinitionBank(int id)
        {
            try
            {
                if (id != 0)
                {
                    var userId = User.Identity.GetUserId();
                    var UserInfo = _unitOfWork.User.GetUserByID(userId);
                    var CompanyInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                    if (UserInfo == null)
                    {
                        RedirectToAction("", "");
                    }

                    var ObjDelete = _unitOfWork.DefinitionBank.GetDefinitionBankByID(UserInfo.fCompanyId, id);
                    DefinitionBankVM Obj = new DefinitionBankVM
                    {
                        DefinitionBank = new DefinitionBank
                        {
                            BankID = ObjDelete.BankID,
                            BankAccountNumber = ObjDelete.BankAccountNumber,
                            ChequeUnderCollectionAccountNumber = ObjDelete.ChequeUnderCollectionAccountNumber,
                            PostdatedChequeAccountNumber = ObjDelete.PostdatedChequeAccountNumber,
                            BillsOfExchangeAccountNumber = ObjDelete.BillsOfExchangeAccountNumber
                        },
                        BankAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjDelete.BankAccountNumber),
                        ChequeUnderCollectionAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjDelete.ChequeUnderCollectionAccountNumber),
                        PostdatedChequeAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjDelete.PostdatedChequeAccountNumber),
                        BillsOfExchangeAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjDelete.BillsOfExchangeAccountNumber)
                    };

                    return View("DeleteDefinitionBank", Obj);
                }
                return View("DeleteDefinitionBank", new DefinitionBank());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return View("Error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "CoOwner,UpdateBank")]
        public JsonResult UpdateDefinitionBank(DefinitionBank ObjUpdate)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);

                ObjUpdate.CompanyID = UserInfo.fCompanyId;
                ObjUpdate.InsDateTime = DateTime.Now;
                ObjUpdate.InsUserID = userId;
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
                _unitOfWork.DefinitionBank.Update(ObjUpdate);
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


        [HttpPost]
        [Authorize(Roles = "CoOwner,DeleteBank")]
        public JsonResult DeleteDefinitionBank(DefinitionBank ObjDelete)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);

                ObjDelete.CompanyID = UserInfo.fCompanyId;

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
                _unitOfWork.DefinitionBank.UpdateDeleteFlag(ObjDelete);
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
        [HttpGet]
        public JsonResult CheckDefinitionBankBeforDelete(string id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            if (id != "")
            {
                string AccountNumber = _unitOfWork.NativeSql.CheckDefinitionBank(UserInfo.fCompanyId, id);

                return Json(AccountNumber, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult CheckBankAccountNumber(string id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);

            var AccountInfo = _unitOfWork.NativeSql.CheckBankAccountNumber(UserInfo.fCompanyId, id).FirstOrDefault();
            if (AccountInfo == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(AccountInfo, JsonRequestBehavior.AllowGet);
            }

        }
    }
}