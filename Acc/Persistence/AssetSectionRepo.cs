using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class AssetSectionRepo : IAssetSectionRepo
    {
        private readonly ApplicationDbContext _context;

        public AssetSectionRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(AssetSection ObjSave)
        {
            _context.AssetSections.Add(ObjSave);
        }

        public void Delete(AssetSection ObjDelete)
        {
            var ObjToDelete = _context.AssetSections.SingleOrDefault(m => m.CompanyID == ObjDelete.CompanyID && m.SectionID == ObjDelete.SectionID);
            if (ObjToDelete != null)
            {
                _context.AssetSections.Remove(ObjToDelete);
            }
        }

        public IEnumerable<AssetSection> GetAllAssetSection(int CompanyID)
        {
            return _context.AssetSections.Where(m => m.CompanyID == CompanyID).ToList();
        }

        public int GetMaxSerial(int CompanyID)
        {
            var Obj = _context.AssetSections.FirstOrDefault(m => m.CompanyID == CompanyID);
            if (Obj == null)
            {
                return 1;
            }
            else
            {
                return _context.AssetSections.Where(m => m.CompanyID == CompanyID).Max(p => p.SectionID) + 1;
            }
        }

        public AssetSection GetAssetSectionByID(int CompanyID, int SectionID)
        {
            return _context.AssetSections.FirstOrDefault(m => m.CompanyID == CompanyID && m.SectionID == SectionID);
        }
        public void Update(AssetSection ObjUpdate)
        {
            var ObjToUpdate = _context.AssetSections.FirstOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.SectionID == ObjUpdate.SectionID);
            if (ObjToUpdate != null)
            {
                ObjToUpdate.SectionName = ObjUpdate.SectionName;
                ObjToUpdate.InsDateTime = ObjUpdate.InsDateTime;
                ObjToUpdate.InsUserID = ObjUpdate.InsUserID;
            }
        }
    }
}