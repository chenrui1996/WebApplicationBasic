using Autofac;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace WebApplicationDemo.Host.Extensions
{
    public static class HostExtension
    {
        public static void ConfigWebApplicationBuilder(this WebApplicationBuilder builder)
        {
            //builder.Host.UseContentRoot("/path/to/content/root");
            //builder.WebHost.UseUrls("http://*:9001");
            builder.WebHost.ConfigureKestrel(options =>
            {
                // 限制请求大小
                options.Limits.MaxRequestBodySize = 10 * 1024 * 1024;
                // 请求头读取超时时间
                options.Limits.RequestHeadersTimeout = TimeSpan.FromSeconds(30);
                // 最大并发连接数
                options.Limits.MaxConcurrentConnections = 100;
                // WebSocket等升级连接的最大并发数
                options.Limits.MaxConcurrentUpgradedConnections = 100;
                //通过限制请求速率来防止 DoS 攻击：
                //MinRequestBodyDataRate：设置请求体的最小数据传输速率。
                //bytesPerSecond：最低传输速率（字节 / 秒）。
                //gracePeriod：在此时间内，服务器不会强制执行速率限制
                options.Limits.MinRequestBodyDataRate =
                    new MinDataRate(bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));
                // 设置请求队列大小
                // 请求行的最大长度
                options.Limits.MaxRequestLineSize = 8192;
                // 请求缓冲区的大小
                options.Limits.MaxRequestBufferSize = 65536; 
            });
        }

        public static void ConfigWebApplication(this WebApplication app, WebApplicationBuilder builder)
        {
            //app.Urls.Add("http://*:9001");

            //var port = builder.Configuration["Port"] ?? "3000";
            //app.Urls.Add($"http://*:{port}");
        }
    }
}
