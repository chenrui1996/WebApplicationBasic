
using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Globalization;
using System.Reflection;
using System.Threading.RateLimiting;
using WebApplicationDemo.DependencyInjection.AutofacDI.Extensions;
using WebApplicationDemo.DependencyInjection.DefultDI.Basic;
using WebApplicationDemo.DependencyInjection.DefultDI.Extensions;
using WebApplicationDemo.Host.Extensions;
using WebApplicationDemo.Middleware.Custom;
using WebApplicationDemo.Other.MakeHttpRequest;
using WebApplicationDemo.Routing.Concepts;
using WebApplicationDemo.Routing.Filters.ActionFilter;
using WebApplicationDemo.Routing.Filters.AuthorizeFilter;
using WebApplicationDemo.Routing.Filters.ExceptionFilter;
using WebApplicationDemo.Routing.Filters.ResourceFilter;
using WebApplicationDemo.Routing.Filters.ResultFilter;
using WebApplicationDemo.Routing.Matching.Constraints;
using WebApplicationDemo.Routing.Matching.Prefix;

namespace WebApplicationDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            #region 主机(WebApplicationBuilder)
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                Args = args,
                // Look for static files in webroot
                ApplicationName = typeof(Program).Assembly.FullName,
                //ContentRootPath = Directory.GetCurrentDirectory(),
                //EnvironmentName = Environments.Development,
                WebRootPath = "customwwwroot"
            });
            // 清除所有默认的日志提供程序
            builder.Logging.ClearProviders();
            // 添加到控制台
            //builder.Logging.AddConsole();
            // 添加到调试窗口
            //builder.Logging.AddDebug();

            // 从 appsettings.json 配置 Serilog
            builder.Host.UseSerilog((context, services, configuration) => configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services) // 允许从依赖注入中获取服务
                .Enrich.FromLogContext()     // 添加上下文信息（如请求 ID）
            );

            builder.ConfigWebApplicationBuilder();
            #endregion

            #region 依赖注入(Dependency injection)
            //默认DI
            builder.RegisterBusinessContainers();
            //Autofac
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
            {
                //推荐使用拓展方法将注册集中处理
                containerBuilder.RegisterBusinessContainers();
            });
            #endregion

            #region 依赖注入的其他服务
            //注入 MVC 控制器服务
            //注意; 由于 AOT 编译不会在运行时生成代码，它在处理动态功能时存在一些局限性，比如反射、动态代理等。
            //ASP.NET Core 的部分功能依赖于这些机制，因此在启用 AOT 时可能会触发警告。
            builder.Services.AddControllers(options =>
            {
                // options.Conventions.Add(new RoutePrefixConvention("v1"));
                //全局过滤器作用于全局，每个Controller都会执行
                // 注册为全局过滤器
                options.Filters.Add<CustomAuthorizationFilter>();

                // 注册为全局资源过滤器
                //options.Filters.Add<CustomResourceFilter>();
                //options.Filters.Add<AsyncCustomResourceFilter>();

                // 注册为全局异常过滤器
                options.Filters.Add<CustomExceptionFilter>();
                //options.Filters.Add<AsyncCustomExceptionFilter>();

                // 注册为全局操作过滤器
                //options.Filters.Add<CustomActionFilter>();
                //options.Filters.Add<AsyncCustomActionFilter>();

                // 注册为全局结果过滤器
                //options.Filters.Add<CustomResultFilter>();
                options.Filters.Add<AsyncCustomResultFilter>();
            });

            // Swagger服务
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                // 根据模块和版本生成多个文档
                var apiAssembly = Assembly.GetExecutingAssembly();
                var apiDescriptions = apiAssembly.GetTypes()
                    .Where(t => t.GetCustomAttributes<ApiDescriptionAttribute>().Any())
                    .Select(t => t.GetCustomAttribute<ApiDescriptionAttribute>())
                    .Distinct();

                foreach (var desc in apiDescriptions)
                {
                    if (desc != null)
                    {
                        if (string.IsNullOrEmpty(desc.Version))
                        {
                            options.SwaggerDoc($"{desc.Title}", new OpenApiInfo { Title = $"{desc.Title} API", Version = desc.Version, Description = desc.Description, });
                        }
                        else
                        {
                            options.SwaggerDoc($"{desc.Title}-{desc.Version}", new OpenApiInfo
                            {
                                Title = $"{desc.Title} API",
                                Version = desc.Version,
                                Description = desc.Description,
                            });
                        }
                    }
                }
                //没有加特性的分到这个NoGroup上
                options.SwaggerDoc("NoGroup", new OpenApiInfo
                {
                    Title = "无分组"
                });

                //判断接口归于哪个分组
                options.DocInclusionPredicate((docName, apiDescription) =>
                {
                    if (docName == "NoGroup")
                    {
                        //当分组为NoGroup时，只要没加特性的都属于这个组
                        return string.IsNullOrEmpty(apiDescription.GroupName);
                    }
                    else
                    {
                        return apiDescription.GroupName == docName;
                    }
                });

                //// 添加安全定义
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer" // 注意 "Bearer" 必须使用大写 "B"
                });

                // 添加全局的安全要求
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });

            });

            //添加Cors
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("https://example.com")  // 允许的跨域请求来源
                           .AllowAnyHeader()                    // 允许所有请求头
                           .AllowAnyMethod();                   // 允许所有 HTTP 方法
                });
                //添加AllowSpecificOrigin Cors
                //仅作为演示，和上方代码重复
                options.AddPolicy("AllowSpecificOrigin", builder =>
                {
                    builder.WithOrigins("https://example.com")
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            // 添加速率限制器服务
            builder.Services.AddRateLimiter(options =>
            {
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                        factory: partition => new FixedWindowRateLimiterOptions
                        {
                            AutoReplenishment = true,
                            PermitLimit = 5,  // 最大请求数
                            Window = TimeSpan.FromSeconds(10),  // 时间窗口
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 0,  // 请求队列长度
                        }));
                options.OnRejected = (context, token) =>
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    context.HttpContext.Response.WriteAsync("Too many requests. Please try again later.");

                    return new ValueTask();
                };

            });

            builder.Services.AddTransient<FactoryCustomMiddleware>();

            builder.Services.AddRouting(options =>
                    options.ConstraintMap.Add("noZeroes", typeof(NoZeroesRouteConstraint)));

            builder.Services.AddScoped<CustomResultFilter>(); // 注册过滤器为 Scoped 服务

            //鉴权与权限验证
            builder.UseSimpleAuthorizeService();

            builder.Services.AddHealthChecks();

            // 注册 HttpClient 作为服务
            builder.Services.AddHttpClient();

            builder.Services.AddScoped<CustomHttpService>();
            #endregion

            var app = builder.Build();
            //app.MapGet("/", () => "Hello World!");
            #region 中间件
            app.UseSerilogRequestLogging();
            app.UseMiddlewares();
            app.UseHealthChecks("/health");
            //app.UseEndpointConcepts();
            //app.UseTerminalMiddlewareConcepts();
            #endregion

            #region 主机(WebApplication)
            //app.Run("http://localhost:3000");
            app.ConfigWebApplication(builder);
            #endregion

            app.Run();
        }
    }
}
