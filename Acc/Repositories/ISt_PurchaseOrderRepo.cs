using Acc.Models;
using Acc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ISt_PurchaseOrderRepo
    {
        void AddSt_PurchaseOrderHeader(St_PurchaseOrderHeader ObjToSave);
        void AddSt_PurchaseOrderTransaction(St_PurchaseOrderTransaction ObjToSave);
        int GetMaxVoucher(int CompanyID, int CompanyTransactionKindNo, int TransactionKindNo, int CompanyYear);
        St_PurchaseOrderHeader GetSt_PurchaseOrderHeaderDataById(string VoucherNumber, int CompanyID, int CompanyTransactionKindNo, int TransactionKindNo,int CompanyYear);
        void DeleteSt_PurchaseOrderHeader(St_PurchaseOrderHeader ObjToDelete);
    }
}