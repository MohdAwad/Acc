using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class St_WarehouseAccountRepo : ISt_WarehouseAccountRepo
    {
        private readonly ApplicationDbContext _context;
        public St_WarehouseAccountRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(St_WarehouseAccount ObjSave)
        {
            _context.St_WarehouseAccounts.Add(ObjSave);
        }
        public St_WarehouseAccount GetSt_WarehouseAccountByID(int CompanyID, string StockCode)
        {
            return _context.St_WarehouseAccounts.FirstOrDefault(m => m.CompanyID == CompanyID && m.StockCode == StockCode);
        }
        public void Delete(St_WarehouseAccount ObjDelete)
        {
            var ObjToDelete = _context.St_WarehouseAccounts.SingleOrDefault(m => m.CompanyID == ObjDelete.CompanyID && m.StockCode == ObjDelete.StockCode);
            if (ObjToDelete != null)
            {
                _context.St_WarehouseAccounts.Remove(ObjToDelete);
            }
        }
    }
}