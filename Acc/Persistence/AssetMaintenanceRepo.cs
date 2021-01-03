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
    public class AssetMaintenanceRepo : IAssetMaintenanceRepo
    {
        private readonly ApplicationDbContext _context;

        public AssetMaintenanceRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<AssetMaintenanceVM> GetAllAssetMaintenanceNative(int CompanyID)
        {
            try

            {
                return _context.Database.SqlQuery<AssetMaintenanceVM>(
                " select A.*,T.Name  as AssetTypeName from AssetMaintenances A,AssetTypes T " +
                " where  A.FAssetTypeID=T.AssetTypeID " +
                " and A.CompanyID=@CompanyID and T.CompanyID=@CompanyID  and A.CompanyID=T.CompanyID    "
                , new SqlParameter("@CompanyID", CompanyID)
                 

            ).ToList();
            }
            catch (Exception ex)
            {
                return new List<AssetMaintenanceVM>();
            }
        }
        public int GetMaxSerial(int CompanyID)
        {
            var Obj = _context.AssetMaintenances.FirstOrDefault(m => m.CompanyID == CompanyID);
            if (Obj == null)
            {
                return 1;
            }
            else
            {
                return _context.AssetMaintenances.Where(m => m.CompanyID == CompanyID).Max(p => p.Serial) + 1;
            }
        }
        public void Add(AssetMaintenance ObjToSave)
        {
            _context.AssetMaintenances.Add(ObjToSave);
        }

        public void Delete(int CompanyID, int Id)
        {
            var D = _context.AssetMaintenances.FirstOrDefault(m => m.CompanyID == CompanyID && m.Serial == Id);
            if (D != null)
            {
                _context.AssetMaintenances.Remove(D);
            }
        }

        public IEnumerable<AssetMaintenance> GetAllAssetMaintenance(int CompanyID)
        {
            return _context.AssetMaintenances.Where(m => m.CompanyID == CompanyID).ToList();
        }

        public AssetMaintenance GetAssetMaintenanceByID(int CompanyID, int Id)
        {
          return _context.AssetMaintenances.FirstOrDefault(m => m.CompanyID == CompanyID && m.Serial == Id);
        }

        

        public void Update(AssetMaintenance ObjToSave)
        {
            var D = _context.AssetMaintenances.FirstOrDefault(m => m.CompanyID == ObjToSave.CompanyID && m.Serial == ObjToSave.Serial);
            if (D != null)
            {
                D.GrundToDate = ObjToSave.GrundToDate;
                D.MaintenanceNo = ObjToSave.MaintenanceNo;
                D.MaintenanceNote = ObjToSave.MaintenanceNote;
                D.MaintenanceVoucherNo = ObjToSave.MaintenanceVoucherNo;
                D.PayValue = ObjToSave.PayValue;
                D.TrDate = ObjToSave.TrDate;
                D. TrKind = ObjToSave.TrKind;
                D.VoucherDate = ObjToSave.VoucherDate;
                D.AddBy = ObjToSave.AddBy;

            }
        }
    }
}