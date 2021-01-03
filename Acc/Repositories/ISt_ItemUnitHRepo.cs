using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ISt_ItemUnitHRepo
    {
        IEnumerable<St_ItemUnitH> GetAllSt_ItemUnitH(int CompanyID);
        St_ItemUnitH GetSt_ItemUnitHByID(int CompanyID, int ItemUnitCode);
        void Add(St_ItemUnitH ObjSave);
        void Update(St_ItemUnitH ObjUpdate);
        void Delete(St_ItemUnitH ObjDelete);
        int GetMaxSerial(int CompanyID);
    }
}