using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Acc.Persistence
{
    public class St_ItemCardRepo : ISt_ItemCardRepo
    {
        private readonly ApplicationDbContext _context;
        public St_ItemCardRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public void AddItemCard(St_ItemCard ObjSave)
        {
            _context.St_ItemCards.Add(ObjSave);
        }
        public void AddItemWarehous(St_ItemWarehouse ObjSave)
        {
            _context.St_ItemWarehouses.Add(ObjSave);
        }
        public void AddSimilarItem(St_SimilarItem ObjSave)
        {
            _context.St_SimilarItems.Add(ObjSave);
        }
        public void AddAlternativeItem(St_AlternativeItem ObjSave)
        {
            _context.St_AlternativeItems.Add(ObjSave);
        }
        public void AddItemOffer(St_ItemOffer ObjSave)
        {
            _context.St_ItemOffers.Add(ObjSave);
        }
        public void AddItemOtherUnit(St_ItemOtherUnit ObjSave)
        {
            _context.St_ItemOtherUnits.Add(ObjSave);
        }
        public void AddItemAdvertisingRepresentative(St_ItemAdvertisingRepresentative ObjSave)
        {
            _context.St_ItemAdvertisingRepresentatives.Add(ObjSave);
        }
        public St_ItemCard GetSt_ItemCardById(string ItemCode, int CompanyID)
        {
            return _context.St_ItemCards.FirstOrDefault(m => m.CompanyID == CompanyID && m.ItemCode == ItemCode);
        }

        public void UpdateLogo(string ItemdId, string Img)
        {
            var d = _context.St_ItemCards.FirstOrDefault(m => m.ItemCode == ItemdId);
            if (d != null)
            {
                d.ItemLogo = Img;
            }
        }
        public void UpdateItemWarehous(St_ItemWarehouse ObjUpdate)
        {
            var ObjToUpdate = _context.St_ItemWarehouses.FirstOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.StockCode == ObjUpdate.StockCode && m.ItemCode == ObjUpdate.ItemCode);
            if (ObjToUpdate != null)
            {
                ObjToUpdate.StockMaximumItemNo = ObjUpdate.StockMaximumItemNo;
                ObjToUpdate.StockMinimumItemNo = ObjUpdate.StockMinimumItemNo;
            }
        }
    }
}