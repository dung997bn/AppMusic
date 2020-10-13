using Data.DbContexts.IdentityServer.Core;
using Data.DbContexts.IdentityServer.Factory;
using Data.Models.IdentityServer;
using Data.Repositories.IdentityServer.Implementations;
using Data.Repositories.IdentityServer.Interfaces;
using Data.Store;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace CoreAPI
{
    public partial class InitializeIoC
    {
        public static void InitIoC(IServiceCollection services, IConfiguration configuration)
        {
            //Identity
            services.AddTransient<IUserStore<AppUser>, UserStore>();
            services.AddTransient<IRoleStore<AppRole>, RoleStore>();

            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddIdentity<AppUser, AppRole>();

            services.Configure<IdentityOptions>(opt =>
            {
                opt.Password.RequireDigit = true;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequiredLength = 6;
                opt.Password.RequiredUniqueChars = 1;
            });

            services.AddSingleton<IDbConnectionFactory>((c) =>
            {
                var connectionString = configuration.GetConnectionString("AuthConnectionString");
                return new SqlServerConnectionFactory(connectionString);
            });
        }

        public static void AuthenConfig(IServiceCollection services, IConfiguration Configuration)
        {
            var authenConfig = Configuration.GetSection("AuthenSettingConfigs");
            services.AddAuthentication(opt=>opt.DefaultAuthenticateScheme = "Authorize Schema")
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
    }
}
