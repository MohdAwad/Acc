using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface IFavScreenRepo
    {
        IEnumerable<FavScreen> GetAllFavScreen(int CompanyID,string UserId);
        FavScreen  GetFavById(int CompanyID, string UserId,int id);

        void Add(FavScreen ObjSave);
        
        void Delete(int id,int CoId);
 
    }
}