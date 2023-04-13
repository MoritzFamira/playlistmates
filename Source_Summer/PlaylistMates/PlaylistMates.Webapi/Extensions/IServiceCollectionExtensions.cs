using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Threading.Tasks;

#nullable enable
namespace PlaylistMates.Webapi.Extensions
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Aktiviert die JWT Authentication in ASP.NET Core.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="secret">Base64 codiertes Secret, welches für die Validierung
        /// des Tokens verwendet wird.</param>
        public static void AddJwtAuthentication(this IServiceCollection services,
            string secret,
            bool setDefault)
        {
            if (string.IsNullOrEmpty(secret))
            {
                throw new ArgumentException("Secret is null.", nameof(secret));
            }

            byte[] key = Convert.FromBase64String(secret);
            services.AddAuthentication(options =>
            {
                if (setDefault)
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                }
            })
            .AddJwtBearer(options =>
            {
                // Damit der Token auch als GET Parameter in der Form ...?token=xxxx übergben
                // werden kann, reagieren wir auf den Event für ankommende Anfragen.
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = ctx =>
                    {
                        string token = ctx.Request.Query["token"];
                        if (!string.IsNullOrEmpty(token))
                        {
                            ctx.Token = token;
                        }
                        return Task.CompletedTask;
                    }
                };
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }
    }
}