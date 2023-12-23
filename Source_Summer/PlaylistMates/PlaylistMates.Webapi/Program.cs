using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using PlaylistMates.Application.Dto;
using PlaylistMates.Application.Infrastructure;
using PlaylistMates.Application.Model;
using PlaylistMates.Webapi.Extensions;
using PlaylistMates.Webapi.Services;
using PlaylistMates.Application.Data;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
var builder = WebApplication.CreateBuilder(args);
/*
// Add services to the container.
var opt = new DbContextOptionsBuilder()
    .UseNpgsql("Host=localhost;Database=pos;Username=pos;Password=pos;")
    .Options;

//builder.Services.AddDbContext<Context>(options =>
//    options.UseNpgsql(builder.Configuration["AppSettings:Database"].Replace("${DATABASE_HOST}", builder.Configuration["DATABASE_HOST"] ?? "localhost")));



DbInitializer _dbinit = new DbInitializer(opt);

try
{
    // wait 120 seconds
    _dbinit.EnsureDbConnectionAsync(120000).GetAwaiter().GetResult();
}
catch (Exception ex)
{
    // Log the error, send a notification, etc.
    Console.Error.WriteLine($"Failed to connect to the database: {ex.Message}");
    Environment.Exit(1);
}

_dbinit.Init();
*/
string jwtSecret = builder.Configuration["AppSettings:Secret"] ?? AuthService.GenerateRandom(1024);
var db = new MongoDatabase("localhost", "PlaylistMates");
builder.Services.AddTransient(provider => new MongoDatabase("localhost", "PlaylistMates"));

builder.Services.AddHttpContextAccessor();


// JWT aktivieren, aber nicht standardm��ig aktivieren. Daher muss beim Controller
//     [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
// geschrieben werden. Wird nur eine API bereitgestellt, kann dieser Parameter auf
// true gesetzt und Cookies nat�rlich deaktiviert werden.
builder.Services.AddJwtAuthentication(jwtSecret, setDefault: true);
builder.Services.AddScoped<AuthService>(services =>
    new AuthService(jwtSecret, services.GetRequiredService<Context>()));
// builder.Services.AddScoped<IAuthorizationHandler, PlaylistRoleHandler>();

// builder.Services.AddAuthorization(options =>
// {
//     options.AddPolicy("PlaylistOwnerPolicy", policy =>
//         policy.Requirements.Add(new PlaylistRoleRequirement(new List<PlaylistRole> { PlaylistRole.OWNER })));
//     options.AddPolicy("PlaylistCollaboratorPolicy", policy =>
//         policy.Requirements.Add(new PlaylistRoleRequirement(new List<PlaylistRole> { PlaylistRole.COLLABORATOR })));
//     options.AddPolicy("PlaylistListenerPolicy", policy =>
//         policy.Requirements.Add(new PlaylistRoleRequirement(new List<PlaylistRole> { PlaylistRole.LISTENER })));
//     options.AddPolicy("PlaylistAnyRole", policy =>
//         policy.Requirements.Add(new PlaylistRoleRequirement(new List<PlaylistRole> { PlaylistRole.LISTENER, PlaylistRole.COLLABORATOR, PlaylistRole.OWNER })));
//     options.AddPolicy("PlaylistCollaboratorOrOwner", policy =>
//         policy.Requirements.Add(new PlaylistRoleRequirement(new List<PlaylistRole> { PlaylistRole.COLLABORATOR, PlaylistRole.OWNER })));
//
// });


// Juston_Von@gmail.com,pw:1234, Collaborator at 41
builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// override default logging provider
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowAnyOrigin());
    //.AllowCredentials()); // allow credentials

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
