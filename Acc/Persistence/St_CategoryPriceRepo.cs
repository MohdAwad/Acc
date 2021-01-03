using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Acc.Persistence
{
    public class St_CategoryPriceRepo : ISt_CategoryPriceRepo
    {
        private readonly ApplicationDbContext _context;
        public St_CategoryPriceRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public St_CategoryPrice GetSt_CategoryPriceByID(int CompanyID, int CategoryPriceID)
        {
            return _context.St_CategoryPrices.FirstOrDefault(m => m.CompanyID == CompanyID && m.CategoryPriceID == CategoryPriceID);
        }
        public void Update(St_CategoryPrice ObjUpdate)
        {
            var ObjToUpdate = _context.St_CategoryPrices.SingleOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.CategoryPriceID == ObjUpdate.CategoryPriceID);
            if (ObjToUpdate != null)
            {
                ObjToUpdate.ArabicName = ObjUpdate.ArabicName;
                ObjToUpdate.EnglishName = ObjUpdate.EnglishName;
                ObjToUpdate.InsDateTime = ObjUpdate.InsDateTime;
                ObjToUpdate.InsUserID = ObjUpdate.InsUserID;
            }
        }
        public IEnumerable<St_CategoryPrice> GetAllSt_CategoryPrice(int CompanyID)
        {
            return _context.St_CategoryPrices.Where(m => m.CompanyID == CompanyID).ToList();
        }
        public string GetSt_CategoryPriceNameByID(int CompanyID, int CategoryPriceID)
        {
            var ObjName = _context.St_CategoryPrices.FirstOrDefault(m => m.CompanyID == CompanyID && m.CategoryPriceID == CategoryPriceID);
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