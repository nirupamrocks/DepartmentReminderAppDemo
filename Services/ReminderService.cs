using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DepartmentReminderAppDemo.Data;


namespace DepartmentReminderAppDemo.Services
{
    public class ReminderService :IHostedService , IDisposable
    {
        private readonly ILogger<ReminderService> _logger;
        private Timer _timer;
        private readonly IServiceProvider _serviceProvider;

        public ReminderService(ILogger<ReminderService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Reminder Service is starting.");
            _timer = new Timer(CheckReminders, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
            return Task.CompletedTask;
        }

        private async void CheckReminders(object state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var reminders = context.Reminders
                                       .Where(r => !r.IsNotified && r.DueDate <= DateTime.Now)
                                       .ToList();

                foreach (var reminder in reminders)
                {
                    // Send email notification logic here
                    // Example: await _emailService.SendEmailAsync(reminder);

                    // Mark as notified
                    reminder.IsNotified = true;
                    context.Reminders.Update(reminder);
                }

                await context.SaveChangesAsync();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Reminder Service is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
