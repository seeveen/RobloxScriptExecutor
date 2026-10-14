using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using RobloxScriptExecutor.Integration;

namespace RobloxScriptExecutor.Integration
{
    public record LogEntry(string Level, string Message, DateTime Timestamp);

    public class BridgeServer
    {
        private WebApplication? _app;
        private ExecutionRequest? _pendingRequest;
        private readonly int _port;

        // Event to notify UI about new logs
        public event Action<LogEntry>? OnLogReceived;

        public BridgeServer(int port = 8080)
        {
            _port = port;
        }

        public async Task StartAsync()
        {
            var builder = WebApplication.CreateBuilder();

            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenLocalhost(_port);
            });

            _app = builder.Build();

            _app.MapGet("/poll", () =>
            {
                if (_pendingRequest == null)
                {
                    return Results.Ok(new BridgeResponse(false, null, null));
                }

                var response = new BridgeResponse(true, _pendingRequest.ScriptContent, _pendingRequest.ScriptName);
                _pendingRequest = null;
                return Results.Ok(response);
            });

            _app.MapPost("/execute", async (HttpContext context) =>
            {
                using var reader = new System.IO.StreamReader(context.Request.Body);
                var body = await reader.ReadToEndAsync();

                try
                {
                    var request = JsonSerializer.Deserialize<ExecutionRequest>(body);
                    if (request != null)
                    {
                        _pendingRequest = request;
                        return Results.Ok(new { status = "Queued" });
                    }
                }
                catch (JsonException)
                {
                    return Results.BadRequest("Invalid JSON");
                }

                return Results.BadRequest("Request failed");
            });

            // Endpoint for Roblox Studio Plugin to send logs back
            _app.MapPost("/log", async (HttpContext context) =>
            {
                using var reader = new System.IO.StreamReader(context.Request.Body);
                var body = await reader.ReadToEndAsync();
                try
                {
                    var log = JsonSerializer.Deserialize<LogEntry>(body);
                    if (log != null)
                    {
                        OnLogReceived?.Invoke(log);
                        return Results.Ok();
                    }
                }
                catch { }
                return Results.BadRequest();
            });

            await _app.RunAsync();
        }

        public async Task StopAsync()
        {
            if (_app != null)
            {
                await _app.StopAsync();
                await _app.DisposeAsync();
            }
        }

        public int Port => _port;
    }
}
