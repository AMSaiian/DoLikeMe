namespace AMSaiian.Shared.Application.Wrappers
{
    public struct Undefinable<TValue>
    {
        public Undefinable(bool isDefined = false, TValue value = default!)
        {
            if (!isDefined
             && !EqualityComparer<TValue>.Default.Equals(value, default))
            {
                throw new InvalidOperationException("Can't create undefined with defined value");
            }

            Value = value;
            IsDefined = isDefined;
        }

        public TValue Value { get; } = default!;
        public bool IsDefined { get; } = false;

        public static implicit operator Undefinable<TValue>(TValue value) => new Undefinable<TValue>(true, value);
        public static implicit operator TValue(Undefinable<TValue> undefinable) => undefinable.Value;
    }
}


