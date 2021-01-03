using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ISt_CarpenterHRepo
    {
        void Add(St_CarpenterH ObjSave);

        void Update(St_CarpenterH ObjUpdate);
        St_CarpenterH GetSt_CarpenterHByID(int CompanyID, int CarpenterID);
        IEnumerable<St_CarpenterH> GetAllSt_CarpenterH(int CompanyID);

        void Delete(St_CarpenterH ObjDelete);

        int GetMaxSerial(int CompanyID);

    }
}