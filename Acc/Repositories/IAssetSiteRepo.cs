using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface IAssetSiteRepo
    {
        IEnumerable<AssetSite> GetAllAssetSite(int CompanyID);
        AssetSite GetAssetSiteByID(int CompanyID, int SiteID);
        void Add(AssetSite ObjSave);
        void Update(AssetSite ObjUpdate);
        void Delete(AssetSite ObjDelete);
        int GetMaxSerial(int CompanyID);
    }
}
