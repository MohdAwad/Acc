using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ISupplierInformationRepo
    {
      void  Add(SupplierInformation ObjToSave);
        void Delete(SupplierInformation ObjToSave);

        SupplierInformation GetSupplierInformation(int CompanyID, string AccountNo);
    }
}