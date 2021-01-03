using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ISt_CountryOfOriginRepo
    {
        IEnumerable<St_CountryOfOrigin> GetAllSt_CountryOfOrigin(int CompanyID);
        St_CountryOfOrigin GetSt_CountryOfOriginByID(int CompanyID, int CountryOfOriginID);
        void Add(St_CountryOfOrigin ObjSave);
        void Update(St_CountryOfOrigin ObjUpdate);
        void Delete(St_CountryOfOrigin ObjDelete);
        int GetMaxSerial(int CompanyID);
    }
}