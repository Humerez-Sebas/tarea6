using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var mqttFactory = new MqttFactory();
var mqttClient = mqttFactory.CreateManagedMqttClient();

mqttClient.ConnectedAsync += _ =>
{
    Console.WriteLine("MQTT connected");
    return Task.CompletedTask;
};

mqttClient.DisconnectedAsync += _ =>
{
    Console.WriteLine("MQTT disconnected");
    return Task.CompletedTask;
};

var mqttOptions = new MqttClientOptionsBuilder()
    .WithTcpServer("localhost", 1883)
    .Build();

await mqttClient.StartAsync(new ManagedMqttClientOptionsBuilder()
    .WithClientOptions(mqttOptions)
    .Build());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/notify", async (Notification notification) =>
{
    await mqttClient.EnqueueAsync("notifications", notification.Message);
    return Results.Ok(new { sent = notification.Message });
});

app.Run();

record Notification(string Message);
