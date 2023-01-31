using AutoMapper;
using Data.Repositories.Interface;
using Shared.Utilities.Extenstions.Interface;

namespace Business.Services.Abstract;

public class BaseManager
{
    protected IUow Uow;
    protected IMapper Mapper;
    protected IDateTimeExtensions DateTimeExtensions;
    protected IGenerateBase64SessionIdExtenstion GenerateBase64SessionIdExtenstion;

    public BaseManager(IUow uow, IMapper mapper, IDateTimeExtensions dateTimeExtensions, IGenerateBase64SessionIdExtenstion generateBase64SessionIdExtenstion)
    {
        Uow = uow;
        Mapper = mapper;
        DateTimeExtensions = dateTimeExtensions;
        GenerateBase64SessionIdExtenstion = generateBase64SessionIdExtenstion;
    }

}