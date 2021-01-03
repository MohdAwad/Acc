using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Acc.Persistence
{
    public class AssetAdministrationRepo : IAssetAdministrationRepo
    {
        private readonly ApplicationDbContext _context;

        public AssetAdministrationRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(AssetAdministration ObjSave)
        {
            _context.AssetAdministrations.Add(ObjSave);
        }

        public void Delete(AssetAdministration ObjDelete)
        {
            var ObjToDelete = _context.AssetAdministrations.SingleOrDefault(m => m.CompanyID == ObjDelete.CompanyID && m.AdministrationID == ObjDelete.AdministrationID);
            if (ObjToDelete != null)
            {
                _context.AssetAdministrations.Remove(ObjToDelete);
            }
        }

        public IEnumerable<AssetAdministration> GetAllAssetAdministration(int CompanyID)
        {
            return _context.AssetAdministrations.Where(m => m.CompanyID == CompanyID).ToList();
        }

        public int GetMaxSerial(int CompanyID)
        {
            var Obj = _context.AssetAdministrations.FirstOrDefault(m => m.CompanyID == CompanyID);
            if (Obj == null)
            {
                return 1;
            }
            else
            {
                return _context.AssetAdministrations.Where(m => m.CompanyID == CompanyID).Max(p => p.AdministrationID) + 1;
            }
        }

        public AssetAdministration GetAssetAdministrationByID(int CompanyID, int AdministrationID)
        {
            return _context.AssetAdministrations.FirstOrDefault(m => m.CompanyID == CompanyID && m.AdministrationID == AdministrationID);
        }
        public void Update(AssetAdministration ObjUpdate)
        {
            var ObjToUpdate = _context.AssetAdministrations.FirstOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.AdministrationID == ObjUpdate.AdministrationID);
            if (ObjToUpdate != null)
            {
                ObjToUpdate.AdministrationName = ObjUpdate.AdministrationName;
                ObjToUpdate.InsDateTime = ObjUpdate.InsDateTime;
                ObjToUpdate.InsUserID = ObjUpdate.InsUserID;
            }
        }
    }
}