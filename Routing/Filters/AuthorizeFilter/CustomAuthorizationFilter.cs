using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;

namespace WebApplicationDemo.Routing.Filters.AuthorizeFilter
{
    public class CustomAuthorizationFilter : IAuthorizationFilter
    {
        private readonly ILogger<CustomAuthorizationFilter> _logger;

        public CustomAuthorizationFilter(ILogger<CustomAuthorizationFilter> logger)
        {
            _logger = logger;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                if (context.HttpContext.User.Identity == null)
                {
                    return;
                }

                if (!context.HttpContext.User.Identity.IsAuthenticated)
                { 
                    // 返回 401 Unauthorized
                    // 设置result会导致短路
                    // context.Result = new UnauthorizedResult();
                    return;
                }

                var expTimeStampStr = context.HttpContext.User.Claims
                    .Where(r => r.Type == JwtRegisteredClaimNames.Exp)
                    .Select(x => x.Value).FirstOrDefault();

                if (!string.IsNullOrEmpty(expTimeStampStr))
                {
                    var expTimeStamp = Convert.ToInt64(expTimeStampStr);
                    DateTime expTime = new DateTime(1970, 1, 1).AddSeconds(expTimeStamp);

                    var expTimeSeconds = (expTime - DateTime.UtcNow).TotalSeconds;
                    //小于5分钟，标注自动刷新，客户端拿到自动刷新
                    if (expTimeSeconds < 300)
                    {
                        context.HttpContext.Response.Headers.Append("ExpFlag", "1");
                        _logger.LogInformation($"{expTimeSeconds}s后过期, 自动刷新......");
                        return;
                    }
                    _logger.LogInformation($"{expTimeSeconds}s后过期, 无需自动刷新......");
                }



            }
            catch(Exception e)
            {
                _logger.LogError(e.Message + "\r\n" + e.StackTrace);
            }
        }
    }
}
