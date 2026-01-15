using HomeHub.Application.Communities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HomeHub.Application.Services
{
    public interface ICommunityService
    {
        Task<IReadOnlyList<CommunityListItemDto>> GetListAsync(CancellationToken ct);

        // FIXED: int -> Guid
        Task<CommunityDetailDto?> GetByIdAsync(Guid id, CancellationToken ct);

        Task<CommunityDetailDto> CreateAsync(CreateCommunityRequest request, CancellationToken ct);

        // FIXED: int -> Guid
        Task<bool> UpdateAsync(Guid id, UpdateCommunityRequest request, CancellationToken ct);

        // FIXED: int -> Guid
        Task<DeleteCommunityResult> DeleteAsync(Guid id, CancellationToken ct);
    }

    public sealed class DeleteCommunityResult
    {
        private DeleteCommunityResult(bool success, string? error)
        {
            Success = success;
            Error = error;
        }

        public bool Success { get; }
        public string? Error { get; }

        public static DeleteCommunityResult Ok() => new(true, null);
        public static DeleteCommunityResult NotFound() => new(false, "NotFound");
        public static DeleteCommunityResult HasDependencies() => new(false, "HasDependencies");
    }
}