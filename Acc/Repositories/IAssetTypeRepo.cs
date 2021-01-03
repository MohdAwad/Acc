using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface IAssetTypeRepo
    {
        IEnumerable<AssetType> GetAllAssetType(int CompanyID);
        AssetType GetAssetTypeByID(int CompanyID, int AssetTypeID);
        void Add(AssetType ObjSave);
        void Update(AssetType ObjUpdate);
        void Delete(AssetType ObjDelete);
        int GetMaxSerial(int CompanyID);
        bool IsConected(int CompanyID, int AssetTypeID);
    }
}