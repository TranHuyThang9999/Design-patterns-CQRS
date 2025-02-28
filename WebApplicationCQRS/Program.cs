using Microsoft.EntityFrameworkCore;
using WebApplicationCQRS.Application.Features.Users.Commands;
using WebApplicationCQRS.Application.Features.Users.Queries;
using WebApplicationCQRS.Infrastructure.Persistence.Context;
using WebApplicationCQRS.Domain.Interfaces;
using WebApplicationCQRS.Infrastructure.Middleware;
using WebApplicationCQRS.Infrastructure.Persistence.Repositories;
using WebApplicationCQRS.Infrastructure.Security;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
    typeof(CreateUserCommandHandler).Assembly,
    typeof(GetUserQueryHandler).Assembly
));

// 
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddControllers();
//

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
//
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        dbContext.Database.OpenConnection();
        dbContext.Database.CloseConnection();
        Console.WriteLine("✅ Kết nối SQL Server thành công!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Lỗi kết nối SQL Server: {ex.Message}");
    }
}
//
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<JwtMiddleware>();

app.UseHttpsRedirection();
app.MapControllers();


app.Run();
