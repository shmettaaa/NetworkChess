using Microsoft.AspNetCore.Mvc;
using NetworkWebChess.Dtos;
using NetworkWebChess.Services;

namespace NetworkWebChess.Controllers
{
    [ApiController]
    [Route("api/games")]
    public class GamesController : ControllerBase
    {
        private readonly GameService _gameService;

        public GamesController(GameService gameService)
        {
            _gameService = gameService;
        }

        [HttpPost]
        public IActionResult CreateGame()
        {
            Guid gameId = _gameService.CreateNewGame();

            return CreatedAtAction(
                nameof(GetGameState),
                new { gameId },
                new { gameId, message = "Новая игра успешно создана" }
            );
        }

        [HttpGet("{gameId}")]
        public IActionResult GetGameState(Guid gameId)
        {
            GameStateDto state = _gameService.GetGameState(gameId);
            return Ok(state);
        }


        [HttpPost("{gameId}/move")]
        public async Task<IActionResult> MakeMove(
     Guid gameId,
     [FromBody] MoveRequestDto request,
     [FromQuery] string playerId)   
        {
            if (string.IsNullOrEmpty(playerId))
            {
                return BadRequest("playerId обязателен");
            }

            GameStateDto result = await _gameService.MakeMove(gameId, request, playerId);
            return Ok(result);
        }

        [HttpPost("{gameId}/join")]
        public IActionResult JoinGame(Guid gameId, [FromBody] JoinGameRequestDto request)
        {
            var (role, state) = _gameService.JoinGame(gameId, request.PlayerId);

            return Ok(new
            {
                role,
                state
            });
        }


    }
}