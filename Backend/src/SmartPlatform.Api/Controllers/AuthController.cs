using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SecureApi.Controllers;


[ApiController]
[Route("api")]
public class AuthController : ControllerBase
{
    [HttpGet("public")]
    public IActionResult Public()
    {
        return Ok("Public endpoint");
    }

    [Authorize]
    [HttpGet("secure")]
    public IActionResult Secure()
    {
        return Ok("Authenticated user");
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin")]
    public IActionResult AdminOnly()
    {
        return Ok("Admin only endpoint");
    }

    [Authorize(Roles = "Buyer")]
    [HttpGet("buyer")]
    public IActionResult BuyerOnly()
    {
        return Ok("Buyer only endpoint");
    }

    [Authorize(Roles = "Seller")]
    [HttpGet("seller")]
    public IActionResult SellerOnly()
    {
        return Ok("Seller only endpoint");
    }
}
