using API.Auth;
using API.Common.Constants;
using API.Common.CountriesApi;
using API.Common.Exceptions;
using API.Common.WeatherApi;
using API.DependencyInjection;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

builder.Services.AddMemoryCache();

var tokenOptions = builder.Configuration.GetSection(ApiConstatns.TokenOptions).Get<TokenOptions>()!;
builder.Services.Configure<TokenOptions>(builder.Configuration.GetSection(ApiConstatns.TokenOptions));

builder.Services.AddAuth(tokenOptions);

builder.Services.AddDbContext<DataContext>(options=> {
    var dbPath = Path.GetFullPath(builder.Configuration.GetValue<string>("SqliteDbPath")!);
    options.UseSqlite($"Data Source={dbPath}");
});

builder.Services.AddAuthorization(); 

builder.Services.AddHttpClient();

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ICountriesApiClient, CountriesApiClient>();
builder.Services.AddScoped<IWeatherApiClient, WeatherApiClient>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(Program)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseMiddleware<ExceptionHandlingMiddleware>();

await app.RunAsync();
