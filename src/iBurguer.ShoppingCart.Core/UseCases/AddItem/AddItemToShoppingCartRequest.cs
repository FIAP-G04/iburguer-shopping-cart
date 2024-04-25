using FluentValidation;

namespace iBurguer.ShoppingCart.Core.UseCases.AddItem;

public class AddItemToShoppingCartRequest
{
    public Guid ShoppingCartId { get; set; }
    public Guid ProductId { get; set; }
    public ushort Quantity { get; set; }
    
    public class Validator : AbstractValidator<AddItemToShoppingCartRequest>
    {
        public Validator()
        {
            RuleFor(r => r.ShoppingCartId).NotEmpty();
            RuleFor(r => r.ProductId).NotEmpty();
            RuleFor(r => r.Quantity).NotEmpty().GreaterThan((ushort)0);
        }
    }
}