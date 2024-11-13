using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplicationDemo.DependencyInjection.DefaultDI.Lifecycle.Singleton
{
    public interface ISingletonService
    {
        Guid GetOperationId();
    }

    public class SingletonService : ISingletonService
    {
        private readonly Guid _operationId;

        public SingletonService()
        {
            _operationId = Guid.NewGuid();
        }

        public Guid GetOperationId()
        {
            return _operationId;
        }
    }



    [Route("api/[controller]")]
    [ApiController]
    public class SingletonDIController : ControllerBase
    {
        private readonly ISingletonService _singletonService1;
        private readonly ISingletonService _singletonService2;

        public SingletonDIController(ISingletonService singletonService1, ISingletonService singletonService2)
        {
            _singletonService1 = singletonService1;
            _singletonService2 = singletonService2;
        }

        [Route("GetOperationId1")]
        [HttpGet]
        public IActionResult GetOperationId1()
        {
            var id1 = _singletonService1.GetOperationId();  // 永远相同的 ID
            return Content($"Singleton 1: {id1}");
        }

        [Route("GetOperationId2")]
        [HttpGet]
        public IActionResult GetOperationId2()
        {
            var id2 = _singletonService2.GetOperationId();  // 永远相同的 ID
            return Content($"Singleton 2: {id2}");
        }
    }
}
