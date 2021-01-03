using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class AssetTypeRepo: IAssetTypeRepo
    {
        private readonly ApplicationDbContext _context;

        public AssetTypeRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(AssetType ObjSave)
        {
            _context.AssetTypes.Add(ObjSave);
        }

        public void Delete(AssetType ObjDelete)
        {
            var D = _context.AssetTypes.FirstOrDefault(m => m.CompanyID == ObjDelete.CompanyID && m.AssetTypeID == ObjDelete.AssetTypeID);
            if (D != null)
            {
                _context.AssetTypes.Remove(D);
            }
        }

        public IEnumerable<AssetType> GetAllAssetType(int CompanyID)
        {
            try
            {
                return _context.AssetTypes.Where(m => m.CompanyID == CompanyID).ToList(); ;
            }
            catch(Exception ex)
            {
                return new List<AssetType>();
            }
        }

        public AssetType GetAssetTypeByID(int CompanyID, int AssetTypeID)
        {
            try
            {
                return _context.AssetTypes.FirstOrDefault(m => m.CompanyID == CompanyID && m.AssetTypeID == AssetTypeID);
            }
            catch (Exception ex)
            {
                return     new AssetType();
            }
        }

        public int GetMaxSerial(int CompanyID)
        {
            var Obj = _context.AssetTypes.FirstOrDefault(m => m.CompanyID == CompanyID);
            if (Obj == null)
            {
                return 1;
            }
            else
            {
                return _context.AssetTypes.Where(m => m.CompanyID == CompanyID).Max(p => p.AssetTypeID) + 1;
            }
        }

        public bool IsConected(int CompanyID, int AssetTypeID)
        {
            var D = _context.Assets.FirstOrDefault(m => m.CompanyID ==  CompanyID && m.FAssetTypeID ==  AssetTypeID);
            if (D != null)
            {
                return true;
            }
            else
                return false;
        }

        public void Update(AssetType ObjUpdate)
        {
            var D = _context.AssetTypes.FirstOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.AssetTypeID == ObjUpdate.AssetTypeID);
            if (D != null)
            {
                D.ConsRatio = ObjUpdate.ConsRatio;
                D.Name = ObjUpdate.Name;
                D.InsDateTime = ObjUpdate.InsDateTime;
                D.InsUserID = ObjUpdate.InsUserID;
            }
        }
    }
}