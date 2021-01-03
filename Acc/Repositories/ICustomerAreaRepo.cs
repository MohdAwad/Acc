using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ICustomerAreaRepo
    {
        void Add(CustomerArea ObjSave);
        void Update(CustomerArea ObjUpdate);
        void Delete(CustomerArea ObjDelete);
        int GetMaxSerial(int CompanyID);
        IEnumerable<CustomerArea> GetAllCustomerArea(int CompanyID);
        IEnumerable<CustomerArea> GetAllCustomerAreaByCityID(int CompanyID,int CustomerCityID);
    }
}