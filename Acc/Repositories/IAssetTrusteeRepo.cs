using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface IAssetTrusteeRepo
    {
        IEnumerable<AssetTrustee> GetAllAssetTrustee(int CompanyID);
        AssetTrustee GetAssetTrusteeByID(int CompanyID, int TrusteeID);
        void Add(AssetTrustee ObjSave);
        void Update(AssetTrustee ObjUpdate);
        void Delete(AssetTrustee ObjDelete);
        int GetMaxSerial(int CompanyID);
    }
}