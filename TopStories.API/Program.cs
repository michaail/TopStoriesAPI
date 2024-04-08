using Microsoft.Extensions.Caching.Memory;
using TopStories.Services.CacheService;
using TopStories.Services.HackerNews;
using TopStories.Services.HackerNewsService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<HttpClient>();
builder.Services.AddSingleton<IMemoryCache, MemoryCache>();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<IApiService, HackerNewsAPIService>();
builder.Services.AddScoped<HackerNewsTopStoriesService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
