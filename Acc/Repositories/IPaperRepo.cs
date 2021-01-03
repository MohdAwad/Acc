using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface IPaperRepo
    {
        void Add(Paper ObjToSave);
        IEnumerable<Paper> GetPaperDetail(string VoucherNumber, int CompanyTransactionKindNo, int CompanyID, int TransactionKindNo,int CompanyYear);
    }
}