using HomeHub.Application.Communities;
using HomeHub.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace HomeHub.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommunitiesController : ControllerBase
    {
        private readonly ICommunityService _service;

        public CommunitiesController(ICommunityService service)
        {
            _service = service;
        }

        // GET: api/communities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommunityListItemDto>>> GetCommunities(CancellationToken ct)
        {
            var items = await _service.GetListAsync(ct);
            return Ok(items);
        }

        // GET: api/communities/{guid}
        // FIX: Changed constraint from :int to :guid
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CommunityDetailDto>> GetCommunity(Guid id, CancellationToken ct)
        {
            var community = await _service.GetByIdAsync(id, ct);

            if (community is null)
            {
                return NotFound();
            }

            return Ok(community);
        }

        // POST: api/communities
        [HttpPost]
        public async Task<ActionResult<CommunityDetailDto>> CreateCommunity(
            [FromBody] CreateCommunityRequest request,
            CancellationToken ct)
        {
            var created = await _service.CreateAsync(request, ct);

            // This now works because GetCommunity accepts a Guid
            return CreatedAtAction(nameof(GetCommunity), new { id = created.Id }, created);
        }

        // PUT: api/communities/{guid}
        // FIX: Changed constraint from :int to :guid
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCommunity(
            Guid id,
            [FromBody] UpdateCommunityRequest request,
            CancellationToken ct)
        {
            var updated = await _service.UpdateAsync(id, request, ct);

            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/communities/{guid}
        // FIX: Changed constraint from :int to :guid
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCommunity(Guid id, CancellationToken ct)
        {
            var result = await _service.DeleteAsync(id, ct);

            if (!result.Success && result.Error == "NotFound")
            {
                return NotFound();
            }

            if (!result.Success && result.Error == "HasDependencies")
            {
                return Conflict("Cannot delete a community that has homes or leads.");
            }

            return NoContent();
        }
    }
}