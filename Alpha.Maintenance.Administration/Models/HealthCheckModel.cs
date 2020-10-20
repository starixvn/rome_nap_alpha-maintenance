namespace Alpha.Maintenance.Administration.Models
{
    public class HealthCheckModel
    {
        public string RedisConnection { get; private set; }

        public bool IsOk { get; private set; } = true;

        public string Message { get; private set; } = "Good";

        public HealthCheckModel(string redisConnection)
        {
            RedisConnection = redisConnection;
        }

        public HealthCheckModel(string redisConnection, string message)
            : this(redisConnection)
        {
            IsOk = false;
            Message = message;
        }
    }
}
