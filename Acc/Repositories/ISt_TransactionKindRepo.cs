using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ISt_TransactionKindRepo
    {
        IEnumerable<St_TransactionKind> GetAllSt_TransactionKind();
        St_TransactionKind GetSt_TransactionKindByID(int St_TransactionKindByID);
    }
}