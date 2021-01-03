using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class St_ItemGallaryRepo : ISt_ItemGallaryRepo
    {
        private readonly ApplicationDbContext _context;

        public St_ItemGallaryRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(St_ItemGallary ObjToSave)
        {
            _context.St_ItemGallarys.Add(ObjToSave);
        }

        public void Delete(int CoId, int Id )
        {
            var D = _context.St_ItemGallarys.FirstOrDefault(m => m.Id == Id   && m.CompanyID==CoId);
            if (D != null)
            {
                _context.St_ItemGallarys.Remove(D);
            }
            
        }

        public IEnumerable<St_ItemGallary> GetAllFiles(int CoId, string ItemCode)
        {
          return  _context.St_ItemGallarys.Where(m => m.CompanyID == CoId && m.ItemCode == ItemCode).ToList();
        }

        public St_ItemGallary GetFileById(int CoId, int Id )
        {
            var D = _context.St_ItemGallarys.FirstOrDefault(m => m.Id == Id && m.CompanyID == CoId);
            return D;
        }
    }
}