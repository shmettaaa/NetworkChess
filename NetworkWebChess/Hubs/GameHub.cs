using Microsoft.AspNetCore.SignalR;
using NetworkWebChess.Dtos;
using NetworkWebChess.Services;

namespace NetworkWebChess.Hubs;

public class GameHub : Hub
{
    private readonly GameService _service;
    private readonly GameLifecycleService _lifecycle;

    public GameHub(
        GameService service,
        GameLifecycleService lifecycle)
    {
        _service = service;
        _lifecycle = lifecycle;
    }

    public async Task JoinGame(string gameId, string playerId, string? preferredColor)
    {
        if (!Guid.TryParse(gameId, out var id))
            return;

        await Groups.AddToGroupAsync(Context.ConnectionId, gameId);

        var (role, state) = _service.JoinGame(id, playerId, preferredColor);

        if (state == null)
        {
            await Clients.Caller.SendAsync("GameDeleted", "not_found");
            return;
        }

        await Clients.Caller.SendAsync("Init", new { role, state });
    }

    public async Task MakeMove(string gameId, string playerId, string from, string to)
    {
        if (!Guid.TryParse(gameId, out var id))
            return;

        var state = _service.MakeMove(
            id,
            new MoveRequestDto(from, to),
            playerId
        );

        if (state == null)
        {
            await Clients.Caller.SendAsync("GameDeleted", "not_found");
            return;
        }

        await Clients.Group(gameId)
            .SendAsync("ReceiveMove", state);
    }

    public async Task LeaveGame(string gameId)
    {
        if (!Guid.TryParse(gameId, out var id))
            return;

        await _lifecycle.DeleteGame(id, "manual");
    }
}