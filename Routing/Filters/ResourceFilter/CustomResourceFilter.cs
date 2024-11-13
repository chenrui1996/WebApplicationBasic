using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApplicationDemo.Routing.Filters.ResourceFilter
{
    public class CustomResourceFilter : IResourceFilter
    {
        private readonly ILogger<CustomResourceFilter> _logger;

        public CustomResourceFilter(ILogger<CustomResourceFilter> logger)
        {
            _logger = logger;
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            // 在资源执行之前执行
            _logger.LogInformation("Before executing resource.");
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            // 在资源执行之后执行
            _logger.LogInformation("After executing resource.");
        }
    }

    public class AsyncCustomResourceFilter : IAsyncResourceFilter
    {
        private readonly ILogger<AsyncCustomResourceFilter> _logger;

        public AsyncCustomResourceFilter(ILogger<AsyncCustomResourceFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            _logger.LogInformation("Before executing async resource.");

            // 执行下一个过滤器或操作
            var resultContext = await next();

            _logger.LogInformation("After executing async resource.");
        }
    }
}
