using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ISt_ItemGroupHRepo
    {
        IEnumerable<St_ItemGroupH> GetAllSt_ItemGroupH(int CompanyID);
        St_ItemGroupH GetSt_ItemGroupHByID(int CompanyID, string GroupCode);
        void Add(St_ItemGroupH ObjSave);
        void Update(St_ItemGroupH ObjUpdate);
        void Delete(St_ItemGroupH ObjDelete);
        string CheckIfGroupCodeExisting(int CompanyID, string GroupCode);
    }
}