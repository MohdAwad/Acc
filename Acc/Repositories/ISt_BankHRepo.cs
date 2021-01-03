using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ISt_BankHRepo
    {
        void Add(St_BankH ObjSave);
        void Update(St_BankH ObjUpdate);
        void Delete(St_BankH ObjDelete);
        int GetMaxSerial(int CompanyID);
        IEnumerable<St_BankH> GetAllSt_BankH(int CompanyID);
        St_BankH GetAllSt_BankHByID(int CompanyID, int BankID);
    }
}