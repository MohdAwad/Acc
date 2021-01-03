using Acc.Models;
using Acc.Repositories;
using Acc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class TempPrepaidAndRevenueReceivedDetailsRepo : ITempPrepaidAndRevenueReceivedDetailsRepo
    {
        private readonly ApplicationDbContext _context;

        public TempPrepaidAndRevenueReceivedDetailsRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(TempPrepaidAndRevenueReceivedDetail ObjToSave)
        {
            _context.TempPrepaidAndRevenueReceivedDetails.Add(ObjToSave);
        }
        public void UpdateToExport(TempPrepaidAndRevenueReceivedDetail ObjToUpdate)
        {
            var ObjUpdate = _context.TempPrepaidAndRevenueReceivedDetails.FirstOrDefault(m => m.CompanyID == ObjToUpdate.CompanyID &&
                            m.TransactionKindNo == ObjToUpdate.TransactionKindNo  && 
                            m.VoucherNumber == ObjToUpdate.VoucherNumber && m.CompanyYear == ObjToUpdate.CompanyYear &&
                            m.RowNumber == ObjToUpdate.RowNumber);
            if (ObjUpdate != null)
            {
                ObjUpdate.Exported = ObjToUpdate.Exported;
            }
        }
        public TempPrepaidAndRevenueReceivedDetail GetTempPrepaidAndRevenueReceivedDetailData(string VoucherNumber, int CompanyID, int CompanyTransactionKindNo, int TransactionKindNo , int CompanyYear)
        {

            return _context.TempPrepaidAndRevenueReceivedDetails.FirstOrDefault(m => m.CompanyID == CompanyID &&
                                                   m.CompanyTransactionKindNo == CompanyTransactionKindNo
                                                   && m.VoucherNumber == VoucherNumber
                                                   && m.TransactionKindNo == TransactionKindNo
                                                   && m.RowNumber == 1
                                                   && m.CompanyYear == CompanyYear);
        }
    }
}