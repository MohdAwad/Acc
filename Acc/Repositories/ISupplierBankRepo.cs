using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ISupplierBankRepo
    {
        IEnumerable<SupplierBank> GetAllSupplierBank(int CompanyID);
        SupplierBank GetSupplierBankByID(int CompanyID, int SupplierBankID);
        void Add(SupplierBank ObjSave);
        void Update(SupplierBank ObjUpdate);
        void Delete(SupplierBank ObjDelete);
        int GetMaxSerial(int CompanyID);
    }
}