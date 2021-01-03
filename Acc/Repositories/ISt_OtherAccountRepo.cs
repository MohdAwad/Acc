using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ISt_OtherAccountRepo
    {
        void Add(St_OtherAccount ObjSave);
        St_OtherAccount GetSt_OtherAccountByID(int CompanyID);
        void Delete(St_OtherAccount ObjDelete);
    }
}