using iBurguer.ShoppingCart.Infrastructure.Http;
using iBurguer.ShoppingCart.Infrastructure.IoC;
using iBurguer.ShoppingCart.Infrastructure.Redis;
using iBurguer.ShoppingCart.Infrastructure.Repositories;
using iBurguer.ShoppingCart.Infrastructure.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddRestClient()
    .AddRedis()
    .AddRepositories()
    .AddWebApi()
    .AddUseCases();

builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseWebApi();
app.MapHealthChecks("/hc");
app.Run();
