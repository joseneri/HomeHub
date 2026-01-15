using AutoMapper;
using FluentValidation;
using HomeHub.Api.Endpoints;
using HomeHub.Api.Infrastructure;
using HomeHub.Application.Mapping;
using HomeHub.Application.Services;
using HomeHub.Domain.Interfaces;
using HomeHub.Infrastructure.External;
using HomeHub.Infrastructure.Messaging;
using HomeHub.Infrastructure.Persistence;
using HomeHub.Infrastructure.Persistence.Repositories;
using HomeHub.Infrastructure.Services;
using MassTransit; // This is the only messaging namespace you need
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Polly;

// REMOVE: using RabbitMQ.Client; (Not needed with MassTransit)

var builder = WebApplication.CreateBuilder(args);

// --- 1. SERVICES REGISTRATION ---

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "HomeHub API",
        Version = "v1",
        Description = "API for HomeHub real estate app (portfolio project)."
    });
});

// Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                        ?? "Data Source=homehub.db";
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite(connectionString);
});

// AutoMapper
builder.Services.AddAutoMapper(typeof(CommunityProfile).Assembly);

// Fluent Validation
builder.Services.AddValidatorsFromAssembly(typeof(CommunityProfile).Assembly);

// Services
builder.Services.AddScoped<ICommunityService, CommunityService>();
builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddScoped<ILeadService, LeadService>();

builder.Services.AddScoped<ICommunityRepository, CommunityRepository>();
builder.Services.AddScoped<IHomeRepository, HomeRepository>();
builder.Services.AddScoped<ILeadRepository, LeadRepository>();

// HTTP Client
builder.Services.AddHttpClient<IAddressClient, AddressClient>(client =>
{
    var baseUrl = builder.Configuration["ExternalServices:Zippopotamus:BaseUrl"];
    if (string.IsNullOrWhiteSpace(baseUrl))
    {
        throw new InvalidOperationException("Configuration 'ExternalServices:Zippopotamus:BaseUrl' is missing.");
    }
    client.BaseAddress = new Uri(baseUrl);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
})
.AddTransientHttpErrorPolicy(policyBuilder =>
    policyBuilder.WaitAndRetryAsync(
        3,
        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
    )
);

// --- CHAPTER 10: MassTransit + RabbitMQ ---
// REMOVED: The manual AddSingleton<IConnection> block.
// MassTransit manages the connection lifecycle for us.

builder.Services.AddMassTransit(x =>
{
    // 1. Register the Consumer
    x.AddConsumer<LeadCreatedConsumer>();

    // 2. Configure RabbitMQ
    x.UsingRabbitMq((context, cfg) =>
    {
        // In Prod, read this from appsettings.json!
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        // 3. Automatically create queues based on Consumers
        cfg.ConfigureEndpoints(context);
    });
});

// Global Exception Handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// --- 2. PIPELINE CONFIGURATION ---

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "HomeHub API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.MapControllers();

app.MapGet("/health", () =>
{
    var result = new
    {
        status = "Healthy",
        service = "HomeHub.Api",
        version = typeof(Program).Assembly.GetName().Version?.ToString() ?? "1.0.0",
        timestampUtc = DateTime.UtcNow
    };
    return Results.Ok(result);
}).WithName("Health");

if (app.Environment.IsDevelopment())
{
    app.MapCommunityEndpoints();
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        db.Database.Migrate();
    }
}

app.Run();

public partial class Program;