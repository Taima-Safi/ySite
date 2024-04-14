using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ySite.Auth.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BaseController : ControllerBase
{
    protected string GetUserId()
        => User.FindFirst(ClaimTypes.NameIdentifier).Value;
}