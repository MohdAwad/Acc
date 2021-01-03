using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface IAssetCircleRepo
    {
        IEnumerable<AssetCircle> GetAllAssetCircle(int CompanyID);
        AssetCircle GetAssetCircleByID(int CompanyID, int CircleID);
        void Add(AssetCircle ObjSave);
        void Update(AssetCircle ObjUpdate);
        void Delete(AssetCircle ObjDelete);
        int GetMaxSerial(int CompanyID);
    }
}