using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BaseController : ControllerBase
    {
        private IMediator? _mediator;
        protected IMediator Mediator => (_mediator ??= HttpContext.RequestServices.GetService<IMediator>())!;
    }
}
