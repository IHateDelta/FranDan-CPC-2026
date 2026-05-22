
using FranDanBackend;
using FranDanBackend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "FranDan-CPC-2026",
        Description = "Backend",
        Version = "v1.0"
    });

    //Swagger + JWT
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Wklej swój token JWT:",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
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
            new List<string>()
        }
    });
});

builder.Services.AddDbContext<MyContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")
        ?? "Data Source=FranDanDB.db"));

builder.Services.AddScoped<MySeeder>();

//Do JWT
string secretKeyString = builder.Configuration["JwtSecretKey"]
    ?? throw new InvalidOperationException("JwtSecretKey is not configured.");
var secretKey = Encoding.UTF8.GetBytes(secretKeyString);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "FranDanBackend",
        ValidAudience = "AppUsers",
        IssuerSigningKey = new SymmetricSecurityKey(secretKey)
    };
});
builder.Services.AddScoped<JWTGenerator>();

//Do Serwisów
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PlanService>();

builder.Services.AddAuthorization();


var app = builder.Build();


//Użycie Seedera
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<MySeeder>();
    seeder.Seed();
}

if (app.Environment.IsDevelopment())
{
    //Użycie Swaggera
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); //KONIECZNE DO JWT
app.UseAuthorization(); //KONIECZNE DO JWT

app.MapControllers();

app.Run();

namespace FranDanBackend
{
    public class Program
    {
         public static void Main(string[] args) {
        }
    }
}