using System;
using System.IO;
using System.Net;

namespace Demo.App;

internal sealed partial class SimpleHttpServer : IDisposable
{
    private readonly HttpListener _listener = new();

    public SimpleHttpServer(string[] prefixes)
    {
        foreach (var prefix in prefixes)
        {
            _listener.Prefixes.Add(prefix);
        }
    }

    internal void Start()
    {
        _listener.Start();
        _listener.BeginGetContext(OnContext, null);
    }

    internal void Stop()
    {
        _listener.Stop();
    }

    private static string GetPath(string path)
    {
        return Path.Combine(AppContext.BaseDirectory, "wwwroot", path);
    }

    private void OnContext(IAsyncResult result)
    {
        var context = _listener.EndGetContext(result);

        var localPath = context.Request.Url?.LocalPath.TrimStart('/') ?? string.Empty;
        var filePath = GetPath(localPath);
        if (string.IsNullOrEmpty(localPath) || !File.Exists(filePath))
        {
            filePath = GetPath("index.html");
        }

        if (File.Exists(filePath))
        {
            var fileName = Path.GetFileName(filePath);

            if (MimeTypes.TryGetMimeType(fileName, out var mimeType))
            {
                context.Response.Headers.Set("Content-Type", mimeType);

                var buffer = File.ReadAllBytes(filePath);
                context.Response.ContentLength64 = buffer.Length;
                context.Response.OutputStream.Write(buffer);
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.Headers.Set("Content-Type", "text/plain");

                var buffer = System.Text.Encoding.UTF8.GetBytes("500 Internal Server Error");
                context.Response.ContentLength64 = buffer.Length;
                context.Response.OutputStream.Write(buffer);
            }
        }
        else
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            context.Response.Headers.Set("Content-Type", "text/plain");

            var buffer = System.Text.Encoding.UTF8.GetBytes("404 Not Found");
            context.Response.ContentLength64 = buffer.Length;
            context.Response.OutputStream.Write(buffer);
        }

        context.Response.OutputStream.Close();
        context.Response.Close();

        _listener.BeginGetContext(OnContext, null);
    }

    public void Dispose()
    {
        _listener.Close();
    }
}
