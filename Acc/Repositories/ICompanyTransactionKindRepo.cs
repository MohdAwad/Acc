using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ICompanyTransactionKindRepo
    {
        void Add(CompanyTransactionKind ObjSave);
        IEnumerable<CompanyTransactionKind> GetAllCompanyTransactionKind(int CompanyID); 
        int GetMaxSerial(int CompanyID);
        void Update(CompanyTransactionKind ObjUpdate);
        void Delete(CompanyTransactionKind ObjDelete);
        CompanyTransactionKind GetCompanyTransactionKindByID(int CompanyID, int CompanyTransactionKindID);
        int GetTransKindForCompanyTransactionKind(int CompanyID, int CompanyTransactionKindID);
        int GetCompanyTransactionKindByTransKindNo(int CompanyID, int TransactionKindID);
    }
}