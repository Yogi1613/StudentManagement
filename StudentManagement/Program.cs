
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentManagement.Data;
using StudentManagement.Middleware;
using StudentManagement.Model;
using StudentManagement.Repository;
using StudentManagement.Repository.IRepository;
using StudentManagement.Service;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add Controllers and Fluent Validation setup
builder.Services.AddControllers().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<StudentValidator>());

// Setup Database and Repositories/Services
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<StudentService>();
builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();
builder.Services.AddScoped<TeacherService>();
builder.Services.AddScoped<IPasswordHasher<Teacher>, PasswordHasher<Teacher>>();

// --- JWT Configuration Setup ---
var key = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is not configured.");
var issuer = builder.Configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer is not configured.");
var audience = builder.Configuration["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience is not configured.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // FIX: Set these to true for consistency and security
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        RoleClaimType = ClaimTypes.Role
    };
    options.Events = new JwtBearerEvents
    {
        //CRITICAL FIX: Manually extract the token from the header
        //OnMessageReceived = context =>
        //{
        //    // Read the Authorization header
        //    var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();

        //    if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        //    {
        //        // Assign the extracted token to context.Token for the middleware to use
        //        context.Token = authorizationHeader.Substring("Bearer ".Length).Trim();
        //        Console.WriteLine($"\n--- DEBUG: Token Manually Extracted: {context.Token.Substring(0, 10)}... ---");
        //    }
        //    else
        //    {
        //        Console.WriteLine($"\n--- DEBUG: Authorization header was not found or did not start with Bearer. ---");
        //    }

        //    return Task.CompletedTask;
        //},

        OnMessageReceived = context =>
        {
            var header = context.Request.Headers["Authorization"].FirstOrDefault();
            if (!string.IsNullOrEmpty(header))
            {
                if (header.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                    context.Token = header.Substring("Bearer ".Length).Trim();
                else
                    context.Token = header; // fallback if user pastes only token
            }
            return Task.CompletedTask;
        },


        OnAuthenticationFailed = context =>
        {
            // Log the actual exception for better debugging
            Console.WriteLine($"Authentication failed: {context.Exception.GetType().Name} - {context.Exception.Message}");
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine("Token validated successfully.");
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            Console.WriteLine("OnChallenge triggered.");
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();
// --- End JWT Configuration Setup ---


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "StudentManagement API", Version = "v1" });

    // Add JWT support in Swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token.\n\nExample: Bearer eyJhbGciOiJIUzI1NiIs..."
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

//app.UseRoleCheck();

app.UseAuthorization();

app.MapControllers();

app.Run();
