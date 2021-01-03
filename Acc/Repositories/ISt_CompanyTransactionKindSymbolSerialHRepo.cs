using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ISt_CompanyTransactionKindSymbolSerialHRepo
    {
        void Add(St_CompanyTransactionKindSymbolSerialH ObjSave);
        int GetMaxSerial(int CompanyID, int St_CompanyTransactionKindID, string StockCode);
        void Update(int CompanyID, int St_CompanyTransactionKindID, string StockCode);
        void Delete(int CompanyID, int St_CompanyTransactionKindID);
        St_CompanyTransactionKindSymbolSerialH CheckIfSave(int CompanyID, int St_CompanyTransactionKindID);
    }
}