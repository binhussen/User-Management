using AutoMapper;
using Entities.DTOs;
using Entities.Models;

namespace API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Users
            CreateMap<User, UserDto>();
            CreateMap<User, UserForCreationDto>();
            CreateMap<UserForUpdateDto, User>().ReverseMap();
        }
    }
}
