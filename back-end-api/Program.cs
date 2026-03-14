using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using BackEndApi.Models.User;
using BackEndApi.Database;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddHealthChecks();
builder.Services.AddSwaggerGen();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSingleton<PostgresDatabase>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// JWT Authentication
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.Authority = "http://localhost:5000";
        options.Audience = "transaction-api";
        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "http://localhost:5000",
            ValidAudience = "transaction-api",
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWT_TOKEN_SECRET")
                    ?? throw new InvalidOperationException("JWT_TOKEN_SECRET missing")))
        };
    });

// App pipeline
var app = builder.Build();

app.UseCors(policy => policy
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .WithMethods("GET", "POST"));

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Transaction API V1"));
    app.MapHealthChecks("/health");
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();