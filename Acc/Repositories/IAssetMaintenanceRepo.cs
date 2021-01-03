using Acc.Models;
using Acc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface IAssetMaintenanceRepo
    {
        void Add(AssetMaintenance ObjToSave);
        void Update(AssetMaintenance ObjToSave);
        void Delete( int CompanyID, int Id);
        IEnumerable<AssetMaintenance> GetAllAssetMaintenance(int CompanyID);
        AssetMaintenance GetAssetMaintenanceByID(int CompanyID, int Id);
        IEnumerable<AssetMaintenanceVM> GetAllAssetMaintenanceNative(int CompanyID);
        int GetMaxSerial(int CompanyID);
    }
}