using Microsoft.AspNetCore.SignalR;
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

        public async Task JoinGame(string gameId, string playerId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId);

            var (role, state) = _service.JoinGame(Guid.Parse(gameId), playerId);

            await Clients.Caller.SendAsync("Init", new
            {
                role,
                state
            });
        }

        public async Task MakeMove(string gameId, string playerId, string from, string to)
        {
            await _service.MakeMove(
                Guid.Parse(gameId),
                new NetworkWebChess.Dtos.MoveRequestDto(from, to),
                playerId
            );
        }


        public async Task SendChatMessage(string gameId, string playerId, string text)
        {
            await Clients.Group(gameId).SendAsync("ChatMessage", new
            {
                playerId,
                text,
                timestamp = DateTime.UtcNow
            });
        }




    }
}