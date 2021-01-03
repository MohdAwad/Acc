using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ICustomerInformationRepo
    {
        void Add(CustomerInformation ObjToSave);
        void Delete(CustomerInformation ObjToSave);
        void Update(CustomerInformation ObjToSave);

        CustomerInformation GetCustomerInformation(int CompanyID,string AccountNo);

        IEnumerable<CustomerInformation> GetCustByCityAreaAccount(int CompanyID,int CityId,int AreadID);


    }
}