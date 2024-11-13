using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplicationDemo.DependencyInjection.DefaultDI.Lifecycle.Scoped
{
    public interface IScopedService
    {
        Guid GetOperationId();
    }

    public class ScopedService : IScopedService
    {
        private readonly Guid _operationId;

        public ScopedService()
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
    public class ScopedDIController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;
        public IScopedService? ScopedService1 { get; set; }
        public IScopedService? ScopedService2 { get; set; }
        public ScopedDIController(IServiceProvider serviceProvide)
        {
            _serviceProvider = serviceProvide;
        }

        [HttpGet]
        public IActionResult Get()
        {
            ScopedService1 = _serviceProvider.GetService<IScopedService>();
            ScopedService2 = _serviceProvider.GetService<IScopedService>();
            var id1 = ScopedService1?.GetOperationId();  // 在同一请求中，ID 相同
            var id2 = ScopedService2?.GetOperationId();  // 在同一请求中，ID 相同
            return Content($"Scoped 1: {id1},\r\nScoped 2: {id2}");
        }
    }
}
