using iBurguer.ShoppingCart.Infrastructure.Http;
using iBurguer.ShoppingCart.Infrastructure.IoC;
using iBurguer.ShoppingCart.Infrastructure.Redis;
using iBurguer.ShoppingCart.Infrastructure.Repositories;
using iBurguer.ShoppingCart.Infrastructure.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddMenuRestClient()
    .AddRedis()
    .AddRepositories()
    .AddWebApi()
    .AddUseCases();

var app = builder.Build();

app.UseWebApi();
app.Run();
