using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ISt_HeaderRepo
    {
        void AddHeader(St_Header ObjSave);
        void AddTransaction(St_Transaction ObjSave);
        void UpdateRowCountInitialInventory(St_Header ObjUpdate);
        void UpdateInitialInventory(St_Header ObjUpdate);
        int GetMaxRowNumberInitialInventory(int CompanyID);
        St_Header CheckIfInitialInventoryExsitInHeader(int CompanyID);
        St_Transaction CheckIfInitialInventoryExsitInTransaction(int CompanyID, string ItemCode, string StockCode);
        void UpdateInitialInventoryInTransaction(St_Transaction ObjUpdate);
        int GetMaxVoucher(int CompanyID, int CompanyTransactionKindNo, int TransactionKindNo, int CompanyYear, string StockCode);
        St_Header GetSt_HeaderDataById(string VoucherNumber, int CompanyID, int CompanyTransactionKindNo, int TransactionKindNo, int CompanyYear,string StockCode);
        void DeleteHeader(St_Header ObjToDelete);
        void UpdateToExportAndUnExport(St_Header ObjToUpdate);
    }
}