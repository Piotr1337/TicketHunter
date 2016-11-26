using AutoMapper;
using TicketHunter.Domain.Entities;
using TicketHunter.Models;
using TicketReservation.Models;

namespace TicketHunter.Mappings
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DomainToViewModelMappings"; }
        }

        protected override void Configure()
        {
            CreateMap<Events, AdminViewModel>();
            CreateMap<Ticket, TicketViewModel>();
            CreateMap<UserProfileDetails, UserDataViewModel>();
            CreateMap<Artists, ArtistViewModel>();
            CreateMap<Artists, Artists>();
            CreateMap<Events, Events>();
            CreateMap<UserProfileDetails, UserProfileDetails>();
            CreateMap<UserAddress, UserAddress>();
        }
    }
}