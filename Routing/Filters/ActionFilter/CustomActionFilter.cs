using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApplicationDemo.Routing.Filters.ActionFilter
{
    public class CustomActionFilter : IActionFilter
    {
        private readonly ILogger<CustomActionFilter> _logger;

        public CustomActionFilter(ILogger<CustomActionFilter> logger)
        {
            _logger = logger;
        }

        // 在操作执行之前调用
        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("Before the action executes.");
        }

        // 在操作执行之后调用
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("After the action executes.");
        }
    }

    public class AsyncCustomActionFilter : IAsyncActionFilter
    {
        private readonly ILogger<AsyncCustomActionFilter> _logger;

        public AsyncCustomActionFilter(ILogger<AsyncCustomActionFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _logger.LogInformation("Before the action executes(AsyncCustomActionFilter).");

            await next();

            _logger.LogInformation("After the action executes(AsyncCustomActionFilter.");
        }
    }
}
