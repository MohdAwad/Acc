using Acc.Models;
using Acc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface IAssetRepo
    {
        IEnumerable<Asset> GetAllAsset(int CompanyID, int AssetStatus);

        IEnumerable<AssetVM> GetAllAssetNative(int CompanyID,int AssetStatus);
        IEnumerable<AssetInofVM> GetAllAssetNativePrint(int CompanyID, int AssetStatus);
        Asset GetAssetByID(int CompanyID, int AssetID);
        void Add(Asset ObjSave);
        void UpdateBasic(Asset ObjUpdate);
        void Sale(Asset ObjUpdate);
        void Consum(Asset ObjUpdate);
        void UpdateConsumption(Asset ObjUpdate);
        void UpdateWithFinancial(Asset ObjUpdate);

        void UpdateGrant(int CompanyID, int AssetID,DateTime GrntDate);
        void Delete(Asset ObjDelete);
        int GetMaxSerial(int CompanyID,int AssetTypeID);

        bool CehckNumber(int CompanyID, int AssetID);
        bool CheckAssetInTrans(int CompanyID, int AssetID);

        AssetVM CheckAllAssetNative(int CompanyID, int AssetStatus, string AccountNumber);




    }
}