using System;
using System.Collections.Generic;
using UnityEngine;

public static class G
{
    private static readonly Dictionary<Type, object> RegistrationsMap = new();
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Reset() => RegistrationsMap.Clear();
    
    public static void Register<T>(T service) where T : class
    {
        if (RegistrationsMap.ContainsKey(typeof(T)))
        { throw new Exception($"Duplicate registration of type {typeof(T)}"); }
        RegistrationsMap.Add(typeof(T), service);
    }

    public static T Resolve<T>() where T : class
    {
        T result = null;
        if (RegistrationsMap.TryGetValue(typeof(T), out var registration))
        { result = registration as T; }
        
        if (result != null) return result;
        throw new Exception($"Requested type {typeof(T)} was not registered");
    }
}