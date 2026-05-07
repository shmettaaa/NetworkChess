using Microsoft.AspNetCore.Mvc;
using NetworkWebChess.Services;

namespace NetworkWebChess.Controllers;

[ApiController]
[Route("api/games")]
public class GamesController : ControllerBase
{
    private readonly GameService _service;
    private readonly GameLifecycleService _lifecycle;

    public GamesController(GameService service, GameLifecycleService lifecycle)
    {
        _service = service;
        _lifecycle = lifecycle;
    }

    [HttpPost]
    public async Task<IActionResult> CreateGame()
    {
        var id = await _service.CreateNewGameAsync();
        return Ok(new { gameId = id });
    }

    [HttpGet("{gameId}")]
    public async Task<IActionResult> GetGame(Guid gameId)
    {
        var state = await _service.GetGameStateAsync(gameId);

        if (state == null)
            return NotFound(new { message = "Game not found" });

        return Ok(state);
    }

    [HttpDelete("{gameId}")]
    public async Task<IActionResult> DeleteGame(Guid gameId)
    {
        await _lifecycle.DeleteGame(gameId, "manual_delete");
        return Ok(new { message = "Game deleted" });
    }
}