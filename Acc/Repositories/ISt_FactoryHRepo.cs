using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ISt_FactoryHRepo
    {
        IEnumerable<St_FactoryH> GetAllSt_FactoryH(int CompanyID);
        St_FactoryH GetSt_FactoryHByID(int CompanyID, int FactoryID);
        void Add(St_FactoryH ObjSave);
        void Update(St_FactoryH ObjUpdate);
        void Delete(St_FactoryH ObjDelete);
        int GetMaxSerial(int CompanyID);
    }
}