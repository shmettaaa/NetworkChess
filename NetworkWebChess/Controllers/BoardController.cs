using Microsoft.AspNetCore.Mvc;
using NetworkWebChess.Dtos;
using NetworkWebChess.Services;

namespace NetworkWebChess.Controllers
{
    [ApiController]
    [Route("api/board")]
    public class BoardController : ControllerBase
    {
        private readonly BoardService _boardService;

        public BoardController(BoardService boardService)
        {
            _boardService = boardService;
        }

        [HttpGet("initial")]
        public IActionResult GetInitialBoard()
        {
            BoardStateDto dto = _boardService.GetInitialBoard();
            return Ok(dto);
        }

        [HttpPost("move")]
        public IActionResult MakeMove([FromBody] MoveRequestDto request)
        {
            BoardStateDto result = _boardService.MakeMove(request);
            return Ok(result);
        }
    }
}