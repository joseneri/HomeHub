using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Application.Homes
{
    // INPUT: What the frontend sends to create a home
    public class CreateHomeRequest
    {
        public Guid CommunityId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public decimal Price { get; set; }
        public int SqFt { get; set; }
    }

    // OUTPUT: What the frontend receives
    public class HomeDto
    {
        public Guid Id { get; set; }
        public int CommunityId { get; set; }
        public string CommunityName { get; set; } = string.Empty; // Flattened property
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public int SqFt { get; set; }

        // For concurrency checks later (Chapter 7 Part 2)
        // public byte[] RowVersion { get; set; } 
    }
}
