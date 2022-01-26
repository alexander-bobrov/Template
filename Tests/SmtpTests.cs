using Database.Configuration;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MimeKit;
using NUnit.Framework;
using System.Threading.Tasks;
using Template.Services.MailService.Configuration;

namespace Tests
{
    public class SmtpTests
    {
        [SetUp]
        public void Setup()
        {
            WebHost.CreateDefaultBuilder()
                .UseStartup<StartupTest>()
                .Build()
                .RunAsync();
        }

        [Test]
        public void SendMail_Should_BeOk()
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Mr. Test", "mr@test.com"));
            message.To.Add(new MailboxAddress("Mrs. Test", "mrs@test.com"));
            message.Subject = "Test message";

            message.Body = new TextPart("plain")
            {
                Text = @"It's a test message"
            };

            using var client = new SmtpClient();
            client.Connect("127.0.0.1", 25, false);
            client.Send(message);
            client.Disconnect(true);
        }
    
    }

    public class StartupTest 
    {
        private readonly IConfiguration _configuration;

        public StartupTest(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDatabase(_configuration);
            services.UseSmtpService(_configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) { }

    }
}