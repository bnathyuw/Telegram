using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Telegram.Sender.ConnectedTests.TestTelegraphist
{
    public class RabbitManager : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly JavaScriptSerializer _javaScriptDeserializer;

        public RabbitManager()
        {
            var credentials = new NetworkCredential("guest", "guest");
            var handler = new HttpClientHandler { Credentials = credentials };
            _httpClient = new HttpClient(handler);
            _javaScriptDeserializer = new JavaScriptSerializer();
        }

        public async Task PurgeQueue()
        {
            await _httpClient.DeleteAsync("http://localhost:15672/api/test/telegram/contents");
        }

        public async Task<List<Message>> GetMessage()
        {
            var message = new
                {
                    count = 1,
                    requeue = false,
                    encoding = "auto"
                };
            var body = new JavaScriptSerializer().Serialize(message);
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("http://localhost:15672/api/queues/test/telegram/get", content);
            var responseString = await response.Content.ReadAsStringAsync();
            return _javaScriptDeserializer.Deserialize<List<Message>>(responseString);
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}