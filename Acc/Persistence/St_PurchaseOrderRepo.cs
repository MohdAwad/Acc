using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class St_PurchaseOrderRepo : ISt_PurchaseOrderRepo
    {
        private readonly ApplicationDbContext _context;

        public St_PurchaseOrderRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddSt_PurchaseOrderHeader(St_PurchaseOrderHeader ObjToSave)
        {
            _context.St_PurchaseOrderHeaders.Add(ObjToSave);
        }
        public void AddSt_PurchaseOrderTransaction(St_PurchaseOrderTransaction ObjToSave)
        {
            _context.St_PurchaseOrderTransactions.Add(ObjToSave);
        }
        public int GetMaxVoucher(int CompanyID, int CompanyTransactionKindNo, int TransactionKindNo,int CompanyYear)
        {
            var Obj = _context.St_PurchaseOrderHeaders.Where(m => m.CompanyID == CompanyID && m.CompanyTransactionKindNo == CompanyTransactionKindNo &&
            m.TransactionKindNo == TransactionKindNo && m.CompanyYear == CompanyYear).ToList();
            if (Obj.Count > 0)
            {
                return Obj.Max(p => p.VHI) + 1;
            }
            else
            {
                return 1;
            }

        }
        public St_PurchaseOrderHeader GetSt_PurchaseOrderHeaderDataById(string VoucherNumber, int CompanyID, int CompanyTransactionKindNo, int TransactionKindNo,int CompanyYear)
        {

            return _context.St_PurchaseOrderHeaders.FirstOrDefault(m => m.CompanyID == CompanyID &&
                                                   m.CompanyTransactionKindNo == CompanyTransactionKindNo
                                                   && m.VoucherNumber == VoucherNumber && m.TransactionKindNo == TransactionKindNo && m.CompanyYear == CompanyYear);
        }
        public void DeleteSt_PurchaseOrderHeader(St_PurchaseOrderHeader ObjToDelete)
        {
            var ObjDelete = _context.St_PurchaseOrderHeaders.FirstOrDefault(m => m.CompanyID == ObjToDelete.CompanyID &&
                          m.CompanyTransactionKindNo == ObjToDelete.CompanyTransactionKindNo && m.TransactionKindNo == ObjToDelete.TransactionKindNo
                          && m.VoucherNumber == ObjToDelete.VoucherNumber && m.CompanyYear == ObjToDelete.CompanyYear );
            if (ObjDelete != null)
            {

                _context.St_PurchaseOrderHeaders.Remove(ObjDelete);

            }
        }
    }
}