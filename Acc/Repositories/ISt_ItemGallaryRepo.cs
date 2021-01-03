using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ISt_ItemGallaryRepo
    {
        void Add(St_ItemGallary ObjToSave);

        void Delete(int CoId, int Id );

        St_ItemGallary GetFileById(int CoId, int Id);
        IEnumerable<St_ItemGallary> GetAllFiles(int CoId, string ItemCode);

    }
}