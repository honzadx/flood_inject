using FloodInject.Runtime;

namespace SourceGenerators.Sample.Tests;

[GenerateContext(ContextType.Volatile)]
public partial class GameplayContext { }

[GenerateContext(ContextType.Protected)]
public partial class SceneContext { }