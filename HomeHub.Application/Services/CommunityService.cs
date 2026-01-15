using AutoMapper;
using HomeHub.Application.Communities;
using HomeHub.Application.Services;
using HomeHub.Domain.Entities;
using HomeHub.Domain.Interfaces;
using HomeHub.Infrastructure.External; // Keep this if you use IAddressClient

namespace HomeHub.Application.Services // CORRECT NAMESPACE: Application Layer
{
    public class CommunityService : ICommunityService
    {
        private readonly ICommunityRepository _repo;
        private readonly IMapper _mapper;
        private readonly IAddressClient _addressClient;

        public CommunityService(ICommunityRepository repo, IMapper mapper, IAddressClient addressClient)
        {
            _repo = repo;
            _mapper = mapper;
            _addressClient = addressClient;
        }

        public async Task<IReadOnlyList<CommunityListItemDto>> GetListAsync(CancellationToken ct)
        {
            var entities = await _repo.GetAllAsync(ct);
            return _mapper.Map<List<CommunityListItemDto>>(entities);
        }

        // FIXED: int -> Guid
        public async Task<CommunityDetailDto?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(id, ct);
            return _mapper.Map<CommunityDetailDto>(entity);
        }

        public async Task<CommunityDetailDto> CreateAsync(CreateCommunityRequest request, CancellationToken ct)
        {
            // Example Logic: Validate Zip Code
            // await ValidateZipCode(request.ZipCode); 

            var entity = _mapper.Map<Community>(request);

            // CRITICAL: Generate the GUID here in the Application Layer
            entity.Id = Guid.NewGuid();

            var created = await _repo.AddAsync(entity, ct);

            return _mapper.Map<CommunityDetailDto>(created);
        }

        // FIXED: int -> Guid
        public async Task<bool> UpdateAsync(Guid id, UpdateCommunityRequest request, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(id, ct);
            if (entity == null) return false;

            _mapper.Map(request, entity);
            await _repo.UpdateAsync(entity, ct);

            return true;
        }

        // FIXED: int -> Guid
        public async Task<DeleteCommunityResult> DeleteAsync(Guid id, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(id, ct);
            if (entity == null) return DeleteCommunityResult.NotFound();

            // Business Rule Check
            if (entity.Homes.Any() || entity.Leads.Any())
            {
                return DeleteCommunityResult.HasDependencies();
            }

            await _repo.DeleteAsync(entity, ct);
            return DeleteCommunityResult.Ok();
        }
    }
}