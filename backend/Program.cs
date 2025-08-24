using Backend;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MqttConfig>(builder.Configuration.GetSection("Mqtt"));
builder.Services.AddSingleton<MqttService>();
builder.Services.AddSingleton<IMqttPublisher>(sp => sp.GetRequiredService<MqttService>());
builder.Services.AddHostedService(sp => sp.GetRequiredService<MqttService>());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/notify", async (Notification notification, IMqttPublisher publisher) =>
{
    await publisher.PublishAsync(notification.Message);
    return Results.Ok(new { sent = notification.Message });
});

app.Run();

record Notification(string Message);

public partial class Program { }
