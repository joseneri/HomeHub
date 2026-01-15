using HomeHub.Domain.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace HomeHub.Infrastructure.Messaging
{
    public class LeadCreatedConsumer : IConsumer<LeadCreatedEvent>
    {
        private readonly ILogger<LeadCreatedConsumer> _logger;

        public LeadCreatedConsumer(ILogger<LeadCreatedConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<LeadCreatedEvent> context)
        {
            var message = context.Message;

            _logger.LogInformation(
                "🐰 [RabbitMQ] Lead Created: ID {Id} | Name: {Name} | Email: {Email}",
                message.LeadId,
                message.Name,
                message.Email);

            return Task.CompletedTask;
        }
    }
}