namespace CommandContracts.RabbitMQ;

public class RabbitMqConfigValues
{
    public const string SectionName = "RabbitMqSettings";

    public string HostName { get; set; } = "localhost";
    public int Port { get; set; } = 10106;
    public string UserName { get; set; } = "guest";
    public string Password { get; set; } = "guest";

    public class QueueNames
    {
        public const string CartUpdateQueue = nameof(CartUpdateQueue);
    }
}
