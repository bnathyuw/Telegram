using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using NUnit.Framework;

namespace Telegram.Configurator.Tests
{
    [TestFixture]
    public class When_the_configurator_is_run
    {
        private HttpClient _httpClient;
        private JavaScriptSerializer _javaScriptSerializer;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            using (var process = new Process
                {
                    StartInfo = new ProcessStartInfo("Telegram.Configurator.exe")
                        {
                            CreateNoWindow = true,
                            UseShellExecute = false
                        }
                })
            {

                process.Start();

                process.WaitForExit();
            }

            var credentials = new NetworkCredential("guest", "guest");
            var handler = new HttpClientHandler {Credentials = credentials};
            _httpClient = new HttpClient(handler);
            _javaScriptSerializer = new JavaScriptSerializer();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            _httpClient.Dispose();
        }

        [Test]
        public async void Then_there_is_a_virtual_host_named_test()
        {
            var virtualHosts = await GetAs<List<VirtualHost>>("vhosts");

            Assert.That(virtualHosts, Has.Member(new VirtualHost {Name = "test"}));
        }

        [Test]
        public async void Then_the_user_has_permissions_on_the_virtual_host()
        {
            var permissions = await GetAs<Permission>("permissions/test/guest");

            Assert.That(permissions,
                        Is.EqualTo(new Permission
                            {
                                User = "guest",
                                VHost = "test",
                                Configure = ".*",
                                Write = ".*",
                                Read = ".*"
                            }));
        }


        private async Task<T> GetAs<T>(string endpoint)
        {
            var requestUri = "http://localhost:15672/api/" + endpoint;
            
            Console.WriteLine("GET {0}", requestUri);
            
            var responseMessage = await _httpClient.GetAsync(requestUri);

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Status code");

            var httpContent = responseMessage.Content;

            var body = await httpContent.ReadAsStringAsync();

            Console.WriteLine(body);

            return _javaScriptSerializer.Deserialize<T>(body);
        }
    }

    public struct Permission
    {
        public string User { get; set; }
        public string VHost { get; set; }
        public string Configure { get; set; }
        public string Write { get; set; }
        public string Read { get; set; }

        public override string ToString()
        {
            return string.Format("User: {0}; VHost: {1}; Configure: {2}; Write: {3}; Read: {4}", User, VHost, Configure, Write, Read);
        }
    }

    public struct VirtualHost
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return string.Format("Name: {0}", Name);
        }
    }
}