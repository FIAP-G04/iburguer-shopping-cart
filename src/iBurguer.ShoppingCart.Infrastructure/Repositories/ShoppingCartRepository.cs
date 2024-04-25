using System.Text.Json;
using iBurguer.ShoppingCart.Core.Domain;
using iBurguer.ShoppingCart.Infrastructure.Redis;
using Microsoft.Extensions.Caching.Distributed;

namespace iBurguer.ShoppingCart.Infrastructure.Repositories;

public class ShoppingCartRepository : IShoppingCartRepository
{
    private readonly ICacheContext _context;
    private const string GroupName = "cart";

    public ShoppingCartRepository(ICacheContext context)
    {
        _context = context;
    }
    
    public async Task Save(Cart shoppingCart, CancellationToken cancellationToken = default)
    {
        await _context.Set(GroupName, shoppingCart.Id.ToString(), shoppingCart, cancellationToken);
    }

    public async Task<Cart?> GetById(Guid shoppingCartId, CancellationToken cancellationToken = default)
    {
        return await _context.Get<Cart>(GroupName, shoppingCartId.ToString(), cancellationToken);
    }
}