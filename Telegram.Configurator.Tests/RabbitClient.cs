using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using NUnit.Framework;

namespace Telegram.Configurator.Tests
{
    public class RabbitClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly JavaScriptSerializer _javaScriptSerializer;

        public RabbitClient()
        {
            var credentials = new NetworkCredential("guest", "guest");
            var handler = new HttpClientHandler { Credentials = credentials };
            _httpClient = new HttpClient(handler);
            _javaScriptSerializer = new JavaScriptSerializer();
        }

        public HttpClient HttpClient
        {
            get { return _httpClient; }
        }

        public JavaScriptSerializer JavaScriptSerializer
        {
            get { return _javaScriptSerializer; }
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }

        public async Task<T> GetAs<T>(string endpoint)
        {
            var requestUri = "http://localhost:15672/api/" + endpoint;
            
            Console.WriteLine("GET {0}", requestUri);
            
            var responseMessage = await HttpClient.GetAsync(requestUri);

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Status code");

            var httpContent = responseMessage.Content;

            var body = await httpContent.ReadAsStringAsync();

            Console.WriteLine(body);

            return JavaScriptSerializer.Deserialize<T>(body);
        }
    }
}