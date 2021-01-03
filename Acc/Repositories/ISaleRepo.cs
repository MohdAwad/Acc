using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ISaleRepo
    {
        IEnumerable<Sale> GetAllSale(int CompanyID);
        Sale GetSaleByID(int CompanyID, int SalesID);
        void Add(Sale ObjSave);
        void Update(Sale ObjUpdate);
        void Delete(Sale ObjDelete);
        int GetMaxSerial(int CompanyID);
    }
}