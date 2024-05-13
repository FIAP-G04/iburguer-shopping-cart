using iBurguer.ShoppingCart.Core.Abstractions;

namespace iBurguer.ShoppingCart.Core;

public static class Exceptions
{
    public class InvalidPriceException() : DomainException<InvalidPriceException>("The price cannot have a value equal to zero or negative");

    public class InvalidProductTypeException() : DomainException<InvalidProductTypeException>("The product type provided is not valid");

    public class InvalidQuantityException() : DomainException<InvalidQuantityException>("A value greater than zero must be provided for the quantity field.");

    public class ItemNotPresentInCartException() : DomainException<ItemNotPresentInCartException>("The item with Id provided does not exist in the cart.");

    public class CannotUpdateClosedCartException() : DomainException<CannotUpdateClosedCartException>("The shopping cart with Id provided is already closed and cannot be modified.");

    public class UnableToCloseWithoutAnyCartItemsException() : DomainException<UnableToCloseWithoutAnyCartItemsException>("It is not possible to close a shopping cart with Id provided without any items");

    public class InvalidCustomerIdException() : DomainException<InvalidCustomerIdException>("Invalid customer Id.");

    public class ShoppingCartNotFoundException() : DomainException<ShoppingCartNotFoundException>("No shopping cart was found with the provided Id.");

    public class ProductNotFoundException() : DomainException<ProductNotFoundException>("No product was found with the provided Id.");
    
    public class ProductNotPresentInCartException() : DomainException<ProductNotFoundException>("The product with Id provided does not exist in the cart.");

}