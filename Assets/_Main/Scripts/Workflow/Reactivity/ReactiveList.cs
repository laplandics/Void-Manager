using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ReactiveList<T>
{
    [SerializeField] private List<T> list;
    public IReadOnlyList<T> Value => list;
    
    private readonly List<Subscriber<T>> _subscribersOnAdd = new();
    private readonly List<Subscriber<T>> _subscribersOnRemove = new();
    
    private readonly List<Subscriber<T>> _toSubscribeOnAdd = new();
    private readonly List<Subscriber<T>> _toSubscribeOnRemove = new();
    
    private readonly List<Subscriber<T>> _toUnsubscribeFromAdd = new();
    private readonly List<Subscriber<T>> _toUnsubscribeFromRemove = new();

    public ReactiveList() : this(new List<T>()) { }
    public ReactiveList(List<T> value) { list = value; }
    
    private void InvokeSub(Subscriber<T> sub, T value) => sub.Invoke(value);
    private void UnsubscribeFromAdd(Subscriber<T> sub) => _toUnsubscribeFromAdd.Add(sub);
    private void UnsubscribeFromRemove(Subscriber<T> sub) => _toUnsubscribeFromRemove.Add(sub);

    private void NotifyOnAdd(T value)
    {
        if (_toSubscribeOnAdd.Count > 0)
        {
            _subscribersOnAdd.AddRange(_toSubscribeOnAdd);
            _toSubscribeOnAdd.Clear();
        }

        if (_toUnsubscribeFromAdd.Count > 0)
        {
            _toUnsubscribeFromAdd.ForEach(sub => _subscribersOnAdd.Remove(sub));
            _toUnsubscribeFromAdd.Clear();
        }
        
        _subscribersOnAdd.ForEach(sub => sub.Invoke(value));
    }
    
    private void NotifyOnRemove(T value)
    {
        if (_toSubscribeOnRemove.Count > 0)
        {
            _subscribersOnRemove.AddRange(_toSubscribeOnRemove);
            _toSubscribeOnRemove.Clear();
        }

        if (_toUnsubscribeFromRemove.Count > 0)
        {
            _toUnsubscribeFromRemove.ForEach(sub => _subscribersOnRemove.Remove(sub));
            _toUnsubscribeFromRemove.Clear();
        }
        
        _subscribersOnRemove.ForEach(sub => sub.Invoke(value));
    }
    
    public IDisposable OnAdd(Action<T> action)
    {
        var sub = new Subscriber<T>(action, UnsubscribeFromAdd);
        _toSubscribeOnAdd.Add(sub);
        return sub;
    }

    public IDisposable OnRemove(Action<T> action)
    {
        var sub = new Subscriber<T>(action, UnsubscribeFromRemove);
        _toSubscribeOnRemove.Add(sub);
        return sub;
    }

    public void Add(T item) { list.Add(item); NotifyOnAdd(item); }

    public void AddUnique(T item) { if (list.Contains(item)) return; list.Add(item); NotifyOnAdd(item); }

    public void Remove(T item) { if (list.Remove(item)) { NotifyOnRemove(item); } }

    public void Clear() { list.ForEach(NotifyOnRemove); list.Clear(); }
}