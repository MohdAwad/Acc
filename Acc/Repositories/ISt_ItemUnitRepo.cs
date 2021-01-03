using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ISt_ItemUnitRepo
    {
        IEnumerable<St_ItemUnit> GetAllItemUnit(int CompanyID);
        St_ItemUnit GetItemUnitByID(int CompanyID, int ItemUnitCode);
        void Add(St_ItemUnit ObjSave);
        void Update(St_ItemUnit ObjUpdate);
        void Delete(St_ItemUnit ObjDelete);
        int GetMaxSerial(int CompanyID);
    }
}