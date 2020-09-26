using Data.Models;
using Data.Repositories.Implementations;
using Data.Repositories.Interfaces;
using Data.Store;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ultilities.Core;
using Ultilities.Factory;

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
            services.AddIdentity<AppUser, AppRole>()
                .AddDefaultTokenProviders();

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
                var connectionString = configuration.GetConnectionString("DbConnectionString");

                return new SqlServerConnectionFactory(connectionString);
            });
        }
    }
}
