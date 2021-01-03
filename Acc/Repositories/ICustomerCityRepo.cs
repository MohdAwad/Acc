using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ICustomerCityRepo
    {
        IEnumerable<CustomerCity> GetAllCustomerCity(int CompanyID);
        CustomerCity GetCustomerCityByID(int CompanyID, int CustomerCityID);
        void Add(CustomerCity ObjSave);
        void Update(CustomerCity ObjUpdate);
        void Delete(CustomerCity ObjDelete);
        int GetMaxSerial(int CompanyID);
    }
}