

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
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
        Description = "Wklej sw�j token JWT:",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("Bearer", document),
            new List<string>()
        }
    });


});

builder.Services.AddDbContext<MyContext>();

builder.Services.AddScoped<MySeeder>();

//Do JWT
String secretKeyString = builder.Configuration["JwtSecretKey"];
var secretKey = Encoding.UTF8.GetBytes(secretKeyString);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "MyAppName",
        ValidAudience = "MyUsers",
        IssuerSigningKey = new SymmetricSecurityKey(secretKey)
    };
});
builder.Services.AddScoped<JWTGenerator>();

//Do Repozytori�w i Serwis�w
builder.Services.AddScoped<IDriverRepository, DBDriverRepository>();
builder.Services.AddScoped<ITeamRepository, DBTeamRepository>();
builder.Services.AddScoped<DriverService>();
builder.Services.AddScoped<TeamService>();
builder.Services.AddScoped<RaceService>();

builder.Services.AddAuthorization();
builder.Services.AddControllers();


var app = builder.Build();


//U�ycie Seedera
using (var scrope = app.Services.CreateScope())
{
    var seeder = scrope.ServiceProvider.GetRequiredService<MySeeder>();
    seeder.Seed();
}

if (app.Environment.IsDevelopment())
{
    //U�ycie Swaggera
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