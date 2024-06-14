using ContactBook.Core.Enum;
using ContactBook.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Infrastructure.Services
{
    public class StatusUserUpdateService : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private Timer _timer;

        public StatusUserUpdateService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(UpdateStatus, null, TimeSpan.Zero, TimeSpan.FromDays(3));
            return Task.CompletedTask;
        }

        private void UpdateStatus(object state)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ContactBookDbContext>();

                var pendingUsers = context.InviteUsers
                    .Where(u => u.StatusUser == StatusUser.Pending)
                    .ToList();

                if (pendingUsers.Any())
                {
                    pendingUsers.ForEach(user => user.StatusUser = StatusUser.Locked);
                    context.SaveChanges();
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }

}
