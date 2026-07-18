using Business.Concrete;
using DataAccess;
using DataAccess.Concrete;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OpenAI.Chat;
using Serilog;
using Utilities.Logging.ErrorHandling;
using Utilities.Logging.Serilog;
using Utilities.Mapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.Configure<Entities.Configuration.SecurityKey>(builder.Configuration.GetSection("SecurityKey"));
builder.Services.Configure<Entities.Configuration.OpenAIKey>(builder.Configuration.GetSection("OpenAIKey"));

SerilogConfig.ConfigureLogging(builder.Configuration);
builder.Host.UseSerilog();
var corsOrigins = builder.Configuration.GetSection("CORS:Origin").Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultCorsPolicy", policy =>
    {
        policy
            .WithOrigins(corsOrigins)
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
    if (!builder.Environment.IsEnvironment("Docker"))
    {
        options.ListenAnyIP(7077, listenOptions =>
        {
            listenOptions.UseHttps();
        });
    }

    options.ListenAnyIP(8080);
});

builder.Services.Scan(scan => scan
    .FromAssemblies(
        typeof(Business.AssemblyReference).Assembly,
        typeof(DataAccess.AssemblyReference).Assembly,
        typeof(Utilities.AssemblyReference).Assembly)
    .AddClasses(classes => classes
        .Where(c => c != typeof(Utilities.Mapper.Mapper)
                 && !typeof(Exception).IsAssignableFrom(c)))
    .AsImplementedInterfaces()
    .WithScopedLifetime());

builder.Services.AddMapperApplication();

builder.Services.AddSingleton(sp =>
{
    return new ChatClient(
        model: "gpt-5.1",
        apiKey: builder.Configuration["OpenAIKey:AIKey"]
    );
});

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("chat", opt =>
    {
        opt.PermitLimit = 10;
        opt.Window = TimeSpan.FromMinutes(1);
        opt.QueueLimit = 0;
    });
});

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DataDbContext>();
    dbContext.Database.Migrate();
}
// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();
app.UseCors("DefaultCorsPolicy");
app.UseWhen(context => context.Request.Path.StartsWithSegments("/api"), appBuilder =>
{
    appBuilder.UseMiddleware<ApiKeyMiddleware>();
});

if (!app.Environment.IsDevelopment() && !app.Environment.IsEnvironment("Docker"))
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();
app.UseRateLimiter();
app.Run();
