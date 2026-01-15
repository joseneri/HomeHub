using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Domain.Entities
{
    public class Amenity
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public ICollection<HomeAmenity> HomeAmenities { get; set; } = new List<HomeAmenity>();
    }
}
