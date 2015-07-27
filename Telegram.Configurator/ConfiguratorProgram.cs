using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Telegram.Configurator
{
    static class ConfiguratorProgram
    {
        private const string JsonContentType = "application/json";
        private const string ApiRoot = "http://localhost:15672/api/";
        private const string VirtualHostName = "test";
        private const string UserName = "guest";
        private const string Password = "guest";
        private static HttpClient _httpClient;

        static void Main(string[] args)
        {
            var task = RunConfiguration();
            task.Wait();
        }

        private static async Task RunConfiguration()
        {
            var credentials = new NetworkCredential(UserName, Password);
            var handler = new HttpClientHandler {Credentials = credentials};
            _httpClient = new HttpClient(handler);

            await CreateVirtualHost();
            await GiveUserPermissions(UserName);
            await CreateExchange("telegram");
            await CreateQueue("telegram");
            await CreateBinding("telegram", "telegram");
        }

        private static async Task CreateVirtualHost()
        {
            var requestUri = string.Format("{0}vhosts/{1}", ApiRoot, VirtualHostName);
            var content = new StringContent("", Encoding.UTF8, JsonContentType);
            await _httpClient.PutAsync(requestUri, content);
        }

        private static async Task GiveUserPermissions(string userName)
        {
            var requestUri = string.Format("{0}permissions/{1}/{2}", ApiRoot, VirtualHostName, userName);
            var permission = new {configure = ".*", write = ".*", read = ".*"};
            var body = new JavaScriptSerializer().Serialize(permission);
            var content = new StringContent(body, Encoding.UTF8, JsonContentType);
            await _httpClient.PutAsync(requestUri, content);
        }

        private static async Task CreateExchange(string queueName)
        {
            var requestUri = string.Format("{0}/exchanges/{1}/{2}", ApiRoot, VirtualHostName, queueName);
            var exchange = new {type = "direct"};
            var body = new JavaScriptSerializer().Serialize(exchange);
            var content = new StringContent(body, Encoding.UTF8, JsonContentType);
            await _httpClient.PutAsync(requestUri, content);
        }

        private static async Task CreateQueue(string queueName)
        {
            var requestUri = string.Format("{0}/queues/{1}/{2}", ApiRoot, VirtualHostName, queueName);
            var content = new StringContent("{}", Encoding.UTF8, JsonContentType);
            await _httpClient.PutAsync(requestUri, content);
        }

        private static async Task CreateBinding(string exchangeName, string queueName)
        {
            var requestUri = string.Format("{0}/bindings/{1}/e/{2}/q/{3}", ApiRoot, VirtualHostName, exchangeName, queueName);
            var content = new StringContent("", Encoding.UTF8, JsonContentType);
            await _httpClient.PostAsync(requestUri, content);
        }
    }
}
