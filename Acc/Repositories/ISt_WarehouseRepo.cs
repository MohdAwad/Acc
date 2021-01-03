using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ISt_WarehouseRepo
    {
        void Add(St_Warehouse ObjSave);
        void Update(St_Warehouse ObjUpdate);
        void Delete(St_Warehouse ObjDelete);
        St_Warehouse GetSt_WarehouseByID(int CompanyID, string StockCode);
        string CheckIfStockCodeExisting(int CompanyID, string StockCode);
        IEnumerable<St_Warehouse> GetAllSt_Warehouse(int CompanyID);
        string GetFirstStock(int CompanyID);
    }
}