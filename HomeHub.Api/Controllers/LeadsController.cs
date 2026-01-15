using HomeHub.Application.Leads;
using HomeHub.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace HomeHub.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeadsController : ControllerBase
    {
        private readonly ILeadService _leadService;

        public LeadsController(ILeadService leadService)
        {
            _leadService = leadService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateLead(
     [FromBody] CreateLeadRequest request,
     CancellationToken ct)
        {
            // The service returns the Guid directly (e.g., "a1b2c3...")
            var createdLeadId = await _leadService.CreateLeadAsync(request, ct);

            // FIX: Use the variable directly. Do not use .Id
            return Accepted(new { id = createdLeadId, status = "Processing" });
        }
    }
}