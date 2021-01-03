using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Acc.Repositories
{
    public interface ISupplierCountryBankRepo
    {
        IEnumerable<SupplierCountryBank> GetAllSupplierCountryBank(int CompanyID);
        SupplierCountryBank GetSupplierCountryBankByID(int CompanyID, int SupplierCountryBankID);
        void Add(SupplierCountryBank ObjSave);
        void Update(SupplierCountryBank ObjUpdate);
        void Delete(SupplierCountryBank ObjDelete);
        int GetMaxSerial(int CompanyID);
    }
}