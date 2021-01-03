using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class AssetTrusteeRepo : IAssetTrusteeRepo
    {
        private readonly ApplicationDbContext _context;

        public AssetTrusteeRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(AssetTrustee ObjSave)
        {
            _context.AssetTrustees.Add(ObjSave);
        }

        public void Delete(AssetTrustee ObjDelete)
        {
            var ObjToDelete = _context.AssetTrustees.SingleOrDefault(m => m.CompanyID == ObjDelete.CompanyID && m.TrusteeID == ObjDelete.TrusteeID);
            if (ObjToDelete != null)
            {
                _context.AssetTrustees.Remove(ObjToDelete);
            }
        }

        public IEnumerable<AssetTrustee> GetAllAssetTrustee(int CompanyID)
        {
            return _context.AssetTrustees.Where(m => m.CompanyID == CompanyID).ToList();
        }

        public int GetMaxSerial(int CompanyID)
        {
            var Obj = _context.AssetTrustees.FirstOrDefault(m => m.CompanyID == CompanyID);
            if (Obj == null)
            {
                return 1;
            }
            else
            {
                return _context.AssetTrustees.Where(m => m.CompanyID == CompanyID).Max(p => p.TrusteeID) + 1;
            }
        }

        public AssetTrustee GetAssetTrusteeByID(int CompanyID, int TrusteeID)
        {
            return _context.AssetTrustees.FirstOrDefault(m => m.CompanyID == CompanyID && m.TrusteeID == TrusteeID);
        }
        public void Update(AssetTrustee ObjUpdate)
        {
            var ObjToUpdate = _context.AssetTrustees.FirstOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.TrusteeID == ObjUpdate.TrusteeID);
            if (ObjToUpdate != null)
            {
                ObjToUpdate.TrusteeName = ObjUpdate.TrusteeName;
                ObjToUpdate.InsDateTime = ObjUpdate.InsDateTime;
                ObjToUpdate.InsUserID = ObjUpdate.InsUserID;
            }
        }
    }
}