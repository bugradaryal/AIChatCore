using Business.Concrete;
using DataAccess;
using DataAccess.Concrete;
using Microsoft.EntityFrameworkCore;
using OpenAI.Chat;
using Utilitys.Mapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.Configure<Entities.Configuration.SecurityKey>(builder.Configuration.GetSection("SecurityKey"));
builder.Services.Configure<Entities.Configuration.OpenAIKey>(builder.Configuration.GetSection("OpenAIKey"));
builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultCorsPolicy", policy =>
    {
        policy
            .WithOrigins(builder.Configuration["CORS:Origin"])
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
builder.Services.AddDbContext<DataDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    );
});
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(7077, listenOptions =>
    {
        listenOptions.UseHttps();
    });
});

builder.Services.Scan(scan => scan
    .FromAssemblies(
        typeof(Business.AssemblyReference).Assembly,
        typeof(DataAccess.AssemblyReference).Assembly,
        typeof(Utilitys.AssemblyReference).Assembly)
    .AddClasses()
    .AsImplementedInterfaces()
    .WithScopedLifetime());

builder.Services.AddSingleton(sp =>
{
    return new ChatClient(
        model: "gpt-5.1",
        apiKey: builder.Configuration["OpenAIKey:AIKey"]
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseWhen(context => context.Request.Path.StartsWithSegments("/api"), appBuilder =>
{
    appBuilder.UseMiddleware<ApiKeyMiddleware>();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
