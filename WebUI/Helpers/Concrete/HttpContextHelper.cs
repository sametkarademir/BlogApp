using Entities.Concrete;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Options;
using Shared.Entities.Abstract;
using WebUI.Helpers.Interface;

namespace WebUI.Helpers.Concrete
{
    public class HttpContextHelper : IHttpContextHelper
    {
        private readonly UserManager<User> _userManager;
        private readonly SessionOptions _options;

        public HttpContextHelper(UserManager<User> userManager, IOptions<SessionOptions> options)
        {
            _userManager = userManager;
            _options = options.Value;
        }

        public async Task<ServiceInputDto> GetHttpContextContexObject(IHttpContextAccessor httpContextAccessor)
        {
            ServiceInputDto contextObject = new ServiceInputDto();

            var endpoint = httpContextAccessor.HttpContext?.GetEndpoint();
            var actionDescriptor = endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>();
            User? author = await _userManager.GetUserAsync(httpContextAccessor.HttpContext?.User);

            contextObject.RemoteController = actionDescriptor?.ControllerName;
            contextObject.RemoteAction = actionDescriptor?.ActionName;
            contextObject.RemoteAddress = httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            contextObject.RemotePort = httpContextAccessor?.HttpContext?.Connection?.RemotePort.ToString();
            contextObject.UserId = author?.Id.ToString() ?? "null";
            contextObject.Username = author?.UserName ?? "null";
            contextObject.Status = Status.Active;
            
            return contextObject;
        }
    }
}
