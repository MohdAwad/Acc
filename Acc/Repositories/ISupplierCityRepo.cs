using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ISupplierCityRepo
    {
        void Add(SupplierCity ObjSave);
        void Update(SupplierCity ObjUpdate);
        void Delete(SupplierCity ObjDelete);
        int GetMaxSerial(int CompanyID);
        IEnumerable<SupplierCity> GetAllSupplierCity(int CompanyID);
        IEnumerable<SupplierCity> GetAllSupplierCityByCountryID(int CompanyID, int SupplierCountryID);
    }
}