using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class AssetHeaderRepo : IAssetHeaderRepo
    {
        private readonly ApplicationDbContext _context;

        public AssetHeaderRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(AssetHeader ObjToSave)
        {
            _context.AssetHeaders.Add(ObjToSave);
        }

        public void ExportAssetByVouchrNo(int CompanyID, int VouchrNo)
        {
            _context.Database.ExecuteSqlCommand(
                "  Update   AssetHeaders " +
                " set Exported=1   " +

                " where CompanyID=@CompanyID and VouchrNo=@VouchrNo  "


                , new SqlParameter("@CompanyID", CompanyID)
                , new SqlParameter("@VouchrNo", VouchrNo)
              


                );
        }

        public IEnumerable<AssetHeader> GetAssetHeaders(int CompanyID)
        {
            return _context.AssetHeaders.Where(m=>m.CompanyID==CompanyID && m.Exported==0).ToList();
        }

        public int GetMaxSerial(int CompanyID)
        {
            var Obj = _context.AssetHeaders.FirstOrDefault(m => m.CompanyID == CompanyID);
            if (Obj == null)
            {
                return 1;
            }
            else
            {
                return _context.AssetHeaders.Where(m => m.CompanyID == CompanyID).Max(p => p.VouchrNo) + 1;
            }
        }
        public AssetHeader GetAssetHeaderByID(int CompanyID, int VouchrNo)
        {
            try
            {
                return _context.AssetHeaders.FirstOrDefault(m => m.CompanyID == CompanyID && m.VouchrNo == VouchrNo);
            }
            catch (Exception ex)
            {
                return new AssetHeader();
            }
        }
    }
}