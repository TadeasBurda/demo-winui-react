using Microsoft.AspNetCore.SignalR;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Demo.App;

public record PingMessage()
{
    public required string Message { get; init; }
}

public interface IPingHub
{
    Task Ping(PingMessage message);
}

internal sealed partial class PingHub : Hub<IPingHub>
{
    public const string Path = "/ping";

    public async Task Ping(PingMessage message)
    {
        await Clients.All.Ping(message);
    }
}
