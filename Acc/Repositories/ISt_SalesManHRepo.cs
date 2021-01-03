using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ISt_SalesManHRepo
    {
        void Add(St_SalesManH ObjSave);

        void Update(St_SalesManH ObjUpdate);
        St_SalesManH GetSt_SalesManHByID(int CompanyID, int SalesManID);
        IEnumerable<St_SalesManH> GetAllSt_SalesManH(int CompanyID);

        void Delete(St_SalesManH ObjDelete);

        int GetMaxSerial(int CompanyID);
    }
}