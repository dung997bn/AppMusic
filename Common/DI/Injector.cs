using EventBus.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultilities.Constants;

namespace Ultilities.DI
{
    public static class Injector
    {
        public static void InjectAuth(IServiceCollection services, IConfiguration Configuration)
        {
            //Inject AuthenSettingConfigs
            services.Configure<AuthSettingConfigs>(Configuration.GetSection("AuthenSettingConfigs"));

            var authenConfig = Configuration.GetSection("AuthenSettingConfigs");
            services.AddAuthentication(opt => opt.DefaultAuthenticateScheme = "Authorize Schema")
                    .AddJwtBearer("Authorize Schema", x =>
                    {
                        x.SaveToken = true;
                        x.RequireHttpsMetadata = false;
                        x.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authenConfig["Secret"])),
                            ValidateIssuer = true,
                            ValidIssuer = authenConfig["Iss"],
                            ValidateAudience = true,
                            ValidAudience = authenConfig["Aud"],
                            ValidateLifetime = true,
                            RequireExpirationTime = true,
                            ClockSkew = TimeSpan.Zero
                        };
                    });
        }

        public static void InjectEventBus(IServiceCollection services, IConfiguration Configuration)
        {
            services.AddSingleton<IEventBusConnection>(sp =>
            {
                var factory = new ConnectionFactory()
                {
                    HostName = Configuration["EventBus:HostName"]
                };

                if (!string.IsNullOrEmpty(Configuration["EventBus:UserName"]))
                {
                    factory.UserName = Configuration["EventBus:UserName"];
                }
                if (!string.IsNullOrEmpty(Configuration["EventBus:Password"]))
                {
                    factory.Password = Configuration["EventBus:Password"];
                }
                return new EventBusConnection(factory);
            });
        }


        public static void InjectOptions(IServiceCollection services, IConfiguration Configuration)
        {
            services.AddControllers().AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

        }
    }

}
