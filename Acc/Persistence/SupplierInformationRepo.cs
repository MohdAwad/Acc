using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class SupplierInformationRepo : ISupplierInformationRepo
    {
        private readonly ApplicationDbContext _context;

        public SupplierInformationRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Delete(SupplierInformation ObjToSave)
        {
            var ObjToDelete = _context.SupplierInformation.FirstOrDefault(m => m.AccountNumber == ObjToSave.AccountNumber
                            && m.CompanyID == ObjToSave.CompanyID);
            if (ObjToDelete != null)
            {
                _context.SupplierInformation.Remove(ObjToDelete);
            }
        }
        public void Add(SupplierInformation ObjToSave)
        {
            var ObjToUpdate = _context.SupplierInformation.SingleOrDefault
                  (m => m.CompanyID == ObjToSave.CompanyID && m.AccountNumber == ObjToSave.AccountNumber);

            if (ObjToUpdate != null)
            {
                ObjToUpdate.Address = ObjToSave.Address;
                ObjToUpdate.BankAccountNumber = ObjToSave.BankAccountNumber;
                ObjToUpdate.BankAdderss = ObjToSave.BankAdderss;
                ObjToUpdate.BeneficiaryName = ObjToSave.BeneficiaryName;
                ObjToUpdate.IBAN = ObjToSave.IBAN;
                ObjToUpdate.SwiftCode = ObjToSave.SwiftCode;
                ObjToUpdate.Mobile = ObjToSave.Mobile;
                ObjToUpdate.NameOfPersonInCharge = ObjToSave.NameOfPersonInCharge;
          
                ObjToUpdate.PaymnetMethodTypeID = ObjToSave.PaymnetMethodTypeID;
                ObjToUpdate.PhoneOfPersonInCharge = ObjToSave.PhoneOfPersonInCharge;

                ObjToUpdate.SupplierTypeID = ObjToSave.SupplierTypeID;
                ObjToUpdate.SupplierCityID = ObjToSave.SupplierCityID;
                ObjToUpdate.SupplierCountryID = ObjToSave.SupplierCountryID;
                ObjToUpdate.SupplierCityBankID = ObjToSave.SupplierCityBankID;
                ObjToUpdate.SupplierCountryBankID = ObjToSave.SupplierCountryBankID;
                ObjToUpdate.SupplierBranchBankID = ObjToSave.SupplierBranchBankID;
                ObjToUpdate.SupplierBankID = ObjToSave.SupplierBankID;
                ObjToUpdate.TeleFax = ObjToSave.TeleFax;
                ObjToUpdate.Telephone = ObjToSave.Telephone;
                ObjToUpdate.Website = ObjToSave.Website;

                ObjToUpdate.AccountNumber = ObjToSave.AccountNumber;
                ObjToUpdate.DebitPeriod = ObjToSave.DebitPeriod;
                ObjToUpdate.Email = ObjToSave.Email;
            
    






            }
            else
                _context.SupplierInformation.Add(ObjToSave);


        }

     

        public SupplierInformation GetSupplierInformation(int CompanyID, string AccountNo)
        {
            throw new NotImplementedException();
        }
    }
}