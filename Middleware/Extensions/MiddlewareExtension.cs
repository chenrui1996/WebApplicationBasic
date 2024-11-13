using Autofac;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using System.Reflection;
using WebApplicationDemo.Middleware.Custom;
using WebApplicationDemo.Routing.Filters.ResourceFilter;
using WebApplicationDemo.Routing.Matching.Prefix;
using WebApplicationDemo.Routing.Matching.Repetition;

namespace WebApplicationDemo.Host.Extensions
{
    public static class MiddlewareExtension
    {
        public static void UseMiddlewares(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    // 根据模块和版本生成多个文档
                    var apiAssembly = Assembly.GetExecutingAssembly();
                    var apiDescriptions = apiAssembly.GetTypes()
                        .Where(t => t.GetCustomAttributes<ApiDescriptionAttribute>().Any())
                        .Select(t => t.GetCustomAttribute<ApiDescriptionAttribute>())
                        .OrderBy(t => t?.Position ?? int.MaxValue).ThenBy(t => t?.Title).ThenBy(t => t?.Version)
                        .Distinct();

                    foreach (var desc in apiDescriptions)
                    {
                        if (desc != null)
                        {
                            if (string.IsNullOrEmpty(desc.Version))
                            {
                                options.SwaggerEndpoint($"/swagger/{desc.Title}/swagger.json", $"{desc.Title} API");
                            }
                            else
                            {
                                options.SwaggerEndpoint($"/swagger/{desc.Title}-{desc.Version}/swagger.json", $"{desc.Title} API {desc.Version}");
                            }
                        }
                    }

                    options.SwaggerEndpoint("/swagger/NoGroup/swagger.json", "无分组");
                });
            }
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

                    if (exceptionHandlerPathFeature?.Error != null)
                    {
                        var errorResponse = new
                        {
                            Message = "An unexpected error occurred.",
                            Detail = exceptionHandlerPathFeature.Error.Message  // 错误信息
                        };

                        await context.Response.WriteAsJsonAsync(errorResponse);  // 返回 JSON 错误响应
                    }
                });
            });
            //启用静态文件访问
            //一般操作
            //app.UseStaticFiles();
            //指定路径
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "staticfiles")),
                RequestPath = "/staticfiles",
                //禁用文件
                OnPrepareResponse = ctx =>
                {
                    if (ctx.File.Name.EndsWith(".json"))
                    {
                        ctx.Context.Response.StatusCode = 403;
                        ctx.Context.Response.ContentLength = 0;
                        ctx.Context.Response.Body = Stream.Null;
                        //缓存时间
                        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=600");
                    }
                }
            });

            //启用文件浏览
            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "staticfiles")),
                RequestPath = "/staticfiles"
            });
            // 启用速率限制器
            app.UseRateLimiter();
            // 使用路由匹配中间件
            app.UseRouting();
            // 使用自定义中间件
            //app.UseCustomMiddleware();
            // 启用响应缓存中间件
            app.UseResponseCaching();
            // 使用身份验证中间件
            app.UseAuthentication();
            // 使用授权中间件
            app.UseAuthorization();
            //.Net 6 之后推荐使用顶级语句替换UseEndpoints.
            //app.UseEndpoints(endpoints =>  // 执行终结点
            //{
            //    endpoints.MapControllers(a);   // 启用控制器路由,
                
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "api/{controller}/{action}/{id?}");
            //});
            // 启用 CORS 中间件(全局)
            // 需要配合依赖注入的其他服务
            // see Program.cs
            // builder.Services.AddCors……
            app.UseCors();
            app.UseCors("AllowSpecificOrigin");  // 使用名为 "AllowSpecificOrigin" 的策略

            // 防止路由重复
            // app.UseMiddleware<RouteLoggingMiddleware>();
            // 启用控制器路由
            app.MapControllers();

            app.MapControllerRoute(
                name: "default",
                pattern: "api/{controller=Home}/{action=Index}/{id?}");
        }
    }
}
