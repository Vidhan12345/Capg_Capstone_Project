using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WMS.Infrastructure;
using WMS.Infrastructure.Data;
using WMS.Application;
using WMS_Project.JsonConverters;
using WMS_Project.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new CustomDateTimeConverter());
        options.JsonSerializerOptions.Converters.Add(new CustomNullableDateTimeConverter());
    });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "WMS API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT Token"
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


// TEMPORARY OPEN CORS FOR DEBUGGING
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


// JWT AUTH
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtKey = builder.Configuration["Jwt:Key"];

        if (string.IsNullOrEmpty(jwtKey))
        {
            throw new Exception("JWT Key is missing!");
        }

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();


// GLOBAL EXCEPTION HANDLER
app.UseMiddleware<ExceptionMiddleware>();


// ALWAYS ENABLE SWAGGER
app.UseSwagger();
app.UseSwaggerUI();


// MIDDLEWARE
app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();


// TEMPORARILY DISABLED DB SEEDING
/*
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<WMSDbContext>();
    await DbSeeder.SeedAsync(context);
}
*/

app.Run();