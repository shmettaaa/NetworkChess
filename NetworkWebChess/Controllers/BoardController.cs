using NetworkChess.ChessModels;
using Microsoft.AspNetCore.Mvc;
using NetworkWebChess.Dtos;

namespace NetworkWebChess.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BoardController : ControllerBase
    {
        private readonly Board _board;

        public BoardController()
        {
            _board = new Board();
        }

        [HttpGet("initial")]
        public IActionResult GetInitialBoard()
        {
            string initialFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

            var dto = new BoardStateDto(initialFen, "Начальная позиция успешно загружена");

            return Ok(dto);
        }
    }
}