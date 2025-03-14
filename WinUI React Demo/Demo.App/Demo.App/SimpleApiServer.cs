using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Demo.App;

public record Response()
{
    public required string Message { get; init; }
}

public sealed partial class SimpleApiServer : IDisposable
{
    private readonly CancellationTokenSource _cts = new();
    private readonly Task _task;
    private readonly WebApplication _host;

    internal SimpleApiServer()
    {
        var builder = WebApplication.CreateBuilder();

        // Add CORS services
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            });
        });

        builder.Services.AddSignalR();
        builder.Services.AddHostedService<PingWorker>();

        _host = builder.Build();

        // Use CORS middleware
        _host.UseCors();

        _host.MapHub<PingHub>(PingHub.Path);

        _host.MapGet("/", () => new Response() { Message = "Hello from Minimal API in WinUI 3!" });

        _task = _host.RunAsync(_cts.Token);
    }

    public void Dispose()
    {
        _cts.Cancel();
        try
        {
            _task.Wait();
        }
        catch (AggregateException ex)
        {
            ex.Handle(e => e is TaskCanceledException);
        }

        _cts.Dispose();
        _host.DisposeAsync().AsTask().Wait();
    }
}
