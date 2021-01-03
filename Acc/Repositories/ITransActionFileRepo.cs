using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ITransActionFileRepo
    {
        void Add(TransActionFile ObjToSave);

        void Update(TransActionFile ObjToUpdate);

        
        void Delete(int id);

        IEnumerable<TransActionFile> GetAllTransactionFile(int year, string VoucherNumber, string TransactionKindNo, string CompanyTransactionKindNo);

        IEnumerable<TransActionFile> GetAllTransactionFileBank(int year, string VoucherNumber, string TransactionKindNo, string CompanyTransactionKindNo , int CompanyId);

        IEnumerable<TransActionFile> GetType(int CompanyID);

        TransActionFile GetTransActionFilebyId(int id);

        int GetMaxSerial(int CompanyID);

    }
}