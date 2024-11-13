

using Microsoft.AspNetCore.Mvc;

namespace WebApplicationDemo.DependencyInjection.DefaultDI.Registration
{
    public class DIRegistrationServiceA
    {
        private Func<DIRegistrationServiceB> _registrationServiceB;
        
        public DIRegistrationServiceA(Func<DIRegistrationServiceB> registrationServiceB)
        {
            _registrationServiceB = registrationServiceB;
        }

        public string GetStr()
        {
            return "Executing DIInjectionServiceA";
        }

        public string PerformTask()
        {
            return _registrationServiceB.Invoke().GetStr();
        }
    }

    public class DIRegistrationServiceB
    {
        private Func<DIRegistrationServiceA> _registrationServiceA;

        public DIRegistrationServiceB(Func<DIRegistrationServiceA> registrationServiceA)
        {
            _registrationServiceA = registrationServiceA;
        }

        public string GetStr()
        {
            return "Executing DIInjectionServiceB";
        }

        public string PerformTask()
        {
            return _registrationServiceA.Invoke().GetStr();
        }


    }

    [Route("api/[controller]")]
    [ApiController]
    public class DefaultDIRegistratierController : ControllerBase
    {
        [Route("GetServiceA")]
        [HttpGet]
        public IActionResult GetServiceA([FromServices] DIRegistrationServiceA myService)
        {
            var res = myService?.PerformTask();
            return Content($"GetServiceA: {res}");
        }

        [Route("GetServiceB")]
        [HttpGet]
        public IActionResult GetServiceB([FromServices] DIRegistrationServiceB myService)
        {
            var res = myService?.PerformTask();
            return Content($"GetServiceB: {res}");
        }
    }
}
