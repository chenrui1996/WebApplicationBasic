
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
            #region ����(WebApplicationBuilder)
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                Args = args,
                // Look for static files in webroot
                ApplicationName = typeof(Program).Assembly.FullName,
                //ContentRootPath = Directory.GetCurrentDirectory(),
                //EnvironmentName = Environments.Development,
                WebRootPath = "customwwwroot"
            });
            // �������Ĭ�ϵ���־�ṩ����
            builder.Logging.ClearProviders();
            // ��ӵ�����̨
            //builder.Logging.AddConsole();
            // ��ӵ����Դ���
            //builder.Logging.AddDebug();

            // �� appsettings.json ���� Serilog
            builder.Host.UseSerilog((context, services, configuration) => configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services) // ���������ע���л�ȡ����
                .Enrich.FromLogContext()     // �����������Ϣ�������� ID��
            );

            builder.ConfigWebApplicationBuilder();
            #endregion

            #region ����ע��(Dependency injection)
            //Ĭ��DI
            builder.RegisterBusinessContainers();
            //Autofac
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
            {
                //�Ƽ�ʹ����չ������ע�Ἧ�д���
                containerBuilder.RegisterBusinessContainers();
            });
            #endregion

            #region ����ע�����������
            //ע�� MVC ����������
            //ע��; ���� AOT ���벻��������ʱ���ɴ��룬���ڴ���̬����ʱ����һЩ�����ԣ����練�䡢��̬����ȡ�
            //ASP.NET Core �Ĳ��ֹ�����������Щ���ƣ���������� AOT ʱ���ܻᴥ�����档
            builder.Services.AddControllers(options =>
            {
                // options.Conventions.Add(new RoutePrefixConvention("v1"));
                //ȫ�ֹ�����������ȫ�֣�ÿ��Controller����ִ��
                // ע��Ϊȫ�ֹ�����
                options.Filters.Add<CustomAuthorizationFilter>();

                // ע��Ϊȫ����Դ������
                //options.Filters.Add<CustomResourceFilter>();
                //options.Filters.Add<AsyncCustomResourceFilter>();

                // ע��Ϊȫ���쳣������
                options.Filters.Add<CustomExceptionFilter>();
                //options.Filters.Add<AsyncCustomExceptionFilter>();

                // ע��Ϊȫ�ֲ���������
                //options.Filters.Add<CustomActionFilter>();
                //options.Filters.Add<AsyncCustomActionFilter>();

                // ע��Ϊȫ�ֽ��������
                //options.Filters.Add<CustomResultFilter>();
                options.Filters.Add<AsyncCustomResultFilter>();
            });

            // Swagger����
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                // ����ģ��Ͱ汾���ɶ���ĵ�
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
                //û�м����Եķֵ����NoGroup��
                options.SwaggerDoc("NoGroup", new OpenApiInfo
                {
                    Title = "�޷���"
                });

                //�жϽӿڹ����ĸ�����
                options.DocInclusionPredicate((docName, apiDescription) =>
                {
                    if (docName == "NoGroup")
                    {
                        //������ΪNoGroupʱ��ֻҪû�����ԵĶ����������
                        return string.IsNullOrEmpty(apiDescription.GroupName);
                    }
                    else
                    {
                        return apiDescription.GroupName == docName;
                    }
                });

                //// ��Ӱ�ȫ����
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer" // ע�� "Bearer" ����ʹ�ô�д "B"
                });

                // ���ȫ�ֵİ�ȫҪ��
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

            //���Cors
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("https://example.com")  // ����Ŀ���������Դ
                           .AllowAnyHeader()                    // ������������ͷ
                           .AllowAnyMethod();                   // �������� HTTP ����
                });
                //���AllowSpecificOrigin Cors
                //����Ϊ��ʾ�����Ϸ������ظ�
                options.AddPolicy("AllowSpecificOrigin", builder =>
                {
                    builder.WithOrigins("https://example.com")
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            // �����������������
            builder.Services.AddRateLimiter(options =>
            {
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                        factory: partition => new FixedWindowRateLimiterOptions
                        {
                            AutoReplenishment = true,
                            PermitLimit = 5,  // ���������
                            Window = TimeSpan.FromSeconds(10),  // ʱ�䴰��
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 0,  // ������г���
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

            builder.Services.AddScoped<CustomResultFilter>(); // ע�������Ϊ Scoped ����

            //��Ȩ��Ȩ����֤
            builder.UseSimpleAuthorizeService();

            builder.Services.AddHealthChecks();

            // ע�� HttpClient ��Ϊ����
            builder.Services.AddHttpClient();

            builder.Services.AddScoped<CustomHttpService>();
            #endregion

            var app = builder.Build();
            //app.MapGet("/", () => "Hello World!");
            #region �м��
            app.UseSerilogRequestLogging();
            app.UseMiddlewares();
            app.UseHealthChecks("/health");
            //app.UseEndpointConcepts();
            //app.UseTerminalMiddlewareConcepts();
            #endregion

            #region ����(WebApplication)
            //app.Run("http://localhost:3000");
            app.ConfigWebApplication(builder);
            #endregion

            app.Run();
        }
    }
}
