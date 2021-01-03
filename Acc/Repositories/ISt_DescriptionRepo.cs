using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ISt_DescriptionRepo
    {

        void Update(St_Description ObjUpdate);
        St_Description GetSt_DescriptionByID(int CompanyID, int DescriptionID);
        IEnumerable<St_Description> GetAllSt_Description(int CompanyID);
        string GetSt_DescriptionNameByID(int CompanyID, int DescriptionID);

    }
}