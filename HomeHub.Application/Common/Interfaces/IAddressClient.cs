using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Infrastructure.External
{
    public interface IAddressClient
    {
        // Returns (City, State) or null if invalid
        Task<(string City, string State)?> GetAddressByZipCodeAsync(string zipCode, CancellationToken ct);
    }
}
