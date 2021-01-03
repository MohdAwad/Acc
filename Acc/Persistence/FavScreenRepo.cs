using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class FavScreenRepo : IFavScreenRepo
    {
        private readonly ApplicationDbContext _context;

        public FavScreenRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(FavScreen ObjSave)
        {
            var d= _context.FavScreens.FirstOrDefault(m => m.UserId == ObjSave.UserId && m.CompanyID == ObjSave.CompanyID && m.ScreenUrl== ObjSave.ScreenUrl);
            if (d == null)
            {
                _context.FavScreens.Add(ObjSave);

            }
            
        }

        public void Delete(int id, int CoId)
        {
            var d = _context.FavScreens.FirstOrDefault(m => m.Id == id && m.CompanyID == CoId);
            if (d != null)
            {
                _context.FavScreens.Remove(d);
            }
        }
        

            public IEnumerable<FavScreen> GetAllFavScreen(int CompanyID, string UserId)
        {
            return _context.FavScreens.Where(m => m.CompanyID == CompanyID && m.UserId == UserId).ToList();
        }
        public FavScreen  GetFavById(int CompanyID, string UserId, int id) 
        {
            return _context.FavScreens.FirstOrDefault(m=>m.CompanyID==CompanyID && m.UserId==UserId && m.Id==id);
        }
    }
}