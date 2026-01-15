using HomeHub.Application.Communities;
using HomeHub.Domain.Entities;
using HomeHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HomeHub.Api.Endpoints
{
    public static class CommunityEndpoints
    {
        public static void MapCommunityEndpoints(this IEndpointRouteBuilder app)
        {
            // DEV-ONLY seed endpoint (call once to populate the DB)
            app.MapPost("/dev/seed", async (ApplicationDbContext db, CancellationToken ct) =>
            {
                // If we already have data, don't seed again
                if (await db.Communities.AnyAsync(ct))
                {
                    return Results.BadRequest("Database already seeded.");
                }

                var community1 = new Community
                {
                    Name = "Sunset Valley",
                    City = "Austin",
                    State = "TX",
                    MinPrice = 350_000,
                    MaxPrice = 850_000
                };

                var community2 = new Community
                {
                    Name = "Ocean Breeze",
                    City = "San Diego",
                    State = "CA",
                    MinPrice = 500_000,
                    MaxPrice = 1_200_000
                };

                var home1 = new Home
                {
                    Community = community1,
                    Name = "Plan A",
                    Bedrooms = 3,
                    Bathrooms = 2,
                    AreaSqFt = 1800,
                    BasePrice = 380_000,
                    Status = HomeStatus.Available
                };

                var home2 = new Home
                {
                    Community = community1,
                    Name = "Plan B",
                    Bedrooms = 4,
                    Bathrooms = 3,
                    AreaSqFt = 2200,
                    BasePrice = 450_000,
                    Status = HomeStatus.ComingSoon
                };

                var home3 = new Home
                {
                    Community = community2,
                    Name = "Coastal Retreat",
                    Bedrooms = 4,
                    Bathrooms = 3,
                    AreaSqFt = 2400,
                    BasePrice = 750_000,
                    Status = HomeStatus.Available
                };

                db.Communities.AddRange(community1, community2);
                db.Homes.AddRange(home1, home2, home3);

                await db.SaveChangesAsync(ct);

                return Results.Ok("Database seeded with sample communities and homes.");
            })
            .WithName("DevSeed");

            // GET /communities -> returns list of CommunityListItemDto
            /*app.MapGet("/communities", async (ApplicationDbContext db, CancellationToken ct) =>
            {
                var communities = await db.Communities
                    .Select(c => new CommunityListItemDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        City = c.City,
                        State = c.State,
                        MinPrice = c.MinPrice,
                        MaxPrice = c.MaxPrice,
                        HomesCount = c.Homes.Count
                    })
                    .ToListAsync(ct);

                return Results.Ok(communities);
            })
            .WithName("GetCommunities");*/
        }
    }
}
