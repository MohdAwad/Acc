using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class St_SalesOrderRepo : ISt_SalesOrderRepo
    {

        private readonly ApplicationDbContext _context;

        public St_SalesOrderRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddSt_SalesOrderHeader(St_SalesOrderHeader ObjToSave)
        {
            _context.St_SalesOrderHeaders.Add(ObjToSave);
        }

        public void AddSt_SalesOrderTransaction(St_SalesOrderTransaction ObjToSave)
        {
            _context.St_SalesOrderTransactions.Add(ObjToSave);
        }

        public void DeleteSt_SalesOrderHeader(St_SalesOrderHeader ObjToDelete)
        {
            var ObjDelete = _context.St_SalesOrderHeaders.FirstOrDefault(m => m.CompanyID == ObjToDelete.CompanyID &&
                                     m.CompanyTransactionKindNo == ObjToDelete.CompanyTransactionKindNo && m.TransactionKindNo == ObjToDelete.TransactionKindNo
                                     && m.VoucherNumber == ObjToDelete.VoucherNumber && m.CompanyYear == ObjToDelete.CompanyYear);
            if (ObjDelete != null)
            {

                _context.St_SalesOrderHeaders.Remove(ObjDelete);
            }
        }

        public int GetMaxVoucher(int CompanyID, int CompanyTransactionKindNo, int TransactionKindNo, int CompanyYear)
        {
            var Obj = _context.St_SalesOrderHeaders.Where(m => m.CompanyID == CompanyID && m.CompanyTransactionKindNo == CompanyTransactionKindNo &&
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

        public St_SalesOrderHeader GetSt_SalesOrderHeaderDataById(string VoucherNumber, int CompanyID, int CompanyTransactionKindNo, int TransactionKindNo, int CompanyYear)
        {
            return _context.St_SalesOrderHeaders.FirstOrDefault(m => m.CompanyID == CompanyID &&
                                                              m.CompanyTransactionKindNo == CompanyTransactionKindNo
                                                              && m.VoucherNumber == VoucherNumber && m.TransactionKindNo == TransactionKindNo && m.CompanyYear == CompanyYear);
        }
    }
}