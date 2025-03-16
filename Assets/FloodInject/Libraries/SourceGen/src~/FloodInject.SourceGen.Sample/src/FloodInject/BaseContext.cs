using System;
using System.Collections.Generic;
using UnityEngine;

namespace FloodInject.Runtime;

public abstract class BaseContext : ScriptableObject
{
    public abstract System.Type ContextType { get; }

    private readonly Dictionary<Type, object> _bindings = new ();

    public void Bind<T>(T value)
    {
        var key = typeof(T);
        _bindings.Add(key, value);
    }
        
    public void Bind<T>(Type key, T value)
    {
        _bindings.Add(key, value);
    }

    public void Rebind<T>(T value)
    {
        var key = typeof(T);
        _bindings[key] = value;
    }
        
    public void Rebind<T>(Type key, T value)
    {
        _bindings[key] = value;
    }

    public void Unbind<T>()
    {
        var key = typeof(T);
        _bindings.Remove(key);
    }
        
    public void Unbind(Type key)
    {
        _bindings.Remove(key);
    }

    public T Get<T>()
    {
        var key = typeof(T);
        return (T)_bindings[key];
    }
        
    public object? Get<T>(Type key)
    {
        return (T)_bindings[key];
    }
}