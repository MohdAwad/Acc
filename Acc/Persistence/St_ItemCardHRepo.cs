using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class St_ItemCardHRepo : ISt_ItemCardHRepo
    {
        private readonly ApplicationDbContext _context;
        public St_ItemCardHRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public void AddItemCard(St_ItemCardH ObjSave)
        {
            _context.St_ItemCradHs.Add(ObjSave);
        }
        public void AddItemWarehous(St_ItemWarehouseH ObjSave)
        {
            _context.St_ItemWarehouseHs.Add(ObjSave);
        }
        public void AddManufacturingStage(St_ManufacturingStageH ObjSave)
        {
            _context.St_ManufacturingStageHs.Add(ObjSave);
        }
        public void AddRelatedItem(St_RelatedItemH ObjSave)
        {
            _context.St_RelatedItemHs.Add(ObjSave);
        }
        public void AddSimilarItem(St_SimilarItemH ObjSave)
        {
            _context.St_SimilarItemHs.Add(ObjSave);
        }
        public void AddSubColorsItem(St_SubColorsItemH ObjSave)
        {
            _context.St_SubColorsItemHs.Add(ObjSave);
        }
        public void AddRawMaterial(St_RawMaterialH ObjSave)
        {
            _context.St_RawMaterialHs.Add(ObjSave);
        }
        public int GetMaxSerial(int CompanyID, string GroupCode)
        {
            var Obj = _context.St_ItemCradHs.FirstOrDefault(m => m.CompanyID == CompanyID && m.GroupCode == GroupCode);
            if (Obj == null)
            {
                return 1;
            }
            else
            {
                return _context.St_ItemCradHs.Where(m => m.CompanyID == CompanyID && m.GroupCode == GroupCode).Max(p => p.ItemSerial) + 1;
            }
        }
        public St_ItemCardH GetSt_ItemCardById(string ItemCode, int CompanyID)
        {
            return _context.St_ItemCradHs.FirstOrDefault(m => m.CompanyID == CompanyID && m.ItemCode == ItemCode);
        }
    }
}