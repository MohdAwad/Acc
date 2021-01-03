using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class St_DescriptionRepo : ISt_DescriptionRepo
    {
        private readonly ApplicationDbContext _context;
        public St_DescriptionRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public St_Description GetSt_DescriptionByID(int CompanyID, int DescriptionID)
        {
            return _context.St_Descriptions.FirstOrDefault(m => m.CompanyID == CompanyID && m.DescriptionID == DescriptionID);
        }
        public void Update(St_Description ObjUpdate)
        {
            var ObjToUpdate = _context.St_Descriptions.SingleOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.DescriptionID == ObjUpdate.DescriptionID);
            if (ObjToUpdate != null)
            {
                ObjToUpdate.ArabicName = ObjUpdate.ArabicName;
                ObjToUpdate.EnglishName = ObjUpdate.EnglishName;
            }
        }
        public IEnumerable<St_Description> GetAllSt_Description(int CompanyID)
        {
            return _context.St_Descriptions.Where(m => m.CompanyID == CompanyID).ToList();
        }
        public string GetSt_DescriptionNameByID(int CompanyID, int DescriptionID)
        {
            var ObjName = _context.St_Descriptions.FirstOrDefault(m => m.CompanyID == CompanyID && m.DescriptionID == DescriptionID);
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