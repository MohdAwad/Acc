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
    public class AssetController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public AssetController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        [Authorize(Roles = "CoOwner,ShowCalculationOfConsumption")]
        public ActionResult CalculationOfConsumption()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            AssetVM Obj = new AssetVM
            {
                AssetType = _unitOfWork.AssetType.GetAllAssetType(UserInfo.fCompanyId),
                ToDate = DateTime.Now,
                AssetID = _unitOfWork.Asset.GetMaxSerial(UserInfo.fCompanyId, 0),
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency


            };
            return View(Obj);
        }
        [Authorize(Roles = "CoOwner,ShowAsset")]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            AssetVM Obj = new AssetVM
            {
                AssetType = _unitOfWork.AssetType.GetAllAssetType(UserInfo.fCompanyId) ,
                FAssetTypeID=0
                
            };
            return View(Obj);
        }
        [Authorize(Roles = "CoOwner,RepMaintenanceAssetReport")]  
        public ActionResult MaintenanceAssetReport()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            int CurrYear = UserInfo.CurrYear;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            AssetFilterVM Obj = new AssetFilterVM
            {
                AssetType = _unitOfWork.AssetType.GetAllAssetType(UserInfo.fCompanyId),
                AssetTypeID = 0,
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency
            };
            return View(Obj);
        }
        [Authorize(Roles = "CoOwner,RepSoldAsset")]
        public ActionResult SoldAsset()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            int CurrYear = UserInfo.CurrYear;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            AssetFilterVM Obj = new AssetFilterVM
            {
                AssetType = _unitOfWork.AssetType.GetAllAssetType(UserInfo.fCompanyId),
                AssetTypeID = 0,
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,

                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency



            };
            return View(Obj);
        }
        [Authorize(Roles = "CoOwner,RepConsumAssetReport")]
        public ActionResult ConsumAssetReport()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            int CurrYear = UserInfo.CurrYear;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            AssetFilterVM Obj = new AssetFilterVM
            {
                AssetType = _unitOfWork.AssetType.GetAllAssetType(UserInfo.fCompanyId),
                AssetTypeID = 0,
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency



            };
            return View(Obj);
        }
        [Authorize(Roles = "CoOwner,ShowSaleAsset")]
        public ActionResult SaleAsset()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            AssetVM Obj = new AssetVM
            {
                AssetType = _unitOfWork.AssetType.GetAllAssetType(UserInfo.fCompanyId),
                CombinedtDate = DateTime.Now,
                ConsStartDate = DateTime.Now,
                GuaranteedDate = DateTime.Now,
                LastMaintenanceDate = DateTime.Now,
                PurchaseInvDate = DateTime.Now,
                LastConsumptionDate = DateTime.Now,
                SaleDate = DateTime.Now,
                ConsumingDate= DateTime.Now,
                WorkWithCostCenter = Company.WorkWithCostCenter,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency
            };
            return View(Obj);

        }
        [Authorize(Roles = "CoOwner,ShowAssetMaintenance")]
        public ActionResult AssetMaintenance()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            int CurrYear = UserInfo.CurrYear;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            AssetFilterVM Obj = new AssetFilterVM
            {
                AssetType = _unitOfWork.AssetType.GetAllAssetType(UserInfo.fCompanyId),
                AssetTypeID = 0,
                FromDate = DateTime.Parse("01/01/" + CurrYear),
                ToDate = DateTime.Now,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency

            };
            return View(Obj);
        }

        [Authorize(Roles = "CoOwner,AddAssetMaintenance")]
        public ActionResult SaveAssetMaintenance()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            AssetMaintenanceVM Obj = new AssetMaintenanceVM
            {
                AssetType = _unitOfWork.AssetType.GetAllAssetType(UserInfo.fCompanyId),
                GrundToDate = DateTime.Now,
                TrDate = DateTime.Now,
                VoucherDate = DateTime.Now,
                WorkWithCostCenter = Company.WorkWithCostCenter,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency,
                CurrentYear = UserInfo.CurrYear
            };
            return View(Obj);
        }
        [Authorize(Roles = "CoOwner,AddConsumAsset")]  
        public ActionResult ConsumAsset()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            AssetVM Obj = new AssetVM
            {
                AssetType = _unitOfWork.AssetType.GetAllAssetType(UserInfo.fCompanyId),
                CombinedtDate = DateTime.Now,
                ConsStartDate = DateTime.Now,
                GuaranteedDate = DateTime.Now,
                LastMaintenanceDate = DateTime.Now,
                PurchaseInvDate = DateTime.Now,
                LastConsumptionDate = DateTime.Now,
                SaleDate = DateTime.Now,
                ConsumingDate = DateTime.Now,
                WorkWithCostCenter = Company.WorkWithCostCenter,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency
            };
            return View(Obj);
        }


        [Authorize(Roles = "CoOwner,AddAsset")]

        public ActionResult GetSaleAsset(int id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var ObjAss = _unitOfWork.NativeSql.GetAssetByID(UserInfo.fCompanyId, id);
            AssetVM Obj = new AssetVM
            {
                AssetType = _unitOfWork.AssetType.GetAllAssetType(UserInfo.fCompanyId),
                AssetBarCode = ObjAss.AssetBarCode,
                AssetCost = ObjAss.AssetCost,
                AssetID = ObjAss.AssetID,
                AssetConsumRatio = ObjAss.AssetConsumRatio,
                AssetName = ObjAss.AssetName,
                AssetSerialNo = ObjAss.AssetSerialNo,
                BookValue = ObjAss.BookValue,
                CombinedConsum = ObjAss.CombinedConsum,
                CombinedtDate = ObjAss.CombinedtDate,
                CompanyID = ObjAss.CompanyID,
                ConsStartDate = ObjAss.ConsStartDate,
                CreditAccountName = ObjAss.CreditAccountName,
                CreditAccountNo = ObjAss.CreditAccountNo,
                CreditCostName = ObjAss.CreditCostName,
                CreditCostNo = ObjAss.CreditCostNo,
                DebitAccountName = ObjAss.DebitAccountName,
                DebitAccountNo = ObjAss.DebitAccountNo,
                DebitCostName = ObjAss.DebitCostName,
                DebitCostNo = ObjAss.DebitCostNo,
                FAssetTypeID = ObjAss.FAssetTypeID,
                GuaranteedDate = ObjAss.GuaranteedDate,
                IntAssID = ObjAss.IntAssID,
                LastMaintenanceDate = ObjAss.LastMaintenanceDate,
                Note = ObjAss.Note,
                PurchaseInvDate = ObjAss.PurchaseInvDate,
                PurchaseinvoiceNo = ObjAss.PurchaseinvoiceNo,
                PurchaseOrderNo = ObjAss.PurchaseOrderNo,
                SupplierAccountName = ObjAss.SupplierAccountName,
                SupplierAccountNo = ObjAss.SupplierAccountNo,
                SupplierNote = ObjAss.SupplierNote,
                LastConsumptionDate=ObjAss.LastConsumptionDate,
                SaleDate=DateTime.Now,
                SaleValue=0,
            };
            return PartialView(Obj);
        }

        [Authorize(Roles = "CoOwner,ShowAssetToAcc")]

        public ActionResult AssetToAcc()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetCompanyTransactionKind(UserInfo.fCompanyId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            AssetToAccVM Obj =new AssetToAccVM
            {
              TransDate= DateTime.Now,
                CompanyTransactionKind = CompanyTransactionKindObj,
                CompanyTransactionKindID = 1,
                WorkWithCostCenter = Company.WorkWithCostCenter,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency
            };
            return View(Obj);
        }
        [Authorize(Roles = "CoOwner,AddAsset")]
        public ActionResult New()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            AssetVM Obj = new AssetVM
            {
                AssetType=_unitOfWork.AssetType.GetAllAssetType(UserInfo.fCompanyId),
                CombinedtDate=DateTime.Now,
                ConsStartDate= DateTime.Now,
                GuaranteedDate= DateTime.Now,
                LastMaintenanceDate= DateTime.Now,
                PurchaseInvDate= DateTime.Now,
                AssetID =   _unitOfWork.Asset.GetMaxSerial(UserInfo.fCompanyId, 0),
                WorkWithCostCenter = Company.WorkWithCostCenter


            };
            return View(Obj);
        }
        [Authorize(Roles = "CoOwner,DeleteAsset")]
        public ActionResult Delete(int id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var ObjAss = _unitOfWork.NativeSql.GetAssetByID(UserInfo.fCompanyId, id);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            AssetVM Obj = new AssetVM
            {
                AssetType = _unitOfWork.AssetType.GetAllAssetType(UserInfo.fCompanyId),
                AssetBarCode = ObjAss.AssetBarCode,
                AssetCost = ObjAss.AssetCost,
                AssetID = ObjAss.AssetID,
                AssetConsumRatio = ObjAss.AssetConsumRatio,
                AssetName = ObjAss.AssetName,
                AssetSerialNo = ObjAss.AssetSerialNo,
                BookValue = ObjAss.BookValue,
                CombinedConsum = ObjAss.CombinedConsum,
                CombinedtDate = ObjAss.CombinedtDate,
                CompanyID = ObjAss.CompanyID,
                ConsStartDate = ObjAss.ConsStartDate,
                CreditAccountName = ObjAss.CreditAccountName,
                CreditAccountNo = ObjAss.CreditAccountNo,
                CreditCostName = ObjAss.CreditCostName,
                CreditCostNo = ObjAss.CreditCostNo,
                DebitAccountName = ObjAss.DebitAccountName,
                DebitAccountNo = ObjAss.DebitAccountNo,
                DebitCostName = ObjAss.DebitCostName,
                DebitCostNo = ObjAss.DebitCostNo,
                FAssetTypeID = ObjAss.FAssetTypeID,
                GuaranteedDate = ObjAss.GuaranteedDate,
                IntAssID = ObjAss.IntAssID,
                LastMaintenanceDate = ObjAss.LastMaintenanceDate,
                Note = ObjAss.Note,
                PurchaseInvDate = ObjAss.PurchaseInvDate,
                PurchaseinvoiceNo = ObjAss.PurchaseinvoiceNo,
                PurchaseOrderNo = ObjAss.PurchaseOrderNo,
                SupplierAccountName = ObjAss.SupplierAccountName,
                SupplierAccountNo = ObjAss.SupplierAccountNo,
                SupplierNote = ObjAss.SupplierNote,
                WorkWithCostCenter = Company.WorkWithCostCenter
            };
            return PartialView(Obj);
        }
        [Authorize(Roles = "CoOwner,UpdateAsset")]
        public ActionResult Update(int id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var ObjAss = _unitOfWork.NativeSql.GetAssetByID(UserInfo.fCompanyId, id);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            AssetVM Obj = new AssetVM
                {
                    AssetType = _unitOfWork.AssetType.GetAllAssetType(UserInfo.fCompanyId),
                    AssetBarCode = ObjAss.AssetBarCode,
                    AssetCost = ObjAss.AssetCost,
                    AssetID = ObjAss.AssetID,
                    AssetConsumRatio = ObjAss.AssetConsumRatio,
                    AssetName = ObjAss.AssetName,
                    AssetSerialNo = ObjAss.AssetSerialNo,
                    BookValue = ObjAss.BookValue,
                    CombinedConsum = ObjAss.CombinedConsum,
                    CombinedtDate = ObjAss.CombinedtDate,
                    CompanyID = ObjAss.CompanyID,
                    ConsStartDate = ObjAss.ConsStartDate,
                    CreditAccountName = ObjAss.CreditAccountName,
                    CreditAccountNo = ObjAss.CreditAccountNo,
                    CreditCostName = ObjAss.CreditCostName,
                    CreditCostNo = ObjAss.CreditCostNo,
                    DebitAccountName = ObjAss.DebitAccountName,
                    DebitAccountNo = ObjAss.DebitAccountNo,
                    DebitCostName = ObjAss.DebitCostName,
                    DebitCostNo = ObjAss.DebitCostNo,
                    FAssetTypeID = ObjAss.FAssetTypeID,
                    GuaranteedDate = ObjAss.GuaranteedDate,
                    IntAssID = ObjAss.IntAssID,
                    LastMaintenanceDate = ObjAss.LastMaintenanceDate,
                    Note = ObjAss.Note,
                    PurchaseInvDate = ObjAss.PurchaseInvDate,
                    PurchaseinvoiceNo = ObjAss.PurchaseinvoiceNo,
                    PurchaseOrderNo = ObjAss.PurchaseOrderNo,
                    SupplierAccountName = ObjAss.SupplierAccountName,
                    SupplierAccountNo = ObjAss.SupplierAccountNo,
                    SupplierNote = ObjAss.SupplierNote,
                    WorkWithCostCenter = Company.WorkWithCostCenter
                };
                return PartialView(Obj);
        }
        [Authorize(Roles = "CoOwner,UpdateAssetMaintenance")]
        public ActionResult UpdateMaintenance(int id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            AssetMaintenance ObjAss = _unitOfWork.AssetMaintenance.GetAssetMaintenanceByID(UserInfo.fCompanyId, id);
            var AssetObj = _unitOfWork.Asset.GetAssetByID(UserInfo.fCompanyId, ObjAss.AssetID);
            AssetMaintenanceVM Obj = new AssetMaintenanceVM
            {
                AssetType = _unitOfWork.AssetType.GetAllAssetType(UserInfo.fCompanyId),
                AssetID = ObjAss.AssetID,
                AssetName = AssetObj.AssetName,
                CreditAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjAss.CreditAccountNo),
                CreditAccountNo = ObjAss.CreditAccountNo,
                FAssetTypeID = ObjAss.FAssetTypeID,
                GrundToDate = ObjAss.GrundToDate,
                MaintenanceNo = ObjAss.MaintenanceNo,
                MaintenanceNote = ObjAss.MaintenanceNote,
                MaintenanceVoucherNo = ObjAss.MaintenanceVoucherNo,
                PayValue = ObjAss.PayValue,
                Serial=ObjAss.Serial,
                TrDate = ObjAss.TrDate,
                TrKind = ObjAss.TrKind,
                VoucherDate = ObjAss.VoucherDate,
                WorkWithCostCenter = Company.WorkWithCostCenter,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency
            };
            return PartialView(Obj);
        }
       
        [Authorize(Roles = "CoOwner,DeleteAssetMaintenance")]
        public ActionResult DeleteMaintenance(int id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            AssetMaintenance ObjAss = _unitOfWork.AssetMaintenance.GetAssetMaintenanceByID(UserInfo.fCompanyId, id);
            var AssetObj = _unitOfWork.Asset.GetAssetByID(UserInfo.fCompanyId, ObjAss.AssetID);
            AssetMaintenanceVM Obj = new AssetMaintenanceVM
            {
                AssetType = _unitOfWork.AssetType.GetAllAssetType(UserInfo.fCompanyId),
                AssetID = ObjAss.AssetID,
                AssetName = AssetObj.AssetName,
                CreditAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjAss.CreditAccountNo),
                CreditAccountNo = ObjAss.CreditAccountNo,
                FAssetTypeID = ObjAss.FAssetTypeID,
                GrundToDate = ObjAss.GrundToDate,
                MaintenanceNo = ObjAss.MaintenanceNo,
                MaintenanceNote = ObjAss.MaintenanceNote,
                MaintenanceVoucherNo = ObjAss.MaintenanceVoucherNo,
                PayValue = ObjAss.PayValue,
                Serial = ObjAss.Serial,
                TrDate = ObjAss.TrDate,
                TrKind = ObjAss.TrKind,
                VoucherDate = ObjAss.VoucherDate,
                WorkWithCostCenter = Company.WorkWithCostCenter,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency
            };
            return PartialView(Obj);
        }
        [HttpPost]
        [Authorize(Roles = "CoOwner,AddAsset")]
        public JsonResult SaveAsset(Asset ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                ObjToSave.CompanyID = UserInfo.fCompanyId;
                ObjToSave.InsDateTime = DateTime.Now;
                ObjToSave.InsUserID = userId;
                ObjToSave.LastConsumptionDate = ObjToSave.CombinedtDate;
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
                if (_unitOfWork.Asset.CehckNumber(UserInfo.fCompanyId, ObjToSave.AssetID))
                {
                    Msg.Msg = Resources.Resource.ThisNumberAlreadyExist;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);
                }
                int CurrYear = UserInfo.CurrYear;
                DateTime dec31 = new DateTime(CurrYear, 12, 31);
                int DaysInYear = dec31.DayOfYear;

                double Years = (100 / ObjToSave.AssetConsumRatio);
                ObjToSave.AnnualConsumption = Math.Round((ObjToSave.AssetCost / Years),3);
                ObjToSave.ConsumptionPerDay =Math.Round( ObjToSave.AnnualConsumption / DaysInYear,3);
                ObjToSave.BookValue = ObjToSave.AssetCost - ObjToSave.CombinedConsum;
                ObjToSave.SaleDate = ObjToSave.LastConsumptionDate;
                ObjToSave.ConsumingDate = ObjToSave.LastConsumptionDate;


                _unitOfWork.Asset.Add(ObjToSave);
                _unitOfWork.Complete();
                Msg.LastID = _unitOfWork.Asset.GetMaxSerial(UserInfo.fCompanyId,0).ToString();
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
        [Authorize(Roles = "CoOwner,AddCalculationOfConsumption")]

        public JsonResult SaveCalculationOfConsumption( AssetConsumptionVM ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);



                //var CreditData = ObjToSave.AssetData.GroupBy(m=>new { m.CreditAccountNo,m.CreditCostNo}).Select(group=>new { Credit=group.Key,total=group.Sum(m=>m.ValueofConsumption)});
                //var DebitData = ObjToSave.AssetData.GroupBy(m => new { m.DebitAccountNo, m.DebitCostNo }).Select(group => new { Debit = group.Key, total = group.Sum(m => m.ValueofConsumption) });


                //            var query = feeAllRecords
                //.GroupBy(f => new { f.AccountNo, f.FeeTypeID })
                //.Select(group => new { fee = group.Key, total = group.Sum(f => f.FeeAmount) });



                //if (!ModelState.IsValid)
                //{
                //    string Err = " ";
                //    var errors = ModelState.Values.SelectMany(v => v.Errors);
                //    foreach (ModelError error in errors)
                //    {
                //        Err = Err + error.ErrorMessage + " * ";
                //    }

                //    Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + Err;
                //    Msg.Code = 0;
                //    return Json(Msg, JsonRequestBehavior.AllowGet);

                //}

                var sum = ObjToSave.AssetData.Sum(m => m.ValueofConsumption);

                if (sum == 0)
                {
                    Msg.Msg = Resources.Resource.CantBeZero;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);

                }
                AssetHeader ObjH = new AssetHeader
                {
                    CompanyID = UserInfo.fCompanyId,
                    SaveDate = DateTime.Now,
                    VouchrNo = _unitOfWork.AssetHeader.GetMaxSerial(UserInfo.fCompanyId),
                    Total=sum,
                    Exported=0,
                    VouchrDate = ObjToSave.ToDate
                };
                _unitOfWork.AssetHeader.Add(ObjH);


                foreach (var A in ObjToSave.AssetData)
                {
                    if (A.ValueofConsumption > 0)
                    {
                   
                        Asset Obj = new Asset();
                        Obj.CompanyID = UserInfo.fCompanyId;
                        Obj.AssetID = A.AssetID;
                        Obj.LastConsumptionDate = ObjToSave.ToDate;
                        Obj.PreviousConsumption = A.PreviousConsumption+A.ValueofConsumption;
                        Obj.BookValue = A.BookValue;
                        Obj.ValueofConsumption = A.ValueofConsumption;
                        Obj.CombinedConsum =   A.ValueofConsumption;
                        Obj.CombinedtDate = ObjToSave.ToDate;
                        _unitOfWork.Asset.UpdateConsumption(Obj);

                        AssetTransAction ObjTrans = new AssetTransAction
                        {
                            CompanyID = UserInfo.fCompanyId,
                            VouchrNo = ObjH.VouchrNo,
                            AnnualConsumption = A.AnnualConsumption,
                            AssetBarCode = A.AssetBarCode,
                            AssetConsumRatio = A.AssetConsumRatio,
                            AssetCost = A.AssetCost,
                            AssetID = A.AssetID,
                            BookValue = A.BookValue,
                            CombinedConsum = A.CombinedConsum,
                            
                            ComplexConsumption = A.ComplexConsumption+A.ValueofConsumption,
                           
                            ConsumptionPerDay = A.ConsumptionPerDay,
                            Exported = false,
                            LastConsumptionDate = ObjToSave.ToDate,
                            Note = A.Note,
                            PreviousConsumption = A.PreviousConsumption,
                            
                            ValueofConsumption = A.ValueofConsumption
                        };

                        _unitOfWork.AssetTransAction.Add(ObjTrans);
                    }

                  
                }
                _unitOfWork.Complete();
          

              
                Msg.LastID = _unitOfWork.Asset.GetMaxSerial(UserInfo.fCompanyId, 0).ToString();
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
        [Authorize(Roles = "CoOwner,ShowSupplieraccount")]

        public JsonResult GetAssetMaintenance(AssetFilterVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllAsset = _unitOfWork.AssetMaintenance.GetAllAssetMaintenanceNative(UserInfo.fCompanyId);
                int AssetID = 0;
                if (!String.IsNullOrEmpty(Obj.AssetID))
                {
                    try
                    {
                        AssetID = int.Parse(Obj.AssetID);
                    }
                    catch
                    {
                        AssetID = 0;
                    }
                }

                if (AllAsset == null)
                {
                    return Json(new List<AssetMaintenanceVM>(), JsonRequestBehavior.AllowGet);
                }
                AllAsset = AllAsset.Where(m => m.TrDate >= Obj.FromDate && m.TrDate <= Obj.ToDate).ToList();
                if (Obj.AssetTypeID != 0)
                {
                    AllAsset = AllAsset.Where(m => m.FAssetTypeID == Obj.AssetTypeID).ToList();
                }

                if (AssetID != 0)
                {
                    AllAsset= AllAsset.Where(m => m.AssetID == AssetID).ToList();
                }


                return Json(AllAsset, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<AssetMaintenanceVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public JsonResult GetSoldAsset(AssetFilterVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllAsset = _unitOfWork.Asset.GetAllAssetNative(UserInfo.fCompanyId, 1);
                
                if (AllAsset == null)
                {
                    return Json(new List<AssetVM>(), JsonRequestBehavior.AllowGet);
                }
                AllAsset = AllAsset.Where(m => m.SaleDate >= Obj.FromDate && m.SaleDate <= Obj.ToDate).ToList();
                if (Obj.AssetTypeID != 0)
                {
                    AllAsset = AllAsset.Where(m => m.FAssetTypeID == Obj.AssetTypeID).ToList();
                }
                        



                return Json(AllAsset, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<AssetVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public JsonResult GetConsumAsset(AssetFilterVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllAsset = _unitOfWork.Asset.GetAllAssetNative(UserInfo.fCompanyId, 2);

                if (AllAsset == null)
                {
                    return Json(new List<AssetVM>(), JsonRequestBehavior.AllowGet);
                }
                AllAsset = AllAsset.Where(m => m.ConsumingDate >= Obj.FromDate && m.ConsumingDate <= Obj.ToDate).ToList();
                if (Obj.AssetTypeID != 0)
                {
                    AllAsset = AllAsset.Where(m => m.FAssetTypeID == Obj.AssetTypeID).ToList();
                }

                return Json(AllAsset, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<AssetVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public JsonResult GetAllAsset(AssetVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllAsset = _unitOfWork.Asset.GetAllAssetNative(UserInfo.fCompanyId,0);
                if (AllAsset  == null)
                {
                    return Json(new List<AssetVM>(), JsonRequestBehavior.AllowGet);
                }
                if (Obj.FAssetTypeID != 0)
                {
                    AllAsset = AllAsset.Where(m => m.FAssetTypeID == Obj.FAssetTypeID).ToList();
                }

                return Json(AllAsset, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<AssetVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public JsonResult GetAllAssetHeader()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllAsset = _unitOfWork.AssetHeader.GetAssetHeaders(UserInfo.fCompanyId);
                if (AllAsset == null)
                {
                    return Json(new List<AssetHeader>(), JsonRequestBehavior.AllowGet);
                }
                

                return Json(AllAsset, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<AssetHeader>(), JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public JsonResult GetAllAssetTransAction(int id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllAsset = _unitOfWork. NativeSql.GetAllAssetTRanAction(UserInfo.fCompanyId,id);
                if (AllAsset == null)
                {
                    return Json(new List<AssetTransAction>(), JsonRequestBehavior.AllowGet);
                }


                return Json(AllAsset, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<AssetTransAction>(), JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public JsonResult GetAssetTransActionToPost(int id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllCredit = _unitOfWork.AssetTransAction.GetAssetCreditToPost(UserInfo.fCompanyId, id);
                var AllDebit = _unitOfWork.AssetTransAction.GetAssetDebitToPost(UserInfo.fCompanyId, id);
                
                var CostCenter = _unitOfWork.CostCenter.GetAllCostCenter(UserInfo.fCompanyId);
             
 
            
                var TransToPost = new List<AssetTransToPostVM>();
                foreach ( var Trans in AllDebit)
                {
                    Trans.Debit = Trans.Value;
                    if (!String.IsNullOrEmpty(Trans.CostCenterNo))
                        Trans.CostCenterName = CostCenter.FirstOrDefault(m => m.CostNumber == Trans.CostCenterNo).ArabicName;
                    TransToPost.Add(Trans);
 
                }
                foreach (var Trans in AllCredit)
                {
                    Trans.Credit = Trans.Value;
                    if (!String.IsNullOrEmpty(Trans.CostCenterNo))
                        Trans.CostCenterName = CostCenter.FirstOrDefault(m => m.CostNumber == Trans.CostCenterNo).ArabicName;
                    TransToPost.Add(Trans);

                }



                return Json(TransToPost, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<AssetTransToPostVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public JsonResult SaveAssetNewTransAction(AssetTransExportVM Obj )
        {
            int VouchrNo = Obj.VouchrNo;
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                int CurrYear = UserInfo.CurrYear;
                var AllCredit = _unitOfWork.AssetTransAction.GetAssetCreditToPost(UserInfo.fCompanyId, VouchrNo);
                var AllDebit = _unitOfWork.AssetTransAction.GetAssetDebitToPost(UserInfo.fCompanyId, VouchrNo);
                var CoData = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                var CostCenter = _unitOfWork.CostCenter.GetAllCostCenter(UserInfo.fCompanyId);
                var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                var TransToPost = new List<AssetTransToPostVM>();
                foreach (var Trans in AllCredit)
                {
                    if (!String.IsNullOrEmpty(Obj.AccountNo))
                        Trans.AccountNo = Obj.AccountNo;
                    if (!String.IsNullOrEmpty(Obj.CostCenter))
                        Trans.CostCenterNo = Obj.CostCenter;
                    Trans.Credit = Trans.Value;
                    if (!String.IsNullOrEmpty(Trans.CostCenterNo))
                        Trans.CostCenterName = CostCenter.FirstOrDefault(m => m.CostNumber == Trans.CostCenterNo).ArabicName;
                    TransToPost.Add(Trans);
                }
                foreach (var Trans in AllDebit)
                {
                    Trans.Debit = Trans.Value;
                    if (!String.IsNullOrEmpty(Trans.CostCenterNo))
                        Trans.CostCenterName = CostCenter.FirstOrDefault(m => m.CostNumber == Trans.CostCenterNo).ArabicName;
                    TransToPost.Add(Trans);

                }
                var Transaction = new List<Transaction>();
                foreach(var Trans in TransToPost)
                {
                    var TransAc = new Transaction();
                    TransAc.AccountNumber = Trans.AccountNo;
                    TransAc.CompanyID = UserInfo.fCompanyId;
                    TransAc.Debit = Trans.Debit;
                    TransAc.Credit = Trans.Credit;
                    TransAc.CostCenter = Trans.CostCenterNo;
                    Transaction.Add(TransAc);
                };
                var TranAction = Transaction;
                double TotalDebit = 0;
                double TotalCredit = 0;
                double TotalDebitForeign = 0;
                double TotalCreditForeign = 0;
                int iRow = 0;
                var AllAcc = _unitOfWork.NativeSql.GetTransActionAccount(UserInfo.fCompanyId);
                foreach (var Trans in TranAction)
                {
                    if (!String.IsNullOrWhiteSpace(Trans.AccountNumber))
                    {
                        iRow = iRow + 1;
                        try
                        {
                            TotalDebit = TotalDebit + Trans.Debit;
                            TotalCredit = TotalCredit + Trans.Credit;

                            if (Trans.Debit != 0)
                            {
                                TotalDebitForeign = TotalDebitForeign + Trans.CreditDebitForeign;
                            }
                            else
                            {
                                TotalCreditForeign = TotalCreditForeign + Trans.CreditDebitForeign;
                            }

                            if (AllAcc.FirstOrDefault(m => m.AccountNumber == Trans.AccountNumber) == null)
                            {
                                Msg.Msg = Resources.Resource.ThisAccountNoDoseNotExist + " : " + Trans.AccountNumber;
                                Msg.Code = 0;
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

                var ObjHeader = new Header();
                ObjHeader.CompanyTransactionKindNo = Obj.CoTranKind;
                var HeaderTOSave = ObjHeader;
                HeaderTOSave.InsDateTime = DateTime.Now;
                HeaderTOSave.InsUserID = userId;
                HeaderTOSave.TransactionKindNo = _unitOfWork.CompanyTransactionKind.GetTransKindForCompanyTransactionKind(UserInfo.fCompanyId, HeaderTOSave.CompanyTransactionKindNo);
                HeaderTOSave.CompanyID = UserInfo.fCompanyId;
                HeaderTOSave.TotalCredit = TotalCredit;
                HeaderTOSave.TotalDebit = TotalDebit;
                HeaderTOSave.TotalDebitForeign = TotalDebitForeign;
                HeaderTOSave.TotalCreditForeign = TotalCreditForeign;
                HeaderTOSave.CompanyYear = CurrYear;
                HeaderTOSave.FCurrencyID = 1;
                HeaderTOSave.VoucherDate = Obj.TransDate;
                var ObjComapnyTransactionKind = _unitOfWork.CompanyTransactionKind.GetCompanyTransactionKindByID(UserInfo.fCompanyId, HeaderTOSave.CompanyTransactionKindNo);
                if (ObjComapnyTransactionKind.AutoSerial)
                {
                    HeaderTOSave.VoucherNumber = _unitOfWork.Header.GetMaxVHByKind(UserInfo.fCompanyId, HeaderTOSave.CompanyTransactionKindNo, HeaderTOSave.TransactionKindNo, HeaderTOSave.CompanyYear).ToString();
                    HeaderTOSave.VHI = int.Parse(HeaderTOSave.VoucherNumber);
                }
                else
                {
                    string SerialNumber = "";
                    int Serial = ObjComapnyTransactionKind.Serial;
                    string Symbol = ObjComapnyTransactionKind.Symbol;
                    DateTime dDate = HeaderTOSave.VoucherDate;
                    string YearNo = dDate.ToString("yy");
                    string sMonth = dDate.ToString("MM");
                    int MonthNo = Int16.Parse(sMonth);
                    int LengthLastSerial = _unitOfWork.CompanyTransactionKindMonthlySerial.GetMaxSerial(UserInfo.fCompanyId, HeaderTOSave.CompanyTransactionKindNo, MonthNo).ToString().Length;
                    Serial = Serial - LengthLastSerial;
                    for (int i = 0; i <= Serial; i++)
                    {
                        if (i < Serial)
                        {
                            SerialNumber = SerialNumber + "0";
                        }
                        else if (i == Serial)
                        {
                            SerialNumber = SerialNumber + _unitOfWork.CompanyTransactionKindMonthlySerial.GetMaxSerial(UserInfo.fCompanyId, HeaderTOSave.CompanyTransactionKindNo, MonthNo).ToString();
                        }
                    }
                    HeaderTOSave.VoucherNumber = Symbol + YearNo + sMonth + SerialNumber;
                    HeaderTOSave.VHI = _unitOfWork.CompanyTransactionKindMonthlySerial.GetMaxSerial(UserInfo.fCompanyId, HeaderTOSave.CompanyTransactionKindNo, MonthNo);
                    _unitOfWork.CompanyTransactionKindMonthlySerial.Update(UserInfo.fCompanyId, HeaderTOSave.CompanyTransactionKindNo, MonthNo);
                }
                if (Company.DirectExportTheTransactions)
                {
                    HeaderTOSave.Exported = 1;
                }
                else
                {
                    HeaderTOSave.Exported = 0;
                }
                iRow = 0;
                foreach (var Trans in TranAction)
                {
                    if (!String.IsNullOrWhiteSpace(Trans.AccountNumber))
                    {
                        try
                        {
                            iRow = iRow + 1;
                            Trans.CompanyID = UserInfo.fCompanyId;
                            Trans.CompanyYear = HeaderTOSave.CompanyYear;
                            Trans.CompanyTransactionKindNo = HeaderTOSave.CompanyTransactionKindNo;
                            Trans.TransactionKindNo = HeaderTOSave.TransactionKindNo;
                            Trans.VoucherDate = HeaderTOSave.VoucherDate;
                            Trans.VoucherNumber = HeaderTOSave.VoucherNumber;
                            Trans.InsUserID = HeaderTOSave.InsUserID;
                            Trans.VHI = HeaderTOSave.VHI;
                            Trans.InsDateTime = DateTime.Now;
                            Trans.RowNumber = iRow;
                            Trans.Note = Resources.Resource.EditingConsumption + VouchrNo.ToString();

                            if (Trans.Debit > 0)
                            {
                                Trans.DebitForeign = Trans.CreditDebitForeign;
                            }
                            else
                            {
                                Trans.CreditForeign = Trans.CreditDebitForeign;
                            }
                            _unitOfWork.Transaction.Add(Trans);
                        }
                        catch (Exception ex)
                        {
                            Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                            Msg.Code = 0;
                            return Json(Msg, JsonRequestBehavior.AllowGet);

                        }
                    }
                }
                HeaderTOSave.RowCount = iRow;
                HeaderTOSave.Note = Resources.Resource.EditingConsumption + VouchrNo.ToString();
                _unitOfWork.Header.Add(HeaderTOSave);
                _unitOfWork.AssetHeader.ExportAssetByVouchrNo(UserInfo.fCompanyId, VouchrNo);
                _unitOfWork.AssetTransAction.ExportAssetTransByVouchrNo(UserInfo.fCompanyId, VouchrNo);
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
        public JsonResult GetAssetCalculation(AssetVMFilter Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllAsset = _unitOfWork.Asset.GetAllAssetNative(UserInfo.fCompanyId,0);
                if (AllAsset == null)
                {
                    return Json(new List<AssetVM>(), JsonRequestBehavior.AllowGet);
                }
                if (Obj.FAssetTypeID != null)
                {
                    AllAsset = AllAsset.Where(m => m.FAssetTypeID == int.Parse(Obj.FAssetTypeID.ToString())).ToList();
                }
                int CurrYear = UserInfo.CurrYear;
                DateTime dec31 = new DateTime(CurrYear, 12, 31);
                int DaysInYear = dec31.DayOfYear;
                double Years = 0;
                foreach (var A in AllAsset)
                {
                 

                     Years = (100 / A.AssetConsumRatio);
                    A.AnnualConsumption = Math.Round((A.AssetCost / Years), 3);
                    A.ConsumptionPerDay = Math.Round(A.AnnualConsumption / DaysInYear, 3);

                    if (A.LastConsumptionDate< Obj.ToDate)
                    {
                        var DiffDays = (Obj.ToDate-A.LastConsumptionDate ).TotalDays;
                        A.ValueofConsumption = Math.Round( DiffDays * A.ConsumptionPerDay,3);
                        A.ConsumptionEndPeriod = (A.ValueofConsumption + A.CombinedConsum);
                        A.BookValue =  (A.AssetCost -( A.ValueofConsumption+ A.CombinedConsum));

                    }
                    else
                    {
                        A.ValueofConsumption = 0;
                      
                    }
                   
                }

                return Json(AllAsset, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<AssetVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public JsonResult MaintenanceAsset(AssetMaintenance ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                ObjToSave.CompanyID = UserInfo.fCompanyId;
                ObjToSave.AddBy = userId;
                ObjToSave.InsDateTime = DateTime.Now;
                ObjToSave.InsUserID = userId;

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

                ObjToSave.Serial = _unitOfWork.AssetMaintenance.GetMaxSerial(ObjToSave.CompanyID);
                _unitOfWork.AssetMaintenance.Add(ObjToSave);
                _unitOfWork.Asset.UpdateGrant(ObjToSave.CompanyID, ObjToSave.AssetID, ObjToSave.GrundToDate);

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
        [Authorize(Roles = "CoOwner,UpdateAssetMaintenance")]
        public JsonResult UpdateMaintenanceAsset(AssetMaintenance ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                ObjToSave.CompanyID = UserInfo.fCompanyId;
                ObjToSave.InsDateTime = DateTime.Now;
                ObjToSave.InsUserID = userId;
                ObjToSave.AddBy = userId;

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

                
                 _unitOfWork.AssetMaintenance.Update(ObjToSave);
               // _unitOfWork.Asset.UpdateGrant(ObjToSave.CompanyID, ObjToSave.AssetID, ObjToSave.GrundToDate);

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
        [Authorize(Roles = "CoOwner,DeleteAssetMaintenance")]
        public JsonResult DeleteMaintenanceAsset(AssetMaintenance ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                ObjToSave.CompanyID = UserInfo.fCompanyId;
                ObjToSave.AddBy = userId;

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


                 _unitOfWork.AssetMaintenance.Delete(ObjToSave.CompanyID,ObjToSave.Serial);
                
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
        [HttpPost]
        [Authorize(Roles = "CoOwner,UpdateAsset")]
        public JsonResult UpdateAsset(Asset ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                ObjToSave.CompanyID = UserInfo.fCompanyId;
                ObjToSave.InsDateTime = DateTime.Now;
                ObjToSave.InsUserID = userId;
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
               

                if (ObjToSave.UpdateFinancialInfo)
                {
                    if (_unitOfWork.Asset.CheckAssetInTrans(ObjToSave.CompanyID, ObjToSave.AssetID))
                    {
                        Msg.Msg = Resources.Resource.CantUpdate+"  "+Resources.Resource.AssetContainTransAction ;
                        Msg.Code = 0;
                        return Json(Msg, JsonRequestBehavior.AllowGet);
                    }
                    _unitOfWork.Asset.UpdateWithFinancial(ObjToSave);
                    _unitOfWork.Complete();
                }
                else
                {
                    _unitOfWork.Asset.UpdateBasic(ObjToSave);
                    _unitOfWork.Complete();
                }
               
               
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
        [Authorize(Roles = "CoOwner,DeleteAsset")]
        public JsonResult DeleteAsset(Asset ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                ObjToSave.CompanyID = UserInfo.fCompanyId;



                _unitOfWork.NativeSql.DeleteDefinitionAsset(ObjToSave.CompanyID, ObjToSave.AssetID);
                _unitOfWork.Asset.Delete(ObjToSave);
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
        [HttpPost]


        [Authorize(Roles = "CoOwner,AddShowSaleAsset")]

        public JsonResult SaleAsset(Asset ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                ObjToSave.CompanyID = UserInfo.fCompanyId;
                ObjToSave.InsDateTime = DateTime.Now;
                ObjToSave.InsUserID = userId;
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

                AssetSaleConsum assetSaleConsum = new AssetSaleConsum
                {
                    CompanyID = ObjToSave.CompanyID,
                    AssetID = ObjToSave.AssetID,
                    Ammount = ObjToSave.SaleValue,
                    BookAmmount = ObjToSave.BookValue,
                    Note = ObjToSave.SaleNote,
                    SoldBy = userId,
                    TrDate = ObjToSave.SaleDate,
                    Type = 1,
                    Serial = _unitOfWork.AssetSaleConsum.GetMaxSerial(ObjToSave.CompanyID, 1)

                };

                   _unitOfWork.AssetSaleConsum.Add(assetSaleConsum);
                   ObjToSave.SaleConsumID = assetSaleConsum.Serial;
                    _unitOfWork.Asset.Sale(ObjToSave);
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

        [Authorize(Roles = "CoOwner,RepConsumAssetReport")]

        public JsonResult ConsumAsset(Asset ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                ObjToSave.CompanyID = UserInfo.fCompanyId;
                ObjToSave.InsDateTime = DateTime.Now;
                ObjToSave.InsUserID = userId;
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

                AssetSaleConsum assetSaleConsum = new AssetSaleConsum
                {
                    CompanyID = ObjToSave.CompanyID,
                    AssetID = ObjToSave.AssetID,
                    Ammount = ObjToSave.SaleValue,
                    BookAmmount = ObjToSave.BookValue,
                    Note = ObjToSave.ConsumingNote,
                    SoldBy = userId,
                    TrDate = ObjToSave.ConsumingDate,
                    Type = 2,
                    Serial = _unitOfWork.AssetSaleConsum.GetMaxSerial(ObjToSave.CompanyID, 2)

                };

                _unitOfWork.AssetSaleConsum.Add(assetSaleConsum);
                ObjToSave.SaleConsumID = assetSaleConsum.Serial;

                _unitOfWork.Asset.Consum(ObjToSave);
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
        public JsonResult CheckAssetID(int id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);

            var AssetInfo = _unitOfWork.NativeSql.GetAssetByID(UserInfo.fCompanyId, id);
            if (AssetInfo == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(AssetInfo.AssetID, JsonRequestBehavior.AllowGet);
            }

        }
        [Authorize(Roles = "CoOwner,RepAssetsReport")]  
        public ActionResult AssetsReport()
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var AssetTypeObj = _unitOfWork.AssetType.GetAllAssetType(UserInfo.fCompanyId);
            int CurrYear = UserInfo.CurrYear;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var DefinitionAssetSiteVM = new DefinitionAssetSiteVM
            {
                AssetType = AssetTypeObj,
                FromConsStartDate = DateTime.Now,
                FromCombinedtDate = DateTime.Now,
                ToConsStartDate = DateTime.Now,
                ToCombinedtDate = DateTime.Now,
                WorkWithCostCenter = Company.WorkWithCostCenter,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency



            };
            return View(DefinitionAssetSiteVM);
        }
        [HttpPost]
        public JsonResult GetAssetsReport(DefinitionAssetSiteVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllDefinitionAssetSiteVM = _unitOfWork.NativeSql.GetAssetsReport(UserInfo.fCompanyId, Obj.ApproveCombinedtDate,
                Obj.ApproveConsStartDate, Obj.FromCombinedtDate, Obj.ToCombinedtDate, Obj.FromConsStartDate, Obj.ToConsStartDate);
                if (AllDefinitionAssetSiteVM == null)
                {
                    return Json(new List<DefinitionAssetSiteVM>(), JsonRequestBehavior.AllowGet);
                }
                if (Obj.AssetTypeID != 0)
                {
                    AllDefinitionAssetSiteVM = AllDefinitionAssetSiteVM.Where(m => m.AssetTypeID == Obj.AssetTypeID).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.DebitAccountNo))
                {
                    AllDefinitionAssetSiteVM = AllDefinitionAssetSiteVM.Where(m => m.DebitAccountNo == Obj.DebitAccountNo).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.CreditAccountNo))
                {
                    AllDefinitionAssetSiteVM = AllDefinitionAssetSiteVM.Where(m => m.CreditAccountNo == Obj.CreditAccountNo).ToList();
                }
                return Json(AllDefinitionAssetSiteVM, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<DefinitionAssetSiteVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [Authorize(Roles = "CoOwner,RepConsumptionByTypeReport")]
        public ActionResult ConsumptionByTypeReport()
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var AssetTypeObj = _unitOfWork.AssetType.GetAllAssetType(UserInfo.fCompanyId);
            int CurrYear = UserInfo.CurrYear;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var DefinitionAssetSiteVM = new DefinitionAssetSiteVM
            {
                AssetType = AssetTypeObj,
                FromConsStartDate = DateTime.Now,
                ToConsStartDate = DateTime.Now,

                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency


            };
            return View(DefinitionAssetSiteVM);
        }
        [HttpPost]
        public JsonResult GetConsumptionByTypeReport(DefinitionAssetSiteVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllDefinitionAssetSiteVM = _unitOfWork.NativeSql.GetConsumptionByTypeReport(UserInfo.fCompanyId,Obj.ApproveConsStartDate, Obj.FromConsStartDate, Obj.ToConsStartDate);
                if (AllDefinitionAssetSiteVM == null)
                {
                    return Json(new List<DefinitionAssetSiteVM>(), JsonRequestBehavior.AllowGet);
                }
                if (Obj.AssetTypeID != 0)
                {
                    AllDefinitionAssetSiteVM = AllDefinitionAssetSiteVM.Where(m => m.AssetTypeID == Obj.AssetTypeID).ToList();
                }
                return Json(AllDefinitionAssetSiteVM, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<DefinitionAssetSiteVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [Authorize(Roles = "CoOwner,RepAssetDepreciationReport")]
        public ActionResult AssetDepreciationReport()
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            int CurrYear = UserInfo.CurrYear;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var DefinitionAssetSiteVM = new DefinitionAssetSiteVM
            {
                FromVouchrDate = DateTime.Now,
                ToVouchrDate = DateTime.Now,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency


            };
            return View(DefinitionAssetSiteVM);
        }
        [HttpPost]
        public JsonResult GetAssetDepreciationReport(DefinitionAssetSiteVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllDefinitionAssetSiteVM = _unitOfWork.NativeSql.GetAllAssetsHeader(UserInfo.fCompanyId, Obj.FromVouchrDate, Obj.ToVouchrDate);
                if (AllDefinitionAssetSiteVM == null)
                {
                    return Json(new List<DefinitionAssetSiteVM>(), JsonRequestBehavior.AllowGet);
                }
                return Json(AllDefinitionAssetSiteVM, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<DefinitionAssetSiteVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        [Authorize(Roles = "CoOwner,RepAssetDepreciationReport")]
        public ActionResult AssetDepreciationTransactionReport(int id)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var AssetTypeObj = _unitOfWork.AssetType.GetAllAssetType(UserInfo.fCompanyId);
            var AssetHeader = _unitOfWork.AssetHeader.GetAssetHeaderByID(UserInfo.fCompanyId,id);
            int CurrYear = UserInfo.CurrYear;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var DefinitionAssetSiteVM = new DefinitionAssetSiteVM
            {
                VouchrNo = AssetHeader.VouchrNo,
                VouchrDate = AssetHeader.VouchrDate,
                FromConsumptionDate = new DateTime(CurrYear, 01, 01),
                ToConsumptionDate = new DateTime(CurrYear, 12, 31),
                FromLastConsumptionDate = new DateTime(CurrYear, 01, 01),
                ToLastConsumptionDate = new DateTime(CurrYear, 12, 31),
                AssetType = AssetTypeObj,

                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency



            };
            return View(DefinitionAssetSiteVM);
        }
        [HttpPost]
        public JsonResult GetAssetDepreciationTransactionReport(DefinitionAssetSiteVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AssetDepreciationTransactionReport = _unitOfWork.NativeSql.GetAssetDepreciationTransactionReport(UserInfo.fCompanyId, Obj.VouchrNo, Obj.ApproveLastConsumptionDate,
                    Obj.ApproveConsumptionDate, Obj.FromLastConsumptionDate, Obj.ToLastConsumptionDate, Obj.FromConsumptionDate,Obj.ToConsumptionDate);
                if (AssetDepreciationTransactionReport == null)
                {
                    return Json(new List<DefinitionAssetSiteVM>(), JsonRequestBehavior.AllowGet);
                }
                if (Obj.AssetTypeID != 0)
                {
                    AssetDepreciationTransactionReport = AssetDepreciationTransactionReport.Where(m => m.AssetTypeID == Obj.AssetTypeID).ToList();
                }
                if (Obj.AssetID != 0)
                {
                    AssetDepreciationTransactionReport = AssetDepreciationTransactionReport.Where(m => m.AssetID == Obj.AssetID).ToList();
                }
                return Json(AssetDepreciationTransactionReport, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<DefinitionAssetSiteVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public JsonResult CheckAssetBeforDelete(int id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            if (id != 0)
            {
                int AssetID = _unitOfWork.NativeSql.CheckAsset(UserInfo.fCompanyId, id);

                return Json(AssetID, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
    }
}