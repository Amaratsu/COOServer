using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace COO.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public abstract class ApiController : ControllerBase
    {
    }
}
