using Acc.Models;
using Acc.Repositories;
using Acc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class St_FundingAgencyHRepo : ISt_FundingAgencyHRepo
    {

        private readonly ApplicationDbContext _context;
        public St_FundingAgencyHRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Delete(St_FundingAgencyHVM ObjDelete)
        {
            var ObjToDelete = _context.St_FundingAgencyHs.SingleOrDefault(m => m.CompanyID == ObjDelete.CompanyID && m.FundingAgencyID == ObjDelete.FundingAgencyID);
            if (ObjToDelete != null)
            {
                _context.St_FundingAgencyHs.Remove(ObjToDelete);
            }
        }

        public IEnumerable<St_FundingAgencyH> GetAllSt_FundingAgencyH(int CompanyID)
        {
            return _context.St_FundingAgencyHs.Where(m => m.CompanyID == CompanyID).ToList();
        }

        public St_FundingAgencyH GetSt_FundingAgencyHByID(int CompanyID, int FundingAgencyID)
        {
            return _context.St_FundingAgencyHs.FirstOrDefault(m => m.CompanyID == CompanyID && m.FundingAgencyID == FundingAgencyID);
        }

        public void Update(St_FundingAgencyHVM ObjUpdate)
        {
            var ObjToUpdate = _context.St_FundingAgencyHs.SingleOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.FundingAgencyID == ObjUpdate.FundingAgencyID);
            if (ObjToUpdate != null)
            {
                ObjToUpdate.ArabicName = ObjUpdate.ArabicName;
                ObjToUpdate.EnglishName = ObjUpdate.EnglishName;
                ObjToUpdate.CommissionAccount = ObjUpdate.CommissionAccount;
                ObjToUpdate.FlatCommission12 = ObjUpdate.FlatCommission12;
                ObjToUpdate.FlatCommission24 = ObjUpdate.FlatCommission24;
                ObjToUpdate.FlatCommission36 = ObjUpdate.FlatCommission36;
                ObjToUpdate.FlatCommission48 = ObjUpdate.FlatCommission48;
                ObjToUpdate.FlatCommission60 = ObjUpdate.FlatCommission60;
                ObjToUpdate.EmailOfPersonInCharge = ObjUpdate.EmailOfPersonInCharge;
                ObjToUpdate.NameOfPersonInCharge = ObjUpdate.NameOfPersonInCharge;
                ObjToUpdate.PhoneOfPersonInCharge = ObjUpdate.PhoneOfPersonInCharge;
                ObjToUpdate.PhoneOfPersonInCharge = ObjUpdate.PhoneOfPersonInCharge;
                ObjToUpdate.InsDateTime = ObjUpdate.InsDateTime;
                ObjToUpdate.InsUserID = ObjUpdate.InsUserID;

            }
        }

        public int GetMaxSerial(int CompanyID)
        {
            var Obj = _context.St_FundingAgencyHs.FirstOrDefault(m => m.CompanyID == CompanyID);
            if (Obj == null)
            {
                return 1;
            }
            else
            {
                return _context.St_FundingAgencyHs.Where(m => m.CompanyID == CompanyID).Max(p => p.FundingAgencyID) + 1;
            }
        }

        public void Add(St_FundingAgencyH ObjSave)
        {
            _context.St_FundingAgencyHs.Add(ObjSave);
        }
    }
}