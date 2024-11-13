using Autofac.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplicationDemo.Other.MakeHttpRequest
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomSourceController : ControllerBase
    {
        private readonly CustomHttpService _service;
        //这里可以注册自定义服务
        public CustomSourceController(CustomHttpService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<string> Post([FromBody] string option)
        {
            switch (option)
            {
                case "Get":
                    return await _service.GetDataAsync();
                case "GetWithId":
                    return await _service.GetDataAsync(5);
                case "Post":
                    return await _service.PostDataAsync("Seven Chen");
                case "Put":
                    return await _service.PutDataAsync(5, "Seven Chen");
                case "Delete":
                    return await _service.DeleteDataAsync(5);
                default:
                    return "option not found!";
            }
        }
    }
}
