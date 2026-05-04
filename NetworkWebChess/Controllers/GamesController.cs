using Microsoft.AspNetCore.Mvc;
using NetworkWebChess.Services;

namespace NetworkWebChess.Controllers;

[ApiController]
[Route("api/games")]
public class GamesController : ControllerBase
{
    private readonly GameService _service;
    private readonly GameLifecycleService _lifecycle;

    public GamesController(
        GameService service,
        GameLifecycleService lifecycle)
    {
        _service = service;
        _lifecycle = lifecycle;
    }

    [HttpPost]
    public IActionResult CreateGame()
    {
        var id = _service.CreateNewGame();
        return Ok(new { gameId = id });
    }

    [HttpGet("{gameId}")]
    public IActionResult GetGame(Guid gameId)
    {
        var state = _service.GetGameState(gameId);

        if (state == null)
            return NotFound();

        return Ok(state);
    }

    [HttpDelete("{gameId}")]
    public async Task<IActionResult> DeleteGame(Guid gameId)
    {
        await _lifecycle.DeleteGame(gameId, "rest");
        return Ok();
    }
}