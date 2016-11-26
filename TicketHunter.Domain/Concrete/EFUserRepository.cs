using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using TicketHunter.Domain.Abstract;
using TicketHunter.Domain.Entities;

namespace TicketHunter.Domain.Concrete
{
    public class EFUserRepository : IUserRepository
    {
        EFDbContext context = new EFDbContext();

        public IEnumerable<UserProfileDetails> UserProfile => context.UserProfileDetails;

        public IEnumerable<UserAddress> UserAddresses => context.UserAddress;

        public IEnumerable<Countries> Countries => context.Countries;

        public IEnumerable<SelectListItem> CountriesDropList
        {
            get
            {
                var selectListItems = Countries.Select(x => new SelectListItem
                {
                    Value = x.CountryID.ToString(),
                    Text = x.CountryName,
                });
                return selectListItems;
            }
        }

        public void SaveUserData(UserProfileDetails userData)
        {
            if (userData.UserProfileDetailsID == 0)
            {
                context.UserProfileDetails.Add(userData);
            }
            else
            {
                UserProfileDetails dbEntry = context.UserProfileDetails.Find(userData.UserProfileDetailsID);
                if (dbEntry != null)
                {
                    Mapper.Map(userData, dbEntry);
                }
            }
            context.SaveChanges(); 
        }

        public void SaveUserAddress(UserAddress userAddress)
        {
            if (userAddress.UserAddressID == 0)
            {
                context.UserAddress.Add(userAddress);
            }
            else
            {
                UserAddress dbEntry = context.UserAddress.Find(userAddress.UserAddressID);
                if (dbEntry != null)
                {
                    Mapper.Map(userAddress, dbEntry);
                }
            }
            context.SaveChanges();
        }

        public UserProfileDetails GetDetails(string id)
        {
            UserProfileDetails details = context.UserProfileDetails.FirstOrDefault(x => x.Id == id);
            return details;
        }

        public UserAddress RemoveUserAddress(int addressId)
        {
            UserAddress address = context.UserAddress.Find(addressId);
            if (address != null)
            {
                context.UserAddress.Remove(address);
                context.SaveChanges();
            }
            return address;
        }
    }
}
