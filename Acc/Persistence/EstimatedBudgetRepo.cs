using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class EstimatedBudgetRepo : IEstimatedBudgetRepo
    {
        private readonly ApplicationDbContext _context;

        public EstimatedBudgetRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(EstimatedBudget ObjToSave)
        {
            var D = _context.EstimatedBudgets.FirstOrDefault(m => m.CompanyID == ObjToSave.CompanyID && m.CompanyYear == ObjToSave.CompanyYear
            && m.AccountNumber == ObjToSave.AccountNumber && m.BudgetType==ObjToSave.BudgetType && m.CostCenterNumber==ObjToSave.CostCenterNumber);
            if (D != null)
            {
                D.YearBudget = ObjToSave.YearBudget;
                D.BudgetType = ObjToSave.BudgetType;
                D.CostCenterNumber = ObjToSave.CostCenterNumber;
                D.April = ObjToSave.April;
                D.August = ObjToSave.August;
                D.December = ObjToSave.December;
                D.February = ObjToSave.February;
                D.July= ObjToSave.July;
                D.January = ObjToSave.January;
                D.June = ObjToSave.June;
                D.March = ObjToSave.March;
                
                D.May = ObjToSave.May;
                D.November = ObjToSave.November;
                D.October = ObjToSave.October;
                D.September = ObjToSave.September;


            }
            else
            {
                _context.EstimatedBudgets.Add(ObjToSave);
            }
        }

        public IEnumerable<EstimatedBudget> GetAllEstimatedBudgets(int CompanyID, int CompanyYear)
        {
            return _context.EstimatedBudgets.Where(m => m.CompanyID == CompanyID && m.CompanyYear == CompanyYear  ).ToList();
        }

        public EstimatedBudget GetEstimatedBudgetForAcc(int CompanyID, int CompanyYear, string AccountNumber, int BudgetType)
        {
            try
            {  
                if (BudgetType == 2)
                {
                    return _context.EstimatedBudgets.FirstOrDefault(m => m.CompanyID == CompanyID && m.CompanyYear == CompanyYear && m.CostCenterNumber == AccountNumber && m.BudgetType == BudgetType);
                }
                else if (BudgetType == 3)

                {
                    return _context.EstimatedBudgets.FirstOrDefault(m => m.CompanyID == CompanyID && m.CompanyYear == CompanyYear && m.AccountNumber == AccountNumber && m.BudgetType == BudgetType);
                }
                    else
                {
                    return _context.EstimatedBudgets.FirstOrDefault(m => m.CompanyID == CompanyID && m.CompanyYear == CompanyYear && m.AccountNumber == AccountNumber && m.BudgetType == BudgetType);
                }
            }
            catch(Exception ex)
            {
                string s = ex.Message.ToString();
                return new EstimatedBudget();
            }



        }

      
    }
}