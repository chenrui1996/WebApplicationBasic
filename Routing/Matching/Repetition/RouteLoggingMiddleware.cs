using WebApplicationDemo.Middleware.Custom;

namespace WebApplicationDemo.Routing.Matching.Repetition
{
    public class RouteLoggingMiddleware : IMiddleware
    {
        private readonly ILogger _logger;

        public RouteLoggingMiddleware(ILogger<FactoryCustomMiddleware> logger)
        {
            _logger = logger;
        }


        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _logger.LogInformation($"Matched route:");

            var routeData = context.GetRouteData();
            if (routeData != null)
            {
                _logger.LogInformation($"Matched route: {routeData.Values["controller"]}/{routeData.Values["action"]}");
            }
            await next(context);

           
        }
    }
}
