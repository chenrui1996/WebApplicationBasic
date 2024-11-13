using Microsoft.AspNetCore.Mvc;
using WebApplicationDemo.Middleware.Custom;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplicationDemo.Other.MakeHttpRequest
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomTargetController : ControllerBase
    {
        private readonly ILogger _logger;
        //这里可以注册自定义服务
        public CustomTargetController(ILogger<CustomTargetController> logger)
        {
            _logger = logger;
        }

        // GET: api/<CustomTargetController>
        [HttpGet]
        public string Get()
        {
            _logger.LogInformation(this.HttpContext.Request.Host.ToString() + " Requested!");
            return this.HttpContext.Request.Host.ToString() + " Requested!";
        }

        // GET api/<CustomTargetController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            _logger.LogInformation(this.HttpContext.Request.Host.ToString() + " Get [" + id + "]");
            return this.HttpContext.Request.Host.ToString() + " Get [" + id + "]";
        }

        // POST api/<CustomTargetController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
            _logger.LogInformation(this.HttpContext.Request.Host.ToString() + " Post [" + value + "]");
        }

        // PUT api/<CustomTargetController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            _logger.LogInformation(this.HttpContext.Request.Host.ToString() + " Put [" + id.ToString() + value + "]");
        }

        // DELETE api/<CustomTargetController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _logger.LogInformation(this.HttpContext.Request.Host.ToString() + " Delete [" + id.ToString() + "]");

        }
    }
}
