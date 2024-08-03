using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace iBurguer.ShoppingCart.Infrastructure.Swagger;

[ExcludeFromCodeCoverage]
public static class SwaggerHostApplicationExtensions
{
    private const string Title = "iBurguer Shopping Cart API";
    private const string Description = "The ShoppingCart Management API offers a robust solution for managing shopping carts in fast food iBurguer. This RESTful API provides a suite of features designed to facilitate efficient cart management, order generating, and customer engagement.";
    private const string Version = "v1";

    public static IHostApplicationBuilder AddSwagger(this IHostApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(Version, new OpenApiInfo
            {
                Title = Title,
                Description = Description,
                Version = Version
            });

            options.EnableAnnotations();
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "iBurguer.ShoppingCart.API.xml"));
            options.DescribeAllParametersInCamelCase();
        });

        return builder;
    }

    public static void ConfigureSwagger(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment()) return;

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{Title} {Version}");
        });
    }
}