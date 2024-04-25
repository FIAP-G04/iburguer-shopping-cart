using iBurguer.ShoppingCart.Core.Abstractions;

namespace iBurguer.ShoppingCart.Core.Domain;

public static class Exceptions
{
    public class InvalidPrice() : DomainException<InvalidPrice>("The price cannot have a value equal to zero or negative");

    public class InvalidProductType() : DomainException<InvalidProductType>("The product type provided is not valid");

    public class InvalidQuantity() : DomainException<InvalidQuantity>("A value greater than zero must be provided for the quantity field.");

    public class ItemNotPresentInCart() : DomainException<ItemNotPresentInCart>("The item with Id provided does not exist in the cart.");

    public class CannotUpdateClosedCart() : DomainException<CannotUpdateClosedCart>("The shopping cart with Id provided is already closed and cannot be modified.");

    public class UnableToCloseWithoutAnyCartItems() : DomainException<UnableToCloseWithoutAnyCartItems>("It is not possible to close a shopping cart with Id provided without any items");

    public class InvalidCustomerId() : DomainException<InvalidCustomerId>("Invalid customer Id.");

    public class ShoppingCartNotFound() : DomainException<ShoppingCartNotFound>("No shopping cart was found with the provided Id.");

    public class ProductNotFound() : DomainException<ProductNotFound>("No product was found with the provided Id.");
    
    public class ProductNotPresentInCart() : DomainException<ProductNotFound>("The product with Id provided does not exist in the cart.");

}