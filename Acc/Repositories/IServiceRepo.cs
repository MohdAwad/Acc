using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface IServiceRepo
    {
        void Add(Service ObjSave);
        IEnumerable<Service> GetAllService(int CompanyID);
        void Update(Service ObjUpdate);
        void Delete(Service ObjDelete);
        Service GetServiceByID(int CompanyID, int ServiceID);
        int GetMaxSerial(int CompanyID);
    }
}