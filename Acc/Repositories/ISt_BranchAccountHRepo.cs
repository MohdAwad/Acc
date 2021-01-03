using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ISt_BranchAccountHRepo
    {
        void Add(St_BranchAccountH ObjSave);
        void AddBranchExpenseAccountH(St_BranchExpenseAccountH ObjSave);
        void AddBranchOtherExpenseAccountH(St_BranchOtherExpenseAccountH ObjSave);
        void AddSt_BranchExpenseAccount(St_BranchExpenseAccountH ObjSave);
        St_BranchAccountH GetSt_BranchAccountHByID(int CompanyID, string BranchCode);
        void Delete(St_BranchAccountH ObjDelete);
    }
}