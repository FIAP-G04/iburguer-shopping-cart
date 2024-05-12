using System.Reflection;
using System.Runtime.Serialization;
using static iBurguer.ShoppingCart.Core.Exceptions;

namespace iBurguer.ShoppingCart.Core.Domain;

public class ProductType
{
    public static readonly ProductType MainDish = new(1, "MainDish");
    public static readonly ProductType SideDish = new(2, "SideDish");
    public static readonly ProductType Drink = new(3, "Drink");
    public static readonly ProductType Dessert = new(4, "Dessert");
    
    private readonly int _id;
    
    private readonly string _name;

    public ProductType(int id, string name)
    {
        _id = id;
        _name = name;
    }
    
    public override string ToString() => _name;
    
    public int Id() => _id;

    public string Name => _name;
    
    public static ProductType FromName(string name)
    {
        return FindProductType(productType => productType._name == name);
    }
    
    public static ProductType FromId(int id)
    {
        return FindProductType(productType => productType._id == id);
    }

    public static implicit operator ProductType(int value)
    {
        return FindProductType(productType => productType._id == value);
    }
    
    private static ProductType FindProductType(Func<ProductType, bool> predicate)
    {
        Type type = typeof(ProductType);
        FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);

        var productType = fields.FirstOrDefault(field =>
        {
            if (field.FieldType == typeof(ProductType))
            {
                ProductType productType = (ProductType)field.GetValue(null);
                return predicate(productType);
            }
            
            return false;
            
        })?.GetValue(null) as ProductType;

        Exceptions.InvalidProductTypeException.ThrowIfNull(productType);
        
        return productType;
    }
}