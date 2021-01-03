using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ISupplierCountryRepo
    {
        IEnumerable<SupplierCountry> GetAllSupplierCountry(int CompanyID);
        SupplierCountry GetSupplierCountryByID(int CompanyID, int SupplierCountryID);
        void Add(SupplierCountry ObjSave);
        void Update(SupplierCountry ObjUpdate);
        void Delete(SupplierCountry ObjDelete);
        int GetMaxSerial(int CompanyID);
    }
}