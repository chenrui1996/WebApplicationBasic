using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApplicationDemo.Routing.Filters.ServiceFilter
{
    public class CustomMessageFilter : IActionFilter
    {
        private readonly string _message;

        public CustomMessageFilter(string message)
        {
            _message = message;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine($"Message: {_message}");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // 处理逻辑
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(CustomMessageFilter), Arguments = new object[] { "Hello from TypeFilter!" })]
    public class CustomTypeFilterController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { Message = "Testing TypeFilter with CustomMessageFilter" });
        }
    }
}
