using HomeHub.Domain.Entities;
using HomeHub.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HomeHub.Infrastructure.Persistence.Repositories
{
    public class CommunityRepository : ICommunityRepository
    {
        private readonly ApplicationDbContext _db;

        public CommunityRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<Community>> GetAllAsync(CancellationToken ct)
        {
            // Returns pure Entities. The Service will map to DTOs.
            return await _db.Communities.AsNoTracking().ToListAsync(ct);
        }

        // FIXED: int -> Guid
        public async Task<Community?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _db.Communities
                .Include(c => c.Homes)
                .Include(c => c.Leads)
                .FirstOrDefaultAsync(c => c.Id == id, ct);
        }

        public async Task<Community> AddAsync(Community community, CancellationToken ct)
        {
            _db.Communities.Add(community);
            await _db.SaveChangesAsync(ct);
            return community;
        }

        public async Task UpdateAsync(Community community, CancellationToken ct)
        {
            _db.Communities.Update(community);
            await _db.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Community community, CancellationToken ct)
        {
            _db.Communities.Remove(community);
            await _db.SaveChangesAsync(ct);
        }

        // FIXED: int -> Guid
        public async Task<bool> ExistsAsync(Guid id, CancellationToken ct)
        {
            return await _db.Communities.AnyAsync(c => c.Id == id, ct);
        }
    }
}