using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ISt_TransActionFileRepo 
    {

        void Add(St_TransActionFile ObjToSave);

        void Update(St_TransActionFile ObjToUpdate);


        void Delete(int id);

        IEnumerable<St_TransActionFile> GetAllTransactionFile(int year, string VoucherNumber, string TransactionKindNo, string CompanyTransactionKindNo);

        IEnumerable<St_TransActionFile> GetAllTransactionFileBank(int year, string VoucherNumber, string TransactionKindNo, string CompanyTransactionKindNo, int CompanyId);

        IEnumerable<St_TransActionFile> GetType(int CompanyID);

        St_TransActionFile GetTransActionFilebyId(int id);

        int GetMaxSerial(int CompanyID);
    }
}