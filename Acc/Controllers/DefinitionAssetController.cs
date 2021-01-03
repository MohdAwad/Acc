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
    public class DefinitionAssetController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public DefinitionAssetController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        [Authorize(Roles = "CoOwner,ShowDefinitionAsset")]
        public ActionResult Index()
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var AssetAdministrationObj = _unitOfWork.AssetAdministration.GetAllAssetAdministration(UserInfo.fCompanyId);
            var AssetCircleObj = _unitOfWork.AssetCircle.GetAllAssetCircle(UserInfo.fCompanyId);
            var AssetSectionObj = _unitOfWork.AssetSection.GetAllAssetSection(UserInfo.fCompanyId);
            var AssetSiteObj = _unitOfWork.AssetSite.GetAllAssetSite(UserInfo.fCompanyId);
            var DefinitionAssetSiteVM = new DefinitionAssetSiteVM
            {
                TransferDate = DateTime.Now,
                AssetAdministration = AssetAdministrationObj,
                AssetCircle = AssetCircleObj,
                AssetSection = AssetSectionObj,
                AssetSite = AssetSiteObj
            };
            return View(DefinitionAssetSiteVM);
        }

        [Authorize(Roles = "CoOwner,AddDefinitionAsset")]

        public JsonResult SaveDefinitionAssetSite(DefinitionAssetSiteVM ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);

                var SaveDefinitionAssetSite = new DefinitionAssetSite { };
                SaveDefinitionAssetSite.InsDateTime = DateTime.Now;
                SaveDefinitionAssetSite.DeliveryInsDateTime = new DateTime(1900,01,01);
                SaveDefinitionAssetSite.DeliveryDate = new DateTime(1900, 01, 01);
                SaveDefinitionAssetSite.InsUserID = userId;
                SaveDefinitionAssetSite.AssetID = ObjToSave.AssetID;
                SaveDefinitionAssetSite.AssetTypeID = ObjToSave.AssetTypeID;
                SaveDefinitionAssetSite.AdministrationID = ObjToSave.AdministrationID;
                SaveDefinitionAssetSite.CircleID = ObjToSave.CircleID;
                SaveDefinitionAssetSite.SectionID = ObjToSave.SectionID;
                SaveDefinitionAssetSite.SiteID = ObjToSave.SiteID;
                SaveDefinitionAssetSite.Note = ObjToSave.Note;
                SaveDefinitionAssetSite.TransferDate = ObjToSave.TransferDate;
                SaveDefinitionAssetSite.CompanyID = UserInfo.fCompanyId;
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
                _unitOfWork.DefinitionAssetSite.Add(SaveDefinitionAssetSite);
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

        [Authorize(Roles = "CoOwner,RepDefinitionAssetSiteReport")]

        public ActionResult DefinitionAssetSiteReport()
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var AssetAdministrationObj = _unitOfWork.AssetAdministration.GetAllAssetAdministration(UserInfo.fCompanyId);
            var AssetCircleObj = _unitOfWork.AssetCircle.GetAllAssetCircle(UserInfo.fCompanyId);
            var AssetSectionObj = _unitOfWork.AssetSection.GetAllAssetSection(UserInfo.fCompanyId);
            var AssetSiteObj = _unitOfWork.AssetSite.GetAllAssetSite(UserInfo.fCompanyId);
            var AssetTrusteeObj = _unitOfWork.AssetTrustee.GetAllAssetTrustee(UserInfo.fCompanyId);
            var AssetTypeObj = _unitOfWork.AssetType.GetAllAssetType(UserInfo.fCompanyId);
            int CurrYear = UserInfo.CurrYear;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var DefinitionAssetSiteVM = new DefinitionAssetSiteVM
            {
                TransferDate = DateTime.Now,
                AssetAdministration = AssetAdministrationObj,
                AssetCircle = AssetCircleObj,
                AssetSection = AssetSectionObj,
                AssetSite = AssetSiteObj,
                AssetTrustee = AssetTrusteeObj,
                AssetType = AssetTypeObj,
                FromDeliveryDate = DateTime.Now,
                FromTransferDate = DateTime.Now,
                ToDeliveryDate = DateTime.Now,
                ToTransferDate = DateTime.Now,

                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency


            };
            return View(DefinitionAssetSiteVM);
        }
        [HttpPost]
        public JsonResult GetAssetSiteReport(DefinitionAssetSiteVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllDefinitionAssetSiteVM = _unitOfWork.NativeSql.GetAssetSiteReport(UserInfo.fCompanyId,Obj.ApproveDeliveryDate,
                Obj.ApproveTransferDate,Obj.FromTransferDate,Obj.ToTransferDate,Obj.FromDeliveryDate,Obj.ToDeliveryDate);
                if (AllDefinitionAssetSiteVM == null)
                {
                    return Json(new List<DefinitionAssetSiteVM>(), JsonRequestBehavior.AllowGet);
                }
                if (Obj.AssetTypeID != 0)
                {
                    AllDefinitionAssetSiteVM = AllDefinitionAssetSiteVM.Where(m => m.AssetTypeID == Obj.AssetTypeID).ToList();
                }
                if (Obj.AdministrationID != 0)
                {
                    AllDefinitionAssetSiteVM = AllDefinitionAssetSiteVM.Where(m => m.AdministrationID == Obj.AdministrationID).ToList();
                }
                if (Obj.CircleID != 0)
                {
                    AllDefinitionAssetSiteVM = AllDefinitionAssetSiteVM.Where(m => m.CircleID == Obj.CircleID).ToList();
                }
                if (Obj.SectionID != 0)
                {
                    AllDefinitionAssetSiteVM = AllDefinitionAssetSiteVM.Where(m => m.SectionID == Obj.SectionID).ToList();
                }
                if (Obj.SiteID != 0)
                {
                    AllDefinitionAssetSiteVM = AllDefinitionAssetSiteVM.Where(m => m.SiteID == Obj.SiteID).ToList();
                }
                if (Obj.TrusteeID != 0)
                {
                    AllDefinitionAssetSiteVM = AllDefinitionAssetSiteVM.Where(m => m.TrusteeID == Obj.TrusteeID).ToList();
                }
                if (Obj.AssetID != 0)
                {
                    AllDefinitionAssetSiteVM = AllDefinitionAssetSiteVM.Where(m => m.AssetID == Obj.AssetID).ToList();
                }
                return Json(AllDefinitionAssetSiteVM, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<DefinitionAssetSiteVM>(), JsonRequestBehavior.AllowGet);
            }

        }


        [Authorize(Roles = "CoOwner,RepAssetsTransferMovements")]

        public ActionResult AssetsTransferMovements()
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var AssetAdministrationObj = _unitOfWork.AssetAdministration.GetAllAssetAdministration(UserInfo.fCompanyId);
            var AssetCircleObj = _unitOfWork.AssetCircle.GetAllAssetCircle(UserInfo.fCompanyId);
            var AssetSectionObj = _unitOfWork.AssetSection.GetAllAssetSection(UserInfo.fCompanyId);
            var AssetSiteObj = _unitOfWork.AssetSite.GetAllAssetSite(UserInfo.fCompanyId);
            var AssetTrusteeObj = _unitOfWork.AssetTrustee.GetAllAssetTrustee(UserInfo.fCompanyId);
            var AssetTypeObj = _unitOfWork.AssetType.GetAllAssetType(UserInfo.fCompanyId);
            int CurrYear = UserInfo.CurrYear;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var DefinitionAssetSiteVM = new DefinitionAssetSiteVM
            {
                TransferDate = DateTime.Now,
                AssetAdministration = AssetAdministrationObj,
                AssetCircle = AssetCircleObj,
                AssetSection = AssetSectionObj,
                AssetSite = AssetSiteObj,
                AssetTrustee = AssetTrusteeObj,
                AssetType = AssetTypeObj,
                FromDeliveryDate = DateTime.Now,
                FromTransferDate = DateTime.Now,
                ToDeliveryDate = DateTime.Now,
                ToTransferDate = DateTime.Now,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency



            };
            return View(DefinitionAssetSiteVM);
        }
        [HttpPost]
        public JsonResult GetAssetsTransferMovementsReport(DefinitionAssetSiteVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllDefinitionAssetSiteVM = _unitOfWork.NativeSql.GetAssetsTransferMovementsReport(UserInfo.fCompanyId, Obj.ApproveDeliveryDate,
                Obj.ApproveTransferDate, Obj.FromTransferDate, Obj.ToTransferDate, Obj.FromDeliveryDate, Obj.ToDeliveryDate);
                if (AllDefinitionAssetSiteVM == null)
                {
                    return Json(new List<DefinitionAssetSiteVM>(), JsonRequestBehavior.AllowGet);
                }
                if (Obj.AssetTypeID != 0)
                {
                    AllDefinitionAssetSiteVM = AllDefinitionAssetSiteVM.Where(m => m.AssetTypeID == Obj.AssetTypeID).ToList();
                }
                if (Obj.AdministrationID != 0)
                {
                    AllDefinitionAssetSiteVM = AllDefinitionAssetSiteVM.Where(m => m.AdministrationID == Obj.AdministrationID).ToList();
                }
                if (Obj.CircleID != 0)
                {
                    AllDefinitionAssetSiteVM = AllDefinitionAssetSiteVM.Where(m => m.CircleID == Obj.CircleID).ToList();
                }
                if (Obj.SectionID != 0)
                {
                    AllDefinitionAssetSiteVM = AllDefinitionAssetSiteVM.Where(m => m.SectionID == Obj.SectionID).ToList();
                }
                if (Obj.SiteID != 0)
                {
                    AllDefinitionAssetSiteVM = AllDefinitionAssetSiteVM.Where(m => m.SiteID == Obj.SiteID).ToList();
                }
                if (Obj.TrusteeID != 0)
                {
                    AllDefinitionAssetSiteVM = AllDefinitionAssetSiteVM.Where(m => m.TrusteeID == Obj.TrusteeID).ToList();
                }
                if (Obj.AssetID != 0)
                {
                    AllDefinitionAssetSiteVM = AllDefinitionAssetSiteVM.Where(m => m.AssetID == Obj.AssetID).ToList();
                }
                return Json(AllDefinitionAssetSiteVM, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<DefinitionAssetSiteVM>(), JsonRequestBehavior.AllowGet);
            }

        }
    }
}
