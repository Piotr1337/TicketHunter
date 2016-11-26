using AutoMapper;
using TicketHunter.Domain.Entities;
using TicketHunter.Models;
using TicketReservation.Models;

namespace TicketHunter.Mappings
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "ViewModelToDomainMappings"; }
        }

        protected override void Configure()
        {
            CreateMap<AdminViewModel, Events>();
            CreateMap<TicketViewModel, Ticket>();
            CreateMap<UserDataViewModel, UserProfileDetails>();
            CreateMap<UserAddressViewModel, UserAddress>();
            CreateMap<ArtistViewModel, Artists>();
        }
    }
}