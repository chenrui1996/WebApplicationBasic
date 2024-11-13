using Autofac.Core;
using Autofac.Features.AttributeFilters;
using Microsoft.AspNetCore.Mvc;

namespace WebApplicationDemo.DependencyInjection.AutofacDI.MulImpInterface
{
    public interface IAutofacMulImplService
    {
        string PerformTask();
    }

    public class AutofacMulImplServiceA : IAutofacMulImplService
    {
        public string PerformTask()
        {
            return "Executing AutofacMulImplServiceA";
        }
    }

    public class AutofacMulImplServiceB : IAutofacMulImplService
    {
        public string PerformTask()
        {
            return "Executing AutofacMulImplServiceB";
        }
    }


    [Route("api/[controller]")]
    [ApiController]
    public class AutofacMulImpInterfaceController : ControllerBase
    {
        private IAutofacMulImplService _myServiceA;
        private IAutofacMulImplService _myServiceB;

        private int _servicesCount;

        public AutofacMulImpInterfaceController(IEnumerable<IAutofacMulImplService> myServices)
        {
            _servicesCount = myServices.Count();
            _myServiceA = myServices.First();
            _myServiceB = myServices.Last();
        }

        [Route("GetServiceA1")]
        [HttpGet]
        public IActionResult GetServiceA1()
        {
            var res = _myServiceA.PerformTask();
            return Content($"GetServiceA: {res} in {_servicesCount} services");
        }

        [Route("GetServiceB1")]
        [HttpGet]
        public IActionResult GetServiceB1()
        {
            var res = _myServiceB.PerformTask();
            return Content($"GetServiceB: {res}  in {_servicesCount} services");
        }

        [Route("GetServiceA2")]
        [HttpGet]
        public IActionResult GetServiceA2([KeyFilter("ServiceA")] IAutofacMulImplService myServiceA)
        {
            var res = myServiceA.PerformTask();
            return Content($"GetServiceA: {res}");
        }

        [Route("GetServiceB2")]
        [HttpGet]
        public IActionResult GetServiceB2([KeyFilter("ServiceB")] IAutofacMulImplService myServiceB)
        {
            var res = myServiceB.PerformTask();
            return Content($"GetServiceB: {res}");
        }
    }
}
