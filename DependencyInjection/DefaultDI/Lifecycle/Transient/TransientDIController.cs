using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplicationDemo.DependencyInjection.DefaultDI.Lifecycle.Transient
{
    public interface ITransientService
    {
        Guid GetOperationId();
    }

    public class TransientService : ITransientService
    {
        private readonly Guid _operationId;

        public TransientService()
        {
            _operationId = Guid.NewGuid();  // 每次实例化时生成一个新的 GUID
        }

        public Guid GetOperationId()
        {
            return _operationId;
        }
    }


    [Route("api/[controller]")]
    [ApiController]
    public class TransientDIController : ControllerBase
    {
        private readonly ITransientService _transientService1;
        private readonly ITransientService _transientService2;

        public TransientDIController(ITransientService transientService1, ITransientService transientService2)
        {
            _transientService1 = transientService1;
            _transientService2 = transientService2;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var id1 = _transientService1.GetOperationId();  // 每次生成新的 ID
            var id2 = _transientService2.GetOperationId();  // 即使在同一请求中，ID 也是不同的
            return Content($"Transient 1: {id1},\r\n Transient 2: {id2}");
        }
    }
}
