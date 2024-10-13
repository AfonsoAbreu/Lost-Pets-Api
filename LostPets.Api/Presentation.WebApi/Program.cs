using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Infrastructure.Data.Entities;
using Microsoft.OpenApi.Models;
using Infrastructure.Repositories.Interfaces;
using Infrastructure.Repositories;
using Application.Services.Interfaces;
using Application.Services;
using System.Text.Json.Serialization;
using Infrastructure.Facades.Settings;
using Infrastructure.Facades.Interfaces;
using Infrastructure.Facades;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.FileProviders;

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

#region JWT Settings

var jwtSettings = new JwtFacadeSettings();
builder.Configuration.Bind(jwtSettings.SectionName, jwtSettings);

builder.Services.AddSingleton(Options.Create(jwtSettings));
builder.Services.AddSingleton<IJwtFacade, JwtFacade>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
        };
    });

#endregion

#region Image Settings

var imageSettings = new ImageFacadeSettings();
builder.Configuration.Bind(imageSettings.SectionName, imageSettings);

string uploadPath = builder.Environment.WebRootPath;
if (imageSettings.UploadsSubFolder != null)
{
    uploadPath = Path.Combine(uploadPath, imageSettings.UploadsSubFolder);
}

imageSettings = new ImageFacadeSettings
{
    UploadsPath = uploadPath,
    MaxImagesPerMissingPet = imageSettings.MaxImagesPerMissingPet,
    AllowedImageTypes = imageSettings.AllowedImageTypes,
    UploadsSubFolder = imageSettings.UploadsSubFolder,
};

builder.Services.AddSingleton(Options.Create(imageSettings));
builder.Services.AddSingleton<IImageFacade, ImageFacade>();

#endregion

#region Server Settings

var serverSettings = new ServerFacadeSettings();
builder.Configuration.Bind(serverSettings.SectionName, serverSettings);

builder.Services.AddSingleton(Options.Create(serverSettings));
builder.Services.AddSingleton<IServerFacade, ServerFacade>();

#endregion

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
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<IMissingPetImageRepository, MissingPetImageRepository>();

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
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();