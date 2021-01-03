using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ISt_ItemCardHRepo
    {
        void AddItemCard(St_ItemCardH ObjSave);
        void AddItemWarehous(St_ItemWarehouseH ObjSave);
        void AddManufacturingStage(St_ManufacturingStageH ObjSave);
        void AddRelatedItem(St_RelatedItemH ObjSave);
        void AddSimilarItem(St_SimilarItemH ObjSave);
        void AddSubColorsItem(St_SubColorsItemH ObjSave);
        void AddRawMaterial(St_RawMaterialH ObjSave);
        int GetMaxSerial(int CompanyID,string GroupCode);
        St_ItemCardH GetSt_ItemCardById(string ItemCode, int CompanyID);
    }
}