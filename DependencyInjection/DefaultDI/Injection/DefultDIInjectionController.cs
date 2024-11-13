
using Microsoft.AspNetCore.Mvc;

namespace WebApplicationDemo.DependencyInjection.DefultDI.Basic
{
    public interface IDIInjectionService
    {
        string PerformTask();
    }

    public class DIInjectionService : IDIInjectionService
    {
        public string PerformTask()
        {
            return "Executing DIInjectionService";
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class DefultDIInjectionController : ControllerBase
    {
        private IDIInjectionService _myBasicService1;

        /// <summary>
        /// 属性注入
        /// </summary>
        [FromServices]
        public IDIInjectionService? MyBasicService2 { set; get; }

        /// <summary>
        /// 构造器注入
        /// </summary>
        /// <param name="myBasicService"></param>
        public DefultDIInjectionController(IDIInjectionService myBasicService)
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
        public IActionResult GetBasicService3([FromServices] IDIInjectionService myBasicService)
        {
            var res = myBasicService?.PerformTask();
            return Content($"GetBasicService3: {res}");
        }

        [Route("GetBasicService4")]
        [HttpGet]
        public IActionResult GetBasicService4(IServiceProvider serviceProvide)
        {
            var myBasicService = serviceProvide.GetService<IDIInjectionService>();
            var res = myBasicService?.PerformTask();
            return Content($"GetBasicService4: {res}");
        }
    }
}
