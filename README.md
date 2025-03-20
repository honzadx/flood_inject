# flood_inject
Unity source generated dependency injection framework

## Concepts

### Context (DI container)
Contexts are global DI containers, with each class representing a distinct context. Each context holds contracts defined by the user.

There are two types of contexts:
1. `ContextType.Volatile`, which has no restrictions on data flow.
2. `ContextType.Protected`, which operates in two phases:
    1. The first phase involves contract binding, after which the context is locked.
    2. The second phase is contract fulfillment, where the user can retrieve the bound data.

## Workflow

1. Creating contexts

``` C#
[GenerateContext(ContextType.Protected)]
public partial class SceneContext : BaseContext { }
```

``` C#
// <auto-generated />
using global::FloodInject.Runtime;

partial class SceneContext : BaseContext
{
    private bool _isLocked = false;
    
    public bool IsLocked => _isLocked;
    
    public override void Bind<T>(T instance)
    {
        #if UNITY_ASSERTIONS
        global::UnityEngine.Assertions.Assert.IsTrue(!_isLocked, $"Protected context has to be unlocked to modify contracts {this}");
        #else
        global::System.Diagnostics.Debug.Assert(!_isLocked, $"Protected context has to be unlocked to modify contracts {this}");
        #endif
        BindInternal(instance);
    }
    
    public override void Bind<T>(global::System.Func<T> factoryMethod)
    {
        #if UNITY_ASSERTIONS
        global::UnityEngine.Assertions.Assert.IsTrue(!_isLocked, $"Protected context has to be unlocked to modify contracts {this}");
        #else
        global::System.Diagnostics.Debug.Assert(!_isLocked, $"Protected context has to be unlocked to modify contracts {this}");
        #endif
        BindInternal(factoryMethod);
    }
    
    public override void Rebind<T>(T instance)
    {
        #if UNITY_ASSERTIONS
        global::UnityEngine.Assertions.Assert.IsTrue(!_isLocked, $"Protected context has to be unlocked to modify contracts {this}");
        #else
        global::System.Diagnostics.Debug.Assert(!_isLocked, $"Protected context has to be unlocked to modify contracts {this}");
        #endif
        RebindInternal(instance);
    }
    
    public override void Rebind<T>(global::System.Func<T> factoryMethod)
    {
        #if UNITY_ASSERTIONS
        global::UnityEngine.Assertions.Assert.IsTrue(!_isLocked, $"Protected context has to be unlocked to modify contracts {this}");
        #else
        global::System.Diagnostics.Debug.Assert(!_isLocked, $"Protected context has to be unlocked to modify contracts {this}");
        #endif
        RebindInternal(factoryMethod);
    }
    
    public override void Unbind<T>()
    {
        #if UNITY_ASSERTIONS
        global::UnityEngine.Assertions.Assert.IsTrue(!_isLocked, $"Protected context has to be unlocked to modify contracts {this}");
        #else
        global::System.Diagnostics.Debug.Assert(!_isLocked, $"Protected context has to be unlocked to modify contracts {this}");
        #endif
        Unbind<T>();
    }
    
    public override T Get<T>()
    {
        #if UNITY_ASSERTIONS
        global::UnityEngine.Assertions.Assert.IsTrue(_isLocked, $"Protected context has to be locked to retrieve contracts {this}");
        #else
        global::System.Diagnostics.Debug.Assert(_isLocked, $"Protected context has to be locked to retrieve contracts {this}");
        #endif
        return GetInternal<T>();
    }
    
    public override void Reset()
    {
        ResetInternal();
        _isLocked = false;
    }
    
    public void Lock()
    {
        _isLocked = true;
    }
}

```

2. Bind, Rebind, Unbind, Get, Reset functionality

``` C#
// Instance contract
ContextProvider<SceneContext>.GetContext().Bind(data);

// Transient contract
ContextProvider<SceneContext>.GetContext().Rebind(() => new Data());
```

3. Auto injection from ContextProvider

``` C#
[ContextListener]
public partial class DataWrapper
{
    [Inject(typeof(SceneContext))] Data data;
}

[ContextListener(true, AutoInject.Constructor)]
public partial class KeyedDataWrapper : DataWrapper 
{
    [Inject(typeof(SceneContext))] EncryptionKey key;
}
```

``` C#
// <auto-generated />
#pragma warning disable CS0109
using global::FloodInject.Runtime;

partial class DataWrapper
{
    public override void Construct()
    {
        PreConstruct();
        
        data = ContextProvider<SceneContext>.GetContext().Get<Data>();
        
        PostConstruct();
    }
    
    partial void PreConstruct();
    partial void PostConstruct();
}
#pragma warning restore CS0109


// <auto-generated />
#pragma warning disable CS0109
using global::FloodInject.Runtime;

partial class KeyedDataWrapper
{
    public override void Construct()
    {
        PreConstruct();
        
        base.Construct();
        key = ContextProvider<SceneContext>.GetContext().Get<EncryptionKey>();
        
        PostConstruct();
    }
    
    partial void PreConstruct();
    partial void PostConstruct();
}
#pragma warning restore CS0109
```

## TODO:
- [x] Base context definition
    - [x] Bind, Rebind, Unbind
    - [x] Register, Unregister
    - [x] Get
    - [x] Reset
- [x] Context generation
    - [x] Compile time type getter
- [x] Auto Injection
    - [x] Get context type
    - [x] Build field inject metadata
    - [x] Construct inject method
    - [x] Trigger injection automatically
- [x] Handle inherited injections
    - [ ] Handle inherited injections auto magically without isOverride?
- [x] Source gen backend rewrite to record models
- [x] Context backend rewrite to use contracts instead of instances
    - [x] Fulfill() method will return requested service
    - [x] Direct contract -> bind instances
    - [x] Transient contract -> bind factoryMethod
- [ ] Clean up terms 
- [x] Record models conversion from DeclarationSyntax
- [ ] Generated class documentation
- [ ] Remove fully reflection
- [x] Too verbose
- [ ] Code analysis
    - [ ] Validate partial and public keywords and report warnings
- [ ] Context inspector visualizer
- [ ] Handle playmode
- [ ] Unit testing
