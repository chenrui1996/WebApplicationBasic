using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplicationDemo.Routing.Matching.Template
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiTemplateController : ControllerBase
    {
        /// <summary>
        /// 简单路由
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("products")]
        public IActionResult GetProducts()
        {
            // 处理逻辑
            return Ok();
        }

        /// <summary>
        /// 带参数的路由
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("products/{id?}")]
        public IActionResult GetProduct(int id)
        {
            // 根据 id 处理逻辑
            return Ok(id);
        }

        /// <summary>
        /// 带约束的路由
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("blog/{year:int}/{month:int}/{day:int}")]
        public IActionResult Archive(int year, int month, int day)
        {
            // 处理逻辑
            return Ok($"{year}-{month}-{day}");
        }

        /// <summary>
        /// 带约束的路由
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("message/{message:regex(^\\d{{3}}-\\d{{2}}-\\d{{4}}$)}")]
        public IActionResult Message(string message)
        {
            // 处理逻辑
            return Ok(message);
        }
    }
}
