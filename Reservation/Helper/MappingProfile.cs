using AutoMapper;
using Reservation.Core.Entities;
using Reservation.DTOs;

namespace Reservation.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserDto, User>().ReverseMap();
        }
    }
}
