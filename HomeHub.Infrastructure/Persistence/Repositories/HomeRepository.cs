using HomeHub.Domain.Entities;
using HomeHub.Domain.Interfaces;
using HomeHub.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeHub.Infrastructure.Persistence.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _db;

        public HomeRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        // FIXED: Changed int -> Guid to match your new Architecture
        public async Task<Home?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _db.Homes
                .Include(h => h.Community)
                .FirstOrDefaultAsync(h => h.Id == id, ct);
        }

        public async Task<Home> AddAsync(Home home, CancellationToken ct)
        {
            _db.Homes.Add(home);
            await _db.SaveChangesAsync(ct);
            return home;
        }

        public async Task UpdateAsync(Home home, CancellationToken ct)
        {
            _db.Homes.Update(home);
            await _db.SaveChangesAsync(ct);
        }

        // CORRECT: Uses 'HomeSearchOptions' from Domain, not the DTO from Application
        public async Task<(List<Home> Items, int TotalCount)> GetFilteredAsync(HomeSearchOptions options, CancellationToken ct)
        {
            var query = _db.Homes
                .AsNoTracking()
                .Include(h => h.Community)
                .AsQueryable();

            // 1. Apply Filters
            if (options.MinPrice.HasValue)
                query = query.Where(h => h.BasePrice >= options.MinPrice.Value);

            if (options.MaxPrice.HasValue)
                query = query.Where(h => h.BasePrice <= options.MaxPrice.Value);

            if (options.Bedrooms.HasValue)
                query = query.Where(h => h.Bedrooms >= options.Bedrooms.Value);

            if (options.Bathrooms.HasValue)
                query = query.Where(h => h.Bathrooms >= options.Bathrooms.Value);

            // 2. Execute Count
            var totalCount = await query.CountAsync(ct);

            // 3. Execute List (Pagination)
            // Note: Since IDs are now GUIDs, sorting by ID is random (unless using V7). 
            // In a real app, you might prefer .OrderByDescending(h => h.CreatedAt)
            var items = await query
                .OrderByDescending(h => h.Id)
                .Skip((options.Page - 1) * options.PageSize)
                .Take(options.PageSize)
                .ToListAsync(ct);

            return (items, totalCount);
        }
    }
}