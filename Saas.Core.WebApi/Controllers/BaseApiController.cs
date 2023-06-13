using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Saas.Core.WebApi.Controllers
{
    /// <summary>
    /// api基类(默认需要认证才能访问);不需要认证的action,请加上[AllowAnonymous]
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class BaseApiController : Controller
    {

    }
}