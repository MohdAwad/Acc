using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ITransactionKindRepo
    {
        IEnumerable<TransactionKind> GetAllTransactionKind();
        TransactionKind GetTransactionKindByID(int TransactionKindByID);
    }
}