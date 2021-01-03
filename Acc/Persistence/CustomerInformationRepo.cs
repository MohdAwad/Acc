using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class CustomerInformationRepo : ICustomerInformationRepo
    {
        private readonly ApplicationDbContext _context;

        public CustomerInformationRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Delete(CustomerInformation ObjToSave)
        {
            var ObjToDelete = _context.CustomerInformations.FirstOrDefault(m => m.AccountNumber == ObjToSave.AccountNumber
                            && m.CompanyID == ObjToSave.CompanyID);
            if (ObjToDelete != null)
            {
                _context.CustomerInformations.Remove(ObjToDelete);
            }
        }
        public void Add(CustomerInformation ObjToSave)
        {
            var ObjToUpdate = _context.CustomerInformations.SingleOrDefault
                  (m => m.CompanyID == ObjToSave.CompanyID && m.AccountNumber == ObjToSave.AccountNumber);

            if (ObjToUpdate != null)
            {
                ObjToUpdate.AreaID = ObjToSave.AreaID;
                ObjToUpdate.AuthorizedSignatory = ObjToSave.AuthorizedSignatory;

                ObjToUpdate.BuildingNo = ObjToSave.BuildingNo;
                ObjToUpdate.CityID = ObjToSave.CityID;
                ObjToUpdate.CommercialRecord = ObjToSave.CommercialRecord;
                ObjToUpdate.DebitLimit = ObjToSave.DebitLimit;
                ObjToUpdate.DebitPeriod = ObjToSave.DebitPeriod;
                ObjToUpdate.DiscountPercentage = ObjToSave.DiscountPercentage;
                ObjToUpdate.Email = ObjToSave.Email;
                ObjToUpdate.FloorNo = ObjToSave.FloorNo;
                ObjToUpdate.KnownTo = ObjToSave.KnownTo;
                ObjToUpdate.Mobile = ObjToSave.Mobile;
                ObjToUpdate.NationalNumberOfTheFacility = ObjToSave.NationalNumberOfTheFacility;
                ObjToUpdate.NextTo = ObjToSave.NextTo;
                ObjToUpdate.PaymnetMethodTypeID = ObjToSave.PaymnetMethodTypeID;
                ObjToUpdate.ProfessionLicence = ObjToSave.ProfessionLicence;
                ObjToUpdate.StreetName = ObjToSave.StreetName;
                ObjToUpdate.TeleFax = ObjToSave.TeleFax;
                ObjToUpdate.Telephone = ObjToSave.Telephone;
                ObjToUpdate.TradeName = ObjToSave.TradeName;
                ObjToUpdate.Website = ObjToSave.Website;


            }
            else
                _context.CustomerInformations.Add(ObjToSave);


        }
      
        public IEnumerable<CustomerInformation> GetCustByCityAreaAccount(int CompanyID, int CityId, int AreadID)
        {
            return _context.CustomerInformations.Where(m => m.CityID == CityId && m.AreaID == AreadID && m.CompanyID == CompanyID).ToList();
        }
        public CustomerInformation GetCustomerInformation(int CompanyID, string AccountNo)
        {
            throw new NotImplementedException();
        }

        public void Update(CustomerInformation ObjToSave)
        {
            var ObjToUpdate = _context.CustomerInformations.SingleOrDefault
                (m => m.CompanyID == ObjToSave.CompanyID && m.AccountNumber == ObjToSave.AccountNumber);

            if (ObjToUpdate != null)
            {
                ObjToUpdate.AreaID = ObjToSave.AreaID;
                ObjToUpdate.AuthorizedSignatory = ObjToSave.AuthorizedSignatory;
              
                ObjToUpdate.BuildingNo = ObjToSave.BuildingNo;
                ObjToUpdate.CityID = ObjToSave.CityID;
                ObjToUpdate.CommercialRecord = ObjToSave.CommercialRecord;
                ObjToUpdate.DebitLimit = ObjToSave.DebitLimit;
                ObjToUpdate.DebitPeriod = ObjToSave.DebitPeriod;
                ObjToUpdate.DiscountPercentage = ObjToSave.DiscountPercentage;
                ObjToUpdate.Email = ObjToSave.Email;
                ObjToUpdate.FloorNo = ObjToSave.FloorNo;
                ObjToUpdate.KnownTo = ObjToSave.KnownTo;
                ObjToUpdate.Mobile = ObjToSave.Mobile;
                ObjToUpdate.NationalNumberOfTheFacility = ObjToSave.NationalNumberOfTheFacility;
                ObjToUpdate.NextTo = ObjToSave.NextTo;
                ObjToUpdate.PaymnetMethodTypeID = ObjToSave.PaymnetMethodTypeID;
                ObjToUpdate.ProfessionLicence = ObjToSave.ProfessionLicence;
                ObjToUpdate.StreetName = ObjToSave.StreetName;
                ObjToUpdate.TeleFax = ObjToSave.TeleFax;
                ObjToUpdate.Telephone = ObjToSave.Telephone;
                ObjToUpdate.TradeName = ObjToSave.TradeName;
                ObjToUpdate.Website = ObjToSave.Website;
                

            }


        }
    }
}