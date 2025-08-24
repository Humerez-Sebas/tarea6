namespace Backend;

public class MqttConfig
{
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 1883;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Topic { get; set; } = "notifications";
}
