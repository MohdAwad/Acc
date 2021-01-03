using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ISt_ExtraExpenseRepo
    {
        void Update(St_ExtraExpense ObjUpdate);
        St_ExtraExpense GetSt_ExtraExpenseByID(int CompanyID, int ExtraExpenseID);
        IEnumerable<St_ExtraExpense> GetAllSt_ExtraExpense(int CompanyID);
        string GetSt_ExtraExpenseNameByID(int CompanyID, int ExtraExpenseID);
    }
}