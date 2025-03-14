using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Demo.App;

internal sealed partial class PingWorker(IServiceProvider serviceProvider) : BackgroundService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<PingHub, IPingHub>>();

        while (!stoppingToken.IsCancellationRequested)
        {
            await hubContext.Clients.All.Ping(
                new PingMessage() { Message = DateTime.Now.ToString() }
            );
            await Task.Delay(1000, stoppingToken);
        }
    }
}
