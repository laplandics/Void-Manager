using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Reactive<T>
{
    [SerializeField] private T value;
    private readonly List<Subscriber<T, T>> _subscribers = new();
    private readonly List<Subscriber<T, T>> _toSubscribe = new();
    private readonly List<Subscriber<T, T>> _toUnsubscribe = new();

    public Reactive() : this(default) {}
    public Reactive(T value) => this.value = value;

    public T Value
    {
        get => value;
        set
        {
            if (EqualityComparer<T>.Default.Equals(value, this.value)) return;

            var old = this.value;
            this.value = value;
            Notify(value, old);
        }
    }

    private void Notify(T newValue, T oldValue)
    {
        if (_toSubscribe.Count > 0)
        {
            _subscribers.AddRange(_toSubscribe);
            _toSubscribe.Clear();
        }

        if (_toUnsubscribe.Count > 0)
        {
            _toUnsubscribe.ForEach(sub => _subscribers.Remove(sub));
            _toUnsubscribe.Clear();
        }
        
        _subscribers.ForEach(sub => InvokeSub(sub, newValue, oldValue));
    }

    private void InvokeSub(Subscriber<T, T> sub, T newValue, T oldValue) => sub.Invoke(newValue, oldValue);
    
    public IDisposable SubscribeSilently(Action<T, T> action)
    { var sub = new Subscriber<T, T>(action, Unsubscribe); _toSubscribe.Add(sub); return sub; }
    
    public IDisposable Subscribe(Action<T, T> action)
    { var sub = new Subscriber<T, T>(action, Unsubscribe); _toSubscribe.Add(sub); InvokeSub(sub, value, value); return sub; }

    private void Unsubscribe(Subscriber<T, T> sub) => _toUnsubscribe.Add(sub);
}