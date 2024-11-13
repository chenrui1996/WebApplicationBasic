using Autofac;
using WebApplicationDemo.DependencyInjection.AutofacDI.Basic;
using WebApplicationDemo.DependencyInjection.AutofacDI.MulImpInterface;
using WebApplicationDemo.DependencyInjection.DefaultDI.Lifecycle.Scoped;
using WebApplicationDemo.DependencyInjection.DefaultDI.Lifecycle.Singleton;
using WebApplicationDemo.DependencyInjection.DefaultDI.Lifecycle.Transient;
using WebApplicationDemo.DependencyInjection.DefaultDI.Registration;
using WebApplicationDemo.DependencyInjection.DefultDI.Basic;
using WebApplicationDemo.DependencyInjection.DefultDI.MulImpInterface;

namespace WebApplicationDemo.DependencyInjection.DefultDI.Extensions
{
    public static class DefaultContainerExtension
    {
        public static void RegisterBusinessContainers(this WebApplicationBuilder builder)
        {
            //循环依赖
            builder.Services.AddTransient<DIRegistrationServiceA>();
            builder.Services.AddTransient<DIRegistrationServiceB>();

            //注入方式
            builder.Services.AddTransient<IDIInjectionService, DIInjectionService>();

            //多实现接口
            builder.Services.AddTransient<IDIMulImpIService, DIMulImpIServiceA>();
            builder.Services.AddTransient<IDIMulImpIService, DIMulImpIServiceB>();

            //作用域
            //瞬时
            builder.Services.AddTransient<ITransientService, TransientService>();
            //作用域
            builder.Services.AddScoped<IScopedService, ScopedService>();
            //单例
            builder.Services.AddSingleton<ISingletonService, SingletonService>();
        }
    }
}
