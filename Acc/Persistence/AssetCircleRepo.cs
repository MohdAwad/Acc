using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Acc.Persistence
{
    public class AssetCircleRepo : IAssetCircleRepo
    {
        private readonly ApplicationDbContext _context;

        public AssetCircleRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(AssetCircle ObjSave)
        {
            _context.AssetCircles.Add(ObjSave);
        }

        public void Delete(AssetCircle ObjDelete)
        {
            var ObjToDelete = _context.AssetCircles.SingleOrDefault(m => m.CompanyID == ObjDelete.CompanyID && m.CircleID == ObjDelete.CircleID);
            if (ObjToDelete != null)
            {
                _context.AssetCircles.Remove(ObjToDelete);
            }
        }

        public IEnumerable<AssetCircle> GetAllAssetCircle(int CompanyID)
        {
            return _context.AssetCircles.Where(m => m.CompanyID == CompanyID).ToList();
        }

        public int GetMaxSerial(int CompanyID)
        {
            var Obj = _context.AssetCircles.FirstOrDefault(m => m.CompanyID == CompanyID);
            if (Obj == null)
            {
                return 1;
            }
            else
            {
                return _context.AssetCircles.Where(m => m.CompanyID == CompanyID).Max(p => p.CircleID) + 1;
            }
        }

        public AssetCircle GetAssetCircleByID(int CompanyID, int CircleID)
        {
            return _context.AssetCircles.FirstOrDefault(m => m.CompanyID == CompanyID && m.CircleID == CircleID);
        }
        public void Update(AssetCircle ObjUpdate)
        {
            var ObjToUpdate = _context.AssetCircles.FirstOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.CircleID == ObjUpdate.CircleID);
            if (ObjToUpdate != null)
            {
                ObjToUpdate.CircleName = ObjUpdate.CircleName;
                ObjToUpdate.InsDateTime = ObjUpdate.InsDateTime;
                ObjToUpdate.InsUserID = ObjUpdate.InsUserID;
            }
        }
    }
}