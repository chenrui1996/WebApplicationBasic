using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplicationDemo.Routing.Matching.Constraints
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouteConstraintsController : ControllerBase
    {
        [HttpGet]
        [Route("getId/{id:int:min(1)}")]
        public IActionResult GetId(int id)
        {
            return Ok(id);
        }

        [HttpGet]
        [Route("getMessage/{message:regex(^\\d{{3}}-\\d{{2}}-\\d{{4}}$)}")]
        public IActionResult Message(string message)
        {
            return Ok(message);
        }

        [HttpGet("usersid/{id:noZeroes}")]
        public IActionResult GetId(string id) => Content(id);
    }
}
