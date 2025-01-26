using Microsoft.AspNetCore.Authentication.JwtBearer;
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
    options.Authority = "";
    options.Audience = "transaction-api";
    options.RequireHttpsMetadata = true;
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
	app.UseHttpsRedirection();
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