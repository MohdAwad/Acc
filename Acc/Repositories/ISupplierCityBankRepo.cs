using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ISupplierCityBankRepo
    {
        void Add(SupplierCityBank ObjSave);
        void Update(SupplierCityBank ObjUpdate);
        void Delete(SupplierCityBank ObjDelete);
        int GetMaxSerial(int CompanyID);
        IEnumerable<SupplierCityBank> GetAllSupplierCityBank(int CompanyID);
        IEnumerable<SupplierCityBank> GetAllSupplierCityBankByCountryBankID(int CompanyID, int SupplierCountryBankID);
    }
}