using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Workshop_api_greenfield.Application.DTOs;
using Workshop_api_greenfield.Application.Services;
using Workshop_api_greenfield.Domain.Services;
using Workshop_api_greenfield.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateReservationDtoValidator>();

// Add DbContext
builder.Services.AddDbContext<ReservationDbContext>(options =>
    options.UseInMemoryDatabase("ReservationDb"));

// Add repositories
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Add domain services
builder.Services.AddScoped<ReservationValidationService>();

// Add application services
builder.Services.AddScoped<ReservationService>();
builder.Services.AddScoped<CalendarService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "Room Reservation API", Version = "v1" });
    
    // Enable XML comments in Swagger
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Configure static files middleware
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

// Initialize the database with mock data
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Starting database initialization with mock data...");
await DbInitializer.InitializeAsync(app.Services, logger);
logger.LogInformation("Database initialization completed.");

app.Run();
