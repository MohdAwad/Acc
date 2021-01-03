using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class St_WarehouseHRepo : ISt_WarehouseHRepo
    {
        private readonly ApplicationDbContext _context;

        public St_WarehouseHRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public ApplicationDbContext Context => _context;
        public void Add(St_WarehouseH ObjSave)
        {
            Context.St_WarehouseHs.Add(ObjSave);
        }
        public void Delete(St_WarehouseH ObjDelete)
        {
            var ObjToDelete = Context.St_WarehouseHs.SingleOrDefault(m => m.CompanyID == ObjDelete.CompanyID && m.StockCode == ObjDelete.StockCode);
            if (ObjToDelete != null)
            {
                Context.St_WarehouseHs.Remove(ObjToDelete);
            }
        }
        public St_WarehouseH GetSt_WarehouseHByID(int CompanyID, string StockCode)
        {
            return Context.St_WarehouseHs.FirstOrDefault(m => m.CompanyID == CompanyID && m.StockCode == StockCode);
        }
        public void Update(St_WarehouseH ObjUpdate)
        {
            var ObjToUpdate = Context.St_WarehouseHs.FirstOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.StockCode == ObjUpdate.StockCode);
            if (ObjToUpdate != null)
            {
                ObjToUpdate.ArabicName = ObjUpdate.ArabicName;
                ObjToUpdate.EnglishName = ObjUpdate.EnglishName;
                ObjToUpdate.Address = ObjUpdate.Address;
                ObjToUpdate.Telephone = ObjUpdate.Telephone;
                ObjToUpdate.InsDateTime = ObjUpdate.InsDateTime;
                ObjToUpdate.InsUserID = ObjUpdate.InsUserID;
                ObjToUpdate.ManufacturingWarehouse = ObjUpdate.ManufacturingWarehouse;
            }
        }
        public string CheckIfStockCodeExisting(int CompanyID, string StockCode)
        {
            var Obj = _context.St_WarehouseHs.FirstOrDefault(m => m.CompanyID == CompanyID && m.StockCode == StockCode);
            if (Obj != null)
                return Obj.StockCode;
            else
                return "";

        }
        public IEnumerable<St_WarehouseH> GetAllSt_WarehouseH(int CompanyID)
        {
            return _context.St_WarehouseHs.Where(m => m.CompanyID == CompanyID).ToList();
        }
    }
}