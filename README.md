# flood_inject
Unity source generated dependency injection framework


## Workflow

1. Creating context assets

```
[GenerateContext]
public partial class SceneContext : BaseContext { }
```

```
// <auto-generated />
public partial class SceneContext 
{
    public static System.Type Type => typeof(SceneContext);
    public override System.Type ContextType => SceneContext.Type;
}
```

2. Bind, Rebind, Unbind, Get, Reset functionality

```
// Direct contract will be generated on the backend
ContextProvider.GetContext<SceneContext>().Bind(data);

// Transient contract will be generated on the backend
ContextProvider.GetContext<SceneContext>().Rebind(() => new Data());
```

3. Auto injection from ContextProvider

```
[ContextListener]
public partial class DataWrapper
{
    [Inject(typeof(SceneContext))] Data data;
}

[ContextListener(true, AutoInjectType.Constructor)]
public partial class KeyedDataWrapper : DataWrapper 
{
    [Inject(typeof(SceneContext))] EncryptionKey key;
}
```

```
// <auto-generated />
public partial class DataWrapper
{
    public virtual void Inject()
    {
        data = ContextProvider.GetContext(typeof(SceneContext)).Get<Data>();
    }
}

// <auto-generated />
public partial class KeyedDataWrapper
{
    public KeyedDataWrapper() 
    {
        Inject();
    }

    public override void Inject()
    {
        base.Inject();
        data = ContextProvider.GetContext(typeof(SceneContext)).Get<Data>();
    }
}
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
- [ ] Record models conversion from DeclarationSyntax
- [ ] Generated class documentation
- [ ] Remove fully reflection
- [ ] Too verbose
- [ ] Code analysis
    - [ ] Validate partial and public keywords and report warnings
- [ ] Context inspector visualizer
- [ ] Handle playmode
- [ ] Unit testing
