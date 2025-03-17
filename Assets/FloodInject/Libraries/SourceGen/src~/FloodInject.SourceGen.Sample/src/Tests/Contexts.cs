using FloodInject.Runtime;

namespace SourceGenerators.Sample.Tests;

[GenerateContext]
public partial class GameplayContext : BaseContext;

[GenerateContext]
public partial class EnvironmentContext : BaseContext;
