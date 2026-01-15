using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Infrastructure.External
{
    public class AddressClient : IAddressClient
    {
        private readonly HttpClient _httpClient;

        // The container injects a pre-configured HttpClient here
        public AddressClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<(string City, string State)?> GetAddressByZipCodeAsync(string zipCode, CancellationToken ct)
        {
            // Zippopotam format: /us/{zipcode}
            // We assume the BaseAddress is already set in Program.cs
            var response = await _httpClient.GetAsync($"us/{zipCode}", ct);

            if (!response.IsSuccessStatusCode)
            {
                return null; // Or throw specific exception depending on requirement
            }

            var data = await response.Content.ReadFromJsonAsync<ZipCodeResponse>(cancellationToken: ct);

            var place = data?.Places.FirstOrDefault();
            if (place is null) return null;

            return (place.PlaceName, place.State);
        }
    }
}
