using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ICostCenterRepo
    {
        void Add(CostCenter ObjToSave);
        void Update(CostCenter ObjToSave);

        void Delete(CostCenter ObjToSave);

        IEnumerable<CostCenter> GetAllCostCenter(int CompanyID);

        //IEnumerable<CostCenter> GetCostCenterByFather(int CompanyID, string CostFather);

        //IEnumerable<CostCenter> GetCostCenterByLevel(int CostLevel);
          CostCenter GetCostCenterById(int CompanyID, string AccountFather);
        CostCenter GetCostByID(int CompanyID, string AccountFather);
        CostCenter GetCostCenterByFather(int CompanyID, string AccountFather);

        IEnumerable<CostCenter> GetCostCenterByLevel(int AccountLevel);


    }
}