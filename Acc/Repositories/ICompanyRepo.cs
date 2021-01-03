using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ICompanyRepo
    {
        Company GetCompanyByUserID(string UserId);
        void Add(Company ObjTOSave);
        void Update(Company ObjToSave);
        Company GetMyCompany(int CompanyID);
    }
}