using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HomeHub.Domain.Entities
{
    public enum HomeStatus
    {
        Available = 0,
        Sold = 1,
        ComingSoon = 2
    }

    public class Home
    {
        public Guid Id { get; set; }

        public Guid CommunityId { get; set; }
        public Community Community { get; set; } = null!;

        public string Name { get; set; } = string.Empty;

        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public int AreaSqFt { get; set; }

        public decimal BasePrice { get; set; }

        public HomeStatus Status { get; set; } = HomeStatus.Available;

        // --- OPTIMISTIC CONCURRENCY CONTROL ---
        [Timestamp] // <--- Tells EF Core to use this for concurrency checks
        public byte[] RowVersion { get; set; } = null!; // <--- RowVersion property added

        // Navigation
        public ICollection<HomeAmenity> HomeAmenities { get; set; } = new List<HomeAmenity>();
        public ICollection<Lead> Leads { get; set; } = new List<Lead>();
    }
}
