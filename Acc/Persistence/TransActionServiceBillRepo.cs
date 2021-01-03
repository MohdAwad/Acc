using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class TransActionServiceBillRepo : ITransActionServiceBillRepo
    {
        private readonly ApplicationDbContext _context;
        public TransActionServiceBillRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(TransActionServiceBill ObjToSave)
        {
            _context.TransActionServiceBills.Add(ObjToSave);
        }

        public void Delete(int CompanyID, int VoucherNumber, int CompanyTransactionKindNo, int TransactionKindNo, int CompanyYear)
        {
            _context.Database.ExecuteSqlCommand(
                 " Delete  From TransActionServiceBills " +
                 " where CompanyID=@CompanyID and BillID=@VoucherNumber and  CompanyTransactionKindNo=@CompanyTransactionKindNo  " +
                 "  And TransactionKindNo=@TransactionKindNo and CompanyYear=@CompanyYear "

                 , new SqlParameter("@CompanyID", CompanyID)
                 , new SqlParameter("@VoucherNumber", VoucherNumber)
                 , new SqlParameter("@CompanyTransactionKindNo", CompanyTransactionKindNo)
                  , new SqlParameter("@TransactionKindNo", TransactionKindNo)
                  , new SqlParameter("@CompanyYear", CompanyYear)


                 );
        }
    }
}