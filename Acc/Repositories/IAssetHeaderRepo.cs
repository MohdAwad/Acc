using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface IAssetHeaderRepo
    {
        void Add(AssetHeader ObjToSave);
        IEnumerable<AssetHeader> GetAssetHeaders(int CompanyID);
        void ExportAssetByVouchrNo(int CompanyID, int VouchrNo);
        int GetMaxSerial(int CompanyID);
        AssetHeader GetAssetHeaderByID(int CompanyID, int VouchrNo);


    }
}