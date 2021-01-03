using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ISupplierBranchBankRepo
    {
        void Add(SupplierBranchBank ObjSave);
        void Update(SupplierBranchBank ObjUpdate);
        void Delete(SupplierBranchBank ObjDelete);
        int GetMaxSerial(int CompanyID);
        IEnumerable<SupplierBranchBank> GetAllSupplierBranchBank(int CompanyID);
        IEnumerable<SupplierBranchBank> GetAllSupplierBranchBankByBankID(int CompanyID,int SupplierBankID);
    }
}