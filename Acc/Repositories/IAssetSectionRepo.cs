using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface IAssetSectionRepo
    {
        IEnumerable<AssetSection> GetAllAssetSection(int CompanyID);
        AssetSection GetAssetSectionByID(int CompanyID, int SectionID);
        void Add(AssetSection ObjSave);
        void Update(AssetSection ObjUpdate);
        void Delete(AssetSection ObjDelete);
        int GetMaxSerial(int CompanyID);
    }
}