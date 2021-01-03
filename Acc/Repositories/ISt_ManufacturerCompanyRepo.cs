using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ISt_ManufacturerCompanyRepo
    {
        IEnumerable<St_ManufacturerCompany> GetAllSt_ManufacturerCompany(int CompanyID);
        St_ManufacturerCompany GetSt_ManufacturerCompanyByID(int CompanyID, int ManufacturerCompanyID);
        void Add(St_ManufacturerCompany ObjSave);
        void Update(St_ManufacturerCompany ObjUpdate);
        void Delete(St_ManufacturerCompany ObjDelete);
        int GetMaxSerial(int CompanyID);
    }
}