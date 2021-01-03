using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class TransactionRepo : ITransactionRepo
    {
        private readonly ApplicationDbContext _context;

        public TransactionRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(Transaction ObjToSave)
        {
            _context.Transactions.Add(ObjToSave);
        }

        public Transaction CheckAccountTrans(int CompanyID, string AccountNo)
        {
            return _context.Transactions.FirstOrDefault(m => m.CompanyID == CompanyID && m.AccountNumber == AccountNo);
        }

        public IEnumerable<Transaction> GetTransactionsDetail(string VoucherNumber,int CompanyTransactionKindNo, int CompanyID, int TransactionKindNo)
        {
            if (Resources.Resource.CurLang == "Arb")
            {
                try
                {
                    return _context.Database.SqlQuery<Transaction>(
                    " select  T.CompanyID,T.CompanyYear,T.CompanyTransactionKindNo ,T.IsDeleted " +
                    " ,T.TransactionKindNo,T.VoucherNumber,T.RowNumber,T.VHI,T.VoucherDate,T.Debit,T.Credit," +
                    " T.DebitForeign,T.CreditForeign,CC.ArabicName As CostName " +
                    " ,T.AccountNumber,C.ArabicName as AccountName, T.CostCenter,T.Note,T.Exported, " +
                    "  T.InsUserID,T.InsDateTime,T.CreditDebitForeign " +
                    " from ChartOfAccounts C,Transactions T " +
                    " left join CostCenters CC  On " +
                    " CC.CompanyID = T.CompanyID " +
                    " And CC.CostNumber = T.CostCenter " +
                    " Where " +
                    " T.CompanyID=@CompanyID and  C.CompanyID=T.CompanyID and C.AccountNumber=T.AccountNumber " +
                    " and T.VoucherNumber=@VoucherNumber  and T.CompanyTransactionKindNo=@CompanyTransactionKindNo and T.TransactionKindNo=@TransactionKindNo "
                    , new SqlParameter("@CompanyID", CompanyID)
                    , new SqlParameter("@VoucherNumber", VoucherNumber)
                   , new SqlParameter("@CompanyTransactionKindNo", CompanyTransactionKindNo)
                   , new SqlParameter("@TransactionKindNo", TransactionKindNo)


                ).ToList();
                }
                catch (Exception ex)
                {
                    return new List<Transaction>();
                }
            }
            else {
                try
                {
                    return _context.Database.SqlQuery<Transaction>(
                    " select  T.CompanyID,T.CompanyYear,T.CompanyTransactionKindNo ,T.IsDeleted " +
                    " ,T.TransactionKindNo,T.VoucherNumber,T.RowNumber,T.VHI,T.VoucherDate,T.Debit,T.Credit," +
                    " T.DebitForeign,T.CreditForeign,CC.EnglishName As CostName " +
                    " ,T.AccountNumber,C.EnglishName as AccountName, T.CostCenter,T.Note,T.Exported, " +
                    "  T.InsUserID,T.InsDateTime,T.CreditDebitForeign " +
                    " from ChartOfAccounts C,Transactions T " +
                    " left join CostCenters CC  On " +
                    " CC.CompanyID = T.CompanyID " +
                    " And CC.CostNumber = T.CostCenter " +
                    " Where " +
                    " T.CompanyID=@CompanyID and  C.CompanyID=T.CompanyID and C.AccountNumber=T.AccountNumber " +
                    " and T.VoucherNumber=@VoucherNumber  and T.CompanyTransactionKindNo=@CompanyTransactionKindNo and T.TransactionKindNo=@TransactionKindNo "
                    , new SqlParameter("@CompanyID", CompanyID)
                    , new SqlParameter("@VoucherNumber", VoucherNumber)
                   , new SqlParameter("@CompanyTransactionKindNo", CompanyTransactionKindNo)
                   , new SqlParameter("@TransactionKindNo", TransactionKindNo)


                ).ToList();
                }
                catch (Exception ex)
                {
                    return new List<Transaction>();
                }
            }


        }
        public IEnumerable<Transaction> GetTransactionsDetailDebit(string VoucherNumber, int CompanyTransactionKindNo, int CompanyID, int TransactionKindNo)
        {
            return _context.Transactions.Where(m => m.CompanyID == CompanyID &&
            m.VoucherNumber == VoucherNumber && m.CompanyTransactionKindNo == CompanyTransactionKindNo &&
            m.TransactionKindNo == TransactionKindNo && m.Debit > 0).ToList();

        }
        public IEnumerable<Transaction> GetTransactionsDetailCredit(string VoucherNumber, int CompanyTransactionKindNo, int CompanyID, int TransactionKindNo)
        {
            return _context.Transactions.Where(m => m.CompanyID == CompanyID &&
            m.VoucherNumber == VoucherNumber && m.CompanyTransactionKindNo == CompanyTransactionKindNo &&
            m.TransactionKindNo == TransactionKindNo && m.Credit > 0).ToList();

        }
        public void Update(Transaction ObjToSave)
        {
            var ObjToUpdate = _context.Transactions.FirstOrDefault(m => m.CompanyID == ObjToSave.CompanyID &&
                            m.CompanyTransactionKindNo == ObjToSave.CompanyTransactionKindNo
                            && m.VoucherNumber == ObjToSave.VoucherNumber && m.CompanyYear == ObjToSave.CompanyYear && m.RowNumber == ObjToSave.RowNumber);
            if (ObjToUpdate != null)
            {
                if (ObjToUpdate.Exported == 0)
                {
                    ObjToUpdate.Note = ObjToSave.Note;
                    ObjToUpdate.InsUserID = ObjToSave.InsUserID;
                    ObjToUpdate.VoucherDate = ObjToSave.VoucherDate;
                    ObjToUpdate.AccountNumber = ObjToSave.AccountNumber;
                    ObjToUpdate.CostCenter = ObjToSave.CostCenter;
                    ObjToUpdate.Debit = ObjToSave.Debit;
                    ObjToUpdate.Credit = ObjToSave.Credit;
                    ObjToUpdate.DebitForeign = ObjToSave.DebitForeign;
                    ObjToUpdate.CreditForeign = ObjToSave.CreditForeign;
                    ObjToUpdate.CreditDebitForeign = ObjToSave.CreditDebitForeign;
                    ObjToUpdate.InsDateTime = ObjToSave.InsDateTime;
                }
            }

        }

        public void MarkDeleteTrans(int CompanyID, string VoucherNumber, int CompanyTransactionKindNo, int TransactionKindNo, int CompanyYear)
        {
            _context.Database.ExecuteSqlCommand(
               " update Transactions  set IsDeleted=1 " +
               " where CompanyID=@CompanyID and VoucherNumber=@VoucherNumber and  CompanyTransactionKindNo=@CompanyTransactionKindNo  " +
               "  And TransactionKindNo=@TransactionKindNo and CompanyYear=@CompanyYear "

               , new SqlParameter("@CompanyID", CompanyID)
               , new SqlParameter("@VoucherNumber", VoucherNumber)
                  , new SqlParameter("@CompanyTransactionKindNo", CompanyTransactionKindNo)
                  , new SqlParameter("@TransactionKindNo", TransactionKindNo)
                  , new SqlParameter("@CompanyYear", CompanyYear)


               );
        }

        public void UnMarkDeleteTrans(int CompanyID, string VoucherNumber, int CompanyTransactionKindNo, int TransactionKindNo, int CompanyYear)
        {
            _context.Database.ExecuteSqlCommand(
             " update Transactions  set IsDeleted=0 " +
             " where CompanyID=@CompanyID and VoucherNumber=@VoucherNumber and  CompanyTransactionKindNo=@CompanyTransactionKindNo  " +
             "  And TransactionKindNo=@TransactionKindNo and CompanyYear=@CompanyYear  and IsDeleted=1 "

             , new SqlParameter("@CompanyID", CompanyID)
             , new SqlParameter("@VoucherNumber", VoucherNumber)
                , new SqlParameter("@CompanyTransactionKindNo", CompanyTransactionKindNo)
                , new SqlParameter("@TransactionKindNo", TransactionKindNo)
                , new SqlParameter("@CompanyYear", CompanyYear)


             );
        }

        public void DeleteMarkeTrans(int CompanyID, string VoucherNumber, int CompanyTransactionKindNo, int TransactionKindNo, int CompanyYear)
        {
            _context.Database.ExecuteSqlCommand(
             " delete from Transactions    " +
             " where CompanyID=@CompanyID and VoucherNumber=@VoucherNumber and  CompanyTransactionKindNo=@CompanyTransactionKindNo  " +
             "  And TransactionKindNo=@TransactionKindNo and CompanyYear=@CompanyYear  and IsDeleted=1 "

             , new SqlParameter("@CompanyID", CompanyID)
             , new SqlParameter("@VoucherNumber", VoucherNumber)
                , new SqlParameter("@CompanyTransactionKindNo", CompanyTransactionKindNo)
                , new SqlParameter("@TransactionKindNo", TransactionKindNo)
                , new SqlParameter("@CompanyYear", CompanyYear)


             );
        }
    }
}