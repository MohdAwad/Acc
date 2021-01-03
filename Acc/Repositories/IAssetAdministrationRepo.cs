using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface IAssetAdministrationRepo
    {
        IEnumerable<AssetAdministration> GetAllAssetAdministration(int CompanyID);
        AssetAdministration GetAssetAdministrationByID(int CompanyID, int AdministrationID);
        void Add(AssetAdministration ObjSave);
        void Update(AssetAdministration ObjUpdate);
        void Delete(AssetAdministration ObjDelete);
        int GetMaxSerial(int CompanyID);
    }
}