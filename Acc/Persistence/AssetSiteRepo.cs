using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class AssetSiteRepo : IAssetSiteRepo
    {
        private readonly ApplicationDbContext _context;

        public AssetSiteRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(AssetSite ObjSave)
        {
            _context.AssetSites.Add(ObjSave);
        }

        public void Delete(AssetSite ObjDelete)
        {
            var ObjToDelete = _context.AssetSites.SingleOrDefault(m => m.CompanyID == ObjDelete.CompanyID && m.SiteID == ObjDelete.SiteID);
            if (ObjToDelete != null)
            {
                _context.AssetSites.Remove(ObjToDelete);
            }
        }

        public IEnumerable<AssetSite> GetAllAssetSite(int CompanyID)
        {
            return _context.AssetSites.Where(m => m.CompanyID == CompanyID).ToList();
        }

        public int GetMaxSerial(int CompanyID)
        {
            var Obj = _context.AssetSites.FirstOrDefault(m => m.CompanyID == CompanyID);
            if (Obj == null)
            {
                return 1;
            }
            else
            {
                return _context.AssetSites.Where(m => m.CompanyID == CompanyID).Max(p => p.SiteID) + 1;
            }
        }

        public AssetSite GetAssetSiteByID(int CompanyID, int SiteID)
        {
            return _context.AssetSites.FirstOrDefault(m => m.CompanyID == CompanyID && m.SiteID == SiteID);
        }
        public void Update(AssetSite ObjUpdate)
        {
            var ObjToUpdate = _context.AssetSites.FirstOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.SiteID == ObjUpdate.SiteID);
            if (ObjToUpdate != null)
            {
                ObjToUpdate.SiteName = ObjUpdate.SiteName;
                ObjToUpdate.InsDateTime = ObjUpdate.InsDateTime;
                ObjToUpdate.InsUserID = ObjUpdate.InsUserID;
            }
        }
    }
}