using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ISt_BranchHRepo
    {
        St_BranchH GetSt_BranchrByID(int CompanyID, string BranchrCode);
        void Add(St_BranchH ObjSave);
        void Update(St_BranchH ObjUpdate);
        void Delete(St_BranchH ObjDelete);
        string CheckIfBranchCodeExisting(int CompanyID, string BranchCode);
        IEnumerable<St_BranchH> GetAllSt_BranchH(int CompanyID);
    }
}