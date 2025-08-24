namespace Backend;

public interface IMqttPublisher
{
    Task PublishAsync(string message);
}
