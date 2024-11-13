using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplicationDemo.DependencyInjection.DefultDI.Basic;

namespace WebApplicationDemo.DependencyInjection.DefaultDI.Registration
{
    public class AutofacRegistrationServiceA
    {
        public Lazy<AutofacRegistrationServiceB>? RegistrationServiceB{ set; get; }
        
        public string GetStr()
        {
            return "Executing AutofacRegistrationServiceA";
        }

        public string PerformTask()
        {
            return RegistrationServiceB?.Value.GetStr() ?? "";
        }
    }

    public class AutofacRegistrationServiceB
    {
        public Lazy<AutofacRegistrationServiceA>? RegistrationServiceA { set; get; }

        public string GetStr()
        {
            return "Executing AutofacRegistrationServiceB";
        }

        public string PerformTask()
        {
            return RegistrationServiceA?.Value.GetStr()??"";
        }


    }

    [Route("api/[controller]")]
    [ApiController]
    public class AutofacRegistratierController : ControllerBase
    {
        [Route("GetServiceA")]
        [HttpGet]
        public IActionResult GetServiceA([FromServices] AutofacRegistrationServiceA myService)
        {
            var res = myService?.PerformTask();
            return Content($"GetServiceA: {res}");
        }

        [Route("GetServiceB")]
        [HttpGet]
        public IActionResult GetServiceB([FromServices] AutofacRegistrationServiceB myService)
        {
            var res = myService?.PerformTask();
            return Content($"GetServiceB: {res}");
        }
    }
}
