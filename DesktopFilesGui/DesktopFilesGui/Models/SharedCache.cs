using System;
using System.Threading;

namespace DesktopFilesGui.Models;

//Taken from https://github.com/TheMakarik/solid-zip/blob/main/src/SolidZip/Core/Common/SharedCache.cs
public sealed class SharedCache<T> where T : class
{
    private readonly Lock _lock = new();
    private T? _cache;
    private Action<T>? _expandAction;
    private bool _wasChanged = false;

    public T Value
    {
        get
        {
            using (_lock.EnterScope())
                return _cache ?? throw new NullReferenceException(
                    $"Cache of {typeof(T).FullName} was not added, but tried to get, validate it using Exist() method before loading cache");
        }
        set
        {
            
            using (_lock.EnterScope())
            {
                if (_cache is not null)
                    _wasChanged = true;
                _cache = value;
            }
        }
    }

    public void SetValueProperty<TProperty>(Action<T, TProperty> setter, TProperty value)
    {
        using (_lock.EnterScope())
            setter(Value, value);
        this._wasChanged = true;
    }
    
    public bool Exists()
    {
        using (_lock.EnterScope())
            return _cache is not null;
    }

    public void ExpandChanges()
    {
        if (Exists() && _wasChanged)
            _expandAction?.Invoke(Value);
        _cache = null;
    }

    public void AddExpandAction(Action<T> expandAction)
    {
        _expandAction = expandAction;
    }
}