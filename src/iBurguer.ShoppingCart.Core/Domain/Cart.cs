using iBurguer.ShoppingCart.Core.Abstractions;
using Newtonsoft.Json;
using static iBurguer.ShoppingCart.Core.Domain.Exceptions; 

namespace iBurguer.ShoppingCart.Core.Domain;

public class Cart : Entity<Guid>, IAggregateRoot
{
    [JsonProperty("Items")]
    private IList<CartItem> _items = new List<CartItem>();

    [JsonProperty]
    public Guid? CustomerId { get; private set; }

    [JsonProperty]
    public bool Closed { get; private set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public static Cart CreateAnonymousShoppingCart()
    {
        return new Cart { Id = Guid.NewGuid(), Closed = false, CreatedAt = DateTime.Now};
    }

    public static Cart CreateCustomerShoppingCart(Guid customerId)
    {
        InvalidCustomerId.ThrowIf(customerId == Guid.Empty);
        
        return new Cart { Id = Guid.NewGuid(), CustomerId = customerId, Closed = false, CreatedAt = DateTime.Now};
    }

    public Price Total
    {
        get
        {
            if (_items.Any())
            {
                return _items.Sum(x => x.Subtotal);
            }

            return 0;
        }
    }

    [JsonIgnore]
    public IReadOnlyCollection<CartItem> Items => _items.AsReadOnly();

    public CartItem AddCartItem(Product product, Quantity quantity)
    {
        CheckIfTheShoppingCartIsClosed();
        
        UpdatedAt = DateTime.Now;

        var item = GetCartItemByProduct(product.ProductId);

        if (ProductAlreadyExistsIntheCart(item))
        {
            item.Quantity.Increment(quantity);
        }
        else
        {
            item = new CartItem(product, quantity);
            _items.Add(item);
        }
        
        return item;
    }

    private bool ProductAlreadyExistsIntheCart(CartItem item) => item is not null;

    public void Clear()
    {
        _items.Clear();
        
        UpdatedAt = DateTime.Now;
    }

    public void RemoveCartItem(Guid cartItemId)
    {
        CheckIfTheShoppingCartIsClosed();

        _items.Remove(GetCartItemById(cartItemId));
        
        UpdatedAt = DateTime.Now;
    }

    public void IncrementTheQuantityOfTheCartItem(Guid cartItemId, Quantity quantity)
    {
        CheckIfTheShoppingCartIsClosed();

        GetCartItemById(cartItemId).Quantity.Increment(quantity);
        
        UpdatedAt = DateTime.Now;
    }

    public void DecrementTheQuantityOfTheCartItem(Guid cartItemId, Quantity quantity)
    {
        CheckIfTheShoppingCartIsClosed();

        GetCartItemById(cartItemId).Quantity.Decrement(quantity);
        
        UpdatedAt = DateTime.Now;
    }

    public void Close()
    {
        UnableToCloseWithoutAnyCartItems.ThrowIf(!Items.Any());

        Closed = true;
        
        RaiseEvent(new CartClosedDomainEvent(Id));
    }

    public void UpdateItemPriceThroughProduct(Guid product, Price price)
    {
        CheckIfTheShoppingCartIsClosed();

        var item = GetCartItemByProduct(product);
        
        ProductNotPresentInCart.ThrowIfNull(item);
        
        UpdatedAt = DateTime.Now;
        
        item!.UpdatePrice(price);
    }

    private void CheckIfTheShoppingCartIsClosed() => CannotUpdateClosedCart.ThrowIf(Closed);

    public CartItem GetCartItemById(Guid id)
    {
        var item = _items.FirstOrDefault(i => Equals(i.CartItemId, id));

        ItemNotPresentInCart.ThrowIfNull(item);

        return item!;
    }

    private CartItem? GetCartItemByProduct(Guid productId)
    {
        return _items.FirstOrDefault(item => item.Product.ProductId == productId);
    }
}