using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebApplicationDemo.Routing.Filters.AuthorizeFilter
{
    public static class AuthorizeFilterExtension
    {
        public static void UseSimpleAuthorizeService(this WebApplicationBuilder builder)
        {
            // 配置身份验证
            builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "DemoIssuer",
                        ValidAudience = "DemoAudience",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MyNewSuperSecretKey@01234567890123456789"))
                    };
                });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdministratorRole", policy =>
                     policy.RequireRole("Admin"));
            });

        }
    }
}
