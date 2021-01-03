using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ISt_DriverRepo
    {

        IEnumerable<St_Driver> GetAllDriver(int CompanyID);
        St_Driver GetSt_DriverByID(int CompanyID, int DriverID);
        void Add(St_Driver ObjSave);
        void Update(St_Driver ObjUpdate);
        void Delete(St_Driver ObjDelete);
        int GetMaxSerial(int CompanyID);

    }
}