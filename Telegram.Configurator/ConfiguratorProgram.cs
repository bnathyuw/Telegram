﻿using System.Net;
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

            await CreateVirtualHost(VirtualHostName);
            await GiveUserPermissionsOnVirtualHost(VirtualHostName, UserName);
        }

        private static async Task GiveUserPermissionsOnVirtualHost(string virtualHostName, string userName)
        {
            var requestUri = string.Format("{0}permissions/{1}/{2}", ApiRoot, virtualHostName, userName);
            object permission = new {configure = ".*", write = ".*", read = ".*"};
            var body = new JavaScriptSerializer().Serialize(permission);
            var content = new StringContent(body, Encoding.UTF8, JsonContentType);
            await _httpClient.PutAsync(requestUri, content);
        }

        private static async Task CreateVirtualHost(string virtualHostName)
        {
            var requestUri = string.Format("{0}vhosts/{1}", ApiRoot, virtualHostName);
            var content = new StringContent("", Encoding.UTF8, JsonContentType);
            await _httpClient.PutAsync(requestUri, content);
        }
    }
}
