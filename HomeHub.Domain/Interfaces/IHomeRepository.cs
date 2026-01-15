using HomeHub.Domain.Entities;
using HomeHub.Domain.Models;

namespace HomeHub.Domain.Interfaces
{
    public interface IHomeRepository
    {
        // Correct: ID is a Guid
        Task<Home?> GetByIdAsync(Guid id, CancellationToken ct);

        Task<Home> AddAsync(Home home, CancellationToken ct);

        Task UpdateAsync(Home home, CancellationToken ct);

        // Correct: 
        // - 'Items' is a list of Home
        // - 'TotalCount' must be INT (because it's a number, like "10 results found")
        Task<(List<Home> Items, int TotalCount)> GetFilteredAsync(HomeSearchOptions options, CancellationToken ct);
    }
}