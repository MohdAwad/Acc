using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface IServiceGroupRepo
    {
        IEnumerable<ServiceGroup> GetAllServiceGroup(int CompanyID);
        ServiceGroup GetServiceGroupByID(int CompanyID, int ServiceGroupID);
        void Add(ServiceGroup ObjSave);
        void Update(ServiceGroup ObjUpdate);
        void Delete(ServiceGroup ObjDelete);
        int GetMaxSerial(int CompanyID);
    }
}