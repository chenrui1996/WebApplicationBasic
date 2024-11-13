using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using WebApplicationDemo.Routing.Filters.ResourceFilter;

namespace WebApplicationDemo.Routing.Filters.ExceptionFilter
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<CustomExceptionFilter> _logger;

        public CustomExceptionFilter(ILogger<CustomExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            // 记录异常日志（可选）
            _logger.LogError($"Exception caught: {context.Exception.Message}");

            // 自定义响应内容
            context.Result = new ObjectResult(new
            {
                Message = "An unexpected error occurred.",
                Detail = context.Exception.Message // 根据需要控制是否返回详细信息
            })
            {
                StatusCode = 500 // HTTP 状态码
            };

            // 标记异常已处理
            context.ExceptionHandled = true;
        }
    }

    public class AsyncCustomExceptionFilter : IAsyncExceptionFilter
    {
        private readonly ILogger<CustomExceptionFilter> _logger;

        public AsyncCustomExceptionFilter(ILogger<CustomExceptionFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            // 记录异常日志（可选）
            _logger.LogError($"Exception caught: {context.Exception.Message}");

            // 自定义响应内容
            context.Result = new ObjectResult(new
            {
                Message = "An unexpected error occurred.",
                Detail = context.Exception.Message // 根据需要控制是否返回详细信息
            })
            {
                StatusCode = 500 // HTTP 状态码
            };

            // 标记异常已处理
            context.ExceptionHandled = true;

            // 模拟异步操作
            await Task.CompletedTask;
        }
    }
}
