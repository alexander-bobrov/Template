﻿using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Template.Services.CleanupService.Configuration;

namespace Template.Services.CleanupService
{
    public class CleanupService : IHostedService, IDisposable
    {
        private readonly IDbContextFactory<TemplateContext> dbFactory;
        private readonly ILogger<CleanupService> logger;
        private readonly CleanupOptions options;
        private Timer timer = null;

        public CleanupService(IDbContextFactory<TemplateContext> dbFactory, ILogger<CleanupService> logger, IOptions<CleanupOptions> options)
        {
            this.dbFactory = dbFactory;
            this.logger = logger;
            this.options = options.Value;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Background expired account cleaning has beed started");
            timer = new Timer(Cleanup, null, TimeSpan.Zero, options.CleanupInterval);

            return Task.CompletedTask;
        }

        private async void Cleanup(object state)
        {
            using (var db = dbFactory.CreateDbContext())
            {
                var accountsToDelete = db.Accounts.Where(x => x.CreatedAtUtc.AddHours(options.RetentionPeriodInHours) > DateTime.UtcNow);
                foreach (var account in accountsToDelete)
                {
                    db.Accounts.Remove(account);
                    logger.LogInformation($"Account wih login '{account.Login}' has been deleted");
                }

                await db.SaveChangesAsync();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);

            logger.LogInformation("Background expired account cleaning has beed stopped");
            return Task.CompletedTask;
        }

        
        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}
