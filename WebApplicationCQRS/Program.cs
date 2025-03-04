using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApplicationCQRS.Application.Features.Tickets.Commands;
using WebApplicationCQRS.Application.Features.Users.Commands;
using WebApplicationCQRS.Application.Features.Users.Queries;
using WebApplicationCQRS.Domain.Interfaces;
using WebApplicationCQRS.Infrastructure.Middleware;
using WebApplicationCQRS.Infrastructure.Persistence.Context;
using WebApplicationCQRS.Infrastructure.Persistence.Repositories;
using WebApplicationCQRS.Infrastructure.Security;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

var corsSettings = configuration.GetSection("Cors");
var allowedOrigins = corsSettings.GetSection("AllowedOrigins").Get<string[]>() ?? new string[0];
var allowedMethods = corsSettings.GetSection("AllowedMethods").Get<string[]>() ?? new string[] { "GET", "POST" };
var allowedHeaders = corsSettings.GetSection("AllowedHeaders").Get<string[]>() ?? new string[] { "Content-Type", "Authorization" };

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins(allowedOrigins)
            .WithMethods(allowedMethods)
            .WithHeaders(allowedHeaders)
            .AllowCredentials();
    });
});


builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
    typeof(CreateUserHandler).Assembly,
    typeof(GetUserQueryHandler).Assembly,
    typeof(CreateTicketHandler).Assembly,
    typeof(UpdateProfileHandler).Assembly
));

var jwtSettings = configuration.GetRequiredSection("Jwt");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"] ??
                                       throw new InvalidOperationException("SecretKey is missing in configuration"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(secretKey)
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITicketRepository,TicketRepository>();
builder.Services.AddSingleton<IJwtService, JwtService>();

builder.Services.AddTransient<JwtMiddleware>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
var logger = app.Services.GetRequiredService<ILogger<Program>>();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        dbContext.Database.OpenConnection();
        dbContext.Database.CloseConnection();
        logger.LogInformation("Connection database is successful");
    }
    catch (Exception ex)
    {
        logger.LogError($"An error occurred connecting to database: {ex.Message}");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseMiddleware<JwtMiddleware>();
app.UseAuthorization();
app.MapControllers();

app.Run();