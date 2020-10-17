using BusinessConsumerAPI.Consumer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using EventBus;
using Microsoft.Extensions.Options;

namespace BusinessConsumerAPI.Extensions
{
    public static class ApplicationBuilderExtentions
    {
        public static EventBusConstants _constants { get; }
        public static BusinessConsumer Listener { get; set; }

        public static IApplicationBuilder UseEventBusListener(this IApplicationBuilder app)
        {

            Listener = app.ApplicationServices.GetService<BusinessConsumer>();
            var life = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            life.ApplicationStarted.Register(OnStarted);
            life.ApplicationStopping.Register(OnStopping);
            return app;
        }

        private static EventBusConstants GetConstants()
        {
            var builder = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);

            IConfigurationRoot configuration = builder.Build();
            var setting = new EventBusConstants();
            setting.AudioExchange = configuration.GetSection("EventBusConstants:AudioExchange").Value;
            setting.AudioQueue = configuration.GetSection("EventBusConstants:AudioQueue").Value;
            setting.AudioRouting = configuration.GetSection("EventBusConstants:AudioRouting").Value;

            setting.VideoExchange = configuration.GetSection("EventBusConstants:VideoExchange").Value;
            setting.VideoQueue = configuration.GetSection("EventBusConstants:VideoQueue").Value;
            setting.VideoRouting = configuration.GetSection("EventBusConstants:VideoRouting").Value;
            return setting;
        }

        private static void OnStarted()
        {

            var settings = GetConstants();
            Listener.ConsumeAudio(settings.AudioExchange, settings.AudioRouting, settings.AudioQueue);
            Listener.ConsumeVideo(settings.VideoExchange, settings.VideoRouting, settings.VideoQueue);
        }

        private static void OnStopping()
        {
            Listener.Disconnect();
        }
    }
}
