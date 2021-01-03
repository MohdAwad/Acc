using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface IAssetSaleConsumRepo
    {
        void Add(AssetSaleConsum ObjToSave);
        int GetMaxSerial(int CompanyID, int  TypeID);
        IEnumerable<AssetSaleConsum> GetAssetSaleConsums(int CompanyID, int TypeID);

    }
}