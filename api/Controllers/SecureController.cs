using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SecureController : Controller
    {
        [HttpGet]
        public IActionResult Info()
        {
            return new JsonResult(
                from c in User?.Claims
                select new { c.Type, c.Value });
        }
    }
}