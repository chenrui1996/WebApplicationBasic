using System.Text;

namespace WebApplicationDemo.Middleware.Custom
{
    public class DefultCustomMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        // 通过构造函数注入 RequestDelegate，代表下一个中间件
        public DefultCustomMiddleware(ILogger<DefultCustomMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        // Invoke 方法定义了该中间件的逻辑
        public async Task InvokeAsync(HttpContext context)
        {
            // 请求前的操作，例如记录日志
            _logger.LogInformation("请求：约定中间件");

            // 处理context.Request
            if (context.Request.Method == "POST")
            {
                context.Request.EnableBuffering(); // 允许重复读取请求体

                using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, leaveOpen: true))
                {
                    var body = await reader.ReadToEndAsync();
                    Console.WriteLine($"Request Body: {body}");
                    context.Request.Body.Position = 0; // 重置请求体位置以便下一个中间件继续读取
                }
            }

            // 调用下一个中间件
            await _next(context);

            //处理context.Response
            // 自定义响应
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync("This is a custom response from middleware.");

            // 请求后的操作，例如修改响应
            _logger.LogInformation("响应：约定中间件");
        }
    }
}
