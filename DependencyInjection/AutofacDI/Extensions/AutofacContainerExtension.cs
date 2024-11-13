using Autofac;
using WebApplicationDemo.DependencyInjection.AutofacDI.Basic;
using WebApplicationDemo.DependencyInjection.AutofacDI.MulImpInterface;
using WebApplicationDemo.DependencyInjection.DefaultDI.Registration;
using WebApplicationDemo.DependencyInjection.DefultDI.MulImpInterface;

namespace WebApplicationDemo.DependencyInjection.AutofacDI.Extensions
{
    public static class AutofacContainerExtension
    {
        public static void RegisterBusinessContainers(this ContainerBuilder containerBuilder)
        {
            //循环依赖
            containerBuilder.RegisterType<AutofacRegistrationServiceA>().PropertiesAutowired();
            containerBuilder.RegisterType<AutofacRegistrationServiceB>().PropertiesAutowired();

            //注入方式
            containerBuilder.RegisterType<AutofacInjectionService>().As<IAutofacInjectionService>();

            //多接口实现
            containerBuilder.RegisterType<AutofacMulImplServiceA>().As<IAutofacMulImplService>().Named<IAutofacMulImplService>("ServiceA");
            containerBuilder.RegisterType<AutofacMulImplServiceB>().As<IAutofacMulImplService>().Named<IAutofacMulImplService>("ServiceB");

            //containerBuilder.RegisterType<IMyService>().AsSelf();


        }
    }
}
