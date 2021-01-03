using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ISt_SectionsOfFactoryHRepo
    {
        void Add(St_SectionsOfFactoryH ObjSave);
        void Update(St_SectionsOfFactoryH ObjUpdate);
        void Delete(St_SectionsOfFactoryH ObjDelete);
        int GetMaxSerial(int CompanyID);
        IEnumerable<St_SectionsOfFactoryH> GetAllSt_SectionsOfFactory(int CompanyID);
        IEnumerable<St_SectionsOfFactoryH> GetAllSt_SectionsOfFactoryBySt_FactoryID(int CompanyID, int FactoryID);
    }
}