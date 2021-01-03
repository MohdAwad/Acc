using Acc.Models;
using Acc.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Acc.Persistence
{
    public class DrawnBankRepo : IDrawnBankRepo
    {
        private readonly ApplicationDbContext _context;
        public DrawnBankRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(DrawnBank ObjSave)
        {
            _context.DrawnBanks.Add(ObjSave);
        }
        public IEnumerable<DrawnBank> GetAllDrawnBank(int CompanyID)
        {
            return _context.DrawnBanks.Where(m => m.CompanyID == CompanyID).ToList();
        }

        public DrawnBank GetDrawnBankByID(int CompanyID, int BankId)
        {
            return _context.DrawnBanks.FirstOrDefault(m => m.CompanyID == CompanyID && m.BankID == BankId);
        }

        public int GetMaxSerial(int CompanyID)
        {
            var Obj = _context.DrawnBanks.FirstOrDefault(m => m.CompanyID == CompanyID);
            if (Obj == null)
            {
                return 1;
            }
            else
            {
                return _context.DrawnBanks.Where(m => m.CompanyID == CompanyID).Max(p => p.BankID) + 1;
            }
        }
        public void Update(DrawnBank ObjUpdate)
        {
            var ObjToUpdate = _context.DrawnBanks.SingleOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.BankID == ObjUpdate.BankID);
            if (ObjToUpdate != null)
            {
                ObjToUpdate.ArabicName = ObjUpdate.ArabicName;
                ObjToUpdate.EnglishName = ObjUpdate.EnglishName;
                ObjToUpdate.Active = ObjUpdate.Active;
                ObjToUpdate.InsDateTime = ObjUpdate.InsDateTime;
                ObjToUpdate.InsUserID = ObjUpdate.InsUserID;
            }
        }
    }
}