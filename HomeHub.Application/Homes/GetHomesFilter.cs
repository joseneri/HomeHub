using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Application.Homes
{
    public class GetHomesFilter
    {
        // Filters (Optional)
        public string? SearchText { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? Bedrooms { get; set; }
        public int? Bathrooms { get; set; }
        public int? CommunityId { get; set; }

        // Pagination (Defaults)
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
