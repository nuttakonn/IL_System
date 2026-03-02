using ServiceStack.Redis;
using System.Web.Configuration;
namespace EB_Service.DAL
{
    public class RedisHelper
    {
        private readonly RedisEndpoint _redisEndpoint;

        public RedisHelper()
        {
            var host = WebConfigurationManager.AppSettings["RedisHost"].ToString();
            int port = int.Parse(WebConfigurationManager.AppSettings["RedisPort"].ToString());
            var password = WebConfigurationManager.AppSettings["RedisPassword"].ToString();
            _redisEndpoint = new RedisEndpoint(host, port, password);
        }

        public bool IsKeyExists(string key)
        {
            using (var client = new RedisClient(_redisEndpoint))
            {
                if (client.ContainsKey(key))
                    return true;
                else
                    return false;
            }
        }

        public string GetStrings(string key)
        {
            using (var client = new RedisClient(_redisEndpoint))
            {
                return client.GetValue(key);
            }
        }
    }
}