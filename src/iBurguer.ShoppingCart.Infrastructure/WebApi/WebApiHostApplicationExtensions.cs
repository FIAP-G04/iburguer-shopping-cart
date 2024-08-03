using iBurguer.ShoppingCart.Infrastructure.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;

namespace iBurguer.ShoppingCart.Infrastructure.WebApi;

[ExcludeFromCodeCoverage]
public static class WebApiHostApplicationExtensions
{

    public static IHostApplicationBuilder AddWebApi(this IHostApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.AddSwagger();

        return builder;
    }
    
    public static WebApplication UseWebApi(this WebApplication app)
    {
        app.ConfigureSwagger();
        app.UseMiddleware(typeof(ExceptionHandlerMiddleware));
        app.UseHttpsRedirection();
        app.MapControllers();

        return app;
    }
}

