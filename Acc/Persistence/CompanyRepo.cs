using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class CompanyRepo : ICompanyRepo
    {
        private readonly ApplicationDbContext _context;

        public CompanyRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(Company ObjTOSave)
        {
            _context.Companys.Add(ObjTOSave);

          
        }

        public Company GetCompanyByUserID(string UserId)
        {
            throw new NotImplementedException();
        }

        public Company GetMyCompany(int CompanyID)
        {
            return _context.Companys.SingleOrDefault(m => m.CompanyID == CompanyID);
        }

        public void Update(Company ObjToSave)
        {
            var ObjToUpdate = _context.Companys.SingleOrDefault(m => m.CompanyID == ObjToSave.CompanyID);
            if (ObjToUpdate != null)
            {
                ObjToUpdate.EnglishName = ObjToSave.EnglishName;
                ObjToUpdate.ArabicAddress = ObjToSave.ArabicAddress;
                ObjToUpdate.ArabicName = ObjToSave.ArabicName;
                ObjToUpdate.Email = ObjToSave.Email;
                ObjToUpdate.EnglishAddress = ObjToSave.EnglishAddress;
                //ObjToUpdate.LogoPath = ObjToSave.LogoPath;
                ObjToUpdate.Mobile = ObjToSave.Mobile;
                ObjToUpdate.TeleFax = ObjToSave.TeleFax;
                ObjToUpdate.Telephone = ObjToSave.Telephone;
                ObjToUpdate.Website = ObjToSave.Website;
                ObjToUpdate.AccountChart = ObjToSave.AccountChart;
                ObjToUpdate.AccountChartZero = ObjToSave.AccountChartZero;
                ObjToUpdate.CostChart = ObjToSave.CostChart;
                ObjToUpdate.CostChartZero = ObjToSave.CostChartZero;
                ObjToUpdate.WorkWithCostCenter = ObjToSave.WorkWithCostCenter;
                ObjToUpdate.DirectExportTheTransactions = ObjToSave.DirectExportTheTransactions;
                ObjToUpdate.TheDecimalPointForTheForeignCurrency = ObjToSave.TheDecimalPointForTheForeignCurrency;
                ObjToUpdate.TheDecimalPointForTheLocalCurrency = ObjToSave.TheDecimalPointForTheLocalCurrency;

                ObjToUpdate.LogoPath = ObjToSave.LogoPath;
            }

        }
    }
}