using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Domain.Entities
{
    public enum LeadStatus
    {
        New = 0,
        InProgress = 1,
        Closed = 2
    }

    public class Lead
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Message { get; set; }

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public LeadStatus Status { get; set; } = LeadStatus.New;

        public Guid? CommunityId { get; set; }
        public Community? Community { get; set; }

        public Guid? HomeId { get; set; }
        public Home? Home { get; set; }
    }
}
