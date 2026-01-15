using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Domain.Entities
{
    public class HomeAmenity
    {
        public Guid HomeId { get; set; }
        public Home Home { get; set; } = null!;

        public Guid AmenityId { get; set; }
        public Amenity Amenity { get; set; } = null!;
    }
}
