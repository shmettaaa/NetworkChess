using Microsoft.AspNetCore.Mvc;
using NetworkWebChess.Services;

namespace NetworkWebChess.Controllers;

[ApiController]
[Route("api/games")]
public class GamesController : ControllerBase
{
    private readonly GameService _service;
    private readonly GameLifecycleService _lifecycle;
    private readonly AuthService _auth;

    public GamesController(
        GameService service,
        GameLifecycleService lifecycle,
        AuthService auth)
    {
        _service = service;
        _lifecycle = lifecycle;
        _auth = auth;
    }

    [HttpPost]
    public async Task<IActionResult> CreateGame(
        [FromHeader(Name = "X-Session-Token")] string? token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return Unauthorized(new
            {
                message = "Missing token"
            });
        }

        var user = await _auth.GetUserByTokenAsync(token);

        if (user == null)
        {
            return Unauthorized(new
            {
                message = "Invalid token"
            });
        }

        var id = await _service.CreateNewGameAsync();

        return Ok(new
        {
            gameId = id
        });
    }

    [HttpGet("{gameId}")]
    public async Task<IActionResult> GetGame(Guid gameId)
    {
        var state = await _service.GetGameStateAsync(gameId);

        if (state == null)
        {
            return NotFound(new
            {
                message = "Game not found"
            });
        }

        return Ok(state);
    }

    [HttpDelete("{gameId}")]
    public async Task<IActionResult> DeleteGame(Guid gameId)
    {
        await _lifecycle.DeleteGame(
            gameId,
            "manual_delete"
        );

        return Ok(new
        {
            message = "Game deleted"
        });
    }
}