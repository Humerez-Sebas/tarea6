using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;

namespace Backend;

public class MqttService : IHostedService, IMqttPublisher
{
    private readonly IManagedMqttClient _client;
    private readonly MqttConfig _config;

    public MqttService(IOptions<MqttConfig> options)
    {
        _config = options.Value;
        _client = new MqttFactory().CreateManagedMqttClient();

        _client.ConnectedAsync += _ =>
        {
            Console.WriteLine("MQTT connected");
            return Task.CompletedTask;
        };

        _client.DisconnectedAsync += _ =>
        {
            Console.WriteLine("MQTT disconnected");
            return Task.CompletedTask;
        };
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var options = new MqttClientOptionsBuilder()
            .WithTcpServer(_config.Host, _config.Port)
            .WithCredentials(_config.Username, _config.Password)
            .Build();

        var managedOptions = new ManagedMqttClientOptionsBuilder()
            .WithClientOptions(options)
            .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
            .Build();

        await _client.StartAsync(managedOptions);
    }

    public Task StopAsync(CancellationToken cancellationToken) => _client.StopAsync();

    public Task PublishAsync(string message) => _client.EnqueueAsync(_config.Topic, message);
}
