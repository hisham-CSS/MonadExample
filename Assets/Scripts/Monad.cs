using System;

public class Monad<T>
{
    public T Value { get; }

    public Monad(T value)
    {
        Value = value;
    }

    // Bind: Apply a function that returns a new Monad<U>
    public Monad<U> Bind<U>(Func<T, Monad<U>> func)
    {
        return func(Value);
    }

    // Map: Apply a function that transforms T into U
    public Monad<U> Map<U>(Func<T, U> func)
    {
        return new Monad<U>(func(Value));
    }

    // Filter: Apply a predicate to determine if the value should propagate
    public Monad<T> Filter(Func<T, bool> predicate)
    {
        if (predicate(Value))
        {
            return this;
        }
        else
        {
            return new Monad<T>(default); // Or handle failure differently
        }
    }

    // Reduce: Collapse the monad chain into a final result
    public R Reduce<R>(Func<T, R> reducer)
    {
        return reducer(Value);
    }
}