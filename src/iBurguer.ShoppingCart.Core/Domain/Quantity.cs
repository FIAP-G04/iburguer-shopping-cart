using iBurguer.ShoppingCart.Core.Abstractions;
using static iBurguer.ShoppingCart.Core.Exceptions; 

namespace iBurguer.ShoppingCart.Core.Domain;

public sealed record Quantity
{
    private ushort _value = 1;

    public Quantity(ushort quantity) => Value = quantity;

    public ushort Value
    {
        get => _value;
        private set
        {
            InvalidQuantityException.ThrowIf(value < 1);

            _value = value;
        }
    }

    public static implicit operator ushort(Quantity quantity) => quantity.Value;

    public static implicit operator Quantity(ushort value) => new(value);

    public override string ToString() => Value.ToString();

    public bool IsMinimum() => Value == 1;

    public void Increment() => Value++;

    public void Increment(Quantity quantity) => Value = (ushort)(Value + quantity.Value);

    public void Decrement() => Value--;

    public void Decrement(Quantity quantity) => Value = (ushort)(Value - quantity.Value);
}