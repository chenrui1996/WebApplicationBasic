using Microsoft.AspNetCore.Mvc;

namespace WebApplicationDemo.DependencyInjection.DefultDI.MulImpInterface
{
    public interface IDIMulImpIService
    {
        string PerformTask();
    }

    public class DIMulImpIServiceA : IDIMulImpIService
    {
        public string PerformTask()
        {
            return "Executing DIMulImpIServiceA";
        }
    }

    public class DIMulImpIServiceB : IDIMulImpIService
    {
        public string PerformTask()
        {
            return "Executing DIMulImpIServiceB";
        }
    }


    [Route("api/[controller]")]
    [ApiController]
    public class DefaultDIMulImpInterfaceController : ControllerBase
    {
        private IDIMulImpIService _myServiceA;
        private IDIMulImpIService _myServiceB;

        private int _servicesCount;

        public DefaultDIMulImpInterfaceController(IEnumerable<IDIMulImpIService> myServices)
        {
            _servicesCount = myServices.Count();
            _myServiceA = myServices.First();
            _myServiceB = myServices.Last();
        }

        [Route("GetServiceA")]
        [HttpGet]
        public IActionResult GetServiceA()
        {
            var res = _myServiceA.PerformTask();
            return Content($"GetServiceA: {res} in {_servicesCount} services");
        }

        [Route("GetServiceB")]
        [HttpGet]
        public IActionResult GetServiceB()
        {
            var res = _myServiceB.PerformTask();
            return Content($"GetServiceB: {res} in {_servicesCount} services");
        }
    }
}
