using Microsoft.AspNetCore.SignalR;
using NetworkWebChess.Dtos;
using NetworkWebChess.Services;

namespace NetworkWebChess.Hubs
{
    public class GameHub : Hub
    {
        private readonly GameService _gameService;

        public GameHub(GameService gameService)
        {
            _gameService = gameService;
        }

        public async Task JoinGame(string gameId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId);

            var state = _gameService.GetGameState(Guid.Parse(gameId));
            await Clients.Caller.SendAsync("ReceiveMove", state);
        }

        public async Task MakeMove(string gameId, string playerId, string from, string to)
        {
            var result = await _gameService.MakeMove(
                Guid.Parse(gameId),
                new MoveRequestDto(from, to),
                playerId
            );

            await Clients.Group(gameId).SendAsync("ReceiveMove", result);
        }
    }
}