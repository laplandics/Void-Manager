using System;

public class Subscriber<T> : IDisposable
{
    private readonly Action<T> _action;
    private readonly Action<Subscriber<T>> _onDispose;

    public Subscriber(Action<T> action, Action<Subscriber<T>> onDispose)
    { _action = action; _onDispose = onDispose; }
    
    public void Invoke(T arg1) => _action?.Invoke(arg1);
    public void Dispose() => _onDispose?.Invoke(this);
}

public class Subscriber<T1, T2> : IDisposable
{
    private readonly Action<T1, T2> _action;
    private readonly Action<Subscriber<T1, T2>> _onDispose;

    public Subscriber(Action<T1, T2> action, Action<Subscriber<T1, T2>> onDispose)
    { _action = action; _onDispose = onDispose; }

    public void Invoke(T1 arg1, T2 arg2) => _action?.Invoke(arg1, arg2);

    public void Dispose() => _onDispose?.Invoke(this);
}
