using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using PlaylistMates.Application.Infrastructure;
using PlaylistMates.Application.Model;
using PlaylistMates.Webapi.Extensions;
using PlaylistMates.Webapi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<Context>(options =>
    options.UseOracle(builder.Configuration["AppSettings:Database"]));
string jwtSecret = builder.Configuration["AppSettings:Secret"] ?? AuthService.GenerateRandom(1024);

builder.Services.AddHttpContextAccessor();

// JWT aktivieren, aber nicht standardm��ig aktivieren. Daher muss beim Controller
//     [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
// geschrieben werden. Wird nur eine API bereitgestellt, kann dieser Parameter auf
// true gesetzt und Cookies nat�rlich deaktiviert werden.
builder.Services.AddJwtAuthentication(jwtSecret, setDefault: true);
builder.Services.AddScoped<AuthService>(services =>
    new AuthService(jwtSecret, services.GetRequiredService<Context>()));
builder.Services.AddScoped<IAuthorizationHandler, PlaylistRoleHandler>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("PlaylistOwnerPolicy", policy =>
        policy.Requirements.Add(new PlaylistRoleRequirement(PlaylistRole.OWNER)));
    options.AddPolicy("PlaylistCollaboratorPolicy", policy =>
        policy.Requirements.Add(new PlaylistRoleRequirement(PlaylistRole.COLLABORATOR)));
    options.AddPolicy("PlaylistListenerPolicy", policy =>
        policy.Requirements.Add(new PlaylistRoleRequirement(PlaylistRole.LISTENER)));
    //options.AddPolicy("PlaylistAnyRole", policy =>
    //{
    //    policy.Requirements.Add(new PlaylistRoleRequirement(PlaylistRole.LISTENER));
    //    policy.Requirements.Add(new PlaylistRoleRequirement(PlaylistRole.COLLABORATOR));
    //    policy.Requirements.Add(new PlaylistRoleRequirement(PlaylistRole.OWNER));
    //});
    //options.AddPolicy("PlaylistCollaboratorOrOwner", policy =>
    //{
    //    policy.Requirements.Add(new PlaylistRoleRequirement(PlaylistRole.COLLABORATOR));
    //    policy.Requirements.Add(new PlaylistRoleRequirement(PlaylistRole.OWNER));
    //});
});

// Juston_Von@gmail.com,pw:1234, Collaborator at 41
builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.Run();
