using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class St_HeaderRepo : ISt_HeaderRepo
    {
        private readonly ApplicationDbContext _context;
        public St_HeaderRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public void AddHeader(St_Header ObjSave)
        {
            _context.St_Headers.Add(ObjSave);
        }
        public void AddTransaction(St_Transaction ObjSave)
        {
            _context.St_Transactions.Add(ObjSave);
        }
        public void UpdateRowCountInitialInventory(St_Header ObjUpdate)
        {
            var ObjToUpdate = _context.St_Headers.SingleOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.TransactionKindNo == 517);
            if (ObjToUpdate != null)
            {
                ObjToUpdate.DueDate = ObjUpdate.DueDate;
                ObjToUpdate.RowCount = ObjUpdate.RowCount;
                ObjToUpdate.InsDateTime = ObjUpdate.InsDateTime;
                ObjToUpdate.InsUserID = ObjUpdate.InsUserID;

            }
        }
        public void UpdateInitialInventory(St_Header ObjUpdate)
        {
            var ObjToUpdate = _context.St_Headers.SingleOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.TransactionKindNo == 517);
            if (ObjToUpdate != null)
            {
                ObjToUpdate.DueDate = ObjUpdate.DueDate;
                ObjToUpdate.InsDateTime = ObjUpdate.InsDateTime;
                ObjToUpdate.InsUserID = ObjUpdate.InsUserID;

            }
        }
        public int GetMaxRowNumberInitialInventory(int CompanyID)
        {
            var Obj = _context.St_Transactions.FirstOrDefault(m => m.CompanyID == CompanyID && m.TransactionKindNo == 517);
            if (Obj == null)
            {
                return 1;
            }
            else
            {
                return _context.St_Transactions.Where(m => m.CompanyID == CompanyID && m.TransactionKindNo == 517).Max(p => p.RowNumber) + 1;
            }
        }
        public St_Header CheckIfInitialInventoryExsitInHeader(int CompanyID)
        {
            return _context.St_Headers.FirstOrDefault(m => m.CompanyID == CompanyID && m.TransactionKindNo == 517);
        }
        public St_Transaction CheckIfInitialInventoryExsitInTransaction(int CompanyID,string ItemCode,string StockCode)
        {
            return _context.St_Transactions.FirstOrDefault(m => m.CompanyID == CompanyID && m.TransactionKindNo == 517 && m.ItemCode == ItemCode && m.StockCode == StockCode);
        }
        public void UpdateInitialInventoryInTransaction(St_Transaction ObjUpdate)
        {
            var ObjToUpdate = _context.St_Transactions.SingleOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.TransactionKindNo == 517 && m.ItemCode == ObjUpdate.ItemCode && m.StockCode == ObjUpdate.StockCode);
            if (ObjToUpdate != null)
            {
                ObjToUpdate.Quantity = ObjUpdate.Quantity;
                ObjToUpdate.QuantityInputOutput = ObjUpdate.QuantityInputOutput;
                ObjToUpdate.TotalLocalBeforDiscount = ObjUpdate.TotalLocalBeforDiscount;
                ObjToUpdate.TotalLocalAfterDiscount = ObjUpdate.TotalLocalAfterDiscount;
                ObjToUpdate.TotalLocal = ObjUpdate.TotalLocal;
                ObjToUpdate.TotalCostLocal = ObjUpdate.TotalCostLocal;
                ObjToUpdate.PricePieceLocalBeforDiscount = ObjUpdate.PricePieceLocalBeforDiscount;
                ObjToUpdate.PricePieceLocalAfterDiscount = ObjUpdate.PricePieceLocalAfterDiscount;
                ObjToUpdate.PricePieceTotalLocal = ObjUpdate.PricePieceTotalLocal;
                ObjToUpdate.CostPieceLocal = ObjUpdate.CostPieceLocal;
                ObjToUpdate.InsDateTime = ObjUpdate.InsDateTime;
                ObjToUpdate.InsUserID = ObjUpdate.InsUserID;

            }
        }
        public int GetMaxVoucher(int CompanyID, int CompanyTransactionKindNo, int TransactionKindNo, int CompanyYear,string StockCode)
        {
            var Obj = _context.St_Headers.Where(m => m.CompanyID == CompanyID && m.CompanyTransactionKindNo == CompanyTransactionKindNo &&
            m.TransactionKindNo == TransactionKindNo && m.CompanyYear == CompanyYear && m.StockCode == StockCode).ToList();
            if (Obj.Count > 0)
            {
                return Obj.Max(p => p.VHI) + 1;
            }
            else
            {
                return 1;
            }

        }
        public St_Header GetSt_HeaderDataById(string VoucherNumber, int CompanyID, int CompanyTransactionKindNo, int TransactionKindNo, int CompanyYear, string StockCode)
        {

            return _context.St_Headers.FirstOrDefault(m => m.CompanyID == CompanyID &&
                                                   m.CompanyTransactionKindNo == CompanyTransactionKindNo && m.StockCode == StockCode
                                                   && m.VoucherNumber == VoucherNumber && m.TransactionKindNo == TransactionKindNo && m.CompanyYear == CompanyYear);
        }
        public void DeleteHeader(St_Header ObjToDelete)
        {
            var ObjDelete = _context.St_Headers.FirstOrDefault(m => m.CompanyID == ObjToDelete.CompanyID &&
                          m.CompanyTransactionKindNo == ObjToDelete.CompanyTransactionKindNo && m.StockCode == ObjToDelete.StockCode
                          && m.VoucherNumber == ObjToDelete.VoucherNumber && m.CompanyYear == ObjToDelete.CompanyYear);
            if (ObjDelete != null)
            {

                _context.St_Headers.Remove(ObjDelete);

            }
        }
        public void UpdateToExportAndUnExport(St_Header ObjToUpdate)
        {
            if (ObjToUpdate.TransactionKindNo == 507)
            {
                var ObjUpdate = _context.St_Headers.FirstOrDefault(m => m.CompanyID == ObjToUpdate.CompanyID &&
                            m.CompanyTransactionKindNo == ObjToUpdate.CompanyTransactionKindNo && m.TransactionKindNo == ObjToUpdate.TransactionKindNo
                            && m.VoucherNumber == ObjToUpdate.VoucherNumber && m.CompanyYear == ObjToUpdate.CompanyYear);
                if (ObjUpdate != null)
                {
                    ObjUpdate.Exported = ObjToUpdate.Exported;
                }
            }
            else
            {
                var ObjUpdate = _context.St_Headers.FirstOrDefault(m => m.CompanyID == ObjToUpdate.CompanyID &&
                            m.CompanyTransactionKindNo == ObjToUpdate.CompanyTransactionKindNo && m.TransactionKindNo == ObjToUpdate.TransactionKindNo
                            && m.VoucherNumber == ObjToUpdate.VoucherNumber && m.CompanyYear == ObjToUpdate.CompanyYear && m.StockCode == ObjToUpdate.StockCode);
                if (ObjUpdate != null)
                {
                    ObjUpdate.Exported = ObjToUpdate.Exported;
                }
            }
            
        }

    }
}