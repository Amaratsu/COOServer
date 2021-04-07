//using Microsoft.AspNetCore.Authorization;

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace COO.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public abstract class ApiController : ControllerBase
    {
        protected int UserId()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var id = claimsIdentity?.FindFirst(ClaimTypes.Sid)?.Value;
            return id != null ? int.Parse(id) : default;
        }
    }
}
