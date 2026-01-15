using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Domain.Entities
{
    public class Community
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;

        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }

        // Navigation properties
        public ICollection<Home> Homes { get; set; } = new List<Home>();
        public ICollection<Lead> Leads { get; set; } = new List<Lead>();
    }
}
