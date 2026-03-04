using StackExchange.Redis;
namespace ILSCREEN_UI.Services.Common
{
    public class RedisClientService
    {
        private IConnectionMultiplexer _connection;
        public RedisClientService(IConnectionMultiplexer connection)
        {
            _connection = connection;
        }
        public bool KeyExists(string key)
        {
            try
            {
                return _connection.GetDatabase().KeyExists(key);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
