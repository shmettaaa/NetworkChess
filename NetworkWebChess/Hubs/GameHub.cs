using Microsoft.AspNetCore.SignalR;
using NetworkWebChess.Dtos;
using NetworkWebChess.Services;

namespace NetworkWebChess.Hubs
{
    public class GameHub : Hub
    {
        private readonly GameService _service;

        public GameHub(GameService service)
        {
            _service = service;
        }

        public async Task JoinGame(string gameId, string playerId, string? preferredColor)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId);

            var (role, state) = _service.JoinGame(Guid.Parse(gameId), playerId, preferredColor);

            await Clients.Caller.SendAsync("Init", new
            {
                role,
                state
            });
        }

        public async Task MakeMove(string gameId, string playerId, string from, string to)
        {
            if (!Guid.TryParse(gameId, out var parsedId))
                return;

            var state = _service.MakeMove(
                parsedId,
                new MoveRequestDto(from, to),
                playerId
            );

            await Clients.Group(gameId).SendAsync("ReceiveMove", state);
        }
    }
}