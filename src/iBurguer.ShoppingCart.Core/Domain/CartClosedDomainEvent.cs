using iBurguer.ShoppingCart.Core.Abstractions;

namespace iBurguer.ShoppingCart.Core.Domain;

public record CartClosedDomainEvent(Guid ShoppingCartId) : IDomainEvent;