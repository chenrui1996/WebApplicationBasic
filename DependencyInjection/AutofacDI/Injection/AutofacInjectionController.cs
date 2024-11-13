using Autofac;
using Microsoft.AspNetCore.Mvc;

namespace WebApplicationDemo.DependencyInjection.AutofacDI.Basic
{
    public interface IAutofacInjectionService
    {
        string PerformTask();
    }

    public class AutofacInjectionService : IAutofacInjectionService
    {
        public string PerformTask()
        {
            return "Executing AutofacInjectionService";
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class AutofacInjectionController : ControllerBase
    {
        private IAutofacInjectionService _myBasicService1;

        [FromServices]
        public IAutofacInjectionService? MyBasicService2 { set; get; }

        public AutofacInjectionController(IAutofacInjectionService myBasicService)
        {
            _myBasicService1 = myBasicService;
        }

        [Route("GetBasicService1")]
        [HttpGet]
        public IActionResult GetBasicService1()
        {
            var res = _myBasicService1.PerformTask();
            return Content($"GetBasicService1: {res}");
        }

        [Route("GetBasicService2")]
        [HttpGet]
        public IActionResult GetBasicService2()
        {
            var res = MyBasicService2?.PerformTask();
            return Content($"GetBasicService2: {res}");
        }

        [Route("GetBasicService3")]
        [HttpGet]
        public IActionResult GetBasicService3([FromServices] IAutofacInjectionService myBasicService)
        {
            var res = myBasicService?.PerformTask();
            return Content($"GetBasicService3: {res}");
        }

        [Route("GetBasicService4")]
        [HttpGet]
        public IActionResult GetBasicService4(ILifetimeScope lifetimeScope)
        {
            var myBasicService = lifetimeScope.Resolve<IAutofacInjectionService>();
            var res = myBasicService?.PerformTask();
            return Content($"GetBasicService4: {res}");
        }
    }
}
