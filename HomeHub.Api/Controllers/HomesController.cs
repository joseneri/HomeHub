using HomeHub.Application.Common;
using HomeHub.Application.Homes;
using HomeHub.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace HomeHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HomesController : ControllerBase
{
    private readonly IHomeService _service;

    public HomesController(IHomeService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<HomeDto>> Create(
        [FromBody] CreateHomeRequest request,
        CancellationToken ct)
    {
        var created = await _service.CreateAsync(request, ct);

        // Returns 201 Created with Location Header pointing to GetById
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // FIX: Explicitly added :guid constraint
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<HomeDto>> GetById(Guid id, CancellationToken ct)
    {
        var home = await _service.GetByIdAsync(id, ct);

        if (home is null)
        {
            return NotFound();
        }

        return Ok(home);
    }

    // GET: api/homes?minPrice=500&page=2
    [HttpGet]
    public async Task<ActionResult<PagedResult<HomeDto>>> GetHomes(
        [FromQuery] GetHomesFilter filter,
        CancellationToken ct)
    {
        var result = await _service.GetHomesAsync(filter, ct);
        return Ok(result);
    }

    // FIX: Explicitly added :guid constraint
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateHomeRequest request,
        CancellationToken ct)
    {
        await _service.UpdateAsync(id, request, ct);

        return NoContent();
    }
}