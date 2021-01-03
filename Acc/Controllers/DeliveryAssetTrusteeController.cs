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
    public class DeliveryAssetTrusteeController : BaseController
    {
        
        private readonly IUnitOfWork _unitOfWork;
        public DeliveryAssetTrusteeController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }


        [Authorize(Roles = "CoOwner,ShowDeliveryAssetTrustee")]

        public ActionResult Index()
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var AssetTrusteeObj = _unitOfWork.AssetTrustee.GetAllAssetTrustee(UserInfo.fCompanyId);
            var DefinitionAssetSiteVM = new DefinitionAssetSiteVM
            {
                DeliveryDate = DateTime.Now,
                AssetTrustee = AssetTrusteeObj
            };
            return View(DefinitionAssetSiteVM);
        }

        [Authorize(Roles = "CoOwner,AddDeliveryAssetTrustee")]

        public JsonResult SaveDeliveryAssetTrustee(DefinitionAssetSiteVM ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var CheckDefinitionAssetSite = _unitOfWork.NativeSql.CheckDefinitionAssetSite(UserInfo.fCompanyId,ObjToSave.SerialID);
                if (CheckDefinitionAssetSite.TrusteeID == 0)
                {
                    var UpdateDefinitionAssetSite = new DefinitionAssetSite { };
                    UpdateDefinitionAssetSite.DeliveryInsDateTime = DateTime.Now;
                    UpdateDefinitionAssetSite.DeliveryDate = ObjToSave.DeliveryDate;
                    UpdateDefinitionAssetSite.DeliveryInsUserID = userId;
                    UpdateDefinitionAssetSite.TrusteeID = ObjToSave.TrusteeID;
                    UpdateDefinitionAssetSite.DeliveryNote = ObjToSave.DeliveryNote;
                    UpdateDefinitionAssetSite.CompanyID = UserInfo.fCompanyId;
                    UpdateDefinitionAssetSite.Id = ObjToSave.SerialID;
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
                    _unitOfWork.DefinitionAssetSite.Update(UpdateDefinitionAssetSite);
                    _unitOfWork.Complete();
                    Msg.Code = 1;
                    Msg.Msg = Resources.Resource.AddedSuccessfully;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }
                else {
                    var SaveDefinitionAssetSite = new DefinitionAssetSite { };
                    SaveDefinitionAssetSite.InsDateTime = CheckDefinitionAssetSite.InsDateTime;
                    SaveDefinitionAssetSite.InsUserID = CheckDefinitionAssetSite.InsUserID;
                    SaveDefinitionAssetSite.DeliveryInsDateTime = DateTime.Now;
                    SaveDefinitionAssetSite.DeliveryDate = ObjToSave.DeliveryDate;
                    SaveDefinitionAssetSite.DeliveryInsUserID = userId;
                    SaveDefinitionAssetSite.AssetID = CheckDefinitionAssetSite.AssetID;
                    SaveDefinitionAssetSite.AssetTypeID = CheckDefinitionAssetSite.AssetTypeID;
                    SaveDefinitionAssetSite.AdministrationID = CheckDefinitionAssetSite.AdministrationID;
                    SaveDefinitionAssetSite.CircleID = CheckDefinitionAssetSite.CircleID;
                    SaveDefinitionAssetSite.SectionID = CheckDefinitionAssetSite.SectionID;
                    SaveDefinitionAssetSite.SiteID = CheckDefinitionAssetSite.SiteID;
                    SaveDefinitionAssetSite.Note = CheckDefinitionAssetSite.Note;
                    SaveDefinitionAssetSite.TransferDate = CheckDefinitionAssetSite.TransferDate;
                    SaveDefinitionAssetSite.TrusteeID = ObjToSave.TrusteeID;
                    SaveDefinitionAssetSite.DeliveryNote = ObjToSave.DeliveryNote;
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