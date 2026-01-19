using Microsoft.Extensions.Options;
using TripPlanner.Api.Configuration;
using TripPlanner.Api.Services;
using TripPlanner.Api.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// Bind configuration
builder.Services.Configure<AISettings>(
    builder.Configuration.GetSection("AISettings"));

// HttpClient factory
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<IAIProviderService, OpenAIService>((sp, client) =>
{
});
builder.Services.AddScoped<ITripPlannerService, TripPlannerService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAngular");
app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();

