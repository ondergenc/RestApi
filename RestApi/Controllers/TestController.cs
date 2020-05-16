using System;
using Microsoft.AspNetCore.Mvc;

namespace RestApi.Controllers
{
    public class TestController : Controller
    {
        [HttpGet("api/user")]
        public IActionResult Get()
        {
            return Ok(new { name = "User1" });
        }
    }
}
