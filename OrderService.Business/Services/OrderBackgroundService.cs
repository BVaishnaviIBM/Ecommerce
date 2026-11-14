using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace OrderService.Business.Services
{
    public class OrderBackgroundService : BackgroundService
    {
        private readonly ILogger<OrderBackgroundService> _logger;

        public OrderBackgroundService(
            ILogger<OrderBackgroundService> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Order Background Service started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation(
                    "Background job is running at: {Time}",
                    DateTime.UtcNow);

                await Task.Delay(
                    TimeSpan.FromMinutes(5),
                    stoppingToken);
            }

            _logger.LogInformation(
                "Order Background Service stopped.");
        }
    }
}