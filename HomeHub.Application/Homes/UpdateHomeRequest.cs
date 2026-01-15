using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Application.Homes
{
    public class UpdateHomeRequest
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public int SqFt { get; set; }

        // CRITICAL: The version token the client received when they loaded the form.
        public byte[] RowVersion { get; set; } = null!;
    }
}
