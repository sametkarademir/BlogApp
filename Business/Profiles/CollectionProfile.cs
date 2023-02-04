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
        CreateMap<SystemLogAddDto, SystemLog>();
        CreateMap<FolderAddDto, Folder>();

        CreateMap<WebInfoAddDto, WebInfo>();
        CreateMap<WebInfoUpdateDto, WebInfo>();
        CreateMap<WebInfo, WebInfoUpdateDto>();

        CreateMap<ResumeAddDto, Resume>();
        CreateMap<ResumeUpdateDto, Resume>();
        CreateMap<Resume, ResumeUpdateDto>();

        CreateMap<ProjectAddDto, Project>();
        CreateMap<ProjectUpdateDto, Project>();
        CreateMap<Project, ProjectUpdateDto>();

    }
}