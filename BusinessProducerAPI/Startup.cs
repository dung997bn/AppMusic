using Data.Repositories.CloudServer.Implementations;
using Data.Repositories.CloudServer.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
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

            services.AddTransient<IAudioRepository, AudioRepository>();
            services.AddTransient<IVideoRepository, VideoRepository>();
            services.AddTransient<IPhotoRepository, PhotoRepository>();

            services.Configure<CloudinarySettings>(Configuration.GetSection("Cloudinary"));

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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
