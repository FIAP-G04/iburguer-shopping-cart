using FluentValidation;

namespace iBurguer.ShoppingCart.Core.UseCases.Checkout;

public class CheckoutRequest
{
    public Guid ShoppingCartId { get; set; }
    public string OrderType { get; set; } = string.Empty;
    
    public class Validator : AbstractValidator<CheckoutRequest>
    {
        public Validator()
        {
            RuleFor(r => r.ShoppingCartId).NotEmpty();
            RuleFor(r => r.OrderType).NotEmpty();
        }
    }
}