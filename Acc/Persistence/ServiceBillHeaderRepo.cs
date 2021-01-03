using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class ServiceBillHeaderRepo : IServiceBillHeaderRepo
    {
        private readonly ApplicationDbContext _context;
        public ServiceBillHeaderRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(HeaderServiceBill ObjToSave)
        {
            _context.HeaderServiceBills.Add(ObjToSave);
        }

        public void Delete(int CompanyID, int VoucherNumber, int CompanyTransactionKindNo, int TransactionKindNo, int CompanyYear)
        {
            _context.Database.ExecuteSqlCommand(
                " Delete  From HeaderServiceBills " +
                " where CompanyID=@CompanyID and BillID=@VoucherNumber and  CompanyTransactionKindNo=@CompanyTransactionKindNo  " +
                "  And TransactionKindNo=@TransactionKindNo and CompanyYear=@CompanyYear "

                , new SqlParameter("@CompanyID", CompanyID)
                , new SqlParameter("@VoucherNumber", VoucherNumber)
                , new SqlParameter("@CompanyTransactionKindNo", CompanyTransactionKindNo)
                 , new SqlParameter("@TransactionKindNo", TransactionKindNo)
                 , new SqlParameter("@CompanyYear", CompanyYear)


                );
        }

        public IEnumerable<HeaderServiceBill> GetHeaderData(DateTime Fromdate, DateTime ToDate, int CompanyID, int ServiceType)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<HeaderServiceBill> GetHeaderData(DateTime Fromdate, DateTime ToDate, int CompanyID, int ServiceType, int CompanyYear)
        {
            throw new NotImplementedException();
        }

       
        public HeaderServiceBill GetHeaderDataById( int CompanyID, int VoucherNumber, int CompanyTransactionKindNo, int TransactionKindNo, int CompanyYear)
        {
            return _context.HeaderServiceBills.FirstOrDefault(m =>m.BillID== VoucherNumber &&  m.CompanyID == CompanyID && m.TransactionKindNo == TransactionKindNo && m.CompanyYear == CompanyYear);
        }

        public int GetMaxVHByKind(int CompanyID,   int TransactionKindNo, int CompanyYear)
        {
            var Obj = _context.HeaderServiceBills.FirstOrDefault(m => m.CompanyID == CompanyID  && m.TransactionKindNo == TransactionKindNo && m.CompanyYear==CompanyYear);
            if (Obj == null)
            {
                return 1;
            }
            else
            {
                return _context.HeaderServiceBills.Where(m => m.CompanyID == CompanyID && m.TransactionKindNo == TransactionKindNo && m.CompanyYear == CompanyYear).Max(p => p.BillID) + 1;
            }
        }

        public void Update(HeaderServiceBill ObjToSave)
        {
            throw new NotImplementedException();
        }

        public void UpdateToExportAndUnExport(HeaderServiceBill ObjToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}