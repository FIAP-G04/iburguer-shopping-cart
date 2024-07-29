namespace iBurguer.ShoppingCart.Core.Abstractions
{
    public interface ISQSService
    {
        Task SendMessage(IDomainEvent domainEvent, CancellationToken cancellationToken);
    }
}
