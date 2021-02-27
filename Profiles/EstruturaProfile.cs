using AutoMapper;
using Estrutura.Web.API.Dtos;
using Estrutura.Web.API.Entities;

namespace Estrutura.Web.API.Profiles
{
    public class EstruturaProfile : Profile
    {
        public EstruturaProfile()
        {
             CreateMap<UserDto, User>();

              CreateMap<User, UserDto>()
                .ForMember(
                    dest => dest.Password,
                    opt => opt.MapFrom(src => "******")
                );
        }
    }
}