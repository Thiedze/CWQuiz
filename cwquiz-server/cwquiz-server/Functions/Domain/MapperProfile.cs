using AutoMapper;
using CW.Thiedze.Domain;

namespace CW.Thiedze.Functions.Domain
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<User, UserDto>();
        }
    }
}
