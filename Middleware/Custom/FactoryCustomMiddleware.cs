
using Microsoft.Extensions.Logging;

namespace WebApplicationDemo.Middleware.Custom
{
    public class FactoryCustomMiddleware : IMiddleware
    {
        private readonly ILogger _logger;
        //这里可以注册自定义服务
        public FactoryCustomMiddleware(ILogger<FactoryCustomMiddleware> logger)
        {
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            // 请求前的操作，例如记录日志
            _logger.LogInformation("请求：工厂中间件");

            //处理context.Request
            var requestPath = context.Request.Path; // 请求路径
            var method = context.Request.Method;    // 请求方法 (GET, POST 等)
            _logger.LogInformation($"requestPath:{requestPath}; method:{method}");

            //处理context.Response
            var originalBodyStream = context.Response.Body;
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                // 调用下一个中间件
                await next(context); 

                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();

                // 在返回客户端之前修改响应
                var modifiedResponseText = responseText.Replace("original", "modified");

                // 写回修改后的响应
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                await context.Response.WriteAsync(modifiedResponseText);
            }
            context.Response.Body = originalBodyStream;
            // 请求后的操作，例如修改响应
            _logger.LogInformation("响应：工厂中间件");
        }
    }
}
