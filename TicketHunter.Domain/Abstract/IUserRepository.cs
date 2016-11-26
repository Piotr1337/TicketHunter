using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using TicketHunter.Domain.Entities;

namespace TicketHunter.Domain.Abstract
{
    public interface IUserRepository
    {
        IEnumerable<UserProfileDetails> UserProfile { get; }

        IEnumerable<UserAddress> UserAddresses { get; }

        IEnumerable<Countries> Countries { get; }

        IEnumerable<SelectListItem> CountriesDropList { get; }

        void SaveUserData(UserProfileDetails userData);

        void SaveUserAddress(UserAddress userAddress);

        UserProfileDetails GetDetails(string id);

        UserAddress RemoveUserAddress(int addressId);
    }
}
