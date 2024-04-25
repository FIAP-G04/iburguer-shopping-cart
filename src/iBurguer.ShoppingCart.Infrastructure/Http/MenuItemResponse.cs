using iBurguer.ShoppingCart.Core.Domain;

namespace iBurguer.ShoppingCart.Infrastructure.Http;

public record MenuItemResponse
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required decimal Price { get; set; }
    public required string Category { get; set; }
    public required ushort PreparationTime { get; set; }
    public required string[] ImagesUrl { get; set; }

    public Product Convert()
    {
        return new Product(Id, Name, ProductType.FromName(Category), Price);
    }
}