using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplicationDemo.Routing.Matching.Prefix;

namespace WebApplicationDemo.Routing.Filters.AuthorizeFilter
{
    public class UserLogin
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
    }

    [Route("api/v1/[controller]")]
    [ApiController]
    [ApiDescription("AuthorizationSimple", "v1", "权限测试：admin, password")]
    public abstract class AuthorizeBasicController : ControllerBase
    {
    }

    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizeLoginController : AuthorizeBasicController
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLogin login)
        {
            // 在这里，您可以验证用户的凭据
            // 简单的模拟
            if (login.Username == "admin" && login.Password == "password") 
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, login.Username),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MyNewSuperSecretKey@01234567890123456789"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: "DemoIssuer",
                    audience: "DemoAudience",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(6),
                    signingCredentials: creds);

                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
            }

            return Unauthorized();
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthorizeFilterController : AuthorizeBasicController
    {
        [Authorize]
        [HttpGet("AuthorizeOnlyAction")]
        public IActionResult AuthorizeOnlyAction()
        {
            // 仅 Admin 角色可以访问
            return Ok("AuthorizeOnlyAction");
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("AdminOnlyAction")]
        public IActionResult AdminOnlyAction()
        {
            // 仅 Admin 角色可以访问
            return Ok("AdminOnlyAction"); 
        }

        [Authorize(Policy = "RequireAdministratorRole")]
        [HttpGet("AdminAction")]
        public IActionResult AdminAction()
        {
            // 根据策略进行访问控制
            return Ok("RequireAdministratorRole"); 
        }
    }
}
