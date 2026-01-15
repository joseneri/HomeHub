using HomeHub.Application.Leads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Application.Services
{
    public interface ILeadService
    {
        // Returns the ID of the created Lead
        Task<Guid> CreateLeadAsync(CreateLeadRequest request, CancellationToken ct);
    }
}
