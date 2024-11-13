using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplicationDemo.DependencyInjection.DefaultDI.Registration;

namespace WebApplicationDemo.Host
{
    [Route("api/[controller]")]
    [ApiController]
    public class HostTestController : ControllerBase
    {
        [Route("GetWebHostEnvironment")]
        [HttpGet]
        public IActionResult GetWebHostEnvironment([FromServices] IWebHostEnvironment env)
        {
            return Content($"Content Root Path: {env.ContentRootPath}; Web Root Path: {env.WebRootPath}; ");
        }
    }
}
