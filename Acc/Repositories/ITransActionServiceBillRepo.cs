using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ITransActionServiceBillRepo
    {
        void Add(TransActionServiceBill ObjToSave);
        void Delete(int CompanyID, int VoucherNumber, int CompanyTransactionKindNo, int TransactionKindNo, int CompanyYear);
    }
}