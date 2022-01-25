using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;
using Template;

namespace Tests
{
    public class AccountTests
    {
        private TestServer server;
        private HttpClient client;

        [SetUp]
        public void Setup()
        {
            server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            client = server.CreateClient();
        }

        [Test]
        public async Task Test1()
        {
            var r = await client.GetAsync("/");
        }
    }
}