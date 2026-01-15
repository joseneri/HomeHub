using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Application.Communities
{
    public sealed class CommunityListItemDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string City { get; init; } = string.Empty;
        public string State { get; init; } = string.Empty;
        public decimal MinPrice { get; init; }
        public decimal MaxPrice { get; init; }
        public int HomesCount { get; init; }
    }

    public sealed class CommunityDetailDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string City { get; init; } = string.Empty;
        public string State { get; init; } = string.Empty;
        public decimal MinPrice { get; init; }
        public decimal MaxPrice { get; init; }
    }

    public sealed class CreateCommunityRequest
    {
        public string Name { get; init; } = string.Empty;
        public string City { get; init; } = string.Empty;
        public string State { get; init; } = string.Empty;
        public decimal MinPrice { get; init; }
        public decimal MaxPrice { get; init; }
    }

    public sealed class UpdateCommunityRequest
    {
        public string Name { get; init; } = string.Empty;
        public string City { get; init; } = string.Empty;
        public string State { get; init; } = string.Empty;
        public decimal MinPrice { get; init; }
        public decimal MaxPrice { get; init; }
    }
}
