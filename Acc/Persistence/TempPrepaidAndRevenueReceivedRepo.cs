using Acc.Models;
using Acc.Repositories;
using Acc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Acc.Persistence
{
    public class TempPrepaidAndRevenueReceivedRepo : ITempPrepaidAndRevenueReceivedRepo
    {
        private readonly ApplicationDbContext _context;

        public TempPrepaidAndRevenueReceivedRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public int GetMaxVHByKind(int CompanyID, int CompanyTransactionKindNo, int TransactionKindNo , int CompanyYear)
        {
            var Obj = _context.TempPrepaidAndRevenueReceiveds.Where(m => m.CompanyID == CompanyID &&
                                                         m.CompanyTransactionKindNo == CompanyTransactionKindNo &&
                                                         m.TransactionKindNo == TransactionKindNo &&
                                                         m.CompanyYear == CompanyYear).ToList();
            if (Obj.Count > 0)
            {

                return Obj.Max(p => p.VHI) + 1;


            }
            else
            {
                return 1;
            }

        }
        public void Add(TempPrepaidAndRevenueReceived ObjToSave)
        {
            _context.TempPrepaidAndRevenueReceiveds.Add(ObjToSave);
        }
        public TempPrepaidAndRevenueReceived GetTempPrepaidAndRevenueReceivedData(string VoucherNumber, int CompanyID, int CompanyTransactionKindNo, int TransactionKindNo , int CompanyYear)
        {

            return _context.TempPrepaidAndRevenueReceiveds.FirstOrDefault(m => m.CompanyID == CompanyID &&
                                                   m.CompanyTransactionKindNo == CompanyTransactionKindNo
                                                   && m.VoucherNumber == VoucherNumber
                                                   && m.TransactionKindNo == TransactionKindNo
                                                   && m.CompanyYear == CompanyYear);
        }
        public void Delete(TempPrepaidAndRevenueReceivedVM ObjToDelete)
        {
            var ObjDelete = _context.TempPrepaidAndRevenueReceiveds.FirstOrDefault(m => m.CompanyID == ObjToDelete.CompanyID &&
                          m.CompanyTransactionKindNo == ObjToDelete.CompanyTransactionKindNo && m.TransactionKindNo == ObjToDelete.TransactionKindNo
                          && m.VoucherNumber == ObjToDelete.VoucherNumber && m.CompanyYear == ObjToDelete.CompanyYear);
            if (ObjDelete != null)
            {

                _context.TempPrepaidAndRevenueReceiveds.Remove(ObjDelete);

            }
        }
        public void Update(TempPrepaidAndRevenueReceived ObjToUpdate)
        {
            var ObjUpdate = _context.TempPrepaidAndRevenueReceiveds.FirstOrDefault(m => m.CompanyID == ObjToUpdate.CompanyID &&
                          m.CompanyTransactionKindNo == ObjToUpdate.CompanyTransactionKindNo && m.TransactionKindNo == ObjToUpdate.TransactionKindNo
                          && m.VoucherNumber == ObjToUpdate.VoucherNumber && m.CompanyYear == ObjToUpdate.CompanyYear);
            if (ObjUpdate != null)
            {
                ObjUpdate.VoucherDate = ObjToUpdate.VoucherDate;
                ObjUpdate.Total = ObjToUpdate.Total;
                ObjUpdate.Note = ObjToUpdate.Note;
                ObjUpdate.RowCount = ObjToUpdate.RowCount;
                ObjUpdate.NumberOfPayments = ObjToUpdate.NumberOfPayments;
                ObjUpdate.DateFirstPayment = ObjToUpdate.DateFirstPayment;
                ObjToUpdate.InsDateTime = ObjUpdate.InsDateTime;
                ObjToUpdate.InsUserID = ObjUpdate.InsUserID;
            }

        }
    }
}