using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ISt_OtherAccountHRepo
    {
        void Add(St_OtherAccountH ObjSave);
        St_OtherAccountH GetSt_OtherAccountHByID(int CompanyID);
        void Delete(St_OtherAccountH ObjDelete);
    }
}