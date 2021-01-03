using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ISt_SubItemColorHRepo
    {
        St_SubItemColorH GetSt_SubItemColorHByID(int CompanyID, int SubItemColorCode);
        void Add(St_SubItemColorH ObjSave);
        void Update(St_SubItemColorH ObjUpdate);
        void Delete(St_SubItemColorH ObjDelete);
        int GetMaxSerial(int CompanyID);
    }
}