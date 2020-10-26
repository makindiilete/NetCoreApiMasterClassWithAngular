using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    //All other controllers will inherit this class so they have the attribute here added to their class by default and in turn they will inherit from ControllerBase ds class inherits from
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {

    }
}
