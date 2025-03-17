using FloodInject.Runtime;

namespace SourceGenerators.Sample.Tests;

public class HeroManager;
public class UnitManager;
public class Environment;

[ContextListener]
public partial class Unit
{
    [Inject(typeof(GameplayContext))] private UnitManager _unitManager;
}

[ContextListener(isOverride: true)]
public partial class HeroUnit : Unit
{
    [Inject(typeof(GameplayContext))] private HeroManager _heroManager;
}

public class EnvironmentBinding
{
    public void Bind()
    {
        var data = new Environment();
        ContextProvider.GetContext<EnvironmentContext>().Bind(data);
    }
}