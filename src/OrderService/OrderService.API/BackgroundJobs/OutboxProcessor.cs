using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrderService.API.Data;
using SharedKernel.Domain.Common.Events;
using SharedKernel.Infrastructure.Data;
using System.Text.Json;

namespace OrderService.API.BackgroundJobs
{
    public class OutboxProcessor : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<OutboxProcessor> _logger;

        public OutboxProcessor(IServiceScopeFactory scopeFactory, ILogger<OutboxProcessor> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("OutboxProcessor started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();

                    var pendingMessages = await dbContext.Set<OutboxMessage>()
                        .Where(x => x.Status == OutboxStatus.Pending)
                        .OrderBy(x => x.OccurredOn)
                        .Take(10)
                        .ToListAsync(stoppingToken);

                    if (pendingMessages.Any())
                    {
                        _logger.LogInformation("Found {Count} pending messages to process.", pendingMessages.Count);
                    }

                    foreach (var message in pendingMessages)
                    {
                        try
                        {
                            // Deserialize event nếu cần
                            var eventData = JsonSerializer.Deserialize<object>(message.Payload);

                            // TODO: gửi event sang các service khác ở đây
                            // ví dụ: publish qua RabbitMQ, Kafka, hoặc gởi HTTP request
                            _logger.LogInformation("Processing OutboxMessage {Id}: {EventType}", message.Id, message.EventType);

                            // giả sử thành công
                            message.Status = OutboxStatus.Processing;
                            message.ProcessedOn = DateTime.UtcNow;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error processing outbox message {Id}", message.Id);
                            message.Status = OutboxStatus.Failed;
                        }
                    }

                    await dbContext.SaveChangesAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during OutboxProcessor execution.");
                }

                // Delay 10 giây giữa mỗi lần quét
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }

            _logger.LogInformation("OutboxProcessor stopped.");
        }
    }
}
