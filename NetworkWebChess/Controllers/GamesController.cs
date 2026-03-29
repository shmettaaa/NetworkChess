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
                nameof(GetCurrentGameState),     
                new { gameId = gameId },
                new { gameId = gameId, message = "Новая игра успешно создана" }
            );
        }

        [HttpGet("current")]
        public IActionResult GetCurrentGameState()
        {
            BoardStateDto state = _gameService.GetCurrentGameState();
            return Ok(state);
        }

        [HttpPost("move")]
        public IActionResult MakeMove([FromBody] MoveRequestDto request)
        {
            BoardStateDto result = _gameService.MakeMove(request);
            return Ok(result);
        }
    }
}