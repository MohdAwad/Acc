using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class AccreditationInformationRepo : IAccreditationInformationRepo
    {
        private readonly ApplicationDbContext _context;

        public AccreditationInformationRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(AccreditationInformation ObjToSave)
        {
            var ObjToUpdate = _context.AccreditationInformations.SingleOrDefault
                  (m => m.CompanyID == ObjToSave.CompanyID && m.AccountNumber == ObjToSave.AccountNumber);

            if (ObjToUpdate != null)
            {
                ObjToUpdate.AccreditationExpiresPlace = ObjToSave.AccreditationExpiresPlace;

                ObjToUpdate.ArrivePort = ObjToSave.ArrivePort;
                ObjToUpdate.Bank = ObjToSave.Bank;
                ObjToUpdate.BankInsurance = ObjToSave.BankInsurance;
                ObjToUpdate.BeneficiaryOfAccreditation = ObjToSave.BeneficiaryOfAccreditation;
                ObjToUpdate.CommissionsPaid = ObjToSave.CommissionsPaid;
                ObjToUpdate.CreditForeignCurrency = ObjToSave.CreditForeignCurrency;
                ObjToUpdate.CreditInlocalCurrency = ObjToSave.CreditInlocalCurrency;
                ObjToUpdate.CurrencyType = ObjToSave.CurrencyType;
                ObjToUpdate.DateOfIssuanceOfCredit = ObjToSave.DateOfIssuanceOfCredit;
                ObjToUpdate.ExchangeRate = ObjToSave.ExchangeRate;
                ObjToUpdate.LastShipmentDate = ObjToSave.LastShipmentDate;
                ObjToUpdate.MultipleShippingMethods = ObjToSave.MultipleShippingMethods;
                ObjToUpdate.PartialCharging = ObjToSave.PartialCharging;
                ObjToUpdate.PaymentTerms = ObjToSave.PaymentTerms;
                ObjToUpdate.ShippingPort = ObjToSave.ShippingPort;
                ObjToUpdate.TypeOfGoods = ObjToSave.TypeOfGoods;
                ObjToUpdate.ValidityOfAccreditation = ObjToSave.ValidityOfAccreditation;

                




            }
            else
                _context.AccreditationInformations.Add(ObjToSave);

        }

        public AccreditationInformation GetAccreditationInformation(int CompanyID, string AccountNo)
        {
            throw new NotImplementedException();
        }

        public void Update(AccreditationInformation ObjToSave)
        {
            throw new NotImplementedException();
        }
    }
}