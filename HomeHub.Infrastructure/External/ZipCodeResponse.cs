using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HomeHub.Infrastructure.External
{
    public class ZipCodeResponse
    {
        [JsonPropertyName("post code")]
        public string PostCode { get; set; } = string.Empty;

        [JsonPropertyName("country")]
        public string Country { get; set; } = string.Empty;

        [JsonPropertyName("places")]
        public List<Place> Places { get; set; } = new();
    }

    public class Place
    {
        [JsonPropertyName("place name")]
        public string PlaceName { get; set; } = string.Empty; // City

        [JsonPropertyName("state")]
        public string State { get; set; } = string.Empty;
    }
}
