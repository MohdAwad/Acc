using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{

    public class St_OfferPriceRepo : ISt_OfferPriceRepo
    {

        private readonly ApplicationDbContext _context;

        public St_OfferPriceRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddSt_OfferPriceHeader(St_OfferPriceHeader ObjToSave)
        {
            _context.St_OfferPriceHeaders.Add(ObjToSave);
        }

        public void AddSt_OfferPriceTransaction(St_OfferPriceTransaction ObjToSave)
        {
            _context.St_OfferPriceTransactions.Add(ObjToSave);
        }

        public void DeleteSt_OfferPriceHeader(St_OfferPriceHeader ObjToDelete)
        {
            var ObjDelete = _context.St_OfferPriceHeaders.FirstOrDefault(m => m.CompanyID == ObjToDelete.CompanyID &&
                         m.CompanyTransactionKindNo == ObjToDelete.CompanyTransactionKindNo && m.TransactionKindNo == ObjToDelete.TransactionKindNo
                         && m.VoucherNumber == ObjToDelete.VoucherNumber && m.CompanyYear == ObjToDelete.CompanyYear);
            if (ObjDelete != null)
            {

                _context.St_OfferPriceHeaders.Remove(ObjDelete);
            }
        }

        public int GetMaxVoucher(int CompanyID, int CompanyTransactionKindNo, int TransactionKindNo, int CompanyYear)
        {
            var Obj = _context.St_OfferPriceHeaders.Where(m => m.CompanyID == CompanyID && m.CompanyTransactionKindNo == CompanyTransactionKindNo &&
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

        public St_OfferPriceHeader GetSt_OfferPriceHeaderDataById(string VoucherNumber, int CompanyID, int CompanyTransactionKindNo, int TransactionKindNo, int CompanyYear)
        {
            return _context.St_OfferPriceHeaders.FirstOrDefault(m => m.CompanyID == CompanyID &&
                                                              m.CompanyTransactionKindNo == CompanyTransactionKindNo
                                                              && m.VoucherNumber == VoucherNumber && m.TransactionKindNo == TransactionKindNo && m.CompanyYear == CompanyYear);
        }
    }
}