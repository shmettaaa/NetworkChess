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

        [HttpPost("{gameId}/join")]
        public IActionResult Join(Guid gameId, [FromBody] JoinGameRequestDto request)
        {
            var result = _service.JoinGame(gameId, request.PlayerId);
            return Ok(result);
        }

        [HttpPost("{gameId}/move")]
        public async Task<IActionResult> Move(
            Guid gameId,
            [FromBody] MoveRequestDto request,
            [FromQuery] string playerId)
        {
            var result = await _service.MakeMove(gameId, request, playerId);
            return Ok(result);
        }
    }
}