using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ySite.Core.StaticUserRoles;

namespace ySite.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize(Roles = UserRoles.OWNER)]
    public class TeacherController : ControllerBase
    {
        [HttpGet("Get")]
        //[Authorize(Roles = UserRoles.USER)]
        [Authorize(Policy = Policies.ReadPolicy)]
        public IActionResult GetResults()
        {
            return Ok("Hitted");
        }

        [HttpPost("Post")]
        //[Authorize(Roles = UserRoles.ADMIN)]
        [Authorize(Policy = Policies.ReadAndWritePolicy)]
        public IActionResult Post()
        {
            return Ok("Create Hitted");
        }

        [HttpPut("Put")]
        //[Authorize(Roles = UserRoles.ADMIN)]
        [Authorize(Policy = Policies.FullControlPolicy)]
        public IActionResult Put()
        {
            return Ok("Create Hitted");
        }

        [HttpDelete("Delete")]
        //[Authorize(Roles = UserRoles.OWNER)]
        [Authorize(Policy = Policies.FullControlPolicy)]
        public IActionResult Delete()
        {
            return Ok("Delete Hitted");
        }
    }
}
