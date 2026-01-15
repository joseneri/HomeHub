using HomeHub.Domain.Entities;
using HomeHub.Domain.Interfaces;
using HomeHub.Infrastructure.Persistence;

public class LeadRepository : ILeadRepository
{
    private readonly ApplicationDbContext _context;

    public LeadRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Lead> AddAsync(Lead lead, CancellationToken ct = default)
    {
        await _context.Leads.AddAsync(lead, ct);
        await _context.SaveChangesAsync(ct);
        return lead;
    }
}