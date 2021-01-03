using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface IDefinitionAssetSiteRepo
    {
        void Add(DefinitionAssetSite ObjToSave);
        void Update(DefinitionAssetSite ObjUpdate);
    }
}