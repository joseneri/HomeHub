using HomeHub.Application.Leads;
using HomeHub.Application.Services; // References the Interface
using HomeHub.Domain.Entities;
using HomeHub.Domain.Events;
using HomeHub.Domain.Interfaces;
using MassTransit;

namespace HomeHub.Infrastructure.Services
{
    public class LeadService : ILeadService
    {
        private readonly ILeadRepository _leadRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public LeadService(ILeadRepository leadRepository, IPublishEndpoint publishEndpoint)
        {
            _leadRepository = leadRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Guid> CreateLeadAsync(CreateLeadRequest request, CancellationToken ct)
        {
            // 1. Map Request -> Entity
            // (We do this manually here to ensure the fields match your Entity perfectly)
            var lead = new Lead
            {
                Name = request.Name,           // Correct field
                Email = request.Email,
                Phone = request.Phone,
                Message = request.Message,
                CommunityId = request.CommunityId,
                HomeId = request.HomeId,
                Status = LeadStatus.New,
                CreatedAtUtc = DateTime.UtcNow
            };

            // 2. Save to Database
            await _leadRepository.AddAsync(lead, ct);

            // 3. Publish Event to RabbitMQ
            await _publishEndpoint.Publish<LeadCreatedEvent>(new
            {
                LeadId = lead.Id,
                Name = lead.Name,              // Correct field
                Email = lead.Email,
                Phone = lead.Phone,
                CreatedAtUtc = lead.CreatedAtUtc
            }, ct);

            // 4. Return the ID so the Controller can return 202 Accepted
            return lead.Id;
        }
    }
}