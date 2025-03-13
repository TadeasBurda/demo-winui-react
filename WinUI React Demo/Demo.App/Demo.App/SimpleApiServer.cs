using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.App;

public record Response()
{
    public required string Message { get; init; }
}

public sealed partial class SimpleApiServer : IDisposable
{
    private WebApplication? _host;
    private Task? _serverTask;
    private readonly CancellationTokenSource _cts = new();

    internal void Start()
    {
        _serverTask = Task.Run(() =>
        {
            var builder = WebApplication.CreateBuilder();

            // Add CORS services
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            // Use CORS middleware
            app.UseCors();

            app.MapGet("/", () => new Response() { Message = "Hello from Minimal API in WinUI 3!" });

            _host = app;
            _host.RunAsync(_cts.Token).Wait();
        });
    }

    internal void Stop()
    {
        if (_host != null)
        {
            _cts.Cancel();
            _host.StopAsync().Wait();
        }
    }

    public void Dispose()
    {
        Stop();
        _serverTask?.Dispose();
        _cts.Dispose();
    }
}
