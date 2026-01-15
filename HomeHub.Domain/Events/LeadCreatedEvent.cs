using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Domain.Events
{
    public interface LeadCreatedEvent
    {
        // Change Guid -> int to match Lead.cs
        Guid LeadId { get; }

        // Change First/Last Name -> Name
        string Name { get; }

        string Email { get; }
        string? Phone { get; }
        DateTime CreatedAtUtc { get; }
    }
}
