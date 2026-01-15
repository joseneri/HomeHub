using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using HomeHub.Application.Common;
// Add these usings
using HomeHub.Application.Common;
using HomeHub.Application.Homes;
using HomeHub.Application.Services;
using HomeHub.Domain.Entities;
using HomeHub.Domain.Interfaces;
using HomeHub.Domain.Models;
using System.Linq;

namespace HomeHub.Infrastructure.Services
{
    public class HomeService : IHomeService
    {
        private readonly IHomeRepository _homeRepo;
        private readonly ICommunityRepository _communityRepo; // We need to check existence
        private readonly IMapper _mapper;

        public HomeService(IHomeRepository homeRepo, ICommunityRepository communityRepo, IMapper mapper)
        {
            _homeRepo = homeRepo;
            _communityRepo = communityRepo;
            _mapper = mapper;
        }

        public async Task<HomeDto> CreateAsync(CreateHomeRequest request, CancellationToken ct)
        {
            // 1. Validate Community Exists (Business Logic)
            var exists = await _communityRepo.ExistsAsync(request.CommunityId, ct);
            if (!exists)
            {
                throw new KeyNotFoundException($"Community {request.CommunityId} not found.");
            }

            // 2. Create
            var entity = _mapper.Map<Home>(request);
            await _homeRepo.AddAsync(entity, ct);

            // 3. Return (Reloading to get relations if needed, or just map)
            // For simplicity, we map the entity we just created, 
            // but usually we might need to fetch it again to get the Community Name included.
            var loaded = await _homeRepo.GetByIdAsync(entity.Id, ct);
            return _mapper.Map<HomeDto>(loaded);
        }

        public async Task<HomeDto?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            var entity = await _homeRepo.GetByIdAsync(id, ct);
            return _mapper.Map<HomeDto>(entity);
        }

        public async Task<PagedResult<HomeDto>> GetHomesAsync(GetHomesFilter filter, CancellationToken ct)
        {
            // 1. Map DTO (Application) -> Domain Options (Domain)
            var searchOptions = new HomeSearchOptions
            {
                MinPrice = filter.MinPrice,
                MaxPrice = filter.MaxPrice,
                Bedrooms = filter.Bedrooms,
                Bathrooms = filter.Bathrooms,
                Page = filter.Page,
                PageSize = filter.PageSize
            };

            // 2. Call Repository with Domain Object
            var (items, totalCount) = await _homeRepo.GetFilteredAsync(searchOptions, ct);

            // 3. Return Result
            return new PagedResult<HomeDto>
            {
                Items = _mapper.Map<List<HomeDto>>(items),
                TotalCount = totalCount,
                Page = filter.Page,
                PageSize = filter.PageSize
            };
        }

        public async Task UpdateAsync(Guid id, UpdateHomeRequest request, CancellationToken ct)
        {
            var entity = await _homeRepo.GetByIdAsync(id, ct);
            if (entity == null) throw new KeyNotFoundException("Home not found");

            _mapper.Map(request, entity);

            // Pass RowVersion handling logic here or in Repo

            await _homeRepo.UpdateAsync(entity, ct);
        }

    }
}
