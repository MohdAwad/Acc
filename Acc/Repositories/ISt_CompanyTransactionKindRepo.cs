using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ISt_CompanyTransactionKindRepo
    {
        void Add(St_CompanyTransactionKind ObjSave);
        IEnumerable<St_CompanyTransactionKind> GetAllSt_CompanyTransactionKind(int CompanyID);
        int GetMaxSerial(int CompanyID);
        void Update(St_CompanyTransactionKind ObjUpdate);
        St_CompanyTransactionKind GetSt_CompanyTransactionKindByID(int CompanyID, int St_CompanyTransactionKindID);
        int GetSt_TransKindForSt_CompanyTransactionKind(int CompanyID, int St_CompanyTransactionKindID);
        int GetSt_CompanyTransactionKindBySt_TransKindNo(int CompanyID, int St_TransactionKindID);
        int GetSt_CompanyTransactionKind(int CompanyID, int St_TransactionKindID,string StockCode);
        St_CompanyTransactionKind GetSt_CompanyTransactionKindByIDAndStockCode(int CompanyID, int St_CompanyTransactionKindID,string StockCode);
    }
}