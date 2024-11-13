namespace WebApplicationDemo.Middleware.Custom
{
    public static class CustomMiddlewareExtension
    {
        public static void UseCustomMiddleware(this WebApplication app)
        {
            //自定义匿名中间件
            app.Use(async (context, next) =>
            {
                app.Logger.LogInformation("请求：匿名中间件");
                await next();
                app.Logger.LogInformation("响应：匿名中间件");
            });
            //UseWhen类似if,这里是如果访问/put或者/put/xxx会调用该中间件。
            app.Logger.LogInformation("注册自定义匿名中间件（UseWhen）");
            app.UseWhen(context => context.Request.Path.StartsWithSegments("/get"), 
                builder => {
                    builder.Use(async (context, next) => {
                        app.Logger.LogInformation("UseWhen(get)请求：匿名中间件");
                        await next();
                        app.Logger.LogInformation("UseWhen(get)响应：匿名中间件");
                    }); 
                });
            //传统自定义中间件
            app.Logger.LogInformation("传统自定义中间件（DefultCustomMiddleware）");
            app.UseMiddleware<DefultCustomMiddleware>();
            //工厂激活中间件
            app.Logger.LogInformation("工厂激活中间件（FactoryCustomMiddleware）");
            app.UseMiddleware<FactoryCustomMiddleware>();
        }
    }
}
