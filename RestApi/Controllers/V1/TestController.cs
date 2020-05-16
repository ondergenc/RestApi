using System;
using Microsoft.AspNetCore.Mvc;

namespace RestApi.Controllers.V1
{
    public class TestController : Controller
    {
        [HttpGet("api/v1/user")]
        public IActionResult Get()
        {
            return Ok(new { name = "User1" });
        }
    }
}
