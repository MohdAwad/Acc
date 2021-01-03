using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{

    public class St_WarehouseRepo : ISt_WarehouseRepo
    {
        private readonly ApplicationDbContext _context;

        public St_WarehouseRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public ApplicationDbContext Context => _context;
        public void Add(St_Warehouse ObjSave)
        {
            Context.St_Warehouses.Add(ObjSave);
        }
        public void Delete(St_Warehouse ObjDelete)
        {
            var ObjToDelete = Context.St_Warehouses.SingleOrDefault(m => m.CompanyID == ObjDelete.CompanyID && m.StockCode == ObjDelete.StockCode);
            if (ObjToDelete != null)
            {
                Context.St_Warehouses.Remove(ObjToDelete);
            }
        }
        public St_Warehouse GetSt_WarehouseByID(int CompanyID, string StockCode)
        {
            return Context.St_Warehouses.FirstOrDefault(m => m.CompanyID == CompanyID && m.StockCode == StockCode);
        }
        public void Update(St_Warehouse ObjUpdate)
        {
            var ObjToUpdate = Context.St_Warehouses.FirstOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.StockCode == ObjUpdate.StockCode);
            if (ObjToUpdate != null)
            {
                ObjToUpdate.ArabicName = ObjUpdate.ArabicName;
                ObjToUpdate.EnglishName = ObjUpdate.EnglishName;
                ObjToUpdate.AccountNumber = ObjUpdate.AccountNumber;
                ObjToUpdate.CostCenterNumber = ObjUpdate.CostCenterNumber;
                ObjToUpdate.Address = ObjUpdate.Address;
                ObjToUpdate.Telephone = ObjUpdate.Telephone;
                ObjToUpdate.InsDateTime = ObjUpdate.InsDateTime;
                ObjToUpdate.InsUserID = ObjUpdate.InsUserID;
            }
        }
        public string CheckIfStockCodeExisting(int CompanyID, string StockCode)
        {
            var Obj = _context.St_Warehouses.FirstOrDefault(m => m.CompanyID == CompanyID && m.StockCode == StockCode);
            if (Obj != null)
                return Obj.StockCode;
            else
                return "";

        }
        public IEnumerable<St_Warehouse> GetAllSt_Warehouse(int CompanyID)
        {
            return _context.St_Warehouses.Where(m => m.CompanyID == CompanyID).ToList();
        }
        public string GetFirstStock(int CompanyID)
        {
            var Obj = _context.St_Warehouses.FirstOrDefault(m => m.CompanyID == CompanyID);
            if (Obj != null)
                return Obj.StockCode;
            else
                return "";

        }
    }
}