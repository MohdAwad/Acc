using Acc.Models;
using Acc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Acc.Repositories
{
    public interface ISt_OfferPriceRepo
    {
        void AddSt_OfferPriceHeader(St_OfferPriceHeader ObjToSave);
        void AddSt_OfferPriceTransaction(St_OfferPriceTransaction ObjToSave);
        int GetMaxVoucher(int CompanyID, int CompanyTransactionKindNo, int TransactionKindNo, int CompanyYear);
        St_OfferPriceHeader GetSt_OfferPriceHeaderDataById(string VoucherNumber, int CompanyID, int CompanyTransactionKindNo, int TransactionKindNo, int CompanyYear);
        void DeleteSt_OfferPriceHeader(St_OfferPriceHeader ObjToDelete);
    }
}