using Shared.Entities.Abstract;
using WebUI.Models;

namespace WebUI.Helpers.Interface;

public interface IHttpContextHelper
{
    Task<ServiceInputDto> GetHttpContextContexObject(IHttpContextAccessor httpContextAccessor);
}