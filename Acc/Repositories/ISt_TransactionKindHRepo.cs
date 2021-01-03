using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Acc.Repositories
{
    public interface ISt_TransactionKindHRepo
    {
        IEnumerable<St_TransactionKindH> GetAllSt_TransactionKindH();
        St_TransactionKindH GetSt_TransactionKindHByID(int St_TransactionKindByID);
    }
}