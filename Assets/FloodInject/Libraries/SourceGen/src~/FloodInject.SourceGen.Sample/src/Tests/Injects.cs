using FloodInject.Runtime;
using UnityEngine;

namespace SourceGenerators.Sample.Tests;

public class HeroManager;
public class UnitManager;
public class Environment;

[ContextListener]
public partial class Unit
{
    [Inject(typeof(GameplayContext))] private UnitManager _unitManager;
}

[ContextListener(isOverride: true, autoInjectType: AutoInjectType.Constructor)]
public partial class HeroUnit : Unit
{
    [Inject(typeof(GameplayContext))] private HeroManager _heroManager;
}

[ContextListener(autoInjectType: AutoInjectType.Unity)]
public partial class EnvironmentBuilder : MonoBehaviour
{
    [Inject(typeof(EnvironmentContext))] private Environment _environment;
}

public class EnvironmentBinding
{
    public void Bind(Environment environment)
    {
        ContextProvider.GetContext<EnvironmentContext>().Rebind(environment);
    }
}