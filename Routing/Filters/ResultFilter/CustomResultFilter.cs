using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using WebApplicationDemo.Routing.Filters.ResourceFilter;

namespace WebApplicationDemo.Routing.Filters.ResultFilter
{
    public class CustomResultFilter : IResultFilter
    {
        private readonly ILogger<CustomResultFilter> _logger;
        public CustomResultFilter(ILogger<CustomResultFilter> logger)
        {
            _logger = logger;
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            // 结果生成前执行的逻辑
            _logger.LogInformation("Before result execution");

            // 可以在这里对 context.Result 进行修改
            if (context.Result is ObjectResult objectResult)
            {
                objectResult.Value = new
                {
                    Data = objectResult.Value,
                    Message = "Processed by CustomResultFilter"
                };
            }
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            // 结果生成后执行的逻辑
            _logger.LogInformation("After result execution");
        }
    }

    public class AsyncCustomResultFilter : IAsyncResultFilter
    {
        private readonly ILogger<CustomResultFilter> _logger;
        public AsyncCustomResultFilter(ILogger<CustomResultFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var startTime = DateTime.UtcNow;

            // 执行结果生成和返回
            await next();

            var endTime = DateTime.UtcNow;
            var processingTime = endTime - startTime;

            _logger.LogInformation("Request processed in {Time} ms", processingTime.TotalMilliseconds);
            _logger.LogDebug("Request processed in {Time} ms", processingTime.TotalMilliseconds);
            _logger.LogError("Request processed in {Time} ms", processingTime.TotalMilliseconds);
        }
    }
}
