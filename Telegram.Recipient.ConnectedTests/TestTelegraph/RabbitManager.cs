using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using NUnit.Framework;

namespace Telegram.Recipient.ConnectedTests.TestTelegraph
{
    public class RabbitManager:IDisposable
    {
        private readonly HttpClient _httpClient;

        public RabbitManager()
        {
            var credentials = new NetworkCredential("guest", "guest");
            var handler = new HttpClientHandler {Credentials = credentials};
            _httpClient = new HttpClient(handler);
        }

        public async Task PurgeQueue()
        {
            await _httpClient.DeleteAsync("http://localhost:15672/api/test/telegram/contents");
        }

        public async Task PublishMessage(string message)
        {
            var wrappedMessage = new
                {
                    routing_key = "",
                    properties = new {},
                    payload = message,
                    payload_encoding = "string"
                };
            var body = new JavaScriptSerializer().Serialize(wrappedMessage);
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("http://localhost:15672/api/exchanges/test/telegram/publish", content);
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), responseString);
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}