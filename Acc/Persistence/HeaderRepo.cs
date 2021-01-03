using Acc.Models;
using Acc.Repositories;
using Acc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class HeaderRepo : IHeaderRepo
    {
        private readonly ApplicationDbContext _context;

        public HeaderRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(Header ObjToSave)
        {
            _context.Headers.Add(ObjToSave);
        }

        public void Delete(TransActionDeleteVM ObjToSave)
        {
            var ObjToUpdate = _context.Headers.FirstOrDefault(m => m.CompanyID == ObjToSave.CompanyID &&
                          m.CompanyTransactionKindNo == ObjToSave.CompanyTransactionKindNo
                          && m.VoucherNumber == ObjToSave.VoucherNumber && m.CompanyYear == ObjToSave.CompanyYear);
            if (ObjToUpdate != null)
            {

                _context.Headers.Remove(ObjToUpdate);

            }
        }


        public IEnumerable<Header> GetHeaderData(DateTime Fromdate, DateTime ToDate, int CompanyID, int CompanyYear)
        {
            return _context.Headers.Where(m => m.CompanyID == CompanyID && m.VoucherDate 
            >= Fromdate && m.VoucherDate <= ToDate && m.CompanyYear == CompanyYear).ToList();
        }

        public Header GetHeaderDataById(string VoucherNumber, int CompanyID, int CompanyTransactionKindNo, int TransactionKindNo, int CompanyYear)
        {

            return _context.Headers.FirstOrDefault(m => m.CompanyID == CompanyID &&
                                                   m.CompanyTransactionKindNo == CompanyTransactionKindNo
                                                   && m.VoucherNumber == VoucherNumber && m.TransactionKindNo == TransactionKindNo && m.CompanyYear == CompanyYear);
        }

        public Header GetHeaderDataByTransId(string VoucherNumber, int CompanyID, int CompanyTransactionKindNo, int TransactionKindNo, int CompanyYear)
        {

            return _context.Headers.FirstOrDefault(m => m.CompanyID == CompanyID &&
                                                   m.CompanyTransactionKindNo == CompanyTransactionKindNo
                                                   && m.VoucherNumber == VoucherNumber
                                                   && m.TransactionKindNo == TransactionKindNo
                                                   && m.CompanyYear == CompanyYear);
        }

        public int GetMaxVHByKind(int CompanyID, int CompanyTransactionKindNo, int TransactionKindNo, int CompanyYear)
        {
            var Obj = _context.Headers.Where(m => m.CompanyID == CompanyID &&
                                                         m.CompanyTransactionKindNo == CompanyTransactionKindNo && 
                                                         m.TransactionKindNo==TransactionKindNo && m.CompanyYear == CompanyYear).ToList();
            if (Obj.Count>0)
            {
         
                return Obj.Max(p => p.VHI)+1;


            }
            else
            {
                return 1;
            }
      
        }

        public void Update(Header ObjToSave)
        {
            var ObjToUpdate= _context.Headers.FirstOrDefault(m => m.CompanyID == ObjToSave.CompanyID &&
                           m.CompanyTransactionKindNo == ObjToSave.CompanyTransactionKindNo 
                           && m.VoucherNumber == ObjToSave.VoucherNumber && m.CompanyYear== ObjToSave.CompanyYear);
            if (ObjToUpdate != null)
            {
                if (ObjToUpdate.Exported == 0)
                {
                    ObjToUpdate.RowCount = ObjToSave.RowCount;
                    ObjToUpdate.ConversionFactor = ObjToSave.ConversionFactor;
                    ObjToUpdate.FCurrencyID = ObjToSave.FCurrencyID;
                    ObjToUpdate.Note = ObjToSave.Note;
                    ObjToUpdate.InsUserID = ObjToSave.InsUserID;
                    ObjToUpdate.RowCount = ObjToSave.RowCount;
                    ObjToUpdate.TotalCredit = ObjToSave.TotalCredit;
                    ObjToUpdate.TotalCreditForeign = ObjToSave.TotalCreditForeign;
                    ObjToUpdate.TotalDebit = ObjToSave.TotalDebit;
                    ObjToUpdate.TotalDebitForeign = ObjToSave.TotalDebitForeign;
                    ObjToUpdate.VoucherDate = ObjToSave.VoucherDate;
                    ObjToUpdate.SaleID = ObjToSave.SaleID;
                }

            }

        }

        public void UpdateToExportAndUnExport(Header ObjToUpdate)
        {
            var ObjUpdate = _context.Headers.FirstOrDefault(m => m.CompanyID == ObjToUpdate.CompanyID &&
                            m.CompanyTransactionKindNo == ObjToUpdate.CompanyTransactionKindNo && m.TransactionKindNo == ObjToUpdate.TransactionKindNo
                            && m.VoucherNumber == ObjToUpdate.VoucherNumber && m.CompanyYear == ObjToUpdate.CompanyYear);
            if (ObjUpdate != null)
            {
                ObjUpdate.Exported = ObjToUpdate.Exported;
            }
        }
    }
}