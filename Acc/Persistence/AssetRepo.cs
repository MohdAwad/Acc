using Acc.Models;
using Acc.Repositories;
using Acc.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class AssetRepo : IAssetRepo
    {
        private readonly ApplicationDbContext _context;

        public AssetRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(Asset ObjSave)
        {
            _context.Assets.Add(ObjSave);
        }

        public bool CehckNumber(int CompanyID, int AssetID)
        {
            try
            {
                var D = _context.Assets.FirstOrDefault(m => m.CompanyID == CompanyID && m.AssetID == AssetID);
                if (D != null)
                {
                    return true;
                }
                else
                    return false;
            }
            catch(Exception ex)
            {
                string s = ex.Message;
                return true;
            }
        }
        public bool CheckAssetInTrans(int CompanyID, int AssetID)
        {
            try
            {
                var D = _context.AssetTransActions.FirstOrDefault(m => m.CompanyID == CompanyID && m.AssetID == AssetID);
                if (D != null)
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                return true;
            }
        }
        public void Consum(Asset ObjUpdate)
        {
            try
            {
                var D = _context.Assets.FirstOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.AssetID == ObjUpdate.AssetID);
                if (D != null)
                {

                    D.ConsumingDate = ObjUpdate.ConsumingDate;
                    D.ConsumingNote = ObjUpdate.ConsumingNote;
                    D.SaleConsumID = ObjUpdate.SaleConsumID;
                    D.InsDateTime = ObjUpdate.InsDateTime;
                    D.InsUserID = ObjUpdate.InsUserID;

                    D.AssetStatus = 2;



                }
            }
            catch (Exception ex)
            {
                string er = ex.Message;
            }
        }

        public void Delete(Asset ObjDelete)
        {

            try
            {
                var D = _context.Assets.FirstOrDefault(m => m.CompanyID == ObjDelete.CompanyID && m.AssetID == ObjDelete.AssetID);
                if (D != null)
                {
                    _context.Assets.Remove(D);
                }
            }
            catch (Exception ex)
            {
                string er = ex.Message;
            }
        }

       

        public IEnumerable<Asset> GetAllAsset(int CompanyID,int AssetStatus)
        {
            try
            {
                return _context.Assets.Where(m => m.CompanyID == CompanyID && m.AssetStatus== AssetStatus).ToList();
            }
            catch(Exception ex)
            {
                return new List<Asset>();
            }
        }

        public IEnumerable<AssetVM> GetAllAssetNative(int CompanyID,int AssetStatus)
        {
            try

            {
                return _context.Database.SqlQuery<AssetVM>(
                " select A.*,T.Name  as AssetTypeName from Assets A,AssetTypes T " +
                " where  A.FAssetTypeID=T.AssetTypeID " +
                " and A.CompanyID=@CompanyID and T.CompanyID=@CompanyID  and A.CompanyID=T.CompanyID  and A.AssetStatus=@AssetStatus "
                , new SqlParameter("@CompanyID", CompanyID)
                 , new SqlParameter("@AssetStatus", AssetStatus)

            ).ToList();
            }
            catch(Exception ex)
            {
                return new List<AssetVM>();
            }
        }
        public AssetVM CheckAllAssetNative(int CompanyID,int AssetStatus , string AccountNumber)
        {
            try

            {
                return _context.Database.SqlQuery<AssetVM>(
                " select A.*,T.Name  as AssetTypeName from Assets A,AssetTypes T " +
                " where  A.FAssetTypeID=T.AssetTypeID " +
                " and A.CompanyID=@CompanyID and T.CompanyID=@CompanyID  and A.CompanyID=T.CompanyID  and A.AssetStatus=@AssetStatus  and A.AssetID=@AccountNumber "
                , new SqlParameter("@CompanyID", CompanyID)
                 , new SqlParameter("@AssetStatus", AssetStatus)
                 , new SqlParameter("@AccountNumber", AccountNumber)

            ).FirstOrDefault();
            }
            catch(Exception ex)
            {
                return new AssetVM();
            }
        }
        public IEnumerable<AssetInofVM> GetAllAssetNativePrint(int CompanyID, int AssetStatus)
        {
            try

            {
              return _context.Database.SqlQuery<AssetInofVM>(
                " select A.*,T.Name  as AssetTypeName from Assets A,AssetTypes T " +
                " where  A.FAssetTypeID=T.AssetTypeID " +
                " and A.CompanyID=@CompanyID and T.CompanyID=@CompanyID  and A.CompanyID=T.CompanyID  and A.AssetStatus=@AssetStatus "
                , new SqlParameter("@CompanyID", CompanyID)
                 , new SqlParameter("@AssetStatus", AssetStatus)

            ).ToList();
            }
            catch (Exception ex)
            {
                return new List<AssetInofVM>();
            }
        }

        public Asset GetAssetByID(int CompanyID, int AssetID)
        {
            try
            {
                return _context.Assets.FirstOrDefault(m => m.CompanyID == CompanyID && m.AssetID==AssetID) ;
            }
            catch (Exception ex)
            {
                return new  Asset ();
            }
        }

        public int GetMaxSerial(int CompanyID, int AssetTypeID)
        {//&& m.FAssetTypeID==AssetTypeID// && m.FAssetTypeID == AssetTypeID
            var Obj = _context.Assets.FirstOrDefault(m => m.CompanyID == CompanyID );
            if (Obj == null)
            {
                return  1 ;
            }
            else
            {
                return (_context.Assets.Where(m => m.CompanyID == CompanyID).Max(p => p.AssetID) + 1) ;
            }
        }

        public void Sale(Asset ObjUpdate)
        {
            try
            {
                var D = _context.Assets.FirstOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.AssetID == ObjUpdate.AssetID);
                if (D != null)
                {

                    D.SaleDate = ObjUpdate.SaleDate;
                    D.SaleNote = ObjUpdate.SaleNote;
                    D.SaleValue = ObjUpdate.SaleValue;
                    D.SaleConsumID = ObjUpdate.SaleConsumID;
                    D.SoldConsNet =D.BookValue-ObjUpdate.SaleValue;
                    D.AssetStatus = 1;





                }
            }
            catch (Exception ex)
            {
                string er = ex.Message;
            }
        }

        public void UpdateBasic(Asset ObjUpdate)
        {
            try
            {
               var D= _context.Assets.FirstOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.AssetID == ObjUpdate.AssetID);
                if (D != null)
                {
                    D.AssetBarCode = ObjUpdate.AssetBarCode;
                    D.FAssetTypeID = ObjUpdate.FAssetTypeID;
                    D.AssetConsumRatio = ObjUpdate.AssetConsumRatio;
                    //D.AssetCost= ObjUpdate.AssetCost;
                    D.AssetName = ObjUpdate.AssetName;
                    D.AssetSerialNo = ObjUpdate.AssetSerialNo;
                  //  D.BookValue = ObjUpdate.BookValue;
                    D.CombinedConsum = ObjUpdate.CombinedConsum;
                    D.CombinedtDate = ObjUpdate.CombinedtDate;
                    //D.ConsStartDate = ObjUpdate.ConsStartDate;
                    D.CreditAccountNo= ObjUpdate.CreditAccountNo;
                    D.CreditCostNo = ObjUpdate.CreditCostNo;
                    D.DebitAccountNo = ObjUpdate.DebitAccountNo;
                    D.DebitCostNo = ObjUpdate.DebitCostNo;
                    D.GuaranteedDate = ObjUpdate.GuaranteedDate;
                    D.LastMaintenanceDate = ObjUpdate.LastMaintenanceDate;
                    D.Note = ObjUpdate.Note;
                    D.PurchaseInvDate = ObjUpdate.PurchaseInvDate;
                    D.PurchaseinvoiceNo = ObjUpdate.PurchaseinvoiceNo;
                    D.PurchaseOrderNo= ObjUpdate.PurchaseOrderNo;
                    D.SupplierAccountNo = ObjUpdate.SupplierAccountNo;
                    D.SupplierNote = ObjUpdate.SupplierNote;
                    D.LastConsumptionDate = ObjUpdate.CombinedtDate;
                   

                   
                }
            }
            catch(Exception ex)
            {
                string er = ex.Message;
            }
        }

        public void UpdateConsumption(Asset ObjUpdate)
        {
            try
            {
                var D = _context.Assets.FirstOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.AssetID == ObjUpdate.AssetID);
                if (D != null)
                {
                   
                    D.BookValue = ObjUpdate.BookValue;
                    D.LastConsumptionDate = ObjUpdate.LastConsumptionDate;
                    D.ValueofConsumption = ObjUpdate.ValueofConsumption;
                    D.PreviousConsumption = ObjUpdate.PreviousConsumption;
                    D.CombinedConsum = D.CombinedConsum+ObjUpdate.CombinedConsum;
                  
                    D.CombinedtDate = ObjUpdate.CombinedtDate;



                }
            }
            catch (Exception ex)
            {
                string er = ex.Message;
            }
        }

        public void UpdateGrant(int CompanyID, int AssetID, DateTime GrntDate)
        {
            var D = _context.Assets.FirstOrDefault(m => m.CompanyID == CompanyID && m.AssetID == AssetID);
            if (D != null)
            {
                if (D.GuaranteedDate < GrntDate)
                {
                    D.GuaranteedDate = GrntDate;
                }
            }
        }

        public void UpdateWithFinancial(Asset ObjUpdate)
        {
            try
            {
                var D = _context.Assets.FirstOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.AssetID == ObjUpdate.AssetID);
                if (D != null)
                {
                    D.AssetBarCode = ObjUpdate.AssetBarCode;
                    D.FAssetTypeID = ObjUpdate.FAssetTypeID;
                    D.AssetConsumRatio = ObjUpdate.AssetConsumRatio;
                    D.AssetCost = ObjUpdate.AssetCost;
                    D.AssetName = ObjUpdate.AssetName;
                    D.AssetSerialNo = ObjUpdate.AssetSerialNo;
                    D.BookValue = ObjUpdate.BookValue;
                    D.CombinedConsum = ObjUpdate.CombinedConsum;
                    D.CombinedtDate = ObjUpdate.CombinedtDate;
                    D.ConsStartDate = ObjUpdate.ConsStartDate;
                    D.CreditAccountNo = ObjUpdate.CreditAccountNo;
                    D.CreditCostNo = ObjUpdate.CreditCostNo;
                    D.DebitAccountNo = ObjUpdate.DebitAccountNo;
                    D.DebitCostNo = ObjUpdate.DebitCostNo;
                    D.GuaranteedDate = ObjUpdate.GuaranteedDate;
                    D.LastMaintenanceDate = ObjUpdate.LastMaintenanceDate;
                    D.Note = ObjUpdate.Note;
                    D.PurchaseInvDate = ObjUpdate.PurchaseInvDate;
                    D.PurchaseinvoiceNo = ObjUpdate.PurchaseinvoiceNo;
                    D.PurchaseOrderNo = ObjUpdate.PurchaseOrderNo;
                    D.SupplierAccountNo = ObjUpdate.SupplierAccountNo;
                    D.SupplierNote = ObjUpdate.SupplierNote;
                    D.LastConsumptionDate = ObjUpdate.CombinedtDate;


                }
            }
            catch (Exception ex)
            {
                string er = ex.Message;
            }
        }
    }
}