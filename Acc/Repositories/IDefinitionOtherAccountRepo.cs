using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface IDefinitionOtherAccountRepo
    {
        void Add(OtherAccount ObjSave);
        OtherAccount GetDefinitionOtherAccountByID(int CompanyID);
        void Delete(OtherAccount ObjDelete);
    }
}