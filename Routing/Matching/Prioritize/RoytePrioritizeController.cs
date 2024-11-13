using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplicationDemo.Routing.Matching.Prioritize
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoytePrioritizeController : ControllerBase
    {
        // 优先级较高的路由，冲突时优先调用
        [HttpGet("{id:int}", Order = 1)]
        public IActionResult GetRoytePrioritizeById(int id)
        {
            return Ok($"RoytePrioritize id {id}");
        }

        // 优先级较低的路由
        [HttpGet("{name:regex(^\\d{{3}}$)}", Order = 2)]
        public IActionResult GetRoytePrioritizeByName(string name)
        {
            return Ok($"RoytePrioritize name {name}");
        }
    }
}
