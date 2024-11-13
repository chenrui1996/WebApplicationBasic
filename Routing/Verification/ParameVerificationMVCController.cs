using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace WebApplicationDemo.Routing.Verification
{
    public class ParameVerificationMVCController : Controller
    {
        public IActionResult GetValueByRouteId([FromRoute, Range(0, 9)] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(new { Id = id });
        }

        public IActionResult GetValueByRouteRequestId([FromRoute] ParamsVerificationQueryRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(new { request.Id });
        }

        public IActionResult GetValueById([FromQuery, Range(21, 29)] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(new { Id = id });
        }

        public IActionResult GetValueByQueryRequestId([FromQuery] ParamsVerificationQueryRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(new { request.Id });
        }

        [HttpPost]
        public IActionResult GetValueByFormRequestId([FromForm] ParamsVerificationModelRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(request);
        }

        [HttpPost]
        public IActionResult GetValueByBodyRequestId([FromBody] ParamsVerificationModelRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(request);
        }
    }
}
