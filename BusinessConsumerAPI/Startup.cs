using AutoMapper;
using BusinessConsumerAPI.Consumer;
using BusinessConsumerAPI.Extensions;
using Data.DbContexts.SqlServer.Core;
using Data.DbContexts.SqlServer.Factory;
using Data.Models.IdentityServer;
using Data.Repositories.BusinessServer.Implementations;
using Data.Repositories.BusinessServer.Interfaces;
using Data.Repositories.IdentityServer.Implementations;
using Data.Repositories.IdentityServer.Interfaces;
using Data.Store;
using EventBus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BusinessConsumerAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
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


            //Repository
            services.AddTransient<IAudioServerRepository, AudioServerRepository>();
            services.AddTransient<IVideoServerRepository, VideoServerRepository>();
            services.AddTransient<IPhotoServerRepository, PhotoServerRepository>();


            //Sql Connection
            services.AddSingleton<IDbConnectionFactory>((c) =>
            {
                var connectionString = Configuration.GetConnectionString("BusinessConnection");
                return new SqlServerConnectionFactory(connectionString);
            });
            services.Configure<EventBusConstants>(Configuration.GetSection("EventBusConstants"));

            //AutoMapper
            services.AddAutoMapper(typeof(Startup));

            //Authen
            Ultilities.DI.Injector.InjectAuth(services, Configuration);

            //Json Options
            Ultilities.DI.Injector.InjectOptions(services, Configuration);

            //Event Bus
            Ultilities.DI.Injector.InjectEventBus(services, Configuration);
            services.AddSingleton<BusinessConsumer>();

            //Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Business Consumer API", Version = "v1" });
            });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseEventBusListener();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Business Consumer API V1");
            });
        }
    }
}
