using BlogAPI.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers{
    [ApiController]
    [Route("")]    
    public class HomeController : ControllerBase{

        //HealthCheck
        [HttpGet("")]
        [ApiKey]
        public IActionResult Get(){
            return Ok();
        }
    }
}