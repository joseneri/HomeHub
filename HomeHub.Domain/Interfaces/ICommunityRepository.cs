using HomeHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Domain.Interfaces
{
    public interface ICommunityRepository
    {
        Task<List<Community>> GetAllAsync(CancellationToken ct);
        Task<Community?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<Community> AddAsync(Community community, CancellationToken ct);
        Task UpdateAsync(Community community, CancellationToken ct);
        Task DeleteAsync(Community community, CancellationToken ct);
        Task<bool> ExistsAsync(Guid id, CancellationToken ct);
    }
}
