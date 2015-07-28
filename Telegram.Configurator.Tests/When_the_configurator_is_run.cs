using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;

namespace Telegram.Configurator.Tests
{
    [TestFixture]
    public class When_the_configurator_is_run
    {
        private RabbitClient _rabbitClient;

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

            _rabbitClient = new RabbitClient();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            _rabbitClient.Dispose();
        }

        [Test]
        public async void Then_there_is_a_virtual_host_named_test()
        {
            var virtualHosts = await _rabbitClient.GetAs<List<VirtualHost>>("vhosts");

            var expectedVirtualHost = new VirtualHost {Name = "test"};

            Assert.That(virtualHosts, Has.Member(expectedVirtualHost));
        }

        [Test]
        public async void Then_the_user_has_permissions_on_the_virtual_host()
        {
            var permission = await _rabbitClient.GetAs<Permission>("permissions/test/guest");

            var expectedPermission = new Permission
                {
                    User = "guest",
                    VHost = "test",
                    Configure = ".*",
                    Write = ".*",
                    Read = ".*"
                };

            Assert.That(permission, Is.EqualTo(expectedPermission));
        }

        [Test]
        public async void Then_there_is_an_exchange_named_telegram()
        {
            var exchanges = await _rabbitClient.GetAs<List<Exchange>>("exchanges/test");

            var expectedQueue = new Exchange { Name = "telegram" };

            Assert.That(exchanges, Has.Member(expectedQueue));
        }

        [Test]
        public async void Then_there_is_a_queue_named_telegram()
        {
            var queues = await _rabbitClient.GetAs<List<Queue>>("queues/test");

            var expectedQueue = new Queue {Name = "telegram"};

            Assert.That(queues, Has.Member(expectedQueue));
        }

        [Test]
        public async void Then_the_telegram_exchange_and_queue_are_bound()
        {
            var bindings = await _rabbitClient.GetAs<List<Binding>>("bindings/test");

            var expectedBinding = new Binding {Source = "telegram", Destination = "telegram"};

            Assert.That(bindings, Has.Member(expectedBinding));
        }
    }
}