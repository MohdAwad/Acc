using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Acc.Repositories
{
    public interface ISt_CompanyTransactionKindHRepo
    {
        void Add(St_CompanyTransactionKindH ObjSave);
        IEnumerable<St_CompanyTransactionKindH> GetAllSt_CompanyTransactionKindH(int CompanyID);
        int GetMaxSerial(int CompanyID);
        void Update(St_CompanyTransactionKindH ObjUpdate);
        St_CompanyTransactionKindH GetSt_CompanyTransactionKindHByID(int CompanyID, int St_CompanyTransactionKindID);
        int GetSt_TransKindHForSt_CompanyTransactionKindH(int CompanyID, int St_CompanyTransactionKindID);
        int GetSt_CompanyTransactionKindHBySt_TransKindNo(int CompanyID, int St_TransactionKindID);
    }
}