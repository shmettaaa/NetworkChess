using Microsoft.AspNetCore.Mvc;
using NetworkWebChess.Dtos;
using NetworkWebChess.Services;

namespace NetworkWebChess.Controllers
{
    [ApiController]
    [Route("api/games")]
    public class GamesController : ControllerBase
    {
        private readonly GameService _service;

        public GamesController(GameService service)
        {
            _service = service;
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
            return Ok(_service.GetGameState(gameId));
        }

        [HttpDelete("{gameId}")]
        public IActionResult DeleteGame(Guid gameId)
        {
            var removed = _service.TryRemoveGame(gameId);

            if (!removed)
                return BadRequest("Game is not finished or not found");

            return Ok();
        }
    }
}