using AutoMapper;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.Accounts;
using Mo8tareb_RoomRentalWebApp.DAL.Models;

namespace Mo8tareb_RoomRentalWebApp.BL.Dtos
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserForRegistrationDto, AppUser>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));

            //            // application user
            CreateMap<AppUser, ApplicationUserDto>()
               .ForMember(d => d.FirstName, map => map.MapFrom(s => s.FirstName))
               .ForMember(d => d.LastName, map => map.MapFrom(s => s.LastName))
               //.ForMember(d =>d.Email, map => map.MapFrom(s => s.Email))
               .ReverseMap();

        }
    }
}
