using Acc.Models;
using Acc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface IAssetTransActionRepo
    {
        void Add(AssetTransAction ObjToSave);
        IEnumerable<AssetTransToPostVM> GetAssetCreditToPost(int CompanyID, int HeaderID);

        IEnumerable<AssetTransToPostVM> GetAssetDebitToPost(int CompanyID, int HeaderID);

        void ExportAssetTransByVouchrNo(int CompanyID, int VouchrNo);

    }
}