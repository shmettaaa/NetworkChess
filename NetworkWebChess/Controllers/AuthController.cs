using Microsoft.AspNetCore.Mvc;
using NetworkWebChess.Dtos;
using NetworkWebChess.Services;

namespace NetworkWebChess.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _auth;

    public AuthController(AuthService auth)
    {
        _auth = auth;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        RegisterRequestDto request)
    {
        var result = await _auth.RegisterAsync(request);

        if (result == null)
        {
            return BadRequest(new
            {
                message = "Nickname already exists"
            });
        }

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        LoginRequestDto request)
    {
        var result = await _auth.LoginAsync(request);

        if (result == null)
        {
            return Unauthorized(new
            {
                message = "Invalid credentials"
            });
        }

        return Ok(result);
    }

    [HttpGet("me")]
    public async Task<IActionResult> Me(
    [FromHeader(Name = "X-Session-Token")]
    string? token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return Unauthorized();
        }

        var profile =
            await _auth.GetProfileAsync(token);

        if (profile == null)
        {
            return Unauthorized();
        }

        return Ok(profile);
    }


}