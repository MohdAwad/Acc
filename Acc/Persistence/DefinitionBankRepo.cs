using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class DefinitionBankRepo : IDefinitionBankRepo
    {
        private readonly ApplicationDbContext _context;

        public DefinitionBankRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public ApplicationDbContext Context => _context;

         public void Add(DefinitionBank ObjSave)
        {
            Context.DefinitionBanks.Add(ObjSave);
        }

        public void Delete(DefinitionBank ObjDelete)
        {
            var ObjToDelete = Context.DefinitionBanks.SingleOrDefault(m => m.CompanyID == ObjDelete.CompanyID && m.BankID == ObjDelete.BankID);
            if (ObjToDelete != null)
            {
                Context.DefinitionBanks.Remove(ObjToDelete);
            }
        }

        public void UpdateDeleteFlag(DefinitionBank ObjDelete)
        {
            var ObjToDelete = Context.DefinitionBanks.FirstOrDefault(m => m.CompanyID == ObjDelete.CompanyID && m.BankID == ObjDelete.BankID);
            if (ObjToDelete != null)
            {
                ObjToDelete.IsDeleted = 1;
            }
        }

        public IEnumerable<DefinitionBank> GetAllDefinitionBank(int CompanyID)
        {
            return Context.DefinitionBanks.Where(m => m.CompanyID == CompanyID).ToList();
        }

        public int GetMaxSerial(int CompanyID)
        {
            var Obj = _context.DefinitionBanks.FirstOrDefault(m => m.CompanyID == CompanyID);
            if (Obj == null)
            {
                return 1;
            }
            else
            {
                return _context.DefinitionBanks.Where(m => m.CompanyID == CompanyID).Max(p => p.BankID) + 1;
            }
        }

        public void Update(DefinitionBank ObjUpdate)
        {
            var ObjToUpdate = Context.DefinitionBanks.FirstOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.BankID == ObjUpdate.BankID);
            if (ObjToUpdate != null)
            {
                ObjToUpdate.BankAccountNumber = ObjUpdate.BankAccountNumber;
                ObjToUpdate.ChequeUnderCollectionAccountNumber = ObjUpdate.ChequeUnderCollectionAccountNumber;
                ObjToUpdate.PostdatedChequeAccountNumber = ObjUpdate.PostdatedChequeAccountNumber;
                ObjToUpdate.BillsOfExchangeAccountNumber = ObjUpdate.BillsOfExchangeAccountNumber;
                ObjToUpdate.IsDeleted = 0;
                ObjToUpdate.InsDateTime = ObjUpdate.InsDateTime;
                ObjToUpdate.InsUserID = ObjUpdate.InsUserID;
            }
        }

        DefinitionBank IDefinitionBankRepo.GetDefinitionBankByID(int CompanyID, int BankID)
        {
            return Context.DefinitionBanks.FirstOrDefault(m => m.CompanyID == CompanyID && m.BankID == BankID);
        }
    }
}