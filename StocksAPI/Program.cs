using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Hosting.Internal;
using StocksAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<DbPersister, DbPersister>();
builder.Services.AddSingleton<UserService, UserService>();
builder.Services.AddSingleton<FavoritesService, FavoritesService>();
builder.Services.AddSingleton<StocksBaseInfo, StocksBaseInfo>();
builder.Services.AddSingleton<PolygonIOIntegration, PolygonIOIntegration>();
builder.Services.AddSingleton<StocksService, StocksService>();


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
            policy.AllowCredentials();
            policy.SetIsOriginAllowed(origin =>
                origin.Contains("localhost") || origin.Contains("mirodran16.gitlab.io"));
        });
});

builder.Services.AddAuthentication()
    .AddCookie(options =>
    {
        options.Events.OnRedirectToLogin = (context) =>
        {
            context.Response.StatusCode = 401;
            return Task.CompletedTask;
        };

        options.Cookie.HttpOnly = false;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.None;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (true)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.Services.GetService<StocksService>();

//app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
