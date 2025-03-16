using FloodInject.Runtime;

namespace SourceGenerators.Sample.Tests;

public class UnitManager;
public class Environment;

public partial class Unit
{
    [Inject(typeof(GlobalContext))] public UnitManager globalUnitManager;
    [Inject(typeof(GameplayContext))] public UnitManager gameplayUnitManager;
    [Inject(typeof(EnvironmentContext))] public Environment environment;
}

public class EnvironmentBinding
{
    public void Bind()
    {
        var data = new Environment();
        ContextProvider.GetContext<EnvironmentContext>().Bind(data);
    }
}