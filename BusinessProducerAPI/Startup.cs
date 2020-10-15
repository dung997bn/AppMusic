using AutoMapper;
using Data.Repositories.CloudServer.Implementations;
using Data.Repositories.CloudServer.Interfaces;
using EventBus;
using EventBus.Core;
using EventBus.Producers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using RabbitMQ.Client;
using System;
using System.Text;
using Ultilities.Constants;

namespace BusinessProducerAPI
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
            services.AddControllers();

            //Repository
            services.AddTransient<IAudioRepository, AudioRepository>();
            services.AddTransient<IVideoRepository, VideoRepository>();
            services.AddTransient<IPhotoRepository, PhotoRepository>();

            //Inject Configuration
            services.Configure<CloudinarySettings>(Configuration.GetSection("Cloudinary"));
            services.Configure<EventBusConstants>(Configuration.GetSection("EventBusConstants"));

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

            //AutoMapper
            services.AddAutoMapper(typeof(Startup));

            //RabbitMQ
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

            services.AddSingleton<BusinessProducer>();

            //Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Busniess Producer API", Version = "v1" });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Busniess Producer V1");
            });
        }
    }
}
