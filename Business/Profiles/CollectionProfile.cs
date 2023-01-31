using AutoMapper;
using Business.Dtos;
using Entities.Concrete;

namespace Business.Profiles;

public class CollectionProfile : Profile
{
    public CollectionProfile()
    {
        CreateMap<UserAddDto, User>();
        CreateMap<UserUpdateDto, User>();
        CreateMap<User, UserUpdateDto>();

        CreateMap<RoleAddDto, Role>();

        CreateMap<WebInfoAddDto, WebInfo>();
        CreateMap<WebInfoUpdateDto, WebInfo>();
        CreateMap<WebInfo, WebInfoUpdateDto>();
        
    }
}