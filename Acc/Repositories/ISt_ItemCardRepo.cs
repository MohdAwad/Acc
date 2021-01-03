using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ISt_ItemCardRepo
    {
        void UpdateLogo(string ItemdId,string Img);
        void AddItemCard(St_ItemCard ObjSave);
        void AddItemWarehous(St_ItemWarehouse ObjSave);
        void AddSimilarItem(St_SimilarItem ObjSave);
        void AddAlternativeItem(St_AlternativeItem ObjSave);
        void AddItemOffer(St_ItemOffer ObjSave);
        void AddItemOtherUnit(St_ItemOtherUnit ObjSave);
        void AddItemAdvertisingRepresentative(St_ItemAdvertisingRepresentative ObjSave);
        St_ItemCard GetSt_ItemCardById(string ItemCode, int CompanyID);
        void UpdateItemWarehous(St_ItemWarehouse ObjUpdate);
    }
}