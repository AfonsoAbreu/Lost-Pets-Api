using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Infrastructure.Data.Entities;
using Microsoft.OpenApi.Models;
using Infrastructure.Repositories.Interfaces;
using Infrastructure.Repositories;
using Application.Services.Interfaces;
using Application.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals;
    });

// Add Database Context
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptions =>
        {
            sqlServerOptions.UseNetTopologySuite();
            sqlServerOptions.MigrationsAssembly(nameof(Infrastructure));
        }
    )
);

#region Swagger Generation

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

#endregion

#region Identity Service Setup

builder.Services.AddAuthentication()
    .AddBearerToken(options =>
    {
        options.BearerTokenExpiration = TimeSpan.FromDays(1);
    });
builder.Services.AddAuthorization();

// Identity Api Endpoints
builder.Services.AddIdentityApiEndpoints<User>(options =>
{
    // Password settings
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
})
    .AddEntityFrameworkStores<ApplicationDbContext>();

#endregion

#region Application Services

// AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

#region Repositories

builder.Services.AddScoped<IPetRepository, PetRepository>();
builder.Services.AddScoped<IMissingPetRepository, MissingPetRepository>();
builder.Services.AddScoped<ISightingRepository, SightingRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();

#endregion

#region Services

builder.Services.AddScoped<IMissingPetService, MissingPetService>();
builder.Services.AddScoped<IPetService, PetService>();
builder.Services.AddScoped<ISightingService, SightingService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IContactService, ContactService>();

#endregion

#endregion

var app = builder.Build();

app.MapIdentityApi<User>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();