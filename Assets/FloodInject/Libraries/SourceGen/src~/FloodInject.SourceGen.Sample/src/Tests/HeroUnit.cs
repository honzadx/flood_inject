using FloodInject.Runtime;

namespace SourceGenerators.Sample.Tests;

[ContextListener(isOverride: true)]
public partial class HeroUnit : Unit
{
    [Inject(typeof(GameplayContext))] private HeroUnitManager _heroUnitManager;
}