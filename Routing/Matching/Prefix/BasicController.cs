using Microsoft.AspNetCore.Mvc;

namespace WebApplicationDemo.Routing.Matching.Prefix
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ApiDescription("ModuleA", "v1", "A模组测试")]
    public abstract class BasicController_V1 : ControllerBase
    {
    }

    [Route("api/v2/[controller]")]
    [ApiController]
    [ApiDescription("ModuleA", "v2", "A模组测试")]
    public abstract class BasicController_V2 : ControllerBase
    {
    }
}
