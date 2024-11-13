using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplicationDemo.Routing.Params
{
    public class ValueModel
    {
        public int Id { set; get; }

        public string? Name { set; get; }
    }


    [Route("api/[controller]")]
    [ApiController]
    public class RouteParamsController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult GetValueById(int id)
        {
            // 根据 id 获取产品信息
            return Ok(new { Id = id });
        }

        [HttpGet]
        public IActionResult GetValueByQuery([FromQuery] int id, [FromQuery] string name)
        {
            // 处理查询字符串参数 id 和 name
            return Ok(new { Id = id, Name = name });
        }

        [HttpPost("createProductFromForm")]
        public IActionResult CreateProductFromForm([FromForm] ValueModel model)
        {
            // 处理表单数据中的 product 对象
            return Ok(model);
        }

        [HttpPost("createProductFromBody")]
        public IActionResult CreateProductFromBody([FromBody] ValueModel model)
        {
            // 处理请求体中的 product 对象
            return Ok(model);
        }


    }
}
