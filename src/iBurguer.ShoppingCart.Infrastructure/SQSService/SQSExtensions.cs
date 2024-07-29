using iBurguer.ShoppingCart.Core.Abstractions;
using iBurguer.ShoppingCart.Infrastructure.SQS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace iBurguer.ShoppingCart.Infrastructure.SQSService
{
    public static class SQSExtensions
    {
        public static IHostApplicationBuilder AddSQS(this IHostApplicationBuilder builder, IConfiguration configuration)
        {
            builder.Services.AddScoped<ISQSService, SQSService>();
            builder.Services.Configure<SQSConfiguration>(configuration.GetRequiredSection("MassTransit"));

            return builder;
        }
    }
}
