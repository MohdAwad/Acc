using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ITransactionRepo
    {
        void Add(Transaction ObjToSave);
        void Update(Transaction ObjToSave);

        void MarkDeleteTrans(int CompanyID, string VoucherNumber, int CompanyTransactionKindNo, int TransactionKindNo, int CompanyYear );
        void UnMarkDeleteTrans(int CompanyID, string VoucherNumber, int CompanyTransactionKindNo, int TransactionKindNo, int CompanyYear);
        void DeleteMarkeTrans(int CompanyID, string VoucherNumber, int CompanyTransactionKindNo, int TransactionKindNo, int CompanyYear);
        IEnumerable<Transaction> GetTransactionsDetail(string VoucherNumber, int CompanyTransactionKindNo, int CompanyID, int TransactionKindNo);
        IEnumerable<Transaction> GetTransactionsDetailDebit(string VoucherNumber, int CompanyTransactionKindNo, int CompanyID,int TransactionKindNo);
        IEnumerable<Transaction> GetTransactionsDetailCredit(string VoucherNumber, int CompanyTransactionKindNo, int CompanyID, int TransactionKindNo);
        Transaction CheckAccountTrans(int CompanyID, string AccountNo);

    }
}