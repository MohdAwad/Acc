using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Acc.Persistence
{
    public class St_ExtraExpenseRepo : ISt_ExtraExpenseRepo
    {
        private readonly ApplicationDbContext _context;
        public St_ExtraExpenseRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public St_ExtraExpense GetSt_ExtraExpenseByID(int CompanyID, int ExtraExpenseID)
        {
            return _context.St_ExtraExpenses.FirstOrDefault(m => m.CompanyID == CompanyID && m.ExtraExpenseID == ExtraExpenseID);
        }
        public void Update(St_ExtraExpense ObjUpdate)
        {
            var ObjToUpdate = _context.St_ExtraExpenses.SingleOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.ExtraExpenseID == ObjUpdate.ExtraExpenseID);
            if (ObjToUpdate != null)
            {
                ObjToUpdate.ArabicName = ObjUpdate.ArabicName;
                ObjToUpdate.EnglishName = ObjUpdate.EnglishName;
            }
        }
        public IEnumerable<St_ExtraExpense> GetAllSt_ExtraExpense(int CompanyID)
        {
            return _context.St_ExtraExpenses.Where(m => m.CompanyID == CompanyID).ToList();
        }
        public string GetSt_ExtraExpenseNameByID(int CompanyID, int ExtraExpenseID)
        {
            var ObjName = _context.St_ExtraExpenses.FirstOrDefault(m => m.CompanyID == CompanyID && m.ExtraExpenseID == ExtraExpenseID);
            if (ObjName != null)
            {
                if (Resources.Resource.CurLang == "Arb")
                {
                    return ObjName.ArabicName;
                }
                else
                {
                    return ObjName.EnglishName;
                }
            }
            else
            {
                return "";
            }
        }
    }
}