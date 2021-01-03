using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class AssetSaleConsumRepo : IAssetSaleConsumRepo
    {
        private readonly ApplicationDbContext _context;

        public AssetSaleConsumRepo(ApplicationDbContext context)
        {
            _context = context;
        }
         
        public void Add(AssetSaleConsum ObjToSave)
        {
            _context.AssetSaleConsums.Add(ObjToSave);
        }

        public IEnumerable<AssetSaleConsum> GetAssetSaleConsums(int CompanyID, int TypeID)
        {
            return _context.AssetSaleConsums.Where(m => m.CompanyID == CompanyID && m.Type == TypeID).ToList();
        }

        public int GetMaxSerial(int CompanyID, int TypeID)
        {
            var Obj = _context.AssetSaleConsums.FirstOrDefault(m => m.CompanyID == CompanyID && m.Type == TypeID);
            if (Obj == null)
            {
                return 1;
            }
            else
            {
                return _context.AssetSaleConsums.Where(m => m.CompanyID == CompanyID && m.Type==TypeID).Max(p => p.Serial) + 1;
            }
        }
    }
}