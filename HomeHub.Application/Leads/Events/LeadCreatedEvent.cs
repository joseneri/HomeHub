using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Application.Leads.Events
{
    public interface LeadCreatedEvent
    {
        Guid LeadId { get; }
        string FirstName { get; }
        string LastName { get; }
        string Email { get; }
        DateTime CreatedAtUtc { get; }
    }
}
