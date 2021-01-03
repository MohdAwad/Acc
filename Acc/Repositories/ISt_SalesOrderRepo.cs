using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ISt_SalesOrderRepo
    {
        void AddSt_SalesOrderHeader(St_SalesOrderHeader ObjToSave);
        void AddSt_SalesOrderTransaction(St_SalesOrderTransaction ObjToSave);
        int GetMaxVoucher(int CompanyID, int CompanyTransactionKindNo, int TransactionKindNo, int CompanyYear);
        St_SalesOrderHeader GetSt_SalesOrderHeaderDataById(string VoucherNumber, int CompanyID, int CompanyTransactionKindNo, int TransactionKindNo, int CompanyYear);
        void DeleteSt_SalesOrderHeader(St_SalesOrderHeader ObjToDelete);
    }
}