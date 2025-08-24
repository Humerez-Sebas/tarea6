using System.Net.Http.Json;
using Backend;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

public class NotifyEndpointTests
{
    [Fact]
    public async Task PostNotify_ReturnsOk()
    {
        await using var app = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton<IMqttPublisher, FakePublisher>();
                });
            });

        var client = app.CreateClient();
        var response = await client.PostAsJsonAsync("/notify", new { message = "test" });
        response.EnsureSuccessStatusCode();
    }

    private class FakePublisher : IMqttPublisher
    {
        public Task PublishAsync(string message) => Task.CompletedTask;
    }
}
