using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface IEstimatedBudgetRepo
    {

        void Add(EstimatedBudget ObjToSave);
        

        EstimatedBudget GetEstimatedBudgetForAcc(int CompanyID, int CompanyYear, string AccountNumber, int BudgetType );
        IEnumerable<EstimatedBudget> GetAllEstimatedBudgets(int CompanyID, int CompanyYear);




    }
}