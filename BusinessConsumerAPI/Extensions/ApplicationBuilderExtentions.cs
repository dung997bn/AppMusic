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

        private static void OnStarted()
        {

            Listener.ConsumeAudio();
        }

        private static void OnStopping()
        {
            Listener.Disconnect();
        }
    }
}
