using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using dotenv.net;

var builder = WebApplication.CreateBuilder(args);

// Load environment variables
DotEnv.Load();

// Add some base services
builder.Services.AddControllers();
builder.Services.AddHealthChecks();
builder.Services.AddMvc();

// Add Bearer authentication
builder.Services.AddAuthentication("Bearer").AddJwtBearer(options =>
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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWT_TOKEN_SECRET")))
    };
});

var app = builder.Build();

var logger = app.Logger;

// Only allow for HTTPS redirection on production
if(!builder.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
	logger.LogInformation("Production");
}
else
{
	app.MapHealthChecks("/health");
	logger.LogInformation("Development");
}

app.UseCors(builder =>
{
    builder.AllowAnyOrigin().AllowAnyHeader().WithMethods("GET", "POST");
});

// Map endpoints
app.MapControllers();

// Authentication
app.UseAuthentication();
app.UseAuthorization();

// Run the application
app.Run();