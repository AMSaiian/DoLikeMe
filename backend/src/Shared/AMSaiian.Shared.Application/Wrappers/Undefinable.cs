using AMSaiian.Shared.Application.Templates;

namespace AMSaiian.Shared.Application.Wrappers;

public struct Undefinable<TValue>
{
    public Undefinable(bool isDefined = false, TValue value = default!)
    {
        if (!isDefined
         && !EqualityComparer<TValue>.Default.Equals(value, default))
        {
            throw new InvalidOperationException(ErrorTemplates.CantCreateUndefined);
        }

        Value = value;
        IsDefined = isDefined;
    }

    public TValue Value
    {
        readonly get => _value;
        init
        {
            _isDefined = true;
            _value = value;
        }
    }

    public bool IsDefined
    {
        readonly get => _isDefined;
        init
        {
            if (!value)
            {
                _value = default!;
            }
            _isDefined = value;
        }
    }

    public static implicit operator Undefinable<TValue>(TValue value) => new Undefinable<TValue>(true, value);
    public static implicit operator TValue(Undefinable<TValue> undefinable) => undefinable.Value;

    private readonly TValue _value = default!;
    private readonly bool _isDefined = false;
}
