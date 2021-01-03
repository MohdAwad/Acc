using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Acc.Persistence
{
    public class ChartOfAccountRepo : IChartOfAccountRepo
    {
        private readonly ApplicationDbContext _context;

        public ChartOfAccountRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(ChartOfAccount ObjToSave)
        {
            _context.ChartOfAccounts.Add(ObjToSave);
        }

        public void Delete(ChartOfAccount ObjToSave)
        {
            var ObjToDelete = _context.ChartOfAccounts.FirstOrDefault(m => m.AccountNumber == ObjToSave.AccountNumber
                            && m.CompanyID == ObjToSave.CompanyID);
            if (ObjToDelete != null)
            {
                _context.ChartOfAccounts.Remove(ObjToDelete);
            }
        }

        public ChartOfAccount GetAccountByID(int CompanyID, string AccountID)
        {
           return _context.ChartOfAccounts.FirstOrDefault(m => m.AccountNumber == AccountID && m.CompanyID == CompanyID);
        }

        public IEnumerable<ChartOfAccount> GetAllChartOfAccount(int CompanyID)
        {
            return _context.ChartOfAccounts.Where(m => m.CompanyID == CompanyID).ToList();
        }

        public ChartOfAccount GetChartOfAccountById(int CompanyID, string AccountFather)
        {
            return _context.ChartOfAccounts.FirstOrDefault(m => m.CompanyID == CompanyID && m.AccountNumber == AccountFather);
        }

        public IEnumerable<ChartOfAccount> GetChartOfAccountByLevel(int AccountLevel)
        {
            throw new NotImplementedException();
        }
        public void Update(ChartOfAccount ObjToSave)
        {
            var ObjToUpdate = _context.ChartOfAccounts.FirstOrDefault(m => m.AccountNumber == ObjToSave.AccountNumber
              && m.CompanyID == ObjToSave.CompanyID );


            if (ObjToUpdate != null)

            {
                ObjToUpdate.ArabicName = ObjToSave.ArabicName;
                ObjToUpdate.EnglishName = ObjToSave.EnglishName;
                ObjToUpdate.AccountTypeID = ObjToSave.AccountTypeID;
                ObjToUpdate.AccountKind = ObjToSave.AccountKind;
                ObjToUpdate.CurrencyID = ObjToSave.CurrencyID;
                ObjToUpdate.SalesID = ObjToSave.SalesID;
                ObjToUpdate.Note = ObjToSave.Note;
                ObjToUpdate.StoppedAccount = ObjToSave.StoppedAccount;



            }
        }
    }
}