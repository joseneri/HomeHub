using HomeHub.Application.Common;
using HomeHub.Application.Homes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Application.Services
{
    public interface IHomeService
    {
        // We start with Create. We will add Get/Search later.
        Task<HomeDto> CreateAsync(CreateHomeRequest request, CancellationToken ct);

        // Needed for the CreatedAtAction in the controller
        Task<HomeDto?> GetByIdAsync(Guid id, CancellationToken ct);

        Task<PagedResult<HomeDto>> GetHomesAsync(GetHomesFilter filter, CancellationToken ct);

        Task UpdateAsync(Guid id, UpdateHomeRequest request, CancellationToken ct);
    }
}
